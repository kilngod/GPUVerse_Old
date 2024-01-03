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
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif
using Microsoft.Maui.Graphics;

namespace GPUMauiLib.GPUViews
{
	public partial class GPUView : View, IGPUView, IDrawable
    {

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
           throw new NotImplementedException();

        }

        /*
        public static GPUView GPUOpenGL(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.OpenGL, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }

        public static GPUView GPUDirectX(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.DirectX, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
        }
        
        */

        public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
        {
            return new GPUView(GPUEngine.Vulkan, GPUPlatform.Windows)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
                --, ZIndex = 10
            };
        }
     
   
    }
}

