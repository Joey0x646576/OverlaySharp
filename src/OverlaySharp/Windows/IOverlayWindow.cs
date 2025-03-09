using OverlaySharp.Graphics.Adapters;

namespace OverlaySharp.Windows
{
    internal interface IOverlayWindow
    {
        /// <summary>
        /// Starts the overlay as soon as it is ready. The render loop is started in a dedicated task.
        /// </summary>
        public Task StartOverlayAsync();

        /// <summary>
        /// Stops the overlay by disposing of the overlay window and releasing all associated resources.
        /// </summary>
        public Task StopOverlayAsync();

        /// <summary>
        /// Renders the graphics content onto the overlay window.
        /// </summary>
        /// <param name="graphics">The graphics adapter used for drawing operations.</param>
        public void OnRender(IGraphics graphics);
    }
}
