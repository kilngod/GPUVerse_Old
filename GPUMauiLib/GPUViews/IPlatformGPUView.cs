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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUMauiLib.GPUViews
{
    public interface IPlatformGPUView
    {
        event Action ViewSizeChanged;
        event Action ViewLoaded;
        event Action ViewRemoved;

        void Invalidate();
    }
}
