// Decompiled with JetBrains decompiler
// Type: MultiplayerGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class MultiplayerGame : IGame
{
  public IPlayerInfo myPlayer;
  public IPlayerInfo opponentPlayer;
  public MPDisease myDisease;
  public MPDisease theirDisease;
  protected INetworkView netView;
  private CNetworkSteam network;
  public Dictionary<string, GemAbility> gemAbilities;
  private int nexusPickAttempts;
  private bool isRepickingAllowed;
  private bool isRandomingBannedCountriesAllowed;
  private float nexusPickTimer;
  private const float NEXUS_PICK_TIME = 60f;
  private bool requestedRandom;
  internal bool isFirstRound = true;
  internal bool hasTimerStarted;
  private List<string> dissalowedCountries;
  private Dictionary<IPlayerInfo, string> currentCountryPick;
  public bool WantRandomGame;
  private bool forcedRandomPick;
  private bool havePlayersSelectedSameCountry;
  private Dictionary<IPlayerInfo, string[]> geneStore;
  private bool isClientGameSessionReady;
  private bool isServerGameSessionReady;
  private string createGameDiseaseName;
  private Disease.EDiseaseType createGameDiseaseType;
  private Gene[] createGameGenes;
  private float lastTimerResumeMark;
  private int resumeSpeedCountdownTick;
  public bool IsAIGame;
  private MPAIController aiController;
  private const float PING_DELAY = 5f;
  private Coroutine pingCoroutine;
  private const float ANALYTICS_DELAY = 10f;
  private Coroutine analyticsCoroutine;
  private const int CLIENT_TURN_LAG_MAX = 5;
  private IDictionary<string, bool> requestedEvolutions = (IDictionary<string, bool>) new Dictionary<string, bool>();
  private IDictionary<string, bool> requestedDeEvolutions = (IDictionary<string, bool>) new Dictionary<string, bool>();
  private int MAX_GENES = 3;
  private CConfirmOverlay smallDialogue;
  public bool isInDisconnectResponse;
  public bool isInDesyncResponse;
  public bool isGameOutOfSync;
  public bool gameEndedIncomplete;
  public int lastClientTurn;
  private const int FIRST_INFECTION_BONUS = 1;
  private const int FIRST_TOTAL_INFECTED_BONUS = 1;
  private const int MOST_INFECTED_BONUS = 1;
  private const int MOST_DEAD_BONUS = 2;
  private DateTime lastPing;
  private DateTime currentPing;
  public double ping;
  public double averagePing;
  private const int PING_SAMPLE_COUNT = 1;
  private int pingSamples;
  private double weightedPing;

  public override Disease GetMyDisease() => (Disease) this.myDisease;

  public override Disease GetTheirDisease() => (Disease) this.theirDisease;

  public bool IsSPContinue { get; set; }

  public override int NumberOfPlayers => this.network.NumberOfConnectedPlayers;

  public int GetNetworkViewID() => (int) this.netView.viewID;

  public override INetworkView networkView => this.netView;

  public bool IsAlowedToPickNexus => !this.HasRegisteredInterest(this.network.LocalPlayerInfo);

  public override World CreateWorld() => (World) new MPWorld();

  public override void Initialise()
  {
    base.Initialise();
    Application.runInBackground = true;
    this.network = CNetworkManager.network as CNetworkSteam;
    this.isClientGameSessionReady = false;
    this.nexusPickAttempts = 0;
    this.dissalowedCountries = new List<string>();
    this.currentCountryPick = new Dictionary<IPlayerInfo, string>();
    this.isFirstRound = true;
    this.nexusPickTimer = 60f;
    this.geneStore = new Dictionary<IPlayerInfo, string[]>();
    this.IsAIGame = false;
    this.isInDisconnectResponse = false;
    this.isInDesyncResponse = false;
    this.isGameOutOfSync = false;
    this.gameEndedIncomplete = false;
    CCountrySelect.instance.mpPopupSubscreen.SetActive(false);
    this.lastClientTurn = -1;
    this.ping = 0.0;
    this.averagePing = 0.0;
    this.pingSamples = 0;
    this.weightedPing = 0.0;
    if (!(bool) (UnityEngine.Object) this.netView)
      this.netView = this.gameObject.AddComponent<INetworkView>();
    this.netView.viewID = (byte) 2;
    this.network.UnregisterNetworkView(this.netView);
    this.network.RegisterNetworkView(this.netView);
  }

  public override bool ConnectGameSession()
  {
    MultiplayerLobbyScreen.instance.LobbyDebug("Game.ConnectGameSession - network.IsServer:" + this.network.IsServer.ToString());
    if (!this.network.IsServer)
      return false;
    this.network.LobbyHandler.SetCurrentLobbyScope(ELobbyType.k_ELobbyTypePrivate);
    this.network.ConnectGameSession();
    return true;
  }

  public override void CreateMPGameSession(
    string diseaseName,
    Disease.EDiseaseType diseaseType,
    Gene[] genes,
    int difficulty)
  {
    Debug.Log((object) ("MPGame.CreateGameSession - diseaseName:" + diseaseName + ", diseaseType:" + (object) diseaseType + ", genes:" + (object) genes.Length));
    this.IsAIGame = false;
    this.createGameDiseaseName = diseaseName;
    this.createGameDiseaseType = diseaseType;
    this.createGameGenes = genes;
    if (this.network.IsServer)
    {
      this.isServerGameSessionReady = true;
      this.CheckCreateGameSession();
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCCreateGameSessionReady", RPCTarget.Host, true);
  }

  private void CheckCreateGameSession()
  {
    if (!this.network.IsServer || !this.isServerGameSessionReady || !this.isClientGameSessionReady)
      return;
    this.CreateGame((Scenario) null, Disease.EDiseaseType.BACTERIA);
    this.netView.RPC(NetworkChannel.Game, "RPCCreateGameSession", RPCTarget.Others, true);
  }

  public void FinaliseGameSession()
  {
    CInterfaceManager.instance.SetupOnlineGame();
    CInterfaceManager.instance.SetPortRendererState(true);
    CGameManager.game.Difficulty = 1U;
    this.ChooseDisease(this.createGameDiseaseName, this.createGameDiseaseType);
    this.ChooseGenes(this.createGameGenes);
    CInterfaceManager.instance.SetDiseaseCreationProgress(1f);
    DynamicMusic.instance.FadeOut();
    CInterfaceManager.instance.InitialiseCountryViews();
    this.requestedEvolutions.Clear();
    this.requestedDeEvolutions.Clear();
    if (!CNetworkManager.network.IsServer)
      return;
    CUIManager.instance.SetActiveScreen("StartCountryScreen");
  }

  public void CreatePracticeGame(
    string diseaseName,
    Disease.EDiseaseType diseaseType,
    Gene[] genes,
    string aiDiseaseName,
    Disease.EDiseaseType aiDiseaseType,
    Gene[] aiGenes)
  {
    CNetworkManager.network.SetIsOfflineGame(true);
    this.IsAIGame = true;
    this.CreateGame((Scenario) null, Disease.EDiseaseType.BACTERIA);
    CInterfaceManager.instance.SetupOnlineGame();
    CInterfaceManager.instance.SetPortRendererState(true);
    CGameManager.game.Difficulty = 1U;
    this.ChooseDisease(diseaseName, diseaseType);
    this.ChooseGenes(genes);
    this.AddDisease(aiDiseaseName, aiDiseaseType, (IPlayerInfo) null, 0);
    CInterfaceManager.instance.SetDiseaseCreationProgress(1f);
    DynamicMusic.instance.FadeOut();
    CInterfaceManager.instance.InitialiseCountryViews();
    CUIManager.instance.SetActiveScreen("StartCountryScreen");
    CNetworkManager.network.NetworkState = CNetworkManager.ENetworkState.InGame;
  }

  public override void CreateGame(Scenario withScenario = null, Disease.EDiseaseType overrideDiseaseType = Disease.EDiseaseType.BACTERIA)
  {
    base.CreateGame(withScenario, overrideDiseaseType);
    this.myPlayer = this.network.LocalPlayerInfo;
    this.opponentPlayer = this.network.OpponentPlayerInfo;
    this.myPlayer.gameSpeed = 1;
    if (this.opponentPlayer != null)
      this.opponentPlayer.gameSpeed = 1;
    MultiplayerEventsPopup.instance.ClearEvents();
    this.gemAbilities = DataImporter.ImportGemAbilities(CGameManager.LoadGameText("Game/gems"));
    this.SetupParameters.allowedDiseaseTypes = new HashSet<Disease.EDiseaseType>();
    this.SetupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.BACTERIA);
    this.SetupParameters.defaultDiseaseType = Disease.EDiseaseType.BACTERIA;
    this.SetupParameters.lockDiseaseType = true;
    this.SetupParameters.difficulty = "normal";
    this.SetupParameters.lockDifficulty = true;
    this.SetupParameters.lockAllGenes = false;
    this.SetupParameters.lockAllCheats = true;
    this.SetupParameters.defaultName = CGameManager.localPlayerInfo.name;
    this.Difficulty = 1U;
    this.cheatsUsed = new Disease.ECheatType[0];
    this.SetupParameters.fixedStartCountry = false;
    this.SetupParameters.startCountryID = "";
  }

  public override void UpdateExtraPopups(bool enabled)
  {
    Debug.LogError((object) "Cannot enable extra popups in a multiplayer game.");
  }

  public override void ChooseGenes(Gene[] genes)
  {
    IPlayerInfo localPlayerInfo = this.network.LocalPlayerInfo;
    string[] collection = new string[genes.Length];
    string str = "";
    for (int index = 0; index < collection.Length && index < this.MAX_GENES; ++index)
    {
      collection[index] = genes[index].id;
      str = str + genes[index].id + ",";
    }
    if (this.network.IsServer)
      Debug.Log((object) ("-*-*-* GENES FOR " + localPlayerInfo.name + " - " + (object) collection.Length + " = " + str));
    else
      Debug.Log((object) ("-*-*-* RPC REQUEST GENES FOR " + localPlayerInfo.name + " - " + (object) collection.Length + " = " + str));
    string[] array = new HashSet<string>((IEnumerable<string>) collection).ToArray<string>();
    this.geneStore.Add(localPlayerInfo, array);
    this.netView.RPC(NetworkChannel.Game, "RPCChooseGenes", RPCTarget.Others, true, (object) array, (object) localPlayerInfo);
    this.WantRandomGame = false;
    if (genes == null)
      return;
    for (int index = 0; index < genes.Length; ++index)
    {
      if (genes[index].randomNexusFlag != 0)
        this.WantRandomGame = true;
    }
  }

  public override void ChooseCheats(Disease.ECheatType[] cheats)
  {
    if (!this.network.IsServer)
      return;
    base.ChooseCheats(cheats);
  }

  public override void StartGame()
  {
    this.myDisease = (MPDisease) CNetworkManager.network.LocalPlayerInfo.disease;
    this.theirDisease = this.myDisease == World.instance.GetDisease(0) ? (MPDisease) World.instance.GetDisease(1) : (MPDisease) World.instance.GetDisease(0);
    if (this.opponentPlayer != null && this.opponentPlayer.playerStats != null && this.opponentPlayer.playerStats.MP_Rating == 0)
      Debug.LogError((object) ("**** NO RATING GAME START, Host: " + this.network.IsServer.ToString() + ", Opponent: " + this.opponentPlayer.PlayerID + " STATS LOADED: " + this.opponentPlayer.statsLoaded.ToString()));
    if (this.IsReplayActive)
      base.StartGame();
    else if (this.IsAIGame)
    {
      base.StartGame();
    }
    else
    {
      if (!this.network.IsServer)
        return;
      Debug.Log((object) "MP START GAME");
      Analytics.Event("MP HOST Disease Type", CGameManager.localPlayerInfo.disease.diseaseType.ToString());
      Analytics.Event("MP HOST Game Difficulty", CGameManager.localPlayerInfo.disease.difficulty.ToString());
      if (this.CurrentLoadedScenario != null)
        Analytics.Event("MP HOST Scenario", this.CurrentLoadedScenario.scenarioInformation.id);
      base.StartGame();
    }
  }

  public override void DoStartGame(int seed)
  {
    Debug.Log((object) ("MpGame.DoStartGame - network.IsServer:" + this.network.IsServer.ToString() + ", seed:" + (object) seed));
    if (!this.IsReplayActive && this.network.IsServer)
    {
      foreach (IPlayerInfo playerInfo in this.network.PlayerInfos)
      {
        string[] geneIDs = this.geneStore[playerInfo];
        this.netView.RPC(NetworkChannel.Game, "RPCSetGenes", RPCTarget.Others, (object) geneIDs, (object) playerInfo);
        this.RPCSetGenes(geneIDs, playerInfo);
      }
      this.netView.RPC(NetworkChannel.Game, "RPCStartGame", RPCTarget.Others, true, (object) seed, (object) BonusIcon.GetCounter(), (object) Vehicle.GetCounter());
    }
    if (!this.IsReplayActive)
      this.replayData.SetGeneStore((IDictionary<IPlayerInfo, string[]>) this.geneStore);
    if (this.pingCoroutine != null)
      this.StopCoroutine(this.pingCoroutine);
    this.pingCoroutine = this.StartCoroutine(this.SendPing());
    if (this.analyticsCoroutine != null)
      this.StopCoroutine(this.analyticsCoroutine);
    this.analyticsCoroutine = this.StartCoroutine(this.SendAnalytics());
    base.DoStartGame(seed);
    if (!this.IsReplayActive && this.network.IsServer)
    {
      foreach (MPDisease disease in this.world.diseases)
      {
        if (disease.preload != null)
        {
          foreach (Technology technology in disease.preload)
            this.PreloadEvolveTech(disease, technology);
          disease.preload = (List<Technology>) null;
        }
      }
    }
    GeneticDominanceBar geneticDominanceBar = CHUDScreen.instance.geneticDominanceBar;
    if (CInterfaceManager.instance.localPlayerDisease == this.world.diseases[0])
      geneticDominanceBar.SetNames(this.world.diseases[0].name, this.world.diseases[1].name);
    else
      geneticDominanceBar.SetNames(this.world.diseases[1].name, this.world.diseases[0].name);
    if (!this.IsReplayActive)
    {
      this.myDisease.randomStartLocation = this.forcedRandomPick;
      this.theirDisease.randomStartLocation = this.forcedRandomPick;
      CGameManager.GameSpeedChanged(this.actualSpeed, mySpeed: this.actualSpeed, oppSpeed: this.actualSpeed);
    }
    if (!this.IsReplayActive)
    {
      if (this.network.IsServer)
        CGameManager.localPlayerInfo.UpdateIntStat("MP_GameInstance", 1);
      if (this.opponentPlayer != null && !this.IsAIGame)
      {
        CGameManager.localPlayerInfo.RegisterMultiplayerGameStart(this.myDisease, this.opponentPlayer.playerStats.MP_Rating, this.gameParameters.friendMode == INetwork.FriendMode.Public);
        if (this.opponentPlayer.playerStats.MP_Rating == 0)
          Debug.LogError((object) ("**** NO RATING GAME START, Host: " + this.network.IsServer.ToString() + ", Opponent: " + this.opponentPlayer.PlayerID + " STATS LOADED: " + this.opponentPlayer.statsLoaded.ToString()));
      }
    }
    CWorldScreenMP.InstanceMP.OnGameStarted();
    CDiseaseScreen.instance.OnGameStarted();
    MultiplayerEventsPopup.instance.OnGameStarted();
  }

  public override bool ChooseCountry(string countryID, bool isTimeout = false)
  {
    Debug.Log((object) ("MultiplayerGame.ChooseCountry - countryID:" + countryID + ", network.IsServer:" + this.network.IsServer.ToString() + ", isTimeout:" + isTimeout.ToString()));
    this.myDisease = (MPDisease) CNetworkManager.network.LocalPlayerInfo.disease;
    this.theirDisease = this.myDisease == World.instance.GetDisease(0) ? (MPDisease) World.instance.GetDisease(1) : (MPDisease) World.instance.GetDisease(0);
    if (isTimeout && !this.HasRegisteredInterest(this.network.LocalPlayerInfo))
      this.myDisease.timeoutRandomPick = true;
    if (this.network.IsServer)
    {
      if (!this.HasRegisteredInterest(this.network.LocalPlayerInfo))
      {
        if (this.CanPickCountry(countryID))
        {
          if (!this.isFirstRound)
            this.netView.RPC(NetworkChannel.Game, "RPCStartNexusTimer", RPCTarget.All, true);
          this.RegisterInterest(countryID, this.network.LocalPlayerInfo);
          this.CheckNexusPickRules(this.network.LocalPlayerInfo);
          return true;
        }
        this.DisplayNetworkPopup(MultiplayerGame.MultiplayerMessage.PickedBanned);
        return false;
      }
      this.DisplayNetworkPopup(MultiplayerGame.MultiplayerMessage.AlreadyPicked);
      return false;
    }
    if (countryID != "random" && countryID != "notRandom")
      this.RegisterInterest(countryID, this.network.LocalPlayerInfo);
    this.netView.RPC(NetworkChannel.Game, "RPCRegisterCountryInterest", RPCTarget.Host, true, (object) this.network.LocalPlayerInfo, (object) countryID);
    return true;
  }

  private IEnumerator CheapHackDisconnectTimeout()
  {
    yield return (object) new WaitForSeconds(2f);
    CNetworkManager.network.TerminateAndReinitialise();
  }

  public override void EndGame(IGame.EndGameReason reason)
  {
    if (this.IsReplayActive)
      return;
    MultiplayerEventsPopup.instance.ClosePopup();
    if (World.instance == null)
    {
      this.ConfirmEndGame(reason, false);
    }
    else
    {
      Debug.Log((object) ("TRYING TO ENDING GAME: " + (object) reason));
      if (this.gameState == IGame.GameState.EndGame)
        return;
      this.gameState = IGame.GameState.EndGame;
      Debug.Log((object) ("ACTUALLY ENDING GAME: " + (object) reason));
      if (this.pingCoroutine != null)
        this.StopCoroutine(this.pingCoroutine);
      if (this.analyticsCoroutine != null)
        this.StopCoroutine(this.analyticsCoroutine);
      bool validForStats = true;
      if (this.IsAIGame)
        validForStats = false;
      if (CNetworkManager.network.IsServer)
        this.netView.RPC(NetworkChannel.Game, "RPCEndGame", RPCTarget.Others, true);
      IPlayerInfo localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
      CPlayerInfoSteam cplayerInfoSteam = localPlayerInfo as CPlayerInfoSteam;
      switch (reason)
      {
        case IGame.EndGameReason.COMPLETE:
          NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Finish);
          break;
        case IGame.EndGameReason.OPPONENT_LEFT:
          this.world.gameWon = true;
          this.world.gameEnded = true;
          this.world.gameEndResult = IGame.EndGameResult.Resigned;
          this.world.winner = localPlayerInfo.disease;
          this.isInDisconnectResponse = true;
          validForStats = this.world.DiseaseTurn >= 5;
          NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
          NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) this.Difficulty, cplayerInfoSteam.PlayerID, cplayerInfoSteam.name, cplayerInfoSteam.GetMultiplayerRating(), true, -1, this.world.winner.technologies, this.world.winner.genes, this.world.winner.diseaseType, this.world.winner.nexus != null ? this.world.winner.nexus.id : "");
          Analytics.Event("MP Disconnect Game Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted).ToString());
          break;
        case IGame.EndGameReason.LOST_CONNECTION:
          World.instance.gameEndResult = IGame.EndGameResult.Disconnected;
          this.StartCoroutine(this.PingOnlineness(reason));
          return;
        case IGame.EndGameReason.DESYNC:
          this.world.gameWon = false;
          this.world.gameEnded = true;
          this.world.gameEndResult = IGame.EndGameResult.Disconnected;
          this.world.winner = (Disease) null;
          this.isInDesyncResponse = true;
          validForStats = false;
          this.smallDialogue = CUIManager.instance.standardConfirmOverlay;
          this.smallDialogue.ShowLocalised("MP_Only_Desync_Title", "MP_Only_Desync_Text", buttonB: "OK", pressB: (CConfirmOverlay.PressDelegate) (() => this.ConfirmEndGame(reason, validForStats)), isEscapeable: false);
          NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
          Disease disease = localPlayerInfo.disease;
          NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) this.Difficulty, cplayerInfoSteam.PlayerID, cplayerInfoSteam.name, cplayerInfoSteam.GetMultiplayerRating(), false, -1, disease?.technologies, disease?.genes, disease != null ? disease.diseaseType : Disease.EDiseaseType.BACTERIA, disease != null ? disease.nexus.id : string.Empty);
          Analytics.Event("MP Desync Game Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted).ToString());
          return;
        case IGame.EndGameReason.CHEATING:
          this.world.gameWon = true;
          this.world.gameEnded = true;
          this.world.gameEndResult = IGame.EndGameResult.Resigned;
          this.world.winner = localPlayerInfo.disease;
          this.isInDisconnectResponse = true;
          validForStats = this.world.DiseaseTurn >= 5;
          NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
          NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) this.Difficulty, cplayerInfoSteam.PlayerID, cplayerInfoSteam.name, cplayerInfoSteam.GetMultiplayerRating(), true, -1, this.world.winner.technologies, this.world.winner.genes, this.world.winner.diseaseType, this.world.winner.nexus != null ? this.world.winner.nexus.id : "");
          Analytics.Event("MP Disconnect Game Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted).ToString());
          break;
      }
      this.ConfirmEndGame(reason, validForStats);
    }
  }

  private IEnumerator PingOnlineness(IGame.EndGameReason reason)
  {
    MultiplayerGame multiplayerGame = this;
    multiplayerGame.smallDialogue = CUIManager.instance.standardConfirmOverlay;
    multiplayerGame.smallDialogue.ShowLocalised("MP_Checking_Disconnect_Title", "MP_Checking_Disconnect_Text", isEscapeable: false);
    yield return (object) new WaitForSeconds(5f);
    bool isEndGamePingCheckOnline = false;
    UnityWebRequest www = UnityWebRequest.Get(CGameManager.federalServerAddress + "MultiplayerConfig/multiplayerConfig.txt" + "?rnd=" + (object) UnityEngine.Random.Range(0, 9999999));
    yield return (object) www.SendWebRequest();
    if (www.isDone && string.IsNullOrEmpty(www.error))
      isEndGamePingCheckOnline = true;
    if (isEndGamePingCheckOnline)
      Debug.Log((object) "Successfully connected to Federal Anti-cheat Server, your opponent must have made a disconnection on purpose, don't forget to report them!");
    else
      Debug.Log((object) "Failed to connect to Federal Anti-cheat Server, you must have met with a bad connection!");
    Debug.Log((object) ("isEndGamePingCheckOnline:" + isEndGamePingCheckOnline.ToString()));
    multiplayerGame.isInDisconnectResponse = true;
    bool validForStats = multiplayerGame.world.DiseaseTurn >= 50;
    Debug.Log((object) ("NETWORK: " + multiplayerGame.network.IsInternetAvailable.ToString() + " opponent: " + (object) multiplayerGame.opponentPlayer));
    CUIManager.instance.HideOverlay((CGameOverlay) multiplayerGame.smallDialogue);
    if (multiplayerGame.world.DiseaseTurn < 50)
    {
      World.instance.gameEnded = true;
      World.instance.gameWon = false;
      World.instance.winner = (Disease) null;
    }
    else if (!isEndGamePingCheckOnline)
    {
      multiplayerGame.world.gameWon = true;
      multiplayerGame.world.gameEnded = true;
      multiplayerGame.world.winner = multiplayerGame.opponentPlayer.disease;
    }
    else
    {
      multiplayerGame.world.gameWon = true;
      multiplayerGame.world.gameEnded = true;
      multiplayerGame.world.winner = CGameManager.localPlayerInfo.disease;
    }
    NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.Disconnect);
    Disease disease = CGameManager.localPlayerInfo.disease;
    CPlayerInfoSteam localPlayerInfo = CGameManager.localPlayerInfo as CPlayerInfoSteam;
    NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) multiplayerGame.Difficulty, localPlayerInfo.PlayerID, localPlayerInfo.name, localPlayerInfo.GetMultiplayerRating(), true, -1, disease?.technologies, disease?.genes, disease != null ? disease.diseaseType : Disease.EDiseaseType.BACTERIA, disease != null ? disease.nexus.id : string.Empty);
    Analytics.Event("MP Disconnect Game Time", Mathf.FloorToInt(Time.realtimeSinceStartup - multiplayerGame.timeGameSessionStarted).ToString());
    multiplayerGame.ConfirmEndGame(reason, validForStats);
  }

  private void ConfirmEndGame(IGame.EndGameReason reason, bool validForStats)
  {
    this.endGameReason = reason;
    if (!validForStats && !this.IsAIGame)
      (CNetworkManager.network as CNetworkSteam).LocalPlayerInfo.ProcessInvalidGame(this.gameParameters.friendMode == INetwork.FriendMode.Public);
    bool didWin = false;
    Disease diseaseWon = (Disease) null;
    IGame.EndGameResult resultType = IGame.EndGameResult.Disconnected;
    if (validForStats || this.IsAIGame)
    {
      didWin = this.world.winner == CGameManager.localPlayerInfo.disease;
      diseaseWon = this.world.winner;
      resultType = World.instance.gameEndResult;
    }
    CUIManager.Unlock unlocked = new CUIManager.Unlock();
    if (validForStats)
    {
      CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_MP_GeneticChallenger));
      if (this.world.winner == CGameManager.localPlayerInfo.disease)
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_MP_GeneticDomination));
      int num1 = this.opponentPlayer.GetAchievement(EAchievement.A_MP_BetaInfection) ? 1 : 0;
      bool achievement = this.opponentPlayer.GetAchievement(EAchievement.A_MP_NdemicInfection);
      if (num1 != 0)
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_MP_BetaInfection));
      if (achievement)
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_MP_NdemicInfection));
      IPlayerInfo player = CGameManager.localPlayerInfo;
      player.playerStats.MP_VS_Lifetime_Infected += player.disease.accumulatedInfections;
      player.playerStats.MP_VS_Lifetime_Killed += (float) player.disease.totalKilled;
      player.playerStats.MP_VS_Lifetime_Cured += player.disease.accumulatedCures;
      player.playerStats.MP_VS_Lifetime_DNA_Spent += (float) player.disease.evoPointsSpent;
      player.playerStats.MP_VS_Lifetime_Bubbles_Popped += player.disease.bubblesPopped;
      FieldInfo[] fields = typeof (PlayerStats).GetFields();
      int num2 = 2 + player.disease.genes.Count;
      foreach (FieldInfo fieldInfo in fields)
      {
        if (num2 != 0)
        {
          if (fieldInfo.Name == "MP_VS_Start_Country_" + player.disease.nexus.id)
          {
            fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
            --num2;
          }
          else if (fieldInfo.Name == "MP_VS_DISEASE_" + player.disease.diseaseType.ToString())
          {
            fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
            --num2;
          }
          else
          {
            foreach (Gene gene in player.disease.genes)
            {
              if (fieldInfo.Name == "MP_VS_GENE_" + gene.id)
              {
                fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
                --num2;
              }
            }
          }
        }
        else
          break;
      }
      List<Gene> all = this.availableGenes.FindAll((Predicate<Gene>) (a => !player.GetGeneUnlocked(a)));
      if (all.Count > 0)
      {
        Gene gene = all[UnityEngine.Random.Range(0, all.Count)];
        player.SetGeneUnlocked(gene);
        unlocked.gene = gene;
      }
      int a1 = Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted);
      Debug.Log((object) ("*************gameParameters.friendMode:" + (object) this.gameParameters.friendMode));
      if (this.world.winner == player.disease)
      {
        ++player.playerStats.MP_VS_Lifetime_Games_Won;
        player.playerStats.MP_VS_Quickest_Win = player.playerStats.MP_VS_Quickest_Win > 0 ? Mathf.Min(a1, player.playerStats.MP_VS_Quickest_Win) : a1;
        (CNetworkManager.network as CNetworkSteam).LocalPlayerInfo.ProcessMultiplayerGameResult(CGameManager.localPlayerInfo.disease, MultiplayerGame.ResultType.WIN, this.opponentPlayer.playerStats.MP_Rating, this.gameParameters.friendMode == INetwork.FriendMode.Public);
        Analytics.Event("MP Win Game Time", a1.ToString());
        Analytics.Event("MP Win Game Turns", this.world.DiseaseTurn.ToString());
        Analytics.Event("MP Win Disease Type", this.world.diseases[0].diseaseType.ToString());
        Analytics.Event("MP Win Start Country", player.disease.nexus.id);
        int winner = this.havePlayersSelectedSameCountry ? -2 : 1;
        CPlayerInfoSteam localPlayerInfo = CGameManager.localPlayerInfo as CPlayerInfoSteam;
        NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) this.Difficulty, localPlayerInfo.PlayerID, localPlayerInfo.name, localPlayerInfo.GetMultiplayerRating(), true, winner, CGameManager.localPlayerInfo.disease.technologies, CGameManager.localPlayerInfo.disease.genes, CGameManager.localPlayerInfo.disease.diseaseType, CGameManager.localPlayerInfo.disease.nexus.id);
        List<string> stringList = new List<string>();
        if (player.GetIntStat("UNLOCK_MP_VIRUS") == 0)
          stringList.Add("UNLOCK_MP_VIRUS");
        if (player.GetIntStat("UNLOCK_MP_BIO_WEAPON") == 0)
          stringList.Add("UNLOCK_MP_BIO_WEAPON");
        if (player.GetIntStat("UNLOCK_MP_FUNGUS") == 0)
          stringList.Add("UNLOCK_MP_FUNGUS");
        if (player.GetIntStat("UNLOCK_MP_PARASITE") == 0)
          stringList.Add("UNLOCK_MP_PARASITE");
        if (stringList.Count > 0)
        {
          if (player.playerStats.MP_DiseaseUnlockCounter > 5)
            player.playerStats.MP_DiseaseUnlockCounter = 1;
          else if (player.playerStats.MP_DiseaseUnlockCounter == 5)
          {
            string statName = stringList[UnityEngine.Random.Range(0, stringList.Count)];
            player.SetIntStat(statName, 1);
            Disease.EDiseaseType[] values = (Disease.EDiseaseType[]) Enum.GetValues(typeof (Disease.EDiseaseType));
            for (int index = 0; index < values.Length; ++index)
            {
              if (values[index].ToString() == statName.Substring("UNLOCK_MP_".Length))
              {
                unlocked.disease = new Disease.EDiseaseType?(values[index]);
                break;
              }
            }
          }
        }
      }
      else
      {
        if (this.world.winner == null)
        {
          (CNetworkManager.network as CNetworkSteam).LocalPlayerInfo.ProcessMultiplayerGameResult(player.disease, MultiplayerGame.ResultType.DRAW, this.opponentPlayer.playerStats.MP_Rating, this.gameParameters.friendMode == INetwork.FriendMode.Public);
        }
        else
        {
          player.playerStats.MP_VS_Quickest_Loss = player.playerStats.MP_VS_Quickest_Loss > 0 ? Mathf.Min(a1, player.playerStats.MP_VS_Quickest_Loss) : a1;
          (CNetworkManager.network as CNetworkSteam).LocalPlayerInfo.ProcessMultiplayerGameResult(player.disease, MultiplayerGame.ResultType.LOSE, this.opponentPlayer.playerStats.MP_Rating, this.gameParameters.friendMode == INetwork.FriendMode.Public);
        }
        Analytics.Event("MP Lose Game Time", a1.ToString());
        Analytics.Event("MP Lose Game Turns", this.world.DiseaseTurn.ToString());
        Analytics.Event("MP Lose Disease Type", this.world.diseases[0].diseaseType.ToString());
        Analytics.Event("MP Lose Start Country", player.disease.nexus.id);
        int winner = this.havePlayersSelectedSameCountry ? -2 : 0;
        NdemicAnalytics.RecordTechAnalytics(CGameManager.gameType, (int) this.Difficulty, player.PlayerID, player.name, player.GetMultiplayerRating(), true, winner, player.disease.technologies, player.disease.genes, player.disease.diseaseType, player.disease.nexus.id);
      }
    }
    CNetworkManager.network.SaveLocalPlayerData();
    unlocked.gameType = CGameManager.gameType;
    CInterfaceManager.instance.EndGame(CGameManager.gameType, didWin, diseaseWon, resultType, unlocked);
    if (this.world.DiseaseTurn <= 0)
      return;
    this.StartCoroutine(this.ExportMultiplayerGameLogCo());
  }

  private IEnumerator ExportMultiplayerGameLogCo()
  {
    yield return (object) null;
    this.ExportMultiplayerGameLog();
  }

  private StringBuilder AppendDiseaseTag(StringBuilder builder, int winner, int id)
  {
    builder.Append(id == winner ? "Winner" : "Loser").Append("(").Append(id).Append(")");
    return builder;
  }

  public void ExportMultiplayerGameLog()
  {
    int winner = -1;
    if (this.world.gameWon)
      winner = this.world.winner.id;
    StringBuilder builder = new StringBuilder();
    builder.Append("------- MP Gameplay Log ------").AppendLine();
    for (int index1 = 0; index1 < this.world.diseases.Count; ++index1)
    {
      Disease disease = this.world.diseases[index1];
      this.AppendDiseaseTag(builder, winner, disease.id).Append(": name = ").Append(disease.name).Append(": type = ").Append((object) disease.diseaseType).AppendLine();
      if (disease.nexus != null)
        this.AppendDiseaseTag(builder, winner, disease.id).Append(": nexus = ").Append(disease.nexus.id).AppendLine();
      for (int index2 = 0; index2 < disease.genes.Count; ++index2)
        this.AppendDiseaseTag(builder, winner, disease.id).Append(": gene = ").Append(disease.genes[index2].id).AppendLine();
    }
    for (int index = 0; index < this.replayData.replayEvents.Count; ++index)
    {
      ReplayData.ReplayEvent replayEvent = this.replayData.replayEvents[index];
      int diseaseId = replayEvent.diseaseID;
      if (replayEvent.type == ReplayData.ReplayEventType.BONUS_ICON_PRESSED)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": POP BUBBLE - ID: ").Append(replayEvent.id).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.TECH_EVOLVED)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": tech = ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.TECH_DEEVOLVED)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": devolve = ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.LOG_MESSAGE)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": LOG = ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.EVO_POINTS)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": evo change: ").Append(replayEvent.id).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.UNSCHEDULED_FLIGHT)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": make unscheduled flight: ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.BENIGN_MIMIC)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": do benign mimic: ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.IMMUNE_SHOCK)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": do immune shock: ").Append(replayEvent.param).AppendLine();
      }
      else if (replayEvent.type == ReplayData.ReplayEventType.POST_TURN_LOG)
      {
        builder.Append("[").Append(replayEvent.turn).Append("] ");
        this.AppendDiseaseTag(builder, winner, diseaseId).Append(": END TURN = ").Append(replayEvent.param).AppendLine();
      }
    }
    builder.Append(winner.ToString() + " won : " + (this.world as MPWorld).gameOverDescription);
    Debug.Log((object) builder.ToString());
  }

  public override void ReplayGame()
  {
    this.network.LocalPlayerInfo = CGameManager.localPlayerInfo;
    base.ReplayGame();
    for (int index = 0; index < this.world.diseases.Count; ++index)
    {
      Disease disease = this.world.diseases[index];
      if (this.replayData.geneStore.ContainsKey(disease.id))
      {
        List<Gene> geneList = new List<Gene>();
        foreach (string str in this.replayData.geneStore[disease.id])
        {
          string geneId = str;
          Debug.Log((object) ("____geneId:" + geneId));
          Gene gene1 = this.availableGenes.Find((Predicate<Gene>) (gene => gene.id == geneId));
          if (gene1 != null)
            geneList.Add(gene1);
        }
        Debug.Log((object) ("(" + disease.name + ") APPLY GENES - num genes:" + (object) geneList.Count));
        disease.ApplyGenes(geneList.ToArray());
      }
    }
  }

  public override void ChooseDisease(string diseaseName, Disease.EDiseaseType diseaseType)
  {
    if (this.network.IsServer)
      this.AddDisease(diseaseName, diseaseType, CGameManager.localPlayerInfo, Disease.GetCheatFlags(this.cheatsUsed));
    else
      this.netView.RPC(NetworkChannel.Game, "RPCChooseDisease", RPCTarget.Host, true, (object) diseaseName, (object) diseaseType.ToString(), (object) this.network.LocalPlayerInfo, (object) Disease.GetCheatFlags(this.cheatsUsed));
  }

  public override Disease AddDisease(
    string diseaseName,
    Disease.EDiseaseType diseaseType,
    IPlayerInfo player,
    int cheatFlags)
  {
    Disease disease = base.AddDisease(diseaseName, diseaseType, player, cheatFlags);
    if (disease != null)
    {
      disease.showExtraPopups = false;
      if (player != null)
        this.netView.RPC(NetworkChannel.Game, "RPCAddDisease", RPCTarget.Others, true, (object) diseaseName, (object) diseaseType.ToString(), (object) player, (object) cheatFlags);
    }
    return disease;
  }

  public override GameStats RecordStats(Disease forDisease)
  {
    if (!this.gameHistory.GameStats.ContainsKey(forDisease.id))
      this.gameHistory.GameStats[forDisease.id] = new List<GameStats>();
    MPGameStats mpGameStats = new MPGameStats(forDisease, forDisease == World.instance.GetDisease(0) ? World.instance.GetDisease(1) : World.instance.GetDisease(0));
    this.gameHistory.GameStats[forDisease.id].Add((GameStats) mpGameStats);
    return (GameStats) mpGameStats;
  }

  protected override void DebugDiseaseStats(GameStats stats)
  {
  }

  public override void RecoverGameState(string fromState, string replayState)
  {
    if (string.IsNullOrEmpty(fromState))
      return;
    this.LoadGameState(fromState, replayState);
  }

  public override ISaves.SaveMetaData SaveGame(int slotID)
  {
    throw new Exception("Cannot save a multiplayer game.");
  }

  public override string GetSerializedGameState()
  {
    return GameSerializer.Save((IGame) this, this.world, this.gameHistory, Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted), true);
  }

  public override void DroneSetSpawnPosition(Vehicle vehicle)
  {
    this.netView.RPC(NetworkChannel.Game, "RPCDroneSetSpawnPosition", RPCTarget.Others, (object) vehicle.id, (object) vehicle.sourcePosition);
  }

  public void ContinueAsSinglePlayer()
  {
    Analytics.Event("MP Winner Play On", Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted).ToString());
    this.IsSPContinue = true;
    World.instance.winner = (Disease) null;
    World.instance.gameWon = false;
    World.instance.gameEnded = false;
    this.replayData.AddEvent(ReplayData.ReplayEventType.MP_PART_ENDED, World.instance.DiseaseTurn, World.instance.eventTurn, CGameManager.localPlayerInfo.disease, "");
    this.SwitchToSinglePlayer(CGameManager.localPlayerInfo.disease);
    this.gameState = IGame.GameState.InProgress;
    DiseaseTrailParticles.instance.UpdateVisibility();
    this.actualSpeed = 1;
    CGameManager.GameSpeedChanged(1);
    CUIManager.instance.SetActiveScreen("HUDScreen");
  }

  protected override void FireReplayEvent(ReplayData.ReplayEvent replayEvent)
  {
    if (replayEvent.type == ReplayData.ReplayEventType.MP_PART_ENDED)
      this.SwitchToSinglePlayer(World.instance.GetDisease(replayEvent.diseaseID));
    else
      base.FireReplayEvent(replayEvent);
  }

  private void SwitchToSinglePlayer(Disease d)
  {
    CInterfaceManager.instance.MultiPlayerToSinglePlayer();
    Disease acting = (Disease) null;
    for (int index = 0; index < World.instance.diseases.Count; ++index)
    {
      if (World.instance.diseases[index] != d)
        acting = World.instance.diseases[index];
    }
    for (int index = 0; index < World.instance.countries.Count; ++index)
      ((MPCountry) World.instance.countries[index]).TransferAllInfected(acting, d);
    acting.globalInfectiousness = 0.0f;
    acting.globalSeverity = 0.0f;
    acting.globalLethality = 0.0f;
    acting.globalInfectiousnessMax = 0.0f;
    acting.globalSeverityMax = 0.0f;
    acting.globalLethalityMax = 0.0f;
    acting.cureFlag = true;
    Debug.Log((object) ("----evoPoints:" + (object) d.evoPoints + ", points:" + (object) 50));
    d.evoPoints += 50;
  }

  protected override bool CanSeeDots(LocalDisease _ld)
  {
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.IsReplayActive || (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.CurrentGameState == IGame.GameState.EndGame)
      return true;
    if (_ld is MPLocalDisease mpLocalDisease)
    {
      if (mpLocalDisease.isVisible)
        return true;
      if (mpLocalDisease.disease.nexusVisibility)
      {
        Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
        foreach (KeyValuePair<string, NexusObject> nexus in CInterfaceManager.instance.nexusMap)
        {
          if (nexus.Value.disease != disease)
          {
            LocalDisease localDisease = nexus.Value.country.GetLocalDisease(disease);
            if (localDisease.infectedPopulation > 0L || (double) localDisease.deadPercent > 0.0)
              return true;
          }
        }
      }
      if (mpLocalDisease.infectedPopulation > 0L || (double) mpLocalDisease.deadPercent > 0.0)
      {
        mpLocalDisease.isVisible = true;
        return true;
      }
    }
    MultiplayerGame game = CGameManager.game as MultiplayerGame;
    if ((bool) (UnityEngine.Object) game && game.IsSPContinue)
      return true;
    MPCountry country = mpLocalDisease.country as MPCountry;
    bool flag = false;
    if (country.activeGemEffects.Count > 0)
    {
      foreach (GemEffect activeGemEffect in country.activeGemEffects)
      {
        if (activeGemEffect.providesVisibility && activeGemEffect.disease == CGameManager.localPlayerInfo.disease)
          flag = true;
      }
    }
    return flag;
  }

  public void OnEndGameReached()
  {
    this.actualSpeed = 0;
    CGameManager.GameSpeedChanged(0);
  }

  public override void SetSpeed(int gameSpeed)
  {
    Debug.Log((object) ("MultiplayerGame.SetSpeed - gameSpeed:" + (object) gameSpeed));
    if (this.IsReplayActive || this.IsSPContinue)
    {
      this.actualSpeed = this.wantedSpeed = gameSpeed;
      CGameManager.GameSpeedChanged(0);
    }
    else
    {
      if (this.IsAIGame)
      {
        this.actualSpeed = gameSpeed;
        CGameManager.GameSpeedChanged(this.actualSpeed);
      }
      else if (this.resumeSpeedCountdownTick < 0)
        return;
      this.wantedSpeed = gameSpeed;
      CGameManager.localPlayerInfo.gameSpeed = gameSpeed;
      if (this.IsAIGame)
        return;
      if ((UnityEngine.Object) this.netView != (UnityEngine.Object) null)
        this.netView.RPC(NetworkChannel.Game, "RPCSetSpeed", RPCTarget.Others, (object) gameSpeed, (object) this.network.LocalPlayerInfo);
      this.CheckNetSpeed();
    }
  }

  public override void TriggerEventUpdate(int seed)
  {
    if (!this.IsReplayActive)
      this.netView.RPC(NetworkChannel.Game, "RPCEventUpdate", RPCTarget.Others, true, (object) seed, (object) (this.world.eventTurn + 1), (object) this.world.DiseaseTurn);
    base.TriggerEventUpdate(seed);
  }

  public override void TriggerGameUpdate(int seed)
  {
    base.TriggerGameUpdate(seed);
    List<GemEffect> updatedGems = (this.world as MPWorld).updatedGems;
    if (updatedGems.Count > 0 && !this.IsReplayActive)
      CInterfaceManager.instance.SpawnAndUpdateGems(updatedGems);
    updatedGems.Clear();
    if (!this.IsSPContinue)
      this.CalculateScore();
    if (this.network.IsServer && !this.IsReplayActive)
    {
      string s = "";
      for (int index = 0; index < this.world.diseases.Count; ++index)
        s = s + "|" + (object) this.world.diseases[index].totalControlledInfected + "|" + (object) this.world.diseases[index].evoPoints + "|" + (object) this.world.diseases[index].infectedPointsPot;
      byte[] bytes = Encoding.UTF8.GetBytes(s);
      if (!BitConverter.IsLittleEndian)
        Array.Reverse((Array) bytes);
      string str = DataPacket.GenerateHash(bytes, HashingAlgorithm.SHA256).Substring(0, 4);
      this.netView.RPC(NetworkChannel.Game, "RPCGameUpdate", RPCTarget.Others, true, (object) seed, (object) this.world.DiseaseTurn, (object) str);
    }
    Dictionary<Disease, MPCountry> updatedNexuses = (this.world as MPWorld).updatedNexuses;
    if (updatedNexuses.Count > 0)
    {
      foreach (KeyValuePair<Disease, MPCountry> keyValuePair in updatedNexuses)
      {
        Disease key = keyValuePair.Key;
        MPCountry mpCountry = keyValuePair.Value;
        CInterfaceManager.instance.SpawnAndUpdateNexus(key, (Country) mpCountry);
      }
      updatedNexuses.Clear();
    }
    Dictionary<Disease, MPCountry> activeNexuses = (this.world as MPWorld).activeNexuses;
    if (activeNexuses.Count > 0)
    {
      foreach (KeyValuePair<Disease, MPCountry> keyValuePair in activeNexuses)
      {
        Disease key = keyValuePair.Key;
        MPCountry mpCountry = keyValuePair.Value;
        CInterfaceManager.instance.SpawnNexusDNABubble(key, (Country) mpCountry);
      }
      activeNexuses.Clear();
    }
    if (!this.IsAIGame || this.IsReplayActive)
      return;
    for (int index = this.bonusIcons.Count - 1; index >= 0; --index)
    {
      BonusIcon bonusIcon = this.bonusIcons[index];
      if (bonusIcon.disease == this.theirDisease && World.instance.DiseaseTurn > bonusIcon.bornTurn && this.world.ClickBonusIcon(bonusIcon, (Disease) this.theirDisease))
      {
        this.bonusIcons.Remove(bonusIcon);
        CInterfaceManager.instance.ClickBonus(bonusIcon);
        this.replayData.AddEvent(ReplayData.ReplayEventType.BONUS_ICON_PRESSED, this.world.DiseaseTurn, this.world.eventTurn, (Disease) this.theirDisease, bonusIcon.id);
      }
    }
    if (!(bool) (UnityEngine.Object) this.aiController)
      this.aiController = this.gameObject.AddComponent<MPAIController>();
    this.aiController.AIUpdate();
  }

  protected override string GetReplayDesyncInfo(Disease d)
  {
    return this.GetPostTurnLog((Disease) (d as MPDisease));
  }

  protected override string GetPostTurnLog(Disease d0)
  {
    MPDisease mpDisease = d0 as MPDisease;
    return "[ID:" + (object) mpDisease.id + ", name:" + mpDisease.name + "]" + ", evoPoi: " + (object) mpDisease.mData.evoPoints + ", dnaPoiGai: " + (object) mpDisease.mData.dnaPointsGained + ", TotInf: " + (object) mpDisease.mData.totalInfected + ", TotConInf: " + (object) mpDisease.mData.totalControlledInfected + ", InfPoiPot: " + (object) mpDisease.mData.infectedPointsPot + ", TotDead: " + (object) mpDisease.mData.totalDead + ", DeadThisTurn: " + (object) mpDisease.mData.deadThisTurn + ", CurPer: " + (object) mpDisease.mData.cureCompletePercent + ", InfInc: " + (object) mpDisease.mData.infectedIncome;
  }

  public void AIEvolveTech(Disease disease, Technology technology)
  {
    disease.EvolveTech(technology, false);
    this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_EVOLVED, this.world.DiseaseTurn, this.world.eventTurn, disease, technology.id);
  }

  public override bool EvolveTech(Technology technology)
  {
    if (this.network.IsServer)
    {
      if (base.EvolveTech(technology))
      {
        CDiseaseScreen currentScreen = CUIManager.instance.GetCurrentScreen() as CDiseaseScreen;
        if ((UnityEngine.Object) currentScreen != (UnityEngine.Object) null)
          currentScreen.EndEvolve(technology);
        this.netView.RPC(NetworkChannel.Game, "RPCEvolveTech", RPCTarget.Others, true, (object) technology.id, (object) this.network.LocalPlayerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
        return true;
      }
    }
    else
    {
      this.requestedEvolutions[technology.id] = true;
      this.netView.RPC(NetworkChannel.Game, "RPCTryEvolveTech", RPCTarget.Host, true, (object) technology.id, (object) this.network.LocalPlayerInfo);
    }
    return false;
  }

  public bool PreloadEvolveTech(MPDisease disease, Technology technology)
  {
    if (this.network.IsServer)
    {
      if (disease == this.network.LocalPlayerInfo.disease)
        this.EvolveTech(technology);
      else if (this.IsAIGame)
        this.RPCTryEvolveTech(technology.id, this.network.PlayerInfos[0]);
      else
        this.RPCTryEvolveTech(technology.id, this.network.PlayerInfos[0] != this.network.LocalPlayerInfo ? this.network.PlayerInfos[0] : this.network.PlayerInfos[1]);
    }
    return false;
  }

  public override bool DeEvolveTech(Technology technology)
  {
    if (this.network.IsServer)
    {
      if (base.DeEvolveTech(technology))
      {
        CDiseaseScreen currentScreen = CUIManager.instance.GetCurrentScreen() as CDiseaseScreen;
        if ((UnityEngine.Object) currentScreen != (UnityEngine.Object) null)
          currentScreen.EndEvolve(technology);
        this.netView.RPC(NetworkChannel.Game, "RPCDeEvolveTech", RPCTarget.Others, true, (object) technology.id, (object) this.network.LocalPlayerInfo);
        return true;
      }
    }
    else
    {
      this.requestedDeEvolutions[technology.id] = true;
      this.netView.RPC(NetworkChannel.Game, "RPCTryDeEvolveTech", RPCTarget.Host, true, (object) technology.id, (object) this.network.LocalPlayerInfo);
    }
    return false;
  }

  public override bool ClickBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive)
      return false;
    if (this.network.IsServer)
    {
      if (base.ClickBonusIcon(bonusIcon))
      {
        this.netView.RPC(NetworkChannel.Game, "RPCBonusClicked", RPCTarget.Others, true, (object) bonusIcon.id, (object) this.network.LocalPlayerInfo);
        return true;
      }
      Debug.Log((object) ("CANNOT CLICK: " + (object) bonusIcon.id));
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCBonusTryClick", RPCTarget.Host, true, (object) bonusIcon.id, (object) this.network.LocalPlayerInfo);
    return false;
  }

  public override void StartHideBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive || !this.network.IsServer)
      return;
    base.StartHideBonusIcon(bonusIcon);
    this.netView.RPC(NetworkChannel.Game, "RPCStartBonusHidden", RPCTarget.Others, true, (object) bonusIcon.id);
  }

  public override void HideBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive || !CNetworkManager.network.IsServer)
      return;
    base.HideBonusIcon(bonusIcon);
    this.netView.RPC(NetworkChannel.Game, "RPCBonusHidden", RPCTarget.Others, (object) bonusIcon.id);
  }

  public override void VehicleArrived(Vehicle vehicle, Vector3? position = null)
  {
    if (!this.network.IsServer || this.IsReplayActive)
      return;
    base.VehicleArrived(vehicle, position);
    if (vehicle.infectedTotal > 0)
    {
      string str = vehicle.infectedTotal.ToString() + " total ";
      if (vehicle.infected != null)
      {
        for (int id = 0; id < vehicle.infected.Length; ++id)
          str = str + this.world.GetDisease(id).name + "=" + (object) vehicle.infected[id] + " ";
      }
    }
    bool flag = position.HasValue;
    if (flag && vehicle.type != Vehicle.EVehicleType.ZombieHorde && vehicle.type != Vehicle.EVehicleType.Drone && vehicle.subType != Vehicle.EVehicleSubType.Neurax && vehicle.subType != Vehicle.EVehicleSubType.ApeLabPlane)
      flag = false;
    if (flag)
      this.netView.RPC(NetworkChannel.Game, "RPCVehicleArrived", RPCTarget.Others, (object) vehicle.id, (object) position.Value);
    else
      this.netView.RPC(NetworkChannel.Game, "RPCVehicleArrived", RPCTarget.Others, (object) vehicle.id);
  }

  public override DiseaseColourSet GetColourSet(Disease disease)
  {
    int num = 0;
    if (disease != CGameManager.localPlayerInfo.disease)
    {
      num = disease.id;
      if (num == 0)
        num = CGameManager.localPlayerInfo.disease.id;
    }
    if (disease.diseaseType == Disease.EDiseaseType.NECROA)
      return this.zombieDiseaseColourSets[num % this.zombieDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.NEURAX)
      return this.neuraxDiseaseColourSets[num % this.neuraxDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      return this.simianDiseaseColourSets[num % this.simianDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      return this.vampireDiseaseColourSets[num % this.vampireDiseaseColourSets.Length];
    return disease.diseaseType == Disease.EDiseaseType.CURE ? this.cureDiseaseColourSets[num % this.cureDiseaseColourSets.Length] : this.diseaseColourSets[num % this.diseaseColourSets.Length];
  }

  public override void UpdateAchievements() => this.world.ClearAchievements();

  public override void GameUpdate()
  {
    if (this.gameState == IGame.GameState.ChoosingCountry && !this.isFirstRound && this.hasTimerStarted)
    {
      this.nexusPickTimer -= Time.deltaTime;
      if ((double) this.nexusPickTimer <= 0.0 && !this.requestedRandom)
      {
        this.nexusPickTimer = 0.0f;
        this.requestedRandom = true;
        this.ChooseCountry("random", true);
      }
      CCountrySelect.instance.SetMPCountdown(this.nexusPickTimer / 60f);
    }
    else
      CCountrySelect.instance.ClearMPCountdown();
    if (this.gameState == IGame.GameState.EndGame || this.gameState == IGame.GameState.None)
      return;
    if (this.network.NumberOfConnectedPlayers == 1)
      this.NotifyOpponentLeft(this.theirDisease != null ? this.theirDisease.name : CLocalisationManager.GetText("MP_Opponent"));
    if (!this.IsReplayActive && this.network.NumberOfConnectedPlayers == 1 && !this.IsAIGame && World.instance != null && !World.instance.gameEnded && !this.IsSPContinue)
      this.EndGame(IGame.EndGameReason.OPPONENT_LEFT);
    if (CGameManager.game.CurrentGameState != IGame.GameState.InProgress)
      return;
    if (this.isGameOutOfSync)
      this.EndGame(IGame.EndGameReason.DESYNC);
    if (this.network.NetworkState != CNetworkManager.ENetworkState.InGame || this.network.IsServer && !this.IsAIGame && World.instance.DiseaseTurn - this.lastClientTurn > 5)
      return;
    if (this.resumeSpeedCountdownTick < 0 && (double) Time.time - (double) this.lastTimerResumeMark > 1.0)
    {
      ++this.resumeSpeedCountdownTick;
      if (this.resumeSpeedCountdownTick == 0)
      {
        CSoundManager.instance.PlaySFX("multiplayer_chatsend");
        this.actualSpeed = 1;
        this.SetSpeed(1);
      }
      else
        CGameManager.GameSpeedChanged(this.resumeSpeedCountdownTick, false);
      this.lastTimerResumeMark = Time.time;
    }
    if (Input.GetKeyDown(KeyCode.T))
    {
      IGame.showMoreDetail = !IGame.showMoreDetail;
      CHUDScreen.instance.SetDay(World.instance.DiseaseTurn);
    }
    if (!this.network.IsServer && !this.IsReplayActive)
      return;
    base.GameUpdate();
  }

  public override void CreateUnscheduledFlight(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    if (this.network.IsServer)
    {
      int num = this.ProcessUnscheduledFlight(disease, fromCountry, toCountry, sourcePosition, endPosition, -1);
      if (num <= 0)
        return;
      this.netView.RPC(NetworkChannel.Game, "RPCCreateUnscheduledFlight", RPCTarget.Others, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) num, (object) this.network.LocalPlayerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCTryCreateUnscheduledFlight", RPCTarget.Host, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) this.network.LocalPlayerInfo);
  }

  protected override int ProcessUnscheduledFlight(
    Disease disease,
    Country fromCountry,
    Country toCountry,
    Vector3 sourcePosition,
    Vector3 endPosition,
    int presetInfected = -1)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.unscheduled_flight, disease);
    Debug.Log((object) ("[" + (object) this.world.DiseaseTurn + "].ProcessUnscheduledFlight - disease.evoPoints:" + (object) disease.evoPoints + ", cost:" + (object) activeAbilityCost + ", fromCountry.GetLocalDisease(disease).allInfected:" + (object) fromCountry.GetLocalDisease(disease).allInfected + ", CGameManager.UNSCHEDULED_FLIGHT_MINIMUM:" + (object) 1000));
    if (disease.evoPoints >= activeAbilityCost && fromCountry.GetLocalDisease(disease).allInfected > 1000L)
    {
      disease.SpendEvoPoints(activeAbilityCost);
      int infected = presetInfected == -1 ? CUtils.IntRand(10, 30) : presetInfected;
      ((MPWorld) this.world).CreateUnscheduledFlight(disease, (IList<Vector3>) new Vector3[2]
      {
        sourcePosition,
        endPosition
      }, (IList<Country>) new Country[2]
      {
        fromCountry,
        toCountry
      }, infected);
      if (!this.IsReplayActive)
      {
        Debug.Log((object) ("[" + (object) this.world.DiseaseTurn + "].ProcessUnscheduledFlight - ADD REPLAY EVENT[" + (object) this.world.DiseaseTurn + "][" + (object) this.world.eventTurn + "] - " + fromCountry.id + ":" + toCountry.id + ":" + (object) infected));
        this.replayData.AddEvent(ReplayData.ReplayEventType.UNSCHEDULED_FLIGHT, this.world.DiseaseTurn, this.world.eventTurn, disease, fromCountry.id + ":" + toCountry.id + ":" + (object) infected);
      }
      CInterfaceManager.instance.UpdateInterface();
      return infected;
    }
    Debug.LogError((object) "Client tried to create unscheduled flight but insufficient evo points or current population");
    return -1;
  }

  public override void CreateNukeLaunch(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    if (this.network.IsServer)
    {
      this.ProcessNukeLaunch(disease, fromCountry, toCountry, sourcePosition, endPosition);
      this.netView.RPC(NetworkChannel.Game, "RPCCreateNukeLaunch", RPCTarget.Others, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) this.network.LocalPlayerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCTryCreateNukeLaunch", RPCTarget.Host, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) this.network.LocalPlayerInfo);
  }

  public override void ProcessNukeLaunch(
    Disease disease,
    Country fromCountry,
    Country toCountry,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    ((MPWorld) this.world).CreateNukeLaunch(disease, (IList<Vector3>) new Vector3[2]
    {
      sourcePosition,
      endPosition
    }, (IList<Country>) new Country[2]
    {
      fromCountry,
      toCountry
    });
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.NUKE_LAUNCH, this.world.DiseaseTurn, this.world.eventTurn, disease, fromCountry.id + ":" + toCountry.id);
    CInterfaceManager.instance.UpdateInterface();
  }

  [PlagueRPC]
  public override void ImmuneShock(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (this.IsAIGame)
      this.ProcessImmuneShock(countryID, pos, playerSender.disease);
    else if (this.network.IsServer)
      this.netView.RPC(NetworkChannel.Game, "RPCImmuneShock", RPCTarget.All, true, (object) countryID, (object) pos, (object) playerSender);
    else
      this.netView.RPC(NetworkChannel.Game, nameof (ImmuneShock), RPCTarget.Host, true, (object) countryID, (object) pos, (object) playerSender);
  }

  protected override void ProcessImmuneShock(string countryID, Vector3 pos, Disease disease)
  {
    Country country = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.immune_shock, disease);
    if (disease.evoPoints >= activeAbilityCost)
    {
      disease.SpendEvoPoints(activeAbilityCost);
      MPLocalDisease localDisease = (MPLocalDisease) country.GetLocalDisease(disease);
      localDisease.ImmuneShock();
      (disease as MPDisease).RecordActiveAbilityUse(EAbilityType.immune_shock);
      CInterfaceManager.instance.GetCountryView(localDisease.country.id).ImmuneShockEffect(pos, disease == this.myDisease);
      if (this.IsReplayActive)
        return;
      this.replayData.AddEvent(ReplayData.ReplayEventType.IMMUNE_SHOCK, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id + ":" + (object) pos.x + ":" + (object) pos.y + ":" + (object) pos.z);
    }
    else
      Debug.Log((object) ("Immune Shock Cost Failed: " + disease.name + " points: " + (object) disease.evoPoints + " cost: " + (object) activeAbilityCost));
  }

  [PlagueRPC]
  public void RPCImmuneShock(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (playerSender == null)
      return;
    this.ProcessImmuneShock(countryID, pos, playerSender.disease);
  }

  [PlagueRPC]
  public override void BenignMimic(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (this.IsAIGame)
      this.RPCBenignMimic(countryID, pos, playerSender);
    if (this.network.IsServer)
      this.netView.RPC(NetworkChannel.Game, "RPCBenignMimic", RPCTarget.All, true, (object) countryID, (object) pos, (object) playerSender);
    else
      this.netView.RPC(NetworkChannel.Game, nameof (BenignMimic), RPCTarget.Host, true, (object) countryID, (object) pos, (object) playerSender);
  }

  public override void ProcessBenignMimic(string countryID, Vector3 pos, Disease disease)
  {
    Country country = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.benign_mimic, disease);
    Debug.Log((object) ("[" + (object) this.world.DiseaseTurn + "]. ProcessBenignMimic - country:" + country.name + ", by:" + disease.name));
    MPLocalDisease localDisease = (MPLocalDisease) country.GetLocalDisease(disease);
    if (disease.evoPoints >= activeAbilityCost && localDisease.benignMimicCounter <= 0)
    {
      disease.SpendEvoPoints(activeAbilityCost);
      localDisease.BenignMimic();
      (disease as MPDisease).RecordActiveAbilityUse(EAbilityType.benign_mimic);
      CInterfaceManager.instance.GetCountryView(localDisease.country.id).BenignMimicEffect(pos, disease == this.myDisease);
      if (this.IsReplayActive)
        return;
      this.replayData.AddEvent(ReplayData.ReplayEventType.BENIGN_MIMIC, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id + ":" + (object) pos.x + ":" + (object) pos.y + ":" + (object) pos.z);
    }
    else
      Debug.Log((object) ("Benign Mimic Cost Failed: " + disease.name + " points: " + (object) disease.evoPoints + " cost: " + (object) activeAbilityCost + " counter: " + (object) localDisease.benignMimicCounter));
  }

  [PlagueRPC]
  public void RPCBenignMimic(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (playerSender == null)
      return;
    this.ProcessBenignMimic(countryID, pos, playerSender.disease);
  }

  [PlagueRPC]
  public override void CreateNukeStrike(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (this.IsAIGame)
      this.ProcessNukeStrike(countryID, pos, playerSender.disease);
    else if (this.network.IsServer)
      this.netView.RPC(NetworkChannel.Game, "RPCCreateNukeStrike", RPCTarget.All, true, (object) countryID, (object) pos, (object) playerSender);
    else
      this.netView.RPC(NetworkChannel.Game, nameof (CreateNukeStrike), RPCTarget.Host, true, (object) countryID, (object) pos, (object) playerSender);
  }

  public override void ProcessNukeStrike(string countryID, Vector3 pos, Disease disease)
  {
    MPCountry mpCountry = (MPCountry) this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    mpCountry.NukeStrike(disease);
    CInterfaceManager.instance.GetCountryView(mpCountry.id).NukeStrikeEffect(pos, disease == this.myDisease);
    if (this.myDisease == disease)
      CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_MP_Nuclear_Warfare));
    if (this.IsReplayActive)
      return;
    this.replayData.AddEvent(ReplayData.ReplayEventType.NUKE_STRIKE, this.world.DiseaseTurn, this.world.eventTurn, disease, mpCountry.id + ":" + (object) pos.x + ":" + (object) pos.y + ":" + (object) pos.z);
  }

  [PlagueRPC]
  public void RPCCreateNukeStrike(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    if (playerSender == null || playerSender.disease == null)
      return;
    this.ProcessNukeStrike(countryID, pos, playerSender.disease);
  }

  public void CreateGem(GemAbility ability, Disease d, Country c, Vector3 pos)
  {
    if (this.network.IsServer)
    {
      if (d.evoPoints < ability.cost)
        return;
      MPWorld world = (MPWorld) this.world;
      d.SpendEvoPoints(ability.cost);
      GemAbility ability1 = ability;
      Disease d1 = d;
      Country c1 = c;
      Vector3? pos1 = new Vector3?(pos);
      world.CreateGemEffect(ability1, d1, c1, pos1);
      this.netView.RPC(NetworkChannel.Game, "RPCCreateGem", RPCTarget.Others, true, (object) this.GetGemAbilityID(ability), (object) c.id, (object) pos, (object) this.network.LocalPlayerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCTryCreateGem", RPCTarget.Host, true, (object) this.GetGemAbilityID(ability), (object) c.id, (object) pos, (object) this.network.LocalPlayerInfo);
  }

  public void MoveGem(GemEffect gem, Country to)
  {
    if (this.network.IsServer)
    {
      ((MPWorld) this.world).MoveGemEffect(gem, to);
      this.netView.RPC(NetworkChannel.Game, "RPCMoveGem", RPCTarget.Others, true, (object) gem.id, (object) to.id, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      this.netView.RPC(NetworkChannel.Game, "RPCTryMoveGem", RPCTarget.Host, true, (object) gem.id, (object) to.id);
  }

  private void ConfirmDisconnect()
  {
    if (this.world.eventTurn > 0)
    {
      CGameManager.game = (IGame) this;
      this.gameEndedIncomplete = true;
      if (World.instance.diseases[0].totalInfected > World.instance.diseases[1].totalInfected)
      {
        World.instance.winner = World.instance.diseases[0];
        World.instance.gameWon = true;
      }
      else if (World.instance.diseases[1].totalInfected > World.instance.diseases[0].totalInfected)
      {
        World.instance.winner = World.instance.diseases[1];
        World.instance.gameWon = true;
      }
      else
      {
        World.instance.winner = (Disease) null;
        World.instance.gameWon = false;
      }
      CInterfaceManager.instance.EndGame(CGameManager.gameType, World.instance.winner == CGameManager.localPlayerInfo.disease, World.instance.winner, World.instance.gameEndResult, new CUIManager.Unlock());
    }
    else
    {
      CGameManager.ClearGame();
      IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
      screen.HideAllSubScreens();
      CUIManager.instance.ClearHistory();
      List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Clear();
      screen.HideSubScreen("Start");
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Multi"));
      CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
      igameSubScreenList[0].SetActive(true);
    }
    this.isInDisconnectResponse = false;
    this.isInDesyncResponse = false;
    this.isGameOutOfSync = false;
  }

  private void CheckNetSpeed()
  {
    int mySpeed = 0;
    int oppSpeed = 0;
    foreach (IPlayerInfo playerInfo in this.network.PlayerInfos)
    {
      if (playerInfo == this.network.LocalPlayerInfo)
        mySpeed = playerInfo.gameSpeed;
      else
        oppSpeed = playerInfo.gameSpeed;
    }
    int actualSpeed = this.actualSpeed;
    if (mySpeed == 0 && oppSpeed == 0)
      this.actualSpeed = 0;
    else if ((mySpeed > 0 || oppSpeed > 0) && this.resumeSpeedCountdownTick >= 0 && actualSpeed == 0)
    {
      this.actualSpeed = 0;
      this.resumeSpeedCountdownTick = -5;
      this.lastTimerResumeMark = Time.time;
      CGameManager.GameSpeedChanged(this.resumeSpeedCountdownTick, false);
      return;
    }
    CGameManager.GameSpeedChanged(this.actualSpeed, this.actualSpeed != actualSpeed, mySpeed, oppSpeed);
  }

  private bool CanPickCountry(string countryID) => !this.dissalowedCountries.Contains(countryID);

  private void RegisterInterest(string countryID, IPlayerInfo playerInfo)
  {
    this.currentCountryPick[playerInfo] = countryID;
  }

  private void UnregisterInterest(IPlayerInfo playerInfo)
  {
    this.currentCountryPick.Remove(playerInfo);
  }

  private bool HasRegisteredInterest(IPlayerInfo playerInfo)
  {
    return this.currentCountryPick.ContainsKey(playerInfo) && !string.IsNullOrEmpty(this.currentCountryPick[playerInfo]);
  }

  private void CheckNexusPickRules(IPlayerInfo playerInfo)
  {
    Debug.Log((object) ("MultiplayerGame.CheckNexusPickRules[] - playerInfo:" + (playerInfo != null ? (object) playerInfo.name : (object) "NULL") + ", currentCountryPick.Count:" + (object) this.currentCountryPick.Count + ", mpParams.numberOfPlayers:" + (object) this.gameParameters.numberOfPlayers + " PRACTICE: " + this.IsAIGame.ToString()));
    if (this.IsAIGame)
    {
      if (this.currentCountryPick.Count <= 0)
        return;
      string str = this.currentCountryPick[playerInfo];
      if (str == "notRandom")
      {
        this.currentCountryPick.Clear();
      }
      else
      {
        List<Country> countryList = new List<Country>();
        Country forCountry = (Country) null;
        for (int index = 0; index < this.world.countries.Count; ++index)
        {
          Country country = this.world.countries[index];
          if (country.id == this.currentCountryPick[playerInfo])
            forCountry = country;
          else
            countryList.Add(country);
        }
        if (str == "random")
        {
          forCountry = countryList[UnityEngine.Random.Range(0, countryList.Count)];
          countryList.Remove(forCountry);
        }
        this.world.SetNexus((Disease) this.myDisease, forCountry);
        this.world.SetNexus((Disease) this.theirDisease, countryList[UnityEngine.Random.Range(0, countryList.Count)]);
        this.StartGame();
      }
    }
    else if (this.currentCountryPick.Count == (this.IsAIGame ? this.network.NumberOfConnectedPlayers : this.gameParameters.numberOfPlayers))
    {
      bool flag1 = false;
      if (this.isFirstRound)
      {
        bool flag2 = true;
        this.isFirstRound = false;
        foreach (string str in this.currentCountryPick.Values)
        {
          if (str != "random")
            flag2 = false;
        }
        if (flag2)
        {
          List<string> stringList = new List<string>();
          foreach (Country country in this.world.countries)
          {
            string id = country.id;
            stringList.Add(id);
          }
          foreach (IPlayerInfo playerInfo1 in this.network.PlayerInfos)
          {
            string str = stringList[UnityEngine.Random.Range(0, stringList.Count)];
            this.currentCountryPick[playerInfo1] = str;
            stringList.Remove(str);
          }
        }
        else
        {
          this.currentCountryPick.Clear();
          this.netView.RPC(NetworkChannel.Game, "RPCResetPickTimer", RPCTarget.All);
          return;
        }
      }
      else
      {
        List<string> stringList1 = new List<string>();
        bool flag3 = false;
        string str1 = "";
        foreach (string str2 in this.currentCountryPick.Values)
        {
          if (stringList1.Contains(str2))
          {
            str1 = str2;
            flag3 = true;
            break;
          }
          if (str2 != "random")
            stringList1.Add(str2);
          else
            flag1 = true;
        }
        if (flag3)
        {
          ++this.nexusPickAttempts;
          if (this.network.IsServer)
          {
            Debug.Log((object) "Log duplicate");
            Analytics.Event("MP Duplicate Country Pick", str1);
          }
          if (!this.isRepickingAllowed)
          {
            foreach (string str3 in this.currentCountryPick.Values)
            {
              if (!this.dissalowedCountries.Contains(str3))
                this.dissalowedCountries.Add(str3);
            }
          }
          this.currentCountryPick.Clear();
          if (this.nexusPickAttempts >= 5)
          {
            List<string> stringList2 = new List<string>();
            foreach (Country country in this.world.countries)
            {
              string id = country.id;
              if (this.isRandomingBannedCountriesAllowed || !this.dissalowedCountries.Contains(id))
                stringList2.Add(id);
            }
            foreach (IPlayerInfo playerInfo2 in this.network.PlayerInfos)
            {
              string str4 = stringList2[UnityEngine.Random.Range(0, stringList2.Count)];
              this.currentCountryPick[playerInfo2] = str4;
              stringList2.Remove(str4);
            }
            this.netView.RPC(NetworkChannel.Game, "RPCDisplayNetworkPopup", RPCTarget.All, (object) 3);
            this.forcedRandomPick = true;
          }
          else
          {
            this.netView.RPC(NetworkChannel.Game, "RPCDisplayNetworkPopup", RPCTarget.All, (object) 1);
            this.netView.RPC(NetworkChannel.Game, "RPCResetPickTimer", RPCTarget.All);
            return;
          }
        }
      }
      if (flag1)
      {
        List<string> stringList = new List<string>();
        foreach (Country country in this.world.countries)
        {
          string id = country.id;
          if (!this.dissalowedCountries.Contains(id))
            stringList.Add(id);
        }
        foreach (string str in this.currentCountryPick.Values)
          stringList.Remove(str);
        foreach (IPlayerInfo playerInfo3 in this.network.PlayerInfos)
        {
          if (this.currentCountryPick[playerInfo3] == "random")
          {
            string str = stringList[UnityEngine.Random.Range(0, stringList.Count)];
            this.currentCountryPick[playerInfo3] = str;
            stringList.Remove(str);
          }
        }
      }
      foreach (IPlayerInfo playerInfo4 in this.network.PlayerInfos)
      {
        if (this.network.LocalPlayerInfo == playerInfo4)
        {
          base.ChooseCountry(this.currentCountryPick[playerInfo4]);
          this.netView.RPC(NetworkChannel.Game, "RPCSetCountry", RPCTarget.Others, (object) this.network.LocalPlayerInfo, (object) this.currentCountryPick[playerInfo4]);
        }
        else
          this.RPCChooseCountry(playerInfo4, this.currentCountryPick[playerInfo4]);
      }
      if (this.world.nexusCountries.Count < CNetworkManager.network.NumberOfConnectedPlayers)
        return;
      this.StartGame();
    }
    else
    {
      if ((this.currentCountryPick[playerInfo] == "notRandom" ? 0 : (!(this.currentCountryPick[playerInfo] == "random") ? 1 : 0)) == 0)
        return;
      if (playerInfo == this.network.LocalPlayerInfo)
        this.DisplayNetworkPopup(MultiplayerGame.MultiplayerMessage.WaitingForPick);
      else
        this.netView.RPC(NetworkChannel.Game, "RPCDisplayNetworkPopup", playerInfo, (object) 0);
    }
  }

  public void DisplayNetworkPopup(MultiplayerGame.MultiplayerMessage messageCode)
  {
    string text1 = CLocalisationManager.GetText("MP_Country_Selection_Title");
    string text2;
    switch (messageCode)
    {
      case MultiplayerGame.MultiplayerMessage.WaitingForPick:
        IPlayerInfo iplayerInfo = (IPlayerInfo) null;
        foreach (IPlayerInfo playerInfo in CNetworkManager.network.PlayerInfos)
        {
          if (playerInfo != CNetworkManager.network.LocalPlayerInfo)
            iplayerInfo = playerInfo;
        }
        text2 = !this.WantRandomGame ? (iplayerInfo == null ? CLocalisationManager.GetText("MP_Auto_Selection_Warning") : CLocalisationManager.GetText("MP_Auto_Selection_Timer")) : CLocalisationManager.GetText("MP_Country_Selection_Force_Random");
        break;
      case MultiplayerGame.MultiplayerMessage.PickedDuplicates:
        this.havePlayersSelectedSameCountry = true;
        text2 = CLocalisationManager.GetText("MP_Country_Selection_Text_Same_Picked");
        break;
      case MultiplayerGame.MultiplayerMessage.PickedBanned:
        text2 = CLocalisationManager.GetText("MP_Country_Selection_Text_Banned");
        break;
      case MultiplayerGame.MultiplayerMessage.RandomPicks:
        text2 = CLocalisationManager.GetText("MP_Country_Selection_Text_Same_To_Random");
        break;
      case MultiplayerGame.MultiplayerMessage.AlreadyPicked:
        text2 = CLocalisationManager.GetText("MP_Country_Selection_Text_Already_Picked");
        break;
      default:
        text2 = text1;
        break;
    }
    CCountrySelect.instance.SetMPMessage(text2);
  }

  public void DisplayTimerMessage(bool me)
  {
    if (this.WantRandomGame)
      CCountrySelect.instance.SetMPMessage(CLocalisationManager.GetText("MP_Country_Selection_Force_Random"));
    else if (me)
      CCountrySelect.instance.SetMPMessage(CLocalisationManager.GetText("MP_Auto_Selection_Warning"));
    else
      CCountrySelect.instance.SetMPMessage(CLocalisationManager.GetText("MP_Auto_Selection_Timer"));
  }

  public string GetGemAbilityID(GemAbility gem)
  {
    foreach (KeyValuePair<string, GemAbility> gemAbility in this.gemAbilities)
    {
      if (gemAbility.Value.name == gem.name)
        return gemAbility.Key;
    }
    return (string) null;
  }

  private void CalculateScore()
  {
    MPDisease disease1 = this.world.diseases[0] as MPDisease;
    MPDisease disease2 = this.world.diseases[1] as MPDisease;
    disease1.currentScore = 0;
    disease2.currentScore = 0;
    if (disease1.totalInfected == 0L && disease2.totalInfected == 0L)
    {
      this.world.gameEndResult = IGame.EndGameResult.Infected;
      this.world.winner = (Disease) null;
      this.world.gameWon = false;
      this.world.gameEnded = true;
    }
    else if (disease2.totalInfected == 0L)
    {
      this.world.gameEndResult = (double) disease2.cureCompletePercent >= 1.0 ? IGame.EndGameResult.Cured : IGame.EndGameResult.Dead;
      this.world.winner = (Disease) disease1;
      this.world.gameWon = true;
      this.world.gameEnded = true;
    }
    else
    {
      if (disease1.totalInfected != 0L)
        return;
      this.world.gameEndResult = (double) disease1.cureCompletePercent >= 1.0 ? IGame.EndGameResult.Cured : IGame.EndGameResult.Dead;
      this.world.winner = (Disease) disease2;
      this.world.gameWon = true;
      this.world.gameEnded = true;
    }
  }

  public void SendEvoPoints(MPDisease disease, int currentPoints, int points, bool isGain)
  {
    if (this.IsReplayActive)
      return;
    if (!this.IsAIGame && this.network.IsServer)
      this.netView.RPC(NetworkChannel.Game, "RPCApplyBonusEvoPoints", RPCTarget.Others, (object) disease.id, (object) currentPoints, (object) points, (object) isGain);
    this.replayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, this.world.DiseaseTurn, this.world.eventTurn, (Disease) disease, points);
    if (!isGain)
      return;
    this.replayData.AddEvent(ReplayData.ReplayEventType.DNA_POINTS_GAINED, this.world.DiseaseTurn, this.world.eventTurn, (Disease) disease, points);
  }

  [PlagueRPC]
  private void RPCCreateGameSession()
  {
    this.CreateGame((Scenario) null, Disease.EDiseaseType.BACTERIA);
    this.networkView.RPC(NetworkChannel.Game, "RPCCreateGameSessionFinalised", RPCTarget.Host);
    this.FinaliseGameSession();
  }

  [PlagueRPC]
  private void RPCCreateGameSessionReady()
  {
    this.isClientGameSessionReady = true;
    this.CheckCreateGameSession();
  }

  [PlagueRPC]
  private void RPCCreateGameSessionFinalised() => this.FinaliseGameSession();

  [PlagueRPC]
  public void RPCChooseDisease(
    string diseaseName,
    string diseaseType,
    IPlayerInfo playerInfo,
    int cheatFlags)
  {
    if (playerInfo != null && playerInfo.disease == null)
    {
      Disease.EDiseaseType diseaseType1 = (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), diseaseType);
      this.AddDisease(diseaseName, diseaseType1, playerInfo, cheatFlags);
    }
    else
      Debug.LogError((object) "Could not execute RPCChooseDisease");
  }

  [PlagueRPC]
  public void RPCChooseGenes(string[] geneIDs, IPlayerInfo playerInfo)
  {
    string[] strArray1 = new HashSet<string>((IEnumerable<string>) geneIDs).ToArray<string>();
    if (strArray1.Length > this.MAX_GENES)
    {
      string[] strArray2 = new string[this.MAX_GENES];
      for (int index = 0; index < this.MAX_GENES; ++index)
        strArray2[index] = strArray1[index];
      strArray1 = strArray2;
    }
    this.geneStore.Add(playerInfo, strArray1);
  }

  [PlagueRPC]
  public void RPCSetGenes(string[] geneIDs, IPlayerInfo playerInfo)
  {
    Debug.Log((object) ("RPCSetGenes[" + this.network.IsServer.ToString() + "] - playerInfo:" + (object) playerInfo + (playerInfo != null ? (object) playerInfo.name : (object) "")));
    if (playerInfo != null && playerInfo.disease != null)
    {
      Gene[] newGenes = (Gene[]) null;
      string str = "";
      if (geneIDs != null)
      {
        newGenes = new Gene[geneIDs.Length];
        for (int index = 0; index < geneIDs.Length; ++index)
        {
          string id = geneIDs[index];
          Gene gene = this.availableGenes.Find((Predicate<Gene>) (g => g.id == id));
          newGenes[index] = gene;
          str = str + geneIDs[index] + "(" + (object) gene + "),";
        }
      }
      Debug.Log((object) ("-*-*-* SET GENES FOR " + playerInfo.name + " - " + (object) (newGenes == null ? 0 : newGenes.Length) + " = " + str));
      playerInfo.disease.ApplyGenes(newGenes);
    }
    else
      Debug.Log((object) ("RPCSetGenes[" + this.network.IsServer.ToString() + "] Could not execute RPCSetGenes [" + (playerInfo != null ? "true" : "false") + "][" + (playerInfo == null || playerInfo.disease == null ? "false" : "true") + "]"));
  }

  [PlagueRPC]
  public void RPCAddDisease(
    string diseaseName,
    string diseaseType,
    IPlayerInfo playerInfo,
    int cheatFlags)
  {
    Debug.Log((object) ("RPCAddDisease[" + this.network.IsServer.ToString() + "] - playerInfo:" + (object) playerInfo + (playerInfo != null ? (object) playerInfo.name : (object) "") + ", diseaseType:" + diseaseType));
    if (playerInfo != null)
    {
      base.AddDisease(diseaseName, (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), diseaseType), playerInfo, cheatFlags).showExtraPopups = false;
      if (playerInfo != CGameManager.localPlayerInfo)
        return;
      CUIManager.instance.SetActiveScreen("StartCountryScreen");
    }
    else
      Debug.LogError((object) "Could not execute RPCAddDisease");
  }

  [PlagueRPC]
  public void RPCSetCountry(IPlayerInfo playerInfo, string countryID)
  {
    this.replayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, "RPCSetCountry[" + this.network.IsServer.ToString() + "] - playerInfo:" + (object) playerInfo + (playerInfo != null ? (object) playerInfo.name : (object) ""));
    if (playerInfo == null)
      return;
    Country forCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    this.world.SetNexus(playerInfo.disease, forCountry);
  }

  [PlagueRPC]
  public void RPCChooseCountry(IPlayerInfo playerInfo, string countryID)
  {
    if (playerInfo != null)
    {
      if (playerInfo.disease == null)
        Debug.LogError((object) ("Tried to choose a disease nexus for " + playerInfo.name + " but no disease for that player"));
      else if (playerInfo.disease.nexus == null)
      {
        Country forCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
        if (forCountry != null)
        {
          if (!this.world.nexusCountries.Contains(forCountry))
          {
            this.world.SetNexus(playerInfo.disease, forCountry);
            this.netView.RPC(NetworkChannel.Game, "RPCSetCountry", RPCTarget.Others, (object) playerInfo, (object) countryID);
          }
          else
            Debug.LogError((object) ("Remote Player tried to choose '" + countryID + "' but already set to a nexus."));
        }
        else
          Debug.LogError((object) ("Remote Player tried to choose '" + countryID + "' but no country found."));
      }
      else
        Debug.Log((object) ("already chosen nexus: " + playerInfo.disease.nexus.name));
    }
    else
      Debug.LogError((object) "Could not execute RPCChooseCountry");
  }

  [PlagueRPC]
  public void RPCRegisterCountryInterest(IPlayerInfo playerInfo, string countryID)
  {
    Debug.Log((object) ("RPCRegisterCountryInterest[" + this.network.IsServer.ToString() + "] - playerInfo:" + (object) playerInfo + (playerInfo != null ? (object) playerInfo.name : (object) "")));
    if (playerInfo != null)
    {
      if (playerInfo.disease == null)
        Debug.LogError((object) ("Tried to choose a disease nexus for " + playerInfo.name + " but no disease for that player"));
      else if (!this.HasRegisteredInterest(playerInfo))
      {
        if (this.CanPickCountry(countryID))
        {
          if (!this.isFirstRound)
            this.netView.RPC(NetworkChannel.Game, "RPCStartNexusTimer", RPCTarget.All);
          this.RegisterInterest(countryID, playerInfo);
          this.CheckNexusPickRules(playerInfo);
        }
        else
        {
          this.netView.RPC(NetworkChannel.Game, "RPCDisplayNetworkPopup", playerInfo, (object) 2);
          Debug.LogError((object) ("Remote Player tried to choose '" + countryID + "' but its banned."));
        }
      }
      else
        this.netView.RPC(NetworkChannel.Game, "RPCDisplayNetworkPopup", playerInfo, (object) 4);
    }
    else
      Debug.LogError((object) "Could not execute RPCChooseCountry");
  }

  [PlagueRPC]
  public void RPCStartGame(int seed, int bonusIconCounter, int vehicleCounter)
  {
    Debug.Log((object) ("RPCStartGame[" + this.network.IsServer.ToString() + "]"));
    BonusIcon.ResetCounter(bonusIconCounter);
    Vehicle.ResetCounter(vehicleCounter);
    this.PostDiseaseSelectionLoad();
    this.DoStartGame(seed);
    if (this.IsReplayActive)
      return;
    Analytics.Event("MP CLIENT Disease Type", CGameManager.localPlayerInfo.disease.diseaseType.ToString());
    Analytics.Event("MP CLIENT Game Difficulty", CGameManager.localPlayerInfo.disease.difficulty.ToString());
    if (this.CurrentLoadedScenario == null)
      return;
    Analytics.Event("MP CLIENT Scenario", this.CurrentLoadedScenario.scenarioInformation.id);
  }

  [PlagueRPC]
  public void RPCEventUpdate(int seed, int turn, int gameTurn)
  {
    if (this.world == null)
    {
      Debug.LogError((object) "/EVENT/ RPC Event Update but world is null");
    }
    else
    {
      if (gameTurn != this.world.DiseaseTurn)
        Debug.LogError((object) ("/EVENT/ Event Phase out of turn: disease turn: [" + (object) this.world.DiseaseTurn + "] expected: [" + (object) gameTurn + "]"));
      if (turn != this.world.eventTurn + 1)
        Debug.LogError((object) ("/EVENT/ Event Phase out of turn: last turn: [" + (object) this.world.eventTurn + "] this turn: [" + (object) turn + "]"));
      this.replayData.EventTurn(seed);
      this.world.eventTurn = turn - 1;
      base.TriggerEventUpdate(seed);
    }
  }

  [PlagueRPC]
  public void RPCGameUpdate(int seed, int turn, string desyncHash)
  {
    if (this.isGameOutOfSync)
      return;
    if (this.world == null)
    {
      Debug.LogError((object) "/UPDATE/ RPC Game Update but world is null");
    }
    else
    {
      if (turn != this.world.DiseaseTurn + 1)
        Debug.LogError((object) ("/UPDATE/ Game Phase out of turn: last turn: " + (object) this.world.DiseaseTurn + " this turn: " + (object) turn));
      this.replayData.GameTurn(seed);
      this.world.DiseaseTurn = turn - 1;
      this.TriggerGameUpdate(seed);
      string s = "";
      for (int index = 0; index < this.world.diseases.Count; ++index)
        s = s + "|" + (object) this.world.diseases[index].totalControlledInfected + "|" + (object) this.world.diseases[index].evoPoints + "|" + (object) this.world.diseases[index].infectedPointsPot;
      byte[] bytes = Encoding.UTF8.GetBytes(s);
      if (!BitConverter.IsLittleEndian)
        Array.Reverse((Array) bytes);
      if (DataPacket.GenerateHash(bytes, HashingAlgorithm.SHA256).Substring(0, 4) != desyncHash)
      {
        this.isGameOutOfSync = true;
        this.netView.RPC(NetworkChannel.Game, "RPCRequestDesyncInfo", RPCTarget.Host, true);
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC - details should follow"));
      }
      else
        this.netView.RPC(NetworkChannel.Game, "RPCTurnReceived", RPCTarget.Host, true, (object) turn);
    }
  }

  [PlagueRPC]
  public void RPCTurnReceived(int turn) => this.lastClientTurn = turn;

  [PlagueRPC]
  public void RPCRequestDesyncInfo()
  {
    this.isGameOutOfSync = true;
    long[] numArray1 = new long[this.world.diseases.Count];
    int[] numArray2 = new int[this.world.diseases.Count];
    int[] numArray3 = new int[this.world.diseases.Count];
    float[] numArray4 = new float[this.world.diseases.Count];
    float[] numArray5 = new float[this.world.diseases.Count];
    int[] numArray6 = new int[this.world.diseases.Count];
    for (int index = 0; index < this.world.diseases.Count; ++index)
    {
      numArray1[index] = this.world.diseases[index].totalControlledInfected;
      numArray2[index] = this.world.diseases[index].evoPoints;
      numArray3[index] = this.world.diseases[index].evoBoost;
      numArray4[index] = this.world.diseases[index].infectedPointsPot;
      numArray5[index] = this.world.diseases[index].cureCompletePercent;
      numArray6[index] = this.world.diseases[index].dnaPointsGained;
    }
    this.netView.RPC(NetworkChannel.Game, "RPCReceiveDesyncInfo", RPCTarget.Others, true, (object) numArray1, (object) numArray2, (object) numArray3, (object) numArray4, (object) numArray5, (object) numArray6);
  }

  [PlagueRPC]
  public void RPCReceiveDesyncInfo(
    long[] infected,
    int[] evoPoints,
    int[] evoBoost,
    float[] infectedPointsPot,
    float[] cureCompletePercent,
    int[] dnaPointsGained)
  {
    for (int index = 0; index < this.world.diseases.Count; ++index)
    {
      if (this.world.diseases[index].totalControlledInfected != infected[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, INFECTED: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] INFECTED: [" + (object) this.world.diseases[index].totalControlledInfected + "] VS [" + (object) infected[index] + "]"));
      if (this.world.diseases[index].evoPoints != evoPoints[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, EVO POINTS: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] POINTS: [" + (object) this.world.diseases[index].evoPoints + "] VS [" + (object) evoPoints[index] + "]"));
      if (this.world.diseases[index].evoBoost != evoBoost[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, EVO BOOST: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] BOOST: [" + (object) this.world.diseases[index].evoBoost + "] VS [" + (object) evoBoost[index] + "]"));
      if ((double) this.world.diseases[index].infectedPointsPot != (double) infectedPointsPot[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, INFECTED POP POT: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] POT: [" + (object) this.world.diseases[index].infectedPointsPot + "] VS [" + (object) infectedPointsPot[index] + "]"));
      if ((double) this.world.diseases[index].cureCompletePercent != (double) cureCompletePercent[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, CURE COMPLETE PERC: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] PERCENTAGE: [" + (object) this.world.diseases[index].cureCompletePercent + "] VS [" + (object) cureCompletePercent[index] + "]"));
      if (this.world.diseases[index].dnaPointsGained != dnaPointsGained[index])
        Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC, DNA POINTS GAINED: [" + (object) index + "/" + this.world.diseases[index].diseaseName + "] POINTS GAINED: [" + (object) this.world.diseases[index].dnaPointsGained + "] VS [" + (object) dnaPointsGained[index] + "]"));
    }
  }

  [PlagueRPC]
  public void RPCEndGame() => this.EndGame(IGame.EndGameReason.COMPLETE);

  [PlagueRPC]
  public void RPCDroneSetSpawnPosition(int vehicleID, Vector3 position)
  {
    Vehicle vehicle = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == vehicleID));
    if (vehicle == null || vehicle.type != Vehicle.EVehicleType.Drone)
    {
      Debug.LogError((object) ("Drone spawn position set, but no drone found for id: " + (object) vehicleID + " v: " + (object) vehicle));
    }
    else
    {
      vehicle.currentPosition = new Vector3?(position);
      vehicle.sourcePosition = new Vector3?(position);
    }
  }

  [PlagueRPC]
  public void RPCVehicleArrived(int id)
  {
    Vehicle vehicle = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id));
    if (vehicle == null)
      return;
    if (vehicle.type == Vehicle.EVehicleType.Drone)
      Debug.LogError((object) "DRONES NOT CONFIGURED TO WORK ON MULTIPLAYER");
    if (vehicle.infectedTotal > 0)
    {
      string str = vehicle.infectedTotal.ToString() + " total ";
      if (vehicle.infected != null)
      {
        for (int id1 = 0; id1 < vehicle.infected.Length; ++id1)
          str = str + this.world.GetDisease(id1).name + "=" + (object) vehicle.infected[id1] + " ";
      }
    }
    this.world.VehicleArrived(vehicle);
    this.vehicles.Remove(vehicle);
    if (vehicle.infected == null)
      return;
    for (int diseaseID = 0; diseaseID < vehicle.infected.Length; ++diseaseID)
    {
      if (vehicle.infected[diseaseID] > 0)
        DiseaseTrailParticles.instance.SetDiseaseTransferRoute(diseaseID, vehicle.source, vehicle.destination, vehicle.type);
    }
  }

  [PlagueRPC]
  public void RPCBonusTryClick(int id, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      BonusIcon bonusIcon = this.bonusIcons.Find((Predicate<BonusIcon>) (a => a.id == id));
      if (bonusIcon != null && this.world.ClickBonusIcon(bonusIcon, playerInfo.disease))
      {
        if (bonusIcon.type != BonusIcon.EBonusIconType.NUKE)
          this.bonusIcons.Remove(bonusIcon);
        CInterfaceManager.instance.ForceClickBonus(bonusIcon, playerInfo.disease);
        this.replayData.AddEvent(ReplayData.ReplayEventType.BONUS_ICON_PRESSED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, bonusIcon.id);
        this.netView.RPC(NetworkChannel.Game, "RPCBonusClicked", RPCTarget.Others, (object) bonusIcon.id, (object) playerInfo);
      }
      else
        Debug.Log((object) ("CANNOT CLICK BONUS: " + (object) id + " ICON: " + (object) bonusIcon));
    }
    else
      Debug.LogError((object) "Could not execute RPCBonusTryClick");
  }

  [PlagueRPC]
  public void RPCBonusClicked(int bonusIconID, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      Debug.Log((object) ("RPC BONUS CLICK: " + (object) bonusIconID + " player: " + playerInfo.name));
      BonusIcon bonusIcon = this.bonusIcons.Find((Predicate<BonusIcon>) (a => a.id == bonusIconID));
      if (bonusIcon != null)
      {
        if (bonusIcon.type != BonusIcon.EBonusIconType.NUKE)
          this.bonusIcons.Remove(bonusIcon);
        CInterfaceManager.instance.ForceClickBonus(bonusIcon, playerInfo.disease);
        if (!this.world.ClickBonusIcon(bonusIcon, playerInfo.disease))
          Debug.LogError((object) ("Somehow RPCBonusClicked called for an invalid click: " + (object) bonusIcon));
        else
          this.replayData.AddEvent(ReplayData.ReplayEventType.BONUS_ICON_PRESSED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, bonusIcon.id);
      }
      else
        Debug.LogError((object) ("Somehow RPCBonusClicked[" + playerInfo.name + "] called for a nonexistant bonus: " + (object) bonusIconID));
    }
    else
      Debug.LogError((object) "Could not execute RPCBonusClicked");
  }

  [PlagueRPC]
  public void RPCStartBonusHidden(int bonusIconID)
  {
    BonusIcon bonusIcon = this.bonusIcons.Find((Predicate<BonusIcon>) (a => a.id == bonusIconID));
    if (bonusIcon != null)
      base.StartHideBonusIcon(bonusIcon);
    else
      Debug.Log((object) ("Tried to hide nonexistant bonus: " + (object) bonusIconID));
  }

  [PlagueRPC]
  public void RPCBonusHidden(int bonusIconID)
  {
    BonusIcon bonusIcon = this.bonusIcons.Find((Predicate<BonusIcon>) (a => a.id == bonusIconID));
    if (bonusIcon != null)
    {
      base.HideBonusIcon(bonusIcon);
      Debug.LogWarning((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " hide bonus: " + (object) bonusIconID));
      CInterfaceManager.instance.HideBonus(bonusIcon);
    }
    else
      Debug.Log((object) ("Tried to hide nonexistant bonus: " + (object) bonusIconID));
  }

  [PlagueRPC]
  public void RPCTryDeEvolveTech(string technologyID, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      Technology technology = playerInfo.disease.GetTechnology(technologyID);
      if (technology != null && playerInfo.disease.CanDeEvolve(technology) && playerInfo.disease.GetDeEvolveCost(technology) <= playerInfo.disease.evoPoints)
      {
        playerInfo.disease.DeEvolveTech(technology);
        Debug.Log((object) (playerInfo.name + " de-evolved " + technologyID));
        CInterfaceManager.instance.UpdateInterface();
        this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_DEEVOLVED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, technology.id);
        this.netView.RPC(NetworkChannel.Game, "RPCDeEvolveTech", RPCTarget.Others, true, (object) technology.id, (object) playerInfo);
      }
      else
        Debug.LogError((object) ("Client tried to de-evolve " + this.name + " but " + (technology == null ? "Null tech" : "Cannot de-evolve")));
    }
    else
      Debug.LogError((object) "Could not execute RPCTryDeEvolveTech");
  }

  [PlagueRPC]
  public void RPCDeEvolveTech(string technologyID, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      if (playerInfo.disease == this.network.LocalPlayerInfo.disease)
      {
        if (!this.requestedDeEvolutions.ContainsKey(technologyID))
        {
          Debug.LogError((object) ("Received de-evolution RPC for '" + technologyID + "' on our own disease - which we did not request - DESYNC"));
          this.isGameOutOfSync = true;
          this.netView.RPC(NetworkChannel.Game, "RPCRequestDesyncInfo", RPCTarget.Host, true);
          Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC - details should follow"));
          return;
        }
        this.requestedDeEvolutions.Remove(technologyID);
      }
      Technology technology = playerInfo.disease.GetTechnology(technologyID);
      if (technology != null)
      {
        Debug.Log((object) (playerInfo.name + " de-evolved " + technologyID));
        playerInfo.disease.DeEvolveTech(technology);
        this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_DEEVOLVED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, technology.id);
        CInterfaceManager.instance.UpdateInterface();
        if (playerInfo != CGameManager.localPlayerInfo)
          return;
        CDiseaseScreen currentScreen = CUIManager.instance.GetCurrentScreen() as CDiseaseScreen;
        if (!((UnityEngine.Object) currentScreen != (UnityEngine.Object) null))
          return;
        currentScreen.EndEvolve(technology);
      }
      else
        Debug.LogError((object) ("error de-evolving " + this.name + " tech not found"));
    }
    else
      Debug.LogError((object) "Could not execute RPCDeEvolveTech");
  }

  [PlagueRPC]
  public void RPCTryEvolveTech(string technologyID, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      Technology technology = playerInfo.disease.GetTechnology(technologyID);
      if (technology != null && playerInfo.disease.CanEvolve(technology) && !playerInfo.disease.techEvolved.Contains(technologyID) && playerInfo.disease.GetEvolveCost(technology) <= playerInfo.disease.evoPoints)
      {
        playerInfo.disease.EvolveTech(technology, false);
        Debug.Log((object) (playerInfo.name + " evolved " + technologyID));
        CInterfaceManager.instance.UpdateInterface();
        Debug.Log((object) ("_______[2]RECORD TECH_EVOLVED - player.disease:" + playerInfo.disease.name + " evolved:" + technology.id));
        this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_EVOLVED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, technology.id);
        if (this.IsAIGame)
          return;
        this.netView.RPC(NetworkChannel.Game, "RPCEvolveTech", RPCTarget.Others, true, (object) technology.id, (object) playerInfo, (object) World.instance.DiseaseTurn, (object) World.instance.eventTurn);
      }
      else
      {
        string str1 = technologyID;
        string str2;
        if (technology != null)
          str2 = "Cannot evolve Cost:[" + (object) playerInfo.disease.GetEvolveCost(technology) + "] Has:[" + (object) playerInfo.disease.evoPoints + "]";
        else
          str2 = "Null tech";
        Debug.LogError((object) ("Client tried to evolve " + str1 + " but " + str2));
      }
    }
    else
      Debug.LogError((object) "Could not execute RPCTryEvolveTech");
  }

  [PlagueRPC]
  public void RPCEvolveTech(
    string technologyID,
    IPlayerInfo playerInfo,
    int diseaseTurn,
    int eventTurn)
  {
    if (playerInfo != null)
    {
      if (playerInfo.disease == this.network.LocalPlayerInfo.disease)
      {
        if (!this.requestedEvolutions.ContainsKey(technologyID))
        {
          Debug.LogError((object) ("Received de-evolution RPC for '" + technologyID + "' on our own disease - which we did not request - DESYNC"));
          this.isGameOutOfSync = true;
          this.netView.RPC(NetworkChannel.Game, "RPCRequestDesyncInfo", RPCTarget.Host, true);
          Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] - OUT OF SYNC - details should follow"));
          return;
        }
        this.requestedEvolutions.Remove(technologyID);
      }
      if (this.world.DiseaseTurn != diseaseTurn || this.world.eventTurn != eventTurn)
        Debug.LogError((object) (playerInfo.name + " evolved tech '" + technologyID + "turn mismatch: " + (object) diseaseTurn + "/" + (object) eventTurn + " on host, " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn + " on client"));
      Technology technology = playerInfo.disease.GetTechnology(technologyID);
      if (technology != null)
      {
        playerInfo.disease.EvolveTech(technology, false);
        Debug.Log((object) (playerInfo.name + " evolved " + technologyID + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn));
        Debug.Log((object) ("_______[3]RECORD TECH_EVOLVED - player.disease:" + playerInfo.disease.name + " evolved:" + technology.id));
        this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_EVOLVED, this.world.DiseaseTurn, this.world.eventTurn, playerInfo.disease, technology.id);
        CInterfaceManager.instance.UpdateInterface();
        if (playerInfo != CGameManager.localPlayerInfo)
          return;
        CDiseaseScreen currentScreen = CUIManager.instance.GetCurrentScreen() as CDiseaseScreen;
        if (!((UnityEngine.Object) currentScreen != (UnityEngine.Object) null))
          return;
        currentScreen.EndEvolve(technology);
      }
      else
        Debug.LogError((object) ("error de-evolving " + this.name + " tech not found"));
    }
    else
      Debug.LogError((object) "Could not execute RPCEvolveTech");
  }

  [PlagueRPC]
  public void RPCTryCreateGem(
    string gemType,
    string countryID,
    Vector3 pos,
    IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      GemAbility gemAbility = this.gemAbilities[gemType];
      Country c = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
      Disease disease = playerInfo.disease;
      if (gemAbility != null && c != null && disease != null && disease.evoPoints >= gemAbility.cost)
      {
        disease.SpendEvoPoints(gemAbility.cost);
        ((MPWorld) this.world).CreateGemEffect(gemAbility, disease, c, new Vector3?(pos));
        Debug.Log((object) (playerInfo.name + " created gem " + gemType + " : " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn));
        CInterfaceManager.instance.UpdateInterface();
        this.netView.RPC(NetworkChannel.Game, "RPCCreateGem", RPCTarget.Others, (object) gemType, (object) c.id, (object) pos, (object) playerInfo, (object) World.instance.DiseaseTurn, (object) World.instance.eventTurn);
      }
      else
        Debug.LogError((object) ("Client tried to create gem " + gemType + " but Null gem type"));
    }
    else
      Debug.LogError((object) "Could not execute RPCTryCreateGem");
  }

  [PlagueRPC]
  public void RPCCreateGem(
    string gemType,
    string countryID,
    Vector3 pos,
    IPlayerInfo playerInfo,
    int diseaseTurn,
    int eventTurn)
  {
    if (playerInfo != null)
    {
      if (this.world.DiseaseTurn != diseaseTurn || this.world.eventTurn != eventTurn)
        Debug.LogError((object) (playerInfo.name + " create gem '" + gemType + "turn mismatch: " + (object) diseaseTurn + "/" + (object) eventTurn + " on host, " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn + " on client"));
      GemAbility gemAbility = this.gemAbilities[gemType];
      Country c = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
      Disease disease = playerInfo.disease;
      if (gemAbility != null && c != null && disease != null && disease.evoPoints >= gemAbility.cost)
      {
        disease.SpendEvoPoints(gemAbility.cost);
        ((MPWorld) this.world).CreateGemEffect(gemAbility, disease, c, new Vector3?(pos));
        Debug.Log((object) (playerInfo.name + " created gem " + gemType + " : " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn));
        CInterfaceManager.instance.UpdateInterface();
      }
      else
        Debug.LogError((object) ("error creating gem " + gemType + " gem not found"));
    }
    else
      Debug.LogError((object) "Could not execute RPCCreateGem");
  }

  [PlagueRPC]
  public void RPCTryMoveGem(int gemID, string countryID)
  {
    MPWorld world = (MPWorld) this.world;
    GemEffect gem = world.activeGems.Find((Predicate<GemEffect>) (g => g.id == gemID));
    Country to = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    if (gem != null && to != null)
    {
      world.MoveGemEffect(gem, to);
      Debug.Log((object) ("Move gem " + (object) gemID + " : " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn));
      CInterfaceManager.instance.UpdateInterface();
      this.netView.RPC(NetworkChannel.Game, "RPCMoveGem", RPCTarget.Others, (object) gemID, (object) countryID, (object) World.instance.DiseaseTurn, (object) World.instance.eventTurn);
    }
    else
      Debug.LogError((object) ("Client tried to move gem " + (object) gemID + " but could not find gem/country"));
  }

  [PlagueRPC]
  public void RPCMoveGem(int gemID, string countryID, int diseaseTurn, int eventTurn)
  {
    if (this.world.DiseaseTurn != diseaseTurn || this.world.eventTurn != eventTurn)
      Debug.LogError((object) ("Move gem '" + (object) gemID + " turn mismatch: " + (object) diseaseTurn + "/" + (object) eventTurn + " on host, " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn + " on client"));
    MPWorld world = (MPWorld) this.world;
    GemEffect gem = world.activeGems.Find((Predicate<GemEffect>) (g => g.id == gemID));
    Country to = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
    if (gem != null && to != null)
    {
      world.MoveGemEffect(gem, to);
      Debug.Log((object) ("Move gem " + (object) gemID + " : " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn));
      CInterfaceManager.instance.UpdateInterface();
    }
    else
      Debug.LogError((object) ("error moving gem " + (object) gemID + " gem not found"));
  }

  [PlagueRPC]
  public void RPCTryCreateUnscheduledFlight(
    string sourceID,
    string destinationID,
    Vector3 sourcePosition,
    Vector3 endPosition,
    IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      Country fromCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == sourceID));
      Country toCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == destinationID));
      int num = this.ProcessUnscheduledFlight(playerInfo.disease, fromCountry, toCountry, sourcePosition, endPosition, -1);
      if (num <= 0)
        return;
      this.netView.RPC(NetworkChannel.Game, "RPCCreateUnscheduledFlight", RPCTarget.Others, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) num, (object) playerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      Debug.LogError((object) "Could not execute RPCTryCreateUnscheduledFlight");
  }

  [PlagueRPC]
  public void RPCCreateUnscheduledFlight(
    string sourceID,
    string destinationID,
    Vector3 sourcePosition,
    Vector3 endPosition,
    int infected,
    IPlayerInfo playerInfo,
    int diseaseTurn,
    int eventTurn)
  {
    if (playerInfo != null)
    {
      if (this.world.DiseaseTurn != diseaseTurn || this.world.eventTurn != eventTurn)
        Debug.LogError((object) (playerInfo.name + " create unscheduled flight - turn mismatch: " + (object) diseaseTurn + "/" + (object) eventTurn + " on host, " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn + " on client"));
      Country fromCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == sourceID));
      Country toCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == destinationID));
      this.ProcessUnscheduledFlight(playerInfo.disease, fromCountry, toCountry, sourcePosition, endPosition, infected);
    }
    else
      Debug.LogError((object) "Could not execute RPCCreateUnscheduledFlight");
  }

  [PlagueRPC]
  public void RPCTryCreateNukeLaunch(
    string sourceID,
    string destinationID,
    Vector3 sourcePosition,
    Vector3 endPosition,
    IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      Country fromCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == sourceID));
      Country toCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == destinationID));
      this.ProcessNukeLaunch(playerInfo.disease, fromCountry, toCountry, sourcePosition, endPosition);
      this.netView.RPC(NetworkChannel.Game, "RPCCreateNukeLaunch", RPCTarget.Others, true, (object) fromCountry.id, (object) toCountry.id, (object) sourcePosition, (object) endPosition, (object) playerInfo, (object) this.world.DiseaseTurn, (object) this.world.eventTurn);
    }
    else
      Debug.LogError((object) "Could not execute RPCTryCreateNukeLaunch");
  }

  [PlagueRPC]
  public void RPCCreateNukeLaunch(
    string sourceID,
    string destinationID,
    Vector3 sourcePosition,
    Vector3 endPosition,
    IPlayerInfo playerInfo,
    int diseaseTurn,
    int eventTurn)
  {
    if (playerInfo != null)
    {
      if (this.world.DiseaseTurn != diseaseTurn || this.world.eventTurn != eventTurn)
        Debug.LogError((object) (playerInfo.name + " create nuke launch - turn mismatch: " + (object) diseaseTurn + "/" + (object) eventTurn + " on host, " + (object) this.world.DiseaseTurn + "/" + (object) this.world.eventTurn + " on client"));
      Country fromCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == sourceID));
      Country toCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == destinationID));
      this.ProcessNukeLaunch(playerInfo.disease, fromCountry, toCountry, sourcePosition, endPosition);
    }
    else
      Debug.LogError((object) "Could not execute RPCCreateNukeLaunch");
  }

  [PlagueRPC]
  private void RPCSetSpeed(int gameSpeed, IPlayerInfo playerInfo)
  {
    if (playerInfo != null)
    {
      playerInfo.gameSpeed = gameSpeed;
      this.CheckNetSpeed();
    }
    else
      Debug.LogError((object) "Could not execute RPCSetSpeed");
  }

  [PlagueRPC]
  private void RPCDisplayNetworkPopup(int messageCode)
  {
    MultiplayerGame.MultiplayerMessage messageCode1 = (MultiplayerGame.MultiplayerMessage) messageCode;
    this.DisplayNetworkPopup(messageCode1);
    if (!CNetworkManager.network.IsServer && messageCode1 == MultiplayerGame.MultiplayerMessage.PickedDuplicates)
      this.currentCountryPick.Clear();
    if (!CNetworkManager.network.IsServer && messageCode1 == MultiplayerGame.MultiplayerMessage.PickedBanned)
      this.UnregisterInterest(this.network.LocalPlayerInfo);
    if (CNetworkManager.network.IsServer || messageCode1 != MultiplayerGame.MultiplayerMessage.RandomPicks)
      return;
    this.forcedRandomPick = true;
  }

  [PlagueRPC]
  private void RPCResetPickTimer()
  {
    this.nexusPickTimer = 60f;
    this.isFirstRound = this.requestedRandom = false;
  }

  [PlagueRPC]
  private void RPCStartNexusTimer()
  {
    this.DisplayTimerMessage(!this.HasRegisteredInterest(CNetworkManager.network.LocalPlayerInfo));
    this.hasTimerStarted = true;
  }

  [PlagueRPC]
  private void RPCApplyBonusEvoPoints(
    int disease,
    int currentPoints,
    int bonusEvoPoints,
    bool isGain)
  {
    Disease d = this.world.diseases.Find((Predicate<Disease>) (a => a.id == disease));
    if (d.evoPoints != currentPoints)
      Debug.LogError((object) ("/SYNC/ [" + (object) this.world.DiseaseTurn + "]/[" + (object) this.world.eventTurn + "] EvoPoints mismatch. Have [" + (object) d.evoPoints + "] and expected [" + (object) currentPoints + "]"));
    d.evoPoints += bonusEvoPoints;
    this.replayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, this.world.DiseaseTurn, this.world.eventTurn, d, bonusEvoPoints);
    if (!isGain)
      return;
    d.dnaPointsGained += bonusEvoPoints;
    this.replayData.AddEvent(ReplayData.ReplayEventType.DNA_POINTS_GAINED, this.world.DiseaseTurn, this.world.eventTurn, d, bonusEvoPoints);
  }

  [PlagueRPC]
  private void RPCToggleReady(bool val, string diseaseName, int diseaseType, string[] geneIDs)
  {
    Debug.Log((object) "MultiplayerGame.RPCToggleReady");
    UnityEngine.Object.FindObjectOfType<MultiplayerLobbyScreen>().HandleOtherToggleReady(val, diseaseName, diseaseType, geneIDs);
  }

  private IEnumerator SendAnalytics()
  {
    MultiplayerGame multiplayerGame = this;
    while (true)
    {
      if (!multiplayerGame.IsReplayActive && !multiplayerGame.IsAIGame)
        NdemicAnalytics.PostEvent(NdemicAnalytics.EventTypePost.SendPing);
      yield return (object) new WaitForSeconds(10f);
    }
  }

  [PlagueRPC]
  private void RPCReceiveChat(IPlayerInfo player, string message)
  {
    this.AddChatMessage(player, message);
  }

  public override void SendChat(string message)
  {
    this.netView.RPC(NetworkChannel.Game, "RPCReceiveChat", RPCTarget.Others, true, (object) this.myPlayer, (object) message);
  }

  private IEnumerator SendPing()
  {
    while (true)
    {
      this.currentPing = DateTime.Now;
      this.netView.RPC(NetworkChannel.Game, "RPCRequestPing", RPCTarget.Others, true);
      yield return (object) new WaitForSeconds(5f);
    }
  }

  [PlagueRPC]
  private void RPCRequestPing()
  {
    this.netView.RPC(NetworkChannel.Game, "RPCReceiveHeartbeat", RPCTarget.Others, true);
  }

  [PlagueRPC]
  private void RPCReceiveHeartbeat()
  {
    if (this.currentPing == DateTime.MinValue)
    {
      this.currentPing = DateTime.Now;
    }
    else
    {
      this.lastPing = this.currentPing;
      this.currentPing = DateTime.Now;
      this.ping = (this.currentPing - this.lastPing).TotalMilliseconds;
      this.weightedPing += this.ping;
      ++this.pingSamples;
      if (this.pingSamples < 1)
        return;
      this.averagePing = this.weightedPing / 1.0 / 2.0;
      this.weightedPing = 0.0;
      this.pingSamples = 0;
    }
  }

  public void ForceDraw()
  {
  }

  [PlagueRPC]
  private void RPCForceDraw()
  {
  }

  [PlagueRPC]
  private void RPCTryForceDraw()
  {
  }

  public enum ResultType
  {
    WIN,
    LOSE,
    DRAW,
  }

  public enum MultiplayerMessage
  {
    WaitingForPick,
    PickedDuplicates,
    PickedBanned,
    RandomPicks,
    AlreadyPicked,
  }
}
