// Decompiled with JetBrains decompiler
// Type: StandardGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
public class StandardGame : IGame
{
  public override Disease GetMyDisease() => CGameManager.localPlayerInfo.disease;

  public override Disease GetTheirDisease() => (Disease) null;

  public override World CreateWorld() => (World) new SPWorld();

  public override void StartGame()
  {
    if (!this.IsReplayActive)
    {
      Analytics.Event("Disease Type", this.world.diseases[0].diseaseType.ToString());
      Analytics.Event("Game Difficulty", this.world.diseases[0].difficulty.ToString());
      if (this.world.diseases[0].name != RandomNameGenerator.lastRandomName)
        Analytics.Event("Disease Name", this.world.diseases[0].name);
      if (this.CurrentLoadedScenario != null)
        Analytics.Event("Scenario", this.CurrentLoadedScenario.scenarioInformation.id);
    }
    base.StartGame();
  }

  public override void EndGame(IGame.EndGameReason reason)
  {
    CUIManager.Unlock unlocked = new CUIManager.Unlock();
    this.gameState = IGame.GameState.EndGame;
    IPlayerInfo player = CGameManager.localPlayerInfo;
    player.playerStats.SP_Lifetime_Infected += player.disease.accumulatedInfections;
    player.playerStats.SP_Lifetime_Killed += (float) player.disease.totalKilled;
    player.playerStats.SP_Lifetime_Cured += player.disease.accumulatedCures;
    player.playerStats.SP_Lifetime_Zombies += player.disease.accumulatedZombies;
    player.playerStats.SP_Lifetime_Apes += player.disease.accumulatedIntelligentApes;
    player.playerStats.SP_Lifetime_DNA_Spent += (float) player.disease.evoPointsSpent;
    player.playerStats.SP_Lifetime_Bubbles_Popped += player.disease.bubblesPopped;
    FieldInfo[] fields = typeof (PlayerStats).GetFields();
    int num = 3 + player.disease.genes.Count;
    foreach (FieldInfo fieldInfo in fields)
    {
      if (num != 0)
      {
        if (fieldInfo.Name == "SP_Start_Country_" + player.disease.nexus.id)
        {
          fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
          --num;
        }
        else if (fieldInfo.Name == "SP_DISEASE_" + player.disease.diseaseType.ToString())
        {
          fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
          --num;
        }
        else if (this.CurrentLoadedScenario != null && this.CurrentLoadedScenario.scenarioInformation != null && fieldInfo.Name == "SP_Scenario_" + this.CurrentLoadedScenario.scenarioInformation.id)
        {
          fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
          --num;
        }
        else
        {
          foreach (Gene gene in player.disease.genes)
          {
            if (fieldInfo.Name == "SP_GENE_" + gene.id)
            {
              fieldInfo.SetValue((object) player.playerStats, (object) ((int) fieldInfo.GetValue((object) player.playerStats) + 1));
              --num;
            }
          }
        }
      }
      else
        break;
    }
    int a1 = Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted);
    Debug.Log((object) ("GAME END, GameTime: " + (object) a1));
    if (this.world.gameWon)
    {
      if (!this.isGameSessionInterrupted)
        Analytics.Event("Win Game Time", a1.ToString());
      Analytics.Event("Win Game Turns", this.world.DiseaseTurn.ToString());
      Analytics.Event("Win Disease Type", this.world.diseases[0].diseaseType.ToString());
      Analytics.Event("Win Game Difficulty", this.world.diseases[0].difficulty.ToString());
      ++player.playerStats.SP_Lifetime_Games_Won;
      player.playerStats.SP_Quickest_Win = player.playerStats.SP_Quickest_Win > 0 ? Mathf.Min(a1, player.playerStats.SP_Quickest_Win) : a1;
      if (player.disease.numCheats == 0 && (this.CurrentLoadedScenario == null || player.disease.isCure && this.CurrentLoadedScenario.isOfficial))
      {
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_TheEndPlague));
        if (player.disease.diseaseType == Disease.EDiseaseType.NEURAX)
        {
          if (player.disease.daysToGameWin <= 0)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_AssumingDirectControlPC));
          else
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_WormFood));
        }
        else if (player.disease.diseaseType == Disease.EDiseaseType.NECROA)
        {
          if (!player.disease.zday && !player.disease.zdayDone)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_NotAnotherZombieGame));
          else
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_TheGloriousDead));
        }
        else if (player.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
        {
          if (!player.disease.IsTechEvolved("shadow_slaves"))
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_sadomasicist));
        }
        else if (player.disease.diseaseType == Disease.EDiseaseType.BACTERIA && this.world.DiseaseTurn < 366 && string.IsNullOrEmpty(player.disease.scenario))
          CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_InsaneBolt));
        else if (player.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
        {
          if ((double) player.disease.apeTotalInfected > 10.0)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_apeswillrise));
          if (player.disease.nexus.id == "greenland")
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_anapeisforlife));
          if (player.disease.noIdeaFlag == 1)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_noidea));
          if (player.disease.difficulty == 3)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_simianmaster));
        }
        else if (player.disease.diseaseType == Disease.EDiseaseType.CURE)
        {
          if (player.disease.vaccineStage != Disease.EVaccineProgressStage.VACCINE_RELEASE)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_antivaxxer));
          if ((double) (player.disease.totalHealthy + player.disease.totalHealthyRecovered) < (double) World.instance.totalPopulation * 0.05000000074505806)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_doomsdaysave));
        }
        if (player.disease.isSpeedRun && !player.disease.isCure && string.IsNullOrEmpty(player.disease.scenario))
        {
          if (player.disease.CheckTurn() && player.disease.CheckEvo() && this.world.CheckDiseaseTurn())
            CGameManager.scores.AddScore(new IScores.Score(player.disease.name, (long) player.disease.turnNumber, 0, CGameManager.gameType, CGameManager.localPlayerInfo.disease.diseaseType, ""));
        }
        else if (player.disease.isCure || string.IsNullOrEmpty(player.disease.scenario))
        {
          if (player.disease.CheckEvo())
          {
            if (!CGameManager.usingDiseaseX)
            {
              Disease.EDiseaseType d = player.disease.diseaseType;
              if (d == Disease.EDiseaseType.CURE)
                d = CGameManager.GetCureDiseaseType(player.disease.cureScenario);
              CGameManager.scores.AddScore(new IScores.Score(player.disease.name, player.disease.GetScore(true, false), 0, CGameManager.gameType, d, ""));
            }
            if (!player.disease.isCure)
              CGameManager.scores.AddScore(new IScores.Score(player.disease.name, (long) player.disease.turnNumber, 0, IGame.GameType.SpeedRun, CGameManager.localPlayerInfo.disease.diseaseType, ""));
          }
          if (player.disease.isCure)
          {
            if (CGameManager.usingDiseaseX)
            {
              if (player.GetCureCompletion(Disease.ECureScenario.Cure_DiseaseX) < player.disease.difficulty)
                player.SetCureCompletion(Disease.ECureScenario.Cure_DiseaseX, player.disease.difficulty);
            }
            else if (player.GetCureCompletion(player.disease.cureScenario) < player.disease.difficulty)
              player.SetCureCompletion(player.disease.cureScenario, player.disease.difficulty);
          }
          else if (player.GetDiseaseCompletion(player.disease.diseaseType) < player.disease.difficulty)
            player.SetDiseaseCompletion(player.disease.diseaseType, player.disease.difficulty);
          if (player.disease.isCure && !CGameManager.usingDiseaseX)
          {
            if (player.disease.cureScenario == Disease.ECureScenario.Cure_Bioweapon)
            {
              if (player.disease.vaccineStage >= Disease.EVaccineProgressStage.VACCINE_RELEASE)
                CGameManager.AwardCureCompletionAchievement(player.disease.cureScenario, player.disease.difficulty >= 3);
            }
            else
              CGameManager.AwardCureCompletionAchievement(player.disease.cureScenario, player.disease.difficulty >= 3);
          }
          if (player.disease.difficulty >= 1 || COptionsManager.instance.mbPityMode)
          {
            if (player.disease.isCure)
            {
              if (!CGameManager.usingDiseaseX)
              {
                Disease.ECureScenario? cureNext = CGameManager.GetCureNext(player.disease.cureScenario);
                if (cureNext.HasValue && !player.GetCureUnlocked(cureNext.Value))
                {
                  unlocked.gameType = CGameManager.gameType;
                  unlocked.disease = new Disease.EDiseaseType?(CGameManager.GetCureDiseaseType(cureNext.Value));
                  player.SetCureUnlocked(cureNext.Value);
                }
              }
            }
            else
            {
              Disease.EDiseaseType? diseaseNext = CGameManager.GetDiseaseNext(player.disease.diseaseType);
              CGameManager.AwardDiseaseCompletionAchievement(player.disease.diseaseType, player.disease.difficulty >= 3);
              if (diseaseNext.HasValue && !player.GetDiseaseUnlocked(diseaseNext.Value))
              {
                unlocked.gameType = CGameManager.gameType;
                unlocked.disease = new Disease.EDiseaseType?(diseaseNext.Value);
                player.SetDiseaseUnlocked(diseaseNext.Value);
              }
            }
          }
          bool flag1 = true;
          bool flag2 = true;
          bool flag3 = true;
          Disease.EDiseaseType[] values = (Disease.EDiseaseType[]) Enum.GetValues(typeof (Disease.EDiseaseType));
          foreach (Disease.EDiseaseType diseaseType in values)
          {
            switch (diseaseType)
            {
              case Disease.EDiseaseType.TUTORIAL:
              case Disease.EDiseaseType.FAKE_NEWS:
              case Disease.EDiseaseType.CURE:
              case Disease.EDiseaseType.CURETUTORIAL:
              case Disease.EDiseaseType.DISEASEX:
                continue;
              default:
                int diseaseCompletion = player.GetDiseaseCompletion(diseaseType);
                if (diseaseCompletion < 3)
                {
                  if (flag3)
                    Debug.Log((object) ("DiseaseType: " + (object) diseaseType + " val: " + (object) diseaseCompletion + " -> no mega achievement"));
                  flag3 = false;
                }
                if (diseaseType != Disease.EDiseaseType.SIMIAN_FLU)
                {
                  if (diseaseCompletion < 2)
                  {
                    if (flag2)
                      Debug.Log((object) ("DiseaseType: " + (object) diseaseType + " val: " + (object) diseaseCompletion + " -> no brutal cheats"));
                    flag2 = false;
                  }
                  if (diseaseCompletion < 1)
                  {
                    if (flag2)
                      Debug.Log((object) ("DiseaseType: " + (object) diseaseType + " val: " + (object) diseaseCompletion + " -> no cheats"));
                    flag1 = false;
                    continue;
                  }
                  continue;
                }
                continue;
            }
          }
          if (flag2)
            player.SetIntStat("UNLOCK_MEGA_CHEATS", 1);
          if (flag1)
          {
            player.SetIntStat("UNLOCK_BRUTAL_CHEATS", 1);
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_UnlockCheats));
          }
          if (flag3)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_Disease_Master));
          bool flag4 = true;
          bool flag5 = true;
          bool flag6 = true;
          foreach (Disease.EDiseaseType disease in values)
          {
            switch (disease)
            {
              case Disease.EDiseaseType.TUTORIAL:
              case Disease.EDiseaseType.FAKE_NEWS:
              case Disease.EDiseaseType.CURE:
              case Disease.EDiseaseType.CURETUTORIAL:
              case Disease.EDiseaseType.DISEASEX:
                continue;
              default:
                Disease.ECureScenario cureScenarioType = CGameManager.GetCureScenarioType(disease);
                if (cureScenarioType != Disease.ECureScenario.Cure_Standard || disease == Disease.EDiseaseType.BACTERIA)
                {
                  int cureCompletion = player.GetCureCompletion(cureScenarioType);
                  if (cureCompletion < 3)
                  {
                    if (flag6)
                      Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no mega achievement"));
                    flag6 = false;
                  }
                  if (cureCompletion < 2)
                  {
                    if (flag5)
                      Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no brutal cheats"));
                    flag5 = false;
                  }
                  if (cureCompletion < 1)
                  {
                    if (flag5)
                      Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no cheats"));
                    flag4 = false;
                    continue;
                  }
                  continue;
                }
                continue;
            }
          }
          if (flag5)
            player.SetIntStat("UNLOCK_CURE_MEGA_CHEATS", 1);
          if (flag4)
            player.SetIntStat("UNLOCK_CURE_CHEATS", 1);
          List<Gene.EGeneCategory> egeneCategoryList1 = new List<Gene.EGeneCategory>();
          if (player.disease.diseaseType == Disease.EDiseaseType.NECROA)
          {
            egeneCategoryList1.Add(Gene.EGeneCategory.environmental);
            egeneCategoryList1.Add(Gene.EGeneCategory.travel);
            egeneCategoryList1.Add(Gene.EGeneCategory.dna);
            egeneCategoryList1.Add(Gene.EGeneCategory.mutation);
            egeneCategoryList1.Add(Gene.EGeneCategory.zombie);
          }
          else if (player.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
          {
            egeneCategoryList1.Add(Gene.EGeneCategory.environmental);
            egeneCategoryList1.Add(Gene.EGeneCategory.travel);
            egeneCategoryList1.Add(Gene.EGeneCategory.dna);
            egeneCategoryList1.Add(Gene.EGeneCategory.simian1);
            egeneCategoryList1.Add(Gene.EGeneCategory.simian2);
          }
          else if (player.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
          {
            egeneCategoryList1.Add(Gene.EGeneCategory.environmental);
            egeneCategoryList1.Add(Gene.EGeneCategory.travel);
            egeneCategoryList1.Add(Gene.EGeneCategory.blood);
            egeneCategoryList1.Add(Gene.EGeneCategory.flight);
            egeneCategoryList1.Add(Gene.EGeneCategory.shadow);
          }
          else if (player.disease.diseaseType == Disease.EDiseaseType.CURE)
          {
            egeneCategoryList1.Add(Gene.EGeneCategory.cure_transmission);
            egeneCategoryList1.Add(Gene.EGeneCategory.cure_quarantine);
            egeneCategoryList1.Add(Gene.EGeneCategory.cure_country);
            egeneCategoryList1.Add(Gene.EGeneCategory.cure_abilities);
            egeneCategoryList1.Add(Gene.EGeneCategory.cure_operation);
          }
          else
          {
            egeneCategoryList1.Add(Gene.EGeneCategory.environmental);
            egeneCategoryList1.Add(Gene.EGeneCategory.travel);
            egeneCategoryList1.Add(Gene.EGeneCategory.dna);
            egeneCategoryList1.Add(Gene.EGeneCategory.mutation);
            egeneCategoryList1.Add(Gene.EGeneCategory.evolve);
          }
          IDictionary<Gene.EGeneCategory, List<Gene>> dictionary = (IDictionary<Gene.EGeneCategory, List<Gene>>) new Dictionary<Gene.EGeneCategory, List<Gene>>();
          foreach (Gene.EGeneCategory egeneCategory in egeneCategoryList1)
          {
            Gene.EGeneCategory cat = egeneCategory;
            List<Gene> all = this.availableGenes.FindAll((Predicate<Gene>) (a => a.geneCategory == cat && !player.GetGeneUnlocked(a)));
            if (all.Count > 0)
            {
              all.Sort((Comparison<Gene>) ((a, b) => a.techLevel.CompareTo(b.techLevel)));
              dictionary[cat] = all;
            }
          }
          List<Gene.EGeneCategory> egeneCategoryList2 = new List<Gene.EGeneCategory>((IEnumerable<Gene.EGeneCategory>) dictionary.Keys);
          if (egeneCategoryList2.Count > 0)
          {
            Gene gene = dictionary[egeneCategoryList2[ModelUtils.IntRand(0, egeneCategoryList2.Count - 1)]][0];
            player.SetGeneUnlocked(gene);
            unlocked.gene = gene;
            unlocked.gameType = CGameManager.gameType;
          }
        }
      }
      else if (player.disease.numCheats == 0 && this.CurrentLoadedScenario != null && this.CurrentLoadedScenario.isOfficial)
      {
        if (player.disease.scenario == "frozen_virus" && player.disease.daysToGameWin <= 0)
          CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_WhoNeedsBrains));
      }
      else if (player.disease.numCheats == 1)
      {
        Disease.ECheatType cheat = player.disease.GetCheats()[0];
        player.UpdateCheatProgress(cheat, player.disease.difficulty, player.disease.diseaseType);
        if (cheat == Disease.ECheatType.SHUFFLE)
        {
          if (player.disease.difficulty >= 3)
            player.IncrementAchievementStat(EAchievement.A_MegabrutalShuffle, 1, 5);
          if (player.disease.difficulty >= 1)
            player.IncrementAchievementStat(EAchievement.A_NormalShuffle, 1, 5);
        }
        if (cheat == Disease.ECheatType.LUCKY_DIP)
        {
          if (player.disease.difficulty >= 3)
            player.IncrementAchievementStat(EAchievement.A_LotteryWinner, 1, 5);
          if (player.disease.difficulty >= 1)
            player.IncrementAchievementStat(EAchievement.A_Bingo, 1, 5);
        }
        CNetworkManager.network.SaveLocalPlayerData();
      }
    }
    else
    {
      if (!this.isGameSessionInterrupted)
        Analytics.Event("Lose Game Time", Mathf.FloorToInt(Time.realtimeSinceStartup - this.timeGameSessionStarted).ToString());
      Analytics.Event("Lose Game Turns", this.world.DiseaseTurn.ToString());
      Analytics.Event("Lose Disease Type", this.world.diseases[0].diseaseType.ToString());
      Analytics.Event("Lose Game Difficulty", this.world.diseases[0].difficulty.ToString());
      player.playerStats.SP_Quickest_Loss = player.playerStats.SP_Quickest_Loss > 0 ? Mathf.Min(a1, player.playerStats.SP_Quickest_Loss) : a1;
      if (player.disease.CheckEvo() && CGameManager.gameType == IGame.GameType.Classic)
        CGameManager.scores.AddScore(new IScores.Score(player.disease.name, player.disease.GetScore(false, false), 0, CGameManager.gameType, CGameManager.localPlayerInfo.disease.diseaseType, ""));
    }
    if (this.CurrentLoadedScenario != null && !player.disease.isCure && player.disease.numCheats == 0)
    {
      long score = player.disease.GetScore(this.world.gameWon, true);
      if (this.CurrentLoadedScenario.isOfficial)
        CGameManager.scores.AddScore(new IScores.Score(player.disease.name, score, 0, CGameManager.gameType, CGameManager.localPlayerInfo.disease.diseaseType, this.CurrentLoadedScenario.scenarioInformation.id));
      string filename = this.CurrentLoadedScenario.filename;
      if (filename.IndexOf("PIFSL") != -1 && player.disease.difficulty == 3)
      {
        CGameManager.oldPotential = CGameManager.GetPotential();
        CGameManager.GetRatingList();
        CGameManager.oldScore = CSLocalUGCHandler.GetScenarioHighScore(filename);
        CSLocalUGCHandler.SetScenarioHighScore(filename, Mathf.Max(CSLocalUGCHandler.GetScenarioHighScore(filename), (int) score));
        CGameManager.newPotential = CGameManager.GetPotential();
        CGameManager.currentPotential = CGameManager.newPotential;
        CGameManager.currentScore = CGameManager.GetScenarioLibraryScore();
        CGameManager.GetRatingList();
      }
      if (this.world.gameWon)
      {
        int rating = 0;
        int[] numArray = CGameManager.ScenarioRatingBands;
        if (player.disease.scenario == "board_game")
          numArray = CGameManager.BoardGameRatingBands;
        if (player.disease.scenario == "fake_news")
          numArray = CGameManager.FakeNewsRatingBands;
        for (int index = numArray.Length - 1; index >= 0; --index)
        {
          if (score > (long) numArray[index])
          {
            rating = index + 1;
            break;
          }
        }
        if (this.CurrentLoadedScenario.isOfficial)
        {
          player.SetScenarioRating(this.CurrentLoadedScenario.scenarioInformation, player.disease.difficulty, player.disease.diseaseType, rating);
          if (CGameManager.DefaultScenarios.Contains(this.CurrentLoadedScenario.scenarioInformation.id))
            CGameManager.UnlockAllScenariosForPlayer();
          if (rating >= 3)
            CGameManager.AwardScenarioCompletionAchievement(this.CurrentLoadedScenario.scenarioInformation.id, player.CompletedEveryDiseaseInScenarioWith3Biohazards(this.CurrentLoadedScenario.scenarioInformation));
          bool flag = true;
          foreach (ScenarioInformation officialScenario in CGameManager.OfficialScenarios)
          {
            string id = officialScenario.id;
            if (id != null && !id.StartsWith("cure_") && !(id == "cure") && player.GetScenarioRating(officialScenario, 0) < 3)
              flag = false;
          }
          if (flag)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_Scenario_Master));
        }
        if (this.CurrentLoadedScenario.isOfficial && player.disease.scenario == "fake_news")
        {
          if (player.disease.difficulty >= 2)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_postTruthSociety));
          if (player.disease.difficulty >= 1)
            CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_filterBubble));
        }
      }
      else if (this.CurrentLoadedScenario.isOfficial && player.disease.scenario == "fake_news")
        CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_weLoveFactCheckers));
    }
    CGameManager.CheckUnlockedAll();
    CNetworkManager.network.SaveLocalPlayerData();
    CInterfaceManager.instance.EndGame(CGameManager.gameType, this.world.gameWon, this.world.winner, this.world.gameEndResult, unlocked);
  }

  public override ISaves.SaveMetaData SaveGame(int slotID)
  {
    ISaves.SaveMetaData data = new ISaves.SaveMetaData();
    Disease disease = this.world.diseases[0];
    data.difficulty = disease.difficulty;
    data.diseaseName = disease.name;
    data.gameTurn = disease.turnNumber;
    data.isDiseaseX = CGameManager.usingDiseaseX;
    data.curePercent = Mathf.CeilToInt(Mathf.Clamp01(disease.cureCompletePercent) * 100f);
    data.deadPercent = Mathf.CeilToInt(disease.globalDeadPercent * 100f);
    data.infectedPercent = Mathf.CeilToInt(disease.globalInfectedPercent * 100f);
    data.saveSlot = slotID;
    data.diseaseType = disease.diseaseType;
    data.gameType = CGameManager.gameType;
    if (this.CurrentLoadedScenario == null)
    {
      data.scenario = string.Empty;
      data.publishID = 0UL;
    }
    else
    {
      data.scenario = this.CurrentLoadedScenario.scenarioInformation.id;
      if (this.CurrentLoadedScenario.fromNdemic)
        data.scenario = "NDEMIC:" + this.CurrentLoadedScenario.filename;
      ulong result;
      if (ulong.TryParse(this.CurrentLoadedScenario.scenarioInformation.id, out result))
        data.publishID = result;
    }
    Debug.Log((object) ("SAVING: " + (object) slotID + " scenario: " + data.scenario));
    string serializedGameState = this.GetSerializedGameState();
    string replayData = " ";
    if (this.replayData.hasData)
      replayData = GameSerializer.SerializeReplay(this.replayData);
    CGameManager.saves.SaveGame(serializedGameState, CInterfaceManager.instance.GetWorldScreenshot(), data, replayData);
    GC.Collect();
    return data;
  }

  public override void SetSpeed(int gameSpeed)
  {
    this.wantedSpeed = gameSpeed;
    CGameManager.localPlayerInfo.gameSpeed = gameSpeed;
    bool flag = false;
    if (this.actualSpeed != gameSpeed)
      flag = true;
    this.actualSpeed = gameSpeed;
    if (!flag)
      return;
    CGameManager.GameSpeedChanged(this.wantedSpeed);
  }

  public override bool ChooseCountry(string countryID, bool isTimeout = false)
  {
    if (!base.ChooseCountry(countryID))
      return false;
    this.StartGame();
    return true;
  }

  public override void UpdateAchievements()
  {
    if (CGameManager.localPlayerInfo.disease.numCheats == 0 && !CGameManager.usingDiseaseX)
    {
      bool flag = false;
      foreach (EAchievement achievement in this.world.achievements)
      {
        if (CGameManager.AwardAchievement(new EAchievement?(achievement)))
          flag = true;
      }
      if (flag)
        CNetworkManager.network.SaveLocalPlayerData();
    }
    this.world.ClearAchievements();
  }

  public override void CreateUnscheduledFlight(
    Country fromCountry,
    Country toCountry,
    Disease disease,
    Vector3 sourcePosition,
    Vector3 endPosition)
  {
    if (disease.evoPoints < CGameManager.GetActiveAbilityCost(EAbilityType.unscheduled_flight, disease) || fromCountry.totalInfected <= 1000L)
      return;
    int num1 = ModelUtils.IntRand(6, 15);
    if ((double) ((SPCountry) toCountry).mData.accumulatedFlight > 0.0)
    {
      float num2 = ModelUtils.FloatRand(0.8f, 1.1f) * 16f * Mathf.Pow(((SPCountry) toCountry).mData.accumulatedFlight, 3f);
      num1 += (int) num2;
    }
    disease.SendPlane(fromCountry.id, toCountry.id, num1.ToString());
    disease.SpendEvoPoints(CGameManager.GetActiveAbilityCost(EAbilityType.unscheduled_flight, disease));
    if (this.Difficulty >= 2U)
      disease.RecordActiveAbilityUse(EAbilityType.unscheduled_flight);
    if (this.Difficulty == 3U)
      disease.RecordActiveAbilityUse(EAbilityType.unscheduled_flight);
    disease.SetAACostAdditional(EAbilityType.unscheduled_flight, disease.GetActiveAbilityUse(EAbilityType.unscheduled_flight));
    CInterfaceManager.instance.UpdateInterface();
  }

  protected override bool CanSeeDots(LocalDisease ld)
  {
    return !ld.disease.isCure || ld.disease.easyIntel || ld.hasIntel;
  }
}
