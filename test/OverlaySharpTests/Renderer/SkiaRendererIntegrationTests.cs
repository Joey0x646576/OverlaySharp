namespace OverlaySharp.Tests.Renderer
{
    internal class SkiaRendererIntegrationTests
    {
        private IOpenGlContext _context = null!;

        [SetUp]
        public void SetUp()
        {
            _context = OpenGlContextFixture.OpenGlContext;
        }

        [Test]
        public void Initialize_WithValidInput_CreatesCanvas()
        {
            using var skiaRenderer = new SkiaRenderer(_context);
            skiaRenderer.Initialize(811, 600);

            Assert.That(skiaRenderer.Canvas, Is.Not.Null);
            Assert.That(skiaRenderer.Canvas.DeviceClipBounds.Width, Is.EqualTo(800));
            Assert.That(skiaRenderer.Canvas.DeviceClipBounds.Height, Is.EqualTo(600));
        }

        [Test]
        public void Resize_WithNewDimensions_UpdatesCanvasSize()
        {
            using var skiaRenderer = new SkiaRenderer(_context);
            skiaRenderer.Initialize(800, 600);

            skiaRenderer.Resize(1024, 768);

            Assert.That(skiaRenderer.Canvas, Is.Not.Null);
            Assert.That(skiaRenderer.Canvas.DeviceClipBounds.Width, Is.EqualTo(1024));
            Assert.That(skiaRenderer.Canvas.DeviceClipBounds.Height, Is.EqualTo(768));
        }
    }
}
