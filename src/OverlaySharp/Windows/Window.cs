using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using OverlaySharp.Interop.Enums;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.HiDpi;
using OverlaySharp.Graphics.OpenGL.Enums;
using OverlaySharp.Interop.Extensions;
using OverlaySharp.Interop.Utility;
using OverlaySharp.Windows.Events;
using OverlaySharp.Exceptions;

namespace OverlaySharp.Windows
{
    /// <summary>
    /// The <see cref="Window"/> constructs and controls the window.
    /// </summary>
    public abstract unsafe class Window : WindowEventArgs, IDisposable
    {
        /// <summary>
        /// The window handle of the target process.
        /// </summary>
        public nint TargetWindowHandle { get; }

        /// <summary>
        /// Forces the window on top each render, if false it will apply <see cref="WINDOW_EX_STYLE"/> of Topmost.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>false</c>.
        /// </remarks>
        public bool BypassTopmost { get; init; } = false;

        /// <summary>
        /// Sets the vertical synchronization (VSync) state for the OpenGL context.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="VSyncState.On"/>.
        /// </remarks>
        public VSyncState VSyncState { get; init; } = VSyncState.On;

        /// <summary>
        /// Sets the window menu name.
        /// </summary>
        /// <remarks>
        /// Defaults to a random generated <c>window menu name</c>.
        /// </remarks>
        public char* WindowMenuName { get; init; } = WindowHelper.GenerateRandomName().ToCharPointer();

        /// <summary>
        /// Sets the window class name.
        /// </summary>
        /// <remarks>
        /// Defaults to a random generated <c>window class name</c>.
        /// </remarks>
        public char* WindowClassName { get; init; } = WindowHelper.GenerateRandomName().ToCharPointer();

        /// <summary>
        /// Sets the window title name.
        /// </summary>
        /// <remarks>
        /// Defaults to a random generated <c>window title</c>.
        /// </remarks>
        public char* WindowTitle { get; init; } = WindowHelper.GenerateRandomName().ToCharPointer();

        /// <summary>
        /// The created window handle.
        /// </summary>
        public nint WindowHandle { get; private set; }

        /// <summary>
        /// Gets the X-coordinate of the window in pixels.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y-coordinate of the window in pixels.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the width of the window in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the window in pixels.
        /// </summary>
        public int Height { get; private set; }

        private readonly CancellationTokenSource _windowCancellationTokenSource;

        private Thread? _windowThread;
        private const uint DestroyWindowMessage = 0x1337;

        protected Window(nint targetWindowHandle)
        {
            TargetWindowHandle = targetWindowHandle;
            _windowCancellationTokenSource = new CancellationTokenSource();

            Create();
        }

        public void ResizeWindowToTargetHandle()
        {
            var windowBounds = WindowHelper.GetWindowClientArea(new HWND(TargetWindowHandle));
            if (windowBounds == Rectangle.Empty)
            {
                return;
            }

            var x = windowBounds.Left;
            var y = windowBounds.Top;
            var width = windowBounds.Right - windowBounds.Left;
            var height = windowBounds.Bottom - windowBounds.Top;

            if (X != x
                || Y != y
                || Width != width
                || Height != height)
            {
                ResizeWindow(x, y, width, height);
            }
        }

        public virtual Task Stop()
        {
            PInvoke.PostMessage(new HWND(WindowHandle), 0x100, new WPARAM(), new LPARAM());

            _windowCancellationTokenSource.Cancel();
            _windowThread?.Join();

            return Task.CompletedTask;
        }

        private void Create()
        {
            _windowThread = new Thread(WindowThread);
            _windowThread.Start();
        }

