using Windows.Win32.Graphics.OpenGL;
using OverlaySharp.Graphics.OpenGL.Enums;

namespace OverlaySharp.Graphics.OpenGL
{
    /// <summary>
    /// <see cref="IOpenGlContext"/> manages the OpenGL rendering context.
    /// </summary>
    internal interface IOpenGlContext
    {
        public HGLRC OpenGlContextHandle { get; }

        /// <summary>
        /// Makes the OpenGL context current on the calling thread.
        /// </summary>
        void MakeCurrent();

        /// <summary>
        /// Deletes the OpenGL rendering context and releases associated resources.
        /// </summary>
        void DeleteContext();

        /// <summary>
        /// Enables blending in the OpenGL context for handling transparency.
        /// </summary>
        void EnableBlending();

        /// <summary>
        /// Sets blending function used to determine how this bitmap is blended with the current frame buffer color. The default setting is SourceAlpha for source and OneMinusSourceAlpha for destination.
        /// See OpenGLs glBlendFunc for details. https://registry.khronos.org/OpenGL-Refpages/gl4/html/glBlendFunc.xhtml
        /// </summary>
        /// <param name="sFactor">Specifies how the red, green, blue, and alpha source blending factors are computed.</param>
        /// <param name="dFactor">Specifies how the red, green, blue, and alpha destination blending factors are computed</param>
        void SetBlendFunction(uint sFactor, uint dFactor);

        /// <summary>
        /// Sets the viewport dimensions for the OpenGL context.
        /// See OpenGLs glViewport for details. https://registry.khronos.org/OpenGL-Refpages/gl4/html/glViewport.xhtml
        /// </summary>
        /// <param name="x">Specifies the lower left corner of the viewport rectangle, in pixels.</param>
        /// <param name="y">Specifies the lower left corner of the viewport rectangle, in pixels.</param>
        /// <param name="width">Specifies the width of the viewport.</param>
        /// <param name="height">Specifies the height of the viewport.</param>
        void Viewport(int x, int y, int width, int height);

        /// <summary>
        /// The SwapBuffers function exchanges the front and back buffers if the current pixel format for the window referenced by the specified device context includes a back buffer.
        /// See <see href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-swapbuffers">SwapBuffers</see>
        /// </summary>
        void SwapBuffers();

        /// <summary>
        /// Configures vertical synchronization (VSync) for the OpenGL context to control frame presentation timing.
        /// </summary>
        /// <param name="vSyncState">The desired VSync state, such as On, Off, or Adaptive. <see cref="VSyncState"/></param>
        void ConfigureVSync(VSyncState vSyncState);
    }
}
