// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanContextExtensions.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Reflection;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Vulkan specific context extensions.
/// </summary>
public static class VulkanContextExtensions
{
    #region Builder

    /// <summary>
    /// Enables all Vulkan devices for an existing Vulkan instance.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="instance">The Vulkan instance to enumerate.</param>
    /// <returns>The updated builder instance.</returns>
    public static Context.Builder Vulkan(
        this Context.Builder builder,
        VkInstance instance) =>
        builder.Vulkan(instance, _ => true);

    /// <summary>
    /// Enables all Vulkan devices matching the given predicate for an existing
    /// Vulkan instance.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="instance">The Vulkan instance to enumerate.</param>
    /// <param name="predicate">
    /// The predicate to include a given device.
    /// </param>
    /// <returns>The updated builder instance.</returns>
    public static Context.Builder Vulkan(
        this Context.Builder builder,
        VkInstance instance,
        Predicate<VulkanDevice> predicate)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));

        VulkanDevice.GetDevices(instance, predicate, GetDeviceRegistry(builder));
        return builder;
    }

    #endregion

    #region Context

    /// <summary>
    /// Gets the i-th registered Vulkan device.
    /// </summary>
    /// <param name="context">The ILGPU context.</param>
    /// <param name="vulkanDeviceIndex">
    /// The relative device index for the Vulkan device. 0 here refers to the
    /// first Vulkan device, 1 to the second, etc.
    /// </param>
    /// <returns>The registered Vulkan device.</returns>
    public static VulkanDevice GetVulkanDevice(
        this Context context,
        int vulkanDeviceIndex) =>
        context.GetDevice<VulkanDevice>(vulkanDeviceIndex);

    /// <summary>
    /// Gets all registered Vulkan devices.
    /// </summary>
    /// <param name="context">The ILGPU context.</param>
    /// <returns>All registered Vulkan devices.</returns>
    public static Context.DeviceCollection<VulkanDevice> GetVulkanDevices(
        this Context context) =>
        context.GetDevices<VulkanDevice>();

    /// <summary>
    /// Creates a new Vulkan accelerator.
    /// </summary>
    /// <param name="context">The ILGPU context.</param>
    /// <param name="vulkanDeviceIndex">
    /// The relative device index for the Vulkan device. 0 here refers to the
    /// first Vulkan device, 1 to the second, etc.
    /// </param>
    /// <returns>The created Vulkan accelerator.</returns>
    public static VulkanAccelerator CreateVulkanAccelerator(
        this Context context,
        int vulkanDeviceIndex) =>
        context.GetVulkanDevice(vulkanDeviceIndex)
            .CreateVulkanAccelerator(context);

    #endregion

    #region Internals

    private static DeviceRegistry GetDeviceRegistry(Context.Builder builder)
    {
        var property = typeof(Context.Builder).GetProperty(
            "DeviceRegistry",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (property?.GetValue(builder) is DeviceRegistry registry)
            return registry;

        throw new InvalidOperationException(
            "Could not access the ILGPU context builder device registry.");
    }

    #endregion
}
