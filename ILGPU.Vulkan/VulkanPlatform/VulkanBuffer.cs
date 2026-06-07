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
	public static class VulkanBuffer
	{
		public unsafe static void CreateBuffer(this VkDevice device, ulong bufferSize, ref VkBuffer buffer, VkBufferUsageFlags usageFlags = VkBufferUsageFlags.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, VkSharingMode mode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE)
		{
			VkBufferCreateInfo createInfo = new VkBufferCreateInfo()
			{
				 sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
				 sharingMode = mode,
				 size = bufferSize,
				 usage = usageFlags
            };

			fixed (VkBuffer* bufferPtr = &buffer)
			{

				VulkanNative.vkCreateBuffer(device, &createInfo, null, bufferPtr);
			}
			
			
		}
	}
}

