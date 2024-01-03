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
using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using GPUMauiLib.GPURenderers;
using VulkanPlatform;

namespace GPUMauiLib.GPUMetal
{
	public static class MetalConfig
	{
		public static void ConfigureDepthStencil(RendererMetal renderer)
		{
#if DEBUG
            VulkanFlowTracer.AddItem("MetalConfig.ConfigureDepthStencil");
#endif
            var depthStateDesc = new MTLDepthStencilDescriptor
            {
                DepthCompareFunction = MTLCompareFunction.Less,
                DepthWriteEnabled = true
            };

            renderer.DepthStencilState = renderer.Support.MetalDevice.CreateDepthStencilState(depthStateDesc);
        }


        public static void ConfigureView(RendererMetal renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalConfig.ConfigureView");
#endif
            MTKView metalView = renderer.View;
            metalView.Delegate = renderer;

            metalView.SampleCount = 1;
            metalView.DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8;
            metalView.ColorPixelFormat = MTLPixelFormat.BGRA8Unorm;
            metalView.PreferredFramesPerSecond = 60;
            metalView.ClearColor = new MTLClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        }
	}
}

