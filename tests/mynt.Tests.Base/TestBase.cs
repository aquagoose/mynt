using piko.SDL3;

namespace mynt.Tests.Base;

public abstract class TestBase(string name) : IDisposable
{
    private SDL.Window _window;
    private bool _running;
    private Surface _surface = null!;

    public Instance Instance = null!;
    public Device Device = null!;
    public CommandList CommandList = null!;

    protected virtual void Init() { }

    protected virtual void Loop() { }

    public void Run()
    {
        Mynt.MessageLogged += (severity, message, _, _) => Console.WriteLine($"[{severity}] {message}");
        
        if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
            throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

        _window = SDL.CreateWindow(name, 800, 600, 0);
        if (_window.IsNull)
            throw new Exception($"Failed to create window: {SDL.GetError()}");

        InstanceInfo instanceInfo = new InstanceInfo(name, debug: true);
        Instance = Instance.CreateInstance(in instanceInfo);
        _surface = SDL.CreateMyntSurface(Instance, _window);
        Device = Instance.CreateDevice(_surface);
        CommandList = Device.CreateCommandList();
        
        Init();

        _running = true;
        while (_running)
        {
            while (SDL.PollEvent(out SDL.Event sdlEvent))
            {
                switch ((SDL.EventType) sdlEvent.Type)
                {
                    case SDL.EventType.Quit:
                        _running = false;
                        break;
                }
            }
            
            Loop();
        }
    }
    
    public virtual void Dispose()
    {
        CommandList.Dispose();
        Device.Dispose();
        _surface.Dispose();
        Instance.Dispose();
    }
}