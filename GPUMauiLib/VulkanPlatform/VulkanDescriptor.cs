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

/*
 https://vkguide.dev/docs/chapter-4/descriptors_code_more/
  
https://vkguide.dev/docs/extra-chapter/abstracting_descriptors/

Vulkan is meant to be multi-threaded, descriptors are designed to be used with many threads. 
This code need to be adjusted to reflect proper multi threading support.

https://www.intel.com/content/www/us/en/developer/articles/training/api-without-secrets-introduction-to-vulkan-part-6.html

*/

namespace VulkanPlatform
{
	public static class VulkanDescriptor
	{

        

		public unsafe static void CreateDescriptorSetLayout(this VkDevice device, ref VkDescriptorSetLayoutCreateInfo createInfo, ref VkDescriptorSetLayout layout)
        {
            fixed (VkDescriptorSetLayoutCreateInfo* createInfoPtr = &createInfo)
            {
                fixed (VkDescriptorSetLayout* layoutPtr = &layout)
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateDescriptorSetLayout(device, createInfoPtr, null, layoutPtr));
                }
            }
        }

        public unsafe static void CreateDescriptorPool(this VkDevice device, ref VkDescriptorPoolCreateInfo poolCreateInfo, ref VkDescriptorPool pool)
        {
            fixed (VkDescriptorPool* poolPtr = &pool)
            {
                fixed (VkDescriptorPoolCreateInfo* poolInfoPtr = &poolCreateInfo)
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateDescriptorPool(device, poolInfoPtr, null, poolPtr));
                }
            }
        }

		public unsafe static void AllocateDescriptorSets(this VkDevice device, ref VkDescriptorSetAllocateInfo allocInfo, ref VkDescriptorSet descriptorSet)
		{
			fixed (VkDescriptorSetAllocateInfo* allocPtr = &allocInfo)
			{
				fixed (VkDescriptorSet* descripterSetPtr = &descriptorSet)
				{
					VulkanHelpers.CheckErrors(VulkanNative.vkAllocateDescriptorSets(device, allocPtr, descripterSetPtr));
				}
			}

        }

        public unsafe static void UpdateDescriptorSet(this VkDevice device, ref VkWriteDescriptorSet writeSet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet)
            {
                
                    VulkanNative.vkUpdateDescriptorSets(device, 1, writeSetPtr, 0, null);
                
            }

        }

        public unsafe static void UpdateDescriptorSet(this VkDevice device, ref VkWriteDescriptorSet writeSet, ref VkCopyDescriptorSet copySet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet)
            {
                fixed (VkCopyDescriptorSet* copySetPtr = &copySet)
                {
                    VulkanNative.vkUpdateDescriptorSets(device,1, writeSetPtr, 1, copySetPtr);
                }
            }

        }

        public unsafe static void UpdateDescriptorSets(this VkDevice device, ref VkWriteDescriptorSet[] writeSet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet[0])
            {

                VulkanNative.vkUpdateDescriptorSets(device, (uint)writeSet.Length, writeSetPtr, 0, null);

            }

        }
        public unsafe static void UpdateDescriptorSets(this VkDevice device, ref VkWriteDescriptorSet[] writeSet, ref VkCopyDescriptorSet[] copySet)
        {
            fixed (VkWriteDescriptorSet* writeSetPtr = &writeSet[0])
            {
                fixed (VkCopyDescriptorSet* copySetPtr = &copySet[0])
                {
                    VulkanNative.vkUpdateDescriptorSets(device,(uint) writeSet.Length, writeSetPtr, (uint)copySet.Length, copySetPtr);
                }
            }

        }



       
    }
}


