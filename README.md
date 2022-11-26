### Update 1.1.1 Fixed AddOption() accessibility issue.

# ChromiumHTMLToPDF - Windows Only
## Self-Managed PDF Generation from HTML using ChromiumEngine

### Usage For PDF Generation:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
PDF pdfCreator = new PDF();
pdfCreator.AddOption("--headless");
byte[] pdf = pdfCreator.From(htmlContent);
```
### Usage For ScreenCapture:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
ScreenCapture screenCapture = new ScreenCapture();
screenCapture.AddOption("--window-size=800,600"); //Image size width,height
screenCapture.AddOption("—hide-scrollbars");
byte[] png = screenCapture.From(htmlContent);
```
### By default, “—headless” and “—disable-gpu” is added. Add additional command line arguments by using AddOption(“argument”);
