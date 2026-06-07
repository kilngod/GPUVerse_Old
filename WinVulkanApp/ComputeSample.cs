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
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GPUVulkan;
using VulkanPlatform;

namespace WinVulkanApp
{
	public class ComputeSample:IVulkanCompute
	{
        IVulkanSupport _support;

		public ComputeSample(IVulkanSupport support)
		{
            _support = support;
		}
              
        int _computeFamilyIndex = -1;
        public int ComputeCommandBuffers { get; set; } = 1;
        public int ComputeFamilyIndex { get { return _computeFamilyIndex; } }

        private VkQueue _computeQueue;
        public VkQueue ComputeQueue { get { return _computeQueue; } }

        public IVulkanSupport Support { get { return _support; } }

        public VkCommandPool CommandPool { get; set; }
        public VkCommandBuffer[] CommandBuffers { get; set; }
        public VkPipelineLayout PipelineLayout { get { return _pipelineLayout; } }

     
        public uint ComputeDescriptorSets { get; set; } = 1;

        VkDescriptorSet[] _descriptorSets;
        public VkDescriptorSet[] DescriptorSets { get { return _descriptorSets; } }
        public VkDescriptorSetLayout ComputeLayout { get; set; }
        public VkSemaphore ComputeSemaphore { get; set; }

      

        public VkPipeline ComputePipeline { get { return _computePipeline; } }

        private VkBuffer _buffer;
        private VkDeviceMemory _deviceMemory;

        // mandlebrot information
        
        const uint kWidth = 3200;
        const uint kHeight = 2400;
        uint kWorkgroupSize = 32;
        ulong buffer_size;


        public unsafe void SaveRenderedImage()
        {
            Bitmap bitmap = new Bitmap((int) kWidth, (int) kHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            VulkanPixel[] pixel_data = new VulkanPixel[kWidth * kHeight];

            Support.Device.DownloadBufferData(_deviceMemory, ref pixel_data);

           
            
            for (int i = 0; i < kWidth * kHeight; i++)
            {
                VulkanPixel pixel = pixel_data[i];
                int red = Convert.ToInt32(pixel.r * 255);
                int green = Convert.ToInt32(pixel.g * 225);
                int blue = Convert.ToInt32(pixel.b * 225);
                int x = (int)( i % kWidth);
                int y = (int)(i / kWidth);
                bitmap.SetPixel(x,y, Color.FromArgb(red, green, blue));
            }
            string outputPath = Path.Combine(WinIO.WritableOutputFolderPath(), "Mandelbrot.bmp");
            using FileStream outputStream = File.Create(outputPath);
            bitmap.Save(outputStream, ImageFormat.Bmp);
        }


        /*
         private void CreateTextureImage()
        {
            Image<Rgba32> image;
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
                VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
                out VkImage stagingImage,
                out VkDeviceMemory stagingImageMemory);

            VkImageSubresource subresource = new VkImageSubresource();
            subresource.aspectMask = VkImageAspectFlags.Color;
            subresource.mipLevel = 0;
            subresource.arrayLayer = 0;

            vkGetImageSubresourceLayout(_device, stagingImage, ref subresource, out VkSubresourceLayout stagingImageLayout);
            ulong rowPitch = stagingImageLayout.rowPitch;

            void* mappedPtr;
            vkMapMemory(_device, stagingImageMemory, 0, imageSize, 0, &mappedPtr);
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
            vkUnmapMemory(_device, stagingImageMemory);

            CreateImage(
                (uint)image.Width,
                (uint)image.Height,
                VkFormat.R8g8b8a8Unorm,
                VkImageTiling.Optimal,
                VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
                VkMemoryPropertyFlags.DeviceLocal,
                out _textureImage,
                out _textureImageMemory);

            TransitionImageLayout(stagingImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.Preinitialized, VkImageLayout.TransferSrcOptimal);
            TransitionImageLayout(_textureImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.Preinitialized, VkImageLayout.TransferDstOptimal);
            CopyImage(stagingImage, _textureImage, (uint)image.Width, (uint)image.Height);
            TransitionImageLayout(_textureImage, VkFormat.R8g8b8a8Unorm, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal);

            vkDestroyImage(_device, stagingImage, null);
        }


        /*
         * 
         *   void SaveRenderedImage(const char *outfilename) {
    auto pixel_data = static_cast<Pixel *>(
        device_->mapMemory(*buffer_memory_, 0, buffer_size, {}));
    std::vector<unsigned char> image;
    image.reserve(kWidth * kHeight * 4);
    for (int i = 0; i < kWidth * kHeight; ++i) {
      image.push_back(static_cast<unsigned char>(255.0f * (pixel_data[i].r)));
      image.push_back(static_cast<unsigned char>(255.0f * (pixel_data[i].g)));
      image.push_back(static_cast<unsigned char>(255.0f * (pixel_data[i].b)));
      image.push_back(static_cast<unsigned char>(255.0f * (pixel_data[i].a)));
    }
    device_->unmapMemory(*buffer_memory_);
    unsigned error = lodepng::encode(outfilename, image, kWidth, kHeight);
    if (error) {
      throw std::runtime_error("Encoding error: "s + lodepng_error_text(error));
    }
  }
        */


        public void SetupComputePipeline()
        {
            GetComputeQueue();
            // compute
           
            CreateBuffersAndMemory();
            
         

            CreateDescriptors();
            List<VulkanSpirV> computeShaderList = new List<VulkanSpirV>();
            string resourceFolder = AppContext.BaseDirectory + "\\Shaders\\";
            VulkanSpirV computeV = new VulkanSpirV() { EntryName = "main", Name = "Compute", ShaderStageType = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT, SpirVByte = VulkanIO.LoadRawResource(resourceFolder + "comp.spv") };
            computeShaderList.Add(computeV);

            CreatePipeline(computeShaderList);

           
            

        }

        public unsafe void Compute()
        {
            this.CreateCommandPool();

            this.CreateCommandBuffers();

            if (kWorkgroupSize > Support.DeviceProperties.limits.maxComputeWorkGroupSize_0)
            {
                kWorkgroupSize = Support.DeviceProperties.limits.maxComputeWorkGroupSize_0;
            }

            this.FillCommandBuffer(kWidth / kWorkgroupSize, kHeight / kWorkgroupSize, 1);

            fixed (VkCommandBuffer* commandBuffersPtr = &CommandBuffers[0])
            {
                VkSubmitInfo submitInfo = new VkSubmitInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
                    commandBufferCount = (uint)ComputeCommandBuffers,
                    pCommandBuffers = commandBuffersPtr
                };
                this.SubmitAndWait(new VkSubmitInfo[] { submitInfo });
            }
            SaveRenderedImage();
        }

        

