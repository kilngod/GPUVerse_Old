using System;
using GPUVulkan;
namespace VulkanPlatform
{
	public struct VulkanResourceSegment
	{
        public VkDescriptorPoolSize DescriptorPoolSize;

        // we should assume the number of segments is less than or equal to the descriptor pool size
        public VkDescriptorBufferInfo[] DescriptorBufferSegmentInfos;

        public uint BindingPoint;
    }

}

