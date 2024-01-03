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

using GPUMauiLib.GPUViews;
using Microsoft.Maui.Controls;
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;



#if WINDOWS
using Windows.UI.Core;


using WinRT;
#endif

namespace GPUMauiLib
{
	/// <summary>
	/// GPUWindow
	/// 
	/// This class is a wrapper to the application's primary window to support GPU rendering.
	/// A maui application's "content" pages share a primary window, calling AddWindow on the 
	/// application allows multiple window interface for desktop applications. 
	/// </summary>
	public class GPUWindow : Window
#if WINDOWS
  //      ,ISwapChainSurface
#endif
    {
        public GPUWindow() :base()
		{
		}

		public GPUWindow(Microsoft.Maui.Controls.Page page) : base(page)
		{
			
		}

#if WINDOWS
		public nint Hwnd { get { return ((MauiWinUIWindow)Handler.PlatformView).WindowHandle; } }

		public IntPtr HInstance { get { return Marshal.GetHINSTANCE(typeof(PlatformGPUView).Module); } }

		Application _app;
		public Application App { get { return _app; } set { _app = value; } }

		

        /*
		PlatformGPUView _view;
		public void SetSwapChainPanel(PlatformGPUView panel)
		{
			Kind = SwapChainSurfaceType.SwapChainPanel;
			_view = panel;

        }
		public SwapChainSurfaceType Kind { get; set; }
	
        public nint ContextHandle => throw new NotImplementedException();

        public nint Handle { get { return ((IWinRTObject)_view).NativeObject.GetRef(); } }
			*/


   

#endif

    }
}

