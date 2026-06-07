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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanPlatform;


#nullable disable
namespace WinVulkanApp
{
    public class WinVulkanRenderer : IVulkanRenderer
    {
        public WinVulkanRenderer(Form window)
        {
            _window = window;
        }

        Form _window;

        public IntPtr HInstance { get { return Marshal.GetHINSTANCE(typeof(WinVulkanRenderer).Module); } }
        public bool PipelineInitialized { get; set; } = false;

        public IVulkanSupport VSupport { get; set; }

        private VkSurfaceKHR _surface = default(VkSurfaceKHR);

        public VkSurfaceKHR Surface { get { return _surface; } }

        private VkSwapchainKHR _swapchain;
        public VkSwapchainKHR SwapChain { get { return _swapchain; } }

        private VkImage[] _swapChainImages;
        public VkImageView[] _swapChainImageViews;
        public void CreateSwapChainImages(uint imageCount)
        {
            _swapChainImages = new VkImage[imageCount];
            _swapChainImageViews = new VkImageView[imageCount];
        }

        public VkImage[] SwapChainImages { get { return _swapChainImages; } }
        public VkImageView[] SwapChainImageViews { get { return _swapChainImageViews; } }


        public SwapChainSupportDetails SwapChainDetails { get; set; }
        public VkSurfaceFormatKHR SurfaceFormat { get; set; }
        public VkExtent2D SurfaceExtent2D { get; set; }
        public VkPresentModeKHR PresentMode { get; set; }
        public VkFormat ImageFormat { get; set; }
        public VkRenderPass RenderPass { get; set; }
        public VkPipelineLayout PipelineLayout { get; set; }
        public VkPipeline GraphicsPipeline { get; set; }
        public VkFramebuffer[] FrameBuffers { get; set; }
        public VkCommandPool CommandPool { get; set; }
        public VkCommandBuffer[] CommandBuffers { get; set; }
        public QueueFamilyIndices FamilyIndices { get; set; }
        public VkSemaphore ImageAvailableSemaphore { get; set; }
        public VkSemaphore RenderFinishedSemaphore { get; set; }


        public VkQueue _graphicsQueue;
        public VkQueue GraphicsQueue { get { return _graphicsQueue; } }
        public void SetGraphicsQueue(VkQueue graphicsQueue)
        {
            _graphicsQueue = graphicsQueue;
        }


        public VkQueue _presentQueue;
        public VkQueue PresentQueue { get { return _presentQueue; } }
        public void SetPresentQueue(VkQueue presentQueue)
        {
            _presentQueue = presentQueue;
        }

        public int Width { get { return Convert.ToInt32(_window.Width); } }
        public int Height { get { return Convert.ToInt32(_window.Height); } }


        private unsafe void CreateSurface()
        {
            if (_surface != VkSurfaceKHR.Null)
            {
                return;
            }

            fixed(VkSurfaceKHR* surfacePtr = &_surface)
            {
                VulkanSurface.CreateWin32Surface(VSupport, surfacePtr, _window.Handle, HInstance);
            }
        }


        public void SetupSurfaceAndDevice()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.SetupPipeline");
#endif

            CreateSurface();

            this.ConfigureDevices(_surface);
        }

        public void SetupPipeline()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.SetupPipeline");
#endif

            this.CreateSwapChain(_surface, ref _swapchain);
            this.CreateImageViews();
            this.CreateRenderPass();

            List<VulkanSpirV> orderedShaderList = new List<VulkanSpirV>();
            string resourceFolder = AppContext.BaseDirectory + "\\Shaders\\";
            VulkanSpirV vert = new VulkanSpirV() { EntryName = "main", Name = "Vertex for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_VERTEX_BIT, SpirVByte = LoadRawResource(resourceFolder + "vert.spv") };
            orderedShaderList.Add(vert);
            VulkanSpirV frag = new VulkanSpirV() { EntryName = "main", Name = "Fragment for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT, SpirVByte = LoadRawResource(resourceFolder + "frag.spv") };
            orderedShaderList.Add(frag);
            this.CreateGraphicsPipeline(orderedShaderList);

            this.CreateFramebuffers();
            this.CreateCommandPool();
            this.CreateCommandBuffers();
            this.CreateSyncSemaphores();
        }

        public static byte[] LoadRawResource(string ResourceFilePath)
        {
            byte[] byteResult;

            using (FileStream rawStream = new FileStream(ResourceFilePath, FileMode.Open))
            {

                // MemoryStream appearently corrects corruption issue with using Seek on MacCatalyst or Android
                MemoryStream stream = new MemoryStream();
                rawStream.CopyTo(stream);
                byteResult = stream.ToArray();



                rawStream.Close();
            }

            return byteResult;

        }

        public unsafe void CleanUpPipeline()
        {
            VulkanNative.vkDestroySemaphore(VSupport.Device, RenderFinishedSemaphore, null);
            VulkanNative.vkDestroySemaphore(VSupport.Device, ImageAvailableSemaphore, null);

            VulkanNative.vkDestroyCommandPool(VSupport.Device, CommandPool, null);

            foreach (var framebuffer in FrameBuffers)
            {
                VulkanNative.vkDestroyFramebuffer(VSupport.Device, framebuffer, null);
            }

            VulkanNative.vkDestroyPipeline(VSupport.Device, GraphicsPipeline, null);

            VulkanNative.vkDestroyPipelineLayout(VSupport.Device, PipelineLayout, null);

            VulkanNative.vkDestroyRenderPass(VSupport.Device, RenderPass, null);

            foreach (var imageView in SwapChainImageViews)
            {
                VulkanNative.vkDestroyImageView(VSupport.Device, imageView, null);
            }

            VulkanNative.vkDestroySwapchainKHR(VSupport.Device, _swapchain, null);

            if (_surface != VkSurfaceKHR.Null)
            {
                VulkanNative.vkDestroySurfaceKHR(VSupport.Instance, _surface, null);
                _surface = VkSurfaceKHR.Null;
            }

        }


    }
}
