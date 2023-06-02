using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace MoveBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            string downloadPath = @"C:\Users\user\Downloads";

            string pathToMove = @"D:\Amaya\builds";

            string buildVersion, lastBuildPath, finalPath, appName;

            string[] allBuilds = Directory.GetFiles(downloadPath);

            lastBuildPath = TakeLastBuild(allBuilds);

            appName = GetAppName(lastBuildPath, downloadPath);

            Console.WriteLine(appName);

            buildVersion = SetBuildVersion();

            finalPath = CreatePath(appName, buildVersion, pathToMove);

            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);

                File.Move(lastBuildPath, Path.Combine(finalPath, $"{appName}.ipa"));
            }
            
            Process.Start("explorer.exe", finalPath);
        }

        static string TakeLastBuild(string[] allBuilds)
        {
            string lastBuild;

            do
            {
                Array.Sort(allBuilds, (a, b) => new FileInfo(b).LastWriteTime.CompareTo(new FileInfo(a).LastWriteTime));

            } while (!allBuilds[0].EndsWith(".ipa"));

            lastBuild = allBuilds[0];

            return lastBuild;
        }

        static string GetAppName(string buildPath, string path)
        {
            buildPath = buildPath.Replace($@"{path}\", "");
            string[] separatedBuildName = buildPath.Split('(');

            buildPath = separatedBuildName[0];
            buildPath = buildPath.TrimEnd();

            if (buildPath.EndsWith(".ipa"))
            {
               buildPath = buildPath.Replace(".ipa", "");
            }

            return buildPath;
        }

        static string SetBuildVersion()
        {
            string buildVersion;
            Console.WriteLine("Введите номер билда");

            buildVersion = Console.ReadLine();

            if (!buildVersion.Any(char.IsDigit))
            {
                SetBuildVersion();
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
