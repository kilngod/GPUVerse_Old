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

using System;
using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using GPUMauiLib.GPURenderers;

using GPUMauiLib.GPUViews;
using VulkanPlatform;
#nullable disable
namespace GPUMauiLib.GPUMetal
{
	public static class MetalDraw
	{
        static object _RenderLock = new object();

        public static void Draw(this RendererMetal renderer)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("RendererMetal.Draw");
#endif
          

            if (renderer.Active)
            {
                lock (_RenderLock)
                { 
                    PlatformGPUView view = renderer.View;
                
                    // Create a new command buffer for each renderpass to the current drawable
                    var commandBuffer = renderer.CommandQueue.CommandBuffer();

                    // Call the view's completion handler which is required by the view since it will signal its semaphore and set up the next buffer
                    var drawable = view.CurrentDrawable;

                    // Obtain a renderPassDescriptor generated from the view's drawable textures
                    MTLRenderPassDescriptor renderPassDescriptor = view.CurrentRenderPassDescriptor;

                    // If we have a valid drawable, begin the commands to render into it
                    if (renderPassDescriptor != null)
                    {
                        // Create a render command encoder so we can render into something
                        IMTLRenderCommandEncoder renderEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);

                        // Set context state
                        renderEncoder.SetDepthStencilState(renderer.DepthStencilState);
                        renderEncoder.SetRenderPipelineState(renderer.PipelineState);
                        renderEncoder.SetVertexBuffer(renderer.VertexBuffer, 0, 0);

                        // Tell the render context we want to draw our primitives
                        renderEncoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, 3);

                        // We're done encoding commands
                        renderEncoder.EndEncoding();

                        // Schedule a present once the framebuffer is complete using the current drawable
                        commandBuffer.PresentDrawable(drawable);
                    }

                    // Finalize rendering here & push the command buffer to the GPU
                    commandBuffer.Commit();
                }
            }
        }
		
	}
}

