using System.Runtime.CompilerServices;

namespace mynt;

/// <summary>
/// Manages core mynt functions.
/// </summary>
public static class Mynt
{
    public static event OnMessageLogged MessageLogged;

    static Mynt()
    {
        MessageLogged = delegate { };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Log(LogSeverity severity, string message, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        => MessageLogged(severity, message, line, file);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Log(string message, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        => MessageLogged(LogSeverity.Verbose, message, line, file);
    
    public delegate void OnMessageLogged(LogSeverity severity, string message, int line, string file);
}