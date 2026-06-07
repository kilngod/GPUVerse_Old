using System;
using System.Diagnostics;
using System.IO;
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

            string configuredLibrary = Environment.GetEnvironmentVariable("GPUVERSE_VULKAN_LIBRARY");
            if (string.IsNullOrWhiteSpace(configuredLibrary))
            {
                configuredLibrary = Environment.GetEnvironmentVariable("VULKAN_LIBRARY");
            }

            if (!string.IsNullOrWhiteSpace(configuredLibrary))
            {
                return configuredLibrary;
            }

            string runtimeIdentifier = RuntimeInformation.RuntimeIdentifier.ToLowerInvariant();

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
            else if (runtimeIdentifier.IndexOf("ios") >= 0)
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
            else if (runtimeIdentifier.IndexOf("tvos") >= 0)
            {
                return "libMoltenVK.dylib";
            }
            else if (runtimeIdentifier.IndexOf("maccatalyst") >=0)
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dropPoint = "maccatalyst-x64/";
                appDirectory = appDirectory.Substring(0, appDirectory.IndexOf(dropPoint)+ dropPoint.Length);
                string dylibPath = System.IO.Path.Combine(appDirectory, "Platforms/MacCatalyst/libMoltenVK.dylib");
                return dylibPath;
                // relative path does not work :(
                //  return "./libMoltenVK.dylib";

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GetMacOSVulkanName();
            }
            throw new Exception("Application Type Unknown");
        }

        private static string GetMacOSVulkanName()
        {
            foreach (string candidate in GetMacOSVulkanCandidates())
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return "libMoltenVK.dylib";
        }

        private static string[] GetMacOSVulkanCandidates()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string contentsDirectory = GetMacOSBundleContentsDirectory(baseDirectory);

            return new[]
            {
                Path.Combine(baseDirectory, "libMoltenVK.dylib"),
                Path.Combine(contentsDirectory, "MonoBundle", "libMoltenVK.dylib"),
                Path.Combine(contentsDirectory, "Frameworks", "libMoltenVK.dylib"),
                Path.Combine(contentsDirectory, "Resources", "libMoltenVK.dylib"),
                Path.Combine(baseDirectory, "libvulkan.dylib"),
                Path.Combine(contentsDirectory, "MonoBundle", "libvulkan.dylib"),
                Path.Combine(contentsDirectory, "Frameworks", "libvulkan.dylib"),
                Path.Combine(contentsDirectory, "Resources", "libvulkan.dylib"),
                "/opt/homebrew/lib/libvulkan.dylib",
                "/usr/local/lib/libvulkan.dylib"
            };
        }

        private static string GetMacOSBundleContentsDirectory(string baseDirectory)
        {
            DirectoryInfo directory = new DirectoryInfo(baseDirectory);

            while (directory != null && directory.Name != "Contents")
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? baseDirectory;
        }
    }
}
