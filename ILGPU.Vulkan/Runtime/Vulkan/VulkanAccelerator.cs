// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanAccelerator.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a Vulkan compute accelerator backed by a logical Vulkan device.
/// </summary>
public sealed class VulkanAccelerator : Accelerator
{
    /// <summary>
    /// Creates a new Vulkan accelerator from existing Vulkan handles.
    /// </summary>
    /// <param name="context">The ILGPU context.</param>
    /// <param name="device">The ILGPU device descriptor.</param>
    /// <param name="instance">The parent Vulkan instance.</param>
    /// <param name="physicalDevice">The selected Vulkan physical device.</param>
    /// <param name="logicalDevice">The Vulkan logical device.</param>
    /// <param name="computeQueue">The Vulkan compute queue.</param>
    /// <param name="computeQueueFamilyIndex">The compute queue family index.</param>
    /// <param name="ownsLogicalDevice">
    /// Whether this accelerator is responsible for destroying
    /// <paramref name="logicalDevice"/>.
    /// </param>
    internal VulkanAccelerator(
        Context context,
        VulkanDevice device,
        VkInstance instance,
        VkPhysicalDevice physicalDevice,
        VkDevice logicalDevice,
        VkQueue computeQueue,
        uint computeQueueFamilyIndex,
        bool ownsLogicalDevice)
        : base(context, device)
    {
        if (logicalDevice == VkDevice.Null)
            throw new ArgumentException("Invalid Vulkan device.", nameof(logicalDevice));
        if (computeQueue == VkQueue.Null)
            throw new ArgumentException("Invalid Vulkan queue.", nameof(computeQueue));

        Instance = instance;
        PhysicalDevice = physicalDevice;
        LogicalDevice = logicalDevice;
        ComputeQueue = computeQueue;
        ComputeQueueFamilyIndex = computeQueueFamilyIndex;
        OwnsLogicalDevice = ownsLogicalDevice;
        NativePtr = logicalDevice.Handle;

        Bind();
        DefaultStream = new VulkanStream(
            this,
            ComputeQueue,
            ComputeQueueFamilyIndex,
            AcceleratorStreamFlags.None);
        OnAcceleratorCreated();
    }

    /// <summary>
    /// Returns the parent Vulkan instance.
    /// </summary>
    public VkInstance Instance { get; private set; }

    /// <summary>
    /// Returns the selected Vulkan physical device.
    /// </summary>
    public VkPhysicalDevice PhysicalDevice { get; private set; }

    /// <summary>
    /// Returns the Vulkan logical device.
    /// </summary>
    public VkDevice LogicalDevice { get; private set; }

    /// <summary>
    /// Returns the default Vulkan compute queue.
    /// </summary>
    public VkQueue ComputeQueue { get; private set; }

    /// <summary>
    /// Returns the Vulkan queue family used for compute work.
    /// </summary>
    public uint ComputeQueueFamilyIndex { get; }

    /// <summary>
    /// Returns whether this accelerator owns the logical device handle.
    /// </summary>
    public bool OwnsLogicalDevice { get; }

    /// <summary>
    /// Returns the Vulkan device descriptor.
    /// </summary>
    public new VulkanDevice Device => (VulkanDevice)base.Device;

    /// <inheritdoc/>
    protected override AcceleratorStream CreateStreamInternal(
        AcceleratorStreamFlags flags = AcceleratorStreamFlags.Async) =>
        new VulkanStream(this, ComputeQueue, ComputeQueueFamilyIndex, flags);

    /// <inheritdoc/>
    protected override void SynchronizeInternal()
    {
        if (LogicalDevice != VkDevice.Null)
            VulkanAPI.DeviceWaitIdle(LogicalDevice);
    }

    /// <inheritdoc/>
    protected override MemoryBuffer AllocateRawInternal(
        long length,
        int elementSize) =>
        new VulkanMemoryBuffer(this, length, elementSize);

    /// <inheritdoc/>
    protected override Kernel LoadKernel(CompiledKernel compiledKernel) =>
        new VulkanKernel(this, (VulkanCompiledKernel)compiledKernel);

    /// <inheritdoc/>
    protected override void OnBind()
    {
        // Vulkan has no thread-local context model. Logical devices and queues
        // are passed explicitly to backend operations.
    }

    /// <inheritdoc/>
    protected override void OnUnbind()
    {
        // Vulkan has no thread-local context model.
    }

    /// <inheritdoc/>
    protected override int
        EstimateMaxActiveGroupsPerMultiprocessorInternal(
            Kernel kernel,
            int groupSize,
            int dynamicSharedMemorySizeInBytes)
    {
        if (groupSize <= 0)
            return 1;
        return Math.Max(1, MaxNumThreadsPerGroup / groupSize);
    }

    /// <inheritdoc/>
    protected override int EstimateGroupSizeInternal(
        Kernel kernel,
        Func<int, int> computeSharedMemorySize,
        int maxGroupSize,
        out int minGridSize)
    {
        minGridSize = NumMultiprocessors;
        return GetDefaultGroupSize(maxGroupSize);
    }

    /// <inheritdoc/>
    protected override int EstimateGroupSizeInternal(
        Kernel kernel,
        int dynamicSharedMemorySizeInBytes,
        int maxGroupSize,
        out int minGridSize)
    {
        minGridSize = NumMultiprocessors;
        return GetDefaultGroupSize(maxGroupSize);
    }

    /// <inheritdoc/>
    protected override bool CanAccessPeerInternal(Accelerator otherAccelerator) =>
        false;

    /// <inheritdoc/>
    protected override void EnablePeerAccessInternal(Accelerator otherAccelerator) =>
        throw new NotSupportedException("Vulkan peer access is not supported yet.");

    /// <inheritdoc/>
    protected override void DisablePeerAccessInternal(Accelerator otherAccelerator) =>
        throw new NotSupportedException("Vulkan peer access is not supported yet.");

    /// <inheritdoc/>
    protected override PageLockScope<T> CreatePageLockFromPinnedInternal<T>(
        IntPtr pinned,
        long numElements) =>
        throw new NotSupportedException(
            "Vulkan page-locked host memory is not supported yet.");

    /// <inheritdoc/>
    protected override void DisposeAccelerator_Locked(bool disposing)
    {
        if (LogicalDevice != VkDevice.Null)
        {
            VulkanAPI.DeviceWaitIdle(LogicalDevice);

            if (OwnsLogicalDevice)
                VulkanAPI.DestroyDevice(LogicalDevice);

            LogicalDevice = VkDevice.Null;
        }

        ComputeQueue = VkQueue.Null;
        PhysicalDevice = VkPhysicalDevice.Null;
        Instance = VkInstance.Null;
    }

    /// <summary>
    /// Computes a conservative group size for early Vulkan runtime kernels.
    /// </summary>
    private int GetDefaultGroupSize(int maxGroupSize)
    {
        int limit = MaxNumThreadsPerGroup;
        if (maxGroupSize > 0)
            limit = Math.Min(limit, maxGroupSize);

        int warpSize = Math.Max(1, WarpSize);
        return Math.Max(warpSize, (limit / warpSize) * warpSize);
    }
}
