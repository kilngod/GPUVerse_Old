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
using Metal;
using VulkanPlatform;

#nullable disable

namespace GPUMauiLib.GPUMetal
{
	public static class MetalShaderLibrary
	{

        public static void AddDefaultLibrary(this GPUSupportMetal support)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalShaderLibrary.AddDefaultLibrary");
#endif

            support.MetalDevice.CreateDefaultLibrary();
            
        }

        public static async Task<bool> AddLibraryKernels(this GPUSupportMetal support, string name, string kernelsString)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalShaderLibrary.AddLibraryKernels");
#endif

            // add async

            MTLCompileOptions mTLCompileOptions = new MTLCompileOptions()
            {
                LibraryType = MTLLibraryType.Executable
            };


            support.MetalLibrary = await support.MetalDevice.CreateLibraryAsync(kernelsString, mTLCompileOptions);

            return support.MetalLibrary != null;


        }


    }
}

