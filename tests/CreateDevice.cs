#!/usr/bin/env dotnet
#:project mynt.Tests.Base/mynt.Tests.Base.csproj

using mynt;
using mynt.Tests.Base;
using piko.SDL3;

Mynt.MessageLogged += (severity, message, _, _) => Console.WriteLine($"[{severity}] {message}");

if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
    throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

SDL.Window window = SDL.CreateWindow("Create Device Test", 800, 600, 0);
if (window.IsNull)
    throw new Exception($"Failed to create window: {SDL.GetError()}");

InstanceInfo info = new InstanceInfo("Create Instance Test");
Instance instance = Instance.CreateInstance(in info);

Adapter[] adapters = instance.QueryAdapters();
foreach (Adapter adapter in adapters)
    Console.WriteLine(adapter);

Surface surface = SDL.CreateMyntSurface(instance, window);
Device device = instance.CreateDevice(surface);

device.Dispose();
surface.Dispose();
instance.Dispose();
SDL.DestroyWindow(window);
SDL.Quit();