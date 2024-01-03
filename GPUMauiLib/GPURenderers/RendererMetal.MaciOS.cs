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


using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using System.Numerics;
using GPUMauiLib.GPUViews;
using Microsoft.Maui.Controls;
using System.Text;
using Microsoft.Maui;
using GPUMauiLib.GPUDevices;
using GPUMauiLib.GPUMetal;
using VulkanPlatform;

#nullable disable

namespace GPUMauiLib.GPURenderers
{
	public class RendererMetal : NSObject, IMTKViewDelegate, IGPURenderer
    {

        PlatformGPUView _platformView;

        public PlatformGPUView View { get { return _platformView; } }

        object _RenderLock = new object();

        public GPUSupportMetal Support { get { return _platformView.Support as GPUSupportMetal; } }

        public bool PipelineInitialized { get; set; } = false;


        public Vector4[] VertexData = new Vector4[]
        {
              // TriangleList                                      
              new Vector4(0f, 0.5f, 0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
              new Vector4(0.5f, -0.5f, 0.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
              new Vector4(-0.5f, -0.5f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
        };

        public bool Active { get; set; } = false;
   
        //metal support must move to renderer
    
       


        public IMTLRenderPipelineState PipelineState { get; set; } 
        public IMTLDepthStencilState DepthStencilState { get; set; }
        public IMTLBuffer VertexBuffer { get; set; }

        public IMTLCommandQueue CommandQueue { get; private set; }


        public RendererMetal()
		{
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.RendererMetal");
#endif


		}

        public void AddToView(PlatformGPUView platformGPUView)
        {

#if DEBUG
            VulkanFlowTracer.AddItem($"RendererMetal.AddToView");
#endif
            _platformView = platformGPUView;
            _platformView.Delegate = this;

            CommandQueue = (_platformView.Support as GPUSupportMetal).MetalDevice.CreateCommandQueue();
        }

        public void RemoveFromView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.RemoveFromView");
#endif
            lock (_RenderLock)
            {
                Active = false;

                if (_platformView != null)
                {
                    _platformView.Delegate = null;
                    _platformView = null;
                }
            }
        }

        public void RequestRender()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.RequestRender");
#endif
            if (_platformView != null)
            {
                Active = true;
                if (_platformView.EnableSetNeedsDisplay)
                {
                    _platformView.SetNeedsDisplay();
                }
                else
                {
                    _platformView.Draw(_platformView.Bounds);
                }
            }
        }

       

        protected override void Dispose(bool disposing)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.Dispose");
#endif
            base.Dispose(disposing);
        }

        public void Draw(MTKView view)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.Render");
#endif
            MetalDraw.Draw(this);
        }

        public void DrawableSizeWillChange(MTKView view, CGSize size)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.DrawableSizeWillChange");
#endif
            throw new NotImplementedException();
        }


        public async void SetupPipeline()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.SetupPipeline");
#endif
            await Support.AddLibraryKernels("Triangles", MetalTriangleShaderSource.TriangleShaders);

            MetalConfig.ConfigureDepthStencil(this);
            MetalConfig.ConfigureView(this);
            MetalPipeline.SetupPipeline(this);
        }

       

        public void Invalidate()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.Invalidate");
#endif
            throw new NotImplementedException();
        }

        public void SizeChanged(float width, float height)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.SizeChanged");
#endif
            throw new NotImplementedException();
        }

     
    }
}

