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
using System.Diagnostics;
using System.Runtime.InteropServices;
using GPUVulkan;



namespace VulkanPlatform
{
    /// <summary>
    /// Vulkan Surfaces have to be created in a client framework or app.
    /// To make this library compatibile with Maui, Avalonia or direct windows
    /// we don't want to add any additional framework dependencies.
    /// </summary>
    public static class VulkanSurface
    {


        public unsafe static void CreateAndroidSurface(IVulkanSupport support, VkSurfaceKHR* pSurface, IntPtr AndroidWindow)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSurface.CreateAndroidSurface");
#endif
            VkAndroidSurfaceCreateInfoKHR createInfo = new VkAndroidSurfaceCreateInfoKHR()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_ANDROID_SURFACE_CREATE_INFO_KHR,
                window = AndroidWindow,
                pNext = null,
                flags = 0

            };

            VulkanHelpers.CheckErrors(VulkanNative.vkCreateAndroidSurfaceKHR(support.Instance, &createInfo, null, pSurface));
          
        }
      


        public static unsafe void CreateWin32Surface(IVulkanSupport support, VkSurfaceKHR* surfacePtr, IntPtr Hwnd, IntPtr HInstance)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSurface.CreateWin32Surface");
#endif


            VkWin32SurfaceCreateInfoKHR createInfo = new VkWin32SurfaceCreateInfoKHR()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                hwnd = Hwnd,               
                hinstance = HInstance
                //flags = VkWin32SurfaceCreateFlagsKHR. // no current options
            };

    
            VulkanHelpers.CheckErrors(VulkanNative.vkCreateWin32SurfaceKHR(support.Instance, &createInfo, null, surfacePtr));
            
        }

        public static unsafe void CreateiOSMTKViewSurface(IVulkanSupport support, ref VkSurfaceKHR surface, IntPtr mtkViewHandle)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSurface.CreateMTKViewSurface");
#endif
            VkIOSSurfaceCreateInfoMVK surfaceCI = new VkIOSSurfaceCreateInfoMVK()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IOS_SURFACE_CREATE_INFO_MVK,
                // pLayer = view.Handle,
                pNext = null,
                pView = (void*)mtkViewHandle
            };
            //surfaceCI.pLayer = metalLayer.NativePtr.ToPointer();

            fixed (VkSurfaceKHR* surfacePtr = &surface)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateIOSSurfaceMVK(support.Instance, &surfaceCI, null, surfacePtr));
            }
        }




        public static unsafe void CreateMacMTKViewSurface(IVulkanSupport support, ref VkSurfaceKHR surface, IntPtr mtkViewHandle)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSurface.CreateMTKViewSurface");
#endif
            VkMacOSSurfaceCreateInfoMVK surfaceCI = new VkMacOSSurfaceCreateInfoMVK()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_METAL_SURFACE_CREATE_INFO_EXT,
                pView = (void*)mtkViewHandle,
                pNext = null
            };
            //surfaceCI.pLayer = metalLayer.NativePtr.ToPointer();

            fixed (VkSurfaceKHR* surfacePtr = &surface)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateMacOSSurfaceMVK(support.Instance, &surfaceCI, null, surfacePtr));
            }
        }



        public static unsafe void CreateMetalLayerSurface(IVulkanSupport support, ref VkSurfaceKHR surface, IntPtr LayerHandle)
        {
#if DEBUG
            VulkanFlowTracer.AddItem("VulkanSurface.CreateMTKViewSurface");
#endif

            VkMetalSurfaceCreateInfoEXT surfaceCI = new VkMetalSurfaceCreateInfoEXT()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_METAL_IO_SURFACE_INFO_EXT,
                pLayer = LayerHandle,
                pNext = null
            };

            //surfaceCI.pLayer = metalLayer.NativePtr.ToPointer();

            fixed (VkSurfaceKHR* surfacePtr = &surface)
            {
                VulkanHelpers.CheckErrors(VulkanNative.vkCreateMetalSurfaceEXT(support.Instance, &surfaceCI, null, surfacePtr));
            }
        }



    }
}

