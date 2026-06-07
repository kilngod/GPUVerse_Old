using GPUVulkan;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Vulkan;
using System.Runtime.InteropServices;
using System.Text;

const int Width = 3200;
const int Height = 2400;
const int WorkgroupSize = 32;
const int PixelFloats = 4;
const int PixelCount = Width * Height;

var instance = VkInstance.Null;
try
{
    instance = CreateInstance();

    using var context = Context.Create(builder => builder.Vulkan(instance));
    if (context.GetVulkanDevices().Count == 0)
        throw new NotSupportedException("No Vulkan compute devices were found.");

    using var accelerator = context.CreateVulkanAccelerator(0);
    Console.WriteLine($"Using Vulkan device: {accelerator.Device.Name}");

    var spirv = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Shaders", "comp.spv"));
    var compiledKernel = new SpirvVulkanKernel(
        "main",
        spirv,
        descriptorBindingCount: 1);
    var kernel = (VulkanKernel)accelerator.LoadCompiledKernel(compiledKernel);

    using var imageBuffer = (VulkanMemoryBuffer)accelerator.AllocateRaw(
        PixelCount,
        sizeof(float) * PixelFloats);

    kernel.LaunchStorageBufferKernel(
        (VulkanStream)accelerator.DefaultStream,
        new Index3D(
            Width / WorkgroupSize,
            Height / WorkgroupSize,
            1),
        imageBuffer);

    accelerator.Synchronize();

    ReadOnlySpan<float> pixels;
    unsafe
    {
        pixels = MemoryMarshal.Cast<byte, float>(
            new ReadOnlySpan<byte>(
                imageBuffer.NativePtr.ToPointer(),
                checked((int)imageBuffer.LengthInBytes)));
    }

    var center = ((Height / 2) * Width + (Width / 2)) * PixelFloats;
    Console.WriteLine(
        $"Center pixel RGBA: {pixels[center]:F4}, " +
        $"{pixels[center + 1]:F4}, " +
        $"{pixels[center + 2]:F4}, " +
        $"{pixels[center + 3]:F4}");
    Console.WriteLine("Vulkan runtime shader launch completed.");
}
finally
{
    if (instance != VkInstance.Null)
    {
        unsafe
        {
            VulkanNative.vkDestroyInstance(instance, null);
        }
    }
}

static unsafe VkInstance CreateInstance()
{
    var availableExtensions = EnumerateInstanceExtensions();
    var requestedExtensions = new List<string>();
    var flags = VkInstanceCreateFlags.None;

    AddExtensionIfAvailable(
        requestedExtensions,
        availableExtensions,
        VulkanNative.VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);

    if (availableExtensions.Contains(
        VulkanNative.VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME))
    {
        requestedExtensions.Add(
            VulkanNative.VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);
        flags |= VkInstanceCreateFlags
            .VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;
    }

    var appName = Encoding.UTF8.GetBytes("ILGPU.VulkanApp\0");
    var engineName = Encoding.UTF8.GetBytes("ILGPU.Vulkan\0");
    var extensionNameBytes = requestedExtensions
        .Select(extension => Encoding.UTF8.GetBytes(extension + '\0'))
        .ToArray();
    var extensionNameHandles = new GCHandle[extensionNameBytes.Length];

    try
    {
        for (int i = 0; i < extensionNameBytes.Length; ++i)
            extensionNameHandles[i] = GCHandle.Alloc(
                extensionNameBytes[i],
                GCHandleType.Pinned);

        fixed (byte* appNamePtr = appName)
        fixed (byte* engineNamePtr = engineName)
        {
            var extensionNamePtrs = stackalloc byte*[extensionNameBytes.Length];
            for (int i = 0; i < extensionNameBytes.Length; ++i)
                extensionNamePtrs[i] =
                    (byte*)extensionNameHandles[i].AddrOfPinnedObject();

            var applicationInfo = new VkApplicationInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
                pApplicationName = appNamePtr,
                applicationVersion = Version(1, 0, 0),
                pEngineName = engineNamePtr,
                engineVersion = Version(1, 0, 0),
                apiVersion = Version(1, 2, 0),
            };

            var createInfo = new VkInstanceCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                flags = flags,
                pApplicationInfo = &applicationInfo,
                enabledExtensionCount = (uint)extensionNameBytes.Length,
                ppEnabledExtensionNames = extensionNameBytes.Length == 0
                    ? null
                    : extensionNamePtrs,
            };

            VkInstance instance = default;
            ThrowIfFailed(
                VulkanNative.vkCreateInstance(&createInfo, null, &instance));
            return instance;
        }
    }
    finally
    {
        foreach (var handle in extensionNameHandles)
        {
            if (handle.IsAllocated)
                handle.Free();
        }
    }
}

static unsafe HashSet<string> EnumerateInstanceExtensions()
{
    uint count = 0;
    ThrowIfFailed(
        VulkanNative.vkEnumerateInstanceExtensionProperties(null, &count, null));

    var extensions = new VkExtensionProperties[count];
    fixed (VkExtensionProperties* extensionsPtr = extensions)
    {
        ThrowIfFailed(
            VulkanNative.vkEnumerateInstanceExtensionProperties(
                null,
                &count,
                extensionsPtr));
    }

    var result = new HashSet<string>(StringComparer.Ordinal);
    for (int i = 0; i < extensions.Length; ++i)
    {
        fixed (byte* extensionNamePtr = extensions[i].extensionName)
            result.Add(GetString(extensionNamePtr));
    }
    return result;
}

static void AddExtensionIfAvailable(
    List<string> requestedExtensions,
    HashSet<string> availableExtensions,
    string extension)
{
    if (availableExtensions.Contains(extension))
        requestedExtensions.Add(extension);
}

static uint Version(uint major, uint minor, uint patch) =>
    (major << 22) | (minor << 12) | patch;

static void ThrowIfFailed(VkResult result)
{
    if (result < VkResult.VK_SUCCESS)
        throw new InvalidOperationException($"Vulkan call failed: {result}");
}

static unsafe string GetString(byte* stringStart)
{
    int characters = 0;
    while (stringStart[characters] != 0)
        ++characters;
    return Encoding.UTF8.GetString(stringStart, characters);
}

sealed class SpirvVulkanKernel(
    string kernelName,
    byte[] binary,
    uint descriptorBindingCount) : VulkanCompiledKernel(
        Guid.NewGuid(),
        kernelName,
        CompiledKernelType.Grouped,
        CompiledKernelSharedMemoryMode.Static,
        descriptorBindingCount: descriptorBindingCount)
{
    public override ReadOnlyMemory<byte> GetCompiledBinary() => binary;

    public override ReadOnlyMemory<byte> GetCompiledBinaryOrDefault() => binary;
}
