// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanStream.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a Vulkan queue-backed accelerator stream.
/// </summary>
[SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "Matches base class AcceleratorStream naming convention.")]
public sealed class VulkanStream : AcceleratorStream
{
    /// <summary>
    /// Creates a new Vulkan stream backed by an existing queue.
    /// </summary>
    /// <param name="accelerator">The parent Vulkan accelerator.</param>
    /// <param name="queue">The Vulkan queue handle.</param>
    /// <param name="queueFamilyIndex">The queue family index.</param>
    /// <param name="flags">The stream flags.</param>
    internal VulkanStream(
        VulkanAccelerator accelerator,
        VkQueue queue,
        uint queueFamilyIndex,
        AcceleratorStreamFlags flags)
        : base(accelerator, flags)
    {
        if (queue == VkQueue.Null)
            throw new ArgumentException("Invalid Vulkan queue.", nameof(queue));

        Queue = queue;
        QueueFamilyIndex = queueFamilyIndex;
        CommandPool = VulkanAPI.CreateCommandPool(
            accelerator.LogicalDevice,
            QueueFamilyIndex);
    }

    /// <summary>
    /// Returns the parent Vulkan accelerator.
    /// </summary>
    public new VulkanAccelerator Accelerator =>
        (VulkanAccelerator)base.Accelerator!;

    /// <summary>
    /// Returns the Vulkan queue handle.
    /// </summary>
    public VkQueue Queue { get; private set; }

    /// <summary>
    /// Returns the Vulkan queue family index.
    /// </summary>
    public uint QueueFamilyIndex { get; }

    /// <summary>
    /// Returns the Vulkan command pool owned by this stream.
    /// </summary>
    public VkCommandPool CommandPool { get; private set; }

    /// <summary>
    /// Allocates a primary command buffer from this stream's command pool.
    /// </summary>
    /// <returns>The allocated command buffer.</returns>
    public VkCommandBuffer AllocateCommandBuffer() =>
        VulkanAPI.AllocateCommandBuffer(Accelerator.LogicalDevice, CommandPool);

    /// <summary>
    /// Allocates primary command buffers from this stream's command pool.
    /// </summary>
    /// <param name="commandBufferCount">The number of command buffers.</param>
    /// <returns>The allocated command buffers.</returns>
    public VkCommandBuffer[] AllocateCommandBuffers(uint commandBufferCount) =>
        VulkanAPI.AllocateCommandBuffers(
            Accelerator.LogicalDevice,
            CommandPool,
            commandBufferCount);

    /// <summary>
    /// Frees a primary command buffer allocated from this stream's command pool.
    /// </summary>
    /// <param name="commandBuffer">The command buffer to free.</param>
    public void FreeCommandBuffer(VkCommandBuffer commandBuffer) =>
        VulkanAPI.FreeCommandBuffer(
            Accelerator.LogicalDevice,
            CommandPool,
            commandBuffer);

    /// <inheritdoc/>
    public override void Synchronize()
    {
        if (Queue != VkQueue.Null)
            VulkanAPI.QueueWaitIdle(Queue);
    }

    /// <inheritdoc/>
    protected override ProfilingMarker AddProfilingMarkerInternal() =>
        new VulkanProfilingMarker(Accelerator);

    /// <inheritdoc/>
    protected override void DisposeAcceleratorObject(bool disposing)
    {
        Synchronize();

        if (CommandPool != VkCommandPool.Null)
        {
            VulkanAPI.DestroyCommandPool(Accelerator.LogicalDevice, CommandPool);
            CommandPool = VkCommandPool.Null;
        }

        Queue = VkQueue.Null;
    }
}
