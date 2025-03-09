using OverlaySharp.Graphics.Adapters.Enums;
using SkiaSharp;

namespace OverlaySharp.Graphics.Adapters
{
    /// <summary>
    /// Interface for a (someday)common drawing adapter. For now, it's just SkiaSharp.
    /// </summary>
    public interface IGraphics
    {
        /// <summary>
        /// Gets the default font used for rendering text.
        /// </summary>
        public SKFont DefaultFont { get; }

        /// <summary>
        /// Draws text at a specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the text's position.</param>
        /// <param name="y">The y-coordinate of the text's position.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="font">The font to use for rendering the text.</param>
        public void DrawText(float x, float y, string text, SKColor color, SKFont font);

        /// <summary>
        /// Draws text with a shadow at a specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the text's position.</param>
        /// <param name="y">The y-coordinate of the text's position.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="shadowColor">The color of the shadow.</param>
        /// <param name="font">The font to use for rendering the text.</param>
        /// <param name="shadowOffsetX">The horizontal offset of the shadow. Defaults to 2.</param>
        /// <param name="shadowOffsetY">The vertical offset of the shadow. Defaults to 2.</param>
        public void DrawTextShadowed(float x, float y, string text, SKColor color, SKColor shadowColor, SKFont font, float shadowOffsetX = 2, float shadowOffsetY = 2);

        /// <summary>
        /// Draws text with an outline at a specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the text's position.</param>
        /// <param name="y">The y-coordinate of the text's position.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="outlineColor">The color of the outline.</param>
        /// <param name="font">The font to use for rendering the text.</param>
        /// <param name="strokeWidth">The width of the outline. Defaults to 2.</param>

        public void DrawTextOutline(float x, float y, string text, SKColor color, SKColor outlineColor, SKFont font, float strokeWidth = 2);

        /// <summary>
        /// Draws a straight line between two points.
        /// </summary>
        /// <param name="x">The x-coordinate of the start point.</param>
        /// <param name="y">The y-coordinate of the start point.</param>
        /// <param name="endX">The x-coordinate of the end point.</param>
        /// <param name="endY">The y-coordinate of the end point.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="strokeWidth">The width of the line.</param>
        public void DrawLine(float x, float y, float endX, float endY, SKColor color, float strokeWidth);

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color of the rectangle's outline.</param>
        /// <param name="strokeWidth">The width of the outline.</param>
        public void DrawRectangleStroke(SKRect rectangle, SKColor color, float strokeWidth);

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The fill color.</param>
        public void DrawRectangleFill(SKRect rectangle, SKColor color);

        /// <summary>
        /// Draws and fills a rectangle with separate stroke and fill colors.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw and fill.</param>
        /// <param name="fillColor">The fill color.</param>
        /// <param name="strokeColor">The stroke color.</param>
        /// <param name="strokeWidth">The width of the outline.</param>
        public void DrawRectangleStrokeAndFill(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth);

        /// <summary>
        /// Draws a rectangle with edges and fills it with a color.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw and fill.</param>
        /// <param name="fillColor">The fill color.</param>
        /// <param name="strokeColor">The color of the rectangle's edges.</param>
        /// <param name="strokeWidth">The width of the edges.</param>
        public void DrawRectangleEdgesAndFill(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth);

        /// <summary>
        /// Draws the outline of a circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle's outline.</param>
        /// <param name="strokeWidth">The width of the outline.</param>
        public void DrawCircle(float x, float y, float radius, SKColor color, float strokeWidth);

        /// <summary>
        /// Draws the outline of a circle and fills it with a color.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fillColor">The color used to fill the circle.</param>
        /// <param name="strokeColor">The color used for the circle's outline.</param>
        /// <param name="strokeWidth">The width of the circle's outline.</param>
        public void DrawCircle(float x, float y, float radius, SKColor fillColor, SKColor strokeColor, float strokeWidth);

        /// <summary>
        /// Draws a path using a specified stroke color and width.
        /// </summary>
        /// <param name="path">The path to draw.</param>
        /// <param name="color">The stroke color.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        public void DrawPath(SKPath path, SKColor color, float strokeWidth);

        /// <summary>
        /// Draws a progress bar within a specified rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle defining the progress bar area.</param>
        /// <param name="fillColor">The fill color of the progress bar.</param>
        /// <param name="strokeColor">The stroke color of the progress bar.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <param name="percentage">The percentage of the progress bar to fill.</param>
        /// <param name="progressBarOrientation">The orientation of the progress bar.</param>
        public void DrawProgressBar(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth, float percentage, ProgressBarOrientation progressBarOrientation);

        /// <summary>
        /// Draws an image at a specified position.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">The x-coordinate of the image's position.</param>
        /// <param name="y">The y-coordinate of the image's position.</param>
        public void DrawImage(SKImage image, float x, float y);

        /// <summary>
        /// Creates a custom font with specified attributes.
        /// </summary>
        /// <param name="typeface">The typeface to use for the font.</param>
        /// <param name="edging">The edging style for the font. Defaults to subpixel antialiasing.</param>
        /// <param name="textSize">The size of the text. Defaults to 12.</param>
        /// <param name="scaleX">The horizontal scaling factor. Defaults to 1.</param>
        /// <param name="skewX">The horizontal skew factor. Defaults to 0.</param>
        /// <param name="subpixel">Indicates whether to enable subpixel positioning. Defaults to true.</param>
        /// <returns>A new <see cref="SKFont"/> instance, or null if creation fails.</returns>
        public SKFont? CreateFont(SKTypeface typeface, SKFontEdging edging = SKFontEdging.SubpixelAntialias, float textSize = 12, float scaleX = 1, float skewX = 0, bool subpixel = true);
    }
}
