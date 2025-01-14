using System.Drawing;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace OverlaySharp.Interop.Utility
{
    internal static class WindowHelper
    {
        private static readonly char[] AllowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public static Rectangle GetWindowClientArea(HWND targetWindowHandle)
        {
            if (!PInvoke.GetClientRect(targetWindowHandle, out var clientRect))
                throw new System.ComponentModel.Win32Exception($"Unable to call GetClientRect with targetWindowHandle {targetWindowHandle}");

            var topLeft = new Point { X = clientRect.left, Y = clientRect.top };
            if (!PInvoke.ClientToScreen(targetWindowHandle, ref topLeft))
                throw new System.ComponentModel.Win32Exception($"Unable to call ClientToScreen with targetWindowHandle {targetWindowHandle}");

            var width = clientRect.right - clientRect.left;
            var height = clientRect.bottom - clientRect.top;

            return new Rectangle(topLeft.X, topLeft.Y, width, height);
        }

        public static Rectangle GetWindowBounds(HWND targetWindowHandle)
        {
            if (!PInvoke.GetWindowRect(targetWindowHandle, out var rect))
                throw new System.ComponentModel.Win32Exception();

            return new Rectangle(rect.left, rect.top, rect.right, rect.bottom);
        }

        public static void ResizeWindow(HWND windowHandle, int x, int y, int width, int height)
        {
            PInvoke.MoveWindow(windowHandle, x, y, width, height, true);
        }

        public static string GenerateRandomName(int minLength = 10, int maxLength = 20)
        {
            var length = Random.Shared.Next(minLength, maxLength);

            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                sb.Append(AllowedCharacters[Random.Shared.Next(AllowedCharacters.Length)]);
            }

            return sb.ToString();
        }
    }
}
