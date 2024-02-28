// Decompiled with JetBrains decompiler
// Type: Scenario
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#nullable disable
public class Scenario
{
  public string filename;
  public string gamestartData;
  public string countryData;
  public string routeData;
  public string actionsData;
  public string actionsDataZombie;
  public string actionsDataNeurax;
  public string actionsDataSimian;
  public string actionsDataVampire;
  public string actionsDataFakeNews;
  public string actionsDataCure;
  public string eventData;
  public string diseaseData;
  public string aaData;
  public bool fromResources;
  public bool fromNdemic;
  public bool isOfficial;
  public ScenarioInformation scenarioInformation;
  public Dictionary<string, Texture2D> customIcons = new Dictionary<string, Texture2D>();
  public Dictionary<string, string> localisationText;
  public string customUIText;
  internal int iconsLoading;
  public string diseaseData_0;
  public string diseaseData_1;
  public string diseaseData_2;
  public string diseaseData_3;
  public string originScenTitle;
  public System.Type scenarioExternalLibrary;
  public bool functionAllowed;
  public List<string> externalMethods;

  public event Scenario.IconsLoadedEvent onIconsLoaded;

  public List<PetriDishSymptom> GetPetriDishSymptoms(IDictionary<string, PetriDishSymptom> lookup)
  {
    string[] strArray = this.scenarioInformation.scenSymptoms.Split(',');
    List<PetriDishSymptom> petriDishSymptoms = new List<PetriDishSymptom>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (!string.IsNullOrEmpty(strArray[index]) && lookup.ContainsKey(strArray[index]))
        petriDishSymptoms.Add(lookup[strArray[index]]);
    }
    return petriDishSymptoms;
  }

  public Texture2D customScenarioIcon
  {
    get
    {
      return this.scenarioInformation != null && !string.IsNullOrEmpty(this.scenarioInformation.scenIcon) && this.customIcons != null && this.customIcons.ContainsKey(this.scenarioInformation.scenIcon) ? this.customIcons[this.scenarioInformation.scenIcon] : (Texture2D) null;
    }
  }

  public static Scenario LoadScenario(
    string fileName,
    bool isResources,
    bool isNdemic,
    string localFilePath = "")
  {
    if (!isResources && !Directory.Exists(localFilePath))
      return (Scenario) null;
    Debug.Log((object) ("Load Scenario with file name : " + fileName + ", and localFilePath: " + localFilePath));
    Scenario scenario = new Scenario();
    string str1 = Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), fileName, "functions.dll");
    int index;
    if (File.Exists(str1))
    {
      scenario.externalMethods = new List<string>();
      scenario.scenarioExternalLibrary = Assembly.LoadFrom(str1).GetType("PlagueScenarioExternal.Functions");
      scenario.functionAllowed = true;
      index = scenario.scenarioExternalLibrary.GetMethods().Length;
      Debug.Log((object) ("method count: " + index.ToString()));
      MethodInfo[] methods = scenario.scenarioExternalLibrary.GetMethods();
      for (index = 0; index < methods.Length; ++index)
      {
        MethodInfo methodInfo = methods[index];
        Debug.Log((object) ("method name: " + methodInfo.Name));
        scenario.externalMethods.Add(methodInfo.Name);
      }
    }
    scenario.fromResources = isResources;
    scenario.fromNdemic = isNdemic;
    scenario.filename = fileName;
    scenario.gamestartData = scenario.ReadText("scenario");
    scenario.countryData = scenario.ReadText("countries");
    scenario.routeData = scenario.ReadText("routes");
    scenario.actionsData = scenario.ReadText("govactions_standard");
    scenario.actionsDataZombie = scenario.ReadText("govactions_zombie");
    scenario.actionsDataNeurax = scenario.ReadText("govactions_neurax");
    scenario.actionsDataSimian = scenario.ReadText("govactions_simian_flu");
    scenario.actionsDataVampire = scenario.ReadText("govactions_vampire");
    scenario.actionsDataFakeNews = scenario.ReadText("govactions_fake_news");
    scenario.actionsDataCure = scenario.ReadText("govactions_cure");
    scenario.aaData = scenario.ReadText("aa");
    scenario.diseaseData = scenario.ReadText("disease");
    scenario.diseaseData_0 = scenario.ReadText("disease_0");
    scenario.diseaseData_1 = scenario.ReadText("disease_1");
    scenario.diseaseData_2 = scenario.ReadText("disease_2");
    scenario.diseaseData_3 = scenario.ReadText("disease_3");
    if (string.IsNullOrEmpty(scenario.diseaseData_0))
      scenario.diseaseData_0 = scenario.ReadText("disease");
    if (string.IsNullOrEmpty(scenario.diseaseData_1))
      scenario.diseaseData_1 = scenario.ReadText("disease");
    if (string.IsNullOrEmpty(scenario.diseaseData_2))
      scenario.diseaseData_2 = scenario.ReadText("disease");
    if (string.IsNullOrEmpty(scenario.diseaseData_3))
      scenario.diseaseData_3 = scenario.ReadText("disease");
    scenario.customUIText = scenario.ReadText("custom_ui_text");
    string[] languages = CLocalisationManager.GetLanguages();
    Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
    string[] strArray1 = languages;
    for (index = 0; index < strArray1.Length; ++index)
    {
      string str2 = strArray1[index];
      string str3 = scenario.ReadText(CLocalisationManager.GetLanguageCode(str2));
      if (!string.IsNullOrEmpty(str3))
        dictionary1[str2] = str3;
      string str4 = fileName;
      if (str4.EndsWith(".scenario"))
        str4 = str4.Substring(0, str4.Length - 9);
      string str5 = scenario.ReadText(str4 + "_" + CLocalisationManager.GetLanguageCode(str2) + "_pc");
      if (!string.IsNullOrEmpty(str3))
      {
        Dictionary<string, string> dictionary2 = dictionary1;
        string key = str2;
        dictionary2[key] = dictionary2[key] + "\n" + str5;
      }
    }
    scenario.localisationText = dictionary1;
    if (!isResources)
    {
      scenario.eventData = scenario.ReadText("events");
      Debug.Log((object) ("GAMESTART: " + scenario.gamestartData));
      scenario.scenarioInformation = DataImporter.ImportScenarioInformation(scenario.gamestartData);
      if (!string.IsNullOrEmpty(scenario.scenarioInformation.originScenTitle))
        scenario.scenarioInformation.scenTitle = scenario.scenarioInformation.originScenTitle;
      scenario.customIcons = new Dictionary<string, Texture2D>();
      if (isNdemic)
      {
        string[] localFiles = CustomScenarioCache.GetLocalFiles(scenario.filename, ".png");
        scenario.iconsLoading = localFiles.Length;
        string[] strArray2 = localFiles;
        for (index = 0; index < strArray2.Length; ++index)
        {
          string filename = strArray2[index];
          PNGLoader.instance.LocalLoadPNG(filename, Path.Combine(CustomScenarioCache.GetDirectory(), scenario.filename), new PNGLoader.TextureLoaded(scenario.AddCustomIcon), -1, -1);
        }
      }
      else
      {
        string[] localFiles = new CSLocalUGCHandler().GetLocalFiles(scenario.filename, ".png");
        scenario.iconsLoading = localFiles.Length;
        string[] strArray3 = localFiles;
        for (index = 0; index < strArray3.Length; ++index)
        {
          string filename = strArray3[index];
          PNGLoader.instance.LocalLoadPNG(filename, Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), scenario.filename), new PNGLoader.TextureLoaded(scenario.AddCustomIcon), -1, -1);
        }
      }
      if (!string.IsNullOrEmpty(scenario.customUIText) && !string.IsNullOrEmpty(scenario.scenarioInformation.scenMainLanguage))
      {
        Dictionary<string, string> localisationText = scenario.localisationText;
        string scenMainLanguage = scenario.scenarioInformation.scenMainLanguage;
        localisationText[scenMainLanguage] = localisationText[scenMainLanguage] + "\n" + scenario.customUIText;
      }
    }
    return scenario;
  }

  private void AddCustomIcon(string imageName, Texture2D image)
  {
    --this.iconsLoading;
    if ((UnityEngine.Object) image != (UnityEngine.Object) null && !this.customIcons.ContainsKey(imageName))
      this.customIcons[imageName] = image;
    if (this.iconsLoading >= 1 || this.onIconsLoaded == null)
      return;
    this.onIconsLoaded();
  }

  public bool FileExists(string file)
  {
    if (this.fromResources)
      return (UnityEngine.Object) EncodedResources.Load("Data/OfficialScenarios/" + this.filename + "/" + file) != (UnityEngine.Object) null;
    string path;
    if (this.fromNdemic)
      path = CustomScenarioCache.GetPath(long.Parse(this.scenarioInformation.id));
    else
      path = CSLocalUGCHandler.GetScenarioDataPath() + "/" + this.filename + "/" + file;
    return File.Exists(path) || File.Exists(path + ".txt");
  }

  public string ReadText(string txtfile)
  {
    return Scenario.ReadText(this.scenarioInformation, txtfile, this.filename, this.fromResources, this.fromNdemic);
  }

  private static string ReadText(
    ScenarioInformation scenarioInformation,
    string txtfile,
    string filename,
    bool isResources,
    bool isNdemic)
  {
    if (isResources)
    {
      TextAsset textAsset = EncodedResources.Load("Data/OfficialScenarios/" + filename + "/" + txtfile);
      return (UnityEngine.Object) textAsset == (UnityEngine.Object) null ? (string) null : textAsset.text;
    }
    string path = Path.Combine(!isNdemic ? Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), filename) : CustomScenarioCache.GetPath(long.Parse(filename)), txtfile);
    if (File.Exists(path))
      return File.ReadAllText(path);
    return File.Exists(path + ".txt") ? File.ReadAllText(path + ".txt") : (string) null;
  }

  public string GetActionData(Disease.EDiseaseType diseaseType)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        return this.actionsDataNeurax;
      case Disease.EDiseaseType.NECROA:
        return this.actionsDataZombie;
      case Disease.EDiseaseType.SIMIAN_FLU:
        return this.actionsDataSimian;
      case Disease.EDiseaseType.VAMPIRE:
        return this.actionsDataVampire;
      case Disease.EDiseaseType.FAKE_NEWS:
        return this.actionsDataFakeNews;
      case Disease.EDiseaseType.CURE:
        return this.actionsDataCure;
      default:
        return this.actionsData;
    }
  }

  public delegate void IconsLoadedEvent();
}
