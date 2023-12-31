using System;
using GPUVulkan;
using System.Diagnostics;

using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
#nullable disable

namespace VulkanPlatform
{
    public unsafe static class VulkanHelpers
    {
        public static void StringListToByteArrary(List<string> strings, ref IntPtr* stackArrayIntPtr)
        {

            for (int i = 0; i < strings.Count; i++)
            {
                stackArrayIntPtr[i] = Marshal.StringToHGlobalAnsi(strings[i]);
            }

        }

        public static void ListToByteArray<T>(List<T> values, ref IntPtr* stackArrayIntPtr)
        {
            for (int i = 0; i < values.Count; i++)
            {
                Marshal.StructureToPtr<T>(values[i], stackArrayIntPtr[i],false);
                
            }
        }

        public static byte* ToPointer(this string text)
        {
            return (byte*)Marshal.StringToHGlobalAnsi(text);
        }

        public static uint Version(uint major, uint minor, uint patch)
        {
            return (major << 22) | (minor << 12) | patch;
        }

        public static VkMemoryType GetMemoryType(this VkPhysicalDeviceMemoryProperties memoryProperties, uint index)
        {
            return (&memoryProperties.memoryTypes_0)[index];
        }

        public static unsafe string GetString(byte* stringStart)
        {
            int characters = 0;
            while (stringStart[characters] != 0)
            {
                characters++;
            }

            return System.Text.Encoding.UTF8.GetString(stringStart, characters);
        }



        [Conditional("DEBUG")]
        public static void CheckErrors(VkResult result)
        {
            if (result != VkResult.VK_SUCCESS)
            {
                throw new InvalidOperationException(result.ToString());
            }
        }


        /*
        // Incase of emergency... not currently required

        public unsafe static TResult ReinterpretCast<TOriginal, TResult>(this TOriginal original)
           where TOriginal : struct
           where TResult : struct
        {
            return *(TResult*)(void*)&original;
        }

        static unsafe TDest ReinterpretCast<TSource, TDest>(TSource source)
        {
            var sourceRef = __makeref(source);
            var dest = default(TDest);
            var destRef = __makeref(dest);
            *(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
            return __refvalue(destRef, TDest);
        }

        */
    }
}

