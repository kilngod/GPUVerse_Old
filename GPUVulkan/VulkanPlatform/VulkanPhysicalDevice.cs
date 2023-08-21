﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GPUVulkan;
#nullable disable


namespace VulkanPlatform
{

    public struct QueueFamilyIndices
    {
        public uint? graphicsFamily;
        public uint? presentFamily;

        public bool IsComplete()
        {
            return graphicsFamily.HasValue && presentFamily.HasValue;
        }
    }


    public static class VulkanPhysicalDevice
	{

        public unsafe static void CreateLogicalDevice(VkPhysicalDevice physicalDevice, List<string> deviceExtensions, VkSurfaceKHR surface,ref VkDevice device, ref VkQueue graphicsQueue, ref VkQueue presentQueue)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanPhysicalDevice.CreateLogicalDevice");
#endif
            QueueFamilyIndices indices = FindQueueFamilies(physicalDevice, surface);

            List<VkDeviceQueueCreateInfo> queueCreateInfos = new List<VkDeviceQueueCreateInfo>();
            HashSet<uint> uniqueQueueFamilies = new HashSet<uint>() { indices.graphicsFamily.Value, indices.presentFamily.Value };

            float queuePriority = 1.0f;
            foreach (uint queueFamily in uniqueQueueFamilies)
            {
                VkDeviceQueueCreateInfo queueCreateInfo = new VkDeviceQueueCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                    queueFamilyIndex = queueFamily,
                    queueCount = 1,
                    pQueuePriorities = &queuePriority,
                };
                queueCreateInfos.Add(queueCreateInfo);
            }

            VkPhysicalDeviceFeatures deviceFeatures = default;

            int deviceExtensionsCount = deviceExtensions.Count;
            IntPtr* deviceExtensionsArray = stackalloc IntPtr[deviceExtensionsCount];
            for (int i = 0; i < deviceExtensionsCount; i++)
            {
                string extension = deviceExtensions[i];
                deviceExtensionsArray[i] = Marshal.StringToHGlobalAnsi(extension);
            }

            VkDeviceCreateInfo createInfo = new VkDeviceCreateInfo();
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;

            VkDeviceQueueCreateInfo[] queueCreateInfosArray = queueCreateInfos.ToArray();
            fixed (VkDeviceQueueCreateInfo* queueCreateInfosArrayPtr = &queueCreateInfosArray[0])
            {
                createInfo.queueCreateInfoCount = (uint)queueCreateInfos.Count;
                createInfo.pQueueCreateInfos = queueCreateInfosArrayPtr;

            }

            createInfo.pEnabledFeatures = &deviceFeatures;
            createInfo.enabledExtensionCount = (uint)deviceExtensions.Count;
            createInfo.ppEnabledExtensionNames = (byte**)deviceExtensionsArray;

