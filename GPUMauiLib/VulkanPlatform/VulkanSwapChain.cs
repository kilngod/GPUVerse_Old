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
using System.Runtime.CompilerServices;
using GPUVulkan;


namespace VulkanPlatform { 

    public struct SwapChainSupportDetails
    {
        public VkSurfaceCapabilitiesKHR capabilities;
        public VkSurfaceFormatKHR[] formats;
        public VkPresentModeKHR[] presentModes;
    }


    public static class VulkanSwapChain
	{
       

        public unsafe static void CreateSwapChain(this IVulkanRenderer renderer, VkSurfaceKHR surface, ref VkSwapchainKHR swapChain )
        {


#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSwapChain.CreateSwapChain");
#endif

            renderer.SwapChainDetails = QuerySwapChainSupport(renderer.VSupport.PhysicalDevice, surface);

            renderer.SurfaceFormat = ChooseSwapSurfaceFormat(renderer.SwapChainDetails.formats);
            renderer.PresentMode = ChooseSwapPresentMode(renderer.SwapChainDetails.presentModes);
            
            renderer.SurfaceExtent2D = ChooseSwapExtent(renderer.SwapChainDetails.capabilities, renderer.SurfaceExtent2D.width, renderer.SurfaceExtent2D.height);

            uint imageCount = renderer.SwapChainDetails.capabilities.minImageCount + 1;
            if (renderer.SwapChainDetails.capabilities.maxImageCount > 0 && imageCount > renderer.SwapChainDetails.capabilities.maxImageCount)
            {
                imageCount = renderer.SwapChainDetails.capabilities.maxImageCount;
            }

            VkSwapchainCreateInfoKHR createInfo = new VkSwapchainCreateInfoKHR();
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
            createInfo.surface = surface;
            createInfo.minImageCount = imageCount;
            createInfo.imageFormat = renderer.SurfaceFormat.format;
            createInfo.imageColorSpace = renderer.SurfaceFormat.colorSpace;
            createInfo.imageExtent = renderer.SurfaceExtent2D;
            createInfo.imageArrayLayers = 1;
            createInfo.imageUsage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT;

            renderer.FamilyIndices = renderer.VSupport.PhysicalDevice.FindGraphicsQueueFamilies(surface);
            uint* queueFamilyIndices = stackalloc uint[] {(uint) renderer.FamilyIndices.graphicsFamily,(uint) renderer.FamilyIndices.presentFamily };

            if (renderer.FamilyIndices.graphicsFamily != renderer.FamilyIndices.presentFamily)
            {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
                createInfo.queueFamilyIndexCount = 2;
                createInfo.pQueueFamilyIndices = queueFamilyIndices;
            }
            else
            {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
                createInfo.queueFamilyIndexCount = 0; //Optional
                createInfo.pQueueFamilyIndices = null; //Optional
            }
            createInfo.preTransform = renderer.SwapChainDetails.capabilities.currentTransform;
            createInfo.compositeAlpha = VkCompositeAlphaFlagsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
            createInfo.presentMode = renderer.PresentMode;
            createInfo.clipped = true;
            createInfo.oldSwapchain = 0;
            //createInfo.flags = VkSwapchainCreateFlagsKHR.VK_SWAPCHAIN_CREATE_PROTECTED_BIT_KHR;

            fixed (VkSwapchainKHR* swapChainPtr = &swapChain)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateSwapchainKHR(renderer.VSupport.Device, &createInfo, null, swapChainPtr));
            }

#if WINDOWS
            //ComObject.As<ISwapChainPanelNative>(renderer.PlatformView);
#endif

            // SwapChain Images
            VulkanNative.vkGetSwapchainImagesKHR(renderer.VSupport.Device, swapChain, &imageCount, null);
            renderer.CreateSwapChainImages(imageCount);
           
