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
using System.Collections.Generic;
using GPUVulkan;
#nullable disable


namespace VulkanPlatform
{

	public enum DeliveryPlatform { Android, iOS, MacCatalyst, MacOS, Linux,  Windows}


	public class VulkanSupport : IVulkanSupport
    {
		private DeliveryPlatform _platform;

        public DeliveryPlatform Platform { get { return _platform; } }


        public bool EnableValidationLayers { get; set; } = false;


#if DEBUG
        private VkDebugUtilsMessengerEXT _debugMessenger;
#endif
        private VulkanStringList _availableExtensions = new VulkanStringList();
        private VulkanStringList _requiredExtensions = new VulkanStringList();
        private VulkanStringList _availableValidationLayers = new VulkanStringList();
        private VulkanStringList _requiredValidationLayers = new VulkanStringList();
        private VulkanStringList _instanceExtensions = new VulkanStringList();
        private VulkanStringList _deviceExtensions = new VulkanStringList();



        public VulkanStringList AvailableExtensions { get { return _requiredExtensions; } }
        public VulkanStringList RequiredExtensions { get { return _availableExtensions; } }


        public VulkanStringList AvailableValidationLayers { get { return _availableValidationLayers; } }
        public VulkanStringList RequiredValidationLayers { get { return _requiredValidationLayers; } }

        public VulkanStringList InstanceExtensions { get { return _instanceExtensions; } }
        public VulkanStringList DeviceExtensions { get { return _deviceExtensions; } }



        // Vulkan objects

        private VkDevice _device;
        public VkDevice Device { get { return _device; } }

        public void SetDevice (VkDevice device)
        {
            _device = device;
        }

        private VkInstance _instance;
        public VkInstance Instance { get { return _instance; } }

        public void SetInstance(VkInstance instance)
        {
            _instance = instance;
        }

        private VkPhysicalDevice _physicalDevice;

        public VkPhysicalDevice PhysicalDevice { get { return _physicalDevice; } }

        public void SetPhysicalDevice(VkPhysicalDevice physicalDevice)
        {
            _physicalDevice = physicalDevice;
        }

        bool propertiesRetrieved = false;
        VkPhysicalDeviceProperties _deviceProperties = default(VkPhysicalDeviceProperties);
        public VkPhysicalDeviceProperties DeviceProperties
        {
            get
            {
                if (!propertiesRetrieved)
                {
                    GetPhysicalDeviceProperties();
                }

                return _deviceProperties;
            }
        }

        public unsafe void GetPhysicalDeviceProperties()
        {
            fixed (VkPhysicalDeviceProperties* devicePropertiesPtr = &_deviceProperties)
            {
                VulkanNative.vkGetPhysicalDeviceProperties(PhysicalDevice, devicePropertiesPtr);
            }
            propertiesRetrieved = _deviceProperties.limits.maxComputeWorkGroupCount_0 > 0;
        }


        public VulkanSupport(DeliveryPlatform platform)
        {
            _platform = platform;


#if DEBUG
            InstanceExtensions.Add(VulkanNative.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
#else
            EnableValidationLayers = false;
#endif
            DeviceExtensions.Add(VulkanNative.VK_KHR_SWAPCHAIN_EXTENSION_NAME);

            switch (_platform)
            {

                case DeliveryPlatform.iOS:
                case DeliveryPlatform.MacCatalyst:
                case DeliveryPlatform.MacOS:
                    InstanceExtensions.Add(VulkanNative.VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);
                    DeviceExtensions.Add(VulkanNative.VK_KHR_PORTABILITY_SUBSET_EXTENSION_NAME);
                    break;
            }


            InitializeGPU();
        }


        private void InitializeGPU()
        {

#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSupport.InitializeGPU");
#endif
            this.CreateInstance();
            this.SetupDebugMessenger();



        }



        public unsafe void CleanupVulkanSupport()
        {

            VulkanNative.vkDestroyDevice(Device, null);
#if DEBUG
            this.DestroyDebugMessenger();
#endif
            VulkanNative.vkDestroyInstance(Instance, null);
        }

      
    }

}

