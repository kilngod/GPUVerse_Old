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
namespace GPUMauiLib.GPUViews
{
	public partial class GPUView : View, IGPUView
	{
		/*
		public static GPUView GPUOpenGL(int requestedWidth, int requestedHeight)
		{
			return new GPUView(GPUEngine.OpenGL, GPUPlatform.Android)
            {
                WidthRequest = requestedWidth,
                HeightRequest = requestedHeight
            };
		}
		*/
		public static GPUView GPUVulkan(int requestedWidth, int requestedHeight)
		{
			return new GPUView(GPUEngine.Vulkan, GPUPlatform.Android)
			{
				WidthRequest=requestedWidth,
				HeightRequest=requestedHeight
			};
		}


	}
}

