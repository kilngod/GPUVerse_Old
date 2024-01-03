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

#nullable disable

namespace GPUMauiLib.GPUViews
{

    public class TouchEventArgs : EventArgs
    {
        public TouchEventArgs()
        {

        }

        public TouchEventArgs(PointF[] points, bool isInsideBounds)
        {
            Touches = points;
            IsInsideBounds = isInsideBounds;
        }

        /// <summary>
        /// This is only used for EndInteraction;
        /// </summary>
        public bool IsInsideBounds { get; private set; }

        public PointF[] Touches { get; private set; }
    }
}
