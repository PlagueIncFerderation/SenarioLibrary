// Decompiled with JetBrains decompiler
// Type: DynamicNewsLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class DynamicNewsLoader : MonoBehaviour
{
  public static DynamicNewsLoader instance;
  private string gameInfoURL;
  private string dynamicNewsURL;
  private string dynamicPopupURL;
  private string dynamicPopupURLTest;
  internal string dynamicNewsText;
  internal string dynamicPopupText;
  private int sessionID;
  private string loadedLanguage;

  private void Start()
  {
    DynamicNewsLoader.instance = this;
    CLocalisationManager.languageChangeEvent += new CLocalisationManager.LanguageChangeEvent(this.LanguageChanged);
    string languageCode = CLocalisationManager.GetLanguageCode(CLocalisationManager.ActiveLanguage);
    this.sessionID = CUtils.IntRand(0, (int) Mathf.Pow(2f, 30f));
    this.StartCoroutine(this.LoadMultiplayerVersionConfig(false));
    this.StartCoroutine(this.LoadPopup(languageCode));
    this.StartCoroutine(this.LoadNews(languageCode));
  }

  public void RefreshMultiplayerConfig()
  {
    this.StartCoroutine(this.LoadMultiplayerVersionConfig(true));
  }

  private IEnumerator LoadMultiplayerVersionConfig(bool isThrowPopup)
  {
    CGameManager.betaMultiplayerVersionRequired = CGameManager.multiplayerVersionRequired = (CNetworkManager.network as CNetworkSteam).GameNetworkVersion;
    UnityWebRequest www = UnityWebRequest.Get(CGameManager.federalServerAddress + "MultiplayerConfig/multiplayerConfig.txt?rnd=" + (object) UnityEngine.Random.Range(0, 9999999));
    yield return (object) www.SendWebRequest();
    if (www.isDone && string.IsNullOrEmpty(www.error))
    {
      string[] strArray = www.downloadHandler.text.Split('&');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = strArray[index].Split('=')[0];
        string s = Uri.UnescapeDataString(strArray[index].Split('=')[1]);
        switch (str)
        {
          case "rating_increment":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyRatingSearchIncrement);
            break;
          case "beta_version":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.betaMultiplayerVersionRequired);
            break;
          case "min_refresh":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyMinRefresh);
            break;
          case "max_refresh":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyMaxRefresh);
            break;
          case "version":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerVersionRequired);
            break;
          case "ignore_rating_wait":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerIgnoreRankWaitTime);
            break;
          case "dist_wait_2":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyDistanceWait2);
            break;
          case "dist_wait_1":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyDistanceWait1);
            break;
          case "ignore_last_player":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerIgnoreLastPlayerWaitTime);
            break;
          case "rating_start":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerLobbyRatingSearchStartBounds);
            break;
          case "ignore_rating_on_version":
            Debug.Log((object) (str + ": " + s));
            int.TryParse(s, out CGameManager.multiplayerIgnoreRatingOnVersion);
            break;
        }
      }
      if (isThrowPopup && (CNetworkManager.network as CNetworkSteam).GameNetworkVersion < CGameManager.multiplayerVersionRequired || (CNetworkManager.network as CNetworkSteam).IsBeta && (CNetworkManager.network as CNetworkSteam).GameNetworkVersion < CGameManager.betaMultiplayerVersionRequired)
      {
        IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
        screen.HideAllSubScreens();
        screen.ShowInitialSubScreen();
        (screen as CMainMenuScreen).OnMultiplayerVersionError();
      }
    }
    else
      Debug.LogError((object) ("Error game info: " + www.error));
  }

  private void LanguageChanged(string str, string code)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.LoadNews(code));
    this.StartCoroutine(this.LoadPopup(code));
  }

  private IEnumerator LoadNews(string code)
  {
    if (code != this.loadedLanguage)
    {
      CGameManager.dynamicNewsText = PlayerPrefs.GetString("lastDynamicNews" + code, "");
      UnityWebRequest www = UnityWebRequest.Get(this.dynamicNewsURL.Replace("LANG", code));
      yield return (object) www.SendWebRequest();
      if (www.isDone && string.IsNullOrEmpty(www.error))
      {
        this.dynamicNewsText = www.downloadHandler.text;
        PlayerPrefs.SetString("lastDynamicNews" + code, this.dynamicNewsText);
        CGameManager.dynamicNewsText = this.dynamicNewsText;
      }
      else
        Debug.LogError((object) ("Error loading news: " + www.error));
      this.loadedLanguage = code;
      www = (UnityWebRequest) null;
      www = (UnityWebRequest) null;
    }
  }

  private IEnumerator LoadPopup(string code)
  {
    if (code != this.loadedLanguage)
    {
      string postData = CUtils.LoadData("Dynamic/game_services_request");
      if (!string.IsNullOrEmpty(postData))
        postData = postData.Replace("Player_UID", SystemInfo.deviceUniqueIdentifier).Replace("\"Player_SID\"", this.sessionID.ToString()).Replace("Player_Lang", code).Replace("Player_Ver", COptionsManager.instance.version).Replace("Player_Popups", PlayerPrefs.GetString("Dynamic_Popups_Shown", string.Empty)).Replace("Player_Files", PlayerPrefs.GetString("Dynamic_Files_Shown", string.Empty)).Replace("\"Player_FirstTime\"", PlayerPrefs.GetString("Player_FirstTime", "0"));
      UnityWebRequest www = UnityWebRequest.Post(this.dynamicPopupURL, postData);
      yield return (object) www.SendWebRequest();
      if (www.isDone && string.IsNullOrEmpty(www.error))
      {
        this.dynamicPopupText = www.downloadHandler.text;
        JSONNode jsonNode = JSON.Parse(this.dynamicPopupText)["msg"];
        CGameManager.dynamicPopupText = !(jsonNode == (object) null) ? jsonNode.ToString() : "";
      }
      else
        Debug.LogError((object) ("Error loading popup: " + www.error));
      this.loadedLanguage = code;
      www = (UnityWebRequest) null;
      www = (UnityWebRequest) null;
    }
  }

  public DynamicNewsLoader()
  {
    this.gameInfoURL = CGameManager.federalServerAddress + "MultiplayerConfig/multiplayerConfig.txt";
    this.dynamicNewsURL = "http://s-cdn.ndemiccreations.com/LANG/plague/dn/0";
    this.dynamicPopupURL = "http://s.ndemiccreations.com/plague/open";
    this.dynamicPopupURLTest = "http://s1.ndemiccreations.com/plague/open";
  }
}
