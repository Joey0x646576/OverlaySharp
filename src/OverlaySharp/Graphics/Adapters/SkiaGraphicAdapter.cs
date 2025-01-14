using OverlaySharp.Graphics.Adapters.Enums;
using OverlaySharp.Graphics.Skia;
using SkiaSharp;

namespace OverlaySharp.Graphics.Adapters
{
    internal class SkiaGraphicAdapter(ISkiaRenderer skiaRenderer, bool antiAliasing) : IGraphics
    {
        private SKFont? _defaultFont;
        public SKFont DefaultFont => _defaultFont ??= CreateFont(SKTypeface.Default);

        public void DrawText(float x, float y, string text, SKColor color, SKFont font)
        {
            _textPaint.Color = color;

            skiaRenderer.Canvas.DrawText(text, x, y + font.Size, SKTextAlign.Left, font, _textPaint);
        }

        public void DrawTextShadowed(float x, float y, string text, SKColor color, SKColor shadowColor, SKFont font, float shadowOffsetX = 2, float shadowOffsetY = 2)
        {
            _shadowedTextPaint.Color = shadowColor;
            _textPaint.Color = color;

            skiaRenderer.Canvas.DrawText(text, x + shadowOffsetX, y + font.Size + shadowOffsetY, SKTextAlign.Left, font, _shadowedTextPaint);
            skiaRenderer.Canvas.DrawText(text, x, y + font.Size, SKTextAlign.Left, font, _textPaint);
        }

        public void DrawLine(float x, float y, float endX, float endY, SKColor color, float strokeWidth)
        {
            _linePaint.Color = color;
            _linePaint.StrokeWidth = strokeWidth;

            skiaRenderer.Canvas.DrawLine(x, y, endX, endY, _linePaint);
        }

        public void DrawRectangleStroke(SKRect rectangle, SKColor color, float strokeWidth)
        {
            _outlinePaint.Color = color;
            _outlinePaint.StrokeWidth = strokeWidth;

            skiaRenderer.Canvas.DrawRect(rectangle, _outlinePaint);
        }

        public void DrawRectangleFill(SKRect rectangle, SKColor color)
        {
            _fillPaint.Color = color;
            skiaRenderer.Canvas.DrawRect(rectangle, _fillPaint);
        }

        public void DrawRectangleStrokeAndFill(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth)
        {
            DrawRectangleFill(rectangle, fillColor);
            DrawRectangleStroke(rectangle, strokeColor, strokeWidth);
        }

        public void DrawRectangleEdgesAndFill(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth)
        {
            DrawRectangleFill(rectangle, fillColor);

            var width = rectangle.Width;
            var height = rectangle.Height;
            var edgeLength = (width + height) / 2.0f * 0.2f;
            
            using var edgePath = new SKPath();

            edgePath.MoveTo(rectangle.Left, rectangle.Top);
            edgePath.LineTo(rectangle.Left + edgeLength, rectangle.Top);
            edgePath.MoveTo(rectangle.Left, rectangle.Top);
            edgePath.LineTo(rectangle.Left, rectangle.Top + edgeLength);

            edgePath.MoveTo(rectangle.Right, rectangle.Top);
            edgePath.LineTo(rectangle.Right - edgeLength, rectangle.Top);
            edgePath.MoveTo(rectangle.Right, rectangle.Top);
            edgePath.LineTo(rectangle.Right, rectangle.Top + edgeLength);

            edgePath.MoveTo(rectangle.Left, rectangle.Bottom);
            edgePath.LineTo(rectangle.Left + edgeLength, rectangle.Bottom);
            edgePath.MoveTo(rectangle.Left, rectangle.Bottom);
            edgePath.LineTo(rectangle.Left, rectangle.Bottom - edgeLength);

            edgePath.MoveTo(rectangle.Right, rectangle.Bottom);
            edgePath.LineTo(rectangle.Right - edgeLength, rectangle.Bottom);
            edgePath.MoveTo(rectangle.Right, rectangle.Bottom);
            edgePath.LineTo(rectangle.Right, rectangle.Bottom - edgeLength);

            DrawPath(edgePath, strokeColor, strokeWidth);
        }

        public void DrawCircle(float x, float y, float radius, SKColor color, float strokeWidth)
        {
            _outlineCirclePaint.Color = color;
            _outlineCirclePaint.StrokeWidth = strokeWidth;

            skiaRenderer.Canvas.DrawCircle(x, y, radius, _outlineCirclePaint);
        }

        public void DrawCircle(float x, float y, float radius, SKColor fillColor, SKColor strokeColor, float strokeWidth)
        {
            _fillCirclePaint.Color = fillColor;
            DrawCircle(x, y, radius, strokeColor, strokeWidth);

            skiaRenderer.Canvas.DrawCircle(x, y, radius, _fillCirclePaint);
        }

        public void DrawPath(SKPath path, SKColor color, float strokeWidth)
        {
            _linePaint.Color = color;
            _linePaint.StrokeWidth = strokeWidth;

            skiaRenderer.Canvas.DrawPath(path, _linePaint);
        }

        public void DrawProgressBar(SKRect rectangle, SKColor fillColor, SKColor strokeColor, float strokeWidth, float percentage, ProgressBarOrientation progressBarOrientation)
        {
            var fillHeight = rectangle.Height * (percentage / 100f);
            var fillRect = progressBarOrientation == ProgressBarOrientation.Vertical
                ? new SKRect(rectangle.Left, rectangle.Bottom - fillHeight, rectangle.Right, rectangle.Bottom)
                : new SKRect(rectangle.Left, rectangle.Top, rectangle.Left + rectangle.Width * (percentage / 100f), rectangle.Bottom);

            _fillPaint.Color = fillColor;
            _outlinePaint.Color = strokeColor;
            _outlinePaint.StrokeWidth = strokeWidth;

            DrawRectangleFill(fillRect, fillColor);
            DrawRectangleStroke(rectangle, strokeColor, strokeWidth);
        }

        public void DrawImage(SKImage image, float x, float y)
        {
            skiaRenderer.Canvas.DrawImage(image, x, y);
        }

        public SKFont CreateFont(SKTypeface typeface, SKFontEdging edging = SKFontEdging.SubpixelAntialias, float textSize = 12, float scaleX = 1, float skewX = 0, bool subpixel = true)
        {
            var font = new SKFont(typeface, textSize, scaleX, skewX)
            {
                Edging = edging
            };

            return font;
        }

        private readonly SKPaint _fillPaint = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = antiAliasing,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        private readonly SKPaint _outlinePaint = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = antiAliasing,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        private readonly SKPaint _linePaint = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = antiAliasing,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        private readonly SKPaint _outlineCirclePaint = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = antiAliasing
        };

        private readonly SKPaint _fillCirclePaint = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = antiAliasing
        };

        private readonly SKPaint _textPaint = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = antiAliasing
        };

        private readonly SKPaint _shadowedTextPaint = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = antiAliasing,
            MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 1)
        };
    }
}