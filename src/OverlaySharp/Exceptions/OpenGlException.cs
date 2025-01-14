using System.Runtime.InteropServices;

namespace OverlaySharp.Exceptions
{
    internal class OpenGlException(string message)
        : Exception($"{message} (Error {Marshal.GetLastWin32Error()}: {Marshal.GetLastPInvokeErrorMessage()})");
}
