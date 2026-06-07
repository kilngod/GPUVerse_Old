# Handoff: ILGPU Vulkan Runtime Progress - 2026-06-07

## Summary

Today we built the first working ILGPU-style Vulkan runtime backend in
`ILGPU.Vulkan/Runtime/Vulkan` and verified it by launching the existing
`WinVulkanApp` compute shader from a new console app, `ILGPU.VulkanApp`.

The Vulkan runtime is still early, but it now has enough structure to:

- Discover Vulkan devices through an existing `VkInstance`.
- Create a Vulkan-backed ILGPU accelerator.
- Allocate host-visible Vulkan storage buffers.
- Load a SPIR-V compute shader as a Vulkan kernel.
- Own descriptor/pipeline layout/pipeline resources.
- Launch kernels whose arguments are storage buffers, with optional scalar
  payloads through push constants.

The verified sample ran successfully on Apple M1 Pro through the local Vulkan
stack.

## Verified Output

Command:

```bash
dotnet run --project ILGPU.VulkanApp/ILGPU.VulkanApp.csproj --no-build
```

Output:

```text
Using Vulkan device: Apple M1 Pro
Center pixel RGBA: 0.1382, 0.0573, 0.0000, 1.0000
Vulkan runtime shader launch completed.
```

Build command used repeatedly:

```bash
dotnet build ILGPU.Vulkan/ILGPU.Vulkan.csproj --no-restore
dotnet build ILGPU.VulkanApp/ILGPU.VulkanApp.csproj --no-restore
```

Both builds succeeded. `ILGPU.VulkanApp` builds with zero warnings. The Vulkan
library build still has six pre-existing warnings from older `VulkanPlatform`
and `VulkanOther` files, not from the new runtime files.

## New Runtime Files

Created under `ILGPU.Vulkan/Runtime/Vulkan`:

- `VulkanAPI.cs`
- `VulkanAccelerator.cs`
- `VulkanAcceleratorCapabilities.cs`
- `VulkanCompiledKernel.cs`
- `VulkanContextExtensions.cs`
- `VulkanDevice.cs`
- `VulkanException.cs`
- `VulkanKernel.cs`
- `VulkanMemoryBuffer.cs`
- `VulkanProfilingMarker.cs`
- `VulkanStream.cs`

All new runtime code uses namespace:

```csharp
ILGPU.Runtime.Vulkan
```

This was intentional so the folder can eventually move into ILGPU proper with
minimal namespace churn.

## Project Wiring

`ILGPU.Vulkan/ILGPU.Vulkan.csproj` now references the local ILGPU source tree:

```xml
<ProjectReference Include="..\..\ILGPU\Src\ILGPU\ILGPU.csproj" />
```

This is why builds can write artifacts under:

```text
/Users/kilngod/Projects/ILGPU/Bin/Debug/net10.0
```

The temporary backend uses `AcceleratorType.None` because upstream ILGPU does
not yet have a `Vulkan` accelerator enum value. This is a known integration
blocker for eventual upstreaming.

## Device and Context

`VulkanContextExtensions` adds:

- `Context.Builder.Vulkan(VkInstance instance)`
- `Context.Builder.Vulkan(VkInstance instance, Predicate<VulkanDevice>)`
- `Context.GetVulkanDevice(int)`
- `Context.GetVulkanDevices()`
- `Context.CreateVulkanAccelerator(int)`

Because this backend currently lives outside ILGPU proper, the builder extension
uses reflection to access the internal `Context.Builder.DeviceRegistry`.

When moved into ILGPU, replace this reflection bridge with direct registry
access.

`VulkanDevice` now:

- Wraps a Vulkan physical device.
- Queries physical device properties and memory properties.
- Finds a compute queue family.
- Creates a logical device and compute queue.
- Exposes device limits such as:
  - `MaxNumThreadsPerGroup`
  - `MaxSharedMemoryPerGroup`
  - `MaxConstantMemory`
  - `MaxPushConstantSize`

