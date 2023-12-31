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

using System;
using GPUVulkan;

namespace VulkanPlatform
{
	public static class VulkanDraw
	{
        public static unsafe void DrawFrame(this IVulkanRenderer renderer)
        {
            // Acquiring and image from the swap chain
            uint imageIndex;
            VulkanHelpers.CheckErrors(VulkanNative.vkAcquireNextImageKHR(renderer.VSupport.Device, renderer.SwapChain, ulong.MaxValue, renderer.ImageAvailableSemaphore, 0, &imageIndex));

            // Submitting the command buffer
            VkSemaphore* waitSemaphores = stackalloc VkSemaphore[] { renderer.ImageAvailableSemaphore };
            VkPipelineStageFlags* waitStages = stackalloc VkPipelineStageFlags[] { VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT };
            VkSemaphore* signalSemaphores = stackalloc VkSemaphore[] { renderer.RenderFinishedSemaphore };
            VkCommandBuffer* commandBuffersPtr = stackalloc VkCommandBuffer[] { renderer.CommandBuffers[imageIndex] };

            VkSubmitInfo submitInfo = new VkSubmitInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
                waitSemaphoreCount = 1,
                pWaitSemaphores = waitSemaphores,
                pWaitDstStageMask = waitStages,
                commandBufferCount = 1,
                pCommandBuffers = commandBuffersPtr,
                signalSemaphoreCount = 1,
                pSignalSemaphores = signalSemaphores,
            };

            VulkanHelpers.CheckErrors(VulkanNative.vkQueueSubmit(renderer.GraphicsQueue, 1, &submitInfo, 0));

            // Presentation
            VkSwapchainKHR* swapChains = stackalloc VkSwapchainKHR[] { renderer.SwapChain };

            VkPresentInfoKHR presentInfo = new VkPresentInfoKHR()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                waitSemaphoreCount = 1,
                pWaitSemaphores = signalSemaphores,
                swapchainCount = 1,
                pSwapchains = swapChains,
                pImageIndices = &imageIndex,
                pResults = null, // Optional
            };

            VulkanHelpers.CheckErrors(VulkanNative.vkQueuePresentKHR(renderer.PresentQueue, &presentInfo));

            VulkanHelpers.CheckErrors(VulkanNative.vkQueueWaitIdle(renderer.PresentQueue));
        }
    }
}

