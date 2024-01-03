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


using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using GPUMauiLib.GPUHandlers;
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUDevices;
using System.Runtime.InteropServices;
using Java.Interop;
using GPUVulkan;
using Format = Android.Graphics.Format;
using Microsoft.Maui.Controls;
using VulkanPlatform;
#nullable disable

namespace GPUMauiLib.GPUViews
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct ANativeWindow
    {
    }


    public partial class PlatformGPUView : SurfaceView, ISurfaceHolderCallback, ISurfaceHolderCallback2
    {
        public event Action<ISurfaceHolder> AndroidSurfaceCreated;
        public event Action AndroidSurfaceDestoryed;
        public event Action AndroidSurfaceChanged;

        //AndroidWindow _androidWindow;
        protected IntPtr _aNativeWindow = IntPtr.Zero;
     
       
#nullable disable
        private nint _lastSurfaceHandle;
        // public IntPtr SurfaceHandle { get { return _surfaceHandle; } }

        public IntPtr AndroidWidow {get { return _aNativeWindow; } }



     

        public PlatformGPUView(Context context, IAttributeSet attrs, GPUSupport support) : base(context, attrs)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView (context+attrs)");
#endif
            Holder.AddCallback(this);
            _support = support;
            
        }

        public PlatformGPUView(Context context, GPUSupport support) : base(context)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView (context)");
#endif

       //     SurfaceView surface = new SurfaceView(context);
            
            
            Holder.AddCallback(this);

            _support = support;

        }

        protected PlatformGPUView(IntPtr javaReference, JniHandleOwnership transfer, GPUSupport support) : base(javaReference, transfer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView (javaReference)");
#endif
            Holder.AddCallback(this);
            Holder.SetFormat(Format.Rgba8888);
            _support = support;

        }


        protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.OnSizeChanged");
#endif
            base.OnSizeChanged(width, height, oldWidth, oldHeight);
            
            ViewSizeChanged?.Invoke();
        }

        /// <summary>
        /// "SurfaceHolder" SurfaceChanged Callback 
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.SurfaceChanged  (Android SurfaceHolder callback)");
#endif
            
            // the native window needs to be captured when the surface is first created. the surface must be created
            // on creation of the platformview otherwise maui blows up... that said this surface changed message
            // comes though and should be ingored if the surface hand
            if (holder.Surface.Handle != _lastSurfaceHandle)
            {

                if (_aNativeWindow != IntPtr.Zero)
                {
                    AndroidNativeMethods.ANativeWindow_release(_aNativeWindow);
                    _aNativeWindow = IntPtr.Zero;
                }

                ViewRemoved?.Invoke();
                _aNativeWindow = AndroidNativeMethods.ANativeWindow_fromSurface(JniEnvironment.EnvironmentPointer, Holder.Surface.Handle);

                ViewLoaded?.Invoke();

                _lastSurfaceHandle = holder.Surface.Handle;
            }

            AndroidSurfaceChanged?.Invoke();
        }

        /// <summary>
        /// "SurfaceHolder" SurfaceCreated Callback from Android OS
        /// </summary>
        /// 
        /// we have to nab the native window reference for future rendering with Vulkan
        /// <param name="holder"></param>
        public unsafe void SurfaceCreated(ISurfaceHolder holder)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.SurfaceCreated (Android SurfaceHolder callback)");
#endif
            if (_aNativeWindow!=IntPtr.Zero)
            {
                AndroidNativeMethods.ANativeWindow_release(_aNativeWindow);
            }

            _aNativeWindow = AndroidNativeMethods.ANativeWindow_fromSurface(JniEnvironment.EnvironmentPointer, holder.Surface.Handle);
            _lastSurfaceHandle = holder.Surface.Handle;


            AndroidSurfaceCreated?.Invoke(holder);
            ViewLoaded?.Invoke();


        }

       
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.SurfaceDestroyed (Android SurfaceHolder callback)");
#endif
            ViewRemoved?.Invoke();
            _lastSurfaceHandle = nint.Zero;
            if (_aNativeWindow != IntPtr.Zero)
            {
                AndroidNativeMethods.ANativeWindow_release(_aNativeWindow);
            }

            AndroidSurfaceDestoryed?.Invoke();
        }

        public void SurfaceRedrawNeeded(ISurfaceHolder holder)
        {
           // throw new NotImplementedException();

        }
    }


    internal static class AndroidNativeMethods
    {
        const string AndroidRuntimeLibrary = "android";//android.so?

        [DllImport(AndroidRuntimeLibrary)]
        internal static unsafe extern IntPtr ANativeWindow_fromSurface(IntPtr jniEnv, IntPtr handle);

        [DllImport(AndroidRuntimeLibrary)]
        internal static unsafe extern void ANativeWindow_release(IntPtr window);

        [DllImport(AndroidRuntimeLibrary)]
        internal static extern int ANativeWindow_getWidth(IntPtr window);

        [DllImport(AndroidRuntimeLibrary)]
        internal static extern int ANativeWindow_getHeight(IntPtr window);

        [DllImport(AndroidRuntimeLibrary)]
        internal static extern void ANativeWindow_unlockAndPost(IntPtr window);

        [DllImport(AndroidRuntimeLibrary)]
        public static extern int ANativeWindow_setBuffersGeometry(IntPtr aNativeWindow, int width, int height, AndroidPixelFormat format);


        [DllImport(AndroidRuntimeLibrary)]
        internal static extern int ANativeWindow_lock(IntPtr window, out ANativeWindow_Buffer outBuffer, ref ARect inOutDirtyBounds);

    }

    public enum AndroidPixelFormat
    {
        WINDOW_FORMAT_RGBA_8888 = 1,
        WINDOW_FORMAT_RGBX_8888 = 2,
        WINDOW_FORMAT_RGB_565 = 4,
    }
    internal struct ARect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    
    internal struct ANativeWindow_Buffer
    {
        // The number of pixels that are show horizontally.
        public int width;

        // The number of pixels that are shown vertically.
        public int height;

        // The number of *pixels* that a line in the buffer takes in
        // memory.  This may be >= width.
        public int stride;

        // The format of the buffer.  One of WINDOW_FORMAT_*
        public AndroidPixelFormat format;

        // The actual bits.
        public IntPtr bits;

        // Do not touch.
#pragma warning disable CA1823 // Avoid unused private fields
        uint reserved1;
        uint reserved2;
        uint reserved3;
        uint reserved4;
        uint reserved5;
        uint reserved6;
#pragma warning restore CA1823 // Avoid unused private fields
    }
}
