// Decompiled with JetBrains decompiler
// Type: SPDisease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class SPDisease : Disease, ITutorial
{
  public SPDiseaseData diseaseData;
  private string _scenario;
  public float yesterdayCureBaseMultiplier;
  private bool zombieScenarioChecked;
  public int abnormalCheckedDay;
  private bool parasiteScenarioChecked;
  private bool dreamScenarioChecked;
  private bool hellScenarioChecked;
  public float totalMusicImportance;
  public int maxMusicCombo;
  public int currentMusicCombo;
  public float totalMusicPoint;
  public float expectedMusicImportance;
  public int totalMusicBubbleCount;
  public bool backendPlayed;
  public bool fateScenarioChecked;
  public bool finalScenarioChecked;
  public bool scenarioEverChecked;

  public static void TestLibrary()
  {
    SPDiseaseData d = new SPDiseaseData();
    SPDiseaseData spDiseaseData = new SPDiseaseData();
    spDiseaseData.superCureCountryNumber = d.superCureCountryNumber = 23;
    spDiseaseData.totalInfected = d.totalInfected = 10L;
    spDiseaseData.totalFlaskEmpty = d.totalFlaskEmpty = 3;
    spDiseaseData.infectedCountryCount = d.infectedCountryCount = 2;
    spDiseaseData.zombiesThisTurn = d.zombiesThisTurn = 17L;
    spDiseaseData.hordeWaterSpeed = d.hordeWaterSpeed = 5.5f;
    spDiseaseData.nexusMinInfect = d.nexusMinInfect = 7.3f;
    spDiseaseData.nexusBonus = d.nexusBonus = false;
    spDiseaseData.totalControlledInfected = d.totalControlledInfected = 18L;
    spDiseaseData.diseaseType = d.diseaseType = Disease.EDiseaseType.PRION;
    spDiseaseData.numCheats = d.numCheats = 3;
    spDiseaseData.cheatFlags = d.cheatFlags = Disease.GetCheatFlag(Disease.ECheatType.IMMUNE) | Disease.GetCheatFlag(Disease.ECheatType.LUCKY_DIP) | Disease.GetCheatFlag(Disease.ECheatType.SHUFFLE);
    spDiseaseData.apeBonusPriorityGlobal = d.apeBonusPriorityGlobal = 14f;
    spDiseaseData.populationImmunity = d.populationImmunity = 0.1f;
    spDiseaseData.simianNarrativePath = d.simianNarrativePath = 3;
    spDiseaseData.recomputePathsFlag = d.recomputePathsFlag = 0.0f;
    spDiseaseData.vampireConversionPotTrigger = d.vampireConversionPotTrigger = 2356L;
    spDiseaseData.vampHealSacrificeMod = d.vampHealSacrificeMod = 3.4f;
    spDiseaseData.templarDestroyed = d.templarDestroyed = 43;
    spDiseaseData.brexitExecute = d.brexitExecute = 54;
    spDiseaseData.deadBodyTransmission = d.deadBodyTransmission = 8f;
    spDiseaseData.fakeNewsInformDropPercent = d.fakeNewsInformDropPercent = 1.9f;
    spDiseaseData.preSimulate = d.preSimulate = 12;
    spDiseaseData.vaccineStage = d.vaccineStage = Disease.EVaccineProgressStage.VACCINE_NONE;
    spDiseaseData.globalInfectModMAX = d.globalInfectModMAX = 0.1f;
    spDiseaseData.startingAuthority = d.startingAuthority = 0.13f;
    spDiseaseData.nationalPopUpper = d.nationalPopUpper = 4f;
    spDiseaseData.totalInfectedIntel = d.totalInfectedIntel = 1045L;
    spDiseaseData.genericFlags = d.genericFlags = Disease.EGenericDiseaseFlag.GeneAirportController;
    spDiseaseData.totalPopulation = d.totalPopulation = 7124543962L;
    spDiseaseData.totalInfected += 57000L;
    spDiseaseData.totalFlaskEmpty += 666;
    spDiseaseData.infectedCountryCount += 777;
    spDiseaseData.zombiesThisTurn += 93L;
    spDiseaseData.hordeWaterSpeed += 0.34f;
    spDiseaseData.nexusMinInfect += 0.123f;
    spDiseaseData.nexusBonus = true;
    spDiseaseData.totalControlledInfected += 67L;
    spDiseaseData.diseaseType = Disease.EDiseaseType.NECROA;
    spDiseaseData.fortDifficultyModifier = 0.5f;
    spDiseaseData.isScenario = true;
    spDiseaseData.cheatFlags |= Disease.GetCheatFlag(Disease.ECheatType.TURBO);
    spDiseaseData.apeBonusPriorityGlobal += 50f;
    spDiseaseData.populationImmunity += 19f;
    spDiseaseData.simianNarrativePath += 2;
    ++spDiseaseData.recomputePathsFlag;
    ++spDiseaseData.superCureCountryNumber;
    spDiseaseData.vampireConversionPotTrigger += 43L;
    spDiseaseData.vampHealSacrificeMod += 43.27f;
    spDiseaseData.templarDestroyed += 245;
    spDiseaseData.brexitExecute += 7;
    spDiseaseData.deadBodyTransmission += 0.5f;
    spDiseaseData.fakeNewsInformDropPercent += 0.23f;
    spDiseaseData.preSimulate += 22;
    spDiseaseData.vaccineStage = Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL;
    spDiseaseData.globalInfectModMAX += 0.24f;
    spDiseaseData.startingAuthority += 0.25f;
    spDiseaseData.nationalPopUpper += 26f;
    spDiseaseData.totalInfectedIntel += 27L;
    spDiseaseData.genericFlags |= Disease.EGenericDiseaseFlag.CheatTheAvengers;
    spDiseaseData.totalPopulation += 29L;
    SPDiseaseExternal.DiseaseTest(d);
    if (d.totalInfected != spDiseaseData.totalInfected)
      Debug.LogWarning((object) ("DISEASE TEST 0 FAIL: " + (object) d.totalInfected + " != " + (object) spDiseaseData.totalInfected));
    else
      Debug.Log((object) "DISEASE TEST 0 SUCCEED");
    if (d.totalFlaskEmpty != spDiseaseData.totalFlaskEmpty)
      Debug.LogWarning((object) ("DISEASE TEST 1 FAIL: " + (object) d.totalFlaskEmpty + " != " + (object) spDiseaseData.totalFlaskEmpty));
    else
      Debug.Log((object) "DISEASE TEST 1 SUCCEED");
    if (d.infectedCountryCount != spDiseaseData.infectedCountryCount)
      Debug.LogWarning((object) "DISEASE TEST 2 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 2 SUCCEED");
    if (d.zombiesThisTurn != spDiseaseData.zombiesThisTurn)
      Debug.LogWarning((object) ("DISEASE TEST 3 FAIL: " + (object) d.zombiesThisTurn + " != " + (object) spDiseaseData.zombiesThisTurn));
    else
      Debug.Log((object) "DISEASE TEST 3 SUCCEED");
    if ((double) d.hordeWaterSpeed != (double) spDiseaseData.hordeWaterSpeed)
      Debug.LogWarning((object) "DISEASE TEST 4 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 4 SUCCEED");
    if ((double) d.nexusMinInfect != (double) spDiseaseData.nexusMinInfect)
      Debug.LogWarning((object) "DISEASE TEST 5 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 5 SUCCEED");
    if (d.nexusBonus != spDiseaseData.nexusBonus)
      Debug.LogWarning((object) "DISEASE TEST 6 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 6 SUCCEED");
    if (d.totalControlledInfected != spDiseaseData.totalControlledInfected)
      Debug.LogWarning((object) "DISEASE TEST 7 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 7 SUCCEED");
    if (d.diseaseType != spDiseaseData.diseaseType)
      Debug.LogWarning((object) "DISEASE TEST 8 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 8 SUCCEED");
    if ((double) d.fortDifficultyModifier != (double) spDiseaseData.fortDifficultyModifier)
      Debug.LogWarning((object) "DISEASE TEST 9 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 9 SUCCEED");
    if (d.isScenario != spDiseaseData.isScenario)
      Debug.LogWarning((object) "DISEASE TEST 10 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 10 SUCCEED");
    if (d.cheatFlags != spDiseaseData.cheatFlags)
      Debug.LogWarning((object) "DISEASE TEST 11 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 11 SUCCEED");
    if ((double) d.populationImmunity != (double) spDiseaseData.populationImmunity)
      Debug.LogWarning((object) "DISEASE TEST 12 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 12 SUCCEED");
    if ((double) d.apeBonusPriorityGlobal != (double) spDiseaseData.apeBonusPriorityGlobal)
      Debug.LogWarning((object) "DISEASE TEST 13 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 13 SUCCEED");
    if (d.simianNarrativePath != spDiseaseData.simianNarrativePath)
      Debug.LogWarning((object) "DISEASE TEST 14 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 14 SUCCEED");
    if (d.superCureCountryNumber != spDiseaseData.superCureCountryNumber)
      Debug.LogWarning((object) "DISEASE TEST 15 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 15 SUCCEED");
    if (d.vampireConversionPotTrigger != spDiseaseData.vampireConversionPotTrigger)
      Debug.LogWarning((object) "DISEASE TEST 16 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 16 SUCCEED");
    if ((double) d.vampHealSacrificeMod != (double) spDiseaseData.vampHealSacrificeMod)
      Debug.LogWarning((object) "DISEASE TEST 17 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 17 SUCCEED");
    if (d.templarDestroyed != spDiseaseData.templarDestroyed)
      Debug.LogWarning((object) "DISEASE TEST 18 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 18 SUCCEED");
    if (d.brexitExecute != spDiseaseData.brexitExecute)
      Debug.LogWarning((object) "DISEASE TEST 19 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 19 SUCCEED");
    if ((double) d.deadBodyTransmission != (double) spDiseaseData.deadBodyTransmission)
      Debug.LogWarning((object) "DISEASE TEST 20 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 20 SUCCEED");
    if ((double) d.fakeNewsInformDropPercent != (double) spDiseaseData.fakeNewsInformDropPercent)
      Debug.LogWarning((object) "DISEASE TEST 21 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 21 SUCCEED");
    if (d.preSimulate != spDiseaseData.preSimulate)
      Debug.LogWarning((object) "DISEASE TEST 22 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 22 SUCCEED");
    if (d.vaccineStage != spDiseaseData.vaccineStage)
      Debug.LogWarning((object) "DISEASE TEST 23 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 23 SUCCEED");
    if ((double) d.globalInfectModMAX != (double) spDiseaseData.globalInfectModMAX)
      Debug.LogWarning((object) "DISEASE TEST 24 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 24 SUCCEED");
    if ((double) d.startingAuthority != (double) spDiseaseData.startingAuthority)
      Debug.LogWarning((object) "DISEASE TEST 25 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 25 SUCCEED");
    if ((double) d.nationalPopUpper != (double) spDiseaseData.nationalPopUpper)
      Debug.LogWarning((object) "DISEASE TEST 26 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 26 SUCCEED");
    if (d.totalInfectedIntel != spDiseaseData.totalInfectedIntel)
      Debug.LogWarning((object) "DISEASE TEST 27 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 27 SUCCEED");
    if (d.genericFlags != spDiseaseData.genericFlags)
      Debug.LogWarning((object) "DISEASE TEST 28 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 28 SUCCEED");
    if (d.totalPopulation != spDiseaseData.totalPopulation)
      Debug.LogWarning((object) "DISEASE TEST 29 FAIL");
    else
      Debug.Log((object) "DISEASE TEST 29 SUCCEED");
    Debug.Log((object) ("POP: " + (object) d.totalPopulation));
  }

  public override int id
  {
    get => this.diseaseData.id;
    set => this.diseaseData.id = value;
  }

  public override int evoPoints
  {
    get => this.diseaseData.evoPoints;
    set => this.diseaseData.evoPoints = value;
  }

  public override bool closedBordersSpreadEnhance
  {
    get => this.diseaseData.closedBordersSpreadEnhance;
    set => this.diseaseData.closedBordersSpreadEnhance = value;
  }

  public override bool nexusBonus
  {
    get => this.diseaseData.nexusBonus;
    set => this.diseaseData.nexusBonus = value;
  }

  public override float globalInfectiousness
  {
    get => this.diseaseData.globalInfectiousness;
    set => this.diseaseData.globalInfectiousness = value;
  }

  public override float globalInfectiousnessMax
  {
    get => this.diseaseData.globalInfectiousnessMax;
    set => this.diseaseData.globalInfectiousnessMax = value;
  }

  public override float globalSeverity
  {
    get => this.diseaseData.globalSeverity;
    set => this.diseaseData.globalSeverity = value;
  }

  public override float globalSeverityMax
  {
    get => this.diseaseData.globalSeverityMax;
    set => this.diseaseData.globalSeverityMax = value;
  }

  public override float globalLethality
  {
    get => this.diseaseData.globalLethality;
    set => this.diseaseData.globalLethality = value;
  }

  public override float globalLethalityMax
  {
    get => this.diseaseData.globalLethalityMax;
    set => this.diseaseData.globalLethalityMax = value;
  }

  public override float cureRequirements
  {
    get => this.diseaseData.cureRequirements;
    set => this.diseaseData.cureRequirements = value;
  }

  public override float cureRequirementBase
  {
    get => this.diseaseData.cureRequirementBase;
    set => this.diseaseData.cureRequirementBase = value;
  }

  public override float cureBaseMultiplier
  {
    get => this.diseaseData.cureBaseMultiplier;
    set => this.diseaseData.cureBaseMultiplier = value;
  }

  public override float researchInefficiencyMultiplier
  {
    get => this.diseaseData.researchInefficiencyMultiplier;
    set => this.diseaseData.researchInefficiencyMultiplier = value;
  }

  public override float researchInefficiency
  {
    get => this.diseaseData.researchInefficiency;
    set => this.diseaseData.researchInefficiency = value;
  }

  public override float globalCureResearch
  {
    get => this.diseaseData.globalCureResearch;
    set => this.diseaseData.globalCureResearch = value;
  }

  public override float globalCureResearchThisTurn
  {
    get => this.diseaseData.globalCureResearchThisTurn;
    set => this.diseaseData.globalCureResearchThisTurn = value;
  }

  public override float cureCompletePercent
  {
    get => this.diseaseData.cureCompletePercent;
    set => this.diseaseData.cureCompletePercent = value;
  }

  public override float globalAirRate
  {
    get => this.diseaseData.globalAirRate;
    set => this.diseaseData.globalAirRate = value;
  }

  public override float globalSeaRate
  {
    get => this.diseaseData.globalSeaRate;
    set => this.diseaseData.globalSeaRate = value;
  }

  public override float globalLandRate
  {
    get => this.diseaseData.globalLandRate;
    set => this.diseaseData.globalLandRate = value;
  }

  public override float globalInfectedPercent
  {
    get => this.diseaseData.globalInfectedPercent;
    set => this.diseaseData.globalInfectedPercent = value;
  }

  public override float globalZombiePercent
  {
    get => this.diseaseData.globalZombiePercent;
    set => this.diseaseData.globalZombiePercent = value;
  }

  public override float infectedPointsPotAll
  {
    get => this.diseaseData.infectedPointsPotAll;
    set => this.diseaseData.infectedPointsPotAll = value;
  }

  public override float infectedPointsPot
  {
    get => this.diseaseData.infectedPointsPot;
    set => this.diseaseData.infectedPointsPot = value;
  }

  public override int dnaPointsGained
  {
    get => this.diseaseData.dnaPointsGained;
    set => this.diseaseData.dnaPointsGained = value;
  }

  public override float globalSeverityPlusLethality
  {
    get => this.diseaseData.globalSeverityPlusLethality;
    set => this.diseaseData.globalSeverityPlusLethality = value;
  }

  public override int natCatCounter
  {
    get => this.diseaseData.natCatCounter;
    set => this.diseaseData.natCatCounter = value;
  }

  public override int turnNumber
  {
    get => this.diseaseData.turnNumber;
    set => this.diseaseData.turnNumber = value;
  }

  public override float uncontrolledAttackBias
  {
    get => this.diseaseData.uncontrolledAttackBias;
    set => this.diseaseData.uncontrolledAttackBias = value;
  }

  public override int evoPointsSpent
  {
    get => this.diseaseData.evoPointsSpent;
    set => this.diseaseData.evoPointsSpent = value;
  }

  public override int evoPointsPrevTurn
  {
    get => this.diseaseData.evoPointsPrevTurn;
    set => this.diseaseData.evoPointsPrevTurn = value;
  }

  public override int numTurnsWithoutEvoChange
  {
    get => this.diseaseData.numTurnsWithoutEvoChange;
    set => this.diseaseData.numTurnsWithoutEvoChange = value;
  }

  public override int evoBoost
  {
    get => this.diseaseData.evoBoost;
    set => this.diseaseData.evoBoost = value;
  }

  public override int symptomExtraCost
  {
    get => this.diseaseData.symptomExtraCost;
    set => this.diseaseData.symptomExtraCost = value;
  }

  public override int abilityExtraCost
  {
    get => this.diseaseData.abilityExtraCost;
    set => this.diseaseData.abilityExtraCost = value;
  }

  public override int transmissionExtraCost
  {
    get => this.diseaseData.transmissionExtraCost;
    set => this.diseaseData.transmissionExtraCost = value;
  }

  public override bool symptomCostIncrease
  {
    get => this.diseaseData.symptomCostIncrease;
    set => this.diseaseData.symptomCostIncrease = value;
  }

  public override bool abilityCostIncrease
  {
    get => this.diseaseData.abilityCostIncrease;
    set => this.diseaseData.abilityCostIncrease = value;
  }

  public override bool transmissionCostIncrease
  {
    get => this.diseaseData.transmissionCostIncrease;
    set => this.diseaseData.transmissionCostIncrease = value;
  }

  public override float geneticDrift
  {
    get => this.diseaseData.geneticDrift;
    set => this.diseaseData.geneticDrift = value;
  }

  public override float geneticDriftMax
  {
    get => this.diseaseData.geneticDriftMax;
    set => this.diseaseData.geneticDriftMax = value;
  }

  public override int numDNABubbles
  {
    get => this.diseaseData.numDNABubbles;
    set => this.diseaseData.numDNABubbles = value;
  }

  public override int numDNABubblesWithoutTouch
  {
    get => this.diseaseData.numDNABubblesWithoutTouch;
    set => this.diseaseData.numDNABubblesWithoutTouch = value;
  }

  public override int numCureBubblesWithoutTouch
  {
    get => this.diseaseData.numCureBubblesWithoutTouch;
    set => this.diseaseData.numCureBubblesWithoutTouch = value;
  }

  public override int numInfectBubblesWithoutTouch
  {
    get => this.diseaseData.numInfectBubblesWithoutTouch;
    set => this.diseaseData.numInfectBubblesWithoutTouch = value;
  }

  public override int orangeBubbleMult
  {
    get => this.diseaseData.orangeBubbleMult;
    set => this.diseaseData.orangeBubbleMult = value;
  }

  public override int redBubbleMult
  {
    get => this.diseaseData.redBubbleMult;
    set => this.diseaseData.redBubbleMult = value;
  }

  public override bool blueBubbleBonusDNA
  {
    get => this.diseaseData.blueBubbleBonusDNA;
    set => this.diseaseData.blueBubbleBonusDNA = value;
  }

  public override bool bubbleAutopress
  {
    get => this.diseaseData.bubbleAutopress;
    set => this.diseaseData.bubbleAutopress = value;
  }

  public override bool dnaBubbleShowing
  {
    get => this.diseaseData.dnaBubbleShowing;
    set => this.diseaseData.dnaBubbleShowing = value;
  }

  public override int symptomDevolveCost
  {
    get => this.diseaseData.symptomDevolveCost;
    set => this.diseaseData.symptomDevolveCost = value;
  }

  public override int symptomDevolveCostIncrease
  {
    get => this.diseaseData.symptomDevolveCostIncrease;
    set => this.diseaseData.symptomDevolveCostIncrease = value;
  }

  public override int transmissionDevolveCost
  {
    get => this.diseaseData.transmissionDevolveCost;
    set => this.diseaseData.transmissionDevolveCost = value;
  }

  public override int transmissionDevolveCostIncrease
  {
    get => this.diseaseData.transmissionDevolveCostIncrease;
    set => this.diseaseData.transmissionDevolveCostIncrease = value;
  }

  public override int abilityDevolveCost
  {
    get => this.diseaseData.abilityDevolveCost;
    set => this.diseaseData.abilityDevolveCost = value;
  }

  public override int abilityDevolveCostIncrease
  {
    get => this.diseaseData.abilityDevolveCostIncrease;
    set => this.diseaseData.abilityDevolveCostIncrease = value;
  }

  public override bool cureFlag
  {
    get => this.diseaseData.cureFlag;
    set => this.diseaseData.cureFlag = value;
  }

  public override bool cureFlagOverride
  {
    get => this.diseaseData.cureFlagOverride;
    set => this.diseaseData.cureFlagOverride = value;
  }

  public override bool diseaseNoticed
  {
    get => this.diseaseData.diseaseNoticed;
    set => this.diseaseData.diseaseNoticed = value;
  }

  public override float wealthy
  {
    get => this.diseaseData.wealthy;
    set => this.diseaseData.wealthy = value;
  }

  public override float poverty
  {
    get => this.diseaseData.poverty;
    set => this.diseaseData.poverty = value;
  }

  public override float urban
  {
    get => this.diseaseData.urban;
    set => this.diseaseData.urban = value;
  }

  public override float rural
  {
    get => this.diseaseData.rural;
    set => this.diseaseData.rural = value;
  }

  public override float hot
  {
    get => this.diseaseData.hot;
    set => this.diseaseData.hot = value;
  }

  public override float cold
  {
    get => this.diseaseData.cold;
    set => this.diseaseData.cold = value;
  }

  public override float humid
  {
    get => this.diseaseData.humid;
    set => this.diseaseData.humid = value;
  }

  public override float arid
  {
    get => this.diseaseData.arid;
    set => this.diseaseData.arid = value;
  }

  public override float landTransmission
  {
    get => this.diseaseData.landTransmission;
    set => this.diseaseData.landTransmission = value;
  }

  public override float seaTransmission
  {
    get => this.diseaseData.seaTransmission;
    set => this.diseaseData.seaTransmission = value;
  }

  public override float airTransmission
  {
    get => this.diseaseData.airTransmission;
    set => this.diseaseData.airTransmission = value;
  }

  public override float corpseTransmission
  {
    get => this.diseaseData.corpseTransmission;
    set => this.diseaseData.corpseTransmission = value;
  }

  public override float mutation
  {
    get => this.diseaseData.mutation;
    set => this.diseaseData.mutation = value;
  }

  public override float mutationCounter
  {
    get => this.diseaseData.mutationCounter;
    set => this.diseaseData.mutationCounter = value;
  }

  public override float mutationTrigger
  {
    get => this.diseaseData.mutationTrigger;
    set => this.diseaseData.mutationTrigger = value;
  }

  public override bool transmissionRandomMutations
  {
    get => this.diseaseData.transmissionRandomMutations;
    set => this.diseaseData.transmissionRandomMutations = value;
  }

  public override bool abilityRandomMutations
  {
    get => this.diseaseData.abilityRandomMutations;
    set => this.diseaseData.abilityRandomMutations = value;
  }

  public override bool mutatedThisTurn
  {
    get => this.diseaseData.mutatedThisTurn;
    set => this.diseaseData.mutatedThisTurn = value;
  }

  public override float nexusMinInfect
  {
    get => this.diseaseData.nexusMinInfect;
    set => this.diseaseData.nexusMinInfect = value;
  }

  public override float nexusBonusGene
  {
    get => this.diseaseData.nexusBonusGene;
    set => this.diseaseData.nexusBonusGene = value;
  }

  public override float globalInfectiousnessTopMultipler
  {
    get => this.diseaseData.globalInfectiousnessTopMultipler;
    set => this.diseaseData.globalInfectiousnessTopMultipler = value;
  }

  public override float globalInfectiousnessBottomValue
  {
    get => this.diseaseData.globalInfectiousnessBottomValue;
    set => this.diseaseData.globalInfectiousnessBottomValue = value;
  }

  public override float globalSeverityTopMultipler
  {
    get => this.diseaseData.globalSeverityTopMultipler;
    set => this.diseaseData.globalSeverityTopMultipler = value;
  }

  public override float globalSeverityBottomValue
  {
    get => this.diseaseData.globalSeverityBottomValue;
    set => this.diseaseData.globalSeverityBottomValue = value;
  }

  public override float globalLethalityTopMultipler
  {
    get => this.diseaseData.globalLethalityTopMultipler;
    set => this.diseaseData.globalLethalityTopMultipler = value;
  }

  public override float globalLethalityBottomValue
  {
    get => this.diseaseData.globalLethalityBottomValue;
    set => this.diseaseData.globalLethalityBottomValue = value;
  }

  public override float cureRequirementBaseMultipler
  {
    get => this.diseaseData.cureRequirementBaseMultipler;
    set => this.diseaseData.cureRequirementBaseMultipler = value;
  }

  public override float infectedPointsPotChange
  {
    get => this.diseaseData.infectedPointsPotChange;
    set => this.diseaseData.infectedPointsPotChange = value;
  }

  public override int difficulty
  {
    get => this.diseaseData.difficulty;
    set => this.diseaseData.difficulty = value;
  }

  public override float difficultyVariable
  {
    get => this.diseaseData.difficultyVariable;
    set => this.diseaseData.difficultyVariable = value;
  }

  public override bool zday
  {
    get => this.diseaseData.zday;
    set => this.diseaseData.zday = value;
  }

  public override bool zdayDone
  {
    get => this.diseaseData.zdayDone;
    set => this.diseaseData.zdayDone = value;
  }

  public override bool firstFortSelected
  {
    get => this.diseaseData.firstFortSelected;
    set => this.diseaseData.firstFortSelected = value;
  }

  public override int zdayCounter
  {
    get => this.diseaseData.zdayCounter;
    set => this.diseaseData.zdayCounter = value;
  }

  public override int zombieDecreaseTurnCount
  {
    get => this.diseaseData.zombieDecreaseTurnCount;
    set => this.diseaseData.zombieDecreaseTurnCount = value;
  }

  public override int zdayLength
  {
    get => this.diseaseData.zdayLength;
    set => this.diseaseData.zdayLength = value;
  }

  public override int daysUntilMinifortPlaneSpawn
  {
    get => this.diseaseData.daysUntilMinifortPlaneSpawn;
    set => this.diseaseData.daysUntilMinifortPlaneSpawn = value;
  }

  public override int fortSelectionDay
  {
    get => this.diseaseData.fortSelectionDay;
    set => this.diseaseData.fortSelectionDay = value;
  }

  public override int numAliveForts
  {
    get => this.diseaseData.numAliveForts;
    set => this.diseaseData.numAliveForts = value;
  }

  public override float zdayDead
  {
    get => this.diseaseData.zdayDead;
    set => this.diseaseData.zdayDead = value;
  }

  public override float zdayInfected
  {
    get => this.diseaseData.zdayInfected;
    set => this.diseaseData.zdayInfected = value;
  }

  public override float zombieConversionMod
  {
    get => this.diseaseData.zombieConversionMod;
    set => this.diseaseData.zombieConversionMod = value;
  }

  public override float zombieCombatStrength
  {
    get => this.diseaseData.zombieCombatStrength;
    set => this.diseaseData.zombieCombatStrength = value;
  }

  public override float humanCombatStrength
  {
    get => this.diseaseData.humanCombatStrength;
    set => this.diseaseData.humanCombatStrength = value;
  }

  public override float zombieDecay
  {
    get => this.diseaseData.zombieDecay;
    set => this.diseaseData.zombieDecay = value;
  }

  public override float zombieInfect
  {
    get => this.diseaseData.zombieInfect;
    set => this.diseaseData.zombieInfect = value;
  }

  public override float zombieDecayTechMultiplier
  {
    get => this.diseaseData.zombieDecayTechMultiplier;
    set => this.diseaseData.zombieDecayTechMultiplier = value;
  }

  public override float globalZombieDecayMultiplier
  {
    get => this.diseaseData.globalZombieDecayMultiplier;
    set => this.diseaseData.globalZombieDecayMultiplier = value;
  }

  public override float hiZombifiedPopulation
  {
    get => this.diseaseData.hiZombifiedPopulation;
    set => this.diseaseData.hiZombifiedPopulation = value;
  }

  public override float fortDifficultyModifier
  {
    get => this.diseaseData.fortDifficultyModifier;
    set => this.diseaseData.fortDifficultyModifier = value;
  }

  public override float globalDecayChance
  {
    get => this.diseaseData.globalDecayChance;
    set => this.diseaseData.globalDecayChance = value;
  }

  public override float globalBattleVictoryCount
  {
    get => this.diseaseData.globalBattleVictoryCount;
    set => this.diseaseData.globalBattleVictoryCount = value;
  }

  public override bool hotDecayFlag
  {
    get => this.diseaseData.hotDecayFlag;
    set => this.diseaseData.hotDecayFlag = value;
  }

  public override int _ep2
  {
    get => this.diseaseData._ep2;
    set => this.diseaseData._ep2 = value;
  }

  public override float hordeSpeed
  {
    get => this.diseaseData.hordeSpeed;
    set => this.diseaseData.hordeSpeed = value;
  }

  public override float hordeSize
  {
    get => this.diseaseData.hordeSize;
    set => this.diseaseData.hordeSize = value;
  }

  public override float reanimateSize
  {
    get => this.diseaseData.reanimateSize;
    set => this.diseaseData.reanimateSize = value;
  }

  public override float hordeWaterSpeed
  {
    get => this.diseaseData.hordeWaterSpeed;
    set => this.diseaseData.hordeWaterSpeed = value;
  }

  public override float reanimateZombieCombatStrengthMod
  {
    get => this.diseaseData.reanimateZombieCombatStrengthMod;
    set => this.diseaseData.reanimateZombieCombatStrengthMod = value;
  }

  public override float wimpFlag
  {
    get => this.diseaseData.wimpFlag;
    set => this.diseaseData.wimpFlag = value;
  }

  public override int aaCostModifier
  {
    get => this.diseaseData.aaCostModifier;
    set => this.diseaseData.aaCostModifier = value;
  }

  public override bool zombieWin
  {
    get => this.diseaseData.zombieWin;
    set => this.diseaseData.zombieWin = value;
  }

  public override bool zombieLoss
  {
    get => this.diseaseData.zombieLoss;
    set => this.diseaseData.zombieLoss = value;
  }

  public override bool isSpeedRun
  {
    get => this.diseaseData.isSpeedRun;
    set => this.diseaseData.isSpeedRun = value;
  }

  public override bool showExtraPopups
  {
    get => this.diseaseData.showExtraPopups;
    set => this.diseaseData.showExtraPopups = value;
  }

  public override float wormPlaneChance
  {
    get => this.diseaseData.wormPlaneChance;
    set => this.diseaseData.wormPlaneChance = value;
  }

  public override int wormBubbleLastDay
  {
    get => this.diseaseData.wormBubbleLastDay;
    set => this.diseaseData.wormBubbleLastDay = value;
  }

  public override int numWormBubblesWithoutTouch
  {
    get => this.diseaseData.numWormBubblesWithoutTouch;
    set => this.diseaseData.numWormBubblesWithoutTouch = value;
  }

  public override int daysToGameWin
  {
    get => this.diseaseData.daysToGameWin;
    set => this.diseaseData.daysToGameWin = value;
  }

  public override int wormBubbleHiddenStatus
  {
    get => this.diseaseData.wormBubbleHiddenStatus;
    set => this.diseaseData.wormBubbleHiddenStatus = value;
  }

  public override long totalInfected
  {
    get => this.diseaseData.totalInfected;
    set => this.diseaseData.totalInfected = value;
  }

  public override long totalControlledInfected
  {
    get => this.diseaseData.totalControlledInfected;
    set => this.diseaseData.totalControlledInfected = value;
  }

  public override long totalDead
  {
    get => this.diseaseData.totalDead;
    set => this.diseaseData.totalDead = value;
  }

  public override long totalKilled
  {
    get => this.diseaseData.totalKilled;
    set => this.diseaseData.totalKilled = value;
  }

  public override long totalZombie
  {
    get => this.diseaseData.totalZombie;
    set => this.diseaseData.totalZombie = value;
  }

  public override long totalHealthy
  {
    get => this.diseaseData.totalHealthy;
    set => this.diseaseData.totalHealthy = value;
  }

  public override long totalUninfected
  {
    get => this.diseaseData.totalUninfected;
    set => this.diseaseData.totalUninfected = value;
  }

  public override long infectedThisTurn
  {
    get => this.diseaseData.infectedThisTurn;
    set => this.diseaseData.infectedThisTurn = value;
  }

  public override long deadThisTurn
  {
    get => this.diseaseData.deadThisTurn;
    set => this.diseaseData.deadThisTurn = value;
  }

  public override long zombiesThisTurn
  {
    get => this.diseaseData.zombiesThisTurn;
    set => this.diseaseData.zombiesThisTurn = value;
  }

  public override long infectedApesThisTurn
  {
    get => this.diseaseData.infectedApesThisTurn;
    set => this.diseaseData.infectedApesThisTurn = value;
  }

  public override double averageInfected
  {
    get => this.diseaseData.averageInfected;
    set => this.diseaseData.averageInfected = value;
  }

  public override double averageDead
  {
    get => this.diseaseData.averageDead;
    set => this.diseaseData.averageDead = value;
  }

  public override float globalPriority
  {
    get => this.diseaseData.globalPriority;
    set => this.diseaseData.globalPriority = value;
  }

  public override float globalAwareness
  {
    get => this.diseaseData.globalAwareness;
    set => this.diseaseData.globalAwareness = value;
  }

  public override float cureResearchAllocationAverage
  {
    get => this.diseaseData.cureResearchAllocationAverage;
    set => this.diseaseData.cureResearchAllocationAverage = value;
  }

  public override float publicOrderAverage
  {
    get => this.diseaseData.publicOrderAverage;
    set => this.diseaseData.publicOrderAverage = value;
  }

  public override float bonusPriority
  {
    get => this.diseaseData.bonusPriority;
    set => this.diseaseData.bonusPriority = value;
  }

  public override int priorityCounter
  {
    get => this.diseaseData.priorityCounter;
    set => this.diseaseData.priorityCounter = value;
  }

  public override int recentEventCounter
  {
    get => this.diseaseData.recentEventCounter;
    set => this.diseaseData.recentEventCounter = value;
  }

  public override float globalDeadPercent
  {
    get => this.diseaseData.globalDeadPercent;
    set => this.diseaseData.globalDeadPercent = value;
  }

  public override float globalHealthyPercent
  {
    get => this.diseaseData.globalHealthyPercent;
    set => this.diseaseData.globalHealthyPercent = value;
  }

  public override float globalUninfectedPercent
  {
    get => this.diseaseData.globalUninfectedPercent;
    set => this.diseaseData.globalUninfectedPercent = value;
  }

  public override float globalKilledPercent
  {
    get => this.diseaseData.globalKilledPercent;
    set => this.diseaseData.globalKilledPercent = value;
  }

  public override float globalDeadPriority
  {
    get => 0.0f;
    set
    {
    }
  }

  public override float researchFocusPriority
  {
    get => 0.0f;
    set
    {
    }
  }

  public override float researchFocusMod
  {
    get => 0.0f;
    set
    {
    }
  }

  public override int infectedCountryCount
  {
    get => this.diseaseData.infectedCountryCount;
    set => this.diseaseData.infectedCountryCount = value;
  }

  public override int uninfectedCountryCount
  {
    get => this.diseaseData.uninfectedCountryCount;
    set => this.diseaseData.uninfectedCountryCount = value;
  }

  public override int lethalityDelayTurns
  {
    get => this.diseaseData.lethalityDelayTurns;
    set => this.diseaseData.lethalityDelayTurns = value;
  }

  public override int turnsUntilGameEnd
  {
    get => this.diseaseData.turnsUntilGameEnd;
    set
    {
      this.diseaseData.turnsUntilGameEnd = value;
      if (World.instance == null)
        return;
      World.instance.turnsUntilGameEnd = value;
    }
  }

  public override int totalFlaskBroken
  {
    get => this.diseaseData.totalFlaskBroken;
    set => this.diseaseData.totalFlaskBroken = value;
  }

  public override int totalFlaskResearched
  {
    get => this.diseaseData.totalFlaskResearched;
    set => this.diseaseData.totalFlaskResearched = value;
  }

  public override int totalFlaskEmpty
  {
    get => this.diseaseData.totalFlaskEmpty;
    set => this.diseaseData.totalFlaskEmpty = value;
  }

  public override Disease.EDiseaseType diseaseType
  {
    get => this.diseaseData.diseaseType;
    set => this.diseaseData.diseaseType = value;
  }

  public override float zombieTechMummy
  {
    get => this.diseaseData.zombieTechMummy;
    set => this.diseaseData.zombieTechMummy = value;
  }

  public override int _t2
  {
    get => this.diseaseData._t2;
    set => this.diseaseData._t2 = value;
  }

  public override bool isScenario
  {
    get => this.diseaseData.isScenario;
    set => this.diseaseData.isScenario = value;
  }

  public override int numCheats
  {
    get => this.diseaseData.numCheats;
    set => this.diseaseData.numCheats = value;
  }

  public override int cheatFlags
  {
    get => this.diseaseData.cheatFlags;
    set => this.diseaseData.cheatFlags = value;
  }

  public override int countryOrigin
  {
    get => this.diseaseData.countryOrigin;
    set => this.diseaseData.countryOrigin = value;
  }

  public override float populationImmunity
  {
    get => this.diseaseData.populationImmunity;
    set => this.diseaseData.populationImmunity = value;
  }

  public override float apeTotalHealthy
  {
    get => this.diseaseData.apeTotalHealthy;
    set => this.diseaseData.apeTotalHealthy = value;
  }

  public override float apeTotalInfected
  {
    get => this.diseaseData.apeTotalInfected;
    set => this.diseaseData.apeTotalInfected = value;
  }

  public override float apeTotalDead
  {
    get => this.diseaseData.apeTotalDead;
    set => this.diseaseData.apeTotalDead = value;
  }

  public override float apeTotalAliveGlobal
  {
    get => this.diseaseData.apeTotalAliveGlobal;
    set => this.diseaseData.apeTotalAliveGlobal = value;
  }

  public override float apeTotalHealthyPercent
  {
    get => this.diseaseData.apeTotalHealthyPercent;
    set => this.diseaseData.apeTotalHealthyPercent = value;
  }

  public override float apeTotalInfectedPercent
  {
    get => this.diseaseData.apeTotalInfectedPercent;
    set => this.diseaseData.apeTotalInfectedPercent = value;
  }

  public override float apeTotalDeadPercent
  {
    get => this.diseaseData.apeTotalDeadPercent;
    set => this.diseaseData.apeTotalDeadPercent = value;
  }

  public override float apeXSpeciesInfectiousness
  {
    get => this.diseaseData.apeXSpeciesInfectiousness;
    set => this.diseaseData.apeXSpeciesInfectiousness = value;
  }

  public override float apeInfectiousness
  {
    get => this.diseaseData.apeInfectiousness;
    set => this.diseaseData.apeInfectiousness = value;
  }

  public override float apeRescueAbility
  {
    get => this.diseaseData.apeRescueAbility;
    set => this.diseaseData.apeRescueAbility = value;
  }

  public override float apeStrength
  {
    get => this.diseaseData.apeStrength;
    set => this.diseaseData.apeStrength = value;
  }

  public override float apeLethality
  {
    get => this.diseaseData.apeLethality;
    set => this.diseaseData.apeLethality = value;
  }

  public override float apeIntelligence
  {
    get => this.diseaseData.apeIntelligence;
    set => this.diseaseData.apeIntelligence = value;
  }

  public override float apeSurvival
  {
    get => this.diseaseData.apeSurvival;
    set => this.diseaseData.apeSurvival = value;
  }

  public override float apeSpeed
  {
    get => this.diseaseData.apeSpeed;
    set => this.diseaseData.apeSpeed = value;
  }

  public override float changeToHumanImmunity
  {
    get => this.diseaseData.changeToHumanImmunity;
    set => this.diseaseData.changeToHumanImmunity = value;
  }

  public override long apeHordeStash
  {
    get => this.diseaseData.apeHordeStash;
    set => this.diseaseData.apeHordeStash = value;
  }

  public override float apeHordeSpeed
  {
    get => this.diseaseData.apeHordeSpeed;
    set => this.diseaseData.apeHordeSpeed = value;
  }

  public override int daysSinceGlobalDrone
  {
    get => this.diseaseData.daysSinceGlobalDrone;
    set => this.diseaseData.daysSinceGlobalDrone = value;
  }

  public override float apePriorityLevelGlobal
  {
    get => this.diseaseData.apePriorityLevelGlobal;
    set => this.diseaseData.apePriorityLevelGlobal = value;
  }

  public override float migrationCountryDistanceMax
  {
    get => this.diseaseData.migrationCountryDistanceMax;
    set => this.diseaseData.migrationCountryDistanceMax = value;
  }

  public override float migrationDistanceWaterMod
  {
    get => this.diseaseData.migrationDistanceWaterMod;
    set => this.diseaseData.migrationDistanceWaterMod = value;
  }

  public override float migrationDistanceLandMod
  {
    get => this.diseaseData.migrationDistanceLandMod;
    set => this.diseaseData.migrationDistanceLandMod = value;
  }

  public override int apeNoticed
  {
    get => this.diseaseData.apeNoticed;
    set => this.diseaseData.apeNoticed = value;
  }

  public override int intelligentApeNoticed
  {
    get => this.diseaseData.intelligentApeNoticed;
    set => this.diseaseData.intelligentApeNoticed = value;
  }

  public override int apeTotalLabs
  {
    get => this.diseaseData.apeTotalLabs;
    set => this.diseaseData.apeTotalLabs = value;
  }

  public override int apeMaxLabs
  {
    get => this.diseaseData.apeMaxLabs;
    set => this.diseaseData.apeMaxLabs = value;
  }

  public override int apeTotalColonies
  {
    get => this.diseaseData.apeTotalColonies;
    set => this.diseaseData.apeTotalColonies = value;
  }

  public override int apeMaxColonies
  {
    get => this.diseaseData.apeMaxColonies;
    set => this.diseaseData.apeMaxColonies = value;
  }

  public override int apeDaysSinceLastColonyBubble
  {
    get => this.diseaseData.apeDaysSinceLastColonyBubble;
    set => this.diseaseData.apeDaysSinceLastColonyBubble = value;
  }

  public override int apeColonyChance
  {
    get => this.diseaseData.apeColonyChance;
    set => this.diseaseData.apeColonyChance = value;
  }

  public override int apeNumCreateColonyAAUsed
  {
    get => this.diseaseData.apeNumCreateColonyAAUsed;
    set => this.diseaseData.apeNumCreateColonyAAUsed = value;
  }

  public override float labBaseResearch
  {
    get => this.diseaseData.labBaseResearch;
    set => this.diseaseData.labBaseResearch = value;
  }

  public override int labCounter
  {
    get => this.diseaseData.labCounter;
    set => this.diseaseData.labCounter = value;
  }

  public override int labSpawnThreshold
  {
    get => this.diseaseData.labSpawnThreshold;
    set => this.diseaseData.labSpawnThreshold = value;
  }

  public override float apeBonusPriorityGlobal
  {
    get => this.diseaseData.apeBonusPriorityGlobal;
    set => this.diseaseData.apeBonusPriorityGlobal = value;
  }

  public override int labDayCounter
  {
    get => this.diseaseData.labDayCounter;
    set => this.diseaseData.labDayCounter = value;
  }

  public override int apeTotalDestroyedLabs
  {
    get => this.diseaseData.apeTotalDestroyedLabs;
    set => this.diseaseData.apeTotalDestroyedLabs = value;
  }

  public override int apeEscapeFlag
  {
    get => this.diseaseData.apeEscapeFlag;
    set => this.diseaseData.apeEscapeFlag = value;
  }

  public override int genSysWorking
  {
    get => this.diseaseData.genSysWorking;
    set => this.diseaseData.genSysWorking = value;
  }

  public override int simianNarrativePath
  {
    get => this.diseaseData.simianNarrativePath;
    set => this.diseaseData.simianNarrativePath = value;
  }

  public override float colonyDNABoost
  {
    get => this.diseaseData.colonyDNABoost;
    set => this.diseaseData.colonyDNABoost = value;
  }

  public override float colonyInfectionBoost
  {
    get => this.diseaseData.colonyInfectionBoost;
    set => this.diseaseData.colonyInfectionBoost = value;
  }

  public override int droneAttackFlag
  {
    get => this.diseaseData.droneAttackFlag;
    set => this.diseaseData.droneAttackFlag = value;
  }

  public override int apeHumanResponse
  {
    get => this.diseaseData.apeHumanResponse;
    set => this.diseaseData.apeHumanResponse = value;
  }

  public override int apeColonyBonusPoints
  {
    get => this.diseaseData.apeColonyBonusPoints;
    set => this.diseaseData.apeColonyBonusPoints = value;
  }

  public override int apeSlowDeathFlag
  {
    get => this.diseaseData.apeSlowDeathFlag;
    set => this.diseaseData.apeSlowDeathFlag = value;
  }

  public override int apeInfectiousnessBonusFlag
  {
    get => this.diseaseData.apeInfectiousnessBonusFlag;
    set => this.diseaseData.apeInfectiousnessBonusFlag = value;
  }

  public override int labDestroyDnaFlag
  {
    get => this.diseaseData.labDestroyDnaFlag;
    set => this.diseaseData.labDestroyDnaFlag = value;
  }

  public override int reducedLabResearchFlag
  {
    get => this.diseaseData.reducedLabResearchFlag;
    set => this.diseaseData.reducedLabResearchFlag = value;
  }

  public override int droneThreshold
  {
    get => this.diseaseData.droneThreshold;
    set => this.diseaseData.droneThreshold = value;
  }

  public override long apeTotalPopulation
  {
    get => this.diseaseData.apeTotalPopulation;
    set => this.diseaseData.apeTotalPopulation = value;
  }

  public override int noIdeaFlag
  {
    get => this.diseaseData.noIdeaFlag;
    set => this.diseaseData.noIdeaFlag = value;
  }

  public override float sporeCounter
  {
    get => this.diseaseData.sporeCounter;
    set => this.diseaseData.sporeCounter = value;
  }

  public override float decayPercentReduction
  {
    get => this.diseaseData.decayPercentReduction;
    set => this.diseaseData.decayPercentReduction = value;
  }

  public override float geneCompressionCounter
  {
    get => this.diseaseData.geneCompressionCounter;
    set => this.diseaseData.geneCompressionCounter = value;
  }

  public override float nucleicAcidFlag
  {
    get => this.diseaseData.nucleicAcidFlag;
    set => this.diseaseData.nucleicAcidFlag = value;
  }

  public override float replicationOverloadFlag
  {
    get => this.diseaseData.replicationOverloadFlag;
    set => this.diseaseData.replicationOverloadFlag = value;
  }

  public override float interceptorOverloadFlag
  {
    get => this.diseaseData.interceptorOverloadFlag;
    set => this.diseaseData.interceptorOverloadFlag = value;
  }

  public override float apeIntelligenceFlag
  {
    get => this.diseaseData.apeIntelligenceFlag;
    set => this.diseaseData.apeIntelligenceFlag = value;
  }

  public override float recomputePathsFlag
  {
    get => this.diseaseData.recomputePathsFlag;
    set => this.diseaseData.recomputePathsFlag = value;
  }

  public override int transcendenceFlag
  {
    get => this.diseaseData.transcendenceFlag;
    set => this.diseaseData.transcendenceFlag = value;
  }

  public override int trojanInfectiousness
  {
    get => this.diseaseData.trojanInfectiousness;
    set => this.diseaseData.trojanInfectiousness = value;
  }

  public override int trojanLethality
  {
    get => this.diseaseData.trojanLethality;
    set => this.diseaseData.trojanLethality = value;
  }

  public override float trojanPublicOrder
  {
    get => (float) this.diseaseData.trojanPublicOrder / 10000f;
    set => this.diseaseData.trojanPublicOrder = Mathf.RoundToInt(value * 10000f);
  }

  public override int trojanDna
  {
    get => this.diseaseData.trojanDna;
    set => this.diseaseData.trojanDna = value;
  }

  public override int trojanInfected
  {
    get => this.diseaseData.trojanInfected;
    set => this.diseaseData.trojanInfected = value;
  }

  public override int migrationCounter
  {
    get => this.diseaseData.migrationCounter;
    set => this.diseaseData.migrationCounter = value;
  }

  public override bool isVampire
  {
    get => this.diseaseData.isVampire;
    set => this.diseaseData.isVampire = value;
  }

  public override bool vday
  {
    get => this.diseaseData.vday;
    set => this.diseaseData.vday = value;
  }

  public override bool vdayDone
  {
    get => this.diseaseData.vdayDone;
    set => this.diseaseData.vdayDone = value;
  }

  public override bool shadowDay
  {
    get => this.diseaseData.shadowDay;
    set => this.diseaseData.shadowDay = value;
  }

  public override int vampireBonus
  {
    get => this.diseaseData.vampireBonus;
    set => this.diseaseData.vampireBonus = value;
  }

  public override bool shadowDayDone
  {
    get => this.diseaseData.shadowDayDone;
    set => this.diseaseData.shadowDayDone = value;
  }

  public override int shadowDayCounter
  {
    get => this.diseaseData.shadowDayCounter;
    set => this.diseaseData.shadowDayCounter = value;
  }

  public override int vdayCounter
  {
    get => this.diseaseData.vdayCounter;
    set => this.diseaseData.vdayCounter = value;
  }

  public override int shadowDayLength
  {
    get => this.diseaseData.shadowDayLength;
    set => this.diseaseData.shadowDayLength = value;
  }

  public override int vdayLength
  {
    get => this.diseaseData.vdayLength;
    set => this.diseaseData.vdayLength = value;
  }

  public override int vampireInfectionBoost
  {
    get => this.diseaseData.vampireInfectionBoost;
    set => this.diseaseData.vampireInfectionBoost = value;
  }

  public override float castleHealingMod
  {
    get => this.diseaseData.castleHealingMod;
    set => this.diseaseData.castleHealingMod = value;
  }

  public override float castleColdClimateResearchMod
  {
    get => this.diseaseData.castleColdClimateResearchMod;
    set => this.diseaseData.castleColdClimateResearchMod = value;
  }

  public override int vampireStealthMod
  {
    get => this.diseaseData.vampireStealthMod;
    set => this.diseaseData.vampireStealthMod = value;
  }

  public override float vampireConversionMod
  {
    get => this.diseaseData.vampireConversionMod;
    set => this.diseaseData.vampireConversionMod = value;
  }

  public override bool shadowPlagueActive
  {
    get => this.diseaseData.shadowPlagueActive;
    set => this.diseaseData.shadowPlagueActive = value;
  }

  public override float castleWealthyResearchMod
  {
    get => this.diseaseData.castleWealthyResearchMod;
    set => this.diseaseData.castleWealthyResearchMod = value;
  }

  public override float massHypnosisCost
  {
    get => this.diseaseData.massHypnosisCost;
    set => this.diseaseData.massHypnosisCost = value;
  }

  public override float vampireHypnosisImpact
  {
    get => this.diseaseData.vampireHypnosisImpact;
    set => this.diseaseData.vampireHypnosisImpact = value;
  }

  public override int numCastleBubblesWithoutTouch
  {
    get => this.diseaseData.numCastleBubblesWithoutTouch;
    set => this.diseaseData.numCastleBubblesWithoutTouch = value;
  }

  public override int castleDnaCounter
  {
    get => this.diseaseData.castleDnaCounter;
    set => this.diseaseData.castleDnaCounter = value;
  }

  public override float castleReturnSpeed
  {
    get => this.diseaseData.castleReturnSpeed;
    set => this.diseaseData.castleReturnSpeed = value;
  }

  public override float castleColdResearch
  {
    get => this.diseaseData.castleColdResearch;
    set => this.diseaseData.castleColdResearch = value;
  }

  public override float castleHotResearch
  {
    get => this.diseaseData.castleHotResearch;
    set => this.diseaseData.castleHotResearch = value;
  }

  public override float castleWealthyResearch
  {
    get => this.diseaseData.castleWealthyResearch;
    set => this.diseaseData.castleWealthyResearch = value;
  }

  public override float castleHeatClimateResearchMod
  {
    get => this.diseaseData.castleHeatClimateResearchMod;
    set => this.diseaseData.castleHeatClimateResearchMod = value;
  }

  public override float vcomAlert
  {
    get => this.diseaseData.vcomAlert;
    set => this.diseaseData.vcomAlert = value;
  }

  public override int pulseCastles
  {
    get => this.diseaseData.pulseCastles;
    set => this.diseaseData.pulseCastles = value;
  }

  public override float vampireActivity
  {
    get => this.diseaseData.vampireActivity;
    set => this.diseaseData.vampireActivity = value;
  }

  public override int vampireLabLastspawn
  {
    get => this.diseaseData.vampireLabLastspawn;
    set => this.diseaseData.vampireLabLastspawn = value;
  }

  public override long vampireConversionPot
  {
    get => this.diseaseData.vampireConversionPot;
    set => this.diseaseData.vampireConversionPot = value;
  }

  public override long vampireConversionPotTrigger
  {
    get => this.diseaseData.vampireConversionPotTrigger;
    set => this.diseaseData.vampireConversionPotTrigger = value;
  }

  public override int purityEnabledFlag
  {
    get => this.diseaseData.purityEnabledFlag;
    set => this.diseaseData.purityEnabledFlag = value;
  }

  public override int vampLabWorking
  {
    get => this.diseaseData.vampLabWorking;
    set => this.diseaseData.vampLabWorking = value;
  }

  public override int vampLabsDestroyed
  {
    get => this.diseaseData.vampLabsDestroyed;
    set => this.diseaseData.vampLabsDestroyed = value;
  }

  public override int vampLabsCurrent
  {
    get => this.diseaseData.vampLabsCurrent;
    set => this.diseaseData.vampLabsCurrent = value;
  }

  public override float globalVampireActivityBonus
  {
    get => this.diseaseData.globalVampireActivityBonus;
    set => this.diseaseData.globalVampireActivityBonus = value;
  }

  public override int vampireNarrativeStory
  {
    get => this.diseaseData.vampireNarrativeStory;
    set => this.diseaseData.vampireNarrativeStory = value;
  }

  public override int vampHealthIncrease
  {
    get => this.diseaseData.vampHealthIncrease;
    set => this.diseaseData.vampHealthIncrease = value;
  }

  public override int vampLabFortDnaBonus
  {
    get => this.diseaseData.vampLabFortDnaBonus;
    set => this.diseaseData.vampLabFortDnaBonus = value;
  }

  public override int vampRageCostZero
  {
    get => this.diseaseData.vampRageCostZero;
    set => this.diseaseData.vampRageCostZero = value;
  }

  public override int vampBloodRageCasulatiesIncreased
  {
    get => this.diseaseData.vampBloodRageCasulatiesIncreased;
    set => this.diseaseData.vampBloodRageCasulatiesIncreased = value;
  }

  public override int vampBloodRageBonusDna
  {
    get => this.diseaseData.vampBloodRageBonusDna;
    set => this.diseaseData.vampBloodRageBonusDna = value;
  }

  public override int vampBatRangeBonus
  {
    get => this.diseaseData.vampBatRangeBonus;
    set => this.diseaseData.vampBatRangeBonus = value;
  }

  public override int vampFlightCostsZero
  {
    get => this.diseaseData.vampFlightCostsZero;
    set => this.diseaseData.vampFlightCostsZero = value;
  }

  public override int vampMoreHealthFasterFlight
  {
    get => this.diseaseData.vampMoreHealthFasterFlight;
    set => this.diseaseData.vampMoreHealthFasterFlight = value;
  }

  public override int vampFlyFasterLoseHealth
  {
    get => this.diseaseData.vampFlyFasterLoseHealth;
    set => this.diseaseData.vampFlyFasterLoseHealth = value;
  }

  public override int vampAutomaticBloodRage
  {
    get => this.diseaseData.vampAutomaticBloodRage;
    set => this.diseaseData.vampAutomaticBloodRage = value;
  }

  public override int vampHealingBonus
  {
    get => this.diseaseData.vampHealingBonus;
    set => this.diseaseData.vampHealingBonus = value;
  }

  public override int vampLairDefenseBonus
  {
    get => this.diseaseData.vampLairDefenseBonus;
    set => this.diseaseData.vampLairDefenseBonus = value;
  }

  public override int vampActivityLairCountryBonus
  {
    get => this.diseaseData.vampActivityLairCountryBonus;
    set => this.diseaseData.vampActivityLairCountryBonus = value;
  }

  public override int vampLairDnaQuicker
  {
    get => this.diseaseData.vampLairDnaQuicker;
    set => this.diseaseData.vampLairDnaQuicker = value;
  }

  public override int castleNumber
  {
    get => this.diseaseData.castleNumber;
    set => this.diseaseData.castleNumber = value;
  }

  public override int fortDestroyedNumber
  {
    get => this.diseaseData.fortDestroyedNumber;
    set => this.diseaseData.fortDestroyedNumber = value;
  }

  public override int numberOfFortsCreated
  {
    get => this.diseaseData.numberOfFortsCreated;
    set => this.diseaseData.numberOfFortsCreated = value;
  }

  public override float fortDamageBonus
  {
    get => this.diseaseData.fortDamageBonus;
    set => this.diseaseData.fortDamageBonus = value;
  }

  public override float fortCureBonus
  {
    get => this.diseaseData.fortCureBonus;
    set => this.diseaseData.fortCureBonus = value;
  }

  public override float numberOfFortsToSpawn
  {
    get => this.diseaseData.numberOfFortsToSpawn;
    set => this.diseaseData.numberOfFortsToSpawn = value;
  }

  public override int maxVampLabs
  {
    get => this.diseaseData.maxVampLabs;
    set => this.diseaseData.maxVampLabs = value;
  }

  public override float vampHealSacrificeMod
  {
    get => this.diseaseData.vampHealSacrificeMod;
    set => this.diseaseData.vampHealSacrificeMod = value;
  }

  public override int lairDroneAttackTimer
  {
    get => this.diseaseData.lairDroneAttackTimer;
    set => this.diseaseData.lairDroneAttackTimer = value;
  }

  public override float lairDroneAttackDuration
  {
    get => this.diseaseData.lairDroneAttackDuration;
    set => this.diseaseData.lairDroneAttackDuration = value;
  }

  public override int templarDestroyed
  {
    get => this.diseaseData.templarDestroyed;
    set => this.diseaseData.templarDestroyed = value;
  }

  public override long vampireHordeStash
  {
    get => this.diseaseData.vampireHordeStash;
    set => this.diseaseData.vampireHordeStash = value;
  }

  public override bool vampireWin
  {
    get => this.diseaseData.vampireWin;
    set => this.diseaseData.vampireWin = value;
  }

  public override bool vampireLoss
  {
    get => this.diseaseData.vampireLoss;
    set => this.diseaseData.vampireLoss = value;
  }

  public override int brexitExecute
  {
    get => this.diseaseData.brexitExecute;
    set => this.diseaseData.brexitExecute = value;
  }

  public override float deadBodyTransmission
  {
    get => this.diseaseData.deadBodyTransmission;
    set => this.diseaseData.deadBodyTransmission = value;
  }

  public override bool isFakeNews
  {
    get => this.diseaseData.isFakeNews;
    set => this.diseaseData.isFakeNews = value;
  }

  public override bool fakeNewsStarted
  {
    get => this.diseaseData.fakeNewsStarted;
    set => this.diseaseData.fakeNewsStarted = value;
  }

  public override float fakeNewsInformDropPercent
  {
    get => this.diseaseData.fakeNewsInformDropPercent;
    set
    {
      this.diseaseData.fakeNewsInformDropPercent = value;
      if ((double) value == 0.0)
        return;
      Debug.Log((object) ("SETTING INFORM DROP PERCENT: " + (object) value));
    }
  }

  public override bool isCure
  {
    get => this.diseaseData.isCure;
    set => this.diseaseData.isCure = value;
  }

  public override int cureDiseaseDiscoveredTurn
  {
    get => this.diseaseData.cureDiseaseDiscoveredTurn;
    set => this.diseaseData.cureDiseaseDiscoveredTurn = value;
  }

  public override int preSimulate
  {
    get => this.diseaseData.preSimulate;
    set => this.diseaseData.preSimulate = value;
  }

  public override long totalHealthyRecovered
  {
    get => this.diseaseData.totalHealthyRecovered;
    set => this.diseaseData.totalHealthyRecovered = value;
  }

  public override long totalInfectedIntel
  {
    get => this.diseaseData.totalInfectedIntel;
    set => this.diseaseData.totalInfectedIntel = value;
  }

  public override long totalDeadIntel
  {
    get => this.diseaseData.totalDeadIntel;
    set => this.diseaseData.totalDeadIntel = value;
  }

  public override long totalHealthyRecoveredIntel
  {
    get => this.diseaseData.totalHealthyRecoveredIntel;
    set => this.diseaseData.totalHealthyRecoveredIntel = value;
  }

  public override long totalInfectedIntelGUI
  {
    get => this.diseaseData.totalInfectedIntelGUI;
    set => this.diseaseData.totalInfectedIntelGUI = value;
  }

  public override long totalDeadIntelGUI
  {
    get => this.diseaseData.totalDeadIntelGUI;
    set => this.diseaseData.totalDeadIntelGUI = value;
  }

  public override long totalHealthyRecoveredIntelGUI
  {
    get => this.diseaseData.totalHealthyRecoveredIntelGUI;
    set => this.diseaseData.totalHealthyRecoveredIntelGUI = value;
  }

  public override long totalHealthyRecoveredGUI
  {
    get => this.diseaseData.totalHealthyRecoveredGUI;
    set => this.diseaseData.totalHealthyRecoveredGUI = value;
  }

  public override int numCountriesIntel
  {
    get => this.diseaseData.numCountriesIntel;
    set => this.diseaseData.numCountriesIntel = value;
  }

  public override int autoHqCounter
  {
    get => this.diseaseData.autoHqCounter;
    set => this.diseaseData.autoHqCounter = value;
  }

  public override int nextIntelSpread
  {
    get => this.diseaseData.nextIntelSpread;
    set => this.diseaseData.nextIntelSpread = value;
  }

  public override int intelTimeReduction
  {
    get => this.diseaseData.intelTimeReduction;
    set => this.diseaseData.intelTimeReduction = value;
  }

  public override bool intelInfectedFound
  {
    get => this.diseaseData.intelInfectedFound;
    set => this.diseaseData.intelInfectedFound = value;
  }

  public override Disease.EVaccineProgressStage vaccineStage
  {
    get => this.diseaseData.vaccineStage;
    set => this.diseaseData.vaccineStage = value;
  }

  public override float vaccineKnowledge
  {
    get => this.diseaseData.vaccineKnowledge;
    set => this.diseaseData.vaccineKnowledge = value;
  }

  public override float vaccineKnowledgeMonths
  {
    get => this.diseaseData.vaccineKnowledgeMonths;
    set => this.diseaseData.vaccineKnowledgeMonths = value;
  }

  public override float vaccineKnowledgeMonthsStart
  {
    get => this.diseaseData.vaccineKnowledgeMonthsStart;
    set => this.diseaseData.vaccineKnowledgeMonthsStart = value;
  }

  public override float vaccineDevMonths
  {
    get => this.diseaseData.vaccineDevMonths;
    set => this.diseaseData.vaccineDevMonths = value;
  }

  public override float vaccineManMonths
  {
    get => this.diseaseData.vaccineManMonths;
    set => this.diseaseData.vaccineManMonths = value;
  }

  public override float vaccineReleaseMonths
  {
    get => this.diseaseData.vaccineReleaseMonths;
    set => this.diseaseData.vaccineReleaseMonths = value;
  }

  public override float developmentSpeed
  {
    get => this.diseaseData.developmentSpeed;
    set => this.diseaseData.developmentSpeed = value;
  }

  public override float understandingSpeed
  {
    get => this.diseaseData.understandingSpeed;
    set => this.diseaseData.understandingSpeed = value;
  }

  public override float manufactureSpeed
  {
    get => this.diseaseData.manufactureSpeed;
    set => this.diseaseData.manufactureSpeed = value;
  }

  public override float manufactureSpeedAuthBonus
  {
    get => this.diseaseData.manufactureSpeedAuthBonus;
    set => this.diseaseData.manufactureSpeedAuthBonus = value;
  }

  public override float manufactureProgress
  {
    get => this.diseaseData.manufactureProgress;
    set => this.diseaseData.manufactureProgress = value;
  }

  public override bool manufactureSet
  {
    get => this.diseaseData.manufactureSet;
    set => this.diseaseData.manufactureSet = value;
  }

  public override int totalVaccineDuration
  {
    get => this.diseaseData.totalVaccineDuration;
    set => this.diseaseData.totalVaccineDuration = value;
  }

  public override int vaccineFailCount
  {
    get => this.diseaseData.vaccineFailCount;
    set => this.diseaseData.vaccineFailCount = value;
  }

  public override bool skipDevSuccess
  {
    get => this.diseaseData.skipDevSuccess;
    set => this.diseaseData.skipDevSuccess = value;
  }

  public override bool skipDevFired
  {
    get => this.diseaseData.skipDevFired;
    set => this.diseaseData.skipDevFired = value;
  }

  public override int barPulseCounter
  {
    get => this.diseaseData.barPulseCounter;
    set => this.diseaseData.barPulseCounter = value;
  }

  public override float globalMedicalCapacityMultiplier
  {
    get => this.diseaseData.globalMedicalCapacityMultiplier;
    set => this.diseaseData.globalMedicalCapacityMultiplier = value;
  }

  public override float globalInfectMod
  {
    get => this.diseaseData.globalInfectMod;
    set => this.diseaseData.globalInfectMod = value;
  }

  public override float globalInfectModMAX
  {
    get => this.diseaseData.globalInfectModMAX;
    set => this.diseaseData.globalInfectModMAX = value;
  }

  public override float globalLethalityMod
  {
    get => this.diseaseData.globalLethalityMod;
    set => this.diseaseData.globalLethalityMod = value;
  }

  public override float medicalCapacityEffectivenessMulti
  {
    get => this.diseaseData.medicalCapacityEffectivenessMulti;
    set => this.diseaseData.medicalCapacityEffectivenessMulti = value;
  }

  public override float economyDefenseEffectivenessMulti
  {
    get => this.diseaseData.economyDefenseEffectivenessMulti;
    set => this.diseaseData.economyDefenseEffectivenessMulti = value;
  }

  public override float contactTracingEffectiveness
  {
    get => this.diseaseData.contactTracingEffectiveness;
    set => this.diseaseData.contactTracingEffectiveness = value;
  }

  public override float contactTracingEffectivenessMod
  {
    get => this.diseaseData.contactTracingEffectivenessMod;
    set => this.diseaseData.contactTracingEffectivenessMod = value;
  }

  public override float contactTracingEffectivenessMult
  {
    get => this.diseaseData.contactTracingEffectivenessMult;
    set => this.diseaseData.contactTracingEffectivenessMult = value;
  }

  public override float lockdownEffectiveness
  {
    get => this.diseaseData.lockdownEffectiveness;
    set => this.diseaseData.lockdownEffectiveness = value;
  }

  public override float lockdownEffectivenessMod
  {
    get => this.diseaseData.lockdownEffectivenessMod;
    set => this.diseaseData.lockdownEffectivenessMod = value;
  }

  public override float lockdownEffectivenessMult
  {
    get => this.diseaseData.lockdownEffectivenessMult;
    set => this.diseaseData.lockdownEffectivenessMult = value;
  }

  public override float globalLocalPriorityMultiplier
  {
    get => this.diseaseData.globalLocalPriorityMultiplier;
    set => this.diseaseData.globalLocalPriorityMultiplier = value;
  }

  public override float globalPriorityAlertModifier
  {
    get => this.diseaseData.globalPriorityAlertModifier;
    set => this.diseaseData.globalPriorityAlertModifier = value;
  }

  public override float highestLocalPriority
  {
    get => this.diseaseData.highestLocalPriority;
    set => this.diseaseData.highestLocalPriority = value;
  }

  public override float estimatedDeathRate
  {
    get => this.diseaseData.estimatedDeathRate;
    set => this.diseaseData.estimatedDeathRate = value;
  }

  public override float connectedLocalPriorityMultiplier
  {
    get => this.diseaseData.connectedLocalPriorityMultiplier;
    set => this.diseaseData.connectedLocalPriorityMultiplier = value;
  }

  public override float influencePoints
  {
    get => this.diseaseData.influencePoints;
    set => this.diseaseData.influencePoints = value;
  }

  public override float globalBaseInfluence
  {
    get => this.diseaseData.globalBaseInfluence;
    set => this.diseaseData.globalBaseInfluence = value;
  }

  public override float quarantineInfluence
  {
    get => this.diseaseData.quarantineInfluence;
    set => this.diseaseData.quarantineInfluence = value;
  }

  public override int alertLevel
  {
    get => this.diseaseData.alertLevel;
    set => this.diseaseData.alertLevel = value;
  }

  public override float teamHighestInfectedPerc
  {
    get => this.diseaseData.teamHighestInfectedPerc;
    set => this.diseaseData.teamHighestInfectedPerc = value;
  }

  public override float globalUnrestMod
  {
    get => this.diseaseData.globalUnrestMod;
    set => this.diseaseData.globalUnrestMod = value;
  }

  public override float reproductionVisual
  {
    get => this.diseaseData.reproductionVisual;
    set => this.diseaseData.reproductionVisual = value;
  }

  public override float mortalityVisual
  {
    get => this.diseaseData.mortalityVisual;
    set => this.diseaseData.mortalityVisual = value;
  }

  public override float unrestVisual
  {
    get => this.diseaseData.unrestVisual;
    set => this.diseaseData.unrestVisual = value;
  }

  public override float medicalAidDuration
  {
    get => this.diseaseData.medicalAidDuration;
    set => this.diseaseData.medicalAidDuration = value;
  }

  public override float investigatorsDuration
  {
    get => this.diseaseData.investigatorsDuration;
    set => this.diseaseData.investigatorsDuration = value;
  }

  public override float tempLockdownDuration
  {
    get => this.diseaseData.tempLockdownDuration;
    set => this.diseaseData.tempLockdownDuration = value;
  }

  public override bool easyIntel
  {
    get => this.diseaseData.easyIntel;
    set => this.diseaseData.easyIntel = value;
  }

  public override float landTravelRestriction
  {
    get => this.diseaseData.landTravelRestriction;
    set => this.diseaseData.landTravelRestriction = value;
  }

  public override float airTravelRestriction
  {
    get => this.diseaseData.airTravelRestriction;
    set => this.diseaseData.airTravelRestriction = value;
  }

  public override float oceanTravelRestriction
  {
    get => this.diseaseData.oceanTravelRestriction;
    set => this.diseaseData.oceanTravelRestriction = value;
  }

  public override float seaScreeningChance
  {
    get => this.diseaseData.seaScreeningChance;
    set => this.diseaseData.seaScreeningChance = value;
  }

  public override float airScreeningChance
  {
    get => this.diseaseData.airScreeningChance;
    set => this.diseaseData.airScreeningChance = value;
  }

  public override float landScreeningChance
  {
    get => this.diseaseData.landScreeningChance;
    set => this.diseaseData.landScreeningChance = value;
  }

  public override float globalInfectedPercMAX
  {
    get => this.diseaseData.globalInfectedPercMAX;
    set => this.diseaseData.globalInfectedPercMAX = value;
  }

  public override int infectedCountriesMAX
  {
    get => this.diseaseData.infectedCountriesMAX;
    set => this.diseaseData.infectedCountriesMAX = value;
  }

  public override int turnsSinceKnowledge
  {
    get => this.diseaseData.turnsSinceKnowledge;
    set => this.diseaseData.turnsSinceKnowledge = value;
  }

  public override int turnsSinceDeadBubble
  {
    get => this.diseaseData.turnsSinceDeadBubble;
    set => this.diseaseData.turnsSinceDeadBubble = value;
  }

  public override int numTotalDeadBubbles
  {
    get => this.diseaseData.numTotalDeadBubbles;
    set => this.diseaseData.numTotalDeadBubbles = value;
  }

  public override float deadBubbleChance
  {
    get => this.diseaseData.deadBubbleChance;
    set => this.diseaseData.deadBubbleChance = value;
  }

  public override int numTotalDeadBubblesDNA
  {
    get => this.diseaseData.numTotalDeadBubblesDNA;
    set => this.diseaseData.numTotalDeadBubblesDNA = value;
  }

  public override int quarantinesActiveCount
  {
    get => this.diseaseData.quarantinesActiveCount;
    set => this.diseaseData.quarantinesActiveCount = value;
  }

  public override int supportsActiveCount
  {
    get => this.diseaseData.supportsActiveCount;
    set => this.diseaseData.supportsActiveCount = value;
  }

  public override float globalAvgCompliance
  {
    get => this.diseaseData.globalAvgCompliance;
    set => this.diseaseData.globalAvgCompliance = value;
  }

  public override float globalUnrestCount
  {
    get => this.diseaseData.globalUnrestCount;
    set => this.diseaseData.globalUnrestCount = value;
  }

  public override int lockdownTimerMAX
  {
    get => this.diseaseData.lockdownTimerMAX;
    set => this.diseaseData.lockdownTimerMAX = value;
  }

  public override int supportTimerMAX
  {
    get => this.diseaseData.supportTimerMAX;
    set => this.diseaseData.supportTimerMAX = value;
  }

  public override float economyTimeMulti
  {
    get => this.diseaseData.economyTimeMulti;
    set => this.diseaseData.economyTimeMulti = value;
  }

  public override long totalLockdownPopulation
  {
    get => this.diseaseData.totalLockdownPopulation;
    set => this.diseaseData.totalLockdownPopulation = value;
  }

  public override int totalActiveLockdowns
  {
    get => this.diseaseData.totalActiveLockdowns;
    set => this.diseaseData.totalActiveLockdowns = value;
  }

  public override int spreadCountries
  {
    get => this.diseaseData.spreadCountries;
    set => this.diseaseData.spreadCountries = value;
  }

  public override bool isNexusContinentBorder
  {
    get => this.diseaseData.isNexusContinentBorder;
    set => this.diseaseData.isNexusContinentBorder = value;
  }

  public override float startingAuthority
  {
    get => this.diseaseData.startingAuthority;
    set => this.diseaseData.startingAuthority = value;
  }

  public override float baseLethality
  {
    get => this.diseaseData.baseLethality;
    set => this.diseaseData.baseLethality = value;
  }

  public override float baseInfectivity
  {
    get => this.diseaseData.baseInfectivity;
    set => this.diseaseData.baseInfectivity = value;
  }

  public override float authority
  {
    get => this.diseaseData.authority;
    set => this.diseaseData.authority = value;
  }

  public override float lowestAuthority
  {
    get => this.diseaseData.lowestAuthority;
    set => this.diseaseData.lowestAuthority = value;
  }

  public override float authorityDeduction
  {
    get => this.diseaseData.authorityDeduction;
    set => this.diseaseData.authorityDeduction = value;
  }

  public override float authorityMod
  {
    get => this.diseaseData.authorityMod;
    set => this.diseaseData.authorityMod = value;
  }

  public override float authorityModHighest
  {
    get => this.diseaseData.authorityModHighest;
    set => this.diseaseData.authorityModHighest = value;
  }

  public override float authorityGainHold
  {
    get => this.diseaseData.authorityGainHold;
    set => this.diseaseData.authorityGainHold = value;
  }

  public override float authLossInfected
  {
    get => this.diseaseData.authLossInfected;
    set => this.diseaseData.authLossInfected = value;
  }

  public override float authLossInfectedDelay
  {
    get => this.diseaseData.authLossInfectedDelay;
    set => this.diseaseData.authLossInfectedDelay = value;
  }

  public override float authLossInfectedHighest
  {
    get => this.diseaseData.authLossInfectedHighest;
    set => this.diseaseData.authLossInfectedHighest = value;
  }

  public override float authLossInfectedActual
  {
    get => this.diseaseData.authLossInfectedActual;
    set => this.diseaseData.authLossInfectedActual = value;
  }

  public override float authLossInfectedPermanencePerc
  {
    get => this.diseaseData.authLossInfectedPermanencePerc;
    set => this.diseaseData.authLossInfectedPermanencePerc = value;
  }

  public override float authLossInfectedYesterday
  {
    get => this.diseaseData.authLossInfectedYesterday;
    set => this.diseaseData.authLossInfectedYesterday = value;
  }

  public override float authLossInfMulti
  {
    get => this.diseaseData.authLossInfMulti;
    set => this.diseaseData.authLossInfMulti = value;
  }

  public override float authLossDead
  {
    get => this.diseaseData.authLossDead;
    set => this.diseaseData.authLossDead = value;
  }

  public override float authLossDeadDelay
  {
    get => this.diseaseData.authLossDeadDelay;
    set => this.diseaseData.authLossDeadDelay = value;
  }

  public override float authLossDeadHighest
  {
    get => this.diseaseData.authLossDeadHighest;
    set => this.diseaseData.authLossDeadHighest = value;
  }

  public override float authLossDeadActual
  {
    get => this.diseaseData.authLossDeadActual;
    set => this.diseaseData.authLossDeadActual = value;
  }

  public override float authLossDeadPermanencePerc
  {
    get => this.diseaseData.authLossDeadPermanencePerc;
    set => this.diseaseData.authLossDeadPermanencePerc = value;
  }

  public override float authLossDeadYesterday
  {
    get => this.diseaseData.authLossDeadYesterday;
    set => this.diseaseData.authLossDeadYesterday = value;
  }

  public override float authLossDeadMulti
  {
    get => this.diseaseData.authLossDeadMulti;
    set => this.diseaseData.authLossDeadMulti = value;
  }

  public override float authLossSpread
  {
    get => this.diseaseData.authLossSpread;
    set => this.diseaseData.authLossSpread = value;
  }

  public override float authLossSpreadYesterday
  {
    get => this.diseaseData.authLossSpreadYesterday;
    set => this.diseaseData.authLossSpreadYesterday = value;
  }

  public override float authLossCompliance
  {
    get => this.diseaseData.authLossCompliance;
    set => this.diseaseData.authLossCompliance = value;
  }

  public override float authLossComplianceMulti
  {
    get => this.diseaseData.authLossComplianceMulti;
    set => this.diseaseData.authLossComplianceMulti = value;
  }

  public override float authCensorship
  {
    get => this.diseaseData.authCensorship;
    set => this.diseaseData.authCensorship = value;
  }

  public override float authCensorshipTimer
  {
    get => this.diseaseData.authCensorshipTimer;
    set => this.diseaseData.authCensorshipTimer = value;
  }

  public override float authSpreadMod
  {
    get => this.diseaseData.authSpreadMod;
    set => this.diseaseData.authSpreadMod = value;
  }

  public override int authFreezeCount
  {
    get => this.diseaseData.authFreezeCount;
    set => this.diseaseData.authFreezeCount = value;
  }

  public override float authWinMin
  {
    get => this.diseaseData.authWinMin;
    set => this.diseaseData.authWinMin = value;
  }

  public override int authLossDaysToTrack
  {
    get => this.diseaseData.authLossDaysToTrack;
    set => this.diseaseData.authLossDaysToTrack = value;
  }

  public override float complianceReturnPerc
  {
    get => this.diseaseData.complianceReturnPerc;
    set => this.diseaseData.complianceReturnPerc = value;
  }

  public override int globalBorderClosedNum
  {
    get => this.diseaseData.globalBorderClosedNum;
    set => this.diseaseData.globalBorderClosedNum = value;
  }

  public override int globalAirportClosedNum
  {
    get => this.diseaseData.globalAirportClosedNum;
    set => this.diseaseData.globalAirportClosedNum = value;
  }

  public override int globalPortClosedNum
  {
    get => this.diseaseData.globalPortClosedNum;
    set => this.diseaseData.globalPortClosedNum = value;
  }

  public override float globalBorderClosedPerc
  {
    get => this.diseaseData.globalBorderClosedPerc;
    set => this.diseaseData.globalBorderClosedPerc = value;
  }

  public override float globalAirportClosedPerc
  {
    get => this.diseaseData.globalAirportClosedPerc;
    set => this.diseaseData.globalAirportClosedPerc = value;
  }

  public override float globalPortClosedPerc
  {
    get => this.diseaseData.globalPortClosedPerc;
    set => this.diseaseData.globalPortClosedPerc = value;
  }

  public override int infectedCountriesIntel
  {
    get => this.diseaseData.infectedCountriesIntel;
    set => this.diseaseData.infectedCountriesIntel = value;
  }

  public override float diseaseDNAComplexity
  {
    get => this.diseaseData.diseaseDNAComplexity;
    set => this.diseaseData.diseaseDNAComplexity = value;
  }

  public override float globalVaccineSpeed
  {
    get => this.diseaseData.globalVaccineSpeed;
    set => this.diseaseData.globalVaccineSpeed = value;
  }

  public override float bonusLethality
  {
    get => this.diseaseData.bonusLethality;
    set => this.diseaseData.bonusLethality = value;
  }

  public override float scoreDeadImpactMulti
  {
    get => this.diseaseData.scoreDeadImpactMulti;
    set => this.diseaseData.scoreDeadImpactMulti = value;
  }

  public override float narrativeEventCounter
  {
    get => this.diseaseData.narrativeEventCounter;
    set => this.diseaseData.narrativeEventCounter = value;
  }

  public override float narrativeEventGap
  {
    get => this.diseaseData.narrativeEventGap;
    set => this.diseaseData.narrativeEventGap = value;
  }

  public override float cureBioweaponStrengthMod
  {
    get => this.diseaseData.cureBioweaponStrengthMod;
    set => this.diseaseData.cureBioweaponStrengthMod = value;
  }

  public override float cureBioweaponImpact
  {
    get => this.diseaseData.cureBioweaponImpact;
    set => this.diseaseData.cureBioweaponImpact = value;
  }

  public override float bioweaponLethalityGain
  {
    get => this.diseaseData.bioweaponLethalityGain;
    set => this.diseaseData.bioweaponLethalityGain = value;
  }

  public override float bioweaponInfectivityGain
  {
    get => this.diseaseData.bioweaponInfectivityGain;
    set => this.diseaseData.bioweaponInfectivityGain = value;
  }

  public override int prionIncubationMonths
  {
    get => this.diseaseData.prionIncubationMonths;
    set => this.diseaseData.prionIncubationMonths = value;
  }

  public override float prionVaccineSpeedMulti
  {
    get => this.diseaseData.prionVaccineSpeedMulti;
    set => this.diseaseData.prionVaccineSpeedMulti = value;
  }

  public override float prionInfectedThresholdMulti
  {
    get => this.diseaseData.prionInfectedThresholdMulti;
    set => this.diseaseData.prionInfectedThresholdMulti = value;
  }

  public override int preSimMAX
  {
    get => this.diseaseData.preSimMAX;
    set => this.diseaseData.preSimMAX = value;
  }

  public override int nanovirusPauseTimer
  {
    get => this.diseaseData.nanovirusPauseTimer;
    set => this.diseaseData.nanovirusPauseTimer = value;
  }

  public override int nanovirusEndTimer
  {
    get => this.diseaseData.nanovirusEndTimer;
    set => this.diseaseData.nanovirusEndTimer = value;
  }

  public override bool nanovirusHeatWeak
  {
    get => this.diseaseData.nanovirusHeatWeak;
    set => this.diseaseData.nanovirusHeatWeak = value;
  }

  public override bool nanovirusColdWeak
  {
    get => this.diseaseData.nanovirusColdWeak;
    set => this.diseaseData.nanovirusColdWeak = value;
  }

  public override bool isNanovirus
  {
    get => this.diseaseData.isNanovirus;
    set => this.diseaseData.isNanovirus = value;
  }

  public override bool isFungus
  {
    get => this.diseaseData.isFungus;
    set => this.diseaseData.isFungus = value;
  }

  public override bool isPrion
  {
    get => this.diseaseData.isPrion;
    set => this.diseaseData.isPrion = value;
  }

  public override float fungusBloom
  {
    get => this.diseaseData.fungusBloom;
    set => this.diseaseData.fungusBloom = value;
  }

  public override float fungusSporeMulti
  {
    get => this.diseaseData.fungusSporeMulti;
    set => this.diseaseData.fungusSporeMulti = value;
  }

  public override int fungusSporesReleased
  {
    get => this.diseaseData.fungusSporesReleased;
    set => this.diseaseData.fungusSporesReleased = value;
  }

  public override float localPopUpper
  {
    get => this.diseaseData.localPopUpper;
    set => this.diseaseData.localPopUpper = value;
  }

  public override float localPopLower
  {
    get => this.diseaseData.localPopLower;
    set => this.diseaseData.localPopLower = value;
  }

  public override float localPopThreshold
  {
    get => this.diseaseData.localPopThreshold;
    set => this.diseaseData.localPopThreshold = value;
  }

  public override float nationalPopUpper
  {
    get => this.diseaseData.nationalPopUpper;
    set => this.diseaseData.nationalPopUpper = value;
  }

  public override float nationalPopLower
  {
    get => this.diseaseData.nationalPopLower;
    set => this.diseaseData.nationalPopLower = value;
  }

  public override float nationalPopThreshold
  {
    get => this.diseaseData.nationalPopThreshold;
    set => this.diseaseData.nationalPopThreshold = value;
  }

  public override float reproductionBarScale
  {
    get => this.diseaseData.reproductionBarScale;
    set => this.diseaseData.reproductionBarScale = value;
  }

  public override float mortalityBarScale
  {
    get => this.diseaseData.mortalityBarScale;
    set => this.diseaseData.mortalityBarScale = value;
  }

  public override int diseaseLengthMulti
  {
    get => this.diseaseData.diseaseLengthMulti;
    set => this.diseaseData.diseaseLengthMulti = value;
  }

  public override float publicOrderAuthorityLossMod
  {
    get => this.diseaseData.publicOrderAuthorityLossMod;
    set => this.diseaseData.publicOrderAuthorityLossMod = value;
  }

  public override float averageCountryInfectedPerc
  {
    get => this.diseaseData.averageCountryInfectedPerc;
    set => this.diseaseData.averageCountryInfectedPerc = value;
  }

  public override float averageCountryDeadPerc
  {
    get => this.diseaseData.averageCountryDeadPerc;
    set => this.diseaseData.averageCountryDeadPerc = value;
  }

  public override float maxAuthLossPerCountry
  {
    get => this.diseaseData.maxAuthLossPerCountry;
    set => this.diseaseData.maxAuthLossPerCountry = value;
  }

  public override Disease.EGenericDiseaseFlag genericFlags
  {
    get => this.diseaseData.genericFlags;
    set => this.diseaseData.genericFlags = value;
  }

  public override Disease.ECureScenario cureScenario
  {
    get => this.diseaseData.cureScenario;
    set => this.diseaseData.cureScenario = value;
  }

  public override string scenario
  {
    get => this._scenario;
    set
    {
      this._scenario = value;
      this.diseaseData.isScenario = !string.IsNullOrEmpty(this._scenario);
    }
  }

  public override Country nexus
  {
    set
    {
      base.nexus = value;
      if (value == null)
        this.diseaseData.nexusCountryNumber = -1;
      else
        this.diseaseData.nexusCountryNumber = value.countryNumber;
    }
  }

  public override Country superCureCountry
  {
    set
    {
      base.superCureCountry = value;
      if (value == null)
        this.diseaseData.superCureCountryNumber = -1;
      else
        this.diseaseData.superCureCountryNumber = value.countryNumber;
    }
  }

  public override Country hqCountry
  {
    set
    {
      base.hqCountry = value;
      if (value == null)
        this.diseaseData.hqCountryNumber = -1;
      else
        this.diseaseData.hqCountryNumber = value.countryNumber;
    }
  }

  public override Country teamTravelTarget
  {
    set
    {
      base.teamTravelTarget = value;
      if (value == null)
        this.diseaseData.teamTravelTargetCountryNumber = -1;
      else
        this.diseaseData.teamTravelTargetCountryNumber = value.countryNumber;
    }
  }

  public SPDisease()
  {
    this.diseaseData = new SPDiseaseData();
    this.diseaseData.cureRequirementBase = 2.5E+07f;
    this.diseaseData.cureBaseMultiplier = 1f;
    this.diseaseData.geneticDriftMax = 1f;
    this.diseaseData.nexusMinInfect = 8f;
    this.diseaseData.nexusBonusGene = 5f;
    this.diseaseData.globalInfectiousnessTopMultipler = 0.1f;
    this.diseaseData.globalInfectiousnessBottomValue = 1f;
    this.diseaseData.globalSeverityTopMultipler = 0.1f;
    this.diseaseData.globalSeverityBottomValue = 1f;
    this.diseaseData.globalLethalityTopMultipler = 0.05f;
    this.diseaseData.globalLethalityBottomValue = 0.4f;
    this.diseaseData.cureRequirementBaseMultipler = 1.45f;
    this.diseaseData.infectedPointsPotChange = 5f;
    this.diseaseData.symptomCostIncrease = true;
    this.diseaseData.abilityCostIncrease = true;
    this.diseaseData.transmissionCostIncrease = true;
    this.diseaseData.zdayDead = 1E-07f;
    this.diseaseData.zombieConversionMod = 0.69f;
    this.diseaseData.zombieDecay = 3f / 1000f;
    this.diseaseData.zombieInfect = 1f / 1000f;
    this.diseaseData.zombieDecayTechMultiplier = 1f;
    this.diseaseData.globalZombieDecayMultiplier = 1f;
    this.diseaseData.hordeSpeed = 3.5f;
    this.diseaseData.hordeSize = 7f;
    this.diseaseData.reanimateSize = 0.035f;
    this.diseaseData.hordeWaterSpeed = 0.25f;
    this.diseaseData.turnsUntilGameEnd = 100;
    this.diseaseData.lethalityDelayTurns = -1;
    this.diseaseData.mutationTrigger = 60f;
    this.diseaseData.daysUntilMinifortPlaneSpawn = int.MaxValue;
    this.diseaseData.daysToGameWin = int.MaxValue;
    this.diseaseData.showExtraPopups = true;
    this.diseaseData.numCheats = 0;
    this.diseaseData.cheatFlags = 0;
    this.diseaseData.countryOrigin = 0;
    this.diseaseData.apeXSpeciesInfectiousness = 0.0f;
    this.diseaseData.apeInfectiousness = 0.0f;
    this.diseaseData.apeRescueAbility = 1f;
    this.diseaseData.apeMaxLabs = 3;
    this.diseaseData.apeMaxColonies = 2;
    this.diseaseData.apeDaysSinceLastColonyBubble = 0;
    this.diseaseData.apeColonyChance = 0;
    this.diseaseData.labBaseResearch = 25000f;
    this.diseaseData.migrationDistanceLandMod = 1f;
    this.diseaseData.migrationDistanceWaterMod = 1f;
    this.diseaseData.migrationCountryDistanceMax = 0.0f;
    this.diseaseData.apeHordeSpeed = 2f;
    this.diseaseData.apeStrength = 1f;
    this.diseaseData.droneThreshold = 350;
    this.diseaseData.noIdeaFlag = 0;
    this.diseaseData.sporeCounter = 0.0f;
    this.diseaseData.decayPercentReduction = 0.0f;
    this.diseaseData.geneCompressionCounter = 0.0f;
    this.diseaseData.nucleicAcidFlag = 0.0f;
    this.diseaseData.replicationOverloadFlag = 0.0f;
    this.diseaseData.interceptorOverloadFlag = 0.0f;
    this.diseaseData.apeIntelligenceFlag = 0.0f;
    this.diseaseData.recomputePathsFlag = 0.0f;
    this.diseaseData.transcendenceFlag = 0;
    this.accumulatedInfections = 0.0f;
    this.accumulatedCures = 0.0f;
    this.accumulatedZombies = 0.0f;
    this.accumulatedIntelligentApes = 0.0f;
    this.bubblesPopped = 0.0f;
    this.nukeRussia = false;
    this.nukeChina = false;
    this.vday = false;
    this.vdayDone = false;
    this.shadowDay = false;
    this.vampireBonus = 0;
    this.shadowDayDone = false;
    this.shadowDayCounter = 0;
    this.vdayCounter = 0;
    this.shadowDayLength = 0;
    this.vdayLength = 0;
    this.vampLabWorking = 0;
    this.vampireInfectionBoost = 0;
    this.castleHealingMod = 0.0f;
    this.vampireConversionPot = 0L;
    this.vampireConversionPotTrigger = 500000000L;
    this.castleColdClimateResearchMod = 0.0f;
    this.vampireStealthMod = 0;
    this.vampireConversionMod = 1f;
    this.shadowPlagueActive = false;
    this.castleWealthyResearchMod = 0.0f;
    this.massHypnosisCost = 500f;
    this.vampireHypnosisImpact = 50f;
    this.numCastleBubblesWithoutTouch = 0;
    this.castleDnaCounter = 0;
    this.castleReturnSpeed = 0.0f;
    this.castleColdResearch = 0.0f;
    this.castleHotResearch = 0.0f;
    this.castleWealthyResearch = 0.0f;
    this.castleHeatClimateResearchMod = 0.0f;
    this.vcomAlert = 0.0f;
    this.vampireActivity = 0.0f;
    this.castleNumber = 0;
    this.fortDamageBonus = 0.0f;
    this.fortCureBonus = 0.0f;
    this.fortDestroyedNumber = 0;
    this.vampireLabLastspawn = 34;
    this.purityEnabledFlag = 0;
    this.vampLabsDestroyed = 0;
    this.vampLabsCurrent = 0;
    this.maxVampLabs = 0;
    this.numberOfFortsToSpawn = 0.0f;
    this.vampHealSacrificeMod = 0.0f;
    this.globalVampireActivityBonus = 0.0f;
    this.lairDroneAttackTimer = 0;
    this.lairDroneAttackDuration = 8f;
    this.templarDestroyed = 0;
    this.numberOfFortsCreated = 0;
    this.vampireNarrativeStory = 0;
    this.vampHealthIncrease = 0;
    this.vampLabFortDnaBonus = 0;
    this.vampRageCostZero = 0;
    this.vampBloodRageCasulatiesIncreased = 0;
    this.vampBloodRageBonusDna = 0;
    this.vampBatRangeBonus = 0;
    this.vampFlightCostsZero = 0;
    this.vampMoreHealthFasterFlight = 0;
    this.vampFlyFasterLoseHealth = 0;
    this.vampAutomaticBloodRage = 0;
    this.vampHealingBonus = 0;
    this.vampLairDefenseBonus = 0;
    this.vampActivityLairCountryBonus = 0;
    this.vampLairDnaQuicker = 0;
    this.deadBodyTransmission = 0.0f;
    this.componentQty = 0;
    this.themeQty = 0;
    this.mechanicsQty = 0;
    this.playerQty = 0;
    this.isFakeNews = false;
    this.fakeNewsStarted = false;
    this.fakeNewsInformDropPercent = 0.0f;
    this.isCure = false;
    this.intelInfectedFound = false;
    this.vaccineKnowledge = 0.0f;
    this.vaccineKnowledgeMonths = 0.0f;
    this.vaccineKnowledgeMonthsStart = 0.0f;
    this.scoreDeadImpactMulti = 10f;
    this.vaccineDevMonths = 0.0f;
    this.vaccineManMonths = 0.0f;
    this.vaccineReleaseMonths = 0.0f;
    this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_NONE;
    this.developmentSpeed = 1f;
    this.understandingSpeed = 1f;
    this.manufactureSpeed = 1f;
    this.manufactureProgress = 0.0f;
    this.manufactureSet = false;
    this.totalVaccineDuration = 0;
    this.vaccineFailCount = 0;
    this.skipDevSuccess = false;
    this.skipDevFired = false;
    this.barPulseCounter = 6;
    this.globalMedicalCapacityMultiplier = 0.0f;
    this.globalInfectMod = 1f;
    this.globalInfectModMAX = 1f;
    this.globalLethalityMod = 1f;
    this.medicalCapacityEffectivenessMulti = 0.0f;
    this.economyDefenseEffectivenessMulti = 0.0f;
    this.contactTracingEffectiveness = 1f;
    this.contactTracingEffectivenessMod = 1f;
    this.contactTracingEffectivenessMult = 1f;
    this.lockdownEffectiveness = 1f;
    this.lockdownEffectivenessMod = 1f;
    this.lockdownEffectivenessMult = 1f;
    this.globalLocalPriorityMultiplier = 0.0f;
    this.globalPriorityAlertModifier = 1f;
    this.highestLocalPriority = 0.0f;
    this.estimatedDeathRate = 0.0f;
    this.connectedLocalPriorityMultiplier = 0.0f;
    this.influencePoints = 0.0f;
    this.globalBaseInfluence = 0.0f;
    this.quarantineInfluence = 0.0f;
    this.alertLevel = 0;
    this.teamHighestInfectedPerc = 0.0f;
    this.hqCountry = (Country) null;
    this.globalUnrestMod = 1f;
    this.reproductionVisual = 1f;
    this.mortalityVisual = 1f;
    this.unrestVisual = 0.0f;
    this.medicalAidDuration = 60f;
    this.investigatorsDuration = 60f;
    this.tempLockdownDuration = 30f;
    this.easyIntel = false;
    this.landTravelRestriction = 0.0f;
    this.airTravelRestriction = 0.0f;
    this.oceanTravelRestriction = 0.0f;
    this.globalInfectedPercMAX = 0.0f;
    this.infectedCountriesMAX = 0;
    this.teamTravelTarget = (Country) null;
    this.turnsSinceKnowledge = 0;
    this.turnsSinceDeadBubble = 0;
    this.numTotalDeadBubbles = 0;
    this.deadBubbleChance = 0.0f;
    this.numTotalDeadBubblesDNA = 0;
    this.quarantinesActiveCount = 0;
    this.supportsActiveCount = 0;
    this.globalAvgCompliance = 1f;
    this.globalUnrestCount = 0.0f;
    this.lockdownTimerMAX = 0;
    this.supportTimerMAX = 0;
    this.spreadCountries = 0;
    this.isNexusContinentBorder = false;
    this.economyTimeMulti = 1f;
    this.startingAuthority = 0.0f;
    this.baseLethality = 0.0f;
    this.baseInfectivity = 0.0f;
    this.authority = 0.0f;
    this.authorityDeduction = 0.0f;
    this.authorityMod = 0.0f;
    this.authorityModHighest = 0.0f;
    this.authLossInfected = 0.0f;
    this.authLossInfectedDelay = 0.0f;
    this.authLossInfectedActual = 0.0f;
    this.authLossInfectedPermanencePerc = 0.0f;
    this.authLossInfectedYesterday = 0.0f;
    this.authLossInfectedHighest = 0.0f;
    this.authLossInfMulti = 1f;
    this.authLossDead = 0.0f;
    this.authLossDeadDelay = 0.0f;
    this.authLossDeadHighest = 0.0f;
    this.authLossDeadActual = 0.0f;
    this.authLossDeadPermanencePerc = 0.0f;
    this.authLossDeadYesterday = 0.0f;
    this.authLossDeadMulti = 1f;
    this.authLossSpread = 0.0f;
    this.authLossSpreadYesterday = 0.0f;
    this.authLossComplianceMulti = 1f;
    this.authLossCompliance = 0.0f;
    this.authCensorship = 0.0f;
    this.authCensorshipTimer = -10f;
    this.authSpreadMod = 0.0f;
    this.authFreezeCount = 0;
    this.authWinMin = 0.01f;
    this.authLossDaysToTrack = 0;
    this.complianceReturnPerc = 0.2f;
    this.globalBorderClosedNum = 0;
    this.globalAirportClosedNum = 0;
    this.globalPortClosedNum = 0;
    this.globalBorderClosedPerc = 0.0f;
    this.globalAirportClosedPerc = 0.0f;
    this.globalPortClosedPerc = 0.0f;
    this.diseaseDNAComplexity = 1f;
    this.globalVaccineSpeed = 1f;
    this.bonusLethality = 0.0f;
    this.manufactureSpeedAuthBonus = 0.0f;
    this.narrativeEventCounter = 0.0f;
    this.narrativeEventGap = 0.0f;
    this.prionIncubationMonths = 0;
    this.prionVaccineSpeedMulti = 1f;
    this.prionInfectedThresholdMulti = 1f;
    this.preSimMAX = 0;
    this.nanovirusPauseTimer = 0;
    this.nanovirusEndTimer = 0;
    this.nanovirusHeatWeak = false;
    this.nanovirusColdWeak = false;
    this.isNanovirus = false;
    this.fungusBloom = 0.0f;
    this.fungusSporeMulti = 1f;
    this.fungusSporesReleased = 0;
    this.localPopUpper = 5E-06f;
    this.localPopLower = 3E-06f;
    this.localPopThreshold = 0.0f;
    this.nationalPopUpper = 0.00011f;
    this.nationalPopLower = 8E-05f;
    this.nationalPopThreshold = 0.0f;
    this.reproductionBarScale = 0.8f;
    this.mortalityBarScale = 15f;
    this.diseaseLengthMulti = 1;
  }

  public override void Initialise()
  {
    this.isCure = this.diseaseType == Disease.EDiseaseType.CURE;
    this.cureScenario = Disease.ECureScenario.None;
    if (this.isCure)
    {
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        this.cureScenario = Disease.ECureScenario.Cure_Standard;
      }
      else
      {
        this.cureScenario = CGameManager.GetCureScenarioType(this.scenario);
        if (CGameManager.IsFederalScenario("PIFCURE"))
        {
          if (CGameManager.IsFederalScenario("bacteria"))
            this.cureScenario = Disease.ECureScenario.Cure_Standard;
          if (CGameManager.IsFederalScenario("virus"))
            this.cureScenario = Disease.ECureScenario.Cure_Virus;
          if (CGameManager.IsFederalScenario("prion"))
            this.cureScenario = Disease.ECureScenario.Cure_Prion;
          if (CGameManager.IsFederalScenario("parasite"))
            this.cureScenario = Disease.ECureScenario.Cure_Parasite;
          if (CGameManager.IsFederalScenario("nanovirus"))
            this.cureScenario = Disease.ECureScenario.Cure_Nanovirus;
          if (CGameManager.IsFederalScenario("fungus"))
            this.cureScenario = Disease.ECureScenario.Cure_Fungus;
          if (CGameManager.IsFederalScenario("bioweapon"))
            this.cureScenario = Disease.ECureScenario.Cure_Bioweapon;
        }
      }
      if (this.cureScenario == Disease.ECureScenario.Cure_Prion)
      {
        this.isPrion = true;
        this.curveDie = new float[21]
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
          0.0f,
          0.0f,
          0.0f,
          0.0f,
          0.0f,
          1f / 1000f,
          0.01f,
          0.9f,
          0.01f
        };
        this.curveHeal = new float[21]
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
          0.0f,
          0.0f,
          0.0f,
          0.0f,
          0.0f,
          0.0001f,
          1f / 1000f,
          0.005f,
          0.05f
        };
        this.curveInfect = new float[21]
        {
          0.4f,
          0.5f,
          0.65f,
          0.81f,
          1f,
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
          0.0f,
          0.0f,
          0.0f,
          0.0f
        };
      }
      if (this.cureScenario == Disease.ECureScenario.Cure_Fungus)
      {
        this.isFungus = true;
        this.curveDie = new float[21]
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
        this.curveHeal = new float[21]
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
        this.curveInfect = new float[21]
        {
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
          1f,
          1f,
          1f
        };
      }
      if (this.cureScenario == Disease.ECureScenario.Cure_Nanovirus)
        this.isNanovirus = true;
      Debug.Log((object) ("CURE SCENARIO: " + (object) this.cureScenario + " scenario: " + this.scenario));
    }
    SPDiseaseExternal.DiseaseInitialise(this.diseaseData);
    if (this.scenario == "christmas_spirit" || this.scenario == "board_game")
      this.wormPlaneChance = 0.0f;
    if (this.HasCheat(Disease.ECheatType.UNLIMITED))
      this.evoPoints = 9999;
    if (this.HasCheat(Disease.ECheatType.IMMUNE))
      this.diseaseData.researchInefficiencyMultiplier += 2f;
    if (this.HasCheat(Disease.ECheatType.SHUFFLE))
    {
      DataImporter.ShuffleTech((Disease) this);
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatShuffle);
    }
    if (this.HasCheat(Disease.ECheatType.LUCKY_DIP))
    {
      DataImporter.ApplyLuckyDip((Disease) this);
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatLuckyDip);
    }
    if (this.HasCheat(Disease.ECheatType.GOLDEN_HANDSHAKE))
      this.evoPoints = 9000000;
    if (this.HasCheat(Disease.ECheatType.ADVANCE_PLANNING))
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatAdvancePlanning);
    if (this.HasCheat(Disease.ECheatType.FULL_SUPPORT))
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatFullSupport);
    if (this.HasCheat(Disease.ECheatType.MAXIMUM_POWER))
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatMaximumPower);
    if (this.HasCheat(Disease.ECheatType.THE_AVENGERS))
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatTheAvengers);
    if (this.HasCheat(Disease.ECheatType.CURE_LUCKY_DIP))
    {
      DataImporter.ApplyLuckyDip((Disease) this);
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatCureLuckyDip);
    }
    if (this.HasCheat(Disease.ECheatType.CURE_SHUFFLE))
    {
      DataImporter.ShuffleTech((Disease) this);
      this.AddFlag(Disease.EGenericDiseaseFlag.CheatCureShuffle);
    }
    TutorialSystem.RegisterInterface((ITutorial) this);
    if (CGameManager.IsTutorialGame)
      this.evoPoints += 15;
    if (!CGameManager.IsFederalScenario("时生虫ReMASTER"))
      return;
    foreach (Country country in World.instance.countries)
    {
      if (country.hasAirport)
        country.airportStatus = false;
      if (country.hasPorts)
        country.portStatus = false;
    }
  }

  public override void GameUpdate()
  {
    CGameManager.CallExternalMethod("DiseaseExternalHead", World.instance, (Disease) this, (Country) null, (LocalDisease) null);
    if (CGameManager.IsFederalScenario("348关 万圣节特供 夜之复调I"))
      this.customGlobalVariable5 = CInterfaceManager.instance.GetCountryView("china").GetCountry().deadPercent;
    if (CGameManager.IsFederalScenario("世界狂潮"))
      this.CureScenarioUpdate();
    if (CGameManager.IsFederalScenario("海盗瘟疫MXM"))
      this.PirateUpdate();
    if (CGameManager.IsFederalScenario("命运之门"))
      this.FateGateUpdate();
    if (CGameManager.IsFederalScenario("时生虫ReMASTER"))
      this.DiseaseClockUpdate();
    if (CGameManager.IsFederalScenario("时生虫ReCRAFTa"))
      this.ClockCraftUpdate();
    if (CGameManager.IsFederalScenario("Spore War"))
      this.SporeWarUpdate();
    if (CGameManager.IsFederalScenario("263关 Cure-Candida auri") && this.turnNumber >= 10)
      this.customGlobalVariable1 = (float) ModelUtils.Max((double) this.cureCompletePerc, (double) this.customGlobalVariable1);
    this.ApplyDreamParasiteTransformation();
    if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") && CGameManager.game.CurrentLoadedScenario.filename.Contains("铁丝线虫") && (this.IsTechEvolved("faedb921-31d9-4fbf-9526-a6abf158dbc9") || this.IsTechEvolved("7a629190-645f-49b4-bf2c-f537fb9e3e94") || this.IsTechEvolved("739e05e0-3bb1-4453-b7cc-a0753744a803")))
      ++this.customGlobalVariable5;
    if (!this.zombieScenarioChecked)
      this.CheckZombieScenarioRequirement();
    if (!this.parasiteScenarioChecked && (double) this.customGlobalVariable5 <= 0.5)
      this.CheckParasiteScenarioRequirement();
    if (!this.dreamScenarioChecked)
      this.CheckDreamScenarioRequirement();
    if (!this.hellScenarioChecked)
      this.CheckHellScenarioRequirement();
    if (!this.fateScenarioChecked)
      this.CheckFateScenarioRequirement();
    if (!this.finalScenarioChecked)
      this.CheckFinalScenarioRequirement();
    if (!this.scenarioEverChecked)
      this.CheckEverScenarioRequirement();
    if (this.zombieScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 2)
    {
      ParameterisedString title = new ParameterisedString("Z O M B I E   A T T A C K");
      ParameterisedString message = new ParameterisedString("P$ eo&Ple lE*f@t w!i#Ll f;Ace a&N unC?ert+Ain fU-tu/Re Fi~gH=ti]Ng to$War^Ds zo*Mb^I$e#s...");
      CSoundManager.instance.PlaySFX("benign_mimic");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    if (this.parasiteScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 2)
    {
      ParameterisedString title = new ParameterisedString("ANCIENT SPIRIT OF CLEMATIS REVEALED");
      ParameterisedString message = new ParameterisedString("What the little parasite has done revealed the ancient spirit of clematis, causing unstoppable waves towards a parallel universe, CAN YOU DESTROY another universe with worse living circumstantces?");
      CSoundManager.instance.PlaySFX("benign_mimic");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    if (this.dreamScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 2)
    {
      ParameterisedString title = new ParameterisedString("魔法反噬");
      ParameterisedString message = new ParameterisedString("当滥用魔法，必然遭到现实的惩罚，现在也是如此，当你站在人类的对立面之时，无论是多么酣畅淋漓的美梦，他们总有接受现实的那一天。");
      CSoundManager.instance.PlaySFX("benign_mimic");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    if (this.hellScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 4)
      this.PerformAbnormalScenario("Level6666 炼狱（双盘吸虫 极恶）a");
    if (this.scenarioEverChecked && this.turnNumber == this.abnormalCheckedDay + 4)
      this.PerformAbnormalScenario("Level8848 无垠雪峰");
    if (this.zombieScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 5)
      this.PerformAbnormalScenario("L4D");
    if (this.parasiteScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 5 && (double) this.customGlobalVariable5 <= 0.5)
      this.PerformAbnormalScenario("176关 铁线虫入侵（极恶）b");
    if (this.fateScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 5)
      this.PerformAbnormalScenario("命运之门");
    if (this.dreamScenarioChecked && this.turnNumber == this.abnormalCheckedDay + 5)
    {
      if (!this.CheckScenarioExist("DreamParasiteBYD"))
      {
        Debug.Log((object) "Dream Parasite BYD not found, download it!");
        this.PerformAbnormalScenario("DreamParasiteBYD", "test");
      }
      else
      {
        Debug.Log((object) "Dream Parasite BYD already installed, play it!");
        CGameManager.pendingScenarioName = "DreamParasiteBYD";
        IGame.PlayScenarioVideo("test");
      }
    }
    if (this.finalScenarioChecked)
    {
      CSoundManager.instance.PlaySFX("pause_count");
      if (this.turnNumber == this.abnormalCheckedDay + 5)
      {
        if (!this.CheckScenarioExist("时生虫ReMASTER"))
        {
          Debug.Log((object) "时生虫ReMASTER not found, download it!");
          this.PerformAbnormalScenario("时生虫ReMASTER", "clock");
        }
        else
        {
          Debug.Log((object) "时生虫ReMASTER already installed, play it!");
          CGameManager.pendingScenarioName = "时生虫ReMASTER";
          IGame.PlayScenarioVideo("clock");
        }
      }
    }
    if ((double) this.cureRequirementBase <= (double) this.yesterdayCureBaseMultiplier - 2.0 && !CGameManager.IsFederalScenario("PISMG") && !CGameManager.IsFederalScenario("时生虫"))
    {
      ParameterisedString title = new ParameterisedString("NC Stole your cure requirement!");
      ParameterisedString message = new ParameterisedString("Your cure requirement has been stolen by NC and you'd better load your last save then");
      CSoundManager.instance.PlaySFX("benign_mimic");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    this.yesterdayCureBaseMultiplier = this.cureRequirementBase;
    if (this.diseaseType == Disease.EDiseaseType.VAMPIRE)
      this.GameUpdate_Vampire();
    else if (this.diseaseType == Disease.EDiseaseType.FAKE_NEWS)
      this.GameUpdate_FakeNews();
    else if (this.diseaseType == Disease.EDiseaseType.CURE)
    {
      this.GameUpdate_Cure();
    }
    else
    {
      this.diseaseData.mutatedThisTurn = false;
      World instance = World.instance;
      this.diseaseData.apeTotalPopulation = instance.apeTotalPopulation;
      if (this.diseaseData.evoPointsPrevTurn == this.evoPoints)
        ++this.diseaseData.numTurnsWithoutEvoChange;
      this.diseaseData.evoPointsPrevTurn = this.evoPoints;
      if (this.diseaseData.numTurnsWithoutEvoChange >= 3)
      {
        this.diseaseData.numTurnsWithoutEvoChange = 0;
        ++this.diseaseData.evoBoost;
      }
      if (this.infectedCountries.Count > 0 && instance.DiseaseTurn > 10 && this.diseaseData.numTurnsWithoutEvoChange > 0 && !this.diseaseData.dnaBubbleShowing)
      {
        bool flag = false;
        if (this.diseaseData.diseaseType == Disease.EDiseaseType.PARASITE && this.diseaseData.dnaPointsGained < 200)
          flag = (double) ModelUtils.IntRand(1, 150) < (double) (10 + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
        else if (this.diseaseType == Disease.EDiseaseType.NEURAX)
        {
          if (instance.DiseaseTurn - this.diseaseData.wormBubbleLastDay > 25)
            flag = (double) ModelUtils.IntRand(1, 100) < (double) (10 - this.diseaseData.numDNABubbles + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
        }
        else
          flag = (double) ModelUtils.IntRand(1, 100) < (double) (10 - this.diseaseData.numDNABubbles + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
        if (flag && !CGameManager.IsFederalScenario("PISMG"))
        {
          ++this.diseaseData.numDNABubbles;
          ++this.diseaseData.numDNABubblesWithoutTouch;
          this.diseaseData.dnaBubbleShowing = true;
          instance.AddBonusIcon(new BonusIcon((Disease) this, this.infectedCountries[UnityEngine.Random.Range(0, this.infectedCountries.Count)], BonusIcon.EBonusIconType.DNA));
          if (CGameManager.IsTutorialGame && TutorialSystem.IsModuleComplete("4B"))
            TutorialSystem.CheckModule((Func<bool>) (() => true), "5A");
        }
      }
      if (this.diseaseType == Disease.EDiseaseType.NEURAX)
        this.CheckNeuraxBubble();
      SPDiseaseExternal.DiseaseUpdate(this.diseaseData);
      if (this.diseaseData.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        if (instance.DiseaseTurn < 40 && this.diseaseData.totalInfected < 25L && (double) ModelUtils.IntRand(0, 100) < (double) Mathf.Min(10f, Mathf.Max(2f, this.diseaseData.nexusMinInfect + 2f)))
          this.nexus.TransferPopulation(1.0, Country.EPopulationType.HEALTHY_SUSCEPTIBLE, (Disease) this, Country.EPopulationType.INFECTED);
      }
      else if (instance.DiseaseTurn < 40 && this.diseaseData.totalInfected < 25L && (double) ModelUtils.IntRand(0, 100) < (double) Mathf.Min(10f, Mathf.Max(2f, this.diseaseData.nexusMinInfect + 2f)))
        this.nexus.TransferPopulation(1.0, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.INFECTED);
      long totalZombie = this.diseaseData.totalZombie;
      SPDiseaseExternal.DiseasePrepareTotals(this.diseaseData);
      this.infectedCountries = new List<Country>();
      this.apeLabCountries = new List<Country>();
      this.apeColonyCountries = new List<Country>();
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        LocalDisease localDisease = this.GetLocalDisease(country);
        SPDiseaseExternal.DiseaseCountryUpdate(this.diseaseData, ((SPCountry) country).mData, ((SPLocalDisease) localDisease).mData);
        if (localDisease.allInfected > 0L)
          this.infectedCountries.Add(country);
        if (country.hasApeLab)
          this.apeLabCountries.Add(country);
        if (country.hasApeColony)
          this.apeColonyCountries.Add(country);
      }
      this.diseaseData.infectedCountryCount = this.infectedCountries.Count;
      this.diseaseData.uninfectedCountryCount = World.instance.countries.Count - this.diseaseData.infectedCountryCount;
      this.diseaseData.apeTotalLabs = this.apeLabCountries.Count + World.instance.trackedLabPlanes.Count;
      this.diseaseData.apeTotalColonies = this.apeColonyCountries.Count;
      this.diseaseData.totalZombie += this.diseaseData.apeHordeStash;
      if (totalZombie >= this.diseaseData.totalZombie)
        ++this.diseaseData.zombieDecreaseTurnCount;
      else
        this.diseaseData.zombieDecreaseTurnCount = 0;
      SPDiseaseExternal.DiseasePostCountryUpdate(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
      this.diseaseData.hiZombifiedPopulation = 0.0f;
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        LocalDisease localDisease = this.GetLocalDisease(country);
        localDisease.GameUpdate();
        World.instance.CheckGovernmentActions(country);
        this.diseaseData.hiZombifiedPopulation += localDisease.H2Z + localDisease.I2Z + localDisease.D2Z;
      }
      SPDiseaseExternal.DiseasePostLocalUpdate(this.diseaseData, World.instance.totalPopulation);
      if (this.scenario == "board_game" && ModelUtils.IntRand(0, 4) < 1)
        ++this.evoPoints;
      if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
        this.SimianFluUpdate();
      else if (this.diseaseType == Disease.EDiseaseType.NECROA)
        this.NecroaUpdate();
      long num1 = this.diseaseData.infectedThisTurn;
      if (num1 < 0L)
        num1 = 0L;
      this.diseaseData.averageInfected = (this.diseaseData.averageInfected * (double) (this.diseaseData.turnNumber - 1) + (double) num1) / (double) this.diseaseData.turnNumber;
      long num2 = this.diseaseData.deadThisTurn;
      if (num2 < 0L)
        num2 = 0L;
      this.diseaseData.averageDead = (this.diseaseData.averageDead * (double) (this.diseaseData.turnNumber - 1) + (double) num2) / (double) this.diseaseData.turnNumber;
      if (this == CNetworkManager.network.LocalPlayerInfo.disease)
      {
        if (this.infectedThisTurn >= 0L)
          this.accumulatedInfections += (float) this.infectedThisTurn;
        else if (this.cureFlag)
          this.accumulatedCures += (float) -(double) this.infectedThisTurn;
        if (this.zombiesThisTurn > 0L && this.diseaseType == Disease.EDiseaseType.NECROA)
          this.accumulatedZombies += (float) this.zombiesThisTurn;
        if (this.infectedApesThisTurn > 0L && this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
          this.accumulatedIntelligentApes += (float) this.infectedApesThisTurn;
      }
      if (this.nukeRussia)
      {
        CountryView countryView = CInterfaceManager.instance.GetCountryView("russia");
        countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
        this.nukeRussia = false;
      }
      if (this.nukeChina)
      {
        CountryView countryView = CInterfaceManager.instance.GetCountryView("china");
        countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
        this.nukeChina = false;
      }
      CGameManager.CallExternalMethod("DiseaseExternalTail", World.instance, (Disease) this, (Country) null, (LocalDisease) null);
    }
  }

  private void CheckNeuraxBubble()
  {
    int num = World.instance.DiseaseTurn - this.wormBubbleLastDay;
    if ((double) ModelUtils.FloatRand(0.0f, 11f) >= (double) this.wormPlaneChance * (1.1000000238418579 - (double) this.globalDeadPercent) * (double) (num / 25) || (double) this.globalDeadPercent >= 0.949999988079071 || num <= 8 || this.cureFlag)
      return;
    List<Country> countryList = new List<Country>();
    for (int index = 0; index < this.infectedCountries.Count; ++index)
    {
      LocalDisease localDisease = this.GetLocalDisease(this.infectedCountries[index]);
      if ((double) localDisease.infectedPercent > 0.033700000494718552 && (double) localDisease.deadPercent < 0.74830001592636108)
        countryList.Add(this.infectedCountries[index]);
    }
    if (countryList.Count <= 0)
      return;
    ++this.numWormBubblesWithoutTouch;
    this.wormBubbleLastDay = World.instance.DiseaseTurn;
    World.instance.AddNeuraxBubble(countryList[ModelUtils.IntRand(0, countryList.Count - 1)], (Disease) this);
  }

  private void NecroaUpdate()
  {
    if (this.diseaseData.zday && (double) this.diseaseData.globalZombiePercent > 0.0)
    {
      this.diseaseData.zdayDead *= 1.25f;
      this.diseaseData.zdayInfected += 0.005f;
      if (++this.diseaseData.zdayCounter > this.diseaseData.zdayLength)
      {
        this.diseaseData.zday = false;
        this.diseaseData.zdayDone = true;
        this.diseaseData.fortSelectionDay = this.diseaseData.zdayCounter + ModelUtils.IntRand(10, 30);
      }
    }
    else if (this.diseaseData.zdayDone)
    {
      if ((double) this.diseaseData.globalHealthyPercent > (double) this.diseaseData.globalInfectedPercent + (double) this.diseaseData.globalDeadPercent + (double) this.diseaseData.globalZombiePercent && this.evoPoints < 25 && this.diseaseData.zombieDecreaseTurnCount > 25)
        this.diseaseData.globalZombieDecayMultiplier += this.diseaseData.globalZombieDecayMultiplier;
      else if ((double) this.diseaseData.hiZombifiedPopulation < 1.0 && this.evoPoints < 11 && this.diseaseData.zdayCounter > 100 && (double) this.diseaseData.globalInfectedPercent < 1.0 / 1000.0)
        this.diseaseData.globalZombieDecayMultiplier += (float) (0.004999999888241291 + 0.10000000149011612 * (double) this.diseaseData.globalZombieDecayMultiplier);
      else
        this.diseaseData.globalZombieDecayMultiplier = 1f;
      if (ModelUtils.IntRand(0, (int) (14.0 + (double) this.diseaseData.globalDecayChance)) < 1)
        this.diseaseData.zombieDecayTechMultiplier += 0.01f;
      if (this.diseaseData.zdayCounter++ == this.diseaseData.fortSelectionDay)
      {
        float num = 0.0f;
        List<Country> countryList = new List<Country>();
        for (int index = 0; index < World.instance.countries.Count; ++index)
        {
          Country country = World.instance.countries[index];
          if (!country.HasFort() && !country.poverty && (double) country.healthyPercent >= (double) num && (double) country.healthyPercent + (double) country.infectedPercent > 0.25)
          {
            if ((double) country.healthyPercent > (double) num)
            {
              num = country.healthyPercent;
              countryList.Clear();
            }
            countryList.Add(country);
          }
        }
        Country country1 = countryList[ModelUtils.IntRand(0, countryList.Count - 1)];
        country1.fortState = EFortState.FORT_ALIVE;
        Debug.LogWarning((object) ("ZCOM SELECTED: " + country1.id));
        this.diseaseData.firstFortSelected = true;
        World.instance.AddFort(country1);
        this.diseaseData.daysUntilMinifortPlaneSpawn = ModelUtils.IntRand(12, 30);
      }
    }
    this.diseaseData.globalBattleVictoryCount = 0.0f;
    this.diseaseData.numAliveForts = 0;
    int count = World.instance.fortCountries.Count;
    if (count <= 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      Country fortCountry = World.instance.fortCountries[index];
      if (fortCountry.fortState == EFortState.FORT_ALIVE && fortCountry.healthyPopulation + fortCountry.totalInfected < (long) ModelUtils.IntRand(200, 7000) && fortCountry.totalZombie > 100L)
      {
        fortCountry.fortState = EFortState.FORT_DESTROYED;
        fortCountry.fortWasDestroyed = true;
      }
      if (fortCountry.fortState == EFortState.FORT_ALIVE)
      {
        this.diseaseData.globalBattleVictoryCount += fortCountry.battleVictoryCount;
        ++this.diseaseData.numAliveForts;
      }
    }
    World.instance.UpdateDestroyedForts();
    this.diseaseData.globalBattleVictoryCount /= (float) count * this.diseaseData.fortDifficultyModifier;
    if ((double) this.diseaseData.globalBattleVictoryCount >= 1.0)
    {
      List<Country> countryList = new List<Country>();
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        if (country.fortState == EFortState.FORT_NONE && (double) country.healthyPercent > 0.10000000149011612)
        {
          countryList.Add(country);
          countryList.Add(country);
          if (country.hot)
            countryList.Add(country);
          if (country.wealthy)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
          if ((double) country.healthyPercent > 0.60000002384185791)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
          if (country.totalZombie < 1L)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
        }
      }
      if (countryList.Count > 0)
      {
        Country d = countryList[ModelUtils.IntRand(0, countryList.Count - 1)];
        float num = 0.0f;
        Country s = (Country) null;
        for (int index = 0; index < World.instance.fortCountries.Count; ++index)
        {
          Country fortCountry = World.instance.fortCountries[index];
          if (fortCountry.fortState == EFortState.FORT_ALIVE && (double) fortCountry.battleVictoryCount > (double) num)
          {
            num = fortCountry.battleVictoryCount;
            s = fortCountry;
          }
        }
        Debug.LogWarning((object) ("ZCOM NEW FORT: " + d.id + " FROM: " + (object) s));
        if (s == null)
        {
          Debug.Log((object) "No source fort country available.");
        }
        else
        {
          bool flag = false;
          if (d.totalInfected > 300L && (double) (d.healthyPopulation / d.totalInfected) < 0.40000000596046448)
            flag = ModelUtils.IntRand(0, 100) < 10;
          else if (d.healthyPopulation < 1L)
            flag = true;
          int number = ModelUtils.IntRand(10, 250);
          Vehicle vehicle = Vehicle.Create();
          vehicle.type = Vehicle.EVehicleType.Airplane;
          vehicle.subType = Vehicle.EVehicleSubType.Fort;
          if (flag)
            vehicle.AddInfected((Disease) this, number);
          vehicle.SetRoute(s, d);
          vehicle.actingDisease = (Disease) this;
          s.fortPlaneSpawned = true;
          World.instance.AddVehicle(vehicle);
          for (int index = 0; index < World.instance.countries.Count; ++index)
            World.instance.countries[index].battleVictoryCount = 0.0f;
        }
      }
    }
    if (this.diseaseData.daysUntilMinifortPlaneSpawn-- != 0)
      return;
    List<Country> countryList1 = new List<Country>();
    for (int index = 0; index < World.instance.fortCountries.Count; ++index)
    {
      Country fortCountry = World.instance.fortCountries[index];
      if (fortCountry.fortState == EFortState.FORT_ALIVE)
        countryList1.Add(fortCountry);
    }
    if (countryList1.Count < 2)
    {
      Debug.Log((object) "Not enough forts to spawn a minifort plane");
      this.diseaseData.daysUntilMinifortPlaneSpawn = 10;
    }
    else
    {
      Country s = countryList1[ModelUtils.IntRand(0, countryList1.Count - 1)];
      countryList1.Remove(s);
      Country d = countryList1[ModelUtils.IntRand(0, countryList1.Count - 1)];
      Vehicle vehicle = Vehicle.Create();
      vehicle.type = Vehicle.EVehicleType.Airplane;
      vehicle.subType = Vehicle.EVehicleSubType.MiniFort;
      vehicle.SetRoute(s, d);
      vehicle.actingDisease = (Disease) this;
      World.instance.AddVehicle(vehicle);
    }
  }

  private void SimianFluUpdate()
  {
    ++this.diseaseData.daysSinceGlobalDrone;
    ++this.diseaseData.apeDaysSinceLastColonyBubble;
    this.diseaseData.apeTotalAliveGlobal = this.diseaseData.apeTotalHealthy + this.diseaseData.apeTotalInfected;
    if ((double) this.diseaseData.changeToHumanImmunity > 0.0)
    {
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        country.TransferPopulation((double) country.healthyPopulationImmune * (double) this.diseaseData.changeToHumanImmunity, Country.EPopulationType.HEALTHY_IMMUNE, (Disease) this, Country.EPopulationType.HEALTHY_SUSCEPTIBLE);
      }
    }
    this.diseaseData.changeToHumanImmunity = 0.0f;
    int num1 = this.diseaseData.apeTotalDestroyedLabs + this.diseaseData.apeTotalLabs;
    ++this.diseaseData.labDayCounter;
    if (this.diseaseData.genSysWorking > 1 && this.diseaseData.apeTotalLabs <= this.diseaseData.apeMaxLabs)
      this.diseaseData.labCounter += (int) ((double) this.diseaseData.globalPriority / 5.0 * (1.2000000476837158 - (double) this.diseaseData.globalDeadPercent) * (double) Mathf.Max(0.1f, this.diseaseData.publicOrderAverage * 1.5f) * (2.2000000476837158 - 2.0 * ((double) this.diseaseData.apeTotalLabs / ((double) this.diseaseData.apeMaxLabs + 1.0))));
    if (this.diseaseData.labCounter <= this.diseaseData.labSpawnThreshold || (double) this.diseaseData.labDayCounter <= (double) (29 + ModelUtils.IntRand(0, 5)) - (double) this.diseaseData.difficultyVariable)
      return;
    Country suitableApeLabCountry = this.GetSuitableApeLabCountry();
    if (suitableApeLabCountry == null)
      return;
    suitableApeLabCountry.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
    int num2 = num1 + 1;
    this.diseaseData.labSpawnThreshold = (int) Mathf.Max((float) (70.0 - (double) this.diseaseData.difficultyVariable * 3.0), 15f * ModelUtils.FloatRand(0.7f, 1.2f) * (float) num2 * (float) num2);
    this.diseaseData.labCounter = 0;
    this.diseaseData.labDayCounter = 0;
  }

  public override void OnBonusIconClicked(BonusIcon bonusIcon)
  {
    if (CGameManager.IsFederalScenario("时生虫ReMASTER") && this.IsTechEvolved("000cb608-e5a9-4493-a4d5-5ee341326198") && (double) this.customGlobalVariable2 >= 0.10000000149011612)
      this.cureBaseMultiplier += ModelUtils.FloatRand(0.047f, 0.053f);
    if (bonusIcon.extraEvo > 0)
    {
      this.evoPoints += bonusIcon.extraEvo;
      this.dnaPointsGained += bonusIcon.extraEvo;
    }
    if ((CGameManager.IsFederalScenario("时生虫ReMASTER") || CGameManager.IsFederalScenario("时生虫ReCRAFT")) && bonusIcon.type == BonusIcon.EBonusIconType.CURE)
      this.customGlobalVariable1 -= (float) ModelUtils.IntRand(1, 3);
    if (bonusIcon.musicBubble)
    {
      this.OnMusicBubbleClick(1f, bonusIcon.musicImportance, true);
    }
    else
    {
      if (bonusIcon.forceEvo)
        return;
      if (this.isCure)
      {
        Country country = bonusIcon.country;
        LocalDisease localDisease = country.GetLocalDisease((Disease) this);
        switch (bonusIcon.type)
        {
          case BonusIcon.EBonusIconType.CURE:
            localDisease.labCureBubbleVisible = false;
            if (!CGameManager.game.IsReplayActive && this.vaccineStage < Disease.EVaccineProgressStage.VACCINE_RELEASE)
            {
              float num = this.cureRequirements * (0.03f + ModelUtils.FloatRand(0.0f, 0.02f));
              this.globalCureResearch += num;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.VACCINE_RESEARCH, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, num.ToString());
              break;
            }
            break;
          case BonusIcon.EBonusIconType.INFECT:
            if (!CGameManager.game.IsReplayActive && this.vaccineStage == Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE)
            {
              float num = ModelUtils.FloatRand(0.02f, 0.04f);
              this.vaccineKnowledge += num;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.VACCINE_KNOWLEDGE, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, num.ToString());
            }
            float f = (float) (1.0 + 58.0 * (double) this.globalBaseInfluence * (double) localDisease.actualBaseInfluence);
            float num1;
            if ((double) f < 2.0)
            {
              float num2 = Mathf.Round(f);
              num1 = (double) f <= (double) num2 ? num2 : num2 + 1f;
            }
            else
            {
              float num3 = Mathf.Round(f);
              num1 = (double) num3 <= (double) f ? num3 : num3 - 1f;
            }
            this.evoPoints += (int) num1;
            break;
          case BonusIcon.EBonusIconType.DEATH:
            if (!CGameManager.game.IsReplayActive)
            {
              int id = Mathf.Max(1, ModelUtils.IntRand(1, Mathf.RoundToInt(3f * country.publicOrder)));
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
              break;
            }
            break;
          case BonusIcon.EBonusIconType.MEDICAL_SYSTEMS_OVERWHELMED:
            if (!CGameManager.game.IsReplayActive)
            {
              int id = Mathf.Max(1, ModelUtils.IntRand(1, Mathf.RoundToInt(2f * country.publicOrder)));
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
              break;
            }
            break;
          case BonusIcon.EBonusIconType.DISEASE_ORIGIN_COUNTRY:
            if (!CGameManager.game.IsReplayActive)
            {
              int id = ModelUtils.IntRand(4, 8);
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
            }
            if (this.vaccineStage == Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE)
            {
              this.vaccineKnowledge += 0.15f;
              this.understandingSpeed += 0.3f;
              break;
            }
            break;
          case BonusIcon.EBonusIconType.DEADBUBBLE_FOR_CURE:
            if (!CGameManager.game.IsReplayActive)
            {
              int id = ModelUtils.IntRand(3, 4) + ((double) ModelUtils.FloatRand(0.0f, 1f) < (double) this.authority ? 1 : 0);
              this.numTotalDeadBubblesDNA += id;
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
              break;
            }
            break;
          default:
            Debug.LogError((object) ("Not sure what to do with bonus icon of type " + (object) bonusIcon.type + " in CURE mode, in country: " + (object) country));
            break;
        }
        if (bonusIcon.type == BonusIcon.EBonusIconType.CURE)
          return;
        localDisease.mColoredBubble = Country.EGenericCountryFlag.None;
      }
      else
      {
        switch (bonusIcon.type)
        {
          case BonusIcon.EBonusIconType.DNA:
            this.dnaBubbleShowing = false;
            if (!CGameManager.game.IsReplayActive)
            {
              int a = ModelUtils.IntRand(1, 3);
              if (this.diseaseType == Disease.EDiseaseType.PARASITE)
                a *= 2;
              if (this.orangeBubbleMult > 0 && ModelUtils.IntRand(0, 7) < this.orangeBubbleMult)
                ++a;
              else if (this.orangeBubbleMult < 0 && ModelUtils.IntRand(0, 7) < this.orangeBubbleMult * -1)
                --a;
              int id = Mathf.Max(a, 1);
              if (CGameManager.IsFederalScenario("时生虫ReMASTER") || CGameManager.IsFederalScenario("时生虫ReCRAFT"))
                id = ModelUtils.IntRand(2, 10);
              this.evoPoints += id;
              this.dnaPointsGained += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.DNA_POINTS_GAINED, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
            }
            this.evoBoost = 0;
            this.numDNABubblesWithoutTouch = 0;
            this.GetLocalDisease(bonusIcon.country).mColoredBubble = Country.EGenericCountryFlag.None;
            break;
          case BonusIcon.EBonusIconType.CURE:
            this.numCureBubblesWithoutTouch = 0;
            if (CGameManager.game.IsReplayActive)
              break;
            if (this.blueBubbleBonusDNA)
            {
              int id = ModelUtils.IntRand(2, 5);
              if (CGameManager.IsFederalScenario("时生虫ReMASTER") || CGameManager.IsFederalScenario("时生虫ReCRAFT"))
                id = ModelUtils.IntRand(1, 2);
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
            }
            World.instance.AddAchievement(EAchievement.A_BottleSmasher);
            break;
          case BonusIcon.EBonusIconType.INFECT:
            if (!CGameManager.game.IsReplayActive)
            {
              float a1 = 1f;
              float b = Mathf.Round(ModelUtils.FloatRand(0.4f, 1f) * (((double) this.globalSeverity > 0.0 ? Mathf.Log(this.globalSeverity) : 0.0f) * a1) + a1);
              float a2 = Mathf.Max(a1, b);
              if (this.redBubbleMult > 0 && ModelUtils.IntRand(0, 12) < this.redBubbleMult)
                ++a2;
              else if (this.redBubbleMult < 0 && ModelUtils.IntRand(0, 11) < -this.redBubbleMult)
                --a2;
              int id = (int) Mathf.Max(a2, 1f);
              this.evoPoints += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
            }
            this.numInfectBubblesWithoutTouch = 0;
            break;
          case BonusIcon.EBonusIconType.DEATH:
            if (!CGameManager.game.IsReplayActive)
            {
              int id = ModelUtils.IntRand(1, 3);
              this.evoPoints += id;
              this.dnaPointsGained += id;
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.EVO_POINTS, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.DNA_POINTS_GAINED, World.instance.DiseaseTurn, World.instance.eventTurn, (Disease) this, id);
            }
            this.GetLocalDisease(bonusIcon.country).mColoredBubble = Country.EGenericCountryFlag.None;
            break;
          case BonusIcon.EBonusIconType.NEURAX:
            this.wormBubbleHiddenStatus += 2;
            this.numWormBubblesWithoutTouch = 0;
            break;
          case BonusIcon.EBonusIconType.APE_COLONY:
            this.GetLocalDisease(bonusIcon.country).ClickApeColonyBubble(bonusIcon);
            break;
          case BonusIcon.EBonusIconType.CASTLE:
            if (!CGameManager.game.IsReplayActive)
              this.GetLocalDisease(bonusIcon.country).ClickCastleBubble(bonusIcon);
            --this.numCastleBubblesWithoutTouch;
            break;
          default:
            Debug.Log((object) ("BONUS ICON CLICKED BUT NO IDEA WHAT TO DO: " + (object) bonusIcon.type));
            break;
        }
      }
    }
  }

  public override LocalDisease CreateLocalDisease(Country country)
  {
    LocalDisease ld = (LocalDisease) new SPLocalDisease();
    ld.disease = (Disease) this;
    ld.country = country;
    ld.diseaseID = this.id;
    ld.countryNumber = country.countryNumber;
    country.AddLocalDisease(ld);
    this.localDiseases.Add(ld);
    this.countryData[country] = ld;
    if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      country.healthyPopulationImmune = (long) ((double) country.originalPopulation * (double) this.diseaseData.populationImmunity * (double) ModelUtils.FloatRand(0.75f, 1.25f));
      country.healthyPopulationSusceptible = country.originalPopulation - country.healthyPopulationImmune;
      ld.apeHealthyPopulation = country.apeOriginalPopulation;
    }
    return ld;
  }

  public override long GetScore(bool won, bool scenario)
  {
    if (CGameManager.CheckExternalMethodExist("GetScoreWin") & won)
      return (long) CGameManager.CallExternalMethodWithReturnValue("GetScoreWin", World.instance, (Disease) this, (Country) null, (LocalDisease) null);
    if (CGameManager.CheckExternalMethodExist("GetScoreLose") && !won)
      return (long) CGameManager.CallExternalMethodWithReturnValue("GetScoreLose", World.instance, (Disease) this, (Country) null, (LocalDisease) null);
    if (CGameManager.IsFederalScenario("348关 万圣节特供 夜之复调I"))
    {
      if (this.turnNumber > 1000)
        return 0;
      long score = (long) ((double) this.globalDeadPerc * 80000.0);
      if ((double) this.customGlobalVariable5 >= 0.40000000596046448)
        score += 15000L;
      return score;
    }
    if (CGameManager.IsFederalScenario("迪黎克菌：世界狂潮") && (double) this.globalCureResearch < (double) this.cureRequirements)
      return (long) (40000.0 * (double) Mathf.Max(0.0f, this.cureCompletePerc) + (double) this.mutationCounter * 100.0);
    if (CGameManager.IsFederalScenario("海盗瘟疫MXM") && !won)
    {
      long score = (long) (60000.0 + 0.5 * (double) this.mutation);
      if (score > 60000L)
        score = 60000L;
      return score;
    }
    if (CGameManager.IsFederalScenario("绯色审判"))
    {
      if (won)
        return (long) (60000.0 + 20000.0 * (1.0 - (double) this.cureCompletePerc) + 20000.0 * (1.0 - (double) this.globalDeadPerc));
      return !World.instance.firedEvents.ContainsKey("【时空魔法】第二阶段") ? (long) (20000.0 * (double) this.customGlobalVariable4 / 100.0) : (long) (30000.0 + 20000.0 * (double) this.customGlobalVariable4 / 100.0);
    }
    if (CGameManager.IsFederalScenario("终末千面"))
      return (long) (75000.0 * (double) this.globalDeadPerc);
    if (CGameManager.IsFederalScenario("里318 塞贝克鳄（荒寂）") && !won)
      return 4L * (16000L + (long) this.mutation);
    if (CGameManager.IsFederalScenario("263关 Cure-Candida auri"))
    {
      if (won)
        return 75000;
      if (!won)
        return (double) this.customGlobalVariable1 < 0.5 ? 0L : (long) ModelUtils.Min(75000.0, 35000.0 + 100000.0 * ((double) this.customGlobalVariable1 - 0.5));
    }
    if (CGameManager.IsFederalScenario("Spore War"))
      return (long) (75000.0 * (double) this.totalMusicPoint / (double) this.totalMusicImportance);
    if (CGameManager.IsFederalScenario("298关 淬毒蚁国") && !won)
      return this.totalDead * 1300L / 10000000L;
    if (CGameManager.IsFederalScenario("319关 五象鸑鷟") && !won)
      return (long) (1.6 * (38400.0 + (double) this.mutation));
    if (CGameManager.IsFederalScenario("镜生虫ReMASTER") && !won)
      return (double) this.globalDeadPerc < 0.60000002384185791 ? 0L : (long) (40000.0 * (double) this.globalDeadPerc);
    if (CGameManager.IsFederalScenario("时生虫ReCRAFT"))
    {
      if (won)
        return 10000;
      return (double) this.globalDeadPercent >= 0.75 ? 5000L : 0L;
    }
    if (CGameManager.IsFederalScenario("时生虫ReMASTER"))
      return (long) ((double) this.globalDeadPercent * 65000.0 + (double) CSLocalUGCHandler.GetScenarioHighScore("PIFSL 时生虫ReCRAFT"));
    float num1 = this.diseaseData.globalSeverity * (float) (this.evoPoints * 2 + this.diseaseData.evoPointsSpent);
    float num2 = 1f;
    if (this.diseaseData.difficulty == 2)
      num2 = 8f;
    else if (this.diseaseData.difficulty == 1)
      num2 = 4f;
    else if (this.diseaseData.difficulty == 3)
      num2 = 12f;
    float num3 = 0.0f;
    if ((double) this.diseaseData.cureRequirements > 0.0)
      num3 = this.diseaseData.globalCureResearch / this.diseaseData.cureRequirements;
    float a = Mathf.Clamp(num3 * 100f, 0.0f, 100f);
    float num4 = 1f;
    switch (this.diseaseType)
    {
      case Disease.EDiseaseType.VIRUS:
      case Disease.EDiseaseType.FUNGUS:
      case Disease.EDiseaseType.PARASITE:
      case Disease.EDiseaseType.PRION:
      case Disease.EDiseaseType.NANO_VIRUS:
      case Disease.EDiseaseType.BIO_WEAPON:
        num4 = 0.9f;
        break;
      case Disease.EDiseaseType.NEURAX:
        num4 = 1.1f;
        break;
      case Disease.EDiseaseType.NECROA:
        a /= 5f;
        num4 = 1.4f;
        if (this.diseaseData.difficulty >= 2)
        {
          num2 = 10f;
          break;
        }
        break;
    }
    int num5 = Mathf.FloorToInt(Mathf.Log((float) this.diseaseData.turnNumber));
    long f = (long) ((double) Mathf.Max(0.0f, (float) ((double) num1 * (double) num2 * (double) num4 / ((double) Mathf.Max(a, 1f) / 2.0 * (double) num5))) + (double) this.diseaseData.globalDeadPercent * 100.0);
    if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      f = (long) ((double) f * (1.1000000238418579 + 1.0 * (double) this.diseaseData.apeTotalInfected / (double) this.diseaseData.apeTotalPopulation - 1.0 * (double) this.diseaseData.apeTotalDead / (double) this.diseaseData.apeTotalPopulation / 2.0));
    long score1 = won ? f * 10L : (long) Mathf.Max(0.0f, Mathf.Log((float) f) * 10f);
    if (!scenario)
      Debug.Log((object) ("Score Details:\nGenetic Complexity: " + num1.ToString("N2") + "\nDifficulty Modifier: " + num2.ToString("N0") + "\nCure Progress: " + num3.ToString("N6") + "\nCure Modifier: " + a.ToString("N4") + "\nPathogen Modifier: " + num4.ToString("N1") + "\nSpeed Modifier: " + num5.ToString("N0") + "\nFinal Score: " + score1.ToString("N0")));
    if (scenario)
    {
      if (won)
      {
        score1 = (long) Mathf.Max(0.0f, (float) (10000000.0 / ((double) this.diseaseData.turnNumber / 2.0) * ((1.0 - (double) num3) / 3.0 + 1.0)) * World.instance.scenarioScoreMultiplier);
        if (this.scenario == "board_game")
        {
          Debug.Log((object) ("BOARD GAME SCORE: " + (object) this.totalDead));
          score1 = this.totalDead;
        }
        if (CGameManager.IsFederalScenario("ReconstructionIncBeyond") && (double) this.globalHealthyPercent >= 0.0099999997764825821)
          score1 += (long) (15000.0 * Math.Pow((double) this.globalHealthyPercent, 1.17));
        if (CGameManager.IsFederalScenario("298关 淬毒蚁国") & won)
        {
          score1 /= 4L;
          score1 += 75400L;
        }
        if (CGameManager.IsFederalScenario("319关 五象鸑鷟") & won)
        {
          score1 = (long) ((double) score1 / 3.5);
          score1 += 61440L;
        }
        if (CGameManager.IsFederalScenario("镜生虫ReMASTER") & won)
          score1 += 40000L;
        if (CGameManager.IsFederalScenario("里318 塞贝克鳄（荒寂）") & won)
        {
          score1 /= 6L;
          score1 += 65000L;
        }
        if (CGameManager.IsFederalScenario("迪黎克菌：世界狂潮") && (double) this.globalCureResearch >= (double) this.cureRequirements)
          return (long) (40000.0 + (double) score1 + (double) this.mutationCounter * 100.0 + (1.0 - (double) this.globalDeadPerc) * 100.0);
        if (CGameManager.IsFederalScenario("海盗瘟疫MXM") & won)
        {
          long num6 = (long) (60000.0 + 0.5 * (double) this.mutation);
          if (num6 > 60000L)
            num6 = 60000L;
          if (this.diseaseType == Disease.EDiseaseType.FUNGUS)
            num6 -= 2500L;
          else if (this.diseaseType == Disease.EDiseaseType.NEURAX)
            num6 -= 7000L;
          else if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
            num6 += 6000L;
          else if (this.diseaseType == Disease.EDiseaseType.NECROA)
            num6 += 3500L;
          else if (this.diseaseType == Disease.EDiseaseType.VAMPIRE)
            num6 -= 4000L;
          score1 += num6;
        }
      }
      else
        score1 = 0L;
    }
    return score1;
  }

  public override int GetEvolveCost(Technology technology)
  {
    int num = 0;
    if (technology.gridType == Technology.ETechType.transmission)
      num = this.transmissionExtraCost;
    else if (technology.gridType == Technology.ETechType.symptom)
      num = this.symptomExtraCost;
    else if (technology.gridType == Technology.ETechType.ability)
      num = this.abilityExtraCost;
    if (technology.skipIncreaseTypeCost && this.isCure)
      num = 0;
    int cost = -technology.cost + num - this.GetTechCostMod(technology);
    return technology.id.Equals("522226b8-000b-41b0-87e3-2ba57d2b29e2") && CGameManager.IsFederalScenario("时生虫ReCRAFT") && (double) this.GetCountryVariable("greenland") > 0.0 || (technology.id.Equals("9ccf1319-71ba-4c4b-9183-60db33c513e4") || technology.id.Equals("c51ecb6e-5f53-4f94-a682-dba046f89bb0")) && CGameManager.IsFederalScenario("时生虫ReCRAFT") && (double) this.customGlobalVariable2 <= 1.5 ? 114514 : SPDiseaseExternal.GetGeneticDriftCost(this.diseaseData, cost);
  }

  public override int GetDeEvolveCost(Technology technology)
  {
    if (CGameManager.IsFederalScenario("终末千面"))
      return -2;
    if ((double) technology.refundPerc != 0.0)
      return -(int) ((double) this.GetTechCostPaid(technology) * (double) technology.refundPerc);
    int deEvolveCost = 0;
    if (technology.gridType == Technology.ETechType.transmission)
      deEvolveCost = this.transmissionDevolveCost;
    else if (technology.gridType == Technology.ETechType.symptom)
      deEvolveCost = this.symptomDevolveCost;
    else if (technology.gridType == Technology.ETechType.ability)
      deEvolveCost = this.abilityDevolveCost;
    if ((double) technology.devolveCostMultipler > 0.0)
    {
      if (deEvolveCost < 0)
        deEvolveCost = -deEvolveCost;
      deEvolveCost = Mathf.FloorToInt((float) deEvolveCost * technology.devolveCostMultipler);
    }
    int evolveCost = this.GetEvolveCost(technology);
    if (deEvolveCost < -evolveCost)
      deEvolveCost = -evolveCost;
    return deEvolveCost;
  }

  public override bool CheckEvo()
  {
    bool flag = SPDiseaseExternal.CheckEvo(this.diseaseData);
    Debug.Log((object) ("INFO: Check Evo: " + flag.ToString()));
    return flag;
  }

  public override bool CheckTurn()
  {
    bool flag = SPDiseaseExternal.CheckTurn(this.diseaseData);
    Debug.Log((object) ("INFO: Check Turn: " + flag.ToString()));
    return flag;
  }

  public void OnTutorialBegin(Module withModule)
  {
  }

  public void OnTutorialComplete(Module completedModule)
  {
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  [GameEventFunction]
  public override void BoardGameEndMessage()
  {
    List<Technology> all1 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.themeQtyChange > 0));
    List<Technology> all2 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.mechanicsQtyChange > 0));
    List<Technology> all3 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.componentQtyChange > 0));
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    for (; num < 3 && all1.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all1.Count - 1);
      Technology technology = all1[index];
      all1.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append('/');
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    for (; num < 4 && all2.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all2.Count - 1);
      Technology technology = all2[index];
      all2.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append('/');
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    if (all3.Count <= 0)
      return;
    Technology technology1 = all3[ModelUtils.IntRand(0, all3.Count - 1)];
    string text;
    string tagName;
    if (this.totalDead <= 5000L)
    {
      text = "%s sales disappoint";
      tagName = "Oh dear.  Your investors consider your %s game with %s to be a failure and refuse to back future projects.";
    }
    else if (this.totalDead <= 50000L)
    {
      text = "%s a hit!";
      tagName = "Congratulations! %s sold well - players liked your %s game with %s! There's still room for improvement though.";
    }
    else
    {
      text = "%s smashes sales records";
      tagName = "Congratulations! Your game sold amazingly well and is a global success story! Now everyone is trying to make %s games with %s!";
    }
    ParameterisedString title = new ParameterisedString(text, new string[1]
    {
      "disease.name"
    });
    World.instance.popupMessages.Add(new PopupMessage("event_boardgame", title, CGameManager.GetDisplayDate(), new ParameterisedString(CLocalisationManager.GetText(tagName), new StringParameter[2]
    {
      new StringParameter(literal: stringBuilder.ToString()),
      new StringParameter(literal: CLocalisationManager.GetText(technology1.name))
    })
    {
      useLocalisation = false
    }, (Disease) this));
  }

  [GameEventFunction]
  public override void BoardGameLockDesign()
  {
    double num1 = (double) this.themeQty / 15.0;
    double num2 = num1 * num1;
    float num3 = (float) this.mechanicsQty / 12f;
    float num4 = num3 * num3;
    float infectiousnessMax = this.globalInfectiousnessMax;
    if (this.themeQty < 1 || this.mechanicsQty < 1 || this.componentQty < 1 || this.playerQty < 1)
      infectiousnessMax *= 0.01f;
    double num5 = (double) ModelUtils.FloatRand(0.0f, 1f);
    if (num2 > num5 && this.themeQty > 4)
    {
      infectiousnessMax *= 0.5f;
      ParameterisedString title = new ParameterisedString("Theme overload!");
      ParameterisedString message = new ParameterisedString("You've designed a board game with far too many themes - it's confusing players and turning them off your game.");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    if ((double) num4 > (double) ModelUtils.FloatRand(0.0f, 1f) && this.mechanicsQty > 4)
    {
      infectiousnessMax *= 0.5f;
      ParameterisedString title = new ParameterisedString("Mechanics overload!");
      ParameterisedString message = new ParameterisedString("You've designed a board game with far too many mechanics - it's confusing players and turning them off your game.");
      World.instance.popupMessages.Add(new PopupMessage("nuclear_explosion", title, CGameManager.GetDisplayDate(), message, (Disease) this));
    }
    if ((double) this.globalSeverityMax > (double) ModelUtils.IntRand(0, 100))
      infectiousnessMax *= (float) (1.0 - (double) this.globalSeverityMax * 0.004999999888241291);
    this.globalInfectiousnessMax = infectiousnessMax * ModelUtils.FloatRand(1f, 1.4f);
  }

  [GameEventFunction]
  public override void BoardGameLockSymptoms()
  {
    List<Technology> all1 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.themeQtyChange > 0));
    List<Technology> all2 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.mechanicsQtyChange > 0));
    List<Technology> all3 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.componentQtyChange > 0));
    string text1 = "Distribution phase begins";
    string text2 = "Distribution phase begins";
    string icon = "event_boardgame";
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    for (; num < 3 && all1.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all1.Count - 1);
      Technology technology = all1[index];
      all1.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append("/");
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    for (; num < 4 && all2.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all2.Count - 1);
      Technology technology = all2[index];
      all2.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append("/");
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    ParameterisedString message;
    if (all3.Count > 0)
    {
      int index = ModelUtils.IntRand(0, all3.Count - 1);
      Technology technology = all3[index];
      message = new ParameterisedString(CLocalisationManager.GetText("Everyone is certain that a %s game with %s is going to be mega popular..."), new StringParameter[2]
      {
        new StringParameter(literal: stringBuilder.ToString()),
        new StringParameter(literal: CLocalisationManager.GetText(technology.name))
      });
      message.useLocalisation = false;
    }
    else
      message = new ParameterisedString(text2);
    ParameterisedString title = new ParameterisedString(text1, new string[1]
    {
      "disease.name"
    });
    World.instance.popupMessages.Add(new PopupMessage(icon, title, CGameManager.GetDisplayDate(), message, (Disease) this));
  }

  [GameEventFunction]
  public override void BoardGameRepeatingHeadline()
  {
    List<string> stringList = new List<string>();
    stringList.Add("YouTuber sings song about %s element in %s");
    stringList.Add("Blogger raves about %s element in %s");
    stringList.Add("Player rants about %s element in %s");
    stringList.Add("Player loves %s element in %s");
    stringList.Add("Player ambivalent about %s element in %s");
    stringList.Add("Preview positive about %s element in %s");
    stringList.Add("Forum backlash against %s element in %s");
    stringList.Add("Reviewer loves %s element in %s");
    stringList.Add("Concern over %s element in %s");
    stringList.Add("Confusion over %s element in %s");
    List<Technology> all = this.technologies.FindAll((Predicate<Technology>) (a =>
    {
      if (!this.IsTechEvolved(a.id))
        return false;
      return a.themeQtyChange > 0 || a.mechanicsQtyChange > 0 || a.componentQtyChange > 0;
    }));
    if (all.Count <= 0)
      return;
    ParameterisedString n = new ParameterisedString(stringList[ModelUtils.IntRand(0, stringList.Count - 1)], new StringParameter[2]
    {
      new StringParameter(literal: all[ModelUtils.IntRand(0, all.Count - 1)].name),
      new StringParameter("disease.name")
    });
    World.instance.AddNewsItem(new IGame.NewsItem(n, (Disease) this, (Country) null, 2));
  }

  [GameEventFunction]
  public override void BoardGameDesignEnforce()
  {
    List<Technology> all1 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.themeQtyChange > 0));
    List<Technology> all2 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.mechanicsQtyChange > 0));
    List<Technology> all3 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.componentQtyChange > 0));
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    for (; num < 3 && all1.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all1.Count - 1);
      Technology technology = all1[index];
      all1.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append("/");
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    for (; num < 4 && all2.Count > 0; ++num)
    {
      int index = ModelUtils.IntRand(0, all2.Count - 1);
      Technology technology = all2[index];
      all2.RemoveAt(index);
      if (num != 0)
        stringBuilder.Append("/");
      stringBuilder.Append(CLocalisationManager.GetText(technology.name));
    }
    string text = "%s has been designed";
    string icon = "popup_generic_world_icon";
    if (all3.Count <= 0)
      return;
    int index1 = ModelUtils.IntRand(0, all3.Count - 1);
    Technology technology1 = all3[index1];
    ParameterisedString message = new ParameterisedString(CLocalisationManager.GetText("Everyone is certain that a %s game with %s is going to be mega popular..."), new StringParameter[2]
    {
      new StringParameter(literal: stringBuilder.ToString()),
      new StringParameter(literal: CLocalisationManager.GetText(technology1.name))
    });
    message.useLocalisation = false;
    ParameterisedString title = new ParameterisedString(text, new string[1]
    {
      "disease.name"
    });
    World.instance.popupMessages.Add(new PopupMessage(icon, title, CGameManager.GetDisplayDate(), message, (Disease) this));
  }

  [GameEventFunction]
  public override void CustomForceZComSpawn(Country fortCountry)
  {
    if (this.diseaseType != Disease.EDiseaseType.VAMPIRE && this.diseaseType != Disease.EDiseaseType.NECROA)
      return;
    if (!this.diseaseData.firstFortSelected)
    {
      this.diseaseData.firstFortSelected = true;
      this.diseaseData.fortSelectionDay = this.diseaseData.zdayCounter;
    }
    fortCountry.fortState = EFortState.FORT_ALIVE;
    if (this.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      LocalDisease localDisease = this.GetLocalDisease(fortCountry);
      localDisease.fortHealthMax = 100f;
      if (this.difficulty < 1)
        localDisease.fortHealthMax *= 0.75f;
      else if (this.difficulty < 2)
        localDisease.fortHealthMax *= 0.9f;
      localDisease.fortHealth = localDisease.fortHealthMax;
      if (fortCountry.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || fortCountry.apeLabStatus == EApeLabState.APE_LAB_INACTIVE)
        fortCountry.ChangeApeLabState = EApeLabState.APE_LAB_NONE;
    }
    this.diseaseData.daysUntilMinifortPlaneSpawn = ModelUtils.IntRand(12, 30);
    ++this.numberOfFortsCreated;
    World.instance.AddFort(fortCountry);
  }

  public override void OnInitCure()
  {
  }

  public void GameUpdate_Cure()
  {
    this.lastTotalInfectedIntelGUI = this.totalInfectedIntelGUI;
    this.lastTotalDeadIntelGUI = this.totalDeadIntelGUI;
    this.diseaseData.mutatedThisTurn = false;
    World instance = World.instance;
    this.diseaseData.apeTotalPopulation = instance.apeTotalPopulation;
    this.diseaseData.totalPopulation = instance.totalPopulation;
    this.diseaseData.vaccineDevResearched = this.IsTechEvolved("Development") ? 1 : 0;
    this.diseaseData.vaccinePrepManResearched = this.IsTechEvolved("Prepare_Manufacturing") ? 1 : 0;
    this.diseaseData.declareKnowledgeResearched = this.IsTechEvolved("Declare_Knowledge") ? 1 : 0;
    this.diseaseData.skipDevResearched = this.IsTechEvolved("Skip_Development") ? 1 : 0;
    this.diseaseData.NorthAmericaAlertResearched = this.IsTechEvolved("North_America_Alert") ? 1 : 0;
    this.diseaseData.SouthAmericaAlertResearched = this.IsTechEvolved("South_America_Alert") ? 1 : 0;
    this.diseaseData.EuropeAlertResearched = this.IsTechEvolved("Europe_Alert") ? 1 : 0;
    this.diseaseData.AfricaAlertResearched = this.IsTechEvolved("Africa_Alert") ? 1 : 0;
    this.diseaseData.AsiaAlertResearched = this.IsTechEvolved("Asia-Pacific_Alert") ? 1 : 0;
    this.diseaseData.teamCt1Researched = this.IsTechEvolved("Disease_Containment_Experts") ? 1 : 0;
    this.diseaseData.teamAidResearched = this.IsTechEvolved("Emergency_Aid_Experts") ? 1 : 0;
    this.diseaseData.teamFieldResearchersResearched = this.IsTechEvolved("Field_Reseach_Experts") ? 1 : 0;
    this.diseaseData.aaLockdownsResearched = this.IsTechEvolved("Lockdowns") ? 1 : 0;
    this.diseaseData.aaMedicalAidResearched = this.IsTechEvolved("Debt_Relief") ? 1 : 0;
    this.diseaseData.borderMonitoringResearched = this.IsTechEvolved("Border_Monitoring") ? 1 : 0;
    this.diseaseData.damageLimitationResearched = this.IsTechEvolved("Damage_Limitation") ? 1 : 0;
    if (this.preSimulate == 0 && this.numCountriesIntel < instance.countries.Count && this.IsTechEvolved("Build_Monitoring_Stations"))
    {
      if (--this.nextIntelSpread <= 0)
      {
        World.instance.AddAchievement(EAchievement.A_iwillfindyou);
        this.nextIntelSpread = ModelUtils.IntRand(8, 10) - this.intelTimeReduction;
        if (this.IsTechEvolved("Government_Partnerships"))
          --this.nextIntelSpread;
        if (this.IsTechEvolved("Outbreak_Intelligence_Service"))
          --this.nextIntelSpread;
        List<TravelRoute> possibleRoutes = new List<TravelRoute>();
        RandomPicker picker = new RandomPicker();
        for (int index = 0; index < instance.countries.Count; ++index)
        {
          Country source = instance.countries[index];
          LocalDisease localDisease1 = source.GetLocalDisease((Disease) this);
          if (localDisease1.hasIntel || source == this.hqCountry)
          {
            Action<List<TravelRoute>, bool> action = (Action<List<TravelRoute>, bool>) ((routeList, border) =>
            {
              foreach (TravelRoute route in routeList)
              {
                Country destination = route.destination;
                if (destination == source)
                {
                  Debug.LogError((object) "TARGET IS SOURCE? CHECK THIS! - FM");
                }
                else
                {
                  LocalDisease localDisease2 = destination.GetLocalDisease((Disease) this);
                  if (!localDisease2.hasIntel && !localDisease2.HasFlag(Country.EGenericCountryFlag.IntelVehicleDispatched))
                  {
                    float frequency = route.frequency;
                    if (source == this.hqCountry)
                      frequency *= 10f;
                    float poss = frequency * destination.importance;
                    if (!this.easyIntel)
                    {
                      if ((double) destination.infectedPercent > 0.0)
                        poss *= (float) (1.0 + (double) destination.infectedPercent * 10000.0);
                      if (destination == this.nexus && this.numCountriesIntel < 7)
                        poss = 0.0f;
                      if ((double) destination.infectedPercent > 0.0 && this.numCountriesIntel < 5)
                        poss = 0.0f;
                    }
                    else
                    {
                      if ((double) destination.infectedPercent > 0.0)
                      {
                        this.intelInfectedFound = true;
                        poss *= 500f;
                      }
                      if (this.turnNumber < 3 && destination == this.nexus)
                        poss = 0.0f;
                    }
                    if (destination == this.teamTravelTarget)
                      poss = 0.0f;
                    if (this.IsContinentAlert(destination.continentType))
                      poss *= 100f;
                    if ((double) poss > 0.0)
                    {
                      picker.AddPossibility(possibleRoutes.Count, poss);
                      possibleRoutes.Add(route);
                    }
                  }
                }
              }
            });
            action(source.landRoutes, localDisease1.AreBordersOpen());
            action(source.airRoutes, localDisease1.AreAirportsOpen());
            action(source.seaRoutes, localDisease1.ArePortsOpen());
          }
        }
        List<Country> countryList = new List<Country>();
        int num = ModelUtils.IntRand(1, 3);
        if (this.easyIntel)
          num = 1;
        if (this.IsTechEvolved("Government_Partnerships"))
        {
          if (this.easyIntel)
            num = ModelUtils.IntRand(1, 3) + 1;
          else
            ++num;
        }
        if (this.IsTechEvolved("Outbreak_Intelligence_Service"))
          ++num;
        if (this.numCountriesIntel <= 1 && !this.intelInfectedFound && this.easyIntel)
        {
          Country randomInfectedCountry = this.GetRandomInfectedCountry(this.nexus);
          if (randomInfectedCountry != null)
          {
            --num;
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.Airplane;
            vehicle.subType = Vehicle.EVehicleSubType.Intel;
            vehicle.actingDisease = (Disease) this;
            vehicle.SetRoute(this.hqCountry, randomInfectedCountry);
            World.instance.AddVehicle(vehicle);
          }
        }
        for (int index1 = 0; index1 < num && picker.Count() > 0; ++index1)
        {
          int index2 = picker.Pick();
          picker.RemovePossibility(index2);
          if (index2 != -1)
          {
            TravelRoute travelRoute = possibleRoutes[index2];
            Country source = travelRoute.source;
            Country destination = travelRoute.destination;
            Vehicle.EVehicleType evehicleType = Vehicle.EVehicleType.None;
            if (travelRoute.routeType == ERouteType.Land)
              evehicleType = Vehicle.EVehicleType.Helicopter;
            else if (travelRoute.routeType == ERouteType.Sea)
              evehicleType = Vehicle.EVehicleType.Boat;
            else if (travelRoute.routeType == ERouteType.Air)
              evehicleType = Vehicle.EVehicleType.Airplane;
            else
              Debug.Log((object) ("ROUTE: " + (object) travelRoute.routeType + " from " + source.id + " to " + destination.id));
            if (countryList.Contains(destination))
            {
              --index1;
            }
            else
            {
              countryList.Add(destination);
              LocalDisease localDisease = this.GetLocalDisease(destination);
              this.GetLocalDisease(destination).AddFlag(Country.EGenericCountryFlag.IntelVehicleDispatched);
              if (evehicleType != Vehicle.EVehicleType.None)
              {
                Vehicle vehicle = Vehicle.Create();
                vehicle.type = evehicleType;
                vehicle.actingDisease = (Disease) this;
                vehicle.subType = Vehicle.EVehicleSubType.Intel;
                vehicle.SetRoute(source, destination);
                World.instance.AddVehicle(vehicle);
              }
              else
              {
                Debug.LogError((object) "This probably shouldn't be reached - vehicle type null!");
                localDisease.hasIntel = true;
              }
            }
          }
        }
      }
    }
    if (this.diseaseData.evoPointsPrevTurn == this.evoPoints)
      ++this.diseaseData.numTurnsWithoutEvoChange;
    this.diseaseData.evoPointsPrevTurn = this.evoPoints;
    if (this.diseaseData.numTurnsWithoutEvoChange >= 3)
    {
      this.diseaseData.numTurnsWithoutEvoChange = 0;
      ++this.diseaseData.evoBoost;
    }
    if (this.difficulty > 1)
    {
      this.SetTechCostMod(this.GetTechnology("Development"), (int) Mathf.Min(15f, (this.difficulty == 2 ? 7f : 0.0f) + Mathf.Max((float) this.turnsSinceKnowledge * 0.16f, Mathf.Max(this.globalPriority, (float) this.infectedCountriesIntel))));
      this.SetTechCostMod(this.GetTechnology("Prepare_Manufacturing"), -5 - Mathf.RoundToInt((this.difficulty == 2 ? 10f : 15f) * (1f - this.cureCompletePerc)));
    }
    else
    {
      this.SetTechCostMod(this.GetTechnology("Development"), 15);
      this.SetTechCostMod(this.GetTechnology("Prepare_Manufacturing"), -5);
    }
    if (this.preSimulate == 0 && this.turnNumber == 2)
    {
      if (this.difficulty == 2)
        this.SetTechCostMod(this.GetTechnology("Kill_code_Supercomputing"), -5);
      if (this.difficulty == 3)
        this.SetTechCostMod(this.GetTechnology("Kill_code_Supercomputing"), -10);
    }
    SPDiseaseExternal.DiseaseUpdate_Cure(this.diseaseData);
    if (instance.DiseaseTurn < 40 && this.diseaseData.totalInfected < 121L && ModelUtils.IntRand(0, 100) < 10)
      this.nexus.TransferPopulation((double) ModelUtils.IntRand(1, 5), Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.INFECTED);
    long totalInfectedIntel = this.totalInfectedIntel;
    SPDiseaseExternal.DiseasePrepareTotals_Cure(this.diseaseData);
    this.infectedCountries.Clear();
    int num1 = 0;
    int num2 = 0;
    float num3 = 0.0f;
    float num4 = 0.0f;
    this.totalHealthyIntel = 0L;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      if (localDisease.hasIntel)
        this.totalHealthyIntel += country.healthyPopulation;
      SPDiseaseExternal.DiseaseCountryUpdate_Cure(this.diseaseData, ((SPCountry) country).mData, ((SPLocalDisease) localDisease).mData);
      if (!localDisease.AreBordersOpen())
        ++this.globalBorderClosedNum;
      if (country.hasAirports)
      {
        num1 += country.hasAirports ? 1 : 0;
        if (!localDisease.AreAirportsOpen())
          this.globalAirportClosedNum += country.hasAirports ? 1 : 0;
      }
      if (country.hasPorts)
      {
        num2 += country.hasPorts ? 1 : 0;
        if (!localDisease.ArePortsOpen())
          this.globalPortClosedNum += country.hasPorts ? 1 : 0;
      }
      if (localDisease.allInfected > 0L)
      {
        this.infectedCountries.Add(country);
        if (localDisease.allInfected > 20L || country.deadPopulation > 1L)
          ++this.spreadCountries;
      }
      if (localDisease.HasTeams())
        this.teamHighestInfectedPerc = Mathf.Max(localDisease.infectedPercent, this.teamHighestInfectedPerc);
      if (country.hot)
      {
        ++num4;
        if ((double) localDisease.infectedPercent > 0.0)
          ++num3;
      }
    }
    this.diseaseData.infectedCountryCount = this.infectedCountries.Count;
    this.diseaseData.uninfectedCountryCount = World.instance.countries.Count - this.diseaseData.infectedCountryCount;
    this.globalAvgCompliance /= (float) World.instance.countries.Count;
    this.averageCountryInfectedPerc /= (float) World.instance.countries.Count;
    this.averageCountryDeadPerc /= (float) World.instance.countries.Count;
    this.globalBorderClosedPerc = (float) this.globalBorderClosedNum / (float) World.instance.countries.Count;
    if (num1 > 0)
      this.globalAirportClosedPerc = (float) this.globalAirportClosedNum / (float) num1;
    if (num2 > 0)
      this.globalPortClosedPerc = (float) this.globalPortClosedNum / (float) num2;
    SPDiseaseExternal.DiseasePostCountryUpdate_Cure(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
    this.diseaseData.hiZombifiedPopulation = 0.0f;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      localDisease.GameUpdate();
      World.instance.CheckGovernmentActions(country);
      this.diseaseData.hiZombifiedPopulation += localDisease.H2Z + localDisease.I2Z + localDisease.D2Z;
    }
    SPDiseaseExternal.DiseasePostLocalUpdate_Cure(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
    if (this.diseaseNoticed)
    {
      float understandingSpeed = this.understandingSpeed;
      if (this.HasFlag(Disease.EGenericDiseaseFlag.GeneMolecularBiologist))
        understandingSpeed += 0.1f;
      this.vaccineKnowledge += (float) (1.0 / ((double) this.vaccineKnowledgeMonths * 30.0) * (1.0 + 1.0 * (double) Mathf.Min(1f, (float) this.infectedCountriesIntel / 15f))) * understandingSpeed;
      if (this.turnNumber % 4 == 0 && this.nexus.GetLocalDisease((Disease) this).hasTeam)
        this.vaccineKnowledge += 0.01f;
      if ((double) this.vaccineKnowledge >= 1.0 || (double) this.vaccineKnowledgeMonths == 0.0)
      {
        ++this.turnsSinceKnowledge;
        if (!this.IsTechEvolved("Declare_Knowledge"))
          this.EvolveTech(this.GetTechnology("Declare_Knowledge"), true);
      }
    }
    else if ((double) this.vaccineKnowledgeMonths > 0.0 && this.turnNumber > 5 && this.preSimulate == 0)
    {
      this.vaccineKnowledgeMonths -= 0.017f;
      this.vaccineKnowledgeMonths = Mathf.Max(this.vaccineKnowledgeMonthsStart * 0.5f, this.vaccineKnowledgeMonths);
    }
    switch (this.vaccineStage)
    {
      case Disease.EVaccineProgressStage.VACCINE_NONE:
        if ((double) this.vaccineKnowledgeMonths <= 0.0 && this.diseaseNoticed)
        {
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT;
          break;
        }
        if (this.diseaseNoticed)
        {
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE;
          break;
        }
        break;
      case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE:
        if ((double) this.vaccineKnowledge >= 1.0)
        {
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL;
          break;
        }
        break;
      case Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL:
        if ((double) this.vaccineKnowledge >= 1.0 && this.IsTechEvolved("Development"))
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT;
        if (this.isNanovirus && this.IsTechEvolved("Develop_Kill_code"))
        {
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT;
          break;
        }
        break;
      case Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT:
        if ((double) this.cureCompletePerc >= 1.0)
        {
          if (this.isNanovirus)
          {
            this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_RELEASE;
            break;
          }
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_MANUFACTURE;
          break;
        }
        break;
      case Disease.EVaccineProgressStage.VACCINE_MANUFACTURE:
        if ((double) this.cureCompletePerc >= 1.0)
        {
          this.vaccineStage = Disease.EVaccineProgressStage.VACCINE_RELEASE;
          break;
        }
        break;
    }
    Technology technology = this.GetTechnology("Skip_Development");
    if (technology != null)
    {
      if (this.vaccineStage >= Disease.EVaccineProgressStage.VACCINE_RELEASE)
        technology.padlocked = true;
      if (this.IsTechEvolved(technology) && !this.skipDevFired)
      {
        if ((double) this.cureCompletePerc < 1.0)
        {
          if ((double) this.cureCompletePerc >= 0.10000000149011612 && (double) ModelUtils.FloatRand(0.0f, 1f) < (double) this.cureCompletePerc)
          {
            this.globalCureResearch += this.cureRequirements * 0.2f;
            this.skipDevSuccess = true;
          }
          else
          {
            this.globalCureResearch -= this.cureRequirements * 0.2f;
            this.globalCureResearch = Mathf.Max(0.0f, this.globalCureResearch);
          }
        }
        else if ((double) this.manufactureProgress >= 0.10000000149011612 && (double) ModelUtils.FloatRand(0.0f, 1f) < (double) this.manufactureProgress)
        {
          this.manufactureProgress += 0.2f;
          this.manufactureProgress = Mathf.Min(this.manufactureProgress, 1f);
          this.skipDevSuccess = true;
        }
        else
        {
          this.manufactureProgress -= 0.2f;
          this.manufactureProgress = Mathf.Max(this.manufactureProgress, 0.0f);
        }
        this.skipDevFired = true;
      }
    }
    if (this.vaccineStage == Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT && (double) this.cureCompletePercent <= 0.0)
    {
      this.globalCureResearch = 0.0f;
      this.cureBaseMultiplier = 0.0f;
      this.cureRequirements = this.ConvertMonthsToResearchRequirement(this.vaccineDevMonths);
    }
    if (this.vaccineStage == Disease.EVaccineProgressStage.VACCINE_MANUFACTURE && (double) this.cureCompletePercent >= 1.0 && !this.manufactureSet)
    {
      this.cureBaseMultiplier = 0.0f;
      this.cureRequirements = this.ConvertMonthsToResearchRequirement(this.vaccineManMonths);
      this.globalCureResearch = Mathf.Min(0.9f * this.cureRequirements, this.manufactureProgress);
      this.researchWeeklyChange.Clear();
      if (!this.IsTechEvolved("Prepare_Manufacturing"))
        this.EvolveTech(this.GetTechnology("Prepare_Manufacturing"), true);
      if (!this.IsTechEvolved("Accelerated_Research"))
        this.PadlockTech("Accelerated_Research", true);
      if (!this.IsTechEvolved("Global_Research_Treaty"))
        this.PadlockTech("Global_Research_Treaty", true);
      this.manufactureSet = true;
      this.cureCompletePercent = (double) this.cureRequirements == 0.0 ? 0.0f : this.globalCureResearch / this.cureRequirements;
      this.cureCompletePercent = Mathf.Max(0.0f, this.cureCompletePercent);
    }
    if (this.vaccineStage == Disease.EVaccineProgressStage.VACCINE_RELEASE && (double) this.cureCompletePercent >= 1.0 && !this.cureFlag)
    {
      this.globalCureResearch = 0.0f;
      this.cureBaseMultiplier = 0.0f;
      this.cureRequirements = this.ConvertMonthsToResearchRequirement(this.vaccineReleaseMonths);
      if (!this.IsTechEvolved("Accelerated_Manufacturing"))
        this.PadlockTech("Accelerated_Manufacturing", true);
      if (!this.IsTechEvolved("Global_Manufacturing_Treaty"))
        this.PadlockTech("Global_Manufacturing_Treaty", true);
      if (this.isPrion && !this.IsTechEvolved("Neural_Enhancers"))
        this.PadlockTech("Neural_Enhancers", true);
      this.cureFlag = true;
      this.cureCompletePercent = (double) this.cureRequirements == 0.0 ? 0.0f : this.globalCureResearch / this.cureRequirements;
      this.cureCompletePercent = Mathf.Max(0.0f, this.cureCompletePercent);
    }
    SPDiseaseExternal.DiseasePostLocalUpdate_Cure2(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
    this.authLossInfectedList.Push(this.authLossInfectedActual);
    this.authLossDeadList.Push(this.authLossDeadActual);
    this.authLossOtherList.Push(this.authLossSpread);
    this.authLossComplianceList.Push(this.authLossCompliance);
    this.deadBubbleChance = (this.estimatedDeathRate + 200f * this.globalDeadPerc) / Mathf.Max(1f, Mathf.Pow((float) this.numTotalDeadBubblesDNA, 2f) * 0.01f);
    this.deadBubbleChance *= 4f;
    if (this.preSimulate == 0 && (double) this.deadBubbleChance > 0.0)
      ++this.turnsSinceDeadBubble;
    if (this.turnsSinceDeadBubble >= 20 && (double) this.deadBubbleChance > (double) ModelUtils.FloatRand(0.0f, 1f))
    {
      float num5 = 0.0f;
      LocalDisease localDisease3 = (LocalDisease) null;
      foreach (Country country in instance.countries)
      {
        LocalDisease localDisease4 = country.GetLocalDisease((Disease) this);
        if (!localDisease4.IsColouredBubbleVisible())
        {
          float num6 = localDisease4.deadPercWeeklyChange.Oldest();
          float num7 = (double) num6 == 0.0 ? 0.0f : (localDisease4.deadPercent - num6) / num6;
          if ((double) localDisease4.overflowLocalLethality > 0.40000000596046448 + 0.10000000149011612 * (double) localDisease4.numDeadBubbles && localDisease4.country.deadPopulation > 1000L && (double) num7 > 0.5 && localDisease4.country.currentPopulation > 0L)
          {
            float num8 = localDisease4.overflowLocalLethality * (float) (1L + localDisease4.country.originalPopulation / localDisease4.country.currentPopulation);
            if (localDisease3 == null || (double) num8 > (double) num5)
            {
              localDisease3 = localDisease4;
              num5 = num8;
            }
          }
        }
      }
      if (localDisease3 != null)
      {
        localDisease3.SpawnColoredBubble(Country.EGenericCountryFlag.DeadBubbleForCure);
        ++localDisease3.numDeadBubbles;
        ++this.numTotalDeadBubbles;
      }
      this.turnsSinceDeadBubble = 0;
    }
    if (this.preSimulate == 0)
    {
      foreach (Country country in instance.countries)
      {
        LocalDisease localDisease = this.GetLocalDisease(country);
        if (!localDisease.unrestActive)
        {
          float num9 = (float) ((double) localDisease.actualBaseInfluence * (double) this.globalBaseInfluence * (double) this.quarantineInfluence * (double) Mathf.Pow(country.publicOrder - country.govPublicOrder, 0.3f) * (1.0 - (double) country.healthyRecoveredPercent));
          if (this.HasFlag(Disease.EGenericDiseaseFlag.GeneStrategicFundraiser) && country == this.hqCountry && (double) country.infectedPercent <= 0.0)
            num9 *= 1.5f;
          if (!localDisease.hasIntel)
            num9 *= 0.7f;
          this.influencePoints += num9 * (float) (1.0 + 3.0 * (double) Mathf.Max(this.globalDeadPerc, this.averageCountryDeadPerc)) * 0.8f;
        }
      }
    }
    if (this.preSimulate == 0 && this.turnNumber % ModelUtils.IntRand(1, 7) == 0 && this.vaccineStage != Disease.EVaccineProgressStage.VACCINE_RELEASE)
    {
      this.influencePoints *= Mathf.Min(1f, Mathf.Max(0.7f, this.globalInfectedPerc / this.globalInfectedPercMAX));
      this.influencePoints /= 1f + this.manufactureSpeedAuthBonus;
      if (this.vaccineStage >= Disease.EVaccineProgressStage.VACCINE_RELEASE)
        this.influencePoints *= 0.5f;
      this.evoPoints += (int) Mathf.Max(1f, this.influencePoints);
      this.influencePoints = 0.0f;
    }
    this.infectedPercWeeklyChange.Push(this.globalInfectedPercent);
    this.deadPercWeeklyChange.Push(this.globalDeadPercent);
    this.authorityWeeklyChange.Push(this.authority);
    this.researchWeeklyChange.Push(this.globalCureResearch);
    float num10 = (float) (this.totalHealthyRecovered + this.totalDeadIntel);
    float totalDeadIntel = (float) this.totalDeadIntel;
    if ((double) totalDeadIntel > 0.0)
      this.estimatedDeathRate = Mathf.Min(1f, totalDeadIntel / num10);
    else
      this.estimatedDeathRate = 0.0f;
    this.totalInfectedIntelGUI = 0L;
    this.totalDeadIntelGUI = 0L;
    this.totalHealthyRecoveredIntelGUI = 0L;
    this.totalHealthyRecoveredGUI = 0L;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      this.totalHealthyRecoveredGUI += country.healthyRecoveredPopulation;
      if (localDisease.hasIntel)
      {
        this.totalInfectedIntelGUI += localDisease.infectedPopulation;
        this.totalHealthyRecoveredIntelGUI += country.healthyRecoveredPopulation;
        this.totalDeadIntelGUI += country.deadPopulation;
      }
    }
  }

  [GameEventFunction]
  public override void CreateInvestigationTeam()
  {
    Vampire vampire = Vampire.Create();
    vampire.actingDisease = (Disease) this;
    vampire.currentCountry = this.hqCountry;
    vampire.vampireType = Vampire.VampireType.VT_TEAM;
    vampire.currentPosition = new Vector3?();
    vampire.vampireHealth = 100f;
    if (this.vampHealthIncrease > 0)
      vampire.vampireHealthMax *= 1.1f;
    vampire.vampireHealthMax = vampire.vampireHealth;
    LocalDisease localDisease = (this.hqCountry ?? World.instance.countries[0]).GetLocalDisease((Disease) this);
    ++localDisease.vampireBirthCount;
    localDisease.hasTeam = true;
    this.vampires.Add(vampire);
    this.SetAbilityActive(EAbilityType.investigation_team.ToString());
    if (!CGameManager.IsTutorialGame || CGameManager.gameType != IGame.GameType.CureTutorial || !TutorialSystem.IsModuleComplete("C9") || TutorialSystem.IsModuleComplete("C10"))
      return;
    TutorialSystem.CheckModule((Func<bool>) (() => true), "C10");
  }

  [GameEventFunction]
  public override void ApplyCureGenes()
  {
    if (this.HasFlag(Disease.EGenericDiseaseFlag.GeneTechnicalOfficer))
      this.globalInfectiousnessMax -= this.globalInfectiousnessMax * 0.08f;
    if (this.HasFlag(Disease.EGenericDiseaseFlag.GeneConstructionManager))
      this.globalLethalityMax -= this.globalLethalityMax * 0.06f;
    if (!this.HasFlag(Disease.EGenericDiseaseFlag.CheatAdvancePlanning) && !this.HasCheat(Disease.ECheatType.SHUFFLE) && !this.HasFlag(Disease.EGenericDiseaseFlag.CheatCureShuffle))
      return;
    this.EvolveTech(this.GetTechnology("Build_Monitoring_Stations"), true);
  }

  [GameEventFunction]
  public override void CureAuthFallingPopup()
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial)
      return;
    string icon = "event_authority";
    int p = 3;
    string tagName1;
    string tagName2;
    switch (this.GetAuthorityLossReasons(0)[0].type)
    {
      case Disease.EAuthLoss.AUTH_LOSS_PANIC:
        tagName1 = "%s Panic hurts Authority";
        tagName2 = "Infected people are panicking about dying. Fund Response initiatives and lockdowns to reduce Infection";
        break;
      case Disease.EAuthLoss.AUTH_LOSS_DEATHS:
        tagName1 = "%s Fatality rate hurts Authority";
        tagName2 = "Too many dead people. Fund Response initiatives and lockdowns to reduce the Fatality rate";
        break;
      case Disease.EAuthLoss.AUTH_LOSS_SPREAD:
        tagName1 = "%s spread hurts Authority";
        tagName2 = "Too many countries infected. Use Quarantine measures to slow the spread";
        break;
      case Disease.EAuthLoss.AUTH_LOSS_COMPLIANCE:
        tagName1 = "Non-Compliance damaging Authority";
        tagName2 = "Too much Non-Compliance. Fund economic easing initiatives or disable Quarantine measures";
        break;
      default:
        return;
    }
    ParameterisedString parameterisedString = new ParameterisedString(CLocalisationManager.GetText(tagName1), new string[1]
    {
      "disease.name"
    });
    ParameterisedString message = new ParameterisedString(CLocalisationManager.GetText(tagName2));
    World.instance.popupMessages.Add(new PopupMessage(icon, parameterisedString, CGameManager.GetDisplayDate(), message, (Disease) this));
    World.instance.AddNewsItem(new IGame.NewsItem(parameterisedString, (Disease) this, (Country) null, p));
  }

  [GameEventFunction]
  public override void SetFungusInfectedFromCountry(Country c)
  {
    LocalDisease localDisease = this.GetLocalDisease(c);
    localDisease.infectedFromCountry = this.GetRandomInfectedCountry(c) ?? this.nexus;
    localDisease.infectionMethod = Country.EInfectionMethod.IM_FUNGALSPORE;
  }

  [GameEventFunction]
  public override void CureVirusWarnDanger()
  {
    foreach (Country country in World.instance.countries)
      this.GetLocalDisease(country).compliancePercMod += ModelUtils.FloatRand(0.09f, 0.11f);
  }

  [GameEventFunction]
  public override void CureComboCooperation()
  {
    foreach (Country country in World.instance.countries)
      this.GetLocalDisease(country).compliancePercMod += ModelUtils.FloatRand(0.01f, 0.05f);
  }

  [GameEventFunction]
  public override void CureComboSnitches()
  {
    foreach (Country country in World.instance.countries)
    {
      LocalDisease localDisease = this.GetLocalDisease(country);
      float num = ModelUtils.FloatRand(0.01f, 0.05f);
      localDisease.complianceMAX = Mathf.Min(localDisease.complianceMAX + num / 2f, 1f);
      localDisease.compliance = Mathf.Min(localDisease.compliance + num, localDisease.complianceMAX);
    }
  }

  public void GameUpdate_FakeNews()
  {
    this.diseaseData.mutatedThisTurn = false;
    World instance = World.instance;
    this.diseaseData.apeTotalPopulation = instance.apeTotalPopulation;
    if (this.diseaseData.evoPointsPrevTurn == this.evoPoints)
      ++this.diseaseData.numTurnsWithoutEvoChange;
    this.diseaseData.evoPointsPrevTurn = this.evoPoints;
    if (this.diseaseData.numTurnsWithoutEvoChange >= 3)
    {
      this.diseaseData.numTurnsWithoutEvoChange = 0;
      ++this.diseaseData.evoBoost;
    }
    if (this.infectedCountries.Count > 0 && this.diseaseData.fakeNewsStarted && instance.DiseaseTurn > 10 && this.diseaseData.numTurnsWithoutEvoChange > 0 && !this.diseaseData.dnaBubbleShowing)
    {
      bool flag = false;
      if (this.diseaseData.diseaseType == Disease.EDiseaseType.PARASITE && this.diseaseData.dnaPointsGained < 200)
        flag = (double) ModelUtils.IntRand(1, 150) < (double) (10 + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
      else if (this.diseaseType == Disease.EDiseaseType.NEURAX)
      {
        if (instance.DiseaseTurn - this.diseaseData.wormBubbleLastDay > 25)
          flag = (double) ModelUtils.IntRand(1, 100) < (double) (10 - this.diseaseData.numDNABubbles + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
      }
      else
        flag = (double) ModelUtils.IntRand(1, 100) < (double) (10 - this.diseaseData.numDNABubbles + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0;
      if (flag)
      {
        ++this.diseaseData.numDNABubbles;
        ++this.diseaseData.numDNABubblesWithoutTouch;
        this.diseaseData.dnaBubbleShowing = true;
        instance.AddBonusIcon(new BonusIcon((Disease) this, this.infectedCountries[UnityEngine.Random.Range(0, this.infectedCountries.Count)], BonusIcon.EBonusIconType.DNA));
        if (CGameManager.IsTutorialGame && TutorialSystem.IsModuleComplete("4B"))
          TutorialSystem.CheckModule((Func<bool>) (() => true), "5A");
      }
    }
    SPDiseaseExternal.DiseaseUpdate_FakeNews(this.diseaseData);
    if (instance.DiseaseTurn < 40 && this.diseaseData.totalInfected < 25L && (double) ModelUtils.IntRand(0, 100) < (double) Mathf.Min(10f, Mathf.Max(2f, this.diseaseData.nexusMinInfect + 2f)))
      this.nexus.TransferPopulation(1.0, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.INFECTED);
    long totalZombie = this.diseaseData.totalZombie;
    SPDiseaseExternal.DiseasePrepareTotals(this.diseaseData);
    this.infectedCountries = new List<Country>();
    this.apeLabCountries = new List<Country>();
    this.apeColonyCountries = new List<Country>();
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      SPDiseaseExternal.DiseaseCountryUpdate(this.diseaseData, ((SPCountry) country).mData, ((SPLocalDisease) localDisease).mData);
      if (localDisease.allInfected > 0L)
        this.infectedCountries.Add(country);
      if (country.hasApeLab)
        this.apeLabCountries.Add(country);
      if (country.hasApeColony)
        this.apeColonyCountries.Add(country);
    }
    this.diseaseData.infectedCountryCount = this.infectedCountries.Count;
    this.diseaseData.uninfectedCountryCount = World.instance.countries.Count - this.diseaseData.infectedCountryCount;
    this.diseaseData.apeTotalLabs = this.apeLabCountries.Count + World.instance.trackedLabPlanes.Count;
    this.diseaseData.apeTotalColonies = this.apeColonyCountries.Count;
    this.diseaseData.totalZombie += this.diseaseData.apeHordeStash;
    if (totalZombie >= this.diseaseData.totalZombie)
      ++this.diseaseData.zombieDecreaseTurnCount;
    else
      this.diseaseData.zombieDecreaseTurnCount = 0;
    SPDiseaseExternal.DiseasePostCountryUpdate_FakeNews(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
    this.diseaseData.hiZombifiedPopulation = 0.0f;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      localDisease.GameUpdate();
      World.instance.CheckGovernmentActions(country);
      this.diseaseData.hiZombifiedPopulation += localDisease.H2Z + localDisease.I2Z + localDisease.D2Z;
    }
    SPDiseaseExternal.DiseasePostLocalUpdate_FakeNews(this.diseaseData, World.instance.totalPopulation);
    if ((double) this.globalInfectedPercent > 0.40000000596046448 && (double) this.globalDeadPercent > 0.40000000596046448)
      World.instance.AddAchievement(EAchievement.A_onTheFence);
    long num1 = this.diseaseData.infectedThisTurn;
    if (num1 < 0L)
      num1 = 0L;
    this.diseaseData.averageInfected = (this.diseaseData.averageInfected * (double) (this.diseaseData.turnNumber - 1) + (double) num1) / (double) this.diseaseData.turnNumber;
    long num2 = this.diseaseData.deadThisTurn;
    if (num2 < 0L)
      num2 = 0L;
    this.diseaseData.averageDead = (this.diseaseData.averageDead * (double) (this.diseaseData.turnNumber - 1) + (double) num2) / (double) this.diseaseData.turnNumber;
    if (this == CNetworkManager.network.LocalPlayerInfo.disease)
    {
      if (this.infectedThisTurn >= 0L)
        this.accumulatedInfections += (float) this.infectedThisTurn;
      else if (this.cureFlag)
        this.accumulatedCures += (float) -this.infectedThisTurn;
      if (this.zombiesThisTurn > 0L && this.diseaseType == Disease.EDiseaseType.NECROA)
        this.accumulatedZombies += (float) this.zombiesThisTurn;
      if (this.infectedApesThisTurn > 0L && this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
        this.accumulatedIntelligentApes += (float) this.infectedApesThisTurn;
    }
    if (this.nukeRussia)
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("russia");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.nukeRussia = false;
    }
    if (!this.nukeChina)
      return;
    CountryView countryView1 = CInterfaceManager.instance.GetCountryView("china");
    countryView1.NukeStrikeEffect(countryView1.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
    this.nukeChina = false;
  }

  [GameEventFunction]
  public override void FakeNewsCreationConfirmation()
  {
    List<Technology> all1 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.themeQtyChange > 0));
    List<Technology> all2 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.mechanicsQtyChange > 0));
    List<Technology> all3 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.componentQtyChange > 0));
    List<Technology> all4 = this.technologies.FindAll((Predicate<Technology>) (a => this.IsTechEvolved(a.id) && a.playerQtyChange > 0));
    if (all1.Count <= 0 || all2.Count <= 0 || all3.Count <= 0 || all4.Count <= 0)
      return;
    ParameterisedString title = new ParameterisedString(CLocalisationManager.GetText("%s manifesto complete!"), new string[1]
    {
      "disease.name"
    });
    World.instance.popupMessages.Add(new PopupMessage("scenario_fake_news", title, CGameManager.GetDisplayDate(), new ParameterisedString(CLocalisationManager.GetText("%s\n%s\n%s\n%s"), new StringParameter[4]
    {
      new StringParameter(literal: CLocalisationManager.GetText(all1[0].name)),
      new StringParameter(literal: CLocalisationManager.GetText(all2[0].name)),
      new StringParameter(literal: CLocalisationManager.GetText(all3[0].name)),
      new StringParameter(literal: CLocalisationManager.GetText(all4[0].name))
    })
    {
      useLocalisation = false
    }, (Disease) this));
  }

  public void GameUpdate_Vampire()
  {
    World instance = World.instance;
    this.diseaseData.mutatedThisTurn = false;
    if (this.droneAttackFlag > 0)
    {
      if (this.lairDroneAttackTimer-- <= 0 && this.recentEventCounter >= 5 && this.totalZombie >= 0L)
      {
        LocalDisease localDisease1 = (LocalDisease) null;
        int num1 = 1;
        for (int index = 0; index < instance.countries.Count; ++index)
        {
          LocalDisease localDisease2 = this.GetLocalDisease(instance.countries[index]);
          if (localDisease2.HasCastle)
          {
            if ((double) ModelUtils.FloatRand(0.0f, 1f) < 1.0 / (double) num1 || localDisease1 == null)
              localDisease1 = localDisease2;
            ++num1;
          }
        }
        if (localDisease1 != null)
        {
          Country s = (Country) null;
          float num2 = -1f;
          for (int index = 0; index < instance.countries.Count; ++index)
          {
            if (instance.countries[index].fortState == EFortState.FORT_ALIVE)
            {
              float num3 = Vector3.Distance(instance.countries[index].fortPosition.Value, localDisease1.castlePosition.Value);
              if (s == null || (double) num3 < (double) num2)
              {
                s = instance.countries[index];
                num2 = num3;
              }
            }
          }
          if (s != null)
          {
            float attackDuration = (float) (20 - this.difficulty) + Mathf.Clamp((float) (100.0 - (double) (num2 * 0.07042254f) / 2.5), 0.0f, 15f) + (float) ModelUtils.IntRand(0, 3);
            if (this.vampLairDefenseBonus > 0)
              attackDuration *= 1.25f;
            Vehicle vehicle = Vehicle.Create();
            vehicle.type = Vehicle.EVehicleType.TargetingDrone;
            vehicle.subType = Vehicle.EVehicleSubType.TargetingDrone_Linear;
            vehicle.actingDisease = (Disease) this;
            vehicle.SetTargetingPath(s, localDisease1.country, s.fortPosition.Value, localDisease1.castlePosition.Value, 30f, attackDuration);
            s.localHCombatStrength -= 0.1f;
            World.instance.AddVehicle(vehicle);
          }
        }
        this.lairDroneAttackTimer = (int) ((double) Mathf.Max(15f, (float) (190.0 - ((double) this.vampireActivity + (double) this.difficultyVariable))) + (double) ModelUtils.IntRand(0, 20) + (double) (this.fortDestroyedNumber * 3) * (double) this.fortDifficultyModifier);
      }
    }
    if (this.diseaseData.evoPointsPrevTurn == this.evoPoints)
      ++this.diseaseData.numTurnsWithoutEvoChange;
    this.diseaseData.evoPointsPrevTurn = this.evoPoints;
    if (this.diseaseData.numTurnsWithoutEvoChange >= 3)
    {
      this.diseaseData.numTurnsWithoutEvoChange = 0;
      ++this.diseaseData.evoBoost;
    }
    if (this.infectedCountries.Count > 0 && instance.DiseaseTurn > 10 && this.diseaseData.numTurnsWithoutEvoChange > 0 && !this.diseaseData.dnaBubbleShowing && this.vday && (double) ModelUtils.IntRand(1, 100) < (double) (10 - this.diseaseData.numDNABubbles + this.diseaseData.evoBoost) + (double) this.diseaseData.cureCompletePercent * 10.0)
    {
      ++this.diseaseData.numDNABubbles;
      ++this.diseaseData.numDNABubblesWithoutTouch;
      this.diseaseData.dnaBubbleShowing = true;
      instance.AddBonusIcon(new BonusIcon((Disease) this, this.infectedCountries[ModelUtils.IntRand(0, this.infectedCountries.Count - 1)], BonusIcon.EBonusIconType.DNA));
      if (CGameManager.IsTutorialGame && TutorialSystem.IsModuleComplete("4B"))
        TutorialSystem.CheckModule((Func<bool>) (() => true), "5A");
    }
    SPDiseaseExternal.DiseaseUpdate_Vampire(this.diseaseData);
    float num4 = (float) (this.vampLabsCurrent + this.vampLabsDestroyed);
    if (this.recentEventCounter > 6 && (double) num4 < (double) this.maxVampLabs && this.vampLabWorking > 0)
    {
      if (--this.vampireLabLastspawn <= ModelUtils.IntRand(0, 5 + Mathf.FloorToInt(this.globalPriority / 4f)))
      {
        Country researchLabCountry = this.GetSuitableVampireResearchLabCountry();
        if (researchLabCountry != null)
        {
          Debug.Log((object) ("Vampire lab spawned in: " + researchLabCountry.id));
          researchLabCountry.ChangeApeLabStateF(EApeLabState.APE_LAB_INACTIVE);
        }
        this.vampireLabLastspawn = (int) ((double) (20 + this.vampLabsDestroyed) + ((double) num4 - (double) this.maxVampLabs) * 2.0 + (double) ModelUtils.IntRand(0, 35 + this.vampLabsCurrent + this.vampLabsCurrent));
      }
    }
    long totalZombie = this.diseaseData.totalZombie;
    SPDiseaseExternal.DiseasePrepareTotals_Vampire(this.diseaseData);
    this.infectedCountries = new List<Country>();
    this.apeLabCountries = new List<Country>();
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      SPDiseaseExternal.DiseaseCountryUpdate_Vampire(this.diseaseData, ((SPCountry) country).mData, ((SPLocalDisease) localDisease).mData);
      if (localDisease.allInfected > 0L)
        this.infectedCountries.Add(country);
      if (country.hasApeLab)
        this.apeLabCountries.Add(country);
    }
    this.diseaseData.infectedCountryCount = this.infectedCountries.Count;
    this.diseaseData.uninfectedCountryCount = World.instance.countries.Count - this.diseaseData.infectedCountryCount;
    this.diseaseData.apeTotalLabs = this.apeLabCountries.Count + World.instance.trackedLabPlanes.Count;
    this.diseaseData.apeTotalColonies = this.apeColonyCountries.Count;
    if (totalZombie >= this.diseaseData.totalZombie)
      ++this.diseaseData.zombieDecreaseTurnCount;
    else
      this.diseaseData.zombieDecreaseTurnCount = 0;
    SPDiseaseExternal.DiseasePostCountryUpdate_Vampire(this.diseaseData, World.instance.countries.Count, World.instance.totalPopulation);
    this.diseaseData.hiZombifiedPopulation = 0.0f;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = this.GetLocalDisease(country);
      localDisease.GameUpdate();
      World.instance.CheckGovernmentActions(country);
      this.diseaseData.hiZombifiedPopulation += localDisease.H2Z + localDisease.I2Z + localDisease.D2Z;
    }
    SPDiseaseExternal.DiseasePostLocalUpdate_Vampire(this.diseaseData, World.instance.totalPopulation);
    if (this.vday)
    {
      if (++this.vdayCounter > this.vdayLength && this.vdayDone)
      {
        this.vday = false;
        this.vcomAlert = 1f;
        this.fortSelectionDay = this.vdayCounter + ModelUtils.IntRand(30, 50) - this.difficulty * 3;
      }
    }
    else if ((double) this.vcomAlert > 0.0)
    {
      if (this.vdayCounter++ >= this.fortSelectionDay && (double) this.numberOfFortsToSpawn > 0.0 && this.recentEventCounter > 6)
      {
        int numberOfFortsToSpawn = (int) this.numberOfFortsToSpawn;
        Debug.LogWarning((object) ("VCOM SPAWNING FORTS! " + (object) numberOfFortsToSpawn));
        List<Country> countryList = new List<Country>();
        List<float> floatList = new List<float>();
        float max = 0.0f;
        for (int index = 0; index < World.instance.countries.Count; ++index)
        {
          Country country = World.instance.countries[index];
          if (country.fortState == EFortState.FORT_NONE && (this.shadowDayDone ? (double) country.healthyPercent : (double) country.healthyPercent + (double) country.infectedPercent) > 1.0 / 1000.0)
          {
            LocalDisease localDisease = this.GetLocalDisease(country);
            int f = (int) (country.apeLabStatus + 1);
            float num5 = (Mathf.Pow(5f, 2f) - Mathf.Pow((float) f, 2f)) * Mathf.Max(country.healthyPercent * 1f, 0.02f) * (1f - localDisease.deadPercent);
            if (localDisease.zombiePopulation > 0L)
              num5 *= 0.01f;
            if ((double) country.healthyPercent < 0.0099999997764825821)
              num5 *= 0.04f;
            if (country.poverty)
              num5 *= 0.5f;
            if (localDisease.HasCastle)
              num5 *= 0.1f;
            float num6 = num5 * 0.425263464f;
            countryList.Add(country);
            floatList.Add(num6);
            max += num6;
          }
        }
        bool flag = false;
        Debug.LogWarning((object) ("VCOM SPAWNING FORTS! " + (object) numberOfFortsToSpawn + " suitable: " + (object) countryList.Count));
        while (numberOfFortsToSpawn-- > 0 && countryList.Count > 0)
        {
          float num7 = ModelUtils.FloatRand(0.0f, max);
          int index1;
          for (index1 = -1; (double) num7 > 0.0 && index1 < floatList.Count - 1; num7 -= floatList[index1])
            ++index1;
          if (index1 < 0)
            index1 = 0;
          Country country = countryList[index1];
          countryList.RemoveAt(index1);
          max -= floatList[index1];
          floatList.RemoveAt(index1);
          if (country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE || country.apeLabStatus == EApeLabState.APE_LAB_INACTIVE)
            country.ChangeApeLabState = EApeLabState.APE_LAB_NONE;
          country.fortState = EFortState.FORT_ALIVE;
          LocalDisease localDisease = this.GetLocalDisease(country);
          localDisease.fortHealthMax = 100f;
          if (this.difficulty < 1)
            localDisease.fortHealthMax *= 0.75f;
          else if (this.difficulty < 2)
            localDisease.fortHealthMax *= 0.9f;
          localDisease.fortHealth = localDisease.fortHealthMax;
          ++this.numberOfFortsCreated;
          World.instance.AddFort(country);
          Debug.Log((object) ("Fort country selected: " + country.name));
          foreach (Country neighbour in country.neighbours)
          {
            int index2 = countryList.IndexOf(neighbour);
            if (index2 >= 0)
              floatList[index2] *= 0.5f;
          }
          flag = true;
        }
        int num8 = flag ? 1 : 0;
        this.daysUntilMinifortPlaneSpawn = ModelUtils.IntRand(12, 30);
        this.numberOfFortsToSpawn = 0.0f;
      }
    }
    if (this.shadowDay)
    {
      if (++this.shadowDayCounter > this.shadowDayLength)
      {
        this.shadowDay = false;
        this.shadowDayDone = true;
      }
    }
    this.fortDamageBonus += (float) (0.00800000037997961 + (double) this.difficulty * 0.0070000002160668373);
    if (this.infectedCountries.Count > 42 - this.difficulty && (double) this.vcomAlert < 1.0)
      this.globalVampireActivityBonus += 0.1f;
    if (this.infectedCountries.Count > 48 - this.difficulty && (double) this.vcomAlert < 1.0)
      this.globalVampireActivityBonus += 0.4f;
    this.globalBattleVictoryCount = 0.0f;
    if (World.instance.fortCountries.Count > 0)
    {
      this.numAliveForts = 0;
      for (int index3 = 0; index3 < World.instance.fortCountries.Count; ++index3)
      {
        Country fortCountry = World.instance.fortCountries[index3];
        bool flag = false;
        LocalDisease localDisease3 = this.GetLocalDisease(fortCountry);
        if (fortCountry.fortState == EFortState.FORT_ALIVE && (double) localDisease3.fortHealth <= 0.0)
        {
          fortCountry.fortState = EFortState.FORT_DESTROYED;
          fortCountry.fortWasDestroyed = true;
          localDisease3.consumeFlag = 0;
          localDisease3.fortHealth = 0.0f;
          this.fortDamageBonus += (float) (1.5 + (double) this.difficulty + (this.difficulty > 1 ? (double) this.fortDestroyedNumber : 0.0));
          if (this.difficulty > 2)
            this.fortDamageBonus += (float) (this.totalZombie - 1L);
          ++this.fortDestroyedNumber;
          localDisease3.localVampireActivity += 50f;
          int num9 = ModelUtils.IntRand(3, 6) + 3 - this.difficulty;
          if (this.difficulty < 1)
            num9 += 4;
          if (this.vampLabFortDnaBonus > 0)
            num9 += ModelUtils.IntRand(1, 2);
          this.evoPoints += num9;
          if (this.difficulty < 1)
            this.globalCureResearch *= 0.7f;
          else if (this.difficulty < 2)
            this.globalCureResearch *= 0.75f;
          else if (this.difficulty < 3)
            this.globalCureResearch *= 0.83f;
          else
            this.globalCureResearch *= 0.84f;
          flag = true;
          this.SpawnFortEscapePlanes(fortCountry, ModelUtils.IntRand(3, Mathf.Max(5, 9 - this.fortDestroyedNumber)));
        }
        else if (fortCountry.fortState == EFortState.FORT_ALIVE && ((this.shadowDay || this.shadowDayDone) && ((double) fortCountry.healthyPercent < 9.9999997473787516E-05 && ModelUtils.IntRand(0, 20) < 1 || fortCountry.healthyPopulation <= 0L) || (double) fortCountry.healthyPercent + (double) localDisease3.infectedPercent < 9.9999997473787516E-05 && ModelUtils.IntRand(0, 20) < 1 || fortCountry.healthyPopulation + localDisease3.infectedPopulation <= 0L))
        {
          fortCountry.fortState = EFortState.FORT_DESTROYED;
          fortCountry.fortWasDestroyed = true;
          localDisease3.fortHealth = 0.0f;
          localDisease3.consumeFlag = 0;
          this.fortDamageBonus += (float) (1.0 + (double) this.difficultyVariable * 0.5);
          ++this.fortDestroyedNumber;
          this.globalCureResearch *= 0.88f;
          int num10 = ModelUtils.IntRand(2, 4) + 3 - this.difficulty;
          if (this.difficulty < 1)
            num10 += 4;
          if (this.vampLabFortDnaBonus > 0)
            num10 += ModelUtils.IntRand(1, 2);
          this.evoPoints += num10;
          this.SpawnFortEscapePlanes(fortCountry, ModelUtils.IntRand(2, 3));
          flag = true;
        }
        if (flag && this.isVampire)
        {
          List<Vampire> vampires = this.GetVampires(fortCountry);
          for (int index4 = 0; index4 < vampires.Count; ++index4)
          {
            Vampire vampire = vampires[index4];
            if ((double) vampire.vampireHealth / (double) vampire.vampireHealthMax < 0.05000000074505806)
              World.instance.AddAchievement(EAchievement.A_luckofthedevil);
          }
          for (int index5 = 0; index5 < World.instance.countries.Count; ++index5)
          {
            Country country = World.instance.countries[index5];
            if (country.fortState == EFortState.FORT_ALIVE)
            {
              LocalDisease localDisease4 = this.GetLocalDisease(country);
              float num11 = localDisease4.fortHealth / localDisease4.fortHealthMax;
              localDisease4.fortHealthMax += 10f + this.difficultyVariable + (float) (this.totalZombie * 2L);
              localDisease4.fortHealth = localDisease4.fortHealthMax * num11;
              if (this.numberOfFortsCreated - this.fortDestroyedNumber <= 1)
              {
                localDisease4.fortHealthMax += (float) (10.0 + (double) this.difficultyVariable * 2.0) + (float) (3L * this.totalZombie);
                localDisease4.fortHealth = localDisease4.fortHealthMax * num11;
                this.fortDamageBonus += 1f + this.difficultyVariable;
              }
            }
          }
        }
        if (fortCountry.fortState == EFortState.FORT_ALIVE)
        {
          this.globalBattleVictoryCount += fortCountry.battleVictoryCount;
          ++this.numAliveForts;
        }
      }
      this.globalBattleVictoryCount /= (float) World.instance.fortCountries.Count * this.fortDifficultyModifier;
    }
    World.instance.UpdateDestroyedForts();
    if (World.instance.fortCountries.Count > 0 && (double) this.globalBattleVictoryCount >= 2.0)
    {
      List<Country> countryList = new List<Country>();
      for (int index = 0; index < World.instance.countries.Count; ++index)
      {
        Country country = World.instance.countries[index];
        LocalDisease localDisease = this.GetLocalDisease(country);
        if (country.fortState == EFortState.FORT_NONE && (double) country.healthyPercent > 0.5 && localDisease.zombiePopulation < 1L)
        {
          countryList.Add(country);
          countryList.Add(country);
          if (country.hot)
            countryList.Add(country);
          if (country.wealthy)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
          if ((double) country.healthyPercent > 0.60000002384185791)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
          if ((double) country.infectedPercent < 0.20000000298023224)
          {
            countryList.Add(country);
            countryList.Add(country);
          }
        }
      }
      if (countryList.Count > 0)
      {
        Country country = countryList[ModelUtils.IntRand(0, countryList.Count - 1)];
        float num12 = 0.0f;
        Country s = (Country) null;
        for (int index = 0; index < World.instance.fortCountries.Count; ++index)
        {
          Country fortCountry = World.instance.fortCountries[index];
          if (!fortCountry.fortWasDestroyed && (double) fortCountry.battleVictoryCount > (double) num12)
          {
            num12 = fortCountry.battleVictoryCount;
            s = fortCountry;
          }
        }
        if (s == null)
        {
          Debug.Log((object) "Would spawn a fort plane but no source fort countries available.");
        }
        else
        {
          bool flag = false;
          LocalDisease localDisease = this.GetLocalDisease(country);
          if (localDisease.infectedPopulation > 300L && (double) (country.healthyPopulation / localDisease.infectedPopulation) < 0.40000000596046448)
            flag = ModelUtils.IntRand(0, 100) < 10;
          else if (country.healthyPopulation < 1L)
            flag = true;
          int number = ModelUtils.IntRand(10, 250);
          Vehicle vehicle = Vehicle.Create();
          vehicle.type = Vehicle.EVehicleType.Airplane;
          vehicle.subType = Vehicle.EVehicleSubType.Fort;
          if (flag)
            vehicle.AddInfected((Disease) this, number);
          vehicle.SetRoute(s, country);
          vehicle.actingDisease = (Disease) this;
          s.fortPlaneSpawned = true;
          World.instance.AddVehicle(vehicle);
          for (int index = 0; index < World.instance.countries.Count; ++index)
            World.instance.countries[index].battleVictoryCount = 0.0f;
        }
      }
    }
    if (World.instance.fortCountries.Count > 1)
    {
      if (this.daysUntilMinifortPlaneSpawn-- == 0)
      {
        List<Country> countryList = new List<Country>();
        for (int index = 0; index < World.instance.fortCountries.Count; ++index)
        {
          Country fortCountry = World.instance.fortCountries[index];
          if (!fortCountry.fortWasDestroyed)
            countryList.Add(fortCountry);
        }
        if (countryList.Count < 2)
        {
          Debug.Log((object) "Would spawn a fort mini plane but no source or target fort countries available.");
          this.daysUntilMinifortPlaneSpawn = 10;
        }
        else
        {
          Country s = countryList[ModelUtils.IntRand(0, countryList.Count - 1)];
          countryList.Remove(s);
          Country d = countryList[ModelUtils.IntRand(0, countryList.Count - 1)];
          Vehicle vehicle = Vehicle.Create();
          vehicle.type = Vehicle.EVehicleType.Airplane;
          vehicle.subType = Vehicle.EVehicleSubType.MiniFort;
          vehicle.SetRoute(s, d);
          vehicle.actingDisease = (Disease) this;
          World.instance.AddVehicle(vehicle);
        }
      }
    }
    long num13 = this.diseaseData.infectedThisTurn;
    if (num13 < 0L)
      num13 = 0L;
    this.diseaseData.averageInfected = (this.diseaseData.averageInfected * (double) (this.diseaseData.turnNumber - 1) + (double) num13) / (double) this.diseaseData.turnNumber;
    long num14 = this.diseaseData.deadThisTurn;
    if (num14 < 0L)
      num14 = 0L;
    this.diseaseData.averageDead = (this.diseaseData.averageDead * (double) (this.diseaseData.turnNumber - 1) + (double) num14) / (double) this.diseaseData.turnNumber;
    if (this == CNetworkManager.network.LocalPlayerInfo.disease)
    {
      if (this.infectedThisTurn >= 0L)
        this.accumulatedInfections += (float) this.infectedThisTurn;
      else if (this.cureFlag)
        this.accumulatedCures += (float) -this.infectedThisTurn;
      if (this.zombiesThisTurn > 0L)
        this.accumulatedZombies += (float) this.zombiesThisTurn;
    }
    if (this.nukeRussia)
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("russia");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.nukeRussia = false;
    }
    if (this.nukeChina)
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("china");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.nukeChina = false;
    }
    this.weakestGlobalVampireHealth = 1f;
    for (int index = 0; index < this.vampires.Count; ++index)
    {
      Vampire vampire = this.vampires[index];
      if ((double) vampire.vampireHealth <= 0.0)
      {
        this.KillVampire(vampire);
        this.vampires.Remove(vampire);
        --index;
      }
      else
      {
        float num15 = vampire.vampireHealth / vampire.vampireHealthMax;
        if ((double) num15 < (double) this.weakestGlobalVampireHealth)
          this.weakestGlobalVampireHealth = num15;
      }
    }
  }

  private void KillVampire(Vampire v)
  {
    if (v.currentCountry == null)
      return;
    World.instance.destroyedVampires.Add(v);
    LocalDisease localDisease = v.currentCountry.GetLocalDisease((Disease) this);
    ++localDisease.vampireObituaryCount;
    ++localDisease.numberDeadVampires;
  }

  private void SpawnFortEscapePlanes(Country source, int numPlanes)
  {
    List<Country> countryList1 = new List<Country>();
    List<Country> countryList2 = new List<Country>();
    for (int index = 0; index < World.instance.fortCountries.Count; ++index)
    {
      Country fortCountry = World.instance.fortCountries[index];
      if (fortCountry.fortState == EFortState.FORT_ALIVE && fortCountry != source)
        countryList2.Add(fortCountry);
    }
    if (countryList2.Count <= 0)
      return;
    float num = 0.0f;
    for (int index = 0; index < numPlanes; ++index)
    {
      Country d = countryList2[ModelUtils.IntRand(0, countryList2.Count - 1)];
      Vehicle vehicle = Vehicle.Create();
      vehicle.type = Vehicle.EVehicleType.Airplane;
      vehicle.subType = Vehicle.EVehicleSubType.FortEscapees;
      vehicle.SetRoute(source, d);
      vehicle.actingDisease = (Disease) this;
      World.instance.AddVehicle(vehicle);
      if (countryList1.Count > 0 && d == countryList1[countryList1.Count - 1])
        ++num;
      vehicle.delay = num;
      countryList1.Add(d);
      num += 0.5f;
    }
  }

  [GameEventFunction]
  public override void CreateInitialVampire(Country c)
  {
    Vampire vampire1 = Vampire.Create();
    vampire1.currentCountry = c;
    vampire1.currentPosition = c.initialSpawnPosition;
    vampire1.vampireHealth = 100f;
    if (this.vampHealthIncrease > 0)
      vampire1.vampireHealthMax *= 1.1f;
    vampire1.vampireHealthMax = vampire1.vampireHealth;
    this.vampires.Add(vampire1);
    if (this.HasCheat(Disease.ECheatType.DOUBLE_STRAIN) && this.secondNexus != null)
    {
      if (this.secondNexus.healthyPopulation >= 1L)
        this.secondNexus.TransferPopulation(1.0, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.ZOMBIE);
      else if (this.GetLocalDisease(this.secondNexus).infectedPopulation >= 1L)
        this.secondNexus.TransferPopulation(1.0, Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.ZOMBIE);
      Vampire vampire2 = Vampire.Create();
      vampire2.currentCountry = this.secondNexus;
      vampire2.currentPosition = this.secondNexus.initialSpawnPosition;
      vampire2.vampireHealth = vampire1.vampireHealth;
      vampire2.vampireHealthMax = vampire1.vampireHealthMax;
      this.vampires.Add(vampire2);
    }
    CSoundManager.instance.PlaySFX("vampire_created");
  }

  [GameEventFunction]
  public override void SetVampireNexus(Country c)
  {
    if (this.nexus != null)
      this.nexus.diseaseNexus = (Disease) null;
    this.nexus = c;
    c.diseaseNexus = (Disease) this;
    if (c.hot)
      this.hot += 0.08f;
    else if (c.cold)
    {
      this.cold += 0.08f;
    }
    else
    {
      this.hot += 0.03f;
      this.cold += 0.03f;
    }
  }

  [GameEventFunction]
  public override void CreatePurityVampire(Country c)
  {
    Debug.LogWarning((object) "CREATING VAMPIRE!");
    this.purityEnabledFlag = 0;
    this.bonusPriority += 3f;
    this.vampireConversionPot -= (long) ((double) this.vampireConversionPotTrigger * (this.shadowDayDone ? 0.800000011920929 : 1.0));
    if (this.vampireConversionPot < 0L)
      this.vampireConversionPot = 0L;
    this.vampireConversionPotTrigger = (long) ((double) this.vampireConversionPotTrigger * 1.2000000476837158 * (double) ModelUtils.FloatRand(0.8f, 1.1f));
    this.DeEvolveTech(this.GetTechnology("purity_of_the_chosen"), true);
    Vampire vampire = Vampire.Create();
    vampire.currentCountry = c;
    vampire.currentPosition = new Vector3?();
    vampire.vampireHealth = this.difficulty < 2 ? 60f : 40f;
    vampire.vampireHealthMax = 100f;
    if (this.vampHealthIncrease > 0)
      vampire.vampireHealthMax *= 1.1f;
    this.vampires.Add(vampire);
    ++c.GetLocalDisease((Disease) this).vampireBirthCount;
    c.TransferPopulation(1.0, Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.ZOMBIE);
    CSoundManager.instance.PlaySFX("vampire_created");
  }

  [GameEventFunction]
  public override void ChooseVampireNarrativePath()
  {
    List<string> stringList = new List<string>()
    {
      "turkey",
      "central_european_states",
      "united_kingdom"
    };
    List<Country> countryList = new List<Country>();
    for (int index = 0; index < stringList.Count; ++index)
    {
      Country country = World.instance.GetCountry(stringList[index]);
      if (country.deadPopulation < 1L)
        countryList.Add(country);
    }
    if (countryList.Count <= 0)
      return;
    Country country1 = countryList[UnityEngine.Random.Range(0, countryList.Count)];
    this.vampireNarrativeStory = 1 + stringList.IndexOf(country1.id);
    country1.govLocalSeverity -= 3f;
  }

  [GameEventFunction]
  public override void CreateDraculaVampire(Country c)
  {
    LocalDisease localDisease = this.GetLocalDisease(c);
    Vampire vampire = Vampire.Create();
    vampire.currentCountry = c;
    vampire.currentPosition = new Vector3?();
    vampire.vampireHealth = 10f;
    vampire.vampireHealthMax = 100f;
    if (this.vampHealthIncrease > 0)
      vampire.vampireHealthMax *= 1.1f;
    this.vampires.Add(vampire);
    if (localDisease.infectedPopulation > 0L)
      c.TransferPopulation(1.0, Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.ZOMBIE);
    else
      c.TransferPopulation(1.0, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.ZOMBIE);
    CSoundManager.instance.PlaySFX("vampire_created");
  }

  [GameEventFunction]
  public override void TemplarNuke(Country c)
  {
    LocalDisease localDisease = this.GetLocalDisease(c);
    if (c.healthyPopulation > 1000L)
      c.TransferPopulation((double) c.healthyPopulation * (double) ModelUtils.FloatRand(1f / 1000f, 0.05f), Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.DEAD);
    if (localDisease.infectedPopulation > 1000L)
      c.TransferPopulation((double) c.healthyPopulation * (double) ModelUtils.FloatRand(1f / 1000f, 0.05f), Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.DEAD);
    if (localDisease.zombie <= 0L)
      return;
    List<Vampire> vampires = this.GetVampires(c);
    for (int index = 0; index < vampires.Count; ++index)
      vampires[index].vampireHealth = ModelUtils.FloatRand(0.2f, 0.7f) * vampires[index].vampireHealth;
  }

  [GameEventFunction]
  public override void CustomCreateVampire(Country c)
  {
    if (this.diseaseType != Disease.EDiseaseType.VAMPIRE)
      return;
    Debug.LogWarning((object) ("CREATING CUSTOM VAMPIRE in " + (object) c));
    LocalDisease localDisease = this.GetLocalDisease(c);
    if (localDisease.infectedPopulation > 0L)
      c.TransferPopulation(1.0, Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.ZOMBIE);
    else if (localDisease.healthyPopulation > 0L)
    {
      c.TransferPopulation(1.0, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.ZOMBIE);
    }
    else
    {
      Debug.LogError((object) "No population to turn into vampires!");
      return;
    }
    Vampire vampire = Vampire.Create();
    vampire.currentCountry = c;
    vampire.currentPosition = new Vector3?();
    vampire.vampireHealth = this.difficulty < 2 ? 60f : 40f;
    vampire.vampireHealthMax = 100f;
    if (this.vampHealthIncrease > 0)
      vampire.vampireHealthMax *= 1.1f;
    this.vampires.Add(vampire);
    ++localDisease.vampireBirthCount;
    CSoundManager.instance.PlaySFX("vampire_created");
  }

  [GameEventFunction]
  public override void CustomHealVampires(Country c, float f)
  {
    if (this.diseaseType != Disease.EDiseaseType.VAMPIRE)
      return;
    Debug.Log((object) ("HEAL VAMPIRE IN " + (object) c + " BY " + (object) f));
    foreach (Vampire vampire in this.GetVampires(c))
      vampire.vampireHealth = Mathf.Clamp(vampire.vampireHealth + f, 0.0f, vampire.vampireHealthMax);
  }

  [GameEventFunction]
  public override void CustomHealVampires(float f)
  {
    if (this.diseaseType != Disease.EDiseaseType.VAMPIRE)
      return;
    Debug.Log((object) ("HEAL ALL VAMPIRES BY " + (object) f));
    foreach (Vampire vampire in this.vampires)
      vampire.vampireHealth = Mathf.Clamp(vampire.vampireHealth + f, 0.0f, vampire.vampireHealthMax);
  }

  public override bool CanMutate(string tech)
  {
    return this.diseaseType != Disease.EDiseaseType.VAMPIRE || !(tech == "purity_of_the_chosen") && !(tech == "shadow_slaves");
  }

  [GameEventFunction]
  public override void NukeTwoCountries(string parameterCountry1, string parameterCountry2)
  {
    Debug.Log((object) ("Nuke! " + parameterCountry1));
    Debug.Log((object) ("Nuke! " + parameterCountry2));
    CountryView countryView1 = CInterfaceManager.instance.GetCountryView(parameterCountry1);
    countryView1.NukeStrikeEffect(countryView1.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
    CountryView countryView2 = CInterfaceManager.instance.GetCountryView(parameterCountry2);
    countryView2.NukeStrikeEffect(countryView2.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
  }

  [GameEventFunction]
  public override void SendPlane(
    string parameterCountryFrom,
    string parameterCountryTo,
    string parameterInfectedPopulation)
  {
    Country country1 = World.instance.GetCountry(parameterCountryFrom);
    Country country2 = World.instance.GetCountry(parameterCountryTo);
    long number = (long) int.Parse(parameterInfectedPopulation);
    Vehicle vehicle = Vehicle.Create();
    vehicle.delay = 0.0f;
    vehicle.type = Vehicle.EVehicleType.Airplane;
    vehicle.subType = Vehicle.EVehicleSubType.Unscheduled;
    vehicle.SetRoute(country1, country2);
    vehicle.actingDisease = (Disease) this;
    if (number > 0L)
      vehicle.AddInfected((Disease) this, (int) number);
    World.instance.AddVehicle(vehicle);
    ++((SPCountry) country2).mData.accumulatedFlight;
  }

  [GameEventFunction]
  public override void ForceCreateBonusIcon(
    string parameterCountry,
    string parameterBonusIconType,
    string parameterBonusEvo,
    string parameterIgnoreOtherEffect)
  {
    Country country = World.instance.GetCountry(parameterCountry);
    int extraDNA = int.Parse(parameterBonusEvo);
    bool onlyDNA = false;
    if (parameterIgnoreOtherEffect.ToLower().Equals("true"))
      onlyDNA = true;
    if (parameterIgnoreOtherEffect.ToLower().Equals("false"))
      onlyDNA = false;
    BonusIcon.EBonusIconType bonusIconType = BonusIcon.EBonusIconType.DNA;
    if (parameterBonusIconType.ToLower().Equals("orangae") || parameterBonusIconType.ToLower().Equals("dna") || parameterBonusIconType.ToLower().Equals("yellow"))
      bonusIconType = BonusIcon.EBonusIconType.DNA;
    if (parameterBonusIconType.ToLower().Equals("red") || parameterBonusIconType.ToLower().Equals("infect"))
      bonusIconType = BonusIcon.EBonusIconType.INFECT;
    if (parameterBonusIconType.ToLower().Equals("blue") || parameterBonusIconType.ToLower().Equals("cure"))
      bonusIconType = BonusIcon.EBonusIconType.CURE;
    if (parameterBonusIconType.ToLower().Equals("black") || parameterBonusIconType.ToLower().Equals("death"))
      bonusIconType = BonusIcon.EBonusIconType.DEATH;
    if (parameterBonusIconType.ToLower().Equals("neurax") || parameterBonusIconType.ToLower().Equals("trojan"))
      bonusIconType = BonusIcon.EBonusIconType.NEURAX;
    World.instance.AddBonusIcon(new BonusIcon((Disease) this, country, bonusIconType, extraDNA, onlyDNA));
  }

  public IEnumerator DownloadAndInstallScenario(
    Uri uri,
    string downloadFileName,
    string scenarioName = "default scenario")
  {
    using (UnityWebRequest downloader = UnityWebRequest.Get(uri))
    {
      Debug.Log((object) ("Download scenario " + scenarioName));
      downloader.downloadHandler = (DownloadHandler) new DownloadHandlerFile(downloadFileName);
      downloader.SendWebRequest();
      while (!downloader.isDone)
        yield return (object) null;
      if (downloader.error != null)
      {
        Debug.LogError((object) downloader.error);
      }
      else
      {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Creator" + Path.DirectorySeparatorChar.ToString() + "PIFSL " + scenarioName + "/";
        Directory.CreateDirectory(downloadFileName + "cache");
        Debug.Log((object) ("Trying to install scenario " + scenarioName));
        if (!Directory.Exists(path))
          ZipFile.ExtractToDirectory(downloadFileName, path, true);
        else
          Debug.Log((object) ("Scenario " + scenarioName + " already installed, skip installing process"));
        CGameManager.canPlaySFX = false;
        CGameManager.ClearGame();
        DynamicMusic.instance.FadeOut();
        Scenario scenario = Scenario.LoadScenario("PIFSL " + scenarioName, false, false, Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), "PIFSL " + scenarioName));
        scenario.scenarioInformation.id = "PIFSL " + scenarioName;
        CGameManager.gameType = IGame.GameType.Custom;
        CGameManager.CreateGame(scenario);
      }
    }
  }

  public void PerformAbnormalScenario(string scenarioName)
  {
    string uriString = CGameManager.federalServerAddress + "AbnormalScenarios/" + scenarioName + ".zip";
    string downloadFileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Federal Scenario Cache" + Path.DirectorySeparatorChar.ToString() + scenarioName + ".plagueinc";
    Debug.Log((object) ("Download Abnormal Scenario " + scenarioName + ", Arguments " + uriString + " " + downloadFileName + " " + scenarioName));
    CInterfaceManager.instance.StartCoroutine(this.DownloadAndInstallScenario(new Uri(uriString), downloadFileName, scenarioName));
  }

  public bool CheckScenarioExist(string scenarioName)
  {
    return Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Creator" + Path.DirectorySeparatorChar.ToString() + ("PIFSL " + scenarioName));
  }

  public void CheckZombieScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || this.turnNumber > 180 || this.diseaseType != Disease.EDiseaseType.NECROA || this.totalZombie < 170000L || (double) this.cureCompletePerc >= 0.5 || !CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.ToLower().Contains("everyone"))
      return;
    if (!this.CheckScenarioExist("L4D"))
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.zombieScenarioChecked = true;
      this.abnormalCheckedDay = this.turnNumber;
    }
    else
      Debug.Log((object) "L4D already installed");
  }

  public void GetConstList()
  {
    CGameManager.scenarioConstList = new Dictionary<string, int>();
    CGameManager.constedScenarioList = new List<string>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "newConstList.txt"))
    {
      Debug.Log((object) "Const List file not found, skip!");
    }
    else
    {
      string str1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "newConstList.txt");
      CGameManager.scenarioConstList = new Dictionary<string, int>();
      CGameManager.constedScenarioList = new List<string>();
      string[] strArray1 = str1.Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str2 in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str2.Split(chArray);
          string key = "PIFSL " + strArray2[0];
          int num1 = int.Parse(strArray2[1]);
          double num2 = (double) num1 / 10.0;
          CGameManager.scenarioConstList.Add(key, num1);
          CGameManager.constedScenarioList.Add(key);
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong const list, dumbass");
    }
  }

  public static void GetConstListExternal()
  {
    CGameManager.scenarioConstList = new Dictionary<string, int>();
    CGameManager.constedScenarioList = new List<string>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "newConstList.txt"))
    {
      Debug.Log((object) "No Const List File found, skip!");
    }
    else
    {
      string str1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "newConstList.txt");
      CGameManager.scenarioConstList = new Dictionary<string, int>();
      CGameManager.constedScenarioList = new List<string>();
      string[] strArray1 = str1.Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str2 in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str2.Split(chArray);
          string key = "PIFSL " + strArray2[0];
          int num1 = int.Parse(strArray2[1]);
          double num2 = (double) num1 / 10.0;
          CGameManager.scenarioConstList.Add(key, num1);
          CGameManager.constedScenarioList.Add(key);
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong const list, dumbass");
    }
  }

  public void CheckParasiteScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || this.turnNumber > 550 || this.turnNumber < 10 || this.diseaseType != Disease.EDiseaseType.PARASITE || this.totalInfected + this.totalHealthy >= 17600L || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("铁丝线虫") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || this.IsTechEvolved("faedb921-31d9-4fbf-9526-a6abf158dbc9") || this.IsTechEvolved("7a629190-645f-49b4-bf2c-f537fb9e3e94") || this.IsTechEvolved("739e05e0-3bb1-4453-b7cc-a0753744a803"))
      return;
    if (!this.CheckScenarioExist("176关 铁线虫入侵（极恶）b"))
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.parasiteScenarioChecked = true;
      this.abnormalCheckedDay = this.turnNumber;
    }
    else
      Debug.Log((object) "176关 铁线虫入侵（极恶）b already installed");
  }

  public void ApplyDreamParasiteTransformation()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || !CGameManager.game.CurrentLoadedScenario.filename.Contains("DreamParasiteBYD") || (double) this.customGlobalVariable1 < 100.0)
      return;
    foreach (Country country in World.instance.countries)
    {
      country.GetLocalDisease((Disease) this).H2I = 0.0f;
      long number = country.healthyPopulation - country.totalInfected;
      if (number + 10L >= country.currentPopulation)
        number += 10L;
      if (-1L * number + 10L >= country.currentPopulation)
        number -= 10L;
      country.TransferPopulation((double) number, Country.EPopulationType.HEALTHY, (Disease) this, Country.EPopulationType.INFECTED);
    }
    this.customGlobalVariable1 = 0.0f;
    Debug.Log((object) "Dream Parasite Transformation applied!");
  }

  public static void GetUnlockScoreExternal()
  {
    CGameManager.scenarioUnlockConditionOverride = new Dictionary<string, List<CGameManager.ScenarioUnlockCondition>>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockScore.txt"))
    {
      Debug.Log((object) "No Unlock Score File found, skip!");
    }
    else
    {
      string[] strArray1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockScore.txt").Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str.Split(chArray);
          string key = "PIFSL " + strArray2[0];
          List<CGameManager.ScenarioUnlockCondition> scenarioUnlockConditionList = new List<CGameManager.ScenarioUnlockCondition>();
          for (int index = 0; index < (strArray2.Length - 1) / 2; ++index)
          {
            CGameManager.ScenarioUnlockCondition scenarioUnlockCondition = new CGameManager.ScenarioUnlockCondition("PIFSL " + strArray2[2 * index + 1], int.Parse(strArray2[2 * index + 2]));
            scenarioUnlockConditionList.Add(scenarioUnlockCondition);
          }
          CGameManager.scenarioUnlockConditionOverride.Add(key, scenarioUnlockConditionList);
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong unlock score list, dumbass");
    }
  }

  public IEnumerator DownloadAndPlayVideo(
    Uri uri,
    string downloadFileName,
    string videoName,
    string scenarioName = "default scenario")
  {
    using (UnityWebRequest downloader = UnityWebRequest.Get(uri))
    {
      Debug.Log((object) ("Download scenario " + scenarioName));
      downloader.downloadHandler = (DownloadHandler) new DownloadHandlerFile(downloadFileName);
      downloader.SendWebRequest();
      while (!downloader.isDone)
        yield return (object) null;
      if (downloader.error != null)
      {
        Debug.LogError((object) downloader.error);
      }
      else
      {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Creator" + Path.DirectorySeparatorChar.ToString() + "PIFSL " + scenarioName + "/";
        Directory.CreateDirectory(downloadFileName + "cache");
        Debug.Log((object) ("Trying to install scenario " + scenarioName));
        if (!Directory.Exists(path))
          ZipFile.ExtractToDirectory(downloadFileName, path, true);
        else
          Debug.Log((object) ("Scenario " + scenarioName + " already installed, skip installing process"));
        CGameManager.pendingScenarioName = scenarioName;
        IGame.PlayScenarioVideo(videoName);
      }
    }
  }

  public void PerformAbnormalScenario(string scenarioName, string videoName)
  {
    string uriString = CGameManager.federalServerAddress + "AbnormalScenarios/" + scenarioName + ".zip";
    string downloadFileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Federal Scenario Cache" + Path.DirectorySeparatorChar.ToString() + scenarioName + ".plagueinc";
    Debug.Log((object) ("Download Abnormal Scenario " + scenarioName + ", Arguments " + uriString + " " + downloadFileName + " " + scenarioName));
    CInterfaceManager.instance.StartCoroutine(this.DownloadAndPlayVideo(new Uri(uriString), downloadFileName, videoName, scenarioName));
  }

  public void CheckDreamScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || this.diseaseType != Disease.EDiseaseType.PARASITE || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("DreamParasite") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || CGameManager.game.CurrentLoadedScenario.filename.Contains("BYD") || CSLocalUGCHandler.GetScenarioHighScore("PIFSL DreamParasite") < 65000)
      return;
    foreach (Technology technology in this.technologies)
    {
      if (technology.gridType != Technology.ETechType.symptom && !this.IsTechEvolved(technology))
        return;
    }
    CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
    countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
    this.dreamScenarioChecked = true;
    this.abnormalCheckedDay = this.turnNumber;
  }

  public void CheckHellScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || this.turnNumber < 1000 || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("Level 7777 乌托邦") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || !World.instance.firedEvents.ContainsKey("奢求无度"))
      return;
    if (!this.CheckScenarioExist("Level6666 炼狱（双盘吸虫 极恶）a"))
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.hellScenarioChecked = true;
      this.abnormalCheckedDay = this.turnNumber;
      this.daysToGameWin = 114514;
    }
    else
      Debug.Log((object) "Level6666 炼狱（双盘吸虫 极恶）a already installed");
  }

  private void DiseaseClockUpdate()
  {
    if ((double) this.customGlobalVariable3 >= 2.994999885559082 || (double) this.customGlobalVariable4 >= 27.0)
    {
      if (!CGameManager.game.IsReplayActive)
        this.ShowHUD();
      this.cureFlag = true;
      this.globalCureResearch += this.cureRequirements;
    }
    else
    {
      if (CGameManager.spaceTime && !CGameManager.game.IsReplayActive)
      {
        CSoundManager.instance.PlaySFX("pause_count");
        CHUDScreen.instance.SetActive(false);
        CGameManager.spaceTimeRate = 1;
        CGameManager.game.SetSpeed(CGameManager.spaceTimeRate > 0 ? CGameManager.spaceTimeRate : 1);
      }
      if (this.IsTechEvolved("37a8a34a-c0b8-41ba-83a3-235bfec4c7dd") && (double) World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
        World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
      if (this.IsTechEvolved("d25146b7-cbb3-41f6-bf79-994577252c4f") && (double) World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
        World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
      if (this.IsTechEvolved("93705f60-34c4-4f4a-a170-e11a9a481e4b") && (double) World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
        World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
      if (this.IsTechEvolved("3ec63ed2-a675-423f-90ef-53d76559ccae") && (double) World.instance.GetCountry("balcan_states").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
      {
        World.instance.GetCountry("balcan_states").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
        this.globalLethalityMax -= 1000f;
        this.globalSeverityMax += this.customGlobalVariable1 + 30f - this.globalSeverityMax;
      }
      if (this.IsTechEvolved("0f941ebe-ceb5-4a1a-87e1-f7abc5d5b473"))
      {
        foreach (Country country in World.instance.countries)
        {
          if ((double) country.GetLocalDisease((Disease) this).customLocalVariable2 <= 0.5 && (double) country.deadPercent > 0.44999998807907104)
          {
            country.GetLocalDisease((Disease) this).customLocalVariable2 = 1f;
            this.ForceCreateBonusIcon(country.id, "BLUE", "0", "false");
          }
        }
      }
      if (this.IsTechEvolved("0c94b679-333d-425e-b86e-91998e61424b") && (double) World.instance.GetCountry("australia").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
      {
        World.instance.GetCountry("australia").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
        this.globalInfectiousnessTopMultipler += 0.045f;
        this.globalInfectiousnessBottomValue += 0.32f;
        this.globalLethalityTopMultipler += 7f / 400f;
        this.globalLethalityBottomValue += 0.125f;
      }
      if (this.IsTechEvolved("24ab282d-efce-46a8-9bd1-d83243619e64") && (double) World.instance.GetCountry("argentina").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
      {
        World.instance.GetCountry("argentina").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
        this.evoPoints += 15;
      }
      if (this.IsTechEvolved("ec0ea2f0-54d1-4a86-bc04-eb73c2950544") && (double) World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 <= 1.0)
        World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 = 30f;
      if (this.IsTechEvolved("1661868e-52bb-424e-b8b3-9cc1fca2d81b") && (double) World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 <= 1.0)
        World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 = 30f;
      if ((double) this.customGlobalVariable2 <= 0.10000000149011612)
      {
        this.customGlobalVariable4 = 0.0f;
        if ((double) this.cureBaseMultiplier < 0.45)
          this.globalSeverityMax -= ModelUtils.FloatRand(1.32f, 1.75f);
        else
          this.globalSeverityMax -= ModelUtils.FloatRand(0.95f, 1.22f);
        if ((double) this.globalInfectedPercent > 0.15000000596046448 && (double) World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6 <= 16.0)
        {
          if (World.instance.GetCountry("usa").apeLabStatus != EApeLabState.APE_LAB_ACTIVE)
            World.instance.GetCountry("usa").ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
          this.globalInfectiousnessMax -= ModelUtils.FloatRand(4.75f, 8.75f);
          if ((double) World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6 >= 0.5)
          {
            ++World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6;
            if ((double) World.instance.GetCountry("bolivia").GetLocalDisease((Disease) this).customLocalVariable6 >= 15.899999618530273 && World.instance.GetCountry("usa").apeLabStatus == EApeLabState.APE_LAB_ACTIVE)
              World.instance.GetCountry("usa").ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
          }
        }
        if ((double) this.globalInfectedPercent > 0.30000001192092896 && (double) World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6 <= 16.0)
        {
          if (World.instance.GetCountry("china").apeLabStatus != EApeLabState.APE_LAB_ACTIVE)
            World.instance.GetCountry("china").ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
          this.cureBaseMultiplier -= ModelUtils.FloatRand(0.0265f, 0.0315f);
          if ((double) World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6 >= 0.5)
          {
            ++World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6;
            if ((double) World.instance.GetCountry("botswana").GetLocalDisease((Disease) this).customLocalVariable6 >= 15.899999618530273 && World.instance.GetCountry("china").apeLabStatus == EApeLabState.APE_LAB_ACTIVE)
              World.instance.GetCountry("china").ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
          }
        }
        if ((double) this.globalInfectedPercent > 0.44999998807907104 && (double) World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6 <= 16.0)
        {
          if (World.instance.GetCountry("russia").apeLabStatus != EApeLabState.APE_LAB_ACTIVE)
            World.instance.GetCountry("russia").ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
          this.corpseTransmission -= ModelUtils.FloatRand(2.63f, 4.27f);
          if ((double) World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6 >= 0.5)
          {
            ++World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6;
            if ((double) World.instance.GetCountry("brazil").GetLocalDisease((Disease) this).customLocalVariable6 >= 15.899999618530273 && World.instance.GetCountry("russia").apeLabStatus == EApeLabState.APE_LAB_ACTIVE)
              World.instance.GetCountry("russia").ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
          }
        }
        if (this.IsTechEvolved("73531b2e-bab9-483e-a10c-16c2e39e398e"))
          this.globalSeverityMax -= ModelUtils.FloatRand(0.515f, 0.68f);
        if (this.IsTechEvolved("ac8b4c1c-946d-4c63-89f6-4faa299327d4"))
          this.globalSeverityMax -= ModelUtils.FloatRand(0.735f, 0.89f);
        if (this.IsTechEvolved("15981109-184a-469e-9ffc-48637e1db1c1"))
          this.globalLethalityMax += (this.customGlobalVariable1 - 50f) / ModelUtils.FloatRand(75f, 145f);
        if (this.IsTechEvolved("862360cd-54da-45d6-b476-e6a71b7c17e4"))
          this.globalLethalityMax += (this.globalSeverityMax - 65f) / ModelUtils.FloatRand(40f, 67.5f);
        if (this.IsTechEvolved("c8842358-2f25-4a0c-94f4-ba0cc823c243") && (double) this.globalSeverityMax < 25.0)
        {
          this.globalInfectiousnessMax += ModelUtils.FloatRand(1.245f, 1.625f);
          this.globalLethalityMax -= ModelUtils.FloatRand(1.125f, 2.75f);
        }
        if (this.IsTechEvolved("d79fd24f-dfa4-41cd-b5e8-6e1579a27053"))
          this.globalInfectiousnessMax += this.customGlobalVariable1 / ModelUtils.FloatRand(160f, 215f);
        if ((double) World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 >= 20.0)
          ++World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6;
        if ((double) World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 >= 20.0)
          ++World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6;
        if ((double) World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 >= 89.5 && (double) World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 <= 90.5 || (double) this.globalInfectedPercent + (double) this.globalDeadPercent >= 0.89999997615814209)
        {
          World.instance.GetCountry("algeria").GetLocalDisease((Disease) this).customLocalVariable6 = 200f;
          this.globalInfectiousnessMax -= 36f;
        }
        if ((double) World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 >= 89.5 && (double) World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 <= 90.5 || (double) this.globalInfectedPercent + (double) this.globalDeadPercent >= 0.89999997615814209)
        {
          World.instance.GetCountry("angola").GetLocalDisease((Disease) this).customLocalVariable6 = 200f;
          this.globalLethalityMax -= 36f;
        }
        if (this.IsTechEvolved("2b4afd40-a1cb-4a0b-b27f-ac4edddba03f") && (double) this.customGlobalVariable1 >= 0.0 && (double) this.customGlobalVariable1 <= 30.049999237060547)
          this.cold += ModelUtils.FloatRand(0.0145f, 0.0189f);
        if (this.IsTechEvolved("5c6760cf-4ec9-4ed2-aac8-0f94d77abec5") && (double) this.customGlobalVariable1 >= 14.5 && (double) this.customGlobalVariable1 <= 45.5)
          this.rural += ModelUtils.FloatRand(0.0198f, 0.0215f);
        if (this.IsTechEvolved("a6dbeb3e-cf64-4087-a26e-abc1edea2916") && (double) this.customGlobalVariable1 >= 29.5 && (double) this.customGlobalVariable1 <= 60.5)
          this.poverty += ModelUtils.FloatRand(7f / 400f, 0.0238f);
        if (this.IsTechEvolved("ea677471-38d4-4eea-a69d-bdfc756bb4b6") && (double) this.customGlobalVariable1 >= 44.5 && (double) this.customGlobalVariable1 <= 75.5)
          this.hot += ModelUtils.FloatRand(0.0123f, 0.0164f);
        if (this.IsTechEvolved("adc13789-ca20-4c44-a2ee-4d29f21e4155") && (double) this.customGlobalVariable1 >= 59.5 && (double) this.customGlobalVariable1 <= 90.5)
          this.wealthy += ModelUtils.FloatRand(0.0107f, 0.0138f);
        if (this.IsTechEvolved("368bcd23-7fa1-4ba9-b36a-fac6fa816938") && (double) this.customGlobalVariable1 >= 74.5 && (double) this.customGlobalVariable1 <= 105.5)
          this.urban += ModelUtils.FloatRand(7f / 400f, 0.0215f);
      }
      if (this.IsTechEvolved("31292905-97af-47c9-90df-1c3172e1a12a"))
      {
        ++this.customGlobalVariable1;
        if ((double) World.instance.GetCountry("afghanistan").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5)
        {
          World.instance.GetCountry("afghanistan").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
          this.globalInfectiousnessTopMultipler += 0.074f;
          this.globalInfectiousnessBottomValue += 0.85f;
          this.globalLethalityTopMultipler += 0.032f;
          this.globalLethalityBottomValue += 0.27f;
          this.evoPoints += 18;
          foreach (Country country in World.instance.countries)
          {
            if (country.hasAirport)
              country.airportStatus = true;
            if (country.hasPorts)
              country.portStatus = true;
          }
        }
      }
      this.globalCureResearch = this.customGlobalVariable1 / 100f * this.cureRequirements;
      this.cureBaseMultiplier -= ModelUtils.FloatRand(0.0042f, 0.005f);
      if ((double) this.cureBaseMultiplier < 0.85000002384185791)
        World.instance.GetCountry("greenland").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
      if ((double) this.cureBaseMultiplier < 0.75)
      {
        World.instance.GetCountry("iceland").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
        if (this.IsTechEvolved("64a8d6fc-a6e4-4c05-bcee-43ddd88bfa3e"))
          this.geneticDriftMax = (float) (1.0 + 1.5499999523162842 * Math.Pow(1.0 - (double) this.cureBaseMultiplier, 2.0));
        else
          this.geneticDriftMax = (float) (1.0 + 4.0 * Math.Pow(1.0 - (double) this.cureBaseMultiplier, 2.0));
        this.geneticDriftMax /= (float) (1.0 + (double) this.globalInfectedPercent * (double) this.globalInfectedPercent);
      }
      else
      {
        World.instance.GetCountry("iceland").GetLocalDisease((Disease) this).customLocalVariable6 = 0.0f;
        this.geneticDriftMax = 1f;
      }
      if (this.cureFlag)
      {
        if ((double) World.instance.GetCountry("china").GetLocalDisease((Disease) this).customLocalVariable6 <= 0.5 && !CGameManager.game.IsReplayActive)
          this.HideHUD();
        World.instance.GetCountry("china").GetLocalDisease((Disease) this).customLocalVariable6 = 1f;
        if ((double) World.instance.GetCountry("india").GetLocalDisease((Disease) this).customLocalVariable6 > 19.950000762939453 && (double) World.instance.GetCountry("india").GetLocalDisease((Disease) this).customLocalVariable6 < 20.049999237060547 && CSLocalUGCHandler.GetScenarioHighScore("PIFSL 时生虫ReCRAFTa") > 1)
        {
          this.customGlobalVariable1 = 0.0f;
          if (!CGameManager.game.IsReplayActive && (double) this.customGlobalVariable2 >= 0.10000000149011612 && (double) this.customGlobalVariable1 > (double) this.globalSeverityMax)
            this.ShowHUD();
        }
        ++World.instance.GetCountry("india").GetLocalDisease((Disease) this).customLocalVariable6;
      }
      if ((double) this.customGlobalVariable2 >= 0.39950001239776611 && (double) this.customGlobalVariable2 <= 0.40049999952316284)
      {
        if (!CGameManager.game.IsReplayActive)
          this.HideHUD();
        this.customGlobalVariable2 = 1f;
      }
      if ((double) this.customGlobalVariable2 >= 0.2994999885559082 && (double) this.customGlobalVariable2 <= 0.30050000548362732)
        this.customGlobalVariable2 = 0.4f;
      if ((double) this.customGlobalVariable2 <= 0.20000000298023224 && (double) this.globalSeverityMax - (double) this.customGlobalVariable1 > 30.0)
      {
        foreach (Country country in World.instance.countries)
        {
          if (country.hasAirport)
            country.airportStatus = false;
          if (country.hasPorts)
            country.portStatus = false;
        }
        World.instance.GetCountry("france").GetLocalDisease((Disease) this).customLocalVariable6 += 0.6666667f * this.globalInfectiousnessMax;
        World.instance.GetCountry("germany").GetLocalDisease((Disease) this).customLocalVariable6 += 0.6666667f * this.globalLethalityMax;
        this.globalInfectiousnessMax *= 0.333333343f;
        this.globalLethalityMax *= 0.333333343f;
        this.customGlobalVariable2 = 0.3f;
        ++this.customGlobalVariable3;
        World.instance.GetCountry("sweden").GetLocalDisease((Disease) this).customLocalVariable6 = this.globalInfectiousnessTopMultipler;
        World.instance.GetCountry("finland").GetLocalDisease((Disease) this).customLocalVariable6 = this.globalInfectiousnessBottomValue;
        World.instance.GetCountry("norway").GetLocalDisease((Disease) this).customLocalVariable6 = this.globalLethalityTopMultipler;
        World.instance.GetCountry("russia").GetLocalDisease((Disease) this).customLocalVariable6 = this.globalLethalityBottomValue;
        this.globalInfectiousnessBottomValue = 0.0f;
        this.globalLethalityBottomValue = 0.0f;
        this.globalSeverityBottomValue = 0.0f;
        this.globalInfectiousnessTopMultipler = 0.0f;
        this.globalLethalityTopMultipler = 0.0f;
        this.globalSeverityTopMultipler = 0.0f;
      }
      if ((double) this.customGlobalVariable2 >= 0.10000000149011612)
      {
        ++this.customGlobalVariable4;
        --this.globalSeverityMax;
        this.evoPoints = 0;
        if (this.IsTechEvolved("e2633238-6e2b-4c11-a92e-bb904d650600"))
          this.globalLethalityMax += ModelUtils.FloatRand(0.525f, 0.775f);
        if (this.IsTechEvolved("7f3b7473-35fb-459a-a2d1-e3388d926e12"))
          this.globalSeverityMax -= ModelUtils.FloatRand(0.515f, 0.68f);
        if (this.IsTechEvolved("2736b3e0-ccec-475b-9e63-2f311745df18"))
          this.globalSeverityMax -= ModelUtils.FloatRand(0.735f, 0.89f);
      }
      if ((double) this.customGlobalVariable2 >= 0.10000000149011612 && (double) this.customGlobalVariable1 > (double) this.globalSeverityMax)
      {
        foreach (Country country in World.instance.countries)
        {
          if (country.hasAirport)
            country.airportStatus = true;
          if (country.hasPorts)
            country.portStatus = true;
        }
        this.globalInfectiousnessMax += World.instance.GetCountry("france").GetLocalDisease((Disease) this).customLocalVariable6;
        World.instance.GetCountry("france").GetLocalDisease((Disease) this).customLocalVariable6 = 0.0f;
        this.globalLethalityMax += World.instance.GetCountry("germany").GetLocalDisease((Disease) this).customLocalVariable6;
        World.instance.GetCountry("germany").GetLocalDisease((Disease) this).customLocalVariable6 = 0.0f;
        this.customGlobalVariable2 = 0.0f;
        this.globalInfectiousnessBottomValue += World.instance.GetCountry("finland").GetLocalDisease((Disease) this).customLocalVariable6;
        this.globalLethalityBottomValue += World.instance.GetCountry("russia").GetLocalDisease((Disease) this).customLocalVariable6;
        this.globalSeverityBottomValue = 1000f;
        this.globalInfectiousnessTopMultipler += World.instance.GetCountry("sweden").GetLocalDisease((Disease) this).customLocalVariable6 = this.globalInfectiousnessTopMultipler;
        this.globalLethalityTopMultipler += World.instance.GetCountry("norway").GetLocalDisease((Disease) this).customLocalVariable6;
        this.globalSeverityTopMultipler = 1000f;
        if (!CGameManager.game.IsReplayActive && (double) this.customGlobalVariable1 < 100.0)
          this.ShowHUD();
      }
      if ((double) this.globalSeverityMax >= 0.0)
        return;
      this.globalSeverityMax = 0.0f;
    }
  }

  private void HideHUD()
  {
    CSoundManager.instance.PlaySFX("pause_accept");
    CGameManager.spaceTimeRate = 1;
    CGameManager.spaceTime = true;
    CHUDScreen.instance.SetActive(false);
  }

  private void ShowHUD()
  {
    if (!CGameManager.spaceTime)
      return;
    CSoundManager.instance.PlaySFX("multiplayer_chatsend");
    CGameManager.spaceTimeRate = 0;
    CGameManager.spaceTime = false;
    CHUDScreen.instance.SetActive(true);
  }

  private void HideHUD(int spaceTimeRate)
  {
    CGameManager.spaceTimeRate = spaceTimeRate;
    CGameManager.spaceTime = true;
    if (CGameManager.game.ActualSpeed != spaceTimeRate)
      CSoundManager.instance.PlaySFX("pause_accept");
    CHUDScreen.instance.SetActive(false);
  }

  public void OnMusicBubbleClick(float pointRate, float importance, bool combo)
  {
    this.totalMusicImportance += importance;
    this.totalMusicPoint += pointRate * importance;
    if (combo)
      ++this.currentMusicCombo;
    else
      this.currentMusicCombo = 0;
    this.maxMusicCombo = Math.Max(this.maxMusicCombo, this.currentMusicCombo);
    this.evoPoints = this.currentMusicCombo;
    this.globalCureResearch = this.totalMusicPoint / this.expectedMusicImportance * this.cureRequirements;
  }

  public void CreateMusicBubble(
    Country country,
    BonusIcon.EBonusIconType bonusIconType,
    int importance)
  {
    Country country1;
    if (country == null)
    {
      int index = ModelUtils.IntRand(0, World.instance.countries.Count - 1);
      country1 = World.instance.countries[index];
    }
    else
      country1 = country;
    BonusIcon.EBonusIconType bonusIconType1 = bonusIconType;
    World.instance.AddBonusIcon(new BonusIcon((Disease) this, country1, bonusIconType1, importance));
    ++this.totalMusicBubbleCount;
  }

  public void SporeWarUpdate()
  {
    this.expectedMusicImportance = 500f;
    if (CGameManager.game.IsReplayActive)
      return;
    if (this.totalMusicBubbleCount > 470)
      this.HideHUD(5);
    else if (this.totalMusicBubbleCount > 440)
      this.HideHUD(4);
    else if (this.totalMusicBubbleCount > 250)
      this.HideHUD(3);
    else if (this.totalMusicBubbleCount > 20)
      this.HideHUD(2);
    else
      this.HideHUD(1);
    if ((double) this.totalMusicImportance < 499.95001220703125)
    {
      this.customGlobalVariable1 += (float) (0.5 + 0.0032999999821186066 * (double) this.turnNumber);
      if ((double) this.customGlobalVariable1 > (double) this.totalMusicBubbleCount && this.totalMusicBubbleCount < 500)
        this.CreateMusicBubble((Country) null, BonusIcon.EBonusIconType.DNA, 1);
      this.evoPoints = this.currentMusicCombo;
      this.globalCureResearch = this.totalMusicPoint / this.expectedMusicImportance * this.cureRequirements;
      this.cureFlag = false;
    }
    else if ((double) this.totalMusicPoint / (double) this.totalMusicImportance > 0.699999988079071)
    {
      if ((double) this.customGlobalVariable2 <= 0.5)
        this.daysToGameWin = 3;
      this.customGlobalVariable2 = 1f;
      this.cureFlag = false;
    }
    else
      this.cureFlag = true;
  }

  public void ClockCraftUpdate()
  {
    if (CGameManager.spaceTime)
      CSoundManager.instance.PlaySFX("pause_count");
    if (SPDisease.CheckFloatEqual(this.customGlobalVariable2, 2))
      this.StageCycleUpdate();
    if (SPDisease.CheckFloatEqual(this.customGlobalVariable2, 1))
      this.StageRecraftUpdate();
    if (SPDisease.CheckFloatEqual(this.customGlobalVariable2, 0))
    {
      this.customGlobalVariable1 = 100f;
      foreach (Country country in World.instance.countries)
      {
        if (country.hasAirports)
          country.airportStatus = false;
        if (country.hasPorts)
          country.portStatus = false;
        this.globalLandRate = 0.0f;
      }
      this.customGlobalVariable2 = 1f;
    }
    this.globalSeverityMax -= ModelUtils.FloatRand(0.75f, 1.05f);
    this.globalCureResearch = this.customGlobalVariable1 / 100f * this.cureRequirements;
    if (this.IsTechEvolved("34443e34-9957-45c9-8ef0-08d4cd8acefa") && (double) this.GetCountryVariable("china") <= 0.5)
    {
      this.SetCountryVariable("china", 1f);
      foreach (Country country in World.instance.countries)
        country.basePopulationDensity *= 1.5f;
    }
    if (this.IsTechEvolved("b66ec71a-8ba3-4bad-8c2b-46180ef9460d") && (double) this.GetCountryVariable("india") <= 0.5)
    {
      this.SetCountryVariable("india", 1f);
      float infectiousnessMax = this.globalInfectiousnessMax;
      this.globalInfectiousnessMax = this.globalLethalityMax;
      this.globalLethalityMax = infectiousnessMax;
    }
    if (this.IsTechEvolved("c51ecb6e-5f53-4f94-a682-dba046f89bb0"))
    {
      foreach (Country country in World.instance.countries)
      {
        if ((double) country.GetLocalDisease((Disease) this).customLocalVariable2 <= 0.5 && (double) country.deadPercent > 0.44999998807907104)
        {
          country.GetLocalDisease((Disease) this).customLocalVariable2 = 1f;
          this.ForceCreateBonusIcon(country.id, "BLUE", "0", "false");
        }
      }
    }
    if (this.IsTechEvolved("ab4f7edb-e930-4d0b-abbc-d68050cce69d") && this.IsTechEvolved("8acc2c8d-0492-486e-a07f-569c93f4a868"))
    {
      foreach (Country country in World.instance.countries)
      {
        country.cold = false;
        country.hot = false;
      }
    }
    else if (this.IsTechEvolved("ab4f7edb-e930-4d0b-abbc-d68050cce69d"))
    {
      foreach (Country country in World.instance.countries)
      {
        if (country.hot)
        {
          country.cold = true;
          country.hot = false;
        }
      }
    }
    else
    {
      if (this.IsTechEvolved("8acc2c8d-0492-486e-a07f-569c93f4a868"))
      {
        foreach (Country country in World.instance.countries)
        {
          if (country.cold)
          {
            country.cold = false;
            country.hot = true;
          }
        }
      }
      if (this.IsTechEvolved("be38e6fa-bfe9-4f8c-b11f-f61e2e3a43c2"))
        this.globalSeverityMax -= ModelUtils.FloatRand(0.515f, 0.7f);
      if (this.IsTechEvolved("7accf626-4af1-4103-a05c-34ee39e5bf55"))
        this.globalSeverityMax -= ModelUtils.FloatRand(0.515f, 0.7f);
      if ((double) this.globalSeverityMax >= 0.0)
        return;
      this.globalSeverityMax = 0.0f;
    }
  }

  public static bool CheckFloatEqual(float floatVariable, int intTarget)
  {
    return (double) floatVariable >= (double) intTarget - 1.0 / 500.0 && (double) floatVariable <= (double) intTarget + 1.0 / 500.0;
  }

  public void StageRecraftUpdate()
  {
    --this.customGlobalVariable1;
    this.globalInfectiousnessMax += ModelUtils.FloatRand(0.22f, 0.45f);
    this.globalLethalityMax += ModelUtils.FloatRand(0.17f, 0.33f);
    this.sporeCounter += ModelUtils.FloatRand(0.3f, 0.5f);
    if (!SPDisease.CheckFloatEqual(this.customGlobalVariable1, 0))
      return;
    this.StageRecraftEnd();
    this.customGlobalVariable2 = 2f;
  }

  public void StageRecraftEnd()
  {
    foreach (Country country in World.instance.countries)
    {
      if (country.totalInfected + country.totalDead > 0L)
      {
        int num1 = (int) Math.Floor(10000.0 * ((double) country.infectedPercent + (double) country.deadPercent)) / 10 % 10;
        int num2 = (int) Math.Floor(10000.0 * ((double) country.infectedPercent + (double) country.deadPercent)) % 10;
        if ((num1 + num2) % 2 == 1)
          country.cold = true;
        else
          country.hot = true;
        if ((10 * num1 + num2) % 2 == 0 && (10 * num1 + num2) / 2 % 2 == 1)
          country.urban = true;
        if ((10 * num1 + num2) % 2 == 0 && (10 * num1 + num2) / 2 % 2 == 0)
          country.rural = true;
        if ((10 * num1 + num2) % 3 == 0)
          country.wealthy = true;
        if ((10 * num1 + num2 + 1) % 3 == 0)
          country.poverty = true;
        if ((double) country.deadPercent > (double) country.infectedPercent / 2.0)
          country.arid = true;
        else
          country.humid = true;
        if (country.totalInfected == 0L)
          country.basePopulationDensity -= 2f;
        if (country != this.nexus)
          country.TransferPopulation((double) country.originalPopulation, Country.EPopulationType.INFECTED, (Disease) this, Country.EPopulationType.HEALTHY);
      }
      else
        country.basePopulationDensity -= 2f;
    }
    this.globalInfectiousnessMax = 15f;
    this.globalInfectiousness = 15f;
    this.globalLethalityMax = 0.0f;
    this.globalLethality = 0.0f;
    this.customGlobalVariable3 = 1f;
    this.globalLandRate = 1f;
  }

  public void StageCycleUpdate()
  {
    foreach (Country country in World.instance.countries)
      country.govLocalInfectiousness = 0.0f;
    if (this.IsTechEvolved("9ccf1319-71ba-4c4b-9183-60db33c513e4") && !this.backendPlayed)
    {
      this.backendPlayed = true;
      CSoundManager.instance.PlaySFX("multiplayer_chatsend");
    }
    if (SPDisease.CloseInt(this.customGlobalVariable4) >= SPDisease.CloseInt(this.customGlobalVariable3))
    {
      if ((double) this.globalLandRate >= 0.5)
        this.DayToNight();
      else
        this.NightToDay();
      this.customGlobalVariable4 = 0.0f;
    }
    if ((double) this.globalLandRate >= 0.5)
      this.DayUpdate();
    else
      this.NightUpdate();
    if ((double) this.GetCountryVariable("bolivia") < 0.0)
    {
      ++this.customGlobalVariable4;
      ++this.customGlobalVariable1;
    }
    this.SetCountryVariable("greenland", this.GetCountryVariable("greenland") - 1f);
    if (this.IsTechEvolved("2c716690-867e-4d2a-93c7-f0a16668038f") && (double) this.GetCountryVariable("japan") <= 0.5)
    {
      this.SetCountryVariable("japan", 1f);
      this.SetCountryVariable("greenland", 20f);
      if ((double) this.globalLandRate >= 0.5)
      {
        this.customGlobalVariable4 -= 20f;
      }
      else
      {
        float num = this.customGlobalVariable3 - this.customGlobalVariable4;
        this.NightToDay();
        this.customGlobalVariable4 = num - 20f;
      }
    }
    if (this.IsTechEvolved("3173a8e7-66f5-4590-9724-76b8b0651a35") && (double) this.GetCountryVariable("australia") <= 0.5)
    {
      this.SetCountryVariable("australia", 1f);
      this.SetCountryVariable("greenland", 20f);
      if ((double) this.globalLandRate <= 0.5)
      {
        this.customGlobalVariable4 -= 20f;
      }
      else
      {
        float num = this.customGlobalVariable3 - this.customGlobalVariable4;
        this.DayToNight();
        this.customGlobalVariable4 = num - 20f;
      }
    }
    this.SetCountryVariable("bolivia", this.GetCountryVariable("bolivia") - 1f);
    if (SPDisease.CloseInt(this.GetCountryVariable("bolivia")) == -1)
      this.ShowHUD();
    if (!this.IsTechEvolved("522226b8-000b-41b0-87e3-2ba57d2b29e2") || (double) this.GetCountryVariable("argentina") > 0.5)
      return;
    this.HideHUD();
    this.SetCountryVariable("argentina", 1f);
    this.SetCountryVariable("bolivia", 18f);
  }

  public void DayToNight()
  {
    this.hot *= -1f;
    this.cold *= -1f;
    foreach (Country country in World.instance.countries)
    {
      if (country.hasAirports)
        country.airportStatus = false;
      if (country.hasPorts)
        country.portStatus = false;
      country.borderStatus = false;
    }
    this.globalLandRate = 0.0f;
  }

  public void NightToDay()
  {
    this.hot *= -1f;
    this.cold *= -1f;
    foreach (Country country in World.instance.countries)
    {
      if (country.hasAirports)
        country.airportStatus = true;
      if (country.hasPorts)
        country.portStatus = true;
      country.borderStatus = true;
    }
    this.globalLandRate = 1f;
    if (this.IsTechEvolved("82ccbd65-507e-42a9-81a7-0906a82dede5"))
      this.globalInfectiousnessMax += this.globalSeverity / ModelUtils.FloatRand(22.5f, 30f);
    else
      this.globalInfectiousnessMax -= this.globalSeverity / ModelUtils.FloatRand(22.5f, 30f);
    this.globalLethality = 0.0f;
    ++this.customGlobalVariable3;
  }

  public void DayUpdate()
  {
    if (this.IsTechEvolved("a6553b01-1130-4fcf-a4b8-207704ef3b02") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 0, 30))
      this.cold += ModelUtils.FloatRand(0.0137f, 0.0198f);
    if (this.IsTechEvolved("24c6ac66-be0c-4d11-93da-d47e5bec0c5d") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 15, 45))
      this.poverty += ModelUtils.FloatRand(7f / 400f, 0.0238f);
    if (this.IsTechEvolved("06f87c33-6a49-4b35-bf8c-baa398485122") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 30, 60))
      this.rural += ModelUtils.FloatRand(7f / 400f, 0.0215f);
    if (this.IsTechEvolved("7084cf4d-719c-4ffa-9084-bb09dfcfeb98") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 45, 75))
      this.hot += ModelUtils.FloatRand(0.0137f, 0.0198f);
    if (this.IsTechEvolved("f4099890-ae49-4449-87a8-e5f4f02a4881") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 60, 90))
      this.wealthy += ModelUtils.FloatRand(0.0107f, 0.0138f);
    if (this.IsTechEvolved("e084ad68-3e69-43a3-a07c-363d4363493c") && SPDisease.CheckFloatBetween(this.customGlobalVariable1, 75, 105))
      this.urban += ModelUtils.FloatRand(7f / 400f, 0.0215f);
    this.globalInfectiousnessMax += this.customGlobalVariable1 / ModelUtils.FloatRand(70f, 100f);
  }

  public void NightUpdate()
  {
    if (!this.IsTechEvolved("a9beb0c1-ab68-4a00-9b7b-a1ed62de5274"))
      return;
    this.globalLethalityMax += this.customGlobalVariable1 / ModelUtils.FloatRand(67f, 100f);
    if ((double) this.GetCountryVariable("brazil") > 1.5 || (double) ModelUtils.FloatRand(0.0f, 1f) > 0.019999999552965164)
      return;
    this.SetCountryVariable("brazil", this.GetCountryVariable("brazil") + 1f);
    this.globalLethalityMax += 18f;
  }

  public static int CloseInt(float f)
  {
    int num = (int) f;
    return (double) f - (double) num >= 0.5 ? num + 1 : num;
  }

  public static bool CheckFloatBetween(float f, int min, int max)
  {
    return (double) f >= (double) min - 1.0 / 500.0 && (double) f <= (double) max + 1.0 / 500.0;
  }

  public float GetCountryVariable(string country, int id)
  {
    LocalDisease localDisease = World.instance.GetCountry(country).GetLocalDisease((Disease) this);
    switch (id)
    {
      case 1:
        return localDisease.customLocalVariable1;
      case 2:
        return localDisease.customLocalVariable2;
      case 3:
        return localDisease.customLocalVariable3;
      case 4:
        return localDisease.customLocalVariable4;
      case 5:
        return localDisease.customLocalVariable5;
      case 6:
        return localDisease.customLocalVariable6;
      default:
        return 0.0f;
    }
  }

  public void SetCountryVariable(string country, int id, float val)
  {
    LocalDisease localDisease = World.instance.GetCountry(country).GetLocalDisease((Disease) this);
    if (id == 1)
      localDisease.customLocalVariable1 = val;
    if (id == 2)
      localDisease.customLocalVariable2 = val;
    if (id == 3)
      localDisease.customLocalVariable3 = val;
    if (id == 4)
      localDisease.customLocalVariable4 = val;
    if (id == 5)
      localDisease.customLocalVariable5 = val;
    if (id != 6)
      return;
    localDisease.customLocalVariable6 = val;
  }

  public float GetCountryVariable(string country) => this.GetCountryVariable(country, 6);

  public void SetCountryVariable(string country, float val)
  {
    this.SetCountryVariable(country, 6, val);
  }

  public void CheckFateScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("影呓") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || (double) this.wealthy + (double) this.cold + (double) this.hot <= 6.3249998092651367)
      return;
    if (!this.CheckScenarioExist("命运之门"))
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.fateScenarioChecked = true;
      this.abnormalCheckedDay = this.turnNumber;
      this.daysToGameWin = 114514;
    }
    else
      Debug.Log((object) "命运之门 already installed");
  }

  public void CheckFinalScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("命运之门") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || (double) this.customGlobalVariable2 <= 1.0)
      return;
    this.HideHUD(1);
    this.finalScenarioChecked = true;
    this.abnormalCheckedDay = this.turnNumber;
    this.daysToGameWin = 114514;
  }

  public void FateGateUpdate()
  {
    if ((double) this.customGlobalVariable2 > 1.0)
    {
      CSLocalUGCHandler.SetScenarioProperty("PIFSL 命运之门", "unlocked", "1");
    }
    else
    {
      if ((double) this.customGlobalVariable3 <= 1.0)
        return;
      CSLocalUGCHandler.SetScenarioProperty("PIFSL 命运之门", "unlocked", "2");
    }
  }

  public long GetCurrentScore(bool won, bool scenario)
  {
    if (CGameManager.CheckExternalMethodExist("GetScoreWin"))
      return (long) CGameManager.CallExternalMethodWithReturnValue("GetScoreCurrent", World.instance, (Disease) this, (Country) null, (LocalDisease) null);
    if (CGameManager.IsFederalScenario("348关 万圣节特供 夜之复调I"))
    {
      if (this.turnNumber > 1000)
        return 0;
      long currentScore = (long) ((double) this.globalDeadPerc * 80000.0);
      if ((double) this.customGlobalVariable5 >= 0.40000000596046448)
        currentScore += 15000L;
      return currentScore;
    }
    if (CGameManager.IsFederalScenario("迪黎克菌：世界狂潮") && (double) this.globalCureResearch < (double) this.cureRequirements)
      return (long) (40000.0 * (double) Mathf.Max(0.0f, this.cureCompletePerc) + (double) this.mutationCounter * 100.0);
    if (CGameManager.IsFederalScenario("绯色审判"))
      return !World.instance.firedEvents.ContainsKey("【时空魔法】第二阶段") ? (long) (20000.0 * (double) this.customGlobalVariable4 / 100.0) : (long) (30000.0 + 20000.0 * (double) this.customGlobalVariable4 / 100.0);
    if (CGameManager.IsFederalScenario("终末千面"))
      return (long) (75000.0 * (double) this.globalDeadPerc);
    if (this.turnNumber <= 11)
      return 0;
    if (CGameManager.IsFederalScenario("里318 塞贝克鳄（荒寂）"))
      return 4L * (16000L + (long) this.mutation);
    if (CGameManager.IsFederalScenario("263关 Cure-Candida auri"))
      return (double) this.customGlobalVariable1 < 0.5 ? 0L : (long) ModelUtils.Min(75000.0, 35000.0 + 100000.0 * ((double) this.customGlobalVariable1 - 0.5));
    if (CGameManager.IsFederalScenario("Spore War"))
      return (long) (75000.0 * (double) this.totalMusicPoint / (double) this.totalMusicImportance);
    if (CGameManager.IsFederalScenario("298关 淬毒蚁国"))
      return this.totalDead * 1300L / 10000000L;
    if (CGameManager.IsFederalScenario("319关 五象鸑鷟"))
      return (long) (1.6 * (38400.0 + (double) this.mutation));
    if (CGameManager.IsFederalScenario("镜生虫ReMASTER"))
      return (double) this.globalDeadPerc < 0.60000002384185791 ? 0L : (long) (40000.0 * (double) this.globalDeadPerc);
    if (CGameManager.IsFederalScenario("时生虫ReCRAFT"))
      return (double) this.globalDeadPercent >= 0.75 ? 5000L : 0L;
    if (CGameManager.IsFederalScenario("时生虫ReMASTER"))
      return (long) ((double) this.globalDeadPercent * 65000.0 + (double) CSLocalUGCHandler.GetScenarioHighScore("PIFSL 时生虫ReCRAFTa"));
    float num1 = this.diseaseData.globalSeverity * (float) (this.evoPoints * 2 + this.diseaseData.evoPointsSpent);
    float num2 = 1f;
    if (this.diseaseData.difficulty == 2)
      num2 = 8f;
    else if (this.diseaseData.difficulty == 1)
      num2 = 4f;
    else if (this.diseaseData.difficulty == 3)
      num2 = 12f;
    float num3 = 0.0f;
    if ((double) this.diseaseData.cureRequirements > 0.0)
      num3 = this.diseaseData.globalCureResearch / this.diseaseData.cureRequirements;
    float a = Mathf.Clamp(num3 * 100f, 0.0f, 100f);
    float num4 = 1f;
    switch (this.diseaseType)
    {
      case Disease.EDiseaseType.VIRUS:
      case Disease.EDiseaseType.FUNGUS:
      case Disease.EDiseaseType.PARASITE:
      case Disease.EDiseaseType.PRION:
      case Disease.EDiseaseType.NANO_VIRUS:
      case Disease.EDiseaseType.BIO_WEAPON:
        num4 = 0.9f;
        break;
      case Disease.EDiseaseType.NEURAX:
        num4 = 1.1f;
        break;
      case Disease.EDiseaseType.NECROA:
        a /= 5f;
        num4 = 1.4f;
        if (this.diseaseData.difficulty >= 2)
        {
          num2 = 10f;
          break;
        }
        break;
    }
    int num5 = Mathf.FloorToInt(Mathf.Log((float) this.diseaseData.turnNumber));
    long f = (long) ((double) Mathf.Max(0.0f, (float) ((double) num1 * (double) num2 * (double) num4 / ((double) Mathf.Max(a, 1f) / 2.0 * (double) num5))) + (double) this.diseaseData.globalDeadPercent * 100.0);
    if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      f = (long) ((double) f * (1.1000000238418579 + 1.0 * (double) this.diseaseData.apeTotalInfected / (double) this.diseaseData.apeTotalPopulation - 1.0 * (double) this.diseaseData.apeTotalDead / (double) this.diseaseData.apeTotalPopulation / 2.0));
    long currentScore1 = won ? f * 10L : (long) Mathf.Max(0.0f, Mathf.Log((float) f) * 10f);
    if (scenario)
    {
      if (won)
      {
        currentScore1 = (long) Mathf.Max(0.0f, (float) (10000000.0 / ((double) this.diseaseData.turnNumber / 2.0) * ((1.0 - (double) num3) / 3.0 + 1.0)) * World.instance.scenarioScoreMultiplier);
        if (this.scenario == "board_game")
        {
          Debug.Log((object) ("BOARD GAME SCORE: " + (object) this.totalDead));
          currentScore1 = this.totalDead;
        }
        if (CGameManager.IsFederalScenario("ReconstructionIncBeyond") && (double) this.globalHealthyPercent >= 0.0099999997764825821)
          currentScore1 += (long) (15000.0 * Math.Pow((double) this.globalHealthyPercent, 1.17));
        if (CGameManager.IsFederalScenario("298关 淬毒蚁国") & won)
          currentScore1 = currentScore1 / 4L + 75400L;
        if (CGameManager.IsFederalScenario("319关 五象鸑鷟") & won)
          currentScore1 = (long) ((double) currentScore1 / 3.5) + 61440L;
        if (CGameManager.IsFederalScenario("镜生虫ReMASTER") & won)
          currentScore1 += 40000L;
        if (CGameManager.IsFederalScenario("里318 塞贝克鳄（荒寂）") & won)
          currentScore1 = currentScore1 / 6L + 65000L;
        if (CGameManager.IsFederalScenario("迪黎克菌：世界狂潮") && (double) this.globalCureResearch >= (double) this.cureRequirements)
          return (long) (40000.0 + (double) currentScore1 + (double) this.mutationCounter * 100.0 + (1.0 - (double) this.globalDeadPerc) * 100.0);
        if (CGameManager.IsFederalScenario("海盗瘟疫MXM"))
        {
          long num6 = (long) (60000.0 + 0.5 * (double) this.mutation);
          if (num6 > 60000L)
            num6 = 60000L;
          if (this.diseaseType == Disease.EDiseaseType.FUNGUS)
            num6 -= 2500L;
          else if (this.diseaseType == Disease.EDiseaseType.NEURAX)
            num6 -= 7000L;
          else if (this.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
            num6 += 6000L;
          else if (this.diseaseType == Disease.EDiseaseType.NECROA)
            num6 += 3500L;
          else if (this.diseaseType == Disease.EDiseaseType.VAMPIRE)
            num6 -= 4000L;
          currentScore1 += num6;
        }
      }
      else
        currentScore1 = 0L;
    }
    return currentScore1;
  }

  private void PirateUpdate()
  {
    if ((double) this.GetCountryVariable("greenland", 5) > 0.0)
      this.HideHUD(1);
    if ((double) this.GetCountryVariable("greenland", 5) > -0.9 && (double) this.GetCountryVariable("greenland", 5) < -0.1)
      this.ShowHUD();
    this.SetCountryVariable("greenland", 5, this.GetCountryVariable("greenland", 5) - 1f);
  }

  public static void GetScenarioAuthorExternal()
  {
    CGameManager.federalScenarioAuthorList = new Dictionary<string, string>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioAuthor.txt"))
    {
      Debug.Log((object) "No Unlock Score File found, skip!");
    }
    else
    {
      string[] strArray1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioAuthor.txt").Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str1 in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str1.Split(chArray);
          if (strArray2.Length >= 2)
          {
            string key = "PIFSL " + strArray2[0];
            string str2 = strArray2[1];
            CGameManager.federalScenarioAuthorList.Add(key, str2);
          }
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong author list, dumbass");
    }
  }

  public static void GetScenarioUnlockPotentialExternal()
  {
    CGameManager.scenarioUnlockPotential = new Dictionary<string, int>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockPotential.txt"))
    {
      Debug.Log((object) "No Unlock Potential File found, skip!");
    }
    else
    {
      string[] strArray1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockPotential.txt").Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str.Split(chArray);
          if (strArray2.Length >= 2)
          {
            string key = "PIFSL " + strArray2[0];
            int num = int.Parse(strArray2[1]);
            CGameManager.scenarioUnlockPotential.Add(key, num);
          }
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong unlock potential list, dumbass");
    }
  }

  public void CheckEverScenarioRequirement()
  {
    if (CGameManager.game.CurrentLoadedScenario == null || string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) || this.numCheats > 0 || this.difficulty != 3 || CGameManager.game.CurrentLoadedScenario.isOfficial || !CGameManager.game.CurrentLoadedScenario.filename.Contains("Level 7777 乌托邦") || !CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL") || !World.instance.firedEvents.ContainsKey("第二次祈祷成功"))
      return;
    if (!this.CheckScenarioExist("Level8848 无垠雪峰"))
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView("libya");
      countryView.NukeStrikeEffect(countryView.transform.position, this == CNetworkManager.network.LocalPlayerInfo.disease);
      this.scenarioEverChecked = true;
      this.abnormalCheckedDay = this.turnNumber;
      this.daysToGameWin = 114514;
    }
    else
      Debug.Log((object) "Level8848 无垠雪峰 already installed");
  }

  private void CureScenarioUpdate()
  {
    foreach (Country country in World.instance.countries)
    {
      SPLocalDisease localDisease = country.GetLocalDisease((Disease) this) as SPLocalDisease;
      if (country.apeLabStatus != EApeLabState.APE_LAB_ACTIVE && (double) localDisease.customLocalVariable1 >= 1.9500000476837158 && (double) localDisease.customLocalVariable1 <= 2.0499999523162842)
        country.ChangeApeLabStateF(EApeLabState.APE_LAB_ACTIVE);
      if (country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE && ((double) localDisease.customLocalVariable1 <= 1.9500000476837158 || (double) localDisease.customLocalVariable1 >= 2.0499999523162842))
        country.ChangeApeLabStateF(EApeLabState.APE_LAB_DESTROYED);
    }
  }

  public static void GetScenarioNameExternal()
  {
    CGameManager.scenarioName = new Dictionary<string, string>();
    if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioName.txt"))
    {
      Debug.Log((object) "No Scenario Name File found, skip!");
    }
    else
    {
      string[] strArray1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioName.txt").Replace("\r\n", "\n").Split('\n');
      if (strArray1.Length != 0)
      {
        foreach (string str1 in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str1.Split(chArray);
          if (strArray2.Length >= 2)
          {
            string key = "PIFSL " + strArray2[0];
            string str2 = strArray2[1];
            CGameManager.scenarioName.Add(key, str2);
          }
        }
      }
      else
        Debug.Log((object) "You must jave gotten a wrong Scenario Name list, dumbass");
    }
  }

  public void FireEvent(string eventName)
  {
    if (!World.instance.firedEvents.ContainsKey(eventName))
      World.instance.firedEvents[eventName] = (IDictionary<int, int>) new Dictionary<int, int>();
    if (!World.instance.firedEvents[eventName].ContainsKey(this.id))
      World.instance.firedEvents[eventName][this.id] = 1;
    else
      World.instance.firedEvents[eventName][this.id]++;
  }

  public bool IsEventFired(string eventName, int time = 1)
  {
    if (time == 1)
    {
      if (World.instance == null)
        Debug.Log((object) "world is null");
      if (World.instance.firedEvents == null)
        Debug.Log((object) "firedEvents is null");
      return World.instance.firedEvents.ContainsKey(eventName);
    }
    return World.instance.firedEvents.ContainsKey(eventName) && World.instance.firedEvents[eventName][this.id] >= time;
  }
}
