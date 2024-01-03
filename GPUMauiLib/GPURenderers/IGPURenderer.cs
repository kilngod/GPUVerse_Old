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

#if IOS || MACCATALYST
using Metal;
using MetalKit;
#endif

namespace GPUMauiLib.GPURenderers
{
	public interface IGPURenderer : IDisposable
	{
        bool Active { get; set; }

        bool PipelineInitialized { get; set; }

		void SetupPipeline();

        void RequestRender();

#if IOS || MACCATALYST
        void Draw(MTKView view);
#else
        void Draw();
#endif
        void AddToView(PlatformGPUView platformGPUView);

        void RemoveFromView();

        void SizeChanged(float width, float height);
        

    }
}

