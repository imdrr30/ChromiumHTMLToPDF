# ChromiumHTMLToPDF
## Self-Managed PDF Generation from HTML using ChromiumEngine

### Usage For PDF Generation:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
PDF pdfCreator = new PDF();
pdfCreator.AddArgument("--headless");
byte[] pdf = pdfCreator.From(htmlContent);
```
### Usage For ScreenCapture:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
PDF screenCapture = new ScreenCapture();
screenCapture.AddArgument("--headless");
byte[] png = screenCapture.From(htmlContent);
```
