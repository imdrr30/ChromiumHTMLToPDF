using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChromiumHTMLToPDF;
using System.IO;

namespace ChromiumHTMLToPDF.Tests
{
    [TestClass]
    public class PDFTests
    {
        private string htmlContent = "<h1>Hello World!</h1>";

        [TestMethod]
        public void validPdf()
        {
            var pdfCreator = new PDF();
            
            var pdf = pdfCreator.From(htmlContent);
            File.WriteAllBytes("../../../out1.pdf", pdf);

            Assert.IsInstanceOfType(pdf, typeof(byte[]));
            
        }
    }
}
