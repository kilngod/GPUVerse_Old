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
using System.Diagnostics;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Composition;
#else

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
#endif

namespace GPUMauiLib.GPURenderers
{
    public partial class GPUAnimator
    {
         public void start()
        {
            isRunning = true;
            CompositionTarget.Rendering += RenderLoop;
            
        }


        private void RenderLoop(object sender, object e)
        {
            update();
        }


        public void cancel()
        {
            isRunning = false;
            CompositionTarget.Rendering -= RenderLoop;
        }
    }
}

