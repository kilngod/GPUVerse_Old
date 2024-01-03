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

using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUViews;
using CoreGraphics;
using Metal;
using MetalKit;
using SpriteKit;
using UIKit;
using GPUMauiLib.GPUDevices;
using VulkanPlatform;
#nullable disable
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
                    case GPUEngine.Metal:

                        _gpuSupport = new GPUSupportMetal(VirtualView.Platform, VirtualView.Engine);
                        _gpuSupport.RendererCreated += RendererCreated;
                        _gpuSupport.RendererRemoved += RendererRemoved;
                        break;

                    case GPUEngine.Vulkan:

                        _gpuSupport = new GPUSupportVulkan(VirtualView.Platform, VirtualView.Engine);
                        _gpuSupport.RendererCreated += RendererCreated;
                        _gpuSupport.RendererRemoved += RendererRemoved;

                        break;
                }
            }

           

            double _height = VirtualView.Height < 0 ? VirtualView.HeightRequest : VirtualView.Height;
            double _width = VirtualView.Width < 0 ? VirtualView.WidthRequest : VirtualView.Width;
            Rect rect = new Rect(VirtualView.Frame.Left, VirtualView.Frame.Top, _width, _height);
            CGSize drawableSize = new CGSize(_width * UIScreen.MainScreen.Scale, _height * UIScreen.MainScreen.Scale);

            PlatformGPUView mtkView;
            switch (VirtualView.Engine)
            {
                case GPUEngine.Metal:

                    mtkView = new PlatformGPUView(rect, _gpuSupport as GPUSupportMetal)
                    {
                        AutoResizeDrawable = true,
                        ContentScaleFactor = UIScreen.MainScreen.Scale,
                        DrawableSize = drawableSize
                    };



                    return mtkView;

                case GPUEngine.Vulkan:

                    mtkView = new PlatformGPUView(rect, _gpuSupport as GPUSupportVulkan)
                    {
                        AutoResizeDrawable = true,
                        ContentScaleFactor = UIScreen.MainScreen.Scale,
                        DrawableSize = drawableSize
                    };



                    return mtkView;
            }

            return null;
            
        }

        private void RendererCreated()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformViewHandler.RendererCreated");
#endif
            if (!_gpuSupport.Renderer.PipelineInitialized)
            {
                _gpuSupport.Renderer.SetupPipeline();
                _gpuSupport.Renderer.Active = true;
            }
            _gpuSupport.Renderer.RequestRender();

        }

        private void RendererRemoved()
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
            base.ConnectHandler(platformView);

            platformView.Connect(VirtualView);

            _gpuSupport.AttachPlatformView(this.PlatformView);

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
