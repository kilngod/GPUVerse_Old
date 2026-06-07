// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanDevice.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a Vulkan physical device descriptor.
/// </summary>
public sealed class VulkanDevice : Device, IDeviceAcceleratorTypeInfo
{
    /// <summary>
    /// Creates a new Vulkan device descriptor.
    /// </summary>
    /// <param name="instance">The parent Vulkan instance.</param>
    /// <param name="physicalDevice">The Vulkan physical device.</param>
    internal VulkanDevice(VkInstance instance, VkPhysicalDevice physicalDevice)
        : base(AcceleratorType.None)
    {
        if (instance == VkInstance.Null)
            throw new ArgumentException("Invalid Vulkan instance.", nameof(instance));
        if (physicalDevice == VkPhysicalDevice.Null)
        {
            throw new ArgumentException(
                "Invalid Vulkan physical device.",
                nameof(physicalDevice));
        }

        Instance = instance;
        PhysicalDevice = physicalDevice;

        InitDeviceInfo();
        InitCapabilities();
    }

    /// <summary>
    /// Returns the parent Vulkan instance.
    /// </summary>
    public VkInstance Instance { get; }

    /// <summary>
    /// Returns the Vulkan physical device handle.
    /// </summary>
    public VkPhysicalDevice PhysicalDevice { get; }

    /// <summary>
    /// Returns the Vulkan physical device type.
    /// </summary>
    public VkPhysicalDeviceType DeviceType { get; private set; }

    /// <summary>
    /// Returns the Vulkan API version exposed by this device.
    /// </summary>
    public uint ApiVersion { get; private set; }

    /// <summary>
    /// Returns the Vulkan driver version exposed by this device.
    /// </summary>
    public uint DriverVersion { get; private set; }

    /// <summary>
    /// Returns the Vulkan vendor id.
    /// </summary>
    public uint VendorId { get; private set; }

    /// <summary>
    /// Returns the Vulkan device id.
    /// </summary>
    public uint DeviceId { get; private set; }

    /// <summary>
    /// Returns the queue family used for compute work.
    /// </summary>
    public uint ComputeQueueFamilyIndex { get; private set; }

    /// <summary>
    /// Returns the maximum push-constant payload size in bytes.
    /// </summary>
    public uint MaxPushConstantSize { get; private set; }

    /// <summary>
    /// Returns the accelerator type for this device type.
    /// </summary>
    static AcceleratorType IDeviceAcceleratorTypeInfo.AcceleratorType =>
        AcceleratorType.None;

    /// <summary>
    /// Creates a new Vulkan accelerator for this device.
    /// </summary>
    public override Accelerator CreateAccelerator(Context context) =>
        CreateVulkanAccelerator(context);

    /// <summary>
    /// Creates a new Vulkan accelerator for this device.
    /// </summary>
    public VulkanAccelerator CreateVulkanAccelerator(Context context)
    {
        var logicalDevice = CreateLogicalDevice(out var computeQueue);
        return new VulkanAccelerator(
            context,
            this,
            Instance,
            PhysicalDevice,
            logicalDevice,
            computeQueue,
            ComputeQueueFamilyIndex,
            ownsLogicalDevice: true);
    }

    /// <summary>
    /// Enumerates all available Vulkan devices for an existing instance.
    /// </summary>
    /// <param name="instance">The Vulkan instance to enumerate.</param>
    /// <returns>An immutable array of Vulkan devices.</returns>
    public static ImmutableArray<Device> GetDevices(VkInstance instance)
    {
        var registry = new DeviceRegistry();
        GetDevices(instance, _ => true, registry);
        return registry.ToImmutable();
    }

    /// <summary>
    /// Enumerates Vulkan devices matching the given predicate.
    /// </summary>
    /// <param name="instance">The Vulkan instance to enumerate.</param>
    /// <param name="predicate">The predicate to include a given device.</param>
    /// <returns>An immutable array of Vulkan devices.</returns>
    public static ImmutableArray<Device> GetDevices(
        VkInstance instance,
        Predicate<VulkanDevice> predicate)
    {
        var registry = new DeviceRegistry();
        GetDevices(instance, predicate, registry);
        return registry.ToImmutable();
    }

