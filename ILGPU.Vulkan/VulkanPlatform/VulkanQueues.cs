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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPUVulkan;

namespace VulkanPlatform
{

    public struct QueueFamilyIndices
    {
        public int graphicsFamily;
        public int presentFamily;
      


        public bool IsGraphicsComplete()
        {
            return graphicsFamily >= 0 && presentFamily >= 0;
        }


      


        public QueueFamilyIndices(int graphicsFamily = -1, int presentFamily = -1)
        {
            this.graphicsFamily = graphicsFamily;
            this.presentFamily = presentFamily;
        }
    }



  

    public static class VulkanQueues
    {
        public static unsafe QueueFamilyIndices FindGraphicsQueueFamilies(this VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
        {
            QueueFamilyIndices indices = default;

            uint queueFamilyCount = 0;
            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, null);

            VkQueueFamilyProperties* queueFamilies = stackalloc VkQueueFamilyProperties[(int)queueFamilyCount];
            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilies);

            for (int i = 0; i < queueFamilyCount; i++)
            {
                var queueFamily = queueFamilies[i];
                if ((queueFamily.queueFlags & VkQueueFlags.VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    indices.graphicsFamily = i;
                }

                VkBool32 presentSupport = false;
                VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, (uint)i, surface, &presentSupport));

                if (presentSupport)
                {
                    indices.presentFamily = i;
                }

                if (indices.IsGraphicsComplete())
                {
                    break;
                }
            }

            return indices;
        }

        public unsafe static uint FindQueueFamilyIndex(this VkPhysicalDevice device, VkQueueFlags queueFlag)
        {
            uint queueFamilyCount = 0;

            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, null);

            VkQueueFamilyProperties* queueFamilies = stackalloc VkQueueFamilyProperties[(int)queueFamilyCount];

            VulkanNative.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, queueFamilies);

            for (uint i = 0; i < queueFamilyCount; i++)
            {
                var queueFamily = queueFamilies[i];
                if ((queueFamily.queueFlags & queueFlag) != 0)
                {
                    
                    return i;
                }

            }
            return uint.MaxValue;
        }

        public unsafe static void GetQueue(this VkDevice device, int queueFamilyIndex, uint queueIndex, ref VkQueue queue)
        {
            fixed (VkQueue* queuePtr = &queue) 
            {
                VulkanNative.vkGetDeviceQueue(device,(uint) queueFamilyIndex, queueIndex, queuePtr);
            }
        }
    }
}
