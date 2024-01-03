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
using GPUMauiLib.GPUDevices;
using GPUMauiLib.GPURenderers;
using GPUMauiLib.GPUViews;
using Microsoft.Maui.Handlers;

namespace GPUMauiLib.GPUHandlers
{
    public partial class PlatformViewHandler :
#if IOS || MACCATALYST || WINDOWS || ANDROID
    IGPUViewHandler
#else
    ViewHandler<IGPUView, PlatformGPUView>, IGPUViewHandler
#endif
    {
#nullable disable
        private IGPUSupport _gpuSupport;
#nullable enable


        public static IPropertyMapper<IGPUView, IGPUViewHandler> Mapper = new PropertyMapper<IGPUView, PlatformViewHandler>(ViewHandler.ViewMapper)
        {


        };

        public static CommandMapper<IGPUView, IGPUViewHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(IPlatformGPUView.Invalidate)] = MapInvalidate
        };

        public PlatformViewHandler() : base(Mapper, CommandMapper)
        {
        }



#if IOS || MACCATALYST || WINDOWS || ANDROID
        IGPUView IGPUViewHandler.VirtualView => VirtualView;


        PlatformGPUView IGPUViewHandler.PlatformView => PlatformView;


#else

        IGPUView IGPUViewHandler.VirtualView => VirtualView;

        PlatformGPUView IGPUViewHandler.PlatformView => (PlatformGPUView)PlatformView;



        protected override PlatformGPUView CreatePlatformView()
        {
            throw new NotImplementedException();
        }

        public static void MapInvalidate(IGPUViewHandler handler, IGPUView view, object? arg) { }
       
#endif
    }
}

