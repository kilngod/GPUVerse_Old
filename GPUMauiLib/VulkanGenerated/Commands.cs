using System;
using System.Runtime.InteropServices;

#nullable disable

namespace GPUVulkan;
	public static unsafe partial class VulkanNative
	{

#if WINDOWS
		const string  libraryName = "vulkan-1.dll";
#endif
#if ANDROID
		const string  libraryName = "libvulkan.so";
#endif
#if IOS || MACCATALYST
		const string  libraryName = "libMoltenVK.dylib";
#endif
#if IOS || MACCATALYST || WINDOWS || ANDROID
#else
		//linux
		const string  libraryName = "libvulkan.so.1";
#endif



		[DllImport (libraryName)]
		public static extern VkResult vkCreateInstance(VkInstanceCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkInstance* pInstance);


		[DllImport (libraryName)]
		public static extern void vkDestroyInstance(VkInstance instance, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumeratePhysicalDevices(VkInstance instance, uint* pPhysicalDeviceCount, VkPhysicalDevice* pPhysicalDevices);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures* pFeatures);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties* pFormatProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkImageFormatProperties* pImageFormatProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties* pProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice physicalDevice, uint* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceMemoryProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties* pMemoryProperties);


		[DllImport (libraryName)]
		public static extern IntPtr vkGetInstanceProcAddr(VkInstance instance, byte* pName);


		[DllImport (libraryName)]
		public static extern IntPtr vkGetDeviceProcAddr(VkDevice device, byte* pName);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, VkDeviceCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDevice* pDevice);


		[DllImport (libraryName)]
		public static extern void vkDestroyDevice(VkDevice device, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumerateInstanceExtensionProperties(byte* pLayerName, uint* pPropertyCount, VkExtensionProperties* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, byte* pLayerName, uint* pPropertyCount, VkExtensionProperties* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumerateInstanceLayerProperties(uint* pPropertyCount, VkLayerProperties* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkLayerProperties* pProperties);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceQueue(VkDevice device, uint queueFamilyIndex, uint queueIndex, VkQueue* pQueue);


		[DllImport (libraryName)]
		public static extern VkResult vkQueueSubmit(VkQueue queue, uint submitCount, VkSubmitInfo* pSubmits, VkFence fence);


		[DllImport (libraryName)]
		public static extern VkResult vkQueueWaitIdle(VkQueue queue);


		[DllImport (libraryName)]
		public static extern VkResult vkDeviceWaitIdle(VkDevice device);


		[DllImport (libraryName)]
		public static extern VkResult vkAllocateMemory(VkDevice device, VkMemoryAllocateInfo* pAllocateInfo, VkAllocationCallbacks* pAllocator, VkDeviceMemory* pMemory);


		[DllImport (libraryName)]
		public static extern void vkFreeMemory(VkDevice device, VkDeviceMemory memory, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkMapMemory(VkDevice device, VkDeviceMemory memory, ulong offset, ulong size, uint flags, void** ppData);


		[DllImport (libraryName)]
		public static extern void vkUnmapMemory(VkDevice device, VkDeviceMemory memory);


		[DllImport (libraryName)]
		public static extern VkResult vkFlushMappedMemoryRanges(VkDevice device, uint memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);


		[DllImport (libraryName)]
		public static extern VkResult vkInvalidateMappedMemoryRanges(VkDevice device, uint memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceMemoryCommitment(VkDevice device, VkDeviceMemory memory, ulong* pCommittedMemoryInBytes);


		[DllImport (libraryName)]
		public static extern VkResult vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, ulong memoryOffset);


		[DllImport (libraryName)]
		public static extern VkResult vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, ulong memoryOffset);


		[DllImport (libraryName)]
		public static extern void vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, VkMemoryRequirements* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetImageMemoryRequirements(VkDevice device, VkImage image, VkMemoryRequirements* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, uint* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements* pSparseMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlags samples, VkImageUsageFlags usage, VkImageTiling tiling, uint* pPropertyCount, VkSparseImageFormatProperties* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkQueueBindSparse(VkQueue queue, uint bindInfoCount, VkBindSparseInfo* pBindInfo, VkFence fence);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateFence(VkDevice device, VkFenceCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkFence* pFence);


		[DllImport (libraryName)]
		public static extern void vkDestroyFence(VkDevice device, VkFence fence, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkResetFences(VkDevice device, uint fenceCount, VkFence* pFences);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFenceStatus(VkDevice device, VkFence fence);


		[DllImport (libraryName)]
		public static extern VkResult vkWaitForFences(VkDevice device, uint fenceCount, VkFence* pFences, VkBool32 waitAll, ulong timeout);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSemaphore(VkDevice device, VkSemaphoreCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSemaphore* pSemaphore);


		[DllImport (libraryName)]
		public static extern void vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateEvent(VkDevice device, VkEventCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkEvent* pEvent);


		[DllImport (libraryName)]
		public static extern void vkDestroyEvent(VkDevice device, VkEvent vkEvent, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetEventStatus(VkDevice device, VkEvent vkEvent);


		[DllImport (libraryName)]
		public static extern VkResult vkSetEvent(VkDevice device, VkEvent vkEvent);


		[DllImport (libraryName)]
		public static extern VkResult vkResetEvent(VkDevice device, VkEvent vkEvent);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateQueryPool(VkDevice device, VkQueryPoolCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkQueryPool* pQueryPool);


		[DllImport (libraryName)]
		public static extern void vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount, UIntPtr dataSize, void* pData, ulong stride, VkQueryResultFlags flags);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateBuffer(VkDevice device, VkBufferCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkBuffer* pBuffer);


		[DllImport (libraryName)]
		public static extern void vkDestroyBuffer(VkDevice device, VkBuffer buffer, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateBufferView(VkDevice device, VkBufferViewCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkBufferView* pView);


		[DllImport (libraryName)]
		public static extern void vkDestroyBufferView(VkDevice device, VkBufferView bufferView, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateImage(VkDevice device, VkImageCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkImage* pImage);


		[DllImport (libraryName)]
		public static extern void vkDestroyImage(VkDevice device, VkImage image, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkGetImageSubresourceLayout(VkDevice device, VkImage image, VkImageSubresource* pSubresource, VkSubresourceLayout* pLayout);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateImageView(VkDevice device, VkImageViewCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkImageView* pView);


		[DllImport (libraryName)]
		public static extern void vkDestroyImageView(VkDevice device, VkImageView imageView, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateShaderModule(VkDevice device, VkShaderModuleCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkShaderModule* pShaderModule);


		[DllImport (libraryName)]
		public static extern void vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreatePipelineCache(VkDevice device, VkPipelineCacheCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkPipelineCache* pPipelineCache);


		[DllImport (libraryName)]
		public static extern void vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPipelineCacheData(VkDevice device, VkPipelineCache pipelineCache, UIntPtr* pDataSize, void* pData);


		[DllImport (libraryName)]
		public static extern VkResult vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint srcCacheCount, VkPipelineCache* pSrcCaches);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkGraphicsPipelineCreateInfo* pCreateInfos, VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkComputePipelineCreateInfo* pCreateInfos, VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines);


		[DllImport (libraryName)]
		public static extern void vkDestroyPipeline(VkDevice device, VkPipeline pipeline, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreatePipelineLayout(VkDevice device, VkPipelineLayoutCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkPipelineLayout* pPipelineLayout);


		[DllImport (libraryName)]
		public static extern void vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSampler(VkDevice device, VkSamplerCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSampler* pSampler);


		[DllImport (libraryName)]
		public static extern void vkDestroySampler(VkDevice device, VkSampler sampler, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayoutCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDescriptorSetLayout* pSetLayout);


		[DllImport (libraryName)]
		public static extern void vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDescriptorPool(VkDevice device, VkDescriptorPoolCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDescriptorPool* pDescriptorPool);


		[DllImport (libraryName)]
		public static extern void vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkResetDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, uint flags);


		[DllImport (libraryName)]
		public static extern VkResult vkAllocateDescriptorSets(VkDevice device, VkDescriptorSetAllocateInfo* pAllocateInfo, VkDescriptorSet* pDescriptorSets);


		[DllImport (libraryName)]
		public static extern VkResult vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint descriptorSetCount, VkDescriptorSet* pDescriptorSets);


		[DllImport (libraryName)]
		public static extern void vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, VkWriteDescriptorSet* pDescriptorWrites, uint descriptorCopyCount, VkCopyDescriptorSet* pDescriptorCopies);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateFramebuffer(VkDevice device, VkFramebufferCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkFramebuffer* pFramebuffer);


		[DllImport (libraryName)]
		public static extern void vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateRenderPass(VkDevice device, VkRenderPassCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass);


		[DllImport (libraryName)]
		public static extern void vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkGetRenderAreaGranularity(VkDevice device, VkRenderPass renderPass, VkExtent2D* pGranularity);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateCommandPool(VkDevice device, VkCommandPoolCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkCommandPool* pCommandPool);


		[DllImport (libraryName)]
		public static extern void vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkResetCommandPool(VkDevice device, VkCommandPool commandPool, VkCommandPoolResetFlags flags);


		[DllImport (libraryName)]
		public static extern VkResult vkAllocateCommandBuffers(VkDevice device, VkCommandBufferAllocateInfo* pAllocateInfo, VkCommandBuffer* pCommandBuffers);


		[DllImport (libraryName)]
		public static extern void vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint commandBufferCount, VkCommandBuffer* pCommandBuffers);


		[DllImport (libraryName)]
		public static extern VkResult vkBeginCommandBuffer(VkCommandBuffer commandBuffer, VkCommandBufferBeginInfo* pBeginInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkEndCommandBuffer(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern VkResult vkResetCommandBuffer(VkCommandBuffer commandBuffer, VkCommandBufferResetFlags flags);


		[DllImport (libraryName)]
		public static extern void vkCmdBindPipeline(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline);


		[DllImport (libraryName)]
		public static extern void vkCmdSetViewport(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, VkViewport* pViewports);


		[DllImport (libraryName)]
		public static extern void vkCmdSetScissor(VkCommandBuffer commandBuffer, uint firstScissor, uint scissorCount, VkRect2D* pScissors);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLineWidth(VkCommandBuffer commandBuffer, float lineWidth);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthBias(VkCommandBuffer commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);


		[DllImport (libraryName)]
		public static extern void vkCmdSetBlendConstants(VkCommandBuffer commandBuffer, float blendConstants);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthBounds(VkCommandBuffer commandBuffer, float minDepthBounds, float maxDepthBounds);


		[DllImport (libraryName)]
		public static extern void vkCmdSetStencilCompareMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint compareMask);


		[DllImport (libraryName)]
		public static extern void vkCmdSetStencilWriteMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint writeMask);


		[DllImport (libraryName)]
		public static extern void vkCmdSetStencilReference(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint reference);


		[DllImport (libraryName)]
		public static extern void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, VkDescriptorSet* pDescriptorSets, uint dynamicOffsetCount, uint* pDynamicOffsets);


		[DllImport (libraryName)]
		public static extern void vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkIndexType indexType);


		[DllImport (libraryName)]
		public static extern void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, VkBuffer* pBuffers, ulong* pOffsets);


		[DllImport (libraryName)]
		public static extern void vkCmdDraw(VkCommandBuffer commandBuffer, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndexed(VkCommandBuffer commandBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, uint drawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndexedIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, uint drawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatch(VkCommandBuffer commandBuffer, uint groupCountX, uint groupCountY, uint groupCountZ);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatchIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint regionCount, VkBufferCopy* pRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, VkImageCopy* pRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, VkImageBlit* pRegions, VkFilter filter);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, VkBufferImageCopy* pRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyImageToBuffer(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkBuffer dstBuffer, uint regionCount, VkBufferImageCopy* pRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdUpdateBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, ulong dstOffset, ulong dataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdFillBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, ulong dstOffset, ulong size, uint data);


		[DllImport (libraryName)]
		public static extern void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, VkClearColorValue* pColor, uint rangeCount, VkImageSubresourceRange* pRanges);


		[DllImport (libraryName)]
		public static extern void vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, VkClearDepthStencilValue* pDepthStencil, uint rangeCount, VkImageSubresourceRange* pRanges);


		[DllImport (libraryName)]
		public static extern void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, VkClearAttachment* pAttachments, uint rectCount, VkClearRect* pRects);


		[DllImport (libraryName)]
		public static extern void vkCmdResolveImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, VkImageResolve* pRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdSetEvent(VkCommandBuffer commandBuffer, VkEvent vkEvent, VkPipelineStageFlags stageMask);


		[DllImport (libraryName)]
		public static extern void vkCmdResetEvent(VkCommandBuffer commandBuffer, VkEvent vkEvent, VkPipelineStageFlags stageMask);


		[DllImport (libraryName)]
		public static extern void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, VkEvent* pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);


		[DllImport (libraryName)]
		public static extern void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query, VkQueryControlFlags flags);


		[DllImport (libraryName)]
		public static extern void vkCmdEndQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query);


		[DllImport (libraryName)]
		public static extern void vkCmdResetQueryPool(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint firstQuery, uint queryCount);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteTimestamp(VkCommandBuffer commandBuffer, VkPipelineStageFlags pipelineStage, VkQueryPool queryPool, uint query);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyQueryPoolResults(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint firstQuery, uint queryCount, VkBuffer dstBuffer, ulong dstOffset, ulong stride, VkQueryResultFlags flags);


		[DllImport (libraryName)]
		public static extern void vkCmdPushConstants(VkCommandBuffer commandBuffer, VkPipelineLayout layout, VkShaderStageFlags stageFlags, uint offset, uint size, void* pValues);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, VkRenderPassBeginInfo* pRenderPassBegin, VkSubpassContents contents);


		[DllImport (libraryName)]
		public static extern void vkCmdNextSubpass(VkCommandBuffer commandBuffer, VkSubpassContents contents);


		[DllImport (libraryName)]
		public static extern void vkCmdEndRenderPass(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint commandBufferCount, VkCommandBuffer* pCommandBuffers);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumerateInstanceVersion(uint* pApiVersion);


		[DllImport (libraryName)]
		public static extern VkResult vkBindBufferMemory2(VkDevice device, uint bindInfoCount, VkBindBufferMemoryInfo* pBindInfos);


		[DllImport (libraryName)]
		public static extern VkResult vkBindImageMemory2(VkDevice device, uint bindInfoCount, VkBindImageMemoryInfo* pBindInfos);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceGroupPeerMemoryFeatures(VkDevice device, uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, VkPeerMemoryFeatureFlags* pPeerMemoryFeatures);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDeviceMask(VkCommandBuffer commandBuffer, uint deviceMask);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatchBase(VkCommandBuffer commandBuffer, uint baseGroupX, uint baseGroupY, uint baseGroupZ, uint groupCountX, uint groupCountY, uint groupCountZ);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumeratePhysicalDeviceGroups(VkInstance instance, uint* pPhysicalDeviceGroupCount, VkPhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties);


		[DllImport (libraryName)]
		public static extern void vkGetImageMemoryRequirements2(VkDevice device, VkImageMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetBufferMemoryRequirements2(VkDevice device, VkBufferMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetImageSparseMemoryRequirements2(VkDevice device, VkImageSparseMemoryRequirementsInfo2* pInfo, uint* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceFeatures2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures2* pFeatures);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties2* pProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceFormatProperties2(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties2* pFormatProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceImageFormatProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceImageFormatInfo2* pImageFormatInfo, VkImageFormatProperties2* pImageFormatProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDevice physicalDevice, uint* pQueueFamilyPropertyCount, VkQueueFamilyProperties2* pQueueFamilyProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties2* pMemoryProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint* pPropertyCount, VkSparseImageFormatProperties2* pProperties);


		[DllImport (libraryName)]
		public static extern void vkTrimCommandPool(VkDevice device, VkCommandPool commandPool, uint flags);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceQueue2(VkDevice device, VkDeviceQueueInfo2* pQueueInfo, VkQueue* pQueue);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSamplerYcbcrConversion(VkDevice device, VkSamplerYcbcrConversionCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSamplerYcbcrConversion* pYcbcrConversion);


		[DllImport (libraryName)]
		public static extern void vkDestroySamplerYcbcrConversion(VkDevice device, VkSamplerYcbcrConversion ycbcrConversion, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDescriptorUpdateTemplate(VkDevice device, VkDescriptorUpdateTemplateCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDescriptorUpdateTemplate* pDescriptorUpdateTemplate);


		[DllImport (libraryName)]
		public static extern void vkDestroyDescriptorUpdateTemplate(VkDevice device, VkDescriptorUpdateTemplate descriptorUpdateTemplate, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkUpdateDescriptorSetWithTemplate(VkDevice device, VkDescriptorSet descriptorSet, VkDescriptorUpdateTemplate descriptorUpdateTemplate, void* pData);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceExternalBufferProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceExternalBufferInfo* pExternalBufferInfo, VkExternalBufferProperties* pExternalBufferProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceExternalFenceProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceExternalFenceInfo* pExternalFenceInfo, VkExternalFenceProperties* pExternalFenceProperties);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, VkExternalSemaphoreProperties* pExternalSemaphoreProperties);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorSetLayoutSupport(VkDevice device, VkDescriptorSetLayoutCreateInfo* pCreateInfo, VkDescriptorSetLayoutSupport* pSupport);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndirectCount(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndexedIndirectCount(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateRenderPass2(VkDevice device, VkRenderPassCreateInfo2* pCreateInfo, VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginRenderPass2(VkCommandBuffer commandBuffer, VkRenderPassBeginInfo* pRenderPassBegin, VkSubpassBeginInfo* pSubpassBeginInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdNextSubpass2(VkCommandBuffer commandBuffer, VkSubpassBeginInfo* pSubpassBeginInfo, VkSubpassEndInfo* pSubpassEndInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdEndRenderPass2(VkCommandBuffer commandBuffer, VkSubpassEndInfo* pSubpassEndInfo);


		[DllImport (libraryName)]
		public static extern void vkResetQueryPool(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSemaphoreCounterValue(VkDevice device, VkSemaphore semaphore, ulong* pValue);


		[DllImport (libraryName)]
		public static extern VkResult vkWaitSemaphores(VkDevice device, VkSemaphoreWaitInfo* pWaitInfo, ulong timeout);


		[DllImport (libraryName)]
		public static extern VkResult vkSignalSemaphore(VkDevice device, VkSemaphoreSignalInfo* pSignalInfo);


		[DllImport (libraryName)]
		public static extern ulong vkGetBufferDeviceAddress(VkDevice device, VkBufferDeviceAddressInfo* pInfo);


		[DllImport (libraryName)]
		public static extern ulong vkGetBufferOpaqueCaptureAddress(VkDevice device, VkBufferDeviceAddressInfo* pInfo);


		[DllImport (libraryName)]
		public static extern ulong vkGetDeviceMemoryOpaqueCaptureAddress(VkDevice device, VkDeviceMemoryOpaqueCaptureAddressInfo* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceToolProperties(VkPhysicalDevice physicalDevice, uint* pToolCount, VkPhysicalDeviceToolProperties* pToolProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreatePrivateDataSlot(VkDevice device, VkPrivateDataSlotCreateInfo* pCreateInfo, VkAllocationCallbacks* pAllocator, VkPrivateDataSlot* pPrivateDataSlot);


		[DllImport (libraryName)]
		public static extern void vkDestroyPrivateDataSlot(VkDevice device, VkPrivateDataSlot privateDataSlot, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkSetPrivateData(VkDevice device, VkObjectType objectType, ulong objectHandle, VkPrivateDataSlot privateDataSlot, ulong data);


		[DllImport (libraryName)]
		public static extern void vkGetPrivateData(VkDevice device, VkObjectType objectType, ulong objectHandle, VkPrivateDataSlot privateDataSlot, ulong* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdSetEvent2(VkCommandBuffer commandBuffer, VkEvent vkEvent, VkDependencyInfo* pDependencyInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdResetEvent2(VkCommandBuffer commandBuffer, VkEvent vkEvent, ulong stageMask);


		[DllImport (libraryName)]
		public static extern void vkCmdWaitEvents2(VkCommandBuffer commandBuffer, uint eventCount, VkEvent* pEvents, VkDependencyInfo* pDependencyInfos);


		[DllImport (libraryName)]
		public static extern void vkCmdPipelineBarrier2(VkCommandBuffer commandBuffer, VkDependencyInfo* pDependencyInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteTimestamp2(VkCommandBuffer commandBuffer, ulong stage, VkQueryPool queryPool, uint query);


		[DllImport (libraryName)]
		public static extern VkResult vkQueueSubmit2(VkQueue queue, uint submitCount, VkSubmitInfo2* pSubmits, VkFence fence);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyBuffer2(VkCommandBuffer commandBuffer, VkCopyBufferInfo2* pCopyBufferInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyImage2(VkCommandBuffer commandBuffer, VkCopyImageInfo2* pCopyImageInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyBufferToImage2(VkCommandBuffer commandBuffer, VkCopyBufferToImageInfo2* pCopyBufferToImageInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyImageToBuffer2(VkCommandBuffer commandBuffer, VkCopyImageToBufferInfo2* pCopyImageToBufferInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBlitImage2(VkCommandBuffer commandBuffer, VkBlitImageInfo2* pBlitImageInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdResolveImage2(VkCommandBuffer commandBuffer, VkResolveImageInfo2* pResolveImageInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginRendering(VkCommandBuffer commandBuffer, VkRenderingInfo* pRenderingInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdEndRendering(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdSetCullMode(VkCommandBuffer commandBuffer, VkCullModeFlags cullMode);


		[DllImport (libraryName)]
		public static extern void vkCmdSetFrontFace(VkCommandBuffer commandBuffer, VkFrontFace frontFace);


		[DllImport (libraryName)]
		public static extern void vkCmdSetPrimitiveTopology(VkCommandBuffer commandBuffer, VkPrimitiveTopology primitiveTopology);


		[DllImport (libraryName)]
		public static extern void vkCmdSetViewportWithCount(VkCommandBuffer commandBuffer, uint viewportCount, VkViewport* pViewports);


		[DllImport (libraryName)]
		public static extern void vkCmdSetScissorWithCount(VkCommandBuffer commandBuffer, uint scissorCount, VkRect2D* pScissors);


		[DllImport (libraryName)]
		public static extern void vkCmdBindVertexBuffers2(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, VkBuffer* pBuffers, ulong* pOffsets, ulong* pSizes, ulong* pStrides);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthTestEnable(VkCommandBuffer commandBuffer, VkBool32 depthTestEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthWriteEnable(VkCommandBuffer commandBuffer, VkBool32 depthWriteEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthCompareOp(VkCommandBuffer commandBuffer, VkCompareOp depthCompareOp);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthBoundsTestEnable(VkCommandBuffer commandBuffer, VkBool32 depthBoundsTestEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetStencilTestEnable(VkCommandBuffer commandBuffer, VkBool32 stencilTestEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetStencilOp(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, VkStencilOp failOp, VkStencilOp passOp, VkStencilOp depthFailOp, VkCompareOp compareOp);


		[DllImport (libraryName)]
		public static extern void vkCmdSetRasterizerDiscardEnable(VkCommandBuffer commandBuffer, VkBool32 rasterizerDiscardEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthBiasEnable(VkCommandBuffer commandBuffer, VkBool32 depthBiasEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetPrimitiveRestartEnable(VkCommandBuffer commandBuffer, VkBool32 primitiveRestartEnable);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceBufferMemoryRequirements(VkDevice device, VkDeviceBufferMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceImageMemoryRequirements(VkDevice device, VkDeviceImageMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceImageSparseMemoryRequirements(VkDevice device, VkDeviceImageMemoryRequirements* pInfo, uint* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkGetCommandPoolMemoryConsumption(VkDevice device, VkCommandPool commandPool, VkCommandBuffer commandBuffer, VkCommandPoolMemoryConsumption* pConsumption);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFaultData(VkDevice device, VkFaultQueryBehavior faultQueryBehavior, VkBool32* pUnrecordedFaults, uint* pFaultCount, VkFaultData* pFaults);


		[DllImport (libraryName)]
		public static extern void vkDestroySurfaceKHR(VkInstance instance, VkSurfaceKHR surface, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, VkSurfaceKHR surface, VkBool32* pSupported);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR* pSurfaceCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint* pSurfaceFormatCount, VkSurfaceFormatKHR* pSurfaceFormats);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint* pPresentModeCount, VkPresentModeKHR* pPresentModes);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSwapchainKHR(VkDevice device, VkSwapchainCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSwapchainKHR* pSwapchain);


		[DllImport (libraryName)]
		public static extern void vkDestroySwapchainKHR(VkDevice device, VkSwapchainKHR swapchain, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSwapchainImagesKHR(VkDevice device, VkSwapchainKHR swapchain, uint* pSwapchainImageCount, VkImage* pSwapchainImages);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireNextImageKHR(VkDevice device, VkSwapchainKHR swapchain, ulong timeout, VkSemaphore semaphore, VkFence fence, uint* pImageIndex);


		[DllImport (libraryName)]
		public static extern VkResult vkQueuePresentKHR(VkQueue queue, VkPresentInfoKHR* pPresentInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceDisplayPropertiesKHR(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkDisplayPropertiesKHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkDisplayPlanePropertiesKHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDisplayPlaneSupportedDisplaysKHR(VkPhysicalDevice physicalDevice, uint planeIndex, uint* pDisplayCount, VkDisplayKHR* pDisplays);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDisplayModePropertiesKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, uint* pPropertyCount, VkDisplayModePropertiesKHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDisplayModeKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, VkDisplayModeCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDisplayModeKHR* pMode);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDisplayPlaneCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkDisplayModeKHR mode, uint planeIndex, VkDisplayPlaneCapabilitiesKHR* pCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDisplayPlaneSurfaceKHR(VkInstance instance, VkDisplaySurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, VkSwapchainCreateInfoKHR* pCreateInfos, VkAllocationCallbacks* pAllocator, VkSwapchainKHR* pSwapchains);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateXlibSurfaceKHR(VkInstance instance, VkXlibSurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceXlibPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr dpy, IntPtr visualID);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateXcbSurfaceKHR(VkInstance instance, VkXcbSurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceXcbPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr connection, IntPtr visual_id);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateWaylandSurfaceKHR(VkInstance instance, VkWaylandSurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceWaylandPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr display);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateAndroidSurfaceKHR(VkInstance instance, VkAndroidSurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateWin32SurfaceKHR(VkInstance instance, VkWin32SurfaceCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceWin32PresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDebugReportCallbackEXT* pCallback);


		[DllImport (libraryName)]
		public static extern void vkDestroyDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackEXT callback, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkDebugReportMessageEXT(VkInstance instance, VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, ulong vkObject, UIntPtr location, int messageCode, byte* pLayerPrefix, byte* pMessage);


		[DllImport (libraryName)]
		public static extern VkResult vkDebugMarkerSetObjectTagEXT(VkDevice device, VkDebugMarkerObjectTagInfoEXT* pTagInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkDebugMarkerSetObjectNameEXT(VkDevice device, VkDebugMarkerObjectNameInfoEXT* pNameInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDebugMarkerBeginEXT(VkCommandBuffer commandBuffer, VkDebugMarkerMarkerInfoEXT* pMarkerInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDebugMarkerEndEXT(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdDebugMarkerInsertEXT(VkCommandBuffer commandBuffer, VkDebugMarkerMarkerInfoEXT* pMarkerInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceVideoCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkVideoProfileInfoKHR* pVideoProfile, VkVideoCapabilitiesKHR* pCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceVideoFormatPropertiesKHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceVideoFormatInfoKHR* pVideoFormatInfo, uint* pVideoFormatPropertyCount, VkVideoFormatPropertiesKHR* pVideoFormatProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateVideoSessionKHR(VkDevice device, VkVideoSessionCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkVideoSessionKHR* pVideoSession);


		[DllImport (libraryName)]
		public static extern void vkDestroyVideoSessionKHR(VkDevice device, VkVideoSessionKHR videoSession, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetVideoSessionMemoryRequirementsKHR(VkDevice device, VkVideoSessionKHR videoSession, uint* pMemoryRequirementsCount, VkVideoSessionMemoryRequirementsKHR* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern VkResult vkBindVideoSessionMemoryKHR(VkDevice device, VkVideoSessionKHR videoSession, uint bindSessionMemoryInfoCount, VkBindVideoSessionMemoryInfoKHR* pBindSessionMemoryInfos);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateVideoSessionParametersKHR(VkDevice device, VkVideoSessionParametersCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkVideoSessionParametersKHR* pVideoSessionParameters);


		[DllImport (libraryName)]
		public static extern VkResult vkUpdateVideoSessionParametersKHR(VkDevice device, VkVideoSessionParametersKHR videoSessionParameters, VkVideoSessionParametersUpdateInfoKHR* pUpdateInfo);


		[DllImport (libraryName)]
		public static extern void vkDestroyVideoSessionParametersKHR(VkDevice device, VkVideoSessionParametersKHR videoSessionParameters, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginVideoCodingKHR(VkCommandBuffer commandBuffer, VkVideoBeginCodingInfoKHR* pBeginInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdEndVideoCodingKHR(VkCommandBuffer commandBuffer, VkVideoEndCodingInfoKHR* pEndCodingInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdControlVideoCodingKHR(VkCommandBuffer commandBuffer, VkVideoCodingControlInfoKHR* pCodingControlInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDecodeVideoKHR(VkCommandBuffer commandBuffer, VkVideoDecodeInfoKHR* pDecodeInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBindTransformFeedbackBuffersEXT(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, VkBuffer* pBuffers, ulong* pOffsets, ulong* pSizes);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginTransformFeedbackEXT(VkCommandBuffer commandBuffer, uint firstCounterBuffer, uint counterBufferCount, VkBuffer* pCounterBuffers, ulong* pCounterBufferOffsets);


		[DllImport (libraryName)]
		public static extern void vkCmdEndTransformFeedbackEXT(VkCommandBuffer commandBuffer, uint firstCounterBuffer, uint counterBufferCount, VkBuffer* pCounterBuffers, ulong* pCounterBufferOffsets);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginQueryIndexedEXT(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query, VkQueryControlFlags flags, uint index);


		[DllImport (libraryName)]
		public static extern void vkCmdEndQueryIndexedEXT(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query, uint index);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawIndirectByteCountEXT(VkCommandBuffer commandBuffer, uint instanceCount, uint firstInstance, VkBuffer counterBuffer, ulong counterBufferOffset, uint counterOffset, uint vertexStride);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateCuModuleNVX(VkDevice device, VkCuModuleCreateInfoNVX* pCreateInfo, VkAllocationCallbacks* pAllocator, VkCuModuleNVX* pModule);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateCuFunctionNVX(VkDevice device, VkCuFunctionCreateInfoNVX* pCreateInfo, VkAllocationCallbacks* pAllocator, VkCuFunctionNVX* pFunction);


		[DllImport (libraryName)]
		public static extern void vkDestroyCuModuleNVX(VkDevice device, VkCuModuleNVX module, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkDestroyCuFunctionNVX(VkDevice device, VkCuFunctionNVX function, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdCuLaunchKernelNVX(VkCommandBuffer commandBuffer, VkCuLaunchInfoNVX* pLaunchInfo);


		[DllImport (libraryName)]
		public static extern uint vkGetImageViewHandleNVX(VkDevice device, VkImageViewHandleInfoNVX* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetImageViewAddressNVX(VkDevice device, VkImageView imageView, VkImageViewAddressPropertiesNVX* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetShaderInfoAMD(VkDevice device, VkPipeline pipeline, VkShaderStageFlags shaderStage, VkShaderInfoTypeAMD infoType, UIntPtr* pInfoSize, void* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateStreamDescriptorSurfaceGGP(VkInstance instance, VkStreamDescriptorSurfaceCreateInfoGGP* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceExternalImageFormatPropertiesNV(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkExternalMemoryHandleTypeFlagsNV externalHandleType, VkExternalImageFormatPropertiesNV* pExternalImageFormatProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryWin32HandleNV(VkDevice device, VkDeviceMemory memory, VkExternalMemoryHandleTypeFlagsNV handleType, IntPtr pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateViSurfaceNN(VkInstance instance, VkViSurfaceCreateInfoNN* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryWin32HandleKHR(VkDevice device, VkMemoryGetWin32HandleInfoKHR* pGetWin32HandleInfo, IntPtr pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryWin32HandlePropertiesKHR(VkDevice device, VkExternalMemoryHandleTypeFlags handleType, IntPtr handle, VkMemoryWin32HandlePropertiesKHR* pMemoryWin32HandleProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryFdKHR(VkDevice device, VkMemoryGetFdInfoKHR* pGetFdInfo, int* pFd);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryFdPropertiesKHR(VkDevice device, VkExternalMemoryHandleTypeFlags handleType, int fd, VkMemoryFdPropertiesKHR* pMemoryFdProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkImportSemaphoreWin32HandleKHR(VkDevice device, VkImportSemaphoreWin32HandleInfoKHR* pImportSemaphoreWin32HandleInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSemaphoreWin32HandleKHR(VkDevice device, VkSemaphoreGetWin32HandleInfoKHR* pGetWin32HandleInfo, IntPtr pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkImportSemaphoreFdKHR(VkDevice device, VkImportSemaphoreFdInfoKHR* pImportSemaphoreFdInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSemaphoreFdKHR(VkDevice device, VkSemaphoreGetFdInfoKHR* pGetFdInfo, int* pFd);


		[DllImport (libraryName)]
		public static extern void vkCmdPushDescriptorSetKHR(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint set, uint descriptorWriteCount, VkWriteDescriptorSet* pDescriptorWrites);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginConditionalRenderingEXT(VkCommandBuffer commandBuffer, VkConditionalRenderingBeginInfoEXT* pConditionalRenderingBegin);


		[DllImport (libraryName)]
		public static extern void vkCmdEndConditionalRenderingEXT(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdSetViewportWScalingNV(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, VkViewportWScalingNV* pViewportWScalings);


		[DllImport (libraryName)]
		public static extern VkResult vkReleaseDisplayEXT(VkPhysicalDevice physicalDevice, VkDisplayKHR display);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireXlibDisplayEXT(VkPhysicalDevice physicalDevice, IntPtr dpy, VkDisplayKHR display);


		[DllImport (libraryName)]
		public static extern VkResult vkGetRandROutputDisplayEXT(VkPhysicalDevice physicalDevice, IntPtr dpy, IntPtr rrOutput, VkDisplayKHR* pDisplay);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceCapabilities2EXT(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilities2EXT* pSurfaceCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkDisplayPowerControlEXT(VkDevice device, VkDisplayKHR display, VkDisplayPowerInfoEXT* pDisplayPowerInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkRegisterDeviceEventEXT(VkDevice device, VkDeviceEventInfoEXT* pDeviceEventInfo, VkAllocationCallbacks* pAllocator, VkFence* pFence);


		[DllImport (libraryName)]
		public static extern VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, VkDisplayEventInfoEXT* pDisplayEventInfo, VkAllocationCallbacks* pAllocator, VkFence* pFence);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSwapchainCounterEXT(VkDevice device, VkSwapchainKHR swapchain, VkSurfaceCounterFlagsEXT counter, ulong* pCounterValue);


		[DllImport (libraryName)]
		public static extern VkResult vkGetRefreshCycleDurationGOOGLE(VkDevice device, VkSwapchainKHR swapchain, VkRefreshCycleDurationGOOGLE* pDisplayTimingProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPastPresentationTimingGOOGLE(VkDevice device, VkSwapchainKHR swapchain, uint* pPresentationTimingCount, VkPastPresentationTimingGOOGLE* pPresentationTimings);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDiscardRectangleEXT(VkCommandBuffer commandBuffer, uint firstDiscardRectangle, uint discardRectangleCount, VkRect2D* pDiscardRectangles);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDiscardRectangleEnableEXT(VkCommandBuffer commandBuffer, VkBool32 discardRectangleEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDiscardRectangleModeEXT(VkCommandBuffer commandBuffer, VkDiscardRectangleModeEXT discardRectangleMode);


		[DllImport (libraryName)]
		public static extern void vkSetHdrMetadataEXT(VkDevice device, uint swapchainCount, VkSwapchainKHR* pSwapchains, VkHdrMetadataEXT* pMetadata);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSwapchainStatusKHR(VkDevice device, VkSwapchainKHR swapchain);


		[DllImport (libraryName)]
		public static extern VkResult vkImportFenceWin32HandleKHR(VkDevice device, VkImportFenceWin32HandleInfoKHR* pImportFenceWin32HandleInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFenceWin32HandleKHR(VkDevice device, VkFenceGetWin32HandleInfoKHR* pGetWin32HandleInfo, IntPtr pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkImportFenceFdKHR(VkDevice device, VkImportFenceFdInfoKHR* pImportFenceFdInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFenceFdKHR(VkDevice device, VkFenceGetFdInfoKHR* pGetFdInfo, int* pFd);


		[DllImport (libraryName)]
		public static extern VkResult vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, uint* pCounterCount, VkPerformanceCounterKHR* pCounters, VkPerformanceCounterDescriptionKHR* pCounterDescriptions);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR(VkPhysicalDevice physicalDevice, VkQueryPoolPerformanceCreateInfoKHR* pPerformanceQueryCreateInfo, uint* pNumPasses);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireProfilingLockKHR(VkDevice device, VkAcquireProfilingLockInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern void vkReleaseProfilingLockKHR(VkDevice device);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, VkSurfaceCapabilities2KHR* pSurfaceCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, uint* pSurfaceFormatCount, VkSurfaceFormat2KHR* pSurfaceFormats);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceDisplayProperties2KHR(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkDisplayProperties2KHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceDisplayPlaneProperties2KHR(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkDisplayPlaneProperties2KHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDisplayModeProperties2KHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, uint* pPropertyCount, VkDisplayModeProperties2KHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDisplayPlaneCapabilities2KHR(VkPhysicalDevice physicalDevice, VkDisplayPlaneInfo2KHR* pDisplayPlaneInfo, VkDisplayPlaneCapabilities2KHR* pCapabilities);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateIOSSurfaceMVK(VkInstance instance, VkIOSSurfaceCreateInfoMVK* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateMacOSSurfaceMVK(VkInstance instance, VkMacOSSurfaceCreateInfoMVK* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkSetDebugUtilsObjectNameEXT(VkDevice device, VkDebugUtilsObjectNameInfoEXT* pNameInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkSetDebugUtilsObjectTagEXT(VkDevice device, VkDebugUtilsObjectTagInfoEXT* pTagInfo);


		[DllImport (libraryName)]
		public static extern void vkQueueBeginDebugUtilsLabelEXT(VkQueue queue, VkDebugUtilsLabelEXT* pLabelInfo);


		[DllImport (libraryName)]
		public static extern void vkQueueEndDebugUtilsLabelEXT(VkQueue queue);


		[DllImport (libraryName)]
		public static extern void vkQueueInsertDebugUtilsLabelEXT(VkQueue queue, VkDebugUtilsLabelEXT* pLabelInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBeginDebugUtilsLabelEXT(VkCommandBuffer commandBuffer, VkDebugUtilsLabelEXT* pLabelInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdEndDebugUtilsLabelEXT(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdInsertDebugUtilsLabelEXT(VkCommandBuffer commandBuffer, VkDebugUtilsLabelEXT* pLabelInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDebugUtilsMessengerEXT(VkInstance instance, VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pMessenger);


		[DllImport (libraryName)]
		public static extern void vkDestroyDebugUtilsMessengerEXT(VkInstance instance, VkDebugUtilsMessengerEXT messenger, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkSubmitDebugUtilsMessageEXT(VkInstance instance, VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData);


		[DllImport (libraryName)]
		public static extern VkResult vkGetAndroidHardwareBufferPropertiesANDROID(VkDevice device, IntPtr buffer, VkAndroidHardwareBufferPropertiesANDROID* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryAndroidHardwareBufferANDROID(VkDevice device, VkMemoryGetAndroidHardwareBufferInfoANDROID* pInfo, IntPtr pBuffer);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateExecutionGraphPipelinesAMDX(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkExecutionGraphPipelineCreateInfoAMDX* pCreateInfos, VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines);


		[DllImport (libraryName)]
		public static extern VkResult vkGetExecutionGraphPipelineScratchSizeAMDX(VkDevice device, VkPipeline executionGraph, VkExecutionGraphPipelineScratchSizeAMDX* pSizeInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetExecutionGraphPipelineNodeIndexAMDX(VkDevice device, VkPipeline executionGraph, VkPipelineShaderStageNodeCreateInfoAMDX* pNodeInfo, uint* pNodeIndex);


		[DllImport (libraryName)]
		public static extern void vkCmdInitializeGraphScratchMemoryAMDX(VkCommandBuffer commandBuffer, ulong scratch);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatchGraphAMDX(VkCommandBuffer commandBuffer, ulong scratch, VkDispatchGraphCountInfoAMDX* pCountInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatchGraphIndirectAMDX(VkCommandBuffer commandBuffer, ulong scratch, VkDispatchGraphCountInfoAMDX* pCountInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDispatchGraphIndirectCountAMDX(VkCommandBuffer commandBuffer, ulong scratch, ulong countInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdSetSampleLocationsEXT(VkCommandBuffer commandBuffer, VkSampleLocationsInfoEXT* pSampleLocationsInfo);


		[DllImport (libraryName)]
		public static extern void vkGetPhysicalDeviceMultisamplePropertiesEXT(VkPhysicalDevice physicalDevice, VkSampleCountFlags samples, VkMultisamplePropertiesEXT* pMultisampleProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateAccelerationStructureKHR(VkDevice device, VkAccelerationStructureCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VkAccelerationStructureKHR* pAccelerationStructure);


		[DllImport (libraryName)]
		public static extern void vkDestroyAccelerationStructureKHR(VkDevice device, VkAccelerationStructureKHR accelerationStructure, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdBuildAccelerationStructuresKHR(VkCommandBuffer commandBuffer, uint infoCount, VkAccelerationStructureBuildGeometryInfoKHR* pInfos, VkAccelerationStructureBuildRangeInfoKHR** ppBuildRangeInfos);


		[DllImport (libraryName)]
		public static extern void vkCmdBuildAccelerationStructuresIndirectKHR(VkCommandBuffer commandBuffer, uint infoCount, VkAccelerationStructureBuildGeometryInfoKHR* pInfos, ulong* pIndirectDeviceAddresses, uint* pIndirectStrides, uint** ppMaxPrimitiveCounts);


		[DllImport (libraryName)]
		public static extern VkResult vkBuildAccelerationStructuresKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, uint infoCount, VkAccelerationStructureBuildGeometryInfoKHR* pInfos, VkAccelerationStructureBuildRangeInfoKHR** ppBuildRangeInfos);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyAccelerationStructureKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyAccelerationStructureInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyAccelerationStructureToMemoryKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyAccelerationStructureToMemoryInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyMemoryToAccelerationStructureKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyMemoryToAccelerationStructureInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkWriteAccelerationStructuresPropertiesKHR(VkDevice device, uint accelerationStructureCount, VkAccelerationStructureKHR* pAccelerationStructures, VkQueryType queryType, UIntPtr dataSize, void* pData, UIntPtr stride);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyAccelerationStructureKHR(VkCommandBuffer commandBuffer, VkCopyAccelerationStructureInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyAccelerationStructureToMemoryKHR(VkCommandBuffer commandBuffer, VkCopyAccelerationStructureToMemoryInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMemoryToAccelerationStructureKHR(VkCommandBuffer commandBuffer, VkCopyMemoryToAccelerationStructureInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern ulong vkGetAccelerationStructureDeviceAddressKHR(VkDevice device, VkAccelerationStructureDeviceAddressInfoKHR* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteAccelerationStructuresPropertiesKHR(VkCommandBuffer commandBuffer, uint accelerationStructureCount, VkAccelerationStructureKHR* pAccelerationStructures, VkQueryType queryType, VkQueryPool queryPool, uint firstQuery);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceAccelerationStructureCompatibilityKHR(VkDevice device, VkAccelerationStructureVersionInfoKHR* pVersionInfo, VkAccelerationStructureCompatibilityKHR* pCompatibility);


		[DllImport (libraryName)]
		public static extern void vkGetAccelerationStructureBuildSizesKHR(VkDevice device, VkAccelerationStructureBuildTypeKHR buildType, VkAccelerationStructureBuildGeometryInfoKHR* pBuildInfo, uint* pMaxPrimitiveCounts, VkAccelerationStructureBuildSizesInfoKHR* pSizeInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdTraceRaysKHR(VkCommandBuffer commandBuffer, VkStridedDeviceAddressRegionKHR* pRaygenShaderBindingTable, VkStridedDeviceAddressRegionKHR* pMissShaderBindingTable, VkStridedDeviceAddressRegionKHR* pHitShaderBindingTable, VkStridedDeviceAddressRegionKHR* pCallableShaderBindingTable, uint width, uint height, uint depth);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateRayTracingPipelinesKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, VkPipelineCache pipelineCache, uint createInfoCount, VkRayTracingPipelineCreateInfoKHR* pCreateInfos, VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines);


		[DllImport (libraryName)]
		public static extern VkResult vkGetRayTracingShaderGroupHandlesKHR(VkDevice device, VkPipeline pipeline, uint firstGroup, uint groupCount, UIntPtr dataSize, void* pData);


		[DllImport (libraryName)]
		public static extern VkResult vkGetRayTracingCaptureReplayShaderGroupHandlesKHR(VkDevice device, VkPipeline pipeline, uint firstGroup, uint groupCount, UIntPtr dataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdTraceRaysIndirectKHR(VkCommandBuffer commandBuffer, VkStridedDeviceAddressRegionKHR* pRaygenShaderBindingTable, VkStridedDeviceAddressRegionKHR* pMissShaderBindingTable, VkStridedDeviceAddressRegionKHR* pHitShaderBindingTable, VkStridedDeviceAddressRegionKHR* pCallableShaderBindingTable, ulong indirectDeviceAddress);


		[DllImport (libraryName)]
		public static extern ulong vkGetRayTracingShaderGroupStackSizeKHR(VkDevice device, VkPipeline pipeline, uint group, VkShaderGroupShaderKHR groupShader);


		[DllImport (libraryName)]
		public static extern void vkCmdSetRayTracingPipelineStackSizeKHR(VkCommandBuffer commandBuffer, uint pipelineStackSize);


		[DllImport (libraryName)]
		public static extern VkResult vkGetImageDrmFormatModifierPropertiesEXT(VkDevice device, VkImage image, VkImageDrmFormatModifierPropertiesEXT* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateValidationCacheEXT(VkDevice device, VkValidationCacheCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkValidationCacheEXT* pValidationCache);


		[DllImport (libraryName)]
		public static extern void vkDestroyValidationCacheEXT(VkDevice device, VkValidationCacheEXT validationCache, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkMergeValidationCachesEXT(VkDevice device, VkValidationCacheEXT dstCache, uint srcCacheCount, VkValidationCacheEXT* pSrcCaches);


		[DllImport (libraryName)]
		public static extern VkResult vkGetValidationCacheDataEXT(VkDevice device, VkValidationCacheEXT validationCache, UIntPtr* pDataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdBindShadingRateImageNV(VkCommandBuffer commandBuffer, VkImageView imageView, VkImageLayout imageLayout);


		[DllImport (libraryName)]
		public static extern void vkCmdSetViewportShadingRatePaletteNV(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, VkShadingRatePaletteNV* pShadingRatePalettes);


		[DllImport (libraryName)]
		public static extern void vkCmdSetCoarseSampleOrderNV(VkCommandBuffer commandBuffer, VkCoarseSampleOrderTypeNV sampleOrderType, uint customSampleOrderCount, VkCoarseSampleOrderCustomNV* pCustomSampleOrders);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateAccelerationStructureNV(VkDevice device, VkAccelerationStructureCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkAccelerationStructureNV* pAccelerationStructure);


		[DllImport (libraryName)]
		public static extern void vkDestroyAccelerationStructureNV(VkDevice device, VkAccelerationStructureNV accelerationStructure, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkGetAccelerationStructureMemoryRequirementsNV(VkDevice device, VkAccelerationStructureMemoryRequirementsInfoNV* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern VkResult vkBindAccelerationStructureMemoryNV(VkDevice device, uint bindInfoCount, VkBindAccelerationStructureMemoryInfoNV* pBindInfos);


		[DllImport (libraryName)]
		public static extern void vkCmdBuildAccelerationStructureNV(VkCommandBuffer commandBuffer, VkAccelerationStructureInfoNV* pInfo, VkBuffer instanceData, ulong instanceOffset, VkBool32 update, VkAccelerationStructureNV dst, VkAccelerationStructureNV src, VkBuffer scratch, ulong scratchOffset);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyAccelerationStructureNV(VkCommandBuffer commandBuffer, VkAccelerationStructureNV dst, VkAccelerationStructureNV src, VkCopyAccelerationStructureModeKHR mode);


		[DllImport (libraryName)]
		public static extern void vkCmdTraceRaysNV(VkCommandBuffer commandBuffer, VkBuffer raygenShaderBindingTableBuffer, ulong raygenShaderBindingOffset, VkBuffer missShaderBindingTableBuffer, ulong missShaderBindingOffset, ulong missShaderBindingStride, VkBuffer hitShaderBindingTableBuffer, ulong hitShaderBindingOffset, ulong hitShaderBindingStride, VkBuffer callableShaderBindingTableBuffer, ulong callableShaderBindingOffset, ulong callableShaderBindingStride, uint width, uint height, uint depth);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateRayTracingPipelinesNV(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkRayTracingPipelineCreateInfoNV* pCreateInfos, VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines);


		[DllImport (libraryName)]
		public static extern VkResult vkGetAccelerationStructureHandleNV(VkDevice device, VkAccelerationStructureNV accelerationStructure, UIntPtr dataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteAccelerationStructuresPropertiesNV(VkCommandBuffer commandBuffer, uint accelerationStructureCount, VkAccelerationStructureNV* pAccelerationStructures, VkQueryType queryType, VkQueryPool queryPool, uint firstQuery);


		[DllImport (libraryName)]
		public static extern VkResult vkCompileDeferredNV(VkDevice device, VkPipeline pipeline, uint shader);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryHostPointerPropertiesEXT(VkDevice device, VkExternalMemoryHandleTypeFlags handleType, void* pHostPointer, VkMemoryHostPointerPropertiesEXT* pMemoryHostPointerProperties);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteBufferMarkerAMD(VkCommandBuffer commandBuffer, VkPipelineStageFlags pipelineStage, VkBuffer dstBuffer, ulong dstOffset, uint marker);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceCalibrateableTimeDomainsKHR(VkPhysicalDevice physicalDevice, uint* pTimeDomainCount, VkTimeDomainKHR* pTimeDomains);


		[DllImport (libraryName)]
		public static extern VkResult vkGetCalibratedTimestampsKHR(VkDevice device, uint timestampCount, VkCalibratedTimestampInfoKHR* pTimestampInfos, ulong* pTimestamps, ulong* pMaxDeviation);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksNV(VkCommandBuffer commandBuffer, uint taskCount, uint firstTask);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksIndirectNV(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, uint drawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksIndirectCountNV(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdSetExclusiveScissorEnableNV(VkCommandBuffer commandBuffer, uint firstExclusiveScissor, uint exclusiveScissorCount, VkBool32* pExclusiveScissorEnables);


		[DllImport (libraryName)]
		public static extern void vkCmdSetExclusiveScissorNV(VkCommandBuffer commandBuffer, uint firstExclusiveScissor, uint exclusiveScissorCount, VkRect2D* pExclusiveScissors);


		[DllImport (libraryName)]
		public static extern void vkCmdSetCheckpointNV(VkCommandBuffer commandBuffer, void* pCheckpointMarker);


		[DllImport (libraryName)]
		public static extern void vkGetQueueCheckpointDataNV(VkQueue queue, uint* pCheckpointDataCount, VkCheckpointDataNV* pCheckpointData);


		[DllImport (libraryName)]
		public static extern VkResult vkInitializePerformanceApiINTEL(VkDevice device, VkInitializePerformanceApiInfoINTEL* pInitializeInfo);


		[DllImport (libraryName)]
		public static extern void vkUninitializePerformanceApiINTEL(VkDevice device);


		[DllImport (libraryName)]
		public static extern VkResult vkCmdSetPerformanceMarkerINTEL(VkCommandBuffer commandBuffer, VkPerformanceMarkerInfoINTEL* pMarkerInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCmdSetPerformanceStreamMarkerINTEL(VkCommandBuffer commandBuffer, VkPerformanceStreamMarkerInfoINTEL* pMarkerInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCmdSetPerformanceOverrideINTEL(VkCommandBuffer commandBuffer, VkPerformanceOverrideInfoINTEL* pOverrideInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquirePerformanceConfigurationINTEL(VkDevice device, VkPerformanceConfigurationAcquireInfoINTEL* pAcquireInfo, VkPerformanceConfigurationINTEL* pConfiguration);


		[DllImport (libraryName)]
		public static extern VkResult vkReleasePerformanceConfigurationINTEL(VkDevice device, VkPerformanceConfigurationINTEL configuration);


		[DllImport (libraryName)]
		public static extern VkResult vkQueueSetPerformanceConfigurationINTEL(VkQueue queue, VkPerformanceConfigurationINTEL configuration);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPerformanceParameterINTEL(VkDevice device, VkPerformanceParameterTypeINTEL parameter, VkPerformanceValueINTEL* pValue);


		[DllImport (libraryName)]
		public static extern void vkSetLocalDimmingAMD(VkDevice device, VkSwapchainKHR swapChain, VkBool32 localDimmingEnable);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateImagePipeSurfaceFUCHSIA(VkInstance instance, VkImagePipeSurfaceCreateInfoFUCHSIA* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateMetalSurfaceEXT(VkInstance instance, VkMetalSurfaceCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceFragmentShadingRatesKHR(VkPhysicalDevice physicalDevice, uint* pFragmentShadingRateCount, VkPhysicalDeviceFragmentShadingRateKHR* pFragmentShadingRates);


		[DllImport (libraryName)]
		public static extern void vkCmdSetFragmentShadingRateKHR(VkCommandBuffer commandBuffer, VkExtent2D* pFragmentSize, VkFragmentShadingRateCombinerOpKHR combinerOps);


		[DllImport (libraryName)]
		public static extern VkResult vkWaitForPresentKHR(VkDevice device, VkSwapchainKHR swapchain, ulong presentId, ulong timeout);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceCooperativeMatrixPropertiesNV(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkCooperativeMatrixPropertiesNV* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV(VkPhysicalDevice physicalDevice, uint* pCombinationCount, VkFramebufferMixedSamplesCombinationNV* pCombinations);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSurfacePresentModes2EXT(VkPhysicalDevice physicalDevice, VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, uint* pPresentModeCount, VkPresentModeKHR* pPresentModes);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireFullScreenExclusiveModeEXT(VkDevice device, VkSwapchainKHR swapchain);


		[DllImport (libraryName)]
		public static extern VkResult vkReleaseFullScreenExclusiveModeEXT(VkDevice device, VkSwapchainKHR swapchain);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateHeadlessSurfaceEXT(VkInstance instance, VkHeadlessSurfaceCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLineStippleEXT(VkCommandBuffer commandBuffer, uint lineStippleFactor, ushort lineStipplePattern);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDeferredOperationKHR(VkDevice device, VkAllocationCallbacks* pAllocator, VkDeferredOperationKHR* pDeferredOperation);


		[DllImport (libraryName)]
		public static extern void vkDestroyDeferredOperationKHR(VkDevice device, VkDeferredOperationKHR operation, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern uint vkGetDeferredOperationMaxConcurrencyKHR(VkDevice device, VkDeferredOperationKHR operation);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDeferredOperationResultKHR(VkDevice device, VkDeferredOperationKHR operation);


		[DllImport (libraryName)]
		public static extern VkResult vkDeferredOperationJoinKHR(VkDevice device, VkDeferredOperationKHR operation);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPipelineExecutablePropertiesKHR(VkDevice device, VkPipelineInfoKHR* pPipelineInfo, uint* pExecutableCount, VkPipelineExecutablePropertiesKHR* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPipelineExecutableStatisticsKHR(VkDevice device, VkPipelineExecutableInfoKHR* pExecutableInfo, uint* pStatisticCount, VkPipelineExecutableStatisticKHR* pStatistics);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPipelineExecutableInternalRepresentationsKHR(VkDevice device, VkPipelineExecutableInfoKHR* pExecutableInfo, uint* pInternalRepresentationCount, VkPipelineExecutableInternalRepresentationKHR* pInternalRepresentations);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyMemoryToImageEXT(VkDevice device, VkCopyMemoryToImageInfoEXT* pCopyMemoryToImageInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyImageToMemoryEXT(VkDevice device, VkCopyImageToMemoryInfoEXT* pCopyImageToMemoryInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyImageToImageEXT(VkDevice device, VkCopyImageToImageInfoEXT* pCopyImageToImageInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkTransitionImageLayoutEXT(VkDevice device, uint transitionCount, VkHostImageLayoutTransitionInfoEXT* pTransitions);


		[DllImport (libraryName)]
		public static extern void vkGetImageSubresourceLayout2KHR(VkDevice device, VkImage image, VkImageSubresource2KHR* pSubresource, VkSubresourceLayout2KHR* pLayout);


		[DllImport (libraryName)]
		public static extern VkResult vkMapMemory2KHR(VkDevice device, VkMemoryMapInfoKHR* pMemoryMapInfo, void** ppData);


		[DllImport (libraryName)]
		public static extern VkResult vkUnmapMemory2KHR(VkDevice device, VkMemoryUnmapInfoKHR* pMemoryUnmapInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkReleaseSwapchainImagesEXT(VkDevice device, VkReleaseSwapchainImagesInfoEXT* pReleaseInfo);


		[DllImport (libraryName)]
		public static extern void vkGetGeneratedCommandsMemoryRequirementsNV(VkDevice device, VkGeneratedCommandsMemoryRequirementsInfoNV* pInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkCmdPreprocessGeneratedCommandsNV(VkCommandBuffer commandBuffer, VkGeneratedCommandsInfoNV* pGeneratedCommandsInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdExecuteGeneratedCommandsNV(VkCommandBuffer commandBuffer, VkBool32 isPreprocessed, VkGeneratedCommandsInfoNV* pGeneratedCommandsInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBindPipelineShaderGroupNV(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline, uint groupIndex);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateIndirectCommandsLayoutNV(VkDevice device, VkIndirectCommandsLayoutCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkIndirectCommandsLayoutNV* pIndirectCommandsLayout);


		[DllImport (libraryName)]
		public static extern void vkDestroyIndirectCommandsLayoutNV(VkDevice device, VkIndirectCommandsLayoutNV indirectCommandsLayout, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthBias2EXT(VkCommandBuffer commandBuffer, VkDepthBiasInfoEXT* pDepthBiasInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireDrmDisplayEXT(VkPhysicalDevice physicalDevice, int drmFd, VkDisplayKHR display);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDrmDisplayEXT(VkPhysicalDevice physicalDevice, int drmFd, uint connectorId, VkDisplayKHR* display);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceVideoEncodeQualityLevelPropertiesKHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceVideoEncodeQualityLevelInfoKHR* pQualityLevelInfo, VkVideoEncodeQualityLevelPropertiesKHR* pQualityLevelProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetEncodedVideoSessionParametersKHR(VkDevice device, VkVideoEncodeSessionParametersGetInfoKHR* pVideoSessionParametersInfo, VkVideoEncodeSessionParametersFeedbackInfoKHR* pFeedbackInfo, UIntPtr* pDataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdEncodeVideoKHR(VkCommandBuffer commandBuffer, VkVideoEncodeInfoKHR* pEncodeInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateCudaModuleNV(VkDevice device, VkCudaModuleCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkCudaModuleNV* pModule);


		[DllImport (libraryName)]
		public static extern VkResult vkGetCudaModuleCacheNV(VkDevice device, VkCudaModuleNV module, UIntPtr* pCacheSize, void* pCacheData);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateCudaFunctionNV(VkDevice device, VkCudaFunctionCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkCudaFunctionNV* pFunction);


		[DllImport (libraryName)]
		public static extern void vkDestroyCudaModuleNV(VkDevice device, VkCudaModuleNV module, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkDestroyCudaFunctionNV(VkDevice device, VkCudaFunctionNV function, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdCudaLaunchKernelNV(VkCommandBuffer commandBuffer, VkCudaLaunchInfoNV* pLaunchInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdRefreshObjectsKHR(VkCommandBuffer commandBuffer, VkRefreshObjectListKHR* pRefreshObjects);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceRefreshableObjectTypesKHR(VkPhysicalDevice physicalDevice, uint* pRefreshableObjectTypeCount, VkObjectType* pRefreshableObjectTypes);


		[DllImport (libraryName)]
		public static extern void vkExportMetalObjectsEXT(VkDevice device, VkExportMetalObjectsInfoEXT* pMetalObjectsInfo);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorSetLayoutSizeEXT(VkDevice device, VkDescriptorSetLayout layout, ulong* pLayoutSizeInBytes);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorSetLayoutBindingOffsetEXT(VkDevice device, VkDescriptorSetLayout layout, uint binding, ulong* pOffset);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorEXT(VkDevice device, VkDescriptorGetInfoEXT* pDescriptorInfo, UIntPtr dataSize, void* pDescriptor);


		[DllImport (libraryName)]
		public static extern void vkCmdBindDescriptorBuffersEXT(VkCommandBuffer commandBuffer, uint bufferCount, VkDescriptorBufferBindingInfoEXT* pBindingInfos);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDescriptorBufferOffsetsEXT(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint setCount, uint* pBufferIndices, ulong* pOffsets);


		[DllImport (libraryName)]
		public static extern void vkCmdBindDescriptorBufferEmbeddedSamplersEXT(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint set);


		[DllImport (libraryName)]
		public static extern VkResult vkGetBufferOpaqueCaptureDescriptorDataEXT(VkDevice device, VkBufferCaptureDescriptorDataInfoEXT* pInfo, void* pData);


		[DllImport (libraryName)]
		public static extern VkResult vkGetImageOpaqueCaptureDescriptorDataEXT(VkDevice device, VkImageCaptureDescriptorDataInfoEXT* pInfo, void* pData);


		[DllImport (libraryName)]
		public static extern VkResult vkGetImageViewOpaqueCaptureDescriptorDataEXT(VkDevice device, VkImageViewCaptureDescriptorDataInfoEXT* pInfo, void* pData);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSamplerOpaqueCaptureDescriptorDataEXT(VkDevice device, VkSamplerCaptureDescriptorDataInfoEXT* pInfo, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdSetFragmentShadingRateEnumNV(VkCommandBuffer commandBuffer, VkFragmentShadingRateNV shadingRate, VkFragmentShadingRateCombinerOpKHR combinerOps);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksEXT(VkCommandBuffer commandBuffer, uint groupCountX, uint groupCountY, uint groupCountZ);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksIndirectEXT(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, uint drawCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMeshTasksIndirectCountEXT(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDeviceFaultInfoEXT(VkDevice device, VkDeviceFaultCountsEXT* pFaultCounts, VkDeviceFaultInfoEXT* pFaultInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkAcquireWinrtDisplayNV(VkPhysicalDevice physicalDevice, VkDisplayKHR display);


		[DllImport (libraryName)]
		public static extern VkResult vkGetWinrtDisplayNV(VkPhysicalDevice physicalDevice, uint deviceRelativeId, VkDisplayKHR* pDisplay);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateDirectFBSurfaceEXT(VkInstance instance, VkDirectFBSurfaceCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceDirectFBPresentationSupportEXT(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr dfb);


		[DllImport (libraryName)]
		public static extern void vkCmdSetVertexInputEXT(VkCommandBuffer commandBuffer, uint vertexBindingDescriptionCount, VkVertexInputBindingDescription2EXT* pVertexBindingDescriptions, uint vertexAttributeDescriptionCount, VkVertexInputAttributeDescription2EXT* pVertexAttributeDescriptions);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryZirconHandleFUCHSIA(VkDevice device, VkMemoryGetZirconHandleInfoFUCHSIA* pGetZirconHandleInfo, IntPtr pZirconHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryZirconHandlePropertiesFUCHSIA(VkDevice device, VkExternalMemoryHandleTypeFlags handleType, IntPtr zirconHandle, VkMemoryZirconHandlePropertiesFUCHSIA* pMemoryZirconHandleProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkImportSemaphoreZirconHandleFUCHSIA(VkDevice device, VkImportSemaphoreZirconHandleInfoFUCHSIA* pImportSemaphoreZirconHandleInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSemaphoreZirconHandleFUCHSIA(VkDevice device, VkSemaphoreGetZirconHandleInfoFUCHSIA* pGetZirconHandleInfo, IntPtr pZirconHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateBufferCollectionFUCHSIA(VkDevice device, VkBufferCollectionCreateInfoFUCHSIA* pCreateInfo, VkAllocationCallbacks* pAllocator, VkBufferCollectionFUCHSIA* pCollection);


		[DllImport (libraryName)]
		public static extern VkResult vkSetBufferCollectionImageConstraintsFUCHSIA(VkDevice device, VkBufferCollectionFUCHSIA collection, VkImageConstraintsInfoFUCHSIA* pImageConstraintsInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkSetBufferCollectionBufferConstraintsFUCHSIA(VkDevice device, VkBufferCollectionFUCHSIA collection, VkBufferConstraintsInfoFUCHSIA* pBufferConstraintsInfo);


		[DllImport (libraryName)]
		public static extern void vkDestroyBufferCollectionFUCHSIA(VkDevice device, VkBufferCollectionFUCHSIA collection, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetBufferCollectionPropertiesFUCHSIA(VkDevice device, VkBufferCollectionFUCHSIA collection, VkBufferCollectionPropertiesFUCHSIA* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDeviceSubpassShadingMaxWorkgroupSizeHUAWEI(VkDevice device, VkRenderPass renderpass, VkExtent2D* pMaxWorkgroupSize);


		[DllImport (libraryName)]
		public static extern void vkCmdSubpassShadingHUAWEI(VkCommandBuffer commandBuffer);


		[DllImport (libraryName)]
		public static extern void vkCmdBindInvocationMaskHUAWEI(VkCommandBuffer commandBuffer, VkImageView imageView, VkImageLayout imageLayout);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemoryRemoteAddressNV(VkDevice device, VkMemoryGetRemoteAddressInfoNV* pMemoryGetRemoteAddressInfo, void* pAddress);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPipelinePropertiesEXT(VkDevice device, VkPipelineInfoKHR* pPipelineInfo, VkBaseOutStructure* pPipelineProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFenceSciSyncFenceNV(VkDevice device, VkFenceGetSciSyncInfoNV* pGetSciSyncHandleInfo, void* pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFenceSciSyncObjNV(VkDevice device, VkFenceGetSciSyncInfoNV* pGetSciSyncHandleInfo, void* pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkImportFenceSciSyncFenceNV(VkDevice device, VkImportFenceSciSyncInfoNV* pImportFenceSciSyncInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkImportFenceSciSyncObjNV(VkDevice device, VkImportFenceSciSyncInfoNV* pImportFenceSciSyncInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSciSyncAttributesNV(VkPhysicalDevice physicalDevice, VkSciSyncAttributesInfoNV* pSciSyncAttributesInfo, IntPtr pAttributes);


		[DllImport (libraryName)]
		public static extern VkResult vkGetSemaphoreSciSyncObjNV(VkDevice device, VkSemaphoreGetSciSyncInfoNV* pGetSciSyncInfo, void* pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkImportSemaphoreSciSyncObjNV(VkDevice device, VkImportSemaphoreSciSyncInfoNV* pImportSemaphoreSciSyncInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetMemorySciBufNV(VkDevice device, VkMemoryGetSciBufInfoNV* pGetSciBufInfo, IntPtr pHandle);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceExternalMemorySciBufPropertiesNV(VkPhysicalDevice physicalDevice, VkExternalMemoryHandleTypeFlags handleType, IntPtr handle, VkMemorySciBufPropertiesNV* pMemorySciBufProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceSciBufAttributesNV(VkPhysicalDevice physicalDevice, IntPtr pAttributes);


		[DllImport (libraryName)]
		public static extern void vkCmdSetPatchControlPointsEXT(VkCommandBuffer commandBuffer, uint patchControlPoints);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLogicOpEXT(VkCommandBuffer commandBuffer, VkLogicOp logicOp);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateScreenSurfaceQNX(VkInstance instance, VkScreenSurfaceCreateInfoQNX* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSurfaceKHR* pSurface);


		[DllImport (libraryName)]
		public static extern VkBool32 vkGetPhysicalDeviceScreenPresentationSupportQNX(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr window);


		[DllImport (libraryName)]
		public static extern void vkCmdSetColorWriteEnableEXT(VkCommandBuffer commandBuffer, uint attachmentCount, VkBool32* pColorWriteEnables);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMultiEXT(VkCommandBuffer commandBuffer, uint drawCount, VkMultiDrawInfoEXT* pVertexInfo, uint instanceCount, uint firstInstance, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawMultiIndexedEXT(VkCommandBuffer commandBuffer, uint drawCount, VkMultiDrawIndexedInfoEXT* pIndexInfo, uint instanceCount, uint firstInstance, uint stride, int* pVertexOffset);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateMicromapEXT(VkDevice device, VkMicromapCreateInfoEXT* pCreateInfo, VkAllocationCallbacks* pAllocator, VkMicromapEXT* pMicromap);


		[DllImport (libraryName)]
		public static extern void vkDestroyMicromapEXT(VkDevice device, VkMicromapEXT micromap, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern void vkCmdBuildMicromapsEXT(VkCommandBuffer commandBuffer, uint infoCount, VkMicromapBuildInfoEXT* pInfos);


		[DllImport (libraryName)]
		public static extern VkResult vkBuildMicromapsEXT(VkDevice device, VkDeferredOperationKHR deferredOperation, uint infoCount, VkMicromapBuildInfoEXT* pInfos);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyMicromapEXT(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyMicromapInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyMicromapToMemoryEXT(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyMicromapToMemoryInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkCopyMemoryToMicromapEXT(VkDevice device, VkDeferredOperationKHR deferredOperation, VkCopyMemoryToMicromapInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkWriteMicromapsPropertiesEXT(VkDevice device, uint micromapCount, VkMicromapEXT* pMicromaps, VkQueryType queryType, UIntPtr dataSize, void* pData, UIntPtr stride);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMicromapEXT(VkCommandBuffer commandBuffer, VkCopyMicromapInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMicromapToMemoryEXT(VkCommandBuffer commandBuffer, VkCopyMicromapToMemoryInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMemoryToMicromapEXT(VkCommandBuffer commandBuffer, VkCopyMemoryToMicromapInfoEXT* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdWriteMicromapsPropertiesEXT(VkCommandBuffer commandBuffer, uint micromapCount, VkMicromapEXT* pMicromaps, VkQueryType queryType, VkQueryPool queryPool, uint firstQuery);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceMicromapCompatibilityEXT(VkDevice device, VkMicromapVersionInfoEXT* pVersionInfo, VkAccelerationStructureCompatibilityKHR* pCompatibility);


		[DllImport (libraryName)]
		public static extern void vkGetMicromapBuildSizesEXT(VkDevice device, VkAccelerationStructureBuildTypeKHR buildType, VkMicromapBuildInfoEXT* pBuildInfo, VkMicromapBuildSizesInfoEXT* pSizeInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawClusterHUAWEI(VkCommandBuffer commandBuffer, uint groupCountX, uint groupCountY, uint groupCountZ);


		[DllImport (libraryName)]
		public static extern void vkCmdDrawClusterIndirectHUAWEI(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset);


		[DllImport (libraryName)]
		public static extern void vkSetDeviceMemoryPriorityEXT(VkDevice device, VkDeviceMemory memory, float priority);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorSetLayoutHostMappingInfoVALVE(VkDevice device, VkDescriptorSetBindingReferenceVALVE* pBindingReference, VkDescriptorSetLayoutHostMappingInfoVALVE* pHostMapping);


		[DllImport (libraryName)]
		public static extern void vkGetDescriptorSetHostMappingVALVE(VkDevice device, VkDescriptorSet descriptorSet, void** ppData);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMemoryIndirectNV(VkCommandBuffer commandBuffer, ulong copyBufferAddress, uint copyCount, uint stride);


		[DllImport (libraryName)]
		public static extern void vkCmdCopyMemoryToImageIndirectNV(VkCommandBuffer commandBuffer, ulong copyBufferAddress, uint copyCount, uint stride, VkImage dstImage, VkImageLayout dstImageLayout, VkImageSubresourceLayers* pImageSubresources);


		[DllImport (libraryName)]
		public static extern void vkCmdDecompressMemoryNV(VkCommandBuffer commandBuffer, uint decompressRegionCount, VkDecompressMemoryRegionNV* pDecompressMemoryRegions);


		[DllImport (libraryName)]
		public static extern void vkCmdDecompressMemoryIndirectCountNV(VkCommandBuffer commandBuffer, ulong indirectCommandsAddress, ulong indirectCommandsCountAddress, uint stride);


		[DllImport (libraryName)]
		public static extern void vkGetPipelineIndirectMemoryRequirementsNV(VkDevice device, VkComputePipelineCreateInfo* pCreateInfo, VkMemoryRequirements2* pMemoryRequirements);


		[DllImport (libraryName)]
		public static extern void vkCmdUpdatePipelineIndirectBufferNV(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline);


		[DllImport (libraryName)]
		public static extern ulong vkGetPipelineIndirectDeviceAddressNV(VkDevice device, VkPipelineIndirectDeviceAddressInfoNV* pInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdSetTessellationDomainOriginEXT(VkCommandBuffer commandBuffer, VkTessellationDomainOrigin domainOrigin);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthClampEnableEXT(VkCommandBuffer commandBuffer, VkBool32 depthClampEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetPolygonModeEXT(VkCommandBuffer commandBuffer, VkPolygonMode polygonMode);


		[DllImport (libraryName)]
		public static extern void vkCmdSetRasterizationSamplesEXT(VkCommandBuffer commandBuffer, VkSampleCountFlags rasterizationSamples);


		[DllImport (libraryName)]
		public static extern void vkCmdSetSampleMaskEXT(VkCommandBuffer commandBuffer, VkSampleCountFlags samples, uint* pSampleMask);


		[DllImport (libraryName)]
		public static extern void vkCmdSetAlphaToCoverageEnableEXT(VkCommandBuffer commandBuffer, VkBool32 alphaToCoverageEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetAlphaToOneEnableEXT(VkCommandBuffer commandBuffer, VkBool32 alphaToOneEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLogicOpEnableEXT(VkCommandBuffer commandBuffer, VkBool32 logicOpEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetColorBlendEnableEXT(VkCommandBuffer commandBuffer, uint firstAttachment, uint attachmentCount, VkBool32* pColorBlendEnables);


		[DllImport (libraryName)]
		public static extern void vkCmdSetColorBlendEquationEXT(VkCommandBuffer commandBuffer, uint firstAttachment, uint attachmentCount, VkColorBlendEquationEXT* pColorBlendEquations);


		[DllImport (libraryName)]
		public static extern void vkCmdSetColorWriteMaskEXT(VkCommandBuffer commandBuffer, uint firstAttachment, uint attachmentCount, VkColorComponentFlags* pColorWriteMasks);


		[DllImport (libraryName)]
		public static extern void vkCmdSetRasterizationStreamEXT(VkCommandBuffer commandBuffer, uint rasterizationStream);


		[DllImport (libraryName)]
		public static extern void vkCmdSetConservativeRasterizationModeEXT(VkCommandBuffer commandBuffer, VkConservativeRasterizationModeEXT conservativeRasterizationMode);


		[DllImport (libraryName)]
		public static extern void vkCmdSetExtraPrimitiveOverestimationSizeEXT(VkCommandBuffer commandBuffer, float extraPrimitiveOverestimationSize);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthClipEnableEXT(VkCommandBuffer commandBuffer, VkBool32 depthClipEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetSampleLocationsEnableEXT(VkCommandBuffer commandBuffer, VkBool32 sampleLocationsEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetColorBlendAdvancedEXT(VkCommandBuffer commandBuffer, uint firstAttachment, uint attachmentCount, VkColorBlendAdvancedEXT* pColorBlendAdvanced);


		[DllImport (libraryName)]
		public static extern void vkCmdSetProvokingVertexModeEXT(VkCommandBuffer commandBuffer, VkProvokingVertexModeEXT provokingVertexMode);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLineRasterizationModeEXT(VkCommandBuffer commandBuffer, VkLineRasterizationModeEXT lineRasterizationMode);


		[DllImport (libraryName)]
		public static extern void vkCmdSetLineStippleEnableEXT(VkCommandBuffer commandBuffer, VkBool32 stippledLineEnable);


		[DllImport (libraryName)]
		public static extern void vkCmdSetDepthClipNegativeOneToOneEXT(VkCommandBuffer commandBuffer, VkBool32 negativeOneToOne);


		[DllImport (libraryName)]
		public static extern void vkGetShaderModuleIdentifierEXT(VkDevice device, VkShaderModule shaderModule, VkShaderModuleIdentifierEXT* pIdentifier);


		[DllImport (libraryName)]
		public static extern void vkGetShaderModuleCreateInfoIdentifierEXT(VkDevice device, VkShaderModuleCreateInfo* pCreateInfo, VkShaderModuleIdentifierEXT* pIdentifier);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceOpticalFlowImageFormatsNV(VkPhysicalDevice physicalDevice, VkOpticalFlowImageFormatInfoNV* pOpticalFlowImageFormatInfo, uint* pFormatCount, VkOpticalFlowImageFormatPropertiesNV* pImageFormatProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateOpticalFlowSessionNV(VkDevice device, VkOpticalFlowSessionCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkOpticalFlowSessionNV* pSession);


		[DllImport (libraryName)]
		public static extern void vkDestroyOpticalFlowSessionNV(VkDevice device, VkOpticalFlowSessionNV session, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkBindOpticalFlowSessionImageNV(VkDevice device, VkOpticalFlowSessionNV session, VkOpticalFlowSessionBindingPointNV bindingPoint, VkImageView view, VkImageLayout layout);


		[DllImport (libraryName)]
		public static extern void vkCmdOpticalFlowExecuteNV(VkCommandBuffer commandBuffer, VkOpticalFlowSessionNV session, VkOpticalFlowExecuteInfoNV* pExecuteInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdBindIndexBuffer2KHR(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, ulong size, VkIndexType indexType);


		[DllImport (libraryName)]
		public static extern void vkGetRenderingAreaGranularityKHR(VkDevice device, VkRenderingAreaInfoKHR* pRenderingAreaInfo, VkExtent2D* pGranularity);


		[DllImport (libraryName)]
		public static extern void vkGetDeviceImageSubresourceLayoutKHR(VkDevice device, VkDeviceImageSubresourceInfoKHR* pInfo, VkSubresourceLayout2KHR* pLayout);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateShadersEXT(VkDevice device, uint createInfoCount, VkShaderCreateInfoEXT* pCreateInfos, VkAllocationCallbacks* pAllocator, VkShaderEXT* pShaders);


		[DllImport (libraryName)]
		public static extern void vkDestroyShaderEXT(VkDevice device, VkShaderEXT shader, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkGetShaderBinaryDataEXT(VkDevice device, VkShaderEXT shader, UIntPtr* pDataSize, void* pData);


		[DllImport (libraryName)]
		public static extern void vkCmdBindShadersEXT(VkCommandBuffer commandBuffer, uint stageCount, VkShaderStageFlags* pStages, VkShaderEXT* pShaders);


		[DllImport (libraryName)]
		public static extern VkResult vkGetFramebufferTilePropertiesQCOM(VkDevice device, VkFramebuffer framebuffer, uint* pPropertiesCount, VkTilePropertiesQCOM* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkGetDynamicRenderingTilePropertiesQCOM(VkDevice device, VkRenderingInfo* pRenderingInfo, VkTilePropertiesQCOM* pProperties);


		[DllImport (libraryName)]
		public static extern VkResult vkCreateSemaphoreSciSyncPoolNV(VkDevice device, VkSemaphoreSciSyncPoolCreateInfoNV* pCreateInfo, VkAllocationCallbacks* pAllocator, VkSemaphoreSciSyncPoolNV* pSemaphorePool);


		[DllImport (libraryName)]
		public static extern void vkDestroySemaphoreSciSyncPoolNV(VkDevice device, VkSemaphoreSciSyncPoolNV semaphorePool, VkAllocationCallbacks* pAllocator);


		[DllImport (libraryName)]
		public static extern VkResult vkSetLatencySleepModeNV(VkDevice device, VkSwapchainKHR swapchain, VkLatencySleepModeInfoNV* pSleepModeInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkLatencySleepNV(VkDevice device, VkSwapchainKHR swapchain, VkLatencySleepInfoNV* pSleepInfo);


		[DllImport (libraryName)]
		public static extern void vkSetLatencyMarkerNV(VkDevice device, VkSwapchainKHR swapchain, VkSetLatencyMarkerInfoNV* pLatencyMarkerInfo);


		[DllImport (libraryName)]
		public static extern void vkGetLatencyTimingsNV(VkDevice device, VkSwapchainKHR swapchain, VkGetLatencyMarkerInfoNV* pLatencyMarkerInfo);


		[DllImport (libraryName)]
		public static extern void vkQueueNotifyOutOfBandNV(VkQueue queue, VkOutOfBandQueueTypeInfoNV* pQueueTypeInfo);


		[DllImport (libraryName)]
		public static extern VkResult vkGetPhysicalDeviceCooperativeMatrixPropertiesKHR(VkPhysicalDevice physicalDevice, uint* pPropertyCount, VkCooperativeMatrixPropertiesKHR* pProperties);


		[DllImport (libraryName)]
		public static extern void vkCmdSetAttachmentFeedbackLoopEnableEXT(VkCommandBuffer commandBuffer, VkImageAspectFlags aspectMask);


		[DllImport (libraryName)]
		public static extern VkResult vkGetScreenBufferPropertiesQNX(VkDevice device, _screen_buffer* buffer, VkScreenBufferPropertiesQNX* pProperties);


		[DllImport (libraryName)]
		public static extern void vkCmdBindDescriptorSets2KHR(VkCommandBuffer commandBuffer, VkBindDescriptorSetsInfoKHR* pBindDescriptorSetsInfo);


		[DllImport (libraryName)]
		public static extern void vkCmdPushConstants2KHR(VkCommandBuffer commandBuffer, VkPushConstantsInfoKHR* pPushConstantsInfo);


		}
