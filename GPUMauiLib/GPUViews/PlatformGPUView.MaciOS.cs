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
using CoreGraphics;
using Metal;
using MetalKit;
using UIKit;
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUHandlers;

using System.Runtime.CompilerServices;
using SpriteKit;
using Microsoft.Maui.Graphics.Platform;
using GPUMauiLib.GPUDevices;
using VulkanPlatform;

#nullable disable

namespace GPUMauiLib.GPUViews
{
    public partial class PlatformGPUView : MTKView
    {

        bool _firstLoad = true;
        CGSize _lastFrame = CGSize.Empty;

        private CGRect _lastBounds;
        // Add properties for IMTLDevice, IMTLCommandQueue, and pipeline state
        // renderer

        // MacOS and IOs are often shown with a scaling factor, we draw to actual
        // pixels which is the DrawableSize 
        public double Width { get { return this.DrawableSize.Width; } }
        public double Height { get { return this.DrawableSize.Height; } }

        public PlatformGPUView(CGRect frame, GPUSupportMetal support) : base(frame, (support as GPUSupportMetal).MetalDevice)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView");
#endif
            _support = support as GPUSupportMetal;
            _lastBounds = frame;

         //   BackgroundColor = UIColor.Black;

        }

        public PlatformGPUView(CGRect frame, GPUSupportVulkan support) : base(frame, (support as GPUSupportVulkan).MetalDevice)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.PlatformGPUView");
#endif
            _support = support as GPUSupportVulkan;
            _lastBounds = frame;

            //BackgroundColor = UIColor.Black;

        }


        public override CGSize SizeThatFits(CGSize size)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.SizeThatFits");
#endif
            var result = base.SizeThatFits(size);
           

            if(_firstLoad && result.Width> 0 && result.Height>0)
            {
                ViewLoaded?.Invoke();
                _firstLoad = false;
                _lastFrame = result;
                Invalidate();
            }
            else if (result != _lastFrame)
            {
                ViewSizeChanged?.Invoke();
                _lastFrame = result;
                Invalidate();

            }

            return result;
        }

        public override void MovedToWindow()
        {
#if DEBUG
            VulkanFlowTracer.AddItem("PlatformGPUView.MovedToWindow");
#endif
            base.MovedToWindow();

            if (!_firstLoad)
            {
                ViewRemoved?.Invoke();
                _firstLoad = true;
            }
        }

        public override CGRect Bounds
        {
            get
            {
#if DEBUG
                VulkanFlowTracer.AddItem($"PlatformGPUView.Bounds Get ({base.Bounds.Width}, {base.Bounds.Height})");
#endif
                return base.Bounds;
            }

            set
            {
#if DEBUG
                VulkanFlowTracer.AddItem("PlatformGPUView.Bounds Set");
#endif
                var newBounds = value;
                if (_lastBounds.Width != newBounds.Width || _lastBounds.Height != newBounds.Height)
                {

#if DEBUG
                    VulkanFlowTracer.AddItem($"PlatformGPUView.Bounds Changed ({newBounds.Width}, {newBounds.Height})");
#endif

                    base.Bounds = value;
                    _lastBounds = newBounds;
                    Invalidate();
                }
            }
        }

        public void Invalidate()
        {
            SetNeedsDisplay();
        }
    }
}

