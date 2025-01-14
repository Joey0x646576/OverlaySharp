using SkiaSharp;

namespace OverlaySharp.Graphics.Skia
{
    /// <summary>
    /// Provides an abstraction for rendering with Skia on an OpenGL surface.
    /// </summary>
    internal interface ISkiaRenderer
    {
        /// <summary>
        /// Gets the <see cref="SKCanvas"/> used to perform drawing operations.
        /// </summary>
        SKCanvas Canvas { get; }

        /// <summary>
        /// Initializes the Skia rendering surface.
        /// </summary>
        /// <param name="width">The initial width of the surface, in pixels.</param>
        /// <param name="height">The initial height of the surface, in pixels</param>
        void Initialize(int width, int height);

        /// <summary>
        /// Adjusts the size of the current Skia surface to given width and height.
        /// </summary>
        /// <param name="width">The new width of the surface, in pixels.</param>
        /// <param name="height">The new height of the surface, in pixels.</param>
        void Resize(int width, int height);

        /// <summary>
        /// For the GPU backend this will resolve all rendering to the GPU surface backing the surface that owns this canvas.
        /// </summary>
        void Flush();
    }
}