            fixed (VkImage* swapChainImagesPtr = &renderer.SwapChainImages[0])
            {
                VulkanNative.vkGetSwapchainImagesKHR(renderer.VSupport.Device, swapChain, &imageCount, swapChainImagesPtr);
            }

     

        }


        public unsafe static void UpdateSwapChainImageSize(this IVulkanRenderer renderer, VkSurfaceKHR surface, ref VkSwapchainKHR swapChain, int width, int height)
        {


#if DEBUG
            VulkanFlowTracer.AddItem("RendererVulkan.UpdateSwapChainImageSize");
#endif

            // update width and height and then update swapchain
            renderer.SurfaceExtent2D = new VkExtent2D(width, height);


            VkSwapchainCreateInfoKHR createInfo = new VkSwapchainCreateInfoKHR();
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
            createInfo.surface = surface;
            createInfo.minImageCount = (uint)renderer.SwapChainImages.Length;
            createInfo.imageFormat = renderer.SurfaceFormat.format;
            createInfo.imageColorSpace = renderer.SurfaceFormat.colorSpace;
            createInfo.imageExtent = renderer.SurfaceExtent2D;
            createInfo.imageArrayLayers = 1;
            createInfo.imageUsage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VkImageUsageFlags.VK_IMAGE_USAGE_TRANSFER_DST_BIT;

            QueueFamilyIndices indices = renderer.VSupport.PhysicalDevice.FindGraphicsQueueFamilies(surface);
            uint* queueFamilyIndices = stackalloc uint[] { (uint) indices.graphicsFamily, (uint) indices.presentFamily };

            if (indices.graphicsFamily != indices.presentFamily)
            {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
                createInfo.queueFamilyIndexCount = 2;
                createInfo.pQueueFamilyIndices = queueFamilyIndices;
            }
            else
            {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
                createInfo.queueFamilyIndexCount = 0; //Optional
                createInfo.pQueueFamilyIndices = null; //Optional
            }
            createInfo.preTransform = renderer.SwapChainDetails.capabilities.currentTransform;
            createInfo.compositeAlpha = VkCompositeAlphaFlagsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
            createInfo.presentMode = renderer.PresentMode;
            createInfo.clipped = true;
            createInfo.oldSwapchain = swapChain;

            fixed (VkSwapchainKHR* swapChainPtr = &swapChain)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateSwapchainKHR(renderer.VSupport.Device, &createInfo, null, swapChainPtr));
            }

            uint imageCount = (uint)renderer.SwapChainImages.Length;
            // need to nuke old images before allocating new images.
            // SwapChain Images
            VulkanNative.vkGetSwapchainImagesKHR(renderer.VSupport.Device, swapChain, &imageCount, null);
            renderer.CreateSwapChainImages(imageCount);

            fixed (VkImage* swapChainImagesPtr = &renderer.SwapChainImages[0])
            {
                VulkanNative.vkGetSwapchainImagesKHR(renderer.VSupport.Device, swapChain, &imageCount, swapChainImagesPtr);
            }



        }


        public static unsafe void CreateImageViews(this IVulkanRenderer renderer)
        {
           
            for (int i = 0; i < renderer.SwapChainImages.Length; i++)
            {
                VkImageViewCreateInfo createInfo = new VkImageViewCreateInfo();
                createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
                createInfo.image = renderer.SwapChainImages[i];
                createInfo.viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;
                createInfo.format = renderer.SurfaceFormat.format;//. swapChainImageFormat;
                createInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                createInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                createInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                createInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                createInfo.subresourceRange.aspectMask = VkImageAspectFlags.VK_IMAGE_ASPECT_COLOR_BIT;
                createInfo.subresourceRange.baseMipLevel = 0;
                createInfo.subresourceRange.levelCount = 1;
                createInfo.subresourceRange.baseArrayLayer = 0;
                createInfo.subresourceRange.layerCount = 1;

                fixed (VkImageView* swapChainImageViewPtr = &renderer.SwapChainImageViews[i])
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkCreateImageView(renderer.VSupport.Device, &createInfo, null, swapChainImageViewPtr));
                }
            }
        }


        public static unsafe SwapChainSupportDetails QuerySwapChainSupport(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
        {
            SwapChainSupportDetails details = new SwapChainSupportDetails();// default;

            // Capabilities
            VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, &details.capabilities));

            // Formats
            uint formatCount;
            VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &formatCount, null));

            if (formatCount != 0)
            {
                details.formats = new VkSurfaceFormatKHR[formatCount];
                fixed (VkSurfaceFormatKHR* formatsPtr = &details.formats[0])
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &formatCount, formatsPtr));
                }
            }

            // Present Modes
            uint presentModeCount;
            VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, null));

            if (presentModeCount != 0)
            {
                details.presentModes = new VkPresentModeKHR[presentModeCount];
                fixed (VkPresentModeKHR* presentModesPtr = &details.presentModes[0])
                {
                    VulkanHelpers.CheckErrors(VulkanNative.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, presentModesPtr));
                }
            }

            return details;
        }


        private static VkSurfaceFormatKHR ChooseSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats)
        {
            foreach (var availableFormat in availableFormats)
            {
                if (availableFormat.format == VkFormat.VK_FORMAT_B8G8R8A8_UNORM && availableFormat.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
                {
                    return availableFormat;
                }
            }

            return availableFormats[0];
        }

        private static VkPresentModeKHR ChooseSwapPresentMode(VkPresentModeKHR[] availablePresentModes)
        {
            foreach (var availablePresentMode in availablePresentModes)
            {
                if (availablePresentMode == VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR)
                {
                    return availablePresentMode;
                }
            }

            return VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
        }

        private static VkExtent2D ChooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities, uint width, uint height)
        {
            if (capabilities.currentExtent.width != uint.MaxValue)
            {
                return capabilities.currentExtent;
            }
            else
            {
                VkExtent2D actualExtent = new VkExtent2D(width, height);

                actualExtent.width = Math.Max(capabilities.minImageExtent.width, Math.Min(capabilities.maxImageExtent.width, actualExtent.width));
                actualExtent.height = Math.Max(capabilities.minImageExtent.height, Math.Min(capabilities.maxImageExtent.height, actualExtent.height));

                return actualExtent;
            }
        }

    }
}

