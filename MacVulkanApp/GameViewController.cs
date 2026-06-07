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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AppKit;
using CoreGraphics;
using GPUVulkan;
using Metal;
using MetalKit;
using ObjCRuntime;
using VulkanPlatform;

namespace MacVulkanApp
{
    public partial class GameViewController : NSViewController, IMTKViewDelegate, IVulkanRenderer
    {
        public IVulkanSupport VSupport { get; set; }
        public bool PipelineInitialized { get; set; } = false;
        public bool Active { get; set; } = false;

        MTKView _view;

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

        public int Width { get { return Convert.ToInt32(_view.DrawableSize.Width); } }
        public int Height { get { return Convert.ToInt32(_view.DrawableSize.Height); } }

        //GPUAnimator Animator;

        public GameViewController(NativeHandle handle) : base(handle)
        {

        }

        public void Draw(MTKView view)
        {
            if (Active)
            {
                VulkanDraw.DrawFrame(this);
            }
        }

        public void DrawableSizeWillChange(MTKView view, CGSize size)
        {
            this.UpdateSwapChainImageSize(_surface, ref _swapchain, Convert.ToInt32(size.Width), Convert.ToInt32(size.Height));
        }


        private void CreateSurface()
        {
            var metalLayer = _view.Layer ?? throw new InvalidOperationException("MTKView does not have a backing Metal layer.");
            VulkanSurface.CreateMetalLayerSurface(VSupport, ref _surface, metalLayer.Handle);

        }

        void SetupView()
        {
            _view = (MTKView)View;
            _view.Delegate = this;
            _view.WantsLayer = true;

            // Setup the render target, choose values based on your app
            _view.SampleCount = 4;
            _view.DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8;


        }

        protected void DoCompute()
        {
            ComputeSample computeSample = new ComputeSample(VSupport);

            computeSample.SetupComputePipeline();
            computeSample.Compute();

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            VSupport = new VulkanSupport(DeliveryPlatform.MacOS);


            SetupView();

            // update width and height and then update swapchain
            SurfaceExtent2D = new VkExtent2D(Width, Height);

            SetupSurfaceAndDevice();

            DoCompute(); // can't call this until the physical device is configured.

            SetupPipeline();

            Active = true;
        }

        public void SetupSurfaceAndDevice()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.SetupSurfaceAndDevice");
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
            string resourceFolder = GetResourceFolder();
            VulkanSpirV vert = new VulkanSpirV() { EntryName = "main", Name = "Vertex for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_VERTEX_BIT, SpirVByte = VulkanIO.LoadRawResource(Path.Combine(resourceFolder, "vert.spv")) };
            orderedShaderList.Add(vert);
            VulkanSpirV frag = new VulkanSpirV() { EntryName = "main", Name = "Fragment for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT, SpirVByte = VulkanIO.LoadRawResource(Path.Combine(resourceFolder, "frag.spv")) };
            orderedShaderList.Add(frag);
            this.CreateGraphicsPipeline(orderedShaderList);

            this.CreateFramebuffers();
            this.CreateCommandPool();
            this.CreateCommandBuffers();
            this.CreateSyncSemaphores();
        }

        private static string GetResourceFolder()
        {
            string baseDirectory = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string contentsDirectory = Directory.GetParent(baseDirectory)?.FullName;

            if (contentsDirectory is not null && Path.GetFileName(baseDirectory) == "MonoBundle")
            {
                return Path.Combine(contentsDirectory, "Resources");
            }

            return baseDirectory;
        }

        private unsafe void CleanUpPipeline()
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

            VulkanNative.vkDestroySurfaceKHR(VSupport.Instance, _surface, null);

        }



    }
}
