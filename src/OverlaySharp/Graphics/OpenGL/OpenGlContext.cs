using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Graphics.OpenGL;
using OverlaySharp.Exceptions;
using System.Runtime.InteropServices;
using OverlaySharp.Graphics.OpenGL.Enums;

namespace OverlaySharp.Graphics.OpenGL
{
    internal class OpenGlContext : IOpenGlContext
    {
        public HGLRC OpenGlContextHandle => _glrc;

        private readonly HWND _windowHandle;
        private HDC _hdc;
        private HGLRC _glrc;

        private const uint GlBlend = 0x0BE2;
        private const uint GlSrcAlpha = 0x0302;
        private const uint GlOneMinusSrcAlpha = 0x0303;

        private delegate int WglSwapIntervalExtDelegate(VSyncState interval);

        /// <summary>
        /// Directly initializes a OpenGL context.
        /// Sets PixelFormat, Creates a context, sets context to callee thread & sets blending.
        /// </summary>
        /// <param name="windowHandle">Constructed transparent window handle.</param>
        public OpenGlContext(HWND windowHandle)
        {
            _windowHandle = windowHandle;

            Initialize();
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void MakeCurrent()
        {
            if (!PInvoke.wglMakeCurrent(_hdc, _glrc))
            {
                throw new OpenGlException("Failed to make OpenGL context current.");
            }
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void DeleteContext()
        {
            if (_glrc != nint.Zero)
            {
                PInvoke.wglMakeCurrent((HDC)nint.Zero, (HGLRC)nint.Zero);
                PInvoke.wglDeleteContext(_glrc);
                _glrc = (HGLRC)nint.Zero;
            }

            if (_hdc == nint.Zero)
            {
                return;
            }

            _ = PInvoke.ReleaseDC(_windowHandle, _hdc);
            _hdc = (HDC)nint.Zero;
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void EnableBlending()
        {
            PInvoke.glEnable(GlBlend);
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void SetBlendFunction(uint sFactor, uint dFactor)
        {
            PInvoke.glBlendFunc(sFactor, dFactor);
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void Viewport(int x, int y, int width, int height)
        {
            PInvoke.glViewport(x, y, width, height);
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void SwapBuffers()
        {
            PInvoke.SwapBuffers(_hdc);
        }

        /// <inheritdoc cref="IOpenGlContext"/>
        public void ConfigureVSync(VSyncState vSyncState)
        {
            var processAddress = PInvoke.wglGetProcAddress("wglSwapIntervalEXT");
            if (processAddress.IsNull)
            {
                return; // wglSwapIntervalEXT not supported.
            }

            var wglSwapIntervalExt = Marshal.GetDelegateForFunctionPointer<WglSwapIntervalExtDelegate>(processAddress);
            wglSwapIntervalExt(vSyncState);
        }

        private void Initialize()
        {
            _hdc = PInvoke.GetDC(_windowHandle);

            var pfd = new PIXELFORMATDESCRIPTOR
            {
                nSize = (ushort)Unsafe.SizeOf<PIXELFORMATDESCRIPTOR>(),
                nVersion = 1,
                dwFlags = PFD_FLAGS.PFD_DRAW_TO_WINDOW | PFD_FLAGS.PFD_SUPPORT_OPENGL | PFD_FLAGS.PFD_DOUBLEBUFFER,
                iPixelType = PFD_PIXEL_TYPE.PFD_TYPE_RGBA,
                cColorBits = 32,
                cAlphaBits = 8,
                cDepthBits = 24,
                cStencilBits = 8,
                iLayerType = PFD_LAYER_TYPE.PFD_MAIN_PLANE
            };

            var pixelFormat = PInvoke.ChoosePixelFormat(_hdc, in pfd);
            if (pixelFormat == 0)
            {
                throw new OpenGlException("Failed to choose pixel format.");
            }

            if (!PInvoke.SetPixelFormat(_hdc, pixelFormat, in pfd))
            {
                throw new OpenGlException("Failed to set pixel format.");
            }

            _glrc = PInvoke.wglCreateContext(_hdc);
            if (_glrc.Value == nint.Zero)
            {
                throw new OpenGlException("Failed to create OpenGL context.");
            }

            if (!PInvoke.wglMakeCurrent(_hdc, _glrc))
            {
                throw new OpenGlException("Failed to make OpenGL context current.");
            }

            EnableBlending();
            SetBlendFunction(GlSrcAlpha, GlOneMinusSrcAlpha);
        }
    }
}
