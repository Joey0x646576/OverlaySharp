namespace OverlaySharp.Tests.OpenGl
{
    public class OpenGlContextIntegrationTests
    {
        private IOpenGlContext _context = null!;

        [SetUp]
        public void SetUp()
        {
            _context = OpenGlContextFixture.OpenGlContext;
        }

        [Test]
        public void MakeCurrent_DoesNotThrow()
        {
            Assert.DoesNotThrow(_context.MakeCurrent);
        }

        [Test]
        public void EnableBlending_DoesNotThrow()
        {
            Assert.DoesNotThrow(_context.EnableBlending);
        }
    }
}
