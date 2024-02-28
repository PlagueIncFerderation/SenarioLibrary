// Decompiled with JetBrains decompiler
// Type: CPlayerInfoSteam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class CPlayerInfoSteam : IPlayerInfo
{
  public CSteamID steamID;
  public string countryCode;
  public bool p2pConnected;
  [NonSerialized]
  private Dictionary<string, int> scenarioCompletionMap;
  [NonSerialized]
  private Dictionary<string, int> cheatCompletionMap;
  public const int MAX_RATING = 10000;
  public const int MIN_RATING = 1;

  public int initialMultiplayerGameRanking { get; private set; }

  public override string PlayerID
  {
    get => this.steamID != CSteamID.Nil ? ((ulong) this.steamID).ToString() : string.Empty;
  }

  public override void IncrementAchievementStat(EAchievement achievement, int amount, int target)
  {
    if (!SteamManager.Initialized)
      return;
    string pchName = achievement.ToString() + "_COUNTER";
    int pData = 0;
    SteamUserStats.GetStat(pchName, out pData);
    SteamUserStats.SetStat(pchName, pData + amount);
    SteamUserStats.IndicateAchievementProgress(achievement.ToString(), (uint) (pData + amount), (uint) target);
  }

  public override int GetIntStat(string statName)
  {
    int pData;
    return SteamUserStats.GetStat(statName, out pData) ? pData : -1;
  }

  public override void SetIntStat(string statName, int value)
  {
    SteamUserStats.SetStat(statName, value);
  }

  public override int UpdateIntStat(string statName, int change)
  {
    int pData = 0;
    SteamUserStats.GetStat(statName, out pData);
    int nData = pData + change;
    SteamUserStats.SetStat(statName, nData);
    return nData;
  }

  public override float GetFloatStat(string statName)
  {
    float pData;
    return SteamUserStats.GetStat(statName, out pData) ? pData : -1f;
  }

  public override void SetFloatStat(string statName, float value)
  {
    SteamUserStats.SetStat(statName, value);
  }

  public override float UpdateFloatStat(string statName, float change)
  {
    float pData = 0.0f;
    SteamUserStats.GetStat(statName, out pData);
    float fData = pData + change;
    SteamUserStats.SetStat(statName, fData);
    return fData;
  }

  public override void RequestGlobalStats() => SteamUserStats.RequestGlobalStats(0);

  public override float GetGlobalFloatStat(string statName)
  {
    double pData;
    return SteamUserStats.GetGlobalStat(statName, out pData) ? (float) pData : 0.0f;
  }

  public override int GetGlobalIntStat(string statName)
  {
    long pData;
    return SteamUserStats.GetGlobalStat(statName, out pData) ? (int) pData : 0;
  }

  public override bool GetAchievement(EAchievement achievement)
  {
    bool pbAchieved = false;
    if (SteamManager.Initialized)
      SteamUserStats.GetUserAchievement(this.steamID, achievement.ToString(), out pbAchieved);
    return pbAchieved;
  }

  public override void SetAchievement(EAchievement achievement)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetAchievement(achievement.ToString());
  }

  public override int GetDiseaseCompletion(Disease.EDiseaseType diseaseType)
  {
    return CPlayerInfoSteam.GetDiseaseCompletionStatic(diseaseType);
  }

  public static int GetDiseaseCompletionStatic(Disease.EDiseaseType diseaseType)
  {
    if (!SteamManager.Initialized)
      return -1;
    int pData = 0;
    SteamUserStats.GetStat("COMPLETION_" + diseaseType.ToString(), out pData);
    return pData - 1;
  }

  public override void SetDiseaseCompletion(Disease.EDiseaseType diseaseType, int value)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("COMPLETION_" + diseaseType.ToString(), value + 1);
  }

  public override bool GetDiseaseUnlocked(Disease.EDiseaseType diseaseType)
  {
    return diseaseType == Disease.EDiseaseType.BACTERIA || CPlayerInfoSteam.GetDiseaseUnlockedStatic(diseaseType);
  }

  public override bool GetMP_VS_DiseaseUnlocked(Disease.EDiseaseType diseaseType)
  {
    return diseaseType == Disease.EDiseaseType.BACTERIA || CPlayerInfoSteam.GetMP_VS_DiseaseUnlockedStatic(diseaseType);
  }

  public override bool GetMP_Coop_DiseaseUnlocked(Disease.EDiseaseType diseaseType)
  {
    return diseaseType == Disease.EDiseaseType.BACTERIA || CPlayerInfoSteam.GetMP_Coop_DiseaseUnlockedStatic(diseaseType);
  }

  public override int GetCureCompletion(Disease.ECureScenario cureScenario)
  {
    return CPlayerInfoSteam.GetCureCompletionStatic(cureScenario);
  }

  public static int GetCureCompletionStatic(Disease.ECureScenario cureScenario)
  {
    if (!SteamManager.Initialized)
      return -1;
    int pData = 0;
    SteamUserStats.GetStat("COMPLETION_" + cureScenario.ToString(), out pData);
    return pData - 1;
  }

  public override void SetCureCompletion(Disease.ECureScenario cureScenario, int value)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("COMPLETION_" + cureScenario.ToString(), value + 1);
  }

  public override bool GetCureUnlocked(Disease.ECureScenario cureScenario)
  {
    return cureScenario == Disease.ECureScenario.Cure_Standard || cureScenario == Disease.ECureScenario.Cure_DiseaseX || CPlayerInfoSteam.GetCureUnlockedStatic(cureScenario);
  }

  public static bool GetCureUnlockedStatic(Disease.ECureScenario cureScenario)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData;
    SteamUserStats.GetStat("UNLOCK_" + cureScenario.ToString(), out pData);
    return pData != 0;
  }

  public override void SetCureUnlocked(Disease.ECureScenario cureScenario)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("UNLOCK_" + cureScenario.ToString(), 1);
  }

  public override void SetCureLocked(Disease.ECureScenario cureScenario)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("UNLOCK_" + cureScenario.ToString(), 0);
  }

  public static bool GetDiseaseUnlockedStatic(Disease.EDiseaseType diseaseType)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData;
    SteamUserStats.GetStat("UNLOCK_" + diseaseType.ToString(), out pData);
    return pData != 0;
  }

  public static bool GetMP_VS_DiseaseUnlockedStatic(Disease.EDiseaseType diseaseType)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData;
    SteamUserStats.GetStat("UNLOCK_MP_" + diseaseType.ToString(), out pData);
    return pData != 0;
  }

  public static bool GetMP_Coop_DiseaseUnlockedStatic(Disease.EDiseaseType diseaseType)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData;
    SteamUserStats.GetStat("UNLOCK_MP_Coop_" + diseaseType.ToString(), out pData);
    return pData != 0;
  }

  public override bool GetCheatUnlocked(Disease.ECheatType cheatType)
  {
    IPlayerInfo localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
    switch (cheatType)
    {
      case Disease.ECheatType.SHUFFLE:
      case Disease.ECheatType.LUCKY_DIP:
      case Disease.ECheatType.DOUBLE_STRAIN:
        return localPlayerInfo.GetIntStat("UNLOCK_MEGA_CHEATS") == 1;
      case Disease.ECheatType.GOLDEN_HANDSHAKE:
      case Disease.ECheatType.ADVANCE_PLANNING:
      case Disease.ECheatType.FULL_SUPPORT:
      case Disease.ECheatType.MAXIMUM_POWER:
      case Disease.ECheatType.THE_AVENGERS:
        return localPlayerInfo.GetIntStat("UNLOCK_CURE_CHEATS") == 1;
      case Disease.ECheatType.CURE_SHUFFLE:
      case Disease.ECheatType.CURE_LUCKY_DIP:
        return localPlayerInfo.GetIntStat("UNLOCK_CURE_MEGA_CHEATS") == 1;
      default:
        return localPlayerInfo.GetIntStat("UNLOCK_BRUTAL_CHEATS") == 1;
    }
  }

  public override void SetDiseaseUnlocked(Disease.EDiseaseType diseaseType)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("UNLOCK_" + diseaseType.ToString(), 1);
  }

  public override bool GetScenarioUnlocked(string scenarioName)
  {
    return CPlayerInfoSteam.GetScenarioUnlockedStatic(scenarioName);
  }

  public static bool GetScenarioUnlockedStatic(string scenarioName)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData = 0;
    SteamUserStats.GetStat("UNLOCK_" + scenarioName, out pData);
    return pData != 0;
  }

  public override void SetScenarioUnlocked(string scenarioName)
  {
    if (!SteamManager.Initialized || scenarioName == null)
      return;
    string lowerInvariant = scenarioName.ToLowerInvariant();
    if (lowerInvariant.StartsWith("cure_") || lowerInvariant == "cure")
      return;
    SteamUserStats.SetStat("UNLOCK_" + scenarioName, 1);
  }

  public static void ClearAllSteamStats()
  {
    Disease.EDiseaseType[] values1 = (Disease.EDiseaseType[]) Enum.GetValues(typeof (Disease.EDiseaseType));
    for (int index = 0; index < values1.Length; ++index)
    {
      SteamUserStats.SetStat("COMPLETION_" + values1[index].ToString(), 0);
      SteamUserStats.SetStat("UNLOCK_MP_" + values1[index].ToString().ToUpper(), 0);
      SteamUserStats.SetStat("UNLOCK_" + values1[index].ToString().ToUpper(), 0);
    }
    Disease.ECureScenario[] values2 = (Disease.ECureScenario[]) Enum.GetValues(typeof (Disease.ECureScenario));
    for (int index = 0; index < values2.Length; ++index)
    {
      SteamUserStats.SetStat("COMPLETION_" + values2[index].ToString(), 0);
      SteamUserStats.SetStat("UNLOCK_" + values2[index].ToString(), 0);
    }
    List<Gene> geneList = DataImporter.ImportGenes(EncodedResources.Load("Data/Genes/genes").text);
    for (int index = 0; index < geneList.Count; ++index)
      SteamUserStats.SetStat("GENE_" + geneList[index].id, 0);
    foreach (ScenarioInformation officialScenario in CGameManager.OfficialScenarios)
      SteamUserStats.SetStat("UNLOCK_" + officialScenario.id, 0);
    SteamUserStats.SetStat("UNLOCK_MEGA_CHEATS", 0);
    SteamUserStats.SetStat("UNLOCK_BRUTAL_CHEATS", 0);
    SteamUserStats.SetStat("UNLOCK_CURE_MEGA_CHEATS", 0);
    SteamUserStats.SetStat("UNLOCK_CURE_CHEATS", 0);
    CSavesSteam.DeleteCloudData("completion.dat");
  }

  public static void ClearAchievements()
  {
    foreach (EAchievement eachievement in (EAchievement[]) Enum.GetValues(typeof (EAchievement)))
      SteamUserStats.ClearAchievement(eachievement.ToString());
  }

  public override void SetGeneUnlocked(Gene gene)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("GENE_" + gene.id, 1);
  }

  public override void SetMP_Coop_GeneUnlocked(Gene gene)
  {
    if (!SteamManager.Initialized)
      return;
    SteamUserStats.SetStat("GENE_Coop_" + gene.id, 1);
  }

  public override bool GetGeneUnlocked(Gene gene)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData = 0;
    SteamUserStats.GetStat("GENE_" + gene.id, out pData);
    return pData != 0;
  }

  public override bool GetMP_Coop_GeneUnlocked(Gene gene)
  {
    if (!SteamManager.Initialized)
      return false;
    int pData = 0;
    SteamUserStats.GetStat("GENE_Coop_" + gene.id, out pData);
    return pData != 0;
  }

  public override string GetPlayerLanguage()
  {
    Debug.Log((object) SteamApps.GetCurrentGameLanguage());
    return CLocalisationManager.SteamLanguageConversion(SteamApps.GetCurrentGameLanguage());
  }

  public override void UpdateCheatProgress(
    Disease.ECheatType cheatType,
    int difficulty,
    Disease.EDiseaseType diseaseType)
  {
    if (this.cheatCompletionMap == null)
      this.LoadMyCheatCompletion();
    string key = "CheatCompletion_" + (object) cheatType + "_" + (object) difficulty;
    int num1 = 0;
    if (this.cheatCompletionMap.ContainsKey(key))
      num1 = this.cheatCompletionMap[key];
    bool flag = true;
    int num2 = num1 + 1;
    this.cheatCompletionMap[key] = num2;
    int cheatCompletion1 = this.GetCheatCompletion(cheatType);
    if (difficulty > cheatCompletion1)
    {
      flag = true;
      this.cheatCompletionMap["CheatRating_" + (object) cheatType] = difficulty;
    }
    int cheatCompletion2 = this.GetCheatCompletion(cheatType, diseaseType);
    if (difficulty > cheatCompletion2)
    {
      flag = true;
      this.cheatCompletionMap["CheatRating_" + (object) cheatType + "_" + (object) diseaseType] = difficulty;
    }
    if (!flag)
      return;
    this.SaveMyCheatCompletion();
  }

  public override int GetCheatCompletionCount(Disease.ECheatType cheatType, int difficulty)
  {
    if (this.cheatCompletionMap == null)
      this.LoadMyCheatCompletion();
    string key = "CheatCompletion_" + (object) cheatType + "_" + (object) difficulty;
    return !this.cheatCompletionMap.ContainsKey(key) ? 0 : this.cheatCompletionMap[key];
  }

  public override int GetCheatCompletion(Disease.ECheatType cheatType)
  {
    if (this.cheatCompletionMap == null)
      this.LoadMyCheatCompletion();
    string key = "CheatRating_" + (object) cheatType;
    return !this.cheatCompletionMap.ContainsKey(key) ? -1 : this.cheatCompletionMap[key];
  }

  public override int GetCheatCompletion(
    Disease.ECheatType cheatType,
    Disease.EDiseaseType diseaseType)
  {
    if (this.cheatCompletionMap == null)
      this.LoadMyCheatCompletion();
    string key = "CheatRating_" + (object) cheatType + "_" + (object) diseaseType;
    return !this.cheatCompletionMap.ContainsKey(key) ? -1 : this.cheatCompletionMap[key];
  }

  public override void SetScenarioRating(
    ScenarioInformation scenarioInformation,
    int difficulty,
    Disease.EDiseaseType diseaseType,
    int rating)
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    bool flag = false;
    int scenarioRating = this.GetScenarioRating(scenarioInformation, difficulty);
    if (rating > scenarioRating)
    {
      flag = true;
      this.scenarioCompletionMap["ScenarioRating_" + scenarioInformation.scenTitle + "_" + difficulty.ToString()] = rating;
    }
    int scenarioDiseaseRating = this.GetScenarioDiseaseRating(scenarioInformation, difficulty, diseaseType);
    if (rating > scenarioDiseaseRating)
    {
      flag = true;
      this.scenarioCompletionMap["ScenarioDiseaseRating_" + scenarioInformation.scenTitle + "_" + difficulty.ToString() + "_" + diseaseType.ToString()] = rating;
    }
    int diseaseCompletion = this.GetScenarioDiseaseCompletion(scenarioInformation, diseaseType);
    if (difficulty > diseaseCompletion)
    {
      flag = true;
      this.scenarioCompletionMap["ScenarioDiseaseCompletion_" + scenarioInformation.scenTitle + "_" + diseaseType.ToString()] = difficulty;
    }
    if (!flag)
      return;
    this.SaveMyScenarioCompletion();
  }

  public override int GetScenarioRating(ScenarioInformation scenarioInformation, int difficulty)
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    string key = "ScenarioRating_" + scenarioInformation.scenTitle + "_" + difficulty.ToString();
    return this.scenarioCompletionMap.ContainsKey(key) ? this.scenarioCompletionMap[key] : 0;
  }

  public override bool GetScenarioComplete(string scenTitle)
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    for (int index = 0; index < 4; ++index)
    {
      string key = "ScenarioRating_" + scenTitle + "_" + index.ToString();
      if (this.scenarioCompletionMap.ContainsKey(key))
        return this.scenarioCompletionMap[key] > 0;
    }
    return false;
  }

  public override int GetScenarioDiseaseRating(
    ScenarioInformation scenarioInformation,
    int difficulty,
    Disease.EDiseaseType diseaseType)
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    string key = "ScenarioDiseaseRating_" + scenarioInformation.scenTitle + "_" + difficulty.ToString() + "_" + diseaseType.ToString();
    return this.scenarioCompletionMap.ContainsKey(key) ? this.scenarioCompletionMap[key] : 0;
  }

  public override int GetScenarioDiseaseCompletion(
    ScenarioInformation scenarioInformation,
    Disease.EDiseaseType diseaseType)
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    string key = "ScenarioDiseaseCompletion_" + scenarioInformation.scenTitle + "_" + diseaseType.ToString();
    return this.scenarioCompletionMap.ContainsKey(key) ? this.scenarioCompletionMap[key] : -1;
  }

  public override void SaveMyScenarioCompletion()
  {
    if (this.scenarioCompletionMap == null)
      this.LoadMyScenarioCompletion();
    string data = "";
    foreach (KeyValuePair<string, int> scenarioCompletion in this.scenarioCompletionMap)
      data = data + CUtils.Base64Encode(scenarioCompletion.Key + ":" + (object) scenarioCompletion.Value) + "\n";
    CSavesSteam.SaveCloudData("completion.dat", data);
  }

  public override void LoadMyScenarioCompletion()
  {
    this.scenarioCompletionMap = new Dictionary<string, int>();
    string str1 = (string) null;
    if (CSavesSteam.ExistsCloudData("completion.dat"))
      str1 = CSavesSteam.LoadCloudData("completion.dat");
    if (string.IsNullOrEmpty(str1))
      return;
    string str2 = str1;
    char[] chArray = new char[1]{ '\n' };
    foreach (string str3 in str2.Split(chArray))
    {
      if (!string.IsNullOrEmpty(str3))
      {
        try
        {
          string[] strArray = CUtils.Base64Decode(str3).Split(':');
          this.scenarioCompletionMap.Add(strArray[0], int.Parse(strArray[1]));
        }
        catch
        {
          Debug.Log((object) "Completion Line Load Error");
        }
      }
    }
  }

  public override void SaveMyCheatCompletion()
  {
    if (this.cheatCompletionMap == null)
      this.LoadMyCheatCompletion();
    string data = "";
    foreach (KeyValuePair<string, int> cheatCompletion in this.cheatCompletionMap)
      data = data + CUtils.Base64Encode(cheatCompletion.Key + ":" + (object) cheatCompletion.Value) + "\n";
    CSavesSteam.SaveCloudData("cheatCompletion.dat", data);
  }

  public override void LoadMyCheatCompletion()
  {
    this.cheatCompletionMap = new Dictionary<string, int>();
    string str1 = (string) null;
    if (CSavesSteam.ExistsCloudData("cheatCompletion.dat"))
      str1 = CSavesSteam.LoadCloudData("cheatCompletion.dat");
    if (string.IsNullOrEmpty(str1))
      return;
    string str2 = str1;
    char[] chArray = new char[1]{ '\n' };
    foreach (string str3 in str2.Split(chArray))
    {
      if (!string.IsNullOrEmpty(str3))
      {
        try
        {
          string[] strArray = CUtils.Base64Decode(str3).Split(':');
          this.cheatCompletionMap.Add(strArray[0], int.Parse(strArray[1]));
        }
        catch
        {
          Debug.Log((object) "Completion Line Load Error");
        }
      }
    }
  }

  public void PopulateMultiplayerStats() => SteamUserStats.RequestUserStats(this.steamID);

  public override int GetMultiplayerRating() => this.playerStats.MP_Rating;

  public override int GetHighestMultiplayerRating(IGame.GameType gameType, int difficulty)
  {
    if (gameType == IGame.GameType.VersusMP)
      return this.playerStats.MP_HighestRating;
    return gameType == IGame.GameType.CoopMP ? this.playerStats.MP_Coop_BestScore_v4_1 : -1;
  }

  public override string GetHighestMultiplayerBadge(IGame.GameType gameType, int difficulty)
  {
    int multiplayerRating = this.GetHighestMultiplayerRating(gameType, difficulty);
    int[] numArray1 = new int[5];
    switch (gameType)
    {
      case IGame.GameType.VersusMP:
        int[] numArray2 = new int[5]
        {
          1700,
          1400,
          1100,
          1030,
          1000
        };
        string str1 = "MP_Badge_Rank_0";
        for (int index = 0; index < numArray2.Length; ++index)
        {
          if (multiplayerRating > numArray2[index])
            return str1 + (object) (6 - index);
        }
        return str1 + "1";
      case IGame.GameType.CoopMP:
        int[] numArray3 = new int[5]
        {
          350,
          430,
          600,
          700,
          1000
        };
        string str2 = "MP_Badge_Rank_0";
        if (multiplayerRating == 0)
          return str2 + "1";
        for (int index = 0; index < numArray3.Length; ++index)
        {
          Debug.Log((object) ("highestRating:" + (object) multiplayerRating + ", ratingBounds[i]:" + (object) numArray3[index]));
          if (multiplayerRating <= numArray3[index])
            return str2 + (object) (6 - index);
        }
        return str2 + "1";
      default:
        return "MP_Badge_Rank_01";
    }
  }

  protected override void UpdateMultiplayerRating(int newRating)
  {
    this.playerStats.MP_Rating = newRating;
    if (newRating > this.playerStats.MP_HighestRating)
      this.playerStats.MP_HighestRating = newRating;
    Debug.Log((object) ("Update player rating: " + (object) newRating));
  }

  public override void RegisterCooperativeGameStart(CoopDisease myDisease, int opponentRating)
  {
    ++this.playerStats.MP_Coop_GamesPlayed;
    switch (myDisease.diseaseType)
    {
      case Disease.EDiseaseType.BACTERIA:
        ++this.playerStats.MP_Coop_Disease_BACTERIA;
        break;
      case Disease.EDiseaseType.VIRUS:
        ++this.playerStats.MP_Coop_Disease_VIRUS;
        break;
      case Disease.EDiseaseType.FUNGUS:
        ++this.playerStats.MP_Coop_Disease_FUNGUS;
        break;
      case Disease.EDiseaseType.PARASITE:
        ++this.playerStats.MP_Coop_Disease_PARASITE;
        break;
      case Disease.EDiseaseType.BIO_WEAPON:
        ++this.playerStats.MP_Coop_Disease_BIO_WEAPON;
        break;
    }
    if (myDisease.genes == null)
      return;
    foreach (Gene gene in myDisease.genes)
    {
      int pData = 0;
      SteamUserStats.GetStat("GENE_Coop_" + gene.id, out pData);
      ++pData;
      SteamUserStats.SetStat("GENE_Coop_" + gene.id, pData);
    }
  }

  public override void RegisterMultiplayerGameStart(
    MPDisease myDisease,
    int opponentRating,
    bool isRatingGame)
  {
    Debug.Log((object) ("RegisterMultiplayerGameStart - isRatingGame:" + isRatingGame.ToString()));
    if (isRatingGame)
      ++this.playerStats.MP_GamesPlayed;
    switch (myDisease.diseaseType)
    {
      case Disease.EDiseaseType.BACTERIA:
        ++this.playerStats.MP_Disease_BACTERIA;
        break;
      case Disease.EDiseaseType.VIRUS:
        ++this.playerStats.MP_Disease_VIRUS;
        break;
      case Disease.EDiseaseType.FUNGUS:
        ++this.playerStats.MP_Disease_FUNGUS;
        break;
      case Disease.EDiseaseType.PARASITE:
        ++this.playerStats.MP_Disease_PARASITE;
        break;
      case Disease.EDiseaseType.BIO_WEAPON:
        ++this.playerStats.MP_Disease_BIO_WEAPON;
        break;
    }
    if (myDisease.genes != null)
    {
      foreach (Gene gene in myDisease.genes)
      {
        int pData = 0;
        SteamUserStats.GetStat("GENE_" + gene.id, out pData);
        int nData = pData + 1;
        SteamUserStats.SetStat("GENE_" + gene.id, nData);
      }
    }
    this.initialMultiplayerGameRanking = 0;
    if (isRatingGame)
    {
      int multiplayerRating = this.GetMultiplayerRating();
      this.initialMultiplayerGameRanking = multiplayerRating;
      Debug.Log((object) ("Preprocess loss in case of abandon vs opponent: " + (object) opponentRating + " current rating: " + (object) multiplayerRating));
      CPlayerInfoSteam.CalculateResult(false, ref multiplayerRating, opponentRating);
      this.UpdateMultiplayerRating(multiplayerRating);
    }
    CNetworkManager.network.SaveLocalPlayerData();
  }

  public override void ProcessInvalidGame(bool isRatingGame)
  {
    Debug.Log((object) nameof (ProcessInvalidGame));
    if (!isRatingGame || this.initialMultiplayerGameRanking <= 0)
      return;
    --this.playerStats.MP_GamesPlayed;
    this.UpdateMultiplayerRating(this.initialMultiplayerGameRanking);
    this.initialMultiplayerGameRanking = 0;
    CNetworkManager.network.SaveLocalPlayerData();
  }

  public override void ProcessMultiplayerGameResult(
    Disease disease,
    MultiplayerGame.ResultType resultType,
    int opponentRating,
    bool isRatingGame)
  {
    Debug.Log((object) ("ProcessMultiplayerGameResult - resultType:" + (object) resultType + ", isRatingGame:" + isRatingGame.ToString() + ", opponentRating:" + (object) opponentRating));
    if (resultType == MultiplayerGame.ResultType.WIN)
    {
      ++this.playerStats.MP_DiseaseUnlockCounter;
      switch (disease.diseaseType)
      {
        case Disease.EDiseaseType.BACTERIA:
          ++this.playerStats.MP_Wins_Disease_BACTERIA;
          break;
        case Disease.EDiseaseType.VIRUS:
          ++this.playerStats.MP_Wins_Disease_VIRUS;
          break;
        case Disease.EDiseaseType.FUNGUS:
          ++this.playerStats.MP_Wins_Disease_FUNGUS;
          break;
        case Disease.EDiseaseType.PARASITE:
          ++this.playerStats.MP_Wins_Disease_PARASITE;
          break;
        case Disease.EDiseaseType.BIO_WEAPON:
          ++this.playerStats.MP_Wins_Disease_BIO_WEAPON;
          break;
      }
    }
    if (isRatingGame)
    {
      ++this.playerStats.MP_GamesCompleted;
      if (this.initialMultiplayerGameRanking == 0)
      {
        Debug.LogError((object) "****** Initial MP Game Ranking for local player is 0! Do not adjust stats!");
      }
      else
      {
        int multiplayerGameRanking = this.initialMultiplayerGameRanking;
        switch (resultType)
        {
          case MultiplayerGame.ResultType.WIN:
            ++this.playerStats.MP_GamesWon;
            CPlayerInfoSteam.CalculateResult(true, ref multiplayerGameRanking, opponentRating);
            this.UpdateMultiplayerRating(multiplayerGameRanking);
            break;
          case MultiplayerGame.ResultType.LOSE:
            CPlayerInfoSteam.CalculateResult(false, ref multiplayerGameRanking, opponentRating);
            this.UpdateMultiplayerRating(multiplayerGameRanking);
            break;
          case MultiplayerGame.ResultType.DRAW:
            ++this.playerStats.MP_GamesDrawn;
            this.UpdateMultiplayerRating(this.initialMultiplayerGameRanking);
            break;
        }
        this.initialMultiplayerGameRanking = 0;
      }
    }
    CNetworkManager.network.SaveLocalPlayerData();
  }

  public static void CalculateResult(bool isWin, ref int p1Rating, int p2Rating)
  {
    int b;
    if (p1Rating == 0)
    {
      Debug.LogError((object) "**** P1Rating is zero! Rating will not update.");
      b = 0;
    }
    else if (p2Rating == 0)
    {
      Debug.LogError((object) "**** P2Rating is zero! Rating will not update.");
      b = 0;
    }
    else
    {
      float num1 = 32f;
      if ((double) p1Rating > 2000.0)
        num1 = 24f;
      else if ((double) p1Rating > 2400.0)
        num1 = 16f;
      double num2 = (double) Mathf.Pow(10f, (float) p1Rating / 400f);
      float num3 = (float) (num2 / (num2 + (double) Mathf.Pow(10f, (float) p2Rating / 400f)));
      float num4 = isWin ? 1f : 0.0f;
      b = Mathf.FloorToInt(num1 * (num4 - num3));
      if (isWin)
        b = Mathf.Max(1, b);
      if (b > 100)
        b = 100;
      if (b < -100)
        b = -100;
    }
    int num = p1Rating + b;
    if (num < 1)
      num = 1;
    if (num > 10000)
      num = 10000;
    Debug.Log((object) ("Rating Calc: " + (object) p1Rating + " vs " + (object) p2Rating + " win? " + isWin.ToString() + " => " + (object) num));
    p1Rating = num;
  }

  public override void ForceRatingEdit(int targetRating, bool isRatingGame)
  {
    Debug.Log((object) ("RegisterMultiplayerGameStart - isRatingGame:" + isRatingGame.ToString()));
    this.initialMultiplayerGameRanking = 0;
    this.initialMultiplayerGameRanking = this.GetMultiplayerRating();
    Debug.Log((object) ("Preprocess win in case of f*cking game bugs: " + (object) targetRating + " current rating: " + (object) targetRating));
    this.UpdateMultiplayerRating(targetRating);
    CNetworkManager.network.SaveLocalPlayerData();
  }
}
