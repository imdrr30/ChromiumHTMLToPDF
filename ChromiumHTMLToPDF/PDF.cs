using System.Collections.Generic;
using System.IO;
using ChromiumHTMLToPDF.Assets;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace ChromiumHTMLToPDF
{
    public class PDF
    {

        public static string chromium = ConverterExecutable.Get().FullConverterExecutableFilename;
        public static string workingDir = ConverterExecutable.GetWorkingDir();
        private static List<string> pdfArgs = new List<string>();

        public PDF()
        {
            pdfArgs = new List<string>();

        }
        public void AddOption(string arg)
        {
            pdfArgs.Add(arg);
        }
        
        public byte[] From(string htmlContent)
        {

            var tempFileName = Utilities.GetRandomString();
            return CreatePDFFromHTML(htmlContent, tempFileName);

        }
        
        private static void CreateTempHTMLFile(string htmlContent, string fileName)
        {
            File.WriteAllText(fileName, htmlContent);
        }

        private void AddDefaultArguments()
        {
            AddOption("--headless");
            AddOption("--disable-gpu");
            AddOption("--no-sandbox");

        }

        private byte[] CreatePDFFromHTML(string htmlContent, string fileName)

        {
            var htmlLocation = Path.Combine(workingDir, fileName + ".html");
            var pdfLocation = Path.Combine(workingDir, fileName + ".pdf");

            CreateTempHTMLFile(htmlContent, htmlLocation);
            AddDefaultArguments();
            AddOption("--print-to-pdf=\""+ pdfLocation+ "\"");
            AddOption(htmlLocation);


            StartConvertionProcess();

            byte[] pdf = ReadPDF(pdfLocation);

            DeleteTemporaryFiles(pdfLocation, htmlLocation);

            return pdf;
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
            string args =  string.Join(" ", pdfArgs.ToArray());
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

        private static byte[] ReadPDF(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private static void DeleteTemporaryFiles(string pdfFile, string htmlFile)
        {
            DeleteFile(htmlFile);
            DeleteFile(pdfFile);
        }

        private static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }
    }
}
