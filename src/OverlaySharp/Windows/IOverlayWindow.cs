using OverlaySharp.Graphics.Adapters;

namespace OverlaySharp.Windows
{
    internal interface IOverlayWindow
    {
        /// <summary>
        /// Stops the overlay by disposing of the overlay window and releasing all associated resources.
        /// </summary>
        public Task StopOverlay();

        /// <summary>
        /// Renders the graphics content onto the overlay window.
        /// </summary>
        /// <param name="graphics">The graphics adapter used for drawing operations.</param>
        public void Renderer(IGraphics graphics);
    }
}
