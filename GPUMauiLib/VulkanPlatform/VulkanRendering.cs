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
    public static class VulkanRendering
    {
        

        public static unsafe void CreateRenderPass(this IVulkanRenderer renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanRendering.CreateRenderPass");
#endif
            // Attachment description
            VkAttachmentDescription colorAttachment = new VkAttachmentDescription()
            {
                format = renderer.SurfaceFormat.format,
                samples = VkSampleCountFlags.VK_SAMPLE_COUNT_1_BIT,
                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            // Subpasses and attachment references
            VkAttachmentReference colorAttachmentRef = new VkAttachmentReference()
            {
                attachment = 0,
                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            
            VkSubpassDescription subpass = new VkSubpassDescription()
            {
                pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachmentRef,
            };

            // Render  pass            
            VkSubpassDependency dependency = new VkSubpassDependency()
            {
                srcSubpass = VulkanNative.VK_SUBPASS_EXTERNAL,
                dstSubpass = 0,
                srcStageMask = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                srcAccessMask = 0,
                dstStageMask = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                dstAccessMask = VkAccessFlags.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
            };

            VkRenderPassCreateInfo renderPassInfo = new VkRenderPassCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 1,
                pAttachments = &colorAttachment,
                subpassCount = 1,
                pSubpasses = &subpass,
                dependencyCount = 1,
                pDependencies = &dependency,
            };

            VkRenderPass renderPass = default(VkRenderPass);

            VkRenderPass* renderPassPtr = &renderPass;
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateRenderPass(renderer.VSupport.Device, &renderPassInfo, null, renderPassPtr));
            }
            renderer.RenderPass = renderPass;
        }


        public static unsafe void CreateFramebuffers(this IVulkanRenderer renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanRendering.CreateFramebuffers");
#endif
            renderer.FrameBuffers = new VkFramebuffer[renderer.SwapChainImageViews.Length];

            for (int i = 0; i < renderer.SwapChainImageViews.Length; i++)
            {

                VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo();
                framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
                framebufferInfo.renderPass = renderer.RenderPass;
                framebufferInfo.attachmentCount = 1;

                fixed (VkImageView* attachments = &renderer.SwapChainImageViews[i])
                {
                    framebufferInfo.pAttachments = attachments;
                }

                framebufferInfo.width = renderer.SurfaceExtent2D.width;
                framebufferInfo.height = renderer.SurfaceExtent2D.height;
                framebufferInfo.layers = 1;

                fixed (VkFramebuffer* swapChainFramebufferPtr = &renderer.FrameBuffers[i])
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateFramebuffer(renderer.VSupport.Device, &framebufferInfo, null, swapChainFramebufferPtr));
                }
            }
        }




        public static unsafe void CreateCommandPool(this IVulkanRenderer renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanRendering.CreateCommandPool");
#endif    
            VkCommandPoolCreateInfo poolInfo = new VkCommandPoolCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                queueFamilyIndex = (uint) renderer.FamilyIndices.graphicsFamily,
                flags = 0, // Optional,
            };

            VkCommandPool commandPool = default(VkCommandPool);

            VkCommandPool* commandPoolPtr = &commandPool;
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateCommandPool(renderer.VSupport.Device, &poolInfo, null, commandPoolPtr));
            }

            renderer.CommandPool = commandPool;
        }

        public static unsafe void CreateCommandBuffers(this IVulkanRenderer renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanRendering.CreateCommandBuffers");
#endif
            renderer.CommandBuffers = new VkCommandBuffer[renderer.FrameBuffers.Length];

            VkCommandBufferAllocateInfo allocInfo = new VkCommandBufferAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = renderer.CommandPool,
                level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = (uint)renderer.CommandBuffers.Length,
            };

            fixed (VkCommandBuffer* commandBuffersPtr = &renderer.CommandBuffers[0])
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkAllocateCommandBuffers(renderer.VSupport.Device, &allocInfo, commandBuffersPtr));
            }

            // Begin
            for (uint i = 0; i < renderer.CommandBuffers.Length; i++)
            {
                VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                    flags = 0, // Optional
                    pInheritanceInfo = null, // Optional
                };

                VulkanHelpers.CheckErrors(VulkanNative.vkBeginCommandBuffer(renderer.CommandBuffers[i], &beginInfo));

                // Pass
                VkClearValue clearColor = new VkClearValue()
                {
                    color = new VkClearColorValue(0.0f, 0.0f, 0.0f, 1.0f),
                };

                VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                    renderPass = renderer.RenderPass,
                    framebuffer = renderer.FrameBuffers[i],
                    renderArea = new VkRect2D(0, 0, renderer.SurfaceExtent2D.width, renderer.SurfaceExtent2D.height),
                    clearValueCount = 1,
                    pClearValues = &clearColor,
                };

                VulkanNative.vkCmdBeginRenderPass(renderer.CommandBuffers[i], &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);

                // Draw
                VulkanNative.vkCmdBindPipeline(renderer.CommandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, renderer.GraphicsPipeline);

                VulkanNative.vkCmdDraw(renderer.CommandBuffers[i], 3, 1, 0, 0);

                VulkanNative.vkCmdEndRenderPass(renderer.CommandBuffers[i]);

                VulkanHelpers.CheckErrors(VulkanNative.vkEndCommandBuffer(renderer.CommandBuffers[i]));
            }
        }

        
        public static unsafe void CreateSyncSemaphores(this IVulkanRenderer renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanRendering.CreateSyncSemaphores");
#endif
            VkSemaphoreCreateInfo semaphoreInfo = new VkSemaphoreCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
            };

            VkSemaphore imageSemaphore = default(VkSemaphore);
            VkSemaphore* imageAvailableSemaphorePtr = &imageSemaphore;
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateSemaphore(renderer.VSupport.Device, &semaphoreInfo, null, imageAvailableSemaphorePtr));
            }
            renderer.ImageAvailableSemaphore = imageSemaphore;

            VkSemaphore renderFinishedSemaphore = default(VkSemaphore);
            VkSemaphore* renderFinishedSemaphorePtr = &renderFinishedSemaphore;
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateSemaphore(renderer.VSupport.Device, &semaphoreInfo, null, renderFinishedSemaphorePtr));
            }
            renderer.RenderFinishedSemaphore = renderFinishedSemaphore;
        }
    }
}
