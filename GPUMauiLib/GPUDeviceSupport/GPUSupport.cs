// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: GPUSupport.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUViews;
using VulkanPlatform;

#nullable disable

namespace GPUMauiLib.GPUDevices
{
    public class GPUSupport
    {
        protected IGPURenderer _renderer;
        protected PlatformGPUView _platformGPUView;

        public PlatformGPUView PlatformGPUView { get { return _platformGPUView; } }
        public IGPURenderer Renderer { get { return _renderer; } }
    
        protected GPUEngine _engine;
        protected GPUPlatform _platform;
  

        public event Action RendererCreated;
        public event Action RendererRemoved;
        public event Action SizedChanged;


        public GPUSupport(GPUPlatform platform, GPUEngine engine)
        {

            _engine = engine;
            _platform = platform;

        }

        public virtual void AttachPlatformView(PlatformGPUView view)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.AttachPlatformView");
#endif

            _platformGPUView = view;
            _platformGPUView.ViewLoaded += AddRendererToVew;
            _platformGPUView.ViewSizeChanged += ViewSizeChanged;
            _platformGPUView.ViewRemoved += RemoveRendererFromView;

        }

        public virtual void ReleasePlatformView()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUSupport.ReleasePlatformView");
#endif
            // stop rendering pipeline
            _platformGPUView.ViewLoaded -= AddRendererToVew;
            _platformGPUView.ViewSizeChanged -= ViewSizeChanged;
            _platformGPUView.ViewRemoved -= RemoveRendererFromView;
            _platformGPUView = null;
        }

        public virtual void AddRendererToVew()
        {
           
            RendererCreated?.Invoke();

        }

        public virtual void RemoveRendererFromView()
        {
            RendererRemoved?.Invoke();

        }

        public virtual void ViewSizeChanged()
        {
            SizedChanged?.Invoke();
        }


    }
}

