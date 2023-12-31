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
    public interface IVulkanRenderer
    {
        bool PipelineInitialized { get; set; }

        IVulkanSupport VSupport { get; }
        VkSurfaceKHR Surface { get; }
        VkSwapchainKHR SwapChain { get; }
        VkImage[] SwapChainImages { get; }
        VkImageView[] SwapChainImageViews { get; }
        SwapChainSupportDetails SwapChainDetails { get; set; }
        VkSurfaceFormatKHR SurfaceFormat { get; set; }
        VkExtent2D SurfaceExtent2D { get; set; }
        VkPresentModeKHR PresentMode { get; set; }
        VkFormat ImageFormat { get; set; }
        VkRenderPass RenderPass { get; set; }
        VkPipelineLayout PipelineLayout { get; set; }
        VkPipeline GraphicsPipeline { get; set; }
        VkFramebuffer[] FrameBuffers { get; set; }
        VkCommandPool CommandPool { get; set; }
        VkCommandBuffer[] CommandBuffers { get; set; }
        QueueFamilyIndices FamilyIndices { get; set; }
        VkSemaphore ImageAvailableSemaphore { get; set; }
        VkSemaphore RenderFinishedSemaphore { get; set; }
        VkQueue GraphicsQueue { get; }
        void SetGraphicsQueue(VkQueue graphicsQueue);
        VkQueue PresentQueue { get; }
        void SetPresentQueue(VkQueue presentQueue);


        void CreateSwapChainImages(uint imageCount);


    }
}

