# 🛰️ C# Network Simulator (Raylib)
A modular network simulation engine built with C# and Raylib.
Design topologies, connect devices, and simulate packet routing in real time.

![output](https://github.com/user-attachments/assets/5e676133-d2f5-4979-9e5d-92b247fa7ae5)

## 📋 Note
This project started as an assignment for a computer networks course (with some later modifications).
We were given the choice between using OMNeT++ or building a simple network simulation in any programming language.

I think it’s clear what I chose 🙂

I don’t plan to actively maintain this repository. There may be small tweaks from time to time, but no major roadmap.
Mostly, this repo exists as a reference, a playground, and a reminder of how much fun it was to build.

Anyway, back to the README.


## 🚀 Features
Shortest-path routing: Uses Breadth-First Search (BFS) to route messages.
Self-drawing objects: Devices and UI elements know how to draw themselves.
Interactive UI: Simple object-oriented UI with buttons and toggles.
Device types: End devices (PCs) and network switches.
⌨️ Controls

| Input | Action
|------|-------|
| Q | Cycle device colors
| W | Toggle mode (Device / Connection / Message)
| Left Click | Create / Select / Move / Delete
| Right Click | Create a switch (in device mode)

## 🛠️ Built With
* [Raylib-cs](https://github.com/ChrisDill/Raylib-cs) - C# bindings for Raylib.
* [.NET 8.0](https://dotnet.microsoft.com/download)

### 🏁 Getting Started
Preparing devices
After setting up .NET and Raylib, the project should run without issues.
- Choose a color and place PCs where you want them.
- Delete a PC using delete mode (red - button).
- Move a device using move mode (the @ symbol):
- - Click the device to make it follow the mouse.
-  - Click again to drop it.
- Add a switch using right click.
### Wiring connections
- Switch to connection mode.
- Click two devices to connect them.
- Connections can be removed using delete mode.
- Move mode has no effect in connection mode.
### Sending a message
- Once your network is ready:
- Switch to message mode.
- Click two devices.
- Watch the message travel along the shortest path.

That’s basically it.
You can also create your own device types by inheriting from Device.
Have fun, and happy coding :)