## Capabilities

`VulkanAcceleratorCapabilities` now queries Vulkan feature structures through
`vkGetPhysicalDeviceFeatures2`.

Wrapped feature query support includes:

- Core `VkPhysicalDeviceFeatures`
- `VkPhysicalDeviceShaderFloat16Int8Features`
- `VkPhysicalDeviceShaderBfloat16FeaturesKHR`
- `VkPhysicalDeviceShaderFloat8FeaturesEXT`

The implementation only chains optional feature structs when their supporting
API version or device extension is present.

## VulkanAPI Wrapper

`VulkanAPI.cs` is the runtime-facing Vulkan API wrapper. The goal is for runtime
code to use `VulkanAPI`, not call `VulkanNative` directly.

Current wrapper coverage includes:

- Vulkan version helpers
- Error handling through `VulkanException`
- Physical-device enumeration
- Device-extension enumeration
- Physical-device properties, memory properties, and features
- Compute queue-family lookup
- Logical device create/destroy/wait-idle
- Queue get/wait/submit
- Command pool creation/destruction
- Command buffer allocation/free/begin/end
- Fence create/wait/destroy
- Compute command recording:
  - bind pipeline
  - bind descriptor sets
  - push constants
  - dispatch
- Descriptor set layout/pool/set allocation
- Storage-buffer descriptor updates
- Pipeline layout creation with optional push constants
- Compute pipeline creation/destruction
- Buffer creation/destruction
- Buffer memory requirements
- Device memory allocate/free/map/unmap/bind
- Memory type lookup
- Shader module creation/destruction

## Streams

`VulkanStream` now owns:

- `VkQueue`
- queue-family index
- `VkCommandPool`

It can:

- Allocate command buffers from its pool.
- Free command buffers back to its pool.
- Synchronize via `vkQueueWaitIdle`.
- Destroy its command pool during disposal after synchronizing.

This mirrors the current early command-buffer lifecycle used by kernel launch.

## Memory Buffers

`VulkanMemoryBuffer` now:

- Inherits `MemoryBuffer`.
- Creates a `VkBuffer` with storage-buffer and transfer usage.
- Allocates host-visible, host-coherent memory.
- Binds the memory to the buffer.
- Maps the memory and exposes the mapped native pointer through `NativePtr`.
- Implements early CPU-side memset/copy behavior through mapped memory.
- Unmaps/frees memory and destroys the buffer during disposal.

This is intentionally simple and host-visible first. Device-local staging and
explicit transfer paths are still future work.

## Kernel Ownership

`VulkanKernel` now owns:

- `VkShaderModule`
- `VkDescriptorSetLayout`
- `VkDescriptorPool`
- `VkDescriptorSet[]`
- `VkPipelineLayout`
- `VkPipeline`

It disposes resources in reverse order:

1. Compute pipeline
2. Pipeline layout
3. Descriptor pool
4. Descriptor set layout
5. Shader module

`VulkanCompiledKernel` carries early metadata:

- `DescriptorBindingCount`
- `ScalarArgumentSizeInBytes`

The pipeline layout reserves a compute-stage push-constant range when
`ScalarArgumentSizeInBytes > 0`.

## First Kernel Launch Path

`VulkanKernel` has a first explicit launch path:

```csharp
LaunchStorageBufferKernel(
    VulkanStream stream,
    Index3D groupCount,
    params VulkanMemoryBuffer[] buffers)
```

and an overload for scalar push-constant bytes:

```csharp
LaunchStorageBufferKernel(
    VulkanStream stream,
    Index3D groupCount,
    ReadOnlySpan<byte> scalarArguments,
    params VulkanMemoryBuffer[] buffers)
```

Current restrictions:

