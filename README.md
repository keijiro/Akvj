Akvj
====

**Akvj** is a demo project for the Akvfx plugin (Azure Kinect integration for Unity VFX Graph).

![gif](https://i.imgur.com/DAzI6Dx.gif)
![gif](https://i.imgur.com/MGGaUET.gif)

System requirements
-------------------

- DirectX 11 compatible Windows System
- Azure Kinect DK device

How to run the demo
-------------------

Akvj runs in a "fire-and-forget" fashion. Once you start it, it runs automatically without any user interaction (excepting the camera input).

At first, Akvj shows the adjustment screen.

![screenshot](https://i.imgur.com/1kWtXGd.png)

The Depth Threshold slider changes the maximum distance of the points. You can remove the background by tweaking it to make the background points excluded.

The red rectangle indicates the sweet spot of the depth camera. Adjust the device position and angle to place the main object (your upper body and face) inside the rectangle.

The Camera Offset slider changes the distance between the camera and the object. Adjust it to make the object penetrating the rectangle (see the screenshot above).

Press the space key to close the adjustment screen and start the performance mode. You can reopen the adjustment screen by pressing the space key again.
