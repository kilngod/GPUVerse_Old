using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#nullable disable



namespace GPUVulkan
{
    
    /*
    public static unsafe partial class VulkanNative
    {
        private const CallingConvention CallConv = CallingConvention.StdCall;

        public static NativeLibrary NativeLib;

        static VulkanNative()
        {
            
            NativeLib = LoadNativeLibrary();
          
        }

        private static NativeLibrary LoadNativeLibrary()
        {
            return NativeLibrary.Load(GetVulkanName());
        }

        private static string GetVulkanName(string vulkanLibrary = "")
        {

            if (!string.IsNullOrEmpty(vulkanLibrary))
            {
                return vulkanLibrary;
            }

            if (RuntimeInformation.OSDescription.ToLower().IndexOf("android") > 0)// android
            {
                return "libvulkan.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "vulkan-1.dll";
            }            
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "libvulkan.so.1";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "libMoltenVK.dylib";
            }
       // relative path does not work :(
                //  return "./libMoltenVK.dylib";

            }
            throw new Exception("Application Type Unknown");
        }
    }*/
}
