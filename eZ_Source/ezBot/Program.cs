using BananaLib;
using ezBot.Utils;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ezBot
{
    internal class Program
    {
        public static ArrayList accounts = new ArrayList();
        public static ArrayList accountsNew = new ArrayList();
        public static int maxBots = 1;
        public static bool replaceConfig = false;
        public static bool randomChampionPick = false;
        public static string firstChampionPick = "";
        public static string secondChampionPick = "";
        public static string thirdChampionPick = "";
        public static string fourthChampionPick = "";
        public static string fifthChampionPick = "";
        public static int maxLevel = 30;
        public static bool randomSpell = false;
        public static string spell1 = "flash";
        public static string spell2 = "ignite";
        public static string LoLVersion = "";
        public static bool buyExpBoost = false;
        public static int delay1 = 1;
        public static int delay2 = 1;
        public static int lolHeight = 200;
        public static int lolWidth = 300;
        public static bool LOWPriority = true;
        public static string lolPath;
        //Friends...
        public static bool queueWithFriends = false;
        public static string leaderName = "";
        public static string firstFriend = "";
        public static string secondFriend = "";
        public static string thirdFriend = "";
        public static string fourthFriend = "";

        public static string EzBotVersion = "Unknown";
        public static Action<string, string, string> OnInvite;

        private static void Main(string[] args)
        {
            EzBotVersion = LoadEzBotVersion();
            var remoteVersion = LoadRemoteVersion();
            if (string.IsNullOrEmpty(EzBotVersion) || string.IsNullOrEmpty(remoteVersion) || EzBotVersion != remoteVersion)
            {
                Process.Start("AutoUpdater.exe");
                Environment.Exit(0);
            }

            Program.LoadLeagueVersion();
            Console.Title = "ezBot";
            Tools.TitleMessage("ezBot - Auto Queue for LOL: " + Program.LoLVersion.Substring(0, 4));
            Tools.TitleMessage("Made by Tryller updated by Hesa.");
            Tools.TitleMessage("Version: " + EzBotVersion);
            Tools.ConsoleMessage("Skype: wisahesa", ConsoleColor.Magenta);
            
            Tools.ConsoleMessage("I uploaded the source code on github at github.com/hesa2020/HesaElobuddy.", ConsoleColor.Cyan);
            Tools.ConsoleMessage("Please report issue(s) on the ezbot thread customized by hesa on Elobuddy.net.", ConsoleColor.Cyan);

            if (!IsUserAdministrator())
            {
                Tools.ConsoleMessage("ezBot must be started with administrator privileges.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            LoadConfigs();
            Tools.ConsoleMessage("Config loaded.", ConsoleColor.White);

            try
            {
                if (lolPath.Contains("notfound"))
                    throw new Exception();
                var dir = Directory.EnumerateDirectories(lolPath + "RADS\\solutions\\lol_game_client_sln\\releases\\").OrderBy(f => new DirectoryInfo(f).CreationTime).Last() + "\\deploy\\";
            }catch(Exception)
            {
                Tools.ConsoleMessage("Your LauncherPath is invalid.", ConsoleColor.Red);
                Tools.ConsoleMessage("Please try this:", ConsoleColor.Red);
                Tools.ConsoleMessage("1. Make sure the path point to the FOLDER where we can find the launcher for league of legends and not to any exe file.", ConsoleColor.Red);
                Tools.ConsoleMessage("2. Make sure the LauncherPath ends with a \\", ConsoleColor.Red);
                Tools.ConsoleMessage("3. Browse to the LauncherPath", ConsoleColor.Red);
                Tools.ConsoleMessage("4. Browse to RADS\\solutions\\lol_game_client_sln\\releases\\", ConsoleColor.Red);
                Tools.ConsoleMessage("5. Delete all folder in here except: 0.0.1.152", ConsoleColor.Red);
            }
            
            if (replaceConfig)
            {
                Tools.ConsoleMessage("Changing Game Config.", ConsoleColor.White);
                ChangeGameConfig();
            }
            Tools.ConsoleMessage("Loading accounts.", ConsoleColor.White);
            Program.LoadAccounts();
            int num = 0;
            foreach (string account in Program.accounts)
            {
                try
                {
                    Program.accountsNew.RemoveAt(0);
                    string[] strArray = account.Split(new string[1] { "|" }, StringSplitOptions.None);
                    ++num;
                    var isLeader = strArray[4] != null ? (strArray[4].ToLower() == "leader" ? true : false) : true;
                    if (strArray[3] != null)
                    {
                        Generator.CreateRandomThread(Program.delay1, Program.delay2);
                        string queueType = strArray[3];

                        var region = Tools.ParseEnum<Region>(strArray[2].ToUpper());
                        var password = strArray[1];
                        if (region.UseGarena())
                        {
                            password = GetGarenaToken();
                        }
                        ezBot ezBot = new ezBot(strArray[0], password, strArray[2].ToUpper(), Program.lolPath, queueType, Program.LoLVersion, isLeader);
                    }
                    else
                    {
                        Generator.CreateRandomThread(Program.delay1, Program.delay2);
                        string queueType = "ARAM";
                        ezBot ezBot = new ezBot(strArray[0], strArray[1], strArray[2].ToUpper(), Program.lolPath, queueType, Program.LoLVersion, isLeader);
                    }
                    if (num != Program.maxBots)
                        Tools.ConsoleMessage("Maximum bots running: " + (object)Program.maxBots, ConsoleColor.Red);
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Tools.ConsoleMessage(ex.Message + " " + ex.StackTrace, ConsoleColor.Green);
                    Tools.ConsoleMessage("You may have an issue in your accounts.txt", ConsoleColor.Red);
                    Tools.ConsoleMessage("Acconts structure ACCOUNT|PASSWORD|REGION|QUEUE_TYPE|IS_LEADER", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
            while(true)
                Console.ReadKey();
        }

        public static string LoadEzBotVersion()
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

        public static string LoadRemoteVersion()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
                webClient.Headers.Add("Cache-Control", "no-cache");

                return webClient.DownloadString("https://raw.githubusercontent.com/hesa2020/HesaElobuddy/master/eZ_Source/version.txt");
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
            return isAdmin;
        }

        private static string GetGarenaToken()
        {
            string s1 = "";
            bool token = false;
            do
            {
                foreach (var process in Process.GetProcessesByName("lol"))
                {
                    try
                    {

                        s1 = GetCommandLine(process);
                        foreach (var p1 in Process.GetProcessesByName("lolclient"))
                        {
                            p1.Kill();
                        }
                        process.Kill();
                        s1 = s1.Substring(1);
                        token = true;
                    }
                    catch (Win32Exception ex)
                    {
                        Console.WriteLine("Error Get Garena Token");
                        if ((uint)ex.ErrorCode != 0x80004005)
                        {
                            throw;
                        }
                    }
                }
            } while (!token);
            return s1;

        }

        private static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder("");
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                }
            }
            return commandLine.ToString();
        }

        public static void LoadLeagueVersion()
        {
            Program.LoLVersion = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "configs\\version.txt").ReadLine();
        }

        private static void ChangeGameConfig()
        {
            try
            {
                var configFileIni = new IniFile(lolPath + "Config\\game.cfg");
                configFileIni.Write("General", "Height", Program.lolHeight.ToString());
                configFileIni.Write("General", "Width", Program.lolWidth.ToString());
            }
            catch (Exception ex)
            {
                Tools.ConsoleMessage("game.cfg Error: If using VMWare Shared Folder, make sure it is not set to Read-Only.\nException:" + ex.Message, ConsoleColor.Red);
            }
        }

        public static void LognNewAccount()
        {
            Program.accountsNew = Program.accounts;
            Program.accounts.RemoveAt(0);
            int num = 0;
            if (Program.accounts.Count == 0)
                Tools.ConsoleMessage("No more acocunts to login", ConsoleColor.Red);
            foreach (string account in Program.accounts)
            {
                string[] strArray = account.Split(new string[1] { "|" }, StringSplitOptions.None);
                ++num;
                var isLeader = string.IsNullOrEmpty(strArray[4]) ? true : (strArray[4].ToLower() == "leader" ? true : false);
                if (strArray[3] != null)
                {
                    Generator.CreateRandomThread(Program.delay1, Program.delay2);
                    string queueType = strArray[3];
                    ezBot ezBot = new ezBot(strArray[0], strArray[1], strArray[2].ToUpper(), Program.lolPath, queueType, Program.LoLVersion, isLeader);
                }
                else
                {
                    Generator.CreateRandomThread(Program.delay1, Program.delay2);
                    string queueType = "ARAM";
                    ezBot ezBot = new ezBot(strArray[0], strArray[1], strArray[2].ToUpper(), Program.lolPath, queueType, Program.LoLVersion, isLeader);
                }
                if (num == Program.maxBots)
                    break;
                Tools.ConsoleMessage("Maximun bots running: " + (object)Program.maxBots, ConsoleColor.Red);
            }
        }

        public static void LoadConfigs()
        {
            try
            {
                IniFile iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "configs\\settings.ini");
                lolPath = iniFile.Read("GENERAL", "LauncherPath");
                maxBots = Convert.ToInt32(iniFile.Read("GENERAL", "MaxBots"));
                maxLevel = Convert.ToInt32(iniFile.Read("GENERAL", "MaxLevel"));
                randomSpell = Convert.ToBoolean(iniFile.Read("GENERAL", "RandomSpell"));
                spell1 = iniFile.Read("GENERAL", "Spell1").ToUpper();
                spell2 = iniFile.Read("GENERAL", "Spell2").ToUpper();
                delay1 = Convert.ToInt32(iniFile.Read("ACCOUNT", "MinDelay"));
                delay2 = Convert.ToInt32(iniFile.Read("ACCOUNT", "MaxDelay"));
                buyExpBoost = Convert.ToBoolean(iniFile.Read("ACCOUNT", "BuyExpBoost"));
                randomChampionPick = Convert.ToBoolean(iniFile.Read("CHAMPIONS", "PickRandomlyFromThisList"));
                firstChampionPick = iniFile.Read("CHAMPIONS", "FirstChampionPick");
                secondChampionPick = iniFile.Read("CHAMPIONS", "SecondChampionPick");
                thirdChampionPick = iniFile.Read("CHAMPIONS", "ThirdChampionPick");
                fourthChampionPick = iniFile.Read("CHAMPIONS", "FourthChampionPick");
                fifthChampionPick = iniFile.Read("CHAMPIONS", "FifthChampionPick");
                replaceConfig = Convert.ToBoolean(iniFile.Read("LOLSCREEN", "ReplaceLoLConfig"));
                lolHeight = Convert.ToInt32(iniFile.Read("LOLSCREEN", "SreenHeight"));
                lolWidth = Convert.ToInt32(iniFile.Read("LOLSCREEN", "SreenWidth"));
                LOWPriority = Convert.ToBoolean(iniFile.Read("LOLSCREEN", "LOWPriority"));
                try
                {
                    queueWithFriends = Convert.ToBoolean(iniFile.Read("FRIENDS", "QueueWithFriends"));
                    leaderName = iniFile.Read("FRIENDS", "LeaderName");
                    firstFriend = iniFile.Read("FRIENDS", "FirstFriend");
                    secondFriend = iniFile.Read("FRIENDS", "SecondFriend");
                    thirdFriend = iniFile.Read("FRIENDS", "ThirdFriend");
                    fourthFriend = iniFile.Read("FRIENDS", "FourthFriend");
                }
                catch(Exception ex)
                {
                    iniFile.Write("FRIENDS", "QueueWithFriends", "false");
                    iniFile.Write("FRIENDS", "LeaderName", "");
                    iniFile.Write("FRIENDS", "FirstFriend", "");
                    iniFile.Write("FRIENDS", "SecondFriend", "");
                    iniFile.Write("FRIENDS", "ThirdFriend", "");
                    iniFile.Write("FRIENDS", "FourthFriend", "");
                }
            }
            catch (Exception ex)
            {
                Tools.Log(ex.Message);
                Thread.Sleep(10000);
                Application.Exit();
            }
        }

        public static void LoadAccounts()
        {
            TextReader textReader = (TextReader)File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "configs\\accounts.txt");
            string str;
            while ((str = textReader.ReadLine()) != null)
            {
                Program.accounts.Add((object)str);
                Program.accountsNew.Add((object)str);
            }
            textReader.Close();
        }

        private static int victory = 0;
        private static int defeat = 0;
        
        public static void AddGame(bool won)
        {
            if (won) victory++;
            else defeat++;
            Console.Title = string.Format("ezBot - {0} Total - {1} Victory - {2} Defeat", victory + defeat, victory, defeat);
        }

    }
}