using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChromiumHTMLToPDF;

namespace ChromiumHTMLToPDFTests
{
    [TestClass]
    public class PDFTests
    {
        private string htmlContent = "<h1>Hello World!</h1>";

        [TestMethod]
        public void ValidatePdf()
        {
            var pdfCreator = new PDF();
            var pdf = pdfCreator.From(htmlContent);

            Assert.IsInstanceOfType(pdf, typeof(byte[]));
            
        }

        [TestMethod]
        public void validatePNG()
        {
            var screenCapture = new ScreenCapture();
            screenCapture.AddOption("--hide-scrollbars");
            screenCapture.AddOption("--window-size=1024,1024");
            var png = screenCapture.From(htmlContent);

            Assert.IsInstanceOfType(png, typeof(byte[]));

        }
    }
}
