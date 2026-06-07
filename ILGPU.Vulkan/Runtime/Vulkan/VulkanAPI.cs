// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanAPI.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Provides the ILGPU runtime-facing Vulkan API wrapper.
/// </summary>
internal static unsafe class VulkanAPI
{
    /// <summary>
    /// Creates a Vulkan API version value.
    /// </summary>
    public static uint Version(uint major, uint minor, uint patch) =>
        (major << 22) | (minor << 12) | patch;

    /// <summary>
    /// Throws a <see cref="VulkanException"/> if the given result is an error.
    /// </summary>
    public static void ThrowIfFailed(VkResult result)
    {
        if (result < VkResult.VK_SUCCESS)
            throw new VulkanException(result);
    }

    /// <summary>
    /// Enumerates physical devices associated with a Vulkan instance.
    /// </summary>
    public static ImmutableArray<VkPhysicalDevice> EnumeratePhysicalDevices(
        VkInstance instance)
    {
        if (instance == VkInstance.Null)
            return ImmutableArray<VkPhysicalDevice>.Empty;

        uint count = 0;
        ThrowIfFailed(
            VulkanNative.vkEnumeratePhysicalDevices(instance, &count, null));
        if (count == 0)
            return ImmutableArray<VkPhysicalDevice>.Empty;

        var devices = new VkPhysicalDevice[count];
        fixed (VkPhysicalDevice* devicesPtr = devices)
        {
            ThrowIfFailed(
                VulkanNative.vkEnumeratePhysicalDevices(
                    instance,
                    &count,
                    devicesPtr));
        }
        return devices.ToImmutableArray();
    }

    /// <summary>
    /// Enumerates device extension names for a physical device.
    /// </summary>
    public static HashSet<string> EnumerateDeviceExtensions(
        VkPhysicalDevice physicalDevice)
    {
        uint count = 0;
        ThrowIfFailed(
            VulkanNative.vkEnumerateDeviceExtensionProperties(
                physicalDevice,
                null,
                &count,
                null));

        var extensions = new HashSet<string>(StringComparer.Ordinal);
        if (count == 0)
            return extensions;

        var extensionProperties = new VkExtensionProperties[count];
        fixed (VkExtensionProperties* extensionPropertiesPtr = extensionProperties)
        {
            ThrowIfFailed(
                VulkanNative.vkEnumerateDeviceExtensionProperties(
                    physicalDevice,
                    null,
                    &count,
                    extensionPropertiesPtr));
        }

        for (int i = 0; i < count; ++i)
        {
            fixed (byte* extensionName = extensionProperties[i].extensionName)
                extensions.Add(GetString(extensionName));
        }

        return extensions;
    }

    /// <summary>
    /// Returns true if shader Float16/Int8 feature queries are supported.
    /// </summary>
    public static bool SupportsShaderFloat16Int8(
        uint apiVersion,
        HashSet<string> deviceExtensions) =>
        apiVersion >= Version(1, 2, 0) ||
        deviceExtensions.Contains(
            VulkanNative.VK_KHR_SHADER_FLOAT16_INT8_EXTENSION_NAME);

    /// <summary>
    /// Returns true if shader BFloat16 feature queries are supported.
    /// </summary>
    public static bool SupportsShaderBFloat16(
        HashSet<string> deviceExtensions) =>
        deviceExtensions.Contains(
            VulkanNative.VK_KHR_SHADER_BFLOAT16_EXTENSION_NAME);

    /// <summary>
    /// Returns true if shader Float8 feature queries are supported.
    /// </summary>
    public static bool SupportsShaderFloat8(HashSet<string> deviceExtensions) =>
        deviceExtensions.Contains(VulkanNative.VK_EXT_SHADER_FLOAT8_EXTENSION_NAME);

    /// <summary>
    /// Queries physical device properties.
    /// </summary>
    public static VkPhysicalDeviceProperties GetPhysicalDeviceProperties(
        VkPhysicalDevice physicalDevice)
    {
        VkPhysicalDeviceProperties properties = default;
        VulkanNative.vkGetPhysicalDeviceProperties(physicalDevice, &properties);
        return properties;
    }

