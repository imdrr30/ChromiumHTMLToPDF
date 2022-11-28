using System;
using System.IO;
using System.Reflection;

namespace ChromiumHTMLToPDF.Assets
{
    sealed class ConverterExecutable
    {
        private static string ConverterExecutableFilename = Path.Combine("chrome.win", "chrome.exe");
        private const string ConverterExecutableZip = "chrome.win.zip";


        private ConverterExecutable()
        {
        }

        public static ConverterExecutable Get()
        {
            var bundledFile = new ConverterExecutable();

            bundledFile.CreateIfConverterExecutableDoesNotExist();

            return bundledFile;
        }

        public static string GetWorkingDir()
        {
            return Path.Combine( BundledFilesDirectory(), "chrome.win", "Temp");
        }

        public string FullConverterExecutableFilename
        {
            get { return ResolveFullPathToConverterExecutableFile(); }
        }

        public string FullConverterExecutableZip
        {
            get { return ResolveFullPathToConverterZip(); }
        }

        private void CreateIfConverterExecutableDoesNotExist()
        {
            if (!File.Exists(FullConverterExecutableFilename))
                Create(GetConverterExecutableContent());
        }

        private static byte[] GetConverterExecutableContent()
        {
            using (var resourceStream = GetConverterExecutable())
            {
                var resource = new byte[resourceStream.Length];

                resourceStream.Read(resource, 0, resource.Length);

                return resource;
            }
        }

        private static Stream GetConverterExecutable()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("ChromiumHTMLToPDF.Assets.chrome.win.zip");
        }

        private void Create(byte[] fileContent)
        {
            try
            {
                if (!Directory.Exists(BundledFilesDirectory()))
                    Directory.CreateDirectory(BundledFilesDirectory());


                using (var file = File.Open(FullConverterExecutableZip, FileMode.Create))
                {

                    file.Write(fileContent, 0, fileContent.Length);
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(FullConverterExecutableZip, BundledFilesDirectory());

                File.Delete(FullConverterExecutableZip);

            }
            catch (IOException)
            {
            }
        }

        private static string ResolveFullPathToConverterExecutableFile()
        {
            return Path.Combine(BundledFilesDirectory(), ConverterExecutableFilename);
        }

        private static string ResolveFullPathToConverterZip()
        {
            return Path.Combine(BundledFilesDirectory(), ConverterExecutableZip);
        }

        private static string BundledFilesDirectory()
        {
            return Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ChromiumHTMLToPDF", Version());
        }

        private static string Version()
        {
            return string.Format("{0}_{1}",
                Assembly.GetExecutingAssembly().GetName().Version,
                Environment.Is64BitProcess ? 64 : 32);
        }
    }
}