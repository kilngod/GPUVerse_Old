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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace VulkanPlatform
{
    public static class VulkanInstance
    {
        private static string[][] _validationLayerNamesPriorityList =
  {
            new [] { "VK_LAYER_KHRONOS_validation" },
            new [] { "VK_LAYER_LUNARG_standard_validation" },
            new []
            {
                "VK_LAYER_GOOGLE_threading",
                "VK_LAYER_LUNARG_parameter_validation",
                "VK_LAYER_LUNARG_object_tracker",
                "VK_LAYER_LUNARG_core_validation",
                "VK_LAYER_GOOGLE_unique_objects",
            }
        };

        public static unsafe void CreateInstance(this IVulkanSupport support)
        {


            /*
Windows x64/NVidia ****
VK_KHR_device_group_creation
VK_KHR_display
VK_KHR_external_fence_capabilities
VK_KHR_external_memory_capabilities
VK_KHR_external_semaphore_capabilities
VK_KHR_get_display_properties2
VK_KHR_get_physical_device_properties2
VK_KHR_get_surface_capabilities2
VK_KHR_surface
VK_KHR_surface_protected_capabilities
VK_KHR_win32_surface
VK_EXT_debug_report
VK_EXT_debug_utils
VK_EXT_direct_mode_display
VK_EXT_swapchain_colorspace
VK_NV_external_memory_capabilities
VK_KHR_portability_enumeration
VK_LUNARG_direct_driver_loading
            ****
Android 64 api 33 emulator mac
VK_KHR_surface *
VK_KHR_surface_protected_capabilities
VK_KHR_android_surface *
VK_EXT_swapchain_colorspace
VK_KHR_get_surface_capabilities2
VK_GOOGLE_surfaceless_query
VK_EXT_debug_report
VK_KHR_get_physical_device_properties2
VK_KHR_external_semaphore_capabilities
VK_KHR_external_memory_capabilities
VK_KHR_external_fence_capabilities
****
MacCatylist 64
VK_KHR_device_group_creation
VK_KHR_external_fence_capabilities
VK_KHR_external_memory_capabilities
VK_KHR_external_semaphore_capabilities             
VK_KHR_get_physical_device_properties2             
VK_KHR_get_surface_capabilities2
VK_KHR_surface
VK_EXT_debug_report
VK_EXT_debug_utils
VK_EXT_metal_surface
VK_EXT_surface_maintenance1
VK_EXT_swapchain_colorspace
VK_MVK_macos_surface
VK_MVK_moltenvk


            
             */
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanInstance.CreateInstance");
#endif
            uint availbleExtensionCount;

            string extensions = string.Empty;
            try
            {

                VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateInstanceExtensionProperties((byte*)null, &availbleExtensionCount, null));
                VkExtensionProperties* AvailableExtensions = stackalloc VkExtensionProperties[(int)availbleExtensionCount];

                VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateInstanceExtensionProperties((byte*)null, &availbleExtensionCount, AvailableExtensions));

                for (int i = 0; i < availbleExtensionCount; i++)
                {
                    VkExtensionProperties item = AvailableExtensions[i];
                    string extensionName = VulkanHelpers.GetString(item.extensionName);
                    extensions += extensionName + System.Environment.NewLine;
                    support.AvailableExtensions.Add(extensionName);
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Failed to find Vulkan library. (" + ex.Message + ")");
            }




            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_SURFACE_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_SURFACE_EXTENSION_NAME);
            }
            else
            {
                throw new NotSupportedException("KhrSurface Not Supported");
            }

            uint layerCount = 0u;
            VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateInstanceLayerProperties(&layerCount, null));

            if (layerCount > 0)
            {
                VkLayerProperties* availableLayers = stackalloc VkLayerProperties[(int)layerCount];

                VulkanHelpers.CheckErrors(VulkanNative.vkEnumerateInstanceLayerProperties(&layerCount, availableLayers));


                for (int i = 0; i < layerCount; i++)
                {
                    var layerName = VulkanHelpers.GetString(availableLayers[i].layerName);
                    support.AvailableValidationLayers.Add(layerName);
                }



                foreach (var validationLayerNameSet in _validationLayerNamesPriorityList)
                {
                    if (validationLayerNameSet.All(validationLayerName => support.AvailableValidationLayers.Contains(validationLayerName)))
                    {
                        support.RequiredValidationLayers.AddRange(validationLayerNameSet);
                    }
                }

            }
