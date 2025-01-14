namespace OverlaySharp.Tests
{
    [SetUpFixture]
    public class OpenGlContextFixture
    {
        internal static IOpenGlContext OpenGlContext { get; private set; } = null!;
        private static HWND _testWindowHandle;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            _testWindowHandle = CreateHiddenTestWindow();

            OpenGlContext = new OpenGlContext(_testWindowHandle);
            OpenGlContext.MakeCurrent();
        }

        private unsafe HWND CreateHiddenTestWindow()
        {
            var className = WindowHelper.GenerateRandomName().ToCharPointer();
            var wc = new WNDCLASSW
            {
                style = 0,
                lpfnWndProc = &WindowProcedure,
                hInstance = default,
                lpszClassName = className
            };
            PInvoke.RegisterClass(wc);

            return PInvoke.CreateWindowEx(
                0,
                className,
                WindowHelper.GenerateRandomName().ToCharPointer(),
                0,
                0,
                0,
                0,
                0,
                default, default, default);
        }

        private void DestroyTestWindow(HWND hwnd)
        {
            if (!hwnd.IsNull)
            {
                _ = PInvoke.DestroyWindow(hwnd);
            }
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

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            // Clean up context and window
            OpenGlContext.DeleteContext();
            DestroyTestWindow(_testWindowHandle);
        }
    }
}
