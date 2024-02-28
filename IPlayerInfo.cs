// Decompiled with JetBrains decompiler
// Type: IPlayerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public abstract class IPlayerInfo
{
  public string name;
  public bool isInGame;
  public bool isOnline;
  public string playerStatus;
  public Color diseaseColor;
  [NonSerialized]
  public Disease disease;
  public int gameSpeed = 1;
  public PlayerStats playerStats = new PlayerStats();
  [NonSerialized]
  public Texture2D avatar;
  [NonSerialized]
  public Texture2D largeAvatar;
  [NonSerialized]
  public bool statsLoaded;

  public abstract string PlayerID { get; }

  public abstract void IncrementAchievementStat(EAchievement achievement, int amount, int target);

  public abstract bool GetAchievement(EAchievement achievement);

  public abstract void SetAchievement(EAchievement achievement);

  public abstract int GetDiseaseCompletion(Disease.EDiseaseType diseaseType);

  public abstract void SetDiseaseCompletion(Disease.EDiseaseType diseaseType, int value);

  public abstract bool GetDiseaseUnlocked(Disease.EDiseaseType diseaseType);

  public abstract int GetCureCompletion(Disease.ECureScenario cureScenario);

  public abstract void SetCureCompletion(Disease.ECureScenario cureScenario, int value);

  public abstract bool GetCureUnlocked(Disease.ECureScenario cureScenario);

  public abstract void SetCureUnlocked(Disease.ECureScenario cureScenario);

  public abstract void SetCureLocked(Disease.ECureScenario cureScenario);

  public abstract bool GetMP_VS_DiseaseUnlocked(Disease.EDiseaseType diseaseType);

  public abstract bool GetMP_Coop_DiseaseUnlocked(Disease.EDiseaseType diseaseType);

  public abstract void SetDiseaseUnlocked(Disease.EDiseaseType diseaseType);

  public abstract bool GetScenarioUnlocked(string scenarioName);

  public abstract void SetScenarioUnlocked(string scenarioName);

  public abstract void SetGeneUnlocked(Gene gene);

  public abstract void SetMP_Coop_GeneUnlocked(Gene gene);

  public abstract bool GetGeneUnlocked(Gene gene);

  public abstract bool GetMP_Coop_GeneUnlocked(Gene gene);

  public abstract int GetIntStat(string statName);

  public abstract void SetIntStat(string statName, int value);

  public abstract float GetFloatStat(string statName);

  public abstract void SetFloatStat(string statName, float value);

  public abstract void RequestGlobalStats();

  public abstract int GetGlobalIntStat(string statName);

  public abstract float GetGlobalFloatStat(string statName);

  public abstract int UpdateIntStat(string statName, int change);

  public abstract float UpdateFloatStat(string statName, float change);

  public abstract bool GetCheatUnlocked(Disease.ECheatType cheatType);

  public abstract void UpdateCheatProgress(
    Disease.ECheatType cheatType,
    int difficulty,
    Disease.EDiseaseType diseaseType);

  public abstract int GetCheatCompletionCount(Disease.ECheatType cheatType, int difficulty);

  public abstract int GetCheatCompletion(Disease.ECheatType cheatType);

  public abstract int GetCheatCompletion(
    Disease.ECheatType cheatType,
    Disease.EDiseaseType diseaseType);

  public abstract int GetScenarioRating(ScenarioInformation scenarioInformation, int difficulty);

  public abstract void SetScenarioRating(
    ScenarioInformation scenarioInformation,
    int difficulty,
    Disease.EDiseaseType diseaseType,
    int rating);

  public abstract bool GetScenarioComplete(string scenTitle);

  public abstract int GetScenarioDiseaseRating(
    ScenarioInformation scenarioInformation,
    int difficulty,
    Disease.EDiseaseType diseaseType);

  public abstract int GetScenarioDiseaseCompletion(
    ScenarioInformation scenarioInformation,
    Disease.EDiseaseType diseaseType);

  public abstract void SaveMyScenarioCompletion();

  public abstract void LoadMyScenarioCompletion();

  public abstract void SaveMyCheatCompletion();

  public abstract void LoadMyCheatCompletion();

  public abstract string GetPlayerLanguage();

  public abstract void RegisterMultiplayerGameStart(
    MPDisease myDisease,
    int opponentRating,
    bool isRatingGame);

  public abstract void RegisterCooperativeGameStart(CoopDisease myDisease, int opponentRating);

  public abstract int GetMultiplayerRating();

  public abstract int GetHighestMultiplayerRating(IGame.GameType gameType, int difficulty);

  public abstract string GetHighestMultiplayerBadge(IGame.GameType gameType, int difficulty);

  protected abstract void UpdateMultiplayerRating(int newRating);

  public abstract void ProcessInvalidGame(bool isRatingGame);

  public abstract void ProcessMultiplayerGameResult(
    Disease disease,
    MultiplayerGame.ResultType resultType,
    int opponentRating,
    bool isRatingGame);

  public List<Disease.EDiseaseType> GetUnlockedDiseases()
  {
    Disease.EDiseaseType[] values = Enum.GetValues(typeof (Disease.EDiseaseType)) as Disease.EDiseaseType[];
    List<Disease.EDiseaseType> unlockedDiseases = new List<Disease.EDiseaseType>();
    foreach (Disease.EDiseaseType diseaseType in values)
    {
      if (diseaseType != Disease.EDiseaseType.TUTORIAL && this.HasUnlockedDisease(diseaseType))
        unlockedDiseases.Add(diseaseType);
    }
    return unlockedDiseases;
  }

  public List<Disease.EDiseaseType> GetUnlockedCureDiseases()
  {
    Disease.ECureScenario[] values = Enum.GetValues(typeof (Disease.ECureScenario)) as Disease.ECureScenario[];
    List<Disease.EDiseaseType> unlockedCureDiseases = new List<Disease.EDiseaseType>();
    foreach (Disease.ECureScenario ecureScenario in values)
    {
      if (ecureScenario != Disease.ECureScenario.None && this.GetCureUnlocked(ecureScenario))
        unlockedCureDiseases.Add(CGameManager.GetCureDiseaseType(ecureScenario));
    }
    return unlockedCureDiseases;
  }

  public bool HasUnlockedDisease(Disease.EDiseaseType diseaseType)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.BACTERIA:
      case Disease.EDiseaseType.TUTORIAL:
        return true;
      case Disease.EDiseaseType.SIMIAN_FLU:
        this.SetDiseaseUnlocked(Disease.EDiseaseType.SIMIAN_FLU);
        return true;
      case Disease.EDiseaseType.VAMPIRE:
        this.SetDiseaseUnlocked(Disease.EDiseaseType.VAMPIRE);
        return true;
      default:
        return this.GetDiseaseUnlocked(diseaseType);
    }
  }

  public bool CompletedEveryDiseaseInScenarioWith3Biohazards(ScenarioInformation scenarioInformation)
  {
    bool flag1 = true;
    foreach (Disease.EDiseaseType activeDisease in CGameManager.ActiveDiseases)
    {
      if (activeDisease != Disease.EDiseaseType.DISEASEX)
      {
        bool flag2 = false;
        for (int difficulty = 1; difficulty < 4; ++difficulty)
        {
          if (this.GetScenarioDiseaseRating(scenarioInformation, difficulty, activeDisease) >= 3)
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
        {
          Debug.Log((object) (scenarioInformation.id + " No mega achievement: " + (object) activeDisease + " not completed"));
          flag1 = false;
          break;
        }
      }
    }
    return flag1;
  }

  public virtual int[] GetScenarioStars(
    ScenarioInformation scenarioInformation,
    Disease.EDiseaseType? diseaseType = null)
  {
    int[] scenarioStars = new int[3];
    for (int index = 0; index < scenarioStars.Length; ++index)
      scenarioStars[index] = -1;
    int[] numArray = new int[4];
    for (int difficulty = 0; difficulty < numArray.Length; ++difficulty)
      numArray[difficulty] = !diseaseType.HasValue ? this.GetScenarioRating(scenarioInformation, difficulty) : this.GetScenarioDiseaseRating(scenarioInformation, difficulty, diseaseType.Value);
    for (int index1 = 0; index1 < numArray.Length; ++index1)
    {
      int num = numArray[index1];
      for (int index2 = 0; index2 < num; ++index2)
        scenarioStars[index2] = index1;
    }
    return scenarioStars;
  }

  public bool HasUnlockedScenario(string scenarioName)
  {
    return CGameManager.DefaultScenarios.Contains(scenarioName) || this.GetScenarioUnlocked(scenarioName);
  }

  public abstract void ForceRatingEdit(int opponentRating, bool isRatingGame);
}