#if DEBUG

            if ((support.RequiredValidationLayers.Count > 0) && (support.AvailableExtensions.Find(x => VulkanNative.VK_EXT_DEBUG_UTILS_EXTENSION_NAME == x) != null))
            {
                support.RequiredExtensions.Add(VulkanNative.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
                support.EnableValidationLayers = true;

            }
            else if (support.AvailableExtensions.Find(x => VulkanNative.VK_EXT_DEBUG_REPORT_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_EXT_DEBUG_REPORT_EXTENSION_NAME);
            }


#endif

            switch (support.Platform)
            {
                case DeliveryPlatform.Android:
                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_ANDROID_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_KHR_ANDROID_SURFACE_EXTENSION_NAME);

                    }
                    else
                    {
                        throw new NotSupportedException("KhrAndroidSurface Not Supported");
                    }
                    break;
                case DeliveryPlatform.Windows:

                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_WIN32_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_KHR_WIN32_SURFACE_EXTENSION_NAME);
                    }
                    else
                    {
                        throw new NotSupportedException("KhrWin32Surface Not Supported");
                    }
                    break;

                case DeliveryPlatform.iOS:

                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_MVK_IOS_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_MVK_IOS_SURFACE_EXTENSION_NAME);
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }

                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_EXT_METAL_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_EXT_METAL_SURFACE_EXTENSION_NAME);
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }

                    if (support.AvailableExtensions.Find(x => "VK_MVK_moltenvk" == x) != null)
                    {
                        support.RequiredExtensions.Add("VK_MVK_moltenvk");
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }


                    break;


                case DeliveryPlatform.MacCatalyst:
                case DeliveryPlatform.MacOS:
                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_MVK_MACOS_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_MVK_MACOS_SURFACE_EXTENSION_NAME);
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }

                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_EXT_METAL_SURFACE_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_EXT_METAL_SURFACE_EXTENSION_NAME);
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }

                    if (support.AvailableExtensions.Find(x => "VK_MVK_moltenvk" == x) != null)
                    {
                        support.RequiredExtensions.Add("VK_MVK_moltenvk");
                    }
                    else
                    {
                        throw new NotSupportedException("ExtMetalSurface Not Supported");
                    }

                    if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);
                    }

                    break;

                case DeliveryPlatform.Linux:



                    break;
            }



            /* 
             * 
              //determine or support color accuracy for higher level color accuracy
            if (support.AvailableExtensions.Find(x => VulkanNative.VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME);
            }
              
            //portability subset
            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);
            }
             //the following appear to be related to full screen support
            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_GET_SURFACE_CAPABILITIES_2_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_GET_SURFACE_CAPABILITIES_2_EXTENSION_NAME);
            }

            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_GET_DISPLAY_PROPERTIES_2_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_GET_DISPLAY_PROPERTIES_2_EXTENSION_NAME);
            }
            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);
            }
            
            if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_SURFACE_PROTECTED_CAPABILITIES_EXTENSION_NAME == x) != null)
            {
                support.RequiredExtensions.Add(VulkanNative.VK_KHR_SURFACE_PROTECTED_CAPABILITIES_EXTENSION_NAME);

            }

              if (support.AvailableExtensions.Find(x => VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME == x) != null)
                    {
                        support.RequiredExtensions.Add(VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);
                    }
                  
            */


            var appInfo = new VkApplicationInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
                pApplicationName = (byte*)Marshal.StringToHGlobalAnsi("VulkanSupport"),
                applicationVersion = VulkanHelpers.Version(1, 0, 0),
                pEngineName = (byte*)Marshal.StringToHGlobalAnsi("ILGPU.Vulkan"),
                engineVersion = VulkanHelpers.Version(1, 0, 0),
                apiVersion = VulkanHelpers.Version(1, 2, 0)
            };

            IntPtr* extensionsToBytesArray = stackalloc IntPtr[support.RequiredExtensions.Count];
            VulkanHelpers.StringListToByteArrary(support.RequiredExtensions, ref extensionsToBytesArray);
            var instanceCreateInfo = new VkInstanceCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                pApplicationInfo = &appInfo,
                enabledExtensionCount = (uint)support.RequiredExtensions.Count,
                ppEnabledExtensionNames = (byte**)extensionsToBytesArray

            };


            if (support.EnableValidationLayers)
            {
                IntPtr* enabledLayersToBytesArray = stackalloc IntPtr[support.RequiredValidationLayers.Count];

                VulkanHelpers.StringListToByteArrary(support.RequiredValidationLayers, ref enabledLayersToBytesArray);
                instanceCreateInfo.enabledLayerCount = (uint)support.RequiredValidationLayers.Count;
                instanceCreateInfo.ppEnabledLayerNames = (byte**)enabledLayersToBytesArray;
            }
            else
            {
                instanceCreateInfo.enabledLayerCount = 0;
                instanceCreateInfo.pNext = null;
            }
            
            VkInstance instance = default(VkInstance);
            
            VulkanHelpers.CheckErrors(VulkanNative.vkCreateInstance(&instanceCreateInfo, null, &instance));
            (support as VulkanSupport).SetInstance(instance);
        //    VulkanNative.LoadFuncionPointers(instance);
            



            Marshal.FreeHGlobal((IntPtr)appInfo.pApplicationName);
            Marshal.FreeHGlobal((IntPtr)appInfo.pEngineName);




        }


        public static unsafe void ConfigureDevices(this IVulkanRenderer renderer, VkSurfaceKHR surface)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanInstance.ConfigureDevices");
#endif
            List<string> requiredDeviceExtensions = new List<string>();
            requiredDeviceExtensions.Add("VK_KHR_swapchain"); // non-ray tracing
            VkPhysicalDevice physicalDevice = default(VkPhysicalDevice);

            VulkanPhysicalDevice.PickPhysicalDevice(ref physicalDevice, renderer.VSupport.Instance, surface, requiredDeviceExtensions);
            renderer.VSupport.SetPhysicalDevice(physicalDevice);

            VkQueue pqueue = default(VkQueue);
            VkQueue gqueue = default(VkQueue);
            VkDevice device = default(VkDevice);
            VulkanPhysicalDevice.CreateLogicalDevice(physicalDevice, requiredDeviceExtensions, surface, ref device, ref gqueue, ref pqueue);
            renderer.VSupport.SetDevice(device);
            renderer.SetPresentQueue( pqueue);
            renderer.SetGraphicsQueue(gqueue);

        }

    }
}
