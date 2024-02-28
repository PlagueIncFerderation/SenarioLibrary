// Decompiled with JetBrains decompiler
// Type: Disease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class Disease
{
  public int pathFinderStatus;
  public string name;
  public string victoryTechId;
  public ParameterisedString endMessageTitle;
  public string endMessageImage;
  public ParameterisedString endMessageText;
  public string preemptiveTech;
  public string preemptiveName;
  public float evolveCostMultiplier = 1f;
  public bool tARandomMutations;
  public int randomNexusFlag = 1;
  public float customGlobalVariable1;
  public float customGlobalVariable2;
  public float customGlobalVariable3;
  public float customGlobalVariable4;
  public float customGlobalVariable5;
  public float customGlobalVariable6;
  public float[] curveDie = new float[21]
  {
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.01f,
    0.01f,
    0.01f,
    0.01f,
    0.01f,
    0.02f,
    0.02f,
    0.05f,
    0.85f,
    0.01f
  };
  public float[] curveHeal = new float[21]
  {
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.01f,
    0.01f,
    0.01f,
    0.01f,
    0.01f,
    0.02f,
    0.02f,
    0.05f,
    0.85f
  };
  public float[] curveInfect = new float[21]
  {
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.26705f,
    0.38856f,
    0.5311f,
    0.68194f,
    0.82258f,
    0.9321f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f,
    1f
  };
  public CircularBuffer authLossInfectedList = new CircularBuffer(7);
  public CircularBuffer authLossDeadList = new CircularBuffer(7);
  public CircularBuffer authLossOtherList = new CircularBuffer(7);
  public CircularBuffer authLossComplianceList = new CircularBuffer(7);
  public CircularBuffer authLossVaccineList = new CircularBuffer(7);
  public MovingAverage infectedPercWeeklyChange = new MovingAverage(7, 0.0f);
  public MovingAverage deadPercWeeklyChange = new MovingAverage(7, 0.0f);
  public MovingAverage authorityWeeklyChange = new MovingAverage(7, 0.0f);
  public MovingAverage researchWeeklyChange = new MovingAverage(7, 0.0f);
  public List<LockdownHistory> historyLockdown = new List<LockdownHistory>();
  public DiseaseSimulator simulator;
  public List<Technology> preload;
  public IDictionary<string, int> shuffleMap;
  public IDictionary<string, int> shuffleStartMap;
  public IDictionary<EAbilityType, float> aaCostMultipliers;
  public Dictionary<EAbilityType, int> aaUseCount = new Dictionary<EAbilityType, int>();
  public IDictionary<EAbilityType, int> aaCostAdditional;
  public IDictionary<string, int> techCostPaid;
  public IDictionary<string, int> techCostMod;
  public string diseaseTypeGui;
  [NonSerialized]
  public List<Gene> genes = new List<Gene>();
  [NonSerialized]
  public IDictionary<Country, LocalDisease> countryData = (IDictionary<Country, LocalDisease>) new Dictionary<Country, LocalDisease>();
  [NonSerialized]
  public List<LocalDisease> localDiseases = new List<LocalDisease>();
  [NonSerialized]
  public List<Technology> technologies = new List<Technology>();
  private const int TOP_CURE_COUNTRY_COUNT = 3;
  private Country[] topCureContributors = new Country[3];
  private int lastTopCureTurn = -1;
  [NonSerialized]
  protected Country _nexusCountry;
  [NonSerialized]
  protected Country _secondNexusCountry;
  [NonSerialized]
  protected Country _superCureCountry;
  [NonSerialized]
  protected Country _hqCountry;
  [NonSerialized]
  protected Country _teamTravelTarget;
  public List<Country> infectedCountries = new List<Country>();
  protected List<Country> apeLabCountries = new List<Country>();
  protected List<Country> apeColonyCountries = new List<Country>();
  public List<Vampire> vampires = new List<Vampire>();
  internal HashSet<string> activeAbilities = new HashSet<string>();
  internal List<History> history = new List<History>();
  internal HashSet<string> techEvolved = new HashSet<string>();
  internal HashSet<string> initialTechLock = new HashSet<string>();
  internal HashSet<string> initialTechPadlock = new HashSet<string>();
  internal bool hasInitialTechLock;
  internal bool hasInitialTechPadlock;
  private int totalCalculatedTurn = -1;
  private long _totalZombieCurrent;
  private long _totalInfectedCurrent;
  private long _totalKilledCurrent;
  private long _totalControlledInfectedCurrent;
  private long _totalUncontrolledInfectedCurrent;
  private long _totalInfectedApeCurrent;
  private long _totalDeadApeCurrent;
  private long _totalHealthyRecovered;
  private List<Disease.AuthorityLossReason>[] authLossReasons = new List<Disease.AuthorityLossReason>[2];
  private bool technologyNameImported;
  private List<string> technologyNames;
  public string gameEventFunctionParameter1;
  public string gameEventFunctionParameter2;
  public string gameEventFunctionParameter3;
  public string gameEventFunctionParameter4;
  public string gameEventFunctionParameter5;
  public int dnaStart;
  public int dnaExternal;
  public int dnaBubble;
  public int dnaParasite;
  public int dnaCure;
  public int dnaGainedTotal;

  [GameEventFunction]
  public void RecalculatePaths() => ++this.pathFinderStatus;

  [GameEventFunction]
  public virtual void CreateInitialVampire(Country c)
  {
    Debug.LogError((object) "CreateInitialVampire not supported.");
  }

  [GameEventFunction]
  public virtual void SetVampireNexus(Country c)
  {
    Debug.LogError((object) "SetVampireNexus not supported.");
  }

  [GameEventFunction]
  public virtual void CreatePurityVampire(Country c)
  {
    Debug.LogError((object) "CreatePurityVampire not supported.");
  }

  [GameEventFunction]
  public virtual void ChooseVampireNarrativePath()
  {
    Debug.LogError((object) "ChooseVampireNarrativePath not supported.");
  }

  [GameEventFunction]
  public virtual void CreateDraculaVampire(Country c)
  {
    Debug.LogError((object) "CreateDraculaVampire not supported.");
  }

  [GameEventFunction]
  public virtual void TemplarNuke(Country c)
  {
    Debug.LogError((object) "TemplarNuke not supported.");
  }

  [GameEventFunction]
  public virtual void BoardGameEndMessage()
  {
    Debug.LogError((object) "BoardGameEndMessage not supported.");
  }

  [GameEventFunction]
  public virtual void BoardGameDesignEnforce()
  {
    Debug.LogError((object) "BoardGameDesignEnforce not supported.");
  }

  [GameEventFunction]
  public virtual void BoardGameRepeatingHeadline()
  {
    Debug.LogError((object) "BoardGameRepeatingHeadline not supported.");
  }

  [GameEventFunction]
  public virtual void BoardGameLockSymptoms()
  {
    Debug.LogError((object) "BoardGameLockSymptoms not supported.");
  }

  [GameEventFunction]
  public virtual void BoardGameLockDesign()
  {
    Debug.LogError((object) "BoardGameLockDesign not supported.");
  }

  [GameEventFunction]
  public virtual void CustomForceZComSpawn(Country c)
  {
    Debug.LogError((object) "CustomForceZComSpawn not supported.");
  }

  [GameEventFunction]
  public virtual void CustomCreateVampire(Country c)
  {
    Debug.LogError((object) "CustomCreateVampire not supported.");
  }

  [GameEventFunction]
  public virtual void CustomHealVampires(Country c, float f)
  {
    Debug.LogError((object) "CustomHealVampire not supported.");
  }

  [GameEventFunction]
  public virtual void CustomHealVampires(float f)
  {
    Debug.LogError((object) "CustomHealGlobalVampires not supported.");
  }

  [GameEventFunction]
  public virtual void FakeNewsCreationConfirmation()
  {
    Debug.LogError((object) "FakeNewsCreationConfirmation not supported.");
  }

  [GameEventFunction]
  public virtual void CreateInvestigationTeam()
  {
    Debug.LogError((object) "CreateInvestigationTeam not supported.");
  }

  [GameEventFunction]
  public virtual void ApplyCureGenes() => Debug.LogError((object) "ApplyCureGenes not supported.");

  [GameEventFunction]
  public void SpawnVaccineBubble(Country c) => this.GetLocalDisease(c).SpawnVaccineBubble();

  [GameEventFunction]
  public virtual void CureAuthFallingPopup()
  {
    Debug.LogError((object) "CureAuthFallingPopup not supported");
  }

  [GameEventFunction]
  public virtual void SetFungusInfectedFromCountry(Country c)
  {
    Debug.LogError((object) "SetFungusInfectedFromCountry not supported");
  }

  [GameEventFunction]
  public virtual void CureVirusWarnDanger()
  {
    Debug.LogError((object) "CureVirusWarnDanger not supported");
  }

  [GameEventFunction]
  public virtual void CureComboCooperation()
  {
    Debug.LogError((object) "CureComboCooperation not supported");
  }

  [GameEventFunction]
  public virtual void CureComboSnitches()
  {
    Debug.LogError((object) "CureComboSnitches not supported");
  }

  public string diseaseName
  {
    get => this.name;
    set => this.name = value;
  }

  public abstract int id { get; set; }

  public abstract int evoPoints { get; set; }

  public abstract bool closedBordersSpreadEnhance { get; set; }

  public abstract bool nexusBonus { get; set; }

  public abstract float globalInfectiousness { get; set; }

  public abstract float globalInfectiousnessMax { get; set; }

  public abstract float globalSeverity { get; set; }

  public abstract float globalSeverityMax { get; set; }

  public abstract float globalLethality { get; set; }

  public abstract float globalLethalityMax { get; set; }

  public abstract float cureRequirements { get; set; }

  public abstract float cureRequirementBase { get; set; }

  public abstract float cureBaseMultiplier { get; set; }

  public abstract float researchInefficiencyMultiplier { get; set; }

  public abstract float researchInefficiency { get; set; }

  public abstract float globalCureResearch { get; set; }

  public abstract float globalCureResearchThisTurn { get; set; }

  public float cureCompletePerc
  {
    get => this.cureCompletePercent;
    set => this.cureCompletePercent = value;
  }

  public abstract float cureCompletePercent { get; set; }

  public abstract float globalAirRate { get; set; }

  public abstract float globalSeaRate { get; set; }

  public abstract float globalLandRate { get; set; }

  public float globalInfectedPerc
  {
    get => this.globalInfectedPercent;
    set => this.globalInfectedPercent = value;
  }

  public abstract float globalInfectedPercent { get; set; }

  public float globalZombiePerc
  {
    get => this.globalZombiePercent;
    set => this.globalZombiePercent = value;
  }

  public abstract float globalZombiePercent { get; set; }

  public abstract float infectedPointsPotAll { get; set; }

  public abstract float infectedPointsPot { get; set; }

  public abstract int dnaPointsGained { get; set; }

  public abstract float globalSeverityPlusLethality { get; set; }

  public abstract int natCatCounter { get; set; }

  public abstract int turnNumber { get; set; }

  public abstract float uncontrolledAttackBias { get; set; }

  public abstract int evoPointsSpent { get; set; }

  public abstract int evoPointsPrevTurn { get; set; }

  public abstract int numTurnsWithoutEvoChange { get; set; }

  public abstract int evoBoost { get; set; }

  public abstract int symptomExtraCost { get; set; }

  public abstract int abilityExtraCost { get; set; }

  public abstract int transmissionExtraCost { get; set; }

  public abstract bool symptomCostIncrease { get; set; }

  public abstract bool abilityCostIncrease { get; set; }

  public abstract bool transmissionCostIncrease { get; set; }

  public abstract float geneticDrift { get; set; }

  public abstract float geneticDriftMax { get; set; }

  public abstract int numDNABubbles { get; set; }

  public abstract int numDNABubblesWithoutTouch { get; set; }

  public abstract int numCureBubblesWithoutTouch { get; set; }

  public abstract int numInfectBubblesWithoutTouch { get; set; }

  public abstract int orangeBubbleMult { get; set; }

  public abstract int redBubbleMult { get; set; }

  public bool blueBubbleBonusDna
  {
    get => this.blueBubbleBonusDNA;
    set => this.blueBubbleBonusDNA = value;
  }

  public abstract bool blueBubbleBonusDNA { get; set; }

  public abstract bool bubbleAutopress { get; set; }

  public abstract bool dnaBubbleShowing { get; set; }

  public abstract int symptomDevolveCost { get; set; }

  public abstract int symptomDevolveCostIncrease { get; set; }

  public abstract int transmissionDevolveCost { get; set; }

  public abstract int transmissionDevolveCostIncrease { get; set; }

  public abstract int abilityDevolveCost { get; set; }

  public abstract int abilityDevolveCostIncrease { get; set; }

  public abstract bool cureFlag { get; set; }

  public abstract bool cureFlagOverride { get; set; }

  public abstract bool diseaseNoticed { get; set; }

  public abstract float wealthy { get; set; }

  public abstract float poverty { get; set; }

  public abstract float urban { get; set; }

  public abstract float rural { get; set; }

  public abstract float hot { get; set; }

  public abstract float cold { get; set; }

  public abstract float humid { get; set; }

  public abstract float arid { get; set; }

  public abstract float landTransmission { get; set; }

  public abstract float seaTransmission { get; set; }

  public abstract float airTransmission { get; set; }

  public abstract float corpseTransmission { get; set; }

  public abstract float mutation { get; set; }

  public abstract float mutationCounter { get; set; }

  public abstract float mutationTrigger { get; set; }

  public abstract bool transmissionRandomMutations { get; set; }

  public abstract bool abilityRandomMutations { get; set; }

  public abstract bool mutatedThisTurn { get; set; }

  public abstract float nexusMinInfect { get; set; }

  public abstract float nexusBonusGene { get; set; }

  public abstract float globalInfectiousnessTopMultipler { get; set; }

  public abstract float globalInfectiousnessBottomValue { get; set; }

  public abstract float globalSeverityTopMultipler { get; set; }

  public abstract float globalSeverityBottomValue { get; set; }

  public abstract float globalLethalityTopMultipler { get; set; }

  public abstract float globalLethalityBottomValue { get; set; }

  public abstract float cureRequirementBaseMultipler { get; set; }

  public abstract float infectedPointsPotChange { get; set; }

  public abstract int difficulty { get; set; }

  public abstract float difficultyVariable { get; set; }

  public abstract bool zday { get; set; }

  public abstract bool zdayDone { get; set; }

  public abstract bool firstFortSelected { get; set; }

  public abstract int zdayCounter { get; set; }

  public abstract int zombieDecreaseTurnCount { get; set; }

  public abstract int zdayLength { get; set; }

  public abstract int daysUntilMinifortPlaneSpawn { get; set; }

  public abstract int fortSelectionDay { get; set; }

  public abstract int numAliveForts { get; set; }

  public abstract float zdayDead { get; set; }

  public abstract float zdayInfected { get; set; }

  public abstract float zombieConversionMod { get; set; }

  public float zCombatStrength
  {
    get => this.zombieCombatStrength;
    set => this.zombieCombatStrength = value;
  }

  public abstract float zombieCombatStrength { get; set; }

  public float hCombatStrength
  {
    get => this.humanCombatStrength;
    set => this.humanCombatStrength = value;
  }

  public abstract float humanCombatStrength { get; set; }

  public abstract float zombieDecay { get; set; }

  public abstract float zombieInfect { get; set; }

  public abstract float zombieDecayTechMultiplier { get; set; }

  public abstract float globalZombieDecayMultiplier { get; set; }

  public abstract float hiZombifiedPopulation { get; set; }

  public abstract float fortDifficultyModifier { get; set; }

  public abstract float globalDecayChance { get; set; }

  public abstract float globalBattleVictoryCount { get; set; }

  public abstract bool hotDecayFlag { get; set; }

  public abstract int _ep2 { get; set; }

  public abstract float hordeSpeed { get; set; }

  public abstract float hordeSize { get; set; }

  public abstract float reanimateSize { get; set; }

  public abstract float hordeWaterSpeed { get; set; }

  public abstract float reanimateZombieCombatStrengthMod { get; set; }

  public abstract float wimpFlag { get; set; }

  public abstract int aaCostModifier { get; set; }

  public abstract bool zombieWin { get; set; }

  public abstract bool zombieLoss { get; set; }

  public abstract bool isSpeedRun { get; set; }

  public bool showPopups
  {
    get => this.showExtraPopups;
    set => this.showExtraPopups = value;
  }

  public abstract bool showExtraPopups { get; set; }

  public abstract float wormPlaneChance { get; set; }

  public abstract int wormBubbleLastDay { get; set; }

  public abstract int numWormBubblesWithoutTouch { get; set; }

  public abstract int daysToGameWin { get; set; }

  public abstract int wormBubbleHiddenStatus { get; set; }

  public abstract long totalInfected { get; set; }

  public abstract long totalControlledInfected { get; set; }

  public abstract long totalDead { get; set; }

  public abstract long totalKilled { get; set; }

  public abstract long totalZombie { get; set; }

  public abstract long totalHealthy { get; set; }

  public abstract long totalUninfected { get; set; }

  public abstract long infectedThisTurn { get; set; }

  public abstract long deadThisTurn { get; set; }

  public abstract long zombiesThisTurn { get; set; }

  public abstract long infectedApesThisTurn { get; set; }

  public abstract double averageInfected { get; set; }

  public abstract double averageDead { get; set; }

  public abstract float globalPriority { get; set; }

  public abstract float globalAwareness { get; set; }

  public abstract float cureResearchAllocationAverage { get; set; }

  public float publicOrderAvg
  {
    get => this.publicOrderAverage;
    set => this.publicOrderAverage = value;
  }

  public abstract float publicOrderAverage { get; set; }

  public abstract float bonusPriority { get; set; }

  public abstract int priorityCounter { get; set; }

  public abstract int recentEventCounter { get; set; }

  public float globalDeadPerc
  {
    get => this.globalDeadPercent;
    set => this.globalDeadPercent = value;
  }

  public abstract float globalDeadPercent { get; set; }

  public float globalHealthyPerc
  {
    get => this.globalHealthyPercent;
    set => this.globalHealthyPercent = value;
  }

  public abstract float globalHealthyPercent { get; set; }

  public float globalUninfectedPerc
  {
    get => this.globalUninfectedPercent;
    set => this.globalUninfectedPercent = value;
  }

  public abstract float globalUninfectedPercent { get; set; }

  public float globalKilledPerc
  {
    get => this.globalKilledPercent;
    set => this.globalKilledPercent = value;
  }

  public abstract float globalKilledPercent { get; set; }

  public abstract float globalDeadPriority { get; set; }

  public abstract float researchFocusPriority { get; set; }

  public abstract float researchFocusMod { get; set; }

  public abstract int infectedCountryCount { get; set; }

  public abstract int uninfectedCountryCount { get; set; }

  public abstract int lethalityDelayTurns { get; set; }

  public abstract int turnsUntilGameEnd { get; set; }

  public abstract int totalFlaskBroken { get; set; }

  public abstract int totalFlaskResearched { get; set; }

  public abstract int totalFlaskEmpty { get; set; }

  public bool zdayOrDone => this.zday || this.zdayDone;

  public abstract Disease.EDiseaseType diseaseType { get; set; }

  public float techMummy
  {
    get => this.zombieTechMummy;
    set => this.zombieTechMummy = value;
  }

  public abstract float zombieTechMummy { get; set; }

  public abstract int _t2 { get; set; }

  public abstract bool isScenario { get; set; }

  public abstract int numCheats { get; set; }

  public abstract int cheatFlags { get; set; }

  public abstract int countryOrigin { get; set; }

  public abstract float populationImmunity { get; set; }

  public abstract float apeTotalHealthy { get; set; }

  public abstract float apeTotalInfected { get; set; }

  public abstract float apeTotalDead { get; set; }

  public abstract float apeTotalAliveGlobal { get; set; }

  public float apeTotalHealthyPerc
  {
    get => this.apeTotalHealthyPercent;
    set => this.apeTotalHealthyPercent = value;
  }

  public abstract float apeTotalHealthyPercent { get; set; }

  public float apeTotalInfectedPerc
  {
    get => this.apeTotalInfectedPercent;
    set => this.apeTotalInfectedPercent = value;
  }

  public abstract float apeTotalInfectedPercent { get; set; }

  public float apeTotalDeadPerc
  {
    get => this.apeTotalDeadPercent;
    set => this.apeTotalDeadPercent = value;
  }

  public abstract float apeTotalDeadPercent { get; set; }

  public abstract float apeXSpeciesInfectiousness { get; set; }

  public abstract float apeInfectiousness { get; set; }

  public abstract float apeRescueAbility { get; set; }

  public abstract float apeStrength { get; set; }

  public abstract float apeLethality { get; set; }

  public abstract float apeIntelligence { get; set; }

  public abstract float apeSurvival { get; set; }

  public abstract float apeSpeed { get; set; }

  public abstract float changeToHumanImmunity { get; set; }

  public abstract long apeHordeStash { get; set; }

  public abstract float apeHordeSpeed { get; set; }

  public abstract int daysSinceGlobalDrone { get; set; }

  public abstract float apePriorityLevelGlobal { get; set; }

  public abstract float migrationCountryDistanceMax { get; set; }

  public abstract float migrationDistanceWaterMod { get; set; }

  public abstract float migrationDistanceLandMod { get; set; }

  public abstract int apeNoticed { get; set; }

  public abstract int intelligentApeNoticed { get; set; }

  public abstract int apeTotalLabs { get; set; }

  public abstract int apeMaxLabs { get; set; }

  public abstract int apeTotalColonies { get; set; }

  public abstract int apeMaxColonies { get; set; }

  public abstract int apeDaysSinceLastColonyBubble { get; set; }

  public abstract int apeColonyChance { get; set; }

  public abstract int apeNumCreateColonyAAUsed { get; set; }

  public abstract float labBaseResearch { get; set; }

  public abstract int labCounter { get; set; }

  public abstract int labSpawnThreshold { get; set; }

  public abstract float apeBonusPriorityGlobal { get; set; }

  public abstract int labDayCounter { get; set; }

  public abstract int apeTotalDestroyedLabs { get; set; }

  public abstract int apeEscapeFlag { get; set; }

  public abstract int genSysWorking { get; set; }

  public abstract int simianNarrativePath { get; set; }

  public abstract float colonyDNABoost { get; set; }

  public abstract float colonyInfectionBoost { get; set; }

  public abstract int droneAttackFlag { get; set; }

  public abstract int apeHumanResponse { get; set; }

  public abstract int apeColonyBonusPoints { get; set; }

  public abstract int apeSlowDeathFlag { get; set; }

  public abstract int apeInfectiousnessBonusFlag { get; set; }

  public abstract int labDestroyDnaFlag { get; set; }

  public abstract int reducedLabResearchFlag { get; set; }

  public abstract int droneThreshold { get; set; }

  public abstract long apeTotalPopulation { get; set; }

  public abstract int noIdeaFlag { get; set; }

  public abstract float sporeCounter { get; set; }

  public float decayPercReduction
  {
    get => this.decayPercentReduction;
    set => this.decayPercentReduction = value;
  }

  public abstract float decayPercentReduction { get; set; }

  public abstract float geneCompressionCounter { get; set; }

  public abstract float nucleicAcidFlag { get; set; }

  public abstract float replicationOverloadFlag { get; set; }

  public abstract float interceptorOverloadFlag { get; set; }

  public abstract float apeIntelligenceFlag { get; set; }

  public abstract float recomputePathsFlag { get; set; }

  public abstract int transcendenceFlag { get; set; }

  public virtual bool nexusVisibility { get; set; }

  public abstract int trojanInfectiousness { get; set; }

  public abstract int trojanLethality { get; set; }

  public abstract float trojanPublicOrder { get; set; }

  public abstract int trojanDna { get; set; }

  public abstract int trojanInfected { get; set; }

  public abstract int migrationCounter { get; set; }

  public virtual bool nuclearCountryChosen { get; set; }

  public virtual bool nukeArmed { get; set; }

  public virtual int nukeTimer { get; set; }

  public virtual Country nuclearCountry { get; set; }

  public virtual float accumulatedInfections { get; set; }

  public virtual float accumulatedCures { get; set; }

  public virtual float accumulatedIntelligentApes { get; set; }

  public virtual float accumulatedZombies { get; set; }

  public virtual float bubblesPopped { get; set; }

  public virtual bool nukeRussia { get; set; }

  public virtual bool nukeChina { get; set; }

  public virtual int fungusSporeTick { get; set; }

  public virtual int turnsSinceFungusSporeReleased { get; set; }

  public virtual float parasiteCorpseIncomeTurnDNA { get; set; }

  public virtual int parasiteSeverityIncBuffer { get; set; }

  public virtual int parasiteTotalSeverityFromOther { get; set; }

  public virtual bool isVampire { get; set; }

  public virtual bool vday { get; set; }

  public virtual bool vdayDone { get; set; }

  public virtual bool shadowDay { get; set; }

  public virtual int vampireBonus { get; set; }

  public virtual bool shadowDayDone { get; set; }

  public virtual int shadowDayCounter { get; set; }

  public virtual int vdayCounter { get; set; }

  public virtual int shadowDayLength { get; set; }

  public virtual int vdayLength { get; set; }

  public virtual int vampireInfectionBoost { get; set; }

  public virtual float castleHealingMod { get; set; }

  public virtual float castleColdClimateResearchMod { get; set; }

  public virtual int vampireStealthMod { get; set; }

  public virtual float vampireConversionMod { get; set; }

  public virtual bool shadowPlagueActive { get; set; }

  public virtual float castleWealthyResearchMod { get; set; }

  public virtual float massHypnosisCost { get; set; }

  public virtual float vampireHypnosisImpact { get; set; }

  public virtual int numCastleBubblesWithoutTouch { get; set; }

  public virtual int castleDnaCounter { get; set; }

  public virtual float castleReturnSpeed { get; set; }

  public virtual float castleColdResearch { get; set; }

  public virtual float castleHotResearch { get; set; }

  public virtual float castleWealthyResearch { get; set; }

  public virtual float castleHeatClimateResearchMod { get; set; }

  public virtual float vcomAlert { get; set; }

  public virtual int pulseCastles { get; set; }

  public virtual float vampireActivity { get; set; }

  public virtual int vampireLabLastspawn { get; set; }

  public virtual long vampireConversionPot { get; set; }

  public virtual long vampireConversionPotTrigger { get; set; }

  public virtual int purityEnabledFlag { get; set; }

  public virtual int vampLabWorking { get; set; }

  public virtual int vampLabsDestroyed { get; set; }

  public virtual int vampLabsCurrent { get; set; }

  public virtual float globalVampireActivityBonus { get; set; }

  public virtual int vampireNarrativeStory { get; set; }

  public virtual int vampHealthIncrease { get; set; }

  public virtual int vampLabFortDnaBonus { get; set; }

  public virtual int vampRageCostZero { get; set; }

  public virtual int vampBloodRageCasulatiesIncreased { get; set; }

  public virtual int vampBloodRageBonusDna { get; set; }

  public virtual int vampBatRangeBonus { get; set; }

  public virtual int vampFlightCostsZero { get; set; }

  public virtual int vampMoreHealthFasterFlight { get; set; }

  public virtual int vampFlyFasterLoseHealth { get; set; }

  public virtual int vampAutomaticBloodRage { get; set; }

  public virtual int vampHealingBonus { get; set; }

  public virtual int vampLairDefenseBonus { get; set; }

  public virtual int vampActivityLairCountryBonus { get; set; }

  public virtual int vampLairDnaQuicker { get; set; }

  public virtual int castleNumber { get; set; }

  public virtual int fortDestroyedNumber { get; set; }

  public virtual int numberOfFortsCreated { get; set; }

  public virtual float fortDamageBonus { get; set; }

  public virtual float fortCureBonus { get; set; }

  public virtual float numberOfFortsToSpawn { get; set; }

  public virtual int maxVampLabs { get; set; }

  public virtual float vampHealSacrificeMod { get; set; }

  public virtual int lairDroneAttackTimer { get; set; }

  public virtual float lairDroneAttackDuration { get; set; }

  public virtual int templarDestroyed { get; set; }

  public virtual long vampireHordeStash { get; set; }

  public virtual bool vampireWin { get; set; }

  public virtual bool vampireLoss { get; set; }

  public virtual int brexitExecute { get; set; }

  public virtual float deadBodyTransmission { get; set; }

  public virtual int themeQty { get; set; }

  public virtual int mechanicsQty { get; set; }

  public virtual int componentQty { get; set; }

  public virtual int playerQty { get; set; }

  public virtual bool isFakeNews { get; set; }

  public virtual bool fakeNewsStarted { get; set; }

  public virtual float fakeNewsInformDropPercent { get; set; }

  public virtual string nexusId => this.nexus != null ? this.nexus.id : "";

  public virtual bool isCure { get; set; }

  public virtual int cureDiseaseDiscoveredTurn { get; set; }

  public virtual int preSimulate { get; set; }

  public virtual long totalHealthyRecovered { get; set; }

  public virtual long totalInfectedIntel { get; set; }

  public virtual long totalDeadIntel { get; set; }

  public virtual long totalHealthyRecoveredIntel { get; set; }

  public virtual long totalInfectedIntelGUI { get; set; }

  public virtual long totalDeadIntelGUI { get; set; }

  public virtual long totalHealthyRecoveredIntelGUI { get; set; }

  public virtual long totalHealthyRecoveredGUI { get; set; }

  public virtual int numCountriesIntel { get; set; }

  public virtual int autoHqCounter { get; set; }

  public virtual int nextIntelSpread { get; set; }

  public virtual int intelTimeReduction { get; set; }

  public virtual bool intelInfectedFound { get; set; }

  public virtual Disease.EVaccineProgressStage vaccineStage { get; set; }

  public virtual float vaccineKnowledge { get; set; }

  public virtual float vaccineKnowledgeMonths { get; set; }

  public virtual float vaccineKnowledgeMonthsStart { get; set; }

  public virtual float vaccineDevMonths { get; set; }

  public virtual float vaccineManMonths { get; set; }

  public virtual float vaccineReleaseMonths { get; set; }

  public virtual float developmentSpeed { get; set; }

  public virtual float understandingSpeed { get; set; }

  public virtual float manufactureSpeed { get; set; }

  public virtual float manufactureSpeedAuthBonus { get; set; }

  public virtual float manufactureProgress { get; set; }

  public virtual bool manufactureSet { get; set; }

  public virtual int totalVaccineDuration { get; set; }

  public virtual int vaccineFailCount { get; set; }

  public virtual bool skipDevSuccess { get; set; }

  public virtual bool skipDevFired { get; set; }

  public virtual int barPulseCounter { get; set; }

  public virtual float globalMedicalCapacityMultiplier { get; set; }

  public virtual float globalInfectMod { get; set; }

  public virtual float globalInfectModMAX { get; set; }

  public virtual float globalLethalityMod { get; set; }

  public virtual float medicalCapacityEffectivenessMulti { get; set; }

  public virtual float economyDefenseEffectivenessMulti { get; set; }

  public virtual float contactTracingEffectiveness { get; set; }

  public virtual float contactTracingEffectivenessMod { get; set; }

  public virtual float contactTracingEffectivenessMult { get; set; }

  public virtual float lockdownEffectiveness { get; set; }

  public virtual float lockdownEffectivenessMod { get; set; }

  public virtual float lockdownEffectivenessMult { get; set; }

  public virtual float globalLocalPriorityMultiplier { get; set; }

  public virtual float globalPriorityAlertModifier { get; set; }

  public virtual float highestLocalPriority { get; set; }

  public virtual float estimatedDeathRate { get; set; }

  public virtual float connectedLocalPriorityMultiplier { get; set; }

  public virtual float influencePoints { get; set; }

  public virtual float globalBaseInfluence { get; set; }

  public virtual float quarantineInfluence { get; set; }

  public virtual int alertLevel { get; set; }

  public virtual float teamHighestInfectedPerc { get; set; }

  public virtual float globalUnrestMod { get; set; }

  public virtual float reproductionVisual { get; set; }

  public virtual float mortalityVisual { get; set; }

  public virtual float unrestVisual { get; set; }

  public virtual float medicalAidDuration { get; set; }

  public virtual float investigatorsDuration { get; set; }

  public virtual float tempLockdownDuration { get; set; }

  public virtual bool easyIntel { get; set; }

  public virtual float landTravelRestriction { get; set; }

  public virtual float airTravelRestriction { get; set; }

  public virtual float oceanTravelRestriction { get; set; }

  public virtual float seaScreeningChance { get; set; }

  public virtual float airScreeningChance { get; set; }

  public virtual float landScreeningChance { get; set; }

  public virtual float globalInfectedPercMAX { get; set; }

  public virtual int infectedCountriesMAX { get; set; }

  public virtual int turnsSinceKnowledge { get; set; }

  public virtual int turnsSinceDeadBubble { get; set; }

  public virtual int numTotalDeadBubbles { get; set; }

  public virtual float deadBubbleChance { get; set; }

  public virtual int numTotalDeadBubblesDNA { get; set; }

  public virtual int quarantinesActiveCount { get; set; }

  public virtual int supportsActiveCount { get; set; }

  public virtual float globalAvgCompliance { get; set; }

  public virtual float globalUnrestCount { get; set; }

  public virtual int lockdownTimerMAX { get; set; }

  public virtual int supportTimerMAX { get; set; }

  public virtual float economyTimeMulti { get; set; }

  public virtual long totalLockdownPopulation { get; set; }

  public virtual int totalActiveLockdowns { get; set; }

  public virtual int spreadCountries { get; set; }

  public virtual bool isNexusContinentBorder { get; set; }

  public virtual float startingAuthority { get; set; }

  public virtual float baseLethality { get; set; }

  public virtual float baseInfectivity { get; set; }

  public virtual float authority { get; set; }

  public virtual float lowestAuthority { get; set; }

  public virtual float authorityDeduction { get; set; }

  public virtual float authorityMod { get; set; }

  public virtual float authorityModHighest { get; set; }

  public virtual float authorityGainHold { get; set; }

  public virtual float authLossInfected { get; set; }

  public virtual float authLossInfectedDelay { get; set; }

  public virtual float authLossInfectedHighest { get; set; }

  public virtual float authLossInfectedActual { get; set; }

  public virtual float authLossInfectedPermanencePerc { get; set; }

  public virtual float authLossInfectedYesterday { get; set; }

  public virtual float authLossInfMulti { get; set; }

  public virtual float authLossDead { get; set; }

  public virtual float authLossDeadDelay { get; set; }

  public virtual float authLossDeadHighest { get; set; }

  public virtual float authLossDeadActual { get; set; }

  public virtual float authLossDeadPermanencePerc { get; set; }

  public virtual float authLossDeadYesterday { get; set; }

  public virtual float authLossDeadMulti { get; set; }

  public virtual float authLossSpread { get; set; }

  public virtual float authLossSpreadYesterday { get; set; }

  public virtual float authLossCompliance { get; set; }

  public virtual float authLossComplianceMulti { get; set; }

  public virtual float authCensorship { get; set; }

  public virtual float authCensorshipTimer { get; set; }

  public virtual float authSpreadMod { get; set; }

  public virtual int authFreezeCount { get; set; }

  public virtual float authWinMin { get; set; }

  public virtual int authLossDaysToTrack { get; set; }

  public virtual float complianceReturnPerc { get; set; }

  public virtual int globalBorderClosedNum { get; set; }

  public virtual int globalAirportClosedNum { get; set; }

  public virtual int globalPortClosedNum { get; set; }

  public virtual float globalBorderClosedPerc { get; set; }

  public virtual float globalAirportClosedPerc { get; set; }

  public virtual float globalPortClosedPerc { get; set; }

  public virtual int infectedCountriesIntel { get; set; }

  public virtual float diseaseDNAComplexity { get; set; }

  public virtual float globalVaccineSpeed { get; set; }

  public virtual float bonusLethality { get; set; }

  public virtual float scoreDeadImpactMulti { get; set; }

  public virtual float narrativeEventCounter { get; set; }

  public virtual float narrativeEventGap { get; set; }

  public virtual float cureBioweaponStrengthMod { get; set; }

  public virtual float cureBioweaponImpact { get; set; }

  public virtual float bioweaponLethalityGain { get; set; }

  public virtual float bioweaponInfectivityGain { get; set; }

  public virtual int prionIncubationMonths { get; set; }

  public virtual float prionVaccineSpeedMulti { get; set; }

  public virtual float prionInfectedThresholdMulti { get; set; }

  public virtual int preSimMAX { get; set; }

  public virtual int nanovirusPauseTimer { get; set; }

  public virtual int nanovirusEndTimer { get; set; }

  public virtual bool nanovirusHeatWeak { get; set; }

  public virtual bool nanovirusColdWeak { get; set; }

  public virtual bool isNanovirus { get; set; }

  public virtual bool isFungus { get; set; }

  public virtual bool isPrion { get; set; }

  public virtual float fungusBloom { get; set; }

  public virtual float fungusSporeMulti { get; set; }

  public virtual int fungusSporesReleased { get; set; }

  public virtual float localPopUpper { get; set; }

  public virtual float localPopLower { get; set; }

  public virtual float localPopThreshold { get; set; }

  public virtual float nationalPopUpper { get; set; }

  public virtual float nationalPopLower { get; set; }

  public virtual float nationalPopThreshold { get; set; }

  public virtual float reproductionBarScale { get; set; }

  public virtual float mortalityBarScale { get; set; }

  public virtual int diseaseLengthMulti { get; set; }

  public virtual float publicOrderAuthorityLossMod { get; set; }

  public virtual float averageCountryInfectedPerc { get; set; }

  public virtual float averageCountryDeadPerc { get; set; }

  public virtual float maxAuthLossPerCountry { get; set; }

  public virtual Disease.EGenericDiseaseFlag genericFlags { get; set; }

  public bool HasFlag(Disease.EGenericDiseaseFlag flag) => (flag & this.genericFlags) != 0;

  public void AddFlag(Disease.EGenericDiseaseFlag flag) => this.genericFlags |= flag;

  public void RemoveFlag(Disease.EGenericDiseaseFlag flag) => this.genericFlags &= ~flag;

  public virtual Disease.ECureScenario cureScenario { get; set; }

  public Country.EContinentType nexusContinent
  {
    get => this.nexus != null ? this.nexus.continentType : Country.EContinentType.NONE;
  }

  public float infectedWeekly
  {
    get
    {
      return (double) this.infectedPercWeeklyChange.Oldest() != 0.0 ? (float) (((double) this.globalInfectedPercent - (double) this.infectedPercWeeklyChange.Oldest()) / (double) this.infectedPercWeeklyChange.Oldest() * 100.0) : 0.0f;
    }
  }

  public float deadWeekly
  {
    get
    {
      return (double) this.deadPercWeeklyChange.Oldest() != 0.0 ? (float) (((double) this.globalDeadPercent - (double) this.deadPercWeeklyChange.Oldest()) / (double) this.deadPercWeeklyChange.Oldest() * 100.0) : 0.0f;
    }
  }

  public float authorityWeekly
  {
    get
    {
      return (double) this.authorityWeeklyChange.Oldest() != 0.0 ? (float) (((double) this.authority - (double) this.authorityWeeklyChange.Oldest()) * 100.0) : 0.0f;
    }
  }

  public float researchWeekly
  {
    get
    {
      return (double) this.researchWeeklyChange.Oldest() != 0.0 ? (float) (((double) this.globalCureResearch - (double) this.researchWeeklyChange.Oldest()) / (double) this.researchWeeklyChange.Oldest() * 100.0) : 0.0f;
    }
  }

  public virtual long lastTotalInfectedIntelGUI { get; set; }

  public virtual long lastTotalDeadIntelGUI { get; set; }

  public abstract bool CheckEvo();

  public abstract bool CheckTurn();

  public abstract void Initialise();

  public abstract void GameUpdate();

  public abstract long GetScore(bool won, bool scenario);

  public abstract LocalDisease CreateLocalDisease(Country country);

  public abstract int GetEvolveCost(Technology technology);

  public abstract int GetDeEvolveCost(Technology technology);

  public abstract void OnBonusIconClicked(BonusIcon bonusIcon);

  public abstract string scenario { get; set; }

  public virtual string diseaseModelType { get; set; }

  public virtual float weakestGlobalVampireHealth { get; set; }

  public float CustomHealGlobalVampires
  {
    get => 0.0f;
    set => this.CustomHealVampires(value);
  }

  public virtual long totalHealthyIntel { get; set; }

  public virtual bool authorityFailFlag { get; set; }

  public virtual float GetActiveAbilityCost(ActiveAbility ability, EAbilityType abilityType)
  {
    if (ability.aaIsFree > 0 || this.vampRageCostZero > 0 && abilityType == EAbilityType.bloodrage || this.vampFlightCostsZero > 0 && abilityType == EAbilityType.vampiretravel)
      return 0.0f;
    float a = (float) ability.aaCost * this.GetAACostMultiplier(abilityType) + (float) this.GetAACostIncrease(abilityType) + (float) this.GetActiveAbilityUse(abilityType) - (float) this.aaCostModifier;
    if (CGameManager.IsMultiplayerGame && this.diseaseType == Disease.EDiseaseType.FUNGUS && abilityType == EAbilityType.unscheduled_flight)
      a *= 2f;
    return Mathf.Max(a, 1f);
  }

  public float GetAACostMultiplier(EAbilityType abilityType)
  {
    return this.aaCostMultipliers == null || !this.aaCostMultipliers.ContainsKey(abilityType) ? 1f : this.aaCostMultipliers[abilityType];
  }

  public int GetAACostIncrease(EAbilityType abilityType)
  {
    return this.aaCostAdditional == null || !this.aaCostAdditional.ContainsKey(abilityType) ? 0 : this.aaCostAdditional[abilityType];
  }

  public int GetTechCostPaid(Technology tech)
  {
    return this.techCostPaid == null || tech == null || !this.techCostPaid.ContainsKey(tech.id.ToLowerInvariant()) ? 0 : this.techCostPaid[tech.id.ToLowerInvariant()];
  }

  public int GetTechCostMod(Technology tech)
  {
    return this.techCostMod == null || tech == null || !this.techCostMod.ContainsKey(tech.id.ToLowerInvariant()) ? 0 : this.techCostMod[tech.id.ToLowerInvariant()];
  }

  public void SetTechCostMod(Technology tech, int mod)
  {
    if (tech == null)
      return;
    if (this.techCostMod == null)
      this.techCostMod = (IDictionary<string, int>) new Dictionary<string, int>();
    this.techCostMod[tech.id.ToLowerInvariant()] = mod;
  }

  public virtual void RecordTechCostPaid(Technology tech, int cost)
  {
    if (this.techCostPaid == null)
      this.techCostPaid = (IDictionary<string, int>) new Dictionary<string, int>();
    this.techCostPaid[tech.id.ToLowerInvariant()] = cost;
  }

  public virtual void RecordActiveAbilityUse(EAbilityType type)
  {
    this.history.Add((History) new AbilityHistory(type, this.turnNumber, this.history.Count, this));
    if (!this.aaUseCount.ContainsKey(type))
      this.aaUseCount[type] = 1;
    else
      this.aaUseCount[type]++;
  }

  public virtual int GetActiveAbilityUse(EAbilityType type)
  {
    if (!this.aaUseCount.ContainsKey(type))
      this.aaUseCount[type] = 0;
    return this.aaUseCount[type];
  }

  public void SetAACostMultiplier(EAbilityType abilityType, float multiplier)
  {
    if (this.aaCostMultipliers == null)
      this.aaCostMultipliers = (IDictionary<EAbilityType, float>) new Dictionary<EAbilityType, float>();
    this.aaCostMultipliers[abilityType] = multiplier;
  }

  public void SetAACostAdditional(EAbilityType abilityType, int cost)
  {
    if (this.aaCostAdditional == null)
      this.aaCostAdditional = (IDictionary<EAbilityType, int>) new Dictionary<EAbilityType, int>();
    this.aaCostAdditional[abilityType] = cost;
  }

  public static int GetCheatFlag(Disease.ECheatType cheatType)
  {
    return 1 << (int) (cheatType - 1 & (Disease.ECheatType) 31);
  }

  public static int GetCheatFlags(Disease.ECheatType[] cheatTypes)
  {
    int cheatFlags = 0;
    if (cheatTypes != null)
    {
      for (int index = 0; index < cheatTypes.Length; ++index)
      {
        if (cheatTypes[index] != Disease.ECheatType.NONE)
          cheatFlags |= Disease.GetCheatFlag(cheatTypes[index]);
      }
    }
    return cheatFlags;
  }

  public void AddCheat(Disease.ECheatType cheatType)
  {
    if (cheatType == Disease.ECheatType.NONE)
      return;
    int cheatFlag = Disease.GetCheatFlag(cheatType);
    if ((this.cheatFlags & cheatFlag) != 0)
      return;
    ++this.numCheats;
    this.cheatFlags |= cheatFlag;
  }

  public void SetCheats(Disease.ECheatType[] cheatTypes)
  {
    this.ClearCheats();
    for (int index = 0; index < cheatTypes.Length; ++index)
      this.AddCheat(cheatTypes[index]);
  }

  public void SetCheatFlags(int flags)
  {
    this.cheatFlags = flags;
    this.numCheats = 0;
    for (int index = 0; index < 31; ++index)
    {
      if ((flags & 1 << index) != 0)
        ++this.numCheats;
    }
  }

  public Disease.ECheatType[] GetCheats()
  {
    List<Disease.ECheatType> echeatTypeList = new List<Disease.ECheatType>();
    Disease.ECheatType[] values = (Disease.ECheatType[]) Enum.GetValues(typeof (Disease.ECheatType));
    for (int index = 0; index < values.Length; ++index)
    {
      if (this.HasCheat(values[index]))
        echeatTypeList.Add(values[index]);
    }
    return echeatTypeList.ToArray();
  }

  public void ClearCheats()
  {
    this.cheatFlags = 0;
    this.numCheats = 0;
  }

  public bool HasCheat(Disease.ECheatType cheatType)
  {
    return (this.cheatFlags & Disease.GetCheatFlag(cheatType)) > 0;
  }

  public int CureDaysRemaining
  {
    get
    {
      float num = this.globalCureResearchThisTurn - this.globalCureResearchThisTurn * this.researchInefficiencyMultiplier;
      return (double) num > 0.0 ? Mathf.Clamp(Mathf.FloorToInt((this.cureRequirements - this.globalCureResearch) / num), 0, 100000) : -1;
    }
  }

  public Country[] GetTopCureContributors()
  {
    if (this.turnNumber != this.lastTopCureTurn)
    {
      this.localDiseases.Sort((Comparison<LocalDisease>) ((a, b) => b.localCureResearch.CompareTo(a.localCureResearch)));
      for (int index = 0; index < 3 && index < this.localDiseases.Count; ++index)
        this.topCureContributors[index] = (double) this.localDiseases[index].localCureResearch <= 0.0 ? (Country) null : this.localDiseases[index].country;
      this.lastTopCureTurn = this.turnNumber;
    }
    return this.topCureContributors;
  }

  public List<Country> GetCureContributors()
  {
    List<Country> cureContributors = new List<Country>();
    this.localDiseases.Sort((Comparison<LocalDisease>) ((a, b) => b.localCureResearch.CompareTo(a.localCureResearch)));
    for (int index = 0; index < this.localDiseases.Count && (double) this.localDiseases[index].localCureResearch > 0.0; ++index)
      cureContributors.Add(this.localDiseases[index].country);
    return cureContributors;
  }

  public virtual Country nexus
  {
    get => this._nexusCountry;
    set => this._nexusCountry = value;
  }

  public virtual Country secondNexus
  {
    get => this._secondNexusCountry;
    set => this._secondNexusCountry = value;
  }

  public virtual Country superCureCountry
  {
    get => this._superCureCountry;
    set => this._superCureCountry = value;
  }

  public virtual Country hqCountry
  {
    get => this._hqCountry;
    set => this._hqCountry = value;
  }

  public virtual Country teamTravelTarget
  {
    get => this._teamTravelTarget;
    set => this._teamTravelTarget = value;
  }

  public bool IsAbilityActive(string n) => this.activeAbilities.Contains(n);

  public void SetAbilityActive(string n) => this.activeAbilities.Add(n);

  public bool IsTechEvolved(string techID)
  {
    return techID != null && this.techEvolved.Contains(techID.ToLowerInvariant());
  }

  public bool IsTechEvolved(Technology tech) => this.IsTechEvolved(tech.id.ToLowerInvariant());

  public Technology GetTechnology(string techID)
  {
    return this.technologies.Find((Predicate<Technology>) (a => a.id.ToLowerInvariant() == techID.ToLowerInvariant()));
  }

  public bool CanDeEvolve(Technology tech)
  {
    return !tech.cantDevolve && (double) tech.devolveCostMultipler > -1.0;
  }

  public bool CanEvolve(Technology tech)
  {
    if (this.HasFlag(Disease.EGenericDiseaseFlag.CheatCureShuffle) || this.HasFlag(Disease.EGenericDiseaseFlag.CheatShuffle))
      return true;
    if (tech.eventLocked)
    {
      Debug.Log((object) (tech.id + " is eventLocked"));
      return false;
    }
    if (tech.notTechAND.Count > 0)
    {
      bool flag = true;
      for (int index = 0; index < tech.notTechAND.Count; ++index)
      {
        if (!string.IsNullOrEmpty(tech.notTechAND[index]) && !this.IsTechEvolved(tech.notTechAND[index]))
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return false;
    }
    for (int index = 0; index < tech.notTechOR.Count; ++index)
    {
      if (!string.IsNullOrEmpty(tech.notTechOR[index]) && this.IsTechEvolved(tech.notTechOR[index]))
        return false;
    }
    for (int index = 0; index < tech.requiredTechAND.Count; ++index)
    {
      if (!string.IsNullOrEmpty(tech.requiredTechAND[index]) && !this.IsTechEvolved(tech.requiredTechAND[index]) && this.GetTechnology(tech.requiredTechAND[index]) != null)
        return false;
    }
    if (tech.requiredTechOR.Count <= 0)
      return true;
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < tech.requiredTechOR.Count; ++index)
    {
      if (this.GetTechnology(tech.requiredTechOR[index]) != null)
        flag1 = true;
      if (!string.IsNullOrEmpty(tech.requiredTechOR[index]) && this.IsTechEvolved(tech.requiredTechOR[index]))
      {
        flag2 = true;
        break;
      }
    }
    return !flag1 || flag2;
  }

  public virtual bool CanMutate(string tech) => true;

  public void EventLockTech(string techID, bool locked)
  {
    Technology technology = this.GetTechnology(techID);
    if (technology == null)
      return;
    technology.eventLocked = locked;
  }

  public void PadlockTech(string techID, bool locked)
  {
    Technology technology = this.GetTechnology(techID);
    if (technology == null || locked && this.IsTechEvolved(technology))
      return;
    technology.padlocked = locked;
  }

  public void EventLockTech(Technology.ETechType type, bool locked)
  {
    for (int index = 0; index < this.technologies.Count; ++index)
    {
      Technology technology = this.technologies[index];
      if (technology.gridType == type)
        technology.eventLocked = locked;
    }
  }

  public void PadlockTech(Technology.ETechType type, bool locked)
  {
    for (int index = 0; index < this.technologies.Count; ++index)
    {
      Technology technology = this.technologies[index];
      if (technology.gridType == type)
        technology.padlocked = locked;
    }
  }

  public Technology.ETechType evolveTech
  {
    set => this.EvolveRandomTech(value);
  }

  public Technology EvolveRandomTech(Technology.ETechType type)
  {
    List<Technology> technologyList = new List<Technology>();
    for (int index = 0; index < this.technologies.Count; ++index)
    {
      Technology technology = this.technologies[index];
      if (!this.IsTechEvolved(technology) && this.CanEvolve(technology) && (type == Technology.ETechType.all || type == technology.gridType) && this.CanMutate(technology.id))
        technologyList.Add(technology);
    }
    if (technologyList.Count <= 0)
      return (Technology) null;
    int index1 = ModelUtils.IntRand(0, technologyList.Count - 1);
    Technology tech = technologyList[index1];
    this.EvolveTech(tech, true);
    return tech;
  }

  public Technology EvolveRandomTech(string[] list)
  {
    Technology.ETechType etechType = Technology.ETechType.transmission;
    if (this.tARandomMutations)
    {
      float num = ModelUtils.FloatRand(0.0f, 1f);
      etechType = (double) num <= 0.5 ? ((double) num <= 0.25 ? Technology.ETechType.transmission : Technology.ETechType.ability) : Technology.ETechType.symptom;
    }
    List<Technology> technologyList = new List<Technology>();
    HashSet<string> stringSet = (HashSet<string>) null;
    if (list != null && list.Length != 0)
      stringSet = new HashSet<string>((IEnumerable<string>) list);
    for (int index = 0; index < this.technologies.Count; ++index)
    {
      Technology technology = this.technologies[index];
      if (!this.IsTechEvolved(technology) && this.CanEvolve(technology) && (stringSet == null || stringSet.Contains(technology.id)) && this.CanMutate(technology.id))
      {
        if (this.tARandomMutations)
        {
          if (technology.gridType == etechType)
            technologyList.Add(technology);
        }
        else if ((technology.gridType != Technology.ETechType.transmission || this.transmissionRandomMutations) && (technology.gridType != Technology.ETechType.ability || this.abilityRandomMutations))
          technologyList.Add(technology);
      }
    }
    if (technologyList.Count <= 0)
      return (Technology) null;
    int index1 = ModelUtils.IntRand(0, technologyList.Count - 1);
    Technology tech = technologyList[index1];
    this.EvolveTech(tech, true);
    return tech;
  }

  public Technology DeEvolveRandomTech(string[] list)
  {
    List<Technology> technologyList = new List<Technology>();
    HashSet<string> stringSet = (HashSet<string>) null;
    if (list != null && list.Length != 0)
      stringSet = new HashSet<string>((IEnumerable<string>) list);
    for (int index = 0; index < this.technologies.Count; ++index)
    {
      Technology technology = this.technologies[index];
      if (this.IsTechEvolved(technology) && this.CanDeEvolve(technology) && (stringSet == null || stringSet.Contains(technology.id)))
        technologyList.Add(technology);
    }
    if (technologyList.Count <= 0)
      return (Technology) null;
    int index1 = ModelUtils.IntRand(0, technologyList.Count - 1);
    Technology tech = technologyList[index1];
    this.DeEvolveTech(tech, true);
    return tech;
  }

  public bool canDevolveAnyTech
  {
    get
    {
      foreach (string techID in this.techEvolved)
      {
        if (this.CanDeEvolve(this.GetTechnology(techID)))
          return true;
      }
      return false;
    }
  }

  public Technology SelectRandomTech(string[] list)
  {
    return list.Length == 0 ? (Technology) null : this.GetTechnology(list[ModelUtils.IntRand(0, list.Length - 1)]);
  }

  public virtual void EvolveTech(Technology tech, bool free) => this.ApplyEvolveTech(tech, free);

  public virtual void ApplyGenericWorldFlags(Technology tech, bool evolve)
  {
    if (!(tech.id == "Border_Monitoring"))
      return;
    if (evolve)
      this.AddFlag(Disease.EGenericDiseaseFlag.TechBorderMonitoring);
    else
      this.RemoveFlag(Disease.EGenericDiseaseFlag.TechBorderMonitoring);
  }

  public virtual void ApplyEvolveTech(Technology tech, bool free, bool skipDevolveIDs = false)
  {
    int evolveCost = this.GetEvolveCost(tech);
    this.ApplyGenericWorldFlags(tech, true);
    Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "]Disease.EvolveTech[" + tech.id + "] - free:" + free.ToString() + ", cost:" + (object) evolveCost));
    this.SpendEvoPoints(evolveCost, free);
    if ((double) tech.refundPerc != 0.0)
      this.RecordTechCostPaid(tech, free ? 0 : evolveCost);
    if (!free && !tech.skipIncreaseTypeCost)
    {
      if (tech.gridType == Technology.ETechType.transmission && this.transmissionCostIncrease)
        this.transmissionExtraCost += Mathf.RoundToInt(this.evolveCostMultiplier);
      else if (tech.gridType == Technology.ETechType.symptom && this.symptomCostIncrease)
        this.symptomExtraCost += Mathf.RoundToInt(this.evolveCostMultiplier);
      else if (tech.gridType == Technology.ETechType.ability && this.abilityCostIncrease)
        this.abilityExtraCost += Mathf.RoundToInt(this.evolveCostMultiplier);
    }
    this.globalInfectiousnessMax += tech.changeToInfectiousness;
    this.globalSeverityMax += tech.changeToSeverity;
    this.globalLethalityMax += tech.changeToLethality;
    this.airTransmission += tech.changeToAirTransmission;
    this.landTransmission += tech.changeToLandTransmission;
    this.seaTransmission += tech.changeToSeaTransmission;
    this.corpseTransmission += tech.changeToCorpseTransmission;
    this.wealthy += tech.changeToWealthy;
    this.poverty += tech.changeToPoverty;
    this.urban += tech.changeToUrban;
    this.rural += tech.changeToRural;
    this.cold += tech.changeToCold;
    this.hot += tech.changeToHot;
    this.arid += tech.changeToArid;
    this.humid += tech.changeToHumid;
    this.cureBaseMultiplier += tech.changeToCureBaseMultiplier;
    if (!CGameManager.IsCoopMPGame || this.totalInfected > 0L)
      this.researchInefficiencyMultiplier += tech.changeToResearchInefficiencyMultiplier;
    this.mutation += tech.changeToMutation;
    this.apeXSpeciesInfectiousness += tech.changeToApeXSpeciesInfectiousness;
    this.apeInfectiousness += tech.changeToApeInfectiousness;
    this.apeLethality += tech.changeToApeLethality;
    this.apeRescueAbility += tech.changeToApeRescueAbility;
    this.apeStrength += tech.changeToApeStrength;
    this.apeIntelligence += tech.changeToApeIntelligence;
    this.apeSpeed += tech.changeToApeSpeed;
    this.apeSurvival += tech.changeToApeSurvival;
    this.changeToHumanImmunity += tech.changeToHumanImmunity;
    this.sporeCounter += tech.changeToSporeCounter;
    this.zombieCombatStrength += tech.changeToZCombatStrength;
    this.zombieConversionMod += tech.changeToZombieConversionMod;
    this.reanimateZombieCombatStrengthMod += tech.changeToLocalZCombatStrengthMod;
    this.hordeSpeed *= 1f + tech.hordeSpeedMultiplier;
    this.hordeWaterSpeed += tech.changeToHordeWaterSpeed;
    this.reanimateSize *= 1f + tech.reanimateSizeMultiplier;
    if ((double) tech.changeToZday > 9.9999997473787516E-05)
      this.zday = true;
    this.hordeSize += tech.changeToHordeSize;
    this.decayPercentReduction += tech.changeToDecayPercReduction;
    this.geneCompressionCounter += tech.changeToGeneCompressionCounter;
    this.nucleicAcidFlag += tech.changeToNucleicAcidFlag;
    this.replicationOverloadFlag += tech.changeToReplicationOverloadFlag;
    this.interceptorOverloadFlag += tech.changeToInterceptorOverloadFlag;
    this.zombieTechMummy += tech.changeToTechMummy;
    this.wormPlaneChance += tech.changeToWormPlaneChance;
    this.migrationCountryDistanceMax += tech.changeToMigrationCountryDistanceMax;
    this.migrationDistanceLandMod *= 1f + tech.changeToMigrationDistanceLandMod;
    this.apeHordeSpeed *= 1f + tech.changeToApeHordeSpeed;
    this.migrationDistanceWaterMod += tech.changeToMigrationDistanceWaterMod;
    this.colonyInfectionBoost += tech.changeToColonyInfectionBoost;
    this.apeColonyBonusPoints += (int) tech.changeToApeColonyBonusPoints;
    this.apeIntelligenceFlag += tech.changeToApeIntelligenceFlag;
    if ((double) tech.changeToRecomputePathsFlag != 0.0)
      this.RecalculatePaths();
    this.recomputePathsFlag += tech.changeToRecomputePathsFlag;
    this.transcendenceFlag += (int) tech.changeToTranscendenceFlag;
    this.trojanDna += (int) tech.changeToTrojanDna;
    this.trojanInfected += (int) tech.changeToTrojanInfected;
    this.trojanInfectiousness += (int) tech.changeToTrojanInfectiousness;
    this.trojanLethality += (int) tech.changeToTrojanLethality;
    this.trojanPublicOrder += tech.changeToTrojanPublicOrder;
    if ((double) tech.changeToVday > 9.9999997473787516E-05)
      this.vday = true;
    else if ((double) tech.changeToVday < -9.9999997473787516E-05)
      this.vday = false;
    if ((double) tech.changeToShadowDay > 9.9999997473787516E-05)
      this.shadowDay = true;
    else if ((double) tech.changeToShadowDay < -9.9999997473787516E-05)
      this.shadowDay = false;
    this.vampireBonus = (int) ((double) this.vampireBonus + (double) tech.changeToVampireBonus);
    this.vampHealSacrificeMod += tech.changeToVampHealSacrificeMod;
    this.vampireInfectionBoost = (int) ((double) this.vampireInfectionBoost + (double) tech.changeToVampireInfectionBoost);
    this.castleHealingMod += tech.changeToCastleHealingMod;
    this.castleColdClimateResearchMod += tech.changeToCastleColdClimateResearchMod;
    this.vampireStealthMod = (int) ((double) this.vampireStealthMod + (double) tech.changeToVampireStealthMod);
    this.vampireConversionMod += tech.changeToVampireConversionMod;
    if ((double) tech.changeToShadowPlagueActive > 9.9999997473787516E-05)
      this.shadowPlagueActive = true;
    else if ((double) tech.changeToShadowPlagueActive < -9.9999997473787516E-05)
      this.shadowPlagueActive = false;
    this.castleWealthyResearchMod += tech.changeToCastleWealthyResearchMod;
    this.castleDnaCounter = (int) ((double) this.castleDnaCounter + (double) tech.changeToCastleDnaCounter);
    this.castleReturnSpeed += tech.changeToCastleReturnSpeed;
    this.castleHeatClimateResearchMod += tech.changeToCastleHeatClimateResearchMod;
    this.deadBodyTransmission += tech.changeToDeadBodyTransmission;
    this.themeQty += tech.themeQtyChange;
    this.componentQty += tech.componentQtyChange;
    this.mechanicsQty += tech.mechanicsQtyChange;
    this.playerQty += tech.playerQtyChange;
    this.globalMedicalCapacityMultiplier += tech.changeToGlobalMedicalCapacityMultiplier;
    this.contactTracingEffectivenessMod += tech.changeToContactTracingEffectivenessMulti;
    this.medicalCapacityEffectivenessMulti += tech.changeToMedicalCapacityEffectivenessMulti;
    this.lockdownEffectivenessMod += tech.changeToLockdownEffectivenessMulti;
    this.economyDefenseEffectivenessMulti += tech.changeToEconomyDefenseEffectivenessMulti;
    this.globalInfectModMAX *= 1f + tech.changeToGlobalInfectMod;
    this.globalLethalityMod *= 1f + tech.changeToGlobalLethalityMod;
    this.globalLocalPriorityMultiplier += tech.changeToLocalPriorityMultiplier;
    this.globalPriorityAlertModifier += tech.changeToGlobalPriorityAlertModifier;
    this.connectedLocalPriorityMultiplier += tech.changeToConnectedLocalPriorityMultiplier;
    this.alertLevel += tech.changeToAlertLevel;
    this.developmentSpeed += tech.changeToDevelopmentSpeed;
    this.understandingSpeed += tech.changeToUnderstandingSpeed;
    this.manufactureSpeed += tech.changeToManufactureSpeed;
    this.diseaseDNAComplexity += tech.changeToDnaComplexity;
    this.authorityMod += tech.changeToAuthorityMod;
    this.authLossInfMulti *= 1f + tech.changeToAuthorityInfMulti;
    this.authLossDeadMulti *= 1f + tech.changeToAuthorityDeadMulti;
    this.reproductionVisual *= (float) (1.0 + (double) tech.changeToReproductionVisual * 0.039999999105930328);
    this.mortalityVisual *= (float) (1.0 + (double) tech.changeToMortalityVisual * 0.039999999105930328);
    this.unrestVisual *= (float) (1.0 + (double) tech.changeToUnrestVisual * 0.039999999105930328);
    this.landTravelRestriction += tech.changeToLandTravelRestriction;
    this.airTravelRestriction += tech.changeToAirTravelRestriction;
    this.oceanTravelRestriction += tech.changeToOceanTravelRestriction;
    if (!string.IsNullOrEmpty(tech.applyPadlockId))
    {
      string applyPadlockId = tech.applyPadlockId;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in applyPadlockId.Split(chArray))
      {
        string padlockID = str;
        if (!string.IsNullOrEmpty(padlockID))
        {
          bool flag = true;
          if (padlockID.StartsWith("-"))
          {
            padlockID = padlockID.Substring(1);
            flag = false;
          }
          foreach (Technology technology in this.technologies.FindAll((Predicate<Technology>) (a => a.padlockId == padlockID)))
            technology.padlocked = flag;
        }
      }
    }
    if (!skipDevolveIDs && !string.IsNullOrEmpty(tech.applyDevolveIds))
    {
      string applyDevolveIds = tech.applyDevolveIds;
      char[] chArray = new char[1]{ ',' };
      foreach (string techID in applyDevolveIds.Split(chArray))
      {
        if (!string.IsNullOrEmpty(techID))
        {
          bool flag = true;
          if (techID.StartsWith("-"))
          {
            techID = techID.Substring(1);
            flag = false;
          }
          if (flag)
          {
            if (this.IsTechEvolved(techID))
            {
              Technology technology = this.GetTechnology(techID);
              if (technology == null)
                Debug.LogError((object) ("Unable to find '" + techID + "' to evolve from " + tech.id + ".applyDevolveIds"));
              else
                this.DeEvolveTech(technology, true);
            }
          }
          else if (!this.IsTechEvolved(techID))
          {
            if (this.GetTechnology(techID) == null)
              Debug.LogError((object) ("Unable to find '" + techID + "' to evolve from " + tech.id + ".applyDevolveIds"));
            else
              this.ApplyEvolveTech(this.GetTechnology(techID), true, true);
          }
        }
      }
    }
    if (this.isCure)
    {
      if ((double) tech.setAuthCensorshipTimer > 0.0)
        this.authCensorshipTimer = tech.setAuthCensorshipTimer;
      if ((double) tech.refundPerc > 0.0 && this.difficulty > 0)
        this.globalVaccineSpeed *= 0.98f;
      if (tech.diseaseTraitId == "eco")
        this.economyTimeMulti += (float) ((1.0 - (double) this.economyTimeMulti) * 0.5);
    }
    if (!string.IsNullOrEmpty(tech.unlockAa))
      this.SetAbilityActive(tech.unlockAa);
    this.techEvolved.Add(tech.id.ToLowerInvariant());
    this.history.Add((History) new TechHistory(tech.id.ToLowerInvariant(), true, this.turnNumber, this.history.Count, this));
  }

  public virtual void DeEvolveTech(Technology tech, bool free = false)
  {
    this.ApplyDeEvolveTech(tech, free);
  }

  public virtual void ApplyDeEvolveTech(Technology tech, bool free = false)
  {
    int deEvolveCost = this.GetDeEvolveCost(tech);
    if (!free)
    {
      this.SpendEvoPoints(deEvolveCost);
      if (tech.gridType == Technology.ETechType.transmission)
        this.transmissionDevolveCost += this.transmissionDevolveCostIncrease;
      else if (tech.gridType == Technology.ETechType.symptom)
        this.symptomDevolveCost += this.symptomDevolveCostIncrease;
      else if (tech.gridType == Technology.ETechType.ability)
        this.abilityDevolveCost += this.abilityDevolveCostIncrease;
    }
    this.globalInfectiousnessMax -= tech.changeToInfectiousness;
    this.globalSeverityMax -= tech.changeToSeverity;
    this.globalLethalityMax -= tech.changeToLethality;
    this.airTransmission -= tech.changeToAirTransmission;
    this.landTransmission -= tech.changeToLandTransmission;
    this.seaTransmission -= tech.changeToSeaTransmission;
    this.corpseTransmission -= tech.changeToCorpseTransmission;
    this.wealthy -= tech.changeToWealthy;
    this.poverty -= tech.changeToPoverty;
    this.urban -= tech.changeToUrban;
    this.rural -= tech.changeToRural;
    this.cold -= tech.changeToCold;
    this.hot -= tech.changeToHot;
    this.arid -= tech.changeToArid;
    this.humid -= tech.changeToHumid;
    this.cureBaseMultiplier -= tech.changeToCureBaseMultiplier;
    if (!CGameManager.IsCoopMPGame || this.totalInfected > 0L)
      this.researchInefficiencyMultiplier -= tech.changeToResearchInefficiencyMultiplier;
    this.mutation -= tech.changeToMutation;
    this.apeXSpeciesInfectiousness -= tech.changeToApeXSpeciesInfectiousness;
    this.apeInfectiousness -= tech.changeToApeInfectiousness;
    this.apeLethality -= tech.changeToApeLethality;
    this.apeRescueAbility -= tech.changeToApeRescueAbility;
    this.apeStrength -= tech.changeToApeStrength;
    this.apeIntelligence -= tech.changeToApeIntelligence;
    this.apeSpeed -= tech.changeToApeSpeed;
    this.apeSurvival -= tech.changeToApeSurvival;
    this.changeToHumanImmunity -= tech.changeToHumanImmunity;
    this.sporeCounter -= tech.changeToSporeCounter;
    this.zombieCombatStrength -= tech.changeToZCombatStrength;
    this.zombieConversionMod -= tech.changeToZombieConversionMod;
    this.reanimateZombieCombatStrengthMod -= tech.changeToLocalZCombatStrengthMod;
    this.hordeSpeed /= 1f + tech.hordeSpeedMultiplier;
    this.hordeWaterSpeed -= tech.changeToHordeWaterSpeed;
    this.reanimateSize /= 1f + tech.reanimateSizeMultiplier;
    if ((double) tech.changeToZday > 9.9999997473787516E-05)
      this.zday = false;
    this.hordeSize -= tech.changeToHordeSize;
    this.decayPercentReduction -= tech.changeToDecayPercReduction;
    this.geneCompressionCounter -= tech.changeToGeneCompressionCounter;
    this.nucleicAcidFlag -= tech.changeToNucleicAcidFlag;
    this.replicationOverloadFlag -= tech.changeToReplicationOverloadFlag;
    this.interceptorOverloadFlag -= tech.changeToInterceptorOverloadFlag;
    this.zombieTechMummy -= tech.changeToTechMummy;
    this.wormPlaneChance -= tech.changeToWormPlaneChance;
    this.migrationCountryDistanceMax -= tech.changeToMigrationCountryDistanceMax;
    this.migrationDistanceLandMod /= 1f + tech.changeToMigrationDistanceLandMod;
    this.apeHordeSpeed /= 1f + tech.changeToApeHordeSpeed;
    this.migrationDistanceWaterMod -= tech.changeToMigrationDistanceWaterMod;
    this.colonyInfectionBoost -= tech.changeToColonyInfectionBoost;
    this.apeColonyBonusPoints -= (int) tech.changeToApeColonyBonusPoints;
    this.apeIntelligenceFlag -= tech.changeToApeIntelligenceFlag;
    this.recomputePathsFlag -= tech.changeToRecomputePathsFlag;
    if ((double) tech.changeToRecomputePathsFlag != 0.0)
      this.RecalculatePaths();
    this.transcendenceFlag -= (int) tech.changeToTranscendenceFlag;
    this.trojanDna -= (int) tech.changeToTrojanDna;
    this.trojanInfected -= (int) tech.changeToTrojanInfected;
    this.trojanInfectiousness -= (int) tech.changeToTrojanInfectiousness;
    this.trojanLethality -= (int) tech.changeToTrojanLethality;
    this.trojanPublicOrder -= tech.changeToTrojanPublicOrder;
    if ((double) tech.changeToVday > 9.9999997473787516E-05)
      this.vday = false;
    else if ((double) tech.changeToVday < -9.9999997473787516E-05)
      this.vday = true;
    if ((double) tech.changeToShadowDay > 9.9999997473787516E-05)
      this.shadowDay = false;
    else if ((double) tech.changeToShadowDay < -9.9999997473787516E-05)
      this.shadowDay = true;
    this.vampireBonus = (int) ((double) this.vampireBonus - (double) tech.changeToVampireBonus);
    this.vampHealSacrificeMod -= tech.changeToVampHealSacrificeMod;
    this.vampireInfectionBoost = (int) ((double) this.vampireInfectionBoost - (double) tech.changeToVampireInfectionBoost);
    this.castleHealingMod -= tech.changeToCastleHealingMod;
    this.castleColdClimateResearchMod -= tech.changeToCastleColdClimateResearchMod;
    this.vampireStealthMod = (int) ((double) this.vampireStealthMod - (double) tech.changeToVampireStealthMod);
    this.vampireConversionMod -= tech.changeToVampireConversionMod;
    if ((double) tech.changeToShadowPlagueActive > 9.9999997473787516E-05)
      this.shadowPlagueActive = false;
    else if ((double) tech.changeToShadowPlagueActive < -9.9999997473787516E-05)
      this.shadowPlagueActive = true;
    this.castleWealthyResearchMod -= tech.changeToCastleWealthyResearchMod;
    this.castleDnaCounter = (int) ((double) this.castleDnaCounter - (double) tech.changeToCastleDnaCounter);
    this.castleReturnSpeed -= tech.changeToCastleReturnSpeed;
    this.castleHeatClimateResearchMod -= tech.changeToCastleHeatClimateResearchMod;
    this.deadBodyTransmission -= tech.changeToDeadBodyTransmission;
    this.themeQty -= tech.themeQtyChange;
    this.componentQty -= tech.componentQtyChange;
    this.mechanicsQty -= tech.mechanicsQtyChange;
    this.playerQty -= tech.playerQtyChange;
    this.globalMedicalCapacityMultiplier -= tech.changeToGlobalMedicalCapacityMultiplier;
    this.contactTracingEffectivenessMod -= tech.changeToContactTracingEffectivenessMulti;
    this.medicalCapacityEffectivenessMulti -= tech.changeToMedicalCapacityEffectivenessMulti;
    this.lockdownEffectivenessMod -= tech.changeToLockdownEffectivenessMulti;
    this.economyDefenseEffectivenessMulti -= tech.changeToEconomyDefenseEffectivenessMulti;
    this.globalInfectModMAX /= 1f + tech.changeToGlobalInfectMod;
    this.globalLethalityMod /= 1f + tech.changeToGlobalLethalityMod;
    this.globalLocalPriorityMultiplier -= tech.changeToLocalPriorityMultiplier;
    this.globalPriorityAlertModifier -= tech.changeToGlobalPriorityAlertModifier;
    this.connectedLocalPriorityMultiplier -= tech.changeToConnectedLocalPriorityMultiplier;
    this.alertLevel -= tech.changeToAlertLevel;
    this.developmentSpeed -= tech.changeToDevelopmentSpeed;
    this.understandingSpeed -= tech.changeToUnderstandingSpeed;
    this.manufactureSpeed -= tech.changeToManufactureSpeed;
    this.diseaseDNAComplexity -= tech.changeToDnaComplexity;
    this.authorityMod -= tech.changeToAuthorityMod;
    this.authLossInfMulti /= 1f + tech.changeToAuthorityInfMulti;
    this.authLossDeadMulti /= 1f + tech.changeToAuthorityDeadMulti;
    this.reproductionVisual /= (float) (1.0 + (double) tech.changeToReproductionVisual * 0.039999999105930328);
    this.mortalityVisual /= (float) (1.0 + (double) tech.changeToMortalityVisual * 0.039999999105930328);
    this.unrestVisual /= (float) (1.0 + (double) tech.changeToUnrestVisual * 0.039999999105930328);
    this.landTravelRestriction -= tech.changeToLandTravelRestriction;
    this.airTravelRestriction -= tech.changeToAirTravelRestriction;
    this.oceanTravelRestriction -= tech.changeToOceanTravelRestriction;
    if (this.isCure && (double) tech.refundPerc > 0.0)
      this.globalVaccineSpeed /= 0.98f;
    if (this.isCure && !free && !tech.skipIncreaseTypeCost)
    {
      if (tech.gridType == Technology.ETechType.symptom && this.symptomCostIncrease)
        --this.symptomExtraCost;
      else if (tech.gridType == Technology.ETechType.ability && this.abilityCostIncrease)
        --this.abilityExtraCost;
      else if (tech.gridType == Technology.ETechType.transmission && this.transmissionCostIncrease)
        --this.transmissionExtraCost;
    }
    this.techEvolved.Remove(tech.id.ToLowerInvariant());
    this.history.Add((History) new TechHistory(tech.id.ToLowerInvariant(), false, this.turnNumber, this.history.Count, this));
  }

  public float[] GetInfSevLethChange(Technology tech, bool withNegative)
  {
    float num1 = -1f;
    if (!withNegative)
      num1 = 0.0f;
    float num2 = tech.changeToInfectiousness;
    float num3 = tech.changeToSeverity;
    float num4 = tech.changeToLethality;
    if (this.isCure)
    {
      float reproductionVisual = this.reproductionVisual;
      float globalInfectModMax = this.globalInfectModMAX;
      float num5 = this.globalInfectiousness * (globalInfectModMax * reproductionVisual) * this.reproductionBarScale;
      float num6;
      float num7;
      if (withNegative)
      {
        num6 = reproductionVisual / (float) (1.0 + (double) tech.changeToReproductionVisual * 0.039999999105930328);
        num7 = globalInfectModMax / (1f + tech.changeToGlobalInfectMod);
      }
      else
      {
        num6 = reproductionVisual * (float) (1.0 + (double) tech.changeToReproductionVisual * 0.039999999105930328);
        num7 = globalInfectModMax * (1f + tech.changeToGlobalInfectMod);
      }
      float num8 = this.globalInfectiousness * (num7 * num6) * this.reproductionBarScale;
      num2 = !withNegative ? num8 - num5 : num5 - num8;
      float mortalityVisual = this.mortalityVisual;
      float globalLethalityMod = this.globalLethalityMod;
      float num9 = this.globalLethality * (globalLethalityMod * mortalityVisual) * this.mortalityBarScale;
      float num10;
      float num11;
      if (withNegative)
      {
        num10 = mortalityVisual / (float) (1.0 + (double) tech.changeToMortalityVisual * 0.039999999105930328);
        num11 = globalLethalityMod / (1f + tech.changeToGlobalLethalityMod);
      }
      else
      {
        num10 = mortalityVisual * (float) (1.0 + (double) tech.changeToMortalityVisual * 0.039999999105930328);
        num11 = globalLethalityMod * (1f + tech.changeToGlobalLethalityMod);
      }
      float num12 = this.globalLethality * (num11 * num10) * this.mortalityBarScale;
      num3 = !withNegative ? num12 - num9 : num9 - num12;
      float unrestVisual1 = this.unrestVisual;
      float unrestVisual2 = this.unrestVisual;
      if (withNegative)
      {
        float num13 = unrestVisual2 / (float) (1.0 + (double) tech.changeToUnrestVisual * 0.039999999105930328);
        num4 = unrestVisual1 - num13;
      }
      else
        num4 = unrestVisual2 * (float) (1.0 + (double) tech.changeToUnrestVisual * 0.039999999105930328) - unrestVisual1;
    }
    bool flag = this.IsTechEvolved(tech.id);
    return new float[3]
    {
      (float) ((flag ? (double) num1 : 1.0) * ((double) num2 / 100.0)),
      (float) ((flag ? (double) num1 : 1.0) * ((double) num3 / 100.0)),
      (float) ((flag ? (double) num1 : 1.0) * ((double) num4 / 100.0))
    };
  }

  public float[] GetInfSevLethTotal()
  {
    float num1 = this.globalInfectiousness;
    float num2 = this.globalSeverity;
    float num3 = this.globalLethality;
    if (this.isCure)
    {
      num1 = this.globalInfectiousness * (this.globalInfectModMAX * this.reproductionVisual) * this.reproductionBarScale;
      num2 = this.globalLethality * (this.globalLethalityMod * this.mortalityVisual) * this.mortalityBarScale;
      num3 = this.unrestVisual;
    }
    return new float[3]
    {
      num1 / 100f,
      num2 / 100f,
      num3 / 100f
    };
  }

  public virtual void DiscountRandomTech(int amount, bool allowPositive)
  {
    Technology technology = this.technologies[ModelUtils.IntRand(0, this.technologies.Count - 1)];
    int techCostMod = this.GetTechCostMod(technology);
    int num = technology.cost + techCostMod;
    if (((num + amount > 0 || allowPositive ? (num <= 0 || num + amount < 0 ? 0 : (!allowPositive ? 1 : 0)) : 1) | (allowPositive ? 1 : 0)) != 0)
      this.SetTechCostMod(technology, techCostMod + amount);
    else
      this.SetTechCostMod(technology, technology.cost * -1);
  }

  public float ConvertMonthsToResearchRequirement(float months)
  {
    return 338960f * (float) (int) ((double) months * 30.0);
  }

  public void SpendEvoPoints(int cost, bool isFree = false)
  {
    if (!isFree)
      this.evoPoints -= cost;
    this.evoPointsSpent += cost;
  }

  public LocalDisease GetLocalDisease(Country c)
  {
    if (c == null)
      return (LocalDisease) null;
    return this.countryData.ContainsKey(c) ? this.countryData[c] : this.CreateLocalDisease(c);
  }

  public Country GetInfectedCountry(Country exclude)
  {
    if (this.infectedCountries.Count > 1)
    {
      int num = ModelUtils.IntRand(0, this.infectedCountries.Count - 1);
      for (int index = 0; index < this.infectedCountries.Count; ++index)
      {
        if (this.infectedCountries[index] == exclude)
          ++num;
        else if (index == num)
          return this.infectedCountries[index];
      }
    }
    return (Country) null;
  }

  public void FindSuperCureCountry()
  {
    List<Country> countryList = new List<Country>();
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      if (country.airport && country.diseaseNexus == null && country.diseaseSuperCure == null)
        countryList.Add(country);
    }
    if (countryList.Count < 1)
      this.SetSuperCureCountry(World.instance.countries[UnityEngine.Random.Range(0, World.instance.countries.Count)]);
    else
      this.SetSuperCureCountry(countryList[UnityEngine.Random.Range(0, countryList.Count)]);
  }

  public void SetSuperCureCountry(Country country)
  {
    this.superCureCountry = country;
    this.superCureCountry.diseaseSuperCure = this;
  }

  public virtual void SetNexus(Country country)
  {
    this.nexus = country;
    country.diseaseNexus = this;
    if (this.diseaseType != Disease.EDiseaseType.VAMPIRE && this.diseaseType != Disease.EDiseaseType.CURE)
    {
      if (country.hot)
      {
        this.hot += 0.5f;
        this.cold -= 0.5f;
      }
      else if (country.cold)
      {
        this.hot -= 0.3f;
        this.cold += 0.5f;
      }
      else
        this.globalInfectiousnessMax += 2f;
    }
    this.evoPoints += country.startCountryEvoBonus;
    if (this.diseaseType != Disease.EDiseaseType.CURE)
      return;
    this.preSimulate = 10 + this.nexus.presimSteps;
    LocalDisease localDisease = this.GetLocalDisease(this.nexus);
    localDisease.infectionMethod = Country.EInfectionMethod.IM_NEXUS;
    localDisease.infectedFromCountry = this.nexus;
    if (this.cureScenario == Disease.ECureScenario.Cure_Fungus)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 0.3f);
    if (this.difficulty == 0)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 0.4f);
    else if (this.difficulty == 1)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 0.8f);
    else if (this.difficulty == 2)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 1.2f);
    else if (this.difficulty == 3)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 1.4f);
    foreach (Country neighbour in this.nexus.neighbours)
    {
      if (neighbour.continentType != this.nexus.continentType)
      {
        this.isNexusContinentBorder = true;
        break;
      }
    }
    if (this.isNexusContinentBorder)
      this.preSimulate -= 3;
    this.preSimulate = Mathf.Max(0, this.preSimulate);
    this.baseLethality = this.globalLethalityMax;
    this.baseInfectivity = this.globalInfectiousnessMax;
    if (this.isNanovirus)
      this.preSimulate = Mathf.RoundToInt((float) this.preSimulate * 0.5f);
    this.preSimMAX = this.preSimulate;
  }

  public void ApplySimulatorValues(DiseaseSimulator diseaseSim)
  {
    this.globalInfectiousnessMax = diseaseSim.StatValues[0][1];
    this.globalLethalityMax = diseaseSim.StatValues[1][1];
    this.airTransmission = diseaseSim.StatValues[3][1];
    this.seaTransmission = diseaseSim.StatValues[5][1];
    this.landTransmission = diseaseSim.StatValues[4][1];
    this.diseaseLengthMulti = (int) diseaseSim.StatValues[2][1];
    this.hot = diseaseSim.StatValues[6][1];
    this.humid = diseaseSim.StatValues[7][1];
    this.arid = diseaseSim.StatValues[8][1];
    this.cold = diseaseSim.StatValues[9][1];
    this.wealthy = diseaseSim.StatValues[10][1];
    this.poverty = diseaseSim.StatValues[11][1];
    this.urban = diseaseSim.StatValues[12][1];
    this.rural = diseaseSim.StatValues[13][1];
    this.contactTracingEffectiveness = diseaseSim.StatValues[14][1];
    this.lockdownEffectiveness = diseaseSim.StatValues[15][1];
  }

  public void ApplyGenes(Gene[] newGenes)
  {
    foreach (Gene newGene in newGenes)
      this.ApplyGene(newGene);
  }

  public virtual void ApplyGene(Gene gene)
  {
    if (gene.id == "airport_controller")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneAirportController);
    if (gene.id == "port_controller")
      this.AddFlag(Disease.EGenericDiseaseFlag.GenePortController);
    if (gene.id == "land_border_controller")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneLandBorderController);
    if (gene.id == "molecular_biologist")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneMolecularBiologist);
    if (gene.id == "forensic_epidemiologist")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneForensicEpidemiologist);
    if (gene.id == "checkpoint_enforcer")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneCheckpointEnforcer);
    if (gene.id == "quarantine_coordinator")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneQuarantineCoordinator);
    if (gene.id == "local_outbreak_analyst")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneLocalOutbreakAnalyst);
    if (gene.id == "national_outbreak_analyst")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneNationalOutbreakAnalyst);
    if (gene.id == "celebrity_scientist")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneCelebrityScientist);
    if (gene.id == "construction_manager")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneConstructionManager);
    if (gene.id == "empathy_trainer")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneEmpathyTrainer);
    if (gene.id == "regulation_enforcer")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneRegulationEnforcer);
    if (gene.id == "technical_officer")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneTechnicalOfficer);
    if (gene.id == "strategic_fundraiser")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneStrategicFundraiser);
    if (gene.id == "fast_response_emts")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneFastResponseEMTs);
    if (gene.id == "situation_director")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneSituationDirector);
    if (gene.id == "economic_forecaster")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneEconomicForecaster);
    if (gene.id == "outreach_coordinator")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneOutreachCoordinator);
    if (gene.id == "crisis_manager")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneCrisisManager);
    if (gene.id == "ethics_watchdog")
    {
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneEthicsWatchdog);
      this.GetTechnology("Exploit_Misinformation").eventLocked = true;
      this.GetTechnology("Censorship").eventLocked = true;
      this.startingAuthority += 0.15f;
    }
    if (gene.id == "chaos_engineer")
    {
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneChaosEngineer);
      this.DiscountRandomTech(5, false);
    }
    if (gene.id == "medical_coordinator")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneMedicalCoordinator);
    if (gene.id == "disaster_manager")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneDisasterManager);
    if (gene.id == "procurement_director")
      this.AddFlag(Disease.EGenericDiseaseFlag.GeneProcurementDirector);
    this.arid += gene.bonusArid;
    this.humid += gene.bonusHumid;
    this.rural += gene.bonusRural;
    this.urban += gene.bonusUrban;
    this.landTransmission += gene.bonusLandTransfer;
    this.airTransmission += gene.bonusAirTransfer;
    this.seaTransmission += gene.bonusSeaTransfer;
    this.evoPoints += gene.bonusDna;
    this.orangeBubbleMult += gene.orangeBubbleMult;
    this.redBubbleMult += gene.redBubbleMult;
    if (gene.bubbleAutopress)
      this.bubbleAutopress = true;
    this.mutation += gene.mutationChance;
    this.symptomDevolveCost -= gene.devolveDnaAdd;
    this.abilityDevolveCost -= gene.devolveDnaAdd;
    this.transmissionDevolveCost -= gene.devolveDnaAdd;
    if (gene.closedBordersSpreadEnhance)
      this.closedBordersSpreadEnhance = true;
    if (gene.nexusBonus)
      this.nexusBonus = true;
    if (gene.blueBubbleBonusDna)
      this.blueBubbleBonusDNA = gene.blueBubbleBonusDna;
    if (gene.sCostIncrease)
      this.symptomCostIncrease = false;
    if (gene.tCostIncrease)
      this.transmissionCostIncrease = false;
    if (gene.aCostIncrease)
      this.abilityCostIncrease = false;
    if (gene.tRandomMutations)
      this.transmissionRandomMutations = true;
    if (gene.aRandomMutations)
      this.abilityRandomMutations = true;
    this.cureBaseMultiplier += gene.changeToCureBaseMultiplier;
    this.zdayDead += gene.zdayDeadModifier;
    this.globalDecayChance += (float) gene.globalDecayChanceModifier;
    if (gene.hotDecayFlagModifier)
      this.hotDecayFlag = true;
    this.zombieConversionMod += gene.zombieConversionModifier;
    this.aaCostModifier += gene.aaCostModifier;
    if (gene.devolveStopIncrease)
      this.abilityDevolveCostIncrease = this.transmissionDevolveCostIncrease = this.symptomDevolveCostIncrease = 0;
    this.apeSlowDeathFlag += gene.apeSlowDeathFlag;
    this.apeInfectiousnessBonusFlag += gene.apeInfectiousnessBonusFlag;
    this.apeColonyBonusPoints += gene.apeColonyBonusPoints;
    this.apeSurvival += gene.apeSurvival;
    this.labDestroyDnaFlag += gene.labDestroyDnaFlag;
    this.reducedLabResearchFlag += gene.reducedLabResearchFlag;
    this.hot += gene.bonusHot;
    this.cold += gene.bonusCold;
    this.wealthy += gene.bonusWealthy;
    this.poverty += gene.bonusPoverty;
    this.randomNexusFlag += gene.randomNexusFlag;
    this.vampHealthIncrease += gene.vampHealthIncrease;
    this.vampLabFortDnaBonus += gene.vampLabFortDnaBonus;
    this.vampRageCostZero += gene.vampRageCostZero;
    this.vampBloodRageCasulatiesIncreased += gene.vampBloodRageCasulatiesIncreased;
    this.vampBloodRageBonusDna += gene.vampBloodRageBonusDna;
    this.vampBatRangeBonus += gene.vampBatRangeBonus;
    this.vampFlightCostsZero += gene.vampFlightCostsZero;
    this.vampMoreHealthFasterFlight += gene.vampMoreHealthFasterFlight;
    this.vampFlyFasterLoseHealth += gene.vampFlyFasterLoseHealth;
    this.vampAutomaticBloodRage += gene.vampAutomaticBloodRage;
    this.vampHealingBonus += gene.vampHealingBonus;
    this.vampLairDefenseBonus += gene.vampLairDefenseBonus;
    this.vampActivityLairCountryBonus += gene.vampActivityLairCountryBonus;
    this.vampLairDnaQuicker += gene.vampLairDnaQuicker;
    this.genes.Add(gene);
  }

  public Country GetSuitableApeLabCountry()
  {
    List<Country> countryList = new List<Country>();
    List<float> floatList = new List<float>();
    float max = 0.0f;
    bool flag = false;
    List<Vehicle> vehicleList = new List<Vehicle>((IEnumerable<Vehicle>) World.instance.GetVehicles());
    foreach (Country country1 in World.instance.countries)
    {
      Country country = country1;
      LocalDisease localDisease = this.GetLocalDisease(country);
      if (country.hasApeLab || country.apeNumDestroyedLabs > 0)
        flag = true;
      if ((double) localDisease.apeTotalAlivePopulation > 0.0 && !country.hasApeLab && vehicleList.Find((Predicate<Vehicle>) (a => a.destination == country && a.subType == Vehicle.EVehicleSubType.ApeLabPlane)) == null)
      {
        float num1 = 1f;
        if ((double) localDisease.apeTotalAlivePopulation > 150.0)
          num1 *= Mathf.Lerp(1f, 4f, Mathf.Clamp((float) (((double) localDisease.apeTotalAlivePopulation - 100.0) / 10000.0), 0.0f, 1f));
        float num2 = num1 * Mathf.Lerp(1f, 4f, Mathf.Clamp(localDisease.apeInfectedPercent, 0.0f, 1f));
        if (country.wealthy)
          num2 *= 2f;
        if (country.poverty)
          num2 *= 0.5f;
        if (country.apeNumDestroyedLabs < 1)
          num2 *= 4f;
        if (this.difficulty < 2 && (double) localDisease.apeInfectedPercent > 0.0099999997764825821)
          num2 *= 5f;
        if (this.difficulty < 1 && country.apeNumDestroyedLabs > 0)
          num2 = 0.0f;
        countryList.Add(country);
        floatList.Add(num2);
        max += num2;
      }
    }
    if (countryList.Count == 0)
      return (Country) null;
    if (!flag && this.nexus != null && !this.nexus.hasApeLab && this.nexus.apeNumDestroyedLabs == 0)
      return this.nexus;
    float num = ModelUtils.FloatRand(0.0f, max);
    int index = -1;
    for (; (double) num > 0.0; num -= floatList[index])
      ++index;
    if (index < 0)
      index = 0;
    return countryList[index];
  }

  public Country GetSuitableApeColonyCountry()
  {
    if (this.nexus != null && !this.nexus.hasApeColony && !this.nexus.apeHordeMoving && this.nexus.apeNumDestroyedColonies == 0 && this.GetLocalDisease(this.nexus).apeInfectedPopulation > 6L)
      return this.nexus;
    List<Country> countryList = new List<Country>();
    List<float> floatList = new List<float>();
    float max = 0.0f;
    foreach (Country country in World.instance.countries)
    {
      LocalDisease localDisease = this.GetLocalDisease(country);
      if (localDisease.apeInfectedPopulation > 10L && !country.hasApeColony)
      {
        Vehicle hordeFromCountry = World.instance.GetHordeFromCountry(country);
        if (hordeFromCountry == null || hordeFromCountry.subType != Vehicle.EVehicleSubType.ApeHorde && hordeFromCountry.subType != Vehicle.EVehicleSubType.ApeHordeNoColony)
        {
          Vehicle hordeToCountry = World.instance.GetHordeToCountry(country);
          if (hordeToCountry == null || hordeToCountry.subType != Vehicle.EVehicleSubType.ApeHorde)
          {
            float num = 1f;
            if ((double) localDisease.apeTotalAlivePopulation > 100.0)
              num *= Mathf.Lerp(2f, 4f, Mathf.Clamp((float) (((double) localDisease.apeTotalAlivePopulation - 100.0) / 10000.0), 0.0f, 1f));
            if (country.urban)
              num *= 0.75f;
            countryList.Add(country);
            floatList.Add(num);
            max += num;
          }
        }
      }
    }
    if (countryList.Count == 0)
      return (Country) null;
    float num1 = ModelUtils.FloatRand(0.0f, max);
    int index = -1;
    for (; (double) num1 > 0.0; num1 -= floatList[index])
      ++index;
    if (index < 0)
      index = 0;
    return countryList[index];
  }

  public Country GetSuitableVampireResearchLabCountry()
  {
    List<Country> countryList = new List<Country>();
    List<float> floatList = new List<float>();
    float max = 0.0f;
    List<Vehicle> vehicleList = new List<Vehicle>((IEnumerable<Vehicle>) World.instance.GetVehicles());
    foreach (Country country in World.instance.countries)
    {
      LocalDisease localDisease = this.GetLocalDisease(country);
      if (!country.hasApeLab && country.apeLabStatus != EApeLabState.APE_LAB_DESTROYED && country.fortState == EFortState.FORT_NONE && (this.shadowDayDone ? (double) country.healthyPercent : (double) country.healthyPercent + (double) localDisease.infectedPercent) > 9.9999997473787516E-06)
      {
        float num = 1f * Mathf.Max(country.healthyPercent * 2f, 0.01f) * (1f - country.deadPercent);
        if (this.shadowDay || this.shadowDayDone)
          num *= 1f - country.infectedPercent + country.deadPercent;
        if (country.diseaseNexus != null)
          num *= 0.3f;
        if (country.wealthy)
          num *= 2f;
        if (country.poverty)
          num *= 0.5f;
        if (localDisease.HasCastle)
          num *= 0.7f;
        if (country.apeNumDestroyedLabs < 1)
          num *= 4f;
        if (this.difficulty < 1 && (double) country.infectedPercent > 0.0099999997764825821)
          num *= 2f;
        if (this.difficulty > 1 && localDisease.HasCastle)
          num *= 0.1f;
        if (this.difficulty > 1 && (double) country.infectedPercent > 0.0)
          num *= 0.5f;
        if (this.difficulty < 1 && country.apeNumDestroyedLabs > 0)
          num = 0.0f;
        if (localDisease.zombiePopulation > 0L)
          num *= 0.5f;
        countryList.Add(country);
        floatList.Add(num);
        max += num;
      }
    }
    if (countryList.Count == 0)
      return (Country) null;
    float num1 = ModelUtils.FloatRand(0.0f, max);
    int index = -1;
    for (; (double) num1 > 0.0; num1 -= floatList[index])
      ++index;
    if (index < 0)
      index = 0;
    return countryList[index];
  }

  public void OnBonusIconHidden(BonusIcon bonusIcon)
  {
    if (bonusIcon.musicBubble)
      ((SPDisease) this).OnMusicBubbleClick(0.0f, bonusIcon.musicImportance, false);
    if (bonusIcon.type == BonusIcon.EBonusIconType.CURE)
    {
      LocalDisease localDisease = this.GetLocalDisease(bonusIcon.country);
      if (this.isCure)
        localDisease.labCureBubbleVisible = false;
      else
        localDisease.cureResearchAllocation += 0.5f;
    }
    else if (bonusIcon.type == BonusIcon.EBonusIconType.DNA)
      this.dnaBubbleShowing = false;
    else if (bonusIcon.type == BonusIcon.EBonusIconType.NEURAX)
      ++this.wormBubbleHiddenStatus;
    else if (bonusIcon.type == BonusIcon.EBonusIconType.APE_COLONY)
      this.GetLocalDisease(bonusIcon.country).hasApeColonyBubble = false;
    else if (bonusIcon.type == BonusIcon.EBonusIconType.CASTLE)
      --this.numCastleBubblesWithoutTouch;
    BonusIcon.EBonusIconType type = bonusIcon.type;
    switch (type)
    {
      case BonusIcon.EBonusIconType.DNA:
      case BonusIcon.EBonusIconType.DEATH:
        this.GetLocalDisease(bonusIcon.country).mColoredBubble = Country.EGenericCountryFlag.None;
        break;
      default:
        if (type - 15 > BonusIcon.EBonusIconType.INFECT)
          break;
        goto case BonusIcon.EBonusIconType.DNA;
    }
  }

  public bool CanSteal(BonusIcon bonusIcon) => false;

  public void StealBonusIcon(BonusIcon bonusIcon)
  {
    if (bonusIcon.type == BonusIcon.EBonusIconType.CURE)
    {
      bonusIcon.disease.GetLocalDisease(bonusIcon.country).cureResearchAllocation += 0.3f;
    }
    else
    {
      if (bonusIcon.type != BonusIcon.EBonusIconType.DNA)
        return;
      bonusIcon.disease.dnaBubbleShowing = false;
      bool dnaBubbleShowing = this.dnaBubbleShowing;
      this.OnBonusIconClicked(bonusIcon);
      this.dnaBubbleShowing = dnaBubbleShowing;
    }
  }

  private void CalculateTotals()
  {
    if (this.totalCalculatedTurn == this.turnNumber)
      return;
    this.totalCalculatedTurn = this.turnNumber;
    this._totalZombieCurrent = this._totalInfectedCurrent = this._totalKilledCurrent = this._totalControlledInfectedCurrent = this._totalUncontrolledInfectedCurrent = this._totalInfectedApeCurrent = this._totalDeadApeCurrent = 0L;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      this._totalZombieCurrent += this.localDiseases[index].zombie;
      this._totalInfectedCurrent += this.localDiseases[index].allInfected;
      this._totalKilledCurrent += this.localDiseases[index].killedPopulation;
      this._totalControlledInfectedCurrent += this.localDiseases[index].controlledInfected;
      this._totalUncontrolledInfectedCurrent += this.localDiseases[index].uncontrolledInfected;
      this._totalHealthyRecovered += this.localDiseases[index].country.healthyRecoveredPopulation;
      if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        this._totalDeadApeCurrent += this.localDiseases[index].apeDeadPopulation;
        this._totalInfectedApeCurrent += this.localDiseases[index].apeInfectedPopulation;
      }
    }
    this._totalZombieCurrent += this.vampireHordeStash;
    this._totalInfectedApeCurrent += this.apeHordeStash;
  }

  public long GetCurrentTotalZombie()
  {
    this.CalculateTotals();
    return this._totalZombieCurrent;
  }

  public long GetCurrentTotalInfected()
  {
    this.CalculateTotals();
    return this._totalInfectedCurrent;
  }

  public long GetCurrentTotalRecovered()
  {
    this.CalculateTotals();
    return this._totalHealthyRecovered;
  }

  public long GetCurrentTotalControlledInfected()
  {
    this.CalculateTotals();
    return this._totalControlledInfectedCurrent;
  }

  public long GetCurrentTotalUncontrolledInfected()
  {
    this.CalculateTotals();
    return this._totalUncontrolledInfectedCurrent;
  }

  public long GetCurrentTotalKilled()
  {
    this.CalculateTotals();
    return this._totalKilledCurrent;
  }

  public long GetCurrentTotalDeadApes()
  {
    this.CalculateTotals();
    return this._totalDeadApeCurrent;
  }

  public long GetCurrentTotalInfectedApes()
  {
    this.CalculateTotals();
    return this._totalInfectedApeCurrent;
  }

  public List<Vampire> GetVampires(Country c)
  {
    List<Vampire> vampires = new List<Vampire>();
    for (int index = 0; index < this.vampires.Count; ++index)
    {
      if (this.vampires[index].currentCountry == c)
        vampires.Add(this.vampires[index]);
    }
    return vampires;
  }

  public Vampire GetVampire(int id) => this.vampires.Find((Predicate<Vampire>) (a => a.id == id));

  public Vampire GetClosestVampire(Country c, Vector3 hit)
  {
    List<Vampire> vampires = this.GetVampires(c);
    Vampire closestVampire = (Vampire) null;
    if (vampires.Count > 0)
    {
      float num1 = -1f;
      for (int index = 0; index < vampires.Count; ++index)
      {
        if (vampires[index].currentPosition.HasValue)
        {
          float num2 = Vector3.Distance(vampires[index].currentPosition.Value, hit);
          if ((double) num1 < 0.0 || (double) num2 < (double) num1)
          {
            num1 = num2;
            closestVampire = vampires[index];
          }
        }
      }
    }
    return closestVampire;
  }

  public float GetMaxVampireFlightDistance()
  {
    return Mathf.Max(4.05633831f, (float) ((double) this.hordeWaterSpeed * 0.070422537624835968 * 0.60000002384185791));
  }

  public bool IsContinentAlert(Country.EContinentType type)
  {
    switch (type)
    {
      case Country.EContinentType.NORTH_AMERICA:
        return this.IsTechEvolved("North_America_Alert");
      case Country.EContinentType.SOUTH_AMERICA:
        return this.IsTechEvolved("South_America_Alert");
      case Country.EContinentType.AFRICA:
        return this.IsTechEvolved("Africa_Alert");
      case Country.EContinentType.EUROPE:
        return this.IsTechEvolved("Europe_Alert");
      case Country.EContinentType.ASIA:
        return this.IsTechEvolved("Asia-Pacific_Alert");
      default:
        return false;
    }
  }

  public Country GetRandomInfectedCountry(Country exclude)
  {
    int count = this.infectedCountries.Count;
    if (this.infectedCountries.Contains(exclude))
      --count;
    if (count <= 0)
      return (Country) null;
    int index = ModelUtils.IntRand(0, count - 1);
    return this.infectedCountries[index] == exclude ? this.infectedCountries[index + 1] : this.infectedCountries[index];
  }

  public Country GetGuaranteedEventCountry()
  {
    foreach (Country country in World.instance.countries)
    {
      LocalDisease localDisease = country.GetLocalDisease(this);
      if (localDisease.hasIntel && (double) country.importance > 1.0 && (double) localDisease.infectedPercent > 9.9999997473787516E-06 && (double) localDisease.infectedPercent <= (double) ModelUtils.FloatRand(0.08f, 0.12f) && ModelUtils.IntRand(0, 3) == 0)
        return country;
    }
    return this.nexus;
  }

  public virtual void OnInitCure()
  {
  }

  public string GetVisualAuthorityValue()
  {
    return Mathf.Max(0, Mathf.RoundToInt(this.authority * 100f)).ToString();
  }

  public float GetVaccineMaxScore()
  {
    return (float) (100000.0 * (1.0 + (double) this.baseInfectivity * 0.0099999997764825821) * (1.0 + (double) this.baseLethality * 0.019999999552965164) * (1.0 + (double) this.difficulty * 0.10000000149011612));
  }

  public float GetVaccineScoreMod()
  {
    float num1 = Mathf.Pow(Mathf.Max(0.01f, this.authority), (float) (0.10000000149011612 * (1.0 + (double) this.difficulty))) * Mathf.Pow(1f - Mathf.Min(1f, this.globalDeadPercent * this.scoreDeadImpactMulti), (float) (5.0 * (1.0 + (double) this.difficulty)));
    if (this.isNexusContinentBorder)
      num1 *= 1.02f;
    float num2 = 1f;
    if (this.authFreezeCount > 0)
    {
      if (this.authFreezeCount == 1)
        num2 = 0.5f;
      if (this.authFreezeCount == 2)
        num2 = 0.25f;
    }
    return num1 * num2;
  }

  public List<Disease.AuthorityLossReason> GetAuthorityLossReasons(int sortIndex)
  {
    if (sortIndex > 1 || sortIndex < 0)
    {
      Debug.LogError((object) ("Unknown sort index for AuthLossReason: " + (object) sortIndex));
      return (List<Disease.AuthorityLossReason>) null;
    }
    if (this.authLossReasons[sortIndex] == null)
      this.authLossReasons[sortIndex] = new List<Disease.AuthorityLossReason>();
    else
      this.authLossReasons[sortIndex].Clear();
    List<Disease.AuthorityLossReason> authLossReason = this.authLossReasons[sortIndex];
    List<Disease.AuthorityLossReason> authorityLossReasonList1 = authLossReason;
    Disease.AuthorityLossReason authorityLossReason1 = new Disease.AuthorityLossReason();
    authorityLossReason1.type = Disease.EAuthLoss.AUTH_LOSS_PANIC;
    authorityLossReason1.total = this.authLossInfectedActual * 100f;
    authorityLossReason1.weekly = this.authLossInfectedList.DifferenceClamped() * 100f;
    Disease.AuthorityLossReason authorityLossReason2 = authorityLossReason1;
    authorityLossReasonList1.Add(authorityLossReason2);
    List<Disease.AuthorityLossReason> authorityLossReasonList2 = authLossReason;
    authorityLossReason1 = new Disease.AuthorityLossReason();
    authorityLossReason1.type = Disease.EAuthLoss.AUTH_LOSS_DEATHS;
    authorityLossReason1.total = this.authLossDeadActual * 100f;
    authorityLossReason1.weekly = this.authLossDeadList.DifferenceClamped() * 100f;
    Disease.AuthorityLossReason authorityLossReason3 = authorityLossReason1;
    authorityLossReasonList2.Add(authorityLossReason3);
    List<Disease.AuthorityLossReason> authorityLossReasonList3 = authLossReason;
    authorityLossReason1 = new Disease.AuthorityLossReason();
    authorityLossReason1.type = Disease.EAuthLoss.AUTH_LOSS_SPREAD;
    authorityLossReason1.total = this.authLossSpread * 100f;
    authorityLossReason1.weekly = this.authLossOtherList.DifferenceClamped() * 100f;
    Disease.AuthorityLossReason authorityLossReason4 = authorityLossReason1;
    authorityLossReasonList3.Add(authorityLossReason4);
    List<Disease.AuthorityLossReason> authorityLossReasonList4 = authLossReason;
    authorityLossReason1 = new Disease.AuthorityLossReason();
    authorityLossReason1.type = Disease.EAuthLoss.AUTH_LOSS_COMPLIANCE;
    authorityLossReason1.total = this.authLossCompliance * 100f;
    authorityLossReason1.weekly = this.authLossComplianceList.DifferenceClamped() * 100f;
    Disease.AuthorityLossReason authorityLossReason5 = authorityLossReason1;
    authorityLossReasonList4.Add(authorityLossReason5);
    switch (sortIndex)
    {
      case 0:
        authLossReason.Sort((Comparison<Disease.AuthorityLossReason>) ((a, b) => b.total.CompareTo(a.total)));
        break;
      case 1:
        authLossReason.Sort((Comparison<Disease.AuthorityLossReason>) ((a, b) => b.weekly.CompareTo(a.weekly)));
        break;
    }
    return authLossReason;
  }

  private string ReplaceFirst(string text, string token, string replace)
  {
    int length = text.IndexOf(token);
    if (length != -1)
      text = text.Substring(0, length) + replace + text.Substring(length + token.Length);
    return text;
  }

  public List<string> GetTopAuthorityLossReasons(int max)
  {
    List<string> authorityLossReasons1 = new List<string>();
    List<Disease.AuthorityLossReason> authorityLossReasons2 = this.GetAuthorityLossReasons(0);
    List<Disease.AuthorityLossReason> authorityLossReasons3 = this.GetAuthorityLossReasons(1);
    float num = 0.5f;
    string text = CLocalisationManager.GetText("%s: %.0f recent (%.0f total)");
    HashSet<Disease.EAuthLoss> eauthLossSet = new HashSet<Disease.EAuthLoss>();
    foreach (Disease.AuthorityLossReason authorityLossReason in authorityLossReasons3)
    {
      if ((double) authorityLossReason.weekly >= (double) num)
      {
        string str = this.ReplaceFirst(this.ReplaceFirst(this.ReplaceFirst(text, "%s", authorityLossReason.description), "%.0f", Mathf.RoundToInt(authorityLossReason.weekly).ToString()), "%.0f", Mathf.RoundToInt(authorityLossReason.total).ToString());
        eauthLossSet.Add(authorityLossReason.type);
        authorityLossReasons1.Add(str);
      }
    }
    foreach (Disease.AuthorityLossReason authorityLossReason in authorityLossReasons2)
    {
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        if (!eauthLossSet.Contains(authorityLossReason.type))
        {
          if (authorityLossReason.type == Disease.EAuthLoss.AUTH_LOSS_PANIC)
          {
            string str = this.ReplaceFirst(this.ReplaceFirst(this.ReplaceFirst(text, "%s", authorityLossReason.description), "%.0f", Mathf.RoundToInt(10f).ToString()), "%.0f", Mathf.RoundToInt(10f).ToString());
            eauthLossSet.Add(authorityLossReason.type);
            authorityLossReasons1.Add(str);
          }
          else if (authorityLossReason.type == Disease.EAuthLoss.AUTH_LOSS_SPREAD)
          {
            string str = this.ReplaceFirst(this.ReplaceFirst(this.ReplaceFirst(text, "%s", authorityLossReason.description), "%.0f", Mathf.RoundToInt(10f).ToString()), "%.0f", Mathf.RoundToInt(10f).ToString());
            eauthLossSet.Add(authorityLossReason.type);
            authorityLossReasons1.Add(str);
          }
        }
      }
      else if ((double) authorityLossReason.total >= (double) num && !eauthLossSet.Contains(authorityLossReason.type))
      {
        string str = this.ReplaceFirst(this.ReplaceFirst(this.ReplaceFirst(text, "%s", authorityLossReason.description), "%.0f", Mathf.RoundToInt(authorityLossReason.weekly).ToString()), "%.0f", Mathf.RoundToInt(authorityLossReason.total).ToString());
        eauthLossSet.Add(authorityLossReason.type);
        authorityLossReasons1.Add(str);
      }
    }
    return authorityLossReasons1;
  }

  public string GetAuthorityLossTip()
  {
    List<Disease.AuthorityLossReason> authorityLossReasons = this.GetAuthorityLossReasons(1);
    if (!this.diseaseNoticed)
      return "No new disease discovered";
    if (authorityLossReasons.Count == 0 || (double) authorityLossReasons[0].weekly < 3.0)
      return "Control the outbreak to protect your Authority";
    switch (authorityLossReasons[0].type)
    {
      case Disease.EAuthLoss.AUTH_LOSS_PANIC:
        return "Infected people are panicking about dying. Fund Response initiatives and lockdowns to reduce Infection";
      case Disease.EAuthLoss.AUTH_LOSS_DEATHS:
        return "Too many dead people. Fund Response initiatives and lockdowns to reduce the Fatality rate";
      case Disease.EAuthLoss.AUTH_LOSS_SPREAD:
        return "Too many countries infected. Use Quarantine measures to slow the spread";
      case Disease.EAuthLoss.AUTH_LOSS_COMPLIANCE:
        return "Too much Non-Compliance. Fund economic easing initiatives or disable Quarantine measures";
      default:
        return "Control the outbreak to protect your Authority";
    }
  }

  public string GetVaccineStage()
  {
    if (this.isNanovirus)
    {
      switch (this.vaccineStage)
      {
        case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE:
          return "Analysing";
        case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL:
          return "Analysed";
        case Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT:
        case Disease.EVaccineProgressStage.VACCINE_MANUFACTURE:
          return "Kill-code";
        case Disease.EVaccineProgressStage.VACCINE_RELEASE:
          return (double) this.cureCompletePerc >= 1.0 ? "Broadcasted" : "IG_Status_Vaccine_Broadcasting";
      }
    }
    else
    {
      switch (this.vaccineStage)
      {
        case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE:
          return "Analysing";
        case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL:
          return "Analysed";
        case Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT:
          return "Research";
        case Disease.EVaccineProgressStage.VACCINE_MANUFACTURE:
          return (double) this.cureCompletePerc <= 0.98000001907348633 ? "Production" : "Safety Checks";
        case Disease.EVaccineProgressStage.VACCINE_RELEASE:
          return (double) this.cureCompletePerc >= 1.0 ? "Released" : "IG_Status_Vaccine_Releasing";
      }
    }
    return "";
  }

  public virtual void SetHQCountry(Country c)
  {
    this.hqCountry = c;
    this.GetLocalDisease(c).hasIntel = true;
  }

  public float GetHighestAuthorityLoss()
  {
    float highestAuthorityLoss = 0.0f;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.localDiseases[index];
      if ((double) localDisease.totalLocalAuthLoss > (double) highestAuthorityLoss)
        highestAuthorityLoss = localDisease.totalLocalAuthLoss;
    }
    return highestAuthorityLoss;
  }

  public float GetHighestInfectedPercent()
  {
    float highestInfectedPercent = 0.0f;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.localDiseases[index];
      if ((double) localDisease.infectedPercent > (double) highestInfectedPercent)
        highestInfectedPercent = localDisease.infectedPercent;
    }
    return highestInfectedPercent;
  }

  public float GetHighestDeadPercent()
  {
    float highestDeadPercent = 0.0f;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.localDiseases[index];
      if ((double) localDisease.deadPercent > (double) highestDeadPercent)
        highestDeadPercent = localDisease.deadPercent;
    }
    return highestDeadPercent;
  }

  public float GetHighestCureResearch()
  {
    float highestCureResearch = 0.0f;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.localDiseases[index];
      if ((double) localDisease.localCureResearch > (double) highestCureResearch)
        highestCureResearch = localDisease.localCureResearch;
    }
    return highestCureResearch;
  }

  public float[] GetInfSevLethTotalMax()
  {
    float num1 = this.globalInfectiousnessMax;
    float num2 = this.globalSeverityMax;
    float num3 = this.globalLethalityMax;
    if (this.isCure)
    {
      num1 = this.globalInfectiousness * (this.globalInfectModMAX * this.reproductionVisual) * this.reproductionBarScale;
      num2 = this.globalLethality * (this.globalLethalityMod * this.mortalityVisual) * this.mortalityBarScale;
      num3 = this.unrestVisual;
    }
    return new float[3]
    {
      num1 / 100f,
      num2 / 100f,
      num3 / 100f
    };
  }

  [GameEventFunction]
  public virtual void NukeTwoCountries(string parameterCountry1, string parameterCountry2)
  {
    Debug.LogError((object) "NukeTwoCountries not supported in this game");
  }

  [GameEventFunction]
  public virtual void SendPlane(
    string parameterCountryFrom,
    string parameterCountryTo,
    string parameterInfectedPopulation)
  {
    Debug.LogError((object) "SendPlane not supported in this game");
  }

  [GameEventFunction]
  public virtual void ForceCreateBonusIcon(
    string parameterCountry,
    string parameterBonusIconType,
    string parameterBonusEvo,
    string parameterIgnoreOtherEffect)
  {
    Debug.LogError((object) "ForceCreateBonusIcon not supported here");
  }

  public enum EDiseaseType
  {
    BACTERIA,
    VIRUS,
    FUNGUS,
    PARASITE,
    PRION,
    NANO_VIRUS,
    BIO_WEAPON,
    NEURAX,
    NECROA,
    SIMIAN_FLU,
    TUTORIAL,
    VAMPIRE,
    FAKE_NEWS,
    CURE,
    CURETUTORIAL,
    DISEASEX,
  }

  public enum ECheatType
  {
    NONE,
    IMMUNE,
    HIDDEN,
    UNLIMITED,
    TURBO,
    SHUFFLE,
    LUCKY_DIP,
    DOUBLE_STRAIN,
    GOLDEN_HANDSHAKE,
    ADVANCE_PLANNING,
    FULL_SUPPORT,
    MAXIMUM_POWER,
    THE_AVENGERS,
    CURE_SHUFFLE,
    CURE_LUCKY_DIP,
  }

  public enum EVaccineProgressStage
  {
    VACCINE_NONE,
    VACCINE_KNOWLEDGE,
    VACCINE_KNOWLEDGE_FULL,
    VACCINE_DEVELOPMENT,
    VACCINE_MANUFACTURE,
    VACCINE_RELEASE,
  }

  [Flags]
  public enum EGenericDiseaseFlag : long
  {
    None = 1,
    GeneAirportController = 2,
    GenePortController = 4,
    GeneLandBorderController = 8,
    GeneMolecularBiologist = 16, // 0x0000000000000010
    GeneForensicEpidemiologist = 32, // 0x0000000000000020
    GeneCheckpointEnforcer = 64, // 0x0000000000000040
    GeneQuarantineCoordinator = 128, // 0x0000000000000080
    GeneLocalOutbreakAnalyst = 256, // 0x0000000000000100
    GeneNationalOutbreakAnalyst = 512, // 0x0000000000000200
    GeneCelebrityScientist = 1024, // 0x0000000000000400
    GeneConstructionManager = 2048, // 0x0000000000000800
    GeneEmpathyTrainer = 4096, // 0x0000000000001000
    GeneRegulationEnforcer = 8192, // 0x0000000000002000
    GeneTechnicalOfficer = 16384, // 0x0000000000004000
    GeneStrategicFundraiser = 32768, // 0x0000000000008000
    GeneFastResponseEMTs = 65536, // 0x0000000000010000
    GeneSituationDirector = 131072, // 0x0000000000020000
    GeneEconomicForecaster = 262144, // 0x0000000000040000
    GeneOutreachCoordinator = 524288, // 0x0000000000080000
    GeneCrisisManager = 1048576, // 0x0000000000100000
    GeneEthicsWatchdog = 2097152, // 0x0000000000200000
    GeneChaosEngineer = 4194304, // 0x0000000000400000
    GeneMedicalCoordinator = 8388608, // 0x0000000000800000
    GeneDisasterManager = 16777216, // 0x0000000001000000
    GeneProcurementDirector = 33554432, // 0x0000000002000000
    CheatAdvancePlanning = 67108864, // 0x0000000004000000
    CheatFullSupport = 134217728, // 0x0000000008000000
    CheatMaximumPower = 268435456, // 0x0000000010000000
    CheatTheAvengers = 536870912, // 0x0000000020000000
    HasFullyLockedDown = 1073741824, // 0x0000000040000000
    TechBorderMonitoring = 2147483648, // 0x0000000080000000
    IsDiseaseX = 4294967296, // 0x0000000100000000
    CheatCureShuffle = 8589934592, // 0x0000000200000000
    CheatCureLuckyDip = 17179869184, // 0x0000000400000000
    CheatShuffle = 34359738368, // 0x0000000800000000
    CheatLuckyDip = 68719476736, // 0x0000001000000000
  }

  public enum ECureScenario
  {
    None,
    Cure_Standard,
    Cure_Bioweapon,
    Cure_Fungus,
    Cure_Nanovirus,
    Cure_Parasite,
    Cure_Prion,
    Cure_Virus,
    Cure_DiseaseX,
  }

  public enum EAuthLoss
  {
    None,
    AUTH_LOSS_NONE,
    AUTH_LOSS_PANIC,
    AUTH_LOSS_DEATHS,
    AUTH_LOSS_SPREAD,
    AUTH_LOSS_COMPLIANCE,
  }

  public enum EDefeatType
  {
    DEAD,
    CURED,
  }

  public struct AuthorityLossReason
  {
    public Disease.EAuthLoss type;
    public float total;
    public float weekly;

    public string description
    {
      get
      {
        switch (this.type)
        {
          case Disease.EAuthLoss.AUTH_LOSS_PANIC:
            return CLocalisationManager.GetLocalisedText("Panic");
          case Disease.EAuthLoss.AUTH_LOSS_DEATHS:
            return CLocalisationManager.GetLocalisedText("Deaths");
          case Disease.EAuthLoss.AUTH_LOSS_SPREAD:
            return CLocalisationManager.GetLocalisedText("Disease Spread");
          case Disease.EAuthLoss.AUTH_LOSS_COMPLIANCE:
            return CLocalisationManager.GetLocalisedText("Non-Compliance");
          default:
            return "";
        }
      }
    }
  }
}
