# OverlaySharp

[![GitHub Release](https://img.shields.io/github/v/release/Joey0x646576/OverlaySharp?style=flat&logo=github)](https://github.com/Joey0x646576/OverlaySharp/releases/latest)
[![GitHub License](https://img.shields.io/github/license/Joey0x646576/OverlaySharp?style=flat)](https://github.com/Joey0x646576/OverlaySharp/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/download)
[![Platform](https://img.shields.io/badge/Platform-Windows-blue?style=flat&logo=windows)](https://github.com/Joey0x646576/OverlaySharp)
[![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat&logo=csharp)](https://github.com/Joey0x646576/OverlaySharp)
[![Build Status](https://img.shields.io/github/actions/workflow/status/Joey0x646576/OverlaySharp/dotnet-desktop.yml?style=flat&logo=github)](https://github.com/Joey0x646576/OverlaySharp/actions)
[![NuGet](https://img.shields.io/nuget/v/OverlaySharp?style=flat&logo=nuget)](https://www.nuget.org/packages/OverlaySharp/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/OverlaySharp?style=flat&logo=nuget)](https://www.nuget.org/packages/OverlaySharp/)
[![Last Commit](https://img.shields.io/github/last-commit/Joey0x646576/OverlaySharp?style=flat&logo=github)](https://github.com/Joey0x646576/OverlaySharp/commits)

![image](https://i.imgur.com/qw8dZL2.png)
A high-performance .NET library for creating transparent window overlays on a target process using OpenGL and SkiaSharp.

Fully compatible to be published as Native AOT(Windows).

## Features

- Create a transparent window overlay on a target processes
- Hardware-accelerated rendering using OpenGL
- High-performance graphics with SkiaSharp
- Native AOT (Ahead of Time) compilation support
- Windows platform support
- Basic drawing of various shapes, images and text.

### [NuGet](https://www.nuget.org/packages/OverlaySharp/)
#### .NET CLI
    dotnet add package OverlaySharp
#### Package Reference
    <PackageReference Include="OverlaySharp" Version="1.0.*" />

# Requirements
- .NET 8
- A Windows environment
- A functioning OpenGL driver
   - This means you need a GPU - e.g. an integrated GPU (Intel), an NVIDIA GPU or AMD GPU, all of which should support OpenGL.

# Examples
See the [sample project](https://github.com/Joey0x646576/OverlaySharp/tree/main/src/Samples/OverlaySharp.Simple).

Upon starting the overlay, you can draw on the target window by overriding the `OnRender` method.
```csharp
public class YourClass(nint targetWindowHandle) : OverlayWindow(targetWindowHandle)
{
    public override void OnRender(IGraphics graphics)
    {
      // Perform drawing operations through IGraphics here...
    }
}
```

# Contributing
Feel free to contribute! 🥸

# Acknowledgments
- [SkiaSharp](https://github.com/mono/SkiaSharp)
- [OpenGL](https://www.opengl.org/)
- [GameOverlay.Net](https://github.com/michel-pi/GameOverlay.Net)
