using OverlaySharp.Graphics.Adapters;
using OverlaySharp.Graphics.Adapters.Enums;
using OverlaySharp.Windows;
using OverlaySharp.Windows.Events;
using SkiaSharp;

namespace OverlaySharp.Runner.Overlay
{
    internal class OverlayRunner : OverlayWindow
    {
        private SKFont? _font;
        private SKImage? _image;
        private const float Speed = 1;

        public OverlayRunner(nint targetWindowHandle) : base(targetWindowHandle)
        {
            GraphicsReady += OnGraphicsReady;
        }

        public override void Renderer(IGraphics graphics)
        {
            if (_font != null)
            {
                graphics.DrawText(30, 90, "Typography", SKColors.White, _font);
                graphics.DrawText(350, 90, "Shapes", SKColors.White, _font);
                graphics.DrawText(650, 90, "Other", SKColors.White, _font);
                graphics.DrawText(675, 310, "and more...", SKColors.White, _font);

                graphics.DrawText(40, 140, "Regular Text", SKColors.White, _font);
                graphics.DrawTextShadowed(40, 180, "Shadowed Text Black", SKColors.White, SKColors.Black, _font);
                graphics.DrawTextShadowed(40, 220, "Shadowed Text Red", SKColors.White, SKColors.Red, _font);
            }

            graphics.DrawLine(20, 125, Width-20, 125, SKColors.DarkGray, 2);
            graphics.DrawLine(350, 125, 350, 400, SKColors.DarkGray, 2);
            graphics.DrawLine(650, 125, 650, 400, SKColors.DarkGray, 2);

            graphics.DrawRectangleStroke(SKRect.Create(360, 140, 50, 50), SKColors.Red, 2);
            graphics.DrawRectangleStroke(SKRect.Create(415, 140, 50, 50), SKColors.Green, 2);
            graphics.DrawRectangleStroke(SKRect.Create(470, 140, 50, 50), SKColors.Blue, 2);
            graphics.DrawRectangleStroke(SKRect.Create(525, 140, 50, 50), SKColors.WhiteSmoke, 2);

            graphics.DrawRectangleFill(SKRect.Create(360, 200, 50, 50), SKColors.Red);
            graphics.DrawRectangleFill(SKRect.Create(415, 200, 50, 50), SKColors.Green);
            graphics.DrawRectangleFill(SKRect.Create(470, 200, 50, 50), SKColors.Blue);
            graphics.DrawRectangleFill(SKRect.Create(525, 200, 50, 50), SKColors.WhiteSmoke);

            graphics.DrawCircle(380, 280, 20, new SKColor(255, 0, 0, 40), SKColors.WhiteSmoke, 2);
            graphics.DrawCircle(440, 280, 20, new SKColor(0, 255, 0, 40), SKColors.WhiteSmoke, 2);
            graphics.DrawCircle(500, 280, 20, new SKColor(0, 0, 255, 40), SKColors.WhiteSmoke, 2);
            graphics.DrawCircle(560, 280, 20, new SKColor(255, 255, 255, 40), SKColors.WhiteSmoke, 2);

            graphics.DrawRectangleEdgesAndFill(SKRect.Create(360, 320, 225, 125), new SKColor(0, 0, 0, 40), SKColors.Red, 1);
            graphics.DrawRectangleEdgesAndFill(SKRect.Create(Width, Height), new SKColor(0, 0, 0, 0), SKColors.Red, 2);

            graphics.DrawImage(_image!, 675, 140);
            graphics.DrawProgressBar(SKRect.Create(675, 300, 100, 10), SKColors.GreenYellow, SKColors.White, 1, 88, ProgressBarOrientation.Horizontal);
            graphics.DrawProgressBar(SKRect.Create(830, 210, 10, 100), SKColors.GreenYellow, SKColors.White, 1, 33, ProgressBarOrientation.Vertical);
        }

        private void OnGraphicsReady(object? sender, GraphicsReadyEventArgs e)
        {
            _font = e.Graphics.CreateFont(SKTypeface.Default, textSize: 24);

            var bitmap = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, "example-output.png"));
            var resizedBitmap = bitmap.Resize(new SKImageInfo(150, 150), SKFilterQuality.High);
            _image = SKImage.FromBitmap(resizedBitmap);
        }
    }
}