- Arguments must be `VulkanMemoryBuffer` storage buffers.
- Buffer count must match `DescriptorBindingCount`.
- All buffers and stream must belong to the same accelerator.
- Empty/disposed buffers are rejected.
- Scalar argument payload must match `ScalarArgumentSizeInBytes`.
- Scalar payload must be 4-byte aligned.
- Scalar payload must fit within `VulkanDevice.MaxPushConstantSize`.

The launch path:

1. Updates the kernel-owned descriptor set.
2. Allocates a command buffer from the stream command pool.
3. Begins command recording.
4. Binds compute pipeline.
5. Binds descriptor sets.
6. Pushes scalar bytes if present.
7. Records dispatch.
8. Ends command recording.
9. Creates a fence.
10. Submits to the stream queue.
11. Waits for the fence.
12. Destroys the fence.
13. Frees the command buffer.

This is synchronous by design for the first working path.

## Console App Verification

Created/updated `ILGPU.VulkanApp`:

- Added project reference to `ILGPU.Vulkan`.
- Enabled unsafe code.
- Copied `WinVulkanApp/Shaders/comp.spv` to
  `ILGPU.VulkanApp/Shaders/comp.spv`.
- Configured the shader file to copy to output.
- Replaced hello-world `Program.cs` with an end-to-end launch sample.

The sample:

1. Creates a minimal Vulkan instance.
2. Enables portability enumeration when available.
3. Creates an ILGPU context using `builder.Vulkan(instance)`.
4. Creates the first Vulkan accelerator.
5. Reads `Shaders/comp.spv`.
6. Wraps it in a small `SpirvVulkanKernel` subclass.
7. Loads it with `accelerator.LoadCompiledKernel`.
8. Allocates one `VulkanMemoryBuffer`.
9. Dispatches `100 x 75 x 1` workgroups for the shader's
   `3200 x 2400` image and `32 x 32` local size.
10. Reads the mapped buffer and prints the center pixel.

## Git / Workspace Notes

`.DS_Store` was added to `.gitignore`.

Several previously tracked `.DS_Store` files were removed from the git index
and currently show as staged deletions. Do not restore them.

Current notable untracked area:

```text
ILGPU.Vulkan/Runtime/
ILGPU.VulkanApp/
```

`GPUVerse.sln` is modified because the new app was created/added outside the
runtime edits.

## Known Gaps / Next Work

Important next steps:

- Add a real `AcceleratorType.Vulkan` upstream in ILGPU.
- Replace `VulkanContextExtensions` reflection with direct `DeviceRegistry`
  access once moved into ILGPU proper.
- Integrate with ILGPU's real launcher/delegate generation path instead of the
  explicit `LaunchStorageBufferKernel` helper.
- Add SPIR-V reflection or compiler metadata so descriptor bindings, scalar
  payload sizes, and entry points do not have to be manually supplied.
- Add uniform-buffer fallback for scalar payloads that exceed push-constant
  limits.
- Add asynchronous launch semantics and reusable command-buffer/fence strategy.
- Add device-local memory with staging transfers instead of host-visible-only
  buffers.
- Add descriptor-set caching or per-launch descriptor management for concurrent
  launches.
- Add buffer memory barriers where needed for non-host-coherent or async paths.
- Add tests or small verification samples beyond the Mandelbrot shader.
- Consider splitting low-level Vulkan instance bootstrap helpers out of the
  sample app and into reusable runtime/platform utilities.

## Pre-existing Build Warnings

The Vulkan library build still reports the same older warnings from
`VulkanPlatform`/`VulkanOther`:

- `VulkanHelpers.cs`: possible null value warning
- `VulkanResourcePool.cs`: non-nullable `sharedResources`
- `VulkanInstance.cs`: possible null dereference
- `VulkanResourcePool.cs`: unused field
- `NativeLibrary.cs`: unused field
- `VulkanSupport.cs`: unused field

These warnings predate the new runtime work and were not introduced by the
`Runtime/Vulkan` files.
