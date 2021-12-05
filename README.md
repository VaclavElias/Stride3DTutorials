# Stride Game Engine Tutorials

This repo is going to list some tutorials and examples for Stride - Open-source C# Game Engine.

https://www.stride3d.net/

All examples are in .NET 5 and C# 9.0.

## Stride3D tutorials and examples

Example/Tutorial|Status
---------|----------
Drag and Drop|![Research](https://img.shields.io/badge/status-done-green?style=for-the-badge)
Minimal API | ![Research](https://img.shields.io/badge/status-testing-orange?style=for-the-badge)

### Drag And Drop

Very basic implementation of drag and drop window, using Canvas. This demo is using:
- DragAndDropContainer and DragAndDropCanvas
- Multiple drag and drop windows
- Window button triggering entity creation

#### Q1: The font is not sharp and white?
Additional set up (Clean UI) is needed https://github.com/stride3d/stride/issues/1154.

#### Q2: Why are cubes moving funny once landed?
- The first issue was that I set collider size the same as entity scale, that means also collider was scaled.
- The second issue was that Static Collider - Infinity Plane is causing sliding, also in Unity. I changed it to Box type and it seems ok

#### Q3: Why the application gets problem with 10,000 boxes?
- Probably there is a better way to instance boxes

### Minimal Stride API
Minimal APIs are architectd to create APIs with minimal dependencies (improved in .NET 6). They are ideal for apps that want to indlude onlyl the mininum files, features and dependencies.

Explore https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio

<!-- https://img.shields.io/badge/status-in%20progress-green?style=for-the-badge -->

## Other Stride3D tutorials and examples
- Stride YouTube Channel - C# Beginner https://www.youtube.com/playlist?list=PLRZx2y7uC8mNySUMfOQf-TLNVnnHkLfPi
- Stride Tutorials from Jorn Theunissen https://www.youtube.com/playlist?list=PLM8hj-JyVnYr-usNqX5aeXG0IwTY9FVge
- Stride Tutorials from Marian Dziubiak https://www.youtube.com/playlist?list=PL3KxSbsaNqqvlio_mwy0CIMZcYQugcRIc
- And more here https://github.com/stride3d/stride/wiki/StrideCommunityProjects

