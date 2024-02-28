// Decompiled with JetBrains decompiler
// Type: Main
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class Main : MonoBehaviour
{
  internal static bool showSplashVideo = true;
  public static Main instance;
  [Header("Disease Colour Sets")]
  public DiseaseColourSet[] diseaseColourSets;
  public DiseaseColourSet[] zombieDiseaseColourSets;
  public DiseaseColourSet[] neuraxDiseaseColourSets;
  public DiseaseColourSet[] simianDiseaseColourSets;
  public DiseaseColourSet[] vampireDiseaseColourSets;
  public DiseaseColourSet[] cureDiseaseColourSets;
  public static bool DLLLoadError = false;
  public bool editorAlwaysHasCureDLC;
  private const uint cureDLCID = 1472780;
  private const uint appID = 246620;
  public UIAtlas[] atlases;
  private bool reconnecting;
  private CConfirmOverlay mpDisconnect;

  public static bool HasCureDLC() => SteamApps.BIsDlcInstalled(new AppId_t(1472780U));

  private void Awake()
  {
    Main.instance = this;
    CLocalisationManager.Initialise();
    CStatsManager.Initialise();
    CNetworkManager.InitialiseSteam();
    try
    {
      SPPluginManager.InitialisePluginCallbacks();
      MPPluginManager.InitialisePluginCallbacks();
      CoopPluginManager.InitialisePluginCallbacks();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error initialising plugins:\n" + (object) ex));
      Main.DLLLoadError = true;
    }
  }

  private IEnumerator Start()
  {
    Main main1 = this;
    yield return (object) null;
    CGameManager.federalServerAddress = "http://47.242.150.236:34567/";
    CGameManager.UpdateScenarioInfo();
    Main.instance.StartCoroutine(main1.GetFederalScenarioConstList());
    CGameManager.Initialise();
    CUIManager.instance.Initialise();
    if (Main.DLLLoadError)
    {
      Analytics.Event("DLL Error");
    }
    else
    {
      yield return (object) null;
      Analytics.Event("Game Opened");
      yield return (object) new WaitForSeconds(0.5f);
      for (float timeout = 0.0f; !SteamManager.Initialized && (double) timeout < 10.0; timeout += Time.deltaTime)
        yield return (object) null;
      if (!SteamManager.Initialized)
      {
        Analytics.Event("FAIL NO STEAM");
        COptionsManager.instance.StatsNotLoaded();
        CUIManager.instance.canSkipVideo = true;
        CUIManager.instance.initialScreen = CUIManager.instance.errorScreen;
        if (!Main.showSplashVideo)
          CUIManager.instance.ShowInitialScreen();
      }
      else if (SteamApps.BIsSubscribedApp(new AppId_t(246620U)))
      {
        Main main = main1;
        Analytics.Event("STEAM VALID");
        CNetworkSteam network = CNetworkManager.network as CNetworkSteam;
        if ((UnityEngine.Object) network != (UnityEngine.Object) null)
        {
          CNetworkSteam.OnUserStatsReceived onUserStatsReceived = (CNetworkSteam.OnUserStatsReceived) null;
          onUserStatsReceived = (CNetworkSteam.OnUserStatsReceived) (steamID =>
          {
            CGameManager.CheckUnlocks();
            COptionsManager.instance.StatsLoaded();
            closure_0.StatsLoaded();
            network.onUserStatsReceived -= onUserStatsReceived;
          });
          network.onUserStatsReceived += onUserStatsReceived;
          network.Initialize();
          try
          {
            CSteamControllerManager.instance.Initialize();
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
          }
        }
        else
          Debug.LogError((object) "CNetworkSteam not valid");
      }
      else
      {
        Analytics.Event("FAIL NO STEAM");
        CUIManager.instance.canSkipVideo = true;
        CUIManager.instance.initialScreen = CUIManager.instance.errorScreen;
        if (!Main.showSplashVideo)
          CUIManager.instance.ShowInitialScreen();
      }
    }
  }

  private void ExtractDiseaseSprites()
  {
    HashSet<string> stringSet1 = new HashSet<string>();
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    StringBuilder stringBuilder3 = new StringBuilder();
    foreach (Disease.EDiseaseType type in CUtils.GetEnumList<Disease.EDiseaseType>())
    {
      Disease disease = DataImporter.ImportDisease(IGame.GameType.Classic, CGameManager.LoadGameText("Disease/" + DataImporter.GetDiseaseFile(type)), false, (Scenario) null);
      stringBuilder2.Append("*** Tech Sprites in: " + type.ToString() + " ***\n");
      stringBuilder3.Append("*** Tech Sprites in: " + type.ToString() + " ***\n");
      foreach (Technology technology in disease.technologies)
      {
        if (!stringSet1.Contains(technology.baseGraphic))
        {
          stringSet1.Add(technology.baseGraphic);
          stringBuilder1.Append(technology.id + ":  " + technology.baseGraphic + "\n");
          stringBuilder2.Append(technology.id + ":  " + technology.baseGraphic + "\n");
          stringBuilder3.Append(technology.id + ":  " + technology.baseGraphic + "\n");
        }
        else
          stringBuilder3.Append("DUPLUICATE: " + technology.id + ":  " + technology.baseGraphic + "\n");
      }
    }
    File.WriteAllText(Application.dataPath + "/TechSprites.txt", stringBuilder1.ToString());
    File.WriteAllText(Application.dataPath + "/TechSpritesReference.txt", stringBuilder2.ToString());
    File.WriteAllText(Application.dataPath + "/TechSpritesDuplicates.txt", stringBuilder3.ToString());
    HashSet<string> stringSet2 = new HashSet<string>();
    StringBuilder stringBuilder4 = new StringBuilder();
    foreach (UIAtlas atlase in this.atlases)
    {
      stringBuilder4.Append("*** Sprites in Atlas: " + atlase.name + " ***\n");
      int num = 0;
      foreach (UISpriteData sprite in atlase.spriteList)
      {
        ++num;
        stringSet2.Add(sprite.name);
        stringBuilder4.Append(num.ToString() + ":  " + sprite.name + "\n");
      }
    }
    File.WriteAllText(Application.dataPath + "/AtlasSprites.txt", stringBuilder4.ToString());
    foreach (string str in stringSet1)
    {
      if (stringSet2.Contains(str))
        stringSet2.Remove(str);
      else
        Debug.Log((object) ("Missing Sprite: " + str));
    }
    StringBuilder stringBuilder5 = new StringBuilder();
    stringBuilder5.Append("*** Unused Sprites Atlas ***\n");
    foreach (string str in stringSet2)
      stringBuilder5.Append(str + "\n");
    File.WriteAllText(Application.dataPath + "/RemainingSprites.txt", stringBuilder5.ToString());
  }

  private void Update()
  {
    CGameManager.GameUpdate();
    if (CNetworkManager.network.NetworkState == CNetworkManager.ENetworkState.WaitingForReconnect)
    {
      if (this.reconnecting)
        return;
      this.reconnecting = true;
      this.mpDisconnect = CUIManager.instance.standardConfirmOverlay;
      this.mpDisconnect.ShowLocalised("FE_Multiplayer_PopUp_Connection_Lost_Title", "FE_Multiplayer_PopUp_Connection_Lost_Text");
    }
    else
    {
      if (!this.reconnecting)
        return;
      this.reconnecting = false;
      if (!(bool) (UnityEngine.Object) this.mpDisconnect)
        return;
      CUIManager.instance.HideOverlay((CGameOverlay) this.mpDisconnect);
    }
  }

  public void StatsLoaded()
  {
    CUIManager.instance.canSkipVideo = true;
    if (Main.showSplashVideo)
      return;
    CUIManager.instance.ShowInitialScreen();
  }

  public IEnumerator GetFederalScenarioConstList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(CGameManager.federalServerAddress + "ScenarioConfig/scenarioList.txt");
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "scenarioList.json";
      if (File.Exists(path))
        File.Delete(path);
      string text = pendingRequest.downloadHandler.text;
      File.WriteAllText(path, text);
      CGameManager.UpdateScenarioInfo();
    }
    else
      Debug.LogError((object) ("Error while getting Scenario List: \n" + pendingRequest.error));
  }
}
