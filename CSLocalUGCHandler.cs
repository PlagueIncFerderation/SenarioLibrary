// Decompiled with JetBrains decompiler
// Type: CSLocalUGCHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class CSLocalUGCHandler
{
  public List<ScenarioInformation> cachedScenariosInformation = new List<ScenarioInformation>();
  public Dictionary<string, int> scenarioConstList;
  public List<ScenarioInformation> subsetScenarios;

  public bool CacheLocalScenarioData()
  {
    CGameManager.GetRatingList();
    this.cachedScenariosInformation.Clear();
    string[] localFolders = this.GetLocalFolders();
    CGameManager.scenarioNameTitlePair = new Dictionary<string, string>();
    if (localFolders != null)
    {
      foreach (string str1 in localFolders)
      {
        if (CSLocalUGCHandler.LocalScenarioInfoExists(str1))
        {
          ScenarioInformation scenarioInformation = this.GetScenarioInformation(str1);
          scenarioInformation.originScenTitle = scenarioInformation.scenTitle;
          scenarioInformation.scenarioTag = "Default";
          scenarioInformation.scenarioLevel = 0;
          scenarioInformation.scenarioConst = CGameManager.GetScenarioConst(str1);
          string scenarioDifficultyRaw = this.GetScenarioDifficultyRaw(str1);
          scenarioInformation.difficultyRaw = scenarioDifficultyRaw;
          scenarioInformation.levelString = "";
          scenarioInformation.completed = false;
          scenarioInformation.titleWithConst = scenarioInformation.scenTitle;
          scenarioInformation.titleWithLevel = scenarioInformation.scenTitle;
          scenarioInformation.scoreUnlocked = true;
          scenarioInformation.pinyinName = "核弹";
          scenarioInformation.finalLevelString = "";
          if (CGameManager.scenarioName != null && CGameManager.scenarioName.ContainsKey(str1))
            scenarioInformation.pinyinName = CGameManager.scenarioName[str1];
          CGameManager.scenarioNameTitlePair.Add(str1, scenarioInformation.originScenTitle);
          string difficulty1 = "";
          string difficulty2 = "";
          if (str1.IndexOf("PIFSL") != -1 && (CGameManager.constedScenarioList.Contains(str1) || str1.Contains("时生虫ReCRAFT")))
          {
            string scenarioLevelString = CSLocalUGCHandler.GetScenarioLevelString(CGameManager.GetScenarioConst(str1));
            scenarioInformation.levelString = scenarioLevelString;
            string append = "";
            if (scenarioDifficultyRaw.Contains("BYD"))
              append = "BYD";
            if (scenarioDifficultyRaw.Contains("FTR"))
              append = "FTR";
            if (scenarioDifficultyRaw.Contains("PRS"))
              append = "PRS";
            if (scenarioDifficultyRaw.Contains("PST"))
              append = "PST";
            if (scenarioDifficultyRaw.Contains("MXM"))
              append = "MXM";
            int num = CSLocalUGCHandler.GetScenarioLevel(scenarioLevelString, append);
            if (str1.Contains("时生虫ReCRAFT"))
              num = 174;
            scenarioInformation.scenarioLevel = num;
            difficulty1 = append + " " + ((float) CGameManager.GetScenarioConst(str1) / 10f).ToString("N1");
            difficulty2 = append + " " + scenarioLevelString;
          }
          else
            scenarioInformation.scenarioLevel = -1;
          if (str1.IndexOf("PIFSL") != -1 && (CGameManager.constedScenarioList.Contains(str1) || str1.Contains("时生虫ReCRAFT")))
          {
            if (!str1.Contains("时生虫ReCRAFT") || str1.Contains("时生虫ReCRAFTa"))
            {
              if (!string.IsNullOrEmpty(CSLocalUGCHandler.GetScenarioAuthor(str1)))
                scenarioInformation.scenAuthorName = CSLocalUGCHandler.GetScenarioAuthor(str1);
              int scenarioHighScore = CSLocalUGCHandler.GetScenarioHighScore(str1, true);
              if (scenarioHighScore > 0)
                scenarioInformation.completed = true;
              if ((!str1.Contains("时生虫ReMASTER") || CSLocalUGCHandler.GetScenarioProperty("PIFSL 命运之门", "unlocked", false).Equals("1")) && (!str1.Contains("时生虫ReCRAFT") || CSLocalUGCHandler.GetScenarioProperty("PIFSL 命运之门", "unlocked", false).Equals("2")))
              {
                string scenarioLevelString = CSLocalUGCHandler.GetScenarioLevelString(CGameManager.GetScenarioConst(str1));
                string str2 = scenarioDifficultyRaw;
                if ((CGameManager.FilterScenarioComplete.Equals("all") || (!CGameManager.FilterScenarioComplete.Equals("completed") || scenarioHighScore >= 1) && (!CGameManager.FilterScenarioComplete.Equals("not completed") || scenarioHighScore <= 0)) && (CGameManager.FilterScenarioLevel.Equals("all") || scenarioLevelString.Equals(CGameManager.FilterScenarioLevel)) && (CGameManager.FilterScenarioType.Equals("all") || str2.Contains(CGameManager.FilterScenarioType)))
                {
                  scenarioInformation.scenTitle = "[" + this.GetScenarioDifficulty(difficulty2) + "] " + this.GetRank(scenarioHighScore) + " [HS:" + scenarioHighScore.ToString("N0").Replace(',', '\'') + "] " + scenarioInformation.originScenTitle;
                  if (!this.GetUnlocked(str1))
                  {
                    scenarioInformation.scenTitle = "[[7f94d5]Locked[ffffff]]" + scenarioInformation.originScenTitle;
                    scenarioInformation.scoreUnlocked = false;
                  }
                  else
                    scenarioInformation.scoreUnlocked = true;
                  scenarioInformation.finalLevelString = this.GetScenarioDifficulty(scenarioInformation.difficultyRaw.Substring(0, 3) + " " + scenarioInformation.levelString);
                  scenarioInformation.titleWithLevel = "[" + this.GetScenarioDifficulty(scenarioInformation.difficultyRaw.Substring(0, 3) + " " + scenarioInformation.levelString) + "] " + this.GetRank(scenarioHighScore) + " [HS:" + scenarioHighScore.ToString("N0").Replace(',', '\'') + "] " + scenarioInformation.originScenTitle;
                  scenarioInformation.titleWithConst = "[" + this.GetScenarioDifficulty(difficulty1) + "] " + this.GetRank(scenarioHighScore) + " [HS:" + scenarioHighScore.ToString("N0").Replace(',', '\'') + "] " + scenarioInformation.originScenTitle;
                  scenarioInformation.titleWithRating = "[" + (this.GetScenarioDifficulty(difficulty1) + "→" + CGameManager.GetRating(scenarioHighScore, CGameManager.GetScenarioConst(str1)).ToString("N4")) + "] " + this.GetRank(scenarioHighScore) + " [HS:" + scenarioHighScore.ToString("N0").Replace(',', '\'') + "] " + scenarioInformation.originScenTitle;
                  scenarioInformation.scenarioTag = "PIFSL";
                }
                else
                  continue;
              }
              else
                continue;
            }
            else
              continue;
          }
          if (CGameManager.FilterScenarioComplete.Equals("all") && CGameManager.FilterScenarioLevel.Equals("all") && CGameManager.FilterScenarioType.Equals("all") || str1.IndexOf("PIFSL") != -1 && CGameManager.constedScenarioList.Contains(str1))
          {
            if (str1.IndexOf("[Local]") != -1)
            {
              scenarioInformation.scenTitle = "[" + ":00ff00]" + "Local" + ":ffffff]" + "] " + scenarioInformation.scenTitle;
              scenarioInformation.scenarioTag = "Local";
            }
            if (str1.IndexOf("[Legacy]") != -1 || str1.Contains("PIFSL") && !CGameManager.constedScenarioList.Contains(str1) && !str1.Contains("时生虫ReCRAFT"))
            {
              scenarioInformation.scenTitle = "[" + ":999999]" + "Legacy" + ":ffffff]" + "] " + scenarioInformation.scenTitle;
              scenarioInformation.scenarioTag = "Legacy";
            }
            if (str1.Contains("PIFCURE"))
              scenarioInformation.scenTitle = "[" + ":33ffff]" + "CURE" + ":ffffff]" + "] " + scenarioInformation.scenTitle;
            scenarioInformation.fileName = str1;
            this.cachedScenariosInformation.Add(scenarioInformation);
          }
        }
      }
    }
    this.cachedScenariosInformation.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenario));
    return localFolders != null && localFolders.Length != 0;
  }

  public Scenario LocalCreateScenario(string filename)
  {
    Scenario scenario = Scenario.LoadScenario(filename, false, false, Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename));
    if (scenario != null)
      scenario.isOfficial = false;
    return scenario;
  }

  public void GetLocalImageFromFile(
    string filename,
    string image,
    PNGLoader.TextureLoaded onTextureLoaded)
  {
    filename = Path.GetFileName(filename);
    PNGLoader.instance.LocalLoadPNG(image, Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename), onTextureLoaded, -1, -1);
  }

  public string[] GetLocalFiles(string savename, string extension = ".txt")
  {
    savename = Path.GetFileName(savename);
    string[] fileSystemEntries = Directory.GetFileSystemEntries(Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), savename), "*" + extension);
    for (int index = 0; index < fileSystemEntries.Length; ++index)
      fileSystemEntries[index] = Path.GetFileName(fileSystemEntries[index]);
    return fileSystemEntries;
  }

  private string[] GetLocalFolders()
  {
    string[] directories = Directory.GetDirectories(CSLocalUGCHandler.GetScenarioDataPath());
    string[] localFolders = new string[directories.Length];
    for (int index = 0; index < localFolders.Length; ++index)
      localFolders[index] = Path.GetFileName(directories[index]);
    return localFolders;
  }

  private ScenarioInformation GetScenarioInformation(string filename)
  {
    filename = Path.GetFileName(filename);
    string str = Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename);
    ScenarioInformation scenarioInformation = (ScenarioInformation) null;
    if (File.Exists(str + "/scenario.txt"))
    {
      StreamReader streamReader = File.OpenText(str + "/scenario.txt");
      scenarioInformation = DataImporter.ImportScenarioInformation(streamReader.ReadToEnd());
      streamReader.Close();
    }
    return scenarioInformation;
  }

  public static string GetScenarioDataPath()
  {
    char directorySeparatorChar = Path.DirectorySeparatorChar;
    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + directorySeparatorChar.ToString() + "Ndemic Creations" + directorySeparatorChar.ToString() + "Plague Inc. Evolved" + directorySeparatorChar.ToString() + "Scenario Creator" + directorySeparatorChar.ToString();
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    return path;
  }

  public static bool LocalExists(string filename)
  {
    filename = Path.GetFileName(filename);
    return Directory.Exists(Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename));
  }

  public static bool LocalScenarioInfoExists(string filename)
  {
    filename = Path.GetFileName(filename);
    return File.Exists(Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename) + "/scenario.txt");
  }

  public static void LocalSaveExport(string filename, ExportData export, ulong id = 0)
  {
    try
    {
      string scenarioDataPath = CSLocalUGCHandler.GetScenarioDataPath();
      filename = Path.GetFileName(filename);
      string path2 = filename;
      string str = Path.Combine(scenarioDataPath, path2);
      Directory.CreateDirectory(str);
      File.WriteAllText(str + "/scenario.txt", export.scenarioDataText);
      if (!string.IsNullOrEmpty(export.countryDataText))
        File.WriteAllText(str + "/countries.txt", export.countryDataText);
      if (!string.IsNullOrEmpty(export.diseaseDataText))
        File.WriteAllText(str + "/disease.txt", export.diseaseDataText);
      if (!string.IsNullOrEmpty(export.routeDataText))
        File.WriteAllText(str + "/routes.txt", export.routeDataText);
      if (!string.IsNullOrEmpty(export.governmentActionsDataText))
        File.WriteAllText(str + "/govactions_standard.txt", export.governmentActionsDataText);
      if (!string.IsNullOrEmpty(export.govNeuraxDataText))
        File.WriteAllText(str + "/govactions_neurax.txt", export.govNeuraxDataText);
      if (!string.IsNullOrEmpty(export.govNecroaDataText))
        File.WriteAllText(str + "/govactions_zombie.txt", export.govNecroaDataText);
      if (!string.IsNullOrEmpty(export.govVampireDataText))
        File.WriteAllText(str + "/govactions_vampire.txt", export.govVampireDataText);
      if (!string.IsNullOrEmpty(export.govFakeNewsDataText))
        File.WriteAllText(str + "/govactions_fake_news.txt", export.govFakeNewsDataText);
      if (!string.IsNullOrEmpty(export.govSimianDataText))
        File.WriteAllText(str + "/govactions_simian.txt", export.govSimianDataText);
      foreach (KeyValuePair<string, Texture2D> customIcon in export.GetCustomIcons())
      {
        byte[] png = customIcon.Value.EncodeToPNG();
        string key = customIcon.Key;
        if (!key.EndsWith(".png"))
          key += ".png";
        FileStream output = File.Open(Path.Combine(str, key), FileMode.Create);
        new BinaryWriter((Stream) output).Write(png);
        output.Close();
      }
      CSLocalUGCHandler.LocalSavePublish(filename, new PublishData()
      {
        hasPublished = 0,
        publishId = id.ToString(),
        visibility = VisibilityStatus.Public
      });
      Debug.Log((object) "Save Compete");
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
    finally
    {
      Debug.Log((object) "Exit Save");
    }
  }

  public static void LocalSavePublish(string filename, PublishData publish)
  {
    filename = Path.GetFileName(filename);
    string str = Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename);
    string contents = PublishDataExporter.StartExport(publish);
    if (string.IsNullOrEmpty(contents))
      return;
    File.WriteAllText(str + "/publish.txt", contents);
  }

  private string GetScenarioDifficulty(string difficulty)
  {
    string scenarioDifficulty = difficulty;
    if (string.IsNullOrEmpty(scenarioDifficulty))
      return "FTR ?";
    if (scenarioDifficulty.IndexOf("FTR") != -1)
      scenarioDifficulty = ":ff00ff]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("BYD") != -1)
      scenarioDifficulty = ":ff0000]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("PST") != -1)
      scenarioDifficulty = ":00bbff]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("MXM") != -1)
      scenarioDifficulty = ":ffee00]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("PRS") != -1)
      scenarioDifficulty = ":00ff11]" + scenarioDifficulty + ":ffffff]";
    return scenarioDifficulty;
  }

  private int GetScenarioHighScore(string filename)
  {
    if (CGameManager.scenarioScoreDict == null)
    {
      Debug.Log((object) "Null Dict Score");
      return 0;
    }
    return CGameManager.scenarioScoreDict.ContainsKey(filename) ? CGameManager.scenarioScoreDict[filename] : 0;
  }

  public static void SetScenarioHighScore(string filename, int score)
  {
    filename = Path.GetFileName(filename);
    File.WriteAllText(Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename) + "/score.txt", score.ToString());
    if (CGameManager.scenarioScoreDict != null)
    {
      if (CGameManager.scenarioScoreDict.ContainsKey(filename))
        CGameManager.scenarioScoreDict[filename] = score;
      else
        CGameManager.scenarioScoreDict.Add(filename, score);
      CGameManager.SaveFederalScenarioCloudScores();
    }
    else
      Debug.LogError((object) ("Tried to set high score for " + filename.Replace("PIFSL", "") + " but no such scenario found"));
  }

  public static int GetScenarioHighScore(string filename, bool externalCall = true)
  {
    if (CGameManager.scenarioScoreDict == null)
    {
      Debug.Log((object) "Null Dict Score");
      return 0;
    }
    return string.IsNullOrEmpty(filename) || !CGameManager.scenarioScoreDict.ContainsKey(filename) ? 0 : CGameManager.scenarioScoreDict[filename];
  }

  private string GetRank(int score)
  {
    if (score >= 75000)
      return "[:d9d919]S:00ffff]S:ff0000]S:d9d919]+:ffffff]]";
    if (score >= 72500)
      return "[:d9d919]S:00ffff]S:ff0000]S:ffffff]]";
    if (score >= 70000)
      return "[:d9d919]SS+:ffffff]]";
    if (score >= 68500)
      return "[:d9d919]SS:ffffff]]";
    if (score >= 66000 || score >= 66000)
      return "[:d9d919]S+:ffffff]]";
    if (score >= 65000)
      return "[:d9d919]S:ffffff]]";
    if (score >= 64000)
      return "[:ff0000]AAA:ffffff]]";
    if (score >= 62500)
      return "[:ff0000]AA:ffffff]]";
    if (score >= 60000)
      return "[:ff0000]A:ffffff]]";
    if (score >= 55000)
      return "[:00ffff]BBB:ffffff]]";
    if (score >= 45000)
      return "[:00ffff]BB:ffffff]]";
    if (score >= 35000)
      return "[:00ffff]B:ffffff]]";
    return score >= 15000 ? "[:00ff00]C:ffffff]]" : "[:555511]D:ffffff]]";
  }

  private string GetScenarioDifficultyRaw(string filename)
  {
    return CGameManager.GetScenarioDifficultyRaw(filename);
  }

  public static string GetScenarioLevelString(int cons)
  {
    if (cons >= 130)
      return "13";
    if (cons >= 125)
      return "12+";
    if (cons >= 120)
      return "12";
    if (cons >= 115)
      return "11+";
    if (cons >= 110)
      return "11";
    if (cons >= 105)
      return "10+";
    if (cons >= 100)
      return "10";
    if (cons >= 95)
      return "9+";
    if (cons >= 90)
      return "9";
    if (cons >= 80)
      return "8";
    if (cons >= 70)
      return "7";
    if (cons >= 60)
      return "6";
    if (cons >= 50)
      return "5";
    if (cons >= 40)
      return "4";
    if (cons >= 30)
      return "3";
    if (cons >= 20)
      return "2";
    return cons >= 10 ? "1" : "Error";
  }

  public static int GetScenarioLevel(string levelRaw, string append)
  {
    int scenarioLevel = 0;
    if (levelRaw.Equals("Error"))
      scenarioLevel = -1;
    if (levelRaw.Equals("1"))
      scenarioLevel = 10;
    if (levelRaw.Equals("2"))
      scenarioLevel = 20;
    if (levelRaw.Equals("3"))
      scenarioLevel = 30;
    if (levelRaw.Equals("4"))
      scenarioLevel = 40;
    if (levelRaw.Equals("5"))
      scenarioLevel = 50;
    if (levelRaw.Equals("6"))
      scenarioLevel = 60;
    if (levelRaw.Equals("7"))
      scenarioLevel = 70;
    if (levelRaw.Equals("8"))
      scenarioLevel = 80;
    if (levelRaw.Equals("9"))
      scenarioLevel = 90;
    if (levelRaw.Equals("9+"))
      scenarioLevel = 100;
    if (levelRaw.Equals("10"))
      scenarioLevel = 110;
    if (levelRaw.Equals("10+"))
      scenarioLevel = 120;
    if (levelRaw.Equals("11"))
      scenarioLevel = 130;
    if (levelRaw.Equals("11+"))
      scenarioLevel = 140;
    if (levelRaw.Equals("12"))
      scenarioLevel = 150;
    if (levelRaw.Equals("12+"))
      scenarioLevel = 160;
    if (levelRaw.Equals("13"))
      scenarioLevel = 170;
    if (append.Equals("PST"))
      ++scenarioLevel;
    if (append.Equals("PRS"))
      scenarioLevel += 2;
    if (append.Equals("FTR"))
      scenarioLevel += 3;
    if (append.Equals("BYD"))
      scenarioLevel += 4;
    if (append.Equals("MXM"))
      scenarioLevel += 5;
    return scenarioLevel;
  }

  private bool GetUnlocked(string filename)
  {
    filename = Path.GetFileName(filename);
    if (CGameManager.scenarioUnlockConditionOverride != null && CGameManager.scenarioUnlockConditionOverride.ContainsKey(filename) && CGameManager.scenarioUnlockConditionOverride[filename] != null && CGameManager.scenarioUnlockConditionOverride[filename].Count != 0)
    {
      foreach (CGameManager.ScenarioUnlockCondition scenarioUnlockCondition in CGameManager.scenarioUnlockConditionOverride[filename])
      {
        string unlockScenario = scenarioUnlockCondition.unlockScenario;
        int unlockScore = scenarioUnlockCondition.unlockScore;
        if (this.GetScenarioHighScore(unlockScenario) < unlockScore)
          return false;
      }
    }
    return CGameManager.scenarioUnlockPotential == null || !CGameManager.scenarioUnlockPotential.ContainsKey(filename) || CGameManager.scenarioUnlockPotential[filename] == 0 || 100.0 * CGameManager.GetPotential() >= (double) CGameManager.scenarioUnlockPotential[filename];
  }

  public static string GetScenarioDifficultyRawExternal(string filename)
  {
    return CGameManager.GetScenarioDifficultyRaw(filename);
  }

  public static void SetScenarioProperty(string filename, string property, string content)
  {
    filename = Path.GetFileName(filename);
    File.WriteAllText(Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename) + "/" + property + ".txt", content);
  }

  public static string GetScenarioProperty(string filename, string property, bool externalCall = true)
  {
    filename = Path.GetFileName(filename);
    string str = Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename);
    string scenarioProperty = "";
    if (File.Exists(str + "/" + property + ".txt"))
    {
      StreamReader streamReader = File.OpenText(str + "/" + property + ".txt");
      scenarioProperty = streamReader.ReadToEnd();
      streamReader.Close();
    }
    return scenarioProperty;
  }

  public static int GetScenarioHighScoreInitial(string filename, bool externalCall = true)
  {
    filename = Path.GetFileName(filename);
    string str = Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename);
    int result = 0;
    if (File.Exists(str + "/score.txt"))
    {
      StreamReader streamReader = File.OpenText(str + "/score.txt");
      int.TryParse(streamReader.ReadToEnd(), out result);
      streamReader.Close();
    }
    return result;
  }

  public bool GetScenarioSubset()
  {
    this.subsetScenarios = new List<ScenarioInformation>();
    foreach (ScenarioInformation scenarioInformation in this.cachedScenariosInformation)
      this.subsetScenarios.Add(scenarioInformation);
    if (CGameManager.scenarioShow == "rating")
    {
      this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithRating));
      int num = 0;
      for (int index = 0; index < this.subsetScenarios.Count; ++index)
      {
        ScenarioInformation subsetScenario = this.subsetScenarios[index];
        if (CGameManager.constedScenarioList.Contains(subsetScenario.fileName) && subsetScenario.fileName.Contains("PIFSL"))
        {
          ++num;
          subsetScenario.scenTitle = "[#" + num.ToString("N0") + "]" + subsetScenario.titleWithRating;
          if (!subsetScenario.scoreUnlocked)
            subsetScenario.scenTitle = "[[7f94d5]Locked[ffffff]]" + subsetScenario.originScenTitle;
        }
      }
    }
    switch (CGameManager.scenarioSort)
    {
      case "level":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenario));
        break;
      case "const":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithConst));
        break;
      case "score":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithScore));
        break;
      case "name":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithName));
        break;
      case "rating":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithRating));
        break;
      case "pack":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithPack));
        break;
      case "date":
        this.subsetScenarios.Sort(new Comparison<ScenarioInformation>(ScenarioInformation.CompareScenarioWithDate));
        break;
    }
    if (CGameManager.InvertSortType)
      this.subsetScenarios.Reverse();
    switch (CGameManager.scenarioShow)
    {
      case "level":
        using (List<ScenarioInformation>.Enumerator enumerator = this.subsetScenarios.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ScenarioInformation current = enumerator.Current;
            if (CGameManager.constedScenarioList.Contains(current.fileName) && current.fileName.Contains("PIFSL"))
              current.scenTitle = current.titleWithLevel;
            if (!current.scoreUnlocked)
              current.scenTitle = "[[7f94d5]Locked[ffffff]]" + current.originScenTitle;
          }
          break;
        }
      case "const":
      case "pack":
      case "date":
        using (List<ScenarioInformation>.Enumerator enumerator = this.subsetScenarios.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ScenarioInformation current = enumerator.Current;
            if (CGameManager.constedScenarioList.Contains(current.fileName) && current.fileName.Contains("PIFSL"))
            {
              current.scenTitle = current.titleWithConst;
              string str = "";
              if (CGameManager.scenarioShow == "pack")
                str = "[" + CGameManager.GetScenarioPackDisplayName(current.fileName) + "]";
              if (CGameManager.scenarioShow == "date")
              {
                int scenarioDate = CGameManager.GetScenarioDate(current.fileName);
                int num1 = 2000 + scenarioDate / 480;
                int num2 = scenarioDate - (num1 - 2000) * 480;
                int num3 = num2 / 35;
                int num4 = num2 - num3 * 35;
                str = "[" + (num1.ToString() + "/" + num3.ToString() + "/" + num4.ToString()) + "]";
              }
              current.scenTitle = str + current.scenTitle;
            }
            if (!current.scoreUnlocked)
              current.scenTitle = "[[7f94d5]Locked[ffffff]]" + current.originScenTitle;
          }
          break;
        }
    }
    if (CGameManager.FilterScenarioComplete.Equals("all") && CGameManager.FilterScenarioLevel.Equals("all") && CGameManager.FilterScenarioType.Equals("all") && string.IsNullOrEmpty(CGameManager.ScenarioSearchKeyword))
      return this.subsetScenarios != null && this.subsetScenarios.Count > 0;
    List<ScenarioInformation> scenarioInformationList = new List<ScenarioInformation>();
    foreach (ScenarioInformation subsetScenario in this.subsetScenarios)
    {
      if (CGameManager.constedScenarioList.Contains(subsetScenario.fileName) && subsetScenario.fileName.Contains("PIFSL") && (CGameManager.FilterScenarioLevel.Equals("all") || subsetScenario.levelString.Equals(CGameManager.FilterScenarioLevel)) && (CGameManager.FilterScenarioComplete.Equals("all") || CGameManager.FilterScenarioComplete.Equals("completed") && subsetScenario.completed || CGameManager.FilterScenarioComplete.Equals("not completed") && !subsetScenario.completed) && (CGameManager.FilterScenarioType.Equals("all") || subsetScenario.difficultyRaw.Contains(CGameManager.FilterScenarioType)) && (string.IsNullOrEmpty(CGameManager.ScenarioSearchKeyword) || subsetScenario.originScenTitle.ToUpper().Contains(CGameManager.ScenarioSearchKeyword.ToUpper()) || subsetScenario.pinyinName.ToUpper().Contains(CGameManager.ScenarioSearchKeyword.ToUpper())))
        scenarioInformationList.Add(subsetScenario);
    }
    this.subsetScenarios = scenarioInformationList;
    return this.subsetScenarios != null && this.subsetScenarios.Count > 0;
  }

  public static string GetScenarioAuthor(string scenario)
  {
    return CGameManager.federalScenarioAuthorList == null || !CGameManager.federalScenarioAuthorList.ContainsKey(scenario) ? "" : CGameManager.federalScenarioAuthorList[scenario];
  }
}
