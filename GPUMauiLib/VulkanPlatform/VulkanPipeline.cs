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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanPlatform
{
    public static class VulkanPipeline
    {
      
        public static unsafe void CreatePipelineLayout(this VkDevice device, ref VkPipelineLayoutCreateInfo pipelineLayoutCreateInfo, ref VkPipelineLayout pipelineLayout) 
        {
            fixed (VkPipelineLayoutCreateInfo* createInfoPtr = &pipelineLayoutCreateInfo)
            {
                fixed (VkPipelineLayout* pipelineLayoutPtr = &pipelineLayout)
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreatePipelineLayout(device, createInfoPtr, null, pipelineLayoutPtr));
                }
            }
        }


        public static unsafe void CreateGraphicsPipeline(this IVulkanRenderer renderer, List<VulkanSpirV> shaderSource)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanPipeline.CreateGraphicsPipeline");
#endif

        


            // Vertex Input
            VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                vertexBindingDescriptionCount = 0,
                pVertexBindingDescriptions = null, // Optional
                vertexAttributeDescriptionCount = 0,
                pVertexAttributeDescriptions = null, // Optional
            };


            // Input assembly
            VkPipelineInputAssemblyStateCreateInfo inputAssembly = new VkPipelineInputAssemblyStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                primitiveRestartEnable = false,
            };

            // Viewports and scissors
            VkViewport viewport = new VkViewport()
            {
                x = 0.0f,
                y = 0.0f,
                width = (float)renderer.SurfaceExtent2D.width,
                height = (float)renderer.SurfaceExtent2D.height,
                minDepth = 0.0f,
                maxDepth = 1.0f,
            };

            VkRect2D scissor = new VkRect2D()
            {
                offset = new VkOffset2D(0, 0),
                extent = renderer.SurfaceExtent2D,
            };

            VkPipelineViewportStateCreateInfo viewportState = new VkPipelineViewportStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                viewportCount = 1,
                pViewports = &viewport,
                scissorCount = 1,
                pScissors = &scissor,
            };

            // Rasterizer
            VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                depthClampEnable = false,
                rasterizerDiscardEnable = false,
                polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                lineWidth = 1.0f,
                cullMode = VkCullModeFlags.VK_CULL_MODE_BACK_BIT,
                frontFace = VkFrontFace.VK_FRONT_FACE_CLOCKWISE,
                depthBiasEnable = false,
                depthBiasConstantFactor = 0.0f, // Optional
                depthBiasClamp = 0.0f, // Optional
                depthBiasSlopeFactor = 0.0f, // Optional
            };

            VkPipelineMultisampleStateCreateInfo multisampling = new VkPipelineMultisampleStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                sampleShadingEnable = false,
                rasterizationSamples = VkSampleCountFlags.VK_SAMPLE_COUNT_1_BIT,
                minSampleShading = 1.0f, // Optional
                pSampleMask = null, // Optional
                alphaToCoverageEnable = false, // Optional
                alphaToOneEnable = false, // Optional
            };

            // Depth and Stencil testing
            //VkPipelineDepthStencilStateCreateInfo

            // Color blending
            VkPipelineColorBlendAttachmentState colorBlendAttachment = new VkPipelineColorBlendAttachmentState()
            {
                colorWriteMask = VkColorComponentFlags.VK_COLOR_COMPONENT_R_BIT |
                                 VkColorComponentFlags.VK_COLOR_COMPONENT_G_BIT |
                                 VkColorComponentFlags.VK_COLOR_COMPONENT_B_BIT |
                                 VkColorComponentFlags.VK_COLOR_COMPONENT_A_BIT,
                blendEnable = false,
                srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE, // Optional
                dstColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO, // Optional
                colorBlendOp = VkBlendOp.VK_BLEND_OP_ADD, // Optional
                srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE, // Optional
                dstAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO, // Optional
                alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD, // Optional
            };

            VkPipelineColorBlendStateCreateInfo colorBlending = new VkPipelineColorBlendStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                logicOpEnable = false,
                logicOp = VkLogicOp.VK_LOGIC_OP_COPY, // Optional
                attachmentCount = 1,
                pAttachments = &colorBlendAttachment,
                blendConstants_0 = 0.0f, // Optional
                blendConstants_1 = 0.0f, // Optional
                blendConstants_2 = 0.0f, // Optional
                blendConstants_3 = 0.0f, // Optional
            };

            VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
                setLayoutCount = 0, // Optional
                pSetLayouts = null, // Optional
                pushConstantRangeCount = 0, // Optional
                pPushConstantRanges = null, // Optional
            };
            VkPipelineLayout pipelineLayout = default( VkPipelineLayout );

            renderer.VSupport.Device.CreatePipelineLayout(ref pipelineLayoutInfo, ref pipelineLayout);
          
            renderer.PipelineLayout = pipelineLayout;


            VkShaderModule[] shaderModules = new VkShaderModule[shaderSource.Count];
            VkPipelineShaderStageCreateInfo[] stageInfo = new VkPipelineShaderStageCreateInfo[shaderSource.Count];

            for (int i = 0; i < shaderSource.Count; i++)
            {
                shaderModules[i] = renderer.VSupport.Device.CreateShaderModule(shaderSource[i].SpirVByte);
                stageInfo[i] = new VkPipelineShaderStageCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = shaderSource[i].ShaderStageType,
                    module = shaderModules[i],
                    pName = shaderSource[i].EntryName.ToPointer(),
                };

            }

            
            fixed (VkPipelineShaderStageCreateInfo* shaderStages = &stageInfo[0])
            {


                VkGraphicsPipelineCreateInfo pipelineInfo = new VkGraphicsPipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    stageCount = (uint)shaderModules.Length,
                    pStages = shaderStages,
                    pVertexInputState = &vertexInputInfo,
                    pInputAssemblyState = &inputAssembly,
                    pViewportState = &viewportState,
                    pRasterizationState = &rasterizer,
                    pMultisampleState = &multisampling,
                    pDepthStencilState = null, // Optional
                    pColorBlendState = &colorBlending,
                    pDynamicState = null, // Optional
                    layout = pipelineLayout,
                    renderPass = renderer.RenderPass,
                    subpass = 0,
                    basePipelineHandle = 0, // Optional
                    basePipelineIndex = -1, // Optional
                };

                VkPipeline graphicsPipeline = default(VkPipeline);

                VkPipeline* graphicsPipelinePtr = &graphicsPipeline;
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateGraphicsPipelines(renderer.VSupport.Device, 0, 1, &pipelineInfo, null, graphicsPipelinePtr));
                }

                renderer.GraphicsPipeline = graphicsPipeline;
            }

            for (int i = 0; i < shaderModules.Length; i++)
            {
                VulkanNative.vkDestroyShaderModule(renderer.VSupport.Device, shaderModules[i], null);
            }
            

        }

    }
}
