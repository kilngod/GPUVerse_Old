// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanKernel.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Runtime.CompilerServices;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Represents a loaded Vulkan compute kernel backed by a SPIR-V shader module.
/// </summary>
public sealed class VulkanKernel : Kernel
{
    /// <summary>
    /// Creates a new Vulkan kernel by loading the SPIR-V shader module.
    /// </summary>
    /// <param name="accelerator">The parent Vulkan accelerator.</param>
    /// <param name="compiledKernel">The compiled Vulkan kernel.</param>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal VulkanKernel(
        VulkanAccelerator accelerator,
        VulkanCompiledKernel compiledKernel)
        : base(accelerator, compiledKernel)
    {
        DescriptorBindingCount = compiledKernel.DescriptorBindingCount;
        ScalarArgumentSizeInBytes = compiledKernel.ScalarArgumentSizeInBytes;
        ValidateScalarArgumentMetadata(accelerator, ScalarArgumentSizeInBytes);

        DescriptorSetLayout = VulkanAPI.CreateStorageBufferDescriptorSetLayout(
            accelerator.LogicalDevice,
            DescriptorBindingCount);
        DescriptorPool = VulkanAPI.CreateStorageBufferDescriptorPool(
            accelerator.LogicalDevice,
            DescriptorBindingCount);
        DescriptorSets = VulkanAPI.AllocateDescriptorSets(
            accelerator.LogicalDevice,
            DescriptorPool,
            DescriptorSetLayout);
        PipelineLayout = VulkanAPI.CreatePipelineLayout(
            accelerator.LogicalDevice,
            DescriptorSetLayout,
            ScalarArgumentSizeInBytes);

        var binary = compiledKernel.GetCompiledBinary();
        ShaderModule = VulkanAPI.CreateShaderModule(
            accelerator.LogicalDevice,
            binary.Span);
        ComputePipeline = VulkanAPI.CreateComputePipeline(
            accelerator.LogicalDevice,
            ShaderModule,
            PipelineLayout,
            compiledKernel.KernelName);
    }

    /// <summary>
    /// Returns the parent Vulkan accelerator.
    /// </summary>
    public new VulkanAccelerator Accelerator =>
        (VulkanAccelerator)base.Accelerator!;

    /// <summary>
    /// Returns the loaded Vulkan shader module.
    /// </summary>
    public VkShaderModule ShaderModule { get; private set; }

    /// <summary>
    /// Returns the number of storage-buffer descriptor bindings used by this kernel.
    /// </summary>
    public uint DescriptorBindingCount { get; }

    /// <summary>
    /// Returns the scalar argument payload size pushed through push constants.
    /// </summary>
    public uint ScalarArgumentSizeInBytes { get; }

    /// <summary>
    /// Returns the descriptor set layout owned by this kernel.
    /// </summary>
    public VkDescriptorSetLayout DescriptorSetLayout { get; private set; }

    /// <summary>
    /// Returns the descriptor pool owned by this kernel.
    /// </summary>
    public VkDescriptorPool DescriptorPool { get; private set; }

    /// <summary>
    /// Returns the descriptor sets owned by this kernel.
    /// </summary>
    public VkDescriptorSet[] DescriptorSets { get; private set; } = [];

    /// <summary>
    /// Returns the compute pipeline layout owned by this kernel.
    /// </summary>
    public VkPipelineLayout PipelineLayout { get; private set; }

    /// <summary>
    /// Returns the compute pipeline owned by this kernel.
    /// </summary>
    public VkPipeline ComputePipeline { get; private set; }

    /// <summary>
    /// Launches this kernel using storage-buffer-only Vulkan memory-buffer arguments.
    /// </summary>
    /// <param name="stream">The Vulkan stream to submit work to.</param>
    /// <param name="groupCount">The number of workgroups to dispatch.</param>
    /// <param name="buffers">The storage-buffer arguments to bind.</param>
    public void LaunchStorageBufferKernel(
        VulkanStream stream,
        Index3D groupCount,
        params VulkanMemoryBuffer[] buffers)
    {
        LaunchStorageBufferKernel(
            stream,
            groupCount,
            ReadOnlySpan<byte>.Empty,
            buffers);
    }

    /// <summary>
    /// Launches this kernel using push-constant scalar arguments and
    /// storage-buffer-only Vulkan memory-buffer arguments.
    /// </summary>
    /// <param name="stream">The Vulkan stream to submit work to.</param>
    /// <param name="groupCount">The number of workgroups to dispatch.</param>
    /// <param name="scalarArguments">The packed scalar argument bytes.</param>
    /// <param name="buffers">The storage-buffer arguments to bind.</param>
    public void LaunchStorageBufferKernel(
        VulkanStream stream,
        Index3D groupCount,
        ReadOnlySpan<byte> scalarArguments,
        params VulkanMemoryBuffer[] buffers)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));
        if (buffers is null)
            throw new ArgumentNullException(nameof(buffers));
        if (!ReferenceEquals(stream.Accelerator, Accelerator))
        {
            throw new ArgumentException(
                "The stream belongs to a different accelerator.",
                nameof(stream));
        }
        if (buffers.Length != DescriptorBindingCount)
        {
            throw new ArgumentException(
                $"Expected {DescriptorBindingCount} storage buffers, " +
                $"but received {buffers.Length}.",
                nameof(buffers));
        }
        if (scalarArguments.Length != ScalarArgumentSizeInBytes)
        {
            throw new ArgumentException(
                $"Expected {ScalarArgumentSizeInBytes} scalar argument bytes, " +
                $"but received {scalarArguments.Length}.",
                nameof(scalarArguments));
        }
        if ((scalarArguments.Length & 3) != 0)
        {
            throw new ArgumentException(
                "Scalar argument bytes must be 4-byte aligned for Vulkan " +
                "push constants.",
                nameof(scalarArguments));
        }
        if (groupCount.X < 1 || groupCount.Y < 1 || groupCount.Z < 1)
            return;

        for (int i = 0; i < buffers.Length; ++i)
        {
            var buffer = buffers[i] ??
                throw new ArgumentNullException(
                    nameof(buffers),
                    $"Storage buffer at index {i} is null.");
            if (!ReferenceEquals(buffer.Accelerator, Accelerator))
            {
                throw new ArgumentException(
                    $"Storage buffer at index {i} belongs to a different " +
                    "accelerator.",
                    nameof(buffers));
            }
            if (buffer.Buffer == VkBuffer.Null || buffer.LengthInBytes == 0)
            {
                throw new ArgumentException(
                    $"Storage buffer at index {i} is empty or disposed.",
                    nameof(buffers));
            }
        }

        VulkanAPI.UpdateStorageBufferDescriptorSet(
            Accelerator.LogicalDevice,
            DescriptorSets[0],
            buffers);

        var commandBuffer = stream.AllocateCommandBuffer();
        var fence = VkFence.Null;
        try
        {
            VulkanAPI.BeginCommandBuffer(commandBuffer);
            VulkanAPI.CmdBindComputePipeline(commandBuffer, ComputePipeline);
            VulkanAPI.CmdBindComputeDescriptorSets(
                commandBuffer,
                PipelineLayout,
                DescriptorSets);
            VulkanAPI.CmdPushComputeConstants(
                commandBuffer,
                PipelineLayout,
                scalarArguments);
            VulkanAPI.CmdDispatch(
                commandBuffer,
                checked((uint)groupCount.X),
                checked((uint)groupCount.Y),
                checked((uint)groupCount.Z));
            VulkanAPI.EndCommandBuffer(commandBuffer);

            fence = VulkanAPI.CreateFence(Accelerator.LogicalDevice);
            VulkanAPI.QueueSubmit(stream.Queue, commandBuffer, fence);
            VulkanAPI.WaitForFence(Accelerator.LogicalDevice, fence);
        }
        finally
        {
            if (fence != VkFence.Null)
            {
                VulkanAPI.DestroyFence(Accelerator.LogicalDevice, fence);
            }

            stream.FreeCommandBuffer(commandBuffer);
        }
    }

    private static void ValidateScalarArgumentMetadata(
        VulkanAccelerator accelerator,
        uint scalarArgumentSizeInBytes)
    {
        if ((scalarArgumentSizeInBytes & 3) != 0)
        {
            throw new NotSupportedException(
                "Vulkan scalar argument payloads must be 4-byte aligned when " +
                "using push constants.");
        }
        if (scalarArgumentSizeInBytes > accelerator.Device.MaxPushConstantSize)
        {
            throw new NotSupportedException(
                $"The kernel requires {scalarArgumentSizeInBytes} push-constant " +
                $"bytes, but the device supports only " +
                $"{accelerator.Device.MaxPushConstantSize}.");
        }
    }

    /// <inheritdoc/>
    protected override void DisposeAcceleratorObject(bool disposing)
    {
        if (ComputePipeline != VkPipeline.Null)
        {
            VulkanAPI.DestroyPipeline(
                Accelerator.LogicalDevice,
                ComputePipeline);
            ComputePipeline = VkPipeline.Null;
        }

        if (PipelineLayout != VkPipelineLayout.Null)
        {
            VulkanAPI.DestroyPipelineLayout(
                Accelerator.LogicalDevice,
                PipelineLayout);
            PipelineLayout = VkPipelineLayout.Null;
        }

        if (DescriptorPool != VkDescriptorPool.Null)
        {
            VulkanAPI.DestroyDescriptorPool(
                Accelerator.LogicalDevice,
                DescriptorPool);
            DescriptorPool = VkDescriptorPool.Null;
            DescriptorSets = [];
        }

        if (DescriptorSetLayout != VkDescriptorSetLayout.Null)
        {
            VulkanAPI.DestroyDescriptorSetLayout(
                Accelerator.LogicalDevice,
                DescriptorSetLayout);
            DescriptorSetLayout = VkDescriptorSetLayout.Null;
        }

        if (ShaderModule != VkShaderModule.Null)
        {
            VulkanAPI.DestroyShaderModule(
                Accelerator.LogicalDevice,
                ShaderModule);
            ShaderModule = VkShaderModule.Null;
        }
    }
}
