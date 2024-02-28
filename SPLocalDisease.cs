// Decompiled with JetBrains decompiler
// Type: SPLocalDisease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SPLocalDisease : LocalDisease
{
  public SPLocalDiseaseData mData;

  public override int countryNumber
  {
    get => this.mData.countryNumber;
    set => this.mData.countryNumber = value;
  }

  public override int diseaseID
  {
    get => this.mData.diseaseID;
    set => this.mData.diseaseID = value;
  }

  public override long killedPopulation
  {
    get => this.mData.killedPopulation;
    set => this.mData.killedPopulation = value;
  }

  public override long _controlledInfected
  {
    get => this.mData._controlledInfected;
    set => this.mData._controlledInfected = value;
  }

  public override long zombie
  {
    get => this.mData.zombie;
    set => this.mData.zombie = value;
  }

  public override long uncontrolledInfected
  {
    get => this.mData.uncontrolledInfected;
    set => this.mData.uncontrolledInfected = value;
  }

  public override long prevKilled
  {
    get => this.mData.prevKilled;
    set => this.mData.prevKilled = value;
  }

  public override long prevInfected
  {
    get => this.mData.prevInfected;
    set => this.mData.prevInfected = value;
  }

  public override long prevZombie
  {
    get => this.mData.prevZombie;
    set => this.mData.prevZombie = value;
  }

  public override long prevInfectedApes
  {
    get => this.mData.prevInfectedApes;
    set => this.mData.prevInfectedApes = value;
  }

  public override float localPriority
  {
    get => this.mData.localPriority;
    set => this.mData.localPriority = value;
  }

  public override float localBonusPriority
  {
    get => this.mData.localBonusPriority;
    set => this.mData.localBonusPriority = value;
  }

  public override float localCureResearch
  {
    get => this.mData.localCureResearch;
    set => this.mData.localCureResearch = value;
  }

  public override float cureResearchAllocation
  {
    get => this.mData.cureResearchAllocation;
    set => this.mData.cureResearchAllocation = value;
  }

  public override float localPriorityCounter
  {
    get => this.mData.localPriorityCounter;
    set => this.mData.localPriorityCounter = value;
  }

  public override float localInfectiousness
  {
    get => this.mData.localInfectiousness;
    set => this.mData.localInfectiousness = value;
  }

  public override float localSeverity
  {
    get => this.mData.localSeverity;
    set => this.mData.localSeverity = value;
  }

  public override float localLethality
  {
    get => this.mData.localLethality;
    set => this.mData.localLethality = value;
  }

  public override float localCorpseTransmission
  {
    get => this.mData.localCorpseTransmission;
    set => this.mData.localCorpseTransmission = value;
  }

  public override float localInfectedPercent
  {
    get => this.mData.localInfectedPercent;
    set => this.mData.localInfectedPercent = value;
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

  public override float infectedPopulationOverride
  {
    get => this.mData.infectedPopulationOverride;
    set => this.mData.infectedPopulationOverride = value;
  }

  public override float deadPopulationOverride
  {
    get => this.mData.deadPopulationOverride;
    set => this.mData.deadPopulationOverride = value;
  }

  public override float maxDeadChangePerTurn
  {
    get => this.mData.maxDeadChangePerTurn;
    set => this.mData.maxDeadChangePerTurn = value;
  }

  public override float H2I
  {
    get => this.mData.H2I;
    set => this.mData.H2I = value;
  }

  public override float I2D
  {
    get => this.mData.I2D;
    set => this.mData.I2D = value;
  }

  public override float D2Z
  {
    get => this.mData.D2Z;
    set => this.mData.D2Z = value;
  }

  public override float I2Z
  {
    get => this.mData.I2Z;
    set => this.mData.I2Z = value;
  }

  public override float H2Z
  {
    get => this.mData.H2Z;
    set => this.mData.H2Z = value;
  }

  public override float H2D
  {
    get => this.mData.H2D;
    set => this.mData.H2D = value;
  }

  public override float Z2D
  {
    get => this.mData.Z2D;
    set => this.mData.Z2D = value;
  }

  public override float D2ZOverride
  {
    get => this.mData.D2ZOverride;
    set => this.mData.D2ZOverride = value;
  }

  public override float I2ZOverride
  {
    get => this.mData.I2ZOverride;
    set => this.mData.I2ZOverride = value;
  }

  public override float H2ZOverride
  {
    get => this.mData.H2ZOverride;
    set => this.mData.H2ZOverride = value;
  }

  public override float H2DOverride
  {
    get => this.mData.H2DOverride;
    set => this.mData.H2DOverride = value;
  }

  public override float Z2DOverride
  {
    get => this.mData.Z2DOverride;
    set => this.mData.Z2DOverride = value;
  }

  public override float cureRollout
  {
    get => this.mData.cureRollout;
    set => this.mData.cureRollout = value;
  }

  public override float borderStatusValue
  {
    get => this.mData.borderStatusValue;
    set => this.mData.borderStatusValue = value;
  }

  public override int turnsSinceBluePlane
  {
    get => this.mData.turnsSinceBluePlane;
    set => this.mData.turnsSinceBluePlane = value;
  }

  public override bool isSuperCureCountry
  {
    get => this.mData.isSuperCureCountry;
    set => this.mData.isSuperCureCountry = value;
  }

  public override bool isNexus
  {
    get => this.mData.isNexus;
    set => this.mData.isNexus = value;
  }

  public override int zombieInactivityCounter
  {
    get => this.mData.zombieInactivityCounter;
    set => this.mData.zombieInactivityCounter = value;
  }

  public override float localZombieCombatStrength
  {
    get => this.mData.localZombieCombatStrength;
    set => this.mData.localZombieCombatStrength = value;
  }

  public override float localZombieCombatStrengthMax
  {
    get => this.mData.localZombieCombatStrengthMax;
    set => this.mData.localZombieCombatStrengthMax = value;
  }

  public override float localZombieDecayMultiplier
  {
    get => this.mData.localZombieDecayMultiplier;
    set => this.mData.localZombieDecayMultiplier = value;
  }

  public override float localZombieDecayOverride
  {
    get => this.mData.localZombieDecayOverride;
    set => this.mData.localZombieDecayOverride = value;
  }

  public override float maxDecayChangePerTurn
  {
    get => this.mData.maxDecayChangePerTurn;
    set => this.mData.maxDecayChangePerTurn = value;
  }

  public override long originalPopulation
  {
    get => this.mData.originalPopulation;
    set => this.mData.originalPopulation = value;
  }

  public override long uninfectedPopulation
  {
    get => this.mData.uninfectedPopulation;
    set => this.mData.uninfectedPopulation = value;
  }

  public override int flaskBroken
  {
    get => this.mData.flaskBroken;
    set => this.mData.flaskBroken = value;
  }

  public override int flaskResearched
  {
    get => this.mData.flaskResearched;
    set => this.mData.flaskResearched = value;
  }

  public override int flaskEmpty
  {
    get => this.mData.flaskEmpty;
    set => this.mData.flaskEmpty = value;
  }

  public override float I2DAdditional
  {
    get => this.mData.I2DAdditional;
    set => this.mData.I2DAdditional = value;
  }

  public override long apeHealthyPopulation
  {
    get => this.mData.apeHealthyPopulation;
    set => this.mData.apeHealthyPopulation = value;
  }

  public override long apeDeadPopulation
  {
    get => this.mData.apeDeadPopulation;
    set => this.mData.apeDeadPopulation = value;
  }

  public override long apeInfectedPopulation
  {
    get => this.mData.apeInfectedPopulation;
    set => this.mData.apeInfectedPopulation = value;
  }

  public override float healthyImmunePercent
  {
    get => this.mData.healthyImmunePercent;
    set => this.mData.healthyImmunePercent = value;
  }

  public override float healthySusceptiblePercent
  {
    get => this.mData.healthySusceptiblePercent;
    set => this.mData.healthySusceptiblePercent = value;
  }

  public override float apeTotalAlivePopulation
  {
    get => this.mData.apeTotalAlivePopulation;
    set => this.mData.apeTotalAlivePopulation = value;
  }

  public override float apeH2I
  {
    get => this.mData.apeH2I;
    set => this.mData.apeH2I = value;
  }

  public override float apeI2D
  {
    get => this.mData.apeI2D;
    set => this.mData.apeI2D = value;
  }

  public override float apeH2D
  {
    get => this.mData.apeH2D;
    set => this.mData.apeH2D = value;
  }

  public override float apePercCaptivity
  {
    get => this.mData.apePercCaptivity;
    set => this.mData.apePercCaptivity = value;
  }

  public override float apeHealthyPercent
  {
    get => this.mData.apeHealthyPercent;
    set => this.mData.apeHealthyPercent = value;
  }

  public override float apeInfectedPercent
  {
    get => this.mData.apeInfectedPercent;
    set => this.mData.apeInfectedPercent = value;
  }

  public override float apeDeadPercent
  {
    get => this.mData.apeDeadPercent;
    set => this.mData.apeDeadPercent = value;
  }

  public override float immuneH2D
  {
    get => this.mData.immuneH2D;
    set => this.mData.immuneH2D = value;
  }

  public override float susceptibleH2D
  {
    get => this.mData.susceptibleH2D;
    set => this.mData.susceptibleH2D = value;
  }

  public override float apePriorityLevelLocal
  {
    get => this.mData.apePriorityLevelLocal;
    set => this.mData.apePriorityLevelLocal = value;
  }

  public override float apePriorityLevelLocalRaw
  {
    get => this.mData.apePriorityLevelLocalRaw;
    set => this.mData.apePriorityLevelLocalRaw = value;
  }

  public override float apeActivityLevel
  {
    get => this.mData.apeActivityLevel;
    set => this.mData.apeActivityLevel = value;
  }

  public override int apeStatusRampage
  {
    get => this.mData.apeStatusRampage;
    set => this.mData.apeStatusRampage = value;
  }

  public override int daysSinceLocalDrone
  {
    get => this.mData.daysSinceLocalDrone;
    set => this.mData.daysSinceLocalDrone = value;
  }

  public override int apeHiddenCounter
  {
    get => this.mData.apeHiddenCounter;
    set => this.mData.apeHiddenCounter = value;
  }

  public override int apeRampageCounter
  {
    get => this.mData.apeRampageCounter;
    set => this.mData.apeRampageCounter = value;
  }

  public override int apeFirstInfectionEffectDone
  {
    get => this.mData.apeFirstInfectionEffectDone;
    set => this.mData.apeFirstInfectionEffectDone = value;
  }

  public override float apeSuitability
  {
    get => this.mData.apeSuitability;
    set => this.mData.apeSuitability = value;
  }

  public override int masterLabFlag
  {
    get => this.mData.masterLabFlag;
    set => this.mData.masterLabFlag = value;
  }

  public override int apeRampageMission
  {
    get => this.mData.apeRampageMission;
    set => this.mData.apeRampageMission = value;
  }

  public override float localApeInfectiousness
  {
    get => this.mData.localApeInfectiousness;
    set => this.mData.localApeInfectiousness = value;
  }

  public override float localApeLethality
  {
    get => this.mData.localApeLethality;
    set => this.mData.localApeLethality = value;
  }

  public override float apePopMod
  {
    get => this.mData.apePopMod;
    set => this.mData.apePopMod = value;
  }

  public override float apeInfectionRate
  {
    get => this.mData.apeInfectionRate;
    set => this.mData.apeInfectionRate = value;
  }

  public override float humanStrength
  {
    get => this.mData.humanStrength;
    set => this.mData.humanStrength = value;
  }

  public override float localApeStrength
  {
    get => this.mData.localApeStrength;
    set => this.mData.localApeStrength = value;
  }

  public override float apeActivityImpact
  {
    get => this.mData.apeActivityImpact;
    set => this.mData.apeActivityImpact = value;
  }

  public override int apeColonyBubbleCounter
  {
    get => this.mData.apeColonyBubbleCounter;
    set => this.mData.apeColonyBubbleCounter = value;
  }

  public override int droneAttackSuccessFlag
  {
    get => this.mData.droneAttackSuccessFlag;
    set => this.mData.droneAttackSuccessFlag = value;
  }

  public override float localDroneCounter
  {
    get => this.mData.localDroneCounter;
    set => this.mData.localDroneCounter = value;
  }

  public override float vampireUnderAttackFlag
  {
    get => this.mData.vampireUnderAttackFlag;
    set => this.mData.vampireUnderAttackFlag = value;
  }

  public override float vampirePresenceCounter
  {
    get => this.mData.vampirePresenceCounter;
    set => this.mData.vampirePresenceCounter = value;
  }

  public override ECastleState castleState
  {
    get => this.mData.castleState;
    set => this.mData.castleState = value;
  }

  public override float fortDestroyedCounter
  {
    get => this.mData.fortDestroyedCounter;
    set => this.mData.fortDestroyedCounter = value;
  }

  public override float fortResearchDays
  {
    get => this.mData.fortResearchDays;
    set => this.mData.fortResearchDays = value;
  }

  public override float fortRebuildDelay
  {
    get => this.mData.fortRebuildDelay;
    set => this.mData.fortRebuildDelay = value;
  }

  public override bool vampireSpawn
  {
    get => this.mData.vampireSpawn;
    set => this.mData.vampireSpawn = value;
  }

  public override float fortHealth
  {
    get => this.mData.fortHealth;
    set => this.mData.fortHealth = value;
  }

  public override float fortHealthMax
  {
    get => this.mData.fortHealthMax;
    set => this.mData.fortHealthMax = value;
  }

  public override int consumeFlag
  {
    get => this.mData.consumeFlag;
    set => this.mData.consumeFlag = value;
  }

  public override int lastConsumeTurn
  {
    get => this.mData.lastConsumeTurn;
    set => this.mData.lastConsumeTurn = value;
  }

  public override int castleBubbleCounter
  {
    get => this.mData.castleBubbleCounter;
    set => this.mData.castleBubbleCounter = value;
  }

  public override float localVampireActivity
  {
    get => this.mData.localVampireActivity;
    set => this.mData.localVampireActivity = value;
  }

  public override int vampireObituaryCount
  {
    get => this.mData.vampireObituaryCount;
    set => this.mData.vampireObituaryCount = value;
  }

  public override int numberDeadVampires
  {
    get => this.mData.numberDeadVampires;
    set => this.mData.numberDeadVampires = value;
  }

  public override int castleObituaryCount
  {
    get => this.mData.castleObituaryCount;
    set => this.mData.castleObituaryCount = value;
  }

  public override int vampireBirthCount
  {
    get => this.mData.vampireBirthCount;
    set => this.mData.vampireBirthCount = value;
  }

  public override int fortAttackFailedFlag
  {
    get => this.mData.fortAttackFailedFlag;
    set => this.mData.fortAttackFailedFlag = value;
  }

  public override int droneTargetFlag
  {
    get => this.mData.droneTargetFlag;
    set => this.mData.droneTargetFlag = value;
  }

  public override int daysTillActive
  {
    get => this.mData.daysTillActive;
    set => this.mData.daysTillActive = value;
  }

  public override float vampireHealthDamage
  {
    get => this.mData.vampireHealthDamage;
    set => this.mData.vampireHealthDamage = value;
  }

  public override float H2DAdditional
  {
    get => this.mData.H2DAdditional;
    set => this.mData.H2DAdditional = value;
  }

  public override bool peopleAttackVampire
  {
    get => this.mData.peopleAttackVampire;
    set => this.mData.peopleAttackVampire = value;
  }

  public override bool castleBubbleShowing
  {
    get => this.mData.castleBubbleShowing;
    set => this.mData.castleBubbleShowing = value;
  }

  public override float localDeadBodyTransmission
  {
    get => this.mData.localDeadBodyTransmission;
    set => this.mData.localDeadBodyTransmission = value;
  }

  public override float fakeNewsWinRollout
  {
    get => this.mData.fakeNewsWinRollout;
    set => this.mData.fakeNewsWinRollout = value;
  }

  public override int framesWithoutHistory
  {
    get => this.mData.framesWithoutHistory;
    set => this.mData.framesWithoutHistory = value;
  }

  public override bool firstHistoryFrame
  {
    get => this.mData.firstHistoryFrame;
    set => this.mData.firstHistoryFrame = value;
  }

  public override long lastKnownInfectedCount
  {
    get => this.mData.lastKnownInfectedCount;
    set => this.mData.lastKnownInfectedCount = value;
  }

  public override int diseaseLength
  {
    get => this.mData.diseaseLength;
    set => this.mData.diseaseLength = value;
  }

  public override int temporalCounter
  {
    get => this.mData.temporalCounter;
    set => this.mData.temporalCounter = value;
  }

  public override float highestInfectedPercent
  {
    get => this.mData.highestInfectedPercent;
    set => this.mData.highestInfectedPercent = value;
  }

  public override bool hasIntel
  {
    get => this.mData.hasIntel;
    set => this.mData.hasIntel = value;
  }

  public override float authority
  {
    get => this.mData.authority;
    set => this.mData.authority = value;
  }

  public override float vehicleInfectionBoostChance
  {
    get => this.mData.vehicleInfectionBoostChance;
    set => this.mData.vehicleInfectionBoostChance = value;
  }

  public override float overflowLocalLethality
  {
    get => this.mData.overflowLocalLethality;
    set => this.mData.overflowLocalLethality = value;
  }

  public override float capacityOverflowTime
  {
    get => this.mData.capacityOverflowTime;
    set => this.mData.capacityOverflowTime = value;
  }

  public override float localCureProgress
  {
    get => this.mData.localCureProgress;
    set => this.mData.localCureProgress = value;
  }

  public override float standardLocalPriority
  {
    get => this.mData.standardLocalPriority;
    set => this.mData.standardLocalPriority = value;
  }

  public override float connectedLocalPriority
  {
    get => this.mData.connectedLocalPriority;
    set => this.mData.connectedLocalPriority = value;
  }

  public override float paranoia
  {
    get => this.mData.paranoia;
    set => this.mData.paranoia = value;
  }

  public override float capacityOverload
  {
    get => this.mData.capacityOverload;
    set => this.mData.capacityOverload = value;
  }

  public override float teamMedicalCapacityMod
  {
    get => this.mData.teamMedicalCapacityMod;
    set => this.mData.teamMedicalCapacityMod = value;
  }

  public override float actualBaseInfluence
  {
    get => this.mData.actualBaseInfluence;
    set => this.mData.actualBaseInfluence = value;
  }

  public override float actualMedicalCapacity
  {
    get => this.mData.actualMedicalCapacity;
    set => this.mData.actualMedicalCapacity = value;
  }

  public override float buriedDeadPercent
  {
    get => this.mData.buriedDeadPercent;
    set => this.mData.buriedDeadPercent = value;
  }

  public override float societyUnrest
  {
    get => this.mData.societyUnrest;
    set => this.mData.societyUnrest = value;
  }

  public override float economy
  {
    get => this.mData.economy;
    set => this.mData.economy = value;
  }

  public override float economyMAX
  {
    get => this.mData.economyMAX;
    set => this.mData.economyMAX = value;
  }

  public override float lowestEconomy
  {
    get => this.mData.lowestEconomy;
    set => this.mData.lowestEconomy = value;
  }

  public override float economyDamage
  {
    get => this.mData.economyDamage;
    set => this.mData.economyDamage = value;
  }

  public override float economyDefense
  {
    get => this.mData.economyDefense;
    set => this.mData.economyDefense = value;
  }

  public override float diseaseUnrest
  {
    get => this.mData.diseaseUnrest;
    set => this.mData.diseaseUnrest = value;
  }

  public override bool dark
  {
    get => this.mData.dark;
    set => this.mData.dark = value;
  }

  public override int tempLockdownTimer
  {
    get => this.mData.tempLockdownTimer;
    set => this.mData.tempLockdownTimer = value;
  }

  public override bool forceCloseLandBorders
  {
    get => this.mData.forceCloseLandBorders;
    set => this.mData.forceCloseLandBorders = value;
  }

  public override bool forceCloseAirBorders
  {
    get => this.mData.forceCloseAirBorders;
    set => this.mData.forceCloseAirBorders = value;
  }

  public override bool forceClosePortBorders
  {
    get => this.mData.forceClosePortBorders;
    set => this.mData.forceClosePortBorders = value;
  }

  public override int turnVehicleArrived
  {
    get => this.mData.turnVehicleArrived;
    set => this.mData.turnVehicleArrived = value;
  }

  public override float localMedicalAidCapacityMod
  {
    get => this.mData.localMedicalAidCapacityMod;
    set => this.mData.localMedicalAidCapacityMod = value;
  }

  public override float localTempLockdownPriorityMulti
  {
    get => this.mData.localTempLockdownPriorityMulti;
    set => this.mData.localTempLockdownPriorityMulti = value;
  }

  public override float lockdownDamagePublicOrder
  {
    get => this.mData.lockdownDamagePublicOrder;
    set => this.mData.lockdownDamagePublicOrder = value;
  }

  public override float localInfectivityReductionMod
  {
    get => this.mData.localInfectivityReductionMod;
    set => this.mData.localInfectivityReductionMod = value;
  }

  public override float localInfectivityReductionPerc
  {
    get => this.mData.localInfectivityReductionPerc;
    set => this.mData.localInfectivityReductionPerc = value;
  }

  public override float localContactTracingPop
  {
    get => this.mData.localContactTracingPop;
    set => this.mData.localContactTracingPop = value;
  }

  public override float contactTracingRampUp
  {
    get => this.mData.contactTracingRampUp;
    set => this.mData.contactTracingRampUp = value;
  }

  public override float ctFailureChance
  {
    get => this.mData.ctFailureChance;
    set => this.mData.ctFailureChance = value;
  }

  public override bool selfDiscovered
  {
    get => this.mData.selfDiscovered;
    set => this.mData.selfDiscovered = value;
  }

  public override float tempLocalPriorityMultiplier
  {
    get => this.mData.tempLocalPriorityMultiplier;
    set => this.mData.tempLocalPriorityMultiplier = value;
  }

  public override float tempConnectedLocalPriorityMultiplier
  {
    get => this.mData.tempConnectedLocalPriorityMultiplier;
    set => this.mData.tempConnectedLocalPriorityMultiplier = value;
  }

  public override float localNewInfectedGUI
  {
    get => this.mData.localNewInfectedGUI;
    set => this.mData.localNewInfectedGUI = value;
  }

  public override float govActionOddsMulti
  {
    get => this.mData.govActionOddsMulti;
    set => this.mData.govActionOddsMulti = value;
  }

  public override float lockdownAACount
  {
    get => this.mData.lockdownAACount;
    set => this.mData.lockdownAACount = value;
  }

  public override bool lockdownAAActive
  {
    get => this.mData.lockdownAAActive;
    set => this.mData.lockdownAAActive = value;
  }

  public override bool lockdownAAUpgraded
  {
    get => this.mData.lockdownAAUpgraded;
    set => this.mData.lockdownAAUpgraded = value;
  }

  public override int lockdownAACashback
  {
    get => this.mData.lockdownAACashback;
    set => this.mData.lockdownAACashback = value;
  }

  public override bool supportActive
  {
    get => this.mData.supportActive;
    set => this.mData.supportActive = value;
  }

  public override int supportTimer
  {
    get => this.mData.supportTimer;
    set => this.mData.supportTimer = value;
  }

  public override int supportAACashback
  {
    get => this.mData.supportAACashback;
    set => this.mData.supportAACashback = value;
  }

  public override bool hasTeam
  {
    get => this.mData.hasTeam;
    set => this.mData.hasTeam = value;
  }

  public override int teamTurnsSinceArrived
  {
    get => this.mData.teamTurnsSinceArrived;
    set => this.mData.teamTurnsSinceArrived = value;
  }

  public override float localInfectionRate
  {
    get => this.mData.localInfectionRate;
    set => this.mData.localInfectionRate = value;
  }

  public override float infectionRateToday
  {
    get => this.mData.infectionRateToday;
    set => this.mData.infectionRateToday = value;
  }

  public override float infectionRateYesterday
  {
    get => this.mData.infectionRateYesterday;
    set => this.mData.infectionRateYesterday = value;
  }

  public override float localEstimatedDeathRate
  {
    get => this.mData.localEstimatedDeathRate;
    set => this.mData.localEstimatedDeathRate = value;
  }

  public override int numDeadBubbles
  {
    get => this.mData.numDeadBubbles;
    set => this.mData.numDeadBubbles = value;
  }

  public override bool isRussia
  {
    get => this.mData.isRussia;
    set => this.mData.isRussia = value;
  }

  public override bool hasInfectedConnection
  {
    get => this.mData.hasInfectedConnection;
    set => this.mData.hasInfectedConnection = value;
  }

  public override int authLossStatusCounter
  {
    get => this.mData.authLossStatusCounter;
    set => this.mData.authLossStatusCounter = value;
  }

  public override float localAuthLossInfected
  {
    get => this.mData.localAuthLossInfected;
    set => this.mData.localAuthLossInfected = value;
  }

  public override float localAuthLossDead
  {
    get => this.mData.localAuthLossDead;
    set => this.mData.localAuthLossDead = value;
  }

  public override float totalLocalAuthLoss
  {
    get => this.mData.totalLocalAuthLoss;
    set => this.mData.totalLocalAuthLoss = value;
  }

  public override int controlLossPulse
  {
    get => this.mData.controlLossPulse;
    set => this.mData.controlLossPulse = value;
  }

  public override bool contactTracingOverride
  {
    get => this.mData.contactTracingOverride;
    set => this.mData.contactTracingOverride = value;
  }

  public override bool landBorderOverride
  {
    get => this.mData.landBorderOverride;
    set => this.mData.landBorderOverride = value;
  }

  public override bool airportOverride
  {
    get => this.mData.airportOverride;
    set => this.mData.airportOverride = value;
  }

  public override bool portOverride
  {
    get => this.mData.portOverride;
    set => this.mData.portOverride = value;
  }

  public override bool lockdownOverride
  {
    get => this.mData.lockdownOverride;
    set => this.mData.lockdownOverride = value;
  }

  public override bool borderOrLockdownOverride
  {
    get => this.mData.borderOrLockdownOverride;
    set => this.mData.borderOrLockdownOverride = value;
  }

  public override float compliance
  {
    get => this.mData.compliance;
    set => this.mData.compliance = value;
  }

  public override float complianceMAX
  {
    get => this.mData.complianceMAX;
    set => this.mData.complianceMAX = value;
  }

  public override float complianceMAXTime
  {
    get => this.mData.complianceMAXTime;
    set => this.mData.complianceMAXTime = value;
  }

  public override float complianceMAXRand
  {
    get => this.mData.complianceMAXRand;
    set => this.mData.complianceMAXRand = value;
  }

  public override float compliancePercMod
  {
    get => this.mData.compliancePercMod;
    set => this.mData.compliancePercMod = value;
  }

  public override float unrestCooldown
  {
    get => this.mData.unrestCooldown;
    set => this.mData.unrestCooldown = value;
  }

  public override float unrestCooldownMod
  {
    get => this.mData.unrestCooldownMod;
    set => this.mData.unrestCooldownMod = value;
  }

  public override bool unrestActive
  {
    get => this.mData.unrestActive;
    set => this.mData.unrestActive = value;
  }

  public override int unrestDays
  {
    get => this.mData.unrestDays;
    set => this.mData.unrestDays = value;
  }

  public override float complianceDropLand
  {
    get => this.mData.complianceDropLand;
    set => this.mData.complianceDropLand = value;
  }

  public override float complianceDropAir
  {
    get => this.mData.complianceDropAir;
    set => this.mData.complianceDropAir = value;
  }

  public override float complianceDropSea
  {
    get => this.mData.complianceDropSea;
    set => this.mData.complianceDropSea = value;
  }

  public override float complianceDropLockdown
  {
    get => this.mData.complianceDropLockdown;
    set => this.mData.complianceDropLockdown = value;
  }

  public override float complianceMulti
  {
    get => this.mData.complianceMulti;
    set => this.mData.complianceMulti = value;
  }

  public override float complianceBefore
  {
    get => this.mData.complianceBefore;
    set => this.mData.complianceBefore = value;
  }

  public override float complianceRestingPoint
  {
    get => this.mData.complianceRestingPoint;
    set => this.mData.complianceRestingPoint = value;
  }

  public override bool complianceFalling
  {
    get => this.mData.complianceFalling;
    set => this.mData.complianceFalling = value;
  }

  public override float complianceIconScale
  {
    get => this.mData.complianceIconScale;
    set => this.mData.complianceIconScale = value;
  }

  public override float complianceBin
  {
    get => this.mData.complianceBin;
    set => this.mData.complianceBin = value;
  }

  public override Country.EGenericCountryFlag genericFlags
  {
    get => this.mData.genericFlags;
    set => this.mData.genericFlags = value;
  }

  public override float localPriorityTimeDelayed
  {
    get => this.mData.localPriorityTimeDelayed;
    set => this.mData.localPriorityTimeDelayed = value;
  }

  public override float complianceIconFiredAt
  {
    get => this.mData.complianceIconFiredAt;
    set => this.mData.complianceIconFiredAt = value;
  }

  public override bool fireComplianceIcon
  {
    get => this.mData.fireComplianceIcon;
    set => this.mData.fireComplianceIcon = value;
  }

  public static void TestLibrary()
  {
    SPLocalDiseaseData d = new SPLocalDiseaseData();
    SPLocalDiseaseData localDiseaseData = new SPLocalDiseaseData();
    localDiseaseData.flaskEmpty = d.flaskEmpty = 4;
    localDiseaseData.localZombieCombatStrength = d.localZombieCombatStrength = 0.456f;
    localDiseaseData.cureRollout = d.cureRollout = 0.541f;
    localDiseaseData.localCorpseTransmission = d.localCorpseTransmission = 0.798f;
    localDiseaseData.prevInfected = d.prevInfected = 871256124L;
    localDiseaseData.apeSuitability = d.apeSuitability = 0.5f;
    localDiseaseData.apePopMod = d.apePopMod = 1.9f;
    localDiseaseData.localVampireActivity = d.localVampireActivity = 0.3f;
    localDiseaseData.daysTillActive = d.daysTillActive = 5;
    localDiseaseData.localDeadBodyTransmission = d.localDeadBodyTransmission = 0.33f;
    localDiseaseData.fakeNewsWinRollout = d.fakeNewsWinRollout = 3.1f;
    localDiseaseData.lastKnownInfectedCount = d.lastKnownInfectedCount = 45L;
    localDiseaseData.localAuthLossInfected = d.localAuthLossInfected = 0.4f;
    localDiseaseData.complianceBin = d.complianceBin = 0.5f;
    localDiseaseData.genericFlags = d.genericFlags = Country.EGenericCountryFlag.GreenBubble;
    localDiseaseData.flaskEmpty *= 2;
    localDiseaseData.localZombieCombatStrength += 0.111f;
    localDiseaseData.cureRollout -= 0.03f;
    localDiseaseData.localCorpseTransmission *= 3f;
    localDiseaseData.prevInfected /= 2L;
    localDiseaseData.apeSuitability += 1.86f;
    ++localDiseaseData.apePopMod;
    localDiseaseData.localVampireActivity += 34.5f;
    localDiseaseData.daysTillActive += 23;
    localDiseaseData.castleBubbleShowing = true;
    localDiseaseData.localDeadBodyTransmission += 0.01f;
    localDiseaseData.fakeNewsWinRollout += 4f;
    localDiseaseData.lastKnownInfectedCount += 13L;
    localDiseaseData.localAuthLossInfected += 0.14f;
    localDiseaseData.complianceBin += 0.15f;
    localDiseaseData.genericFlags = localDiseaseData.genericFlags | Country.EGenericCountryFlag.DeadBubbleForCure | Country.EGenericCountryFlag.IntelVehicleDispatched;
    UnityEngine.Random.seed = 1;
    SPLocalDiseaseExternal.LocalDiseaseTest(d);
    if (localDiseaseData.flaskEmpty != d.flaskEmpty)
      Debug.LogWarning((object) "LocalDisease TEST 1 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 1 SUCCEED");
    if ((double) localDiseaseData.localZombieCombatStrength != (double) d.localZombieCombatStrength)
      Debug.LogWarning((object) "LocalDisease TEST 2 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 2 SUCCEED");
    if ((double) localDiseaseData.cureRollout != (double) d.cureRollout)
      Debug.LogWarning((object) "LocalDisease TEST 3 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 3 SUCCEED");
    if ((double) localDiseaseData.localCorpseTransmission != (double) d.localCorpseTransmission)
      Debug.LogWarning((object) "LocalDisease TEST 4 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 4 SUCCEED");
    if (localDiseaseData.prevInfected != d.prevInfected)
      Debug.LogWarning((object) "LocalDisease TEST 5 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 5 SUCCEED");
    if ((double) localDiseaseData.apeSuitability != (double) d.apeSuitability)
      Debug.LogWarning((object) "LocalDisease TEST 6 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 6 SUCCEED");
    if ((double) localDiseaseData.apePopMod != (double) d.apePopMod)
      Debug.LogWarning((object) "LocalDisease TEST 7 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 7 SUCCEED");
    if ((double) localDiseaseData.localVampireActivity != (double) d.localVampireActivity)
      Debug.LogWarning((object) "LocalDisease TEST 8 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 8 SUCCEED");
    if (localDiseaseData.daysTillActive != d.daysTillActive)
      Debug.LogWarning((object) "LocalDisease TEST 9 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 9 SUCCEED");
    if (localDiseaseData.castleBubbleShowing != d.castleBubbleShowing)
      Debug.LogWarning((object) "LocalDisease TEST 10 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 10 SUCCEED");
    if ((double) localDiseaseData.localDeadBodyTransmission != (double) d.localDeadBodyTransmission)
      Debug.LogWarning((object) "LocalDisease TEST 11 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 11 SUCCEED");
    if ((double) localDiseaseData.fakeNewsWinRollout != (double) d.fakeNewsWinRollout)
      Debug.LogWarning((object) "LocalDisease TEST 12 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 12 SUCCEED");
    if (localDiseaseData.lastKnownInfectedCount != d.lastKnownInfectedCount)
      Debug.LogWarning((object) "LocalDisease TEST 13 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 13 SUCCEED");
    if ((double) localDiseaseData.localAuthLossInfected != (double) d.localAuthLossInfected)
      Debug.LogWarning((object) "LocalDisease TEST 14 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 14 SUCCEED");
    if ((double) localDiseaseData.complianceBin != (double) d.complianceBin)
      Debug.LogWarning((object) "LocalDisease TEST 15 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 15 SUCCEED");
    if (localDiseaseData.genericFlags != d.genericFlags)
      Debug.LogWarning((object) "LocalDisease TEST 16 FAIL");
    else
      Debug.Log((object) "LocalDisease TEST 16 SUCCEED");
  }

  public SPLocalDisease()
  {
    this.mData = new SPLocalDiseaseData();
    this.borderStatusValue = 1f;
    this.localZombieDecayMultiplier = 1f;
    this.apeColonyBubbleCounter = 65500;
    this.fakeNewsWinRollout = 0.0f;
    this.infectedFromCountryNumber = -1;
  }

  private SPDisease spDisease => (SPDisease) this.disease;

  private SPCountry spCountry => (SPCountry) this.country;

  public override void GameUpdate()
  {
    CGameManager.CallExternalMethod("LocalDiseaseExternalHead", World.instance, this.disease, this.country, (LocalDisease) this);
    if (this.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      this.GameUpdate_Vampire();
    else if (this.disease.diseaseType == Disease.EDiseaseType.FAKE_NEWS)
      this.GameUpdate_FakeNews();
    else if (this.disease.diseaseType == Disease.EDiseaseType.CURE)
    {
      this.GameUpdate_Cure();
    }
    else
    {
      World instance = World.instance;
      this.isSuperCureCountry = this.disease.superCureCountry == this.country;
      this.isNexus = this.disease.nexus == this.country;
      SPLocalDiseaseExternal.LocalDiseaseUpdate(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
      CGameManager.CallExternalMethod("LocalDiseaseExternalFirst", World.instance, this.disease, this.country, (LocalDisease) this);
      if (CGameManager.IsFederalScenario("时生虫ReMASTER") && (double) this.disease.customGlobalVariable2 >= 0.10000000149011612)
        this.localInfectiousness = this.disease.globalInfectiousness + this.country.govLocalInfectiousness;
      if (CGameManager.IsFederalScenario("时生虫ReMASTER") && this.country == this.disease.nexus)
        this.localInfectiousness += 12f;
      if (CGameManager.IsFederalScenario("时生虫ReCRAFT") && (double) this.disease.customGlobalVariable2 >= 1.5 && (double) this.disease.globalLandRate <= 0.5 && this.disease.IsTechEvolved("046cab47-ee94-4eac-be04-79d725e13a7a") && this.healthyPopulation == 0L)
        this.localLethality += ModelUtils.FloatRand(15f, 35f);
      if (CGameManager.IsFederalScenario("时生虫ReMASTER") && this.disease.IsTechEvolved("88c66744-64d1-42f0-ae23-cc94a272e7b0") && (double) this.infectedPercent + (double) this.deadPercent > 0.85)
        this.localLethality += 22.5f;
      if (CGameManager.IsFederalScenario("时生虫ReCRAFT") && this.disease.IsTechEvolved("e30b94b6-222a-4e01-9fa9-c66ea8f25f5b"))
        this.localInfectiousness = this.GetAlternativeImportance(8f, 8f, 15f, 15f, 5f, 5f, 2f, 2f);
      if (CGameManager.IsFederalScenario("时生虫ReCRAFT") && (double) this.country.basePopulationDensity < 0.0)
      {
        if (this.disease.IsTechEvolved("e30b94b6-222a-4e01-9fa9-c66ea8f25f5b"))
          this.localInfectiousness = this.GetAlternativeImportance(8f, 8f, 15f, 15f, 5f, 5f, 2f, 2f, true);
        else
          this.localInfectiousness = this.GetAlternativeImportance(30f, 3f, 10f, 10f, 2f, 2f, 2f, 2f, true);
      }
      if (CGameManager.IsFederalScenario("时生虫ReMASTER") && this.country.deadPopulation == this.country.originalPopulation && (double) this.customLocalVariable3 <= 0.5)
      {
        this.customLocalVariable3 = 1f;
        if (this.disease.IsTechEvolved("15981109-184a-469e-9ffc-48637e1db1c1") && this.disease.IsTechEvolved("862360cd-54da-45d6-b476-e6a71b7c17e4") && this.disease.IsTechEvolved("88c66744-64d1-42f0-ae23-cc94a272e7b0") && this.disease.IsTechEvolved("e2633238-6e2b-4c11-a92e-bb904d650600"))
        {
          int num = Mathf.FloorToInt(ModelUtils.FloatRand((float) ((double) this.disease.globalLethality / 33.0 - 1.0), (float) ((double) this.disease.globalLethality / 33.0 + 1.0)));
          if (num <= 0)
            num = 1;
          this.disease.ForceCreateBonusIcon(this.country.id, "death", num.ToString(), "true");
        }
      }
      if (CGameManager.IsFederalScenario("时生虫ReCRAFT") && this.country.deadPopulation == this.country.originalPopulation && (double) this.customLocalVariable3 <= 0.5)
      {
        this.customLocalVariable3 = 1f;
        if (this.disease.IsTechEvolved("6935d441-14cc-461b-98c1-56c3e160a3e5"))
        {
          int num = Mathf.FloorToInt(ModelUtils.FloatRand((float) ((double) this.disease.globalLethalityMax / 33.0 - 1.0), (float) ((double) this.disease.globalLethalityMax / 33.0 + 1.0)));
          if (num <= 0)
            num = 1;
          this.disease.ForceCreateBonusIcon(this.country.id, "death", num.ToString(), "true");
        }
      }
      this.country.GameUpdate();
      if (this.allInfected < 1L && this.country.currentPopulation > 0L)
      {
        foreach (Country neighbour in this.country.neighbours)
        {
          LocalDisease localDisease = this.disease.GetLocalDisease(neighbour);
          if (localDisease.allInfected >= 1L && SPLocalDiseaseExternal.LandTransfer(this.spDisease.diseaseData, this.mData, this.spCountry.mData, ((SPLocalDisease) localDisease).mData, ((SPCountry) neighbour).mData, World.instance.landRoutesMult))
            this.country.TransferPopulation((double) ModelUtils.IntRand(1, 10), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED);
        }
      }
      if (this.isSuperCureCountry && !this.disease.cureFlag && !CGameManager.IsFederalScenario("时生虫ReMASTER") && !CGameManager.IsFederalScenario("时生虫ReCRAFT"))
      {
        if (this.turnsSinceBluePlane++ > 20 && (double) this.disease.globalAwareness > 0.5 && (double) this.country.publicOrder > 0.0 && (double) ModelUtils.FloatRand(0.0f, 1.3f) < ((double) this.disease.totalInfected + (double) this.disease.totalDead * 2.0) / (double) instance.totalPopulation * (double) this.disease.globalPriority / 100.0)
        {
          this.turnsSinceBluePlane = 0;
          Country infectedCountry = this.disease.GetInfectedCountry(this.country);
          if (infectedCountry != null)
          {
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Airplane;
            vehicle.subType = Vehicle.EVehicleSubType.Cure;
            vehicle.actingDisease = this.disease;
            vehicle.SetRoute(this.country, infectedCountry);
            World.instance.AddVehicle(vehicle);
          }
        }
      }
      double externalResistance = (double) this.GetExternalResistance();
      double localInfectivity = (double) this.GetExternalLocalInfectivity();
      SPLocalDiseaseExternal.LocalDiseaseUpdateSecond(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
      CGameManager.CallExternalMethod("LocalDiseaseExternalSecond", World.instance, this.disease, this.country, (LocalDisease) this);
      long num1;
      if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        if (this.country.hasApeColony)
        {
          this.apeActivityLevel += 0.0001f;
          this.DrawApesFromNeighbours();
          if (this.apeInfectedPopulation < 1L)
            this.country.ChangeApeColonyStateF(EApeColonyState.APE_COLONY_NONE);
        }
        if (this.apeInfectedPopulation < 1L && this.apeHealthyPopulation > 0L)
        {
          foreach (Country neighbour in this.country.neighbours)
          {
            LocalDisease localDisease = neighbour.GetLocalDisease(this.disease);
            if (localDisease.apeInfectedPopulation >= 1L && SPLocalDiseaseExternal.ApeTransfer(this.spDisease.diseaseData, this.mData, this.spCountry.mData, ((SPLocalDisease) localDisease).mData, ((SPCountry) neighbour).mData))
              this.country.TransferPopulation((double) ModelUtils.IntRand(5, 15), Country.EPopulationType.APE_HEALTHY, this.disease, Country.EPopulationType.APE_INFECTED);
          }
        }
        SPLocalDiseaseExternal.LocalSimianUpdateSecond(this.mData, this.spCountry.mData, this.spDisease.diseaseData, World.instance.GetHordeFromCountry(this.country) != null ? 1 : 0);
        int num2 = ModelUtils.IntRand(0, 5);
        float num3 = ModelUtils.FloatRand(0.5f, 1.3f);
        if (this.hasDrone)
        {
          this.daysSinceLocalDrone = 0;
          this.disease.daysSinceGlobalDrone = 0;
        }
        if (this.disease.droneAttackFlag > 0 && (double) this.apeTotalAlivePopulation > 0.0 && (double) this.country.publicOrder > 0.15000000596046448)
        {
          this.localDroneCounter += (float) (((double) this.apePriorityLevelLocal / 10.0 + (double) this.localPriority / 25.0) * (this.country.apeHordeMoving ? 5.0 * (1.0 - (double) this.disease.apeSurvival * 0.5) : 1.0) * (this.country.hasApeColony ? 3.0 * (1.0 - (double) this.disease.apeSurvival * 0.699999988079071) : 1.0)) * this.country.publicOrder * this.country.importance * num3 * this.apePopMod;
          if (num2 < 1 && (double) this.localDroneCounter >= (double) this.disease.droneThreshold && (double) this.disease.daysSinceGlobalDrone > (double) Mathf.Clamp((float) (130.0 - ((double) this.localPriority + (double) this.localDroneCounter / 2.0)), 12f, 70f))
            this.CreateDrone();
        }
        float num4 = 0.0f;
        if (this.disease.genSysWorking > 0 && this.apeStatusRampage == 0 && this.country.hasApeLab)
        {
          float num5 = this.disease.difficulty != 0 ? (this.disease.difficulty <= 2 ? (float) ModelUtils.IntRand(0, 2) : (float) ModelUtils.IntRand(0, 3)) : (float) ModelUtils.IntRand(0, 1);
          if (this.masterLabFlag > 0)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
            SPLocalDiseaseExternal.LocalDiseaseApeLabUpdate(this.mData, this.spCountry.mData, this.spDisease.diseaseData, 1);
          }
          else if (this.country.currentPopulation < 1L)
          {
            this.country.labDayCount = 0.0f;
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
            if (this.disease.labDestroyDnaFlag > 0)
              this.disease.evoPoints += 2;
          }
          else if ((double) this.country.publicOrder < 1.0 / 1000.0)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
            this.country.labDayCount += num5;
            num4 = 1f;
          }
          else if (this.apeHealthyPopulation < 1L && this.apeInfectedPopulation > 0L && this.country.hasApeColony)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
            this.country.labDayCount += num5;
            num4 = 1f;
          }
          else if (this.apeHealthyPopulation + this.apeInfectedPopulation > 0L && (double) this.country.labDayCount < 10.0)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
            this.country.labDayCount += num5;
          }
          else if (this.apeHealthyPopulation + this.apeInfectedPopulation < 1L)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
            this.country.labDayCount += num5;
            num4 = 1f;
          }
          else if (this.apeHealthyPopulation + this.apeInfectedPopulation > 0L)
          {
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
            SPLocalDiseaseExternal.LocalDiseaseApeLabUpdate(this.mData, this.spCountry.mData, this.spDisease.diseaseData, 0);
          }
          else
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
          if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && (double) this.country.labDayCount > 15.0)
          {
            this.localCureResearch += (float) ((double) this.disease.labBaseResearch * (double) ModelUtils.FloatRand(0.1f, 1f) * 0.05000000074505806) * Mathf.Clamp(this.country.publicOrder * 2f, 0.1f, 1f);
            if (ModelUtils.IntRand(0, (int) Mathf.Max(1f, (float) (20.0 - (double) this.disease.difficultyVariable / 2.0 - (double) this.disease.globalPriority / 10.0))) < 1 && (double) num4 > 0.0 && this.apeStatusRampage < 1)
            {
              Country suitableApeLabCountry = this.disease.GetSuitableApeLabCountry();
              if (suitableApeLabCountry != null)
              {
                Vehicle vehicle = Vehicle.Create();
                vehicle.type = Vehicle.EVehicleType.Airplane;
                vehicle.subType = Vehicle.EVehicleSubType.ApeLabPlane;
                vehicle.actingDisease = this.disease;
                vehicle.SetRoute(this.country, suitableApeLabCountry);
                World.instance.AddVehicle(vehicle);
                World.instance.TrackLabPlane(vehicle);
                this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_NONE);
                this.country.labDayCount = -1f;
              }
              else
                this.localCureResearch += this.disease.labBaseResearch * ModelUtils.FloatRand(0.5f, 1f) * Mathf.Clamp(this.country.publicOrder * 2f, 0.1f, 1f);
            }
          }
        }
        if (this.apeStatusRampage > 0)
        {
          SPLocalDiseaseExternal.LocalDiseaseApeRampageUpdate(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
          if (this.apeRampageMission == 1 && (double) this.country.labDayCount < 1.0)
          {
            this.apeStatusRampage = 0;
            this.apeRampageMission = 0;
            this.disease.labCounter = 0;
            if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && this.disease.difficulty > 1)
              this.disease.globalCureResearch *= 0.9f;
            else if (this.disease.difficulty < 1)
              this.disease.globalCureResearch *= 0.7f;
            else if (this.disease.difficulty < 2)
              this.disease.globalCureResearch *= 0.75f;
            else
              this.disease.globalCureResearch *= 0.86f;
            this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
            World.instance.AddAchievement(EAchievement.A_gosimianfaeces);
            this.apeActivityImpact += 0.3f + this.localApeStrength;
            this.masterLabFlag = 0;
            this.country.labDayCount = 0.0f;
            if (this.disease.labDestroyDnaFlag > 0)
              this.disease.evoPoints += 2;
          }
          else if (this.apeRampageMission == 2 && (double) this.apeHealthyPercent <= 0.0)
          {
            this.apeStatusRampage = 0;
            this.apeRampageMission = 0;
          }
          else if (++this.apeRampageCounter == 10 && this.apeRampageMission != 1 || this.apeInfectedPopulation < 1L || (double) this.apeInfectedPopulation < (double) this.apeI2D)
          {
            this.apeStatusRampage = 0;
            this.apeRampageMission = 0;
          }
        }
        if ((double) this.I2DAdditional > 0.0)
        {
          this.country.TransferPopulation((double) this.I2DAdditional, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
          this.I2DAdditional = 0.0f;
        }
        this.country.TransferPopulation((double) this.H2I + (double) this.infectedPopulationOverride, Country.EPopulationType.HEALTHY_SUSCEPTIBLE, this.disease, Country.EPopulationType.INFECTED);
        double number = (double) this.I2D + (double) this.deadPopulationOverride;
        if (number < 0.0)
          number = 0.0;
        this.country.TransferPopulation(this.country.TransferPopulation(number, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD), Country.EPopulationType.HEALTHY_SUSCEPTIBLE, this.disease, Country.EPopulationType.DEAD);
        this.country.TransferPopulation((double) this.susceptibleH2D, Country.EPopulationType.HEALTHY_SUSCEPTIBLE, this.disease, Country.EPopulationType.DEAD);
        this.country.TransferPopulation((double) this.immuneH2D, Country.EPopulationType.HEALTHY_IMMUNE, this.disease, Country.EPopulationType.DEAD);
        int actual_ape_H2I = (int) ((double) this.apeH2I - this.country.TransferPopulation((double) this.apeH2I, Country.EPopulationType.APE_HEALTHY, this.disease, Country.EPopulationType.APE_INFECTED));
        num1 = (long) actual_ape_H2I;
        this.country.TransferPopulation(this.country.TransferPopulation((double) this.apeI2D, Country.EPopulationType.APE_INFECTED, this.disease, Country.EPopulationType.APE_DEAD), Country.EPopulationType.APE_HEALTHY, this.disease, Country.EPopulationType.APE_DEAD);
        this.country.TransferPopulation((double) this.apeH2D, Country.EPopulationType.APE_HEALTHY, this.disease, Country.EPopulationType.APE_DEAD);
        if (this.country.apeOriginalPopulation > 0L)
          SPLocalDiseaseExternal.LocalDiseaseApeActivityUpdate(this.mData, this.spCountry.mData, this.spDisease.diseaseData, actual_ape_H2I);
        else
          Debug.LogError((object) "Country original ape population is 0. Should always be non-zero.");
        if (this.apeFirstInfectionEffectDone == 0 && this.apeInfectedPopulation > 0L)
          this.apeFirstInfectionEffectDone = 1;
        if (this.country.hasApeColony)
        {
          if (this.apeColonyBubbleCounter > 500)
            this.apeColonyBubbleCounter = Mathf.Min(this.apeColonyBubbleCounter, 15 + ModelUtils.IntRand(0, 7));
          if (--this.apeColonyBubbleCounter < 2)
          {
            this.apeColonyBubbleCounter = 37 + ModelUtils.IntRand(0, (int) (12.0 + (double) this.disease.difficultyVariable));
            if (this.country.hasApeColony)
              this.SpawnApeColonyBubble();
          }
        }
        else
          this.apeColonyBubbleCounter = 1000;
      }
      else
      {
        if ((double) this.I2DAdditional > 0.0)
        {
          this.country.TransferPopulation((double) this.I2DAdditional, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
          this.I2DAdditional = 0.0f;
        }
        if (this.disease.diseaseType == Disease.EDiseaseType.NECROA && this.disease.zdayDone && this.zombie < 1000L)
        {
          foreach (Country neighbour in this.country.neighbours)
          {
            LocalDisease localDisease = this.disease.GetLocalDisease(neighbour);
            if (localDisease.zombie >= 1L && SPLocalDiseaseExternal.ZombieTransfer(this.spDisease.diseaseData, this.mData, this.spCountry.mData, ((SPLocalDisease) localDisease).mData, ((SPCountry) neighbour).mData))
              this.country.TransferPopulation(this.country.TransferPopulation((double) ModelUtils.IntRand(1, 10), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE), Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE);
          }
        }
        num1 = (long) ((double) this.H2I + (double) this.infectedPopulationOverride - this.country.TransferPopulation((double) this.H2I + (double) this.infectedPopulationOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED));
        double number1 = (double) this.I2D + (double) this.deadPopulationOverride;
        if (number1 < 0.0)
          number1 = 0.0;
        if (this.disease.zday)
        {
          float num6 = Mathf.Min(1f, ((float) this.disease.zdayCounter - 2f) / (float) this.disease.zdayLength);
          this.country.TransferPopulation(this.country.TransferPopulation(number1 * (double) num6, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
          this.country.TransferPopulation(this.country.TransferPopulation(number1 * (1.0 - (double) num6), Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
        }
        else if (this.disease.zdayDone)
        {
          this.country.TransferPopulation(this.country.TransferPopulation(number1, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
        }
        else
        {
          double number2 = this.country.TransferPopulation(number1, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
          if (number2 > 0.0)
          {
            double num7 = number2;
            double num8 = this.country.TransferPopulation(number2, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
            if (num1 > 0L)
              Debug.Log((object) ("country: " + this.country.id + " dead: " + (object) (num7 - num8)));
            num1 += (long) (num7 - num8);
          }
        }
      }
      if (!this.deadBonusShown && (this.disease.zday || this.disease.zdayDone) && this.country.deadPopulation + this.country.totalZombie >= this.country.originalPopulation - 1L && (this.killedPopulation > 0L || this.zombie > 0L))
      {
        this.deadBonusShown = true;
        instance.AddBonusIcon(new BonusIcon(this.disease, this.country, BonusIcon.EBonusIconType.DEATH));
      }
      if ((double) this.Z2D < 0.0)
        this.Z2D = 0.0f;
      if ((double) this.Z2DOverride < 0.0)
        this.Z2DOverride = 0.0f;
      this.country.TransferPopulation((double) this.D2Z + (double) this.D2ZOverride, Country.EPopulationType.DEAD, this.disease, Country.EPopulationType.ZOMBIE);
      this.country.TransferPopulation((double) this.I2Z + (double) this.I2ZOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE);
      this.country.TransferPopulation((double) this.H2Z + (double) this.H2ZOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
      this.country.TransferPopulation((double) this.Z2D + (double) this.Z2DOverride, Country.EPopulationType.ZOMBIE, this.disease, Country.EPopulationType.DEAD);
      if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY_SUSCEPTIBLE, this.disease, Country.EPopulationType.DEAD);
        this.country.healthyPopulation = this.country.healthyPopulationImmune + this.country.healthyPopulationSusceptible;
      }
      else
        this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
      this.infectedPopulationOverride = this.deadPopulationOverride = 0.0f;
      this.D2ZOverride = this.I2ZOverride = this.H2ZOverride = this.H2DOverride = this.Z2DOverride = 0.0f;
      if (num1 >= 0L && num1 < this.allInfected - this.prevInfected)
        num1 = this.allInfected - this.prevInfected;
      this.disease.infectedThisTurn += num1;
      this.disease.deadThisTurn += this.killedPopulation - this.prevKilled;
      this.disease.zombiesThisTurn += this.zombie - this.prevZombie;
      this.disease.infectedApesThisTurn += this.apeInfectedPopulation - this.prevInfectedApes;
      this.prevInfected = this.allInfected;
      this.prevZombie = this.zombie;
      this.prevKilled = this.killedPopulation;
      this.prevInfectedApes = this.apeInfectedPopulation;
      if (this.allInfected + this.zombie > 0L && !this.infectBonusShown && !CGameManager.IsFederalScenario("PISMG"))
      {
        this.infectBonusShown = true;
        ++this.disease.numInfectBubblesWithoutTouch;
        BonusIcon bonusIcon = new BonusIcon(this.disease, this.country, BonusIcon.EBonusIconType.INFECT);
        instance.AddBonusIcon(bonusIcon);
      }
      if (CGameManager.IsFederalScenario("时生虫ReMASTER") || CGameManager.IsFederalScenario("时生虫ReCRAFT"))
      {
        if (this.infectBonusShown && this.allInfected <= 0L && (double) this.customLocalVariable1 <= 0.5)
          this.customLocalVariable1 = 1f;
        if ((double) this.customLocalVariable1 >= 0.99500000476837158 && (double) this.customLocalVariable1 <= 1.0049999952316284 && this.allInfected > 0L)
        {
          this.customLocalVariable1 = 2f;
          int num9 = Mathf.FloorToInt((double) this.disease.globalSeverity > 0.0 ? (float) Math.Log((double) this.disease.globalSeverity / 1.3300000429153442) : 0.0f);
          if (num9 <= 1)
            num9 = 1;
          this.disease.ForceCreateBonusIcon(this.country.id, "INFECT", num9.ToString(), "true");
        }
      }
      int num10 = Mathf.Clamp((int) (((double) this.country.medicalBudget + 1.0) / 1000.0), 1, 10);
      this.flaskBroken = Mathf.Clamp((int) ((1.0 - (double) this.country.publicOrder) * (double) num10), 0, 10);
      this.flaskResearched = Mathf.Clamp(Mathf.CeilToInt(this.cureResearchAllocation * (float) num10), 0, num10 - this.flaskBroken);
      if ((this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE) && this.disease.genSysWorking > 0)
        this.flaskResearched = num10 - this.flaskBroken;
      if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && this.disease.genSysWorking > 0)
        this.flaskResearched = (num10 - this.flaskBroken) / 2;
      this.flaskEmpty = num10 - this.flaskBroken - this.flaskResearched;
      this.originalPopulation = this.country.originalPopulation;
      this.uninfectedPopulation = this.uninfected;
      CGameManager.CallExternalMethod("LocalDiseaseExternalTail", World.instance, this.disease, this.country, (LocalDisease) this);
    }
  }

  public override float GetPublicOrderEffect()
  {
    return SPCountryExternal.GetPublicOrderEffect(this.mData);
  }

  public void GameUpdate_Cure()
  {
    this.isSuperCureCountry = this.disease.superCureCountry == this.country;
    this.isNexus = this.disease.nexus == this.country;
    this.country.GameUpdate();
    if (this.disease.HasFlag(Disease.EGenericDiseaseFlag.CheatAdvancePlanning))
      this.hasIntel = true;
    this.diseaseLength = 21;
    if (this.disease.isPrion)
    {
      if (this.disease.preSimulate == 0)
        this.diseaseLength = 21 * this.disease.prionIncubationMonths;
      else
        this.diseaseLength = 21 * Mathf.RoundToInt((float) (this.disease.preSimMAX / 2));
      if (this.disease.cureFlag)
        this.diseaseLength = 21 * Mathf.RoundToInt((float) this.disease.prionIncubationMonths * 0.75f);
    }
    if (this.disease.isFungus)
    {
      if (this.disease.cureFlag)
        this.diseaseLength = 21;
      else
        this.diseaseLength = 42;
    }
    if (!this.disease.cureFlag)
      this.diseaseLength *= this.disease.diseaseLengthMulti;
    GovernmentActionManager governmentActionManager = World.instance.governmentActionManager;
    for (int index = 0; index < this.country.actionsSpecialInterest.Count; ++index)
    {
      string name = this.country.actionsSpecialInterest[index];
      GovernmentAction action = governmentActionManager.FindAction(name, this.disease);
      if (action == null)
        Debug.LogError((object) ("Unable to find '" + name + "'"));
      else if (action.removable)
      {
        bool flag1 = false;
        bool flag2 = false;
        if (!this.lockdownAAActive && action.conditionTechResearchedArray.Length != 0)
        {
          foreach (string conditionTechResearched in action.conditionTechResearchedArray)
          {
            if (!this.disease.IsTechEvolved(conditionTechResearched))
            {
              flag1 = true;
              break;
            }
          }
        }
        if (!flag1 && !governmentActionManager.MeetsOverrideConditions(action, (LocalDisease) this))
        {
          if (!governmentActionManager.MeetsPriorityCondition(action, (LocalDisease) this))
            flag2 = true;
          else if (!governmentActionManager.MeetsConditions(action, (LocalDisease) this))
            flag2 = true;
        }
        if (flag2 | flag1)
        {
          Debug.Log((object) ("Removing government action " + action.actionName));
          governmentActionManager.PerformGovernmentAction(this.country, this.disease, action, true);
          if ((double) action.economicDamagePerTurn > 0.0)
          {
            if (action.conditionVariable == "land_border_override")
            {
              this.compliance += this.complianceDropLand * this.disease.complianceReturnPerc;
              this.complianceDropLand = 0.0f;
            }
            if (action.conditionVariable == "airport_override")
            {
              this.compliance += this.complianceDropAir * this.disease.complianceReturnPerc;
              this.complianceDropAir = 0.0f;
            }
            if (action.conditionVariable == "port_override")
            {
              this.compliance += this.complianceDropSea * this.disease.complianceReturnPerc;
              this.complianceDropSea = 0.0f;
            }
            if (action.conditionVariable == "lockdown_override")
            {
              this.compliance += this.complianceDropLockdown * this.disease.complianceReturnPerc;
              this.complianceDropLockdown = 0.0f;
            }
          }
        }
      }
    }
    this.airportOverride = this.portOverride = this.landBorderOverride = this.lockdownOverride = true;
    if (!this.disease.HasFlag(Disease.EGenericDiseaseFlag.GeneCheckpointEnforcer) && (this.disease.HasFlag(Disease.EGenericDiseaseFlag.GeneQuarantineCoordinator) ? (!this.hasInfectedConnection ? 1 : 0) : 1) != 0 && this.hasIntel && (double) this.infectedPercent <= 0.0)
      this.airportOverride = this.portOverride = this.landBorderOverride = this.lockdownOverride = false;
    SPLocalDiseaseExternal.LocalDiseaseUpdate_Cure(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    this.disease.SetAACostAdditional(EAbilityType.raise_priority, this.disease.quarantinesActiveCount);
    if (this.hasTeam && this.disease.IsTechEvolved("Field_Reseach_Experts") && (double) this.infectedPercent > 0.0 && this.disease.vaccineStage == Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT)
    {
      int num = this.lastLabBubbleTurn > 0 ? this.disease.turnNumber - this.lastLabBubbleTurn : -1;
      if (num == -1)
        num = this.isNexus ? 18 : 25;
      if ((double) num >= (this.isNexus ? 34.0 : 40.0) - (double) Mathf.Min(20f, (float) this.teamTurnsSinceArrived * 0.5f))
        this.SpawnVaccineBubble();
    }
    this.infectedPercWeeklyChange.Push(this.infectedPercent);
    this.deadPercWeeklyChange.Push(this.deadPercent);
    float connections_total_local_priority = 0.0f;
    float connections_weightings = 0.0f;
    float connections_highest_priority = 0.0f;
    float num1 = 1f;
    this.hasInfectedConnection = false;
    if (!this.AreBordersOpen())
      num1 = 0.5f;
    foreach (Country neighbour in this.country.neighbours)
    {
      LocalDisease localDisease = neighbour.GetLocalDisease(this.disease);
      float b = Mathf.Max(localDisease.standardLocalPriority, localDisease.connectedLocalPriority / 2f) * num1;
      connections_total_local_priority += b;
      connections_highest_priority = Mathf.Max(connections_highest_priority, b);
      connections_weightings += 0.6f;
      if ((double) localDisease.infectedPercent > 0.0)
        this.hasInfectedConnection = true;
    }
    foreach (TravelRoute seaRoute in this.country.seaRoutes)
    {
      ProcessRoute(seaRoute.destination, seaRoute, true);
      ProcessRoute(seaRoute.source, seaRoute, true);
    }
    foreach (TravelRoute airRoute in this.country.airRoutes)
    {
      ProcessRoute(airRoute.destination, airRoute, false);
      ProcessRoute(airRoute.source, airRoute, false);
    }
    if ((double) connections_total_local_priority == 0.0)
    {
      this.connectedLocalPriority = 0.0f;
    }
    else
    {
      connections_weightings = Mathf.Max(1f, connections_weightings);
      this.connectedLocalPriority = Mathf.Floor(Mathf.Min(connections_highest_priority * 0.7f, connections_total_local_priority / connections_weightings));
      this.connectedLocalPriority *= this.tempConnectedLocalPriorityMultiplier;
    }
    this.connectedLocalPriority *= Mathf.Pow(1f - this.country.healthyRecoveredPercent, 2f);
    if ((double) this.compliance <= 0.0 && this.disease.HasFlag(Disease.EGenericDiseaseFlag.GeneEconomicForecaster))
    {
      ActiveAbility ability = CGameManager.game.Abilities[EAbilityType.economic_support];
      if (this.disease.IsAbilityActive("economic_support") && (double) this.disease.evoPoints >= (double) this.disease.GetActiveAbilityCost(ability, EAbilityType.economic_support))
        this.GiveEconomicAid(ability);
    }
    SPLocalDiseaseExternal.LocalDiseaseUpdate_Cure2(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    double localInfectiousness = (double) this.localInfectiousness;
    float localLethality = this.localLethality;
    float num2 = (float) (localInfectiousness / 100.0);
    float num3 = 0.0f;
    if (this.country.currentPopulation - this.country.healthyRecoveredPopulation > 0L)
      num3 = this.country.basePopulationDensity / ((float) this.country.originalPopulation / (float) (this.country.currentPopulation - this.country.healthyRecoveredPopulation));
    float num4 = 0.0f;
    if (this.country.deadPopulation > 0L)
      num4 = this.localCorpseTransmission / ((float) this.country.originalPopulation / (float) this.country.deadPopulation);
    if ((double) num4 < 0.0)
      num4 = 0.0f;
    float num5 = 0.0f;
    if (this.country.deadPopulation > 0L)
      num5 = this.localDeadBodyTransmission;
    float num6 = num5 * num2;
    float num7 = (float) ((double) num2 * (double) num3 + (double) num2 * (double) num4);
    float num8 = localLethality / 100f;
    this.localInfectionRate = num7;
    int num9 = 10;
    int num10 = 10;
    int num11 = 10;
    int num12 = 20000;
    float a = 0.0f;
    bool flag = this.disease.IsTechEvolved("Disease_Containment_Experts");
    if (!this.hasIntel && this.disease.difficulty > 0)
    {
      num9 *= 2;
      num10 *= 2;
      num11 *= 2;
    }
    if (this.unrestActive)
    {
      num9 *= 5;
      num10 *= 5;
      num11 *= 5;
    }
    if (this.hasTeam & flag)
    {
      num9 = 0;
      num10 = 0;
      num11 = 0;
    }
    if (this.disease.isNanovirus && this.disease.nanovirusPauseTimer > 0)
    {
      num9 = 0;
      num10 = 0;
      num11 = 0;
    }
    if (this.country.airport && this.AreAirportsOpen())
    {
      for (int index = 0; index < this.country.airRoutes.Count; ++index)
      {
        TravelRoute airRoute = this.country.airRoutes[index];
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
        if (s != null && country != null)
        {
          LocalDisease localDisease1 = s.GetLocalDisease(this.disease);
          LocalDisease localDisease2 = country.GetLocalDisease(this.disease);
          if ((localDisease1.AreAirportsOpen() || localDisease1.unrestActive) && (localDisease2.AreAirportsOpen() || localDisease2.unrestActive) && (country != this.disease.hqCountry || this.disease.turnNumber >= ModelUtils.IntRand(10, 20)) && (!flag || (!localDisease2.hasTeam || this.unrestActive) && (!localDisease1.hasTeam || this.unrestActive)))
          {
            float num13 = airRoute.frequency * (1f / (float) s.originalPopulation * (float) s.currentPopulation);
            airRoute.currentTime += num13;
            if ((double) airRoute.currentTime >= 1.0)
            {
              --airRoute.currentTime;
              if (country != null)
              {
                float num14 = ModelUtils.IntRand(0, 100) >= 1 ? (ModelUtils.IntRand(0, 100) >= 5 ? 1f : 100f) : 10000f;
                float airTransmission = this.disease.airTransmission;
                if (this.disease.HasFlag(Disease.EGenericDiseaseFlag.GeneAirportController))
                  airTransmission *= 0.85f;
                double num15 = 100.0 * (double) localDisease1.infectedPercent * (double) num9 * (double) num14;
                float num16 = (float) (num15 * 1.0 * 100.0);
                float num17 = (float) num15 * this.disease.globalAirRate * airTransmission;
                if (this.disease.nexus == s)
                {
                  num17 *= 10f;
                  float num18 = num16 * 10f;
                  a = Mathf.Min((float) ModelUtils.IntRand(0, 20), Mathf.Max(0.0f, (float) localDisease1.infectedPopulation - 2f));
                  if ((double) localDisease2.infectedPopulation + (double) a >= (double) localDisease1.infectedPopulation)
                    a = 0.0f;
                }
                if (this.hasIntel && (double) this.disease.airTravelRestriction == 1.0 && (double) num17 >= 1.0 && (double) ModelUtils.FloatRand(0.0f, 1f) <= (double) this.disease.airScreeningChance * (1.0 - (double) this.infectedPercent) * (double) this.complianceMulti && !this.unrestActive)
                  num17 = 0.0f;
                if ((double) num17 >= 1.0)
                {
                  int number = ModelUtils.IntRand((int) (1.0 + (double) a), (int) ((double) Mathf.Max(0, ModelUtils.IntRand(-5, 5)) + (double) Mathf.Min(200f, (float) (1.0 + (double) localDisease1.localInfectedPercent * (double) num12))));
                  if (this.disease.preSimulate == 0)
                  {
                    Vehicle vehicle = Vehicle.Create();
                    vehicle.type = Vehicle.EVehicleType.Airplane;
                    vehicle.subType = Vehicle.EVehicleSubType.Normal;
                    vehicle.SetRoute(s, country);
                    vehicle.AddInfected(this.disease, number);
                    if (!this.disease.easyIntel && !this.hasIntel)
                      vehicle.SetHiddenInfectedVehicle(true);
                    World.instance.AddVehicle(vehicle);
                  }
                  else
                  {
                    if ((double) localDisease2.infectedPercent <= 0.0)
                    {
                      localDisease2.infectionMethod = Country.EInfectionMethod.IM_PLANE;
                      localDisease2.infectedFromCountry = s;
                    }
                    Debug.LogFormat("pre_simulate moving " + (object) number + " healthy people in " + s.name + " to infected in " + country.name + " by plane.\n");
                    s.TransferPopulationNoPipe((long) number, Country.EPopulationType.HEALTHY, country, Country.EPopulationType.INFECTED, this.disease);
                    country.dispatchedHiddenInfectedFlights.Add(s);
                  }
                }
                else if (this.disease.preSimulate == 0)
                {
                  Vehicle vehicle = Vehicle.Create();
                  vehicle.type = Vehicle.EVehicleType.Airplane;
                  vehicle.subType = Vehicle.EVehicleSubType.Normal;
                  vehicle.SetRoute(s, country);
                  World.instance.AddVehicle(vehicle);
                }
              }
            }
          }
        }
      }
    }
    foreach (Country neighbour1 in this.country.neighbours)
    {
      LocalDisease localDisease3 = neighbour1.GetLocalDisease(this.disease);
      if (localDisease3.infectedPopulation >= 1L && (this.country != this.disease.hqCountry || this.disease.turnNumber >= 10) && (!(localDisease3.hasTeam & flag) || this.unrestActive))
      {
        float num19 = 0.0f;
        float num20 = 0.0f;
        foreach (Country neighbour2 in neighbour1.neighbours)
        {
          LocalDisease localDisease4 = neighbour2.GetLocalDisease(this.disease);
          ++num20;
          if ((double) localDisease4.infectedPercent > 0.0)
            ++num19;
        }
        float num21 = (double) this.infectedPercent >= 9.9999997473787516E-05 * (double) (1 + this.country.countrySize) ? 1f : Mathf.Max(0.0f, num19 / num20);
        float num22 = ModelUtils.IntRand(0, 1000) >= 1 ? (ModelUtils.IntRand(0, 100) >= 1 ? 1f : 100f) : 5000f;
        float landTransmission = this.disease.landTransmission;
        if (this.disease.HasFlag(Disease.EGenericDiseaseFlag.GeneLandBorderController))
          landTransmission *= 0.85f;
        float num23 = 1f;
        float num24 = 1f;
        if ((double) this.borderStatusValue < 1.0 && !this.unrestActive)
          num23 = 0.005f + this.deadPercent;
        if ((double) localDisease3.borderStatusValue < 1.0 && !localDisease3.unrestActive)
          num24 = 0.005f + localDisease3.deadPercent;
        float num25 = num24 * num23;
        float num26 = (float) (100.0 * (double) localDisease3.infectedPercent * (double) num25 * ((double) num10 * 0.10000000149011612)) * num22 * this.disease.globalLandRate * landTransmission;
        if (!neighbour1.hasPorts && !neighbour1.hasAirports)
        {
          float num27 = Mathf.Pow(1f - num21, 3f);
          num26 *= 10f * num27;
          a = (float) ModelUtils.IntRand(0, 50);
          a = Mathf.Min(a, Mathf.Max(0.0f, (float) localDisease3.infectedPopulation - 2f));
          a *= num27;
          if ((double) this.infectedPopulation + (double) a >= (double) localDisease3.infectedPopulation)
            a = 0.0f;
        }
        float num28 = 1f;
        if (localDisease3.isRussia)
        {
          num26 *= 0.35f;
          num28 = 0.5f;
        }
        if (this.hasIntel && (double) this.disease.landTravelRestriction == 1.0 && (double) num26 >= 1.0 && (double) ModelUtils.FloatRand(0.0f, 1f) <= (double) this.disease.landScreeningChance * (1.0 - (double) this.infectedPercent) * (double) this.complianceMulti * (double) num28 && !this.unrestActive)
          num26 = 0.0f;
        if ((double) num26 >= 1.0)
        {
          if (localDisease3.infectedPopulation > this.infectedPopulation)
          {
            this.vehicleInfectionBoostChance += 0.6f * (float) (1L - this.infectedPopulation / localDisease3.infectedPopulation) * num28;
            this.turnVehicleArrived = this.disease.turnNumber;
          }
          this.ctFailureChance += 0.2f * num25;
          int number = ModelUtils.IntRand((int) (1.0 + (double) a), (int) ((double) num25 * ((double) Mathf.Max(0, ModelUtils.IntRand(-5, 5)) + (double) Mathf.Min(200f, (float) (1.0 + (double) localDisease3.localInfectedPercent * (double) num12)))));
          if ((double) this.infectedPercent <= 0.0)
          {
            this.infectionMethod = Country.EInfectionMethod.IM_LAND;
            this.infectedFromCountry = neighbour1;
          }
          this.country.TransferPopulation((double) number, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED);
        }
      }
    }
    if (this.country.hasPorts && this.ArePortsOpen())
    {
      for (int index = 0; index < this.country.seaRoutes.Count; ++index)
      {
        TravelRoute seaRoute = this.country.seaRoutes[index];
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
        if (s != null && country != null)
        {
          LocalDisease localDisease5 = s.GetLocalDisease(this.disease);
          LocalDisease localDisease6 = country.GetLocalDisease(this.disease);
          if ((localDisease6.ArePortsOpen() || localDisease6.unrestActive) && (localDisease5.ArePortsOpen() || localDisease5.unrestActive) && (country != this.disease.hqCountry || this.disease.turnNumber >= ModelUtils.IntRand(10, 20)) && (!(localDisease6.hasTeam & flag) || this.unrestActive) && (!(localDisease5.hasTeam & flag) || this.unrestActive))
          {
            float num29 = seaRoute.frequency * (1f / (float) s.originalPopulation * (float) s.currentPopulation);
            seaRoute.currentTime += num29;
            if ((double) seaRoute.currentTime >= 1.0)
            {
              --seaRoute.currentTime;
              if (country != null)
              {
                float num30 = ModelUtils.IntRand(0, 100) >= 1 ? (ModelUtils.IntRand(0, 100) >= 5 ? 1f : 100f) : 10000f;
                float seaTransmission = this.disease.seaTransmission;
                if (this.disease.HasFlag(Disease.EGenericDiseaseFlag.GenePortController))
                  seaTransmission *= 0.85f;
                float num31 = 100f * localDisease5.infectedPercent * (float) num11 * num30 * this.disease.globalSeaRate * seaTransmission;
                if (this.disease.nexus == s)
                {
                  num31 *= 10f;
                  a = Mathf.Min((float) ModelUtils.IntRand(0, 20), Mathf.Max(0.0f, (float) localDisease5.infectedPopulation - 2f));
                  if ((double) localDisease6.infectedPopulation + (double) a >= (double) localDisease5.infectedPopulation)
                    a = 0.0f;
                }
                if (this.hasIntel && (double) this.disease.oceanTravelRestriction == 1.0 && (double) num31 >= 1.0 && (double) ModelUtils.FloatRand(0.0f, 1f) <= (double) this.disease.seaScreeningChance * (1.0 - (double) this.infectedPercent) * (double) this.complianceMulti && !this.unrestActive)
                  num31 = 0.0f;
                if ((double) num31 >= 1.0)
                {
                  int number = ModelUtils.IntRand((int) (1.0 + (double) a), (int) ((double) Mathf.Max(0, ModelUtils.IntRand(-5, 15)) + (double) Mathf.Min(300f, (float) (1.0 + (double) localDisease5.localInfectedPercent * (double) num12))));
                  if (this.disease.preSimulate == 0)
                  {
                    Vehicle vehicle = Vehicle.Create();
                    vehicle.type = Vehicle.EVehicleType.Boat;
                    vehicle.subType = Vehicle.EVehicleSubType.Normal;
                    vehicle.SetRoute(s, country);
                    vehicle.AddInfected(this.disease, number);
                    if (!this.disease.easyIntel && !this.hasIntel)
                      vehicle.SetHiddenInfectedVehicle(true);
                    World.instance.AddVehicle(vehicle);
                  }
                  else
                  {
                    if ((double) localDisease6.infectedPercent <= 0.0 && number > 0)
                    {
                      localDisease6.infectionMethod = Country.EInfectionMethod.IM_BOAT;
                      localDisease6.infectedFromCountry = s;
                    }
                    Debug.LogFormat("pre_simulate moving %u healthy people in %s to infected in %s by boat.\n", (object) number, (object) s.name, (object) country.name);
                    s.TransferPopulationNoPipe((long) number, Country.EPopulationType.HEALTHY, country, Country.EPopulationType.INFECTED, this.disease);
                    country.dispatchedHiddenInfectedBoats.Add(s);
                  }
                }
                else if (this.disease.preSimulate == 0)
                {
                  Vehicle vehicle = Vehicle.Create();
                  vehicle.type = Vehicle.EVehicleType.Boat;
                  vehicle.subType = Vehicle.EVehicleSubType.Normal;
                  vehicle.SetRoute(s, country);
                  World.instance.AddVehicle(vehicle);
                }
              }
            }
          }
        }
      }
    }
    if (this.mColoredBubble == Country.EGenericCountryFlag.None && this.hasIntel && this.disease.turnNumber - this.mColoredBubbleLastCreated > 5)
    {
      if (!this.HasFlag(Country.EGenericCountryFlag.GreenBubble) && this.country == this.disease.nexus)
        this.SpawnColoredBubble(Country.EGenericCountryFlag.GreenBubble);
      else if (!this.HasFlag(Country.EGenericCountryFlag.GreenBubble) && !this.HasFlag(Country.EGenericCountryFlag.RedBubble) && (double) this.infectedPercent > 0.0)
        this.SpawnColoredBubble(Country.EGenericCountryFlag.RedBubble);
      else if ((!this.HasFlag(Country.EGenericCountryFlag.RedBubble) || this.HasFlag(Country.EGenericCountryFlag.OrangeBubble) || (double) this.infectedPercent <= (double) this.country.medicalCapacity * 2.0) && this.HasFlag(Country.EGenericCountryFlag.RedBubble) && this.HasFlag(Country.EGenericCountryFlag.OrangeBubble) && !this.HasFlag(Country.EGenericCountryFlag.BlackBubble))
      {
        double deadPercent = (double) this.deadPercent;
      }
    }
    float infectedPopulation = (float) this.infectedPopulation;
    if (!this.disease.isNanovirus || this.disease.nanovirusPauseTimer <= 0 || this.disease.cureFlag)
    {
      ++this.temporalCounter;
      if (this.temporalCounter >= this.diseaseLength / this.infectedTemporal.Size())
      {
        this.temporalCounter = 0;
        long val = this.infectedTemporal.AdvanceBucket();
        if (val > 0L)
          this.infectedTemporal.Add(this.infectedTemporal.Size() - 1, val);
      }
      long val1 = this.infectedPopulation - this.lastKnownInfectedCount;
      if (val1 > 0L)
        this.infectedTemporal.Add(0, val1);
      else if (val1 < 0L)
      {
        long num32 = -val1;
        for (int index = 0; index < this.infectedTemporal.Size() && num32 > 0L; ++index)
        {
          if (this.infectedTemporal.Get(index) >= num32)
          {
            this.infectedTemporal.Add(index, -num32);
            num32 = 0L;
          }
          else
          {
            long num33 = this.infectedTemporal.Get(index);
            num32 -= num33;
            this.infectedTemporal.Add(index, -num33);
          }
        }
      }
      for (int index = 0; index < this.infectedTemporal.Size(); ++index)
      {
        if (this.infectedTemporal.Get(index) > 0L)
        {
          this.infectedTemporal.Get(index);
          float num34 = Mathf.Clamp(num8 * this.disease.curveDie[index] * ModelUtils.FloatRand(0.5f, 1.5f), 0.0f, 1f);
          long number1 = (long) Math.Round((double) this.infectedTemporal.Get(index) * (double) num34);
          if (number1 < 0L)
            number1 = 0L;
          if (number1 > this.infectedTemporal.Get(index))
            number1 = this.infectedTemporal.Get(index);
          this.infectedTemporal.Add(index, -number1);
          this.country.TransferPopulationNoPipe(number1, Country.EPopulationType.INFECTED, this.country, Country.EPopulationType.DEAD, this.disease);
          this.I2D += (float) number1;
          float num35 = Mathf.Clamp(this.disease.curveHeal[index] * ModelUtils.FloatRand(0.5f, 1f), 0.0f, 1f);
          long number2 = (long) Math.Round((double) this.infectedTemporal.Get(index) * (double) num35);
          if (number2 < 0L)
            number2 = 0L;
          if (number2 > this.infectedTemporal.Get(index))
            number2 = this.infectedTemporal.Get(index);
          this.infectedTemporal.Add(index, -number2);
          this.country.TransferPopulationNoPipe(number2, Country.EPopulationType.INFECTED, this.country, Country.EPopulationType.HEALTHY_RECOVERED, this.disease);
        }
      }
      this.lastKnownInfectedCount = this.infectedPopulation;
    }
    if (this.disease.cureFlag)
      this.cureRollout += (float) ModelUtils.IntRand(0, 6);
    else
      this.cureRollout = 0.0f;
    if (this.disease.isPrion)
    {
      this.country.TransferPopulation((double) this.infectedPopulation * 0.019999999552965164 * (double) this.cureRollout * (1.1000000238418579 - (double) this.disease.globalLethalityMod), Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.HEALTHY_RECOVERED);
      this.country.TransferPopulation((double) this.cureRollout * (1.1000000238418579 - (double) this.disease.globalLethalityMod), Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.HEALTHY_RECOVERED);
    }
    if (this.hasTeam && this.disease.cureScenario == Disease.ECureScenario.Cure_Bioweapon && this.infectedPopulation > 0L && this.disease.IsTechEvolved("Lethal_Containment"))
    {
      this.complianceMAXTime *= 0.99f;
      this.deadPopulationOverride += Mathf.Min((float) this.infectedPopulation, Mathf.Min(1178f, (float) ModelUtils.IntRand(1, this.teamTurnsSinceArrived) * 2f));
      if (this.disease.IsTechEvolved("Erradication_Squads"))
      {
        this.complianceMAXTime *= 0.99f;
        this.deadPopulationOverride += Mathf.Min((float) this.infectedPopulation, Mathf.Min(11783f, (float) ModelUtils.IntRand(1, this.teamTurnsSinceArrived) * 20f));
      }
    }
    float num36 = 1f;
    if (this.disease.cureFlag)
    {
      num36 = 0.0f;
      this.disease.globalAirRate = 0.0f;
      this.disease.globalSeaRate = 0.0f;
      this.disease.globalLandRate = 0.0f;
      this.disease.airTransmission = 0.0f;
      this.disease.seaTransmission = 0.0f;
      this.disease.landTransmission = 0.0f;
    }
    if (this.disease.isNanovirus)
    {
      if (this.disease.cureFlag)
      {
        this.country.TransferPopulation((double) this.infectedPopulation * 0.40000000596046448, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.HEALTHY_RECOVERED);
        if (this.disease.nanovirusEndTimer > 3)
          this.country.TransferPopulation((double) this.infectedPopulation, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.HEALTHY_RECOVERED);
      }
      if (this.disease.nanovirusPauseTimer > 0 && !this.disease.cureFlag)
        num36 = 0.0f;
    }
    float num37 = 0.0f;
    for (int index = 0; index < this.infectedTemporal.Size(); ++index)
      num37 += (float) this.infectedTemporal.Get(index) * this.disease.curveInfect[index];
    this.H2I += (float) ((double) num36 * (double) num37 * (double) this.localInfectionRate * (double) ModelUtils.FloatRand(0.0f, 1f) * (1.0 - ((double) this.infectedPercent + (double) this.country.healthyRecoveredPercent + (double) this.deadPercent)));
    this.H2I += (float) ((double) num36 * (double) this.country.deadPopulation * (double) num6 * (double) ModelUtils.FloatRand(0.0f, 1f) * (1.0 - ((double) this.infectedPercent + (double) this.country.healthyRecoveredPercent + (double) this.deadPercent)));
    if ((double) this.I2D < 0.0)
      this.I2D = 0.0f;
    float number3 = this.H2I + this.infectedPopulationOverride;
    if ((double) number3 < -9.22337217429373E+17)
      Debug.LogError((object) ("WEIRD TRANSFER AMOUNT: " + (object) number3));
    this.country.TransferPopulation((double) number3, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED);
    this.localNewInfectedGUI = (float) this.infectedPopulation - infectedPopulation;
    this.country.TransferPopulation(this.country.TransferPopulation((double) this.I2D + (double) this.deadPopulationOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
    if (this.disease.difficulty == 3)
      this.country.TransferPopulation((double) this.country.healthyRecoveredPopulation * 0.0099999997764825821, Country.EPopulationType.HEALTHY_RECOVERED, this.disease, Country.EPopulationType.HEALTHY);
    float num38 = (float) (this.country.healthyRecoveredPopulation + this.country.deadPopulation);
    float deadPopulation = (float) this.country.deadPopulation;
    if ((double) deadPopulation > 0.0)
    {
      this.localEstimatedDeathRate = Mathf.Min(1f, deadPopulation / num38);
      double estimatedDeathRate = (double) this.localEstimatedDeathRate;
      if (this.country.deadPopulation < this.infectedPopulation / 10000L)
      {
        double num39 = (double) Mathf.Min(this.localEstimatedDeathRate, this.disease.estimatedDeathRate);
      }
    }
    else
      this.localEstimatedDeathRate = 0.0f;
    if (this.disease.cureFlag && !this.HasFlag(Country.EGenericCountryFlag.ReceivedCurePlane) && (double) ModelUtils.FloatRand(0.0f, 1f) < 0.20000000298023224 && !this.disease.isNanovirus)
    {
      this.AddFlag(Country.EGenericCountryFlag.ReceivedCurePlane);
      Country hqCountry = this.disease.hqCountry;
      if (hqCountry != null)
      {
        Vehicle vehicle = Vehicle.Create();
        vehicle.type = Vehicle.EVehicleType.Airplane;
        vehicle.subType = Vehicle.EVehicleSubType.Blue;
        vehicle.actingDisease = this.disease;
        vehicle.SetRoute(hqCountry, this.country);
        World.instance.AddVehicle(vehicle);
      }
      else
        Debug.LogError((object) "Cannot send cure rollout plane. There's no source country.");
    }
    if (this.hasIntel)
    {
      if (++this.framesWithoutHistory >= 1)
      {
        this.framesWithoutHistory = 0;
        this.historyInfectedPerc.Add(this.infectedPercent);
        this.infectionRateYesterday = this.infectionRateToday;
        this.infectionRateToday = ModelUtils.FloatRand(0.9f, 1.1f) * num7;
        this.historyLocalInfectionRate.Add(Mathf.Max(0.0f, (float) (((double) this.infectionRateToday + (double) this.infectionRateYesterday) / 2.0)));
        this.historyDeathRate.Add(this.localEstimatedDeathRate);
        this.historyCompilance.Add(1f - this.compliance);
        this.firstHistoryFrame = false;
      }
    }
    if (this.disease.turnNumber == 1 && this.country.id == "russia")
      this.isRussia = true;
    if ((double) this.Z2D < 0.0)
      this.Z2D = 0.0f;
    if ((double) this.Z2DOverride < 0.0)
      this.Z2DOverride = 0.0f;
    this.country.TransferPopulation((double) this.D2Z + (double) this.D2ZOverride, Country.EPopulationType.DEAD, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.I2Z + (double) this.I2ZOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.H2Z + (double) this.H2ZOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.Z2D + (double) this.Z2DOverride, Country.EPopulationType.ZOMBIE, this.disease, Country.EPopulationType.DEAD);
    if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY_SUSCEPTIBLE, this.disease, Country.EPopulationType.DEAD);
      this.country.healthyPopulation = this.country.healthyPopulationImmune + this.country.healthyPopulationSusceptible;
    }
    else
      this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
    this.infectedPopulationOverride = this.deadPopulationOverride = 0.0f;
    this.D2ZOverride = this.I2ZOverride = this.H2ZOverride = this.H2DOverride = this.Z2DOverride = 0.0f;
    this.disease.infectedThisTurn += this.allInfected - this.prevInfected;
    this.disease.deadThisTurn += this.killedPopulation - this.prevKilled;
    this.disease.zombiesThisTurn += this.zombie - this.prevZombie;
    this.disease.infectedApesThisTurn += this.apeInfectedPopulation - this.prevInfectedApes;
    this.prevInfected = this.allInfected;
    this.prevZombie = this.zombie;
    this.prevKilled = this.killedPopulation;
    this.prevInfectedApes = this.apeInfectedPopulation;
    int num40 = Mathf.Clamp((int) (((double) this.country.medicalBudget + 1.0) / 1000.0), 1, 10);
    this.flaskBroken = Mathf.Clamp((int) ((1.0 - (double) this.country.publicOrder) * (double) num40), 0, 10);
    this.flaskResearched = Mathf.Clamp(Mathf.CeilToInt(this.cureResearchAllocation * (float) num40), 0, num40 - this.flaskBroken);
    if ((this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE) && this.disease.genSysWorking > 0)
      this.flaskResearched = num40 - this.flaskBroken;
    if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && this.disease.genSysWorking > 0)
      this.flaskResearched = (num40 - this.flaskBroken) / 2;
    this.flaskEmpty = num40 - this.flaskBroken - this.flaskResearched;
    this.originalPopulation = this.country.originalPopulation;
    this.uninfectedPopulation = this.uninfected;

    void ProcessRoute(Country c, TravelRoute route, bool sea)
    {
      if (c == null)
        return;
      LocalDisease localDisease = c.GetLocalDisease(this.disease);
      if ((double) localDisease.infectedPercent > 0.0)
        this.hasInfectedConnection = true;
      if (sea && !this.ArePortsOpen() || !sea && !this.AreAirportsOpen())
        return;
      float b = Mathf.Max(localDisease.standardLocalPriority, (float) ((double) localDisease.connectedLocalPriority / 2.0 * ((double) route.frequency * 10.0)));
      connections_total_local_priority += b;
      connections_highest_priority = Mathf.Max(connections_highest_priority, b);
      connections_weightings += (float) (1.25 - (double) route.frequency * 10.0);
    }
  }

  public void GameUpdate_FakeNews()
  {
    World instance = World.instance;
    this.isSuperCureCountry = this.disease.superCureCountry == this.country;
    this.isNexus = this.disease.nexus == this.country;
    SPLocalDiseaseExternal.LocalDiseaseUpdate_FakeNews(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    this.country.GameUpdate();
    if (this.allInfected < 1L && this.country.currentPopulation > 0L)
    {
      foreach (Country neighbour in this.country.neighbours)
      {
        LocalDisease localDisease = this.disease.GetLocalDisease(neighbour);
        if (localDisease.allInfected >= 1L && SPLocalDiseaseExternal.LandTransfer(this.spDisease.diseaseData, this.mData, this.spCountry.mData, ((SPLocalDisease) localDisease).mData, ((SPCountry) neighbour).mData, World.instance.landRoutesMult))
          this.country.TransferPopulation((double) ModelUtils.IntRand(1, 10), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED);
      }
    }
    if ((double) this.I2DAdditional > 0.0)
    {
      this.country.TransferPopulation((double) this.I2DAdditional, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
      this.I2DAdditional = 0.0f;
    }
    long num1 = (long) ((double) this.H2I + (double) this.infectedPopulationOverride - this.country.TransferPopulation((double) this.H2I + (double) this.infectedPopulationOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED));
    double number = this.country.TransferPopulation((double) this.I2D + (double) this.deadPopulationOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
    if (number > 0.0)
    {
      double num2 = number;
      double num3 = this.country.TransferPopulation(number, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
      num1 += (long) (num2 - num3);
    }
    if ((double) this.Z2D < 0.0)
      this.Z2D = 0.0f;
    if ((double) this.Z2DOverride < 0.0)
      this.Z2DOverride = 0.0f;
    this.country.TransferPopulation((double) this.D2Z + (double) this.D2ZOverride, Country.EPopulationType.DEAD, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.I2Z + (double) this.I2ZOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.H2Z + (double) this.H2ZOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.Z2D + (double) this.Z2DOverride, Country.EPopulationType.ZOMBIE, this.disease, Country.EPopulationType.DEAD);
    this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
    this.infectedPopulationOverride = this.deadPopulationOverride = 0.0f;
    this.D2ZOverride = this.I2ZOverride = this.H2ZOverride = this.H2DOverride = this.Z2DOverride = 0.0f;
    this.disease.infectedThisTurn += num1;
    this.disease.deadThisTurn += this.killedPopulation - this.prevKilled;
    this.disease.zombiesThisTurn += this.zombie - this.prevZombie;
    this.disease.infectedApesThisTurn += this.apeInfectedPopulation - this.prevInfectedApes;
    this.prevInfected = this.allInfected;
    this.prevZombie = this.zombie;
    this.prevKilled = this.killedPopulation;
    this.prevInfectedApes = this.apeInfectedPopulation;
    if (this.allInfected + this.zombie > 0L && !this.infectBonusShown)
    {
      this.infectBonusShown = true;
      ++this.disease.numInfectBubblesWithoutTouch;
      BonusIcon bonusIcon = new BonusIcon(this.disease, this.country, BonusIcon.EBonusIconType.INFECT);
      instance.AddBonusIcon(bonusIcon);
    }
    int num4 = Mathf.Clamp((int) (((double) this.country.medicalBudget + 1.0) / 1000.0), 1, 10);
    this.flaskBroken = Mathf.Clamp((int) ((1.0 - (double) this.country.publicOrder) * (double) num4), 0, 10);
    this.flaskResearched = Mathf.Clamp(Mathf.CeilToInt(this.cureResearchAllocation * (float) num4), 0, num4 - this.flaskBroken);
    if ((this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE) && this.disease.genSysWorking > 0)
      this.flaskResearched = num4 - this.flaskBroken;
    if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && this.disease.genSysWorking > 0)
      this.flaskResearched = (num4 - this.flaskBroken) / 2;
    this.flaskEmpty = num4 - this.flaskBroken - this.flaskResearched;
    this.originalPopulation = this.country.originalPopulation;
    this.uninfectedPopulation = this.uninfected;
  }

  public void GameUpdate_Vampire()
  {
    World instance = World.instance;
    this.isSuperCureCountry = this.disease.superCureCountry == this.country;
    this.isNexus = this.disease.nexus == this.country;
    SPLocalDiseaseExternal.LocalDiseaseUpdate_Vampire(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE)
    {
      if (this.daysTillActive == 0)
        this.daysTillActive = ModelUtils.IntRand(5, 30);
      if (--this.daysTillActive == 0)
      {
        this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
        Debug.Log((object) ("Vampire lab went active in: " + this.country.id));
      }
    }
    if ((this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE) && (this.country.healthyPopulation == 0L && this.disease.shadowDayDone || !this.disease.shadowDayDone && this.country.healthyPopulation + this.allInfected == 0L))
    {
      if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE)
        this.daysTillActive = 0;
      Debug.Log((object) ("Vampire lab destroyed in: " + this.country.id + " was : " + (object) this.country.apeLabStatus));
      this.country.ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
      this.disease.globalCureResearch *= 0.95f;
    }
    if (this.HasCastle)
    {
      --this.castleBubbleCounter;
      if (this.zombie > 0L)
        --this.castleBubbleCounter;
    }
    if (this.HasCastle && this.castleBubbleCounter <= 1 && ModelUtils.IntRand(0, 3) < 1 && !this.castleBubbleShowing && this.disease.numCastleBubblesWithoutTouch < 5)
    {
      this.SpawnCastleDnaBubble();
      this.castleBubbleCounter = 45 + ModelUtils.IntRand(5, 25 + this.disease.castleNumber * this.disease.castleNumber);
    }
    SPLocalDiseaseExternal.LocalDiseaseUpdateSecond_Vampire(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    SPCountry country = this.country as SPCountry;
    country.GameUpdate_Vampire();
    SPLocalDiseaseExternal.LocalDiseaseUpdateThird_Vampire(this.mData, this.spCountry.mData, this.spDisease.diseaseData);
    if (this.country.fortState == EFortState.FORT_ALIVE)
    {
      int num = ModelUtils.IntRand(5, 20);
      if (this.zombie > 0L && (double) this.vampirePresenceCounter > (double) num)
      {
        this.country.localHumanCombatStrength -= 3f / 1000f * (float) this.zombie;
        List<Vampire> vampires = this.disease.GetVampires(this.country);
        for (int index = 0; index < vampires.Count; ++index)
          vampires[index].vampireHealth -= (float) ModelUtils.IntRand(0, 4);
      }
    }
    country.TransmissionUpdate_Vampire();
    if (this.allInfected < 1L && this.country.currentPopulation > 0L)
    {
      foreach (Country neighbour in this.country.neighbours)
      {
        LocalDisease localDisease = this.disease.GetLocalDisease(neighbour);
        if (localDisease.allInfected >= 1L && SPLocalDiseaseExternal.LandTransfer_Vampire(this.spDisease.diseaseData, this.mData, this.spCountry.mData, ((SPLocalDisease) localDisease).mData, ((SPCountry) neighbour).mData, World.instance.landRoutesMult))
          this.country.TransferPopulation((double) ModelUtils.IntRand(1, 10), Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED);
      }
    }
    EApeLabState apeLabStatus = this.country.apeLabStatus;
    List<Vampire> vampires1 = this.disease.GetVampires(this.country);
    if (this.zombie > 0L && this.disease.cureFlag && this.allInfected < 1L)
    {
      for (int index = 0; index < vampires1.Count; ++index)
        vampires1[index].vampireHealth -= Mathf.Min((float) ((double) vampires1[index].vampireHealth * 0.20000000298023224 + 2.0), ModelUtils.FloatRand(1f, (float) (1.0 + (double) this.cureRollout * 0.30000001192092896)));
    }
    if (this.zombie > 0L && this.country.healthyPopulation + this.allInfected < 1L)
    {
      for (int index = 0; index < vampires1.Count; ++index)
        vampires1[index].vampireHealth -= (float) ModelUtils.IntRand(1, 3);
    }
    float h1 = 0.0f;
    float h2 = 0.0f;
    float h3 = 0.0f;
    if (this.consumeFlag > 0)
    {
      for (int index = 0; index < vampires1.Count; ++index)
      {
        h1 += vampires1[index].vampireHealth;
        h2 += vampires1[index].vampireHealthMax;
      }
      if (vampires1.Count > 0)
        h3 = h2 / (float) vampires1.Count;
    }
    this.vampireHealthDamage = 0.0f;
    this.peopleAttackVampire = false;
    SPLocalDiseaseExternal.LocalDiseaseUpdateFourth_Vampire(this.mData, this.spCountry.mData, this.spDisease.diseaseData, h1, h2, h3);
    if (this.country.apeLabStatus != apeLabStatus)
    {
      Debug.Log((object) ("Vampire lab state change from local disease update in: " + this.country.id + " was: " + (object) apeLabStatus + " is now " + (object) this.country.apeLabStatus));
      this.country.ChangeApeLabStateF(this.country.apeLabStatus, true);
    }
    if ((double) this.vampireHealthDamage > 0.0)
    {
      for (int index = 0; index < vampires1.Count; ++index)
      {
        Vampire vampire = vampires1[index];
        float num = Mathf.Max(0.0f, this.vampireHealthDamage) / (float) vampires1.Count * ModelUtils.FloatRand(0.5f, 1.5f);
        vampire.vampireHealth -= Mathf.Min(vampire.vampireHealth + 1f, num + ((double) num > 0.0 ? 1f : 0.0f));
      }
    }
    if (this.peopleAttackVampire)
    {
      float num = this.disease.humanCombatStrength + this.country.localHumanCombatStrength;
      for (int index = 0; index < vampires1.Count; ++index)
      {
        Vampire vampire = vampires1[index];
        float a = Mathf.Min((float) ModelUtils.IntRand(0, (int) ((1.0 * (1.0 + (double) num) + (double) this.disease.vampireInfectionBoost + (double) this.localVampireActivity / 70.0) * (this.HasCastle ? 0.10000000149011612 : 1.0) * (0.0 + (!this.disease.shadowDayDone ? (double) this.country.healthyPercent + (double) this.infectedPercent : (double) this.country.healthyPercent)))), (float) ((double) vampire.vampireHealth * 0.05000000074505806 + 0.10000000149011612));
        vampire.vampireHealth -= Mathf.Max(a, 0.0f);
      }
    }
    for (int index = 0; index < vampires1.Count; ++index)
    {
      float num = 0.0f;
      Vampire vampire = vampires1[index];
      if (!this.disease.cureFlag || this.zombie <= 0L || this.allInfected >= 1L)
      {
        if (this.HasCastle && this.zombie > 0L && this.country.fortState != EFortState.FORT_ALIVE && this.consumeFlag < 1)
          num += Mathf.Max((float) ((0.0099999997764825821 + (double) this.disease.castleHealingMod) * (double) ModelUtils.FloatRand(0.9f, 1f) * (double) vampire.vampireHealthMax * (this.disease.vampHealingBonus > 0 ? 1.1000000238418579 : 1.0)), 1f + ModelUtils.FloatRand(0.6f, 1f) + this.disease.castleHealingMod);
        else if (this.zombie > 0L && this.country.fortState != EFortState.FORT_ALIVE && this.consumeFlag > 0 && this.country.healthyPopulation + this.allInfected > 10L && (double) this.disease.vampHealSacrificeMod > 0.0)
          num += this.disease.vampHealSacrificeMod * vampire.vampireHealthMax * ModelUtils.FloatRand(0.7f, 1.3f);
      }
      vampire.vampireHealth = Mathf.Min(vampire.vampireHealth + num, vampire.vampireHealthMax);
      if ((double) vampire.vampireHealth <= 0.0 && this.zombie > 0L)
        this.Z2D += Mathf.Min(1f, (float) this.zombie);
    }
    if ((double) this.I2DAdditional > 0.0)
    {
      this.country.TransferPopulation((double) this.I2DAdditional, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
      this.I2DAdditional = 0.0f;
    }
    if ((double) this.H2DAdditional > 0.0)
    {
      this.country.TransferPopulation((double) this.H2DAdditional, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
      this.H2DAdditional = 0.0f;
    }
    long num1 = (long) ((double) this.H2I + (double) this.infectedPopulationOverride - this.country.TransferPopulation((double) this.H2I + (double) this.infectedPopulationOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.INFECTED));
    double number1 = (double) this.I2D + (double) this.deadPopulationOverride;
    if (number1 < 0.0)
      number1 = 0.0;
    double number2 = this.country.TransferPopulation(number1, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.DEAD);
    if (number2 > 0.0)
    {
      double num2 = number2;
      double num3 = this.country.TransferPopulation(number2, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
      num1 += (long) (num2 - num3);
    }
    if ((double) this.Z2D < 0.0)
      this.Z2D = 0.0f;
    if ((double) this.Z2DOverride < 0.0)
      this.Z2DOverride = 0.0f;
    this.country.TransferPopulation((double) this.D2Z + (double) this.D2ZOverride, Country.EPopulationType.DEAD, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.I2Z + (double) this.I2ZOverride, Country.EPopulationType.INFECTED, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.H2Z + (double) this.H2ZOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.ZOMBIE);
    this.country.TransferPopulation((double) this.Z2D + (double) this.Z2DOverride, Country.EPopulationType.ZOMBIE, this.disease, Country.EPopulationType.DEAD);
    this.country.TransferPopulation((double) this.H2D + (double) this.H2DOverride, Country.EPopulationType.HEALTHY, this.disease, Country.EPopulationType.DEAD);
    this.infectedPopulationOverride = this.deadPopulationOverride = 0.0f;
    this.D2ZOverride = this.I2ZOverride = this.H2ZOverride = this.H2DOverride = this.Z2DOverride = 0.0f;
    this.disease.infectedThisTurn += num1;
    this.disease.deadThisTurn += this.killedPopulation - this.prevKilled;
    this.disease.zombiesThisTurn += this.zombie - this.prevZombie;
    this.disease.infectedApesThisTurn += this.apeInfectedPopulation - this.prevInfectedApes;
    this.prevInfected = this.allInfected;
    this.prevZombie = this.zombie;
    this.prevKilled = this.killedPopulation;
    this.prevInfectedApes = this.apeInfectedPopulation;
    if (this.allInfected > 0L && !this.infectBonusShown)
    {
      this.infectBonusShown = true;
      ++this.disease.numInfectBubblesWithoutTouch;
      BonusIcon bonusIcon = new BonusIcon(this.disease, this.country, BonusIcon.EBonusIconType.INFECT);
      instance.AddBonusIcon(bonusIcon);
    }
    int num4 = Mathf.Clamp((int) (((double) this.country.medicalBudget + 1.0) / 1000.0), 1, 10);
    this.flaskBroken = Mathf.Clamp((int) ((1.0 - (double) this.country.publicOrder) * (double) num4), 0, 10);
    this.flaskResearched = Mathf.Clamp(Mathf.CeilToInt(this.cureResearchAllocation * (float) num4), 0, num4 - this.flaskBroken);
    if ((this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE) && this.disease.genSysWorking > 0)
      this.flaskResearched = num4 - this.flaskBroken;
    if (this.country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE && this.disease.genSysWorking > 0)
      this.flaskResearched = (num4 - this.flaskBroken) / 2;
    this.flaskEmpty = num4 - this.flaskBroken - this.flaskResearched;
    this.originalPopulation = this.country.originalPopulation;
    this.uninfectedPopulation = this.uninfected;
    this.weakestVampireHealth = 1f;
    for (int index = 0; index < vampires1.Count; ++index)
    {
      float num5 = vampires1[index].vampireHealth / vampires1[index].vampireHealthMax;
      if ((double) num5 < (double) this.weakestVampireHealth)
        this.weakestVampireHealth = num5;
    }
  }

  public void SpawnCastleDnaBubble()
  {
    ++this.disease.numCastleBubblesWithoutTouch;
    World.instance.AddBonusIcon(new BonusIcon(this.disease, this.country, BonusIcon.EBonusIconType.CASTLE));
  }

  public override void ClickCastleBubble(BonusIcon b)
  {
    int id = (int) Mathf.Max(1f, (float) (2L + this.zombie) + ((double) this.infectedPercent + (this.disease.vampLairDnaQuicker > 0 ? 0.10000000149011612 : 0.0) > 0.89999997615814209 ? 1.5f : ((double) this.infectedPercent + (this.disease.vampLairDnaQuicker > 0 ? 0.10000000149011612 : 0.0) > 0.64999997615814209 ? 1f : ((double) this.infectedPercent + (this.disease.vampLairDnaQuicker > 0 ? 0.090000003576278687 : 0.0) > 0.40000000596046448 ? 0.0f : ((double) this.infectedPercent + (this.disease.vampLairDnaQuicker > 0 ? 0.05000000074505806 : 0.0) > 0.079999998211860657 ? -1f : -2f)))) + (float) ModelUtils.IntRand(0, (int) (1.0 + (double) Mathf.Round(this.disease.castleDnaCounter > 0 ? this.disease.globalInfectedPercent * (float) this.disease.castleDnaCounter : 0.0f))));
    this.disease.evoPoints += id;
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, this.disease, id);
  }

  public float GetAlternativeImportance(
    float wealthyI,
    float povertyI,
    float hotI,
    float coldI,
    float urbanI,
    float ruralI,
    float humidI,
    float aridI)
  {
    Disease disease = this.disease;
    Country country = this.country;
    float num1 = disease.globalInfectiousness + country.govLocalInfectiousness;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if (country.wealthy)
    {
      num2 += wealthyI;
      num3 += wealthyI * disease.wealthy;
    }
    if (country.poverty)
    {
      num2 += povertyI;
      num3 += povertyI * disease.poverty;
    }
    if (country.hot)
    {
      num2 += hotI;
      num3 += hotI * disease.hot;
    }
    if (country.cold)
    {
      num2 += coldI;
      num3 += coldI * disease.cold;
    }
    if (country.urban)
    {
      num2 += urbanI;
      num3 += urbanI * disease.urban;
    }
    if (country.rural)
    {
      num2 += ruralI;
      num3 += ruralI * disease.rural;
    }
    if (country.humid)
    {
      num2 += humidI;
      num3 += humidI * disease.humid;
    }
    if (country.arid)
    {
      num2 += aridI;
      num3 += aridI * disease.arid;
    }
    float num4 = 1f;
    if ((double) num2 >= 1.0 / 500.0)
      num4 = num3 / num2;
    float alternativeImportance = num4 * num1;
    if ((double) alternativeImportance < 0.05000000074505806)
      alternativeImportance = 0.05f;
    return alternativeImportance;
  }

  public float GetAlternativeImportance(
    float wealthyI,
    float povertyI,
    float hotI,
    float coldI,
    float urbanI,
    float ruralI,
    float humidI,
    float aridI,
    bool allowNegative = false)
  {
    Disease disease = this.disease;
    Country country = this.country;
    float num1 = disease.globalInfectiousness + country.govLocalInfectiousness;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if (country.wealthy)
    {
      num2 += wealthyI;
      num3 += wealthyI * disease.wealthy;
    }
    if (country.poverty)
    {
      num2 += povertyI;
      num3 += povertyI * disease.poverty;
    }
    if (country.hot)
    {
      num2 += hotI;
      num3 += hotI * disease.hot;
    }
    if (country.cold)
    {
      num2 += coldI;
      num3 += coldI * disease.cold;
    }
    if (country.urban)
    {
      num2 += urbanI;
      num3 += urbanI * disease.urban;
    }
    if (country.rural)
    {
      num2 += ruralI;
      num3 += ruralI * disease.rural;
    }
    if (country.humid)
    {
      num2 += humidI;
      num3 += humidI * disease.humid;
    }
    if (country.arid)
    {
      num2 += aridI;
      num3 += aridI * disease.arid;
    }
    float num4 = 1f;
    if ((double) num2 >= 1.0 / 500.0)
      num4 = num3 / num2;
    float alternativeImportance = num4 * num1;
    if ((double) alternativeImportance < 0.05000000074505806 && !allowNegative)
      alternativeImportance = 0.05f;
    return alternativeImportance;
  }

  public float GetExternalResistance()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 30f;
    float num4 = 3f;
    float num5 = 10f;
    float num6 = 10f;
    float num7 = 2f;
    float num8 = 2f;
    float num9 = 2f;
    float num10 = 2f;
    Country country = this.country;
    Disease disease = this.disease;
    if (CGameManager.CheckExternalMethodExist(nameof (GetExternalResistance)))
      return (float) CGameManager.CallExternalMethodWithReturnValue(nameof (GetExternalResistance), World.instance, this.disease, this.country, (LocalDisease) this);
    if (disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      num3 = 6f;
      num4 = 3f;
      num5 = 8f;
      num6 = 10f;
      num7 = 2f;
      num8 = 2f;
      num9 = 2f;
      num10 = 2f;
    }
    if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      num3 = 37f;
      num4 = 5f;
      num5 = 12f;
      num6 = 12f;
      num7 = 3f;
      num8 = 3f;
      num9 = 3f;
      num10 = 3f;
    }
    if (CGameManager.IsFederalScenario("Reconstruction"))
    {
      num3 = 10f;
      num4 = 10f;
      num5 = 3f;
      num6 = 3f;
      num7 = 6f;
      num8 = 6f;
      num9 = 2f;
      num10 = 2f;
    }
    if (country.wealthy)
    {
      num2 += num3 * disease.wealthy;
      num1 += num3;
    }
    if (country.poverty)
    {
      num2 += num4 * disease.poverty;
      num1 += num4;
    }
    if (country.hot)
    {
      num2 += num5 * disease.hot;
      num1 += num5;
    }
    if (country.cold)
    {
      num2 += num6 * disease.cold;
      num1 += num6;
    }
    if (country.urban)
    {
      num2 += num7 * disease.urban;
      num1 += num7;
    }
    if (country.rural)
    {
      num2 += num8 * disease.rural;
      num1 += num8;
    }
    if (country.humid)
    {
      num2 += num9 * disease.humid;
      num1 += num9;
    }
    if (country.arid)
    {
      num2 += num10 * disease.arid;
      num1 += num10;
    }
    return (double) num1 == 0.0 ? 1f : num2 / num1;
  }

  public float GetExternalLocalInfectivity()
  {
    if (CGameManager.CheckExternalMethodExist(nameof (GetExternalLocalInfectivity)))
    {
      float localInfectivity = (float) CGameManager.CallExternalMethodWithReturnValue(nameof (GetExternalLocalInfectivity), World.instance, this.disease, this.country, (LocalDisease) this);
      this.localInfectiousness = localInfectivity;
      return localInfectivity;
    }
    if (!CGameManager.CheckExternalMethodExist("GetExternalResistance"))
      return this.localInfectiousness;
    float localInfectivity1 = Mathf.Max(0.05f, (this.disease.globalInfectiousness + this.country.govLocalInfectiousness) * this.GetExternalResistance());
    this.localInfectiousness = localInfectivity1;
    return localInfectivity1;
  }
}
