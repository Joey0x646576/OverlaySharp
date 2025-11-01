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

        public OverlayRunner(nint targetWindowHandle) : base(targetWindowHandle)
        {
            GraphicsReady += OnGraphicsReady;
        }

        public override void OnRender(IGraphics graphics)
        {
            if (_font != null)
            {
                graphics.DrawText(30, 90, "Typography", SKColors.White, _font);
                graphics.DrawText(350, 90, "Shapes", SKColors.White, _font);
                graphics.DrawText(650, 90, "Other", SKColors.White, _font);
                graphics.DrawText(675, 310, "and more...", SKColors.White, _font);
                graphics.DrawText(875, 90, "Portal", SKColors.White, _font);


                graphics.DrawText(40, 140, "Regular Text", SKColors.White, _font);
                graphics.DrawTextShadowed(40, 180, "Shadowed Text Black", SKColors.White, SKColors.Black, _font);
                graphics.DrawTextShadowed(40, 220, "Shadowed Text Red", SKColors.White, SKColors.Red, _font);

                DrawMeasuredText(graphics);
            }

            graphics.DrawLine(20, 125, Width-20, 125, SKColors.DarkGray, 2);
            graphics.DrawLine(350, 125, 350, 400, SKColors.DarkGray, 2);
            graphics.DrawLine(650, 125, 650, 400, SKColors.DarkGray, 2);
            graphics.DrawLine(875, 125, 875, 400, SKColors.DarkGray, 2);

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

            graphics.DrawEllipse(620, 280, 20, 10, SKColors.WhiteSmoke, 2);

            graphics.DrawRectangleEdgesAndFill(SKRect.Create(360, 320, 225, 125), new SKColor(0, 0, 0, 40), SKColors.Red, 1);
            graphics.DrawRectangleEdgesAndFill(SKRect.Create(Width, Height), new SKColor(0, 0, 0, 0), SKColors.Red, 2);

            graphics.DrawImage(_image!, 675, 140);
            graphics.DrawProgressBar(SKRect.Create(675, 300, 100, 10), SKColors.GreenYellow, SKColors.White, 1, 88, ProgressBarOrientation.Horizontal);
            graphics.DrawProgressBar(SKRect.Create(830, 210, 10, 100), SKColors.GreenYellow, SKColors.White, 1, 33, ProgressBarOrientation.Vertical);

            DrawPortalAnimation(graphics);
        }

        private void OnGraphicsReady(object? sender, GraphicsReadyEventArgs e)
        {
            _font = e.Graphics.CreateFont(SKTypeface.Default, textSize: 24);

            var bitmap = SKBitmap.Decode(Path.Combine(AppContext.BaseDirectory, "example-output.png"));
            var resizedBitmap = bitmap.Resize(new SKImageInfo(150, 150), SKFilterQuality.High);
            _image = SKImage.FromBitmap(resizedBitmap);
        }

        private void DrawMeasuredText(IGraphics graphics)
        {
            const string measuredText = "Measured Text";
            const int textX = 40;
            const int textY = 260;
            const int padding = 4;

            var textWidth = graphics.MeasureTextWidth(measuredText, _font!);
            var textHeight = graphics.MeasureTextHeight(_font!);

            var bgRect = new SKRect(textX - padding, textY - padding, textX + textWidth + padding, textY + textHeight + padding);

            graphics.DrawRectangleStrokeAndFill(bgRect, new SKColor(50, 50, 150, 200), SKColors.LightBlue, 1);
            graphics.DrawText(textX, textY, measuredText, SKColors.White, _font!);
        }

        private float _fallingRectY = 150;
        private float _fallingRectVelocity = 1.5f;
        private const float Gravity = 0.2f;
        private const float MaxVelocity = 20f;
        private const float PortalTopY = 150;
        private const float PortalBottomY = 400;
        private const float PortalX = 975;
        private const float PortalRadiusX = 60;
        private const float PortalRadiusY = 15;
        private const float RectSize = 30;

        private void DrawPortalAnimation(IGraphics graphics)
        {
            _fallingRectY += _fallingRectVelocity;

            if (_fallingRectY > PortalBottomY)
            {
                _fallingRectY = PortalTopY;

                _fallingRectVelocity += Gravity;
                _fallingRectVelocity = Math.Min(_fallingRectVelocity, MaxVelocity);
            }

            DrawPortal(graphics, PortalX, PortalTopY, new SKColor(255, 140, 0));
            DrawPortal(graphics, PortalX, PortalBottomY, new SKColor(0, 150, 255));

            var distanceFromBottom = Math.Abs(_fallingRectY - PortalBottomY);
            var distanceFromTop = Math.Abs(_fallingRectY - PortalTopY);
            var minDistance = Math.Min(distanceFromBottom, distanceFromTop);
            var opacity = minDistance < 20 ? (byte)(minDistance / 20 * 255) : (byte)255;

            graphics.DrawRectangleStrokeAndFill(
                SKRect.Create(PortalX - RectSize / 2, _fallingRectY - RectSize / 2, RectSize, RectSize),
                new SKColor(150, 50, 200, opacity),
                new SKColor(200, 100, 255, opacity),
                2
            );
        }

        private void DrawPortal(IGraphics graphics, float x, float y, SKColor portalColor)
        {
            graphics.DrawEllipse(x, y, PortalRadiusX * 1.3f, PortalRadiusY * 1.3f,
                new SKColor(portalColor.Red, portalColor.Green, portalColor.Blue, 60),
                new SKColor(0, 0, 0, 0), 1);
            graphics.DrawEllipse(x, y, PortalRadiusX * 1.15f, PortalRadiusY * 1.15f,
                new SKColor(portalColor.Red, portalColor.Green, portalColor.Blue, 100),
                new SKColor(0, 0, 0, 0), 1);
            graphics.DrawEllipse(x, y, PortalRadiusX, PortalRadiusY,
                new SKColor(portalColor.Red, portalColor.Green, portalColor.Blue, 180),
                portalColor, 3);

            graphics.DrawEllipse(x, y, PortalRadiusX * 0.6f, PortalRadiusY * 0.6f,
                new SKColor(0, 0, 0, 200),
                new SKColor(0, 0, 0, 0), 1);
        }
    }
}
