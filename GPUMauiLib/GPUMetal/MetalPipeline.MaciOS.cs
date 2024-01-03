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
using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using GPUMauiLib.GPURenderers;
using VulkanPlatform;
#nullable disable
namespace GPUMauiLib.GPUMetal
{
	public static class MetalPipeline
	{
		
        
        public static bool SetupPipeline(this RendererMetal renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("MetalPipeline.SetupPipeline");
#endif


            GPUSupportMetal support = renderer.Support;

            // Load the vertex program into the library
            IMTLFunction vertexProgram = support.MetalLibrary.CreateFunction("triangle_vertex");

            // Load the fragment program into the library
            IMTLFunction fragmentProgram = support.MetalLibrary.CreateFunction("triangle_fragment");

            // Create a vertex descriptor from the MTKMesh       
            MTLVertexDescriptor vertexDescriptor = new MTLVertexDescriptor();
            vertexDescriptor.Attributes[0].Format = MTLVertexFormat.Float4;
            vertexDescriptor.Attributes[0].BufferIndex = 0;
            vertexDescriptor.Attributes[0].Offset = 0;
            vertexDescriptor.Attributes[1].Format = MTLVertexFormat.Float4;
            vertexDescriptor.Attributes[1].BufferIndex = 0;
            vertexDescriptor.Attributes[1].Offset = 4 * sizeof(float);

            vertexDescriptor.Layouts[0].Stride = 8 * sizeof(float);

            vertexDescriptor.Layouts[0].StepRate = 1;
            vertexDescriptor.Layouts[0].StepFunction = MTLVertexStepFunction.PerVertex;

            renderer.VertexBuffer = support.MetalDevice.CreateBuffer(renderer.VertexData, MTLResourceOptions.CpuCacheModeDefault);

            // Create a reusable pipeline state
            var pipelineStateDescriptor = new MTLRenderPipelineDescriptor
            {
                SampleCount = renderer.View.SampleCount,
                VertexFunction = vertexProgram,
                FragmentFunction = fragmentProgram,
                VertexDescriptor = vertexDescriptor,
                DepthAttachmentPixelFormat = renderer.View.DepthStencilPixelFormat,
                StencilAttachmentPixelFormat = renderer.View.DepthStencilPixelFormat
            };

            pipelineStateDescriptor.ColorAttachments[0].PixelFormat = renderer.View.ColorPixelFormat;

            NSError error;

            renderer.PipelineState = support.MetalDevice.CreateRenderPipelineState(pipelineStateDescriptor, out error);
            if ((support.Renderer as RendererMetal).PipelineState == null)
                Console.WriteLine("Failed to created pipeline state, error {0}", error);

            

            return true;

        }
        
    }
}

