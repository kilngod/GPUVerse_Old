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


namespace VulkanPlatform
{
#if DEBUG
    public static class VulkanDebug
    {

        private static VkDebugUtilsMessengerEXT debugMessenger;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate VkResult vkCreateDebugUtilsMessengerEXTDelegate(
            VkInstance instance,
            VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo,
            VkAllocationCallbacks* pAllocator,
            VkDebugUtilsMessengerEXT* pMessenger);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate void vkDestroyDebugUtilsMessengerEXTDelegate(
            VkInstance instance,
            VkDebugUtilsMessengerEXT messenger,
            VkAllocationCallbacks* pAllocator);



        public static unsafe void SetupDebugMessenger(this IVulkanSupport support)
        {
            VulkanFlowTracer.AddItem("VulkanDebug.SetupDebugMessenger");
            fixed (VkDebugUtilsMessengerEXT* debugMessengerPtr = &debugMessenger)
            {
                var funcPtr = VulkanNative.vkGetInstanceProcAddr(support.Instance, "vkCreateDebugUtilsMessengerEXT".ToPointer());
                if (funcPtr != IntPtr.Zero)
                {
                    var createDebugUtilsMessenger = Marshal.GetDelegateForFunctionPointer<vkCreateDebugUtilsMessengerEXTDelegate>(funcPtr);

                    VkDebugUtilsMessengerCreateInfoEXT createInfo = new VkDebugUtilsMessengerCreateInfoEXT()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
                        messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
                        messageType = VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT | VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT,
                        pfnUserCallback = Marshal.GetFunctionPointerForDelegate(DebugCallback),
                        pUserData = null
                    };
                    VulkanHelpers.CheckErrors(createDebugUtilsMessenger(support.Instance, &createInfo, null, debugMessengerPtr));
                }
            }
        }


        private unsafe static VkBool32 DebugCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
            VkDebugUtilsMessageTypeFlagsEXT messageType,
            VkDebugUtilsMessengerCallbackDataEXT pCallbackData,
            void* pUserData)
        {
            VulkanFlowTracer.AddItem("VulkanDebug.DebugCallback");

            if (messageSeverity > VkDebugUtilsMessageSeverityFlagsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT)
            {
                Console.WriteLine
                    ($"{messageSeverity} {messageType}" + Marshal.PtrToStringAnsi((nint)pCallbackData.pMessage));

            }

            return false;
        }

        public static unsafe void DestroyDebugMessenger(this IVulkanSupport support)
        {

            VulkanFlowTracer.AddItem("VulkanDebug.DestroyDebugMessenger");
            var funcPtr = VulkanNative.vkGetInstanceProcAddr(support.Instance, "vkDestroyDebugUtilsMessengerEXT".ToPointer());
            if (funcPtr != IntPtr.Zero)
            {
                var destroyDebugUtilsMessenger = Marshal.GetDelegateForFunctionPointer<vkDestroyDebugUtilsMessengerEXTDelegate>(funcPtr);
                destroyDebugUtilsMessenger(support.Instance, debugMessenger, null);
            }

        }

    }
#endif
}