    /// <summary>
    /// Detects Vulkan devices matching the given predicate and registers them.
    /// </summary>
    /// <param name="instance">The Vulkan instance to enumerate.</param>
    /// <param name="predicate">The predicate to include a given device.</param>
    /// <param name="registry">The registry to add all devices to.</param>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "We want to hide all exceptions at this level")]
    internal static void GetDevices(
        VkInstance instance,
        Predicate<VulkanDevice> predicate,
        DeviceRegistry registry)
    {
        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));
        if (registry is null)
            throw new ArgumentNullException(nameof(registry));

        try
        {
            GetDevicesInternal(instance, predicate, registry);
        }
        catch (Exception)
        {
            // Ignore API-specific exceptions at this point.
        }
    }

    private static void GetDevicesInternal(
        VkInstance instance,
        Predicate<VulkanDevice> predicate,
        DeviceRegistry registry)
    {
        if (instance == VkInstance.Null)
            return;

        foreach (var physicalDevice in VulkanAPI.EnumeratePhysicalDevices(instance))
        {
            var computeQueueFamilyIndex = VulkanAPI.FindQueueFamilyIndex(
                physicalDevice,
                VkQueueFlags.VK_QUEUE_COMPUTE_BIT);
            if (computeQueueFamilyIndex == uint.MaxValue)
                continue;

            var device = new VulkanDevice(instance, physicalDevice);
            if (predicate(device))
                registry.Register(device);
        }
    }

    private unsafe void InitDeviceInfo()
    {
        var properties = VulkanAPI.GetPhysicalDeviceProperties(PhysicalDevice);

        Name = VulkanAPI.GetString(properties.deviceName);
        DeviceType = properties.deviceType;
        ApiVersion = properties.apiVersion;
        DriverVersion = properties.driverVersion;
        VendorId = properties.vendorID;
        DeviceId = properties.deviceID;

        ComputeQueueFamilyIndex = VulkanAPI.FindQueueFamilyIndex(
            PhysicalDevice,
            VkQueueFlags.VK_QUEUE_COMPUTE_BIT);
        if (ComputeQueueFamilyIndex == uint.MaxValue)
            throw new NotSupportedException("Vulkan compute queue not supported.");

        var limits = properties.limits;
        WarpSize = 32;
        MaxNumThreadsPerGroup = (int)Math.Min(
            limits.maxComputeWorkGroupInvocations,
            int.MaxValue);
        MaxSharedMemoryPerGroup = (int)Math.Min(
            limits.maxComputeSharedMemorySize,
            int.MaxValue);
        MaxConstantMemory = (int)Math.Min(
            limits.maxUniformBufferRange,
            int.MaxValue);
        MaxPushConstantSize = limits.maxPushConstantsSize;

        NumMultiprocessors = 1;
        MaxNumThreadsPerMultiprocessor = MaxNumThreadsPerGroup;
        MemorySize = GetDeviceLocalMemorySize();

        OptimalKernelSize = new KernelSize(
            Math.Max(1, limits.maxComputeWorkGroupCount_0),
            Math.Max(WarpSize, MaxNumThreadsPerGroup / 2));
    }

    /// <summary>
    /// Returns the queried Vulkan accelerator capabilities.
    /// </summary>
    public new VulkanAcceleratorCapabilities Capabilities =>
        (VulkanAcceleratorCapabilities)base.Capabilities;

    private void InitCapabilities() =>
        base.Capabilities = VulkanAcceleratorCapabilities.Query(
            PhysicalDevice,
            ApiVersion);

    private unsafe VkDevice CreateLogicalDevice(out VkQueue computeQueue)
    {
        var deviceFeatures = Capabilities.ToPhysicalDeviceFeatures();
        var logicalDevice = VulkanAPI.CreateDevice(
            PhysicalDevice,
            ComputeQueueFamilyIndex,
            deviceFeatures);
        computeQueue = VulkanAPI.GetDeviceQueue(
            logicalDevice,
            ComputeQueueFamilyIndex,
            0);
        return logicalDevice;
    }

    private unsafe long GetDeviceLocalMemorySize()
    {
        var memoryProperties =
            VulkanAPI.GetPhysicalDeviceMemoryProperties(PhysicalDevice);

        ulong total = 0;
        for (uint i = 0; i < memoryProperties.memoryHeapCount; ++i)
        {
            var heap = (&memoryProperties.memoryHeaps_0)[i];
            if ((heap.flags & VkMemoryHeapFlags.VK_MEMORY_HEAP_DEVICE_LOCAL_BIT) != 0)
                total += heap.size;
        }

        return total > long.MaxValue ? long.MaxValue : (long)total;
    }

    /// <inheritdoc/>
    protected override void PrintHeader(System.IO.TextWriter writer)
    {
        base.PrintHeader(writer);
        writer.Write("Vulkan Device: ");
        writer.WriteLine(Name);
    }

    /// <inheritdoc/>
    protected override void PrintGeneralInfo(System.IO.TextWriter writer)
    {
        base.PrintGeneralInfo(writer);
        writer.Write("  Vulkan Device Type: ");
        writer.WriteLine(DeviceType);
        writer.Write("  Vulkan API Version: ");
        writer.WriteLine(ApiVersion);
        writer.Write("  Compute Queue Family: ");
        writer.WriteLine(ComputeQueueFamilyIndex);
    }
}
