using OverlaySharp.Graphics.Adapters;
using OverlaySharp.Windows;
using SkiaSharp;

namespace OverlaySharp.Simple
{
    internal class SimpleOverlay(nint targetWindowHandle) : OverlayWindow(targetWindowHandle)
    {
        public override void OnRender(IGraphics graphics)
        {
            // Draws a line from top left to bottom right.
            graphics.DrawLine(0, 0, Width, Height, SKColors.Black, 2);
        }
    }
}
