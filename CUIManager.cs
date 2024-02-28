// Decompiled with JetBrains decompiler
// Type: CUIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CUIManager : MonoBehaviour
{
  [Header("Cameras")]
  public Camera mpCamera;
  public Camera[] renderEffectCameras;
  public Camera earthCamera;
  [Header("Screens")]
  public IGameScreen mpBlackoutScreen;
  public GameObject introVideoScreen;
  [Header("Effects")]
  public CountryParticles reanimateEffect;
  public CountryParticles rampageEffect;
  [Header("Misc")]
  public GameObject hexGrid;
  public GameObject hexGridStandard;
  public GameObject hexGridCure;
  public CUIManager.OnChangeScreen onChangeScreen;
  public CUIManager.OnChangeOverlays onChangeOverlays;
  private GameObject introVideoInstance;
  internal bool canSkipVideo;
  private Vector2 camInitialPos;
  private Vector2 camPos;
  private bool mbCameraMoving;
  private Dictionary<string, IGameScreen> mGameScreens;
  private Dictionary<string, CGameOverlay> mGameOverlays;
  private IGameScreen mpCurrentScreen;
  private List<CGameOverlay> mlCurrentOverlays = new List<CGameOverlay>();
  private List<IGameScreen> mlScreenStack;
  private const string EARLY_ACCESS_URL = "http://www.ndemiccreations.com/en/31-plague-inc-evolved-early-access";
  public static CUIManager instance;
  public CConfirmOverlay redConfirmOverlay;
  public CConfirmOverlay redSmallConfirmOverlay;
  public CConfirmImageOverlay redConfirmImageOverlay;
  public CConfirmOverlay rateModal;
  public CConfirmOverlay redConfirmOverlayCure;
  public CConfirmOverlay redSmallConfirmOverlayCure;
  public CConfirmImageOverlay redConfirmImageOverlayCure;
  public CConfirmOverlay standardConfirmOverlay;
  public CConfirmOverlay earlyAccessConfirmOverlay;
  public CConfirmOverlay kickstarterConfirmOverlay;
  public CConfirmOverlay cureKickstarterConfirmOverlay;
  public CConfirmOverlay mobileConfirmOverlay;
  public CConfirmOverlay cureMobileConfirmOverlay;
  public CConfirmOverlay cureConfirmOverlay;
  public CConfirmOverlay cureDLCOverlay;
  public CConfirmOverlay redSingleButtonConfirmOverlay;
  public CConfirmOverlay redSingleButtonConfirmOverlayCure;
  public CPopupOverlay popupOverlay;
  public CChatOverlay chatOverlay;
  public IGameScreen[] allScreens;
  public CGameOverlay[] allOverlays;
  public IGameScreen initialScreen;
  public IGameScreen errorScreen;
  public IGameScreen dllErrorScreen;
  public IGameScreen gameCenterErrorScreen;
  public IGameScreen stwEditionInitialScreen;
  private List<CUIManager.ScreenHistory> screenHistory = new List<CUIManager.ScreenHistory>();
  private IDictionary<string, string> screenSubstitutions = (IDictionary<string, string>) new Dictionary<string, string>();

  public int CameraPixelSize => Screen.height;

  public Camera NGUICamera => this.mpCamera;

  private void Awake()
  {
    CUIManager.instance = this;
    if (Application.isPlaying)
    {
      UIPanel.allowSorting = false;
      for (int index = 0; index < this.allScreens.Length; ++index)
      {
        if ((UnityEngine.Object) this.allScreens[index].transform.parent == (UnityEngine.Object) null)
        {
          IGameScreen igameScreen = UnityEngine.Object.Instantiate<IGameScreen>(this.allScreens[index], this.mpCamera.transform.position, Quaternion.identity);
          igameScreen.name = this.allScreens[index].name;
          igameScreen.transform.parent = this.mpCamera.transform;
          igameScreen.transform.localScale = this.allScreens[index].transform.localScale;
          igameScreen.transform.localPosition = this.allScreens[index].transform.localPosition;
          if ((UnityEngine.Object) this.allScreens[index] == (UnityEngine.Object) this.initialScreen)
            this.initialScreen = igameScreen;
          if ((UnityEngine.Object) this.allScreens[index] == (UnityEngine.Object) this.errorScreen)
            this.errorScreen = igameScreen;
          if ((UnityEngine.Object) this.allScreens[index] == (UnityEngine.Object) this.dllErrorScreen)
            this.dllErrorScreen = igameScreen;
          if ((UnityEngine.Object) this.allScreens[index] == (UnityEngine.Object) this.gameCenterErrorScreen)
            this.gameCenterErrorScreen = igameScreen;
          this.allScreens[index] = igameScreen;
          igameScreen.gameObject.SetActive(true);
          foreach (Animator componentsInChild in igameScreen.GetComponentsInChildren<Animator>(true))
            componentsInChild.applyRootMotion = false;
        }
      }
      for (int index = 0; index < this.allOverlays.Length; ++index)
      {
        if ((UnityEngine.Object) this.allOverlays[index].transform.parent == (UnityEngine.Object) null)
        {
          CGameOverlay cgameOverlay = UnityEngine.Object.Instantiate<CGameOverlay>(this.allOverlays[index], this.mpCamera.transform.position, Quaternion.identity);
          cgameOverlay.name = this.allOverlays[index].name;
          cgameOverlay.transform.parent = this.mpCamera.transform;
          cgameOverlay.transform.localScale = this.allOverlays[index].transform.localScale;
          cgameOverlay.transform.localPosition = this.allOverlays[index].transform.localPosition;
          cgameOverlay.gameObject.SetActive(true);
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redConfirmOverlay)
            this.redConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redConfirmOverlayCure)
            this.redConfirmOverlayCure = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.standardConfirmOverlay)
            this.standardConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redSmallConfirmOverlay)
            this.redSmallConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redSmallConfirmOverlayCure)
            this.redSmallConfirmOverlayCure = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.earlyAccessConfirmOverlay)
            this.earlyAccessConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.kickstarterConfirmOverlay)
            this.kickstarterConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.cureKickstarterConfirmOverlay)
            this.cureKickstarterConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.cureConfirmOverlay)
            this.cureConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.cureDLCOverlay)
            this.cureDLCOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.mobileConfirmOverlay)
            this.mobileConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.cureMobileConfirmOverlay)
            this.cureMobileConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redSingleButtonConfirmOverlay)
            this.redSingleButtonConfirmOverlay = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redSingleButtonConfirmOverlayCure)
            this.redSingleButtonConfirmOverlayCure = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.rateModal)
            this.rateModal = cgameOverlay as CConfirmOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redConfirmImageOverlay)
            this.redConfirmImageOverlay = cgameOverlay as CConfirmImageOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.redConfirmImageOverlayCure)
            this.redConfirmImageOverlayCure = cgameOverlay as CConfirmImageOverlay;
          if ((UnityEngine.Object) this.allOverlays[index] == (UnityEngine.Object) this.popupOverlay)
            this.popupOverlay = cgameOverlay as CPopupOverlay;
          this.allOverlays[index] = cgameOverlay;
          foreach (Animator componentsInChild in cgameOverlay.GetComponentsInChildren<Animator>(true))
            componentsInChild.applyRootMotion = false;
        }
      }
      if ((UnityEngine.Object) this.mpBlackoutScreen.transform.parent == (UnityEngine.Object) null)
      {
        IGameScreen igameScreen = UnityEngine.Object.Instantiate<IGameScreen>(this.mpBlackoutScreen, this.mpCamera.transform.position, Quaternion.identity);
        igameScreen.name = this.mpBlackoutScreen.name;
        igameScreen.transform.parent = this.mpCamera.transform;
        igameScreen.transform.localScale = this.mpBlackoutScreen.transform.localScale;
        igameScreen.transform.localPosition = this.mpBlackoutScreen.transform.localPosition;
        this.mpBlackoutScreen = igameScreen;
        igameScreen.gameObject.SetActive(true);
      }
      UIPanel.allowSorting = true;
    }
    this.hexGrid.gameObject.SetActive(true);
  }

  private string GetComponentPath(UnityEngine.Component obj)
  {
    string componentPath = "/" + obj.name;
    while ((UnityEngine.Object) obj.transform.parent != (UnityEngine.Object) null)
    {
      obj = (UnityEngine.Component) obj.transform.parent;
      componentPath = "/" + obj.name + componentPath;
    }
    return componentPath;
  }

  public void Initialise()
  {
    if (this.mGameScreens == null)
    {
      this.mGameScreens = new Dictionary<string, IGameScreen>();
      this.mGameOverlays = new Dictionary<string, CGameOverlay>();
    }
    else
    {
      this.mGameScreens.Clear();
      this.mGameOverlays.Clear();
    }
    for (int index = 0; index < this.allScreens.Length; ++index)
      this.AddScreen(this.allScreens[index]);
    this.AddScreen(this.mpBlackoutScreen);
    for (int index = 0; index < this.allOverlays.Length; ++index)
      this.AddOverlay(this.allOverlays[index]);
    this.camInitialPos = new Vector2(this.mpCamera.transform.position.x, this.mpCamera.transform.position.y);
    this.camPos = new Vector2(this.camInitialPos.x, this.camInitialPos.y);
    this.mbCameraMoving = false;
    this.InitScreens();
    this.HideInactive();
    this.SetupScreens();
    this.mpCurrentScreen = this.mpBlackoutScreen;
    this.mpBlackoutScreen.SetActive(true);
    Camera_Zoom.instance.enabled = false;
    if (Main.DLLLoadError)
      this.ShowDLLErrorScreen();
    else if (Main.showSplashVideo)
    {
      CNetworkSteam network = CNetworkManager.network as CNetworkSteam;
      if ((UnityEngine.Object) network != (UnityEngine.Object) null && network.WasGameStartedBySteamLobbyInvite)
      {
        this.ShowOverlay("SplashScreen");
        Main.showSplashVideo = false;
      }
      else
        this.StartCoroutine(this.IntroVideo());
    }
    else
      this.ShowOverlay("SplashScreen");
    CActionManager.instance.AddListener("SC_INPUT_CLOSE_PAUSE_MENU", new Action<CActionManager.ActionType>(this.ClosePauseScreen), this.gameObject);
  }

  private IEnumerator IntroVideo()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(0.1f);
    yield return (object) new WaitForEndOfFrame();
    this.introVideoInstance = NGUITools.AddChild(this.mpCamera.gameObject, this.introVideoScreen);
    this.introVideoInstance.transform.parent = this.mpCamera.transform;
    this.introVideoInstance.transform.localScale = this.introVideoScreen.transform.localScale;
    this.introVideoInstance.transform.localPosition = this.introVideoScreen.transform.localPosition;
    this.introVideoInstance.name = this.introVideoScreen.name;
  }

  public void IntroSplashReadyToPlay() => this.mpBlackoutScreen.SetActive(false);

  public void ShowInitialScreen() => this.SetActiveScreen(this.initialScreen);

  public void ShowErrorScreen() => this.SetActiveScreen(this.errorScreen);

  public void ShowDLLErrorScreen() => this.SetActiveScreen(this.dllErrorScreen);

  public void Update()
  {
    if (!this.mbCameraMoving)
      return;
    if ((double) Mathf.Pow(this.camPos.x - this.mpCamera.transform.position.x, 2f) + (double) Mathf.Pow(this.camPos.y - this.mpCamera.transform.position.y, 2f) > 9.9999999747524271E-07)
    {
      Vector3 position = this.mpCamera.transform.position;
      position.x += (float) (((double) this.camPos.x - (double) position.x) * (double) Time.deltaTime * 7.0);
      position.y += (float) (((double) this.camPos.y - (double) position.y) * (double) Time.deltaTime * 7.0);
      this.mpCamera.transform.position = position;
    }
    else
    {
      this.mpCamera.transform.position = this.mpCamera.transform.position with
      {
        x = this.camPos.x,
        y = this.camPos.y
      };
      this.mbCameraMoving = false;
    }
  }

  public void TrackCameraToPosition(float x, float y)
  {
    this.camPos.x = x;
    this.camPos.y = y;
    this.mbCameraMoving = true;
  }

  public void TrackCameraToOrigin()
  {
    this.camPos.x = this.camInitialPos.x;
    this.camPos.y = this.camInitialPos.y;
    this.mbCameraMoving = true;
  }

  public void TrackCameraToOriginOffset(float xOffset, float yOffset)
  {
    this.camPos.x = this.camInitialPos.x + xOffset;
    this.camPos.y = this.camInitialPos.y + yOffset;
    this.mbCameraMoving = true;
  }

  public void InitScreens()
  {
    foreach (KeyValuePair<string, IGameScreen> mGameScreen in this.mGameScreens)
      mGameScreen.Value.Initialise();
    foreach (KeyValuePair<string, CGameOverlay> mGameOverlay in this.mGameOverlays)
      mGameOverlay.Value.Initialise();
  }

  public void SetHexGrid(bool cure)
  {
    this.hexGridStandard.gameObject.SetActive(!cure);
    this.hexGridCure.gameObject.SetActive(cure);
  }

  public void SetupScreens()
  {
    CInterfaceManager.instance.SetCursorSelection(EHudMode.Normal);
    this.SetHexGrid(false);
    foreach (KeyValuePair<string, IGameScreen> mGameScreen in this.mGameScreens)
      mGameScreen.Value.Setup();
    foreach (KeyValuePair<string, CGameOverlay> mGameOverlay in this.mGameOverlays)
      mGameOverlay.Value.Setup();
  }

  public void HideInactive()
  {
    foreach (KeyValuePair<string, IGameScreen> mGameScreen in this.mGameScreens)
    {
      if ((UnityEngine.Object) mGameScreen.Value != (UnityEngine.Object) this.mpCurrentScreen)
        mGameScreen.Value.SetActive(false);
    }
    foreach (KeyValuePair<string, CGameOverlay> mGameOverlay in this.mGameOverlays)
    {
      if (this.mlCurrentOverlays.IndexOf(mGameOverlay.Value) < 0)
        mGameOverlay.Value.SetActive(false);
    }
  }

  public bool GetOpenOverlay(out CGameOverlay overlay)
  {
    if (this.mGameOverlays == null)
    {
      overlay = (CGameOverlay) null;
      return false;
    }
    foreach (KeyValuePair<string, CGameOverlay> mGameOverlay in this.mGameOverlays)
    {
      if (mGameOverlay.Value.gameObject.activeInHierarchy)
      {
        overlay = mGameOverlay.Value;
        return true;
      }
    }
    overlay = (CGameOverlay) null;
    return false;
  }

  public void HideAllOverlays()
  {
    foreach (CGameOverlay mlCurrentOverlay in this.mlCurrentOverlays)
      mlCurrentOverlay.SetActive(false);
    this.mlCurrentOverlays.Clear();
  }

  public virtual void SaveBreadcrumb(IGameScreen screen, List<IGameSubScreen> subScreens = null)
  {
    if ((UnityEngine.Object) screen == (UnityEngine.Object) null)
      screen = this.mpCurrentScreen;
    CUIManager.ScreenHistory screenHistory = new CUIManager.ScreenHistory();
    screenHistory.screen = screen;
    screenHistory.subScreens = new List<IGameSubScreen>();
    for (int index = 0; index < subScreens.Count; ++index)
    {
      IGameSubScreen subScreen = subScreens[index];
      screenHistory.subScreens.Add(subScreen);
    }
    this.screenHistory.Add(screenHistory);
  }

  public virtual void SaveBreadcrumbCurrent()
  {
    CUIManager.ScreenHistory screenHistory = new CUIManager.ScreenHistory();
    screenHistory.screen = this.mpCurrentScreen;
    screenHistory.subScreens = new List<IGameSubScreen>();
    for (int index = 0; index < this.mpCurrentScreen.subScreens.Count; ++index)
    {
      IGameSubScreen subScreen = this.mpCurrentScreen.subScreens[index];
      if ((bool) (UnityEngine.Object) subScreen && subScreen.gameObject.activeSelf)
        screenHistory.subScreens.Add(subScreen);
    }
    this.screenHistory.Add(screenHistory);
  }

  public void GoBack()
  {
    if (this.screenHistory.Count > 0)
    {
      CUIManager.ScreenHistory screenHistory = this.screenHistory.Last<CUIManager.ScreenHistory>();
      if ((UnityEngine.Object) this.mpCurrentScreen == (UnityEngine.Object) screenHistory.screen)
      {
        this.mpCurrentScreen.SetActiveSubScreen((IGameSubScreen) null);
        for (int index = 0; index < screenHistory.subScreens.Count; ++index)
          this.mpCurrentScreen.SetActiveSubScreen(screenHistory.subScreens[index]);
      }
      else
        this.SetActiveScreen(screenHistory.screen, overrideSubScreens: screenHistory.subScreens);
      this.screenHistory.Remove(this.screenHistory.LastOrDefault<CUIManager.ScreenHistory>());
    }
    else
    {
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
        return;
      Debug.LogError((object) ("Tried to go back from " + (object) this.mpCurrentScreen + " - no screen history to go back to"));
      if ((bool) (UnityEngine.Object) CGameManager.game && CGameManager.game.CurrentGameState == IGame.GameState.InProgress)
        this.OpenGameScreen();
      else
        this.SetActiveScreen("MainMenuScreen");
    }
  }

  public void ClearHistory() => this.screenHistory.Clear();

  public void OpenGameScreen()
  {
    this.SaveBreadcrumbCurrent();
    this.SetActiveScreen((IGameScreen) (this.GetScreen("HUDScreen") as CHUDScreen));
  }

  public void OpenPauseScreen(bool canSave = true)
  {
    CPauseScreen screen = CUIManager.instance.GetScreen("PauseScreen") as CPauseScreen;
    if ((UnityEngine.Object) CUIManager.instance.GetCurrentScreen() != (UnityEngine.Object) screen)
      CUIManager.instance.SaveBreadcrumbCurrent();
    CUIManager.instance.SetActiveScreen((IGameScreen) screen);
    if ((UnityEngine.Object) CNetworkManager.network != (UnityEngine.Object) null && CNetworkManager.network.NumberOfConnectedPlayers > 1)
    {
      canSave = false;
      screen.SetLoadState(false);
    }
    screen.SetSaveState(canSave);
  }

  public void OpenMultiplayerPauseScreen(bool saveBreadcrumb = true)
  {
    if (saveBreadcrumb)
      CUIManager.instance.SaveBreadcrumbCurrent();
    CPauseScreen screen = CUIManager.instance.GetScreen("PauseScreen") as CPauseScreen;
    CUIManager.instance.SetActiveScreen((IGameScreen) screen);
    screen.SetActiveSubScreen(screen.GetSubScreen("Pause_Sub_MP"));
  }

  public void ClosePauseScreen(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START || !((UnityEngine.Object) this.mpCurrentScreen == (UnityEngine.Object) (CUIManager.instance.GetScreen("PauseScreen") as CPauseScreen)) || CUIManager.instance.GetOpenOverlay(out CGameOverlay _))
      return;
    this.GoBack();
  }

  public void OpenCountryScreen(Country country = null, Disease disease = null)
  {
    if (this.IsEndScreenShowing() || CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      return;
    country = country ?? CInterfaceManager.instance.SelectedCountryView.GetCountry();
    disease = disease ?? CGameManager.localPlayerInfo.disease;
    this.SaveBreadcrumbCurrent();
    CCountryScreen screen = this.GetScreen("CountryScreen") as CCountryScreen;
    screen.SetCountry(country, disease);
    this.SetActiveScreen((IGameScreen) screen);
  }

  public void OpenWorldScreen()
  {
    if (this.IsEndScreenShowing())
      return;
    this.SaveBreadcrumbCurrent();
    this.SetActiveScreen((IGameScreen) (this.GetScreen("WorldScreen") as CWorldScreen));
  }

  private bool IsEndScreenShowing()
  {
    return this.GetOverlay("MP_WinOverlay").gameObject.activeSelf || this.GetOverlay("MP_LoseOverlay").gameObject.activeSelf || this.GetOverlay("MP_DrawOverlay").gameObject.activeSelf;
  }

  public void OpenDiseaseScreen()
  {
    if (this.IsEndScreenShowing())
      return;
    this.SaveBreadcrumbCurrent();
    this.SetActiveScreen((IGameScreen) (this.GetScreen("DiseaseScreen") as CDiseaseScreen));
  }

  public CGraphScreen OpenGraphScreen(string graph = null)
  {
    graph = graph ?? "Population";
    this.SaveBreadcrumbCurrent();
    CGraphScreen screen = this.GetScreen("GraphScreen") as CGraphScreen;
    screen.SetGraphExclusive(graph);
    this.SetActiveScreen((IGameScreen) screen);
    return screen;
  }

  public void WaitForFrame(Action action) => this.StartCoroutine(this.WaitForFrameCo(action));

  private IEnumerator WaitForFrameCo(Action action)
  {
    yield return (object) new WaitForEndOfFrame();
    if (action != null)
      action();
  }

  public void AddScreen(IGameScreen screen)
  {
    this.mGameScreens[screen.name] = screen;
    screen.SetActive(false);
  }

  public void RemoveScreen(string screenName)
  {
    if (screenName.Length == 0 || !this.mGameScreens.ContainsKey(screenName))
      return;
    IGameScreen component = this.mGameScreens[screenName].GetComponent<IGameScreen>();
    this.mGameScreens.Remove(screenName);
    component.DestroySelf();
  }

  public IGameScreen GetScreen(string screenName)
  {
    if (screenName.Length != 0)
    {
      if (this.screenSubstitutions.ContainsKey(screenName))
        screenName = this.screenSubstitutions[screenName];
      if (this.mGameScreens.ContainsKey(screenName))
        return this.mGameScreens[screenName];
    }
    return (IGameScreen) null;
  }

  public void SetSubScreen(IGameSubScreen sub)
  {
    CActionManager.instance.ClearDoubleClick();
    this.mpCurrentScreen.SetActiveSubScreen(sub);
  }

  public void SetActiveScreen(
    string screenName,
    float fFadeOutDuration = 0.0f,
    float fFadeInDuration = 0.0f,
    List<IGameSubScreen> overrideSubScreens = null)
  {
    CActionManager.instance.ClearDoubleClick();
    IGameScreen screen = this.GetScreen(screenName);
    if ((UnityEngine.Object) screen != (UnityEngine.Object) null)
      this.SetActiveScreen(screen, fFadeOutDuration, fFadeInDuration, overrideSubScreens);
    else
      Debug.LogError((object) ("Unknown screen: '" + screenName + "'"));
  }

  public void BackToScreen(string screenName, float fFadeOutDuration = 0.0f, float fFadeInDuration = 0.0f)
  {
    if (!this.mGameScreens.ContainsKey(screenName))
      return;
    this.SetActiveScreen(this.mGameScreens[screenName], fFadeOutDuration, fFadeInDuration, goBack: true);
  }

  public void SetActiveScreen(
    IGameScreen newScreen,
    float fFadeOutDuration = 0.0f,
    float fFadeInDuration = 0.0f,
    List<IGameSubScreen> overrideSubScreens = null,
    bool goBack = false)
  {
    if (!((UnityEngine.Object) newScreen != (UnityEngine.Object) this.mpCurrentScreen))
      return;
    this.StopAllCoroutines();
    this.StartCoroutine(this.CoSetActiveScreen(newScreen, fFadeOutDuration, fFadeInDuration, overrideSubScreens, goBack));
    CSteamControllerManager.instance.UpdateActionSet(newScreen);
  }

  public void ClearAllCameras()
  {
    foreach (Behaviour renderEffectCamera in this.renderEffectCameras)
      renderEffectCamera.enabled = false;
  }

  public void AddScreenSubstitution(string screen1, string screen2)
  {
    this.screenSubstitutions[screen1] = screen2;
  }

  public void ClearScreenSubstitutions() => this.screenSubstitutions.Clear();

  private IEnumerator CoSetActiveScreen(
    IGameScreen screen,
    float fFadeOutDuration,
    float fFadeInDuration,
    List<IGameSubScreen> overrideSubScreens,
    bool goBack)
  {
    if ((UnityEngine.Object) this.mpCurrentScreen != (UnityEngine.Object) screen)
    {
      foreach (Camera renderEffectCamera in this.renderEffectCameras)
      {
        bool flag1 = (UnityEngine.Object) screen != (UnityEngine.Object) null && screen.enabledRenderEffectCameras.Contains(renderEffectCamera.name);
        bool flag2 = (UnityEngine.Object) this.mpCurrentScreen != (UnityEngine.Object) null && this.mpCurrentScreen.enabledRenderEffectCameras.Contains(renderEffectCamera.name);
        renderEffectCamera.enabled = flag1 | flag2;
      }
      if ((UnityEngine.Object) this.mpCurrentScreen != (UnityEngine.Object) null)
      {
        if ((double) fFadeOutDuration > 0.0)
        {
          while ((double) this.mpCurrentScreen.Alpha > 0.0)
          {
            this.mpCurrentScreen.Alpha -= Time.deltaTime / fFadeOutDuration;
            yield return (object) null;
          }
        }
        else
          this.mpCurrentScreen.Alpha = 0.0f;
        this.mpCurrentScreen.SetActive(false);
      }
      this.mpCurrentScreen = screen;
      if (overrideSubScreens != null)
        this.mpCurrentScreen.overrideSubScreens = overrideSubScreens;
      if ((UnityEngine.Object) screen != (UnityEngine.Object) null)
      {
        Camera_Zoom.instance.enabled = screen.enableZoomCamera;
        screen.reloadHistory = goBack;
        if ((double) fFadeInDuration > 0.0)
        {
          screen.Alpha = 0.0f;
          screen.SetActive(true);
          while ((double) screen.Alpha < 1.0)
          {
            screen.Alpha += Time.deltaTime / fFadeInDuration;
            yield return (object) null;
          }
        }
        else
        {
          screen.Alpha = 1f;
          screen.SetActive(true);
        }
        foreach (Camera renderEffectCamera in this.renderEffectCameras)
          renderEffectCamera.enabled = screen.enabledRenderEffectCameras.Contains(renderEffectCamera.name);
        if (this.onChangeScreen != null)
          this.onChangeScreen(screen);
      }
    }
  }

  public IGameScreen GetCurrentScreen() => this.mpCurrentScreen;

  public GameObject GetCurrentScreenChild(string textureName)
  {
    Transform transform = this.mpCurrentScreen.transform.Find(textureName);
    return (UnityEngine.Object) transform != (UnityEngine.Object) null ? transform.gameObject : (GameObject) null;
  }

  public void AddOverlay(CGameOverlay overlay) => this.mGameOverlays[overlay.name] = overlay;

  public void RemoveOverlay(string overlayName)
  {
    if (overlayName.Length == 0 || !this.mGameOverlays.ContainsKey(overlayName))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mGameOverlays[overlayName]);
    this.mGameOverlays.Remove(overlayName);
  }

  public void ShowOverlay(string screenName, float nFadeInTime = -1f)
  {
    if (screenName.Length == 0 || this.IsActiveOverlay(screenName) || !this.mGameOverlays.ContainsKey(screenName))
      return;
    this.ShowOverlay(this.mGameOverlays[screenName], nFadeInTime);
  }

  public void ShowOverlay(CGameOverlay pOverlay, float nFadeInTime = -1f)
  {
    if ((UnityEngine.Object) pOverlay == (UnityEngine.Object) null)
      return;
    pOverlay.SetActive(true);
    pOverlay.GetComponent<UIPanel>().alpha = 1f;
    if (!this.mlCurrentOverlays.Contains(pOverlay))
      this.mlCurrentOverlays.Add(pOverlay);
    if (this.onChangeOverlays == null)
      return;
    this.onChangeOverlays(this.mlCurrentOverlays);
  }

  public CGameOverlay GetOverlay(string overlayName)
  {
    return this.mGameOverlays.ContainsKey(overlayName) ? this.mGameOverlays[overlayName] : (CGameOverlay) null;
  }

  public void HideOverlay(string screenName, float nFadeOutTime = -1f)
  {
    if (screenName.Length == 0 || !this.IsActiveOverlay(screenName) || !this.mGameOverlays.ContainsKey(screenName))
      return;
    this.HideOverlay(this.mGameOverlays[screenName], nFadeOutTime);
  }

  public void ShowEarlyAccess()
  {
    CUIManager.instance.earlyAccessConfirmOverlay.ShowLocalised("IG_Early_Access_Warning_Title_Generic", "IG_Early_Access_Warning_Text_Generic", "IG_Early_Access_Warning_Text_Generic_Link_Button", "OK", (CConfirmOverlay.PressDelegate) (() => Application.OpenURL("http://www.ndemiccreations.com/en/31-plague-inc-evolved-early-access")));
  }

  public void HideOverlay(CGameOverlay pOverlay, float nFadeOutTime = -1f)
  {
    if ((UnityEngine.Object) pOverlay == (UnityEngine.Object) null)
      return;
    this.mlCurrentOverlays.Remove(pOverlay);
    pOverlay.SetActive(false);
  }

  public bool IsActiveOverlay(string overlayName)
  {
    return this.mGameOverlays.ContainsKey(overlayName) && (UnityEngine.Object) this.mlCurrentOverlays.Find((Predicate<CGameOverlay>) (o => o.name == overlayName)) != (UnityEngine.Object) null;
  }

  public bool IsHovering() => (UnityEngine.Object) UICamera.hoveredObject != (UnityEngine.Object) null;

  public GameObject GetHover() => UICamera.hoveredObject;

  public CConfirmOverlay LocalisedConfirmOverlay(
    CConfirmOverlay overlay,
    string title,
    string body,
    string buttonA = null,
    string buttonB = null,
    CConfirmOverlay.PressDelegate pressA = null,
    CConfirmOverlay.PressDelegate pressB = null,
    CConfirmOverlay.PressDelegate update = null)
  {
    overlay.Title = CLocalisationManager.GetText(title);
    overlay.Body = CLocalisationManager.GetText(body);
    overlay.ButtonTextA = buttonA == null ? (string) null : CLocalisationManager.GetText(buttonA);
    overlay.ButtonTextB = buttonB == null ? (string) null : CLocalisationManager.GetText(buttonB);
    overlay.UpdateCallback = update;
    overlay.ACallback = pressA;
    overlay.BCallback = pressB;
    this.ShowOverlay((CGameOverlay) overlay);
    return overlay;
  }

  public CConfirmOverlay RawConfirmOverlay(
    CConfirmOverlay overlay,
    string title,
    string body,
    string buttonA = null,
    string buttonB = null,
    CConfirmOverlay.PressDelegate pressA = null,
    CConfirmOverlay.PressDelegate pressB = null,
    CConfirmOverlay.PressDelegate update = null)
  {
    overlay.Title = title;
    overlay.Body = body;
    overlay.ButtonTextA = buttonA == null ? (string) null : buttonA;
    overlay.ButtonTextB = buttonB == null ? (string) null : buttonB;
    overlay.UpdateCallback = update;
    overlay.ACallback = pressA;
    overlay.BCallback = pressB;
    this.ShowOverlay((CGameOverlay) overlay);
    return overlay;
  }

  private string CapsToSprite(string name)
  {
    string str1 = name[0].ToString();
    string str2 = name.Substring(1);
    return string.Format("{0}{1}", (object) str1.ToUpper(), (object) str2.ToLower());
  }

  public void ShowUnlockPopup(CUIManager.Unlock unlock)
  {
    this.StartCoroutine(this.UnlockPopup(unlock));
  }

  private IEnumerator UnlockPopup(CUIManager.Unlock unlock)
  {
    yield return (object) null;
    CConfirmImageOverlay cconfirmImageOverlay = CUIManager.instance.redConfirmImageOverlay;
    bool flag = unlock.gameType == IGame.GameType.Cure;
    if (flag)
      cconfirmImageOverlay = CUIManager.instance.redConfirmImageOverlayCure;
    Disease.EDiseaseType? disease = unlock.disease;
    Gene gene = unlock.gene;
    if (disease.HasValue || gene != null)
    {
      if (disease.HasValue && gene != null)
      {
        CUIManager.instance.HideAllOverlays();
        if (flag)
          cconfirmImageOverlay.ShowLocalised("Cure_New_Plague_Team_Member_Discovered_Title", "Cure_New_Plague_Team_Member_Discovered_Text", buttonB: "OK");
        else
          cconfirmImageOverlay.ShowLocalised("FE_Plagues_New_Plague_Gene_Discovered_Title", "FE_Plagues_New_Plague_Gene_Discovered_Text", buttonB: "OK");
        string diseaseName = CGameManager.GetDiseaseName(disease.Value);
        cconfirmImageOverlay.Title = cconfirmImageOverlay.Title.Replace("%GAMEMODE", CLocalisationManager.GetText(CGameManager.GetGameModeName(unlock.gameType)));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%plague", CLocalisationManager.GetText(diseaseName));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%gene", CLocalisationManager.GetText(gene.geneName));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%gamemode", CLocalisationManager.GetText(CGameManager.GetGameModeName(unlock.gameType)));
        cconfirmImageOverlay.SpriteName = this.CapsToSprite(disease.Value.ToString());
      }
      else if (!disease.HasValue && gene != null)
      {
        CUIManager.instance.HideAllOverlays();
        if (flag)
          cconfirmImageOverlay.ShowLocalised("Cure_New_Team_Member_Discovered_Title", "Cure_New_Team_Member_Discovered_Text", buttonB: "OK");
        else
          cconfirmImageOverlay.ShowLocalised("New Gene Discovered", "FE_Plagues_New_Gene_Discovered_Text", buttonB: "OK");
        cconfirmImageOverlay.Title = cconfirmImageOverlay.Title.ToUpper();
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%gene", CLocalisationManager.GetText(gene.geneName));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%gamemode", CLocalisationManager.GetText(CGameManager.GetGameModeName(unlock.gameType)));
        cconfirmImageOverlay.SpriteName = gene.geneGraphic.Replace("'", "");
      }
      else if (disease.HasValue && gene == null)
      {
        string diseaseName = CGameManager.GetDiseaseName(disease.Value);
        CUIManager.instance.HideAllOverlays();
        cconfirmImageOverlay.ShowLocalised("FE_Plagues_New_Plague_Discovered_Title", "FE_Plagues_New_Plague_Discovered_Text", buttonB: "OK");
        cconfirmImageOverlay.Title = cconfirmImageOverlay.Title.Replace("%GAMEMODE", CLocalisationManager.GetText(CGameManager.GetGameModeName(unlock.gameType)));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%plague", CLocalisationManager.GetText(diseaseName));
        cconfirmImageOverlay.Body = cconfirmImageOverlay.Body.Replace("%gamemode", CLocalisationManager.GetText(CGameManager.GetGameModeName(unlock.gameType)));
        cconfirmImageOverlay.SpriteName = this.CapsToSprite(disease.Value.ToString());
      }
    }
  }

  public void PlayVideo()
  {
    this.StartCoroutine(this.IntroVideo());
    CActionManager.instance.AddListener("SC_INPUT_CLOSE_PAUSE_MENU", new Action<CActionManager.ActionType>(this.ClosePauseScreen), this.gameObject);
  }

  public delegate void OnChangeScreen(IGameScreen screen);

  public delegate void OnChangeOverlays(List<CGameOverlay> overlays);

  public struct ScreenData
  {
    public IGameScreen screenObject;
    public string sScreenName;
    public float fFadeOutDuration;
    public float fFadeInDuration;
  }

  public struct OverlayData
  {
    public CGameOverlay overlayObject;
    public string sOverlayName;
    public float fFadeOutDuration;
    public float fFadeInDuration;
  }

  public struct ScreenHistory
  {
    public IGameScreen screen;
    public List<IGameSubScreen> subScreens;
  }

  public struct Unlock
  {
    public Disease.EDiseaseType? disease;
    public Gene gene;
    public IGame.GameType gameType;
  }
}
