IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var sourcePath = Configuration["Directories:SourcePath"];
var targetPath = Configuration["Directories:TargetPath"];
var sortBy = Configuration["Sorting:SortBy"];
var deleteFilesInTargetDirectory = Configuration["Files:DeleteFilesInTargetDirectory"];
var copyFileTypes = Configuration["Files:CopyFileTypes"];

Console.WriteLine("UnzipLHub version 1.0.0");

string[] zipFiles = Directory.GetFiles(sourcePath!, "*.zip");

if (deleteFilesInTargetDirectory != null && deleteFilesInTargetDirectory == "yes")
{
    int result = Helper.DeleteAllFilesInFolder(targetPath!);
    if (result == -1) return;
}

List<string>? fileTypes = null;
if (!string.IsNullOrEmpty(copyFileTypes))
    fileTypes = copyFileTypes.Split(',').ToList();

string? lastName;

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

foreach (var item in fileTypes!)
{
    string[] htmlFiles = Directory.GetFiles(sourcePath!, $"*.{item}");
    foreach (var source in htmlFiles)
    {
        Helper.CopyFileToTarget(source, targetPath!);
    }
}
