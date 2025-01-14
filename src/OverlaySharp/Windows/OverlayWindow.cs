using Windows.Win32.Foundation;
using OverlaySharp.Graphics.Adapters;
using OverlaySharp.Graphics.OpenGL;
using OverlaySharp.Graphics.Skia;
using OverlaySharp.Windows.Events;
using SkiaSharp;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace OverlaySharp.Windows
{
    public abstract class OverlayWindow : Window, IOverlayWindow
    {
        /// <summary>
        /// Measures and draws the frames per second (FPS) on the overlay window in the top-left corner.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>false</c>.
        /// </remarks>
        public bool MeasureFps { get; init; } = false;

        /// <summary>
        /// Enables antialiasing for the overlay window. This applies to all drawing operations.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>true</c>.
        /// </remarks>
        public bool AntiAliasing { get; init; } = true;

        private readonly Stopwatch _fpsStopwatch = new();
        private readonly CancellationTokenSource _overlayCancellationTokenSource;

        private ISkiaRenderer? _skiaRenderer;
        private IOpenGlContext _openGlContext = null!;
        private IGraphics _graphicsAdapter = null!;

        private Task? _overlayTask;
        private int _framesThisSecond;
        private int _currentFrameRate;
        private long _lastTickCount = Environment.TickCount64;

        private const long TickIntervalInMs = 25;

        protected OverlayWindow(nint targetWindowHandle) : base(targetWindowHandle)
        {
            _overlayCancellationTokenSource = new CancellationTokenSource();

            WindowReady += WindowReadyHandler;
            SizeChanged += WindowSizeChangedHandler;
        }

        public async Task StopOverlay()
        {
            await _overlayCancellationTokenSource.CancelAsync();
            if (_overlayTask != null)
            {
                await _overlayTask;
            }

            Dispose();
        }

        public abstract void Renderer(IGraphics graphics);

        private void WindowReadyHandler()
        {
            _overlayTask = Task.Factory.StartNew(
                StartOverlayTask,
                _overlayCancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void StartOverlayTask()
        {
            ResizeWindowToTargetHandle();

            _openGlContext = new OpenGlContext(new HWND(WindowHandle));
            _openGlContext.ConfigureVSync(VSyncState);

            _skiaRenderer = new SkiaRenderer(_openGlContext);
            _skiaRenderer.Initialize(Width, Height);

            _graphicsAdapter = new SkiaGraphicAdapter(_skiaRenderer, AntiAliasing);
            OnGraphicsReady(new GraphicsReadyEventArgs(_graphicsAdapter));

            if (MeasureFps)
            {
                // Using stopwatch for better precision, Environment.TickCount64 is not precise enough.
                _fpsStopwatch.Start();
            }

            while (!_overlayCancellationTokenSource.Token.IsCancellationRequested)
            {
                if (IsInFrequentUpdateRequired())
                {
                    if (BypassTopmost)
                    {
                        PlaceAboveTargetHandle();
                    }

                    ResizeWindowToTargetHandle();
                }

                Render();
            }

            _openGlContext.DeleteContext();
        }

        private void Render()
        {
            _skiaRenderer?.Canvas.Clear(SKColors.Transparent);

            if (MeasureFps)
            {
                _framesThisSecond++;
                if (_fpsStopwatch.ElapsedMilliseconds >= 1000)
                {
                    _currentFrameRate = _framesThisSecond;
                    _framesThisSecond = 0;
                    _fpsStopwatch.Restart();
                }

                _graphicsAdapter.DrawText(10, 10, $"FPS: {_currentFrameRate}", SKColors.White, _graphicsAdapter.DefaultFont);
            }

            Renderer(_graphicsAdapter);

            _skiaRenderer?.Flush();
            _openGlContext.SwapBuffers();
        }

        private void PlaceAboveTargetHandle()
        {
            var windowAboveParentWindow = PInvoke.GetWindow(new HWND(TargetWindowHandle), GET_WINDOW_CMD.GW_HWNDPREV);

            if (windowAboveParentWindow != WindowHandle)
            {
                PInvoke.SetWindowPos(new HWND(WindowHandle), new HWND(windowAboveParentWindow), 0, 0, 0, 0,
                    SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE |
                    SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_ASYNCWINDOWPOS);
            }
        }

        private bool IsInFrequentUpdateRequired()
        {
            var now = Environment.TickCount64;
            if (now - _lastTickCount <= TickIntervalInMs)
                return false;

            _lastTickCount = now;
            return true;
        }

        private void WindowSizeChangedHandler(object? sender, SizeChangedEventArgs e)
        {
            _skiaRenderer?.Resize(e.Width, e.Height);
        }

        public override void Dispose()
        {
            _overlayTask?.Dispose();
            base.Dispose();
        }
    }
}