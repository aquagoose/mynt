using System.Runtime.CompilerServices;

namespace mynt;

/// <summary>
/// Core mynt utilities, including logging and metrics.
/// </summary>
public static class Mynt
{
    public static event OnMessageLogged MessageLogged = delegate { };

    public static void Log(LogSeverity severity, string message, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        => MessageLogged(message, severity, line, file);

    public static void Log(string message, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        => MessageLogged(message, LogSeverity.Verbose, line, file);

    /// <summary>
    /// Compares if a managed and unmanaged string are equal, without performing any allocations.
    /// </summary>
    /// <param name="string">The managed string.</param>
    /// <param name="unmanagedString">The unmanaged string.</param>
    /// <returns><see langword="true"/> if the two strings are equal, otherwise <see langword="false"/>.</returns>
    /// <remarks>This does <b>NOT</b> handle multibyte strings.</remarks>
    public static unsafe bool ManagedAndUnmanagedStringsAreEqual(string @string, sbyte* unmanagedString)
    {
        int i = 0;
        do
        {
            if (i >= @string.Length || (sbyte) @string[i] != unmanagedString[i])
                return false;
        } while (unmanagedString[++i] != 0);

        return true;
    }

    public enum LogSeverity
    {
        Verbose,
        Info,
        Warning,
        Error
    }

    public delegate void OnMessageLogged(string message, LogSeverity severity, int line, string file);
}