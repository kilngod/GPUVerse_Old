// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: GPUSupportMetal.MaciOS.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUMauiLib.GPUViews;
using CoreGraphics;
using Metal;
using MetalKit;
using SpriteKit;
using UIKit;
using CoreAnimation;
using Foundation;
using GPUMauiLib.GPURenderers;
using VulkanPlatform;
#nullable disable
namespace GPUMauiLib.GPUDevices
{
	public partial class  GPUSupportMetal : GPUSupport, IGPUSupport
    {
	


        public IMTLDevice MetalDevice { get; private set; }

        public IMTLLibrary MetalLibrary { get; set; } // should be on renderer?


        public GPUSupportMetal(GPUPlatform platform, GPUEngine engine):base(platform,engine)
		{
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal");
#endif
            
            InitializeGPU();
        }

        public void InitializeGPU()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal.InitializeGPU");
#endif
            MetalDevice = MTLDevice.SystemDefault;
            
        }
       


        public override void AddRendererToVew()
		{
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal.AddRendererToVew");
#endif
            if (_engine==GPUEngine.Metal)
			{
				_renderer = new RendererMetal();
                _renderer.AddToView(_platformGPUView);
                base.AddRendererToVew();
                
			}


		}

        public override void RemoveRendererFromView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal.RemoveRendererFromView");
#endif
            _renderer.RemoveFromView();
            base.RemoveRendererFromView();
            
        }

        public override void ViewSizeChanged()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal.ViewSizeChanged");
#endif
        }

        public void Dispose()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportMetal.Dispose");
#endif
            throw new NotImplementedException();
        }
    }


}

