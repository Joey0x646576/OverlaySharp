using Windows.Win32;
using SkiaSharp;
using OverlaySharp.Graphics.OpenGL;

namespace OverlaySharp.Graphics.Skia
{
    internal class SkiaRenderer(IOpenGlContext openGlContext) : ISkiaRenderer, IDisposable
    {
        public SKCanvas Canvas { get; private set; } = null!;

        private GRContext? _grContext;
        private GRBackendRenderTarget? _renderTarget;
        private SKSurface? _surface;

        private const uint GlFrameBufferBinding = 0x8CA6;
        private const uint GlRgba8 = 0x8058;

        public void Initialize(int width, int height)
        {
            var glInterface = GRGlInterface.Create();
            _grContext = GRContext.CreateGl(glInterface);

            _renderTarget = CreateRenderTarget(width, height);
            _surface = CreateSkSurface(_grContext, _renderTarget);

            Canvas = _surface.Canvas;
        }

        public void Resize(int width, int height)
        {
            Canvas.Dispose();
            _renderTarget?.Dispose();
            _surface?.Dispose();

            openGlContext.Viewport(0,0, width, height);

            _renderTarget = CreateRenderTarget(width, height);
            _surface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            Canvas = _surface.Canvas;
        }

        public void Flush()
        {
            Canvas.Flush();
        }

        public void Dispose()
        {
            _grContext?.Dispose();
            _renderTarget?.Dispose();
            _surface?.Dispose();
            Canvas.Dispose();
        }

        private static GRBackendRenderTarget CreateRenderTarget(int width, int height)
        {
            var frameBuffer = 0;
            PInvoke.glGetIntegerv(GlFrameBufferBinding, ref frameBuffer);

            var frameBufferInfo = new GRGlFramebufferInfo
            {
                FramebufferObjectId = (uint)frameBuffer,
                Format = GlRgba8
            };

            return new GRBackendRenderTarget(width, height, 0, 8, frameBufferInfo);
        }

        private static SKSurface CreateSkSurface(GRContext grContext, GRBackendRenderTarget renderTarget)
        {
            return SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
        }
    }
}
