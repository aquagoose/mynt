#!/usr/bin/env dotnet
#:project mynt.Tests.Core/mynt.Tests.Core.csproj

using mynt;
using mynt.Tests.Core;
using piko.SDL3;

Mynt.MessageLogged += (message, severity, _, _) => Console.WriteLine($"[{severity}] {message}");

if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
    throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

SDL.Window window = SDL.CreateWindow("Create Device Test", 800, 600, 0);
if (window.IsNull)
    throw new Exception($"Failed to create window: {SDL.GetError()}");

InstanceInfo instanceInfo = new InstanceInfo("Create Device Test", true);
Instance instance = Instance.Create(in instanceInfo);
Console.WriteLine(instance.Backend);

Surface surface = SDL.CreateMyntSurface(window, instance);
Device device = instance.CreateDevice(surface);
CommandList cl = device.CreateCommandList();

bool alive = true;
while (alive)
{
    while (SDL.PollEvent(out SDL.Event sdlEvent))
    {
        switch ((SDL.EventType) sdlEvent.Type)
        {
            case SDL.EventType.Quit:
                alive = false;
                break;
        }
    }

    cl.Begin();
    cl.End();
    device.ExecuteCommandList(cl);
}

cl.Dispose();
device.Dispose();
surface.Dispose();
instance.Dispose();
SDL.DestroyWindow(window);
SDL.Quit();