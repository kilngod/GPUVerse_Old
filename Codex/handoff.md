# GPUVerse Codex Handoff

Date: 2026-06-07

## Context

This thread focused on moving `WinVulkanApp` toward .NET 10 and switching it from the older `GPUVulkan` binding project to the newer `ILGPU.Vulkan` binding project.

The repository is a Git working tree connected to:

```text
https://github.com/kilngod/GPUVerse.git
```

Git commands from Codex may require marking the repo safe because the sandbox user differs from the repo owner.

## .NET Setup

The machine has .NET SDK `10.0.204` installed.

The solution is mixed:

- `GPUMauiLib` targets `net10.0-*`.
- `MoltenVKGen` targets `net10.0`.
- `WinVulkanApp` was upgraded from `net8.0-windows` to `net10.0-windows`.
- Some other MAUI projects still target `net8.0-*`.

No `global.json` was found, so SDK selection comes from the installed machine SDK.

## WinVulkanApp Changes

`WinVulkanApp` now:

- Targets `net10.0-windows`.
- Has unsafe compilation enabled with `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>`.
- References `..\ILGPU.Vulkan\ILGPU.Vulkan.csproj` instead of `..\GPUVulkan\GPUVulkan.csproj`.
- Builds successfully with:

```powershell
dotnet build .\WinVulkanApp\WinVulkanApp.csproj
```

## ILGPU.Vulkan Changes

`ILGPU.Vulkan` now:

- Has unsafe compilation enabled.
- Adds generated compatibility shims in `VulkanGenerated/TypeAliases.cs`:
  - `_screen_buffer` opaque QNX handle type.
  - No-op `VulkanNative.LoadFuncionPointers(...)` compatibility method for shared platform code.
- Registers a `DllImportResolver` in `VulkanOther/Commands.cs` so generated `[DllImport]` calls resolve to the same native Vulkan library handle that the old loader found.
- Loads debug-utils extension create/destroy functions through `vkGetInstanceProcAddr` in `VulkanPlatform/VulkanDebug.cs`.

## Vulkan Fixes

Several validation issues were fixed:

- `VK_SUBOPTIMAL_KHR` and `VK_ERROR_OUT_OF_DATE_KHR` from swapchain acquire/present are handled as non-fatal frame-skip states in `ILGPU.Vulkan/VulkanPlatform/VulkanDraw.cs`.
- The WinForms main loop now checks for window closing immediately after `Application.DoEvents()` to avoid drawing one extra frame during shutdown.
- `WinVulkanRenderer` no longer creates two `VkSurfaceKHR` handles. `CreateSurface()` is idempotent, `SetupPipeline()` no longer recreates/reconfigures the surface/device, and cleanup resets the surface to `VkSurfaceKHR.Null`.
- `ComputeSample` now destroys compute shader modules after pipeline creation.
- `ComputeSample.CleanUp()` destroys compute-owned Vulkan resources before device shutdown:
  - compute pipeline
  - pipeline layout
  - descriptor pool
  - descriptor set layout
  - storage buffer
  - device memory
  - command pool, which also releases its command buffers
- `VulkanComputing.SubmitAndWait()` now destroys its temporary fence after waiting.

## App-Side Vulkan Contract Fixes

`ComputeSample` had a few invalid or missing `sType` values that `ILGPU.Vulkan` exposed via validation:

- `VkSubmitInfo` now uses `VK_STRUCTURE_TYPE_SUBMIT_INFO`.
- `VkWriteDescriptorSet` now sets `VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET`.
- `VkPipelineLayoutCreateInfo` now uses `VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO`.

## Output Path Fix

`SaveRenderedImage()` was failing with a generic GDI+ error when trying to save into the Windows Downloads folder from the Codex sandbox context.

`WinIO.WritableOutputFolderPath()` now:

- Tries Downloads first.
- Verifies write access with a probe file.
- Falls back to `AppContext.BaseDirectory\Output`.

`SaveRenderedImage()` saves `Mandelbrot.bmp` through a `FileStream` with explicit `ImageFormat.Bmp`.

Observed output location during testing:

```text
WinVulkanApp\bin\Debug\net10.0-windows\Output\Mandelbrot.bmp
```

## Validation Layer Note

The remaining message:

```text
UNASSIGNED-CreateInstance-status-message(INFO / SPEC)
Khronos Validation Layer Active
Settings File: C:\Users\James\AppData\Local\LunarG\vkconfig\override\vk_layer_settings.txt
```

is informational, not a Vulkan misuse error. It means Vulkan Configurator / VkConfig has validation enabled on the machine.

## Current Status

Last verified command:

```powershell
dotnet build .\WinVulkanApp\WinVulkanApp.csproj
```

Result: build succeeded with `0` errors.

The app has been observed to open the triangle window and draw. User later reported only the validation-layer startup information message, not a remaining object-lifetime error.

## Useful Follow-Ups

- Run `WinVulkanApp` manually, close the window, and confirm validation no longer reports leaked device children or leaked surfaces.
- Consider making `ComputeSample` implement `IDisposable` so cleanup is harder to forget.
- Consider moving the compute/image-save demo behind a flag if the desired first-run behavior is only triangle rendering.
- Consider fixing the existing nullable warnings in `ILGPU.Vulkan` once the Vulkan behavior is stable.
