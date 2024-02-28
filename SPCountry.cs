// Decompiled with JetBrains decompiler
// Type: SPCountry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SPCountry : Country
{
  public SPCountryData mData;

  public override int countryNumber
  {
    get => this.mData.countryNumber;
    set => this.mData.countryNumber = value;
  }

  public override float closedBorderOpacity
  {
    get => this.mData.closedBorderOpacity;
    set => this.mData.closedBorderOpacity = value;
  }

  public override bool airport
  {
    get => this.mData.airport;
    set => this.mData.airport = value;
  }

  public override int startCountryEvoBonus
  {
    get => this.mData.startCountryEvoBonus;
    set => this.mData.startCountryEvoBonus = value;
  }

  public override long currentPopulation
  {
    get => this.mData.currentPopulation;
    set => this.mData.currentPopulation = value;
  }

  public override long originalPopulation
  {
    get => this.mData.originalPopulation;
    set => this.mData.originalPopulation = value;
  }

  public override long healthyPopulation
  {
    get => this.mData.healthyPopulation;
    set => this.mData.healthyPopulation = value;
  }

  public override long deadPopulation
  {
    get => this.mData.deadPopulation;
    set => this.mData.deadPopulation = value;
  }

  public override float publicOrder
  {
    get => this.mData.publicOrder;
    set => this.mData.publicOrder = value;
  }

  public override float importance
  {
    get => this.mData.importance;
    set => this.mData.importance = value;
  }

  public override float medicalBudget
  {
    get => this.mData.medicalBudget;
    set => this.mData.medicalBudget = value;
  }

  public override float basePopulationDensity
  {
    get => this.mData.basePopulationDensity;
    set => this.mData.basePopulationDensity = value;
  }

  public override bool isDestroyed
  {
    get => this.mData.isDestroyed;
    set => this.mData.isDestroyed = value;
  }

  public override bool wealthy
  {
    get => this.mData.wealthy;
    set => this.mData.wealthy = value;
  }

  public override bool poverty
  {
    get => this.mData.poverty;
    set => this.mData.poverty = value;
  }

  public override bool urban
  {
    get => this.mData.urban;
    set => this.mData.urban = value;
  }

  public override bool rural
  {
    get => this.mData.rural;
    set => this.mData.rural = value;
  }

  public override bool hot
  {
    get => this.mData.hot;
    set => this.mData.hot = value;
  }

  public override bool cold
  {
    get => this.mData.cold;
    set => this.mData.cold = value;
  }

  public override bool humid
  {
    get => this.mData.humid;
    set => this.mData.humid = value;
  }

  public override bool arid
  {
    get => this.mData.arid;
    set => this.mData.arid = value;
  }

  public override float localHumanCombatStrength
  {
    get => this.mData.localHumanCombatStrength;
    set => this.mData.localHumanCombatStrength = value;
  }

  public override float localHumanCombatStrengthMax
  {
    get => this.mData.localHumanCombatStrengthMax;
    set => this.mData.localHumanCombatStrengthMax = value;
  }

  public override float battleVictoryCount
  {
    get => this.mData.battleVictoryCount;
    set => this.mData.battleVictoryCount = value;
  }

  public override bool fortWasDestroyed
  {
    get => this.mData.fortWasDestroyed;
    set => this.mData.fortWasDestroyed = value;
  }

  public override bool fortPlaneSpawned
  {
    get => this.mData.fortPlaneSpawned;
    set => this.mData.fortPlaneSpawned = value;
  }

  public override EFortState fortState
  {
    get => this.mData.fortState;
    set => this.mData.fortState = value;
  }

  public override float govLocalInfectiousness
  {
    get => this.mData.govLocalInfectiousness;
    set => this.mData.govLocalInfectiousness = value;
  }

  public override float govLocalCorpseTransmission
  {
    get => this.mData.govLocalCorpseTransmission;
    set => this.mData.govLocalCorpseTransmission = value;
  }

  public override float govLocalLethality
  {
    get => this.mData.govLocalLethality;
    set => this.mData.govLocalLethality = value;
  }

  public override float govLocalSeverity
  {
    get => this.mData.govLocalSeverity;
    set => this.mData.govLocalSeverity = value;
  }

  public override float govPublicOrder
  {
    get => this.mData.govPublicOrder;
    set => this.mData.govPublicOrder = value;
  }

  public override bool borderStatus
  {
    get => this.mData.borderStatus;
    set => this.mData.borderStatus = value;
  }

  public override bool airportStatus
  {
    get => this.mData.airportStatus;
    set => this.mData.airportStatus = value;
  }

  public override bool portStatus
  {
    get => this.mData.portStatus;
    set => this.mData.portStatus = value;
  }

  public override bool govActionsAllowed
  {
    get => this.mData.govActionsAllowed;
    set => this.mData.govActionsAllowed = value;
  }

  public override float deadPercent
  {
    get => this.mData.deadPercent;
    set => this.mData.deadPercent = value;
  }

  public override float infectedPercent
  {
    get => this.mData.infectedPercent;
    set => this.mData.infectedPercent = value;
  }

  public override float zombiePercent
  {
    get => this.mData.zombiePercent;
    set => this.mData.zombiePercent = value;
  }

  public override float healthyPercent
  {
    get => this.mData.healthyPercent;
    set => this.mData.healthyPercent = value;
  }

  public override bool hasPorts
  {
    get => this.mData.hasPorts;
    set => this.mData.hasPorts = value;
  }

  public override long totalZombie
  {
    get => this.mData.totalZombie;
    set => this.mData.totalZombie = value;
  }

  public override long totalInfected
  {
    get => this.mData.totalInfected;
    set => this.mData.totalInfected = value;
  }

  public override long healthyPopulationSusceptible
  {
    get => this.mData.healthyPopulationSusceptible;
    set => this.mData.healthyPopulationSusceptible = value;
  }

  public override long healthyPopulationImmune
  {
    get => this.mData.healthyPopulationImmune;
    set => this.mData.healthyPopulationImmune = value;
  }

  public override long apeOriginalPopulation
  {
    get => this.mData.apeOriginalPopulation;
    set => this.mData.apeOriginalPopulation = value;
  }

  public override EApeLabState apeLabStatus
  {
    get => this.mData.apeLabStatus;
    set => this.mData.apeLabStatus = value;
  }

  public override EApeColonyState apeColonyStatus
  {
    get => this.mData.apeColonyStatus;
    set => this.mData.apeColonyStatus = value;
  }

  public override int apeNumDestroyedLabs
  {
    get => this.mData.apeNumDestroyedLabs;
    set => this.mData.apeNumDestroyedLabs = value;
  }

  public override int apeNumDestroyedColonies
  {
    get => this.mData.apeNumDestroyedColonies;
    set => this.mData.apeNumDestroyedColonies = value;
  }

  public override bool apeHordeMoving
  {
    get => this.mData.apeHordeMoving;
    set => this.mData.apeHordeMoving = value;
  }

  public override float labDayCount
  {
    get => this.mData.labDayCount;
    set => this.mData.labDayCount = value;
  }

  public override float labCapacity
  {
    get => this.mData.labCapacity;
    set => this.mData.labCapacity = value;
  }

  public override float labDailyResearch
  {
    get => this.mData.labDailyResearch;
    set => this.mData.labDailyResearch = value;
  }

  public override float govLocalApeInfectiousness
  {
    get => this.mData.govLocalApeInfectiousness;
    set => this.mData.govLocalApeInfectiousness = value;
  }

  public override float govLocalApeLethality
  {
    get => this.mData.govLocalApeLethality;
    set => this.mData.govLocalApeLethality = value;
  }

  public override float localDanger
  {
    get => this.mData.localDanger;
    set => this.mData.localDanger = value;
  }

  public override int infectBoostCount
  {
    get => 0;
    set
    {
    }
  }

  public override int lethalBoostCount
  {
    get => 0;
    set
    {
    }
  }

  public override bool doubleInfected
  {
    get => false;
    set
    {
    }
  }

  public override ECoopLabState coopLabStatus
  {
    get => ECoopLabState.COOP_LAB_NONE;
    set
    {
    }
  }

  public override int coopLabDiseaseId
  {
    get => 0;
    set
    {
    }
  }

  public override bool isNuclear
  {
    get => false;
    set
    {
    }
  }

  public override float govDeadBodyTransmission
  {
    get => this.mData.govDeadBodyTransmission;
    set => this.mData.govDeadBodyTransmission = value;
  }

  public override Country.EContinentType continentType
  {
    get => this.mData.continentType;
    set => this.mData.continentType = value;
  }

  public override int presimSteps
  {
    get => this.mData.presimSteps;
    set => this.mData.presimSteps = value;
  }

  public override float startingMedicalCapacity
  {
    get => this.mData.startingMedicalCapacity;
    set => this.mData.startingMedicalCapacity = value;
  }

  public override float medicalCapacity
  {
    get => this.mData.medicalCapacity;
    set => this.mData.medicalCapacity = value;
  }

  public override float baseInfluence
  {
    get => this.mData.baseInfluence;
    set => this.mData.baseInfluence = value;
  }

  public override long healthyRecoveredPopulation
  {
    get => this.mData.healthyRecoveredPopulation;
    set => this.mData.healthyRecoveredPopulation = value;
  }

  public override float healthyRecoveredPercent
  {
    get => this.mData.healthyRecoveredPercent;
    set => this.mData.healthyRecoveredPercent = value;
  }

  public override int numNeighbours
  {
    get => this.mData.numNeighbours;
    set => this.mData.numNeighbours = value;
  }

  public override string id
  {
    get => this.mData.id;
    set => this.mData.id = value;
  }

  public static void TestLibrary()
  {
    SPCountryData c = new SPCountryData();
    SPCountryData spCountryData = new SPCountryData();
    spCountryData.totalZombie = c.totalZombie = 9912318237L;
    spCountryData.battleVictoryCount = c.battleVictoryCount = 0.5f;
    spCountryData.urban = c.urban = true;
    spCountryData.publicOrder = c.publicOrder = 0.7f;
    spCountryData.airport = c.airport = false;
    spCountryData.apeColonyStatus = c.apeColonyStatus = EApeColonyState.APE_COLONY_ALIVE;
    spCountryData.labDailyResearch = c.labDailyResearch = 124f;
    spCountryData.localDanger = c.localDanger = 20f;
    spCountryData.govDeadBodyTransmission = c.govDeadBodyTransmission = 17f;
    spCountryData.healthyRecoveredPercent = c.healthyRecoveredPercent = 0.34f;
    spCountryData.totalZombie /= 10L;
    spCountryData.battleVictoryCount += 0.1f;
    spCountryData.urban = false;
    spCountryData.publicOrder -= 0.06999f;
    spCountryData.airport = true;
    spCountryData.apeColonyStatus = EApeColonyState.APE_COLONY_DESTROYED;
    spCountryData.labDailyResearch += 19f;
    spCountryData.localDanger -= 2f;
    spCountryData.govDeadBodyTransmission += 3f;
    spCountryData.healthyRecoveredPercent += 0.1f;
    SPCountryExternal.CountryTest(c);
    if (spCountryData.totalZombie != c.totalZombie)
      Debug.LogWarning((object) ("COUNTRY TEST 1 FAIL: " + (object) spCountryData.totalZombie + " != " + (object) c.totalZombie));
    else
      Debug.Log((object) "COUNTRY TEST 1 SUCCESS");
    if ((double) spCountryData.battleVictoryCount != (double) c.battleVictoryCount)
      Debug.LogWarning((object) "COUNTRY TEST 2 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 2 SUCCESS");
    if (c.urban != spCountryData.urban)
      Debug.LogWarning((object) "COUNTRY TEST 3 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 3 SUCCESS");
    if ((double) c.publicOrder != (double) spCountryData.publicOrder)
      Debug.LogWarning((object) "COUNTRY TEST 4 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 4 SUCCESS");
    if (c.airport != spCountryData.airport)
      Debug.LogWarning((object) "COUNTRY TEST 5 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 5 SUCCESS");
    if ((double) c.labDailyResearch != (double) spCountryData.labDailyResearch)
      Debug.LogWarning((object) "COUNTRY TEST 6 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 6 SUCCESS");
    if (c.apeColonyStatus != spCountryData.apeColonyStatus)
      Debug.LogWarning((object) "COUNTRY TEST 7 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 7 SUCCESS");
    if ((double) c.localDanger != (double) spCountryData.localDanger)
      Debug.LogWarning((object) "COUNTRY TEST 8 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 8 SUCCESS");
    if ((double) c.govDeadBodyTransmission != (double) spCountryData.govDeadBodyTransmission)
      Debug.LogWarning((object) "COUNTRY TEST 9 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 9 SUCCESS");
    if ((double) c.healthyRecoveredPercent != (double) spCountryData.healthyRecoveredPercent)
      Debug.LogWarning((object) "COUNTRY TEST 10 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 10 SUCCESS");
  }

  public SPCountry() => this.mData = new SPCountryData();

  public override void Initialise()
  {
    base.Initialise();
    this.healthyPopulation = this.originalPopulation;
    this.publicOrder = 1f;
    this.govActionsAllowed = true;
    this.borderStatus = this.airportStatus = this.portStatus = true;
  }

  public override long TransferPopulationNoPipe(
    long number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0)
  {
    if (number < 0L)
    {
      int num = (int) PT_from;
      PT_from = PT_to;
      PT_to = (Country.EPopulationType) num;
      number = -number;
    }
    LocalDisease localDisease = (LocalDisease) null;
    if (acting != null)
      localDisease = acting.GetLocalDisease((Country) this);
    long num1 = number;
    long num2 = 0;
    if (num1 != 0L)
    {
      if (PT_from == Country.EPopulationType.NONE && PT_to != Country.EPopulationType.APE_DEAD && PT_to != Country.EPopulationType.APE_HEALTHY && PT_to != Country.EPopulationType.APE_INFECTED)
        this.mData.originalPopulation += num1;
      switch (PT_from)
      {
        case Country.EPopulationType.HEALTHY:
          if (num1 > this.mData.healthyPopulation)
          {
            num2 = num1 - this.mData.healthyPopulation;
            num1 = this.mData.healthyPopulation;
          }
          this.mData.healthyPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.INFECTED:
          if (num1 > localDisease.controlledInfected)
          {
            num2 = num1 - localDisease.controlledInfected;
            num1 = localDisease.controlledInfected;
          }
          localDisease.controlledInfected -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.DEAD:
          if (num1 > this.mData.deadPopulation)
          {
            num2 = num1 - this.mData.deadPopulation;
            num1 = this.mData.deadPopulation;
          }
          this.mData.deadPopulation -= num1;
          if (num1 > localDisease.killedPopulation)
          {
            localDisease.killedPopulation = 0L;
            goto case Country.EPopulationType.NONE;
          }
          else
          {
            localDisease.killedPopulation -= num1;
            goto case Country.EPopulationType.NONE;
          }
        case Country.EPopulationType.ZOMBIE:
          long num3 = num1;
          if (localDisease.zombie < num3)
            num3 = localDisease.zombie;
          localDisease.zombie -= num3;
          num2 = num1 - num3;
          num1 = num3;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.NONE:
          switch (PT_to)
          {
            case Country.EPopulationType.HEALTHY:
              this.mData.healthyPopulation += num1;
              break;
            case Country.EPopulationType.INFECTED:
              localDisease.controlledInfected += num1;
              break;
            case Country.EPopulationType.DEAD:
            case Country.EPopulationType.ZOMBIE:
              if (PT_to == Country.EPopulationType.DEAD)
              {
                this.mData.deadPopulation += num1;
                localDisease.killedPopulation += num1;
                break;
              }
              localDisease.zombie += num1;
              break;
            case Country.EPopulationType.NONE:
              if (PT_from != Country.EPopulationType.APE_DEAD && PT_from != Country.EPopulationType.APE_HEALTHY && PT_from != Country.EPopulationType.APE_INFECTED)
              {
                this.mData.originalPopulation -= num1;
                break;
              }
              break;
            case Country.EPopulationType.HEALTHY_SUSCEPTIBLE:
              this.mData.healthyPopulationSusceptible += num1;
              break;
            case Country.EPopulationType.HEALTHY_IMMUNE:
              this.mData.healthyPopulationImmune += num1;
              break;
            case Country.EPopulationType.APE_HEALTHY:
              localDisease.apeHealthyPopulation += num1;
              break;
            case Country.EPopulationType.APE_INFECTED:
              localDisease.apeInfectedPopulation += num1;
              break;
            case Country.EPopulationType.APE_DEAD:
              localDisease.apeDeadPopulation += num1;
              break;
            case Country.EPopulationType.HEALTHY_RECOVERED:
              this.mData.healthyRecoveredPopulation += num1;
              break;
            default:
              Debug.LogError((object) ("UNHANDLED 'to' POP TYPE: " + (object) PT_to));
              break;
          }
          break;
        case Country.EPopulationType.HEALTHY_SUSCEPTIBLE:
          if (num1 > this.mData.healthyPopulationSusceptible)
          {
            num2 = num1 - this.mData.healthyPopulationSusceptible;
            num1 = this.mData.healthyPopulationSusceptible;
          }
          this.mData.healthyPopulationSusceptible -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.HEALTHY_IMMUNE:
          if (num1 > this.mData.healthyPopulationImmune)
          {
            num2 = num1 - this.mData.healthyPopulationImmune;
            num1 = this.mData.healthyPopulationImmune;
          }
          this.mData.healthyPopulationImmune -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_HEALTHY:
          if (num1 > localDisease.apeHealthyPopulation)
          {
            num2 = num1 - localDisease.apeHealthyPopulation;
            num1 = localDisease.apeHealthyPopulation;
          }
          localDisease.apeHealthyPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_INFECTED:
          if (num1 > localDisease.apeInfectedPopulation)
          {
            num2 = num1 - localDisease.apeInfectedPopulation;
            num1 = localDisease.apeInfectedPopulation;
          }
          localDisease.apeInfectedPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_DEAD:
          if (num1 > localDisease.apeDeadPopulation)
          {
            num2 = num1 - localDisease.apeDeadPopulation;
            num1 = localDisease.apeDeadPopulation;
          }
          localDisease.apeDeadPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.HEALTHY_RECOVERED:
          if (num1 > this.mData.healthyRecoveredPopulation)
          {
            num2 = num1 - this.mData.healthyRecoveredPopulation;
            num1 = this.mData.healthyRecoveredPopulation;
          }
          this.mData.healthyRecoveredPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        default:
          Debug.LogError((object) ("UNHANDLED 'from' POP TYPE: " + (object) PT_from));
          goto case Country.EPopulationType.NONE;
      }
    }
    return num2;
  }

  public override double TransferPopulation(
    double number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0)
  {
    if (PT_from == Country.EPopulationType.HEALTHY && acting.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      PT_from = Country.EPopulationType.HEALTHY_SUSCEPTIBLE;
    if (number < 0.0)
    {
      int num = (int) PT_from;
      PT_from = PT_to;
      PT_to = (Country.EPopulationType) num;
      number = -number;
    }
    LocalDisease localDisease = (LocalDisease) null;
    if (acting != null)
      localDisease = acting.GetLocalDisease((Country) this);
    Country.PopulationPipe pipe = this.GetPipe(acting, PT_from, PT_to);
    pipe.Add(number);
    long num1 = (long) pipe.val;
    if (num1 > 0L)
      pipe.Add((double) -num1);
    else
      num1 = 0L;
    long num2 = 0;
    if (num1 != 0L)
    {
      if (PT_from == Country.EPopulationType.NONE && PT_to != Country.EPopulationType.APE_DEAD && PT_to != Country.EPopulationType.APE_HEALTHY && PT_to != Country.EPopulationType.APE_INFECTED)
        this.mData.originalPopulation += num1;
      switch (PT_from)
      {
        case Country.EPopulationType.HEALTHY:
          if (num1 > this.mData.healthyPopulation)
          {
            num2 = num1 - this.mData.healthyPopulation;
            num1 = this.mData.healthyPopulation;
          }
          this.mData.healthyPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.INFECTED:
          if (num1 > localDisease.controlledInfected)
          {
            num2 = num1 - localDisease.controlledInfected;
            num1 = localDisease.controlledInfected;
          }
          localDisease.controlledInfected -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.DEAD:
          if (num1 > this.mData.deadPopulation)
          {
            num2 = num1 - this.mData.deadPopulation;
            num1 = this.mData.deadPopulation;
          }
          this.mData.deadPopulation -= num1;
          if (num1 > localDisease.killedPopulation)
          {
            localDisease.killedPopulation = 0L;
            goto case Country.EPopulationType.NONE;
          }
          else
          {
            localDisease.killedPopulation -= num1;
            goto case Country.EPopulationType.NONE;
          }
        case Country.EPopulationType.ZOMBIE:
          long num3 = num1;
          if (localDisease.zombie < num3)
            num3 = localDisease.zombie;
          localDisease.zombie -= num3;
          num2 = num1 - num3;
          num1 = num3;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.NONE:
          switch (PT_to)
          {
            case Country.EPopulationType.HEALTHY:
              this.mData.healthyPopulation += num1;
              break;
            case Country.EPopulationType.INFECTED:
              localDisease.controlledInfected += num1;
              break;
            case Country.EPopulationType.DEAD:
            case Country.EPopulationType.ZOMBIE:
              if (PT_to == Country.EPopulationType.DEAD)
              {
                this.mData.deadPopulation += num1;
                localDisease.killedPopulation += num1;
                break;
              }
              localDisease.zombie += num1;
              break;
            case Country.EPopulationType.NONE:
              if (PT_from != Country.EPopulationType.APE_DEAD && PT_from != Country.EPopulationType.APE_HEALTHY && PT_from != Country.EPopulationType.APE_INFECTED)
              {
                this.mData.originalPopulation -= num1;
                break;
              }
              break;
            case Country.EPopulationType.HEALTHY_SUSCEPTIBLE:
              this.mData.healthyPopulationSusceptible += num1;
              break;
            case Country.EPopulationType.HEALTHY_IMMUNE:
              this.mData.healthyPopulationImmune += num1;
              break;
            case Country.EPopulationType.APE_HEALTHY:
              localDisease.apeHealthyPopulation += num1;
              break;
            case Country.EPopulationType.APE_INFECTED:
              localDisease.apeInfectedPopulation += num1;
              break;
            case Country.EPopulationType.APE_DEAD:
              localDisease.apeDeadPopulation += num1;
              break;
            default:
              Debug.LogError((object) ("UNHANDLED 'to' POP TYPE: " + (object) PT_to));
              break;
          }
          break;
        case Country.EPopulationType.HEALTHY_SUSCEPTIBLE:
          if (num1 > this.mData.healthyPopulationSusceptible)
          {
            num2 = num1 - this.mData.healthyPopulationSusceptible;
            num1 = this.mData.healthyPopulationSusceptible;
          }
          this.mData.healthyPopulationSusceptible -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.HEALTHY_IMMUNE:
          if (num1 > this.mData.healthyPopulationImmune)
          {
            num2 = num1 - this.mData.healthyPopulationImmune;
            num1 = this.mData.healthyPopulationImmune;
          }
          this.mData.healthyPopulationImmune -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_HEALTHY:
          if (num1 > localDisease.apeHealthyPopulation)
          {
            num2 = num1 - localDisease.apeHealthyPopulation;
            num1 = localDisease.apeHealthyPopulation;
          }
          localDisease.apeHealthyPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_INFECTED:
          if (num1 > localDisease.apeInfectedPopulation)
          {
            num2 = num1 - localDisease.apeInfectedPopulation;
            num1 = localDisease.apeInfectedPopulation;
          }
          localDisease.apeInfectedPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.APE_DEAD:
          if (num1 > localDisease.apeDeadPopulation)
          {
            num2 = num1 - localDisease.apeDeadPopulation;
            num1 = localDisease.apeDeadPopulation;
          }
          localDisease.apeDeadPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        case Country.EPopulationType.HEALTHY_RECOVERED:
          if (num1 > this.mData.healthyRecoveredPopulation)
          {
            num2 = num1 - this.mData.healthyRecoveredPopulation;
            num1 = this.mData.healthyRecoveredPopulation;
          }
          this.mData.healthyRecoveredPopulation -= num1;
          goto case Country.EPopulationType.NONE;
        default:
          Debug.LogError((object) ("UNHANDLED 'from' POP TYPE: " + (object) PT_from));
          goto case Country.EPopulationType.NONE;
      }
    }
    return (double) num2;
  }

  public override void GameUpdate()
  {
    Disease disease = World.instance.diseases[0];
    CGameManager.CallExternalMethod("CountryExternalHead", World.instance, disease, (Country) this, this.GetLocalDisease(disease));
    if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      this.GameUpdate_Vampire();
    else if (disease.diseaseType == Disease.EDiseaseType.FAKE_NEWS)
      this.GameUpdate_FakeNews();
    else if (disease.diseaseType == Disease.EDiseaseType.CURE)
    {
      this.GameUpdate_Cure();
    }
    else
    {
      this.mData.deadPercent = 1f * (float) this.totalDead / (float) this.mData.originalPopulation;
      this.mData.infectedPercent = 1f * (float) this.mData.totalInfected / (float) this.mData.originalPopulation;
      this.mData.zombiePercent = 1f * (float) this.mData.totalZombie / (float) this.mData.originalPopulation;
      this.mData.healthyPercent = 1f * (float) this.totalHealthy / (float) this.mData.originalPopulation;
      float num1 = 0.0f;
      bool flag = false;
      LocalDisease localDisease1 = this.localDiseases[0];
      float infectedPublicOrder = num1 + Mathf.Max(0.0f, localDisease1.GetPublicOrderEffect());
      if (localDisease1.disease.diseaseType == Disease.EDiseaseType.NEURAX)
        flag = true;
      SPCountryExternal.UpdatePublicOrder(this.mData, infectedPublicOrder, disease.difficultyVariable);
      if (this.airRoutes.Count > 0 && this.mData.airportStatus)
      {
        for (int index = 0; index < this.airRoutes.Count; ++index)
        {
          TravelRoute airRoute = this.airRoutes[index];
          if (!airRoute.initialised)
          {
            airRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
            airRoute.forward = ModelUtils.IntRand(0, 10) > 5;
            airRoute.initialised = true;
          }
          if (ModelUtils.IntRand(0, 100) < 80)
            airRoute.forward = !airRoute.forward;
          Country s = airRoute.forward ? airRoute.source : airRoute.destination;
          Country country = airRoute.forward ? airRoute.destination : airRoute.source;
          if (s != null && country != null && s.airportStatus && country.airportStatus)
          {
            float num2 = disease.scenario == "christmas_spirit" || disease.scenario == "board_game" ? airRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation : (float) ((double) airRoute.frequency * (double) s.currentPopulation / (double) s.originalPopulation * (flag ? 1.0 - (double) s.totalSeverity / 100.0 : 1.0));
            airRoute.currentTime += num2;
            if ((double) airRoute.currentTime >= 1.0)
            {
              --airRoute.currentTime;
              LocalDisease localDisease2 = s.GetLocalDisease(disease);
              LocalDisease localDisease3 = disease.GetLocalDisease(country);
              Vehicle vehicle = Vehicle.Create();
              vehicle.type = Vehicle.EVehicleType.Airplane;
              vehicle.subType = Vehicle.EVehicleSubType.Normal;
              vehicle.SetRoute(s, country);
              if (SPLocalDiseaseExternal.AirTransfer(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease2).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease3).mData, ((SPCountry) country).mData))
                vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
              World.instance.AddVehicle(vehicle);
            }
          }
        }
      }
      if (this.seaRoutes.Count > 0 && this.mData.portStatus)
      {
        for (int index = 0; index < this.seaRoutes.Count; ++index)
        {
          TravelRoute seaRoute = this.seaRoutes[index];
          if (!seaRoute.initialised)
          {
            seaRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
            seaRoute.forward = ModelUtils.IntRand(0, 10) > 5;
            seaRoute.initialised = true;
          }
          if (ModelUtils.IntRand(0, 100) < 80)
            seaRoute.forward = !seaRoute.forward;
          Country s = seaRoute.forward ? seaRoute.source : seaRoute.destination;
          Country country = seaRoute.forward ? seaRoute.destination : seaRoute.source;
          if (s.portStatus && country.portStatus)
          {
            float num3 = disease.scenario == "christmas_spirit" || disease.scenario == "board_game" ? seaRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation : (float) ((double) seaRoute.frequency * (double) s.currentPopulation / (double) s.originalPopulation * (flag ? 1.0 - (double) s.totalSeverity / 100.0 : 1.0));
            seaRoute.currentTime += num3;
            if ((double) seaRoute.currentTime >= 1.0)
            {
              --seaRoute.currentTime;
              Vehicle vehicle = Vehicle.Create();
              vehicle.type = Vehicle.EVehicleType.Boat;
              vehicle.subType = Vehicle.EVehicleSubType.Normal;
              vehicle.SetRoute(s, country);
              LocalDisease localDisease4 = s.GetLocalDisease(disease);
              LocalDisease localDisease5 = disease.GetLocalDisease(country);
              if (SPLocalDiseaseExternal.SeaTransfer(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease4).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease5).mData, ((SPCountry) country).mData))
                vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
              World.instance.AddVehicle(vehicle);
            }
          }
        }
      }
      this.mData.currentPopulation = this.totalInfected + this.totalZombie + this.totalHealthy;
      if (!this.mData.isDestroyed && (double) this.mData.deadPercent + (double) this.mData.zombiePercent >= 1.0)
        this.mData.isDestroyed = true;
      long num4 = 0;
      for (int index = 0; index < this.localDiseases.Count; ++index)
        num4 += this.localDiseases[index].controlledInfected;
      this.mData.totalInfected = num4;
      long num5 = 0;
      for (int index = 0; index < this.localDiseases.Count; ++index)
        num5 += this.localDiseases[index].zombie;
      this.mData.totalZombie = num5;
      LocalDisease localDisease6 = this.GetLocalDisease(World.instance.diseases[0]);
      this.myUninfected = (float) localDisease6.uninfectedPopulation;
      this.myUninfectedPercentage = (float) localDisease6.uninfectedPopulation / (float) this.originalPopulation;
      CGameManager.CallExternalMethod("CountryExternalTail", World.instance, disease, (Country) this, this.GetLocalDisease(disease));
    }
  }

  public override void VehicleArrived(Vehicle vehicle, int presetInt = -1)
  {
    if (vehicle.subType == Vehicle.EVehicleSubType.CureInvestigate)
    {
      Vampire vampire = vehicle.actingDisease.GetVampire(vehicle.vampireID);
      if (vampire != null)
      {
        vampire.currentCountry = (Country) this;
        vampire.currentPosition = vehicle.destinationPosition;
        LocalDisease localDisease = this.GetLocalDisease(vampire.actingDisease);
        localDisease.hasTeam = true;
        localDisease.hasIntel = true;
      }
      else
        Debug.LogError((object) "Could not map vehicle's payload to a team (vampire) ID.");
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.Fort)
    {
      if (this.mData.healthyPopulation + this.totalInfected > 200L && this.mData.fortState != EFortState.FORT_ALIVE)
      {
        this.mData.fortState = EFortState.FORT_ALIVE;
        World.instance.AddFort((Country) this);
      }
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        if (country.HasFort())
          country.localHumanCombatStrengthMax += 0.01f;
      }
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.FortEscapees)
    {
      if (this.fortState != EFortState.FORT_ALIVE)
        return;
      this.localHumanCombatStrengthMax += 0.01f;
      LocalDisease localDisease = this.GetLocalDisease(vehicle.actingDisease);
      localDisease.fortHealth = Mathf.Min(localDisease.fortHealth + localDisease.fortHealthMax * 0.1f, localDisease.fortHealthMax);
      float num = localDisease.fortHealth / localDisease.fortHealthMax;
      localDisease.fortHealthMax += 11f + localDisease.disease.difficultyVariable + (float) localDisease.disease.difficulty;
      localDisease.fortHealth = localDisease.fortHealthMax * num;
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.MiniFort)
    {
      int num = presetInt <= -1 ? ModelUtils.IntRand(10, 40) : presetInt;
      vehicle.actingDisease.daysUntilMinifortPlaneSpawn = num;
      this.mData.battleVictoryCount += 0.1f;
      if (CGameManager.game.IsReplayActive)
        return;
      CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.VEHICLE_PLANE_MINIFORT_ARRIVED, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) null, vehicle.id.ToString() + ":" + (object) vehicle.actingDisease.daysUntilMinifortPlaneSpawn);
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.Cure)
    {
      if (vehicle.actingDisease.superCureCountry != this)
      {
        Vehicle vehicle1 = Vehicle.Create();
        vehicle1.type = Vehicle.EVehicleType.Airplane;
        vehicle1.subType = Vehicle.EVehicleSubType.Cure;
        vehicle1.actingDisease = vehicle.actingDisease;
        vehicle1.SetRoute((Country) this, vehicle1.actingDisease.superCureCountry);
        World.instance.AddVehicle(vehicle1);
      }
      this.SpawnCureIcon(vehicle.actingDisease);
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.Neurax)
    {
      Debug.Log((object) "_____________________________________________LAND NEURAX");
      World.instance.AddAchievement(EAchievement.A_TrojanHorse);
      if (vehicle.infected == null)
        return;
      for (int index = 0; index < vehicle.infected.Length; ++index)
      {
        if (vehicle.infected[index] > 0)
        {
          Disease disease = World.instance.diseases[index];
          if (disease.scenario == "christmas_spirit")
          {
            this.TransferPopulation(ModelUtils.Max(3.0, (double) disease.trojanInfected * (double) ModelUtils.FloatRand(0.2f, 1f)), Country.EPopulationType.HEALTHY, disease, Country.EPopulationType.INFECTED, 0.0);
            if (this.poverty)
              this.govLocalInfectiousness += 2f;
            disease.evoPoints += disease.trojanDna;
          }
          else if (!CGameManager.game.IsReplayActive)
            this.ProcessNeuraxPlaneArrival(vehicle, disease);
          this.govLocalInfectiousness += (float) disease.trojanInfectiousness;
          this.govLocalLethality += (float) disease.trojanLethality;
          this.govPublicOrder += disease.trojanPublicOrder;
        }
      }
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.ApeHorde)
    {
      this.apeColonyPosition = vehicle.currentPosition;
      this.ChangeApeColonyStateF(EApeColonyState.APE_COLONY_ALIVE);
    }
    else if (vehicle.subType == Vehicle.EVehicleSubType.ApeLabPlane)
      this.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
    else if (vehicle.subType == Vehicle.EVehicleSubType.Intel)
      this.GetLocalDisease(vehicle.actingDisease).hasIntel = true;
    else if (vehicle.subType == Vehicle.EVehicleSubType.VampireHordeFast || vehicle.subType == Vehicle.EVehicleSubType.VampireHordeSlow)
    {
      this.TransferPopulation(1.0, Country.EPopulationType.NONE, vehicle.actingDisease, Country.EPopulationType.ZOMBIE, 0.0);
      ++vehicle.actingDisease.totalZombie;
      --vehicle.actingDisease.vampireHordeStash;
      Vampire vampire = vehicle.actingDisease.GetVampire(vehicle.vampireID);
      if (vampire == null)
      {
        Debug.LogError((object) ("NULL VAMPIRE! LOOKING FOR ID: " + (object) vehicle.vampireID + " in disease: " + (object) vehicle.actingDisease));
      }
      else
      {
        vampire.currentCountry = (Country) this;
        vampire.currentPosition = vehicle.destinationPosition;
        LocalDisease localDisease = this.GetLocalDisease(vehicle.actingDisease);
        if (vehicle.actingDisease.vampAutomaticBloodRage > 0 && vehicle.actingDisease.IsTechEvolved("blood_rage"))
          localDisease.consumeFlag = 1;
        localDisease.localVampireActivity += 0.5f;
      }
    }
    else if (vehicle.type == Vehicle.EVehicleType.Drone)
    {
      Debug.LogError((object) "Should never have a VehicleArrived with a DRONE - handled by DroneAttack");
    }
    else
    {
      if (vehicle.subType == Vehicle.EVehicleSubType.Blue && vehicle.actingDisease != null && vehicle.actingDisease.isCure && !CGameManager.game.IsReplayActive)
        CSoundManager.instance.PlaySFX("cure_plane_land");
      if (vehicle.zombies != null)
      {
        for (int index = 0; index < vehicle.zombies.Length; ++index)
        {
          if (vehicle.zombies[index] > 0)
          {
            Disease disease = World.instance.diseases[index];
            LocalDisease localDisease = vehicle.currentCountry.GetLocalDisease(disease);
            if (localDisease.zombie > 1L)
            {
              double num = vehicle.currentCountry.TransferPopulation((double) vehicle.zombies[index], Country.EPopulationType.ZOMBIE, disease, Country.EPopulationType.NONE);
              double number = (double) vehicle.zombies[index] - num;
              vehicle.destination.TransferPopulation(number, Country.EPopulationType.NONE, disease, Country.EPopulationType.ZOMBIE);
            }
            else
            {
              double number = 1.0 - vehicle.currentCountry.TransferPopulation(1.0, Country.EPopulationType.DEAD, disease, Country.EPopulationType.NONE);
              vehicle.destination.TransferPopulation(number, Country.EPopulationType.NONE, disease, Country.EPopulationType.ZOMBIE);
            }
            localDisease.zombieInactivityCounter = 0;
          }
        }
      }
      if (vehicle.infected == null)
        return;
      for (int index = 0; index < vehicle.infected.Length; ++index)
      {
        if (vehicle.infected[index] > 0)
        {
          Disease disease = World.instance.diseases[index];
          LocalDisease localDisease = this.GetLocalDisease(disease);
          if (disease.isCure)
          {
            if (vehicle.hiddenInfected)
            {
              if (vehicle.type == Vehicle.EVehicleType.Airplane || vehicle.type == Vehicle.EVehicleType.Helicopter)
                this.dispatchedHiddenInfectedFlights.Add(vehicle.source);
              else if (vehicle.type == Vehicle.EVehicleType.Boat)
                this.dispatchedHiddenInfectedBoats.Add(vehicle.source);
            }
            localDisease.turnVehicleArrived = disease.turnNumber;
            if (localDisease.infectedFromCountry == null)
            {
              localDisease.infectionMethod = vehicle.type != Vehicle.EVehicleType.Airplane ? Country.EInfectionMethod.IM_BOAT : Country.EInfectionMethod.IM_PLANE;
              localDisease.infectedFromCountry = vehicle.source;
            }
            if (disease.difficulty >= 3)
            {
              localDisease.contactTracingRampUp *= 0.8f;
              localDisease.ctFailureChance += 0.25f;
              localDisease.vehicleInfectionBoostChance += 5f;
            }
            else if (disease.difficulty == 2)
            {
              localDisease.contactTracingRampUp *= 0.85f;
              localDisease.ctFailureChance += 0.2f;
              localDisease.vehicleInfectionBoostChance += 4f;
            }
            else
            {
              localDisease.contactTracingRampUp *= 0.9f;
              localDisease.ctFailureChance += 0.15f;
              localDisease.vehicleInfectionBoostChance += 3f;
            }
          }
          this.TransferPopulation((double) vehicle.infected[index], Country.EPopulationType.HEALTHY, disease, Country.EPopulationType.INFECTED, 0.0);
        }
      }
    }
  }

  public void ProcessNeuraxPlaneArrival(
    Vehicle vehicle,
    Disease d,
    double presetTransferred = -1.0,
    int presetPoints = -1)
  {
    Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "].ProcessNeuraxPlaneArrival - seed:" + (object) Random.seed));
    double number = presetTransferred < 0.0 ? ModelUtils.Min((double) vehicle.source.infectedPercent * (double) ModelUtils.IntRand(5, 1000) + 4.0, (double) (vehicle.source.totalInfected / 4L)) : presetTransferred;
    Debug.Log((object) ("---transferred:" + (object) number));
    this.TransferPopulation(number, Country.EPopulationType.HEALTHY, d, Country.EPopulationType.INFECTED, 0.0);
    int num = presetPoints < 0 ? (d.difficulty < 2 ? ModelUtils.IntRand(1, 3) : ModelUtils.IntRand(1, 2)) : presetPoints;
    d.evoPoints += num;
    ++this.govLocalInfectiousness;
    if (CGameManager.game.IsReplayActive)
      return;
    Debug.Log((object) ("_________________________RECORD NEURAX_PLANE_LANDED - param:" + this.id + ":" + (object) number + ":" + (object) num));
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.NEURAX_PLANE_LANDED, World.instance.DiseaseTurn, World.instance.eventTurn, d, this.id + ":" + (object) number + ":" + (object) num);
  }

  public override float DroneAttack(Disease d, bool colonyDestroyed, float presetFloat = -1f)
  {
    LocalDisease localDisease = d.GetLocalDisease((Country) this);
    ++localDisease.droneAttackSuccessFlag;
    float num = presetFloat;
    if (colonyDestroyed)
    {
      if ((double) num < 0.0)
        num = ModelUtils.FloatRand(0.8f, 1f);
      World.instance.achievements.Add(EAchievement.A_attackofthedrones);
      if (d.difficulty < 2)
        this.TransferPopulation((double) Mathf.Max(1f, (float) localDisease.apeInfectedPopulation * 0.4f * num), Country.EPopulationType.APE_INFECTED, d, Country.EPopulationType.APE_DEAD, 0.0);
      else
        this.TransferPopulation((double) Mathf.Max(1f, (float) localDisease.apeInfectedPopulation * 0.8f * num), Country.EPopulationType.APE_INFECTED, d, Country.EPopulationType.APE_DEAD, 0.0);
    }
    else
    {
      if ((double) num < 0.0)
        num = ModelUtils.FloatRand(0.5f, 1f);
      this.TransferPopulation((double) Mathf.Max(1f, (float) localDisease.apeInfectedPopulation * 0.1f * num), Country.EPopulationType.APE_INFECTED, d, Country.EPopulationType.APE_DEAD, 0.0);
    }
    localDisease.daysSinceLocalDrone = 0;
    return num;
  }

  public void GameUpdate_Cure()
  {
    long num1 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num1 += this.localDiseases[index].controlledInfected;
    this.mData.totalInfected = num1;
    long num2 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num2 += this.localDiseases[index].zombie;
    this.mData.totalZombie = num2;
    this.currentPopulation = this.totalInfected + this.totalZombie + this.totalHealthy + this.healthyRecoveredPopulation;
    LocalDisease localDisease = this.GetLocalDisease(World.instance.diseases[0]);
    this.myUninfected = (float) localDisease.uninfectedPopulation;
    this.myUninfectedPercentage = (float) localDisease.uninfectedPopulation / (float) this.originalPopulation;
    this.mData.deadPercent = 1f * (float) this.totalDead / (float) this.mData.originalPopulation;
    this.mData.infectedPercent = 1f * (float) this.mData.totalInfected / (float) this.mData.originalPopulation;
    this.mData.zombiePercent = 1f * (float) this.mData.totalZombie / (float) this.mData.originalPopulation;
    this.mData.healthyPercent = 1f * (float) this.totalHealthy / (float) this.mData.originalPopulation;
    this.mData.healthyRecoveredPercent = 1f * (float) this.mData.healthyRecoveredPopulation / (float) this.mData.originalPopulation;
  }

  public void GameUpdate_FakeNews()
  {
    Disease disease = World.instance.diseases[0];
    this.mData.deadPercent = 1f * (float) this.totalDead / (float) this.mData.originalPopulation;
    this.mData.infectedPercent = 1f * (float) this.mData.totalInfected / (float) this.mData.originalPopulation;
    this.mData.zombiePercent = 1f * (float) this.mData.totalZombie / (float) this.mData.originalPopulation;
    this.mData.healthyPercent = 1f * (float) this.totalHealthy / (float) this.mData.originalPopulation;
    if (this.airRoutes.Count > 0 && this.mData.airportStatus)
    {
      for (int index = 0; index < this.airRoutes.Count; ++index)
      {
        TravelRoute airRoute = this.airRoutes[index];
        if (!airRoute.initialised)
        {
          airRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
          airRoute.forward = ModelUtils.IntRand(0, 10) > 5;
          airRoute.initialised = true;
        }
        if (ModelUtils.IntRand(0, 100) < 80)
          airRoute.forward = !airRoute.forward;
        Country s = airRoute.forward ? airRoute.source : airRoute.destination;
        Country country = airRoute.forward ? airRoute.destination : airRoute.source;
        if (s != null && country != null && s.airportStatus && country.airportStatus)
        {
          float num = airRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation;
          airRoute.currentTime += num;
          if ((double) airRoute.currentTime >= 1.0)
          {
            --airRoute.currentTime;
            LocalDisease localDisease1 = s.GetLocalDisease(disease);
            LocalDisease localDisease2 = disease.GetLocalDisease(country);
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Airplane;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            if (SPLocalDiseaseExternal.AirTransfer(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease1).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease2).mData, ((SPCountry) country).mData))
              vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    if (this.seaRoutes.Count > 0 && this.mData.portStatus)
    {
      for (int index = 0; index < this.seaRoutes.Count; ++index)
      {
        TravelRoute seaRoute = this.seaRoutes[index];
        if (!seaRoute.initialised)
        {
          seaRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
          seaRoute.forward = ModelUtils.IntRand(0, 10) > 5;
          seaRoute.initialised = true;
        }
        if (ModelUtils.IntRand(0, 100) < 80)
          seaRoute.forward = !seaRoute.forward;
        Country s = seaRoute.forward ? seaRoute.source : seaRoute.destination;
        Country country = seaRoute.forward ? seaRoute.destination : seaRoute.source;
        if (s.portStatus && country.portStatus)
        {
          float num = seaRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation;
          seaRoute.currentTime += num;
          if ((double) seaRoute.currentTime >= 1.0)
          {
            --seaRoute.currentTime;
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Boat;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            LocalDisease localDisease3 = s.GetLocalDisease(disease);
            LocalDisease localDisease4 = disease.GetLocalDisease(country);
            if (SPLocalDiseaseExternal.SeaTransfer(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease3).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease4).mData, ((SPCountry) country).mData))
              vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    this.mData.currentPopulation = this.totalInfected + this.totalZombie + this.totalHealthy;
    if (!this.mData.isDestroyed && (double) this.mData.deadPercent + (double) this.mData.zombiePercent >= 1.0)
      this.mData.isDestroyed = true;
    long num1 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num1 += this.localDiseases[index].controlledInfected;
    this.mData.totalInfected = num1;
    long num2 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num2 += this.localDiseases[index].zombie;
    this.mData.totalZombie = num2;
    LocalDisease localDisease = this.GetLocalDisease(World.instance.diseases[0]);
    this.myUninfected = (float) localDisease.uninfectedPopulation;
    this.myUninfectedPercentage = (float) localDisease.uninfectedPopulation / (float) this.originalPopulation;
  }

  public void GameUpdate_Vampire()
  {
    Disease disease = World.instance.diseases[0];
    this.mData.deadPercent = 1f * (float) this.totalDead / (float) this.mData.originalPopulation;
    this.mData.infectedPercent = 1f * (float) this.mData.totalInfected / (float) this.mData.originalPopulation;
    this.mData.zombiePercent = 1f * (float) this.mData.totalZombie / (float) this.mData.originalPopulation;
    this.mData.healthyPercent = 1f * (float) this.totalHealthy / (float) this.mData.originalPopulation;
    float num1 = 0.0f;
    LocalDisease localDisease = this.localDiseases[0];
    float num2 = num1 + Mathf.Max(0.0f, localDisease.GetPublicOrderEffect());
    if (disease.shadowDayDone)
      num2 = Mathf.Max(num2, localDisease.infectedPercent * 1.2f);
    SPCountryExternal.UpdatePublicOrder(this.mData, num2, disease.difficultyVariable);
  }

  public void TransmissionUpdate_Vampire()
  {
    Disease disease = World.instance.diseases[0];
    if (this.airRoutes.Count > 0 && this.mData.airportStatus)
    {
      for (int index = 0; index < this.airRoutes.Count; ++index)
      {
        TravelRoute airRoute = this.airRoutes[index];
        if (!airRoute.initialised)
        {
          airRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
          airRoute.forward = ModelUtils.IntRand(0, 10) > 5;
          airRoute.initialised = true;
        }
        if (ModelUtils.IntRand(0, 100) < 80)
          airRoute.forward = !airRoute.forward;
        Country s = airRoute.forward ? airRoute.source : airRoute.destination;
        Country country = airRoute.forward ? airRoute.destination : airRoute.source;
        if (s != null && country != null && s.airportStatus && country.airportStatus)
        {
          float num = airRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation;
          airRoute.currentTime += num;
          if ((double) airRoute.currentTime >= 1.0)
          {
            --airRoute.currentTime;
            LocalDisease localDisease1 = s.GetLocalDisease(disease);
            LocalDisease localDisease2 = disease.GetLocalDisease(country);
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Airplane;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            if (SPLocalDiseaseExternal.AirTransfer_Vampire(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease1).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease2).mData, ((SPCountry) country).mData))
              vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    if (this.seaRoutes.Count > 0 && this.mData.portStatus)
    {
      for (int index = 0; index < this.seaRoutes.Count; ++index)
      {
        TravelRoute seaRoute = this.seaRoutes[index];
        if (!seaRoute.initialised)
        {
          seaRoute.currentTime = ModelUtils.FloatRand(-1f, 1f);
          seaRoute.forward = ModelUtils.IntRand(0, 10) > 5;
          seaRoute.initialised = true;
        }
        if (ModelUtils.IntRand(0, 100) < 80)
          seaRoute.forward = !seaRoute.forward;
        Country s = seaRoute.forward ? seaRoute.source : seaRoute.destination;
        Country country = seaRoute.forward ? seaRoute.destination : seaRoute.source;
        if (s.portStatus && country.portStatus)
        {
          float num = seaRoute.frequency * (float) s.currentPopulation / (float) s.originalPopulation;
          seaRoute.currentTime += num;
          if ((double) seaRoute.currentTime >= 1.0)
          {
            --seaRoute.currentTime;
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Boat;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            LocalDisease localDisease3 = s.GetLocalDisease(disease);
            LocalDisease localDisease4 = disease.GetLocalDisease(country);
            if (SPLocalDiseaseExternal.SeaTransfer_Vampire(((SPDisease) disease).diseaseData, ((SPLocalDisease) localDisease3).mData, ((SPCountry) s).mData, ((SPLocalDisease) localDisease4).mData, ((SPCountry) country).mData))
              vehicle.AddInfected(disease, ModelUtils.IntRand(1, 10));
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    this.mData.currentPopulation = this.totalInfected + this.totalZombie + this.totalHealthy;
    if (!this.mData.isDestroyed && (double) this.mData.deadPercent + (double) this.mData.zombiePercent >= 1.0)
      this.mData.isDestroyed = true;
    long num1 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num1 += this.localDiseases[index].controlledInfected;
    this.mData.totalInfected = num1;
    long num2 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num2 += this.localDiseases[index].zombie;
    this.mData.totalZombie = num2;
    LocalDisease localDisease = this.GetLocalDisease(World.instance.diseases[0]);
    this.myUninfected = (float) localDisease.uninfectedPopulation;
    this.myUninfectedPercentage = (float) localDisease.uninfectedPopulation / (float) this.originalPopulation;
  }
}
