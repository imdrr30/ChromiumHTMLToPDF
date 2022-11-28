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

        public static string chromium;
        public static string workingDir;
        public bool streamOutput;
        private static List<string> pdfArgs;

        public PDF(bool streamOutput=false)
        {
            chromium = ConverterExecutable.Get().FullConverterExecutableFilename;
            workingDir = ConverterExecutable.GetWorkingDir();
            pdfArgs = new List<string>();
            this.streamOutput = streamOutput;

        }
        public void AddOption(string arg)
        {
            try
            {
                pdfArgs.Add(arg);
            }catch(Exception ex)
            {
                Console.WriteLine("Add Option failed due to "+ex.Message);
            }
        }
        
        public byte[] From(string htmlContent)
        {
            try
            {
                var tempFileName = Utilities.GetRandomString();
                return CreatePDFFromHTML(htmlContent, tempFileName);
            }catch(Exception ex)
            {
                Console.WriteLine("PDF Creation was failed due to " + ex.Message);
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

        private byte[] CreatePDFFromHTML(string htmlContent, string fileName)

        {
            try
            {
                var htmlLocation = Path.Combine(workingDir, fileName + ".html");
                var pdfLocation = Path.Combine(workingDir, fileName + ".pdf");

                CreateTempHTMLFile(htmlContent, htmlLocation);
                AddDefaultArguments();
                AddOption("--print-to-pdf=\"" + pdfLocation + "\"");
                AddOption(htmlLocation);


                StartConvertionProcess();

                byte[] pdf = ReadPDF(pdfLocation);

                DeleteTemporaryFiles(pdfLocation, htmlLocation);

                return pdf;

            }catch(Exception ex)
            {
                Console.WriteLine("PDF Creation was failed due to " + ex.Message);
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
            }catch(Exception ex)
            {
                Console.WriteLine("Chromium Process was failed to start due to " + ex.Message);
            }
        }

        private string GetCommandLineArguments()
        {
            string args =  string.Join(" ", pdfArgs.ToArray());
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

        private byte[] ReadPDF(string fileName)
        {
            try
            {
                return File.ReadAllBytes(fileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Cannot Read the PDF File due to " + ex.Message);
                return null;
            }
            
        }

        private void DeleteTemporaryFiles(string pdfFile, string htmlFile)
        {
            try
            {
                DeleteFile(htmlFile);
                DeleteFile(pdfFile);
            }
            catch(Exception ex)
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
