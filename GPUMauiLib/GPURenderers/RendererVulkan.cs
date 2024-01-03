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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using GPUMauiLib.GPUViews;
using GPUMauiLib.GPUDevices;


using System.Runtime.CompilerServices;

#if ANDROID
using Android.Views;
using Android.Runtime;
using static Android.Provider.SyncStateContract;
using static AndroidX.Core.Content.PM.ShortcutInfoCompat;

#endif

#if WINDOWS

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

using WinRT.Interop;


#endif

using Microsoft.Maui.Controls;
using GPUVulkan;
using VulkanPlatform;
using GPUMauiLib.GPUMaui;





#if IOS || MACCATALYST
using Foundation;
using MetalKit;
using CoreGraphics;
#endif

#nullable disable

//using SharpGen.Runtime;

namespace GPUMauiLib.GPURenderers
{
    public unsafe class RendererVulkan:
#if MACCATALYST || IOS
        NSObject, IMTKViewDelegate, IGPURenderer, IVulkanRenderer
    
#elif WINDOWS
        IGPURenderer, IDrawable, IVulkanRenderer
#else

        IGPURenderer, IVulkanRenderer
#endif
    {
        PlatformGPUView _platformView;

        public PlatformGPUView PlatformView { get { return _platformView; } }

        public IVulkanSupport VSupport { get { return (_platformView.Support as GPUSupportVulkan).VSupport; } }

        public GPUSupportVulkan Support { get { return _platformView.Support as GPUSupportVulkan; } }


        public bool Active { get; set; } = false;

    
        private VkSurfaceKHR _surface = default(VkSurfaceKHR);

        public VkSurfaceKHR Surface { get { return _surface; } }


        private VkSwapchainKHR  _swapchain;
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

        public bool PipelineInitialized { get; set; } = false;


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

        public int Width { get; set; }
        public int Height { get; set; }
        

#if ANDROID
        private IntPtr _window;
#endif
        public RendererVulkan()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.RendererVulkan (create)");
#endif

        }



        // Add methods for creating other Vulkan objects, like surface, physical device, logical device, swap chain, etc.
        // ...

        GPUAnimator Animator;

        public void RequestRender()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.RequestRender");

#endif
// Record command buffers, submit them to the queue, and present the frame
// ...
#if IOS || MACCATALYST
            // Apple will call the renderer directly
#else
            Animator = new GPUAnimator();
            Animator.set(Draw);
            //if (AutoReDraw == true)
                Animator.start();
#endif
        }

#if IOS || MACCATALYST

        public void DrawableSizeWillChange(MTKView view, CGSize size)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.DrawableSizeWillChange");
#endif
            throw new NotImplementedException();
        }


        // IOS and MACCATALYST will trigger drawing from the framework.
        // Likely better to let Apple trigger the draw than control it ourselves.
        public void Draw(MTKView view)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererVulkan.Draw (MTKView Triggered)");
#endif

            if (Active)
            {
                this.DrawFrame();
            }
        }




#else
      
        public void Draw()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.Render");
            //          string path = VulkanFlowTracer.FlowContentLog();
#endif
            // Record command buffers, submit them to the queue, and present the frame
            // ...
            if (Active)
            {
                VulkanDraw.DrawFrame(this);

#if WINDOWS
             //   BringWindowToFront(_platformView.ChildWindow,Convert.ToInt32(_platformView.ActualOffset.X), Convert.ToInt32(_platformView.ActualOffset.Y)) ;
                //_platformView.View.Invalidate();
#endif
            }
        }

        public void Dispose()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.Dispose");
#endif
            CleanUpPipeline();

        }

#endif


        public void SetupPipeline()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.SetupPipeline");
#endif

            CreateSurface();
            this.ConfigureDevices(_surface);
#if ANDROID || MACCATALYST || IOS || WINDOWS
            this.SurfaceExtent2D = new VkExtent2D(_platformView.BestWidth, _platformView.BestHeight);
#endif
            this.CreateSwapChain(_surface, ref _swapchain);
            this.CreateImageViews();
            this.CreateRenderPass();

            List<VulkanSpirV> orderedShaderList = new List<VulkanSpirV>();
             
            VulkanSpirV vert = new VulkanSpirV() { EntryName = "main", Name = "Vertex for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_VERTEX_BIT, SpirVByte= MauiIO.LoadRawResource("vert.spv") };
            orderedShaderList.Add(vert);
            VulkanSpirV frag = new VulkanSpirV() { EntryName = "main", Name = "Fragment for Trangle", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT, SpirVByte = MauiIO.LoadRawResource("frag.spv") };
            orderedShaderList.Add(frag);
            this.CreateGraphicsPipeline(orderedShaderList);

            this.CreateFramebuffers();
            this.CreateCommandPool();
            this.CreateCommandBuffers();
            this.CreateSyncSemaphores();
        }

        private void CleanUpPipeline()
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



