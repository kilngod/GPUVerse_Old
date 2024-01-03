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

using CoreAnimation;
using Foundation;
using VulkanPlatform;

#nullable disable

namespace GPUMauiLib.GPURenderers
{
    /// <summary>
    /// Animator for iOS/Macatalyst, not sure this service useful as MTKView has a built-in service.
    /// </summary>
    public partial class GPUAnimator
    {

        CADisplayLink displayLink;

        public void start()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUAnimator.start");
#endif

            isRunning = true;
            displayLink = CADisplayLink.Create(update);
            displayLink?.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);
        }


        public void cancel()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUAnimator.cancel");
#endif

            isRunning = false;
            this.displayLink?.RemoveFromRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);
            this.displayLink = null;
        }
    }
}

