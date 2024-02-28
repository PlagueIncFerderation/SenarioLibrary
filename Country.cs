// Decompiled with JetBrains decompiler
// Type: Country
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class Country
{
  public string name;
  public string countryDescription;
  public string countryDescriptionExtended;
  public string airportPos;
  public string fortPos;
  public Vector3? apeColonyPosition;
  public Vector2 specialBuildingLocation;
  public Vector3? fortPosition;
  public Vector3? initialSpawnPosition;
  public List<Country> dispatchedHiddenInfectedFlights = new List<Country>();
  public List<Country> dispatchedHiddenInfectedBoats = new List<Country>();
  public List<Country.GovernmentActionEvent> governmentActionEvents = new List<Country.GovernmentActionEvent>();
  [NonSerialized]
  public Disease diseaseNexus;
  [NonSerialized]
  public Disease diseaseSuperCure;
  [NonSerialized]
  public List<TravelRoute> airRoutes = new List<TravelRoute>();
  [NonSerialized]
  public List<TravelRoute> seaRoutes = new List<TravelRoute>();
  [NonSerialized]
  public List<TravelRoute> apeMigrationRoutes = new List<TravelRoute>();
  [NonSerialized]
  public HashSet<Country> neighbours = new HashSet<Country>();
  [NonSerialized]
  public List<TravelRoute> landRoutes = new List<TravelRoute>();
  [NonSerialized]
  public IDictionary<Country, float> neighbourDistance = (IDictionary<Country, float>) new Dictionary<Country, float>();
  [NonSerialized]
  public List<LocalDisease> localDiseases = new List<LocalDisease>();
  internal IDictionary<int, Country.PopulationPipe> pipes = (IDictionary<int, Country.PopulationPipe>) new Dictionary<int, Country.PopulationPipe>();
  [NonSerialized]
  public HashSet<string> actionsTaken = new HashSet<string>();
  [NonSerialized]
  public List<string> actionsSpecialInterest = new List<string>();
  private static HashSet<string> countryIDContainsContinent = new HashSet<string>()
  {
    "south_east_asia",
    "central_asia",
    "central_european_states",
    "east_africa",
    "western_africa",
    "central_africa",
    "south_africa"
  };

  public abstract int countryNumber { get; set; }

  public abstract float closedBorderOpacity { get; set; }

  public bool hasAirports
  {
    get => this.airport;
    set => this.airport = value;
  }

  public abstract bool airport { get; set; }

  public abstract int startCountryEvoBonus { get; set; }

  public abstract long currentPopulation { get; set; }

  public abstract long originalPopulation { get; set; }

  public abstract long healthyPopulation { get; set; }

  public abstract long deadPopulation { get; set; }

  public abstract float publicOrder { get; set; }

  public abstract float importance { get; set; }

  public abstract float medicalBudget { get; set; }

  public abstract float basePopulationDensity { get; set; }

  public abstract bool isDestroyed { get; set; }

  public abstract bool wealthy { get; set; }

  public abstract bool poverty { get; set; }

  public abstract bool urban { get; set; }

  public abstract bool rural { get; set; }

  public abstract bool hot { get; set; }

  public abstract bool cold { get; set; }

  public abstract bool humid { get; set; }

  public abstract bool arid { get; set; }

  public float localHCombatStrength
  {
    get => this.localHumanCombatStrength;
    set => this.localHumanCombatStrength = value;
  }

  public abstract float localHumanCombatStrength { get; set; }

  public float localHCombatStrengthMax
  {
    get => this.localHumanCombatStrengthMax;
    set => this.localHumanCombatStrengthMax = value;
  }

  public abstract float localHumanCombatStrengthMax { get; set; }

  public abstract float battleVictoryCount { get; set; }

  public abstract bool fortWasDestroyed { get; set; }

  public abstract bool fortPlaneSpawned { get; set; }

  public abstract EFortState fortState { get; set; }

  public abstract float govLocalInfectiousness { get; set; }

  public abstract float govLocalCorpseTransmission { get; set; }

  public abstract float govLocalLethality { get; set; }

  public abstract float govLocalSeverity { get; set; }

  public abstract float govPublicOrder { get; set; }

  public abstract bool borderStatus { get; set; }

  public abstract bool airportStatus { get; set; }

  public abstract bool portStatus { get; set; }

  public abstract bool govActionsAllowed { get; set; }

  public abstract float deadPercent { get; set; }

  public abstract float infectedPercent { get; set; }

  public abstract float zombiePercent { get; set; }

  public abstract float healthyPercent { get; set; }

  public abstract bool hasPorts { get; set; }

  public abstract long totalZombie { get; set; }

  public abstract long totalInfected { get; set; }

  public abstract long healthyPopulationSusceptible { get; set; }

  public abstract long healthyPopulationImmune { get; set; }

  public abstract long apeOriginalPopulation { get; set; }

  public abstract EApeLabState apeLabStatus { get; set; }

  public abstract EApeColonyState apeColonyStatus { get; set; }

  public abstract int apeNumDestroyedLabs { get; set; }

  public abstract int apeNumDestroyedColonies { get; set; }

  public abstract bool apeHordeMoving { get; set; }

  public abstract float labDayCount { get; set; }

  public abstract float labCapacity { get; set; }

  public abstract float labDailyResearch { get; set; }

  public abstract float govLocalApeInfectiousness { get; set; }

  public abstract float govLocalApeLethality { get; set; }

  public abstract float localDanger { get; set; }

  public abstract int infectBoostCount { get; set; }

  public abstract int lethalBoostCount { get; set; }

  public abstract bool doubleInfected { get; set; }

  public abstract ECoopLabState coopLabStatus { get; set; }

  public abstract int coopLabDiseaseId { get; set; }

  public abstract string id { get; set; }

  public abstract bool isNuclear { get; set; }

  public virtual float govDeadBodyTransmission { get; set; }

  public virtual string continent { get; set; }

  public virtual Country.EContinentType continentType { get; set; }

  public virtual int presimSteps { get; set; }

  public virtual float startingMedicalCapacity { get; set; }

  public virtual float medicalCapacity { get; set; }

  public virtual float baseInfluence { get; set; }

  public virtual long healthyRecoveredPopulation { get; set; }

  public virtual float healthyRecoveredPercent { get; set; }

  public virtual int numNeighbours { get; set; }

  public abstract double TransferPopulation(
    double number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0);

  public abstract void GameUpdate();

  public abstract void VehicleArrived(Vehicle vehicle, int presetInt = -1);

  public abstract float DroneAttack(Disease d, bool colonyDestroyed, float presetFloat = -1f);

  public virtual long TransferPopulationNoPipe(
    long number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0)
  {
    Debug.LogError((object) "Not implemnted in this game mode!");
    return 0;
  }

  public virtual float myUninfected { get; set; }

  public virtual float myUninfectedPercentage { get; set; }

  public virtual float theirUninfected { get; set; }

  public virtual float theirUninfectedPercentage { get; set; }

  public virtual bool nuked { get; set; }

  public virtual string nukedBy { get; set; }

  public virtual float area { get; set; }

  public virtual Country.ECountrySize countrySize { get; set; }

  public bool IsIsland() => this.neighbours.Count == 0;

  public bool IsNeighbour(Country c) => this.neighbours.Contains(c);

  public float zombieOrDeadPercent => this.zombiePercent + this.deadPercent;

  public long totalHealthy => this.healthyPopulation;

  public bool hasAirport => this.airport;

  public bool hasApeLab
  {
    get
    {
      return this.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.apeLabStatus == EApeLabState.APE_LAB_INACTIVE;
    }
  }

  public bool hasApeColony => this.apeColonyStatus == EApeColonyState.APE_COLONY_ALIVE;

  public List<Country.GovernmentActionEvent> GetGovActionEvents()
  {
    if (!CGameManager.IsCureGame)
      return this.governmentActionEvents;
    this.governmentActionEvents.Sort((Comparison<Country.GovernmentActionEvent>) ((a, b) => a.removed.CompareTo(b.removed) * 100 + a.turn.CompareTo(b.turn)));
    return this.governmentActionEvents;
  }

  public void AddNeighbour(Country c, float distance)
  {
    this.neighbourDistance[c] = distance;
    if (!this.neighbours.Add(c))
      return;
    this.landRoutes.Add(new TravelRoute(0.2f)
    {
      source = this,
      destination = c,
      routeType = ERouteType.Land
    });
    this.numNeighbours = this.neighbours.Count;
  }

  public float GetDistance(Country c) => this.neighbourDistance[c];

  public void SetDistance(Country c, float dist) => this.neighbourDistance[c] = dist;

  public virtual void Initialise() => this.localDiseases.Clear();

  public void InitialiseSimian(Disease disease)
  {
  }

  public long totalDead => this.deadPopulation;

  private long totalCurrentInfected
  {
    get
    {
      long totalCurrentInfected = 0;
      for (int index = 0; index < this.localDiseases.Count; ++index)
        totalCurrentInfected += this.localDiseases[index].controlledInfected;
      return totalCurrentInfected;
    }
  }

  private long totalCurrentZombie
  {
    get
    {
      long totalCurrentZombie = 0;
      for (int index = 0; index < this.localDiseases.Count; ++index)
        totalCurrentZombie += this.localDiseases[index].zombie;
      return totalCurrentZombie;
    }
  }

  public float totalSeverity
  {
    get
    {
      float totalSeverity = 0.0f;
      for (int index = 0; index < this.localDiseases.Count; ++index)
        totalSeverity += this.localDiseases[index].localSeverity;
      return totalSeverity;
    }
  }

  public LocalDisease GetLocalDisease(Disease d)
  {
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      if (this.localDiseases[index].disease == d)
        return this.localDiseases[index];
    }
    return d?.CreateLocalDisease(this);
  }

  public void AddLocalDisease(LocalDisease ld) => this.localDiseases.Add(ld);

  protected Country.PopulationPipe GetCountryTransferPipe(
    Disease acting,
    Country destination,
    Country.EPopulationType popType)
  {
    int key1 = (acting == null ? 0 : acting.id) + destination.countryNumber * 16 + this.countryNumber * 256 + (int) popType * 4096;
    if (World.instance.countryTransferPipes.ContainsKey(key1))
    {
      Country.PopulationPipe countryTransferPipe = World.instance.countryTransferPipes[key1];
      countryTransferPipe.reverse = false;
      return countryTransferPipe;
    }
    int key2 = (acting == null ? 0 : acting.id) + destination.countryNumber * 16 + this.countryNumber * 256;
    if (World.instance.countryTransferPipes.ContainsKey(key2))
    {
      Country.PopulationPipe countryTransferPipe = World.instance.countryTransferPipes[key2];
      countryTransferPipe.reverse = true;
      return countryTransferPipe;
    }
    Country.PopulationPipe countryTransferPipe1 = new Country.PopulationPipe();
    World.instance.countryTransferPipes[key1] = countryTransferPipe1;
    return countryTransferPipe1;
  }

  protected Country.PopulationPipe GetPipe(
    Disease acting,
    Country.EPopulationType PT_from,
    Country.EPopulationType PT_to)
  {
    int key1 = (acting == null ? 0 : acting.id) + (int) PT_from * 16 + (int) PT_to * 256;
    if (this.pipes.ContainsKey(key1))
    {
      Country.PopulationPipe pipe = this.pipes[key1];
      pipe.reverse = false;
      return pipe;
    }
    int key2 = (acting == null ? 0 : acting.id) + (int) PT_from * 256 + (int) PT_to * 16;
    if (this.pipes.ContainsKey(key2))
    {
      Country.PopulationPipe pipe = this.pipes[key2];
      pipe.reverse = true;
      return pipe;
    }
    Country.PopulationPipe pipe1 = new Country.PopulationPipe();
    this.pipes[key1] = pipe1;
    return pipe1;
  }

  public double TransferPopulation(
    double number,
    Country destinationCountry,
    Country.EPopulationType popType,
    Disease acting)
  {
    Country.PopulationPipe countryTransferPipe = this.GetCountryTransferPipe(acting, destinationCountry, popType);
    countryTransferPipe.Add(number);
    long val = (long) countryTransferPipe.val;
    double num = this.TransferPopulation((double) val, popType, acting, Country.EPopulationType.NONE);
    destinationCountry.TransferPopulation((double) val - num, Country.EPopulationType.NONE, acting, popType);
    return num;
  }

  public long TransferPopulationNoPipe(
    long number,
    Country.EPopulationType sourcePopType,
    Country destinationCountry,
    Country.EPopulationType destPopType,
    Disease acting)
  {
    long num = this.TransferPopulationNoPipe(number, sourcePopType, acting, Country.EPopulationType.NONE);
    destinationCountry.TransferPopulationNoPipe(number - num, Country.EPopulationType.NONE, acting, destPopType);
    return num;
  }

  public void SpawnCureIcon(Disease d)
  {
    World.instance.AddBonusIcon(new BonusIcon(d, this, BonusIcon.EBonusIconType.CURE));
    ++d.numCureBubblesWithoutTouch;
  }

  public void SpawnNeuraxPlane(Country other, Disease controller)
  {
    Vehicle vehicle = Vehicle.Create();
    vehicle.type = Vehicle.EVehicleType.Airplane;
    vehicle.subType = Vehicle.EVehicleSubType.Neurax;
    LocalDisease localDisease = controller.GetLocalDisease(this);
    vehicle.AddInfected(controller, (int) Mathf.Min((float) ((double) localDisease.infectedPercent * (double) ModelUtils.IntRand(5, 1000) + 4.0), (float) localDisease.allInfected / 4f));
    vehicle.SetRoute(this, other);
    World.instance.AddVehicle(vehicle);
  }

  public EApeLabState ChangeApeLabState
  {
    set => this.ChangeApeLabStateF(value, true);
  }

  public void ChangeApeLabStateF(EApeLabState state, bool force = false)
  {
    if (!(this.apeLabStatus != state | force))
      return;
    this.apeLabStatus = state;
    if (state == EApeLabState.APE_LAB_INACTIVE && World.instance.diseases[0].diseaseType == Disease.EDiseaseType.VAMPIRE)
      ++World.instance.diseases[0].vampLabsCurrent;
    if (state == EApeLabState.APE_LAB_NONE && World.instance.diseases[0].diseaseType == Disease.EDiseaseType.VAMPIRE)
      World.instance.diseases[0].vampLabsCurrent = Mathf.Max(World.instance.diseases[0].vampLabsCurrent - 1, 0);
    World.instance.UpdateApeLab(this);
    if (state != EApeLabState.APE_LAB_DESTROYED)
      return;
    ++this.apeNumDestroyedLabs;
    if (World.instance.diseases[0].diseaseType != Disease.EDiseaseType.VAMPIRE)
      return;
    ++World.instance.diseases[0].vampLabsDestroyed;
  }

  public EApeColonyState ChangeApeColonyState
  {
    set => this.ChangeApeColonyStateF(value);
  }

  public void ChangeApeColonyStateF(EApeColonyState state)
  {
    if (this.apeColonyStatus == state)
      return;
    this.apeColonyStatus = state;
    World.instance.UpdateApeColony(this);
    if (state != EApeColonyState.APE_COLONY_DESTROYED)
      return;
    ++this.apeNumDestroyedColonies;
  }

  public EFortState ChangeFortState
  {
    set => this.ChangeFortStateF(value);
  }

  public void ChangeFortStateF(EFortState state)
  {
    if (this.fortState == state)
      return;
    this.fortState = state;
    if (state == EFortState.FORT_ALIVE)
      World.instance.AddFort(this);
    if (state != EFortState.FORT_DESTROYED)
      return;
    this.fortWasDestroyed = true;
  }

  public bool HasFort() => this.fortState == EFortState.FORT_ALIVE;

  public bool HasApeRampage()
  {
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      if (this.localDiseases[index].apeStatusRampage > 0)
        return true;
    }
    return false;
  }

  public bool IsActionTaken(GovernmentAction action) => this.actionsTaken.Contains(action.id);

  public bool IsActionTaken(string actionID) => this.actionsTaken.Contains(actionID);

  public void SetActionTaken(GovernmentAction action)
  {
    if (string.IsNullOrEmpty(action.id))
      Debug.LogError((object) ("Action: " + (object) action + " " + action.actionName + " has a blank ID"));
    this.actionsTaken.Add(action.id);
    this.governmentActionEvents.RemoveAll((Predicate<Country.GovernmentActionEvent>) (a => a.id == action.id));
    this.governmentActionEvents.Add(new Country.GovernmentActionEvent(action.id, World.instance.DiseaseTurn, false));
  }

  public int GetLastFired(string actionId)
  {
    Country.GovernmentActionEvent governmentActionEvent = this.governmentActionEvents.Find((Predicate<Country.GovernmentActionEvent>) (a => a.id == actionId && !a.removed));
    return governmentActionEvent != null ? governmentActionEvent.turn : 0;
  }

  public void RemoveActionTaken(GovernmentAction action)
  {
    this.actionsTaken.Remove(action.id);
    this.governmentActionEvents.RemoveAll((Predicate<Country.GovernmentActionEvent>) (a => a.id == action.id));
    this.governmentActionEvents.Add(new Country.GovernmentActionEvent(action.id, World.instance.DiseaseTurn, true));
  }

  public string GetActionText(GovernmentAction ga, Disease d, bool removed)
  {
    if (removed)
    {
      if (string.IsNullOrEmpty(ga.newsContentDestroyed))
        return (string) null;
      if (!CGameManager.IsCureGame)
        return CLocalisationManager.GetText(ga.newsContentDestroyed);
      string[] variables = new string[1]{ this.name };
      return CLocalisationManager.GetLocalisedText(ga.newsContentDestroyed, variables);
    }
    if (!d.isCure || !(ga.id == "infection"))
      return CLocalisationManager.GetText(ga.actionName);
    LocalDisease localDisease = this.GetLocalDisease(d);
    if (localDisease.infectedFromCountry == null)
      return CLocalisationManager.GetText("New disease detected");
    string text;
    switch (localDisease.infectionMethod)
    {
      case Country.EInfectionMethod.IM_PLANE:
        text = "Infected by plane from %s";
        break;
      case Country.EInfectionMethod.IM_LAND:
        text = "Infected by land from %s";
        break;
      case Country.EInfectionMethod.IM_NEXUS:
        text = "New disease detected";
        break;
      case Country.EInfectionMethod.IM_BOAT:
        text = "Infected by boat from %s";
        break;
      case Country.EInfectionMethod.IM_FUNGALSPORE:
        text = "Infected by fungal spore from %s";
        break;
      default:
        text = "Infected by unknown origin";
        break;
    }
    return CLocalisationManager.GetParameterisedText(new ParameterisedString(text, new string[1]
    {
      "country.name"
    }), d, localDisease.infectedFromCountry);
  }

  public bool HasTrait(Country.Trait trait)
  {
    if (trait == Country.Trait.None)
      return true;
    foreach (Country.Trait trait1 in this.Traits)
    {
      if (trait == trait1)
        return true;
    }
    return false;
  }

  public bool HasTrait(MiniMapController.Trait trait, Disease d)
  {
    switch (trait)
    {
      case MiniMapController.Trait.Arid:
        return this.HasTrait(Country.Trait.Arid);
      case MiniMapController.Trait.Humid:
        return this.HasTrait(Country.Trait.Humid);
      case MiniMapController.Trait.Cold:
        return this.HasTrait(Country.Trait.Cold);
      case MiniMapController.Trait.Hot:
        return this.HasTrait(Country.Trait.Hot);
      case MiniMapController.Trait.Rich:
        return this.HasTrait(Country.Trait.Rich);
      case MiniMapController.Trait.Poor:
        return this.HasTrait(Country.Trait.Poor);
      case MiniMapController.Trait.Urban:
        return this.HasTrait(Country.Trait.Urban);
      case MiniMapController.Trait.Rural:
        return this.HasTrait(Country.Trait.Rural);
      case MiniMapController.Trait.None:
        return true;
      case MiniMapController.Trait.LostAuthority:
        return (double) this.GetLocalDisease(d).totalLocalAuthLoss > 0.0;
      case MiniMapController.Trait.NonCompliance:
        return (double) this.GetLocalDisease(d).compliance < 1.0;
      case MiniMapController.Trait.InfectedPop:
        return this.totalInfected > 0L && this.GetLocalDisease(d).hasIntel;
      case MiniMapController.Trait.DeadPop:
        return this.deadPopulation > 0L && this.GetLocalDisease(d).hasIntel;
      case MiniMapController.Trait.VaccineDev:
        LocalDisease localDisease = this.GetLocalDisease(d);
        return localDisease.hasIntel && (double) localDisease.localCureResearch > 0.0;
      default:
        return false;
    }
  }

  public bool hasTrait(Country.Trait trait)
  {
    if (trait == Country.Trait.None)
      return true;
    foreach (Country.Trait trait1 in this.Traits)
    {
      if (trait == trait1)
        return true;
    }
    return false;
  }

  public Country.Trait[] Traits
  {
    get
    {
      return new Country.Trait[4]
      {
        !this.hot ? (!this.cold ? Country.Trait.None : Country.Trait.Cold) : Country.Trait.Hot,
        !this.arid ? (!this.humid ? Country.Trait.None : Country.Trait.Humid) : Country.Trait.Arid,
        !this.urban ? (!this.rural ? Country.Trait.None : Country.Trait.Rural) : Country.Trait.Urban,
        !this.wealthy ? (!this.poverty ? Country.Trait.None : Country.Trait.Poor) : Country.Trait.Rich
      };
    }
  }

  public static bool CheckUsesContinentTag(string countryID)
  {
    return !Country.countryIDContainsContinent.Contains(countryID);
  }

  public static string GetContinentTag(Country.EContinentType continentType)
  {
    string continentTag;
    switch (continentType)
    {
      case Country.EContinentType.NONE:
        continentTag = "";
        break;
      case Country.EContinentType.NORTH_AMERICA:
        continentTag = "North America";
        break;
      case Country.EContinentType.SOUTH_AMERICA:
        continentTag = "South America";
        break;
      case Country.EContinentType.AFRICA:
        continentTag = "Africa";
        break;
      case Country.EContinentType.EUROPE:
        continentTag = "Europe";
        break;
      case Country.EContinentType.ASIA:
        continentTag = "Asia-Pacific";
        break;
      default:
        continentTag = "";
        break;
    }
    return continentTag;
  }

  public string GetActionDate(GovernmentAction ga) => ga.GetDate().ToString() + " Day";

  public class GovernmentActionEvent
  {
    public int turn;
    public string id;
    public bool removed;

    public GovernmentActionEvent(string id, int turn, bool removed)
    {
      this.turn = turn;
      this.id = id;
      this.removed = removed;
    }
  }

  public enum EContinentType
  {
    NONE,
    NORTH_AMERICA,
    SOUTH_AMERICA,
    AFRICA,
    EUROPE,
    ASIA,
  }

  public enum EPopulationType
  {
    HEALTHY,
    INFECTED,
    DEAD,
    ZOMBIE,
    NONE,
    HEALTHY_SUSCEPTIBLE,
    HEALTHY_IMMUNE,
    APE_HEALTHY,
    APE_INFECTED,
    APE_DEAD,
    INFECTED_SHARED,
    HEALTHY_RECOVERED,
  }

  [Flags]
  public enum EGenericCountryFlag
  {
    None = 1,
    OrangeBubble = 2,
    BlackBubble = 4,
    GreenBubble = 8,
    RedBubble = 16, // 0x00000010
    CountryStatusShown = 32, // 0x00000020
    CureProgressBubble = 64, // 0x00000040
    ReceivedCurePlane = 128, // 0x00000080
    DeadBubbleForCure = 256, // 0x00000100
    IntelVehicleDispatched = 512, // 0x00000200
  }

  public enum EInfectionMethod
  {
    IM_UNKNOWN,
    IM_PLANE,
    IM_LAND,
    IM_NEXUS,
    IM_BOAT,
    IM_FUNGALSPORE,
  }

  public enum ECountrySize
  {
    GC_TINY,
    GC_SMALL,
    GC_MEDIUM,
    GC_LARGE,
    GC_HUGE,
  }

  public class PopulationPipe
  {
    public bool reverse;
    private double _val;

    public void Set(double v) => this._val = this.reverse ? -v : v;

    public void Add(double v) => this._val += this.reverse ? -v : v;

    public double val => !this.reverse ? this._val : -this._val;
  }

  public enum Trait
  {
    Arid,
    Humid,
    Cold,
    Hot,
    Rich,
    Poor,
    Urban,
    Rural,
    None,
  }
}
