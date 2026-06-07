// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanMemoryBuffer.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Runtime.CompilerServices;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a Vulkan memory buffer backed by a <see cref="VkBuffer"/> and
/// host-visible <see cref="VkDeviceMemory"/>.
/// </summary>
public sealed class VulkanMemoryBuffer : MemoryBuffer
{
    /// <summary>
    /// Creates a new Vulkan memory buffer.
    /// </summary>
    /// <param name="accelerator">The parent Vulkan accelerator.</param>
    /// <param name="length">The number of elements.</param>
    /// <param name="elementSize">The size of each element in bytes.</param>
    internal VulkanMemoryBuffer(
        VulkanAccelerator accelerator,
        long length,
        int elementSize)
        : base(accelerator, length, elementSize)
    {
        if (LengthInBytes == 0)
        {
            Buffer = VkBuffer.Null;
            DeviceMemory = VkDeviceMemory.Null;
            NativePtr = IntPtr.Zero;
            return;
        }

        Buffer = VulkanAPI.CreateBuffer(
            accelerator.LogicalDevice,
            checked((ulong)LengthInBytes),
            VkBufferUsageFlags.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
            VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_SRC_BIT |
            VkBufferUsageFlags.VK_BUFFER_USAGE_TRANSFER_DST_BIT);

        var memoryRequirements = VulkanAPI.GetBufferMemoryRequirements(
            accelerator.LogicalDevice,
            Buffer);
        var memoryProperties = VulkanAPI.GetPhysicalDeviceMemoryProperties(
            accelerator.PhysicalDevice);
        var memoryTypeIndex = VulkanAPI.FindMemoryType(
            memoryProperties,
            memoryRequirements.memoryTypeBits,
            VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
            VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);

        DeviceMemory = VulkanAPI.AllocateMemory(
            accelerator.LogicalDevice,
            memoryRequirements.size,
            memoryTypeIndex);
        VulkanAPI.BindBufferMemory(
            accelerator.LogicalDevice,
            Buffer,
            DeviceMemory,
            0);

        NativePtr = VulkanAPI.MapMemory(
            accelerator.LogicalDevice,
            DeviceMemory,
            0,
            checked((ulong)LengthInBytes));
    }

    /// <summary>
    /// Returns the Vulkan buffer handle.
    /// </summary>
    public VkBuffer Buffer { get; private set; }

    /// <summary>
    /// Returns the Vulkan device memory handle.
    /// </summary>
    public VkDeviceMemory DeviceMemory { get; private set; }

    /// <summary>
    /// Returns the parent Vulkan accelerator.
    /// </summary>
    public new VulkanAccelerator Accelerator =>
        (VulkanAccelerator)base.Accelerator!;

    /// <inheritdoc/>
    protected override unsafe void MemSet(
        AcceleratorStream stream,
        byte value,
        in ArrayView<byte> targetView)
    {
        if (targetView.Length == 0)
            return;

        stream.Synchronize();

        var target = targetView.LoadEffectiveAddressAsPtr();
        InitBlock(target, value, targetView.Length);
    }

    /// <inheritdoc/>
    protected override unsafe void CopyTo(
        AcceleratorStream stream,
        in ArrayView<byte> sourceView,
        in ArrayView<byte> targetView)
    {
        if (sourceView.Length == 0)
            return;

        stream.Synchronize();

        var source = sourceView.LoadEffectiveAddressAsPtr();
        var target = targetView.LoadEffectiveAddressAsPtr();
        System.Buffer.MemoryCopy(
            source.ToPointer(),
            target.ToPointer(),
            targetView.Length,
            sourceView.Length);
    }

    /// <inheritdoc/>
    protected override unsafe void CopyFrom(
        AcceleratorStream stream,
        in ArrayView<byte> sourceView,
        in ArrayView<byte> targetView)
    {
        if (sourceView.Length == 0)
            return;

        stream.Synchronize();

        var source = sourceView.LoadEffectiveAddressAsPtr();
        var target = targetView.LoadEffectiveAddressAsPtr();
        System.Buffer.MemoryCopy(
            source.ToPointer(),
            target.ToPointer(),
            targetView.Length,
            sourceView.Length);
    }

    /// <inheritdoc/>
    protected override void DisposeAcceleratorObject(bool disposing)
    {
        var device = Accelerator.LogicalDevice;

        if (DeviceMemory != VkDeviceMemory.Null)
        {
            if (NativePtr != IntPtr.Zero)
            {
                VulkanAPI.UnmapMemory(device, DeviceMemory);
                NativePtr = IntPtr.Zero;
            }

            VulkanAPI.FreeMemory(device, DeviceMemory);
            DeviceMemory = VkDeviceMemory.Null;
        }

        if (Buffer != VkBuffer.Null)
        {
            VulkanAPI.DestroyBuffer(device, Buffer);
            Buffer = VkBuffer.Null;
        }
    }

    private static unsafe void InitBlock(IntPtr target, byte value, long length)
    {
        byte* current = (byte*)target.ToPointer();
        var remaining = length;
        while (remaining > 0)
        {
            var chunk = remaining > uint.MaxValue
                ? uint.MaxValue
                : (uint)remaining;
            Unsafe.InitBlock(current, value, chunk);
            current += chunk;
            remaining -= chunk;
        }
    }
}
