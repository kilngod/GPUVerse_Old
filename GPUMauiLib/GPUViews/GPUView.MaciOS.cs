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

using UIKit;
using VulkanPlatform;

namespace GPUMauiLib.GPUViews
{
    public partial class GPUView : View, IGPUView
    {
        /*
        public static GPUView GPUOpenGL(int requestedWidth, int requestedHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUView.GPUOpenGL");
#endif
            return new GPUView(GPUEngine.OpenGL, GPUPlatform.MacCataylst)
            {

                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }
        */

        public static GPUView GPUMetal(int requestedWidth, int requestedHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUView.GPUMetal");
#endif

#if MACCATALYST

            return new GPUView(GPUEngine.Metal, GPUPlatform.MacCataylst)

#else
            return new GPUView(GPUEngine.Metal, GPUPlatform.iOS)
#endif
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }

        public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("GPUView.GPUVulkan");
#endif
#if MACCATALYST
            return new GPUView(GPUEngine.Vulkan, GPUPlatform.MacCataylst)
#else
            return new GPUView(GPUEngine.Vulkan, GPUPlatform.iOS)
#endif
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }
    }
}