        private void WindowThread()
        {
            PInvoke.SetThreadDpiAwarenessContext(new DPI_AWARENESS_CONTEXT(-4));

            RegisterWindowClass();
            CreateWindow();

            while (!_windowCancellationTokenSource.IsCancellationRequested)
            {
                PInvoke.WaitMessage();
                if (PInvoke.PeekMessage(out var message, new HWND(0), 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE))
                {
                    // TODO: Create a message handler for user input e.g. keyboard, mouse, etc.
                    if (message.message == DestroyWindowMessage)
                    {
                        break;
                    }

                    PInvoke.TranslateMessage(message);
                    PInvoke.DispatchMessage(message);
                }
            }

            PInvoke.UnregisterClass(WindowClassName, new HINSTANCE());
        }

        private void RegisterWindowClass()
        {
            var windowClass = new WNDCLASSW
            {
                style = 0,
                lpfnWndProc = &WindowProcedure,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = default,
                hIcon = default,
                hCursor = default,
                hbrBackground = default,
                lpszMenuName = WindowMenuName,
                lpszClassName = WindowClassName
            };

            if (PInvoke.RegisterClass(windowClass) != 0)
            {
                return;
            }

            throw new WindowClassRegistrationException("Failed to register window class");
        }

        private void CreateWindow()
        {
            var windowExtendedStyle = WINDOW_EX_STYLE.WS_EX_TRANSPARENT | WINDOW_EX_STYLE.WS_EX_LAYERED | WINDOW_EX_STYLE.WS_EX_NOACTIVATE;
            if (!BypassTopmost)
            {
                windowExtendedStyle |= WINDOW_EX_STYLE.WS_EX_TOPMOST;
            }

            ResizeWindowToTargetHandle();
            WindowHandle = PInvoke.CreateWindowEx(
                windowExtendedStyle,
                WindowClassName,
                WindowTitle,
                WINDOW_STYLE.WS_POPUP | WINDOW_STYLE.WS_VISIBLE,
                X,
                Y,
                Width,
                Height,
                default, default, default);

            PInvoke.SetLayeredWindowAttributes(new HWND(WindowHandle), new COLORREF(0), 255, LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_ALPHA);
            PInvoke.UpdateWindow(new HWND(WindowHandle));

            DwmExtendFrameIntoClientArea();

            OnWindowReady();
        }

        private void DwmExtendFrameIntoClientArea()
        {
            var margin = new MARGINS
            {
                cxLeftWidth = -1,
                cxRightWidth = -1,
                cyBottomHeight = -1,
                cyTopHeight = -1
            };

            PInvoke.DwmExtendFrameIntoClientArea(new HWND(WindowHandle), in margin);
        }

        private void ResizeWindow(int x, int y, int width, int height)
        {
            PInvoke.MoveWindow((HWND)WindowHandle, x, y, width, height, true);

            X = x; Y = y; Width = width; Height = height;

            DwmExtendFrameIntoClientArea();

            OnPositionChanged(new PositionChangedEventArgs(x, y));
            OnSizeChanged(new SizeChangedEventArgs(width, height));
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
        private static LRESULT WindowProcedure(HWND windowHandle, uint windowMessage, WPARAM wParam, LPARAM lParam)
        {
            switch ((WindowMessage)windowMessage)
            {
                case WindowMessage.EraseBackground:
                    PInvoke.SendMessage(windowHandle, (uint)WindowMessage.Paint, 0, 0);
                    break;
                case WindowMessage.NcPaint or WindowMessage.Paint:
                    return (LRESULT)0;
                case WindowMessage.Destroy:
                    PInvoke.PostQuitMessage(0);
                    break;
                default:
                    return PInvoke.DefWindowProc(new HWND(windowHandle), windowMessage, wParam, lParam);
            }

            return new LRESULT();
        }

        public virtual void Dispose()
        {
            Stop();
            _windowCancellationTokenSource.Dispose();
            PInvoke.DestroyWindow(new HWND(WindowHandle));

            Marshal.FreeHGlobal((nint)WindowClassName);
            Marshal.FreeHGlobal((nint)WindowMenuName);
            Marshal.FreeHGlobal((nint)WindowTitle);
        }
    }
}
