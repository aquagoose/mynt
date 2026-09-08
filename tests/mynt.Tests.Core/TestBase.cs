using piko.SDL3;

namespace mynt.Tests.Core;

public class TestBase(string name) : IDisposable
{
    private SDL.Window _window;

    protected Instance Instance = null!;
    protected Surface Surface = null!;
    protected Device Device = null!;

    public void Run()
    {
        Mynt.MessageLogged += (message, severity, _, _) => Console.WriteLine($"[{severity}] {message}");

        if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
            throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

        _window = SDL.CreateWindow(name, 800, 600, SDL.WindowFlags.Resizable);
        if (_window.IsNull)
            throw new Exception($"Failed to create window: {SDL.GetError()}");

        InstanceInfo instanceInfo = new InstanceInfo(name, true);
        Instance = Instance.Create(in instanceInfo);
        Surface = SDL.CreateMyntSurface(_window, Instance);
        Device = Instance.CreateDevice(Surface);
    }

    public void Dispose()
    {
        Device.Dispose();
        Surface.Dispose();
        Instance.Dispose();

        SDL.DestroyWindow(_window);
        SDL.Quit();
    }
}