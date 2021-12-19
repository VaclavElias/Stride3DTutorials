# Stride Game Engine Tutorials

This repo is going to list some tutorials and examples for Stride - Open-source C# Game Engine.

https://www.stride3d.net/

All examples are in .NET 5 using C# 9.0 or .NET 6 using C# 10. 

## Stride3D tutorials and examples

Example/Tutorial|Status
---------|----------
Drag and Drop|![Research](https://img.shields.io/badge/status-done-green?style=for-the-badge)
Minimal API | ![Research](https://img.shields.io/badge/status-testing-orange?style=for-the-badge)
Multi Screen | ![Research](https://img.shields.io/badge/status-research-blue?style=for-the-badge)

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
Minimal APIs are architected to create APIs with minimal dependencies (improved in .NET 6). They are ideal for apps that want to include only the mininum files, features and dependencies.

**ToDos**
- Make some examples
- Minimal API for 2D
- Minimal API for 3D
  - Minimal API pure
  - Miniaml API with nice set up (lights, sky, ..) out of the box

Explore https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio

#### Why would you use Minimal APIs and not Stride Editor?
There might be many reasons, including:
- You don't want to install anything on your computer
- You want to start very quickly
- You want to learn C# programming with a nice visual output instead of console
- You want to learn game programming in the most simple way, without using the game editor
- You find coding and tools very complex to understand and navigate around
- You want to start with game develepment basics before you even start exploring the game editor

Please, let me know through Issues if you find any other reasons.

#### Used Resources
https://github.com/IceReaper/StrideTest

<!-- https://img.shields.io/badge/status-in%20progress-green?style=for-the-badge -->

### Multi Screen
This will be an example of using SignalR and Stride, to use a browser for a game output, simple interaction from a browser with a game. I wish we had multi screen games where the other screens could be used as a Dashboard or Control Panels or anything you would like to see but not have it in the main game window, still some interactions from these side screens would be nice.

I have got a working prototype just need to make is more user friendly to move here.

## Other Stride3D tutorials and examples
- Stride YouTube Channel - C# Beginner https://www.youtube.com/playlist?list=PLRZx2y7uC8mNySUMfOQf-TLNVnnHkLfPi
- Stride Tutorials from Jorn Theunissen https://www.youtube.com/playlist?list=PLM8hj-JyVnYr-usNqX5aeXG0IwTY9FVge
- Stride Tutorials from Marian Dziubiak https://www.youtube.com/playlist?list=PL3KxSbsaNqqvlio_mwy0CIMZcYQugcRIc
- And more here https://github.com/stride3d/stride/wiki/StrideCommunityProjects