    /// <summary>
    /// Queries physical device memory properties.
    /// </summary>
    public static VkPhysicalDeviceMemoryProperties GetPhysicalDeviceMemoryProperties(
        VkPhysicalDevice physicalDevice)
    {
        VkPhysicalDeviceMemoryProperties memoryProperties = default;
        VulkanNative.vkGetPhysicalDeviceMemoryProperties(
            physicalDevice,
            &memoryProperties);
        return memoryProperties;
    }

    /// <summary>
    /// Queries physical device features through a caller-owned feature chain.
    /// </summary>
    public static void GetPhysicalDeviceFeatures2(
        VkPhysicalDevice physicalDevice,
        ref VkPhysicalDeviceFeatures2 features)
    {
        fixed (VkPhysicalDeviceFeatures2* featuresPtr = &features)
            VulkanNative.vkGetPhysicalDeviceFeatures2(physicalDevice, featuresPtr);
    }

    /// <summary>
    /// Finds the first queue family supporting the given flags.
    /// </summary>
    public static uint FindQueueFamilyIndex(
        VkPhysicalDevice physicalDevice,
        VkQueueFlags queueFlags)
    {
        uint count = 0;
        VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(
            physicalDevice,
            &count,
            null);
        if (count == 0)
            return uint.MaxValue;

        var queueFamilies = new VkQueueFamilyProperties[count];
        fixed (VkQueueFamilyProperties* queueFamiliesPtr = queueFamilies)
        {
            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(
                physicalDevice,
                &count,
                queueFamiliesPtr);
        }

        for (uint i = 0; i < count; ++i)
        {
            if ((queueFamilies[i].queueFlags & queueFlags) != 0)
                return i;
        }

        return uint.MaxValue;
    }

    /// <summary>
    /// Creates a logical device with a single queue from a queue family.
    /// </summary>
    public static VkDevice CreateDevice(
        VkPhysicalDevice physicalDevice,
        uint queueFamilyIndex,
        in VkPhysicalDeviceFeatures enabledFeatures)
    {
        float queuePriority = 1.0f;
        var queueCreateInfo = new VkDeviceQueueCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
            queueFamilyIndex = queueFamilyIndex,
            queueCount = 1,
            pQueuePriorities = &queuePriority,
        };

