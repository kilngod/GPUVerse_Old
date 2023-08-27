﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPUVulkan;
using Microsoft.VisualBasic;

namespace VulkanPlatform
{
#nullable disable
    public static class VulkanComputing
    {


        public static unsafe void CreateComputePipeline(this VkDevice device, ref VkComputePipelineCreateInfo pipelineCreateInfo, ref VkPipeline pipeline, ref VkAllocationCallbacks allocationCallbacks, VkPipelineCache cache = default(VkPipelineCache))
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
            VulkanFlowTracer.AddItem("VulkanRendering.CreateCommandPool");
#endif    
            VkCommandPoolCreateInfo poolInfo = new VkCommandPoolCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                queueFamliyIndex = (uint)compute.ComputeFamilyIndex,
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
            VulkanFlowTracer.AddItem("VulkanRendering.CreateCommandBuffers");
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

            // Begin
            for (uint i = 0; i < compute.CommandBuffers.Length; i++)
            {
                VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                    flags =  VkCommandBufferUsageFlags.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT, // Optional
                    pInheritanceInfo = null, // Optional
                };

                VulkanHelpers.CheckErrors(VulkanNative.vkBeginCommandBuffer(compute.CommandBuffers[i], &beginInfo));

                // Pass
                VkClearValue clearColor = new VkClearValue()
                {
                    color = new VkClearColorValue(0.0f, 0.0f, 0.0f, 1.0f),
                };

               

                // Draw
                VulkanNative.vkCmdBindPipeline(compute.CommandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE, compute.ComputePipeline);

              

                VulkanHelpers.CheckErrors(VulkanNative.vkEndCommandBuffer(compute.CommandBuffers[i]));
            }
        }
    }
}