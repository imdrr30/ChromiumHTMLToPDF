using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ChromiumHTMLToPDF.Assets;

namespace ChromiumHTMLToPDF
{
    public class ScreenCapture
    {

        public static string chromium = ConverterExecutable.Get().FullConverterExecutableFilename;
        public static string workingDir = ConverterExecutable.GetWorkingDir();
        private static List<string> pngArgs = new List<string>();

        public ScreenCapture()
        {
            pngArgs = new List<string>();

        }

        public static void AddOption(string arg)
        {
            pngArgs.Add(arg);
        }

        public byte[] From(string htmlContent)
        {

            var tempFileName = Utilities.GetRandomString();
            return CreatePNGFromHTML(htmlContent, tempFileName);

        }

        private static void CreateTempHTMLFile(string htmlContent, string fileName)
        {
            File.WriteAllText(fileName, htmlContent);
        }

        private static void AddDefaultArguments()
        {
            AddOption("--headless");
            AddOption("--disable-gpu");
            AddOption("--no-sandbox");

        }

        private static byte[] CreatePNGFromHTML(string htmlContent, string fileName)

        {
            var htmlLocation = Path.Combine(workingDir, fileName + ".html");
            var pngLocation = Path.Combine(workingDir, fileName + ".png");

            CreateTempHTMLFile(htmlContent, htmlLocation);
            AddDefaultArguments();
            AddOption("--screenshot=\"" + pngLocation+ "\"");
            AddOption(htmlLocation);


            StartConvertionProcess();

            byte[] png = ReadPNG(pngLocation);

            DeleteTemporaryFiles(pngLocation, htmlLocation);

            return png;
        }

        private static void StartConvertionProcess()
        {
            var processInfo = GetChromiumProcess();
            var process = Process.Start(processInfo);

            string output = process.StandardOutput.ReadToEnd();

            //Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            //Console.WriteLine(err);
            process.WaitForExit();
        }

        private static string GetCommandLineArguments()
        {
            string args = string.Join(" ", pngArgs.ToArray());
            return args;
        }

        private static ProcessStartInfo GetChromiumProcess()
        {
            return new ProcessStartInfo
            {
                FileName = ConverterExecutable.Get().FullConverterExecutableFilename,
                Arguments = GetCommandLineArguments(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
        }

        private static byte[] ReadPNG(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private static void DeleteTemporaryFiles(string pngFile, string htmlFile)
        {
            DeleteFile(htmlFile);
            DeleteFile(pngFile);
        }

        private static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

    }
}
