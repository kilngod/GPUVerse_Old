using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable

namespace GPUVulkan
{
    
    internal static class Libdl
    {
        private const string libraryName = "libSystem.dylib";

        [DllImport(libraryName)]
        public static extern IntPtr dlopen(string fileName, int flags);

        [DllImport(libraryName)]
        public static extern IntPtr dlsym(IntPtr handle, string name);

        [DllImport(libraryName)]
        public static extern int dlclose(IntPtr handle);

        [DllImport(libraryName)]
        public static extern string dlerror();

        public const int RTLD_NOW = 0x002;
    }
}
