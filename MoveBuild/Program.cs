using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MoveBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            string downloadPath = @"C:\Users\user\Downloads";

            string pathToMove = @"D:\Amaya\builds";

            string buildVersion, lastBuildPath, finalPath, appName;

            Console.WriteLine($"Default path where files come from: {downloadPath}.");

            string[] filesInPath = Directory.GetFiles(downloadPath);

            lastBuildPath = GetLastBuildPath(filesInPath);

            if (lastBuildPath == string.Empty)
            {
                Console.WriteLine("There are no builds.");
                Console.Read();
                return;
            }

            appName = GetAppName(lastBuildPath, downloadPath);

            Console.WriteLine($"App for replacing: {appName}.");

            buildVersion = GetBuildVersion();

            finalPath = CreatePath(appName, buildVersion, pathToMove);

            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);

                File.Move(lastBuildPath, Path.Combine(finalPath, $"{appName}.ipa"));
            }

            Process.Start("explorer.exe", finalPath);
        }

        static string GetLastBuildPath(string[] filesInPath)
        {
            if (filesInPath.Length == 0)
            {
                return string.Empty;
            }

            string lastBuildPath;
            List<string> ipaFiles = new List<string>();

            Array.Sort(filesInPath, (a, b) => new FileInfo(b).LastWriteTime.CompareTo(new FileInfo(a).LastWriteTime));

            foreach (var file in filesInPath)
            {
                if (file.EndsWith(".ipa"))
                {
                    ipaFiles.Add(file);
                }
            }

            if (ipaFiles.Count == 0)
            {
                return string.Empty;
            }
            
            lastBuildPath = ipaFiles[0];

            return lastBuildPath;
        }

        static string GetAppName(string buildPath, string path)
        {
            buildPath = buildPath.Replace($@"{path}\", "");
            string[] separatedBuildName = buildPath.Split('(');
            string appName;

            appName = separatedBuildName[0];
            appName = appName.TrimEnd();

            if (appName.EndsWith(".ipa"))
            {
                appName = appName.Replace(".ipa", "");
            }

            return appName;
        }

        static string GetBuildVersion()
        {
            string buildVersion;
            Console.Write("Write the build version: ");

            buildVersion = Console.ReadLine();

            if (!buildVersion.Any(char.IsDigit))
            {
                GetBuildVersion();
            }

            return buildVersion;
        }

        static string CreatePath(string appName, string version, string pathToMove)
        {
            string finalPath = $@"{pathToMove}\{appName}\{version}";

            return finalPath;
        }
    }
}
