// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanException.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a Vulkan runtime error.
/// </summary>
public sealed class VulkanException : Exception
{
    /// <summary>
    /// Constructs a new Vulkan exception.
    /// </summary>
    /// <param name="error">The Vulkan result code.</param>
    public VulkanException(VkResult error)
        : this(error, $"Vulkan call failed with result '{error}'.")
    { }

    /// <summary>
    /// Constructs a new Vulkan exception.
    /// </summary>
    /// <param name="error">The Vulkan result code.</param>
    /// <param name="message">The exception message.</param>
    public VulkanException(VkResult error, string message)
        : base(message)
    {
        Error = error;
    }

    /// <summary>
    /// Constructs a new Vulkan exception.
    /// </summary>
    /// <param name="error">The Vulkan result code.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public VulkanException(
        VkResult error,
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        Error = error;
    }

    /// <summary>
    /// Returns the Vulkan result code.
    /// </summary>
    public VkResult Error { get; }
}
