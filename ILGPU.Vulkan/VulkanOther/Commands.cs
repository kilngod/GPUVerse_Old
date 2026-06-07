using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
#nullable disable

namespace GPUVulkan
{
    public static unsafe partial class VulkanNative
    {
        private const CallingConvention CallConv = CallingConvention.StdCall;

        public static NativeLibrary NativeLib;

        static VulkanNative()
        {
            
            NativeLib = LoadNativeLibrary();
            System.Runtime.InteropServices.NativeLibrary.SetDllImportResolver(
                typeof(VulkanNative).Assembly,
                ResolveVulkanImport);
            LoadFuncionPointers();
        }

        private static IntPtr ResolveVulkanImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == "vulkan-1.dll" ||
                libraryName == "libvulkan.so" ||
                libraryName == "libvulkan.so.1" ||
                libraryName == "libvulkan.dylib" ||
                libraryName == "__Internal")
            {
                return NativeLib.NativeHandle;
            }

            return IntPtr.Zero;
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
                return "libvulkan.dylib";
            }
            else if (RuntimeInformation.RuntimeIdentifier.ToLower().IndexOf("ios") >= 0)
            {
                // note for iOS we will have to rewrite the vulkan code generator to use static linking or setup the library as a framework package
                // the iphone store does not allow dynamically linked libraries.
                throw new Exception("iOS does not directly support dynamic loading of DLLs, must be made static or added to a framework package.");
                /*
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dropPoint = "GPUMauiApp.app";
               // appDirectory = appDirectory.Substring(0, appDirectory.IndexOf(dropPoint) - dropPoint.Length);
                string dylibPath = System.IO.Path.Combine(appDirectory, "libMoltenVK.dylib");
                //return dylibPath;
                return "libMoltenVK.dylib";
                */
            }
            else if (RuntimeInformation.RuntimeIdentifier.ToLower().IndexOf("tvos") >= 0)
            {
                return "libMoltenVK.dylib";
            }
            else if (RuntimeInformation.RuntimeIdentifier.ToLower().IndexOf("maccatalyst") >=0)
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dropPoint = "maccatalyst-x64/";
                appDirectory = appDirectory.Substring(0, appDirectory.IndexOf(dropPoint)+ dropPoint.Length);
                string dylibPath = System.IO.Path.Combine(appDirectory, "Platforms/MacCatalyst/libMoltenVK.dylib");
                return dylibPath;
                // relative path does not work :(
                //  return "./libMoltenVK.dylib";

            }
            throw new Exception("Application Type Unknown");
        }
    }
}
