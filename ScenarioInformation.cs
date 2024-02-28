// Decompiled with JetBrains decompiler
// Type: ScenarioInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

#nullable disable
public class ScenarioInformation
{
  public string filePath;
  public string fileName;
  public string scenIdentifier;
  public string id;
  public string scenTitle;
  public string scenSymptoms;
  public string scenDescription;
  public string scenIcon;
  [CompareIgnore]
  public string scenCreatedDate;
  [CompareIgnore]
  public string scenEditedDate;
  [CompareIgnore]
  public long scenStartDate = -1;
  public string scenAuthorName;
  public Texture2D customScenIcon;
  public string scenVersion;
  public string scenLinkTitle;
  public string scenLink;
  public string scenPlagueType;
  public string scenDifficulty;
  public string scenName;
  public string scenCheats = "1";
  public string scenGenes = "0";
  public string scenNexus = "";
  public string scenNexusPopInfected;
  public string scenNexusPopDead;
  public string scenNexusPopZombie;
  public bool scenAutoLoad;
  public bool scenFree;
  public string scenInitPopup1Title;
  public string scenInitPopup1Text;
  public string scenInitPopup2Title;
  public string scenInitPopup2Text;
  public bool scenEventRestriction;
  public float scenScoreMultiplier;
  public string scenGameWinTech;
  public string scenEndMessageImage;
  public string scenEndMessageTitle;
  public string scenEndMessageText;
  public string scenEndGameTagline;
  public bool scenFreeNonpremium;
  public bool isMine;
  public string scenMainLanguage;
  public string scenDefaultLanguage;
  public string scenDefaultEvents;
  public string scenEndGameScreenTitlePc;
  public bool scenUseCustomColour;
  public string scenDiseaseDotColourSet;
  public string scenPublicEmail;
  public string scenPrivateEmail;
  public string originScenTitle;
  public string scenarioTag;
  public int scenarioLevel;
  public int scenarioConst;
  public string difficultyRaw;
  public string levelString;
  public bool completed;
  public string titleWithConst;
  public string titleWithLevel;
  public bool scoreUnlocked;
  public string titleWithRating;
  public static CultureInfo culture = new CultureInfo("zh-CN");
  public static CompareInfo compareInfo = ScenarioInformation.culture.CompareInfo;
  public string pinyinName;
  public string finalLevelString;

  private Color DeserializeColour(string str)
  {
    string[] strArray = str.Split('|');
    int[] numArray = new int[4];
    for (int index = 0; index < numArray.Length; ++index)
    {
      int result;
      if (strArray.Length > index && int.TryParse(strArray[index], out result))
        numArray[index] = result;
    }
    return new Color((float) numArray[0] / (float) byte.MaxValue, (float) numArray[1] / (float) byte.MaxValue, (float) numArray[2] / (float) byte.MaxValue, (float) numArray[3] / (float) byte.MaxValue);
  }

