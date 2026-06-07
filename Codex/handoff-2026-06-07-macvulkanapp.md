# GPUVerse MacVulkanApp Handoff

Date: 2026-06-07

## Context

This handoff captures the `MacVulkanApp` .NET 10/macOS Vulkan migration and troubleshooting state.

The app started as an older Xamarin.Mac-style project. It has now been moved to SDK-style .NET 10 macOS, updated to use the `ILGPU.Vulkan` project, and verified by the user in Rider to build, run, display the expected triangle, and terminate correctly when the red window close button is clicked.

Important workflow note: use Rider for `MacVulkanApp` build/run validation. Rider appears to pass additional macOS build/run switches that the plain CLI path does not fully mirror. During Codex verification, `dotnet build MacVulkanApp/MacVulkanApp.csproj` compiled successfully but repeatedly paused in the macOS bundle/signing tail after producing `MacVulkanApp.dll`.

## Current Status

- `MacVulkanApp` builds in Rider.
- `MacVulkanApp` runs in Rider.
- The app displays the expected triangle.
- Closing the window with the red close button now terminates the app process, so Rider no longer thinks the app is still running.
- The previous code-signing, missing Vulkan library, and missing shader file failures have been addressed.

## Project Upgrade

`MacVulkanApp/MacVulkanApp.csproj` was converted from the old Xamarin.Mac project format to SDK-style:

- Targets `net10.0-macos`.
- Uses `Microsoft.NET.Sdk`.
- References `..\ILGPU.Vulkan\ILGPU.Vulkan.csproj`.
- Enables unsafe code.
- Uses ad-hoc signing for local debug runs:

```xml
<EnableCodeSigning>true</EnableCodeSigning>
<CodesignKey>-</CodesignKey>
```

- Includes a Debug-only target, `AdHocSignDebugAppBundle`, that re-signs the local `.app` bundle with:

```text
/usr/bin/codesign --force --deep --sign -
```

This keeps the app as a local macOS developer app, not an Apple Store/provisioning app.

## Rider Launch Path

Rider originally looked for the app at the old upgraded-project path:

```text
MacVulkanApp/MacVulkanApp.app/Contents/MacOS/MacVulkanApp
```

The .NET 10 macOS output path is:

```text
MacVulkanApp/bin/Debug/net10.0-macos/osx-arm64/MacVulkanApp.app
```

The solution/project metadata was updated so the SDK-style project is treated as a normal .NET SDK project rather than the old Xamarin.Mac project type.

## Code Signing

Several launches failed with:

```text
SIGKILL (Code Signature Invalid)
Namespace CODESIGNING, Code 2, Invalid Page
```

The current app bundle verifies after the ad-hoc signing step:

```text
MacVulkanApp.app: valid on disk
MacVulkanApp.app: satisfies its Designated Requirement
Signature=adhoc
TeamIdentifier=not set
```

## MoltenVK And Vulkan Loading

The machine had older LunarG Vulkan SDK artifacts around, including:

```text
/Users/kilngod/VulkanSDK/1.3.250.1
/usr/local/lib/libMoltenVK.dylib
/usr/local/share/vulkan/icd.d/MoltenVK_icd.json
```

Homebrew Vulkan packages were later installed by the user. The last observed versions were:

```text
molten-vk 1.4.1
vulkan-loader 1.4.350.0
vulkan-tools 1.4.350.0
```

The repository copy at:

```text
MacVulkan/libMoltenVK.dylib
```

was verified as a universal dylib containing:

```text
x86_64 arm64
```

`ILGPU.Vulkan/VulkanOther/Commands.cs` was updated so macOS Vulkan native library resolution can find bundled or Homebrew Vulkan/MoltenVK libraries. The lookup now supports explicit environment overrides and searches app bundle locations such as:

- `Contents/Resources/libMoltenVK.dylib`
- `Contents/MonoBundle/libMoltenVK.dylib`
- `Contents/Frameworks/libMoltenVK.dylib`
- Homebrew loader locations such as `/opt/homebrew/lib/libvulkan.dylib`

## Metal Surface Update

The Vulkan surface path was moved from deprecated MoltenVK-specific surface extensions to the current Metal surface extension:

```text
VK_EXT_metal_surface
vkCreateMetalSurfaceEXT
VkMetalSurfaceCreateInfoEXT
```

Files involved:

- `ILGPU.Vulkan/VulkanPlatform/VulkanSurface.cs`
- `ILGPU.Vulkan/VulkanPlatform/VulkanInstance.cs`
- `MacVulkanApp/GameViewController.cs`

`GameViewController` now creates the Vulkan surface from the `MTKView` backing Metal layer.

## Shader Resource Fix

One launch failed with:

```text
Could not find file '.../MacVulkanApp.app/Contents/Resourcesvert.spv'
```

The issue was string concatenation missing a path separator. `GameViewController` now resolves shader files with `Path.Combine(...)` and a `GetResourceFolder()` helper.

The app bundle was verified to contain:

```text
Contents/Resources/vert.spv
Contents/Resources/frag.spv
Contents/Resources/comp.spv
```

## Window Close / Rider Run State

After the triangle rendered correctly, closing the window left Rider thinking the app was still running. This was normal AppKit behavior: closing the last window does not automatically terminate the application process.

`MacVulkanApp/AppDelegate.cs` now overrides:

```csharp
public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
{
    return true;
}
```

The user verified this fixed the Rider run-state issue.

## Package Cleanup

`MoltenVKGen/MoltenVKGen.csproj` no longer references the unnecessary vulnerable identity package chain that included:

```text
Microsoft.Identity.Client
```

`MacVulkanApp/packages.config` was removed because the app is now SDK-style and no longer uses the old packages.config restore model.

## Known Warnings / Notes

- Plain CLI `dotnet build` for `MacVulkanApp` is not currently the trusted verification path. Prefer Rider for this app.
- CLI build emitted existing warnings in `ComputeSample.cs` about unused fields and an obsolete `NSData.Save(string, bool)` call.
- CLI restore/build may warn that NuGet vulnerability data could not be fetched when network access is unavailable.
- Several `.DS_Store` files were modified or created by macOS/Finder and are unrelated to the code migration.

## Useful Follow-Ups

- Consider cleaning up or ignoring `.DS_Store` files.
- Consider modernizing `ComputeSample` output saving to use `NSUrl` instead of the obsolete `NSData.Save(string, bool)` overload.
- Consider adding a small helper in `ILGPU.Vulkan` for app-bundle resource lookup if more macOS samples need bundled resources.
- If moving beyond local debug runs, revisit signing, entitlements, bundle identifier, and packaging separately from the current ad-hoc developer setup.
