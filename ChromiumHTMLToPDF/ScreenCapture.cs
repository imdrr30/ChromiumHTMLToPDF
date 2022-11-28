using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ChromiumHTMLToPDF.Assets;

namespace ChromiumHTMLToPDF
{
    public class ScreenCapture
    {

        public static string chromium;
        public static string workingDir;
        public bool streamOutput;
        private static List<string> pngArgs;

        public ScreenCapture(bool streamOutput = false)
        {
            chromium = ConverterExecutable.Get().FullConverterExecutableFilename;
            workingDir = ConverterExecutable.GetWorkingDir();
            pngArgs = new List<string>();
            this.streamOutput = streamOutput;

        }
        public void AddOption(string arg)
        {
            try
            {
                pngArgs.Add(arg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Option failed due to " + ex.Message);
            }
        }

        public byte[] From(string htmlContent)
        {
            try
            {
                var tempFileName = Utilities.GetRandomString();
                return CreatePNGFromHTML(htmlContent, tempFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Screenshot was failed due to " + ex.Message);
                return null;
            }

        }

        private void CreateTempHTMLFile(string htmlContent, string fileName)
        {
            try
            {
                File.WriteAllText(fileName, htmlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Temporary HTML failed to create due to " + ex.Message);
            }
        }

        private void AddDefaultArguments()
        {
            AddOption("--headless");
            AddOption("--disable-gpu");
        }

        private byte[] CreatePNGFromHTML(string htmlContent, string fileName)

        {
            try
            {
                var htmlLocation = Path.Combine(workingDir, fileName + ".html");
                var pngLocation = Path.Combine(workingDir, fileName + ".png");

                CreateTempHTMLFile(htmlContent, htmlLocation);
                AddDefaultArguments();
                AddOption("--screenshot=\"" + pngLocation + "\"");
                AddOption(htmlLocation);


                StartConvertionProcess();

                byte[] png = ReadPNG(pngLocation);

                DeleteTemporaryFiles(pngLocation, htmlLocation);

                return png;

            }
            catch (Exception ex)
            {
                Console.WriteLine("PNG Creation was failed due to " + ex.Message);
                return null;
            }
        }

        private void StartConvertionProcess()
        {
            try
            {
                var processInfo = GetChromiumProcess();
                var process = Process.Start(processInfo);
                if (this.streamOutput)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    string err = process.StandardError.ReadToEnd();
                    Console.WriteLine(err);
                }
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chromium Process was failed to start due to " + ex.Message);
            }
        }

        private static string GetCommandLineArguments()
        {
            string args = string.Join(" ", pngArgs.ToArray());
            return args;
        }

        private ProcessStartInfo GetChromiumProcess()
        {
            return new ProcessStartInfo
            {
                FileName = chromium,
                Arguments = GetCommandLineArguments(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
        }

        private byte[] ReadPNG(string fileName)
        {
            try
            {
                return File.ReadAllBytes(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot Read the PNG File due to " + ex.Message);
                return null;
            }

        }

        private void DeleteTemporaryFiles(string pngFile, string htmlFile)
        {
            try
            {
                DeleteFile(htmlFile);
                DeleteFile(pngFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot Delete Temporary files due to " + ex.Message);
            }

        }

        private void DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot Delete Temporary files due to " + ex.Message);
            }

        }
    }
}
