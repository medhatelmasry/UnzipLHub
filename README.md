# UnzipLHub
This is a console application that facilitates the extraction of files from D2L (A.K.A. Learning Hub).

### Disclaimer
* application was developed at BCIT where student numbers are in the format of A00######.
* app was developed and tested on Windows 10 and macOS
* app works with the current version of .NET, which happens to be .NET 6.0

### Usage
1. Inside of D2L (Learning Hub), select multiple files then download onto your computer's file system
2. Create source and target directories. Example: d:\work\src and d:\work\target
3. Extract the file that you downloaded to the source directory
4. Edit the configuration file name *appsettings.json*, which looks like this:

```
{
    "Directories": {
      "SourcePath": "D:\\work\\s",
      "TargetPath": "D:\\work\\t"
    },
    "Sorting": {
        // "SortBy": "last-name" | "student-number"
        "SortBy": "last-name"
    },
    "Files": {
        // "DeleteFilesInTargetFirectory": "yes" | "no"
        "DeleteFilesInTargetDirectory": "yes",
        "CopyFileTypes": "html,xls,txt"
    }
}
```
  
Note these configuration settings:

| Setting | Description |
| ------- | ----------- |
| Directories:SourcePath | directory that contains the files that were extracted from the D2L (Learning Hub) download |
| Directories:TargetPath | directory which the files will be extracted to  |
| Sorting:SortBy | Sorting order that you wish the files to be extracted by. Your options are *last-name* or *student-number* |
| Files:DeleteFilesInTargetDirectory | *yes* if you want files in the target directory to be deleted before the extraction process starts |
| Files:CopyFileTypes | file types that will be copied from source to destination directories (example: html,xls,txt) |

5. to run the application enter the following from a console window:

dotnet run

6. A log file is created in the filename format of yyyymmdd_log.txt.

### Creating a self-contained executeable file

This will create a self containe exe file in the dist folder on Windows or osx

| Operating System | Self-contained publish command |
| ---------------- | ------------------------------ |
| Windows x64 | dotnet publish -o dist --runtime win-x64 -p:PublishSingleFile=true --self-contained true |
| macOS x64   | dotnet publish -o dist --runtime osx.10.11-x64 -p:PublishSingleFile=true --self-contained true |

On macOS, you need to grant execute rights by calling:

```
chmod a+x ./UnzipLHub
```