#if WINDOWS

   

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
uint dwExStyle,
string lpClassName,
string lpWindowName,
uint dwStyle,
int x,
int y,
int nWidth,
int nHeight,
IntPtr hWndParent,
IntPtr hMenu,
IntPtr hInstance,
IntPtr lpParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        public void BringWindowToFront(IntPtr hwnd, int x, int y )
        {
            
            const uint SWP_NOMOVE = 0x0002;
            const uint SWP_NOSIZE = 0x0001;
            const uint SWP_SHOWWINDOW = 0x0040;

            if (!SetWindowPos(hwnd, IntPtr.Zero, x, y, 0, 0,  SWP_NOSIZE | SWP_SHOWWINDOW))
            {
                int errorCode = Marshal.GetLastWin32Error();
            }
        }
#endif


        private unsafe void CreateSurface()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.CreateSurface");
#endif


#if IOS || MACCATALYST

            
            VulkanSurface.CreateMetalLayerSurface(VSupport,ref _surface, _platformView.Layer.Handle);

#elif WINDOWS
            GPUWindow window = _platformView.View.Window as GPUWindow;
            
            const uint WS_CHILD = 0x40000000;
            const uint WS_VISIBLE = 0x10000000;
            const uint WS_TABSTOP = 0x00010000; // allow user input?

            int x = Convert.ToInt32( PlatformView.ActualOffset.X);
            int y = Convert.ToInt32(PlatformView.ActualOffset.Y); 


         /*   PlatformView.ChildWindow = CreateWindowEx(0, "STATIC", string.Empty, WS_CHILD| WS_VISIBLE| WS_TABSTOP, Convert.ToInt32(_platformView.ActualOffset.X), Convert.ToInt32(_platformView.ActualOffset.Y), _platformView.BestWidth, _platformView.BestHeight,window.Hwnd, IntPtr.Zero, window.HInstance, IntPtr.Zero);

            if (PlatformView.ChildWindow == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
            }
         */
            fixed (VkSurfaceKHR* surfacePtr = &_surface)
            {
                VulkanSurface.CreateWin32Surface(VSupport, surfacePtr, window.Hwnd, window.HInstance);
            }
#elif ANDROID
            fixed (VkSurfaceKHR* surfacePtr = &_surface)
            {
                VulkanSurface.CreateAndroidSurface(VSupport, surfacePtr, _platformView.AndroidWidow);
            }
#endif

           

        }

        //   private VkImageView[] swapChainImageViews;



        public void AddToView(PlatformGPUView platformGPUView)
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.AddToView");
#endif
           _platformView = platformGPUView;
#if MACCATALYST || IOS
            _platformView.Delegate = this;
#endif
            //await this.LoadSharderSpirV();

#if ANDROID

            _platformView.AndroidSurfaceCreated += _platformView_AndroidSurfaceCreated;
            _platformView.AndroidSurfaceChanged += _platformView_AndroidSurfaceChanged;
            _platformView.AndroidSurfaceDestoryed += _platformView_AndroidSurfaceDestoryed; 
#endif

        }


#if ANDROID
        private void _platformView_AndroidSurfaceDestoryed()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererVulkan._platformView_AndroidSurfaceDestoryed");
#endif

        }


        private void _platformView_AndroidSurfaceChanged()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererVulkan._platformView_AndroidSurfaceChanged");
#endif
            if (PipelineInitialized && (SurfaceExtent2D.width != _platformView.BestWidth || SurfaceExtent2D.height != _platformView.BestHeight))
            {
                this.UpdateSwapChainImageSize(_surface, ref _swapchain, _platformView.BestWidth, _platformView.BestHeight);
            }
            
        }

        private void _platformView_AndroidSurfaceCreated(ISurfaceHolder holder)
        {

            // this should be thread synced
#if DEBUG
            VulkanFlowTracer.AddItem("RendererVulkan._platformView_AndroidSurfaceCreated");
#endif

         //   _window = VulkanAndoidRuntime.ANativeWindow_fromSurface(Java.Interop.JniEnvironment.EnvironmentPointer, holder.Handle);



        }

#endif


        public void RemoveFromView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.RemoveFromView");
#endif

#if ANDROID

            _platformView.AndroidSurfaceCreated -= _platformView_AndroidSurfaceCreated;
            _platformView.AndroidSurfaceChanged -= _platformView_AndroidSurfaceChanged;
            _platformView.AndroidSurfaceDestoryed -= _platformView_AndroidSurfaceDestoryed;
#endif

            _platformView = null;
        }



        public void SizeChanged(float width, float height)
        {
#if DEBUG
            VulkanFlowTracer.AddItem($"RendererVulkan.SizeChanged");
#endif
            throw new NotImplementedException();
        }

       

#if WINDOWS
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Active)
            {
                VulkanDraw.DrawFrame(this);
            }
        }
#endif
    }
}
