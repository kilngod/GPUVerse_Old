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

using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace VulkanPlatform
{
    public static class VulkanMemory
    {

        public unsafe static void DownloadBufferData<T>(this VkDevice device, VkDeviceMemory memory,ref T[] data) where T : struct
        {
            ulong size = (ulong)(data.Length * Unsafe.SizeOf<T>());
            void* mappedMemory;
            VulkanNative.vkMapMemory(device, memory, 0, size, 0, &mappedMemory);
            GCHandle gh = GCHandle.Alloc(data, GCHandleType.Pinned);
            Unsafe.CopyBlock(gh.AddrOfPinnedObject().ToPointer(), mappedMemory, (uint)size);
            gh.Free();
            VulkanNative.vkUnmapMemory(device, memory);
        }

        public unsafe static void DownloadBufferData<T>(this VkDevice device, VkDeviceMemory memory, ref T data, uint count) where T : struct
        {
            ulong size = (ulong)(count * Unsafe.SizeOf<T>());
            void* mappedMemory;
            VulkanNative.vkMapMemory(device, memory, 0, size, 0, &mappedMemory);
            void* dataPtr = Unsafe.AsPointer(ref data);
            Unsafe.CopyBlock(dataPtr, mappedMemory, (uint)size);
            VulkanNative.vkUnmapMemory(device, memory);
        }

        public unsafe static void UploadBufferData<T>(this VkDevice device, VkDeviceMemory memory,ref T[] data) where T : struct
        {
            ulong size = (ulong)(data.Length * Unsafe.SizeOf<T>());
            void* mappedMemory;
            VulkanNative.vkMapMemory(device, memory, 0, size, 0, &mappedMemory);
            GCHandle gh = GCHandle.Alloc(data, GCHandleType.Pinned);
            Unsafe.CopyBlock(mappedMemory, gh.AddrOfPinnedObject().ToPointer(), (uint)size);
            gh.Free();
            VulkanNative.vkUnmapMemory(device, memory);
        }

        

        public unsafe static void UploadBufferData<T>(this VkDevice device, VkDeviceMemory memory, ref T data, uint count) where T : struct
        {
            ulong size = (ulong)(count * Unsafe.SizeOf<T>());
            void* mappedMemory;
            VulkanNative.vkMapMemory(device, memory, 0, size, 0, &mappedMemory);
            void* dataPtr = Unsafe.AsPointer(ref data);
            Unsafe.CopyBlock(mappedMemory, dataPtr, (uint)size);
            VulkanNative.vkUnmapMemory(device, memory);
        }

        public static uint FindMemoryType(uint memory_type_bits, VkPhysicalDeviceMemoryProperties memoryProperties, VkMemoryPropertyFlags requestedMemoryFlags)
        {

            VkMemoryPropertyFlags memoryPropertyFlags = memoryProperties.GetMemoryType(0).propertyFlags;

            if ((memoryPropertyFlags & requestedMemoryFlags) == requestedMemoryFlags)
            {
            //    localGPUMemory = (requestedMemoryFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT) > 0;
            //    localCPUMemory = (requestedMemoryFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_CACHED_BIT) > 0;
                return 0;
            }
            for (int i = 1; i < 32; i++)
            {
                memoryPropertyFlags = memoryProperties.GetMemoryType(1).propertyFlags;
                if (((memory_type_bits << (i - 1) & 1) == 1) & (memoryPropertyFlags & requestedMemoryFlags) == requestedMemoryFlags)
                {
              //      localGPUMemory = (memoryPropertyFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT) > 0;
              //      localCPUMemory = (requestedMemoryFlags & VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_CACHED_BIT) > 0;
                    return (uint)i;
                }
            }
            return uint.MaxValue;

        }

        public static unsafe void AllocateMemory(this IVulkanSupport support, ref VkBuffer buffer, ref VkDeviceMemory deviceMemory)
        {
            VkMemoryRequirements memoryRequirements = default(VkMemoryRequirements);
            VkPhysicalDeviceMemoryProperties memoryProperties = default(VkPhysicalDeviceMemoryProperties);

            VulkanNative.vkGetBufferMemoryRequirements(support.Device, buffer, &memoryRequirements);

            VulkanNative.vkGetPhysicalDeviceMemoryProperties(support.PhysicalDevice, &memoryProperties);

            VkMemoryAllocateInfo allocateInfo = new VkMemoryAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                memoryTypeIndex = FindMemoryType(memoryRequirements.memoryTypeBits, memoryProperties, VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT | VkMemoryPropertyFlags.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT),
                allocationSize = memoryRequirements.size
            };

            fixed (VkDeviceMemory* memoryDevicePtr = &deviceMemory)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkAllocateMemory(support.Device, &allocateInfo, null, memoryDevicePtr));

            }

        }

        public static unsafe void BindDeviceMemory(this VkDevice device, ref VkBuffer buffer, ref VkDeviceMemory memory, uint Offset)
        {

            VulkanHelpers.CheckErrors(VulkanNative.vkBindBufferMemory(device, buffer, memory, Offset));

        }
    }
}
