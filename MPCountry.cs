// Decompiled with JetBrains decompiler
// Type: MPCountry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MPCountry : Country
{
  public List<GemEffect> activeGemEffects = new List<GemEffect>();
  public MPCountryData mData;
  private int ticksToNexusDnaSpawnTime;
  private Vehicle landedVehicle;

  public override int countryNumber
  {
    get => this.mData.countryNumber;
    set => this.mData.countryNumber = value;
  }

  public override float closedBorderOpacity
  {
    get => (float) this.mData.closedBorderOpacity;
    set => this.mData.closedBorderOpacity = (double) value;
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
    get => (float) this.mData.publicOrder;
    set => this.mData.publicOrder = (double) value;
  }

  public override float importance
  {
    get => (float) this.mData.importance;
    set => this.mData.importance = (double) value;
  }

  public override float medicalBudget
  {
    get => (float) this.mData.medicalBudget;
    set => this.mData.medicalBudget = (double) value;
  }

  public override float basePopulationDensity
  {
    get => (float) this.mData.basePopulationDensity;
    set => this.mData.basePopulationDensity = (double) value;
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
    get => (float) this.mData.localHumanCombatStrength;
    set => this.mData.localHumanCombatStrength = (double) value;
  }

  public override float localHumanCombatStrengthMax
  {
    get => (float) this.mData.localHumanCombatStrengthMax;
    set => this.mData.localHumanCombatStrengthMax = (double) value;
  }

  public override float battleVictoryCount
  {
    get => (float) this.mData.battleVictoryCount;
    set => this.mData.battleVictoryCount = (double) value;
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
    get => (float) this.mData.govLocalInfectiousness;
    set => this.mData.govLocalInfectiousness = (double) value;
  }

  public override float govLocalCorpseTransmission
  {
    get => (float) this.mData.govLocalCorpseTransmission;
    set => this.mData.govLocalCorpseTransmission = (double) value;
  }

  public override float govLocalLethality
  {
    get => (float) this.mData.govLocalLethality;
    set => this.mData.govLocalLethality = (double) value;
  }

  public override float govLocalSeverity
  {
    get => (float) this.mData.govLocalSeverity;
    set => this.mData.govLocalSeverity = (double) value;
  }

  public override float govPublicOrder
  {
    get => (float) this.mData.govPublicOrder;
    set => this.mData.govPublicOrder = (double) value;
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
    get => (float) this.mData.deadPercent;
    set => this.mData.deadPercent = (double) value;
  }

  public override float infectedPercent
  {
    get => (float) this.mData.infectedPercent;
    set => this.mData.infectedPercent = (double) value;
  }

  public override float zombiePercent
  {
    get => (float) this.mData.zombiePercent;
    set => this.mData.zombiePercent = (double) value;
  }

  public override float healthyPercent
  {
    get => (float) this.mData.healthyPercent;
    set => this.mData.healthyPercent = (double) value;
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
    get => (float) this.mData.labDayCount;
    set => this.mData.labDayCount = (double) value;
  }

  public override float labCapacity
  {
    get => (float) this.mData.labCapacity;
    set => this.mData.labCapacity = (double) value;
  }

  public override float labDailyResearch
  {
    get => (float) this.mData.labDailyResearch;
    set => this.mData.labDailyResearch = (double) value;
  }

  public override float govLocalApeInfectiousness
  {
    get => (float) this.mData.govLocalApeInfectiousness;
    set => this.mData.govLocalApeInfectiousness = (double) value;
  }

  public override float govLocalApeLethality
  {
    get => (float) this.mData.govLocalApeLethality;
    set => this.mData.govLocalApeLethality = (double) value;
  }

  public override float localDanger
  {
    get => (float) this.mData.localDanger;
    set => this.mData.localDanger = (double) value;
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
    get => this.mData.isNuclear;
    set => this.mData.isNuclear = value;
  }

  public override string id
  {
    get => this.mData.id;
    set => this.mData.id = value;
  }

  public bool isAnyNexus
  {
    get => this.diseaseNexus != null;
    set
    {
    }
  }

  public void AddGem(GemEffect effect) => this.activeGemEffects.Add(effect);

  public void RemoveGem(GemEffect effect) => this.activeGemEffects.Remove(effect);

  public static void TestLibrary()
  {
    MPCountryData c = new MPCountryData();
    MPCountryData mpCountryData = new MPCountryData();
    mpCountryData.id = c.id = "wibble";
    mpCountryData.totalZombie = c.totalZombie = 9912318237L;
    mpCountryData.battleVictoryCount = c.battleVictoryCount = 0.5;
    mpCountryData.urban = c.urban = true;
    mpCountryData.publicOrder = c.publicOrder = 0.699999988079071;
    mpCountryData.airport = c.airport = false;
    mpCountryData.apeColonyStatus = c.apeColonyStatus = EApeColonyState.APE_COLONY_ALIVE;
    mpCountryData.labDailyResearch = c.labDailyResearch = 124.0;
    mpCountryData.localDanger = c.localDanger = 20.0;
    mpCountryData.totalZombie /= 10L;
    mpCountryData.battleVictoryCount += 0.10000000149011612;
    mpCountryData.urban = false;
    mpCountryData.publicOrder -= 0.069990001618862152;
    mpCountryData.airport = true;
    mpCountryData.apeColonyStatus = EApeColonyState.APE_COLONY_DESTROYED;
    mpCountryData.labDailyResearch += 19.0;
    mpCountryData.localDanger -= 2.0;
    MPCountryExternal.CountryTest(c);
    if (mpCountryData.totalZombie != c.totalZombie)
      Debug.LogWarning((object) ("COUNTRY TEST 1 FAIL: " + (object) mpCountryData.totalZombie + " != " + (object) c.totalZombie));
    else
      Debug.Log((object) "COUNTRY TEST 1 SUCCESS");
    if (mpCountryData.battleVictoryCount != c.battleVictoryCount)
      Debug.LogWarning((object) "COUNTRY TEST 2 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 2 SUCCESS");
    if (c.urban != mpCountryData.urban)
      Debug.LogWarning((object) "COUNTRY TEST 3 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 3 SUCCESS");
    if (c.publicOrder != mpCountryData.publicOrder)
      Debug.LogWarning((object) "COUNTRY TEST 4 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 4 SUCCESS");
    if (c.airport != mpCountryData.airport)
      Debug.LogWarning((object) "COUNTRY TEST 5 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 5 SUCCESS");
    if (c.labDailyResearch != mpCountryData.labDailyResearch)
      Debug.LogWarning((object) "COUNTRY TEST 6 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 6 SUCCESS");
    if (c.apeColonyStatus != mpCountryData.apeColonyStatus)
      Debug.LogWarning((object) "COUNTRY TEST 7 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 7 SUCCESS");
    if (c.localDanger != mpCountryData.localDanger)
      Debug.LogWarning((object) "COUNTRY TEST 8 FAIL");
    else
      Debug.Log((object) "COUNTRY TEST 8 SUCCESS");
  }

  public MPCountry() => this.mData = new MPCountryData();

  public override void Initialise()
  {
    base.Initialise();
    this.localDiseases.Clear();
    this.mData.healthyPopulation = this.mData.originalPopulation;
    this.mData.publicOrder = 1.0;
    this.mData.govActionsAllowed = true;
    this.mData.borderStatus = this.mData.airportStatus = this.mData.portStatus = true;
  }

  public void TransferAllInfected(Disease acting, Disease target)
  {
    LocalDisease localDisease1 = this.GetLocalDisease(acting);
    LocalDisease localDisease2 = this.GetLocalDisease(target);
    localDisease2.controlledInfected += localDisease1.controlledInfected;
    localDisease2.uncontrolledInfected = 0L;
    localDisease1.controlledInfected = 0L;
    localDisease1.uncontrolledInfected = 0L;
  }

  public override double TransferPopulation(
    double number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0)
  {
    return ((MPWorld) World.instance).allowMultipleInfections ? this.TransferPopulationMultipleInfection(number, PT_from, acting, PT_to, defenderKillAttribution) : this.TransferPopulationSingleInfection(number, PT_from, acting, PT_to);
  }

  public double TransferPopulationSingleInfection(
    double number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to)
  {
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
    if (PT_to == Country.EPopulationType.INFECTED && localDisease.allInfected == 0L && number > 0.0)
      Debug.Log((object) (acting.name + " just transferred: " + (object) number + " in " + this.id));
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
      if (PT_from == Country.EPopulationType.NONE)
        this.mData.originalPopulation += num1;
      if (PT_from == Country.EPopulationType.HEALTHY)
      {
        if (num1 > this.mData.healthyPopulation)
        {
          num2 = num1 - this.mData.healthyPopulation;
          num1 = this.mData.healthyPopulation;
        }
        this.mData.healthyPopulation -= num1;
      }
      else if (PT_from == Country.EPopulationType.DEAD)
      {
        if (num1 > this.mData.deadPopulation)
        {
          num2 = num1 - this.mData.deadPopulation;
          num1 = this.mData.deadPopulation;
        }
        this.mData.deadPopulation -= num1;
        if (num1 > localDisease.killedPopulation)
          localDisease.killedPopulation = 0L;
        else
          localDisease.killedPopulation -= num1;
      }
      else if (PT_from == Country.EPopulationType.INFECTED)
      {
        if (num1 > localDisease.controlledInfected)
        {
          num2 = num1 - localDisease.controlledInfected;
          num1 = localDisease.controlledInfected;
        }
        localDisease.controlledInfected -= num1;
      }
      else if (PT_from != Country.EPopulationType.NONE)
        Debug.LogError((object) ("UNHANDLED 'from' POP TYPE: " + (object) PT_from));
      switch (PT_to)
      {
        case Country.EPopulationType.HEALTHY:
          this.mData.healthyPopulation += num1;
          break;
        case Country.EPopulationType.INFECTED:
          localDisease.controlledInfected += num1;
          break;
        case Country.EPopulationType.DEAD:
          this.mData.deadPopulation += num1;
          localDisease.killedPopulation += num1;
          break;
        case Country.EPopulationType.NONE:
          this.mData.originalPopulation -= num1;
          break;
        default:
          Debug.LogError((object) ("UNHANDLED 'to' POP TYPE: " + (object) PT_to));
          break;
      }
    }
    return (double) num2;
  }

  public double TransferPopulationMultipleInfection(
    double number,
    Country.EPopulationType PT_from,
    Disease acting,
    Country.EPopulationType PT_to,
    double defenderKillAttribution = 0.0)
  {
    if (number < 0.0)
    {
      int num = (int) PT_from;
      PT_from = PT_to;
      PT_to = (Country.EPopulationType) num;
      number = -number;
    }
    MPDisease d1 = acting as MPDisease;
    MPLocalDisease localDisease1 = (MPLocalDisease) this.GetLocalDisease((Disease) d1);
    MPDisease d2 = World.instance.diseases.Find((Predicate<Disease>) (a => a != acting)) as MPDisease;
    MPLocalDisease localDisease2 = (MPLocalDisease) this.GetLocalDisease((Disease) d2);
    Country.PopulationPipe pipe = this.GetPipe(acting, PT_from, PT_to);
    pipe.Add(number);
    long num1 = (long) pipe.val;
    if (num1 > 0L)
      pipe.Add((double) -num1);
    else
      num1 = 0L;
    if (num1 == 0L)
      return 0.0;
    long num2 = 0;
    long num3 = 0;
    long num4 = 0;
    long num5 = 0;
    long num6 = 0;
    double num7 = 0.0;
    long controlledInfected1 = localDisease1.controlledInfected;
    long uncontrolledInfected1 = localDisease1.uncontrolledInfected;
    if (PT_from == Country.EPopulationType.NONE)
      this.originalPopulation += num1;
    long num8;
    if (PT_from == Country.EPopulationType.HEALTHY)
    {
      if (PT_to == Country.EPopulationType.INFECTED && localDisease1.immuneShockCounter > 0)
      {
        float num9 = localDisease2.controlledInfected + localDisease2.uncontrolledInfected > 0L ? 4f : 1.7f;
        num1 = ModelUtils.RoundToLong((double) num1 * (double) num9);
      }
      if (num1 > localDisease1.uninfectedPopulation)
        num1 = localDisease1.uninfectedPopulation;
      if (PT_to == Country.EPopulationType.INFECTED)
      {
        double num10 = 0.0;
        if (localDisease1.uninfectedPopulation > 0L)
        {
          num10 = (double) this.healthyPopulation / (double) localDisease1.uninfectedPopulation;
          if (num10 > 1.0)
            num10 = 1.0;
          if (num10 < 0.0)
            num10 = 0.0;
        }
        num6 = ModelUtils.RoundToLong((double) num1 * num10);
        if (num6 > num1)
          num6 = num1;
        if (num6 > this.healthyPopulation)
        {
          num2 = num6 - this.healthyPopulation;
          num6 = this.healthyPopulation;
        }
        num3 = num1 - num6;
        if (num3 > 0L)
        {
          long num11 = localDisease2.controlledInfected - localDisease1.uncontrolledInfected;
          if (num11 < 0L)
            num11 = 0L;
          if (num3 > num11)
          {
            num2 = num3 - num11;
            num3 = num11;
          }
          float num12 = d1.NonControlInfectAttack + localDisease1.LocalNonControlInfectAttack - d2.ControlInfectDefence;
          num3 = ModelUtils.RoundToLong((double) num3 * (double) Mathf.Clamp(d1.NonControlInfectMod + ((double) num12 > 0.0 ? 2f : 1f) * num12, 0.01f, 10f));
          if (num3 < 0L)
          {
            Debug.LogError((object) (this.name + " negative uncontrolled change: " + (object) num3 + " after attack"));
            num3 = 0L;
          }
          if (num3 > num11)
            num3 = num11;
        }
        if (this.healthyPopulation - num6 < 0L)
          Debug.LogError((object) (this.name + " NEGATIVE UNINFECTED?"));
        this.healthyPopulation -= num6;
      }
      else
      {
        num6 = num1;
        if (num6 > this.healthyPopulation)
        {
          num2 = num6 - this.healthyPopulation;
          num6 = this.healthyPopulation;
        }
        if (this.healthyPopulation - num6 < 0L)
        {
          Debug.LogError((object) (this.name + " NEGATIVE UNINFECTED?"));
          this.healthyPopulation = 0L;
        }
        else
          this.healthyPopulation -= num6;
      }
    }
    else if (PT_from == Country.EPopulationType.INFECTED)
    {
      if (num1 > localDisease1.uncontrolledInfected + localDisease1.controlledInfected)
        num1 = localDisease1.uncontrolledInfected + localDisease1.controlledInfected;
      if (localDisease1.uncontrolledInfected + localDisease1.controlledInfected > 0L)
      {
        num7 = (double) localDisease1.uncontrolledInfected / (double) (localDisease1.uncontrolledInfected + localDisease1.controlledInfected);
        if (num7 > 1.0)
          num7 = 1.0;
        if (num7 < 0.0)
          num7 = 0.0;
      }
      num3 = ModelUtils.RoundToLong((double) num1 * num7);
      num4 = num1 - num3;
      num2 = 0L;
      if (num3 > localDisease1.uncontrolledInfected)
      {
        num2 = num3 - localDisease1.uncontrolledInfected;
        num3 = localDisease1.uncontrolledInfected;
      }
      if (num4 > localDisease1.controlledInfected)
        num4 = localDisease1.controlledInfected;
      if (num4 + num5 > localDisease1.controlledInfected)
        num5 = localDisease1.controlledInfected - num4;
      num8 = 0L;
    }
    else if (PT_from == Country.EPopulationType.INFECTED_SHARED)
    {
      if (num1 > localDisease1.uncontrolledInfected + localDisease1.controlledInfected)
        num1 = localDisease1.uncontrolledInfected + localDisease1.controlledInfected;
      long num13 = localDisease1.uncontrolledInfected + localDisease2.uncontrolledInfected;
      if (num1 > num13)
        num1 = num13;
      long uncontrolledInfected2 = localDisease2.uncontrolledInfected;
      double num14 = 1.0;
      if (num13 > 0L)
        num14 = (double) localDisease1.uncontrolledInfected / (double) num13;
      long uncontrolledInfected3 = ModelUtils.RoundToLong((double) num1 * num14);
      if (uncontrolledInfected3 > localDisease1.uncontrolledInfected)
        uncontrolledInfected3 = localDisease1.uncontrolledInfected;
      num4 = num1 - uncontrolledInfected3;
      if (num4 > uncontrolledInfected2)
      {
        num2 = num4 - uncontrolledInfected2;
        num4 = uncontrolledInfected2;
      }
      num3 = uncontrolledInfected3 + num2;
      num2 = 0L;
      if (num3 > localDisease1.uncontrolledInfected)
      {
        num2 = num3 - localDisease1.uncontrolledInfected;
        num3 = localDisease1.uncontrolledInfected;
      }
      num8 = 0L;
    }
    else if (PT_from == Country.EPopulationType.DEAD)
    {
      if (num1 > this.deadPopulation)
      {
        num2 = num1 - this.deadPopulation;
        num1 = this.deadPopulation;
      }
      this.deadPopulation -= num1;
      if (num1 > localDisease1.killedPopulation)
        num1 = localDisease1.killedPopulation;
      localDisease1.killedPopulation -= num1;
      num6 = num1;
    }
    else if (PT_from != Country.EPopulationType.NONE)
      Debug.LogError((object) ("UNHANDLED 'from' POP TYPE: " + (object) PT_from));
    long num15 = num6 + num4 + num3;
    switch (PT_to)
    {
      case Country.EPopulationType.HEALTHY:
        if (PT_from == Country.EPopulationType.INFECTED || PT_from == Country.EPopulationType.INFECTED_SHARED)
        {
          long num16 = num4;
          if (num4 > 0L && localDisease1.controlledInfected > 0L)
          {
            for (int index = 0; index < this.localDiseases.Count; ++index)
            {
              LocalDisease localDisease3 = this.localDiseases[index];
              if (localDisease3 != localDisease1)
              {
                long num17 = num16;
                if (num17 > localDisease3.uncontrolledInfected)
                  num17 = localDisease3.uncontrolledInfected;
                localDisease3.controlledInfected += num17;
                localDisease3.uncontrolledInfected -= num17;
                num16 -= num17;
              }
            }
          }
          localDisease1.controlledInfected -= num4;
          localDisease1.uncontrolledInfected -= num3;
          if (num4 != 0L)
            ;
          if (num16 > 0L)
          {
            this.healthyPopulation += num16;
            break;
          }
          break;
        }
        break;
      case Country.EPopulationType.INFECTED:
        if (num3 < 0L)
          Debug.LogError((object) (this.name + " uncontrolled change < 0 attacker: " + localDisease1.disease.name + " fROM: " + (object) PT_from + " to " + (object) PT_to + " number:" + (object) number));
        localDisease1.controlledInfected += num6;
        localDisease1.uncontrolledInfected += num3;
        break;
      case Country.EPopulationType.DEAD:
        if ((PT_from == Country.EPopulationType.INFECTED || PT_from == Country.EPopulationType.INFECTED_SHARED) && num15 > 0L)
        {
          if (World.instance.diseases.Count > 2)
            Debug.LogError((object) "ERROR! Control/Uncontrolled infection population transfer not supported for >2 players. Refactor!");
          long controlledInfected2 = localDisease1.controlledInfected;
          localDisease1.controlledInfected -= num4;
          localDisease1.uncontrolledInfected -= num3;
          if (this.localDiseases.Count > 1)
          {
            if (num5 > localDisease2.uncontrolledInfected)
              num5 = localDisease2.uncontrolledInfected;
            localDisease2.uncontrolledInfected -= num5;
            long num18 = num3;
            if (num18 > localDisease2.controlledInfected)
              num18 = localDisease2.controlledInfected;
            localDisease2.controlledInfected -= num18;
            long num19 = num4;
            if (localDisease2.uncontrolledInfected < controlledInfected2 && PT_from != Country.EPopulationType.INFECTED_SHARED)
              num19 = (long) ((double) (num19 * localDisease2.uncontrolledInfected) * 1.0 / (double) controlledInfected2);
            if (num19 > localDisease2.uncontrolledInfected)
            {
              Debug.LogError((object) ("Wanted to remove " + (object) num19 + " uncontrolled from " + (object) localDisease2.diseaseID + " but only " + (object) localDisease2.uncontrolledInfected + " left"));
              num19 = localDisease2.uncontrolledInfected;
            }
            localDisease2.uncontrolledInfected -= num19;
            long num20 = (long) (defenderKillAttribution * (double) num15);
            if (defenderKillAttribution > 0.0)
              localDisease2.killedPopulation += num20;
            if (localDisease2.uncontrolledInfected > localDisease1.controlledInfected)
              Debug.LogError((object) (this.name + " defender has more uncontrolled than attacker has controlled: " + (object) localDisease2.uncontrolledInfected + " UC " + (object) localDisease1.controlledInfected + " transferring :" + (object) PT_from + " to " + (object) PT_to + " num: " + (object) number + " attacker: " + localDisease1.disease.name));
          }
        }
        if (num15 < 0L)
          Debug.LogError((object) ("NEGATIVE ALL CHANGED FOR DEAD? " + (object) num15 + " controlled: " + (object) num4 + " shared: " + (object) num5 + " uncontrolled: " + (object) num3 + " FROM: " + (object) PT_from + " to " + (object) PT_to + " number: " + (object) number));
        this.deadPopulation += num15;
        long num21 = (long) (defenderKillAttribution * (double) num15);
        localDisease1.killedPopulation += num15 - num21;
        break;
      case Country.EPopulationType.NONE:
        this.originalPopulation -= num15;
        break;
      default:
        Debug.LogError((object) ("UNHANDLED 'to' POP TYPE: " + (object) PT_to));
        break;
    }
    return (double) num2;
  }

  public double TransferInfected(double number, Disease acting)
  {
    MPLocalDisease localDisease1 = (MPLocalDisease) this.GetLocalDisease((Disease) (acting as MPDisease));
    Country.PopulationPipe pipe = this.GetPipe(acting, Country.EPopulationType.HEALTHY, Country.EPopulationType.INFECTED);
    pipe.Add(number);
    long num1 = (long) pipe.val;
    if (num1 > 0L)
      pipe.Add((double) -num1);
    else
      num1 = 0L;
    long num2 = 0;
    if (num1 > localDisease1.uninfectedPopulation)
    {
      num2 = num1 - localDisease1.uninfectedPopulation;
      num1 = localDisease1.uninfectedPopulation;
    }
    double num3 = 0.0;
    if (localDisease1.uninfectedPopulation > 0L)
    {
      num3 = (double) this.healthyPopulation / (double) localDisease1.uninfectedPopulation;
      if (num3 > 1.0)
        num3 = 1.0;
      if (num3 < 0.0)
        num3 = 0.0;
    }
    long num4 = ModelUtils.RoundToLong(num3 * (double) num1);
    if (num4 > num1)
      num4 = num1;
    long num5 = num1 - num4;
    if (this.healthyPopulation - num4 < 0L)
    {
      Debug.Log((object) "Blah");
      this.healthyPopulation = 0L;
    }
    else
      this.healthyPopulation -= num4;
    MPLocalDisease localDisease2 = (MPLocalDisease) this.GetLocalDisease((Disease) (World.instance.diseases.Find((Predicate<Disease>) (a => a != acting)) as MPDisease));
    if (localDisease2 != null)
    {
      if (num5 + localDisease1.uncontrolledInfected > localDisease2.controlledInfected)
        num5 = localDisease2.controlledInfected - localDisease1.uncontrolledInfected;
    }
    else
      num5 = 0L;
    localDisease1.controlledInfected += num4;
    localDisease1.uncontrolledInfected += num5;
    return (double) num2;
  }

  public void CountryLethalAttack(double killed, Disease acting)
  {
    MPDisease d1 = acting as MPDisease;
    MPLocalDisease localDisease = (MPLocalDisease) this.GetLocalDisease((Disease) d1);
    MPDisease d2 = World.instance.GetDisease(0) == acting ? (MPDisease) World.instance.GetDisease(1) : (MPDisease) World.instance.GetDisease(0);
    long num1 = localDisease.uncontrolledInfected + this.GetLocalDisease((Disease) d2).uncontrolledInfected;
    double num2 = 0.0;
    if (this.totalInfected > 0L)
      num2 = (double) num1 / (double) this.totalInfected;
    long num3 = (long) (killed * num2);
    float num4 = d1.NonControlLethalAttack - d2.ControlLethalDefence;
    long number = ModelUtils.RoundToLong((double) num3 * (double) Mathf.Clamp(d1.NonControlLethalMod + ((double) num4 > 0.0 ? 2f : 1f) * num4, 0.01f, 10f));
    if (number <= 0L)
      return;
    this.TransferPopulationMultipleInfection((double) number, Country.EPopulationType.INFECTED_SHARED, acting, Country.EPopulationType.DEAD);
  }

  public void CountryImmuneShock(Disease acting)
  {
    float number = (float) (10.0 * (double) Mathf.Pow((float) ((MPLocalDisease) this.GetLocalDisease(acting)).immuneShockCounter, 3f) * 0.25) * Mathf.Max(1f, Mathf.Pow(acting.globalLethalityMax, 2f)) * ModelUtils.FloatRand(0.9f, 1.1f);
    if ((double) number <= 0.0)
      return;
    float defenderKillAttribution = ModelUtils.FloatRand(0.45f, 0.55f);
    this.TransferPopulationMultipleInfection((double) number, Country.EPopulationType.INFECTED_SHARED, acting, Country.EPopulationType.DEAD, (double) defenderKillAttribution);
  }

  public void CountryBenignMimic(Disease acting)
  {
    MPLocalDisease localDisease1 = (MPLocalDisease) this.GetLocalDisease(acting);
    if (localDisease1.benignMimicCounter > 0)
    {
      localDisease1.localCureResearch = 0.0f;
    }
    else
    {
      if ((double) this.deadPercent >= 1.0)
        return;
      for (int index = 0; index < this.localDiseases.Count; ++index)
      {
        MPLocalDisease localDisease2 = (MPLocalDisease) this.localDiseases[index];
        if (localDisease1 != localDisease2 && localDisease2.benignMimicCounter > 0)
        {
          float a = (float) (localDisease2.controlledInfected - localDisease1.uncontrolledInfected) / (float) this.originalPopulation;
          MPDisease disease = localDisease2.disease as MPDisease;
          float num1 = (float) ((double) this.medicalBudget * (double) (Mathf.Lerp(0.4f, 1f, (float) ((disease.benignCounterMax - localDisease2.benignMimicCounter) / disease.benignCounterMax)) * Mathf.Clamp(Mathf.Max(acting.globalPriority, disease.globalPriority) / 50f, 0.1f, 2f)) * (double) ModelUtils.FloatRand(9f, 10f) * (double) Mathf.Max(a, 0.7f) * ((double) Mathf.Pow((float) localDisease2.benignMimicCount, 1.7f) + 1.0)) * Mathf.Max(this.govPublicOrder, 0.3f);
          float num2 = localDisease1.disease.cureRequirements * 0.04f;
          if ((double) num1 > (double) num2)
            num1 = num2;
          localDisease1.localCureResearch += num1;
          break;
        }
      }
    }
  }

  public void BenignMimic(MPLocalDisease actingLD)
  {
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      this.localDiseases[index].cureResearchAllocation += 0.1f;
      if (this.localDiseases[index] != actingLD)
      {
        ((MPLocalDisease) this.localDiseases[index]).LocalNonControlInfectAttack += 4f;
        if ((double) this.localDiseases[index].infectedPercent < 0.25)
        {
          ((MPLocalDisease) this.localDiseases[index]).NonControlLethalAttack -= 0.8f;
          ++((MPLocalDisease) this.localDiseases[index]).bmLethalReductionsApplied;
        }
      }
    }
  }

  public void BenignMimicEnd(MPLocalDisease actingLD)
  {
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      if (this.localDiseases[index] != actingLD)
      {
        ((MPLocalDisease) this.localDiseases[index]).LocalNonControlInfectAttack -= 4f;
        if (((MPLocalDisease) this.localDiseases[index]).bmLethalReductionsApplied > 0)
        {
          ((MPLocalDisease) this.localDiseases[index]).NonControlLethalAttack += 0.8f;
          --((MPLocalDisease) this.localDiseases[index]).bmLethalReductionsApplied;
        }
      }
    }
  }

  public override void GameUpdate()
  {
    if (this.diseaseNexus != null)
    {
      --this.ticksToNexusDnaSpawnTime;
      if (this.ticksToNexusDnaSpawnTime <= 0)
      {
        if (this.ticksToNexusDnaSpawnTime == 0)
          (World.instance as MPWorld).TryCreateNexusDNABubble(this.diseaseNexus, (Country) this);
        this.ticksToNexusDnaSpawnTime = ModelUtils.IntRand(20, 40);
      }
    }
    this.deadPercent = 1f * (float) this.totalDead / (float) this.originalPopulation;
    this.infectedPercent = 1f * (float) this.totalInfected / (float) this.originalPopulation;
    this.zombiePercent = 1f * (float) this.totalZombie / (float) this.originalPopulation;
    this.healthyPercent = 1f * (float) this.totalHealthy / (float) this.originalPopulation;
    double infectedPublicOrder = 0.0;
    bool flag = false;
    for (int index = 0; index < this.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.localDiseases[index];
      infectedPublicOrder += ModelUtils.Max(0.0, (double) localDisease.GetPublicOrderEffect());
      if (localDisease.disease.diseaseType == Disease.EDiseaseType.NEURAX)
        flag = true;
    }
    MPCountryExternal.UpdatePublicOrder(this.mData, infectedPublicOrder, (double) World.instance.diseases[0].difficultyVariable);
    if (this.airRoutes.Count > 0 && this.airportStatus)
    {
      for (int index1 = 0; index1 < this.airRoutes.Count; ++index1)
      {
        TravelRoute airRoute = this.airRoutes[index1];
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
          float num = (float) ((double) airRoute.frequency * (double) s.currentPopulation / (double) s.originalPopulation * (flag ? 1.0 - (double) s.totalSeverity / 100.0 : 1.0));
          airRoute.currentTime += num;
          if ((double) airRoute.currentTime >= 1.0)
          {
            --airRoute.currentTime;
            List<Disease> diseaseList = new List<Disease>();
            for (int index2 = 0; index2 < World.instance.diseases.Count; ++index2)
            {
              MPDisease disease = (MPDisease) World.instance.diseases[index2];
              MPLocalDisease localDisease1 = (MPLocalDisease) s.GetLocalDisease((Disease) disease);
              MPLocalDisease localDisease2 = (MPLocalDisease) disease.GetLocalDisease(country);
              if (MPLocalDiseaseExternal.AirTransfer(disease.mData, localDisease1.mData, ((MPCountry) s).mData, localDisease2.mData, ((MPCountry) country).mData))
                diseaseList.Add(localDisease1.disease);
            }
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Airplane;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            if (diseaseList != null)
            {
              foreach (Disease d in diseaseList)
                vehicle.AddInfected(d, ModelUtils.IntRand(1, 10));
            }
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    if (this.seaRoutes.Count > 0 && this.portStatus)
    {
      for (int index3 = 0; index3 < this.seaRoutes.Count; ++index3)
      {
        TravelRoute seaRoute = this.seaRoutes[index3];
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
          float num = (float) ((double) seaRoute.frequency * (double) s.currentPopulation / (double) s.originalPopulation * (flag ? 1.0 - (double) s.totalSeverity / 100.0 : 1.0));
          seaRoute.currentTime += num;
          if ((double) seaRoute.currentTime >= 1.0)
          {
            --seaRoute.currentTime;
            List<Disease> diseaseList = new List<Disease>();
            for (int index4 = 0; index4 < World.instance.diseases.Count; ++index4)
            {
              MPDisease disease = (MPDisease) World.instance.diseases[index4];
              MPLocalDisease localDisease3 = (MPLocalDisease) s.GetLocalDisease((Disease) disease);
              MPLocalDisease localDisease4 = (MPLocalDisease) disease.GetLocalDisease(country);
              if (MPLocalDiseaseExternal.SeaTransfer(disease.mData, localDisease3.mData, ((MPCountry) s).mData, localDisease4.mData, ((MPCountry) country).mData))
                diseaseList.Add(localDisease3.disease);
            }
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Boat;
            vehicle.subType = Vehicle.EVehicleSubType.Normal;
            vehicle.SetRoute(s, country);
            if (diseaseList != null)
            {
              foreach (Disease d in diseaseList)
                vehicle.AddInfected(d, ModelUtils.IntRand(1, 10));
            }
            World.instance.AddVehicle(vehicle);
          }
        }
      }
    }
    if (!this.isDestroyed && (double) this.deadPercent + (double) this.zombiePercent >= 1.0)
      this.isDestroyed = true;
    long num1 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num1 += this.localDiseases[index].controlledInfected;
    this.totalInfected = num1;
    long num2 = 0;
    for (int index = 0; index < this.localDiseases.Count; ++index)
      num2 += this.localDiseases[index].zombie;
    this.totalZombie = num2;
    Disease disease1 = CNetworkManager.network.LocalPlayerInfo.disease;
    Disease d1 = World.instance.diseases[0] == disease1 ? World.instance.diseases[1] : World.instance.diseases[0];
    LocalDisease localDisease5 = this.GetLocalDisease(disease1);
    LocalDisease localDisease6 = this.GetLocalDisease(d1);
    this.myUninfected = (float) localDisease5.uninfectedPopulation;
    this.myUninfectedPercentage = (float) localDisease5.uninfectedPopulation / (float) this.originalPopulation;
    this.theirUninfected = (float) localDisease6.uninfectedPopulation;
    this.theirUninfectedPercentage = (float) localDisease6.uninfectedPopulation / (float) this.originalPopulation;
    this.currentPopulation = this.totalInfected + this.totalZombie + this.totalHealthy;
  }

  public override void VehicleArrived(Vehicle vehicle, int presetInt = -1)
  {
    if (vehicle.subType == Vehicle.EVehicleSubType.Cure)
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
    else
    {
      if (vehicle.infected == null)
        return;
      MPLocalDisease localDisease = (MPLocalDisease) this.GetLocalDisease(vehicle.actingDisease);
      for (int index = 0; index < vehicle.infected.Length; ++index)
      {
        if (vehicle.infected[index] > 0)
        {
          Disease disease = World.instance.diseases[index];
          int number = vehicle.infected[index];
          if (vehicle.actingDisease == disease && localDisease != null && localDisease.unscheduledFlightCounter > 0)
            number += Mathf.Max(1, (int) ((double) localDisease.unscheduledFlightCounter * 10.0));
          if (vehicle.subType == Vehicle.EVehicleSubType.Unscheduled)
            Debug.Log((object) (World.instance.diseases[index].name + " Unscheduled Infected: " + (object) number));
          this.TransferInfected((double) number, disease);
        }
      }
      if (vehicle.subType != Vehicle.EVehicleSubType.Unscheduled)
        return;
      localDisease.UnscheduledFlight();
    }
  }

  public override float DroneAttack(Disease d, bool colonyDestroyed, float presetFloat = -1f)
  {
    return presetFloat;
  }

  public void NukeStrike(Disease sender)
  {
    if (sender.nuclearCountry == null)
    {
      Debug.Log((object) "Nuclear Cheating detected, end game!");
      CGameManager.cheatDetected = true;
      CGameManager.game.EndGame(IGame.EndGameReason.CHEATING);
    }
    else
    {
      this.TransferPopulation((double) this.totalInfected, Country.EPopulationType.INFECTED, sender, Country.EPopulationType.DEAD, 0.0);
      this.TransferPopulation((double) this.totalInfected, Country.EPopulationType.INFECTED_SHARED, sender, Country.EPopulationType.DEAD, 0.0);
      this.TransferPopulation((double) this.totalInfected, Country.EPopulationType.INFECTED, World.instance.diseases.Find((Predicate<Disease>) (a => a != sender)), Country.EPopulationType.DEAD, 1.0);
      this.TransferPopulation((double) this.healthyPopulation, Country.EPopulationType.HEALTHY, sender, Country.EPopulationType.DEAD, 0.0);
      this.nukedBy = sender.nuclearCountry.name;
      this.nuked = true;
    }
  }
}
