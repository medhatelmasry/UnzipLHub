namespace UnzipLHub.Models;

public class Helper
{
    public static void Log(string logMessage, TextWriter w)
    {
        w.Write("\r\nLog Entry : ");
        w.WriteLine("{0} {1}", System.DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", logMessage);
        w.WriteLine("-------------------------------");
    }

    public static int DeleteAllFilesInFolder(string path)
    {
        try
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            int count = 0;
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
                count++;
            }

            return count;
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Directory {path} could not be found.");
            return -1;
        }
    }

    public static void Log(string msg)
    {
        string filename = DateTime.Now.ToString("yyyyMMdd") + "_log.txt";
        using (StreamWriter w = File.AppendText(filename))
        {
            Log(msg, w);
            Log(msg, w);
        }
    }

    public static string? GetLastNameBefore(string filename, int index)
    {
        int startLastName = -1;

        try {
            startLastName = filename.Substring(0, index).LastIndexOf("_");
        } catch {
            Console.WriteLine("It looks like the ZIP file being used is not a D2L (learning-hub) file.");
            return null;
        }

        return filename.Substring(startLastName + 1, index - startLastName);
    }

    public static int GetMonthIndex(string filename)
    {
        int index = -1;
        string[] months = { "Jan", "Feb", "Mar", "Apr", "May",
            "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        foreach (var mnth in months)
        {
            if (filename.IndexOf("_" + mnth + " ") > -1)
            {
                index = filename.IndexOf("_" + mnth);
                break;
            }
        }

        return index;
    }

    public static void CopyFileToTarget(string source, string targetPath) {
            string? filename = Path.GetFileName(source);
            string? sourcePath = Path.GetDirectoryName(source);

            Console.WriteLine("Copying {0}", filename);
            string? targetFilename = source.Replace(sourcePath!, targetPath!);
            File.Copy(source, targetFilename);
    }

}