  public HashSet<Disease.EDiseaseType> GetDiseaseTypes()
  {
    string[] strArray = this.scenPlagueType.Split(',');
    HashSet<Disease.EDiseaseType> diseaseTypes = new HashSet<Disease.EDiseaseType>();
    foreach (string str in strArray)
    {
      if (!string.IsNullOrEmpty(str))
        diseaseTypes.Add((Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), str));
    }
    return diseaseTypes;
  }

  public static ScenarioInformation Instantiate(ScenarioInformation scenarioInfo)
  {
    if (scenarioInfo == null)
      return (ScenarioInformation) null;
    ScenarioInformation scenarioInformation = new ScenarioInformation();
    foreach (FieldInfo field in typeof (ScenarioInformation).GetFields(BindingFlags.Instance | BindingFlags.Public))
      field.SetValue((object) scenarioInformation, field.GetValue((object) scenarioInfo));
    return scenarioInformation;
  }

  public static bool Equals(ScenarioInformation infoA, ScenarioInformation infoB)
  {
    if (infoA == null || infoB == null)
      return false;
    foreach (FieldInfo field in typeof (ScenarioInformation).GetFields(BindingFlags.Instance | BindingFlags.Public))
    {
      if (!Attribute.IsDefined((MemberInfo) field, typeof (CompareIgnoreAttribute)))
      {
        object obj1 = field.GetValue((object) infoA);
        object obj2 = field.GetValue((object) infoB);
        bool flag = obj1 == obj2 || obj1 != null && obj1.Equals(obj2);
        Debug.Log((object) ("--- " + field.Name + " infoA: " + obj1 + " infoB: " + obj2 + " compare: " + flag.ToString()));
        if (!flag)
          return false;
      }
    }
    return true;
  }

  private long GetPopValue(string str, long pop)
  {
    if (string.IsNullOrEmpty(str))
      return 0;
    return str.EndsWith("%") ? (long) (double.Parse(str.Substring(0, str.Length - 1)) * (double) pop / 100.0) : long.Parse(str);
  }

  public long GetAdditionalNexusInfected(Disease d, Country nexus)
  {
    LocalDisease localDisease = nexus.GetLocalDisease(d);
    long popValue = this.GetPopValue(this.scenNexusPopInfected, nexus.originalPopulation);
    return popValue > 0L ? popValue - localDisease.infectedPopulation : 0L;
  }

  public long GetAdditionalNexusDead(Disease d, Country nexus)
  {
    long popValue = this.GetPopValue(this.scenNexusPopDead, nexus.originalPopulation);
    return popValue > 0L ? popValue - nexus.deadPopulation : 0L;
  }

  public long GetAdditionalNexusZombie(Disease d, Country nexus)
  {
    LocalDisease localDisease = nexus.GetLocalDisease(d);
    long popValue = this.GetPopValue(this.scenNexusPopZombie, nexus.originalPopulation);
    return popValue > 0L ? popValue - localDisease.zombiePopulation : 0L;
  }

  public static int CompareScenario(ScenarioInformation s1, ScenarioInformation s2)
  {
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public int scenarioTagNum
  {
    get
    {
      if (this.scenarioTag.Equals("PIFSL"))
        return 0;
      if (this.scenarioTag.Equals("Legacy"))
        return 1;
      return this.scenarioTag.Equals("Local") ? 2 : 3;
    }
  }

  public int GetScenarioHighScore() => CSLocalUGCHandler.GetScenarioHighScore(this.fileName);

  public static int CompareScenarioWithConst(ScenarioInformation s1, ScenarioInformation s2)
  {
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    if (s1.scenarioConst < s2.scenarioConst)
      return 1;
    if (s1.scenarioConst > s2.scenarioConst)
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public double rating => CGameManager.GetRating(this.GetScenarioHighScore(), this.scenarioConst);

  public static int CompareScenarioWithName(ScenarioInformation s1, ScenarioInformation s2)
  {
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    return s1.scenarioTagNum < s2.scenarioTagNum ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public static int CompareScenarioWithScore(ScenarioInformation s1, ScenarioInformation s2)
  {
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    if (s1.GetScenarioHighScore() < s2.GetScenarioHighScore())
      return 1;
    if (s1.GetScenarioHighScore() > s2.GetScenarioHighScore())
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public static int CompareScenarioWithRating(ScenarioInformation s1, ScenarioInformation s2)
  {
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    if (s1.rating < s2.rating)
      return 1;
    if (s1.rating > s2.rating)
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public static int CompareScenarioWithPack(ScenarioInformation s1, ScenarioInformation s2)
  {
    int num = string.Compare(CGameManager.GetScenarioPackName(s1.fileName), CGameManager.GetScenarioPackName(s2.fileName));
    if (num > 0)
      return 1;
    if (num < 0)
      return -1;
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    if (s1.scenarioConst < s2.scenarioConst)
      return 1;
    if (s1.scenarioConst > s2.scenarioConst)
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }

  public static int CompareScenarioWithDate(ScenarioInformation s1, ScenarioInformation s2)
  {
    int scenarioDate1 = CGameManager.GetScenarioDate(s1.fileName);
    int scenarioDate2 = CGameManager.GetScenarioDate(s2.fileName);
    if (scenarioDate1 < scenarioDate2)
      return 1;
    if (scenarioDate1 > scenarioDate2)
      return -1;
    if (s1.scenarioTagNum > s2.scenarioTagNum)
      return 1;
    if (s1.scenarioTagNum < s2.scenarioTagNum)
      return -1;
    if (s1.scenarioConst < s2.scenarioConst)
      return 1;
    if (s1.scenarioConst > s2.scenarioConst)
      return -1;
    int scenarioLevel1 = s1.scenarioLevel;
    int scenarioLevel2 = s2.scenarioLevel;
    if (!s1.scoreUnlocked)
      scenarioLevel1 -= 5;
    if (!s2.scoreUnlocked)
      scenarioLevel2 -= 5;
    if (scenarioLevel1 < scenarioLevel2)
      return 1;
    return scenarioLevel1 > scenarioLevel2 ? -1 : string.Compare(s1.pinyinName, s2.pinyinName);
  }
}