        private void GetComputeQueue()
        {

            _computeFamilyIndex = (int) Support.PhysicalDevice.FindQueueFamilyIndex(VkQueueFlags.VK_QUEUE_COMPUTE_BIT);

            Support.Device.GetQueue(ComputeFamilyIndex, 0, ref _computeQueue);
        }

        private unsafe void CreateBuffersAndMemory()
        {
            // size pixel map
            buffer_size = (ulong)sizeof(VulkanPixel) * kWidth * kHeight;
            _buffer = default(VkBuffer);
            Support.Device.CreateBuffer(buffer_size, ref _buffer);
            
            //allocate memory
            _deviceMemory = default(VkDeviceMemory);
            Support.AllocateMemory(ref _buffer, ref _deviceMemory);

            // bind memory
            Support.Device.BindDeviceMemory(ref _buffer, ref _deviceMemory, 0);
        }



        VkDescriptorSetLayout _descriptorSetLayout = default(VkDescriptorSetLayout);
        VkDescriptorPool _descriptorPool = default(VkDescriptorPool);

        //https://vkguide.dev/docs/extra-chapter/abstracting_descriptors/
        // "Creating and managing descriptor sets is one of the most painful things about Vulkan"
        // managing descriptor sets falls under the label of future research.
        private unsafe void CreateDescriptors()
        {
            // descriptor binding
            VkDescriptorSetLayoutBinding layoutBinding = new VkDescriptorSetLayoutBinding()
            {   
                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                descriptorCount = ComputeDescriptorSets,
                stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_COMPUTE_BIT
            };

            VkDescriptorSetLayoutCreateInfo layoutCreateInfo = new VkDescriptorSetLayoutCreateInfo() 
            { 
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                bindingCount = 1, 
                pBindings = &layoutBinding 
            };
            this.Support.Device.CreateDescriptorSetLayout(ref layoutCreateInfo, ref _descriptorSetLayout);

            // descriptor pool
            VkDescriptorPoolSize descriptorPoolSize = new VkDescriptorPoolSize() 
            { 
                descriptorCount = 1, 
                type = VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER 
            };

            VkDescriptorPoolCreateInfo poolCreateInfo = new VkDescriptorPoolCreateInfo()
            { 
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                poolSizeCount = 1,
                pPoolSizes = &descriptorPoolSize,
                maxSets = 1
            };

           
            this.Support.Device.CreateDescriptorPool(ref poolCreateInfo, ref _descriptorPool);

            fixed (VkDescriptorSetLayout* layoutPtr = &_descriptorSetLayout)
            {
                _descriptorSets = new VkDescriptorSet[ComputeDescriptorSets];

                // descriptor sets
                VkDescriptorSetAllocateInfo allocateInfo = new VkDescriptorSetAllocateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                    descriptorPool = _descriptorPool,
                    descriptorSetCount = ComputeDescriptorSets,
                    pSetLayouts = layoutPtr
                };
            
                this.Support.Device.AllocateDescriptorSets(ref allocateInfo, ref _descriptorSets[0]);
            }

