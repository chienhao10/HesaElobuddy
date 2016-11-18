namespace ezBot.Languages
{
    public class KoreanTranslator : ITranslator
    {
        public string EzBot { get { return "ezBot - Auto Queue for LOL: {0}"; } }
        public string By { get { return "Made by Tryller updated, customized and supported by Hesa."; } }
        public string Version { get { return "Version: {0}"; } }
        public string Support { get { return "Get support on discord: https://discord.gg/Fg2tQGa"; } }
        public string Garena { get { return "Garena now supported!"; } }
        public string SourceCode { get { return "I uploaded the source code on github at github.com/hesa2020/HesaElobuddy."; } }
        public string Issues { get { return "Please report issue(s) on the ezbot thread customized by hesa on Elobuddy.net."; } }
        public string AdministratorRequired { get { return "ezBot must be started with administrator privileges."; } }
        public string ConfigLoaded { get { return "설정을 불러옵니다.."; } }
        public string LauncherPathInvalid { get { return "LauncherPath을 다시해주세요. (경로에 문제가 있습니다)"; } }
        public string PleaseTryThis { get { return "아래처럼 설정해주세요:"; } }
        public string LauncherFix1 { get { return "1. 경로가 제대로 설정되어있는지 확인해주세요. exe 파일이 아니라 폴더 경로 입니다."; } }
        public string LauncherFix2 { get { return "2. LauncherPath 경로 끝에 \\ 를 추가해주세요."; } }
        public string LauncherFix3 { get { return "3. LauncherPath 에 들어갑니다."; } }
        public string LauncherFix4 { get { return "4. 그리고 다음 경로에 들어갑니다. RADS\\solutions\\lol_game_client_sln\\releases\\"; } }
        public string LauncherFix5 { get { return "5. 다음을 제외한 모든 폴더를 삭제합니다. 제외 폴더: 0.0.1.152"; } }
        public string ChangingGameConfig { get { return "게임 설정이 변경되었습니다."; } }
        public string LoadingAccounts { get { return "계정을 불러오는 중입니다."; } }
        public string MaximumBotsRunning { get { return "최대 봇 수 작동중!: {0}"; } }
        public string YouMayHaveAnIssueInAccountsFile { get { return "account.txt에 문제가 있습니다."; } }
        public string AccountsStructure { get { return "계정 설정 구성: ACCOUNT|PASSWORD|REGION|QUEUE_TYPE|IS_LEADER"; } }
        public string ErrorGetGarenaToken { get { return "가레나 토큰 불러오기 에러!"; } }
        public string ErrorLeagueGameCfgRegular { get { return "일반 롤 클라이언트 game.cfg 에러: 만약에 VMWare 공유 폴더 사용시, 읽기전용으로 설정되어 있지 않은지 확인하세요.\nException: {0}"; } }
        public string ErrorLeagueGameCfgGarena { get { return "가레나 롤 클라이언트 game.cfg 에러: 만약에 VMWare 공유 폴더 사용시, 읽기전용으로 설정되어 있지 않은지 확인하세요.\nException: {0}"; } }
        public string NoMoreAccountsToLogin { get { return "더 로그인 할 계정이 없습니다."; } }
        public string GameModeInvalid { get { return "게임 모드가 잘못 되었습니다, 게임 모드를 제대로 설정해주세요. ( CASE SENSITIVE )"; } }
        public string WillShutdownOnceCurrentMatchEnds { get { return "이 게임이 끝나면 종료됩니다."; } }
        public string EzBotGameStatus { get { return "ezBot - {0} 게임 - {1} 승 - {2} 패"; } }
        public string AcceptingLobbyInvite { get { return "로비 초대를 수락합니다."; } }
        public string AllPlayersAccepted { get { return "모든 인원 수락함, 대기열 시작중."; } }
        public string PlayersAcceptedCount { get { return "{0}/{1} 명(s) 수락함, 나머지 인원 기다리는 중입니다."; } }
        public string EnteringChampionSelect { get { return "Entering champion selection."; } }
        public string YouAreInChampionSelect { get { return "챔피언이 선택되었습니다."; } }
        public string SelectedChampion { get { return "챔피언 선택: {0}."; } }
        public string WaitingForOtherPlayersLockin { get { return "다른 인원들의 락인을 기다립니다."; } }
        public string ChampionNotAvailable { get { return "챔피언 '{0}' 을 선택하지 못했습니다, 챔피언이 없거나 이미 선택되었습니다."; } }
        public string WaitingChampSelectTimer { get { return "챔피언 선택시간이 0초가 될때까지 기다리는 중입니다."; } }
        public string YouAreInQueue { get { return "대기열에 있습니다."; } }
        public string ReQueued { get { return "다시 큐는 중: {0} as {1}."; } }
        public string QueuePopped { get { return "큐가 닷지남."; } }
        public string AcceptedQueue { get { return "큐 잡힘!"; } }
        public string YouHaveLeaverBuster { get { return "탈주 패널티가 있습니다."; } }
        public string LaunchingLeagueOfLegends { get { return "League of Legends를 시작중입니다."; } }
        public string ClosingGameClient { get { return "게임 클라이언트 종료중."; } }
        public string InQueueAs { get { return "In Queue: {0} as {1}."; } }
        public string QueueFailedReason { get { return "큐 실패!, 이유: {0}."; } }
        public string LeaverBusterTaintedWarningError { get { return "탈주 패널티 경고 :\n{0}"; } }
        public string WaitingDodgeTimer { get { return "대기열 닷지 시간 기다리는 중입니다: {0} 만큼 남았습니다!"; } }
        public string WaitingLeaverTimer { get { return "탈주 패널티 시간 기다리는 중입니다: {0} 만큼 남았습니다!"; } }
        public string JoinedLowPriorityQueue { get { return "낮은 순위 대기열에 참여했습니다! {0}."; } }
        public string ErrorJoiningLowPriorityQueue { get { return "우선 순위가 낮은 대기열에 참여하는 도중 오류가 발했습니다.\n연결 해제중."; } }
        public string ErrorOccured { get { return "에러 발생:\n{0}"; } }
        public string RestartingLeagueOfLegends { get { return "League of Legends.exe를 재시작 중입니다."; } }
        public string RestartingLeagueOfLegendsAt { get { return "League of Legends.exe를 재시작 중입니다 {0} 기다려주세요."; } }
        public string PositionInLoginQueue { get { return "로그인 대기열 상태: {0}."; } }
        public string LoggingIntoAccount { get { return "계정에 로그인 중입니다..."; } }
        public string SummonerDoesntExist { get { return "해당 계정이 없습니다."; } }
        public string CreatingSummoner { get { return "Summoner 만드는 중..."; } }
        public string CreatedSummoner { get { return "Summoner 만들어짐: {0}."; } }
        public string AlreadyMaxLevel { get { return "Summoner: {0} 는 이미 최대 레벨입니다."; } }
        public string LogIntoNewAccount { get { return "Log into new account."; } }
        public string BuyingXpBoost { get { return "XP 부스트 구매"; } }
        public string CouldntBuyBoost { get { return "부스트 구매 에러:\n{0}"; } }
        public string Normal5Requirements { get { return "일반 비공개 5대5(NORMAL_5X5)를 하려면 레벨 3 이상이 되어야합니다."; } }
        public string JoinCoopBeginnerUntil { get { return "Co-Op vs AI (초급봇) 대기열에 들어갑니다 {0}."; } }
        public string NeedLevel6BeforeAram { get { return "칼바람 나락(ARAM)을 하려면 레벨 6 이상이 되어야합니다."; } }
        public string NeedLevel7Before3v3 { get { return "일반 비공개 3대3(NORMAL_3X3)를 하려면 레벨 7 이상이 되어야합니다."; } }
        public string Welcome { get { return "환영합니다! {0} - lvl ({1}) IP: ({2}) - XP: ({3} / {4})."; } }
        public string SendingGameInvites { get { return "게임 초대 보내기"; } }
        public string WaitingGameInviteFrom { get { return "게임 초대 기다리는 중입니다 {0}."; } }
        public string LevelUp { get { return "레벨 업: {0}."; } }
        public string CurrentRp { get { return "현재 RP: {0}."; } }
        public string CurrentIp { get { return "현재 IP: {0}."; } }
        public string CharacterReachedMaxLevel { get { return "계정이 최대 레벨에 도달 했습니다.: {0}."; } }
        public string DownloadingMasteries { get { return "champion.gg에서 마스터리를 다운로드 중입니다"; } }
        public string UpdatingMasteries { get { return "마스터리를 업데이트 했습니다."; } }
        public string Disconnected { get { return "연결해제 되었습니다."; } }
        public string BoughtXpBoost3Days { get { return "'XP 부스트: 3 일' 구매했습니다!"; } }

    }
}