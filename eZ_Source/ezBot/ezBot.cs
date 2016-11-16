using BananaLib;
using BananaLib.RiotObjects.Platform;
using BananaLib.RiotObjects.Team;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ezBot
{
    internal class ezBot
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        public LoginDataPacket loginPacket = new LoginDataPacket();
        public bool firstTimeInLobby = true;
        public bool firstTimeInQueuePop = true;
        public bool firstTimeInCustom = true;
        public bool firstTimeInPostChampSelect = true;
        public Process exeProcess;
        public LoLClient connection;
        public string Accountname;
        public string Password;
        public string ipath;
        public bool useGarena;
        public ChampionDTO[] AvailableChampions;
        public DateTime? GameStartedAt = null;

        public string region { get; set; }

        public string sumName { get; set; }

        public double sumId { get; set; }

        public double sumLevel { get; set; }

        public double archiveSumLevel { get; set; }

        public double rpBalance { get; set; }

        public double ipBalance { get; set; }

        public double m_leaverBustedPenalty { get; set; }

        public string m_accessToken { get; set; }

        public string queueType { get; set; }

        public string actualQueueType { get; set; }

        private bool ShouldBeInGame { get; set; }
        private bool IsInQueue { get; set; }

        private LobbyStatus Lobby { get; set; }

        private bool m_isLeader { get; set; }

        private int pickAtTurn = 0;
        private int turn = 0;

        private ConsoleEventDelegate handler;   // Keeps it from getting garbage collected

        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //
        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(int hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(int hWnd, StringBuilder title, int size);

        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        private bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                try
                {
                    connection.LeaveGroupFinderLobby();
                    connection.DestroyGroupFinderLobby();
                    connection.Disconnect();
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                }
            }
            return false;
        }

        public ezBot(string username, string password, string reg, string queueType, string LoLVersion, bool isLeader)
        {
            this.useGarena = Tools.ParseEnum<Region>(reg).UseGarena();
            this.ipath = useGarena ? Program.lolGarenaPath : Program.lolPath;
            this.queueType = queueType;
            this.region = reg;
            this.connection = new LoLClient(username, password, Tools.ParseEnum<Region>(this.region), LoLVersion);
            this.Accountname = username;
            this.Password = password;
            this.connection.OnConnect += connection_OnConnect;
            this.connection.OnDisconnect += connection_OnDisconnect;
            this.connection.OnError += connection_OnError;
            this.connection.OnLogin += connection_OnLogin;
            this.connection.OnMessageReceived += connection_OnMessageReceived;
            this.connection.ConnectAndLogin().Wait();
            this.m_isLeader = isLeader;

            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            Program.OnInvite += OnReceiveInvite;

            new Thread(() =>
            {
                while (true)
                {
                    if (exeProcess != null)
                    {
                        foreach (ProcessThread processThread in exeProcess.Threads)
                        {
                            EnumThreadWindows(processThread.Id,
                             (hWnd, lParam) =>
                             {
                                 //Check if Window is Visible or not.
                                 if (!IsWindowVisible((int)hWnd))
                                     return true;

                                 //Get the Window's Title.
                                 StringBuilder title = new StringBuilder(256);
                                 GetWindowText((int)hWnd, title, 256);

                                 //Check if Window has Title.
                                 //if (title.Length == 0)
                                     //return true;

                                 //_childrenhWnd.Add(hWnd);

                                 if(title.ToString().ToLower() == "network warning")
                                 {
                                     exeProcess.Kill();
                                     //await Task.Delay(1000);
                                     Thread.Sleep(1000);
                                     if (exeProcess.Responding)
                                         Process.Start("taskkill /F /PID " + exeProcess.Id);//Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                                 }

                                 return true;
                             }, IntPtr.Zero);
                        }
                        /*
                        var childCount = new WindowHandleInfo(exeProcess.MainWindowHandle).GetAllChildHandles().Count;
                        //if (exeProcess.HandleCount > 1)
                        //Console.WriteLine("Child Count = " + childCount);
                        //Console.WriteLine("Handle Count = " + exeProcess.HandleCount);
                        if (childCount > 0)
                        {
                            exeProcess.Kill();
                            await Task.Delay(1000);
                            if (exeProcess.Responding)
                                Process.Start("taskkill /F /PID " + exeProcess.Id);//Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                        }*/
                    }
                    Thread.Sleep(10 * 1000);
                }
            }).Start();
        }

        public string EncryptText(string input, string password)
        {
            var hasher = new SHA1CryptoServiceProvider();
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(input, password));
            byte[] hashedBytes = hasher.ComputeHash(textWithSaltBytes);
            hasher.Clear();
            return Convert.ToBase64String(hashedBytes);
        }

        public async void OnReceiveInvite(string from, string to, string inviteId)
        {
            if (to.ToLower() == sumName.ToLower())
            {
                if (from.ToLower() == Program.leaderName.ToLower())
                {
                    Tools.ConsoleMessage("Accepting lobby invite", ConsoleColor.Cyan);
                    await Task.Delay(new Random().Next(1, 3) * new Random().Next(800, 1200));
                    await this.connection.AcceptLobbyInvite(inviteId);
                }
                else
                {
                    await Task.Delay(new Random().Next(1, 3) * new Random().Next(800, 1200));
                    await this.connection.DeclineLobbyInvite(inviteId);
                }
            }
        }

        public bool IsCurrentlyInMatch()
        {
            try
            {
                WebClient webClient = new WebClient();
                var json = webClient.DownloadString(string.Format("https://{0}.api.pvp.net/observer-mode/rest/consumer/getSpectatorGameInfo/{0}1/{1}?api_key=RGAPI-4840c81c-2c7f-47bc-a370-11f73e20cf19", this.region, this.sumId));
                return !json.Contains("\"status_code\": 404");
            }
            catch (Exception ex)
            {
                Tools.Log(ex.StackTrace);
            }
            return false;
        }

        public async void connection_OnMessageReceived(object sender, object messageReceivedEventArgs)
        {
            try
            {
                object message = !(messageReceivedEventArgs is MessageReceivedEventArgs) ? messageReceivedEventArgs : ((MessageReceivedEventArgs)messageReceivedEventArgs).Message.Body;
                if (message is PlatformGameLifecycleDTO)
                {
                    PlatformGameLifecycleDTO PGLC = message as PlatformGameLifecycleDTO;
                    Tools.ConsoleMessage(PGLC.Game.GameState, ConsoleColor.Gray);
                    Tools.ConsoleMessage(PGLC.Game.GameStateString, ConsoleColor.Gray);
                    PGLC = null;
                }

                #region LobbyStatus

                if (message is LobbyStatus && !IsInQueue)
                {
                    Lobby = message as LobbyStatus;
                    if (Lobby.Members.Count == GetFriendsToInvite().Count + 1)
                    {
                        Tools.ConsoleMessage("All players accepted, starting queue.", ConsoleColor.Cyan);
                        this.EnterQueue();
                    }
                    else
                    {
                        Tools.ConsoleMessage(string.Format("{0}/{1} player(s) accepted, waiting till everybody accepted.", Lobby.Members.Count - 1, GetFriendsToInvite().Count), ConsoleColor.Cyan);
                    }
                }

                #endregion LobbyStatus

                #region GameDTO

                if (message is GameDTO)
                {
                    GameDTO gameDTO = message as GameDTO;
                    string gameState;
                    if ((gameState = gameDTO.GameState) != null)
                    {
                        //Tools.ConsoleMessage("Game state=" + gameState, ConsoleColor.Cyan);
                        switch (gameState)
                        {
                            case "IDLE":
                            break;

                            case "TEAM_SELECT":
                            if (firstTimeInCustom)
                            {
                                Tools.ConsoleMessage("Entering champion selection", ConsoleColor.White);
                                await connection.StartChampionSelection(gameDTO.Id, gameDTO.OptimisticLock);
                                firstTimeInCustom = false;
                            }
                            break;

                            case "CHAMP_SELECT":
                            {
                                this.firstTimeInCustom = true;
                                this.firstTimeInQueuePop = true;
                                if (this.firstTimeInLobby)
                                {
                                    if (this.queueType != "ARAM")
                                    {
                                        if (pickAtTurn == 0)
                                        {
                                            Tools.ConsoleMessage("You are in champion select.", ConsoleColor.White);

                                            try
                                            {
                                                await connection.SetClientReceivedGameMessage(gameDTO.Id, "CHAMP_SELECT_CLIENT");
                                            }
                                            catch (InvocationException ex)
                                            {
                                                Tools.Log(ex.StackTrace);
                                                Tools.ConsoleMessage("Fault Code: " + ex.FaultCode + " Fault String" + ex.FaultString + " Fault Detail:" + ex.FaultDetail + "Root Cause:" + ex.RootCause, ConsoleColor.White);
                                            }
                                            if (!m_isLeader || !Program.queueWithFriends)
                                            {
                                                pickAtTurn = new Random().Next(1, 3);
                                                turn = 0;
                                                return;
                                            }
                                        }
                                        else if (turn < pickAtTurn)
                                        {
                                            turn++;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Tools.ConsoleMessage("You are in champion select.", ConsoleColor.White);
                                        try
                                        {
                                            await connection.SetClientReceivedGameMessage(gameDTO.Id, "CHAMP_SELECT_CLIENT");
                                        }
                                        catch (InvocationException ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                            Tools.ConsoleMessage("Fault Code: " + ex.FaultCode + " Fault String" + ex.FaultString + " Fault Detail:" + ex.FaultDetail + "Root Cause:" + ex.RootCause, ConsoleColor.White);
                                        }
                                    }
                                    this.firstTimeInLobby = false;

                                    #region Not Aram

                                    if (this.queueType != "ARAM" && this.queueType != "ARAM_UNRANKED_1x1" && this.queueType != "ARAM_UNRANKED_2x2" && (this.queueType != "ARAM_UNRANKED_3x3" && this.queueType != "ARAM_UNRANKED_5x5") && this.queueType != "ARAM_UNRANKED_6x6")
                                    {
                                        List<ChampionDTO> ChampList = new List<ChampionDTO>(this.AvailableChampions);

                                        if (this.loginPacket.ClientSystemStates.freeToPlayChampionsForNewPlayersMaxLevel >= this.loginPacket.AllSummonerData.SummonerLevel.Level)
                                        {
                                            foreach (ChampionDTO championDto in ChampList)
                                            {
                                                var freeToPlay = ((IEnumerable<int>)this.loginPacket.ClientSystemStates.freeToPlayChampionForNewPlayersIdList).Contains(championDto.ChampionId);
                                                if (freeToPlay)
                                                {
                                                    championDto.FreeToPlay = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (ChampionDTO championDto in ChampList)
                                            {
                                                var freeToPlay = ((IEnumerable<int>)this.loginPacket.ClientSystemStates.freeToPlayChampionIdList).Contains(championDto.ChampionId);
                                                if (freeToPlay)
                                                {
                                                    championDto.FreeToPlay = true;
                                                }
                                            }
                                        }
                                        int selectedChampionId = 22;
                                        string championName = "Ashe";
                                        string[] strArray = new string[5]
                                        {
                                            Program.firstChampionPick,
                                            Program.secondChampionPick,
                                            Program.thirdChampionPick,
                                            Program.fourthChampionPick,
                                            Program.fifthChampionPick
                                        };
                                        if (Program.randomChampionPick)
                                        {
                                            List<string> champList = new List<string>();
                                            champList.AddRange(strArray);
                                            bool found = false;
                                            while (!found && champList.Count != 0)
                                            {
                                                var index = new Random().Next(0, champList.Count - 1);
                                                var championString = champList[index];
                                                champList.RemoveAt(index);

                                                int championId = Enums.GetChampion(championString);
                                                ChampionDTO champDto = ChampList.FirstOrDefault(c => c.ChampionId == championId);
                                                //TEMP FIX use Active instead of FreeToPlay...
                                                if (champDto != null && (champDto.Owned || champDto.FreeToPlay && !gameDTO.PlayerChampionSelections.Any(c => c.ChampionId == championId)))
                                                {
                                                    selectedChampionId = championId;
                                                    championName = UcFirst(Enums.GetChampionById(selectedChampionId));
                                                    if (string.IsNullOrEmpty(championName)) championName = UcFirst(championString);
                                                    found = true;
                                                    break;
                                                }
                                                champDto = null;
                                                championString = null;
                                            }
                                        }
                                        else
                                        {
                                            for (int index = 0; index < strArray.Length; ++index)
                                            {
                                                string championString = strArray[index];
                                                int championId = Enums.GetChampion(championString);
                                                ChampionDTO champDto = ChampList.FirstOrDefault(c => c.ChampionId == championId);
                                                if (champDto != null && (champDto.Owned || champDto.FreeToPlay && !gameDTO.PlayerChampionSelections.Any(c => c.ChampionId == championId)))
                                                {
                                                    selectedChampionId = championId;
                                                    championName = UcFirst(Enums.GetChampionById(selectedChampionId));
                                                    if (string.IsNullOrEmpty(championName)) championName = UcFirst(championString);
                                                    break;
                                                }
                                                champDto = null;
                                                championString = null;
                                            }
                                        }

                                        strArray = null;
                                        try
                                        {
                                            await connection.SelectChampion(selectedChampionId);
                                        }
                                        catch (InvocationException ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                            Tools.ConsoleMessage(string.Format("Champion '{0}' is not owned, is not free to play or has already been choosen.", championName), ConsoleColor.Red);
                                            Console.WriteLine(ex.StackTrace);
                                        }
                                        Tools.ConsoleMessage("Selected Champion: " + championName, ConsoleColor.DarkYellow);
                                        await Task.Delay(new Random().Next(1, 9) * new Random().Next(800, 1000));
                                        await connection.ChampionSelectCompleted();
                                        Tools.ConsoleMessage("Waiting for other players to lockin.", ConsoleColor.White);

                                        //Select Summoners Spell
                                        int spellOneId;
                                        int spellTwoId;
                                        if (!Program.randomSpell)
                                        {
                                            spellOneId = Enums.GetSpell(Program.spell1);
                                            spellTwoId = Enums.GetSpell(Program.spell2);
                                        }
                                        else
                                        {
                                            Random random = new Random();
                                            List<int> list = new List<int>()
                                        {
                                            13,
                                            6,
                                            7,
                                            10,
                                            1,
                                            11,
                                            21,
                                            12,
                                            3,
                                            14,
                                            2,
                                            4
                                        };
                                            int index = random.Next(list.Count);
                                            int index2 = random.Next(list.Count);
                                            int num = list[index];
                                            int num2 = list[index2];
                                            if (num == num2)
                                            {
                                                int index3 = random.Next(list.Count);
                                                num2 = list[index3];
                                            }
                                            spellOneId = Convert.ToInt32(num);
                                            spellTwoId = Convert.ToInt32(num2);
                                            random = null;
                                            list = null;
                                        }
                                        await Task.Delay(new Random().Next(1, 9) * new Random().Next(800, 1000));
                                        try
                                        {
                                            await connection.SelectSpells(spellOneId, spellTwoId);
                                        }
                                        catch (Exception e)
                                        {
                                            Tools.Log(e.StackTrace);
                                        }
                                        ChampList = null;
                                        championName = null;
                                    }

                                    #endregion Not Aram

                                    #region ARAM

                                    if (this.queueType == "ARAM" || this.queueType == "ARAM_UNRANKED_1x1" || this.queueType == "ARAM_UNRANKED_2x2" || (this.queueType == "ARAM_UNRANKED_3x3" || this.queueType == "ARAM_UNRANKED_5x5") || this.queueType == "ARAM_UNRANKED_6x6")
                                    {
                                        var champion = gameDTO.PlayerChampionSelections.FirstOrDefault(x => x.SummonerInternalName.ToLower() == sumName.ToLower().Replace(" ", ""));
                                        if (champion != null)
                                        {
                                            var championName = Enums.GetChampionById(champion.ChampionId);
                                            if (!string.IsNullOrEmpty(championName))
                                            {
                                                Tools.ConsoleMessage("Selected Champion: " + UcFirst(championName.ToLower()), ConsoleColor.DarkYellow);
                                            }
                                        }

                                        int Spell1;
                                        int Spell2;
                                        if (!Program.randomSpell)
                                        {
                                            Spell1 = Enums.GetSpell(Program.spell1);
                                            Spell2 = Enums.GetSpell(Program.spell2);
                                        }
                                        else
                                        {
                                            var random = new Random();
                                            var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                                            int index = random.Next(spellList.Count);
                                            int index2 = random.Next(spellList.Count);

                                            int randomSpell1 = spellList[index];
                                            int randomSpell2 = spellList[index2];

                                            if (randomSpell1 == randomSpell2)
                                            {
                                                int index3 = random.Next(spellList.Count);
                                                randomSpell2 = spellList[index3];
                                            }

                                            Spell1 = Convert.ToInt32(randomSpell1);
                                            Spell2 = Convert.ToInt32(randomSpell2);
                                        }

                                        try
                                        {
                                            await Task.Delay(new Random().Next(1, 9) * new Random().Next(800, 1000));
                                            await connection.SelectSpells(Spell1, Spell2);
                                            await connection.ChampionSelectCompleted();
                                            await Task.Delay(new Random().Next(1, 9) * new Random().Next(800, 1000));
                                        }
                                        catch (Exception ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                        }
                                        Tools.ConsoleMessage("Waiting for other players to lockin.", ConsoleColor.White);
                                    }

                                    #endregion ARAM
                                }
                            }
                            break;

                            case "POST_CHAMP_SELECT":
                            {
                                firstTimeInLobby = false;
                                if (firstTimeInPostChampSelect)
                                {
                                    firstTimeInPostChampSelect = false;
                                    Tools.ConsoleMessage("Waiting champ select timer to reach 0", ConsoleColor.White);
                                }
                            }
                            break;

                            case "PRE_CHAMP_SELECT":
                            break;

                            case "START_REQUESTED":
                            {
                                ShouldBeInGame = true;
                            }
                            break;

                            case "GAME_START_CLIENT":
                            break;

                            case "GameClientConnectedToServer":
                            break;

                            case "IN_PROGRESS":
                            break;

                            case "IN_QUEUE":
                            {
                                Tools.ConsoleMessage("You are in queue.", ConsoleColor.White);
                            }
                            break;

                            case "POST_GAME":
                            break;

                            case "TERMINATED":
                            {
                                pickAtTurn = 0;
                                Tools.ConsoleMessage("Re-queued: " + this.queueType + " as " + this.sumName + ".", ConsoleColor.Cyan);
                                firstTimeInQueuePop = true;
                                firstTimeInPostChampSelect = true;
                                //ShouldBeInGame = false;
                                if (!Program.queueWithFriends)
                                {
                                    if (!IsInQueue)
                                    {
                                        new Thread(async () =>
                                        {
                                            await Task.Delay(1000 * 3);
                                            if (!IsInQueue) this.EnterQueue();
                                        }).Start();
                                    }
                                }
                                else
                                {
                                    if (m_isLeader)
                                    {
                                        sendGameInvites();
                                    }
                                }
                            }
                            break;

                            case "TERMINATED_IN_ERROR":
                            break;

                            case "CHAMP_SELECT_CLIENT":
                            //ShouldBeInGame = true;
                            break;

                            case "GameReconnect":
                            break;

                            case "GAME_IN_PROGRESS":
                            break;

                            case "JOINING_CHAMP_SELECT":
                            {
                                if (firstTimeInQueuePop)
                                {
                                    if (gameDTO.StatusOfParticipants.Contains("1"))
                                    {
                                        Tools.ConsoleMessage("Queue popped.", ConsoleColor.White);
                                        firstTimeInQueuePop = false;
                                        firstTimeInLobby = true;
                                        await Task.Delay(new Random().Next(1, Program.queueWithFriends ? (GetFriendsToInvite().ToList().Count > 3 ? 2 : 4) : 8) * new Random().Next(800, 1000));
                                        Tools.ConsoleMessage("Accepted Queue!", ConsoleColor.White);
                                        try
                                        {
                                            await connection.AcceptPoppedGame(true);
                                            ShouldBeInGame = false;
                                        }
                                        catch (InvocationException ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                            Tools.ConsoleMessage("Fault Code: " + ex.FaultCode + " Fault String" + ex.FaultString + " Fault Detail:" + ex.FaultDetail + "Root Cause:" + ex.RootCause, ConsoleColor.White);
                                        }
                                        catch (InvalidCastException ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                            Tools.ConsoleMessage(ex.StackTrace, ConsoleColor.Red);
                                        }
                                        catch (Exception ex)
                                        {
                                            Tools.Log(ex.StackTrace);
                                            Tools.ConsoleMessage(ex.StackTrace, ConsoleColor.Red);
                                        }
                                    }
                                }
                            }
                            break;

                            case "WAITING":
                            break;

                            case "DISCONNECTED":
                            break;

                            case "LEAVER_BUSTED":
                            {
                                Tools.ConsoleMessage("You have leave buster.", ConsoleColor.White);
                            }
                            break;

                            default:
                            break;
                        }
                    }
                    gameDTO = null;
                    gameState = null;
                }

                #endregion GameDTO

                #region PlayerCredentials

                else if (message is PlayerCredentialsDto)
                {
                    try
                    {
                        if (GameStartedAt == null) GameStartedAt = DateTime.Now;
                        this.firstTimeInPostChampSelect = true;
                        PlayerCredentialsDto playerCredentialsDto = message as PlayerCredentialsDto;
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = false;
                        startInfo.WorkingDirectory = this.FindLoLExe();
                        //Console.WriteLine(FindLoLExe() + "League of Legends.exe");
                        startInfo.FileName = "League of Legends.exe";
                        startInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"" + playerCredentialsDto.ServerIp + " " + (object)playerCredentialsDto.ServerPort + " " + playerCredentialsDto.EncryptionKey + " " + (object)this.loginPacket.AllSummonerData.Summoner.SummonerId + "\"";
                        new Thread(() =>
                        {
                            try
                            {
                                this.exeProcess = Process.Start(startInfo);
                                this.exeProcess.Exited += exeProcess_Exited;
                                do
                                {
                                }
                                while (this.exeProcess.MainWindowHandle == IntPtr.Zero);
                                this.exeProcess.PriorityClass = !Program.LOWPriority ? ProcessPriorityClass.High : ProcessPriorityClass.Idle;
                                this.exeProcess.EnableRaisingEvents = true;
                            }
                            catch (InvocationException ex)
                            {
                                Tools.Log(ex.StackTrace);
                            }
                        }).Start();
                        Tools.ConsoleMessage("Launching League of Legends.", ConsoleColor.White);
                        playerCredentialsDto = null;
                        ShouldBeInGame = true;
                        IsInQueue = false;
                    }
                    catch (Exception ex)
                    {
                        Tools.Log(ex.StackTrace);
                        Console.WriteLine(ex.StackTrace);
                        ShouldBeInGame = false;
                        //
                    }
                }

                #endregion PlayerCredentials

                else
                {
                    if (message is GameNotification || message is SearchingForMatchNotification)
                    {
                        return;
                    }
                    if (message is EndOfGameStats)
                    {
                        var match = message as EndOfGameStats;
                        if (match.TeamPlayerParticipantStats != null && match.TeamPlayerParticipantStats.Count > 0)
                        {
                            var game = match.TeamPlayerParticipantStats.FirstOrDefault(x => x.SummonerName.ToLower() == sumName.ToLower().Replace(" ", ""));

                            if (game != null)
                            {
                                if (Program.printGameStats) foreach (var stat1 in game.Statistics) Tools.ConsoleMessage(stat1.StatTypeName + " = " + stat1.Value.ToString(), ConsoleColor.White);
                                var statWin = game.Statistics.FirstOrDefault(x => x.StatTypeName == "WIN");
                                bool win = statWin != null && statWin.Value == 1;

                                Program.AddGame(win);

                                var kills = 0;
                                var deaths = 0;
                                var assists = 0;

                                var k = game.Statistics.FirstOrDefault(x => x.StatTypeName == "CHAMPIONS_KILLED");
                                if (k != null) kills = (int)k.Value;

                                var d = game.Statistics.FirstOrDefault(x => x.StatTypeName == "NUM_DEATHS");
                                if (d != null) deaths = (int)d.Value;

                                var a = game.Statistics.FirstOrDefault(x => x.StatTypeName == "ASSISTS");
                                if (a != null) assists = (int)a.Value;

                                Tools.ConsoleMessage(string.Format("{4} - {0} - {1}/{2}/{3}", win ? "Victory" : "Defeat", kills, deaths, assists, sumName), ConsoleColor.Magenta);
                            }
                        }
                        ShouldBeInGame = false;
                        GameStartedAt = null;
                        if (exeProcess == null)
                        {
                            if (!Program.queueWithFriends)
                            {
                                if (!IsInQueue) this.EnterQueue();
                            }
                            else
                            {
                                if (m_isLeader)
                                {
                                    sendGameInvites();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (message is AsObject)
                        {
                            var temp = (AsObject)message;
                            //Console.WriteLine(temp.TypeName);
                            if (temp.TypeName.ToLower().Contains("eog") || (queueType == "INTRO_BOT" || queueType == "BEGINNER_BOT" || queueType == "MEDIUM_BOT" || queueType == "BOT_3X3") && temp.TypeName.ToLower().Contains("storeaccountbalancenotification"))
                            {
                                lock (locker)
                                {
                                    if (exeProcess == null) return;
                                    new Thread(async () =>
                                    {
                                        await CloseGame();
                                    }).Start();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Tools.Log(exc.StackTrace);
                //Tools.ConsoleMessage(exc.StackTrace, ConsoleColor.Red);
            }
        }

        private static string UcFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        private object locker = new object();

        private async Task CloseGame()
        {
            try
            {
                GameStartedAt = null;
                ShouldBeInGame = false;
                if (exeProcess == null) return;
                Tools.ConsoleMessage("Closing game client.", ConsoleColor.White);
                EndOfGameStats eog = new EndOfGameStats();
                this.connection_OnMessageReceived(this, eog);
                this.exeProcess.Exited -= new EventHandler(exeProcess_Exited);
                this.exeProcess.Kill();
                await Task.Delay(1000);
                if (this.exeProcess.Responding)
                    Process.Start("taskkill /F /PID " + exeProcess.Id); //Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                this.exeProcess = null;
                this.loginPacket = await this.connection.GetLoginDataPacketForUser();
                this.archiveSumLevel = this.sumLevel;
                this.sumLevel = this.loginPacket.AllSummonerData.SummonerLevel.Level;

                if (this.sumLevel == this.archiveSumLevel)
                    return;
                this.levelUp();
            }
            catch (Exception ex)
            {
                Tools.Log(ex.Message.ToString());
            }
            if (!Program.queueWithFriends)
            {
                if (!IsInQueue)
                {
                    new Thread(async () =>
                    {
                        await Task.Delay(1000 * 3);
                        if (!IsInQueue) this.EnterQueue();
                    }).Start();
                }
            }
            else
            {
                if (m_isLeader)
                {
                    sendGameInvites();
                }
            }
        }

        private async void EnterQueue()
        {
            if (IsInQueue)
                return;
            IsInQueue = true;

            ShouldBeInGame = false;
            try
            {
                MatchMakerParams matchMakerParams = new MatchMakerParams();
                if (this.queueType == "INTRO_BOT")
                    matchMakerParams.BotDifficulty = "INTRO";
                else if (this.queueType == "BEGINNER_BOT")
                    matchMakerParams.BotDifficulty = "EASY";
                else if (this.queueType == "MEDIUM_BOT")
                    matchMakerParams.BotDifficulty = "MEDIUM";
                else if (this.queueType.ToUpper() == "BOT_3X3")
                {
                    matchMakerParams.BotDifficulty = "MEDIUM";
                }
                if (this.sumLevel == 3.0 && this.actualQueueType == "NORMAL_5X5")
                    this.queueType = this.actualQueueType;
                else if (this.sumLevel == 6.0 && this.actualQueueType == "ARAM")
                    this.queueType = this.actualQueueType;
                else if (this.sumLevel == 7.0 && this.actualQueueType == "NORMAL_3X3")
                    this.queueType = this.actualQueueType;
                else if (this.sumLevel == 10.0 && this.actualQueueType == "TT_HEXAKILL")
                    this.queueType = this.actualQueueType;

                var queues = await connection.GetAvailableQueues();

                matchMakerParams.QueueIds = new int[1] { (int)Enum.Parse(typeof(QueueTypeId), this.queueType) };
                if (Lobby != null && Program.queueWithFriends)
                {
                    matchMakerParams.InvitationId = Lobby.InvitationID;
                    matchMakerParams.Team = Lobby.Members.Select(stats => Convert.ToInt32(stats.SummonerId)).ToList();
                }

                SearchingForMatchNotification searchingForMatchNotification = null;
                try
                {
                    if (Program.queueWithFriends)
                        searchingForMatchNotification = await connection.AttachTeamToQueue(matchMakerParams);
                    else
                        searchingForMatchNotification = await connection.AttachToQueue(matchMakerParams);
                }
                catch (RtmpSharp.Net.ClientDisconnectedException ex)
                {
                    Tools.Log(ex.StackTrace);
                    Console.WriteLine("Got an exception where we should not!");
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Console.WriteLine(ex.StackTrace);
                }

                if (searchingForMatchNotification == null)
                {
                    return;
                }

                if (searchingForMatchNotification.PlayerJoinFailures == null)
                {
                    if (searchingForMatchNotification.JoinedQueues.Count == 0)
                    {
                        IsInQueue = false;
                        //Tools.ConsoleMessage("Cannot queue for: " + queueType.ToString() + " at the moment.", ConsoleColor.Red);
                    }
                    else
                    {
                        Tools.ConsoleMessage("In Queue: " + queueType.ToString() + " as " + loginPacket.AllSummonerData.Summoner.Name + ".", ConsoleColor.Cyan);
                        IsInQueue = true;
                    }
                }
                else
                {
                    foreach (FailedJoinPlayer playerJoinFailure in searchingForMatchNotification.PlayerJoinFailures)
                    {
                        IsInQueue = false;
                        FailedJoinPlayer failedJoinPlayer = playerJoinFailure;
                        Tools.ConsoleMessage("Queue failed, reason: " + failedJoinPlayer.ReasonFailed + ".", ConsoleColor.Red);
                        if (failedJoinPlayer is BustedLeaver)
                        {
                            BustedLeaver current = failedJoinPlayer as BustedLeaver;
                            if (current.ReasonFailed == "LEAVER_BUSTED")
                            {
                                this.m_accessToken = current.AccessToken;
                                if (current.LeaverPenaltyMilisRemaining > this.m_leaverBustedPenalty)
                                    this.m_leaverBustedPenalty = current.LeaverPenaltyMilisRemaining;
                            }
                            else if (current.ReasonFailed == "LEAVER_BUSTER_TAINTED_WARNING")
                            {
                                try
                                {
                                    object obj1 = await this.connection.ackLeaverBusterWarning();
                                    object obj2 = await this.connection.callPersistenceMessaging(new SimpleDialogMessageResponse()
                                    {
                                        AccountId = this.loginPacket.AllSummonerData.Summoner.SummonerId,
                                        MessageId = this.loginPacket.AllSummonerData.Summoner.SummonerId.ToString(),
                                        Command = "ack"
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Tools.ConsoleMessage("Leaver Buster Tainted Warning error: " + ex.StackTrace, ConsoleColor.Red);
                                }
                                this.connection_OnMessageReceived(null, new EndOfGameStats());
                            }
                            current = null;
                            failedJoinPlayer = null;
                        }
                        else if (failedJoinPlayer is QueueDodger)
                        {
                            QueueDodger current = failedJoinPlayer as QueueDodger;
                            if (current.ReasonFailed == "QUEUE_DODGER")
                            {
                                Tools.ConsoleMessage("Waiting queue dodger timer: " + (float)(current.PenaltyRemainingTime / 1000.0 / 60.0) + " minutes!", ConsoleColor.White);
                                //Thread.Sleep((int)(current.PenaltyRemainingTime));
                                await Task.Delay(TimeSpan.FromMilliseconds(current.PenaltyRemainingTime));
                                //Thread.Sleep(TimeSpan.FromMilliseconds(current.PenaltyRemainingTime));
                                this.connection_OnMessageReceived(null, new EndOfGameStats());
                            }
                            current = null;
                            failedJoinPlayer = null;
                        }
                        else if (failedJoinPlayer.ReasonFailed == "LEAVER_BUSTER_TAINTED_WARNING")
                        {
                            try
                            {
                                object obj1 = await this.connection.ackLeaverBusterWarning();
                                object obj2 = await this.connection.callPersistenceMessaging(new SimpleDialogMessageResponse()
                                {
                                    AccountId = this.loginPacket.AllSummonerData.Summoner.SummonerId,
                                    MessageId = this.loginPacket.AllSummonerData.Summoner.SummonerId.ToString(),
                                    Command = "ack"
                                });
                            }
                            catch (Exception ex)
                            {
                                Tools.ConsoleMessage("Leaver Buster Tainted Warning error: " + ex.StackTrace, ConsoleColor.Red);
                            }
                            this.connection_OnMessageReceived(null, new EndOfGameStats());
                        }
                    }
                    //List<FailedJoinPlayer>.Enumerator enumerator = new List<FailedJoinPlayer>.Enumerator();
                    if (!string.IsNullOrEmpty(this.m_accessToken))
                    {
                        Tools.ConsoleMessage("Waiting leaver buster timer: " + (float)(this.m_leaverBustedPenalty / 1000.0 / 60.0) + " minutes!", ConsoleColor.White);
                        //Thread.Sleep(TimeSpan.FromMilliseconds(this.m_leaverBustedPenalty));
                        await Task.Delay(TimeSpan.FromMilliseconds(this.m_leaverBustedPenalty));
                        Dictionary<string, object> lbdic = new Dictionary<string, object>();
                        lbdic.Add("LEAVER_BUSTER_ACCESS_TOKEN", this.m_accessToken);

                        searchingForMatchNotification = await this.connection.AttachToQueue(matchMakerParams, new AsObject(lbdic));

                        if (searchingForMatchNotification.PlayerJoinFailures == null)
                        {
                            Tools.ConsoleMessage("Joined lower priority queue! as " + this.loginPacket.AllSummonerData.Summoner.Name + ".", ConsoleColor.Cyan);
                            IsInQueue = true;
                        }
                        else
                        {
                            Tools.ConsoleMessage("There was an error in joining lower priority queue.\nDisconnecting.", ConsoleColor.White);
                            this.connection.Disconnect();
                            Thread.Sleep(500);
                            connection.ConnectAndLogin().Wait();
                        }
                        lbdic = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Log(ex.StackTrace);
                Tools.ConsoleMessage("Error occured: " + ex.StackTrace, ConsoleColor.Red);
                connection.ConnectAndLogin().Wait();
            }
        }

        private string FindLoLExe()
        {
            if (ipath.Contains("notfound"))
                return ipath;
            if(useGarena)
            {
                return ipath + "GAME\\";
            }
            else
            return Directory.EnumerateDirectories(ipath + "RADS\\solutions\\lol_game_client_sln\\releases\\").OrderBy(f => new DirectoryInfo(f).CreationTime).Last() + "\\deploy\\";
        }

        private async void RestartLeague()
        {
            var found = false;
            while (!found)
            {
                if (GameStartedAt == null)// || exeProcess != null)
                {
                    found = true;
                    break;
                }
                var ellapsedTime = DateTime.Now.Subtract(GameStartedAt.Value).Minutes;
                if (ellapsedTime >= 0)
                {
                    try
                    {
                        this.loginPacket = await connection.GetLoginDataPacketForUser();
                        if (this.loginPacket.ReconnectInfo != null || ((PlatformGameLifecycleDTO)loginPacket.ReconnectInfo).Game != null)
                        {
                            Tools.ConsoleMessage("Restarting League of Legends.", ConsoleColor.White);
                            connection_OnMessageReceived(this, ((PlatformGameLifecycleDTO)loginPacket.ReconnectInfo).PlayerCredentials);
                        }
                        else
                        {
                            connection_OnMessageReceived(this, new EndOfGameStats());
                            ShouldBeInGame = false;
                        }
                        found = true;
                        break;
                    }
                    catch (InvocationException ex)
                    {
                        Tools.Log(ex.StackTrace);
                        //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                        //connection.ConnectAndLogin().Wait();
                    }
                    catch (NullReferenceException ex)
                    {
                        Tools.Log(ex.StackTrace);
                        //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                        if (ShouldBeInGame)
                            connection.ConnectAndLogin().Wait();
                    }
                    catch (RtmpSharp.Net.ClientDisconnectedException ex)
                    {
                        Tools.Log(ex.StackTrace);
                        //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                        connection.ConnectAndLogin().Wait();
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private async void exeProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                this.exeProcess = null;
                this.loginPacket = await connection.GetLoginDataPacketForUser();
                if (this.loginPacket.ReconnectInfo != null || ((PlatformGameLifecycleDTO)loginPacket.ReconnectInfo).Game != null)
                {
                    var ellapsedTime = DateTime.Now.Subtract(GameStartedAt.Value).Minutes;

                    if (ellapsedTime >= 0)
                    {
                        Tools.ConsoleMessage("Restarting League of Legends.", ConsoleColor.White);
                        connection_OnMessageReceived(sender, ((PlatformGameLifecycleDTO)loginPacket.ReconnectInfo).PlayerCredentials);
                    }
                    else
                    {
                        Tools.ConsoleMessage("Restarting League of Legends at " + GameStartedAt.Value.AddMinutes(1).ToLongTimeString() + " please wait.", ConsoleColor.White);
                        new Thread(RestartLeague).Start();
                    }
                }
                else
                {
                    ShouldBeInGame = false;
                    connection_OnMessageReceived(sender, new EndOfGameStats());
                }
            }
            catch (InvocationException ex)
            {
                Tools.Log(ex.StackTrace);
                //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                //connection.ConnectAndLogin().Wait();
            }
            catch (NullReferenceException ex)
            {
                Tools.Log(ex.StackTrace);
                //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                if (ShouldBeInGame)
                    connection.ConnectAndLogin().Wait();
            }
            catch (RtmpSharp.Net.ClientDisconnectedException ex)
            {
                Tools.Log(ex.StackTrace);
                //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                connection.ConnectAndLogin().Wait();
            }
        }

        private void connection_OnLoginQueueUpdate(int positionInLine)
        {
            if (positionInLine <= 0)
                return;
            Tools.ConsoleMessage("Position to login: " + (object)positionInLine, ConsoleColor.White);
        }

        private async void connection_OnLogin(string username)
        {
            try
            {
                await this.InitializeLogin();
            }
            catch (RtmpSharp.Net.ClientDisconnectedException ex)
            {
                Tools.Log(ex.StackTrace);
                //Console.WriteLine("Got ClientDisconnected exception after the game has quit.");
                connection.ConnectAndLogin().Wait();
            }
        }

        private async Task InitializeLogin()
        {
            Tools.ConsoleMessage("Loging into account...", ConsoleColor.White);
            this.loginPacket = await this.connection.GetLoginDataPacketForUser();
            AvailableChampions = await this.connection.GetAvailableChampions();

            if (this.loginPacket.AllSummonerData == null)
            {
                Tools.ConsoleMessage("Summoner not found in account.", ConsoleColor.Red);
                Tools.ConsoleMessage("Creating Summoner...", ConsoleColor.Red);
                Random random = new Random();
                string text = this.Accountname;
                if (text.Length > 16)
                    text = text.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                AllSummonerData allSummonerData1 = await this.connection.CreateDefaultSummoner(text);
                this.loginPacket.AllSummonerData = allSummonerData1;
                Tools.ConsoleMessage("Created Summoner: " + text, ConsoleColor.White);
                text = null;
                allSummonerData1 = null;
            }
            this.sumLevel = (double)this.loginPacket.AllSummonerData.SummonerLevel.Level;
            sumName = this.loginPacket.AllSummonerData.Summoner.Name;
            sumId = this.loginPacket.AllSummonerData.Summoner.SummonerId;

            double summonerId = this.loginPacket.AllSummonerData.Summoner.SummonerId;
            this.rpBalance = this.loginPacket.RpBalance;
            this.ipBalance = this.loginPacket.IpBalance;
            if (this.sumLevel >= (double)Program.maxLevel)
            {
                Tools.ConsoleMessage("Summoner: " + sumName + " is already max level.", ConsoleColor.White);
                this.connection.Disconnect();
                Tools.ConsoleMessage("Log into new account.", ConsoleColor.White);
                Program.LognNewAccount();
            }
            else
            {
                if (this.rpBalance == 400.0 && Program.buyExpBoost)
                {
                    Tools.ConsoleMessage("Buying XP Boost", ConsoleColor.White);
                    try
                    {
                        Task task = new Task(new Action(this.buyBoost));
                        task.Start();
                        task = null;
                    }
                    catch (Exception ex)
                    {
                        Tools.Log(ex.StackTrace);
                        Tools.ConsoleMessage("Couldn't buy RP Boost.\n" + ex.Message.ToString(), ConsoleColor.White);
                    }
                }
                var queueLevel = await GetQueueLevel();
                if (queueLevel < 3.0 && this.queueType == "NORMAL_5X5")
                {
                    Tools.ConsoleMessage("Need to be Level 3 before NORMAL_5X5 queue.", ConsoleColor.White);
                    Tools.ConsoleMessage("Joins Co-Op vs AI (Beginner) queue until 3", ConsoleColor.White);
                    this.queueType = "BEGINNER_BOT";
                    this.actualQueueType = "NORMAL_5X5";
                }
                else if (queueLevel < 6.0 && this.queueType == "ARAM")
                {
                    Tools.ConsoleMessage("Need to be Level 6 before ARAM queue.", ConsoleColor.White);
                    Tools.ConsoleMessage("Joins Co-Op vs AI (Beginner) queue until 6", ConsoleColor.White);
                    this.queueType = "BEGINNER_BOT";
                    this.actualQueueType = "ARAM";
                }
                else if (queueLevel < 7.0 && this.queueType == "NORMAL_3X3")
                {
                    Tools.ConsoleMessage("Need to be Level 7 before NORMAL_3X3 queue.", ConsoleColor.White);
                    Tools.ConsoleMessage("Joins Co-Op vs AI (Beginner) queue until 7", ConsoleColor.White);
                    this.queueType = "BEGINNER_BOT";
                    this.actualQueueType = "NORMAL_3X3";
                }

                Tools.ConsoleMessage("Welcome " + this.loginPacket.AllSummonerData.Summoner.Name + " - lvl (" + (object)this.loginPacket.AllSummonerData.SummonerLevel.Level + ") IP: (" + this.ipBalance.ToString() + ") - XP: (" + this.loginPacket.AllSummonerData.SummonerLevelAndPoints.ExpPoints + " / " + this.loginPacket.AllSummonerData.SummonerLevel.ExpToNextLevel + ")", ConsoleColor.White);

                PlayerDto player = await this.connection.CreatePlayer();
                if (this.loginPacket.ReconnectInfo != null && ((PlatformGameLifecycleDTO)this.loginPacket.ReconnectInfo).Game != null)
                    this.connection_OnMessageReceived((object)this, (object)((PlatformGameLifecycleDTO)this.loginPacket.ReconnectInfo).PlayerCredentials);
                else
                {
                    try
                    {
                        await this.connection.DestroyGroupFinderLobby();
                    }
                    catch (Exception ex)
                    {
                        Tools.Log(ex.StackTrace);
                    }
                    if (Program.queueWithFriends)
                    {
                        if (this.m_isLeader)
                        {
                            Tools.ConsoleMessage("Sending game invites.", ConsoleColor.Cyan);
                            sendGameInvites();
                        }
                        else
                        {
                            Tools.ConsoleMessage(string.Format("Waiting game invite from {0}.", Program.leaderName), ConsoleColor.Cyan);
                        }
                    }
                    else
                    {
                        this.connection_OnMessageReceived((object)this, (object)new EndOfGameStats());
                    }
                }
            }
        }

        private async void sendGameInvites()
        {
            try
            {
                if (queueType == "INTRO_BOT" || queueType == "BEGINNER_BOT")
                {
                    var lobby = await connection.CreateArrangedBotTeamLobby(GetGameModeId(), "EASY");
                }
                else if (queueType == "MEDIUM_BOT" || queueType == "BOT_3x3")
                {
                    var lobby = await connection.CreateArrangedBotTeamLobby(GetGameModeId(), "MEDIUM");
                }
                else
                {
                    var lobby = await connection.CreateArrangedTeamLobby(GetGameModeId());
                }
                //1
                try
                {
                    if (!string.IsNullOrEmpty(Program.firstFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.firstFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);
                        var invitation = await connection.InvitePlayer(summonerData.Summoner.SumId);
                        await Task.Delay(500);
                        Program.OnInvite?.Invoke(sumName, Program.firstFriend.ToLower(), Lobby.InvitationID);
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.firstFriend = null;
                }
                //2
                try
                {
                    if (!string.IsNullOrEmpty(Program.secondFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.secondFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);
                        InvitationRequest invitation = await connection.InvitePlayer(summonerData.Summoner.SumId) as InvitationRequest;
                        Program.OnInvite?.Invoke(sumName, Program.secondFriend.ToLower(), Lobby.InvitationID);
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.secondFriend = null;
                }
                //3
                try
                {
                    if (!string.IsNullOrEmpty(Program.thirdFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.thirdFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);
                        InvitationRequest invitation = await connection.InvitePlayer(summonerData.Summoner.SumId) as InvitationRequest;
                        Program.OnInvite?.Invoke(sumName, Program.thirdFriend.ToLower(), Lobby.InvitationID);
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.thirdFriend = null;
                }
                //4
                try
                {
                    if (!string.IsNullOrEmpty(Program.fourthFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.fourthFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);
                        InvitationRequest invitation = await connection.InvitePlayer(summonerData.Summoner.SumId) as InvitationRequest;
                        Program.OnInvite?.Invoke(sumName, Program.fourthFriend.ToLower(), Lobby.InvitationID);
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.fourthFriend = null;
                }
            }
            catch (InvocationException ex)
            {
                Tools.ConsoleMessage(ex.StackTrace, ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                Tools.ConsoleMessage(ex.StackTrace, ConsoleColor.Red);
            }
        }

        public async Task<double> GetQueueLevel()
        {
            double level = 30;
            if (Program.queueWithFriends)
            {
                //1
                try
                {
                    if (!string.IsNullOrEmpty(Program.firstFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.firstFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);

                        if (summonerData.SummonerLevel.Level < level)
                            level = summonerData.SummonerLevel.Level;
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.firstFriend = null;
                }
                //2
                try
                {
                    if (!string.IsNullOrEmpty(Program.secondFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.secondFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);

                        if (summonerData.SummonerLevel.Level < level)
                            level = summonerData.SummonerLevel.Level;
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.secondFriend = null;
                }
                //3
                try
                {
                    if (!string.IsNullOrEmpty(Program.thirdFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.thirdFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);

                        if (summonerData.SummonerLevel.Level < level)
                            level = summonerData.SummonerLevel.Level;
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.thirdFriend = null;
                }
                //4
                try
                {
                    if (!string.IsNullOrEmpty(Program.fourthFriend))
                    {
                        var summoner = await connection.GetSummonerByName(Program.fourthFriend);
                        var summonerData = await connection.GetAllPublicSummonerDataByAccount(summoner.AccountId);

                        if (summonerData.SummonerLevel.Level < level)
                            level = summonerData.SummonerLevel.Level;
                    }
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Program.fourthFriend = null;
                }
            }
            //Me
            if (sumLevel < level)
                level = sumLevel;

            return level;
        }

        public List<string> GetFriendsToInvite()
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(Program.firstFriend))
            {
                list.Add(Program.firstFriend);
            }
            if (!string.IsNullOrEmpty(Program.secondFriend))
            {
                list.Add(Program.secondFriend);
            }
            if (!string.IsNullOrEmpty(Program.thirdFriend))
            {
                list.Add(Program.thirdFriend);
            }
            if (!string.IsNullOrEmpty(Program.fourthFriend))
            {
                list.Add(Program.fourthFriend);
            }
            return list;
        }

        public int GetGameModeId()
        {
            if (this.sumLevel == 3.0 && this.actualQueueType == "NORMAL_5X5")
                this.queueType = this.actualQueueType;
            else if (this.sumLevel == 6.0 && this.actualQueueType == "ARAM")
                this.queueType = this.actualQueueType;
            else if (this.sumLevel == 7.0 && this.actualQueueType == "NORMAL_3X3")
                this.queueType = this.actualQueueType;
            else if (this.sumLevel == 10.0 && this.actualQueueType == "TT_HEXAKILL")
                this.queueType = this.actualQueueType;
            return (int)Enum.Parse(typeof(QueueTypeId), this.queueType);
        }

        private void connection_OnError(Error error)
        {
            if (error.Message.Contains("is not owned by summoner"))
                return;
            if (error.Message.Contains("Your summoner level is too low to select the spell"))
            {
                var random = new Random();
                var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                int index = random.Next(spellList.Count);
                int index2 = random.Next(spellList.Count);

                int randomSpell1 = spellList[index];
                int randomSpell2 = spellList[index2];

                if (randomSpell1 == randomSpell2)
                {
                    int index3 = random.Next(spellList.Count);
                    randomSpell2 = spellList[index3];
                }

                int Spell1 = Convert.ToInt32(randomSpell1);
                int Spell2 = Convert.ToInt32(randomSpell2);
                return;
            }
            else
                Tools.ConsoleMessage("error received:\n" + error.Message, ConsoleColor.White);
        }

        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            Console.Title = "ezBot - Offline";
            Tools.ConsoleMessage("Disconnected", ConsoleColor.White);
        }

        private void connection_OnConnect(object sender, EventArgs e)
        {
            Console.Title = "ezBot - Online";
        }

        private async void buyBoost()
        {
            try
            {
                if (this.region == "EUW")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text = str;
                    str = (string)null;
                    HttpClient httpClient = new HttpClient();
                    Console.WriteLine(text);
                    string stringAsync1 = await httpClient.GetStringAsync(text);
                    string requestUri = "https://store.euw1.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient.GetStringAsync(requestUri);
                    string requestUri2 = "https://store.euw1.lol.riotgames.com/store/purchase/item";
                    HttpContent content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUri2, content);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient.Dispose();
                    text = (string)null;
                    httpClient = (HttpClient)null;
                    requestUri = (string)null;
                    requestUri2 = (string)null;
                    content = (HttpContent)null;
                }
                else if (this.region == "EUNE")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text2 = str;
                    str = (string)null;
                    HttpClient httpClient2 = new HttpClient();
                    Console.WriteLine(text2);
                    string stringAsync1 = await httpClient2.GetStringAsync(text2);
                    string requestUri3 = "https://store.eun1.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient2.GetStringAsync(requestUri3);
                    string requestUri4 = "https://store.eun1.lol.riotgames.com/store/purchase/item";
                    HttpContent content2 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient2.PostAsync(requestUri4, content2);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient2.Dispose();
                    text2 = (string)null;
                    httpClient2 = (HttpClient)null;
                    requestUri3 = (string)null;
                    requestUri4 = (string)null;
                    content2 = (HttpContent)null;
                }
                else if (this.region == "NA")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text3 = str;
                    str = (string)null;
                    HttpClient httpClient3 = new HttpClient();
                    Console.WriteLine(text3);
                    string stringAsync1 = await httpClient3.GetStringAsync(text3);
                    string requestUri5 = "https://store.na2.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient3.GetStringAsync(requestUri5);
                    string requestUri6 = "https://store.na2.lol.riotgames.com/store/purchase/item";
                    HttpContent content3 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient3.PostAsync(requestUri6, content3);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient3.Dispose();
                    text3 = (string)null;
                    httpClient3 = (HttpClient)null;
                    requestUri5 = (string)null;
                    requestUri6 = (string)null;
                    content3 = (HttpContent)null;
                }
                else if (this.region == "KR")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text4 = str;
                    str = (string)null;
                    HttpClient httpClient4 = new HttpClient();
                    Console.WriteLine(text4);
                    string stringAsync1 = await httpClient4.GetStringAsync(text4);
                    string requestUri7 = "https://store.kr.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient4.GetStringAsync(requestUri7);
                    string requestUri8 = "https://store.kr.lol.riotgames.com/store/purchase/item";
                    HttpContent content4 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient4.PostAsync(requestUri8, content4);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient4.Dispose();
                    text4 = (string)null;
                    httpClient4 = (HttpClient)null;
                    requestUri7 = (string)null;
                    requestUri8 = (string)null;
                    content4 = (HttpContent)null;
                }
                else if (this.region == "BR")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text5 = str;
                    str = (string)null;
                    HttpClient httpClient5 = new HttpClient();
                    Console.WriteLine(text5);
                    string stringAsync1 = await httpClient5.GetStringAsync(text5);
                    string requestUri9 = "https://store.br.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient5.GetStringAsync(requestUri9);
                    string requestUri10 = "https://store.br.lol.riotgames.com/store/purchase/item";
                    HttpContent content5 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient5.PostAsync(requestUri10, content5);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient5.Dispose();
                    text5 = (string)null;
                    httpClient5 = (HttpClient)null;
                    requestUri9 = (string)null;
                    requestUri10 = (string)null;
                    content5 = (HttpContent)null;
                }
                else if (this.region == "RU")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text6 = str;
                    str = (string)null;
                    HttpClient httpClient6 = new HttpClient();
                    Console.WriteLine(text6);
                    string stringAsync1 = await httpClient6.GetStringAsync(text6);
                    string requestUri11 = "https://store.ru.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient6.GetStringAsync(requestUri11);
                    string requestUri12 = "https://store.ru.lol.riotgames.com/store/purchase/item";
                    HttpContent content6 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient6.PostAsync(requestUri12, content6);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient6.Dispose();
                    text6 = (string)null;
                    httpClient6 = (HttpClient)null;
                    requestUri11 = (string)null;
                    requestUri12 = (string)null;
                    content6 = (HttpContent)null;
                }
                else if (this.region == "TR")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text7 = str;
                    str = (string)null;
                    HttpClient httpClient7 = new HttpClient();
                    Console.WriteLine(text7);
                    string stringAsync1 = await httpClient7.GetStringAsync(text7);
                    string requestUri13 = "https://store.tr.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient7.GetStringAsync(requestUri13);
                    string requestUri14 = "https://store.tr.lol.riotgames.com/store/purchase/item";
                    HttpContent content7 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient7.PostAsync(requestUri14, content7);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient7.Dispose();
                    text7 = (string)null;
                    httpClient7 = (HttpClient)null;
                    requestUri13 = (string)null;
                    requestUri14 = (string)null;
                    content7 = (HttpContent)null;
                }
                else if (this.region == "LAS")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text8 = str;
                    str = (string)null;
                    HttpClient httpClient8 = new HttpClient();
                    Console.WriteLine(text8);
                    string stringAsync1 = await httpClient8.GetStringAsync(text8);
                    string requestUri15 = "https://store.la2.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient8.GetStringAsync(requestUri15);
                    string requestUri16 = "https://store.la2.lol.riotgames.com/store/purchase/item";
                    HttpContent content8 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient8.PostAsync(requestUri16, content8);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient8.Dispose();
                    text8 = (string)null;
                    httpClient8 = (HttpClient)null;
                    requestUri15 = (string)null;
                    requestUri16 = (string)null;
                    content8 = (HttpContent)null;
                }
                else if (this.region == "LAN")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text9 = str;
                    str = (string)null;
                    HttpClient httpClient9 = new HttpClient();
                    Console.WriteLine(text9);
                    string stringAsync1 = await httpClient9.GetStringAsync(text9);
                    string requestUri17 = "https://store.la1.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient9.GetStringAsync(requestUri17);
                    string requestUri18 = "https://store.la1.lol.riotgames.com/store/purchase/item";
                    HttpContent content9 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient9.PostAsync(requestUri18, content9);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient9.Dispose();
                    text9 = (string)null;
                    httpClient9 = (HttpClient)null;
                    requestUri17 = (string)null;
                    requestUri18 = (string)null;
                    content9 = (HttpContent)null;
                }
                else if (this.region == "OCE")
                {
                    string str = await this.connection.GetStoreUrl();
                    string text10 = str;
                    str = (string)null;
                    HttpClient httpClient10 = new HttpClient();
                    Console.WriteLine(text10);
                    string stringAsync1 = await httpClient10.GetStringAsync(text10);
                    string requestUri19 = "https://store.oc1.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient10.GetStringAsync(requestUri19);
                    string requestUri20 = "https://store.oc1.lol.riotgames.com/store/purchase/item";
                    HttpContent content10 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient10.PostAsync(requestUri20, content10);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient10.Dispose();
                    text10 = (string)null;
                    httpClient10 = (HttpClient)null;
                    requestUri19 = (string)null;
                    requestUri20 = (string)null;
                    content10 = (HttpContent)null;
                }
                else
                {
                    if (!(this.region == "JP"))
                        return;
                    string str = await this.connection.GetStoreUrl();
                    string text11 = str;
                    str = (string)null;
                    HttpClient httpClient11 = new HttpClient();
                    Console.WriteLine(text11);
                    string stringAsync1 = await httpClient11.GetStringAsync(text11);
                    string requestUri21 = "https://store.jp1.lol.riotgames.com/store/tabs/view/boosts/1";
                    string stringAsync2 = await httpClient11.GetStringAsync(requestUri21);
                    string requestUri22 = "https://store.jp1.lol.riotgames.com/store/purchase/item";
                    HttpContent content11 = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("item_id", "boosts_2"),
            new KeyValuePair<string, string>("currency_type", "rp"),
            new KeyValuePair<string, string>("quantity", "1"),
            new KeyValuePair<string, string>("rp", "260"),
            new KeyValuePair<string, string>("ip", "null"),
            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
            new KeyValuePair<string, string>("duration", "3")
          });
                    HttpResponseMessage httpResponseMessage = await httpClient11.PostAsync(requestUri22, content11);
                    Tools.ConsoleMessage("Bought 'XP Boost: 3 Days'!", ConsoleColor.White);
                    httpClient11.Dispose();
                    text11 = (string)null;
                    httpClient11 = (HttpClient)null;
                    requestUri21 = (string)null;
                    requestUri22 = (string)null;
                    content11 = (HttpContent)null;
                }
            }
            catch (Exception ex)
            {
                Tools.Log(ex.StackTrace);
                Console.WriteLine((object)ex);
            }
        }

        public void levelUp()
        {
            Tools.ConsoleMessage("Level Up: " + (object)this.sumLevel, ConsoleColor.Yellow);
            this.rpBalance = this.loginPacket.RpBalance;
            this.ipBalance = this.loginPacket.IpBalance;
            Tools.ConsoleMessage("Your Current IP: " + (object)this.ipBalance, ConsoleColor.Yellow);
            if (this.sumLevel >= (double)Program.maxLevel)
            {
                Tools.ConsoleMessage("Your character reached the max level: " + (object)Program.maxLevel, ConsoleColor.Red);
                this.connection.Disconnect();
            }
            else
            {
                if (this.rpBalance != 400.0 || !Program.buyExpBoost)
                    return;
                Tools.ConsoleMessage("Buying XP Boost", ConsoleColor.White);
                try
                {
                    new Task(new Action(this.buyBoost)).Start();
                }
                catch (Exception ex)
                {
                    Tools.Log(ex.StackTrace);
                    Tools.ConsoleMessage("Couldn't buy RP Boost.\n" + (object)ex, ConsoleColor.Red);
                }
            }
        }

        private string RandomString(int size)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < size; ++index)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * ezBot.random.NextDouble() + 65.0)));
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }
    }
}