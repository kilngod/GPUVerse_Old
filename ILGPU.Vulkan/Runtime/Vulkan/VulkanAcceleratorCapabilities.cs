// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2026 ILGPU Project
//                                    www.ilgpu.net
//
// File: VulkanAcceleratorCapabilities.cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;

namespace ILGPU.Runtime.Vulkan;

/// <summary>
/// Describes the queried Vulkan accelerator capability set.
/// </summary>
public record class VulkanAcceleratorCapabilities : AcceleratorCapabilities
{
    /// <summary>
    /// Queries all known Vulkan feature structures supported by the current
    /// bindings for a physical device.
    /// </summary>
    /// <param name="physicalDevice">The Vulkan physical device to query.</param>
    /// <param name="apiVersion">The Vulkan API version exposed by the device.</param>
    /// <returns>The queried Vulkan accelerator capabilities.</returns>
    public static unsafe VulkanAcceleratorCapabilities Query(
        VkPhysicalDevice physicalDevice,
        uint apiVersion)
    {
        if (physicalDevice == VkPhysicalDevice.Null)
        {
            throw new ArgumentException(
                "Invalid Vulkan physical device.",
                nameof(physicalDevice));
        }

        var deviceExtensions = VulkanAPI.EnumerateDeviceExtensions(physicalDevice);
        var shaderFloat16Int8Features =
            new VkPhysicalDeviceShaderFloat16Int8Features
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_SHADER_FLOAT16_INT8_FEATURES,
            };
        var shaderBFloat16Features =
            new VkPhysicalDeviceShaderBfloat16FeaturesKHR
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_SHADER_BFLOAT16_FEATURES_KHR,
            };
        var shaderFloat8Features =
            new VkPhysicalDeviceShaderFloat8FeaturesEXT
            {
                sType = VkStructureType
                    .VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_SHADER_FLOAT8_FEATURES_EXT,
            };

        var features2 = new VkPhysicalDeviceFeatures2
        {
            sType = VkStructureType.VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_FEATURES_2,
        };

        void* next = null;
        if (VulkanAPI.SupportsShaderFloat8(deviceExtensions))
        {
            shaderFloat8Features.pNext = next;
            next = &shaderFloat8Features;
        }
        if (VulkanAPI.SupportsShaderBFloat16(deviceExtensions))
        {
            shaderBFloat16Features.pNext = next;
            next = &shaderBFloat16Features;
        }
        if (VulkanAPI.SupportsShaderFloat16Int8(apiVersion, deviceExtensions))
        {
            shaderFloat16Int8Features.pNext = next;
            next = &shaderFloat16Int8Features;
        }

        features2.pNext = next;
        VulkanAPI.GetPhysicalDeviceFeatures2(physicalDevice, ref features2);

        return FromFeatures(
            features2.features,
            shaderFloat16Int8Features,
            shaderBFloat16Features,
            shaderFloat8Features);
    }

    /// <summary>
    /// Creates a capability set from queried Vulkan feature structures.
    /// </summary>
    public static VulkanAcceleratorCapabilities FromFeatures(
        VkPhysicalDeviceFeatures features,
        VkPhysicalDeviceShaderFloat16Int8Features shaderFloat16Int8Features,
        VkPhysicalDeviceShaderBfloat16FeaturesKHR shaderBFloat16Features,
        VkPhysicalDeviceShaderFloat8FeaturesEXT shaderFloat8Features) =>
        new()
        {
            RobustBufferAccess = features.robustBufferAccess,
            FullDrawIndexUInt32 = features.fullDrawIndexUint32,
            ImageCubeArray = features.imageCubeArray,
            IndependentBlend = features.independentBlend,
            GeometryShader = features.geometryShader,
            TessellationShader = features.tessellationShader,
            SampleRateShading = features.sampleRateShading,
            DualSourceBlend = features.dualSrcBlend,
            LogicOperation = features.logicOp,
            MultiDrawIndirect = features.multiDrawIndirect,
            DrawIndirectFirstInstance = features.drawIndirectFirstInstance,
            DepthClamp = features.depthClamp,
            DepthBiasClamp = features.depthBiasClamp,
            FillModeNonSolid = features.fillModeNonSolid,
            DepthBounds = features.depthBounds,
            WideLines = features.wideLines,
            LargePoints = features.largePoints,
            AlphaToOne = features.alphaToOne,
            MultiViewport = features.multiViewport,
            SamplerAnisotropy = features.samplerAnisotropy,
            TextureCompressionETC2 = features.textureCompressionETC2,
            TextureCompressionASTCLdr = features.textureCompressionASTC_LDR,
            TextureCompressionBC = features.textureCompressionBC,
            OcclusionQueryPrecise = features.occlusionQueryPrecise,
            PipelineStatisticsQuery = features.pipelineStatisticsQuery,
            VertexPipelineStoresAndAtomics = features.vertexPipelineStoresAndAtomics,
            FragmentStoresAndAtomics = features.fragmentStoresAndAtomics,
            ShaderTessellationAndGeometryPointSize =
                features.shaderTessellationAndGeometryPointSize,
            ShaderImageGatherExtended = features.shaderImageGatherExtended,
            ShaderStorageImageExtendedFormats =
                features.shaderStorageImageExtendedFormats,
            ShaderStorageImageMultisample = features.shaderStorageImageMultisample,
            ShaderStorageImageReadWithoutFormat =
                features.shaderStorageImageReadWithoutFormat,
            ShaderStorageImageWriteWithoutFormat =
                features.shaderStorageImageWriteWithoutFormat,
            ShaderUniformBufferArrayDynamicIndexing =
                features.shaderUniformBufferArrayDynamicIndexing,
            ShaderSampledImageArrayDynamicIndexing =
                features.shaderSampledImageArrayDynamicIndexing,
            ShaderStorageBufferArrayDynamicIndexing =
                features.shaderStorageBufferArrayDynamicIndexing,
            ShaderStorageImageArrayDynamicIndexing =
                features.shaderStorageImageArrayDynamicIndexing,
            ShaderClipDistance = features.shaderClipDistance,
            ShaderCullDistance = features.shaderCullDistance,
            ShaderInt64 = features.shaderInt64,
            ShaderInt16 = features.shaderInt16,
            ShaderResourceResidency = features.shaderResourceResidency,
            ShaderResourceMinLod = features.shaderResourceMinLod,
            SparseBinding = features.sparseBinding,
            SparseResidencyBuffer = features.sparseResidencyBuffer,
            SparseResidencyImage2D = features.sparseResidencyImage2D,
            SparseResidencyImage3D = features.sparseResidencyImage3D,
            SparseResidency2Samples = features.sparseResidency2Samples,
            SparseResidency4Samples = features.sparseResidency4Samples,
            SparseResidency8Samples = features.sparseResidency8Samples,
            SparseResidency16Samples = features.sparseResidency16Samples,
            SparseResidencyAliased = features.sparseResidencyAliased,
            VariableMultisampleRate = features.variableMultisampleRate,
            InheritedQueries = features.inheritedQueries,
            Float16 = shaderFloat16Int8Features.shaderFloat16,
            Float64 = features.shaderFloat64,
            BFloat16 = shaderBFloat16Features.shaderBFloat16Type,
            FP8 = shaderFloat8Features.shaderFloat8,
            ShaderInt8 = shaderFloat16Int8Features.shaderInt8,
            ShaderBFloat16DotProduct =
                shaderBFloat16Features.shaderBFloat16DotProduct,
            ShaderBFloat16CooperativeMatrix =
                shaderBFloat16Features.shaderBFloat16CooperativeMatrix,
            ShaderFloat8CooperativeMatrix =
                shaderFloat8Features.shaderFloat8CooperativeMatrix,
        };

    /// <inheritdoc/>
    public override AcceleratorType AcceleratorType => AcceleratorType.None;

    /// <inheritdoc/>
    public override bool Float16 { get; init; }

    /// <inheritdoc/>
    public override bool Float64 { get; init; }

    /// <inheritdoc/>
    public override bool BFloat16 { get; init; }

    /// <inheritdoc/>
    public override bool FP8 { get; init; }

    /// <inheritdoc/>
    public override bool FP4 { get; init; }

    /// <summary>Returns true if robust buffer access is supported.</summary>
    public bool RobustBufferAccess { get; init; }

    /// <summary>Returns true if full 32-bit draw indices are supported.</summary>
    public bool FullDrawIndexUInt32 { get; init; }

    /// <summary>Returns true if cube-map image arrays are supported.</summary>
    public bool ImageCubeArray { get; init; }

    /// <summary>Returns true if independent blending is supported.</summary>
    public bool IndependentBlend { get; init; }

    /// <summary>Returns true if geometry shaders are supported.</summary>
    public bool GeometryShader { get; init; }

    /// <summary>Returns true if tessellation shaders are supported.</summary>
    public bool TessellationShader { get; init; }

    /// <summary>Returns true if sample-rate shading is supported.</summary>
    public bool SampleRateShading { get; init; }

    /// <summary>Returns true if dual-source blending is supported.</summary>
    public bool DualSourceBlend { get; init; }

    /// <summary>Returns true if logic operations are supported.</summary>
    public bool LogicOperation { get; init; }

    /// <summary>Returns true if multi-draw indirect is supported.</summary>
    public bool MultiDrawIndirect { get; init; }

    /// <summary>Returns true if first-instance indirect draws are supported.</summary>
    public bool DrawIndirectFirstInstance { get; init; }

    /// <summary>Returns true if depth clamping is supported.</summary>
    public bool DepthClamp { get; init; }

    /// <summary>Returns true if depth-bias clamping is supported.</summary>
    public bool DepthBiasClamp { get; init; }

    /// <summary>Returns true if non-solid fill modes are supported.</summary>
    public bool FillModeNonSolid { get; init; }

    /// <summary>Returns true if depth bounds tests are supported.</summary>
    public bool DepthBounds { get; init; }

    /// <summary>Returns true if wide lines are supported.</summary>
    public bool WideLines { get; init; }

    /// <summary>Returns true if large points are supported.</summary>
    public bool LargePoints { get; init; }

    /// <summary>Returns true if alpha-to-one is supported.</summary>
    public bool AlphaToOne { get; init; }

    /// <summary>Returns true if multiple viewports are supported.</summary>
    public bool MultiViewport { get; init; }

    /// <summary>Returns true if sampler anisotropy is supported.</summary>
    public bool SamplerAnisotropy { get; init; }

    /// <summary>Returns true if ETC2 texture compression is supported.</summary>
    public bool TextureCompressionETC2 { get; init; }

    /// <summary>Returns true if ASTC LDR texture compression is supported.</summary>
    public bool TextureCompressionASTCLdr { get; init; }

    /// <summary>Returns true if BC texture compression is supported.</summary>
    public bool TextureCompressionBC { get; init; }

    /// <summary>Returns true if precise occlusion queries are supported.</summary>
    public bool OcclusionQueryPrecise { get; init; }

    /// <summary>Returns true if pipeline statistics queries are supported.</summary>
    public bool PipelineStatisticsQuery { get; init; }

    /// <summary>Returns true if vertex pipeline atomics are supported.</summary>
    public bool VertexPipelineStoresAndAtomics { get; init; }

    /// <summary>Returns true if fragment shader atomics are supported.</summary>
    public bool FragmentStoresAndAtomics { get; init; }

    /// <summary>Returns true if shader point-size writes are supported.</summary>
    public bool ShaderTessellationAndGeometryPointSize { get; init; }

    /// <summary>Returns true if extended image gather is supported.</summary>
    public bool ShaderImageGatherExtended { get; init; }

    /// <summary>Returns true if extended storage image formats are supported.</summary>
    public bool ShaderStorageImageExtendedFormats { get; init; }

    /// <summary>Returns true if multisample storage images are supported.</summary>
    public bool ShaderStorageImageMultisample { get; init; }

    /// <summary>Returns true if format-less storage image reads are supported.</summary>
    public bool ShaderStorageImageReadWithoutFormat { get; init; }

    /// <summary>Returns true if format-less storage image writes are supported.</summary>
    public bool ShaderStorageImageWriteWithoutFormat { get; init; }

    /// <summary>Returns true if dynamic uniform-buffer indexing is supported.</summary>
    public bool ShaderUniformBufferArrayDynamicIndexing { get; init; }

    /// <summary>Returns true if dynamic sampled-image indexing is supported.</summary>
    public bool ShaderSampledImageArrayDynamicIndexing { get; init; }

    /// <summary>Returns true if dynamic storage-buffer indexing is supported.</summary>
    public bool ShaderStorageBufferArrayDynamicIndexing { get; init; }

    /// <summary>Returns true if dynamic storage-image indexing is supported.</summary>
    public bool ShaderStorageImageArrayDynamicIndexing { get; init; }

    /// <summary>Returns true if clip distances are supported.</summary>
    public bool ShaderClipDistance { get; init; }

    /// <summary>Returns true if cull distances are supported.</summary>
    public bool ShaderCullDistance { get; init; }

    /// <summary>Returns true if 64-bit integer shader operations are supported.</summary>
    public bool ShaderInt64 { get; init; }

    /// <summary>Returns true if 16-bit integer shader operations are supported.</summary>
    public bool ShaderInt16 { get; init; }

    /// <summary>Returns true if 8-bit integer shader operations are supported.</summary>
    public bool ShaderInt8 { get; init; }

    /// <summary>Returns true if shader resource residency is supported.</summary>
    public bool ShaderResourceResidency { get; init; }

    /// <summary>Returns true if shader resource min-lod is supported.</summary>
    public bool ShaderResourceMinLod { get; init; }

    /// <summary>Returns true if sparse memory binding is supported.</summary>
    public bool SparseBinding { get; init; }

    /// <summary>Returns true if sparse buffer residency is supported.</summary>
    public bool SparseResidencyBuffer { get; init; }

    /// <summary>Returns true if sparse 2D image residency is supported.</summary>
    public bool SparseResidencyImage2D { get; init; }

    /// <summary>Returns true if sparse 3D image residency is supported.</summary>
    public bool SparseResidencyImage3D { get; init; }

    /// <summary>Returns true if sparse 2-sample residency is supported.</summary>
    public bool SparseResidency2Samples { get; init; }

    /// <summary>Returns true if sparse 4-sample residency is supported.</summary>
    public bool SparseResidency4Samples { get; init; }

    /// <summary>Returns true if sparse 8-sample residency is supported.</summary>
    public bool SparseResidency8Samples { get; init; }

    /// <summary>Returns true if sparse 16-sample residency is supported.</summary>
    public bool SparseResidency16Samples { get; init; }

    /// <summary>Returns true if sparse aliased residency is supported.</summary>
    public bool SparseResidencyAliased { get; init; }

    /// <summary>Returns true if variable multisample rates are supported.</summary>
    public bool VariableMultisampleRate { get; init; }

    /// <summary>Returns true if inherited queries are supported.</summary>
    public bool InheritedQueries { get; init; }

    /// <summary>Returns true if BFloat16 dot product is supported.</summary>
    public bool ShaderBFloat16DotProduct { get; init; }

    /// <summary>Returns true if BFloat16 cooperative matrix support is available.</summary>
    public bool ShaderBFloat16CooperativeMatrix { get; init; }

    /// <summary>Returns true if FP8 cooperative matrix support is available.</summary>
    public bool ShaderFloat8CooperativeMatrix { get; init; }

    /// <inheritdoc/>
    public override int SpecificityOrdinal =>
        CountEnabledCapabilities();

    /// <summary>
    /// Creates the core Vulkan feature structure represented by this capability
    /// set.
    /// </summary>
    /// <returns>The core Vulkan feature structure.</returns>
    public VkPhysicalDeviceFeatures ToPhysicalDeviceFeatures() =>
        new()
        {
            robustBufferAccess = RobustBufferAccess,
            fullDrawIndexUint32 = FullDrawIndexUInt32,
            imageCubeArray = ImageCubeArray,
            independentBlend = IndependentBlend,
            geometryShader = GeometryShader,
            tessellationShader = TessellationShader,
            sampleRateShading = SampleRateShading,
            dualSrcBlend = DualSourceBlend,
            logicOp = LogicOperation,
            multiDrawIndirect = MultiDrawIndirect,
            drawIndirectFirstInstance = DrawIndirectFirstInstance,
            depthClamp = DepthClamp,
            depthBiasClamp = DepthBiasClamp,
            fillModeNonSolid = FillModeNonSolid,
            depthBounds = DepthBounds,
            wideLines = WideLines,
            largePoints = LargePoints,
            alphaToOne = AlphaToOne,
            multiViewport = MultiViewport,
            samplerAnisotropy = SamplerAnisotropy,
            textureCompressionETC2 = TextureCompressionETC2,
            textureCompressionASTC_LDR = TextureCompressionASTCLdr,
            textureCompressionBC = TextureCompressionBC,
            occlusionQueryPrecise = OcclusionQueryPrecise,
            pipelineStatisticsQuery = PipelineStatisticsQuery,
            vertexPipelineStoresAndAtomics = VertexPipelineStoresAndAtomics,
            fragmentStoresAndAtomics = FragmentStoresAndAtomics,
            shaderTessellationAndGeometryPointSize =
                ShaderTessellationAndGeometryPointSize,
            shaderImageGatherExtended = ShaderImageGatherExtended,
            shaderStorageImageExtendedFormats =
                ShaderStorageImageExtendedFormats,
            shaderStorageImageMultisample = ShaderStorageImageMultisample,
            shaderStorageImageReadWithoutFormat =
                ShaderStorageImageReadWithoutFormat,
            shaderStorageImageWriteWithoutFormat =
                ShaderStorageImageWriteWithoutFormat,
            shaderUniformBufferArrayDynamicIndexing =
                ShaderUniformBufferArrayDynamicIndexing,
            shaderSampledImageArrayDynamicIndexing =
                ShaderSampledImageArrayDynamicIndexing,
            shaderStorageBufferArrayDynamicIndexing =
                ShaderStorageBufferArrayDynamicIndexing,
            shaderStorageImageArrayDynamicIndexing =
                ShaderStorageImageArrayDynamicIndexing,
            shaderClipDistance = ShaderClipDistance,
            shaderCullDistance = ShaderCullDistance,
            shaderFloat64 = Float64,
            shaderInt64 = ShaderInt64,
            shaderInt16 = ShaderInt16,
            shaderResourceResidency = ShaderResourceResidency,
            shaderResourceMinLod = ShaderResourceMinLod,
            sparseBinding = SparseBinding,
            sparseResidencyBuffer = SparseResidencyBuffer,
            sparseResidencyImage2D = SparseResidencyImage2D,
            sparseResidencyImage3D = SparseResidencyImage3D,
            sparseResidency2Samples = SparseResidency2Samples,
            sparseResidency4Samples = SparseResidency4Samples,
            sparseResidency8Samples = SparseResidency8Samples,
            sparseResidency16Samples = SparseResidency16Samples,
            sparseResidencyAliased = SparseResidencyAliased,
            variableMultisampleRate = VariableMultisampleRate,
            inheritedQueries = InheritedQueries,
        };

    /// <inheritdoc/>
    public override bool IsCompatible(AcceleratorCapabilities kernelRequirements)
    {
        if (kernelRequirements is not VulkanAcceleratorCapabilities req)
            return false;
        return IsCompatibleWith(req);
    }

    /// <inheritdoc/>
    public override void CheckCompatibility(AcceleratorCapabilities kernelRequirements)
    {
        if (kernelRequirements is not VulkanAcceleratorCapabilities req)
        {
            throw new CapabilityNotSupportedException(
                $"Kernel targets {kernelRequirements.AcceleratorType} but device"
                + " is Vulkan.");
        }

        CheckCompatibilityWith(req);
    }

    private void CheckCompatibilityWith(VulkanAcceleratorCapabilities req)
    {
        foreach (var property in typeof(VulkanAcceleratorCapabilities)
            .GetProperties())
        {
            if (property.PropertyType != typeof(bool))
                continue;

            var deviceValue = (bool)property.GetValue(this)!;
            var requiredValue = (bool)property.GetValue(req)!;
            Check(property.Name, deviceValue, requiredValue);
        }
    }

    private bool IsCompatibleWith(VulkanAcceleratorCapabilities req)
    {
        try
        {
            CheckCompatibilityWith(req);
            return true;
        }
        catch (CapabilityNotSupportedException)
        {
            return false;
        }
    }

    private static void Check(string name, bool device, bool required)
    {
        if (required && !device)
        {
            throw new CapabilityNotSupportedException(
                $"Kernel requires Vulkan capability '{name}' which is not " +
                "available on this device.");
        }
    }

    private int CountEnabledCapabilities()
    {
        int result = 0;
        foreach (var property in typeof(VulkanAcceleratorCapabilities)
            .GetProperties())
        {
            if (property.PropertyType == typeof(bool) &&
                (bool)property.GetValue(this)!)
            {
                ++result;
            }
        }
        return result;
    }
}