            fixed (VkDevice* devicePtr = &device)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateDevice(physicalDevice, &createInfo, null, devicePtr));
            }

            fixed (VkQueue* graphicsQueuePtr = &graphicsQueue)
            {
                VulkanNative.vkGetDeviceQueue(device, indices.graphicsFamily.Value, 0, graphicsQueuePtr);
            }

            fixed (VkQueue* presentQueuePtr = &presentQueue)
            {
                VulkanNative.vkGetDeviceQueue(device, indices.presentFamily.Value, 0, presentQueuePtr); // TODO queue index 0 ?¿?¿
            }
        }




        public static unsafe void PickPhysicalDevice(ref VkPhysicalDevice physicalDevice, VkInstance instance, VkSurfaceKHR surface, List<string> requiredDeviceExtensions)
        {

#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.PickPhysicalDevice");
#endif
            uint deviceCount = 0;

            VulkanHelpers.CheckErrors(VulkanNative.vkEnumeratePhysicalDevices(instance, &deviceCount, null));
            if (deviceCount == 0)
            {
                throw new Exception("Failed to find GPUs with Vulkan support!");
            }

            VkPhysicalDevice* physicaldevices = stackalloc VkPhysicalDevice[(int)deviceCount];
            VulkanHelpers.CheckErrors(VulkanNative.vkEnumeratePhysicalDevices(instance, &deviceCount, physicaldevices));

            for (int i = 0; i < deviceCount; i++)
            {
                var pDevice = physicaldevices[i];

                if (IsPhysicalDeviceSuitable(pDevice, surface, requiredDeviceExtensions))
                {
                    physicalDevice = pDevice;
                    break;
                }
            }

        }
        /*
//android emulator
VK_KHR_incremental_present
VK_GOOGLE_display_timing
VK_KHR_vulkan_memory_model
VK_KHR_maintenance1
VK_KHR_maintenance2
VK_KHR_maintenance3
VK_KHR_bind_memory2
VK_KHR_dedicated_allocation
VK_KHR_get_memory_requirements2
VK_KHR_sampler_ycbcr_conversion
VK_KHR_shader_subgroup_extended_types
VK_EXT_provoking_vertex
VK_EXT_line_rasterization
VK_EXT_load_store_op_none
VK_EXT_image_robustness
VK_EXT_custom_border_color
VK_EXT_shader_stencil_export
VK_KHR_image_format_list
VK_EXT_queue_family_foreign
VK_KHR_external_semaphore
VK_KHR_external_memory
VK_KHR_external_fence
VK_EXT_device_memory_report
VK_KHR_swapchain
VK_KHR_external_fence_fd
VK_ANDROID_external_memory_android_hardware_buffer*/

        private static unsafe bool CheckPhysicalDeviceExtensionSupport(VkPhysicalDevice physicalDevice, List<string> requiredDeviceExtensions)
        {
            uint extensionCount;
            VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateDeviceExtensionProperties(physicalDevice, null, &extensionCount, null));

            VkExtensionProperties* availableExtensions = stackalloc VkExtensionProperties[(int)extensionCount];
            VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateDeviceExtensionProperties(physicalDevice, null, &extensionCount, availableExtensions));

            HashSet<string> mandatoryExtensions = new HashSet<string>(requiredDeviceExtensions);
            string extensions = string.Empty;
            for (int i = 0; i < extensionCount; i++)
            {
                var extension = availableExtensions[i];
                string extensionName = VulkanHelpers.GetString(extension.extensionName);
                extensions += extensionName + System.Environment.NewLine;
                mandatoryExtensions.Remove(VulkanHelpers.GetString(extension.extensionName));
            }

            return mandatoryExtensions.Count == 0;
        }

        private static unsafe bool IsPhysicalDeviceSuitable(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, List<string> requiredDeviceExtensions)
        {
            QueueFamilyIndices indices = FindQueueFamilies(physicalDevice, surface);

            bool extensionsSupported = CheckPhysicalDeviceExtensionSupport(physicalDevice,requiredDeviceExtensions);

            bool swapChainAdequate = false;
            if (extensionsSupported)
            {
                SwapChainSupportDetails swapChainSupport = VulkanSwapChain.QuerySwapChainSupport(physicalDevice,surface);
                swapChainAdequate = (swapChainSupport.formats.Length != 0 && swapChainSupport.presentModes.Length != 0);
            }

            return indices.IsComplete() && extensionsSupported && swapChainAdequate;
        }


       




        public static unsafe QueueFamilyIndices FindQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
        {
            QueueFamilyIndices indices = default;

            uint queueFamilyCount = 0;
            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, null);

            VkQueueFamilyProperties* queueFamilies = stackalloc VkQueueFamilyProperties[(int)queueFamilyCount];
            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilies);

            for (uint i = 0; i < queueFamilyCount; i++)
            {
                var queueFamily = queueFamilies[i];
                if ((queueFamily.queueFlags & VkQueueFlags.VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    indices.graphicsFamily = i;
                }

                VkBool32 presentSupport = false;
                VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, i, surface, &presentSupport));

                if (presentSupport)
                {
                    indices.presentFamily = i;
                }

                if (indices.IsComplete())
                {
                    break;
                }
            }

            return indices;
        }

    }
}
