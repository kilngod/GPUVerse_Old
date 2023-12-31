// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;

using GPUVulkan;

namespace VulkanPlatform
{
    public static class VulkanImage
    {
        /*
        public static void CreateTextureImage(this IVulkanSupport support)
        {
            VkImage image;
            using (var fs = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "Textures", "texture.jpg")))
            {
                image = Image.Load(fs);
            }
            ulong imageSize = (ulong)(image.Width * image.Height * Unsafe.SizeOf<Rgba32>());

            CreateImage(
                (uint)image.Width,
                (uint)image.Height,
                VkFormat.R8g8b8a8Unorm,
                VkImageTiling.Linear,
                VkImageUsageFlags.TransferSrc,
                VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT,
                out VkImage stagingImage,
                out VkDeviceMemory stagingImageMemory);

            VkImageSubresource subresource = new VkImageSubresource();
            subresource.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
            subresource.mipLevel = 0;
            subresource.arrayLayer = 0;

            VulkanNative.vkGetImageSubresourceLayout(support.Device, stagingImage, ref subresource, out VkSubresourceLayout stagingImageLayout);
            ulong rowPitch = stagingImageLayout.rowPitch;

            void* mappedPtr;
            VulkanNative.vkMapMemory(_device, stagingImageMemory, 0, imageSize, 0, &mappedPtr);
            fixed (void* pixelsPtr = &image.DangerousGetPinnableReferenceToPixelBuffer())
            {
                if (rowPitch == (ulong)image.Width)
                {
                    Buffer.MemoryCopy(pixelsPtr, mappedPtr, imageSize, imageSize);
                }
                else
                {
                    for (uint y = 0; y < image.Height; y++)
                    {
                        byte* dstRowStart = ((byte*)mappedPtr) + (rowPitch * y);
                        byte* srcRowStart = ((byte*)pixelsPtr) + (image.Width * y * Unsafe.SizeOf<Rgba32>());
                        Unsafe.CopyBlock(dstRowStart, srcRowStart, (uint)(image.Width * Unsafe.SizeOf<Rgba32>()));
                    }
                }
            }
            VulkanNative.vkUnmapMemory(_device, stagingImageMemory);

            CreateImage(
                (uint)image.Width,
                (uint)image.Height,
                VkFormat.R8g8b8a8Unorm,
                VkImageTiling.Optimal,
                VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
                VkMemoryPropertyFlags.DeviceLocal,
                out _textureImage,
                out _textureImageMemory);

            VulkanNative.TransitionImageLayout(stagingImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.Preinitialized, VkImageLayout.TransferSrcOptimal);
            VulkanNative.TransitionImageLayout(_textureImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.Preinitialized, VkImageLayout.TransferDstOptimal);
            VulkanNative.CopyImage(stagingImage, _textureImage, (uint)image.Width, (uint)image.Height);
            VulkanNative.TransitionImageLayout(_textureImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal);

            VulkanNative.vkDestroyImage(support.Device, stagingImage, null);
        }
        */

        public unsafe static void CreateImage(
            this IVulkanSupport support,
         uint width,
         uint height,
         VkFormat format,
         VkImageTiling tiling,
         VkImageUsageFlags usage,
         VkMemoryPropertyFlags requestedMemoryProperties,
         ref VkImage image,
         ref VkDeviceMemory memory)
        {
            VkImageCreateInfo imageCI = new VkImageCreateInfo()
            {
                imageType = VkImageType.VK_IMAGE_TYPE_2D,
                extent = new VkExtent3D() { width = width, height = height, depth = 1 },
                mipLevels = 1,
                arrayLayers = 1,
                format = format,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_PREINITIALIZED,
                usage = usage,
                sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE,
                samples = VkSampleCountFlags.VK_SAMPLE_COUNT_1_BIT
            };


            fixed (VkImage* imagePtr = &image)
            {
                VulkanNative.vkCreateImage(support.Device, &imageCI, null, imagePtr);
            }
            VkMemoryRequirements memoryRequirements = default(VkMemoryRequirements);
            VkPhysicalDeviceMemoryProperties memoryProperties = default(VkPhysicalDeviceMemoryProperties);

            VulkanNative.vkGetPhysicalDeviceMemoryProperties(support.PhysicalDevice, &memoryProperties);

            VulkanNative.vkGetImageMemoryRequirements(support.Device, image, &memoryRequirements);
        
            VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo()
            {
                allocationSize = memoryRequirements.size,
                memoryTypeIndex = VulkanMemory.FindMemoryType(memoryRequirements.memoryTypeBits, memoryProperties, requestedMemoryProperties)
            };

            fixed (VkDeviceMemory* memoryPtr = &memory)
                VulkanNative.vkAllocateMemory(support.Device, &allocInfo, null, memoryPtr);

            VulkanNative.vkBindImageMemory(support.Device, image, memory, 0);
        }
    }
}

