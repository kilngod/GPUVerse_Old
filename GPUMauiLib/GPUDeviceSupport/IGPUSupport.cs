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
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUViews;

namespace GPUMauiLib.GPUDevices
{
    public interface IGPUSupport : IDisposable
    {
        event Action RendererCreated;
        event Action RendererRemoved;

        public IGPURenderer Renderer { get; }
        public PlatformGPUView PlatformGPUView { get; }
        void AttachPlatformView(PlatformGPUView view);
        void ReleasePlatformView();

     

    }
}

