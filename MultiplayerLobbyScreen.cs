// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Matchmaking;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class MultiplayerLobbyScreen : IGameSubScreen
{
  internal IGame.GameType gameType;
  private int nextRefreshTime = 999999;
  private int currentLobbyPlayers;
  private bool hasMeReadied;
  private bool hasOppReadied;
  private bool arePlayersReady;
  private bool hasInvitedFriend;
  public GameParameters lobbyParams;
  private MultiplayerLobbyScreen.UIState currentUIState;
  private MultiplayerLobbyScreen.UIState prevUIState;
  private IPlayerInfo myPlayer;
  private IPlayerInfo opponentPlayer;
  private string previewOpponentName;
  private Disease.EDiseaseType previewOpponentDisease;
  private Gene[] previewOpponentGenes;
  private MultiplayerLobbyScreen.PlayerSummaryMode currentPlayerSummaryMode;
  private bool isTogglingPlayerSummaryMode;
  public LobbyTipBarUI infoBarUI;
  public LobbyGameModeStatusUI gameModeStatusUI;
  public LobbyPlayerMySlotUI myPlayerSlotUI;
  public LobbyPlayerOpponentSlotUI opponentPlayerSlotUI;
  public LobbyPlagueEditorUI plagueEditorUI;
  public UILabel activeGamesLabel;
  public Texture2D opponentPlayerAIAvatar;
  [Header("Animations")]
  public Animator mainLobbyAnimator;
  public AnimatorOverrideController lobbyCoopFriendsAnimController;
  public AnimatorOverrideController lobbyCoopPracticeAnimController;
  public AnimatorOverrideController lobbyCoopQuickMatchAnimController;
  public RuntimeAnimatorController lobbyVersusController;
  public LobbyTipBarAnim lobbyTipBarAnim;
  [Header("Rankings")]
  public MultiplayerRankingsWidget rankingsWidget;
  private const int EDITING_DURATION_SECS = 60;
  private const int READY_DURATION_SECS = 10;
  private const int CONFIG_REFRESH_WAIT_TIME = 240;
  private const int CONFIG_REFRESH_ACTIVE_GAMES = 5;
  private DateTime startSearchingTimeStamp;
  private DateTime startLastSearchTimeStamp;
  private DateTime startEditingCountdownTimeStamp;
  private DateTime startFinalReadiedCountdownTimeStamp;
  private TimeSpan editingDuration;
  private TimeSpan readyDuration;
  private bool hasGameStarted;
  private bool hasRefreshedConfig;
  private string lastPlayerPlayed = "";
  private bool isAIGame;
  private int p1Difficulty = 1;
  private int p2Difficulty = 1;
  private int currentRatingDifferential;
  private List<Disease.EDiseaseType> availableDiseases;
  private List<Gene> availableGenes;
  private int ndemicStatsActiveGames;
  private float ndemicStatsWaitTimes = -1f;
  private int ndemicWaitTimeRetrievalCount;
  private Coroutine pingNdemicActiveGamesCo;
  private Coroutine pingNdemicWaitTimeCo;
  private CNetworkSteam network;
  private IGame game;
  public static MultiplayerLobbyScreen instance;
  private string debugText = "";
  private Coroutine _recordFindLobbyTime;
  private float gameFindStartTime;
  private float lastClickMark;
  private string playerCheater;
  private string playerAnnoyer;
  private string playerMultiAccount;
  public static string cheaterURL = CGameManager.federalServerAddress + "cheater.txt";
  public static string annoyerURL = CGameManager.federalServerAddress + "annoyer.txt";
  public static string multiAccountURL = CGameManager.federalServerAddress + "multiaccount.txt";
  public static int steamActiveGames;

  private void Awake() => MultiplayerLobbyScreen.instance = this;

  public override void Initialise()
  {
    base.Initialise();
    this.network = CNetworkManager.network as CNetworkSteam;
    this.editingDuration = TimeSpan.FromSeconds(60.0);
    this.readyDuration = TimeSpan.FromSeconds(10.0);
    this.InitUIHandlers();
    this.lobbyParams = new GameParameters();
    this.lobbyParams.multiplayerGameVersion = this.network.GameNetworkVersion;
    this.lobbyParams.friendMode = INetwork.FriendMode.Public;
    this.lobbyParams.numberOfPlayers = 2;
    this.myPlayerSlotUI.diseaseNameInput.characterLimit = 256;
  }

  private void OnEnable()
  {
    this.myPlayerSlotUI.diseaseNameInput.characterLimit = 256;
    CNetworkSteam network = CNetworkManager.network as CNetworkSteam;
    if (!((UnityEngine.Object) network != (UnityEngine.Object) null))
      return;
    network.onNetworkStateChanged += new Action<CNetworkManager.ENetworkState>(this.network_onNetworkStateChanged);
    network.onUserStatsReceived += new CNetworkSteam.OnUserStatsReceived(this.network_onUserStatsReceived);
    network.onGameVersionMismatch += new CNetworkSteam.OnGameVersionMismatch(this.network_onGameVersionMismatch);
  }

  private void OnDisable()
  {
    CNetworkSteam network = CNetworkManager.network as CNetworkSteam;
    if (!((UnityEngine.Object) network != (UnityEngine.Object) null))
      return;
    network.onNetworkStateChanged -= new Action<CNetworkManager.ENetworkState>(this.network_onNetworkStateChanged);
    network.onUserStatsReceived -= new CNetworkSteam.OnUserStatsReceived(this.network_onUserStatsReceived);
    network.onGameVersionMismatch -= new CNetworkSteam.OnGameVersionMismatch(this.network_onGameVersionMismatch);
  }

  public override void SetActive(bool b)
  {
    if (CGameManager.oldPotential == 25.5555)
      CNetworkManager.network.LocalPlayerInfo.ForceRatingEdit(1989, true);
    base.SetActive(b);
    if (b)
    {
      this.myPlayerSlotUI.diseaseNameInput.characterLimit = 256;
      this.StartCoroutine(this.GetSpecialPlayerList(MultiplayerLobbyScreen.SpecialPlayerType.Annoyer));
      this.StartCoroutine(this.GetSpecialPlayerList(MultiplayerLobbyScreen.SpecialPlayerType.Cheater));
      this.StartCoroutine(this.GetSpecialPlayerList(MultiplayerLobbyScreen.SpecialPlayerType.MultiAccount));
      this.debugText = "";
      this.LobbyDebug("****SET ACTIVE - GAME TYPE:" + (object) this.gameType);
      if (this.gameType != IGame.GameType.VersusMP && this.gameType != IGame.GameType.CoopMP)
        this.gameType = IGame.GameType.CoopMP;
      if (this.gameType == IGame.GameType.VersusMP)
        MultiplayerRankings.instance.LoadMyMultiplayerRanking(this.gameType, 0);
      else if (this.gameType == IGame.GameType.CoopMP)
      {
        MultiplayerRankings.instance.LoadMyMultiplayerRanking(this.gameType, 0);
        MultiplayerRankings.instance.LoadMyMultiplayerRanking(this.gameType, 1);
        MultiplayerRankings.instance.LoadMyMultiplayerRanking(this.gameType, 2);
      }
      this.isAIGame = false;
      CGameManager.ClearGame();
      CGameManager.InitialiseGame(this.gameType);
      CGameManager.game.SetGameParameters(this.lobbyParams);
      this.availableGenes = new List<Gene>()
      {
        new Gene()
        {
          id = "_empty",
          geneName = "MP_Empty_Gene_Title",
          geneDescription = "MP_Empty_Gene_Description"
        }
      };
      this.availableGenes.AddRange((IEnumerable<Gene>) DataImporter.ImportGenes(CGameManager.LoadGameText("Genes/genes")));
      this.availableDiseases = new List<Disease.EDiseaseType>()
      {
        Disease.EDiseaseType.BACTERIA,
        Disease.EDiseaseType.VIRUS,
        Disease.EDiseaseType.FUNGUS,
        Disease.EDiseaseType.PARASITE,
        Disease.EDiseaseType.BIO_WEAPON
      };
      this.startEditingCountdownTimeStamp = DateTime.MaxValue;
      this.plagueEditorUI.SetToDefaults();
      this.plagueEditorUI.Init(this.gameType, this.availableDiseases, this.availableGenes);
      this.ResetDifficultyToggles();
      this.lobbyTipBarAnim.Init(this.gameType);
      this.UpdatePlayerSummaryMode(MultiplayerLobbyScreen.PlayerSummaryMode.PlagueDetails);
      this.startSearchingTimeStamp = DateTime.Now;
      this.hasGameStarted = false;
      this.myPlayer = CNetworkManager.network.LocalPlayerInfo;
      this.opponentPlayer = (IPlayerInfo) null;
      this.StartCoroutine(this.InitHostPlayerSlot());
      this.network_onNetworkStateChanged((CNetworkManager.network as CNetworkSteam).NetworkState);
      if (this.pingNdemicActiveGamesCo != null)
        this.StopCoroutine(this.pingNdemicActiveGamesCo);
      this.pingNdemicActiveGamesCo = this.StartCoroutine(this.GetNdemicActiveGames());
    }
    else
    {
      this.DestroyGame();
      if (this.pingNdemicActiveGamesCo != null)
        this.StopCoroutine(this.pingNdemicActiveGamesCo);
      if (this.pingNdemicWaitTimeCo == null)
        return;
      this.StopCoroutine(this.pingNdemicWaitTimeCo);
    }
  }

  public void SetGameType(IGame.GameType gameType) => this.gameType = gameType;

  public void CreateGame()
  {
    this.LobbyDebug("CreateGame - gameType:" + (object) this.gameType + ", game:" + (object) this.game);
    if ((UnityEngine.Object) this.game != (UnityEngine.Object) null)
      return;
    this.game = CGameManager.InitialiseGame(this.gameType);
    this.game.SetGameParameters(this.lobbyParams);
  }

  private void DestroyGame() => this.game = (IGame) null;

  private IEnumerator GetNdemicActiveGames()
  {
    MultiplayerLobbyScreen multiplayerLobbyScreen = this;
    while (true)
    {
      multiplayerLobbyScreen.ndemicStatsActiveGames = multiplayerLobbyScreen.steamActiveGames + 3;
      // ISSUE: reference to a compiler-generated method
      NdemicAnalytics.GetEvent(NdemicAnalytics.EventTypeGet.ActiveGames, new Action<string>(multiplayerLobbyScreen.\u003CGetNdemicActiveGames\u003Eb__0_0));
      multiplayerLobbyScreen.ndemicStatsActiveGames = multiplayerLobbyScreen.steamActiveGames + 3;
      yield return (object) new WaitForSeconds(10f);
    }
  }

  private void RetrieveNdemicWaitTime()
  {
    NdemicAnalytics.GetEvent(NdemicAnalytics.EventTypeGet.WaitTimes, (Action<string>) (res =>
    {
      this.ndemicStatsWaitTimes = -1f;
      float.TryParse(res, out this.ndemicStatsWaitTimes);
      this.UpdateAvgWaitTime();
      if (this.ndemicWaitTimeRetrievalCount != 0)
        return;
      if (this.pingNdemicWaitTimeCo != null)
        this.StopCoroutine(this.pingNdemicWaitTimeCo);
      this.pingNdemicWaitTimeCo = this.StartCoroutine(this.GetNdemicWaitTimeCo());
    }));
  }

  private IEnumerator GetNdemicWaitTimeCo()
  {
    MultiplayerLobbyScreen multiplayerLobbyScreen = this;
    while (true)
    {
      float seconds = 20f;
      if (multiplayerLobbyScreen.ndemicWaitTimeRetrievalCount == 0)
      {
        multiplayerLobbyScreen.LobbyDebug("---ndemicStatsWaitTimes:" + (object) multiplayerLobbyScreen.ndemicStatsWaitTimes);
        seconds = 20f + UnityEngine.Random.Range(0.0f, (float) (10.0 + (double) multiplayerLobbyScreen.ndemicStatsWaitTimes / 10.0));
      }
      ++multiplayerLobbyScreen.ndemicWaitTimeRetrievalCount;
      multiplayerLobbyScreen.LobbyDebug("INITIAL WAIT TIME TIME:" + (object) seconds + ", ndemicStatsWaitTimes:" + (object) multiplayerLobbyScreen.ndemicStatsWaitTimes);
      yield return (object) new WaitForSeconds(seconds);
      multiplayerLobbyScreen.RetrieveNdemicWaitTime();
    }
  }

  private void UpdateAvgWaitTime()
  {
    if (this.ndemicWaitTimeRetrievalCount >= 1 && (double) this.ndemicStatsWaitTimes > 0.0 && (double) this.ndemicStatsWaitTimes < 480.0)
    {
      float ndemicStatsWaitTimes = this.ndemicStatsWaitTimes;
      string str;
      if ((double) ndemicStatsWaitTimes < 120.0)
        str = CLocalisationManager.GetText("MP_Under") + " 2 " + CLocalisationManager.GetText("MP_Minutes");
      else
        str = CLocalisationManager.GetText("MP_Under") + " " + (object) Mathf.FloorToInt(ndemicStatsWaitTimes / 60f) + " " + CLocalisationManager.GetText("MP_Minutes");
      this.opponentPlayerSlotUI.avgWaitTimeLabel.text = CLocalisationManager.GetText("MP_Average_Wait_Time") + " " + str;
    }
    else if (MultiplayerLobbyScreen.steamActiveGames > 0)
      this.opponentPlayerSlotUI.avgWaitTimeLabel.text = CLocalisationManager.GetText("MP_Average_Wait_Time") + " Less than 114514 Seconds";
    else
      this.opponentPlayerSlotUI.avgWaitTimeLabel.text = CLocalisationManager.GetText("MP_Average_Wait_Time") + " More than 114514 Years";
  }

  private void network_onNetworkStateChanged(CNetworkManager.ENetworkState newState)
  {
    this.LobbyDebug("Lobby[" + this.gameObject.activeSelf.ToString() + "] newState:" + (object) newState);
    if (!this.gameObject.activeSelf)
      return;
    switch (newState)
    {
      case CNetworkManager.ENetworkState.Idle:
        this.currentLobbyPlayers = 0;
        this.UpdateUIState(MultiplayerLobbyScreen.UIState.LobbyEntrance);
        this.StopCoroutine("TryConnectGameSession");
        break;
      case CNetworkManager.ENetworkState.InLobby:
        CNetworkSteam network = CNetworkManager.network as CNetworkSteam;
        this.LobbyDebug("GetCurrentLobbyType():" + (object) network.LobbyHandler.GetCurrentLobbyType() + ", network.LobbyHandler.ConnectedLobbyMembers.Count:" + (object) network.LobbyHandler.ConnectedLobbyMembers.Count);
        if (network.LobbyHandler.GetCurrentLobbyType() == INetwork.FriendMode.Friends)
        {
          this.lobbyParams.friendMode = INetwork.FriendMode.Friends;
          this.mainLobbyAnimator.runtimeAnimatorController = this.gameType == IGame.GameType.VersusMP ? this.lobbyVersusController : (RuntimeAnimatorController) this.lobbyCoopFriendsAnimController;
          if (network.LobbyHandler.ConnectedLobbyMembers.Count == 2)
            this.UpdateUIState(MultiplayerLobbyScreen.UIState.ConnectingToFriend);
          else
            this.UpdateUIState(MultiplayerLobbyScreen.UIState.WaitingForFriends);
        }
        this.StopCoroutine("TryConnectGameSession");
        break;
      case CNetworkManager.ENetworkState.InGame:
        this.OnOpponentValidated();
        break;
      case CNetworkManager.ENetworkState.JoinLobbyFailed:
        this.SetNextLobbyRefreshTime();
        break;
      default:
        this.StopCoroutine("TryConnectGameSession");
        break;
    }
  }

  private void network_onUserStatsReceived(CSteamID steamID)
  {
    this.LobbyDebug("network_onUserStatsReceived - steamID:" + (object) steamID);
  }

  private void network_onGameVersionMismatch(bool isOlder)
  {
    this.LobbyDebug("network_onSteamGameVersionMismatch - isOlder:" + isOlder.ToString());
    this.ThrowMismatchMultiplayerVersionScreen();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.G))
    {
      CGameManager.canMatchAnnoyer = !CGameManager.canMatchAnnoyer;
      string body = "";
      switch (CGameManager.blockType)
      {
        case CGameManager.EBlockType.BLOCK:
          body = "You [00ffff]can[ffffff] match players regarded as annoyer now!\n(Opponent can only be [00ff00]guest[ffffff])";
          CGameManager.blockType = CGameManager.EBlockType.GUEST_ONLY;
          break;
        case CGameManager.EBlockType.GUEST_ONLY:
          body = "You [00ffff]can[ffffff] match players regarded as annoyer now!";
          CGameManager.blockType = CGameManager.EBlockType.ALLOW;
          break;
        case CGameManager.EBlockType.ALLOW:
          body = "You [ffff00]cannot[ffffff] match players regarded as annoyer now!";
          CGameManager.blockType = CGameManager.EBlockType.BLOCK;
          break;
      }
      CUIManager.instance.standardConfirmOverlay.ShowLocalised("Reminder", body, "YES", "OK");
    }
    this.network = CNetworkManager.network as CNetworkSteam;
    if (this.network.IsOfflineGame)
      return;
    switch (this.network.NetworkState)
    {
      case CNetworkManager.ENetworkState.SearchingLobbies:
      case CNetworkManager.ENetworkState.JoinLobbyFailed:
        this.OnUpdateSearchingStatus();
        break;
      case CNetworkManager.ENetworkState.InLobby:
        if (this.network.NumberOfConnectedPlayers == 2)
        {
          if (this.currentLobbyPlayers != this.network.NumberOfConnectedPlayers)
            this.OnOpponentJoined();
        }
        else if (this.network.NumberOfConnectedPlayers == 1 && this.currentLobbyPlayers != this.network.NumberOfConnectedPlayers)
        {
          if (this.currentLobbyPlayers == 0)
            this.OnCreatedLobby();
          else
            this.OnOpponentLeft();
        }
        this.OnUpdateWaitingStatus();
        this.currentLobbyPlayers = this.network.NumberOfConnectedPlayers;
        break;
      case CNetworkManager.ENetworkState.InGame:
        if (this.network.NumberOfConnectedPlayers == 1)
        {
          if (this.currentLobbyPlayers != this.network.NumberOfConnectedPlayers)
            this.OnOpponentLeft();
        }
        else
          this.OnUpdateReadyStatus();
        this.currentLobbyPlayers = this.network.NumberOfConnectedPlayers;
        break;
    }
  }

  public void LobbyDebug(string msg)
  {
    msg = "[" + DateTime.Now.ToString("hh:mm:ss fff") + "] " + msg;
    Debug.Log((object) msg);
  }

  private void OnCreatedLobby()
  {
    this.LobbyDebug(nameof (OnCreatedLobby));
    this.SetNextLobbyRefreshTime();
  }

  private void OnUpdateSearchingStatus()
  {
    TimeSpan elapsed = DateTime.Now - this.startSearchingTimeStamp;
    this.opponentPlayerSlotUI.searchingTimeLabel.text = CLocalisationManager.GetText("MP_Finding_Match") + " " + this.FormatTimespan(elapsed);
    if (elapsed.TotalSeconds <= (double) this.nextRefreshTime)
      return;
    this.FindQuickMatch(false);
  }

  private void OnUpdateWaitingStatus()
  {
    TimeSpan elapsed = DateTime.Now - this.startSearchingTimeStamp;
    if (Mathf.FloorToInt(Time.realtimeSinceStartup - this.gameFindStartTime) > 240 && this.ndemicStatsActiveGames > 5 && !this.hasRefreshedConfig)
    {
      this.hasRefreshedConfig = true;
      DynamicNewsLoader.instance.RefreshMultiplayerConfig();
    }
    else if (this.lobbyParams.friendMode == INetwork.FriendMode.Public)
    {
      this.opponentPlayerSlotUI.searchingTimeLabel.text = CLocalisationManager.GetText("MP_Finding_Match") + " " + this.FormatTimespan(elapsed);
      if (elapsed.TotalSeconds <= (double) this.nextRefreshTime)
        return;
      this.LobbyDebug("FindLobbyInBackground - spentWaiting.TotalSeconds:" + (object) elapsed.TotalSeconds + ", nextRefreshTime:" + (object) this.nextRefreshTime);
      this.currentRatingDifferential += CGameManager.multiplayerLobbyRatingSearchIncrement;
      this.FindQuickMatch(false);
    }
    else
      this.opponentPlayerSlotUI.searchingTimeLabel.text = CLocalisationManager.GetText("MP_P2_Slot_Status_Waiting_Friend");
  }

  private void SetNextLobbyRefreshTime()
  {
    int num = UnityEngine.Random.Range(CGameManager.multiplayerLobbyMinRefresh, CGameManager.multiplayerLobbyMaxRefresh);
    this.LobbyDebug("SetNextLobbyRefreshTime() - advanceTime:" + (object) num);
    if (num < 5)
      num = 5;
    this.nextRefreshTime += num;
  }

  private void OnOpponentJoined()
  {
    this.LobbyDebug("OnOpponentJoined - network.NumberOfConnectedPlayers:" + (object) this.network.NumberOfConnectedPlayers);
    this.startEditingCountdownTimeStamp = DateTime.MaxValue;
    this.network.LobbyHandler.CancelFindLobby();
    this.nextRefreshTime = 9999999;
    this.StartCoroutine("TryConnectGameSession");
  }

  private void OnOpponentValidated()
  {
    this.LobbyDebug(nameof (OnOpponentValidated));
    this.hasGameStarted = false;
    this.hasOppReadied = false;
    this.hasMeReadied = false;
    this.arePlayersReady = false;
    if (this.lobbyParams.friendMode == INetwork.FriendMode.Public && this.startEditingCountdownTimeStamp == DateTime.MaxValue)
      this.startEditingCountdownTimeStamp = DateTime.Now;
    this.StartCoroutine(this.InitOpponentPlayerSlot());
    IPlayerInfo guestPlayerInfo = this.GetGuestPlayerInfo();
    if (guestPlayerInfo == null)
    {
      CSoundManager.instance.PlaySFX("immune_shock");
      this.LobbyDebug("Guest could not be found");
      this.CancelFromLobby();
      this.PressQuickMatch();
    }
    else
    {
      this.LobbyDebug("Guest joined: " + guestPlayerInfo.name + ", guest rating:" + (object) guestPlayerInfo.playerStats.MP_Rating);
      if (MultiplayerLobbyScreen.IsAnnoyer(guestPlayerInfo.PlayerID) && CGameManager.blockType == CGameManager.EBlockType.BLOCK)
      {
        this.LobbyDebug("Guest Found but due to opponent is annoyer " + guestPlayerInfo.name + ", according to the Constitution of Plague Inc Federation, this game is automatically stopped and returning you to the lobby.");
        CSoundManager.instance.PlaySFX("benign_mimic");
        this.CancelFromLobby();
        this.PressQuickMatch();
      }
      else
      {
        CSoundManager.instance.PlaySFX("pause_request");
        int num = Mathf.FloorToInt(Time.realtimeSinceStartup - this.gameFindStartTime);
        if (this.network.LobbyHandler.GetCurrentLobbyType() == INetwork.FriendMode.Friends)
          num = -1;
        NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.GameStart, num, this.myPlayer.GetMultiplayerRating(), guestPlayerInfo.GetMultiplayerRating(), SteamUtils.GetIPCountry());
        this.UpdateUIState(MultiplayerLobbyScreen.UIState.WaitingToStart);
        this.UpdatePlayerSummaryMode(this.currentPlayerSummaryMode);
        if (this.gameType != IGame.GameType.CoopMP)
          return;
        this.UpdateDifficulty(this.p1Difficulty);
      }
    }
  }

  private void OnOpponentLeft()
  {
    this.LobbyDebug("OnOpponentLeft - currentLobbyPlayers:" + (object) this.currentLobbyPlayers + ", lobby:" + (object) this.network.LobbyHandler.GetCurrentLobbyType());
    this.opponentPlayer = (IPlayerInfo) null;
    this.startEditingCountdownTimeStamp = DateTime.MaxValue;
    if (this.network.LobbyHandler.GetCurrentLobbyType() == INetwork.FriendMode.Friends)
    {
      this.CancelFromLobby();
    }
    else
    {
      NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
      Disease disease = CGameManager.localPlayerInfo.disease;
      CPlayerInfoSteam localPlayerInfo = CGameManager.localPlayerInfo as CPlayerInfoSteam;
      NdemicAnalytics.RecordTechAnalytics(this.gameType, 0, localPlayerInfo.PlayerID, localPlayerInfo.name, localPlayerInfo.GetMultiplayerRating(), false, -1, disease?.technologies, disease?.genes, disease != null ? disease.diseaseType : Disease.EDiseaseType.BACTERIA, disease != null ? disease.nexus.id : string.Empty);
      this.network.LobbyHandler.LeaveLobby();
      this.FindQuickMatch(true);
    }
  }

  private void OnUpdateReadyStatus()
  {
    bool flag = this.hasMeReadied && this.hasOppReadied;
    if (this.arePlayersReady != flag && flag)
    {
      this.startFinalReadiedCountdownTimeStamp = DateTime.Now;
      this.UpdateUIState(MultiplayerLobbyScreen.UIState.PlayersReadied);
    }
    if ((this.hasMeReadied || this.hasOppReadied) && this.lobbyParams.friendMode == INetwork.FriendMode.Friends && this.startEditingCountdownTimeStamp == DateTime.MaxValue)
      this.startEditingCountdownTimeStamp = DateTime.Now;
    TimeSpan elapsed;
    if (flag)
    {
      elapsed = this.readyDuration - (DateTime.Now - this.startFinalReadiedCountdownTimeStamp);
      this.SetFixedDifficulty(this.GetDifficulty());
    }
    else
    {
      elapsed = this.editingDuration - (DateTime.Now - this.startEditingCountdownTimeStamp);
      if (this.gameType == IGame.GameType.VersusMP)
      {
        this.opponentPlayerSlotUI.editingInfo.SetActive(!this.hasOppReadied);
        this.opponentPlayerSlotUI.readyInfo.SetActive(this.hasOppReadied);
      }
      if (!this.hasMeReadied)
      {
        int num = this.hasOppReadied ? 1 : 0;
      }
    }
    string str = this.FormatSecondsTimespan(elapsed);
    if (elapsed.TotalSeconds <= 0.0)
    {
      if (flag)
      {
        if (!this.hasGameStarted)
          this.StartCoroutine(this.StartGame());
      }
      else if (!this.gameModeStatusUI.p1StatusToggle.value)
      {
        this.FinishEditPlague();
        this.gameModeStatusUI.p1StatusToggle.value = true;
      }
    }
    else if (elapsed.TotalSeconds <= 6000.0)
    {
      this.gameModeStatusUI.gameStatusPlaceholder.SetActive(false);
      this.gameModeStatusUI.gameStatusTimerLabel.gameObject.SetActive(true);
      this.gameModeStatusUI.gameStatusTimerLabel.text = str;
    }
    this.arePlayersReady = flag;
  }

  private IEnumerator TryConnectGameSession()
  {
    MultiplayerLobbyScreen multiplayerLobbyScreen = this;
    multiplayerLobbyScreen.LobbyDebug("TryConnectGameSession - network.IsServer:" + multiplayerLobbyScreen.network.IsServer.ToString());
    if (multiplayerLobbyScreen.network.LobbyHandler.GetCurrentLobbyType() == INetwork.FriendMode.Friends)
      multiplayerLobbyScreen.UpdateUIState(MultiplayerLobbyScreen.UIState.ConnectingToFriend);
    while (multiplayerLobbyScreen.network.NetworkState != CNetworkManager.ENetworkState.InGame)
    {
      multiplayerLobbyScreen.LobbyDebug("Trying to do ConnectGameSession - isServer: " + multiplayerLobbyScreen.network.IsServer.ToString() + ", currentLobbyId:" + (object) multiplayerLobbyScreen.network.LobbyHandler.CurrentLobbyID);
      multiplayerLobbyScreen.LobbyDebug("------ gameType:" + (object) multiplayerLobbyScreen.gameType + ", game:" + (object) multiplayerLobbyScreen.game + ", CGameManager.gameType:" + (object) CGameManager.gameType);
      if (CGameManager.game.ConnectGameSession())
        break;
      yield return (object) new WaitForSeconds(2f);
    }
  }

  private IEnumerator StartGame()
  {
    this.LobbyDebug(nameof (StartGame));
    this.hasGameStarted = true;
    this.lastPlayerPlayed = this.opponentPlayer.PlayerID;
    this.UpdateUIState(MultiplayerLobbyScreen.UIState.StartingGame);
    yield return (object) new WaitForSeconds(0.1f);
    CGameManager.game.CreateMPGameSession(this.GetDiseaseName(), this.GetInsertedDiseaseType(), this.GetConvertedInsertedGenes(), this.GetDifficulty());
  }

  private IEnumerator InitHostPlayerSlot()
  {
    this.LobbyDebug(nameof (InitHostPlayerSlot));
    CNetworkSteam ntwrk = CNetworkManager.network as CNetworkSteam;
    while ((UnityEngine.Object) null == (UnityEngine.Object) ntwrk)
      yield return (object) false;
    this.myPlayerSlotUI.diseaseNameInput.savedAs = "UIInput_MP_LobbyDiseaseName_" + ntwrk.LocalPlayerInfo.name;
    string text = CLocalisationManager.GetText("MP_Name_Your_Plague");
    this.myPlayerSlotUI.diseaseNameInput.value = PlayerPrefs.GetString(this.myPlayerSlotUI.diseaseNameInput.savedAs, text);
    this.myPlayerSlotUI.diseaseNameInput.defaultText = text;
    this.myPlayerSlotUI.headerLabel.text = CLocalisationManager.GetText("%name(ELO:" + ntwrk.LocalPlayerInfo.playerStats.MP_Rating.ToString() + ")").Replace("%name", CUtils.CharacterLimit(ntwrk.LocalPlayerInfo.name, 20, "..."));
    this.myPlayerSlotUI.badgeSprite.spriteName = ntwrk.LocalPlayerInfo.GetHighestMultiplayerBadge(this.gameType, this.GetDifficulty());
    this.RefreshMyPlayerPlagueSummary();
    while ((UnityEngine.Object) null == (UnityEngine.Object) ntwrk.LocalPlayerInfo.largeAvatar)
      yield return (object) false;
    this.myPlayerSlotUI.avatar.mainTexture = (Texture) ntwrk.LocalPlayerInfo.largeAvatar;
  }

  private void RefreshMyPlayerPlagueSummary()
  {
    this.myPlayerSlotUI.plagueDetailsUI.UpdatePlague(this.GetInsertedDiseaseType(), "", this.GetInsertedGenes(), 0);
  }

  private IEnumerator InitOpponentPlayerSlot()
  {
    MultiplayerLobbyScreen multiplayerLobbyScreen = this;
    Debug.Log((object) "INIT GUEST PLAYER SLOT");
    CNetworkSteam ntwrk = CNetworkManager.network as CNetworkSteam;
    while ((UnityEngine.Object) null == (UnityEngine.Object) ntwrk)
      yield return (object) false;
    SteamLobbyHandler lobby = (CNetworkManager.network as CNetworkSteam).LobbyHandler;
    Debug.Log((object) ("Check connected members: " + (object) lobby.ConnectedLobbyMembers.Count));
    while (lobby.ConnectedLobbyMembers.Count < 2)
      yield return (object) false;
    multiplayerLobbyScreen.LobbyDebug("HAVE A GUEST!");
    string str = lobby.ConnectedLobbyMembers[0] == (ntwrk.LocalPlayerInfo as CPlayerInfoSteam).steamID ? lobby.ConnectedLobbyMembers[1].m_SteamID.ToString() : lobby.ConnectedLobbyMembers[0].m_SteamID.ToString();
    multiplayerLobbyScreen.opponentPlayer = ntwrk.GetPlayerInfo(str);
    multiplayerLobbyScreen.LobbyDebug("---opponentPlayer.played:" + (object) multiplayerLobbyScreen.opponentPlayer.playerStats.MP_GamesPlayed + "\nPlayer ID:" + str);
    if (MultiplayerLobbyScreen.IsCheater(str))
      multiplayerLobbyScreen.opponentPlayerSlotUI.headerLabel.color = Color.green;
    else if (MultiplayerLobbyScreen.IsMultiAccount(str))
      multiplayerLobbyScreen.opponentPlayerSlotUI.headerLabel.color = Color.blue;
    else
      multiplayerLobbyScreen.opponentPlayerSlotUI.headerLabel.color = Color.white;
    multiplayerLobbyScreen.opponentPlayerSlotUI.headerLabel.text = CLocalisationManager.GetText("%name(ELO:" + multiplayerLobbyScreen.opponentPlayer.playerStats.MP_Rating.ToString() + ")").Replace("%name", CUtils.CharacterLimit(multiplayerLobbyScreen.opponentPlayer.name, 20, "..."));
    multiplayerLobbyScreen.opponentPlayerSlotUI.badgeSprite.spriteName = multiplayerLobbyScreen.opponentPlayer.GetHighestMultiplayerBadge(multiplayerLobbyScreen.gameType, multiplayerLobbyScreen.GetDifficulty());
    multiplayerLobbyScreen.opponentPlayerSlotUI.badgeAnimator.SetTrigger(multiplayerLobbyScreen.isAIGame ? "Badge_Off" : "Badge_On");
    while ((UnityEngine.Object) null == (UnityEngine.Object) multiplayerLobbyScreen.opponentPlayer.largeAvatar)
      yield return (object) false;
    multiplayerLobbyScreen.opponentPlayerSlotUI.avatar.gameObject.SetActive(true);
    multiplayerLobbyScreen.opponentPlayerSlotUI.avatar.mainTexture = (Texture) multiplayerLobbyScreen.opponentPlayer.largeAvatar;
    if (multiplayerLobbyScreen.gameType == IGame.GameType.CoopMP)
    {
      multiplayerLobbyScreen.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(false);
      multiplayerLobbyScreen.UpdateToggleAndDiseaseInfo();
    }
  }

  private void InitAIOpponentSlot()
  {
    string text = CLocalisationManager.GetText("MP_AI_Name");
    this.opponentPlayerSlotUI.headerLabel.text = text;
    this.opponentPlayerSlotUI.diseaseNameInput.defaultText = text;
    this.opponentPlayerSlotUI.plagueDetailsUI.UpdatePlague(this.GetAIInsertedDiseaseType(), this.GetAIDiseaseName(), this.GetAIConvertedInsertedGenes(), 1);
    this.opponentPlayerSlotUI.avatar.gameObject.SetActive(true);
    this.opponentPlayerSlotUI.avatar.mainTexture = (Texture) this.opponentPlayerAIAvatar;
    this.opponentPlayerSlotUI.badgeAnimator.SetTrigger(this.isAIGame ? "Badge_Off" : "Badge_On");
  }

  public void ThrowConnectionErrorScreen()
  {
    IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
    screen.HideAllSubScreens();
    screen.ShowInitialSubScreen();
    (screen as CMainMenuScreen).OnNetworkConnectionError();
  }

  public void ThrowMismatchMultiplayerVersionScreen()
  {
    CUIManager.instance.ClearHistory();
    CGameManager.ClearGame();
    DynamicMusic.instance.FadeOut();
    CUIManager.instance.SetupScreens();
    IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
    List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
    igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
    CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
    igameSubScreenList.Clear();
    igameSubScreenList.Add(screen.GetSubScreen("MultiplayerLobby"));
    CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
    CUIManager.instance.redSingleButtonConfirmOverlay.ShowLocalised("FE_Multiplayer_PopUp_Network_Connection_Error_Title", "FE_Multiplayer_PopUp_Network_Connection_Error_Text", "OK");
    CNetworkManager.network.TerminateAndReinitialise();
  }

  private void ClickPracticeMatch()
  {
    CUIManager.instance.SaveBreadcrumbCurrent();
    this.mainLobbyAnimator.runtimeAnimatorController = this.gameType == IGame.GameType.VersusMP ? this.lobbyVersusController : (RuntimeAnimatorController) this.lobbyCoopPracticeAnimController;
    this.isAIGame = true;
    this.UpdateUIState(MultiplayerLobbyScreen.UIState.StartingPractice);
    this.ResetDifficultyToggles();
    this.UpdateDifficultyPlayerIndicator(0);
    this.UpdateDifficultyPlayerIndicator(1);
    this.InitAIOpponentSlot();
  }

  private void StartPracticeMatch()
  {
    this.LobbyDebug("StartPracticeMatch: " + (object) CGameManager.game);
    this.CreateGame();
    this.network.LobbyHandler.LeaveLobby();
    if (CGameManager.gameType == IGame.GameType.VersusMP)
    {
      MultiplayerGame game = CGameManager.game as MultiplayerGame;
      Analytics.Event("Start Verses Practice Match");
      string diseaseName = this.GetDiseaseName();
      int insertedDiseaseType1 = (int) this.GetInsertedDiseaseType();
      Gene[] convertedInsertedGenes1 = this.GetConvertedInsertedGenes();
      string aiDiseaseName = this.GetAIDiseaseName();
      int insertedDiseaseType2 = (int) this.GetAIInsertedDiseaseType();
      Gene[] convertedInsertedGenes2 = this.GetAIConvertedInsertedGenes();
      game.CreatePracticeGame(diseaseName, (Disease.EDiseaseType) insertedDiseaseType1, convertedInsertedGenes1, aiDiseaseName, (Disease.EDiseaseType) insertedDiseaseType2, convertedInsertedGenes2);
    }
    else
    {
      if (CGameManager.gameType != IGame.GameType.CoopMP)
        return;
      CooperativeGame game = CGameManager.game as CooperativeGame;
      Analytics.Event("Start Coop Practice Match");
      string diseaseName = this.GetDiseaseName();
      int insertedDiseaseType3 = (int) this.GetInsertedDiseaseType();
      Gene[] convertedInsertedGenes3 = this.GetConvertedInsertedGenes();
      int p1Difficulty = this.p1Difficulty;
      string aiDiseaseName = this.GetAIDiseaseName();
      int insertedDiseaseType4 = (int) this.GetAIInsertedDiseaseType();
      Gene[] convertedInsertedGenes4 = this.GetAIConvertedInsertedGenes();
      game.CreatePracticeGame(diseaseName, (Disease.EDiseaseType) insertedDiseaseType3, convertedInsertedGenes3, p1Difficulty, aiDiseaseName, (Disease.EDiseaseType) insertedDiseaseType4, convertedInsertedGenes4);
    }
  }

  private void PressGoBack()
  {
    CUIManager.instance.ClearHistory();
    CUIManager.instance.SaveBreadcrumb(this.gameScreen, new List<IGameSubScreen>()
    {
      this.gameScreen.GetSubScreen("Main_Sub_Main")
    });
    IGameSubScreen subScreen = this.gameScreen.GetSubScreen("Main_Sub_Multi");
    CUIManager.instance.SetSubScreen(subScreen);
  }

  public void PressQuickMatch()
  {
    CUIManager.instance.SaveBreadcrumbCurrent();
    this.mainLobbyAnimator.runtimeAnimatorController = this.gameType == IGame.GameType.VersusMP ? this.lobbyVersusController : (RuntimeAnimatorController) this.lobbyCoopQuickMatchAnimController;
    this.UpdateDifficulty(1);
    this.isAIGame = false;
    this.CreateGame();
    this.ndemicWaitTimeRetrievalCount = 0;
    this.RetrieveNdemicWaitTime();
    this.FindQuickMatch(true);
  }

  private void FindQuickMatch(bool isInitialSearch)
  {
    this.LobbyDebug("FindQuickMatch - isInitialSearch:" + isInitialSearch.ToString());
    if (!CNetworkManager.network.IsInternetAvailable)
    {
      this.nextRefreshTime = 9999999;
      this.ThrowConnectionErrorScreen();
    }
    else
    {
      this.lobbyParams.gameType = this.gameType;
      this.lobbyParams.hostPlayerID = CNetworkManager.network.LocalPlayerInfo.PlayerID;
      Debug.Log((object) ("---lobbyParams.hostPlayerID:" + this.lobbyParams.hostPlayerID));
      this.lobbyParams.friendMode = INetwork.FriendMode.Public;
      this.lobbyParams.playerRating = CNetworkManager.network.LocalPlayerInfo.playerStats.MP_HighestRating;
      this.LobbyDebug("---my playerRating:" + (object) this.lobbyParams.playerRating);
      if (isInitialSearch)
      {
        this.currentRatingDifferential = CGameManager.multiplayerLobbyRatingSearchStartBounds;
        this.startSearchingTimeStamp = DateTime.Now;
        this.nextRefreshTime = 0;
        if (this._recordFindLobbyTime != null)
          this.StopCoroutine(this._recordFindLobbyTime);
        this._recordFindLobbyTime = this.StartCoroutine(this.RecordFindLobbyTime());
        this.hasRefreshedConfig = false;
      }
      this.SetNextLobbyRefreshTime();
      this.startLastSearchTimeStamp = DateTime.Now;
      this.UpdateUIState(MultiplayerLobbyScreen.UIState.SearchingForPlayers);
      this.network.LobbyHandler.FindLobby(this.lobbyParams, this.currentRatingDifferential, Time.realtimeSinceStartup - this.gameFindStartTime, this.lastPlayerPlayed, !isInitialSearch);
    }
  }

  private IEnumerator RecordFindLobbyTime()
  {
    Analytics.Event("MP Attempt Find Lobby");
    this.gameFindStartTime = Time.realtimeSinceStartup;
    while (this.network.NetworkState == CNetworkManager.ENetworkState.SearchingLobbies)
      yield return (object) null;
    if (this.network.NetworkState == CNetworkManager.ENetworkState.JoiningLobby || this.network.NetworkState == CNetworkManager.ENetworkState.InLobby)
    {
      Analytics.Event("MP Wait Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.gameFindStartTime));
      Analytics.Event("MP Found Lobby", "Existing");
    }
    else
    {
      if (this.network.NetworkState == CNetworkManager.ENetworkState.CreatingLobby)
      {
        while (this.network.NetworkState == CNetworkManager.ENetworkState.CreatingLobby || this.network.NetworkState == CNetworkManager.ENetworkState.InLobby)
        {
          if (this.network.NumberOfConnectedPlayers > 1)
          {
            Analytics.Event("MP Wait Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.gameFindStartTime));
            Analytics.Event("MP Found Lobby", "Created");
            yield break;
          }
          else
            yield return (object) null;
        }
      }
      Analytics.Event("MP Find Fail", this.network.NetworkState.ToString());
    }
  }

  private void CancelFromLobby()
  {
    this.LobbyDebug(nameof (CancelFromLobby));
    NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
    Disease disease = CGameManager.localPlayerInfo.disease;
    CPlayerInfoSteam localPlayerInfo = CGameManager.localPlayerInfo as CPlayerInfoSteam;
    NdemicAnalytics.RecordTechAnalytics(this.gameType, 0, localPlayerInfo.PlayerID, localPlayerInfo.name, localPlayerInfo.GetMultiplayerRating(), false, -1, disease?.technologies, disease?.genes, disease != null ? disease.diseaseType : Disease.EDiseaseType.BACTERIA, disease != null ? disease.nexus.id : string.Empty);
    CNetworkManager.network.TerminateAndReinitialise();
    this.UpdateUIState(MultiplayerLobbyScreen.UIState.LobbyEntrance);
  }

  private void UpdatePlayerSummaryMode(MultiplayerLobbyScreen.PlayerSummaryMode newMode)
  {
    this.LobbyDebug("UpdatePlayerSummaryMode - newMode:" + (object) newMode);
    switch (newMode)
    {
      case MultiplayerLobbyScreen.PlayerSummaryMode.PlagueDetails:
        this.myPlayerSlotUI.slotAnimator.ResetTrigger("ShowStats");
        this.myPlayerSlotUI.slotAnimator.SetTrigger("ShowSummary");
        this.opponentPlayerSlotUI.slotAnimator.ResetTrigger("ShowStats");
        this.opponentPlayerSlotUI.slotAnimator.SetTrigger("ShowSummary");
        break;
      case MultiplayerLobbyScreen.PlayerSummaryMode.Stats:
        this.myPlayerSlotUI.slotAnimator.ResetTrigger("ShowSummary");
        this.myPlayerSlotUI.slotAnimator.SetTrigger("ShowStats");
        this.myPlayerSlotUI.statsUI.Populate(this.gameType, 0, this.myPlayer, this.availableDiseases, this.availableGenes);
        if (this.opponentPlayer != null)
        {
          this.opponentPlayerSlotUI.slotAnimator.ResetTrigger("ShowSummary");
          this.opponentPlayerSlotUI.slotAnimator.SetTrigger("ShowStats");
          this.opponentPlayerSlotUI.statsUI.Populate(this.gameType, 1, this.opponentPlayer, this.availableDiseases, this.availableGenes);
          break;
        }
        break;
    }
    this.currentPlayerSummaryMode = newMode;
  }

  private void EditPlague()
  {
    this.mainLobbyAnimator.SetTrigger("Lobby_Plague_Editor");
    this.plagueEditorUI.Show();
    this.gameModeStatusUI.p1StatusToggle.enabled = false;
    this.RefreshDifficultyVisibilities(this.currentUIState);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.ActionCloseEditPlague), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.ActionCloseEditPlague), this.gameObject);
  }

  public void ActionCloseEditPlague(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.FinishEditPlague();
  }

  private void FinishEditPlague()
  {
    if (!this.plagueEditorUI.gameObject.activeSelf)
      return;
    this.mainLobbyAnimator.SetTrigger("Lobby_Default");
    this.plagueEditorUI.Hide();
    this.RefreshMyPlayerPlagueSummary();
    PlayerPrefs.SetInt("MP_" + (this.gameType == IGame.GameType.CoopMP ? "Coop_" : "") + "Last_Disease_Selected", (int) this.GetInsertedDiseaseType());
    PlayerPrefs.SetString("MP_" + (this.gameType == IGame.GameType.CoopMP ? "Coop_" : "") + "Last_Gene_Selected_1", this.GetInsertedGenes()[0] != null ? this.GetInsertedGenes()[0].id : "");
    PlayerPrefs.SetString("MP_" + (this.gameType == IGame.GameType.CoopMP ? "Coop_" : "") + "Last_Gene_Selected_2", this.GetInsertedGenes()[1] != null ? this.GetInsertedGenes()[1].id : "");
    PlayerPrefs.SetString("MP_" + (this.gameType == IGame.GameType.CoopMP ? "Coop_" : "") + "Last_Gene_Selected_3", this.GetInsertedGenes()[2] != null ? this.GetInsertedGenes()[2].id : "");
    this.gameModeStatusUI.p1StatusToggle.enabled = true;
    this.RefreshDifficultyVisibilities(this.currentUIState);
    CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.ActionCloseEditPlague), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.ActionCloseEditPlague), this.gameObject);
    if (this.gameType != IGame.GameType.CoopMP)
      return;
    this.UpdateToggleAndDiseaseInfo();
  }

  private void InitUIHandlers()
  {
    Debug.Log((object) nameof (InitUIHandlers));
    EventDelegate.Set(this.gameModeStatusUI.backToMenuButton.onClick, (EventDelegate.Callback) (() => this.PressGoBack()));
    EventDelegate.Set(this.gameModeStatusUI.quickMatchButton.onClick, (EventDelegate.Callback) (() => this.PressQuickMatch()));
    EventDelegate.Set(this.gameModeStatusUI.inviteFriendButton.onClick, (EventDelegate.Callback) (() => this.InviteFriend()));
    EventDelegate.Set(this.gameModeStatusUI.cancelSearchButton.onClick, (EventDelegate.Callback) (() => this.CancelFromLobby()));
    EventDelegate.Set(this.gameModeStatusUI.quitGameButton.onClick, (EventDelegate.Callback) (() => this.CancelFromLobby()));
    EventDelegate.Set(this.gameModeStatusUI.sendInviteButton.onClick, (EventDelegate.Callback) (() => this.ShowInviteDialogue()));
    EventDelegate.Set(this.gameModeStatusUI.practiceMatchButton.onClick, (EventDelegate.Callback) (() => this.ClickPracticeMatch()));
    EventDelegate.Set(this.gameModeStatusUI.startPracticeGameButton.onClick, (EventDelegate.Callback) (() => this.StartPracticeMatch()));
    EventDelegate.Set(this.myPlayerSlotUI.editPlagueBtn.onClick, (EventDelegate.Callback) (() => this.EditPlague()));
    EventDelegate.Set(this.myPlayerSlotUI.diseaseNameInput.onSubmit, (EventDelegate.Callback) (() =>
    {
      if (UIInput.current.value.Trim().Length != 0)
        return;
      UIInput.current.value = UIInput.current.defaultText;
    }));
    EventDelegate.Add(this.myPlayerSlotUI.statsToggle.onChange, (EventDelegate.Callback) (() =>
    {
      if (this.isTogglingPlayerSummaryMode)
        return;
      this.UpdatePlayerSummaryMode(this.myPlayerSlotUI.statsToggle.value ? MultiplayerLobbyScreen.PlayerSummaryMode.Stats : MultiplayerLobbyScreen.PlayerSummaryMode.PlagueDetails);
      this.isTogglingPlayerSummaryMode = true;
      this.opponentPlayerSlotUI.statsToggle.value = this.myPlayerSlotUI.statsToggle.value;
      this.isTogglingPlayerSummaryMode = false;
    }));
    EventDelegate.Add(this.opponentPlayerSlotUI.statsToggle.onChange, (EventDelegate.Callback) (() =>
    {
      if (this.isTogglingPlayerSummaryMode)
        return;
      this.UpdatePlayerSummaryMode(this.opponentPlayerSlotUI.statsToggle.value ? MultiplayerLobbyScreen.PlayerSummaryMode.Stats : MultiplayerLobbyScreen.PlayerSummaryMode.PlagueDetails);
      this.isTogglingPlayerSummaryMode = true;
      this.myPlayerSlotUI.statsToggle.value = this.opponentPlayerSlotUI.statsToggle.value;
      this.isTogglingPlayerSummaryMode = false;
    }));
    EventDelegate.Add(this.gameModeStatusUI.difficultyToggles[0].mainToggle.onChange, (EventDelegate.Callback) (() =>
    {
      if (!this.gameModeStatusUI.difficultyToggles[0].mainToggle.value)
        return;
      this.UpdateDifficulty(this.gameModeStatusUI.difficultyToggles[0].difficulty);
    }));
    EventDelegate.Add(this.gameModeStatusUI.difficultyToggles[1].mainToggle.onChange, (EventDelegate.Callback) (() =>
    {
      if (!this.gameModeStatusUI.difficultyToggles[1].mainToggle.value)
        return;
      this.UpdateDifficulty(this.gameModeStatusUI.difficultyToggles[1].difficulty);
    }));
    EventDelegate.Add(this.gameModeStatusUI.difficultyToggles[2].mainToggle.onChange, (EventDelegate.Callback) (() =>
    {
      if (!this.gameModeStatusUI.difficultyToggles[2].mainToggle.value)
        return;
      this.UpdateDifficulty(this.gameModeStatusUI.difficultyToggles[2].difficulty);
    }));
    EventDelegate.Set(this.plagueEditorUI.acceptChangesBtn.onClick, (EventDelegate.Callback) (() => this.FinishEditPlague()));
    EventDelegate.Set(this.gameModeStatusUI.p1StatusToggle.onChange, (EventDelegate.Callback) (() =>
    {
      this.hasMeReadied = this.gameModeStatusUI.p1StatusToggle.value;
      Debug.Log((object) ("---CGameManager.game.networkView:" + (object) CGameManager.game.networkView));
      this.UpdateToggleAndDiseaseInfo();
    }));
  }

  private void UpdateToggleAndDiseaseInfo()
  {
    if (!((UnityEngine.Object) CGameManager.game.networkView != (UnityEngine.Object) null))
      return;
    CGameManager.game.networkView.RPC(NetworkChannel.Game, "RPCToggleReady", RPCTarget.Others, true, (object) this.gameModeStatusUI.p1StatusToggle.value, (object) this.GetDiseaseName(), (object) (int) this.GetInsertedDiseaseType(), (object) this.GetInsertedGeneIDs());
  }

  public void HandleOtherToggleReady(bool val, string oppName, int diseaseType, string[] geneIDs)
  {
    Debug.Log((object) ("HandleOtherToggleReady - geneIDs.Length:" + (object) geneIDs.Length));
    this.hasOppReadied = val;
    this.previewOpponentName = oppName;
    this.previewOpponentDisease = (Disease.EDiseaseType) diseaseType;
    this.previewOpponentGenes = new Gene[geneIDs.Length];
    for (int index1 = 0; index1 < geneIDs.Length; ++index1)
    {
      Debug.Log((object) ("geneIDs:" + geneIDs[index1]));
      for (int index2 = 0; index2 < this.availableGenes.Count; ++index2)
      {
        if (this.availableGenes[index2].id == geneIDs[index1])
          this.previewOpponentGenes[index1] = this.availableGenes[index2];
      }
    }
    if (this.gameType == IGame.GameType.CoopMP)
      this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(true);
    this.opponentPlayerSlotUI.plagueDetailsUI.UpdatePlague(this.previewOpponentDisease, this.previewOpponentName, this.previewOpponentGenes, 1);
  }

  private void RefreshDifficultyVisibilities(MultiplayerLobbyScreen.UIState nextState)
  {
    this.gameModeStatusUI.difficultyPresetRoot.SetActive(false);
    this.gameModeStatusUI.difficultyTogglesRoot.SetActive(false);
    this.gameModeStatusUI.difficultyStatusLabel.gameObject.SetActive(false);
    if (this.gameType != IGame.GameType.CoopMP || this.plagueEditorUI.gameObject.activeSelf)
      return;
    if (this.lobbyParams.friendMode == INetwork.FriendMode.Public && !this.isAIGame)
    {
      this.gameModeStatusUI.difficultyPresetRoot.SetActive(true);
    }
    else
    {
      this.gameModeStatusUI.difficultyTogglesRoot.SetActive(true);
      if (nextState == MultiplayerLobbyScreen.UIState.PlayersReadied || nextState == MultiplayerLobbyScreen.UIState.StartingGame)
      {
        foreach (LobbyDifficultyToggle difficultyToggle in this.gameModeStatusUI.difficultyToggles)
          difficultyToggle.mainToggle.enabled = false;
      }
      else
      {
        foreach (LobbyDifficultyToggle difficultyToggle in this.gameModeStatusUI.difficultyToggles)
          difficultyToggle.mainToggle.enabled = true;
        this.gameModeStatusUI.difficultyStatusLabel.gameObject.SetActive(true);
      }
    }
  }

  private void ResetDifficultyToggles()
  {
    for (int index = 0; index < this.gameModeStatusUI.difficultyToggles.Length; ++index)
      this.gameModeStatusUI.difficultyToggles[index].mainToggle.value = this.p1Difficulty == index;
  }

  private int GetDifficulty()
  {
    return this.isAIGame ? this.p1Difficulty : Mathf.Min(this.p1Difficulty, this.p2Difficulty);
  }

  private void UpdateDifficulty(int difficulty)
  {
    this.p1Difficulty = difficulty;
    Debug.Log((object) ("Update difficulty - difficulty:" + (object) difficulty));
    if (this.isAIGame)
      this.lobbyParams.gameDifficulty = this.p1Difficulty;
    else if ((UnityEngine.Object) CGameManager.game.networkView != (UnityEngine.Object) null)
      CGameManager.game.networkView.RPC(NetworkChannel.Game, "RPCSetDifficulty", RPCTarget.Others, (object) difficulty);
    this.UpdateDifficultyPlayerIndicator(0, this.p1Difficulty);
  }

  private void UpdateDifficultyPlayerIndicator(int playerId, int selectedToggleId = -1)
  {
    foreach (LobbyDifficultyToggle difficultyToggle in this.gameModeStatusUI.difficultyToggles)
    {
      if (playerId == 0)
        difficultyToggle.p1Indicator.SetActive(false);
      if (playerId == 1)
        difficultyToggle.p2Indicator.SetActive(false);
    }
    this.RefreshDifficultyText();
    if (this.isAIGame || selectedToggleId <= -1)
      return;
    if (playerId == 0)
      this.gameModeStatusUI.difficultyToggles[selectedToggleId].p1Indicator.SetActive(true);
    if (playerId != 1)
      return;
    this.gameModeStatusUI.difficultyToggles[selectedToggleId].p2Indicator.SetActive(true);
  }

  public void HandleOtherDifficulty(int difficulty)
  {
    this.p2Difficulty = difficulty;
    this.UpdateDifficultyPlayerIndicator(1, difficulty);
  }

  private void RefreshDifficultyText()
  {
    if (this.gameType == IGame.GameType.CoopMP && (this.lobbyParams.friendMode != INetwork.FriendMode.Public || this.isAIGame))
    {
      int key1 = Mathf.Min(this.p1Difficulty, this.p2Difficulty);
      int key2 = Mathf.Max(this.p1Difficulty, this.p2Difficulty);
      if (this.isAIGame)
        key1 = key2 = this.p1Difficulty;
      Debug.Log((object) ("--lowest:" + (object) key1 + ", highest:" + (object) key2));
      this.gameModeStatusUI.difficultyStatusLabel.text = CLocalisationManager.GetText(this.p1Difficulty == this.p2Difficulty || this.isAIGame ? "MP_CO_Difficulty_Select_Status_Match_True" : "MP_CO_Difficulty_Select_Status_Match_False").Replace("%difficulty1", CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) key1])).Replace("%difficulty2", CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) key2]));
    }
    else
      this.gameModeStatusUI.difficultyStatusLabel.text = "";
  }

  private void SetFixedDifficulty(int fixedDifficulty)
  {
    this.gameModeStatusUI.difficultyPresetIcon.spriteName = this.GetDifficultyIcon(fixedDifficulty);
    this.gameModeStatusUI.difficultyPresetLabel.text = CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) fixedDifficulty]);
  }

  private string GetDifficultyIcon(int difficulty)
  {
    string str = "MP_Icon_Difficulty_";
    switch (difficulty)
    {
      case 0:
        return str + "Casual";
      case 1:
        return str + "Normal";
      case 2:
        return str + "Brutal";
      default:
        return "";
    }
  }

  private string FormatTimespan(TimeSpan elapsed)
  {
    int minutes = elapsed.Minutes;
    int seconds = elapsed.Seconds;
    return minutes.ToString("D2") + ":" + seconds.ToString("D2");
  }

  private string FormatSecondsTimespan(TimeSpan elapsed) => elapsed.Seconds.ToString("D2");

  private string FormatSecondsTimespan(int seconds)
  {
    int num1 = Mathf.FloorToInt((float) seconds / 60f);
    int num2 = seconds % 60;
    return num1.ToString("D2") + ":" + num2.ToString("D2");
  }

  private IPlayerInfo GetGuestPlayerInfo()
  {
    return (CNetworkManager.network as CNetworkSteam).PlayerInfos.Count > 1 ? (CNetworkManager.network as CNetworkSteam).PlayerInfos[1] : (IPlayerInfo) null;
  }

  private string ExtractGeneSpriteName(Gene geneDat) => geneDat.geneGraphic.Replace("'", "") + "_0";

  private string ExtractGeneCategory(Gene.EGeneCategory geneCat)
  {
    return CLocalisationManager.GetText(CGameManager.GeneCategoryNames[geneCat]);
  }

  private string ExtractGeneName(Gene geneDat) => CLocalisationManager.GetText(geneDat.geneName);

  private string ExtractGeneDescription(Gene geneDat)
  {
    return CLocalisationManager.GetText(geneDat.geneDescription);
  }

  private Disease.EDiseaseType GetInsertedDiseaseType() => this.plagueEditorUI.insertedDiseaseType;

  private Disease.EDiseaseType GetAIInsertedDiseaseType() => Disease.EDiseaseType.BACTERIA;

  private Gene[] GetInsertedGenes() => this.plagueEditorUI.insertedGenes;

  private string[] GetInsertedGeneIDs()
  {
    string[] insertedGeneIds = new string[this.plagueEditorUI.insertedGenes.Length];
    for (int index = 0; index < insertedGeneIds.Length; ++index)
      insertedGeneIds[index] = this.plagueEditorUI.insertedGenes[index].id;
    return insertedGeneIds;
  }

  private Gene[] GetConvertedInsertedGenes() => this.plagueEditorUI.GetConvertedGenes();

  private Gene[] GetAIConvertedInsertedGenes()
  {
    return new Gene[3]
    {
      new Gene()
      {
        id = "_empty",
        geneName = "MP_Empty_Gene_Title",
        geneDescription = "MP_Empty_Gene_Description"
      },
      new Gene()
      {
        id = "_empty",
        geneName = "MP_Empty_Gene_Title",
        geneDescription = "MP_Empty_Gene_Description"
      },
      new Gene()
      {
        id = "_empty",
        geneName = "MP_Empty_Gene_Title",
        geneDescription = "MP_Empty_Gene_Description"
      }
    };
  }

  private string GetDiseaseName()
  {
    Debug.Log((object) ("---hostPlayerSlotUI.diseaseName.value:" + this.myPlayerSlotUI.diseaseNameInput.value + ", hostPlayerSlotUI.diseaseName.defaultText:" + this.myPlayerSlotUI.diseaseNameInput.defaultText));
    return !string.IsNullOrEmpty(this.myPlayerSlotUI.diseaseNameInput.value) && this.myPlayerSlotUI.diseaseNameInput.value != this.myPlayerSlotUI.diseaseNameInput.defaultText ? this.myPlayerSlotUI.diseaseNameInput.value : (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).name;
  }

  private string GetAIDiseaseName() => CLocalisationManager.GetText("FE_Plagues_Default_Name");

  private IEnumerator AutoFocusTextfieldCo()
  {
    yield return (object) null;
    this.myPlayerSlotUI.diseaseNameInput.isSelected = true;
  }

  private void UpdateUIState(MultiplayerLobbyScreen.UIState to)
  {
    Debug.Log((object) ("*****UpdateUIState - currentUIState:" + (object) this.currentUIState + ", to:" + (object) to + ", gameType:" + (object) this.gameType));
    this.gameModeStatusUI.gameStatusRoot.SetActive(false);
    this.gameModeStatusUI.COOPLabel.gameObject.SetActive(false);
    this.gameModeStatusUI.VSLabel.gameObject.SetActive(false);
    if (this.gameType == IGame.GameType.VersusMP)
    {
      this.gameModeStatusUI.VSLabel.gameObject.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_A.spriteName = "Icon_Lobby_Versus_A";
      this.gameModeStatusUI.gameModeIcon_B.spriteName = "Icon_Lobby_Versus_B";
    }
    else if (this.gameType == IGame.GameType.CoopMP)
    {
      this.gameModeStatusUI.COOPLabel.gameObject.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_A.spriteName = "Icon_Lobby_Coop_A";
      this.gameModeStatusUI.gameModeIcon_B.spriteName = "Icon_Lobby_Coop_B";
    }
    this.gameModeStatusUI.gameModeIcon_A.gameObject.SetActive(false);
    this.gameModeStatusUI.gameModeIcon_B.gameObject.SetActive(false);
    this.gameModeStatusUI.cancelSearchButton.gameObject.SetActive(false);
    this.gameModeStatusUI.quitGameButton.gameObject.SetActive(false);
    this.gameModeStatusUI.backToMenuButton.gameObject.SetActive(false);
    this.gameModeStatusUI.quickMatchButton.gameObject.SetActive(false);
    this.gameModeStatusUI.inviteFriendButton.gameObject.SetActive(false);
    this.gameModeStatusUI.practiceMatchButton.gameObject.SetActive(false);
    this.gameModeStatusUI.p1StatusToggle.gameObject.SetActive(false);
    this.gameModeStatusUI.startingGameRoot.gameObject.SetActive(false);
    this.gameModeStatusUI.sendInviteButton.gameObject.SetActive(false);
    this.gameModeStatusUI.startPracticeGameButton.gameObject.SetActive(false);
    this.gameModeStatusUI.gameStatusLabel.gameObject.SetActive(false);
    this.RefreshDifficultyVisibilities(to);
    this.activeGamesLabel.text = this.ndemicStatsActiveGames.ToString();
    this.opponentPlayerSlotUI.gameObject.SetActive(false);
    this.rankingsWidget.gameObject.SetActive(false);
    if (to == MultiplayerLobbyScreen.UIState.LobbyEntrance)
    {
      this.DestroyGame();
      this.FinishEditPlague();
      this.infoBarUI.gameObject.SetActive(false);
      this.gameModeStatusUI.gameObject.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_A.gameObject.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_B.gameObject.SetActive(true);
      this.gameModeStatusUI.quickMatchButton.gameObject.SetActive(true);
      this.gameModeStatusUI.inviteFriendButton.gameObject.SetActive(true);
      if (this.gameType == IGame.GameType.VersusMP)
        this.gameModeStatusUI.practiceMatchButton.gameObject.SetActive(true);
      this.gameModeStatusUI.backToMenuButton.gameObject.SetActive(true);
      this.myPlayerSlotUI.statsToggle.value = false;
      if (this.myPlayerSlotUI.gameObject.activeSelf)
        this.myPlayerSlotUI.gameObject.SetActive(false);
      this.rankingsWidget.gameObject.SetActive(true);
      this.rankingsWidget.Refresh(this.gameType, this.gameType == IGame.GameType.CoopMP ? 1 : 0);
    }
    else
    {
      this.infoBarUI.gameObject.SetActive(true);
      this.gameModeStatusUI.gameObject.SetActive(true);
      this.gameModeStatusUI.gameStatusRoot.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_A.gameObject.SetActive(true);
      this.gameModeStatusUI.gameModeIcon_B.gameObject.SetActive(true);
      this.gameModeStatusUI.gameStatusTimerLabel.gameObject.SetActive(false);
      this.gameModeStatusUI.gameStatusPlaceholder.SetActive(true);
      if (!this.myPlayerSlotUI.gameObject.activeSelf)
        this.myPlayerSlotUI.gameObject.SetActive(true);
      this.myPlayerSlotUI.editPlagueBtn.gameObject.SetActive(true);
      this.myPlayerSlotUI.diseaseNameInput.enabled = true;
      this.myPlayerSlotUI.diseaseNameBtn.isEnabled = true;
      if ((this.prevUIState == MultiplayerLobbyScreen.UIState.LobbyEntrance || this.prevUIState == MultiplayerLobbyScreen.UIState.Init) && !CSteamControllerManager.instance.controllerActive)
        this.StartCoroutine(this.AutoFocusTextfieldCo());
      this.myPlayerSlotUI.statsToggle.gameObject.SetActive(true);
      this.opponentPlayerSlotUI.gameObject.SetActive(true);
      this.opponentPlayerSlotUI.diseaseNameInput.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.badgeInfo.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.badgeSprite.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.headerLabel.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.avatar.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.preFriendInfo.SetActive(false);
      this.opponentPlayerSlotUI.editingInfo.SetActive(false);
      this.opponentPlayerSlotUI.readyInfo.SetActive(false);
      this.opponentPlayerSlotUI.searchingInfo.SetActive(false);
      this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.statsToggle.gameObject.SetActive(false);
      this.opponentPlayerSlotUI.avgWaitTimeLabel.gameObject.SetActive(false);
      switch (to)
      {
        case MultiplayerLobbyScreen.UIState.SearchingForPlayers:
        case MultiplayerLobbyScreen.UIState.WaitingForFriends:
        case MultiplayerLobbyScreen.UIState.ConnectingToFriend:
          this.gameModeStatusUI.cancelSearchButton.gameObject.SetActive(true);
          this.gameModeStatusUI.cancelSearchLabel.text = CLocalisationManager.GetText("MP_Cancel_Search");
          this.gameModeStatusUI.gameModeIcon_A.gameObject.SetActive(true);
          this.gameModeStatusUI.gameModeIcon_B.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.searchingTimeLabel.text = CLocalisationManager.GetText("MP_Finding_Match") + " 00:00";
          this.opponentPlayerSlotUI.searchingAnim.SetTrigger("MP_Search_Active");
          if (to == MultiplayerLobbyScreen.UIState.WaitingForFriends)
          {
            this.gameModeStatusUI.sendInviteButton.gameObject.SetActive(true);
            if (this.network.LobbyHandler.IsOwner)
            {
              this.gameModeStatusUI.sendInviteButton.isEnabled = true;
              this.gameModeStatusUI.sendInviteLoadingWidget.gameObject.SetActive(false);
            }
            else
            {
              this.gameModeStatusUI.sendInviteButton.isEnabled = false;
              this.gameModeStatusUI.sendInviteLoadingWidget.gameObject.SetActive(true);
            }
            this.opponentPlayerSlotUI.preFriendInfo.SetActive(!this.hasInvitedFriend);
            this.opponentPlayerSlotUI.searchingInfo.SetActive(this.hasInvitedFriend);
            this.opponentPlayerSlotUI.searchingTextLabel.text = CLocalisationManager.GetText("MP_P2_Slot_Status_Waiting_Friend");
            this.gameModeStatusUI.cancelSearchLabel.text = CLocalisationManager.GetText("Back");
            break;
          }
          if (to == MultiplayerLobbyScreen.UIState.ConnectingToFriend)
          {
            this.opponentPlayerSlotUI.searchingInfo.SetActive(true);
            this.gameModeStatusUI.gameStatusLabel.text = CLocalisationManager.GetText("Loading...");
            this.gameModeStatusUI.inviteFriendButton.gameObject.SetActive(false);
            this.gameModeStatusUI.cancelSearchButton.gameObject.SetActive(false);
            if (!this.network.IsServer)
            {
              this.gameModeStatusUI.difficultyTogglesRoot.SetActive(false);
              this.gameModeStatusUI.difficultyStatusLabel.text = "";
            }
            this.opponentPlayerSlotUI.searchingTextLabel.text = CLocalisationManager.GetText("MP_Friend_Joined_Game");
            break;
          }
          if (to == MultiplayerLobbyScreen.UIState.SearchingForPlayers)
          {
            this.opponentPlayerSlotUI.searchingInfo.SetActive(true);
            this.opponentPlayerSlotUI.headerLabel.text = "";
            this.opponentPlayerSlotUI.searchingTextLabel.text = "";
            this.opponentPlayerSlotUI.avgWaitTimeLabel.gameObject.SetActive(true);
            this.UpdateAvgWaitTime();
            this.gameModeStatusUI.gameStatusLabel.text = "";
            break;
          }
          break;
        case MultiplayerLobbyScreen.UIState.WaitingToStart:
        case MultiplayerLobbyScreen.UIState.PlayersReadied:
        case MultiplayerLobbyScreen.UIState.StartingGame:
          this.opponentPlayerSlotUI.badgeInfo.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.badgeSprite.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.headerLabel.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.statsToggle.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.avatar.gameObject.SetActive(true);
          if (to == MultiplayerLobbyScreen.UIState.WaitingToStart)
          {
            this.gameModeStatusUI.quitGameButton.gameObject.SetActive(true);
            this.gameModeStatusUI.p1StatusToggle.gameObject.SetActive(true);
            this.gameModeStatusUI.p1StatusToggle.value = false;
            if (this.gameType == IGame.GameType.CoopMP)
              this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(true);
            this.gameModeStatusUI.gameStatusLabel.text = "";
            this.myPlayerSlotUI.statsToggle.value = false;
            break;
          }
          if (to == MultiplayerLobbyScreen.UIState.PlayersReadied)
          {
            this.myPlayerSlotUI.diseaseNameInput.enabled = false;
            this.myPlayerSlotUI.diseaseNameBtn.isEnabled = false;
            this.myPlayerSlotUI.editPlagueBtn.gameObject.SetActive(false);
            this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(true);
            this.myPlayerSlotUI.statsToggle.value = false;
            this.myPlayerSlotUI.statsToggle.gameObject.SetActive(false);
            this.opponentPlayerSlotUI.statsToggle.gameObject.SetActive(false);
            this.gameModeStatusUI.gameStatusLabel.text = "";
            this.gameModeStatusUI.p1StatusToggle.gameObject.SetActive(false);
            this.gameModeStatusUI.startingGameRoot.gameObject.SetActive(true);
            this.gameModeStatusUI.startingGameLbl.text = CLocalisationManager.GetText("MP_Lobby_Starting_Game");
            break;
          }
          if (to == MultiplayerLobbyScreen.UIState.StartingGame)
          {
            this.myPlayerSlotUI.diseaseNameInput.enabled = false;
            this.myPlayerSlotUI.diseaseNameBtn.isEnabled = false;
            this.myPlayerSlotUI.editPlagueBtn.gameObject.SetActive(false);
            this.myPlayerSlotUI.statsToggle.gameObject.SetActive(false);
            this.opponentPlayerSlotUI.statsToggle.gameObject.SetActive(false);
            this.gameModeStatusUI.gameStatusLabel.text = "";
            this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(true);
            this.gameModeStatusUI.p1StatusToggle.gameObject.SetActive(false);
            this.gameModeStatusUI.startingGameRoot.gameObject.SetActive(true);
            this.gameModeStatusUI.startingGameLbl.text = CLocalisationManager.GetText("Loading...");
            break;
          }
          break;
        case MultiplayerLobbyScreen.UIState.StartingPractice:
          this.gameModeStatusUI.quitGameButton.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.diseaseNameInput.enabled = true;
          this.opponentPlayerSlotUI.badgeInfo.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.headerLabel.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.avatar.gameObject.SetActive(true);
          this.opponentPlayerSlotUI.plagueDetailsUI.gameObject.SetActive(true);
          this.gameModeStatusUI.startPracticeGameButton.gameObject.SetActive(true);
          this.gameModeStatusUI.gameStatusLabel.text = "";
          break;
      }
    }
    this.prevUIState = this.currentUIState;
    this.currentUIState = to;
  }

  private void InviteFriend()
  {
    Debug.Log((object) ("[" + (object) Time.realtimeSinceStartup + "] - InviteFriend()"));
    CUIManager.instance.SaveBreadcrumbCurrent();
    this.mainLobbyAnimator.runtimeAnimatorController = this.gameType == IGame.GameType.VersusMP ? this.lobbyVersusController : (RuntimeAnimatorController) this.lobbyCoopFriendsAnimController;
    this.isAIGame = false;
    this.CreateGame();
    this.LobbyDebug(nameof (InviteFriend));
    if (!CNetworkManager.network.IsInternetAvailable)
    {
      this.ThrowConnectionErrorScreen();
    }
    else
    {
      this.lobbyParams.gameType = this.gameType;
      this.lobbyParams.friendMode = INetwork.FriendMode.Friends;
      this.hasInvitedFriend = false;
      this.startSearchingTimeStamp = DateTime.Now;
      this.UpdateUIState(MultiplayerLobbyScreen.UIState.WaitingForFriends);
      this.ResetDifficultyToggles();
      this.UpdateDifficultyPlayerIndicator(0, this.p1Difficulty);
      this.UpdateDifficultyPlayerIndicator(1);
      (CNetworkManager.network as CNetworkSteam).LobbyHandler.CreateLobby(this.lobbyParams);
    }
  }

  private void ShowInviteDialogue()
  {
    this.LobbyDebug(nameof (ShowInviteDialogue));
    bool flag = false;
    this.LobbyDebug("---my steam id:" + (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).PlayerID);
    if (flag && (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).steamID.m_SteamID == 76561198247962578UL)
      SteamMatchmaking.InviteUserToLobby((CNetworkManager.network as CNetworkSteam).LobbyHandler.CurrentLobbyID, new CSteamID(76561198008614787UL));
    else if (flag && (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).steamID.m_SteamID == 76561198008614787UL)
      SteamMatchmaking.InviteUserToLobby((CNetworkManager.network as CNetworkSteam).LobbyHandler.CurrentLobbyID, new CSteamID(76561198247962578UL));
    else if (flag && (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).steamID.m_SteamID == 76561198104583779UL)
      SteamMatchmaking.InviteUserToLobby((CNetworkManager.network as CNetworkSteam).LobbyHandler.CurrentLobbyID, new CSteamID(76561198128467216UL));
    else if (flag && (CNetworkManager.network.LocalPlayerInfo as CPlayerInfoSteam).steamID.m_SteamID == 76561198128467216UL)
      SteamMatchmaking.InviteUserToLobby((CNetworkManager.network as CNetworkSteam).LobbyHandler.CurrentLobbyID, new CSteamID(76561198104583779UL));
    else
      SteamFriends.ActivateGameOverlayInviteDialog((CNetworkManager.network as CNetworkSteam).LobbyHandler.CurrentLobbyID);
    this.hasInvitedFriend = true;
    this.UpdateUIState(MultiplayerLobbyScreen.UIState.WaitingForFriends);
  }

  private IEnumerator DelayedFindCo(float time)
  {
    this.lastClickMark = time;
    yield return (object) new WaitForSeconds(time);
  }

  private IEnumerator GetSpecialPlayerList(
    MultiplayerLobbyScreen.SpecialPlayerType specialPlayerType)
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    MultiplayerLobbyScreen.annoyerURL = CGameManager.federalServerAddress + "annoyer.txt";
    MultiplayerLobbyScreen.cheaterURL = CGameManager.federalServerAddress + "cheater.txt";
    MultiplayerLobbyScreen.multiAccountURL = CGameManager.federalServerAddress + "multiaccount.txt";
    Debug.Log((object) ("Pending URL: " + MultiplayerLobbyScreen.annoyerURL));
    Debug.Log((object) ("Pending URL: " + MultiplayerLobbyScreen.cheaterURL));
    Debug.Log((object) ("Pending URL: " + MultiplayerLobbyScreen.multiAccountURL));
    if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.Annoyer)
      pendingRequest = UnityWebRequest.Get(MultiplayerLobbyScreen.annoyerURL);
    if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.Cheater)
      pendingRequest = UnityWebRequest.Get(MultiplayerLobbyScreen.cheaterURL);
    if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.MultiAccount)
      pendingRequest = UnityWebRequest.Get(MultiplayerLobbyScreen.multiAccountURL);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string text = pendingRequest.downloadHandler.text;
      if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.Annoyer)
      {
        this.playerAnnoyer = text;
        string[] strArray = text.Replace("\r\n", "\n").Replace("\n\r", "\n").Split('\n');
        List<string> stringList = new List<string>();
        foreach (string str in strArray)
          stringList.Add(str);
        CGameManager.annoyerList = stringList;
        Debug.Log((object) ("Annoyer list get: " + text));
        foreach (string annoyer in CGameManager.annoyerList)
          Debug.Log((object) ("Annoyer Added : " + annoyer));
      }
      if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.Cheater)
      {
        this.playerCheater = text;
        Debug.Log((object) ("Cheater list get: " + text));
      }
      if (specialPlayerType == MultiplayerLobbyScreen.SpecialPlayerType.MultiAccount)
      {
        this.playerMultiAccount = text;
        Debug.Log((object) ("MultiAccount list get: " + text));
      }
    }
    else
      Debug.LogError((object) ("Error player list: " + (object) pendingRequest));
  }

  public static bool IsCheater(string steamID)
  {
    return MultiplayerLobbyScreen.instance.playerCheater.Contains(steamID);
  }

  public static bool IsAnnoyer(string steamID)
  {
    return MultiplayerLobbyScreen.instance.playerAnnoyer.Contains(steamID);
  }

  public static bool IsMultiAccount(string steamID)
  {
    return MultiplayerLobbyScreen.instance.playerMultiAccount.Contains(steamID);
  }

  private enum UIState
  {
    Init,
    LobbyEntrance,
    SearchingForPlayers,
    WaitingForFriends,
    ConnectingToFriend,
    WaitingToStart,
    PlayersReadied,
    StartingGame,
    StartingPractice,
  }

  private enum PlayerSummaryMode
  {
    PlagueDetails,
    Stats,
  }

  private enum SpecialPlayerType
  {
    Cheater,
    Annoyer,
    MultiAccount,
  }
}
