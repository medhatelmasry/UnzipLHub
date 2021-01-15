using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UnzipLHub.Models;

namespace UnzipLHub
{
    class Program
    {
        static void Main(string[] args)
        {

            IConfiguration Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .AddCommandLine(args)
              .Build();

            var sourcePath = Configuration["Directories:SourcePath"];
            var targetPath = Configuration["Directories:TargetPath"];
            var sortBy = Configuration["Sorting:SortBy"];
            var deleteFilesInTargetDirectory = Configuration["Files:DeleteFilesInTargetDirectory"];

            Console.WriteLine("UnzipLHub version 1.0.0");

            string[] zipFiles = Directory.GetFiles(sourcePath, "*.zip");

            if (deleteFilesInTargetDirectory != null && deleteFilesInTargetDirectory == "yes")
            {
                int result = Helper.DeleteAllFilesInFolder(targetPath);
                if (result == -1) return;
            }

            string lastName;

            foreach (var zip in zipFiles)
            {
                string filename = Path.GetFileNameWithoutExtension(zip);

                int startIndex = filename.IndexOf("A00");
                if (startIndex < 0 || startIndex > 16)
                {
                    // this represents a group filename
                    startIndex = filename.IndexOf(" - ");
                    startIndex += 3;
                }
                int endIndex = Helper.GetMonthIndex(filename);

                if (sortBy != null && sortBy == "last-name")
                {
                    lastName = Helper.GetLastNameBefore(filename, endIndex);
                    string dirName = lastName + filename.Substring(startIndex, endIndex - startIndex);
                    string destination = targetPath + Path.DirectorySeparatorChar + dirName + Path.DirectorySeparatorChar;
                    try
                    {
                        ZipFile.ExtractToDirectory(zip, destination);
                    }
                    catch (Exception ex)
                    {
                        Helper.Log($"sortBy = last-name\r\n {ex.ToString()}");
                    }
                }
                else
                {
                    if (endIndex > -1)
                    {
                        string dirName = filename.Substring(startIndex, endIndex - startIndex);
                        string destination = targetPath + Path.DirectorySeparatorChar + dirName + Path.DirectorySeparatorChar;
                        try
                        {
                            ZipFile.ExtractToDirectory(zip, destination);
                        }
                        catch (Exception ex)
                        {
                            Helper.Log($"sortBy = student-number\r\n {ex.ToString()}");
                        }
                    }

                }

                Console.WriteLine("Extracted {0}", zip);
            }

            // copy any html files
            string[] htmlFiles = Directory.GetFiles(sourcePath, "*.html");
            foreach (var source in htmlFiles)
            {
                Helper.CopyFileToTarget(source, targetPath);
            }

            // copy any xls (Excel) files
            string[] excelFiles = Directory.GetFiles(sourcePath, "*.xls");
            foreach (var source in excelFiles)
            {
                Helper.CopyFileToTarget(source, targetPath);
            }

            // copy any txt files
            string[] txtFiles = Directory.GetFiles(sourcePath, "*.txt");
            foreach (var source in txtFiles)
            {
                Helper.CopyFileToTarget(source, targetPath);
            }
        }

    }
}