            // connect buffer to descriptor sets
            VkDescriptorBufferInfo descriptorBufferInfo = new VkDescriptorBufferInfo()
            {
                buffer = _buffer,
                offset = 0,
                range = buffer_size
            };

            VkWriteDescriptorSet writeDescriptorSet = new VkWriteDescriptorSet()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                dstSet = _descriptorSets[0],
                descriptorCount = (uint)_descriptorSets.Length,
                dstBinding = 0,
                descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
                pBufferInfo = &descriptorBufferInfo
            };

      
            this.Support.Device.UpdateDescriptorSet(ref writeDescriptorSet);

            
        }


        VkPipelineLayout _pipelineLayout = default(VkPipelineLayout);
        VkPipeline _computePipeline = default(VkPipeline);
        public unsafe void CreatePipeline(List<VulkanSpirV> shaderSource)
        {

            VkShaderModule[] shaderModules = new VkShaderModule[shaderSource.Count];
            VkPipelineShaderStageCreateInfo[] stageInfo = new VkPipelineShaderStageCreateInfo[shaderSource.Count];

            for (int i = 0; i < shaderSource.Count; i++)
            {
                shaderModules[i] = this.Support.Device.CreateShaderModule(shaderSource[i].SpirVByte);
                stageInfo[i] = new VkPipelineShaderStageCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = shaderSource[i].ShaderStageType,
                    module = shaderModules[i],
                    pName = shaderSource[i].EntryName.ToPointer(),
                };

            }




            fixed (VkDescriptorSetLayout* layout = &_descriptorSetLayout)
            {
                VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
                    setLayoutCount = 1,
                    pSetLayouts = layout
                };

                /*
                VkPipelineCacheCreateInfo cacheCreateInfo = new VkPipelineCacheCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_CACHE_CREATE_INFO,
                    
                }
                VkPipelineCache
                */



        Support.Device.CreatePipelineLayout(ref pipelineLayoutInfo, ref _pipelineLayout);

                VkComputePipelineCreateInfo pipelineCreateInfo = new VkComputePipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO,
                    layout = _pipelineLayout,
                    stage = stageInfo[0]                    
                    
                };

              
                
                Support.Device.CreateComputePipeline(ref pipelineCreateInfo, ref _computePipeline);
            }

            for (int i = 0; i < shaderModules.Length; i++)
            {
                if (shaderModules[i] != VkShaderModule.Null)
                {
                    VulkanNative.vkDestroyShaderModule(Support.Device, shaderModules[i], null);
                    shaderModules[i] = VkShaderModule.Null;
                }
            }
        }

        public unsafe void CleanUp()
        {
            VulkanHelpers.CheckErrors(VulkanNative.vkDeviceWaitIdle(Support.Device));

            if (_computePipeline != VkPipeline.Null)
            {
                VulkanNative.vkDestroyPipeline(Support.Device, _computePipeline, null);
                _computePipeline = VkPipeline.Null;
            }

            if (_pipelineLayout != VkPipelineLayout.Null)
            {
                VulkanNative.vkDestroyPipelineLayout(Support.Device, _pipelineLayout, null);
                _pipelineLayout = VkPipelineLayout.Null;
            }

            if (_descriptorPool != VkDescriptorPool.Null)
            {
                VulkanNative.vkDestroyDescriptorPool(Support.Device, _descriptorPool, null);
                _descriptorPool = VkDescriptorPool.Null;
            }

            if (_descriptorSetLayout != VkDescriptorSetLayout.Null)
            {
                VulkanNative.vkDestroyDescriptorSetLayout(Support.Device, _descriptorSetLayout, null);
                _descriptorSetLayout = VkDescriptorSetLayout.Null;
            }

            if (_buffer != VkBuffer.Null)
            {
                VulkanNative.vkDestroyBuffer(Support.Device, _buffer, null);
                _buffer = VkBuffer.Null;
            }

            if (_deviceMemory != VkDeviceMemory.Null)
            {
                VulkanNative.vkFreeMemory(Support.Device, _deviceMemory, null);
                _deviceMemory = VkDeviceMemory.Null;
            }

            if (CommandPool != VkCommandPool.Null)
            {
                VulkanNative.vkDestroyCommandPool(Support.Device, CommandPool, null);
                CommandPool = VkCommandPool.Null;
            }
        }
           
        
    }

  
}

