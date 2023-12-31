using System;
using GPUVulkan;

namespace VulkanPlatform
{
	/// <summary>
	/// allocate per thread resouces
	/// </summary>
	public struct VulkanResourcePerThread
	{
		public VkDescriptorSet[] DescriptorSets;
		public VkDescriptorSetLayout DescriptorSetLayout; 
		public VkDescriptorPool DescriptorPool;

		public bool InUse;
		public bool Allocated;
        
	}
}

