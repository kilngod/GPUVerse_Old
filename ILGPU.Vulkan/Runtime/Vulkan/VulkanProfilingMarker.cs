// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanProfilingMarker.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Host-side Vulkan profiling marker.
/// </summary>
internal sealed class VulkanProfilingMarker : ProfilingMarker
{
    private readonly long _timestamp;

    /// <summary>
    /// Creates a new Vulkan profiling marker.
    /// </summary>
    /// <param name="accelerator">The parent accelerator.</param>
    internal VulkanProfilingMarker(Accelerator accelerator)
        : base(accelerator)
    {
        _timestamp = Stopwatch.GetTimestamp();
    }

    /// <inheritdoc/>
    public override void Synchronize()
    {
        // Host-side marker: no native Vulkan resource to synchronize.
    }

    /// <inheritdoc/>
    public override TimeSpan MeasureFrom(ProfilingMarker marker)
    {
        if (marker is not VulkanProfilingMarker startMarker)
        {
            throw new ArgumentException(
                "The profiling marker must be a Vulkan profiling marker.",
                nameof(marker));
        }

        long ticks = _timestamp - startMarker._timestamp;
        return Stopwatch.GetElapsedTime(0, ticks);
    }

    /// <inheritdoc/>
    protected override void DisposeAcceleratorObject(bool disposing)
    {
        // No native resources to release.
    }
}
