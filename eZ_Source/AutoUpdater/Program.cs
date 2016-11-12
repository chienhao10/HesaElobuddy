using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Threading;

namespace AutoUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checking ezBot Version");
            var localVersion = LoadVersion();
            Console.WriteLine("Current ezBot Version: " + localVersion);
            var remoteVersion = LoadRemoteVersion();
            Console.WriteLine("Remote ezBot Version: " + remoteVersion);
            if(string.IsNullOrEmpty(localVersion) || string.IsNullOrEmpty(remoteVersion) || localVersion != remoteVersion)
            {
                ShutdownEzBot();
                Console.WriteLine("Currently updating ezBot please wait.");
                var updated = DownloadUpdate();
                if (updated)
                    Console.WriteLine("You are up to date!");
                else
                    Console.WriteLine("An error occured during the update. please tell Hesa!");
            }
            else
            {
                Console.WriteLine("You are up to date!");
            }
            Process.Start("ezBot.exe");
        }
        static string LoadVersion()
        {
            try
            {
                using (StreamReader reader = new StreamReader("version.txt"))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        static string LoadRemoteVersion()
        {
            try
            {
                WebClient webClient = new WebClient();
                return webClient.DownloadString("http://ezbot.curssor.com/version.txt");
            }catch(Exception ex)
            {
            }
            return null;
        }

        static bool DownloadUpdate()
        {
            try
            {
                var tempFileName = Environment.CurrentDirectory + @"\update.temp";
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://ezbot.curssor.com/update.zip", tempFileName);
                }
                Thread.Sleep(1000);
                if(File.Exists(tempFileName))
                {
                    using (FileStream stream = new FileStream(tempFileName, FileMode.Open))
                    {
                        using (ZipArchive archive = new ZipArchive(stream))
                        {
                            archive.ExtractToDirectory(Environment.CurrentDirectory, true);
                        }
                    }
                    File.Delete(tempFileName);
                }
                return true;
            }
            catch(Exception ex)
            {

            }
            return false;
        }

        static void ShutdownEzBot()
        {
            Process[] processes = Process.GetProcessesByName("ezBot");
            foreach(var process in processes)
            {
                process.Kill();
                Thread.Sleep(500);
            }
        }

    }
}