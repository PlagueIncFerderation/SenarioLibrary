// Decompiled with JetBrains decompiler
// Type: SPCountryData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
public class SPCountryData
{
  public int countryNumber = -1;
  public float closedBorderOpacity;
  public int _airport;
  public int startCountryEvoBonus;
  public long currentPopulation;
  public long originalPopulation;
  public long healthyPopulation;
  public long deadPopulation;
  public float publicOrder;
  public float importance;
  public float medicalBudget;
  public float basePopulationDensity;
  public int _isDestroyed;
  public int _wealthy;
  public int _poverty;
  public int _urban;
  public int _rural;
  public int _hot;
  public int _cold;
  public int _humid;
  public int _arid;
  public float localHumanCombatStrength;
  public float localHumanCombatStrengthMax;
  public float battleVictoryCount;
  public int _fortWasDestroyed;
  public int _fortPlaneSpawned;
  public EFortState fortState;
  public float govLocalInfectiousness;
  public float govLocalCorpseTransmission;
  public float govLocalLethality;
  public float govLocalSeverity;
  public float govPublicOrder;
  public int _borderStatus;
  public int _airportStatus;
  public int _portStatus;
  public int _govActionsAllowed;
  public float deadPercent;
  public float infectedPercent;
  public float zombiePercent;
  public float healthyPercent;
  public int _hasPorts;
  public long totalZombie;
  public long totalInfected;
  public long healthyPopulationSusceptible;
  public long healthyPopulationImmune;
  public long apeOriginalPopulation;
  public EApeLabState apeLabStatus;
  public EApeColonyState apeColonyStatus;
  public int apeNumDestroyedLabs;
  public int apeNumDestroyedColonies;
  public int _apeHordeMoving;
  public float labDayCount;
  public float labCapacity;
  public float labDailyResearch;
  public float govLocalApeInfectiousness;
  public float govLocalApeLethality;
  public float localDanger;
  public float govDeadBodyTransmission;
  public Country.EContinentType continentType;
  public int presimSteps;
  public float startingMedicalCapacity;
  public float medicalCapacity;
  public float baseInfluence;
  public long healthyRecoveredPopulation;
  public float healthyRecoveredPercent;
  public int numNeighbours;
  public string id;
  public float accumulatedFlight;

  public bool airport
  {
    get => this._airport != 0;
    set => this._airport = value ? 1 : 0;
  }

  public bool isDestroyed
  {
    get => this._isDestroyed != 0;
    set => this._isDestroyed = value ? 1 : 0;
  }

  public bool wealthy
  {
    get => this._wealthy != 0;
    set => this._wealthy = value ? 1 : 0;
  }

  public bool poverty
  {
    get => this._poverty != 0;
    set => this._poverty = value ? 1 : 0;
  }

  public bool urban
  {
    get => this._urban != 0;
    set => this._urban = value ? 1 : 0;
  }

  public bool rural
  {
    get => this._rural != 0;
    set => this._rural = value ? 1 : 0;
  }

  public bool hot
  {
    get => this._hot != 0;
    set => this._hot = value ? 1 : 0;
  }

  public bool cold
  {
    get => this._cold != 0;
    set => this._cold = value ? 1 : 0;
  }

  public bool humid
  {
    get => this._humid != 0;
    set => this._humid = value ? 1 : 0;
  }

  public bool arid
  {
    get => this._arid != 0;
    set => this._arid = value ? 1 : 0;
  }

  public bool fortWasDestroyed
  {
    get => this._fortWasDestroyed != 0;
    set => this._fortWasDestroyed = value ? 1 : 0;
  }

  public bool fortPlaneSpawned
  {
    get => this._fortPlaneSpawned != 0;
    set => this._fortPlaneSpawned = value ? 1 : 0;
  }

  public bool borderStatus
  {
    get => this._borderStatus != 0;
    set => this._borderStatus = value ? 1 : 0;
  }

  public bool airportStatus
  {
    get => this._airportStatus != 0;
    set => this._airportStatus = value ? 1 : 0;
  }

  public bool portStatus
  {
    get => this._portStatus != 0;
    set => this._portStatus = value ? 1 : 0;
  }

  public bool govActionsAllowed
  {
    get => this._govActionsAllowed != 0;
    set => this._govActionsAllowed = value ? 1 : 0;
  }

  public float zombieOrDeadPercent => this.zombiePercent + this.deadPercent;

  public bool hasPorts
  {
    get => this._hasPorts != 0;
    set => this._hasPorts = value ? 1 : 0;
  }

  public bool hasAirport => this.airport;

  public long totalHealthy => this.healthyPopulation;

  public bool hasApeLab
  {
    get
    {
      return this.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.apeLabStatus == EApeLabState.APE_LAB_INACTIVE;
    }
  }

  public bool hasApeColony => this.apeColonyStatus == EApeColonyState.APE_COLONY_ALIVE;

  public bool apeHordeMoving
  {
    get => this._apeHordeMoving != 0;
    set => this._apeHordeMoving = value ? 1 : 0;
  }
}
