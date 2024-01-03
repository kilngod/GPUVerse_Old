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

namespace GPUMauiLib.GPURenderers
{
    public static class HLSLTriangleShaderSource
    {
        public const string TriangleShaders = @"
struct VSInput {
    float4 Position : POSITION;
    float4 Color : COLOR;
};

struct PSInput {
    float4 Position : SV_POSITION;
    float4 Color : COLOR;
};

PSInput VSMain(VSInput input) {
    PSInput result;
    result.Position = input.Position;
    result.Color = input.Color;
    return result;
}

float4 PSMain(PSInput input) : SV_TARGET{
    return input.Color;
}
";
    }
}