        var features = enabledFeatures;
        var createInfo = new VkDeviceCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
            queueCreateInfoCount = 1,
            pQueueCreateInfos = &queueCreateInfo,
            pEnabledFeatures = &features,
        };

        VkDevice device = default;
        ThrowIfFailed(
            VulkanNative.vkCreateDevice(
                physicalDevice,
                &createInfo,
                null,
                &device));
        return device;
    }

    /// <summary>
    /// Gets a queue from a logical device.
    /// </summary>
    public static VkQueue GetDeviceQueue(
        VkDevice device,
        uint queueFamilyIndex,
        uint queueIndex)
    {
        VkQueue queue = default;
        VulkanNative.vkGetDeviceQueue(
            device,
            queueFamilyIndex,
            queueIndex,
            &queue);
        return queue;
    }

    /// <summary>
    /// Waits until all work on a logical device is idle.
    /// </summary>
    public static void DeviceWaitIdle(VkDevice device) =>
        ThrowIfFailed(VulkanNative.vkDeviceWaitIdle(device));

    /// <summary>
    /// Waits until all work on a Vulkan queue is idle.
    /// </summary>
    public static void QueueWaitIdle(VkQueue queue) =>
        ThrowIfFailed(VulkanNative.vkQueueWaitIdle(queue));

    /// <summary>
    /// Destroys a logical device.
    /// </summary>
    public static void DestroyDevice(VkDevice device) =>
        VulkanNative.vkDestroyDevice(device, null);

    /// <summary>
    /// Creates a Vulkan command pool for the given queue family.
    /// </summary>
    public static VkCommandPool CreateCommandPool(
        VkDevice device,
        uint queueFamilyIndex,
        VkCommandPoolCreateFlags flags =
            VkCommandPoolCreateFlags.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT)
    {
        var createInfo = new VkCommandPoolCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
            flags = flags,
            queueFamilyIndex = queueFamilyIndex,
        };

        VkCommandPool commandPool = default;
        ThrowIfFailed(
            VulkanNative.vkCreateCommandPool(
                device,
                &createInfo,
                null,
                &commandPool));
        return commandPool;
    }

    /// <summary>
    /// Destroys a Vulkan command pool.
    /// </summary>
    public static void DestroyCommandPool(
        VkDevice device,
        VkCommandPool commandPool) =>
        VulkanNative.vkDestroyCommandPool(device, commandPool, null);

    /// <summary>
    /// Allocates primary command buffers from a command pool.
    /// </summary>
    public static VkCommandBuffer[] AllocateCommandBuffers(
        VkDevice device,
        VkCommandPool commandPool,
        uint commandBufferCount,
        VkCommandBufferLevel level =
            VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY)
    {
        if (commandBufferCount == 0)
            return [];

        var allocateInfo = new VkCommandBufferAllocateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            commandPool = commandPool,
            level = level,
            commandBufferCount = commandBufferCount,
        };

        var commandBuffers = new VkCommandBuffer[commandBufferCount];
        fixed (VkCommandBuffer* commandBuffersPtr = commandBuffers)
        {
            ThrowIfFailed(
                VulkanNative.vkAllocateCommandBuffers(
                    device,
                    &allocateInfo,
                    commandBuffersPtr));
        }
        return commandBuffers;
    }

    /// <summary>
    /// Allocates a single primary command buffer from a command pool.
    /// </summary>
    public static VkCommandBuffer AllocateCommandBuffer(
        VkDevice device,
        VkCommandPool commandPool) =>
        AllocateCommandBuffers(device, commandPool, 1)[0];

    /// <summary>
    /// Frees command buffers allocated from a command pool.
    /// </summary>
    public static void FreeCommandBuffers(
        VkDevice device,
        VkCommandPool commandPool,
        ReadOnlySpan<VkCommandBuffer> commandBuffers)
    {
        if (commandBuffers.IsEmpty)
            return;

        fixed (VkCommandBuffer* commandBuffersPtr = commandBuffers)
        {
            VulkanNative.vkFreeCommandBuffers(
                device,
                commandPool,
                (uint)commandBuffers.Length,
                commandBuffersPtr);
        }
    }

    /// <summary>
    /// Frees a command buffer allocated from a command pool.
    /// </summary>
    public static void FreeCommandBuffer(
        VkDevice device,
        VkCommandPool commandPool,
        VkCommandBuffer commandBuffer)
    {
        var commandBuffers = new[] { commandBuffer };
        FreeCommandBuffers(device, commandPool, commandBuffers);
    }

    /// <summary>
    /// Begins recording a command buffer.
    /// </summary>
    public static void BeginCommandBuffer(
        VkCommandBuffer commandBuffer,
        VkCommandBufferUsageFlags flags =
            VkCommandBufferUsageFlags.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT)
    {
        var beginInfo = new VkCommandBufferBeginInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            flags = flags,
        };

        ThrowIfFailed(
            VulkanNative.vkBeginCommandBuffer(commandBuffer, &beginInfo));
    }

    /// <summary>
    /// Ends command-buffer recording.
    /// </summary>
    public static void EndCommandBuffer(VkCommandBuffer commandBuffer) =>
        ThrowIfFailed(VulkanNative.vkEndCommandBuffer(commandBuffer));

    /// <summary>
    /// Creates a Vulkan fence.
    /// </summary>
    public static VkFence CreateFence(
        VkDevice device,
        VkFenceCreateFlags flags = VkFenceCreateFlags.None)
    {
        var createInfo = new VkFenceCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
            flags = flags,
        };

        VkFence fence = default;
        ThrowIfFailed(
            VulkanNative.vkCreateFence(device, &createInfo, null, &fence));
        return fence;
    }

    /// <summary>
    /// Destroys a Vulkan fence.
    /// </summary>
    public static void DestroyFence(VkDevice device, VkFence fence) =>
        VulkanNative.vkDestroyFence(device, fence, null);

    /// <summary>
    /// Waits for a Vulkan fence.
    /// </summary>
    public static void WaitForFence(
        VkDevice device,
        VkFence fence,
        ulong timeout = ulong.MaxValue) =>
        ThrowIfFailed(
            VulkanNative.vkWaitForFences(
                device,
                1,
                &fence,
                true,
                timeout));

    /// <summary>
    /// Submits command buffers to a queue.
    /// </summary>
    public static void QueueSubmit(
        VkQueue queue,
        ReadOnlySpan<VkCommandBuffer> commandBuffers,
        VkFence fence)
    {
        if (commandBuffers.IsEmpty)
            return;

        fixed (VkCommandBuffer* commandBuffersPtr = commandBuffers)
        {
            var submitInfo = new VkSubmitInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
                commandBufferCount = (uint)commandBuffers.Length,
                pCommandBuffers = commandBuffersPtr,
            };

            ThrowIfFailed(
                VulkanNative.vkQueueSubmit(queue, 1, &submitInfo, fence));
        }
    }

    /// <summary>
    /// Submits a single command buffer to a queue.
    /// </summary>
    public static void QueueSubmit(
        VkQueue queue,
        VkCommandBuffer commandBuffer,
        VkFence fence)
    {
        var commandBuffers = new[] { commandBuffer };
        QueueSubmit(queue, commandBuffers, fence);
    }

    /// <summary>
    /// Binds a compute pipeline to a command buffer.
    /// </summary>
    public static void CmdBindComputePipeline(
        VkCommandBuffer commandBuffer,
        VkPipeline pipeline) =>
        VulkanNative.vkCmdBindPipeline(
            commandBuffer,
            VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE,
            pipeline);

    /// <summary>
    /// Binds descriptor sets for a compute pipeline.
    /// </summary>
    public static void CmdBindComputeDescriptorSets(
        VkCommandBuffer commandBuffer,
        VkPipelineLayout pipelineLayout,
        ReadOnlySpan<VkDescriptorSet> descriptorSets)
    {
        if (descriptorSets.IsEmpty)
            return;

        fixed (VkDescriptorSet* descriptorSetsPtr = descriptorSets)
        {
            VulkanNative.vkCmdBindDescriptorSets(
                commandBuffer,
                VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE,
                pipelineLayout,
                0,
                (uint)descriptorSets.Length,
                descriptorSetsPtr,
                0,
                null);
        }
    }

    /// <summary>
    /// Pushes compute-stage constants into a command buffer.
    /// </summary>
    public static void CmdPushComputeConstants(
        VkCommandBuffer commandBuffer,
        VkPipelineLayout pipelineLayout,
        ReadOnlySpan<byte> data,
        uint offset = 0)
    {
        if (data.IsEmpty)
            return;
        if ((offset & 3) != 0 || (data.Length & 3) != 0)
        {
            throw new ArgumentException(
                "Vulkan push-constant offset and size must be 4-byte aligned.",
                nameof(data));
        }

        fixed (byte* dataPtr = data)
        {
            VulkanNative.vkCmdPushConstants(
                commandBuffer,
                pipelineLayout,
                VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT,
                offset,
                (uint)data.Length,
                dataPtr);
        }
    }

    /// <summary>
    /// Records a compute dispatch command.
    /// </summary>
    public static void CmdDispatch(
        VkCommandBuffer commandBuffer,
        uint groupCountX,
        uint groupCountY,
        uint groupCountZ) =>
        VulkanNative.vkCmdDispatch(
            commandBuffer,
            groupCountX,
            groupCountY,
            groupCountZ);

    /// <summary>
    /// Creates a storage-buffer descriptor set layout for compute kernels.
    /// </summary>
    public static VkDescriptorSetLayout CreateStorageBufferDescriptorSetLayout(
        VkDevice device,
        uint descriptorCount)
    {
        if (descriptorCount == 0)
        {
            var emptyCreateInfo = new VkDescriptorSetLayoutCreateInfo
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
            };

            VkDescriptorSetLayout emptyLayout = default;
            ThrowIfFailed(
                VulkanNative.vkCreateDescriptorSetLayout(
                    device,
                    &emptyCreateInfo,
                    null,
                    &emptyLayout));
            return emptyLayout;
        }

        var bindings = new VkDescriptorSetLayoutBinding[descriptorCount];
        for (uint i = 0; i < descriptorCount; ++i)
        {
            bindings[i] = new VkDescriptorSetLayoutBinding
            {
                binding = i,
                descriptorType = VkDescriptorType
                    .VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                descriptorCount = 1,
                stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT,
            };
        }

        fixed (VkDescriptorSetLayoutBinding* bindingsPtr = bindings)
        {
            var createInfo = new VkDescriptorSetLayoutCreateInfo
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                bindingCount = descriptorCount,
                pBindings = bindingsPtr,
            };

            VkDescriptorSetLayout descriptorSetLayout = default;
            ThrowIfFailed(
                VulkanNative.vkCreateDescriptorSetLayout(
                    device,
                    &createInfo,
                    null,
                    &descriptorSetLayout));
            return descriptorSetLayout;
        }
    }

    /// <summary>
    /// Destroys a Vulkan descriptor set layout.
    /// </summary>
    public static void DestroyDescriptorSetLayout(
        VkDevice device,
        VkDescriptorSetLayout descriptorSetLayout) =>
        VulkanNative.vkDestroyDescriptorSetLayout(
            device,
            descriptorSetLayout,
            null);

    /// <summary>
    /// Creates a descriptor pool for storage-buffer descriptor sets.
    /// </summary>
    public static VkDescriptorPool CreateStorageBufferDescriptorPool(
        VkDevice device,
        uint descriptorCount,
        uint maxSets = 1)
    {
        if (descriptorCount == 0)
            descriptorCount = 1;

        var poolSize = new VkDescriptorPoolSize
        {
            type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
            descriptorCount = descriptorCount,
        };

        var createInfo = new VkDescriptorPoolCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
            maxSets = maxSets,
            poolSizeCount = 1,
            pPoolSizes = &poolSize,
        };

        VkDescriptorPool descriptorPool = default;
        ThrowIfFailed(
            VulkanNative.vkCreateDescriptorPool(
                device,
                &createInfo,
                null,
                &descriptorPool));
        return descriptorPool;
    }

    /// <summary>
    /// Destroys a Vulkan descriptor pool and all descriptor sets allocated from it.
    /// </summary>
    public static void DestroyDescriptorPool(
        VkDevice device,
        VkDescriptorPool descriptorPool) =>
        VulkanNative.vkDestroyDescriptorPool(device, descriptorPool, null);

    /// <summary>
    /// Allocates descriptor sets from a descriptor pool.
    /// </summary>
    public static VkDescriptorSet[] AllocateDescriptorSets(
        VkDevice device,
        VkDescriptorPool descriptorPool,
        VkDescriptorSetLayout descriptorSetLayout,
        uint descriptorSetCount = 1)
    {
        if (descriptorSetCount == 0)
            return [];

        var layouts = new VkDescriptorSetLayout[descriptorSetCount];
        Array.Fill(layouts, descriptorSetLayout);

        var descriptorSets = new VkDescriptorSet[descriptorSetCount];
        fixed (VkDescriptorSetLayout* layoutsPtr = layouts)
        fixed (VkDescriptorSet* descriptorSetsPtr = descriptorSets)
        {
            var allocateInfo = new VkDescriptorSetAllocateInfo
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                descriptorPool = descriptorPool,
                descriptorSetCount = descriptorSetCount,
                pSetLayouts = layoutsPtr,
            };

            ThrowIfFailed(
                VulkanNative.vkAllocateDescriptorSets(
                    device,
                    &allocateInfo,
                    descriptorSetsPtr));
        }
        return descriptorSets;
    }

    /// <summary>
    /// Updates a descriptor set with storage-buffer bindings.
    /// </summary>
    public static void UpdateStorageBufferDescriptorSet(
        VkDevice device,
        VkDescriptorSet descriptorSet,
        ReadOnlySpan<VulkanMemoryBuffer> buffers)
    {
        if (buffers.IsEmpty)
            return;

        var bufferInfos = new VkDescriptorBufferInfo[buffers.Length];
        var writes = new VkWriteDescriptorSet[buffers.Length];
        for (uint i = 0; i < buffers.Length; ++i)
        {
            var buffer = buffers[(int)i];
            bufferInfos[i] = new VkDescriptorBufferInfo
            {
                buffer = buffer.Buffer,
                offset = 0,
                range = checked((ulong)buffer.LengthInBytes),
            };
        }

        fixed (VkDescriptorBufferInfo* bufferInfosPtr = bufferInfos)
        fixed (VkWriteDescriptorSet* writesPtr = writes)
        {
            for (uint i = 0; i < buffers.Length; ++i)
            {
                writes[i] = new VkWriteDescriptorSet
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                    dstSet = descriptorSet,
                    dstBinding = i,
                    descriptorCount = 1,
                    descriptorType = VkDescriptorType
                        .VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                    pBufferInfo = bufferInfosPtr + i,
                };
            }

            VulkanNative.vkUpdateDescriptorSets(
                device,
                (uint)writes.Length,
                writesPtr,
                0,
                null);
        }
    }

    /// <summary>
    /// Creates a Vulkan pipeline layout for a single descriptor set layout.
    /// </summary>
    public static VkPipelineLayout CreatePipelineLayout(
        VkDevice device,
        VkDescriptorSetLayout descriptorSetLayout,
        uint pushConstantBytes = 0)
    {
        var setLayout = descriptorSetLayout;
        var pushConstantRange = new VkPushConstantRange
        {
            stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT,
            offset = 0,
            size = pushConstantBytes,
        };

        var createInfo = new VkPipelineLayoutCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
            setLayoutCount = descriptorSetLayout == VkDescriptorSetLayout.Null
                ? 0u
                : 1u,
            pSetLayouts = descriptorSetLayout == VkDescriptorSetLayout.Null
                ? null
                : &setLayout,
            pushConstantRangeCount = pushConstantBytes == 0 ? 0u : 1u,
            pPushConstantRanges = pushConstantBytes == 0
                ? null
                : &pushConstantRange,
        };

        VkPipelineLayout pipelineLayout = default;
        ThrowIfFailed(
            VulkanNative.vkCreatePipelineLayout(
                device,
                &createInfo,
                null,
                &pipelineLayout));
        return pipelineLayout;
    }

    /// <summary>
    /// Destroys a Vulkan pipeline layout.
    /// </summary>
    public static void DestroyPipelineLayout(
        VkDevice device,
        VkPipelineLayout pipelineLayout) =>
        VulkanNative.vkDestroyPipelineLayout(device, pipelineLayout, null);

    /// <summary>
    /// Creates a Vulkan compute pipeline.
    /// </summary>
    public static VkPipeline CreateComputePipeline(
        VkDevice device,
        VkShaderModule shaderModule,
        VkPipelineLayout pipelineLayout,
        string entryPoint)
    {
        var entryPointBytes = Encoding.UTF8.GetBytes(entryPoint + '\0');
        fixed (byte* entryPointPtr = entryPointBytes)
        {
            var stageCreateInfo = new VkPipelineShaderStageCreateInfo
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                stage = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT,
                module = shaderModule,
                pName = entryPointPtr,
            };

            var createInfo = new VkComputePipelineCreateInfo
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO,
                stage = stageCreateInfo,
                layout = pipelineLayout,
            };

            VkPipeline pipeline = default;
            ThrowIfFailed(
                VulkanNative.vkCreateComputePipelines(
                    device,
                    VkPipelineCache.Null,
                    1,
                    &createInfo,
                    null,
                    &pipeline));
            return pipeline;
        }
    }

    /// <summary>
    /// Destroys a Vulkan pipeline.
    /// </summary>
    public static void DestroyPipeline(VkDevice device, VkPipeline pipeline) =>
        VulkanNative.vkDestroyPipeline(device, pipeline, null);

    /// <summary>
    /// Creates a Vulkan buffer.
    /// </summary>
    public static VkBuffer CreateBuffer(
        VkDevice device,
        ulong size,
        VkBufferUsageFlags usage)
    {
        var createInfo = new VkBufferCreateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
            size = size,
            usage = usage,
            sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
        };

        VkBuffer buffer = default;
        ThrowIfFailed(
            VulkanNative.vkCreateBuffer(device, &createInfo, null, &buffer));
        return buffer;
    }

    /// <summary>
    /// Destroys a Vulkan buffer.
    /// </summary>
    public static void DestroyBuffer(VkDevice device, VkBuffer buffer) =>
        VulkanNative.vkDestroyBuffer(device, buffer, null);

    /// <summary>
    /// Gets memory requirements for a Vulkan buffer.
    /// </summary>
    public static VkMemoryRequirements GetBufferMemoryRequirements(
        VkDevice device,
        VkBuffer buffer)
    {
        VkMemoryRequirements requirements = default;
        VulkanNative.vkGetBufferMemoryRequirements(device, buffer, &requirements);
        return requirements;
    }

    /// <summary>
    /// Allocates Vulkan device memory.
    /// </summary>
    public static VkDeviceMemory AllocateMemory(
        VkDevice device,
        ulong allocationSize,
        uint memoryTypeIndex)
    {
        var allocateInfo = new VkMemoryAllocateInfo
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
            allocationSize = allocationSize,
            memoryTypeIndex = memoryTypeIndex,
        };

        VkDeviceMemory memory = default;
        ThrowIfFailed(
            VulkanNative.vkAllocateMemory(device, &allocateInfo, null, &memory));
        return memory;
    }

    /// <summary>
    /// Frees Vulkan device memory.
    /// </summary>
    public static void FreeMemory(VkDevice device, VkDeviceMemory memory) =>
        VulkanNative.vkFreeMemory(device, memory, null);

    /// <summary>
    /// Binds Vulkan device memory to a buffer.
    /// </summary>
    public static void BindBufferMemory(
        VkDevice device,
        VkBuffer buffer,
        VkDeviceMemory memory,
        ulong memoryOffset) =>
        ThrowIfFailed(
            VulkanNative.vkBindBufferMemory(
                device,
                buffer,
                memory,
                memoryOffset));

    /// <summary>
    /// Maps Vulkan device memory into the host address space.
    /// </summary>
    public static IntPtr MapMemory(
        VkDevice device,
        VkDeviceMemory memory,
        ulong offset,
        ulong size)
    {
        void* data = null;
        ThrowIfFailed(
            VulkanNative.vkMapMemory(
                device,
                memory,
                offset,
                size,
                0,
                &data));
        return new IntPtr(data);
    }

    /// <summary>
    /// Unmaps Vulkan device memory.
    /// </summary>
    public static void UnmapMemory(VkDevice device, VkDeviceMemory memory) =>
        VulkanNative.vkUnmapMemory(device, memory);

    /// <summary>
    /// Finds a compatible Vulkan memory type.
    /// </summary>
    public static uint FindMemoryType(
        VkPhysicalDeviceMemoryProperties memoryProperties,
        uint memoryTypeBits,
        VkMemoryPropertyFlags requiredFlags)
    {
        for (uint i = 0; i < memoryProperties.memoryTypeCount; ++i)
        {
            var supported = (memoryTypeBits & (1u << (int)i)) != 0;
            var memoryType = (&memoryProperties.memoryTypes_0)[i];
            if (supported &&
                (memoryType.propertyFlags & requiredFlags) == requiredFlags)
            {
                return i;
            }
        }

        throw new NotSupportedException(
            $"No Vulkan memory type supports '{requiredFlags}'.");
    }

    /// <summary>
    /// Creates a Vulkan shader module from SPIR-V code.
    /// </summary>
    public static VkShaderModule CreateShaderModule(
        VkDevice device,
        ReadOnlySpan<byte> spirv)
    {
        if (spirv.IsEmpty)
            throw new ArgumentException("SPIR-V code cannot be empty.", nameof(spirv));
        if ((spirv.Length & 3) != 0)
        {
            throw new ArgumentException(
                "SPIR-V code size must be a multiple of 4 bytes.",
                nameof(spirv));
        }

        fixed (byte* spirvPtr = spirv)
        {
            var createInfo = new VkShaderModuleCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                codeSize = new UIntPtr((uint)spirv.Length),
                pCode = (uint*)spirvPtr,
            };

            VkShaderModule shaderModule = default;
            ThrowIfFailed(
                VulkanNative.vkCreateShaderModule(
                    device,
                    &createInfo,
                    null,
                    &shaderModule));
            return shaderModule;
        }
    }

    /// <summary>
    /// Destroys a Vulkan shader module.
    /// </summary>
    public static void DestroyShaderModule(
        VkDevice device,
        VkShaderModule shaderModule) =>
        VulkanNative.vkDestroyShaderModule(device, shaderModule, null);

    /// <summary>
    /// Gets a managed string from a null-terminated UTF-8 pointer.
    /// </summary>
    public static string GetString(byte* stringStart)
    {
        int characters = 0;
        while (stringStart[characters] != 0)
            ++characters;
        return Encoding.UTF8.GetString(stringStart, characters);
    }
}
