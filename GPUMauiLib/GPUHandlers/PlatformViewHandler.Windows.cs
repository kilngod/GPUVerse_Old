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

using Microsoft.Maui.Handlers;
using GPUMauiLib.GPUViews;
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUDevices;
using System.Drawing;
using VulkanPlatform;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

#endif

namespace GPUMauiLib.GPUHandlers
{



    public partial class PlatformViewHandler : ViewHandler<GPUView, PlatformGPUView>
    {
        
        protected override PlatformGPUView CreatePlatformView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.CreatePlatformView");
#endif
            if (_gpuSupport == null)
            {
                switch (VirtualView.Engine)
                {
                    /*
                    case GPUEngine.DirectX:
                        
                        _gpuSupport = new GPUSupportDirectX(VirtualView.Platform, VirtualView.Engine);
                        _gpuSupport.RendererCreated += RendererCreated;
                        _gpuSupport.RendererRemoved += RendererRemoved;
                      
                        break;
                    */
                    case GPUEngine.Vulkan:
                        _gpuSupport = new GPUSupportVulkan(VirtualView.Platform, VirtualView.Engine);
                        _gpuSupport.RendererCreated += RendererCreated;
                        _gpuSupport.RendererRemoved += RendererRemoved;
                        break;

                 

                }


            }

            PlatformGPUView platformView = new PlatformGPUView(_gpuSupport as GPUSupport);

            platformView.Loaded += PlatformView_Loaded;
            platformView.Unloaded += PlatformView_Unloaded;
            platformView.SizeChanged += PlatformView_SizeChanged;
            

            //GPUWindow window = platformView.View.Window as GPUWindow;



            return platformView;

            
        }

      
       
        private void PlatformView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.SwapChainPanel_SizeChanged");
#endif
            PlatformGPUView platformView = sender as PlatformGPUView;

            if ((e.NewSize.Width > 0 && e.NewSize.Height> 0) &&(e.NewSize.Width != platformView.Width || e.NewSize.Height != platformView.Height))
            {
                platformView.Width = e.NewSize.Width;
                platformView.Height = e.NewSize.Height;


            }
          
        }

        private void PlatformView_Unloaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.SwapChainPanel_Unloaded");
#endif
            PlatformGPUView platformView = sender as PlatformGPUView;

        }

        private void PlatformView_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.SwapChainPanel_Loaded");
#endif
            PlatformGPUView platformView = sender as PlatformGPUView;

            platformView.Support.AddRendererToVew();

            
        }

        protected void RendererCreated()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.RendererCreated");
#endif
            if (!_gpuSupport.Renderer.PipelineInitialized)
            {
                _gpuSupport.Renderer.SetupPipeline();
                _gpuSupport.Renderer.Active = true;
            }
            _gpuSupport.PlatformGPUView.Drawable = _gpuSupport.Renderer as IDrawable;
            _gpuSupport.Renderer.RequestRender();

            
        }

        protected void RendererRemoved()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.RendererRemoved");
#endif
            _gpuSupport.Renderer.Active = false;

        }

        
        protected override void ConnectHandler(PlatformGPUView platformView)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.ConnectHandler");
#endif
            platformView.Connect(VirtualView);

            base.ConnectHandler(platformView);

            _gpuSupport.AttachPlatformView(platformView);

           




        }

    protected override void DisconnectHandler(PlatformGPUView platformView)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.DisconnectHandler");
#endif
            platformView.Disconnect();

            // Perform cleanup when disconnecting the handler from the virtualView
            _gpuSupport.ReleasePlatformView();

            base.DisconnectHandler(platformView);



        }

#nullable enable
        public static void MapInvalidate(IGPUViewHandler handler, IGPUView view, object? arg)
        {
            handler.PlatformView?.Invalidate();
        }




#nullable disable

    }
}
