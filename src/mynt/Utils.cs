using System.Runtime.InteropServices;

namespace mynt;

internal static class Utils
{
    public static unsafe bool NullTerminatedStringEqualsManagedString(byte* nullTerminatedString, string str)
    {
        fixed (char* pStr = str)
        {
            uint length = 0;
            do
            {
                if (length > str.Length || nullTerminatedString[length] != (byte) pStr[length])
                    return false;
            } while (nullTerminatedString[++length] != 0);

            return str.Length == length;
        }
    }
}