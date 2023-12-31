// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------


using GPUVulkan;


namespace VulkanPlatform
{

    public static class VulkanComputing
    {

        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline)
        {
            fixed (VkPipeline* pipelinePtr = &pipeline)
            {
                fixed (VkComputePipelineCreateInfo* pipelineCreateInfoPtr = &pipelineCreateInfo)
                {
                    
                    VulkanNative.vkCreateComputePipelines(device, VkPipelineCache.Null, 1, pipelineCreateInfoPtr, null, pipelinePtr);
                    
                }
            }

        }

        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline, ref VkAllocationCallbacks allocationCallbacks)
        {
            fixed (VkPipeline* pipelinePtr = &pipeline)
            {
                fixed (VkComputePipelineCreateInfo* pipelineCreateInfoPtr = &pipelineCreateInfo)
                {
                    fixed (VkAllocationCallbacks* callbacksPtr = &allocationCallbacks)
                    {
                        VulkanNative.vkCreateComputePipelines(device, VkPipelineCache.Null, 1, pipelineCreateInfoPtr, callbacksPtr, pipelinePtr);
                    }
                }
            }

        }


        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline, ref VkPipelineCache cache)
        {
            fixed (VkPipeline* pipelinePtr = &pipeline)
            {
                fixed (VkComputePipelineCreateInfo* pipelineCreateInfoPtr = &pipelineCreateInfo)
                {

                    VulkanNative.vkCreateComputePipelines(device, cache, 1, pipelineCreateInfoPtr, null, pipelinePtr);

                }
            }

        }


        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline, ref VkAllocationCallbacks allocationCallbacks, ref VkPipelineCache cache)
        {
            fixed (VkPipeline* pipelinePtr = &pipeline)
            {
                fixed (VkComputePipelineCreateInfo* pipelineCreateInfoPtr = &pipelineCreateInfo)
                {
                    fixed (VkAllocationCallbacks* callbacksPtr = &allocationCallbacks)
                    {
                        VulkanNative.vkCreateComputePipelines(device, cache, 1, pipelineCreateInfoPtr, callbacksPtr, pipelinePtr);
                    }
                }
            }

        }


        public static unsafe void CreateCommandPool(this IVulkanCompute compute)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanComputing.CreateCommandPool");
#endif    
            VkCommandPoolCreateInfo poolInfo = new VkCommandPoolCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                queueFamilyIndex = (uint)compute.ComputeFamilyIndex,
                flags = 0, // Optional,
            };

            VkCommandPool commandPool = default(VkCommandPool);

            VkCommandPool* commandPoolPtr = &commandPool;
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateCommandPool(compute.Support.Device, &poolInfo, null, commandPoolPtr));
            }

            compute.CommandPool = commandPool;
        }

        public static unsafe void CreateCommandBuffers(this IVulkanCompute compute)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanComputing.CreateCommandBuffers");
#endif
            compute.CommandBuffers = new VkCommandBuffer[compute.ComputeCommandBuffers];

            VkCommandBufferAllocateInfo allocInfo = new VkCommandBufferAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = compute.CommandPool,
                level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = (uint)compute.CommandBuffers.Length,
            };

            fixed (VkCommandBuffer* commandBuffersPtr = &compute.CommandBuffers[0])
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkAllocateCommandBuffers(compute.Support.Device, &allocInfo, commandBuffersPtr));
            }

        }

        // clearly multiple definitions are needed based on the type of compute.
        public unsafe static void FillCommandBuffer(this IVulkanCompute compute, uint groupCountX, uint groupCountY, uint groupCountZ)
        {

            // Begin
            for (uint i = 0; i < compute.CommandBuffers.Length; i++)
            {
                VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                    flags = VkCommandBufferUsageFlags.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT, // Optional
                    pInheritanceInfo = null, // Optional
                };

                VulkanHelpers.CheckErrors(VulkanNative.vkBeginCommandBuffer(compute.CommandBuffers[i], &beginInfo));


                VulkanNative.vkCmdBindPipeline(compute.CommandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE, compute.ComputePipeline);


                fixed (VkDescriptorSet* descriptorSetPtr = &compute.DescriptorSets[0])
                {
                    VulkanNative.vkCmdBindDescriptorSets(compute.CommandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE,
                        compute.PipelineLayout, 0, (uint)compute.DescriptorSets.Length, descriptorSetPtr, 0, null);
                }


                VulkanNative.vkCmdDispatch(compute.CommandBuffers[i], groupCountX, groupCountY, groupCountZ);

                VulkanHelpers.CheckErrors(VulkanNative.vkEndCommandBuffer(compute.CommandBuffers[i]));
            }
        }

        public unsafe static void SubmitAndWait(this IVulkanCompute compute, VkSubmitInfo[] submitInfos)
        {




            VkFenceCreateInfo fenceCreateInfo = new VkFenceCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                flags = VkFenceCreateFlags.None
            };

            VkFence fence = default(VkFence);
            VulkanNative.vkCreateFence(compute.Support.Device, &fenceCreateInfo, null, &fence);

            fixed (VkSubmitInfo* submitInfoPtr = &submitInfos[0])
            {
                VulkanNative.vkQueueSubmit(compute.ComputeQueue, (uint)submitInfos.Length, submitInfoPtr, fence);
            }

            VulkanNative.vkWaitForFences(compute.Support.Device, 1, &fence, true, 100000000);
        }
    }
}


