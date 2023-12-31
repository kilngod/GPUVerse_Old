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
    public interface IVulkanCompute
    {
        int ComputeFamilyIndex { get; }
        IVulkanSupport Support { get; }
        VkCommandPool CommandPool { get; set; }
        int ComputeCommandBuffers { get; set; }
        VkCommandBuffer[] CommandBuffers { get; set; }
        VkPipelineLayout PipelineLayout { get; }
        VkPipeline ComputePipeline { get; }
        uint ComputeDescriptorSets { get; set; }
        VkDescriptorSet[] DescriptorSets { get; }
        VkDescriptorSetLayout ComputeLayout { get; set; }
        VkSemaphore ComputeSemaphore { get; set; }
        VkQueue ComputeQueue { get; }
        void SetupComputePipeline();

        /*
         * 
         * 
         // setup for vulkan
          ProbeInstallation();
    CreateInstance();
    RegisterDebugReportCallback();
    GetPhysicalDevice();
    FindQueueFamily();
    CreateLogicalDevice();
    GetQueue();
        // compute
    CreateBuffer();
    AllocateDeviceMemory();
    BindDeviceMemory();
    CreateDescriptorSetLayout();
    CreateDescriptorPool();
    CreateDescriptorSets();
    ConnectBufferWithDescriptorSets();
    CreateShaderModule("shaders/comp.spv");
    CreateComputePipeline();
    CreateCommandPool();
    CreateCommandBuffers();
    FillCommandBuffer();
    SubmitAndWait();
    SaveRenderedImage("mandelbrot.png");

        */
    }
}

