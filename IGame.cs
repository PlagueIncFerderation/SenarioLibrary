// Decompiled with JetBrains decompiler
// Type: IGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public abstract class IGame : MonoBehaviour
{
  internal World world;
  protected Scenario currentLoadedScenario;
  protected Dictionary<EAbilityType, ActiveAbility> abilities;
  protected IGame.GameSetupParameters setupParameters;
  protected List<Gene> availableGenes;
  protected IGame.GameHistory gameHistory;
  protected IGame.GameState gameState;
  protected IGame.EndGameReason endGameReason;
  protected ReplayData replayData;
  protected List<EndTurnData> endTurnDatas;
  protected int _actualSpeed;
  protected int wantedSpeed;
  protected float secondsPerTurn;
  protected float secondsPerEvent;
  protected TaskHandler taskHandler;
  protected List<Vehicle> vehicles;
  protected List<BonusIcon> bonusIcons;
  protected Disease.ECheatType[] cheatsUsed;
  protected int replayTurnCounter;
  protected float timeGameSessionStarted;
  protected bool isGameSessionInterrupted;
  protected float timer;
  protected float eventTimer;
  internal DiseaseColourSet[] diseaseColourSets;
  internal DiseaseColourSet[] zombieDiseaseColourSets;
  internal DiseaseColourSet[] neuraxDiseaseColourSets;
  internal DiseaseColourSet[] simianDiseaseColourSets;
  internal DiseaseColourSet[] vampireDiseaseColourSets;
  internal DiseaseColourSet[] cureDiseaseColourSets;
  internal int initialGameSeed;
  internal int firstTurnSinceLoad;
  public Action<ChatEntry> onChatAdded;
  public List<ChatEntry> chatHistory;
  private bool hasNotifiedOpponentLeft;
  protected int currentThreshold;
  protected List<float> threshold;
  protected GameParameters gameParameters;
  protected int previousRecordedGameTime;
  public DiseaseSimulator diseaseSimulator;
  private float tutorialCureTimer;
  public bool tutorialCureCheck;
  protected ulong lastTotalDead;
  protected ulong lastSum;
  public static bool showMoreDetail;
  public static string constPath = CGameManager.federalServerAddress + "ScenarioConfig/ConstList.txt";

  protected int actualSpeed
  {
    get => this._actualSpeed;
    set
    {
      if (value > 3 && !this.IsReplayActive && !CGameManager.IsFederalScenario("PISMG"))
        this.EndGame(IGame.EndGameReason.DESYNC);
      else
        this._actualSpeed = value;
    }
  }

  public bool isChatMuted { get; private set; }

  public World WorldInstance => this.world;

  public List<Vehicle> ActiveVehicles => this.vehicles;

  public List<BonusIcon> ActiveBonusIcons => this.bonusIcons;

  public uint Difficulty { get; set; }

  public List<Gene> AvailableGenes => this.availableGenes;

  public virtual int NumberOfPlayers => 1;

  public bool IsReplayActive { get; set; }

  public Dictionary<EAbilityType, ActiveAbility> Abilities => this.abilities;

  public ActiveAbility GetActiveAbility(EAbilityType type)
  {
    return this.Abilities.ContainsKey(type) ? this.Abilities[type] : (ActiveAbility) null;
  }

  public Scenario CurrentLoadedScenario
  {
    set => this.currentLoadedScenario = value;
    get => this.currentLoadedScenario;
  }

  public IGame.GameSetupParameters SetupParameters => this.setupParameters;

  public bool IsPaused => this.actualSpeed == 0;

  public IGame.GameState CurrentGameState => this.gameState;

  public int WantedSpeed => this.wantedSpeed;

  public int ActualSpeed => this.actualSpeed;

  public float DeltaGameTime => Time.deltaTime * (float) this.actualSpeed;

  public bool HasReplayData => this.replayData != null && this.replayData.hasData;

  public ReplayData ReplayData => this.replayData;

  public virtual INetworkView networkView => (INetworkView) null;

  public static IGame.GameType ParseGameType(string str)
  {
    IGame.GameType[] values = (IGame.GameType[]) Enum.GetValues(typeof (IGame.GameType));
    for (int index = 0; index < values.Length; ++index)
    {
      if (values[index].ToString().ToUpperInvariant() == str.ToUpperInvariant())
        return values[index];
    }
    Debug.LogWarning((object) ("Unable to parse game type '" + str + "' as IGame.GameType"));
    return IGame.GameType.Invalid;
  }

  public abstract ISaves.SaveMetaData SaveGame(int slotID);

  public abstract void EndGame(IGame.EndGameReason reason);

  public abstract void UpdateAchievements();

  public abstract void SetSpeed(int speed);

  private IEnumerator FinishedInitializing()
  {
    yield return (object) new WaitForEndOfFrame();
    CGameManager.canPlaySFX = true;
  }

  public virtual void Initialise()
  {
    CGameManager.spaceTime = false;
    this.StartCoroutine(this.FinishedInitializing());
    this.setupParameters = new IGame.GameSetupParameters();
    this.wantedSpeed = this.actualSpeed = 1;
    this.gameHistory = new IGame.GameHistory();
    this.taskHandler = TaskHandler.Instance;
    this.IsReplayActive = false;
    this.timer = 0.0f;
    this.eventTimer = 0.0f;
    this.replayData = new ReplayData();
  }

  public virtual void RecoverGameState(string fromState, string replayState)
  {
    CGameManager.spaceTime = false;
    if (string.IsNullOrEmpty(fromState))
      return;
    this.LoadGameState(fromState, replayState);
    CGameManager.localPlayerInfo.disease = this.world.diseases[0];
  }

  public virtual void LoadGameState(string withState, string replayState)
  {
    CGameManager.spaceTime = false;
    this.replayData = new ReplayData();
    this.gameHistory = new IGame.GameHistory();
    this.endTurnDatas.Clear();
    List<Vehicle> vehicles = new List<Vehicle>();
    List<BonusIcon> bonusIcons = new List<BonusIcon>();
    this.previousRecordedGameTime = 0;
    if (!string.IsNullOrEmpty(replayState))
    {
      GameSerializer.UnserializeReplay(this.replayData, replayState);
      GameSerializer.LoadGame(withState, this.world, (ReplayData) null, this.gameHistory, this.availableGenes, ref vehicles, ref bonusIcons, ref this.diseaseSimulator, out this.previousRecordedGameTime, out this.initialGameSeed);
    }
    else
      GameSerializer.LoadGame(withState, this.world, this.replayData, this.gameHistory, this.availableGenes, ref vehicles, ref bonusIcons, ref this.diseaseSimulator, out this.previousRecordedGameTime, out this.initialGameSeed);
    this.firstTurnSinceLoad = this.world.DiseaseTurn;
    for (int index = 0; index < this.replayData.diseases.Count; ++index)
    {
      this.replayData.diseases[index].shuffleMap = this.world.diseases[index].shuffleMap;
      this.replayData.diseases[index].shuffleStartMap = this.world.diseases[index].shuffleStartMap;
    }
    foreach (Disease disease in this.world.diseases)
    {
      string diseaseFile = DataImporter.GetDiseaseFile(disease.diseaseType);
      if (this.CurrentLoadedScenario == null)
      {
        CGameManager.ImportDiseaseTech(disease, CGameManager.LoadGameText("Disease/" + diseaseFile), true, (Scenario) null);
      }
      else
      {
        disease.victoryTechId = this.CurrentLoadedScenario.scenarioInformation.scenGameWinTech;
        if (disease.victoryTechId == "0")
          disease.victoryTechId = (string) null;
        if (disease.victoryTechId != null)
        {
          disease.endMessageImage = this.CurrentLoadedScenario.scenarioInformation.scenEndMessageImage;
          ParameterisedString parameterisedString1 = new ParameterisedString(this.CurrentLoadedScenario.scenarioInformation.scenEndMessageText, new string[1]
          {
            "disease.name"
          });
          ParameterisedString parameterisedString2 = new ParameterisedString(this.CurrentLoadedScenario.scenarioInformation.scenEndMessageTitle, new string[1]
          {
            "disease.name"
          });
          disease.endMessageText = parameterisedString1;
          disease.endMessageTitle = parameterisedString2;
        }
        Debug.Log((object) ("Load Game: " + disease.victoryTechId));
        bool flag = !this.CurrentLoadedScenario.isOfficial;
        Debug.Log((object) ("CUSTOM :" + flag.ToString()));
        if (this.CurrentLoadedScenario.FileExists(flag ? "disease" : diseaseFile))
        {
          Debug.Log((object) ("Scenario has file for: " + (flag ? "disease" : diseaseFile)));
          CGameManager.ImportDiseaseTech(disease, this.CurrentLoadedScenario.ReadText(flag ? "disease" : diseaseFile), true, this.CurrentLoadedScenario);
        }
        else if (!string.IsNullOrEmpty(this.CurrentLoadedScenario.diseaseData))
        {
          CGameManager.ImportDiseaseTech(disease, this.CurrentLoadedScenario.diseaseData, true, this.CurrentLoadedScenario);
        }
        else
        {
          Debug.Log((object) ("Scenario missing file for: " + (flag ? "disease" : diseaseFile)));
          CGameManager.ImportDiseaseTech(disease, CGameManager.LoadGameText("Disease/" + diseaseFile), true, this.CurrentLoadedScenario);
        }
      }
      if (disease.hasInitialTechLock)
      {
        foreach (Technology technology in disease.technologies)
          technology.eventLocked = false;
        foreach (string techID in disease.initialTechLock)
        {
          Technology technology = disease.GetTechnology(techID);
          if (technology != null)
            technology.eventLocked = true;
          else
            Debug.Log((object) ("COULD NOT INITIAL TECH LOCK: '" + techID + "' TECH TOTAL: " + (object) disease.technologies.Count));
        }
        disease.hasInitialTechLock = false;
        disease.initialTechLock.Clear();
      }
      if (disease.hasInitialTechPadlock)
      {
        foreach (Technology technology in disease.technologies)
          technology.padlocked = false;
        foreach (string techID in disease.initialTechPadlock)
          disease.GetTechnology(techID).padlocked = true;
        disease.hasInitialTechPadlock = false;
        disease.initialTechPadlock.Clear();
      }
      if (disease.HasCheat(Disease.ECheatType.SHUFFLE) || disease.HasCheat(Disease.ECheatType.CURE_SHUFFLE))
        DataImporter.ApplyShuffle(disease);
    }
    Debug.Log((object) ("LOADED VEHICLES: " + (object) vehicles.Count));
    if (vehicles.Count > 0)
    {
      foreach (Vehicle vehicle in vehicles)
      {
        Debug.Log((object) ("LOOADED: " + (object) vehicle.id + " type: " + (object) vehicle.type + " sub: " + (object) vehicle.subType));
        CInterfaceManager.instance.SpawnVehicle(vehicle);
        this.vehicles.Add(vehicle);
        if (vehicle.type == Vehicle.EVehicleType.ZombieHorde)
          this.world.TrackHorde(vehicle);
        if (vehicle.type == Vehicle.EVehicleType.Drone)
          vehicle.currentCountry.GetLocalDisease(vehicle.actingDisease).hasDrone = true;
      }
    }
    Debug.Log((object) ("LOADED BONUS ICONS: " + (object) bonusIcons.Count));
    if (bonusIcons.Count > 0)
    {
      this.bonusIcons.AddRange((IEnumerable<BonusIcon>) bonusIcons);
      CInterfaceManager.instance.SpawnBonuses(bonusIcons);
    }
    this.PostDiseaseSelectionLoad();
  }

  public virtual string GetSerializedGameState()
  {
    return GameSerializer.Save(this, this.world, this.gameHistory, Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted), false);
  }

  public virtual DiseaseColourSet GetColourSet(Disease disease)
  {
    if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
      return (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], (Country) null, (LocalDisease) null);
    if (disease.diseaseType == Disease.EDiseaseType.NECROA)
      return this.zombieDiseaseColourSets[disease.id % this.zombieDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.NEURAX)
      return this.neuraxDiseaseColourSets[disease.id % this.neuraxDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      return this.simianDiseaseColourSets[disease.id % this.simianDiseaseColourSets.Length];
    if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      return this.vampireDiseaseColourSets[disease.id % this.vampireDiseaseColourSets.Length];
    return disease.diseaseType == Disease.EDiseaseType.CURE ? this.cureDiseaseColourSets[disease.id % this.cureDiseaseColourSets.Length] : this.diseaseColourSets[disease.id % this.diseaseColourSets.Length];
  }

  public virtual DiseaseColourSet GetColourSet(int diseaseID)
  {
    return this.GetColourSet(this.world.GetDisease(diseaseID));
  }

  public virtual World CreateWorld() => (World) null;

  public virtual void CreateGame(Scenario withScenario = null, Disease.EDiseaseType overrideDiseaseType = Disease.EDiseaseType.BACTERIA)
  {
    CGameManager.spaceTime = false;
    this.bonusIcons.Clear();
    this.vehicles.Clear();
    if (withScenario != null && withScenario.scenarioInformation.id.Contains("PIFCURE"))
    {
      Debug.Log((object) ("With scenario: " + withScenario.scenarioInformation.id));
      CGameManager.gameType = IGame.GameType.Cure;
    }
    if (!this.IsReplayActive)
      this.initialGameSeed = UnityEngine.Random.seed = Mathf.FloorToInt(UnityEngine.Random.value * (float) int.MaxValue);
    else
      UnityEngine.Random.seed = this.initialGameSeed;
    if (CGameManager.IsCureGame)
      CUIManager.instance.AddScreenSubstitution("PauseScreen", "PauseScreen_Cure");
    else
      CUIManager.instance.ClearScreenSubstitutions();
    switch (CGameManager.gameType)
    {
      case IGame.GameType.SpeedRun:
        withScenario = (Scenario) null;
        this.setupParameters.defaultDiseaseType = overrideDiseaseType;
        this.setupParameters.lockDiseaseType = true;
        this.setupParameters.lockDifficulty = true;
        this.setupParameters.lockAllCheats = false;
        this.setupParameters.difficulty = "normal";
        break;
      case IGame.GameType.Tutorial:
        withScenario = (Scenario) null;
        this.setupParameters = new IGame.GameSetupParameters();
        this.setupParameters.defaultDiseaseType = overrideDiseaseType;
        this.setupParameters.lockDiseaseType = true;
        this.setupParameters.lockAllGenes = true;
        this.setupParameters.lockDifficulty = true;
        this.setupParameters.lockAllCheats = true;
        this.setupParameters.difficulty = "casual";
        break;
      case IGame.GameType.CureTutorial:
        withScenario = (Scenario) null;
        this.setupParameters = new IGame.GameSetupParameters();
        this.setupParameters.defaultDiseaseType = Disease.EDiseaseType.CURE;
        this.setupParameters.lockDiseaseType = true;
        this.setupParameters.lockAllGenes = true;
        this.setupParameters.lockDifficulty = true;
        this.setupParameters.lockAllCheats = true;
        this.setupParameters.difficulty = "casual";
        this.tutorialCureCheck = false;
        this.tutorialCureTimer = 0.0f;
        break;
    }
    this.CurrentLoadedScenario = withScenario;
    if (this.world != null)
      this.world.Clear();
    this.world = this.CreateWorld();
    this.world = World.Initialise(this.world, this.CurrentLoadedScenario, CGameManager.LoadGameText("World/countries"), CGameManager.LoadGameText("World/routes"), CGameManager.LoadGameText("World/countryorder"), CGameManager.LoadGameText("World/petridish_symptoms"));
    if (CGameManager.IsCureGame)
      this.world.gameSpeedModifier = 0.75f;
    IGame.GameSetupParameters setupParameters1 = this.setupParameters;
    DateTime dateTime = DateTime.Now;
    long ticks1 = dateTime.Ticks;
    setupParameters1.startDate = ticks1;
    string abilityText = (string) null;
    if (CGameManager.usingDiseaseX)
    {
      if (this.diseaseSimulator == null)
        this.LoadDiseaseSimulator(false);
    }
    else
      this.diseaseSimulator = (DiseaseSimulator) null;
    if (withScenario != null)
    {
      ScenarioInformation scenarioInformation = withScenario.scenarioInformation;
      string scenPlagueType = scenarioInformation.scenPlagueType;
      if (!string.IsNullOrEmpty(scenPlagueType) && scenPlagueType != "0" && scenPlagueType != "!special")
      {
        string[] strArray = scenPlagueType.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (this.setupParameters.allowedDiseaseTypes == null)
            this.setupParameters.allowedDiseaseTypes = new HashSet<Disease.EDiseaseType>();
          this.setupParameters.allowedDiseaseTypes.Add(CGameManager.GetDiseaseType(strArray[index]));
        }
        this.setupParameters.lockDiseaseType = this.setupParameters.allowedDiseaseTypes != null && this.setupParameters.allowedDiseaseTypes.Count == 1;
        this.setupParameters.defaultDiseaseType = CGameManager.GetDiseaseType(strArray[0]);
      }
      if (scenPlagueType == "!special")
      {
        this.setupParameters.allowedDiseaseTypes = new HashSet<Disease.EDiseaseType>();
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.BACTERIA);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.VIRUS);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.FUNGUS);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.PARASITE);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.PRION);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.NANO_VIRUS);
        this.setupParameters.allowedDiseaseTypes.Add(Disease.EDiseaseType.BIO_WEAPON);
      }
      if (scenarioInformation.id != "" && this.setupParameters.defaultDiseaseType == Disease.EDiseaseType.CURE)
      {
        string id = scenarioInformation.id;
        this.setupParameters.cureScenarioDiseaseType = !(id == "cure") ? (!id.Contains("PIFCURE") ? CGameManager.GetCureDiseaseType(id) : this.setupParameters.defaultDiseaseType) : Disease.EDiseaseType.BACTERIA;
      }
      string str = scenarioInformation.scenNexus;
      if (!string.IsNullOrEmpty(str) && str != "0")
      {
        if (str.ToUpper() == "RANDOM")
          str = this.world.countries[UnityEngine.Random.Range(0, this.world.countries.Count)].id;
        this.setupParameters.startCountryID = str;
        this.setupParameters.fixedStartCountry = true;
      }
      if (!string.IsNullOrEmpty(scenarioInformation.scenName) && scenarioInformation.scenName != "0")
        this.setupParameters.defaultName = scenarioInformation.scenName;
      if (!string.IsNullOrEmpty(scenarioInformation.scenDifficulty) && scenarioInformation.scenDifficulty != "0")
      {
        this.setupParameters.lockDifficulty = true;
        this.setupParameters.difficulty = scenarioInformation.scenDifficulty;
      }
      if (scenarioInformation.scenStartDate > 0L)
      {
        IGame.GameSetupParameters setupParameters2 = this.setupParameters;
        dateTime = new DateTime(scenarioInformation.scenStartDate);
        long ticks2 = dateTime.Ticks;
        setupParameters2.startDate = ticks2;
      }
      abilityText = withScenario.aaData;
      this.setupParameters.lockAllGenes = scenarioInformation.scenGenes == "-1";
      this.setupParameters.lockAllCheats = scenarioInformation.scenCheats == "0";
      if (scenarioInformation.scenUseCustomColour)
      {
        switch (scenarioInformation.scenDiseaseDotColourSet)
        {
          case "neurax":
            this.diseaseColourSets[0] = this.neuraxDiseaseColourSets[0];
            break;
          case "necroa":
            this.diseaseColourSets[0] = this.zombieDiseaseColourSets[0];
            break;
          case "simian":
            this.diseaseColourSets[0] = this.simianDiseaseColourSets[0];
            break;
          case "vampire":
            this.diseaseColourSets[0] = this.vampireDiseaseColourSets[0];
            break;
          case "cure":
            this.diseaseColourSets[0] = this.cureDiseaseColourSets[0];
            break;
        }
        this.neuraxDiseaseColourSets[0] = this.diseaseColourSets[0];
        this.zombieDiseaseColourSets[0] = this.diseaseColourSets[0];
        this.simianDiseaseColourSets[0] = this.diseaseColourSets[0];
        this.vampireDiseaseColourSets[0] = this.diseaseColourSets[0];
      }
    }
    if (this.setupParameters.lockDifficulty)
      this.Difficulty = (uint) Mathf.Max(CGameManager.Difficulties.IndexOf(this.setupParameters.difficulty), 0);
    this.world.isSpeedRun = CGameManager.gameType == IGame.GameType.SpeedRun;
    if (withScenario != null)
    {
      this.world.isScenario = true;
      this.world.scenarioEventRestriction = withScenario.scenarioInformation.scenEventRestriction;
      this.world.scenarioScoreMultiplier = withScenario.scenarioInformation.scenScoreMultiplier + 1f;
    }
    this.world.startDate = this.setupParameters.startDate;
    this.availableGenes = DataImporter.ImportGenes(CGameManager.LoadGameText("Genes/genes"));
    if (string.IsNullOrEmpty(abilityText))
      abilityText = CGameManager.LoadGameText("Game/aa");
    this.abilities = DataImporter.ImportAbilities(abilityText, withScenario);
    if (!string.IsNullOrEmpty(CGameManager.dynamicNewsText))
      this.world.LoadDynamicNews(CGameManager.dynamicNewsText);
    string[] strArray1 = CGameManager.LoadGameText("World/borders").Split('\n');
    Country c1 = (Country) null;
    foreach (string str in strArray1)
    {
      string line = str;
      if (!string.IsNullOrEmpty(line))
      {
        if (c1 == null || !line.StartsWith(c1.name + ":"))
          c1 = this.world.countries.Find((Predicate<Country>) (a => a.id == line.Substring(0, line.IndexOf(':'))));
        Country c2 = this.world.countries.Find((Predicate<Country>) (a => a.id == line.Substring(line.IndexOf(':') + 1)));
        float distance = Vector3.Distance(CInterfaceManager.instance.GetCountryView(c1.id).transform.position, CInterfaceManager.instance.GetCountryView(c2.id).transform.position);
        c1.AddNeighbour(c2, distance);
        c2.AddNeighbour(c1, distance);
      }
    }
    for (int index = 0; index < this.world.countries.Count; ++index)
    {
      Country country = this.world.countries[index];
      foreach (TravelRoute apeMigrationRoute in country.apeMigrationRoutes)
      {
        Country c3 = apeMigrationRoute.source;
        if (apeMigrationRoute.source == country)
          c3 = apeMigrationRoute.destination;
        float dist = Vector3.Distance(CInterfaceManager.instance.GetCountryView(country.id).transform.position, CInterfaceManager.instance.GetCountryView(c3.id).transform.position);
        country.SetDistance(c3, dist);
      }
    }
    if (this.IsReplayActive)
    {
      this.world.startDate = this.replayData.startDate;
      Gene[] newGenes = new Gene[this.replayData.genes.Length];
      for (int i = 0; i < this.replayData.genes.Length; i++)
        newGenes[i] = this.availableGenes.Find((Predicate<Gene>) (a => a.id == this.replayData.genes[i]));
      for (int index = 0; index < this.replayData.diseases.Count; ++index)
      {
        ReplayData.DiseaseParameters disease1 = this.replayData.diseases[index];
        string diseaseFile = DataImporter.GetDiseaseFile(disease1.type);
        Disease disease2;
        if (withScenario == null)
        {
          disease2 = CGameManager.ImportDisease(CGameManager.LoadGameText("Disease/" + diseaseFile), false, withScenario);
        }
        else
        {
          disease2 = !withScenario.isOfficial || !withScenario.FileExists(diseaseFile) ? (string.IsNullOrEmpty(withScenario.diseaseData) ? CGameManager.ImportDisease(CGameManager.LoadGameText("Disease/" + diseaseFile), false, withScenario) : CGameManager.ImportDisease(withScenario.diseaseData, false, withScenario)) : CGameManager.ImportDisease(withScenario.ReadText(diseaseFile), false, withScenario);
          disease2.scenario = !withScenario.isOfficial ? "custom" : withScenario.scenarioInformation.id;
        }
        disease2.SetCheatFlags(this.replayData.cheatFlags);
        disease2.diseaseType = disease1.type;
        disease2.name = disease1.name;
        disease2.difficulty = disease1.difficulty;
        disease2.isSpeedRun = CGameManager.gameType == IGame.GameType.SpeedRun;
        disease2.showExtraPopups = this.replayData.showExtraPopups;
        this.world.AddDisease(disease2);
        if (disease1.shuffleMap != null)
        {
          disease2.shuffleMap = disease1.shuffleMap;
          disease2.shuffleStartMap = disease1.shuffleStartMap;
          DataImporter.ApplyShuffle(disease2);
        }
        if (!string.IsNullOrEmpty(disease1.hqCountry))
          disease2.SetHQCountry(World.instance.GetCountry(disease1.hqCountry));
        Country country1 = this.world.GetCountry(disease1.nexus);
        this.world.SetNexus(disease2, country1);
        if (CGameManager.usingDiseaseX)
        {
          if (this.diseaseSimulator != null)
          {
            disease2.simulator = this.diseaseSimulator;
            disease2.ApplySimulatorValues(this.diseaseSimulator);
          }
          else
            Debug.LogError((object) "useSimulatedDiseaseValues is set to true, however the diseaseSimulator OBJ is null");
        }
        country1.initialSpawnPosition = (double) disease1.initialSpawnPosition.magnitude <= 1.0 / 1000.0 ? new Vector3?() : new Vector3?(disease1.initialSpawnPosition);
        if (disease1.secondNexus != null)
        {
          Country country2 = this.world.GetCountry(disease1.secondNexus);
          this.world.SetSecondNexus(disease2, country2);
          country1.initialSpawnPosition = (double) disease1.initialSecondSpawnPosition.magnitude <= 1.0 / 1000.0 ? new Vector3?() : new Vector3?(disease1.initialSecondSpawnPosition);
        }
        this.ApplyScenarioPopulations(this.CurrentLoadedScenario, disease2, country1);
        disease2.ApplyGenes(newGenes);
      }
    }
    else
    {
      this.gameState = IGame.GameState.Choosing;
      this.replayData = new ReplayData();
      this.replayData.startDate = this.world.startDate;
      this.replayData.showExtraPopups = COptionsManager.instance.mbExtraPopups;
    }
  }

  public void ApplyScenarioPopulations(Scenario scen, Disease disease, Country country)
  {
    if (scen == null)
      return;
    long additionalNexusInfected = scen.scenarioInformation.GetAdditionalNexusInfected(disease, country);
    if (additionalNexusInfected != 0L)
      disease.nexus.TransferPopulation((double) additionalNexusInfected, Country.EPopulationType.HEALTHY, disease, Country.EPopulationType.INFECTED);
    long additionalNexusDead = scen.scenarioInformation.GetAdditionalNexusDead(disease, country);
    if (additionalNexusDead != 0L)
      disease.nexus.TransferPopulation((double) additionalNexusDead, Country.EPopulationType.HEALTHY, disease, Country.EPopulationType.DEAD);
    if (disease.diseaseType != Disease.EDiseaseType.NECROA)
      return;
    long additionalNexusZombie = scen.scenarioInformation.GetAdditionalNexusZombie(disease, country);
    if (additionalNexusZombie == 0L)
      return;
    disease.nexus.TransferPopulation((double) additionalNexusZombie, Country.EPopulationType.HEALTHY, disease, Country.EPopulationType.ZOMBIE);
  }

  public virtual void ChooseGenes(Gene[] genes)
  {
    foreach (Disease disease in this.world.diseases)
      disease.ApplyGenes(genes);
    this.replayData.SetGenes(genes);
  }

  public virtual void ChooseCheats(Disease.ECheatType[] cheatTypes)
  {
    this.cheatsUsed = cheatTypes;
    this.replayData.SetCheatFlags(Disease.GetCheatFlags(cheatTypes));
  }

  public virtual void ChooseDisease(string diseaseName, Disease.EDiseaseType diseaseType)
  {
    this.AddDisease(diseaseName, diseaseType, CGameManager.localPlayerInfo, Disease.GetCheatFlags(this.cheatsUsed));
  }

  public virtual Disease AddDisease(
    string diseaseName,
    Disease.EDiseaseType diseaseType,
    IPlayerInfo player,
    int cheatFlags)
  {
    if (this.gameState != IGame.GameState.Choosing && this.gameState != IGame.GameState.ChoosingCountry)
      Debug.LogError((object) ("NOT IN CHOOSING STATE WHEN ADDING DISEASE! in " + (object) this.gameState));
    bool flag = this.CurrentLoadedScenario != null && !this.CurrentLoadedScenario.isOfficial;
    string diseaseFile = DataImporter.GetDiseaseFile(diseaseType);
    Disease disease;
    if (this.CurrentLoadedScenario == null)
    {
      Debug.Log((object) "No scenario loaded currently");
      disease = CGameManager.ImportDisease(CGameManager.LoadGameText("Disease/" + diseaseFile), false, this.CurrentLoadedScenario);
    }
    else
    {
      Debug.Log((object) diseaseFile);
      if (!flag && this.CurrentLoadedScenario.FileExists(diseaseFile))
      {
        Debug.Log((object) ("Scenario has file for " + diseaseFile));
        disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.ReadText(diseaseFile), false, this.CurrentLoadedScenario);
      }
      else if (!string.IsNullOrEmpty(this.CurrentLoadedScenario.diseaseData))
      {
        Debug.Log((object) "Disease Data is valid, loading it");
        disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.diseaseData, false, this.CurrentLoadedScenario);
        Debug.Log((object) "disease data loaded");
        if (this.CurrentLoadedScenario.diseaseData == null)
          Debug.Log((object) "But it's null");
        int difficulty = (int) this.Difficulty;
        if (difficulty == 0 && this.CurrentLoadedScenario.diseaseData_0 != null)
          disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.diseaseData_0, false, this.CurrentLoadedScenario);
        if (difficulty == 1 && this.CurrentLoadedScenario.diseaseData_1 != null)
          disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.diseaseData_1, false, this.CurrentLoadedScenario);
        if (difficulty == 2 && this.CurrentLoadedScenario.diseaseData_2 != null)
          disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.diseaseData_2, false, this.CurrentLoadedScenario);
        if (difficulty == 3 && this.CurrentLoadedScenario.diseaseData_3 != null)
          disease = CGameManager.ImportDisease(this.CurrentLoadedScenario.diseaseData_3, false, this.CurrentLoadedScenario);
      }
      else
      {
        Debug.Log((object) ("Scenario using defaults for " + diseaseFile));
        disease = CGameManager.ImportDisease(CGameManager.LoadGameText("Disease/" + diseaseFile), false, this.CurrentLoadedScenario);
      }
      disease.scenario = !this.CurrentLoadedScenario.isOfficial ? "custom" : this.CurrentLoadedScenario.scenarioInformation.id;
      disease.victoryTechId = this.CurrentLoadedScenario.scenarioInformation.scenGameWinTech;
      if (disease.victoryTechId == "0")
        disease.victoryTechId = (string) null;
      if (disease.victoryTechId != null)
      {
        disease.endMessageImage = this.CurrentLoadedScenario.scenarioInformation.scenEndMessageImage;
        ParameterisedString parameterisedString1 = new ParameterisedString(this.CurrentLoadedScenario.scenarioInformation.scenEndMessageText, new string[1]
        {
          "disease.name"
        });
        ParameterisedString parameterisedString2 = new ParameterisedString(this.CurrentLoadedScenario.scenarioInformation.scenEndMessageTitle, new string[1]
        {
          "disease.name"
        });
        disease.endMessageText = parameterisedString1;
        disease.endMessageTitle = parameterisedString2;
      }
      Debug.Log((object) ("VICTORY TECH IS: " + disease.victoryTechId));
    }
    disease.SetCheatFlags(cheatFlags);
    disease.diseaseType = diseaseType;
    disease.difficulty = (int) this.Difficulty;
    disease.name = diseaseName;
    disease.showExtraPopups = COptionsManager.instance.mbExtraPopups;
    this.world.AddDisease(disease);
    Debug.Log((object) ("ADDING DISEASE[" + (object) disease.diseaseType + "]: " + (object) disease + " FOR PLAYER: " + (player != null ? (object) player.name : (object) "NULL-BOT")));
    if (player != null)
      player.disease = disease;
    CLocalisationManager.ClearCustomLocalisation();
    this.LoadScenarioStrings();
    if (player == CNetworkManager.network.LocalPlayerInfo)
      this.gameState = IGame.GameState.ChoosingCountry;
    return disease;
  }

  public virtual void LoadScenarioStrings()
  {
    if (this.CurrentLoadedScenario == null)
      return;
    Debug.Log((object) "Loading Scenario");
    switch (CGameManager.gameType)
    {
      case IGame.GameType.Official:
        CLocalisationManager.AddCustomLanguages(this.CurrentLoadedScenario.localisationText, "English");
        CLocalisationManager.SetCustomLanguage(CLocalisationManager.ActiveLanguage);
        break;
      case IGame.GameType.Custom:
        ScenarioInformation scenarioInformation = this.CurrentLoadedScenario.scenarioInformation;
        if (!string.IsNullOrEmpty(scenarioInformation.scenMainLanguage))
        {
          CLocalisationManager.AddCustomLanguages(this.CurrentLoadedScenario.localisationText, scenarioInformation.scenDefaultLanguage);
          CLocalisationManager.SetCustomLanguage(CLocalisationManager.ActiveLanguage);
          break;
        }
        if (scenarioInformation != null)
        {
          CLocalisationManager.AddCustomString(scenarioInformation.scenTitle, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenDescription, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenInitPopup1Title, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenInitPopup1Text, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenInitPopup2Title, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenInitPopup2Text, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenEndGameTagline, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenEndMessageText, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenEndMessageTitle, true);
          CLocalisationManager.AddCustomString(scenarioInformation.scenName, true);
        }
        if (!string.IsNullOrEmpty(this.CurrentLoadedScenario.diseaseData))
        {
          foreach (Technology technology in World.instance.GetDisease(0).technologies)
          {
            CLocalisationManager.AddCustomString(technology.name);
            CLocalisationManager.AddCustomString(technology.description);
          }
        }
        if (!string.IsNullOrEmpty(this.CurrentLoadedScenario.countryData))
        {
          foreach (Country country in this.world.countries)
          {
            CLocalisationManager.AddCustomString(country.name);
            CLocalisationManager.AddCustomString(country.countryDescription);
            CLocalisationManager.AddCustomString(country.countryDescriptionExtended);
          }
        }
        if (string.IsNullOrEmpty(this.CurrentLoadedScenario.customUIText))
          break;
        Dictionary<string, string> languageData = new Dictionary<string, string>();
        foreach (string language in CLocalisationManager.GetLanguages())
          languageData[language] = this.CurrentLoadedScenario.customUIText;
        CLocalisationManager.AddCustomLanguages(languageData, CLocalisationManager.ActiveLanguage);
        CLocalisationManager.SetCustomLanguage(CLocalisationManager.ActiveLanguage);
        break;
    }
  }

  public void LoadDiseaseSimulator(bool reset, bool loadPreviousValues = false)
  {
    if (!CGameManager.usingDiseaseX || !(this.diseaseSimulator == null | reset))
      return;
    this.diseaseSimulator = new DiseaseSimulator();
  }

  public virtual void SetPlayerDisease(IPlayerInfo player, int diseaseID)
  {
    Disease disease = this.world.diseases.Find((Predicate<Disease>) (a => a.id == diseaseID));
    if (disease == null)
      return;
    player.disease = disease;
  }

  public virtual void StartGame()
  {
    CGameManager.spaceTime = false;
    Debug.Log((object) ("IGame.StartGame() - type:" + (object) CGameManager.gameType + ", IsReplayActive:" + this.IsReplayActive.ToString()));
    if (this.IsReplayActive)
    {
      this.PostDiseaseSelectionLoad();
      Debug.Log((object) string.Format("INFO: Replaying {0} turns with seed {1}.", (object) this.replayData.maxGameTurn, (object) this.replayData.initialSeed));
      this.DoStartGame(this.replayData.initialSeed);
    }
    else
    {
      this.PostDiseaseSelectionLoad();
      this.DoStartGame(this.initialGameSeed);
    }
  }

  public virtual void CreateMPGameSession(
    string diseaseName,
    Disease.EDiseaseType diseaseType,
    Gene[] genes,
    int difficulty)
  {
    Debug.LogError((object) ("Cannot create MP game session - not a multiplayer game: " + (object) this));
  }

  public virtual bool ConnectGameSession()
  {
    Debug.LogError((object) ("Cannot connect game session - not a multiplayer game: " + (object) this));
    return false;
  }

  public virtual void ReplayGame()
  {
    CGameManager.spaceTime = false;
    if (!this.replayData.hasData)
      return;
    UnityEngine.Random.seed = this.replayData.initialSeed;
    this.IsReplayActive = true;
    this.CreateGame(this.CurrentLoadedScenario);
    this.timer = 0.0f;
    this.eventTimer = 0.0f;
    this.replayTurnCounter = 0;
    this.replayData.Rewind();
    this.SetSpeed(1);
    CGameManager.localPlayerInfo.disease = this.world.diseases[CGameManager.localPlayerInfo.disease.id];
    CInterfaceManager.instance.Cleanup();
    CInterfaceManager.instance.InitialiseCountryViews();
    this.StartGame();
  }

  public virtual void PostDiseaseSelectionLoad()
  {
    bool allowAchievements = this.CurrentLoadedScenario == null || this.CurrentLoadedScenario.isOfficial;
    if (CGameManager.gameType == IGame.GameType.Tutorial || CGameManager.gameType == IGame.GameType.CureTutorial)
      allowAchievements = TutorialSystem.IsModuleSectionComplete("Free Play");
    if (CGameManager.usingDiseaseX)
      allowAchievements = false;
    if (this.CurrentLoadedScenario != null && !string.IsNullOrEmpty(this.CurrentLoadedScenario.eventData))
    {
      this.world.LoadEvents(this.CurrentLoadedScenario.eventData, GameEvent.EEventDiseaseType.CUSTOM, allowAchievements);
      foreach (GameEvent gameEvent in this.world.eventManager.events)
      {
        foreach (EventOutcome eventOutcome in gameEvent.eventOutcomes)
        {
          if (eventOutcome.popupTitle != null)
            this.AddToLocalisationStrings(eventOutcome.popupTitle.text);
          if (eventOutcome.popupMessage != null)
            this.AddToLocalisationStrings(eventOutcome.popupMessage.text);
          if (eventOutcome.newsMessages != null)
          {
            for (int index = 0; index < eventOutcome.newsMessages.Count; ++index)
              this.AddToLocalisationStrings(eventOutcome.newsMessages[index].text);
          }
        }
      }
      string scenDefaultEvents = this.CurrentLoadedScenario.scenarioInformation.scenDefaultEvents;
      if (scenDefaultEvents.Length > 1 && scenDefaultEvents[scenDefaultEvents.Length - 1] != ',')
        scenDefaultEvents += ",";
      string str = scenDefaultEvents + "essential_text,essential_no_text";
      Debug.Log((object) str);
      Disease.EDiseaseType diseaseType = this.world.diseases[0].diseaseType;
      this.world.LoadEvents(CGameManager.GetDiseaseEventData(diseaseType), CGameManager.GetDiseaseEventType(diseaseType), allowAchievements, str);
    }
    else
    {
      for (int index = 0; index < this.world.diseases.Count; ++index)
      {
        Debug.Log((object) ("DIS[" + this.world.diseases[index].name + "] ---totalInf:" + (object) this.world.diseases[index].totalInfected));
        Disease.EDiseaseType diseaseType = this.world.diseases[index].diseaseType;
        if (this.CurrentLoadedScenario == null || string.IsNullOrEmpty(this.CurrentLoadedScenario.eventData))
          this.world.LoadEvents(CGameManager.GetDiseaseEventData(diseaseType), CGameManager.GetDiseaseEventType(diseaseType), allowAchievements);
      }
    }
    if (this.CurrentLoadedScenario != null)
    {
      for (int index = 0; index < this.world.diseases.Count; ++index)
      {
        Disease.EDiseaseType diseaseType = this.world.diseases[index].diseaseType;
        string actionData = this.CurrentLoadedScenario.GetActionData(diseaseType);
        if (string.IsNullOrEmpty(actionData))
          this.world.LoadGovernmentActions(GovernmentAction.GetData(diseaseType), GovernmentAction.GetType(diseaseType));
        else
          this.world.LoadGovernmentActions(actionData, GovernmentAction.GetType(diseaseType));
      }
    }
    else
    {
      for (int index = 0; index < this.world.diseases.Count; ++index)
      {
        Disease.EDiseaseType diseaseType = this.world.diseases[index].diseaseType;
        this.world.LoadGovernmentActions(GovernmentAction.GetData(diseaseType), GovernmentAction.GetType(diseaseType));
      }
    }
    Disease disease = this.world.diseases[0];
    foreach (Country country in this.world.countries)
    {
      foreach (string name in country.actionsTaken)
      {
        GovernmentAction action = this.world.governmentActionManager.FindAction(name, disease);
        if (action != null && (action.removable || (double) action.economicDamagePerTurn > 0.0))
          country.actionsSpecialInterest.Add(name);
      }
    }
  }

  private void AddToLocalisationStrings(string localisableString)
  {
    if (string.IsNullOrEmpty(localisableString))
      return;
    CLocalisationManager.AddCustomString(localisableString, true);
  }

  public virtual void ResumeGame()
  {
    this.isGameSessionInterrupted = true;
    this.actualSpeed = this.wantedSpeed = 1;
    this.gameState = IGame.GameState.InProgress;
    this.timeGameSessionStarted = Time.realtimeSinceStartup - (float) this.previousRecordedGameTime;
    CInterfaceManager.instance.Initialise();
    CInterfaceManager.instance.InitialiseCountryViews();
    CInterfaceManager.instance.SetupGameHUD();
    CHUDScreen.instance.AddArchiveNews(this.gameHistory.NewsStories);
    CUIManager.instance.SetHexGrid(CGameManager.gameType == IGame.GameType.Cure);
    CInterfaceManager.instance.SetCursorSelection(EHudMode.Normal);
    foreach (Country nexusCountry in this.world.nexusCountries)
      CInterfaceManager.instance.GetCountryView(nexusCountry.id).GetInfectionSystem().UpdateSystem(nexusCountry.diseaseNexus.GetLocalDisease(nexusCountry), 20);
    DiseaseTrailParticles.instance.UpdateVisibility();
  }

  public virtual bool ChooseCountry(string countryID, bool isTimeout = false)
  {
    Disease disease = CGameManager.localPlayerInfo.disease;
    if (disease.isCure && disease.nexus == null)
    {
      disease.SetHQCountry(World.instance.GetCountry(countryID));
      List<Country> all = World.instance.countries.FindAll((Predicate<Country>) (a => a != disease.hqCountry && !a.IsIsland() && !a.IsNeighbour(disease.hqCountry)));
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        Country forCountry = all[0];
        for (int index = 0; index < all.Count; ++index)
        {
          if (all[index].name == "Brazil")
            forCountry = all[index];
        }
        this.world.SetNexus(disease, forCountry);
      }
      else
        this.world.SetNexus(disease, all[UnityEngine.Random.Range(0, all.Count)]);
      if (CGameManager.usingDiseaseX)
      {
        if (this.diseaseSimulator != null)
        {
          disease.simulator = this.diseaseSimulator;
          disease.ApplySimulatorValues(this.diseaseSimulator);
        }
        else
          Debug.LogError((object) "useSimulatedDiseaseValues is set to true, however the diseaseSimulator OBJ is null");
      }
      this.ApplyScenarioPopulations(this.CurrentLoadedScenario, disease, disease.nexus);
      return true;
    }
    if (disease.nexus == null)
    {
      Country forCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
      if (forCountry != null && !this.world.nexusCountries.Contains(forCountry))
      {
        this.world.SetNexus(disease, forCountry);
        this.ApplyScenarioPopulations(this.CurrentLoadedScenario, disease, disease.nexus);
        return !disease.HasCheat(Disease.ECheatType.DOUBLE_STRAIN) || disease.secondNexus != null;
      }
      Debug.Log((object) ("Failed to choose: " + countryID + " " + (forCountry != null ? "Already chosen" : "Country not found")));
    }
    else if (disease.HasCheat(Disease.ECheatType.DOUBLE_STRAIN) && disease.secondNexus == null)
    {
      Country forCountry = this.world.countries.Find((Predicate<Country>) (a => a.id == countryID));
      if (forCountry != null && !this.world.nexusCountries.Contains(forCountry))
      {
        this.world.SetSecondNexus(disease, forCountry);
        this.ApplyScenarioPopulations(this.CurrentLoadedScenario, disease, disease.secondNexus);
        return true;
      }
    }
    return false;
  }

  public virtual void DoStartGame(int randomSeed)
  {
    this.isGameSessionInterrupted = false;
    this.timeGameSessionStarted = Time.realtimeSinceStartup;
    if (!this.IsReplayActive)
    {
      this.endTurnDatas.Clear();
      this.replayData.initialSeed = randomSeed;
      this.replayData.initialBonusIcon = BonusIcon.GetCounter();
    }
    this.actualSpeed = this.wantedSpeed = 1;
    UnityEngine.Random.seed = randomSeed;
    this.eventTimer = this.timer = 0.0f;
    this.world.StartSimulation();
    this.gameState = IGame.GameState.InProgress;
    if (!this.IsReplayActive)
    {
      for (int index = 0; index < this.world.diseases.Count; ++index)
        this.replayData.AddDisease(this.world.diseases[index]);
    }
    else
    {
      BonusIcon.ResetCounter(this.replayData.initialBonusIcon);
      Vehicle.ResetCounter();
      Vampire.ResetCounter();
    }
    CInterfaceManager.instance.SetupGameHUD(this.IsReplayActive);
    DiseaseTrailParticles.instance.UpdateVisibility();
  }

  public virtual void UpdateExtraPopups(bool IsEnabled)
  {
    if (this.world == null || this.IsReplayActive)
      return;
    foreach (Disease disease in this.world.diseases)
      disease.showExtraPopups = IsEnabled;
    this.replayData.AddEvent(IsEnabled ? ReplayData.ReplayEventType.EXTRA_POPUPS_ENABLED : ReplayData.ReplayEventType.EXTRA_POPUPS_DISABLED, this.world.DiseaseTurn, this.world.eventTurn, (Disease) null, (string) null);
  }

  public virtual bool DeEvolveTech(Technology technology)
  {
    if (this.IsReplayActive)
      return false;
    IPlayerInfo localPlayerInfo = CGameManager.localPlayerInfo;
    if (!localPlayerInfo.disease.CanDeEvolve(technology) || localPlayerInfo.disease.GetDeEvolveCost(technology) > localPlayerInfo.disease.evoPoints)
      return false;
    localPlayerInfo.disease.DeEvolveTech(technology);
    this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_DEEVOLVED, this.world.DiseaseTurn, this.world.eventTurn, localPlayerInfo.disease, technology.id);
    CInterfaceManager.instance.UpdateInterface();
    return true;
  }

  public virtual bool EvolveTech(Technology technology)
  {
    if (this.IsReplayActive)
      return false;
    IPlayerInfo localPlayerInfo = CGameManager.localPlayerInfo;
    if (!localPlayerInfo.disease.CanEvolve(technology) || localPlayerInfo.disease.GetEvolveCost(technology) > localPlayerInfo.disease.evoPoints)
      return false;
    localPlayerInfo.disease.EvolveTech(technology, false);
    CInterfaceManager.instance.UpdateInterface();
    Debug.Log((object) ("_______[0]RECORD TECH_EVOLVED - player.disease:" + localPlayerInfo.disease.name + " evolved:" + technology.id));
    this.replayData.AddEvent(ReplayData.ReplayEventType.TECH_EVOLVED, this.world.DiseaseTurn, this.world.eventTurn, localPlayerInfo.disease, technology.id);
    return true;
  }

  public virtual void UseApeRampage(CountryView countryView, Disease disease)
  {
    Country country = countryView.GetCountry();
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.rampage, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.APE_RAMPAGE, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id);
    countryView.ApeRampageEffect(disease.GetLocalDisease(country).apeInfectedPopulation);
    this.world.UseActiveAbility(EAbilityType.rampage, disease, (IList<Vector3>) null, (IList<Country>) new Country[1]
    {
      country
    });
    this.ClearExistingHorde(country);
  }

  public virtual void ClearExistingHorde(Country country)
  {
    Vehicle hordeFromCountry = this.world.GetHordeFromCountry(country);
    if (hordeFromCountry == null)
      return;
    this.VehicleArrived(hordeFromCountry, hordeFromCountry.currentPosition);
    CInterfaceManager.instance.RemoveVehicle(hordeFromCountry);
    this.vehicles.Remove(hordeFromCountry);
  }

  public virtual void CreateApeColony(Country country, Disease disease, Vector3 position)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.create_colony, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.APE_CREATE_COLONY, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id + ":" + CUtils.SerializeVector3(position));
    disease.SpendEvoPoints(activeAbilityCost);
    float aaCostMultiplier = disease.GetAACostMultiplier(EAbilityType.create_colony);
    float multiplier = disease.difficulty <= 2 ? aaCostMultiplier * 1.6f : aaCostMultiplier * 1.8f;
    disease.SetAACostMultiplier(EAbilityType.create_colony, multiplier);
    this.world.UseActiveAbility(EAbilityType.create_colony, disease, (IList<Vector3>) new Vector3[1]
    {
      position
    }, (IList<Country>) new Country[1]{ country });
    this.ClearExistingHorde(country);
    CInterfaceManager.instance.UpdateApeColonies(this.world.GetUpdatedColonies());
    this.world.ClearApeColonies();
  }

  public virtual void ResetApeColony(Country country, Disease disease)
  {
    country.GetLocalDisease(disease).StartApeColony();
  }

  public virtual long Reanimate(Country country, Disease disease)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.reanimate, disease);
    if (disease.evoPoints < activeAbilityCost)
      return 0;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.REANIMATE, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id);
    disease.SpendEvoPoints(activeAbilityCost);
    World.instance.AddAchievement(EAchievement.A_IllBeBack);
    return country.GetLocalDisease(disease).Reanimate();
  }

  public virtual void ImmuneShock(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    Debug.LogError((object) "Immune Shock not enabled");
  }

  protected virtual void ProcessImmuneShock(string countryID, Vector3 pos, Disease disease)
  {
    Debug.LogError((object) "Immune Shock not enabled");
  }

  public virtual void BenignMimic(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    Debug.LogError((object) "Benign Mimic not enabled");
  }

  public virtual void ProcessBenignMimic(string countryID, Vector3 pos, Disease disease)
  {
    Debug.LogError((object) "Benign Mimic not enabled");
  }

  public virtual void InfectBoost(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    Debug.LogError((object) "Infect Boost not enabled");
  }

  public virtual void ProcessInfectBoost(string countryID, Vector3 pos, Disease disease)
  {
    Debug.LogError((object) "Infect Boost not enabled");
  }

  public virtual void LethalBoost(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    Debug.LogError((object) "Lethal Boost not enabled");
  }

  public virtual void ProcessLethalBoost(string countryID, Vector3 pos, Disease disease)
  {
    Debug.LogError((object) "Lethal Boost not enabled");
  }

  public virtual void CreateNeuraxVehicle(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition,
    int presetInfected = -1)
  {
    this.world.UseActiveAbility(EAbilityType.neurax, disease, (IList<Vector3>) new Vector3[2]
    {
      sourcePosition,
      endPosition
    }, (IList<Country>) new Country[2]
    {
      fromCountry,
      toCountry
    }, presetInfected);
  }

  public virtual void CreateZombieVehicle(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition,
    int presetHordeAmount = -1)
  {
    this.world.UseActiveAbility(EAbilityType.zombie_horde, disease, (IList<Vector3>) new Vector3[2]
    {
      sourcePosition,
      endPosition
    }, (IList<Country>) new Country[2]
    {
      fromCountry,
      toCountry
    }, presetHordeAmount);
  }

  public virtual void CreateUnscheduledFlight(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    Debug.LogError((object) "Unscheduled flight not enabled");
  }

  protected virtual int ProcessUnscheduledFlight(
    Disease disease,
    Country fromCountry,
    Country toCountry,
    Vector3 sourcePosition,
    Vector3 endPosition,
    int presetInfected = -1)
  {
    Debug.LogError((object) "Unscheduled flight not enabled");
    return -1;
  }

  public virtual void CreateNukeLaunch(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    Debug.LogError((object) "Nuke launch not enabled");
  }

  public virtual void ProcessNukeLaunch(
    Disease disease,
    Country fromCountry,
    Country toCountry,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    Debug.LogError((object) "Nuke launch not enabled");
  }

  public virtual void CreateNukeStrike(string countryID, Vector3 pos, IPlayerInfo playerSender)
  {
    Debug.LogError((object) "Nuke Strike not enabled");
  }

  public virtual void ProcessNukeStrike(string countryID, Vector3 pos, Disease disease)
  {
    Debug.LogError((object) "Nuke Strike not enabled");
  }

  public virtual void DroneSetSpawnPosition(Vehicle vehicle)
  {
    if (this.IsReplayActive)
      return;
    this.replayData.AddEvent(ReplayData.ReplayEventType.VEHICLE_SOURCE_POSITION, this.world.DiseaseTurn, this.world.eventTurn, (Disease) null, vehicle.id.ToString() + ":" + CUtils.SerializeVector3(vehicle.sourcePosition.Value));
  }

  public virtual void HordeCountryChange(HordeObject horde, Country country)
  {
    if (this.IsReplayActive || horde.mpVehicle.subType != Vehicle.EVehicleSubType.ApeHorde && horde.mpVehicle.subType != Vehicle.EVehicleSubType.ApeHordeNoColony)
      return;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.APE_HORDE_CHANGE_COUNTRY, this.world.DiseaseTurn, this.world.eventTurn, (Disease) null, horde.mpVehicle.id.ToString() + ":" + (horde.mpVehicle.currentCountry != null ? (object) horde.mpVehicle.currentCountry.id : (object) string.Empty) + ":" + (country != null ? (object) country.id : (object) string.Empty));
    this.world.ChangeApeHordeCountry(horde.mpVehicle, horde.mpVehicle.currentCountry, country);
    if (!this.IsReplayActive)
      return;
    horde.mpVehicle.currentCountry = country;
  }

  public virtual void CreateApeVehicle(
    Country from,
    Country to,
    Disease disease,
    Vector3 startPos,
    Vector3 endPos)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.move, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    disease.SpendEvoPoints(activeAbilityCost);
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.APE_HORDE_CREATED, this.world.DiseaseTurn, this.world.eventTurn, disease, from.id + ":" + to.id + ":" + CUtils.SerializeVector3(startPos) + ":" + CUtils.SerializeVector3(endPos));
    this.ClearExistingHorde(from);
    this.world.UseActiveAbility(EAbilityType.move, disease, (IList<Vector3>) new Vector3[2]
    {
      startPos,
      endPos
    }, (IList<Country>) new Country[2]{ from, to });
    if (!from.hasApeColony)
      return;
    CSoundManager.instance.PlaySFX("ape_colony_destroyed");
  }

  public virtual void UseBloodRage(CountryView countryView, Disease disease)
  {
    Country country = countryView.GetCountry();
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.bloodrage, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.BLOOD_RAGE, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id);
    if (countryView.GetCountry().GetLocalDisease(disease).consumeFlag < 1)
      countryView.BloodRageEffect();
    this.world.UseActiveAbility(EAbilityType.bloodrage, disease, (IList<Vector3>) null, (IList<Country>) new Country[1]
    {
      country
    });
  }

  public virtual void CreateVampireVehicle(
    Country from,
    Country to,
    Disease disease,
    Vector3 startPos,
    Vector3 endPos)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.vampiretravel, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    disease.SpendEvoPoints(activeAbilityCost);
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.VAMPIRE_VEHICLE_CREATED, this.world.DiseaseTurn, this.world.eventTurn, disease, from.id + ":" + to.id + ":" + CUtils.SerializeVector3(startPos) + ":" + CUtils.SerializeVector3(endPos));
    this.world.UseActiveAbility(EAbilityType.vampiretravel, disease, (IList<Vector3>) new Vector3[2]
    {
      startPos,
      endPos
    }, (IList<Country>) new Country[2]{ from, to });
  }

  public virtual void CreateCastle(Country country, Disease disease, Vector3 position)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.castle, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.CREATE_CASTLE, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id + ":" + CUtils.SerializeVector3(position));
    disease.SpendEvoPoints(activeAbilityCost);
    float multiplier = disease.GetAACostMultiplier(EAbilityType.castle) * 2f;
    disease.SetAACostMultiplier(EAbilityType.castle, multiplier);
    this.world.UseActiveAbility(EAbilityType.castle, disease, (IList<Vector3>) new Vector3[1]
    {
      position
    }, (IList<Country>) new Country[1]{ country });
    CInterfaceManager.instance.UpdateCastles(this.world.GetUpdatedCastles());
    this.world.ClearCastles();
  }

  public virtual void CreateInvestigationTeamFlight(
    Country toCountry,
    Disease disease,
    Vector3 endPosition)
  {
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.investigation_team, disease);
    if (disease.evoPoints < activeAbilityCost)
      return;
    if (disease.vampires.Count <= 0)
    {
      Debug.LogError((object) "Created investigation team flight but no teams available!");
    }
    else
    {
      Vampire vampire = disease.vampires[0];
      Country currentCountry = vampire.currentCountry;
      if (currentCountry == null)
      {
        Debug.LogError((object) "Investigation team is not in a country,");
      }
      else
      {
        disease.SpendEvoPoints(activeAbilityCost);
        Vector3 sourcePos = vampire.currentPosition.Value;
        LocalDisease localDisease = currentCountry.GetLocalDisease(disease);
        localDisease.hasIntel = true;
        localDisease.hasTeam = false;
        disease.teamTravelTarget = toCountry;
        vampire.currentCountry = (Country) null;
        Vehicle vehicle = Vehicle.Create();
        vehicle.type = Vehicle.EVehicleType.ZombieHorde;
        vehicle.subType = Vehicle.EVehicleSubType.CureInvestigate;
        vehicle.actingDisease = disease;
        vehicle.SetRoute(currentCountry, toCountry, sourcePos, endPosition);
        vehicle.AddVampire(vampire);
        if (!CGameManager.game.IsReplayActive)
          CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.INVESTIGATION_TEAM, World.instance.DiseaseTurn, World.instance.eventTurn, disease, toCountry.id + ":" + CUtils.SerializeVector3(endPosition));
        this.world.AddVehicle(vehicle);
      }
    }
  }

  public virtual bool UseEconomicSupport(CountryView countryView, Disease disease)
  {
    Country country = countryView.GetCountry();
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.economic_support, disease);
    if (disease.evoPoints < activeAbilityCost)
      return false;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.ECONOMIC_SUPPORT, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id);
    country.GetLocalDisease(disease).GiveEconomicAid(CGameManager.game.GetActiveAbility(EAbilityType.economic_support));
    return true;
  }

  public virtual bool UseRaisePriority(CountryView countryView, Disease disease)
  {
    Country country = countryView.GetCountry();
    int activeAbilityCost = CGameManager.GetActiveAbilityCost(EAbilityType.raise_priority, disease);
    if (disease.evoPoints < activeAbilityCost)
      return false;
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.RAISE_PRIORITY, this.world.DiseaseTurn, this.world.eventTurn, disease, country.id);
    country.GetLocalDisease(disease).UseRaisePriority(CGameManager.game.GetActiveAbility(EAbilityType.raise_priority));
    return true;
  }

  public void HandleC12Module()
  {
    if (!CGameManager.IsCureTutorialGame || !TutorialSystem.IsModuleComplete("C11") || TutorialSystem.IsModuleActive("C12") || TutorialSystem.IsModuleComplete("C12"))
      return;
    Country country = this.world.GetCountry("brazil");
    if (this.world.GetDisease(0).IsTechEvolved("Declare_Knowledge") && country.GetLocalDisease(this.world.diseases[0]).infectedPopulation >= 1000000L)
    {
      TutorialSystem.CheckModule((Func<bool>) (() => true), "C12");
      WorldMapController.instance.SetSelectedCountry("brazil");
    }
    else
    {
      CGameManager.SetPaused(false, true);
      country.GetLocalDisease(this.world.diseases[0]).infectedPopulationOverride += 25f;
      this.StartCoroutine(this.RetryHandleC12Module());
    }
  }

  public void HandleC56Module()
  {
    this.tutorialCureTimer += Time.deltaTime;
    if ((double) this.tutorialCureTimer < 1.0)
      return;
    if (this.world.diseases[0].vaccineStage == Disease.EVaccineProgressStage.VACCINE_MANUFACTURE)
    {
      if (!this.tutorialCureCheck)
      {
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
        this.tutorialCureCheck = true;
        this.tutorialCureTimer = 0.0f;
      }
      else
        this.StartCoroutine(this.CheckC56NotFrozen());
    }
    else
      this.world.diseases[0].globalCureResearch += this.world.diseases[0].cureRequirements * 0.2f;
    this.tutorialCureTimer = 0.0f;
    CHUDScreen.instance.Refresh();
  }

  private IEnumerator CheckC56NotFrozen()
  {
    yield return (object) new WaitForSeconds(1f);
    if (TutorialSystem.IsModuleActive("C56"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
      this.tutorialCureCheck = true;
      this.tutorialCureTimer = 0.0f;
    }
  }

  private IEnumerator RetryHandleC12Module()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.HandleC12Module();
  }

  public virtual void GameUpdate()
  {
    if (!CGameManager.spaceTime)
    {
      if (Input.GetKeyDown(KeyCode.L) && World.instance.diseases.Count == 1)
      {
        List<ISaves.SaveMetaData> savedGames = CGameManager.saves.GetSavedGames();
        int slotID = 0;
        bool customGame = false;
        for (int index = savedGames.Count - 1; index >= 0; --index)
        {
          if (savedGames[index].valid)
          {
            slotID = savedGames[index].saveSlot;
            customGame = true;
            break;
          }
        }
        if (customGame)
          CGameManager.LoadGame(slotID, ref customGame);
      }
      if (CGameManager.IsFederalScenario("sorder"))
      {
        World.instance.diseases[0].customGlobalVariable1 = (float) DateTime.Now.Hour;
        Disease disease1 = World.instance.diseases[0];
        DateTime now = DateTime.Now;
        double minute = (double) now.Minute;
        disease1.customGlobalVariable2 = (float) minute;
        Disease disease2 = World.instance.diseases[0];
        now = DateTime.Now;
        double second = (double) now.Second;
        disease2.customGlobalVariable3 = (float) second;
        CHUDScreen.instance.SetDay(World.instance.DiseaseTurn);
      }
      if (Input.GetKeyDown(KeyCode.T) && World.instance.diseases.Count == 1)
      {
        IGame.showMoreDetail = !IGame.showMoreDetail;
        CHUDScreen.instance.SetDay(World.instance.DiseaseTurn);
      }
      if (Input.GetKeyDown(KeyCode.K) && World.instance.diseases.Count == 1)
        CPauseMainSubScreen.ExternalRestartGame();
      if (Input.GetKeyDown(KeyCode.M) && World.instance.diseases.Count == 1)
        CGameManager.GenerateRatingList();
      if (Input.GetKeyDown(KeyCode.P) && World.instance.diseases.Count == 1)
      {
        List<ISaves.SaveMetaData> savedGames = CGameManager.saves.GetSavedGames();
        int slotID = 0;
        for (int index = 0; index < savedGames.Count; ++index)
        {
          if (savedGames[index].saveSlot >= slotID)
            slotID = savedGames[index].saveSlot + 1;
        }
        CGameManager.game.SaveGame(slotID);
      }
      if (Input.GetKeyDown(KeyCode.N) && World.instance.diseases.Count == 1)
      {
        List<ISaves.SaveMetaData> savedGames = CGameManager.saves.GetSavedGames();
        int index = savedGames.Count - 1;
        if (savedGames[index].valid)
          CGameManager.saves.DeleteGame(savedGames[index].saveSlot);
      }
      if (Input.GetKeyDown(KeyCode.G) && World.instance.diseases.Count == 1)
        CUIManager.instance.standardConfirmOverlay.ShowLocalised("Warning", "All you sure to [ff0000]delete all the saves[ffffff]? This [00ffff]cannot be cancelled[ffffff]!\n(Your game might be in lag for a few seconds)", "YES", "OK", new CConfirmOverlay.PressDelegate(IGame.DeleteAllSaves), new CConfirmOverlay.PressDelegate(IGame.DeleteAllSaves));
      if (Input.GetKeyDown(KeyCode.B) && World.instance.diseases.Count == 1)
      {
        List<ISaves.SaveMetaData> savedGames = CGameManager.saves.GetSavedGames();
        int index = 0;
        if (savedGames[index].valid)
          CGameManager.saves.DeleteGame(savedGames[index].saveSlot);
      }
    }
    if (this.world == null)
      return;
    if (this.IsPaused)
    {
      if (CGameManager.gameType != IGame.GameType.CureTutorial)
        return;
      if (TutorialSystem.IsModuleComplete("C44") && !TutorialSystem.IsModuleComplete("C45"))
      {
        CHUDScreen.instance.Refresh();
        this.world.diseases[0].evoPoints = 40;
      }
      if (TutorialSystem.IsModuleActive("C30"))
      {
        WorldMapController.instance.SetSelectedCountry("brazil");
        this.tutorialCureCheck = false;
      }
      else if (TutorialSystem.IsModuleComplete("C30") && !TutorialSystem.IsModuleComplete("C45"))
        WorldMapController.instance.SetSelectedCountry("brazil");
      if (!TutorialSystem.IsModuleActive("C56"))
        return;
      this.HandleC56Module();
      CGameManager.SetPaused(false, true);
    }
    else if (this.IsReplayActive)
    {
      this.ReplayUpdate();
    }
    else
    {
      if (this.gameState != IGame.GameState.InProgress)
        return;
      if (Input.GetKeyDown(KeyCode.F))
      {
        Disease disease = CGameManager.localPlayerInfo.disease;
        if (this.diseaseSimulator != null)
        {
          if (!CGameManager.usingDiseaseX)
            Debug.Log((object) "DiseaseSimulator active but usingDiseaseX is false?");
          Debug.Log((object) ("DiseaseSimulator Infectivity = " + (object) this.diseaseSimulator.StatValues[0][1]));
        }
        else
          Debug.Log((object) "DiseaseSimulator null");
      }
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        this.HandleC12Module();
        if (TutorialSystem.CureTutorialModulesPauseSimulation && TutorialSystem.IsModuleActive() && !TutorialSystem.IsModuleActive("C56") && TutorialSystem.IsModuleActive())
          CGameManager.SetPaused(true, true);
        if (TutorialSystem.IsModuleActive("C56"))
          this.HandleC56Module();
        if (TutorialSystem.IsModuleComplete("C29") && !TutorialSystem.IsModuleActive("C30"))
        {
          this.tutorialCureTimer += Time.deltaTime;
          if ((double) this.tutorialCureTimer >= 4.0)
          {
            WorldMapController.instance.SetSelectedCountry();
            this.tutorialCureTimer = 0.0f;
            this.world.GetCountry("brazil").GetLocalDisease(this.world.diseases[0]).compliance = 0.0f;
            CHUDScreen.instance.Refresh();
            TutorialSystem.CheckModule((Func<bool>) (() => true), "C30");
          }
        }
        if (TutorialSystem.IsModuleComplete("C19") && !TutorialSystem.IsModuleActive("C20") && this.world.GetCountry("brazil").GetLocalDisease(this.world.diseases[0]).infectedPopulation >= 3000000L)
          TutorialSystem.CheckModule((Func<bool>) (() => true), "C20");
      }
      for (int index = 0; index < this.actualSpeed; ++index)
      {
        float gameSpeedModifier = this.world.gameSpeedModifier;
        this.timer += Time.deltaTime * gameSpeedModifier;
        this.eventTimer += Time.deltaTime * gameSpeedModifier;
        bool flag = true;
        ModelUtils.ClearCounter();
        while (flag)
        {
          if ((double) this.eventTimer > (double) this.secondsPerEvent)
          {
            this.eventTimer -= this.secondsPerEvent;
            int seed = Mathf.FloorToInt(UnityEngine.Random.value * (float) int.MaxValue);
            this.replayData.EventTurn(seed);
            this.TriggerEventUpdate(seed);
          }
          if ((double) this.timer > (double) this.secondsPerTurn)
          {
            this.timer -= this.secondsPerTurn;
            int seed = Mathf.FloorToInt(UnityEngine.Random.value * (float) int.MaxValue);
            this.replayData.GameTurn(seed);
            this.TriggerGameUpdate(seed);
          }
          flag = (double) this.eventTimer > (double) this.secondsPerEvent || (double) this.timer > (double) this.secondsPerTurn;
          if (CGameManager.gameType != IGame.GameType.CureTutorial && this.world.gameEnded && this.world.turnsUntilGameEnd <= 0)
          {
            flag = false;
            index = this.actualSpeed;
            this.EndGame(IGame.EndGameReason.COMPLETE);
            ModelUtils.PrintCounter("ENDGAME");
          }
        }
      }
    }
  }

  public void ForceSpawnVehicles()
  {
    List<Vehicle> vehicleList = new List<Vehicle>((IEnumerable<Vehicle>) this.world.GetVehicles());
    this.world.ClearVehicles();
    if (vehicleList.Count <= 0)
      return;
    this.vehicles.AddRange((IEnumerable<Vehicle>) vehicleList);
    CInterfaceManager.instance.ForceSpawnVehicles(vehicleList);
  }

  public virtual void TriggerGameUpdate(int seed)
  {
    UnityEngine.Random.seed = seed;
    ModelUtils.WithinGameTurn = true;
    this.world.GameUpdate();
    ModelUtils.WithinGameTurn = false;
    List<Vehicle> vehicleList = new List<Vehicle>((IEnumerable<Vehicle>) this.world.GetVehicles());
    this.world.ClearVehicles();
    if (vehicleList.Count > 0)
    {
      this.vehicles.AddRange((IEnumerable<Vehicle>) vehicleList);
      CInterfaceManager.instance.SpawnVehicles(vehicleList);
    }
    List<BonusIcon> bonusIconList = new List<BonusIcon>((IEnumerable<BonusIcon>) this.world.GetBonusIcons());
    this.world.ClearBonusIcons();
    if (bonusIconList.Count > 0)
    {
      this.bonusIcons.AddRange((IEnumerable<BonusIcon>) bonusIconList);
      if (!this.IsReplayActive)
        CInterfaceManager.instance.SpawnBonuses(bonusIconList);
      for (int index = 0; index < bonusIconList.Count; ++index)
      {
        if (!this.IsReplayActive && bonusIconList[index].type == BonusIcon.EBonusIconType.INFECT)
          this.replayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, this.world.DiseaseTurn, this.world.eventTurn, bonusIconList[index].disease, "INFECT: " + bonusIconList[index].country.id);
      }
    }
    if (!this.IsReplayActive)
      CInterfaceManager.instance.QueuePopups(this.world.popupMessages);
    this.world.popupMessages.Clear();
    List<IGame.NewsItem> newsItems = this.world.GetNewsItems();
    this.gameHistory.NewsStories.AddRange((IEnumerable<IGame.NewsItem>) newsItems);
    if (!this.IsReplayActive)
      CInterfaceManager.instance.AddNews(newsItems);
    this.world.ClearNewsItems();
    CInterfaceManager.instance.SpawnForts(this.world.GetCreatedForts());
    CInterfaceManager.instance.DestroyForts(this.world.GetDestroyedForts());
    CInterfaceManager.instance.UpdateApeLabs(this.world.GetUpdatedLabs());
    CInterfaceManager.instance.UpdateApeColonies(this.world.GetUpdatedColonies());
    CInterfaceManager.instance.UpdateCoopLabs(this.world.GetUpdatedCoopLabs());
    CInterfaceManager.instance.UpdateCastles(this.world.GetUpdatedCastles());
    CInterfaceManager.instance.UpdateVampires();
    this.CheckCureAnimation();
    this.world.ClearApeLabs();
    this.world.ClearApeColonies();
    this.world.ClearForts();
    this.world.ClearCoopLabs();
    this.world.ClearCastles();
    if (!this.IsReplayActive)
    {
      this.UpdateAchievements();
      int num = 5;
      if (CGameManager.gameType == IGame.GameType.Cure)
        num = 3;
      if (this.world.DiseaseTurn % num == 0 || this.world.DiseaseTurn == 1)
      {
        for (int index = 0; index < this.world.diseases.Count; ++index)
        {
          this.RecordStats(this.world.diseases[index]);
          if (this.world.diseases[index].isCure)
          {
            foreach (LocalDisease localDisease in this.world.diseases[index].localDiseases)
              this.RecordLocalStats(localDisease);
          }
        }
      }
    }
    this.OnWorldUpdate();
    ModelUtils.PrintCounter("POST GAME UPDATE");
    if (this.IsReplayActive)
      return;
    this.RecordEndTurnData();
  }

  public event Action OnWorldUpdate;

  public virtual void TriggerEventUpdate(int seed)
  {
    ModelUtils.WithinEventTurn = true;
    UnityEngine.Random.seed = seed;
    this.world.EventUpdate();
    ModelUtils.WithinEventTurn = false;
    if (!this.IsReplayActive)
      this.UpdateAchievements();
    if (!this.IsReplayActive)
      CInterfaceManager.instance.QueuePopups(this.world.popupMessages);
    this.world.popupMessages.Clear();
    CInterfaceManager.instance.UpdateApeLabs(this.world.GetUpdatedLabs());
    CInterfaceManager.instance.UpdateApeColonies(this.world.GetUpdatedColonies());
    CInterfaceManager.instance.UpdateCoopLabs(this.world.GetUpdatedCoopLabs());
    CInterfaceManager.instance.UpdateCastles(this.world.GetUpdatedCastles());
    CInterfaceManager.instance.UpdateVampires();
    this.world.ClearApeLabs();
    this.world.ClearApeColonies();
    this.world.ClearCoopLabs();
    this.world.ClearCastles();
    CInterfaceManager.instance.PostEventUpdate();
    ModelUtils.PrintCounter("POST EVENT UPDATE");
    if (this.IsReplayActive)
      return;
    this.RecordEndTurnData();
  }

  private void ReplayUpdate()
  {
    if (this.gameState != IGame.GameState.InProgress)
      return;
    for (int index1 = 0; index1 < this.actualSpeed; ++index1)
    {
      this.timer += Time.deltaTime;
      this.eventTimer += Time.deltaTime;
      bool flag1 = true;
      ModelUtils.ClearCounter();
      while (flag1)
      {
        bool flag2 = false;
        if (this.replayData.turnOrder != null && this.replayData.turnOrder.Count > 0 && this.replayTurnCounter >= this.replayData.turnOrder.Count)
        {
          Debug.Log((object) "OUT OF TURNS");
          this.gameState = IGame.GameState.EndGame;
          Debug.Log((object) "End of replay");
          return;
        }
        int num1 = -1;
        if (this.replayData.turnOrder != null && this.replayData.turnOrder.Count > 0)
          num1 = this.replayData.turnOrder[this.replayTurnCounter];
        if ((double) this.eventTimer > (double) this.secondsPerEvent && num1 < 0)
        {
          this.eventTimer -= this.secondsPerEvent;
          if (this.world.eventTurn < this.replayData.maxEventTurn)
          {
            this.TriggerEventUpdate(this.replayData.GetEventSeed(this.world.eventTurn + 1));
            this.CheckEndTurnData();
            flag2 = true;
          }
          ++this.replayTurnCounter;
        }
        if (flag2)
        {
          flag2 = false;
          List<ReplayData.ReplayEvent> events = this.replayData.GetEvents(this.world.DiseaseTurn, this.world.eventTurn);
          if (events != null)
          {
            for (int index2 = 0; index2 < events.Count; ++index2)
              this.FireReplayEvent(events[index2]);
          }
          ModelUtils.ClearCounter();
        }
        int num2 = 1;
        if (this.replayData.turnOrder != null && this.replayData.turnOrder.Count > 0)
          num2 = this.replayData.turnOrder[this.replayTurnCounter];
        if ((double) this.timer > (double) this.secondsPerTurn && num2 > 0)
        {
          this.timer -= this.secondsPerTurn;
          if (this.world.DiseaseTurn <= this.replayData.maxGameTurn)
          {
            this.TriggerGameUpdate(this.replayData.GetGameSeed(this.world.DiseaseTurn + 1));
            this.CheckEndTurnData();
            flag2 = true;
          }
          ++this.replayTurnCounter;
        }
        if (flag2)
        {
          List<ReplayData.ReplayEvent> events = this.replayData.GetEvents(this.world.DiseaseTurn, this.world.eventTurn);
          if (events != null)
          {
            for (int index3 = 0; index3 < events.Count; ++index3)
              this.FireReplayEvent(events[index3]);
          }
          ModelUtils.ClearCounter();
          this.CheckEndTurnData();
        }
        if (this.replayData.turnOrder != null && this.replayData.turnOrder.Count > 0 && this.replayTurnCounter >= this.replayData.turnOrder.Count)
        {
          Debug.Log((object) "OUT OF TURNS");
          return;
        }
        int num3 = 0;
        if (this.replayData.turnOrder != null && this.replayData.turnOrder.Count > 0)
          num3 = this.replayData.turnOrder[this.replayTurnCounter];
        flag1 = num3 <= 0 && (double) this.eventTimer > (double) this.secondsPerEvent || num3 >= 0 && (double) this.timer > (double) this.secondsPerTurn;
        if (this.world.gameEnded && this.world.turnsUntilGameEnd > 0)
        {
          this.gameState = IGame.GameState.EndGame;
          Debug.Log((object) "End of replay");
          return;
        }
        if (this.world.DiseaseTurn > this.replayData.maxGameTurn)
        {
          this.gameState = IGame.GameState.EndGame;
          Debug.Log((object) "End of replay");
          return;
        }
      }
    }
  }

  private void CheckEndTurnData()
  {
  }

  protected void RecordEndTurnData()
  {
  }

  protected virtual string GetPostTurnLog(Disease d)
  {
    return "[ID:" + (object) d.id + ", name:" + d.name + "]" + ", evoPoi: " + (object) d.evoPoints + ", dnaPoiGai: " + (object) d.dnaPointsGained + ", TotInf: " + (object) d.totalInfected + ", TotConInf: " + (object) d.totalControlledInfected + ", InfPoiPot: " + (object) d.infectedPointsPot + ", TotDead: " + (object) d.totalDead + ", DeadThisTurn: " + (object) d.deadThisTurn + ", CurPer: " + (object) d.cureCompletePercent;
  }

  protected virtual string GetReplayDesyncInfo(Disease d) => "";

  protected virtual void FireReplayEvent(ReplayData.ReplayEvent replayEvent)
  {
    Disease disease1 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
    switch (replayEvent.type)
    {
      case ReplayData.ReplayEventType.BONUS_ICON_PRESSED:
      case ReplayData.ReplayEventType.BONUS_ICON_FADED:
        BonusIcon bonusIcon = this.bonusIcons.Find((Predicate<BonusIcon>) (a => a.id == replayEvent.id));
        if (bonusIcon != null)
        {
          if (replayEvent.type == ReplayData.ReplayEventType.BONUS_ICON_PRESSED)
          {
            this.world.ClickBonusIcon(bonusIcon, disease1);
            CInterfaceManager.instance.PopBonus(bonusIcon, disease1);
          }
          else
          {
            this.world.HideBonusIcon(bonusIcon);
            CInterfaceManager.instance.HideBonus(bonusIcon);
          }
          this.bonusIcons.Remove(bonusIcon);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown bonus icon: " + (object) replayEvent.id + " - " + (object) replayEvent.type));
        break;
      case ReplayData.ReplayEventType.VEHICLE_ARRIVED:
        Vehicle vehicle1 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == replayEvent.id));
        if (vehicle1 != null)
        {
          this.world.VehicleArrived(vehicle1);
          this.vehicles.Remove(vehicle1);
          this.AddVehicleSpreadWave(vehicle1);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown vehicle: " + (object) replayEvent.id + " turn: " + (object) this.world.DiseaseTurn));
        break;
      case ReplayData.ReplayEventType.TECH_EVOLVED:
      case ReplayData.ReplayEventType.TECH_DEEVOLVED:
        Technology technology = disease1.GetTechnology(replayEvent.param);
        if (replayEvent.type == ReplayData.ReplayEventType.TECH_EVOLVED)
        {
          disease1.EvolveTech(technology, false);
          break;
        }
        if (replayEvent.type != ReplayData.ReplayEventType.TECH_DEEVOLVED)
          break;
        disease1.DeEvolveTech(technology);
        break;
      case ReplayData.ReplayEventType.REANIMATE:
        Disease disease2 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        this.Reanimate(this.world.GetCountry(replayEvent.param), disease2);
        break;
      case ReplayData.ReplayEventType.ZOMBIE_HORDE:
        Disease disease3 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        string[] strArray1 = replayEvent.param.Split(':');
        Country country1 = this.world.GetCountry(strArray1[0]);
        Country country2 = this.world.GetCountry(strArray1[1]);
        int result1 = -1;
        int.TryParse(strArray1[2], out result1);
        this.CreateZombieVehicle(country1, country2, disease3, CInterfaceManager.instance.GetCountryView(country1.id).transform.position, CInterfaceManager.instance.GetCountryView(country2.id).transform.position, result1);
        break;
      case ReplayData.ReplayEventType.NEURAX_PLANE:
        Disease disease4 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        string[] strArray2 = replayEvent.param.Split(':');
        Country country3 = this.world.GetCountry(strArray2[0]);
        Country country4 = this.world.GetCountry(strArray2[1]);
        int result2 = -1;
        int.TryParse(strArray2[2], out result2);
        CInterfaceManager.instance.TargetBubbleSuccess(CInterfaceManager.instance.GetCountryView(country4.id), Vector3.zero);
        this.CreateNeuraxVehicle(country3, country4, disease4, CInterfaceManager.instance.GetCountryView(country3.id).transform.position, CInterfaceManager.instance.GetCountryView(country4.id).transform.position, result2);
        break;
      case ReplayData.ReplayEventType.EXTRA_POPUPS_ENABLED:
        for (int index = 0; index < this.world.diseases.Count; ++index)
          this.world.diseases[index].showExtraPopups = true;
        break;
      case ReplayData.ReplayEventType.EXTRA_POPUPS_DISABLED:
        for (int index = 0; index < this.world.diseases.Count; ++index)
          this.world.diseases[index].showExtraPopups = false;
        break;
      case ReplayData.ReplayEventType.APE_HORDE_CREATED:
        string[] strArray3 = replayEvent.param.Split(':');
        Country country5 = this.world.GetCountry(strArray3[0]);
        Country country6 = this.world.GetCountry(strArray3[1]);
        Vector3 startPos1 = CUtils.UnserializeVector3(strArray3[2]);
        Vector3 endPos1 = CUtils.UnserializeVector3(strArray3[3]);
        Disease disease5 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        this.CreateApeVehicle(country5, country6, disease5, startPos1, endPos1);
        break;
      case ReplayData.ReplayEventType.APE_HORDE_CHANGE_COUNTRY:
        string[] strArray4 = replayEvent.param.Split(':');
        int id1 = int.Parse(strArray4[0]);
        Vehicle apeHorde = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id1));
        if (apeHorde == null)
        {
          Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown ape horde: " + (object) id1 + " turn: " + (object) this.world.DiseaseTurn));
          break;
        }
        Country country7 = this.world.GetCountry(strArray4[1]);
        Country country8 = this.world.GetCountry(strArray4[2]);
        this.world.ChangeApeHordeCountry(apeHorde, country7, country8);
        if (!this.IsReplayActive)
          break;
        apeHorde.currentCountry = country8;
        break;
      case ReplayData.ReplayEventType.VEHICLE_DISAPPEAR:
        Vehicle vehicle2 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == replayEvent.id));
        if (vehicle2 != null)
        {
          CInterfaceManager.instance.RemoveVehicle(vehicle2);
          this.vehicles.Remove(vehicle2);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown vehicle to disappear: " + (object) replayEvent.id));
        break;
      case ReplayData.ReplayEventType.DRONE_ATTACK:
        string[] strArray5 = replayEvent.param.Split(':');
        int id2 = int.Parse(strArray5[0]);
        Vehicle vehicle3 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id2));
        if (vehicle3 != null)
        {
          if (vehicle3.type == Vehicle.EVehicleType.TargetingDrone)
          {
            LocalDisease localDisease = vehicle3.actingDisease.GetLocalDisease(vehicle3.destination);
            if (localDisease.castleState == ECastleState.CASTLE_ALIVE)
            {
              localDisease.ChangeCastleStateF(ECastleState.CASTLE_DESTROYED);
              string[] strArray6 = strArray5[1].Split('|');
              float[] numArray = new float[strArray6.Length];
              for (int index = 0; index < numArray.Length; ++index)
                numArray[index] = float.Parse(strArray6[index]);
              List<Vampire> vampires = vehicle3.actingDisease.GetVampires(vehicle3.destination);
              for (int index = 0; index < vampires.Count && index < numArray.Length; ++index)
              {
                numArray[index] = ModelUtils.FloatRand(0.7f, 0.8f);
                vampires[index].vampireHealth *= numArray[index];
                if ((double) vampires[index].vampireHealth < 5.0)
                  vampires[index].vampireHealth = 5f;
              }
              CInterfaceManager.instance.DroneAttackEffect(true, vehicle3);
            }
          }
          else
          {
            vehicle3.currentCountry.GetLocalDisease(vehicle3.actingDisease).hasDrone = false;
            string[] strArray7 = strArray5[1].Split(',');
            string[] strArray8 = strArray5[2].Split(',');
            string[] strArray9 = strArray5[3].Split(',');
            List<Country> countryList1 = new List<Country>();
            List<float> floatList = new List<float>();
            List<Vehicle> vehicleList = new List<Vehicle>();
            List<Country> countryList2 = new List<Country>();
            for (int index = 0; index < strArray7.Length; ++index)
            {
              if (!string.IsNullOrEmpty(strArray7[index]))
              {
                string[] strArray10 = strArray7[index].Split('|');
                countryList1.Add(this.world.GetCountry(strArray10[0]));
                floatList.Add(float.Parse(strArray10[1]));
              }
            }
            for (int index = 0; index < strArray8.Length; ++index)
            {
              if (!string.IsNullOrEmpty(strArray8[index]))
              {
                int vid = int.Parse(strArray8[index]);
                Vehicle vehicle4 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == vid));
                if (vehicle4 != null)
                  vehicleList.Add(vehicle4);
                else
                  Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown horde to for drone to destroy: " + (object) id2));
              }
            }
            for (int index = 0; index < strArray9.Length; ++index)
            {
              if (!string.IsNullOrEmpty(strArray9[index]))
                countryList2.Add(this.world.GetCountry(strArray9[index]));
            }
            for (int index = 0; index < countryList1.Count; ++index)
            {
              Country country9 = countryList1[index];
              double num = (double) country9.DroneAttack(vehicle3.actingDisease, countryList2.Contains(country9), floatList[index]);
            }
            for (int index = 0; index < vehicleList.Count; ++index)
            {
              Vehicle vehicle5 = vehicleList[index];
              this.world.HordeKilled(vehicle5);
              CInterfaceManager.instance.RemoveVehicle(vehicle5);
              this.vehicles.Remove(vehicle5);
            }
            for (int index = 0; index < countryList2.Count; ++index)
              countryList2[index].ChangeApeColonyStateF(EApeColonyState.APE_COLONY_DESTROYED);
            bool success = countryList1.Count > 0;
            if (success)
            {
              this.world.ResetDroneData(vehicle3);
              CInterfaceManager.instance.DroneAttackEffect(success, vehicle3);
            }
          }
          this.vehicles.Remove(vehicle3);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown drone: " + (object) id2 + " turn: " + (object) this.world.DiseaseTurn));
        break;
      case ReplayData.ReplayEventType.VEHICLE_SOURCE_POSITION:
        string[] strArray11 = replayEvent.param.Split(':');
        int id3 = int.Parse(strArray11[0]);
        Vehicle vehicle6 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id3));
        if (vehicle6 != null)
        {
          vehicle6.sourcePosition = new Vector3?(CUtils.UnserializeVector3(strArray11[1]));
          vehicle6.currentPosition = vehicle6.sourcePosition;
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown vehicle to change source position on: " + (object) id3));
        break;
      case ReplayData.ReplayEventType.APE_RAMPAGE:
        Disease disease6 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        Country country10 = this.world.GetCountry(replayEvent.param);
        this.UseApeRampage(CInterfaceManager.instance.GetCountryView(country10.id), disease6);
        break;
      case ReplayData.ReplayEventType.APE_CREATE_COLONY:
        string[] strArray12 = replayEvent.param.Split(':');
        Country country11 = this.world.GetCountry(strArray12[0]);
        Vector3 position1 = CUtils.UnserializeVector3(strArray12[1]);
        this.CreateApeColony(country11, disease1, position1);
        break;
      case ReplayData.ReplayEventType.LOG_MESSAGE:
        break;
      case ReplayData.ReplayEventType.UNSCHEDULED_FLIGHT:
        Debug.Log((object) ("[" + (object) this.world.DiseaseTurn + "].ReplayEvent t:" + (object) replayEvent.type));
        string[] strArray13 = replayEvent.param.Split(':');
        Country country12 = this.world.GetCountry(strArray13[0]);
        Country country13 = this.world.GetCountry(strArray13[1]);
        int result3 = 0;
        int.TryParse(strArray13[2], out result3);
        this.ProcessUnscheduledFlight(disease1, country12, country13, CInterfaceManager.instance.GetCountryView(country12.id).transform.position, CInterfaceManager.instance.GetCountryView(country13.id).transform.position, result3);
        break;
      case ReplayData.ReplayEventType.IMMUNE_SHOCK:
        string[] strArray14 = replayEvent.param.Split(':');
        this.ProcessImmuneShock(this.world.GetCountry(strArray14[0]).id, new Vector3(float.Parse(strArray14[1]), float.Parse(strArray14[2]), float.Parse(strArray14[3])), disease1);
        break;
      case ReplayData.ReplayEventType.BENIGN_MIMIC:
        string[] strArray15 = replayEvent.param.Split(':');
        this.ProcessBenignMimic(this.world.GetCountry(strArray15[0]).id, new Vector3(float.Parse(strArray15[1]), float.Parse(strArray15[2]), float.Parse(strArray15[3])), disease1);
        break;
      case ReplayData.ReplayEventType.EVO_POINTS:
        disease1.evoPoints += replayEvent.id;
        break;
      case ReplayData.ReplayEventType.DNA_POINTS_GAINED:
        disease1.dnaPointsGained += replayEvent.id;
        break;
      case ReplayData.ReplayEventType.POST_TURN_LOG:
        break;
      case ReplayData.ReplayEventType.NEURAX_PLANE_LANDED:
        this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        string[] strArray16 = replayEvent.param.Split(':');
        Country country14 = this.world.GetCountry(strArray16[0]);
        double result4 = 0.0;
        double.TryParse(strArray16[1], out result4);
        int result5 = 0;
        int.TryParse(strArray16[2], out result5);
        (country14 as SPCountry).ProcessNeuraxPlaneArrival((Vehicle) null, disease1, result4, result5);
        break;
      case ReplayData.ReplayEventType.VEHICLE_PLANE_MINIFORT_ARRIVED:
        string[] strArray17 = replayEvent.param.Split(':');
        int id4 = int.Parse(strArray17[0]);
        int presetInt1 = int.Parse(strArray17[1]);
        Vehicle vehicle7 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id4));
        if (vehicle7 != null)
        {
          this.world.VehicleArrived(vehicle7, presetInt1);
          this.vehicles.Remove(vehicle7);
          this.AddVehicleSpreadWave(vehicle7);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown vehicle: " + (object) replayEvent.id + " turn: " + (object) this.world.DiseaseTurn));
        break;
      case ReplayData.ReplayEventType.INFECT_BOOST:
        string[] strArray18 = replayEvent.param.Split(':');
        this.ProcessInfectBoost(this.world.GetCountry(strArray18[0]).id, new Vector3(float.Parse(strArray18[1]), float.Parse(strArray18[2]), float.Parse(strArray18[3])), disease1);
        break;
      case ReplayData.ReplayEventType.LETHAL_BOOST:
        string[] strArray19 = replayEvent.param.Split(':');
        this.ProcessLethalBoost(this.world.GetCountry(strArray19[0]).id, new Vector3(float.Parse(strArray19[1]), float.Parse(strArray19[2]), float.Parse(strArray19[3])), disease1);
        break;
      case ReplayData.ReplayEventType.NUKE_LAUNCH:
        string[] strArray20 = replayEvent.param.Split(':');
        Country country15 = this.world.GetCountry(strArray20[0]);
        Country country16 = this.world.GetCountry(strArray20[1]);
        this.ProcessNukeLaunch(disease1, country15, country16, CInterfaceManager.instance.GetCountryView(country15.id).transform.position, CInterfaceManager.instance.GetCountryView(country16.id).transform.position);
        break;
      case ReplayData.ReplayEventType.NUKE_STRIKE:
        string[] strArray21 = replayEvent.param.Split(':');
        Country country17 = this.world.GetCountry(strArray21[0]);
        Vector3 vector3 = new Vector3(float.Parse(strArray21[1]), float.Parse(strArray21[2]), float.Parse(strArray21[3]));
        this.ProcessNukeStrike(country17.id, CInterfaceManager.instance.GetCountryView(country17.id).transform.position, disease1);
        break;
      case ReplayData.ReplayEventType.BLOOD_RAGE:
        Disease disease7 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        Country country18 = this.world.GetCountry(replayEvent.param);
        this.UseBloodRage(CInterfaceManager.instance.GetCountryView(country18.id), disease7);
        break;
      case ReplayData.ReplayEventType.VAMPIRE_VEHICLE_CREATED:
        string[] strArray22 = replayEvent.param.Split(':');
        Country country19 = this.world.GetCountry(strArray22[0]);
        Country country20 = this.world.GetCountry(strArray22[1]);
        Vector3 startPos2 = CUtils.UnserializeVector3(strArray22[2]);
        Vector3 endPos2 = CUtils.UnserializeVector3(strArray22[3]);
        Disease disease8 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        this.CreateVampireVehicle(country19, country20, disease8, startPos2, endPos2);
        this.ForceSpawnVehicles();
        break;
      case ReplayData.ReplayEventType.CREATE_CASTLE:
        string[] strArray23 = replayEvent.param.Split(':');
        Country country21 = this.world.GetCountry(strArray23[0]);
        Vector3 position2 = CUtils.UnserializeVector3(strArray23[1]);
        this.CreateCastle(country21, disease1, position2);
        break;
      case ReplayData.ReplayEventType.VEHICLE_PLANE_FORTESCAPEES_ARRIVED:
        string[] strArray24 = replayEvent.param.Split(':');
        int id5 = int.Parse(strArray24[0]);
        int presetInt2 = int.Parse(strArray24[1]);
        Vehicle vehicle8 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == id5));
        if (vehicle8 != null)
        {
          this.world.VehicleArrived(vehicle8, presetInt2);
          this.vehicles.Remove(vehicle8);
          this.AddVehicleSpreadWave(vehicle8);
          break;
        }
        Debug.LogError((object) (this.world.DiseaseTurn.ToString() + "/" + (object) this.world.eventTurn + " - Unknown vehicle: " + (object) replayEvent.id + " turn: " + (object) this.world.DiseaseTurn));
        break;
      case ReplayData.ReplayEventType.DRONE_CIRCLE:
        Vehicle v = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == replayEvent.id));
        World.instance.DroneStartCircling(v);
        break;
      case ReplayData.ReplayEventType.DRONE_DESTROYED:
        Vehicle vehicle9 = this.vehicles.Find((Predicate<Vehicle>) (a => a.id == replayEvent.id));
        World.instance.DroneDestroyed(vehicle9);
        CInterfaceManager.instance.DestroyDrone(vehicle9);
        break;
      case ReplayData.ReplayEventType.INVESTIGATION_TEAM:
        string[] strArray25 = replayEvent.param.Split(':');
        Country country22 = this.world.GetCountry(strArray25[0]);
        Vector3 endPosition = CUtils.UnserializeVector3(strArray25[1]);
        this.CreateInvestigationTeamFlight(country22, disease1, endPosition);
        break;
      case ReplayData.ReplayEventType.RAISE_PRIORITY:
        Disease disease9 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        Country country23 = this.world.GetCountry(replayEvent.param);
        this.UseRaisePriority(CInterfaceManager.instance.GetCountryView(country23.id), disease9);
        break;
      case ReplayData.ReplayEventType.ECONOMIC_SUPPORT:
        Disease disease10 = this.world.diseases.Find((Predicate<Disease>) (a => a.id == replayEvent.diseaseID));
        Country country24 = this.world.GetCountry(replayEvent.param);
        this.UseEconomicSupport(CInterfaceManager.instance.GetCountryView(country24.id), disease10);
        break;
      case ReplayData.ReplayEventType.VACCINE_KNOWLEDGE:
        disease1.vaccineKnowledge += float.Parse(replayEvent.param);
        break;
      case ReplayData.ReplayEventType.VACCINE_RESEARCH:
        disease1.globalCureResearch += float.Parse(replayEvent.param);
        break;
      default:
        Debug.LogError((object) ("Unknown event type: " + (object) replayEvent.type));
        break;
    }
  }

  public virtual GameStats RecordStats(Disease forDisease)
  {
    if (!this.gameHistory.GameStats.ContainsKey(forDisease.id))
      this.gameHistory.GameStats[forDisease.id] = new List<GameStats>();
    GameStats gameStats = new GameStats(forDisease);
    this.gameHistory.GameStats[forDisease.id].Add(gameStats);
    return gameStats;
  }

  public virtual LocalGameStats RecordLocalStats(LocalDisease forLocal)
  {
    int index = forLocal.GetIndex();
    if (!this.gameHistory.LocalGameStats.ContainsKey(index))
      this.gameHistory.LocalGameStats[index] = new List<LocalGameStats>();
    LocalGameStats localGameStats = new LocalGameStats(forLocal);
    this.gameHistory.LocalGameStats[index].Add(localGameStats);
    return localGameStats;
  }

  protected virtual void DebugDiseaseStats(GameStats stats)
  {
  }

  public virtual List<LocalGameStats> GetLocalHistory(LocalDisease forDisease)
  {
    int index = forDisease.GetIndex();
    return this.gameHistory.LocalGameStats.ContainsKey(index) ? this.gameHistory.LocalGameStats[index] : new List<LocalGameStats>();
  }

  public virtual List<GameStats> GetHistory(Disease forDisease)
  {
    return this.gameHistory.GameStats.ContainsKey(forDisease.id) ? this.gameHistory.GameStats[forDisease.id] : new List<GameStats>();
  }

  public void DroneDestroyed(Vehicle destroyedVehicle)
  {
    World.instance.DroneDestroyed(destroyedVehicle);
    this.vehicles.Remove(destroyedVehicle);
  }

  public virtual void VehicleDisappeared(Vehicle disappearedVehicle)
  {
    if (!this.IsReplayActive)
      this.replayData.AddEvent(ReplayData.ReplayEventType.VEHICLE_DISAPPEAR, this.world.DiseaseTurn, this.world.eventTurn, (Disease) null, disappearedVehicle.id.ToString());
    this.world.VehicleDisappeared(disappearedVehicle);
  }

  public virtual void VehicleArrived(Vehicle vehicle, Vector3? position = null)
  {
    if (this.IsReplayActive)
      return;
    if (vehicle.type == Vehicle.EVehicleType.TargetingDrone)
    {
      Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "]VEHICLE DRONE!!!"));
      LocalDisease localDisease = vehicle.actingDisease.GetLocalDisease(vehicle.destination);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(vehicle.id).Append(":");
      stringBuilder.Append(vehicle.destination.id);
      stringBuilder.Append(":");
      if (localDisease.castleState == ECastleState.CASTLE_ALIVE)
      {
        localDisease.ChangeCastleStateF(ECastleState.CASTLE_DESTROYED);
        List<Vampire> vampires = vehicle.actingDisease.GetVampires(vehicle.destination);
        float[] numArray = new float[vampires.Count];
        for (int index = 0; index < vampires.Count; ++index)
        {
          numArray[index] = ModelUtils.FloatRand(0.7f, 0.8f);
          vampires[index].vampireHealth *= numArray[index];
          if ((double) vampires[index].vampireHealth < 5.0)
            vampires[index].vampireHealth = 5f;
          if (index > 0)
            stringBuilder.Append("|");
          stringBuilder.Append(numArray[index]);
        }
        CInterfaceManager.instance.DroneAttackEffect(true, vehicle);
        CSoundManager.instance.PlaySFX("lair_destroyed");
      }
      this.vehicles.Remove(vehicle);
      this.replayData.AddEvent(ReplayData.ReplayEventType.DRONE_ATTACK, this.world.DiseaseTurn, this.world.eventTurn, vehicle.actingDisease, stringBuilder.ToString());
    }
    else if (vehicle.type == Vehicle.EVehicleType.Drone)
    {
      Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "]VEHICLE DRONE!!!"));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(vehicle.id).Append(":");
      HashSet<Country> countrySet = new HashSet<Country>();
      HashSet<Vehicle> vehicleSet = new HashSet<Vehicle>();
      List<Country> countryList = new List<Country>();
      LocalDisease localDisease1 = vehicle.currentCountry.GetLocalDisease(vehicle.actingDisease);
      localDisease1.hasDrone = false;
      Vehicle horde;
      if ((double) localDisease1.apeTotalAlivePopulation > 0.0 && vehicle.CheckHordeInRange(vehicle.currentCountry, out horde))
      {
        vehicleSet.Add(horde);
        countrySet.Add(vehicle.currentCountry);
      }
      else if ((double) localDisease1.apeTotalAlivePopulation > 0.0 && vehicle.CheckColonyInRange(vehicle.currentCountry))
      {
        countryList.Add(vehicle.currentCountry);
        countrySet.Add(vehicle.currentCountry);
      }
      foreach (Country country in this.world.countries)
      {
        if (country != vehicle.source)
        {
          LocalDisease localDisease2 = country.GetLocalDisease(vehicle.actingDisease);
          if ((double) localDisease2.apeTotalAlivePopulation > 0.0 && vehicle.CheckHordeInRange(country, out horde))
          {
            vehicleSet.Add(horde);
            countrySet.Add(country);
          }
          else if ((double) localDisease2.apeTotalAlivePopulation > 0.0 && vehicle.CheckColonyInRange(country))
          {
            countryList.Add(country);
            countrySet.Add(country);
          }
        }
      }
      foreach (Country country in countrySet)
      {
        float num = country.DroneAttack(vehicle.actingDisease, countryList.Contains(country));
        stringBuilder.Append(country.id).Append("|").Append(num).Append(",");
      }
      stringBuilder.Append(":");
      foreach (Vehicle vehicle1 in vehicleSet)
      {
        this.world.HordeKilled(vehicle1);
        stringBuilder.Append(vehicle1.id).Append(",");
        CInterfaceManager.instance.RemoveVehicle(vehicle1);
        this.vehicles.Remove(vehicle1);
      }
      stringBuilder.Append(":");
      foreach (Country country in countryList)
      {
        stringBuilder.Append(country.id).Append(",");
        country.ChangeApeColonyStateF(EApeColonyState.APE_COLONY_DESTROYED);
        CSoundManager.instance.PlaySFX("ape_colony_destroy");
      }
      bool success = countrySet.Count > 0;
      if (success)
      {
        this.world.ResetDroneData(vehicle);
        CInterfaceManager.instance.DroneAttackEffect(success, vehicle);
        CSoundManager.instance.PlaySFX("drone_attack");
      }
      else
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_evacuape));
      this.vehicles.Remove(vehicle);
      this.replayData.AddEvent(ReplayData.ReplayEventType.DRONE_ATTACK, this.world.DiseaseTurn, this.world.eventTurn, vehicle.actingDisease, stringBuilder.ToString());
    }
    else
    {
      Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
      long zombie = vehicle.destination.GetLocalDisease(disease).zombie;
      this.world.VehicleArrived(vehicle);
      if (vehicle.type == Vehicle.EVehicleType.ZombieHorde && vehicle.subType == Vehicle.EVehicleSubType.Normal)
      {
        long number = vehicle.destination.GetLocalDisease(disease).zombie - zombie;
        if (number > 0L)
        {
          if (!position.HasValue)
            position = new Vector3?(CInterfaceManager.instance.GetCountryView(vehicle.destination.id).transform.position);
          CInterfaceManager.instance.SetNecroaDisplay(number, position.Value);
        }
      }
      this.vehicles.Remove(vehicle);
      this.AddVehicleSpreadWave(vehicle, position);
      if (vehicle.infected != null)
      {
        for (int diseaseID = 0; diseaseID < vehicle.infected.Length; ++diseaseID)
        {
          if (vehicle.infected[diseaseID] > 0)
            DiseaseTrailParticles.instance.SetDiseaseTransferRoute(diseaseID, vehicle.source, vehicle.destination, vehicle.type);
        }
      }
    }
    if (vehicle == null || vehicle.actingDisease == null || vehicle.actingDisease.diseaseType != Disease.EDiseaseType.VAMPIRE || vehicle.subType != Vehicle.EVehicleSubType.MiniFort && vehicle.subType != Vehicle.EVehicleSubType.FortEscapees)
      return;
    CSoundManager.instance.PlaySFX("vampire_fort_create");
  }

  public virtual void AddVehicleSpreadWave(Vehicle vehicle, Vector3? position = null)
  {
    CountryView countryView = CInterfaceManager.instance.GetCountryView(vehicle.destination.id);
    bool flag = false;
    if (vehicle.infected != null)
    {
      for (int index = 0; index < vehicle.infected.Length; ++index)
      {
        if (vehicle.infected[index] > 0)
        {
          flag = true;
          if (vehicle.type == Vehicle.EVehicleType.Airplane && (bool) (UnityEngine.Object) countryView.mpAirport)
            countryView.AddSpreadWave(countryView.mpAirport.transform.position, this.world.diseases[index].id);
          else if (vehicle.type == Vehicle.EVehicleType.Boat && countryView.mpSeaport != null)
            countryView.AddSpreadWave(countryView.mpSeaport[0].transform.position, this.world.diseases[index].id);
          else
            countryView.AddSpreadWave(countryView.GetRandomPositionInsideCountry(), this.world.diseases[index].id);
        }
      }
    }
    if (flag || vehicle.zombies == null)
      return;
    for (int index = 0; index < vehicle.zombies.Length; ++index)
    {
      if (vehicle.zombies[index] > 0)
      {
        if (position.HasValue)
          countryView.AddSpreadWave(position.Value, this.world.diseases[index].id);
        else
          countryView.AddSpreadWave(countryView.GetRandomPositionInsideCountry(), this.world.diseases[index].id);
      }
    }
  }

  public virtual bool ClickBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive || !this.world.ClickBonusIcon(bonusIcon, CGameManager.localPlayerInfo.disease))
      return false;
    if (bonusIcon.type != BonusIcon.EBonusIconType.NUKE)
      this.bonusIcons.Remove(bonusIcon);
    CInterfaceManager.instance.ClickBonus(bonusIcon);
    this.replayData.AddEvent(ReplayData.ReplayEventType.BONUS_ICON_PRESSED, this.world.DiseaseTurn, this.world.eventTurn, CGameManager.localPlayerInfo.disease, bonusIcon.id);
    return true;
  }

  public virtual void StartHideBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive)
      return;
    CInterfaceManager.instance.StartHideBonus(bonusIcon);
  }

  public virtual void HideBonusIcon(BonusIcon bonusIcon)
  {
    if (this.IsReplayActive)
      return;
    this.bonusIcons.Remove(bonusIcon);
    this.world.HideBonusIcon(bonusIcon);
    this.replayData.AddEvent(ReplayData.ReplayEventType.BONUS_ICON_FADED, this.world.DiseaseTurn, this.world.eventTurn, (Disease) null, bonusIcon.id);
  }

  public bool CanSeeDots(Country c)
  {
    return c == null || this.CanSeeDots(c.GetLocalDisease(CGameManager.localPlayerInfo.disease));
  }

  public bool CanSeeDots(Disease d, Country c) => this.CanSeeDots(c.GetLocalDisease(d));

  protected virtual bool CanSeeDots(LocalDisease ld) => true;

  public virtual Disease GetMyDisease()
  {
    return World.instance != null && World.instance.diseases.Count > 0 ? World.instance.diseases[0] : (Disease) null;
  }

  public virtual Disease GetTheirDisease() => (Disease) null;

  public virtual Disease GetOtherDisease(Disease d) => (Disease) null;

  protected void CheckCureAnimation()
  {
    Disease myDisease = this.GetMyDisease();
    CHUDScreen screen = CUIManager.instance.GetScreen("HUDScreen") as CHUDScreen;
    if (this.currentThreshold < this.threshold.Count && (double) myDisease.cureCompletePercent >= (double) this.threshold[this.currentThreshold])
    {
      if ((double) myDisease.cureCompletePercent > 1.0)
        screen.CureCompleteAnimation();
      else
        screen.CureThresholdAnimation(myDisease.cureCompletePercent);
      ++this.currentThreshold;
    }
    else
    {
      if (this.currentThreshold <= 0 || (double) myDisease.cureCompletePercent >= (double) this.threshold[this.currentThreshold - 1])
        return;
      if (this.currentThreshold == this.threshold.Count)
        screen.StopCureAnimations();
      --this.currentThreshold;
    }
  }

  public virtual void SendChat(string message)
  {
  }

  public void AddChatMessage(IPlayerInfo player, string message)
  {
    int num = ModelUtils.IntRand(1, 65536);
    ++CGameManager.messageReceived;
    string timeStamp = IGame.GetTimeStamp(false);
    if (this.isChatMuted && player != null)
      return;
    ChatEntry chatEntry = new ChatEntry()
    {
      id = this.chatHistory.Count,
      player = player,
      message = message,
      createdAt = Time.time
    };
    this.chatHistory.Add(chatEntry);
    if (this.onChatAdded != null)
      this.onChatAdded(chatEntry);
    string str = IGame.EncodeBase64((CGameManager.messageReceived * num).ToString() + IGame.EncodeBase64((num / CGameManager.messageReceived).ToString() + player.PlayerID + "," + timeStamp + "," + message) + "114514");
    Debug.Log((object) ("\nChat Received: \nMessage Sender: " + player.name + "\nMessage Content: " + message + "\nTime Stamp: " + timeStamp + "\nRandom Seed: " + num.ToString() + "\nSignature Info: " + str));
  }

  public void ClearChat() => this.chatHistory.Clear();

  public void SetChatMute(bool isMuted) => this.isChatMuted = isMuted;

  protected void NotifyOpponentLeft(string name)
  {
    if (this.hasNotifiedOpponentLeft)
      return;
    this.hasNotifiedOpponentLeft = true;
    CLocalisationManager.GetText("MP_Chat_Player_Has_Left").Replace("%name", name);
    this.AddChatMessage((IPlayerInfo) null, name + " has left");
  }

  public virtual void SetGameParameters(GameParameters mpParams) => this.gameParameters = mpParams;

  protected IGame()
  {
    this.availableGenes = new List<Gene>();
    this.endTurnDatas = new List<EndTurnData>();
    this._actualSpeed = 1;
    this.wantedSpeed = 1;
    this.secondsPerTurn = 1f;
    this.secondsPerEvent = 0.7f;
    this.vehicles = new List<Vehicle>();
    this.bonusIcons = new List<BonusIcon>();
    this.chatHistory = new List<ChatEntry>();
    this.threshold = new List<float>()
    {
      0.01f,
      0.2f,
      0.4f,
      0.6f,
      0.8f,
      0.85f,
      0.9f,
      0.95f,
      1f
    };
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public IEnumerator GetFederalScenarioConstList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(IGame.constPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      CGameManager.scenarioConstList.Clear();
      CGameManager.constedScenarioList.Clear();
      string[] strArray1 = pendingRequest.downloadHandler.text.Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str.Split(chArray);
          string key = "PIFSL " + strArray2[0];
          int num1 = int.Parse(strArray2[1]);
          double num2 = (double) num1 / 10.0;
          CGameManager.scenarioConstList.Add(key, num1);
          CGameManager.constedScenarioList.Add(key);
        }
        CGameManager.GetRatingList();
      }
      else
        Debug.Log((object) "You must jave gotten a wrong const list, dumbass");
    }
    else
      Debug.LogError((object) ("Error while getting Federal Scenario Const List: \n" + pendingRequest.error));
  }

  public static string GetTimeStamp(bool bflag)
  {
    TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
    string empty = string.Empty;
    return !bflag ? Convert.ToInt64(timeSpan.TotalMilliseconds).ToString() : Convert.ToInt64(timeSpan.TotalSeconds).ToString();
  }

  public static string EncodeBase64(Encoding encode, string source)
  {
    byte[] bytes = encode.GetBytes(source);
    try
    {
      return Convert.ToBase64String(bytes);
    }
    catch
    {
      return source;
    }
  }

  public static string EncodeBase64(string source) => IGame.EncodeBase64(Encoding.UTF8, source);

  public static string DecodeBase64(Encoding encode, string result)
  {
    byte[] bytes = Convert.FromBase64String(result);
    try
    {
      return encode.GetString(bytes);
    }
    catch
    {
      return result;
    }
  }

  public static string DecodeBase64(string result) => IGame.DecodeBase64(Encoding.UTF8, result);

  public static string CalcMD5(string str)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    using (MD5 md5 = MD5.Create())
    {
      byte[] hash = md5.ComputeHash(bytes);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return stringBuilder.ToString();
    }
  }

  public static void playImportantVideo()
  {
    ((SPDisease) World.instance.diseases[0]).PerformAbnormalScenario("DreamParasiteBYD", "test");
  }

  public static void PlayScenarioVideo(string filename)
  {
    IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
    CUIManager.instance.SetActiveScreen(screen);
    string str = CGameManager.federalServerAddress + filename + ".mp4";
    CGameManager.playOnlineVideo = true;
    CGameManager.onlineVideoLink = str;
    CGameManager.canPlaySFX = false;
    CGameManager.ClearGame();
    DynamicMusic.instance.FadeOut();
    DynamicMusic.instance.state = DynamicMusic.MusicState.Stop;
    Main.showSplashVideo = true;
    CUIManager.instance.PlayVideo();
  }

  public static void DeleteAllSaves()
  {
    ((CSavesSteam) CGameManager.saves).DeleteAllGames();
    CUIManager.instance.standardConfirmOverlay.ShowLocalised("[ffff00]Congratulations![ffffff]", "All your [00ffff]saves[ffffff] have been nuked!", "YES", "OK");
  }

  public enum EndGameReason
  {
    COMPLETE,
    OPPONENT_LEFT,
    LOST_CONNECTION,
    DESYNC,
    CHEATING,
  }

  public enum EndGameResult
  {
    None,
    Infected,
    Dead,
    Cured,
    Disconnected,
    Resigned,
    Destroyed,
  }

  public enum GameType
  {
    Classic,
    SpeedRun,
    Official,
    Custom,
    Tutorial,
    Invalid,
    VersusMP,
    CoopMP,
    Cure,
    CureTutorial,
  }

  public enum GameState
  {
    None,
    Initialise,
    Choosing,
    ChoosingCountry,
    InProgress,
    EndGame,
  }

  public class GameSetupParameters
  {
    public bool lockDiseaseType;
    public Disease.EDiseaseType defaultDiseaseType;
    public Disease.EDiseaseType cureScenarioDiseaseType;
    public HashSet<Disease.EDiseaseType> allowedDiseaseTypes;
    public bool lockAllGenes;
    public List<Gene> defaultGenes = new List<Gene>();
    public IDictionary<Gene.EGeneCategory, bool> lockedCategories = (IDictionary<Gene.EGeneCategory, bool>) new Dictionary<Gene.EGeneCategory, bool>();
    public bool lockAllCheats;
    public bool fixedStartCountry;
    public string startCountryID = string.Empty;
    public bool lockDifficulty;
    public string difficulty = string.Empty;
    public bool lockName;
    public string defaultName = string.Empty;
    public long startDate;
  }

  public class NewsItem
  {
    public ParameterisedString news;
    public Disease[] diseases;
    public Country[] countries;
    public int turn;
    public int priority;

    public NewsItem()
    {
    }

    public NewsItem(ParameterisedString n, Disease d, Country c, int p)
    {
      this.news = n;
      this.diseases = new Disease[1]{ d };
      this.countries = new Country[1]{ c };
      this.priority = p;
    }

    public NewsItem(ParameterisedString n, Disease[] diseases, Country[] countries, int p)
    {
      this.news = n;
      this.diseases = diseases;
      this.countries = countries;
      this.priority = p;
    }

    public Disease disease
    {
      get => this.diseases != null && this.diseases.Length != 0 ? this.diseases[0] : (Disease) null;
    }

    public Country country
    {
      get
      {
        return this.countries != null && this.countries.Length != 0 ? this.countries[0] : (Country) null;
      }
    }
  }

  public class GameHistory
  {
    public Dictionary<int, List<GameStats>> GameStats { get; private set; }

    public Dictionary<int, List<LocalGameStats>> LocalGameStats { get; private set; }

    public List<IGame.NewsItem> NewsStories { get; private set; }

    public GameHistory()
    {
      this.GameStats = new Dictionary<int, List<GameStats>>();
      this.LocalGameStats = new Dictionary<int, List<LocalGameStats>>();
      this.NewsStories = new List<IGame.NewsItem>();
    }

    public void Clear()
    {
      this.GameStats.Clear();
      this.NewsStories.Clear();
      this.LocalGameStats.Clear();
    }
  }
}
