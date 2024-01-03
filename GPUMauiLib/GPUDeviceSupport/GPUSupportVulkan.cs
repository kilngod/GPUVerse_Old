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
using GPUMauiLib.GPUViews;
using GPUMauiLib.GPURenderers;
using GPUVulkan;

#if ANDROID
using static Android.Provider.SyncStateContract;
using Android.Mtp;

#endif

#if MACCATALYST || IOS
using CoreGraphics;
using Metal;
using MetalKit;
#endif



using System.Runtime.CompilerServices;

using System.Runtime.InteropServices;

using System.Collections.Generic;
//using SharpGen.Runtime;

using VulkanPlatform;



using System.Text;
#nullable disable

namespace GPUMauiLib.GPUDevices
{
    
	public class GPUSupportVulkan: GPUSupport,IGPUSupport
    {

        public VulkanSupport VSupport { get; private set; }


#if IOS || MACCATALYST
        public IMTLDevice MetalDevice { get; private set; }

#endif

        public GPUSupportVulkan(GPUPlatform platform, GPUEngine engine):base(platform, engine)
        {
            InitializeGPU();
        }


        private void InitializeGPU()
        {
#if MACCATALYST || IOS
            MetalDevice = MTLDevice.SystemDefault;
#endif

            switch (_platform)
            {
                case GPUPlatform.Android:
                    VSupport = new VulkanSupport(DeliveryPlatform.Android);
                    break;
                case GPUPlatform.iOS:
                    VSupport = new VulkanSupport(DeliveryPlatform.iOS);
                    break;
                case GPUPlatform.MacCataylst:
                    VSupport = new VulkanSupport(DeliveryPlatform.MacCatalyst);
                    break;
                case GPUPlatform.Windows:
                    throw new Exception("Platform Not Supported.");
                    // We tried Maui Windows but all we could get were a few initial renders before WinRT(DirectX) took control and
                    // screwed over Vulkan rendering. We tried opening a child window and got screwed over too. As far as we can tell the
                    // WinRT compositor is so locked down as to not support direct access to the renderer even with DirectX.
                    // The compositor supports is own version of DirectX 2D rendering and little more.
                    // DirectX 3D rendering appears to be completely off limits with maui.
                    // Sadly, in our opinion Microsoft overthought WinRT as a software development framework, which 
                    // means noboby will be using Maui for Game Apps to the Windows store. Very Sad! 
                    //
                    // VSupport = new VulkanSupport(DeliveryPlatform.Windows);
                    // break;
                default:
                    throw new Exception("Platform Not Supported.");
                    
            }


#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.InitializeGPU");
#endif
            VSupport.CreateInstance();
            VSupport.SetupDebugMessenger();

          

        }






#nullable enable




     


        public override void AddRendererToVew()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.AddRendererToVew");
#endif
            if (_engine == GPUEngine.Vulkan)
            {
                _renderer = new RendererVulkan();
                _renderer.AddToView(_platformGPUView);
               // _renderer.SetupPipeline();
                base.AddRendererToVew();
            }
          

        }

        public override void RemoveRendererFromView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.RemoveRendererFromView");
#endif
            _renderer.RemoveFromView();
            base.RemoveRendererFromView();

        }

        public override void ViewSizeChanged()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.ViewSizeChanged");
#endif
        }

        public unsafe void Dispose()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupportvulkan.Dispose");
#endif
            if (VSupport != null)
            {
                VSupport.CleanupVulkanSupport();
            }
        

        }
    }

    
}

