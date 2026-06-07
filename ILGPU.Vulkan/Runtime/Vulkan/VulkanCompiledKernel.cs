// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanCompiledKernel.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a compiled Vulkan kernel with SPIR-V capability metadata.
/// </summary>
public class VulkanCompiledKernel(
    Guid guid,
    string kernelName,
    CompiledKernelType kernelType,
    CompiledKernelSharedMemoryMode sharedMemoryMode,
    uint descriptorBindingCount = 1,
    uint scalarArgumentSizeInBytes = 0,
    VulkanAcceleratorCapabilities? requiredCapabilities = null) :
    CompiledKernel(guid, kernelName, kernelType, sharedMemoryMode),
    ICompiledKernelKind
{
    /// <summary>
    /// Returns the general Vulkan accelerator type.
    /// </summary>
    public static AcceleratorType GeneralAcceleratorType => AcceleratorType.None;

    /// <inheritdoc/>
    public override AcceleratorCapabilities RequiredCapabilities { get; } =
        requiredCapabilities ?? new VulkanAcceleratorCapabilities();

    /// <summary>
    /// Returns the number of storage-buffer descriptor bindings used by this kernel.
    /// </summary>
    public uint DescriptorBindingCount { get; } = descriptorBindingCount;

    /// <summary>
    /// Returns the scalar argument payload size pushed through push constants.
    /// </summary>
    public uint ScalarArgumentSizeInBytes { get; } = scalarArgumentSizeInBytes;

    /// <inheritdoc/>
    public override string GetSourceAsString() =>
        throw new InvalidOperationException(
            "Override GetSourceAsString in derived Vulkan compiled kernels.");
}
