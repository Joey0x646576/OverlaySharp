using OverlaySharp.Graphics.Adapters;

namespace OverlaySharp.Windows.Events
{
    public class WindowEventArgs
    {
        /// <summary>
        /// Occurs when the window position changes.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs>? PositionChanged;

        /// <summary>
        /// Occurs when the window gets resized.
        /// </summary>
        public event EventHandler<SizeChangedEventArgs>? SizeChanged;

        /// <summary>
        /// Occurs when the window is ready and the graphic adapter is configured.
        /// </summary>
        public event EventHandler<GraphicsReadyEventArgs>? GraphicsReady;

        /// <summary>
        /// Occurs when the window has been successfully constructed and is ready for use.
        /// </summary>
        public event Action? WindowReady;

        protected void OnSizeChanged(SizeChangedEventArgs sizeChangedEventArgs)
        {
            SizeChanged?.Invoke(this, sizeChangedEventArgs);
        }

        protected void OnPositionChanged(PositionChangedEventArgs positionChangedEventArgs)
        {
            PositionChanged?.Invoke(this, positionChangedEventArgs);
        }

        protected void OnGraphicsReady(GraphicsReadyEventArgs graphicsReadyEventArgs)
        {
            GraphicsReady?.Invoke(this, graphicsReadyEventArgs);
        }

        protected void OnWindowReady()
        {
            WindowReady?.Invoke();
        }
    }

    public class PositionChangedEventArgs(int x, int y) : EventArgs
    {
        public int X { get; set; } = x;

        public int Y { get; set; } = y;
    }

    public class SizeChangedEventArgs(int width, int height) : EventArgs
    {
        public int Width { get; set; } = width;

        public int Height { get; set; } = height;
    }

    public class GraphicsReadyEventArgs(IGraphics graphics) : EventArgs
    {
        public IGraphics Graphics { get; set; } = graphics;
    }
}
