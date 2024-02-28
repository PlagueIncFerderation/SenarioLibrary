// Decompiled with JetBrains decompiler
// Type: CHUDScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
public class CHUDScreen : BaseMapScreen, ITutorial
{
  public static CHUDScreen instance;
  [Header("HUDElements")]
  [SerializeField]
  private GameObject diseaseHUDSubView;
  [SerializeField]
  private GameObject worldHUDSubView;
  [Header("Game Speed / Clock")]
  [SerializeField]
  private UIButton pauseButton;
  [SerializeField]
  private UIButton Speed0Button;
  [SerializeField]
  private UIButton Speed1Button;
  [SerializeField]
  private UIButton Speed2Button;
  [SerializeField]
  private UIButton Speed3Button;
  [SerializeField]
  private UIToggle speed0Toggle;
  [SerializeField]
  private UIToggle speed1Toggle;
  [SerializeField]
  private UIToggle speed2Toggle;
  [SerializeField]
  private UIToggle speed3Toggle;
  [SerializeField]
  private UILabel mpClockText;
  [SerializeField]
  private GameObject speedControlsContainer;
  [SerializeField]
  private CHUDMPPopupSubScreen mpMessage;
  [SerializeField]
  private UIButton mpEndGameButton;
  [SerializeField]
  private UILabel mpEndGameTitle;
  [Header("World / Cure")]
  [SerializeField]
  private UILabel cureText;
  [SerializeField]
  private UISlider cureBar;
  [SerializeField]
  private UILabel worldText;
  [SerializeField]
  private UIButton worldScreenButton;
  [SerializeField]
  private GameObject worldScreenControllerButton;
  [SerializeField]
  private UISprite worldScreenControllerButtonGlyph;
  [SerializeField]
  private ButtonChooseScreen worldBoxButtonChoose;
  [SerializeField]
  private Animator cureAnimator;
  [SerializeField]
  private CureParticles cureParticles;
  [SerializeField]
  private UISprite cureFillBottleGraphic;
  [Header("Disease / DNA")]
  [SerializeField]
  private UILabel diseaseText;
  [SerializeField]
  private UISlider diseaseBar;
  [SerializeField]
  private UIButton diseaseScreenButton;
  [SerializeField]
  private GameObject diseaseScreenControllerButton;
  [SerializeField]
  private UISprite diseaseScreenControllerButtonGlyph;
  [SerializeField]
  private ButtonChooseScreen diseaseBoxButtonChoose;
  [SerializeField]
  private UILabel dnaText;
  [SerializeField]
  private UILabel dnaIncreaseLabel;
  [SerializeField]
  private UITweener[] dnaIncreaseTweens;
  [SerializeField]
  private UILabel dnaDecreaseLabel;
  [SerializeField]
  private UITweener[] dnaDecreaseTweens;
  [Header("News")]
  public NewsHistory newsHistory;
  [SerializeField]
  private NewsItemObject newsObject;
  [SerializeField]
  private GameObject newsOffset;
  [SerializeField]
  private UIButton newsButton;
  [SerializeField]
  private UIButton newsButtonBox;
  [SerializeField]
  private UIButton newsButtonExpanded;
  [SerializeField]
  private DateTime currentDate;
  public GameObject newsScrollDetector;
  [Header("Context Bars (Mode Specific)")]
  [SerializeField]
  private UIButton countryContextButton;
  [SerializeField]
  private CHUDContextSubScreen baseSubscreen;
  [SerializeField]
  private CHUDNecroaSubScreen necroaSubscreen;
  [SerializeField]
  private CHUDSimianContextSubScreen simianSubScreen;
  [SerializeField]
  private CHUDContextSubScreen vampireSubscreen;
  [SerializeField]
  private CHUDCureContextSubScreen cureSubscreen;
  [SerializeField]
  private DefconObject defconObject;
  [Header("Cure game mode")]
  [SerializeField]
  private bool isCureGameMode;
  [SerializeField]
  private UILabel authorityText;
  [SerializeField]
  private UILabel authorityRateOfChangeText;
  [SerializeField]
  private UILabel authorityIncreaseLabel;
  [SerializeField]
  private UITweener[] authorityIncreaseTweens;
  [SerializeField]
  private UILabel authorityDecreaseLabel;
  [SerializeField]
  private UITweener[] authorityDecreaseTweens;
  [SerializeField]
  private UISlider authorityBar;
  [SerializeField]
  private UILabel curePercIncreaseLabel;
  [SerializeField]
  private UITweener[] curePercIncreaseTweens;
  [SerializeField]
  private UILabel curePercDecreaseLabel;
  [SerializeField]
  private UITweener[] curePercDecreaseTweens;
  [SerializeField]
  private UILabel cureStageText;
  [SerializeField]
  private UILabel cureRateText;
  [Header("Misc")]
  public CHUDAbilitySubScreen abilitySubscreen;
  [SerializeField]
  private GameObject actionsZombie;
  [SerializeField]
  private UIScrollBar mpZoomScroll;
  public CHUDGemSubScreen gemSubscreen;
  public GeneticDominanceBar geneticDominanceBar;
  public CoopBar coopBar;
  public CoopLabBar coopLabBar;
  [Header("Chat")]
  public Transform chatHolder;
  public CChatOverlay chatOverlayPfb;
  private DateTime mDate;
  private List<IGame.NewsItem> mlNews;
  private Animator actionsZombieAnimator;
  private CHUDPopupSubScreen popupSubscreen;
  private CHUDMPPopupSubScreen multiPlayerPopupSubscreen;
  private CHUDPopupSubScreen singlePlayerPopupSubscreen;
  private CHUDContextSubScreen contextSubscreen;
  private PopupMessage activePopup;
  private int lastDna;
  private int lastAuthValue;
  private float currentCureValue;
  private IEnumerator cureRoutine;
  private float scrollSpeed;
  private EHudMode meHudMode;
  private int lastNonZeroSpeed = 1;
  private float lastCountryClickTime = -1f;
  private CountryView lastCountryClicked;
  private Vector3 start = Vector3.zero;
  private Vector3 end = Vector3.zero;
  private DateTime startDate;
  private bool tutorialPauseState;
  private int dnaCountForTutorial11A;
  private MultiplayerData_Overlay multiplayerDataOverlay;
  private INetwork network;
  private Action<CActionManager.ActionType> inputSpeed1Action;
  private Action<CActionManager.ActionType> inputSpeed2Action;
  private Action<CActionManager.ActionType> inputSpeed3Action;
  private bool isLoopingCureSound;
  public AudioSource loopingCureSound;
  public VampireFlightRadiusObject vampireFlightRadiusPrefab;
  internal VampireFlightRadiusObject vampireFlightRadius;
  private bool hasPlayedLoopingCureSound;
  private int timesToPlay;
  private bool showDiseaseScreenButton = true;
  private bool showWorldScreenButton = true;

  public CChatOverlay chatOverlay { get; private set; }

  public EHudMode HudInterfaceMode
  {
    set
    {
      CInterfaceManager.instance.StopSpline();
      if ((bool) (UnityEngine.Object) this.vampireFlightRadius)
      {
        this.vampireFlightRadius.Hide();
        if (CGameManager.localPlayerInfo.disease.IsTechEvolved("shadow_portal"))
        {
          foreach (Country country in World.instance.countries)
          {
            LocalDisease localDisease = country.GetLocalDisease(CGameManager.localPlayerInfo.disease);
            if (localDisease != null && localDisease.castleState == ECastleState.CASTLE_ALIVE)
              CInterfaceManager.instance.GetCountryView(country.id).StopPulseBorders();
          }
        }
      }
      switch (this.meHudMode)
      {
        case EHudMode.Neurax:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            break;
          }
          break;
        case EHudMode.Reanimate:
        case EHudMode.ImmuneShock:
        case EHudMode.InfectBoost:
        case EHudMode.LethalBoost:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            CSoundManager.instance.PlaySFX("aasuccess");
            break;
          }
          break;
        case EHudMode.SelectHorde:
        case EHudMode.SelectUnscheduledFlight:
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.SendHorde:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            CSoundManager.instance.PlaySFX("horderelease");
            break;
          }
          break;
        case EHudMode.SelectApeHorde:
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.SendApeHorde:
        case EHudMode.SendApeColony:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            CSoundManager.instance.PlaySFX("ape_move_release");
            break;
          }
          break;
        case EHudMode.SendUnscheduledFlight:
        case EHudMode.NukeStrike:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            break;
          }
          break;
        case EHudMode.BloodRage:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            CSoundManager.instance.PlaySFX("bloodrage_start");
            break;
          }
          break;
        case EHudMode.SelectVampire:
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.SendVampire:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
          {
            WorldMapController.instance.SetSelectedCountry(CInterfaceManager.instance.HoverCountry);
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
            CSoundManager.instance.PlaySFX("vampire_move_release");
          }
          using (List<Country>.Enumerator enumerator = World.instance.countries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Country current = enumerator.Current;
              CountryView countryView = CInterfaceManager.instance.GetCountryView(current.id);
              if ((bool) (UnityEngine.Object) countryView.mpCastleObject)
                countryView.mpCastleObject.SetPortalActive(false);
            }
            break;
          }
      }
      this.meHudMode = value;
      CInterfaceManager.instance.SetCursorSelection(this.meHudMode);
      switch (this.meHudMode)
      {
        case EHudMode.Neurax:
        case EHudMode.SendHorde:
        case EHudMode.SendUnscheduledFlight:
        case EHudMode.NukeStrike:
          CInterfaceManager.instance.SetLineType(this.meHudMode);
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.Reanimate:
        case EHudMode.SelectHorde:
        case EHudMode.SelectUnscheduledFlight:
        case EHudMode.ImmuneShock:
        case EHudMode.InfectBoost:
        case EHudMode.LethalBoost:
          CSoundManager.instance.PlaySFX("aasuccess");
          break;
        case EHudMode.ApeRampage:
        case EHudMode.SelectApeHorde:
        case EHudMode.ApeCreateColony:
          CInterfaceManager.instance.SetHighlightMode(new Disease.EDiseaseType?(Disease.EDiseaseType.SIMIAN_FLU));
          CSoundManager.instance.PlaySFX("aasuccess");
          break;
        case EHudMode.SendApeHorde:
        case EHudMode.SendApeColony:
          CInterfaceManager.instance.SetHighlightMode(new Disease.EDiseaseType?(Disease.EDiseaseType.SIMIAN_FLU));
          CInterfaceManager.instance.SetLineType(this.meHudMode);
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.PlaceGem:
          CSoundManager.instance.PlaySFX("aasuccess");
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.MoveGem:
          WorldMapController.instance.SetSelectedCountry();
          break;
        case EHudMode.BloodRage:
        case EHudMode.SelectVampire:
        case EHudMode.CreateCastle:
          CInterfaceManager.instance.SetHighlightMode(new Disease.EDiseaseType?(Disease.EDiseaseType.VAMPIRE));
          CSoundManager.instance.PlaySFX("aasuccess");
          break;
        case EHudMode.SendVampire:
          CInterfaceManager.instance.SetHighlightMode(new Disease.EDiseaseType?(Disease.EDiseaseType.VAMPIRE));
          CInterfaceManager.instance.SetLineType(this.meHudMode);
          WorldMapController.instance.SetSelectedCountry();
          if (!(bool) (UnityEngine.Object) this.vampireFlightRadius)
          {
            this.vampireFlightRadius = UnityEngine.Object.Instantiate<VampireFlightRadiusObject>(this.vampireFlightRadiusPrefab);
            this.vampireFlightRadius.transform.parent = CInterfaceManager.instance.transform;
            this.vampireFlightRadius.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
            CInterfaceManager.instance.mpScaleObjects.Add((ScaleObject) this.vampireFlightRadius);
          }
          this.vampireFlightRadius.Show();
          this.vampireFlightRadius.transform.position = CInterfaceManager.instance.splineStartPosition + new Vector3(0.0f, 0.0f, 0.0f);
          this.vampireFlightRadius.SetFlightRadius(CNetworkManager.network.LocalPlayerInfo.disease.GetMaxVampireFlightDistance());
          bool active = CGameManager.localPlayerInfo.disease.IsTechEvolved("shadow_portal");
          using (List<Country>.Enumerator enumerator = World.instance.countries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Country current = enumerator.Current;
              LocalDisease localDisease = current.GetLocalDisease(CGameManager.localPlayerInfo.disease);
              if (localDisease != null && localDisease.castleState == ECastleState.CASTLE_ALIVE)
              {
                CountryView countryView = CInterfaceManager.instance.GetCountryView(current.id);
                if (active)
                  countryView.BeginPulseBorders(Color.white);
                if ((bool) (UnityEngine.Object) countryView.mpCastleObject)
                  countryView.mpCastleObject.SetPortalActive(active);
              }
            }
            break;
          }
        case EHudMode.SendInvestigationTeam:
        case EHudMode.RaisePriority:
        case EHudMode.EconomicSupport:
          if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
            CInterfaceManager.instance.HoverCountry = (CountryView) null;
          CSoundManager.instance.PlaySFX("aapickup");
          WorldMapController.instance.SetSelectedCountry();
          break;
        default:
          if (CInterfaceManager.instance.TabMode == EHudTabMode.NORMAL)
          {
            CInterfaceManager.instance.SetHighlightMode(new Disease.EDiseaseType?());
            break;
          }
          break;
      }
      if (CGameManager.IsCureTutorialGame && TutorialSystem.IsModuleActive("C11"))
        CInterfaceManager.instance.SetCountryHighlight(CInterfaceManager.instance.GetCountryView("brazil"), CountryView.EOverlayState.SELECTED);
      else
        CInterfaceManager.instance.SetCountryHighlights();
      if (CGameManager.IsMultiplayerGame)
        return;
      CGameManager.pauseInterface = this.meHudMode != EHudMode.Normal;
      CGameManager.SetPaused(this.meHudMode != EHudMode.Normal && this.meHudMode != EHudMode.PlaceGem && this.meHudMode != EHudMode.MoveGem && this.meHudMode != EHudMode.SelectUnscheduledFlight && this.meHudMode != EHudMode.SendUnscheduledFlight && this.meHudMode != EHudMode.NukeStrike && this.meHudMode != EHudMode.ImmuneShock || TutorialSystem.IsModuleActive());
    }
    get => this.meHudMode;
  }

  private void Awake()
  {
    if (!((UnityEngine.Object) CHUDScreen.instance == (UnityEngine.Object) null))
      return;
    CHUDScreen.instance = this;
  }

  public static void SetInstance()
  {
    (CUIManager.instance.GetScreen("HUDScreen") as CHUDScreen).SetInstanceToThis();
  }

  public void SetInstanceToThis() => CHUDScreen.instance = this;

  public override void Initialise()
  {
    base.Initialise();
    this.mlNews = new List<IGame.NewsItem>();
    this.actionsZombieAnimator = this.actionsZombie.GetComponent<Animator>();
    this.multiPlayerPopupSubscreen = this.subScreens.Find((Predicate<IGameSubScreen>) (a => a is CHUDMPPopupSubScreen)) as CHUDMPPopupSubScreen;
    this.singlePlayerPopupSubscreen = this.subScreens.Find((Predicate<IGameSubScreen>) (a => a is CHUDPopupSubScreen)) as CHUDPopupSubScreen;
    this.popupSubscreen = this.singlePlayerPopupSubscreen;
    this.newsObject.onFinishMovement = new NewsItemObject.OnFinishMovement(this.BufferNews);
    Camera_Zoom.instance.RegisterScrollBar(this.mpZoomScroll);
    if ((bool) (UnityEngine.Object) this.mpEndGameButton)
      EventDelegate.Set(this.mpEndGameButton.onClick, new EventDelegate.Callback(this.ShowEndGameScreen));
    EventDelegate.Set(this.mpZoomScroll.onChange, new EventDelegate.Callback(this.ZoomCamera));
    EventDelegate.Set(this.newsButton.onClick, new EventDelegate.Callback(this.newsHistory.ToggleNews));
    EventDelegate.Set(this.newsButtonExpanded.onClick, new EventDelegate.Callback(this.newsHistory.ToggleNews));
    EventDelegate.Set(this.Speed0Button.onClick, new EventDelegate.Callback(this.OnClickSetSpeed));
    EventDelegate.Set(this.Speed1Button.onClick, new EventDelegate.Callback(this.OnClickSetSpeed));
    EventDelegate.Set(this.Speed2Button.onClick, new EventDelegate.Callback(this.OnClickSetSpeed));
    EventDelegate.Set(this.Speed3Button.onClick, new EventDelegate.Callback(this.OnClickSetSpeed));
    this.speed0Toggle = this.Speed0Button.GetComponent<UIToggle>();
    this.speed1Toggle = this.Speed1Button.GetComponent<UIToggle>();
    this.speed2Toggle = this.Speed2Button.GetComponent<UIToggle>();
    this.speed3Toggle = this.Speed3Button.GetComponent<UIToggle>();
    EventDelegate.Set(this.pauseButton.onClick, new EventDelegate.Callback(this.GoToPause));
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject, 1);
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.ClickReturn), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.PressReturn), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
    this.inputSpeed1Action = (Action<CActionManager.ActionType>) (type => this.SetSpeed(type, 1));
    this.inputSpeed2Action = (Action<CActionManager.ActionType>) (type => this.SetSpeed(type, 2));
    this.inputSpeed3Action = (Action<CActionManager.ActionType>) (type => this.SetSpeed(type, 3));
    CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_SPEED_INC", new Action<CActionManager.ActionType>(this.IncreaseSpeed), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_SPEED_DEC", new Action<CActionManager.ActionType>(this.DecreaseSpeed), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_OPEN_PAUSE_MENU", new Action<CActionManager.ActionType>(this.GoToPause), this.gameObject);
    this.newsHistory.Init();
    TutorialSystem.RegisterInterface((ITutorial) this);
    this.dnaCountForTutorial11A = int.MaxValue;
  }

  public override void Setup()
  {
    base.Setup();
    this.hasPlayedLoopingCureSound = false;
    this.newsObject.offset = this.newsOffset.transform.localPosition.x - 200f;
    this.activePopup = (PopupMessage) null;
    this.mlNews.Clear();
    this.newsHistory.Init();
    this.singlePlayerPopupSubscreen?.SetActive(false);
    this.multiPlayerPopupSubscreen?.SetActive(false);
    if ((bool) (UnityEngine.Object) this.mpEndGameButton)
      this.mpEndGameButton.gameObject.SetActive(false);
    this.speedControlsContainer.SetActive(true);
    if (CGameManager.IsMultiplayerGame)
    {
      this.popupSubscreen = (CHUDPopupSubScreen) this.multiPlayerPopupSubscreen;
      this.speed0Toggle.transform.parent.localPosition = this.speed0Toggle.transform.parent.localPosition with
      {
        x = -152f
      };
      bool flag = CGameManager.IsCoopMPGame || CGameManager.IsAIGame;
      this.speed2Toggle.gameObject.SetActive(flag);
      this.speed3Toggle.gameObject.SetActive(flag);
      if (CGameManager.IsCoopMPGame)
      {
        this.geneticDominanceBar.transform.parent.gameObject.SetActive(false);
        this.coopBar.transform.parent.gameObject.SetActive(true);
        this.coopBar.ResetBuffs();
        this.coopLabBar.gameObject.SetActive(true);
        this.coopLabBar.Setup();
      }
      else if (CGameManager.IsVersusMPGame)
      {
        if (!CGameManager.IsAIGame)
        {
          Vector3 localPosition = this.speed0Toggle.transform.parent.localPosition;
          localPosition.x += 81f;
          this.speed0Toggle.transform.parent.localPosition = localPosition;
        }
        this.coopBar.transform.parent.gameObject.SetActive(false);
        this.coopLabBar.gameObject.SetActive(false);
        this.geneticDominanceBar.transform.parent.gameObject.SetActive(true);
        this.geneticDominanceBar.ResetBuffs();
      }
      if (!(bool) (UnityEngine.Object) this.chatOverlayPfb)
        return;
      if ((UnityEngine.Object) this.chatOverlay != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.chatOverlay.gameObject);
      if (CGameManager.IsAIGame)
        return;
      this.chatOverlay = UnityEngine.Object.Instantiate<CChatOverlay>(this.chatOverlayPfb);
      this.chatOverlay.transform.parent = this.chatHolder;
      this.chatOverlay.transform.localPosition = Vector3.zero;
      this.chatOverlay.transform.localScale = Vector3.one;
      CCountrySelect screen = (CCountrySelect) CUIManager.instance.GetScreen("StartCountryScreen");
      if ((UnityEngine.Object) screen != (UnityEngine.Object) null)
        this.chatOverlay.Init(CGameManager.game, isInitOpen: screen.chatOverlay.isOpen, initScrollIndex: screen.chatOverlay.scrollIndex, initText: screen.chatOverlay.chatIpt.value);
      else
        this.chatOverlay.Init(CGameManager.game);
    }
    else
      this.popupSubscreen = this.singlePlayerPopupSubscreen;
  }

  public void SetEnable(bool enabled)
  {
    Animator component = this.GetComponent<Animator>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    if (enabled)
      component.SetTrigger("Default");
    else
      component.SetTrigger("MP_Endgame");
  }

  public void Default()
  {
    if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
    {
      CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.OFF);
      CInterfaceManager.instance.HoverCountry = (CountryView) null;
    }
    this.abilitySubscreen.AbilityExit();
    if (this.HudInterfaceMode == EHudMode.PlaceGem)
      this.gemSubscreen.AbilityExit();
    if (this.HudInterfaceMode == EHudMode.MoveGem)
      CInterfaceManager.instance.CancelGem();
    WorldMapController.instance.SetSelectedCountry();
    this.HudInterfaceMode = EHudMode.Normal;
  }

  private void OnClickSetSpeed()
  {
    IntObject component = UIButton.current.gameObject.GetComponent<IntObject>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || TutorialSystem.CureTutorialModulesPauseSimulation)
      return;
    if (!CGameManager.IsMultiplayerGame && component.intVal > 0)
      CGameManager.SetPaused(false, true);
    this.SetSpeed(component.intVal);
  }

  private void SetSpeed(CActionManager.ActionType type, int speed)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.SetSpeed(speed);
  }

  private void ToggleSpeed(CActionManager.ActionType type)
  {
    CGameOverlay overlay;
    if (type != CActionManager.ActionType.START || CUIManager.instance.GetOpenOverlay(out overlay) && !(((object) overlay).GetType() != typeof (MultiplayerResultOverlay)))
      return;
    if (CGameManager.game.WantedSpeed == 0)
    {
      CGameManager.SetPaused(false, true);
      this.SetSpeed(this.lastNonZeroSpeed);
    }
    else
      this.SetSpeed(0);
  }

  private void IncreaseSpeed(CActionManager.ActionType type)
  {
    CGameOverlay overlay;
    if (type != CActionManager.ActionType.START || CUIManager.instance.GetOpenOverlay(out overlay) && !(((object) overlay).GetType() != typeof (MultiplayerResultOverlay)))
      return;
    if (CGameManager.game.WantedSpeed == 0)
    {
      CGameManager.SetPaused(false, true);
      this.SetSpeed(1);
    }
    else
      this.SetSpeed(Mathf.Min(CGameManager.game.WantedSpeed + 1, 3));
  }

  private void DecreaseSpeed(CActionManager.ActionType type)
  {
    CGameOverlay overlay;
    if (type != CActionManager.ActionType.START || CUIManager.instance.GetOpenOverlay(out overlay) && !(((object) overlay).GetType() != typeof (MultiplayerResultOverlay)))
      return;
    if (CGameManager.game.WantedSpeed <= 1)
      this.SetSpeed(0);
    else
      this.SetSpeed(CGameManager.game.WantedSpeed - 1);
  }

  private void SetSpeed(int speed)
  {
    CGameOverlay overlay;
    if (CUIManager.instance.GetOpenOverlay(out overlay) && ((object) overlay).GetType() == typeof (MultiplayerResultOverlay))
      return;
    if (TutorialSystem.IsModuleActive("7B") || TutorialSystem.IsModuleActive("8A") || TutorialSystem.IsModuleActive("9A"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (!CGameManager.IsMultiplayerGame && CGameManager.game.WantedSpeed == speed)
      return;
    if (!CGameManager.IsVersusMPGame)
      CGameManager.lastSpeed = speed;
    if (speed != 0)
      this.lastNonZeroSpeed = speed;
    CInterfaceManager.instance.SetGameSpeed(speed);
  }

  private void PressContinue(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    if ((bool) (UnityEngine.Object) this.GetActiveSubScreen("popup"))
    {
      this.ClosePopup();
    }
    else
    {
      if (!((UnityEngine.Object) this.chatOverlay != (UnityEngine.Object) null))
        return;
      this.chatOverlay.Toggle();
    }
  }

  public void AutoCloseChat()
  {
    if (!((UnityEngine.Object) this.chatOverlay != (UnityEngine.Object) null))
      return;
    this.chatOverlay.Hide();
  }

  private void PressReturn(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    if ((bool) (UnityEngine.Object) this.GetActiveSubScreen("popup"))
      this.ClosePopup();
    else if (this.newsHistory.isActive)
      this.newsHistory.ToggleNews();
    else if (this.HudInterfaceMode != EHudMode.Normal)
    {
      CInterfaceManager.instance.TargetBubbleCancel();
    }
    else
    {
      if (CUIManager.instance.IsActiveOverlay("ConfirmOverlay"))
        return;
      this.GoToPause();
    }
  }

  private void ClickReturn(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START || !(bool) (UnityEngine.Object) this.GetActiveSubScreen("popup"))
      return;
    this.ClosePopup();
  }

  private void GoToDisease(CActionManager.ActionType type)
  {
    CGameOverlay overlay;
    if (CGameManager.gameType == IGame.GameType.CureTutorial && !CDiseaseScreen.instance.CheckCureTutorialRestrictions() || type != CActionManager.ActionType.START || CUIManager.instance.GetOpenOverlay(out overlay) && !(((object) overlay).GetType() != typeof (MultiplayerResultOverlay)))
      return;
    if (CGameManager.IsTutorialGame)
    {
      if (TutorialSystem.IsModuleActive("6A") || TutorialSystem.IsModuleActive("11B"))
      {
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
      }
      this.dnaCountForTutorial11A = int.MaxValue;
      CUIManager.instance.WaitForFrame(new Action(CUIManager.instance.OpenDiseaseScreen));
    }
    else
      CUIManager.instance.OpenDiseaseScreen();
  }

  private void GoToWorld(CActionManager.ActionType type)
  {
    CGameOverlay overlay;
    if (CGameManager.gameType == IGame.GameType.CureTutorial && !CWorldScreen.instance.CheckCureTutorialRestrictions() || type != CActionManager.ActionType.START || CUIManager.instance.GetOpenOverlay(out overlay) && !(((object) overlay).GetType() != typeof (MultiplayerResultOverlay)))
      return;
    if (CGameManager.IsTutorialGame)
    {
      if (TutorialSystem.IsModuleActive("19B"))
      {
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
      }
      CUIManager.instance.WaitForFrame(new Action(CUIManager.instance.OpenWorldScreen));
    }
    else if ((UnityEngine.Object) CInterfaceManager.instance.SelectedCountryView != (UnityEngine.Object) null && CSteamControllerManager.instance.controllerActive)
      CUIManager.instance.OpenCountryScreen();
    else
      CUIManager.instance.OpenWorldScreen();
  }

  private void GoToPause() => this.GoToPause(CActionManager.ActionType.START);

  private void GoToPause(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START)
      return;
    if (CGameManager.IsMultiplayerGame)
      CUIManager.instance.OpenMultiplayerPauseScreen();
    else
      CUIManager.instance.OpenPauseScreen();
  }

  public override void SetActive(bool isActive)
  {
    base.SetActive(isActive);
    if (CGameManager.IsTutorialGame)
    {
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        this.showWorldScreenButton = false;
        this.showDiseaseScreenButton = true;
        if (CGameManager.gameType == IGame.GameType.CureTutorial)
        {
          this.speed0Toggle.enabled = false;
          this.speed1Toggle.enabled = false;
          this.speed2Toggle.enabled = false;
          this.speed3Toggle.enabled = false;
          CActionManager.instance.RemoveListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
          CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
          CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
          CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
          CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
          CActionManager.instance.RemoveListener("SC_INPUT_SPEED_INC", new Action<CActionManager.ActionType>(this.IncreaseSpeed), this.gameObject);
          CActionManager.instance.RemoveListener("SC_INPUT_SPEED_DEC", new Action<CActionManager.ActionType>(this.DecreaseSpeed), this.gameObject);
        }
        else
        {
          this.speed0Toggle.enabled = true;
          this.speed1Toggle.enabled = true;
          this.speed2Toggle.enabled = true;
          this.speed3Toggle.enabled = true;
          CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
          CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
          CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
          CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
          CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
          CActionManager.instance.AddListener("SC_INPUT_SPEED_INC", new Action<CActionManager.ActionType>(this.IncreaseSpeed), this.gameObject);
          CActionManager.instance.AddListener("SC_INPUT_SPEED_DEC", new Action<CActionManager.ActionType>(this.DecreaseSpeed), this.gameObject);
        }
      }
    }
    else
    {
      this.diseaseBoxButtonChoose.gameObject.SetActive(isActive);
      this.diseaseScreenButton.gameObject.SetActive(isActive);
      this.worldBoxButtonChoose.gameObject.SetActive(isActive);
      this.worldScreenButton.gameObject.SetActive(isActive);
      this.countryContextButton.gameObject.SetActive(isActive);
    }
    if (isActive)
    {
      CHUDScreen.instance = this;
      this.SetEnable(true);
      this.diseaseHUDSubView.SetActive(isActive);
      this.worldHUDSubView.SetActive(isActive);
      if (this.activePopup != null)
        this.ShowPopup(this.activePopup);
      this.HudInterfaceMode = EHudMode.Normal;
      this.dnaIncreaseLabel.gameObject.SetActive(false);
      this.dnaDecreaseLabel.gameObject.SetActive(false);
      if (this.isCureGameMode)
      {
        this.authorityIncreaseLabel.gameObject.SetActive(false);
        this.curePercIncreaseLabel.gameObject.SetActive(false);
        this.authorityDecreaseLabel.gameObject.SetActive(false);
        this.curePercDecreaseLabel.gameObject.SetActive(false);
      }
      this.actionsZombieAnimator.SetTrigger("Start");
      this.SetActiveSubScreen((IGameSubScreen) this.contextSubscreen);
      this.lastDna = CInterfaceManager.instance.localPlayerDisease.evoPoints;
      this.DisplayCurrentContext();
      this.Refresh();
      if ((double) CGameManager.localPlayerInfo.disease.cureCompletePerc >= 1.0)
        this.CureCompleteAnimation();
      else if (this.timesToPlay > 0)
        this.PlayCureThresholdAnimation(CGameManager.localPlayerInfo.disease.cureCompletePerc);
      if (!CGameManager.IsMultiplayerGame)
        this.SetButtonState(CGameManager.game.WantedSpeed);
      if ((UnityEngine.Object) CNetworkManager.network != (UnityEngine.Object) null && CGameManager.IsMultiplayerGame && CNetworkManager.network.NumberOfConnectedPlayers >= 1)
      {
        CUIManager.instance.ShowOverlay("Multiplayer_Data_Overlay");
        CUIManager.instance.ShowOverlay("Multiplayer_Events");
        this.multiplayerDataOverlay = (MultiplayerData_Overlay) CUIManager.instance.GetOverlay("Multiplayer_Data_Overlay");
      }
    }
    else
    {
      if (this.meHudMode != EHudMode.Disabled && this.meHudMode != EHudMode.Normal)
        CInterfaceManager.instance.TargetBubbleCancel();
      if (World.instance != null && World.instance.gameEnded && (UnityEngine.Object) CInterfaceManager.instance.resultOverlay != (UnityEngine.Object) null)
        CInterfaceManager.instance.resultOverlay.Continue();
      CUIManager.instance.HideOverlay("Multiplayer_Data_Overlay");
      CUIManager.instance.HideOverlay("Multiplayer_Events");
    }
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.spaceTime)
    {
      CGameManager.pauseHud = !isActive;
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.Tutorial)
      {
        if (!isActive)
        {
          this.tutorialPauseState = CGameManager.game.IsPaused;
          CGameManager.SetPaused(!isActive);
        }
        else
          CGameManager.SetPaused(this.tutorialPauseState);
      }
      else if (!CGameManager.IsMultiplayerGame)
        CGameManager.SetPaused(!isActive);
    }
    if (CGameManager.spaceTime)
      CGameManager.game.SetSpeed(CGameManager.spaceTimeRate > 0 ? CGameManager.spaceTimeRate : 1);
    this.network = CNetworkManager.network;
  }

  public override void Refresh()
  {
    base.Refresh();
    Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
    if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      disease.authority = TutorialSystem.IsModuleComplete("C44") ? 0.8f : 1f;
    this.SetDNAPoints(disease.evoPoints);
    this.SetCurePercent(disease);
    this.SetDay(disease.turnNumber);
    if (this.isCureGameMode)
      this.SetAuthorityValue(Mathf.RoundToInt(Mathf.Max(0.0f, disease.authority * 100f)), Mathf.RoundToInt(disease.authorityWeekly));
    this.MultiPlayerDataUpdate();
    if (CGameManager.IsVersusMPGame)
    {
      this.geneticDominanceBar.Refresh();
    }
    else
    {
      if (!CGameManager.IsCoopMPGame)
        return;
      this.coopBar.Refresh();
      this.coopLabBar.Refresh();
    }
  }

  public void MPGameEnded(MultiplayerResultOverlay.MultiplayerResult result)
  {
    this.coopLabBar.gameObject.SetActive(false);
    this.mpEndGameButton.gameObject.SetActive(true);
    this.mpEndGameTitle.gameObject.SetActive(true);
    switch (result)
    {
      case MultiplayerResultOverlay.MultiplayerResult.Win:
        this.mpEndGameTitle.text = CLocalisationManager.GetText("MP_Endgame_Win");
        break;
      case MultiplayerResultOverlay.MultiplayerResult.Lose:
        this.mpEndGameTitle.text = CLocalisationManager.GetText("MP_Endgame_Loss");
        break;
      case MultiplayerResultOverlay.MultiplayerResult.Draw:
        this.mpEndGameTitle.text = CLocalisationManager.GetText("MP_Endgame_Draw");
        break;
    }
  }

  private void MultiPlayerDataUpdate()
  {
    if (!((UnityEngine.Object) this.multiplayerDataOverlay != (UnityEngine.Object) null) || !((UnityEngine.Object) CNetworkManager.network != (UnityEngine.Object) null) || !CGameManager.IsMultiplayerGame || CNetworkManager.network.NumberOfConnectedPlayers < 1)
      return;
    CountryView selectedCountryView = CInterfaceManager.instance.SelectedCountryView;
    StringBuilder stringBuilder = new StringBuilder();
    if ((UnityEngine.Object) selectedCountryView != (UnityEngine.Object) null)
    {
      Country country = selectedCountryView.GetCountry();
      stringBuilder.AppendLine("Country: " + country.name);
      stringBuilder.AppendLine("Original Population: " + (object) country.originalPopulation);
    }
    else
    {
      stringBuilder.AppendLine("World");
      stringBuilder.AppendLine("");
    }
    stringBuilder.AppendLine("Healthy: " + (object) MPPlayerData.GetHealthy() + " [" + MPPlayerData.GetHealthyPercentage().ToString("f2") + "%]");
    stringBuilder.AppendLine("Dead: " + (object) MPPlayerData.GetDeadTotal() + " [" + MPPlayerData.GetDeadTotalPercentage().ToString("f2") + "%]");
    stringBuilder.AppendLine("---");
    foreach (Disease disease in World.instance.diseases)
    {
      stringBuilder.AppendLine(MPPlayerData.GetPlayerDiseaseName(disease) + "'s total infected: " + MPPlayerData.GetPlayerTotalInfected(disease).ToString("N0") + " [" + MPPlayerData.GetPlayerTotalInfectedPercentage(disease).ToString("f2") + "%]");
      stringBuilder.AppendLine(MPPlayerData.GetPlayerDiseaseName(disease) + "'s single infected: " + MPPlayerData.GetPlayerSingleInfected(disease).ToString("N0") + " [" + MPPlayerData.GetPlayerSingleInfectedPercentage(disease).ToString("f2") + "%]");
      stringBuilder.AppendLine(MPPlayerData.GetPlayerDiseaseName(disease) + "'s dead: " + (object) MPPlayerData.GetPlayerDead(disease) + " [" + MPPlayerData.GetPlayerDeadPercentage(disease).ToString("f2") + "%]");
      stringBuilder.AppendLine("---");
    }
    stringBuilder.AppendLine("Joint Infected" + MPPlayerData.GetJointInfected().ToString("N0") + " [" + MPPlayerData.GetJointInfectedPercentage().ToString("f2") + "%]");
    this.multiplayerDataOverlay.playerLabels[0].SetText(stringBuilder.ToString());
  }

  public override void OnCountryHover(CountryView cv, Vector3 hit)
  {
    switch (this.meHudMode)
    {
      case EHudMode.Neurax:
      case EHudMode.SendHorde:
      case EHudMode.SendApeHorde:
      case EHudMode.SendApeColony:
      case EHudMode.SendUnscheduledFlight:
      case EHudMode.NukeStrike:
      case EHudMode.SendVampire:
        if (this.meHudMode == EHudMode.SendVampire && (double) CInterfaceManager.instance.localPlayerDisease.castleReturnSpeed > 0.0)
        {
          for (int index = 0; index < World.instance.countries.Count; ++index)
          {
            Country country = World.instance.countries[index];
            if (country.GetLocalDisease(CInterfaceManager.instance.localPlayerDisease).castleState == ECastleState.CASTLE_ALIVE)
              CInterfaceManager.instance.GetCountryView(country.id).SetSelected(CountryView.EOverlayState.PORTAL_TARGET);
          }
        }
        if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
        {
          if (CInterfaceManager.instance.IsValidHoverState(this.meHudMode, CInterfaceManager.instance.HoverCountry))
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.VALID);
          else
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.OFF);
        }
        CInterfaceManager.instance.HoverCountry = cv;
        ECursorMode cursorMode1;
        if (CUIManager.instance.IsHovering())
        {
          cursorMode1 = ECursorMode.NORMAL;
          this.contextSubscreen.SetContextWorld();
          CInterfaceManager.instance.StopSpline();
        }
        else
        {
          CountryView targetBubbleStart = CInterfaceManager.instance.mpTargetBubbleStart;
          CountryView hoverCountry = this.GetHoverCountry(this.meHudMode, targetBubbleStart, CInterfaceManager.instance.HoverCountry, CInterfaceManager.instance.splineStartPosition, hit, this.meHudMode == EHudMode.SendApeColony);
          bool flag;
          if (flag = CInterfaceManager.instance.IsValidHoverState(this.meHudMode, targetBubbleStart, hoverCountry))
          {
            cursorMode1 = Input.GetMouseButton(0) || CSteamControllerManager.instance.GetAction(ESteamControllerDigitalAction.main_select) ? ECursorMode.CLICK : ECursorMode.HOVER;
            if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry == (UnityEngine.Object) hoverCountry)
            {
              Color color = CGameManager.game.GetColourSet(CGameManager.localPlayerInfo.disease.id).lineColour;
              if (CGameManager.IsCoopMPGame)
              {
                Country country = targetBubbleStart.GetCountry();
                LocalDisease localDisease1 = country.GetLocalDisease(CInterfaceManager.instance.localPlayerDisease);
                LocalDisease localDisease2 = localDisease1 == country.localDiseases[0] ? country.localDiseases[1] : country.localDiseases[0];
                color = localDisease1.allInfected < localDisease2.allInfected ? CGameManager.game.GetColourSet(localDisease2.disease.id).lineColour : CGameManager.game.GetColourSet(localDisease1.disease.id).lineColour;
              }
              CInterfaceManager.instance.SetSplineRenderer(hit, color, this.meHudMode == EHudMode.Neurax);
            }
            else
              CInterfaceManager.instance.SetSplineRenderer(hit, CInterfaceManager.instance.colorLineDisabled, this.meHudMode == EHudMode.Neurax);
          }
          else
          {
            cursorMode1 = ECursorMode.NORMAL;
            CInterfaceManager.instance.SetSplineRenderer(hit, CInterfaceManager.instance.colorLineDisabled, this.meHudMode == EHudMode.Neurax);
          }
          CInterfaceManager.instance.HoverCountry = hoverCountry;
          if ((UnityEngine.Object) hoverCountry != (UnityEngine.Object) null)
          {
            this.contextSubscreen.SetContextCountry(hoverCountry.GetCountry());
            hoverCountry.SetSelected(flag ? CountryView.EOverlayState.SELECTED : CountryView.EOverlayState.OFF);
          }
          else
            this.contextSubscreen.SetContextWorld();
        }
        if (cursorMode1 == CInterfaceManager.instance.cursorMode)
          break;
        CInterfaceManager.instance.SetCursorMode(cursorMode1);
        break;
      case EHudMode.Reanimate:
      case EHudMode.SelectHorde:
      case EHudMode.ApeRampage:
      case EHudMode.SelectApeHorde:
      case EHudMode.ApeCreateColony:
      case EHudMode.SelectUnscheduledFlight:
      case EHudMode.ImmuneShock:
      case EHudMode.BenignMimic:
      case EHudMode.InfectBoost:
      case EHudMode.LethalBoost:
      case EHudMode.BloodRage:
      case EHudMode.SelectVampire:
      case EHudMode.CreateCastle:
        if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
        {
          if (CInterfaceManager.instance.IsValidHoverState(this.meHudMode, CInterfaceManager.instance.HoverCountry))
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.VALID);
          else
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.OFF);
        }
        CInterfaceManager.instance.HoverCountry = cv;
        ECursorMode cursorMode2;
        if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry == (UnityEngine.Object) null)
        {
          cursorMode2 = ECursorMode.NORMAL;
          this.contextSubscreen.SetContextWorld();
        }
        else
        {
          bool flag;
          if (flag = CInterfaceManager.instance.IsValidHoverState(this.meHudMode, CInterfaceManager.instance.HoverCountry))
          {
            cursorMode2 = Input.GetMouseButton(0) || CSteamControllerManager.instance.GetAction(ESteamControllerDigitalAction.main_select) ? ECursorMode.CLICK : ECursorMode.HOVER;
            flag = true;
          }
          else
            cursorMode2 = ECursorMode.NORMAL;
          CInterfaceManager.instance.HoverCountry.SetSelected(flag ? CountryView.EOverlayState.SELECTED : CountryView.EOverlayState.OFF);
          this.contextSubscreen.SetContextCountry(cv.GetCountry());
        }
        if (cursorMode2 == CInterfaceManager.instance.cursorMode)
          break;
        CInterfaceManager.instance.SetCursorMode(cursorMode2);
        break;
      case EHudMode.PlaceGem:
      case EHudMode.MoveGem:
        if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
        {
          if (CInterfaceManager.instance.IsValidHoverState(this.meHudMode, CInterfaceManager.instance.HoverCountry))
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.VALID);
          else
            CInterfaceManager.instance.HoverCountry.SetSelected(CountryView.EOverlayState.OFF);
        }
        CInterfaceManager.instance.HoverCountry = cv;
        ECursorMode cursorMode3 = ECursorMode.NORMAL;
        if (CUIManager.instance.IsHovering())
        {
          cursorMode3 = ECursorMode.NORMAL;
          this.contextSubscreen.SetContextWorld();
        }
        else
        {
          CountryView targetBubbleStart = CInterfaceManager.instance.mpTargetBubbleStart;
          CountryView hoverCountry = this.GetHoverCountry(this.meHudMode, targetBubbleStart, CInterfaceManager.instance.HoverCountry, CInterfaceManager.instance.splineStartPosition, hit, this.meHudMode == EHudMode.SendApeColony);
          bool flag;
          if (this.meHudMode == EHudMode.MoveGem)
          {
            Vector3 vector3 = Vector3.zero;
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, 1))
              vector3 = hitInfo.point;
            if (flag = CInterfaceManager.instance.IsValidHoverState(this.meHudMode, targetBubbleStart, hoverCountry))
            {
              cursorMode3 = Input.GetMouseButton(0) || CSteamControllerManager.instance.GetAction(ESteamControllerDigitalAction.main_select) ? ECursorMode.CLICK : ECursorMode.HOVER;
              CInterfaceManager.instance.movingGem.transform.position = vector3;
            }
            else
            {
              cursorMode3 = ECursorMode.NORMAL;
              CInterfaceManager.instance.movingGem.transform.position = vector3;
            }
          }
          else if (flag = CInterfaceManager.instance.IsValidHoverState(this.meHudMode, targetBubbleStart, hoverCountry))
            cursorMode3 = Input.GetMouseButton(0) || CSteamControllerManager.instance.GetAction(ESteamControllerDigitalAction.main_select) ? ECursorMode.CLICK : ECursorMode.HOVER;
          CInterfaceManager.instance.HoverCountry = hoverCountry;
          if ((UnityEngine.Object) hoverCountry != (UnityEngine.Object) null)
          {
            this.contextSubscreen.SetContextCountry(hoverCountry.GetCountry());
            hoverCountry.SetSelected(flag ? CountryView.EOverlayState.SELECTED : CountryView.EOverlayState.OFF);
          }
          else
            this.contextSubscreen.SetContextWorld();
        }
        if (cursorMode3 == CInterfaceManager.instance.cursorMode)
          break;
        CInterfaceManager.instance.SetCursorMode(cursorMode3);
        break;
      case EHudMode.SendInvestigationTeam:
      case EHudMode.RaisePriority:
      case EHudMode.EconomicSupport:
        CInterfaceManager.instance.HoverCountry = cv;
        if ((UnityEngine.Object) CInterfaceManager.instance.HoverCountry != (UnityEngine.Object) null)
        {
          this.contextSubscreen.SetContextCountry(cv.GetCountry());
          break;
        }
        this.contextSubscreen.SetContextWorld();
        break;
    }
  }

  public override void OnCountryClick(
    CountryView clickedCountryView,
    Vector3 hit,
    bool downCountrySameAsUp)
  {
    if (Camera_Zoom.instance.IsScrolling && CUIManager.instance.IsHovering() || CGameManager.game.CurrentGameState != IGame.GameState.EndGame && CGameManager.game.CurrentGameState != IGame.GameState.InProgress || CGameManager.IsTutorialGame && TutorialSystem.IsModuleSectionActive("Evolving a Tech") || TutorialSystem.IsModuleSectionActive("Controlling Time/Pausing") || TutorialSystem.IsModuleSectionActive("Infecting New Country") || TutorialSystem.IsModuleSectionActive("Country Info") || TutorialSystem.IsModuleSectionActive("ZoomIn/Out") || TutorialSystem.IsModuleSectionActive("News Screen") || TutorialSystem.IsModuleSectionActive("World/Cure") || TutorialSystem.IsModuleSectionActive("Free Play"))
      return;
    if (this.meHudMode != EHudMode.SendVampire && this.meHudMode != EHudMode.EconomicSupport && this.meHudMode != EHudMode.RaisePriority)
    {
      WorldMapController.instance.SetSelectedCountry(clickedCountryView);
      CInterfaceManager.instance.UpdateCountryState();
    }
    if (this.HudInterfaceMode != EHudMode.Normal && CGameManager.game.CurrentGameState != IGame.GameState.InProgress)
      this.HudInterfaceMode = EHudMode.Normal;
    if (this.meHudMode != EHudMode.SendHorde && ((UnityEngine.Object) clickedCountryView == (UnityEngine.Object) null || !downCountrySameAsUp))
      return;
    switch (this.meHudMode)
    {
      case EHudMode.Normal:
        bool flag = false;
        if ((UnityEngine.Object) clickedCountryView == (UnityEngine.Object) this.lastCountryClicked)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastCountryClickTime < 0.30000001192092896)
          {
            flag = true;
            this.lastCountryClickTime = -1f;
            this.lastCountryClicked = (CountryView) null;
          }
          else
          {
            this.lastCountryClickTime = Time.realtimeSinceStartup;
            this.lastCountryClicked = clickedCountryView;
          }
        }
        else
        {
          this.lastCountryClickTime = Time.realtimeSinceStartup;
          this.lastCountryClicked = clickedCountryView;
        }
        if (!flag || CGameManager.IsTutorialGame && !TutorialSystem.IsModuleComplete("6A") || TutorialSystem.IsModuleSectionActive("Controlling Time/Pausing") || TutorialSystem.IsModuleSectionActive("Evolving a Tech") || TutorialSystem.IsModuleSectionActive("Infecting New Country") || TutorialSystem.IsModuleSectionActive("Free Play") || TutorialSystem.IsModuleSectionActive("ZoomIn/Out") || TutorialSystem.IsModuleSectionActive("News Screen") || TutorialSystem.IsModuleSectionActive("World/Cure") || TutorialSystem.IsModuleSectionActive("Country Info"))
          break;
        CUIManager.instance.OpenCountryScreen();
        break;
      case EHudMode.Neurax:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !((UnityEngine.Object) CInterfaceManager.instance.mpTargetBubbleStart != (UnityEngine.Object) clickedCountryView))
          break;
        Country country1 = CInterfaceManager.instance.mpTargetBubbleStart.GetCountry();
        CInterfaceManager.instance.TargetBubbleSuccess(clickedCountryView, hit);
        CGameManager.game.CreateNeuraxVehicle(country1, clickedCountryView.GetCountry(), CGameManager.localPlayerInfo.disease, CInterfaceManager.instance.splineStartPosition, CInterfaceManager.instance.splineEndPosition);
        break;
      case EHudMode.Reanimate:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        Disease disease1 = CGameManager.localPlayerInfo.disease;
        long num = CGameManager.game.Reanimate(clickedCountryView.GetCountry(), disease1);
        if (num <= 0L)
          break;
        this.SetDNAPoints(disease1.evoPoints);
        CInterfaceManager.instance.SetNecroaDisplay(num, hit);
        clickedCountryView.ReanimateEffect(num);
        this.abilitySubscreen.UseAbility(EAbilityType.reanimate);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.SelectHorde:
        if (clickedCountryView.GetCountry().totalZombie <= 0L)
          break;
        CSoundManager.instance.PlaySFX("hordein");
        CInterfaceManager.instance.splineStartPosition = hit;
        CInterfaceManager.instance.mpTargetBubbleStart = clickedCountryView;
        this.HudInterfaceMode = EHudMode.SendHorde;
        break;
      case EHudMode.SendHorde:
        if (!((UnityEngine.Object) CInterfaceManager.instance.mpTargetBubbleStart != (UnityEngine.Object) clickedCountryView))
          break;
        CountryView targetBubbleStart1 = CInterfaceManager.instance.mpTargetBubbleStart;
        CountryView hoverCountry1 = this.GetHoverCountry(this.meHudMode, targetBubbleStart1, clickedCountryView, CInterfaceManager.instance.splineStartPosition, hit, false);
        Disease disease2 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost1 = CGameManager.GetActiveAbilityCost(EAbilityType.zombie_horde, disease2);
        if (!((UnityEngine.Object) hoverCountry1 != (UnityEngine.Object) null) || disease2.evoPoints < activeAbilityCost1)
          break;
        World.instance.AddAchievement(EAchievement.A_FlashMob);
        CInterfaceManager.instance.TargetBubbleSuccess(hoverCountry1, hit);
        CGameManager.game.CreateZombieVehicle(targetBubbleStart1.GetCountry(), hoverCountry1.GetCountry(), disease2, CInterfaceManager.instance.splineStartPosition, CInterfaceManager.instance.splineEndPosition);
        this.SetDNAPoints(disease2.evoPoints);
        CSoundManager.instance.PlaySFX("horderelease");
        this.abilitySubscreen.UseAbility(EAbilityType.zombie_horde);
        break;
      case EHudMode.ApeRampage:
        Disease disease3 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost2 = CGameManager.GetActiveAbilityCost(EAbilityType.rampage, disease3);
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || disease3.evoPoints < activeAbilityCost2 || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        CGameManager.game.UseApeRampage(clickedCountryView, CGameManager.localPlayerInfo.disease);
        this.SetDNAPoints(disease3.evoPoints);
        CSoundManager.instance.PlaySFX("ape_rampage");
        this.abilitySubscreen.UseAbility(EAbilityType.rampage);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.SelectApeHorde:
        Country country2 = clickedCountryView.GetCountry();
        if (CGameManager.localPlayerInfo.disease.GetLocalDisease(country2).apeInfectedPopulation <= 0L)
          break;
        CGameManager.game.ClearExistingHorde(country2);
        CSoundManager.instance.PlaySFX("ape_move_bubble");
        if (country2.apeColonyPosition.HasValue)
        {
          CInterfaceManager.instance.splineStartPosition = country2.apeColonyPosition.Value;
          this.HudInterfaceMode = EHudMode.SendApeColony;
        }
        else
        {
          CInterfaceManager.instance.splineStartPosition = hit;
          this.HudInterfaceMode = EHudMode.SendApeHorde;
        }
        CInterfaceManager.instance.mpTargetBubbleStart = clickedCountryView;
        break;
      case EHudMode.SendApeHorde:
      case EHudMode.SendApeColony:
        CountryView targetBubbleStart2 = CInterfaceManager.instance.mpTargetBubbleStart;
        CountryView hoverCountry2 = this.GetHoverCountry(this.meHudMode, targetBubbleStart2, clickedCountryView, CInterfaceManager.instance.splineStartPosition, hit, true);
        Disease disease4 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost3 = CGameManager.GetActiveAbilityCost(EAbilityType.move, disease4);
        if (!((UnityEngine.Object) hoverCountry2 != (UnityEngine.Object) null) || disease4.evoPoints < activeAbilityCost3)
          break;
        this.SetDNAPoints(disease4.evoPoints);
        CGameManager.game.CreateApeVehicle(targetBubbleStart2.GetCountry(), hoverCountry2.GetCountry(), disease4, CInterfaceManager.instance.splineStartPosition, hit);
        CInterfaceManager.instance.TargetBubbleSuccess(hoverCountry2, hit);
        CSoundManager.instance.PlaySFX("ape_move_release");
        this.abilitySubscreen.UseAbility(EAbilityType.move);
        break;
      case EHudMode.ApeCreateColony:
        Disease disease5 = CGameManager.localPlayerInfo.disease;
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        int activeAbilityCost4 = CGameManager.GetActiveAbilityCost(EAbilityType.create_colony, disease5);
        if (disease5.evoPoints < activeAbilityCost4)
          break;
        CGameManager.game.CreateApeColony(clickedCountryView.GetCountry(), disease5, hit);
        this.SetDNAPoints(disease5.evoPoints);
        CSoundManager.instance.PlaySFX("ape_colony_created");
        this.abilitySubscreen.UseAbility(EAbilityType.create_colony);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.PlaceGem:
        Disease disease6 = CGameManager.localPlayerInfo.disease;
        int gemCost = this.gemSubscreen.GetGemCost();
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || disease6.evoPoints < gemCost)
          break;
        this.gemSubscreen.UseAbility(hit);
        this.SetDNAPoints(disease6.evoPoints);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.MoveGem:
        CInterfaceManager.instance.DoGemPlace(clickedCountryView, hit);
        WorldMapController.instance.SetSelectedCountry();
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.SelectUnscheduledFlight:
        Country country3 = clickedCountryView.GetCountry();
        if (country3.GetLocalDisease(CGameManager.localPlayerInfo.disease).allInfected <= 1000L && (!CGameManager.IsCoopMPGame || country3.localDiseases.Where<LocalDisease>((Func<LocalDisease, bool>) (l_d => l_d.allInfected > 1000L)).Count<LocalDisease>() <= 0))
          break;
        CSoundManager.instance.PlaySFX("neuraxin");
        CInterfaceManager.instance.splineStartPosition = hit;
        CInterfaceManager.instance.mpTargetBubbleStart = clickedCountryView;
        this.HudInterfaceMode = EHudMode.SendUnscheduledFlight;
        break;
      case EHudMode.SendUnscheduledFlight:
        if (!((UnityEngine.Object) CInterfaceManager.instance.mpTargetBubbleStart != (UnityEngine.Object) clickedCountryView))
          break;
        CountryView targetBubbleStart3 = CInterfaceManager.instance.mpTargetBubbleStart;
        CountryView hoverCountry3 = this.GetHoverCountry(this.meHudMode, targetBubbleStart3, clickedCountryView, CInterfaceManager.instance.splineStartPosition, hit, false);
        Disease disease7 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost5 = CGameManager.GetActiveAbilityCost(EAbilityType.unscheduled_flight, disease7);
        if (!((UnityEngine.Object) hoverCountry3 != (UnityEngine.Object) null) || disease7.evoPoints < activeAbilityCost5)
          break;
        CInterfaceManager.instance.TargetBubbleSuccess(hoverCountry3, hit);
        CGameManager.game.CreateUnscheduledFlight(targetBubbleStart3.GetCountry(), hoverCountry3.GetCountry(), disease7, CInterfaceManager.instance.splineStartPosition, CInterfaceManager.instance.splineEndPosition);
        CSoundManager.instance.PlaySFX("neuraxrelease");
        this.SetDNAPoints(disease7.evoPoints);
        this.abilitySubscreen.UseAbility(EAbilityType.unscheduled_flight);
        break;
      case EHudMode.ImmuneShock:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        Disease disease8 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost6 = CGameManager.GetActiveAbilityCost(EAbilityType.immune_shock, disease8);
        if (disease8.evoPoints < activeAbilityCost6)
          break;
        CGameManager.game.ImmuneShock(clickedCountryView.GetCountry().id, hit, CGameManager.localPlayerInfo);
        this.abilitySubscreen.UseAbility(EAbilityType.immune_shock);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.BenignMimic:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        Disease disease9 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost7 = CGameManager.GetActiveAbilityCost(EAbilityType.benign_mimic, disease9);
        if (disease9.evoPoints < activeAbilityCost7)
          break;
        CGameManager.game.BenignMimic(clickedCountryView.GetCountry().id, hit, CGameManager.localPlayerInfo);
        this.abilitySubscreen.UseAbility(EAbilityType.benign_mimic);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.InfectBoost:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        Disease disease10 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost8 = CGameManager.GetActiveAbilityCost(EAbilityType.infect_boost, disease10);
        if (disease10.evoPoints < activeAbilityCost8)
          break;
        CGameManager.game.InfectBoost(clickedCountryView.GetCountry().id, hit, CGameManager.localPlayerInfo);
        this.abilitySubscreen.UseAbility(EAbilityType.infect_boost);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.LethalBoost:
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        Disease disease11 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost9 = CGameManager.GetActiveAbilityCost(EAbilityType.lethal_boost, disease11);
        if (disease11.evoPoints < activeAbilityCost9)
          break;
        CGameManager.game.LethalBoost(clickedCountryView.GetCountry().id, hit, CGameManager.localPlayerInfo);
        this.abilitySubscreen.UseAbility(EAbilityType.lethal_boost);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.NukeStrike:
        CInterfaceManager instance1 = CInterfaceManager.instance;
        CountryView targetBubbleStart4 = CInterfaceManager.instance.mpTargetBubbleStart;
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) targetBubbleStart4))
          break;
        CountryView hoverCountry4 = this.GetHoverCountry(this.meHudMode, targetBubbleStart4, clickedCountryView, instance1.splineStartPosition, hit, false);
        if ((UnityEngine.Object) instance1.nukeBubble != (UnityEngine.Object) null)
        {
          World.instance.bonusIcons.Remove(instance1.nukeBubble.mpBonus);
          instance1.nukeBubble.mbBlocked = true;
          instance1.nukeBubble.DoBubblePop(CNetworkManager.network.LocalPlayerInfo.disease);
        }
        CGameManager.game.CreateNukeLaunch(targetBubbleStart4.GetCountry(), hoverCountry4.GetCountry(), CGameManager.localPlayerInfo.disease, instance1.splineStartPosition, hit);
        CSoundManager.instance.PlaySFX("nuke_launch");
        this.abilitySubscreen.UseAbility(EAbilityType.nuke);
        break;
      case EHudMode.BloodRage:
        Disease disease12 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost10 = CGameManager.GetActiveAbilityCost(EAbilityType.bloodrage, disease12);
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || disease12.evoPoints < activeAbilityCost10 || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        CGameManager.game.UseBloodRage(clickedCountryView, CGameManager.localPlayerInfo.disease);
        this.SetDNAPoints(disease12.evoPoints);
        if (clickedCountryView.GetCountry().GetLocalDisease(disease12).consumeFlag > 0)
          CSoundManager.instance.PlaySFX("bloodrage_start");
        this.abilitySubscreen.UseAbility(EAbilityType.bloodrage);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.SelectVampire:
        Country country4 = clickedCountryView.GetCountry();
        Disease disease13 = CGameManager.localPlayerInfo.disease;
        if (country4.GetLocalDisease(disease13).zombie <= 0L)
          break;
        Vampire closestVampire = disease13.GetClosestVampire(country4, hit);
        if (closestVampire == null)
          break;
        CSoundManager.instance.PlaySFX("vampire_move_bubble");
        CInterfaceManager.instance.splineStartPosition = closestVampire.currentPosition.Value;
        CInterfaceManager.instance.mpTargetBubbleStart = clickedCountryView;
        this.HudInterfaceMode = EHudMode.SendVampire;
        break;
      case EHudMode.SendVampire:
        CountryView targetBubbleStart5 = CInterfaceManager.instance.mpTargetBubbleStart;
        CountryView hoverCountry5 = CInterfaceManager.instance.HoverCountry;
        Disease disease14 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost11 = CGameManager.GetActiveAbilityCost(EAbilityType.vampiretravel, disease14);
        if (!((UnityEngine.Object) hoverCountry5 != (UnityEngine.Object) null) || disease14.evoPoints < activeAbilityCost11)
          break;
        WorldMapController.instance.SetSelectedCountry(hoverCountry5);
        CInterfaceManager.instance.UpdateCountryState();
        CInterfaceManager.instance.TargetBubbleSuccess(hoverCountry5, hit);
        CGameManager.game.CreateVampireVehicle(targetBubbleStart5.GetCountry(), hoverCountry5.GetCountry(), disease14, CInterfaceManager.instance.splineStartPosition, CInterfaceManager.instance.splineEndPosition);
        this.SetDNAPoints(disease14.evoPoints);
        CSoundManager.instance.PlaySFX("vampire_move_release");
        this.abilitySubscreen.UseAbility(EAbilityType.vampiretravel);
        CInterfaceManager.instance.UpdateVampires();
        CGameManager.game.ForceSpawnVehicles();
        break;
      case EHudMode.CreateCastle:
        Disease disease15 = CGameManager.localPlayerInfo.disease;
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        int activeAbilityCost12 = CGameManager.GetActiveAbilityCost(EAbilityType.castle, disease15);
        if (disease15.evoPoints < activeAbilityCost12)
          break;
        CGameManager.game.CreateCastle(clickedCountryView.GetCountry(), disease15, hit);
        this.SetDNAPoints(disease15.evoPoints);
        CSoundManager.instance.PlaySFX("lair_created");
        this.abilitySubscreen.UseAbility(EAbilityType.castle);
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.SendInvestigationTeam:
        Disease disease16 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost13 = CGameManager.GetActiveAbilityCost(EAbilityType.investigation_team, disease16);
        CountryView cv = clickedCountryView;
        if (!((UnityEngine.Object) cv != (UnityEngine.Object) null) || disease16.evoPoints < activeAbilityCost13 || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView))
          break;
        if (CGameManager.gameType == IGame.GameType.CureTutorial && TutorialSystem.IsModuleActive("C11"))
        {
          if (cv.countryID != "brazil")
            break;
          PIETutorialSystem instance2 = (PIETutorialSystem) TutorialSystem.Instance;
          instance2.StartCoroutine(instance2.UpdateTutorial());
          CGameManager.SetPaused(false, true);
          TutorialSystem.MarkModuleComplete("C11");
        }
        CGameManager.game.CreateInvestigationTeamFlight(cv.GetCountry(), disease16, hit);
        CSoundManager.instance.PlaySFX("aasuccess");
        WorldMapController.instance.SetSelectedCountry(cv);
        CInterfaceManager.instance.UpdateCountryState();
        this.SetDNAPoints(disease16.evoPoints);
        this.abilitySubscreen.UseAbility(EAbilityType.investigation_team);
        CInterfaceManager.instance.UpdateVampires();
        CGameManager.game.ForceSpawnVehicles();
        this.HudInterfaceMode = EHudMode.Normal;
        break;
      case EHudMode.RaisePriority:
        Disease disease17 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost14 = CGameManager.GetActiveAbilityCost(EAbilityType.raise_priority, disease17);
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || disease17.evoPoints < activeAbilityCost14 || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView) || !CGameManager.game.UseRaisePriority(clickedCountryView, CGameManager.localPlayerInfo.disease))
          break;
        CSoundManager.instance.PlaySFX("aasuccess");
        this.SetDNAPoints(disease17.evoPoints);
        this.abilitySubscreen.UseAbility(EAbilityType.raise_priority);
        this.HudInterfaceMode = EHudMode.Normal;
        WorldMapController.instance.SetSelectedCountry(clickedCountryView);
        CInterfaceManager.instance.UpdateCountryState();
        LocalDisease localDisease1 = clickedCountryView.GetCountry().GetLocalDisease(CGameManager.localPlayerInfo.disease);
        clickedCountryView.SetCureIconState(localDisease1.supportActive ? (float) localDisease1.supportTimer / (float) disease17.supportTimerMAX : 0.0f, (float) localDisease1.tempLockdownTimer / (float) disease17.lockdownTimerMAX, localDisease1.unrestActive, localDisease1.fireComplianceIcon, localDisease1.complianceIconScale);
        break;
      case EHudMode.EconomicSupport:
        Disease disease18 = CGameManager.localPlayerInfo.disease;
        int activeAbilityCost15 = CGameManager.GetActiveAbilityCost(EAbilityType.economic_support, disease18);
        if (!((UnityEngine.Object) clickedCountryView != (UnityEngine.Object) null) || disease18.evoPoints < activeAbilityCost15 || !CInterfaceManager.instance.IsValidHoverState(this.meHudMode, clickedCountryView) || !CGameManager.game.UseEconomicSupport(clickedCountryView, CGameManager.localPlayerInfo.disease))
          break;
        WorldMapController.instance.SetSelectedCountry(clickedCountryView);
        CInterfaceManager.instance.UpdateCountryState();
        this.SetDNAPoints(disease18.evoPoints);
        CSoundManager.instance.PlaySFX("aid_release");
        this.abilitySubscreen.UseAbility(EAbilityType.economic_support);
        this.HudInterfaceMode = EHudMode.Normal;
        LocalDisease localDisease2 = clickedCountryView.GetCountry().GetLocalDisease(CGameManager.localPlayerInfo.disease);
        clickedCountryView.SetCureIconState(localDisease2.supportActive ? (float) localDisease2.supportTimer / (float) disease18.supportTimerMAX : 0.0f, (float) localDisease2.tempLockdownTimer / (float) disease18.lockdownTimerMAX, localDisease2.unrestActive, localDisease2.fireComplianceIcon, localDisease2.complianceIconScale);
        break;
    }
  }

  private CountryView GetHoverCountry(
    EHudMode hudMode,
    CountryView cvFrom,
    CountryView cvTo,
    Vector3 mapStart,
    Vector3 mapHit,
    bool allowSameCountry)
  {
    if (hudMode != EHudMode.SendHorde)
      return cvTo;
    Vector3 b = mapStart;
    Vector3 vector3 = mapHit;
    b.z -= 0.2f;
    vector3.z -= 0.7f;
    RaycastHit[] raycastHitArray = Physics.CapsuleCastAll(b - Vector3.forward * 2f, vector3 - Vector3.forward * 2f, 0.01f, Vector3.forward, 5f, (int) CInterfaceManager.instance.countryLayer);
    this.start = b;
    this.end = vector3;
    float num1 = (vector3 - b).magnitude;
    CountryView hoverCountry = CInterfaceManager.instance.GetHoverCountry(false);
    int num2 = 0;
    for (int index = 0; index < raycastHitArray.Length; ++index)
    {
      CountryView component = raycastHitArray[index].collider.GetComponent<CountryView>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && (!((UnityEngine.Object) component == (UnityEngine.Object) cvFrom) || allowSameCountry))
      {
        float num3 = Vector3.Distance(raycastHitArray[index].point, b);
        ++num2;
        if ((double) num1 < 0.0 || (double) num3 < (double) num1)
        {
          num1 = num3;
          hoverCountry = component;
        }
      }
    }
    return hoverCountry;
  }

  private void OnDrawGizmos()
  {
    Vector3 vector3 = this.end - this.start;
    Gizmos.color = Color.red;
    for (int index = 0; index < 10; ++index)
    {
      Gizmos.DrawWireSphere(this.start + Vector3.forward + (float) index * vector3 / 10f, 0.05f);
      Gizmos.DrawWireSphere(this.start - Vector3.forward + (float) index * vector3 / 10f, 0.05f);
    }
    Gizmos.color = Color.green;
    Gizmos.DrawLine(this.start, this.end);
  }

  public void DisplayCurrentContext()
  {
    if ((UnityEngine.Object) CInterfaceManager.instance.SelectedCountryView != (UnityEngine.Object) null)
      this.contextSubscreen.SetContextCountry(CInterfaceManager.instance.SelectedCountryView.GetCountry());
    else
      this.contextSubscreen.SetContextWorld();
  }

  public void SetContextType()
  {
    if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.NECROA)
      this.contextSubscreen = (CHUDContextSubScreen) this.necroaSubscreen;
    else if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      this.contextSubscreen = (CHUDContextSubScreen) this.simianSubScreen;
    else if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      this.contextSubscreen = this.vampireSubscreen;
    else if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.CURE)
      this.contextSubscreen = (CHUDContextSubScreen) this.cureSubscreen;
    else
      this.contextSubscreen = this.baseSubscreen;
  }

  private void Update()
  {
    CSteamControllerManager instance = CSteamControllerManager.instance;
    if ((Input.GetMouseButtonDown(1) || instance.GetActionDown(ESteamControllerDigitalAction.main_back_deselect)) && !TutorialSystem.IsModuleActive())
      CInterfaceManager.instance.TargetBubbleCancel();
    if (Input.GetKeyDown(KeyCode.LeftControl))
      this.scrollSpeed = (float) CGameManager.game.WantedSpeed;
    else if (Input.GetKey(KeyCode.LeftControl))
    {
      this.scrollSpeed = Mathf.Clamp(this.scrollSpeed + Input.GetAxis("Mouse ScrollWheel") * 9f, 0.0f, 3f);
      int scrollSpeed = (int) this.scrollSpeed;
      if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.IsMultiplayerGame && CGameManager.game.WantedSpeed != scrollSpeed)
        this.SetSpeed(scrollSpeed);
    }
    if (this.showDiseaseScreenButton)
    {
      List<string> names;
      if (instance.controllerActive && instance.GetGlyphNames(ESteamControllerDigitalAction.main_open_disease, out names) > 0)
      {
        this.diseaseScreenControllerButtonGlyph.spriteName = names[0];
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.diseaseScreenControllerButton.SetActive(true);
      }
      else
      {
        this.diseaseScreenControllerButton.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(true);
      }
    }
    if (!this.showWorldScreenButton)
      return;
    List<string> names1;
    if (instance.controllerActive && instance.GetGlyphNames(ESteamControllerDigitalAction.main_open_world, out names1) > 0)
    {
      this.worldScreenControllerButtonGlyph.spriteName = names1[0];
      this.worldScreenButton.gameObject.SetActive(false);
      this.worldScreenControllerButton.SetActive(true);
    }
    else
    {
      this.worldScreenControllerButton.SetActive(false);
      this.worldScreenButton.gameObject.SetActive(true);
    }
  }

  public void CloseTutorialPopup(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
    if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
    {
      TutorialPopupOverlay currentPopupOverlay = instance.GetCurrentPopupOverlay();
      if ((UnityEngine.Object) currentPopupOverlay != (UnityEngine.Object) null && currentPopupOverlay.gameObject.activeInHierarchy)
      {
        if (currentPopupOverlay.hasButton)
        {
          CGameManager.pausePopup = false;
          currentPopupOverlay.DoClick();
        }
        if (!TutorialSystem.IsModuleActive("26B") || !this.newsHistory.isActive)
          return;
        instance.StartCoroutine(instance.UpdateTutorial());
        this.newsHistory.ToggleNews();
        return;
      }
      if ((bool) (UnityEngine.Object) this.GetActiveSubScreen("popup"))
      {
        this.ClosePopup();
        return;
      }
      if (this.newsHistory.isActive)
      {
        this.newsHistory.ToggleNews();
        return;
      }
    }
    this.GoToPause();
  }

  public void ClosePopup()
  {
    this.activePopup = (PopupMessage) null;
    CGameManager.pausePopup = false;
    if (!CGameManager.IsMultiplayerGame)
      CGameManager.SetPaused(false);
    this.HideSubScreen("popup");
  }

  public void ShowPopup(PopupMessage message, CHUDPopupSubScreen.OnClose close = null)
  {
    if (CGameManager.IsMultiplayerGame)
    {
      MultiplayerEventsPopup.instance.SendPopupMessage(message);
    }
    else
    {
      this.activePopup = message;
      Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
      Texture2D customIcon = (Texture2D) null;
      if (currentLoadedScenario != null && currentLoadedScenario.customIcons != null && currentLoadedScenario.customIcons.ContainsKey(message.icon))
      {
        Debug.Log((object) ("Found Custom Icon " + message.icon));
        customIcon = currentLoadedScenario.customIcons[message.icon];
      }
      string tagName = (string) null;
      Disease disease = message.disease;
      if (disease != null && disease.techEvolved.Count > 0 && disease.history[message.disease.history.Count - 1] is TechHistory techHistory)
        tagName = CLocalisationManager.GetText(disease.GetTechnology(techHistory.id).name);
      if (string.IsNullOrEmpty(tagName))
        tagName = "IG_No_Evolution";
      string str1 = CLocalisationManager.GetParameterisedText(message.title, message.diseases, message.countries);
      if (message.country != null)
        str1 = str1.Replace("%C", CLocalisationManager.GetText(message.country.name));
      if (message.disease != null)
        str1 = str1.Replace("%D", message.disease.name);
      string titleText = str1.Replace("%E", CLocalisationManager.GetText(tagName));
      string str2 = CLocalisationManager.GetParameterisedText(message.message, message.diseases, message.countries);
      if (message.country != null)
        str2 = str2.Replace("%C", CLocalisationManager.GetText(message.country.name));
      if (message.disease != null)
        str2 = str2.Replace("%D", message.disease.name);
      string messageText = str2.Replace("%E", CLocalisationManager.GetText(tagName));
      this.popupSubscreen.SetMessage(titleText, messageText, message.icon, customIcon);
      this.popupSubscreen.onClose = close ?? new CHUDPopupSubScreen.OnClose(this.ClosePopup);
      this.popupSubscreen.SetActive(true);
      this.SetActiveSubScreen((IGameSubScreen) this.popupSubscreen);
    }
  }

  public void AddNews(List<IGame.NewsItem> news)
  {
    if (news.Count <= 0)
      return;
    this.mlNews.AddRange((IEnumerable<IGame.NewsItem>) news);
    this.AddArchiveNews(news);
    this.SetNextNews();
  }

  public void AddArchiveNews(List<IGame.NewsItem> news)
  {
    for (int index = 0; index < news.Count; ++index)
    {
      DateTime dateTime = this.startDate.AddDays((double) news[index].turn);
      this.newsHistory.AddNewsHistory(news[index], dateTime.Day.ToString() + "-" + (object) dateTime.Month + "-" + (object) dateTime.Year);
    }
    if (news.Count <= 0)
      return;
    this.newsHistory.Reposition();
  }

  public void SetNextNews()
  {
    if (this.mlNews.Count <= 0 || this.newsObject.IsDisplaying)
      return;
    List<IGame.NewsItem> newsItemList = new List<IGame.NewsItem>();
    for (int index = 0; index < this.mlNews.Count; ++index)
    {
      if (this.mlNews[index].turn >= World.instance.DiseaseTurn - (this.mlNews[index].priority > 1 ? 10 : 3))
        newsItemList.Add(this.mlNews[index]);
    }
    this.mlNews = newsItemList;
    if (this.mlNews.Count <= 0)
      return;
    this.newsObject.SetNewsItem(this.mlNews[0]);
    this.mlNews.RemoveAt(0);
  }

  public void BufferNews() => this.SetNextNews();

  public void SetWorldText(string name) => this.worldText.text = name;

  public void SetCurePercent(Disease disease)
  {
    float f = Mathf.Clamp01(disease.cureCompletePercent);
    if (disease.isCure && disease.vaccineStage == Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE || disease.vaccineStage == Disease.EVaccineProgressStage.VACCINE_KNOWLEDGE_FULL)
      f = Mathf.Clamp01(disease.vaccineKnowledge);
    if ((double) f <= 1.0 / 1000.0 || float.IsNaN(f))
    {
      this.cureText.text = "0%";
      this.cureBar.value = 0.0f;
    }
    else
    {
      if ((double) f > 0.99900001287460327)
        f = 1f;
      this.cureText.text = (f * 100f).ToString("f2") + "%";
      this.cureBar.value = f;
      if ((double) f >= 1.0 && !this.isLoopingCureSound && !this.hasPlayedLoopingCureSound)
      {
        float num = 5f;
        this.StartCoroutine(this.CoStopLoopingCureSound(num));
        this.isLoopingCureSound = true;
        this.hasPlayedLoopingCureSound = true;
        CSoundManager.instance.PlaySFX("cure_100_bubble", out this.loopingCureSound, true, stopLoopAfter: num);
      }
      else if ((double) f < 1.0)
      {
        this.hasPlayedLoopingCureSound = false;
        if (this.isLoopingCureSound)
          this.StopLoopingCureSound();
      }
    }
    if (this.isCureGameMode)
    {
      this.cureStageText.text = CLocalisationManager.GetText(disease.GetVaccineStage());
      if (disease.vaccineStage == Disease.EVaccineProgressStage.VACCINE_DEVELOPMENT || disease.vaccineStage == Disease.EVaccineProgressStage.VACCINE_MANUFACTURE)
      {
        string str = CUtils.FormatPercentSignificantDigits(disease.researchWeekly);
        if ((double) disease.researchWeekly >= 0.01)
          str = "+" + str;
        this.cureRateText.text = str + "%";
      }
      int num = Mathf.RoundToInt(f - this.currentCureValue);
      if (num > 0)
      {
        this.curePercIncreaseLabel.gameObject.SetActive(true);
        this.curePercIncreaseLabel.text = CUtils.FormatValueToDisplay((float) num, showPlus: true);
        for (int index = 0; index < this.curePercIncreaseTweens.Length; ++index)
        {
          this.curePercIncreaseTweens[index].Reset();
          this.curePercIncreaseTweens[index].PlayForward();
        }
      }
      else if (num < 0)
      {
        this.curePercDecreaseLabel.gameObject.SetActive(true);
        this.curePercDecreaseLabel.text = CUtils.FormatValueToDisplay((float) num, showPlus: true);
        for (int index = 0; index < this.curePercDecreaseTweens.Length; ++index)
        {
          this.curePercDecreaseTweens[index].Reset();
          this.curePercDecreaseTweens[index].PlayForward();
        }
      }
    }
    this.cureFillBottleGraphic.fillAmount = this.cureBar.value;
    this.currentCureValue = f;
  }

  public IEnumerator CoStopLoopingCureSound(float time)
  {
    yield return (object) new WaitForSeconds(time);
    this.StopLoopingCureSound();
  }

  public void StopLoopingCureSound()
  {
    if ((UnityEngine.Object) this.loopingCureSound != (UnityEngine.Object) null)
      this.loopingCureSound.Stop();
    CSoundManager.instance.StopSFX("cure_100_bubble");
    this.isLoopingCureSound = false;
  }

  public void SetDiseaseName(string name) => this.diseaseText.text = name;

  public void SetAuthorityValue(int authValue, int authRateOfChange)
  {
    if (!this.isCureGameMode)
    {
      Debug.LogError((object) "SetAuthorityValue() should never be called from a HUD Screen reference not marked as 'Cure'");
    }
    else
    {
      int num1 = authValue - this.lastAuthValue;
      if (num1 > 0)
      {
        this.lastAuthValue = authValue;
        this.authorityIncreaseLabel.gameObject.SetActive(true);
        this.authorityIncreaseLabel.text = CUtils.FormatValueToDisplay((float) num1, showPlus: true);
        for (int index = 0; index < this.authorityIncreaseTweens.Length; ++index)
        {
          this.authorityIncreaseTweens[index].Reset();
          this.authorityIncreaseTweens[index].PlayForward();
        }
      }
      else if (num1 < 0)
      {
        this.lastAuthValue = authValue;
        this.authorityDecreaseLabel.gameObject.SetActive(true);
        this.authorityDecreaseLabel.text = CUtils.FormatValueToDisplay((float) num1, showPlus: true);
        for (int index = 0; index < this.authorityDecreaseTweens.Length; ++index)
        {
          this.authorityDecreaseTweens[index].Reset();
          this.authorityDecreaseTweens[index].PlayForward();
        }
      }
      this.authorityText.text = authValue.ToString();
      float num2 = Mathf.Clamp01((float) authValue * 0.01f);
      this.authorityBar.value = (double) num2 <= 1.0 / 1000.0 || float.IsNaN((float) authValue) ? 0.0f : num2;
      this.authorityRateOfChangeText.text = authRateOfChange.ToString();
    }
  }

  public void SetDNAPoints(int dna)
  {
    int num1 = dna - this.lastDna;
    if (num1 > 0)
    {
      this.lastDna = dna;
      this.dnaIncreaseLabel.gameObject.SetActive(true);
      this.dnaIncreaseLabel.text = this.isCureGameMode ? CUtils.FormatValueToDisplay((float) num1, showPlus: true) : Mathf.Abs(num1).ToString();
      for (int index = 0; index < this.dnaIncreaseTweens.Length; ++index)
      {
        this.dnaIncreaseTweens[index].Reset();
        this.dnaIncreaseTweens[index].PlayForward();
      }
    }
    else if (num1 < 0)
    {
      this.lastDna = dna;
      this.dnaDecreaseLabel.gameObject.SetActive(true);
      this.dnaDecreaseLabel.text = this.isCureGameMode ? CUtils.FormatValueToDisplay((float) num1, showPlus: true) : Mathf.Abs(num1).ToString();
      for (int index = 0; index < this.dnaDecreaseTweens.Length; ++index)
      {
        this.dnaDecreaseTweens[index].Reset();
        this.dnaDecreaseTweens[index].PlayForward();
      }
    }
    this.dnaText.text = dna.ToString();
    if (CGameManager.IsTutorialGame && !TutorialSystem.IsModuleComplete("11A") && !TutorialSystem.IsModuleComplete("12A") && this.dnaCountForTutorial11A != int.MaxValue && this.dnaCountForTutorial11A <= dna)
    {
      if (!TutorialSystem.IsModuleActive())
      {
        TutorialSystem.CheckModule((Func<bool>) (() => true), "11A", true);
        this.dnaCountForTutorial11A = int.MaxValue;
      }
      else
      {
        Debug.LogWarning((object) "Other module active delaying 11A");
        this.dnaCountForTutorial11A = this.lastDna + 15;
      }
    }
    float num2 = Mathf.Clamp01((float) dna * 0.01f);
    this.diseaseBar.value = (double) num2 <= 1.0 / 1000.0 || float.IsNaN((float) dna) ? 0.0f : num2;
    Disease disease = CGameManager.localPlayerInfo.disease;
    if (disease.diseaseType == Disease.EDiseaseType.NECROA)
    {
      this.abilitySubscreen.SetAbilityState(EAbilityType.reanimate, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.reanimate, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.zombie_horde, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.zombie_horde, disease));
    }
    else if (disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      this.abilitySubscreen.SetAbilityState(EAbilityType.move, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.move, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.rampage, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.rampage, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.create_colony, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.create_colony, disease));
    }
    else if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      this.abilitySubscreen.SetAbilityState(EAbilityType.bloodrage, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.bloodrage, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.castle, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.castle, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.vampiretravel, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.vampiretravel, disease));
    }
    else if (disease.diseaseType == Disease.EDiseaseType.CURE)
    {
      this.abilitySubscreen.SetAbilityState(EAbilityType.economic_support, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.economic_support, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.investigation_team, disease.vampires.Count > 0 && disease.vampires[0].currentCountry != null && disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.investigation_team, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.raise_priority, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.raise_priority, disease));
    }
    else
    {
      if (!CGameManager.IsMultiplayerGame)
        return;
      this.abilitySubscreen.SetAbilityState(EAbilityType.unscheduled_flight, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.unscheduled_flight, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.immune_shock, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.immune_shock, disease));
      if (!CGameManager.IsCoopMPGame)
        return;
      this.abilitySubscreen.SetAbilityState(EAbilityType.infect_boost, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.infect_boost, disease));
      this.abilitySubscreen.SetAbilityState(EAbilityType.lethal_boost, disease.evoPoints >= CGameManager.GetActiveAbilityCost(EAbilityType.lethal_boost, disease));
    }
  }

  public void SetStartDate(DateTime date)
  {
    this.startDate = date;
    Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
    int num1;
    string str1;
    if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL"))
    {
      string scenarioDifficultyNum = CGameManager.GetScenarioDifficultyNum(CGameManager.game.CurrentLoadedScenario.filename);
      string str2 = !CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("BYD") ? "Future :ff00ff]" + scenarioDifficultyNum + ":ffffff]" : "Beyond :ff0000]" + scenarioDifficultyNum + ":ffffff]";
      string[] strArray = new string[49];
      strArray[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
      strArray[1] = this.startDate.Day.ToString();
      strArray[2] = "-";
      strArray[3] = this.startDate.Month.ToString();
      strArray[4] = "-";
      num1 = this.startDate.Year;
      strArray[5] = num1.ToString();
      strArray[6] = "\n\n\n";
      strArray[7] = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + ":ffffff]";
      strArray[8] = "\n";
      strArray[9] = str2;
      strArray[10] = "\n";
      strArray[11] = "富裕 ";
      strArray[12] = disease.wealthy.ToString("N3");
      strArray[13] = "\n";
      strArray[14] = "贫困 ";
      float num2 = disease.poverty;
      strArray[15] = num2.ToString("N3");
      strArray[16] = "\n";
      strArray[17] = "炎热 ";
      num2 = disease.hot;
      strArray[18] = num2.ToString("N3");
      strArray[19] = "\n";
      strArray[20] = "寒冷 ";
      num2 = disease.cold;
      strArray[21] = num2.ToString("N3");
      strArray[22] = "\n";
      strArray[23] = "城市 ";
      num2 = disease.urban;
      strArray[24] = num2.ToString("N3");
      strArray[25] = "\n";
      strArray[26] = "农村 ";
      num2 = disease.rural;
      strArray[27] = num2.ToString("N3");
      strArray[28] = "\n";
      strArray[29] = "湿润 ";
      num2 = disease.humid;
      strArray[30] = num2.ToString("N3");
      strArray[31] = "\n";
      strArray[32] = "干燥 ";
      num2 = disease.arid;
      strArray[33] = num2.ToString("N3");
      strArray[34] = "\n";
      strArray[35] = "陆地 ";
      num2 = disease.landTransmission;
      strArray[36] = num2.ToString("N3");
      strArray[37] = "\n";
      strArray[38] = "空气 ";
      num2 = disease.airTransmission;
      strArray[39] = num2.ToString("N3");
      strArray[40] = "\n";
      strArray[41] = "海洋 ";
      num2 = disease.seaTransmission;
      strArray[42] = num2.ToString("N3");
      strArray[43] = "\n";
      strArray[44] = "突变 ";
      num2 = disease.mutation;
      strArray[45] = num2.ToString("N3");
      strArray[46] = "\n";
      strArray[47] = "尸传 ";
      num2 = disease.corpseTransmission;
      strArray[48] = num2.ToString("N3");
      str1 = string.Concat(strArray);
    }
    else
    {
      string[] strArray = new string[45];
      strArray[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
      strArray[1] = this.startDate.Day.ToString();
      strArray[2] = "-";
      strArray[3] = this.startDate.Month.ToString();
      strArray[4] = "-";
      num1 = this.startDate.Year;
      strArray[5] = num1.ToString();
      strArray[6] = "\n\n\n";
      strArray[7] = "富裕 ";
      strArray[8] = disease.wealthy.ToString("N3");
      strArray[9] = "\n";
      strArray[10] = "贫困 ";
      strArray[11] = disease.poverty.ToString("N3");
      strArray[12] = "\n";
      strArray[13] = "炎热 ";
      strArray[14] = disease.hot.ToString("N3");
      strArray[15] = "\n";
      strArray[16] = "寒冷 ";
      strArray[17] = disease.cold.ToString("N3");
      strArray[18] = "\n";
      strArray[19] = "城市 ";
      strArray[20] = disease.urban.ToString("N3");
      strArray[21] = "\n";
      strArray[22] = "农村 ";
      strArray[23] = disease.rural.ToString("N3");
      strArray[24] = "\n";
      strArray[25] = "湿润 ";
      strArray[26] = disease.humid.ToString("N3");
      strArray[27] = "\n";
      strArray[28] = "干燥 ";
      strArray[29] = disease.arid.ToString("N3");
      strArray[30] = "\n";
      strArray[31] = "陆地 ";
      strArray[32] = disease.landTransmission.ToString("N3");
      strArray[33] = "\n";
      strArray[34] = "空气 ";
      strArray[35] = disease.airTransmission.ToString("N3");
      strArray[36] = "\n";
      strArray[37] = "海洋 ";
      strArray[38] = disease.seaTransmission.ToString("N3");
      strArray[39] = "\n";
      strArray[40] = "突变 ";
      strArray[41] = disease.mutation.ToString("N3");
      strArray[42] = "\n";
      strArray[43] = "尸传 ";
      strArray[44] = disease.corpseTransmission.ToString("N3");
      str1 = string.Concat(strArray);
    }
    if (!IGame.showMoreDetail)
    {
      if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL"))
      {
        string scenarioDifficultyNum = CGameManager.GetScenarioDifficultyNum(CGameManager.game.CurrentLoadedScenario.filename);
        string str3 = !CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("BYD") ? "Future :ff00ff]" + scenarioDifficultyNum + ":ffffff]" : "Beyond :ff0000]" + scenarioDifficultyNum + ":ffffff]";
        string[] strArray = new string[10];
        strArray[0] = "\n\n\n\n";
        num1 = this.startDate.Day;
        strArray[1] = num1.ToString();
        strArray[2] = "-";
        num1 = this.startDate.Month;
        strArray[3] = num1.ToString();
        strArray[4] = "-";
        num1 = this.startDate.Year;
        strArray[5] = num1.ToString();
        strArray[6] = "\n\n\n";
        strArray[7] = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + ":ffffff]";
        strArray[8] = "\n";
        strArray[9] = str3;
        str1 = string.Concat(strArray);
      }
      else
      {
        string[] strArray = new string[5];
        num1 = this.startDate.Day;
        strArray[0] = num1.ToString();
        strArray[1] = "-";
        num1 = this.startDate.Month;
        strArray[2] = num1.ToString();
        strArray[3] = "-";
        num1 = this.startDate.Year;
        strArray[4] = num1.ToString();
        str1 = string.Concat(strArray);
      }
    }
    this.mpClockText.width = 900;
    this.mpClockText.height = 1600;
    this.mpClockText.multiLine = true;
    this.mpClockText.maxLineCount = 200;
    this.mpClockText.text = str1;
  }

  public void SetDay(int turn)
  {
    this.mDate = this.startDate.AddDays((double) turn);
    CGameManager.currentGameDate = this.mDate;
    Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
    float inefficiencyMultiplier = disease.researchInefficiencyMultiplier;
    string str1 = "阻滞 ";
    float num1;
    string str2;
    if ((double) inefficiencyMultiplier >= 0.0)
    {
      string str3 = str1;
      num1 = inefficiencyMultiplier * 100f;
      string str4 = num1.ToString("N2");
      str2 = str3 + "[ff0000]" + str4 + "%[ffffff]";
    }
    else
    {
      string str5 = str1;
      num1 = inefficiencyMultiplier * 100f;
      string str6 = num1.ToString("N2");
      str2 = str5 + "[11ffee]" + str6 + "%[ffffff]";
    }
    long num2 = 0;
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.IsMultiplayerGame && !CGameManager.IsCoopMPGame)
      num2 = CGameManager.game.CurrentLoadedScenario == null ? ((SPDisease) World.instance.diseases[0]).GetCurrentScore(true, false) : ((SPDisease) World.instance.diseases[0]).GetCurrentScore(true, true);
    string[] strArray1 = new string[5]
    {
      this.mDate.Day.ToString(),
      "-",
      null,
      null,
      null
    };
    int num3 = this.mDate.Month;
    strArray1[2] = num3.ToString();
    strArray1[3] = "-";
    num3 = this.mDate.Year;
    strArray1[4] = num3.ToString();
    string str7 = string.Concat(strArray1);
    if (CGameManager.IsFederalScenario("sorder"))
    {
      DateTime now = DateTime.Now;
      string[] strArray2 = new string[5];
      num3 = now.Hour;
      strArray2[0] = num3.ToString("00");
      strArray2[1] = ":";
      num3 = now.Minute;
      strArray2[2] = num3.ToString("00");
      strArray2[3] = ":";
      num3 = now.Second;
      strArray2[4] = num3.ToString("00");
      str7 = string.Concat(strArray2);
    }
    if (CGameManager.IsFederalScenario("绯色审判"))
    {
      num1 = disease.mutationCounter;
      string str8 = num1.ToString("N1");
      num1 = disease.mutationTrigger;
      string str9 = num1.ToString("N1");
      string str10 = str8 + "/" + str9;
      str7 = (double) disease.customGlobalVariable5 <= 0.05000000074505806 ? ((double) disease.mutationCounter >= 70.0 ? "[ff1111]" + str10 + "[ffffff]" : "[11ff11]" + str10 + "[ffffff]") : "[00ffff]" + str10 + "[ffffff]";
    }
    string str11;
    if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL"))
    {
      string scenarioDifficultyNum = CGameManager.GetScenarioDifficultyNum(CGameManager.game.CurrentLoadedScenario.filename);
      string str12 = !CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("BYD") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("FTR") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("PRS") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("PST") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("MXM") ? "Unknown [ffffff]" + scenarioDifficultyNum + "[ffffff]" : "Maximum [ffee00]" + scenarioDifficultyNum + "[ffffff]") : "Past [00bbff]" + scenarioDifficultyNum + "[ffffff]") : "Present [00ff11]" + scenarioDifficultyNum + "[ffffff]") : "Future [ff00ff]" + scenarioDifficultyNum + "[ffffff]") : "Beyond [ff0000]" + scenarioDifficultyNum + "[ffffff]";
      string[] strArray3 = new string[59];
      strArray3[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
      strArray3[1] = str7;
      strArray3[2] = "\n\n\n";
      strArray3[3] = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + "[ffffff]";
      strArray3[4] = "\n";
      strArray3[5] = str12;
      strArray3[6] = "\n";
      strArray3[7] = "富裕 ";
      num1 = disease.wealthy;
      strArray3[8] = num1.ToString("N3");
      strArray3[9] = "\n";
      strArray3[10] = "贫困 ";
      num1 = disease.poverty;
      strArray3[11] = num1.ToString("N3");
      strArray3[12] = "\n";
      strArray3[13] = "炎热 ";
      num1 = disease.hot;
      strArray3[14] = num1.ToString("N3");
      strArray3[15] = "\n";
      strArray3[16] = "寒冷 ";
      num1 = disease.cold;
      strArray3[17] = num1.ToString("N3");
      strArray3[18] = "\n";
      strArray3[19] = "城市 ";
      num1 = disease.urban;
      strArray3[20] = num1.ToString("N3");
      strArray3[21] = "\n";
      strArray3[22] = "农村 ";
      num1 = disease.rural;
      strArray3[23] = num1.ToString("N3");
      strArray3[24] = "\n";
      strArray3[25] = "湿润 ";
      num1 = disease.humid;
      strArray3[26] = num1.ToString("N3");
      strArray3[27] = "\n";
      strArray3[28] = "干燥 ";
      num1 = disease.arid;
      strArray3[29] = num1.ToString("N3");
      strArray3[30] = "\n";
      strArray3[31] = "陆地 ";
      num1 = disease.landTransmission;
      strArray3[32] = num1.ToString("N3");
      strArray3[33] = "\n";
      strArray3[34] = "空气 ";
      num1 = disease.airTransmission;
      strArray3[35] = num1.ToString("N3");
      strArray3[36] = "\n";
      strArray3[37] = "海洋 ";
      num1 = disease.seaTransmission;
      strArray3[38] = num1.ToString("N3");
      strArray3[39] = "\n";
      strArray3[40] = "突变 ";
      num1 = disease.mutation;
      strArray3[41] = num1.ToString("N3");
      strArray3[42] = "\n";
      strArray3[43] = "尸传 ";
      num1 = disease.corpseTransmission;
      strArray3[44] = num1.ToString("N3");
      strArray3[45] = "\n";
      strArray3[46] = str2;
      strArray3[47] = "\n";
      strArray3[48] = "储备 ";
      num1 = disease.infectedPointsPot;
      strArray3[49] = num1.ToString("N3");
      strArray3[50] = "\n";
      strArray3[51] = "进度 ";
      string str13;
      if (!CGameManager.IsFederalScenario("终末千面"))
      {
        num1 = disease.mutationCounter;
        string str14 = num1.ToString("N1");
        num1 = disease.mutationTrigger;
        string str15 = num1.ToString("N1");
        str13 = str14 + "/" + str15;
      }
      else
        str13 = "---";
      strArray3[52] = str13;
      strArray3[53] = "\n";
      strArray3[54] = "分数 ";
      strArray3[55] = num2.ToString("N0");
      strArray3[56] = "\n";
      strArray3[57] = "孢子 ";
      num1 = disease.sporeCounter;
      strArray3[58] = num1.ToString("N2");
      str11 = string.Concat(strArray3);
      if (CGameManager.IsFederalScenario("世界狂潮"))
      {
        string str16;
        if (!World.instance.firedEvents.ContainsKey("eventPrompt I"))
          str16 = "[838b8b]未知   0威望[ffffff]";
        else if ((double) disease.mutationCounter > 70.0)
        {
          num1 = disease.mutationCounter;
          str16 = "[7cfc00]正常   " + num1.ToString("N0") + "威望[ffffff]";
        }
        else if ((double) disease.mutationCounter > 40.0)
        {
          num1 = disease.mutationCounter;
          str16 = "[ffd700]良好   " + num1.ToString("N0") + "威望[ffffff]";
        }
        else
        {
          num1 = disease.mutationCounter;
          str16 = "[ff4500]危险   " + num1.ToString("N0") + "威望[ffffff]";
        }
        string str17;
        double num4;
        if (disease.difficulty < 3)
          str17 = "[32cd32]0%[ffffff]";
        else if ((double) disease.geneticDriftMax < 1.2000000476837158)
        {
          num4 = 100.0 * (double) disease.geneticDriftMax - 100.0;
          str17 = "[32cd32]" + num4.ToString("N0") + "%[ffffff]";
        }
        else
        {
          num4 = 100.0 * (double) disease.geneticDriftMax - 100.0;
          str17 = "[ff3030]" + num4.ToString("N0") + "%[ffffff]";
        }
        string[] strArray4 = new string[59];
        strArray4[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        strArray4[1] = str16;
        strArray4[2] = "\n\n\n";
        strArray4[3] = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + "[ffffff]";
        strArray4[4] = "\n";
        strArray4[5] = str12;
        strArray4[6] = "\n";
        strArray4[7] = "富裕 ";
        num1 = disease.wealthy;
        strArray4[8] = num1.ToString("N3");
        strArray4[9] = "\n";
        strArray4[10] = "贫困 ";
        num1 = disease.poverty;
        strArray4[11] = num1.ToString("N3");
        strArray4[12] = "\n";
        strArray4[13] = "炎热 ";
        num1 = disease.hot;
        strArray4[14] = num1.ToString("N3");
        strArray4[15] = "\n";
        strArray4[16] = "寒冷 ";
        num1 = disease.cold;
        strArray4[17] = num1.ToString("N3");
        strArray4[18] = "\n";
        strArray4[19] = "城市 ";
        num1 = disease.urban;
        strArray4[20] = num1.ToString("N3");
        strArray4[21] = "\n";
        strArray4[22] = "农村 ";
        num1 = disease.rural;
        strArray4[23] = num1.ToString("N3");
        strArray4[24] = "\n";
        strArray4[25] = "湿润 ";
        num1 = disease.humid;
        strArray4[26] = num1.ToString("N3");
        strArray4[27] = "\n";
        strArray4[28] = "干燥 ";
        num1 = disease.arid;
        strArray4[29] = num1.ToString("N3");
        strArray4[30] = "\n";
        strArray4[31] = "陆地 ";
        num1 = disease.landTransmission;
        strArray4[32] = num1.ToString("N3");
        strArray4[33] = "\n";
        strArray4[34] = "空气 ";
        num1 = disease.airTransmission;
        strArray4[35] = num1.ToString("N3");
        strArray4[36] = "\n";
        strArray4[37] = "海洋 ";
        num1 = disease.seaTransmission;
        strArray4[38] = num1.ToString("N3");
        strArray4[39] = "\n";
        strArray4[40] = "腐败 ";
        num4 = (double) disease.mutation * -100.0;
        strArray4[41] = num4.ToString("N0") + "%";
        strArray4[42] = "\n";
        strArray4[43] = "尸传 ";
        num1 = disease.corpseTransmission;
        strArray4[44] = num1.ToString("N3");
        strArray4[45] = "\n";
        strArray4[46] = str2;
        strArray4[47] = "\n";
        strArray4[48] = "储备 ";
        num1 = disease.infectedPointsPot;
        strArray4[49] = num1.ToString("N3");
        strArray4[50] = "\n";
        strArray4[51] = "市场 ";
        strArray4[52] = str17;
        strArray4[53] = "\n";
        strArray4[54] = "分数 ";
        strArray4[55] = num2.ToString("N0");
        strArray4[56] = "\n";
        strArray4[57] = "潜力 ";
        num4 = (double) disease.sporeCounter * -1.0;
        strArray4[58] = num4.ToString("N2");
        str11 = string.Concat(strArray4);
      }
      if (CGameManager.IsGiantScenario())
      {
        string[] strArray5 = new string[66];
        strArray5[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        strArray5[1] = str7;
        strArray5[2] = "\n\n\n";
        strArray5[3] = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + "[ffffff]";
        strArray5[4] = "\n";
        strArray5[5] = str12;
        strArray5[6] = "\n";
        strArray5[7] = "富裕 ";
        num1 = disease.wealthy;
        strArray5[8] = num1.ToString("N3");
        strArray5[9] = "\n";
        strArray5[10] = "贫困 ";
        num1 = disease.poverty;
        strArray5[11] = num1.ToString("N3");
        strArray5[12] = "\n";
        strArray5[13] = "炎热 ";
        num1 = disease.hot;
        strArray5[14] = num1.ToString("N3");
        strArray5[15] = "\n";
        strArray5[16] = "寒冷 ";
        num1 = disease.cold;
        strArray5[17] = num1.ToString("N3");
        strArray5[18] = "\n";
        strArray5[19] = "城市 ";
        num1 = disease.urban;
        strArray5[20] = num1.ToString("N3");
        strArray5[21] = "\n";
        strArray5[22] = "农村 ";
        num1 = disease.rural;
        strArray5[23] = num1.ToString("N3");
        strArray5[24] = "\n";
        strArray5[25] = "湿润 ";
        num1 = disease.humid;
        strArray5[26] = num1.ToString("N3");
        strArray5[27] = "\n";
        strArray5[28] = "干燥 ";
        num1 = disease.arid;
        strArray5[29] = num1.ToString("N3");
        strArray5[30] = "\n";
        strArray5[31] = "陆地 ";
        num1 = disease.landTransmission;
        strArray5[32] = num1.ToString("N3");
        strArray5[33] = "\n";
        strArray5[34] = "空气 ";
        num1 = disease.airTransmission;
        strArray5[35] = num1.ToString("N3");
        strArray5[36] = "\n";
        strArray5[37] = "海洋 ";
        num1 = disease.seaTransmission;
        strArray5[38] = num1.ToString("N3");
        strArray5[39] = "\n";
        strArray5[40] = "尸传 ";
        num1 = disease.corpseTransmission;
        strArray5[41] = num1.ToString("N3");
        strArray5[42] = "\n";
        strArray5[43] = "储备 ";
        num1 = disease.infectedPointsPot;
        strArray5[44] = num1.ToString("N3");
        strArray5[45] = "\n";
        strArray5[46] = "行动 ";
        num1 = disease.cureBaseMultiplier;
        strArray5[47] = num1.ToString("N3");
        strArray5[48] = "\n";
        strArray5[49] = "损伤 ";
        num1 = disease.researchInefficiencyMultiplier * -100f;
        strArray5[50] = "[ff1111]" + num1.ToString("N1") + "%[ffffff]";
        strArray5[51] = "\n";
        strArray5[52] = "血量 ";
        num1 = -1f * disease.mutation;
        strArray5[53] = "[11ffff]" + num1.ToString("N1") + "[ffffff]";
        strArray5[54] = "\n";
        strArray5[55] = "攻击 ";
        num1 = -1f * disease.sporeCounter;
        strArray5[56] = num1.ToString("N1");
        strArray5[57] = "\n";
        strArray5[58] = "加成 ";
        num1 = (float) (100.0 * (double) disease.globalLethalityBottomValue - 100.0);
        strArray5[59] = num1.ToString("N1") + "%";
        strArray5[60] = "\n";
        strArray5[61] = "分数 ";
        strArray5[62] = num2.ToString("N0");
        strArray5[63] = "\n";
        strArray5[64] = "孢子 ";
        num1 = disease.sporeCounter;
        strArray5[65] = num1.ToString("N2");
        str11 = string.Concat(strArray5);
      }
      if (CGameManager.IsFederalScenario("ReconstructionIncBeyond"))
        str11 = "\n\n\n" + str11 + "\n" + "陆传 " + disease.customGlobalVariable3.ToString("N1") + "\n" + "水传 " + disease.customGlobalVariable4.ToString("N1") + "\n" + "空传 " + disease.customGlobalVariable5.ToString("N1");
    }
    else
    {
      string[] strArray6 = new string[55];
      strArray6[0] = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
      strArray6[1] = str7;
      strArray6[2] = "\n\n\n";
      strArray6[3] = "富裕 ";
      num1 = disease.wealthy;
      strArray6[4] = num1.ToString("N3");
      strArray6[5] = "\n";
      strArray6[6] = "贫困 ";
      num1 = disease.poverty;
      strArray6[7] = num1.ToString("N3");
      strArray6[8] = "\n";
      strArray6[9] = "炎热 ";
      num1 = disease.hot;
      strArray6[10] = num1.ToString("N3");
      strArray6[11] = "\n";
      strArray6[12] = "寒冷 ";
      num1 = disease.cold;
      strArray6[13] = num1.ToString("N3");
      strArray6[14] = "\n";
      strArray6[15] = "城市 ";
      num1 = disease.urban;
      strArray6[16] = num1.ToString("N3");
      strArray6[17] = "\n";
      strArray6[18] = "农村 ";
      num1 = disease.rural;
      strArray6[19] = num1.ToString("N3");
      strArray6[20] = "\n";
      strArray6[21] = "湿润 ";
      num1 = disease.humid;
      strArray6[22] = num1.ToString("N3");
      strArray6[23] = "\n";
      strArray6[24] = "干燥 ";
      num1 = disease.arid;
      strArray6[25] = num1.ToString("N3");
      strArray6[26] = "\n";
      strArray6[27] = "陆地 ";
      num1 = disease.landTransmission;
      strArray6[28] = num1.ToString("N3");
      strArray6[29] = "\n";
      strArray6[30] = "空气 ";
      num1 = disease.airTransmission;
      strArray6[31] = num1.ToString("N3");
      strArray6[32] = "\n";
      strArray6[33] = "海洋 ";
      num1 = disease.seaTransmission;
      strArray6[34] = num1.ToString("N3");
      strArray6[35] = "\n";
      strArray6[36] = "突变 ";
      num1 = disease.mutation;
      strArray6[37] = num1.ToString("N3");
      strArray6[38] = "\n";
      strArray6[39] = "尸传 ";
      num1 = disease.corpseTransmission;
      strArray6[40] = num1.ToString("N3");
      strArray6[41] = "\n";
      strArray6[42] = str2;
      strArray6[43] = "\n";
      strArray6[44] = "储备 ";
      num1 = disease.infectedPointsPot;
      strArray6[45] = num1.ToString("N3");
      strArray6[46] = "\n";
      strArray6[47] = "进度 ";
      string str18;
      if (!CGameManager.IsFederalScenario("终末千面"))
      {
        num1 = disease.mutationCounter;
        string str19 = num1.ToString("N1");
        num1 = disease.mutationTrigger;
        string str20 = num1.ToString("N1");
        str18 = str19 + "/" + str20;
      }
      else
        str18 = "---";
      strArray6[48] = str18;
      strArray6[49] = "\n";
      strArray6[50] = "分数 ";
      strArray6[51] = num2.ToString("N0");
      strArray6[52] = "\n";
      strArray6[53] = "孢子 ";
      num1 = disease.sporeCounter;
      strArray6[54] = num1.ToString("N2");
      str11 = string.Concat(strArray6);
    }
    if (CGameManager.CheckExternalMethodExist("SidebarAppend"))
    {
      (string[], string[]) tuple = CGameManager.CallSidebarAppendDouble(new string[1]
      {
        "Title"
      }, new string[1]{ "Value" }, World.instance, disease, (Country) null, (LocalDisease) null);
      int index = 0;
      string[] strArray7 = tuple.Item1;
      for (string[] strArray8 = tuple.Item2; index < strArray7.Length && index < strArray8.Length; ++index)
        str11 = "\n" + str11 + "\n" + strArray7[index] + " " + strArray8[index];
    }
    if (!IGame.showMoreDetail)
    {
      if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL"))
      {
        string scenarioDifficultyNum = CGameManager.GetScenarioDifficultyNum(CGameManager.game.CurrentLoadedScenario.filename);
        string str21 = !CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("BYD") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("FTR") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("PRS") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("PST") ? (!CSLocalUGCHandler.GetScenarioDifficultyRawExternal(CGameManager.game.CurrentLoadedScenario.filename).Contains("MXM") ? "Unknown [ffffff]" + scenarioDifficultyNum + "[ffffff]" : "Maximum [ffee00]" + scenarioDifficultyNum + "[ffffff]") : "Past [00bbff]" + scenarioDifficultyNum + "[ffffff]") : "Present [00ff11]" + scenarioDifficultyNum + "[ffffff]") : "Future [ff00ff]" + scenarioDifficultyNum + "[ffffff]") : "Beyond [ff0000]" + scenarioDifficultyNum + "[ffffff]";
        str11 = "\n\n\n\n" + str7 + "\n\n\n" + (CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle + "[ffffff]") + "\n" + str21;
      }
      else
        str11 = str7;
    }
    this.mpClockText.width = 900;
    this.mpClockText.height = 1600;
    this.mpClockText.multiLine = true;
    this.mpClockText.maxLineCount = 200;
    this.mpClockText.text = str11;
  }

  private void ZoomCamera() => Camera_Zoom.instance.SetZoomFactor(this.mpZoomScroll.value);

  public void SetButtonState(int speed, int mySpeed = 0, int oppSpeed = 0)
  {
    if (CGameManager.IsMultiplayerGame && !CGameManager.IsAIGame)
    {
      if ((UnityEngine.Object) this.mpMessage == (UnityEngine.Object) null)
        return;
      this.speed0Toggle.value = false;
      this.speed1Toggle.value = false;
      this.speed2Toggle.value = false;
      this.speed3Toggle.value = false;
      this.mpMessage.gameObject.SetActive(false);
      bool state = false;
      if (speed < 0)
      {
        state = true;
        this.speed0Toggle.value = true;
        this.mpMessage.gameObject.SetActive(true);
        this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Resume_In") + " " + (object) -speed + "...";
        CSoundManager.instance.PlaySFX("pause_count");
      }
      else if (speed == 0)
      {
        state = true;
        this.speed0Toggle.value = true;
        if (mySpeed == 0 && oppSpeed == 0)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Click_To_Resume");
          CSoundManager.instance.PlaySFX("pause_accept");
        }
        else if (mySpeed == 1 && oppSpeed == 0)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Me_Resume_Request");
        }
        else if (mySpeed == 0 && oppSpeed == 1)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Opp_Resume_Request");
        }
      }
      else if (speed > 0)
      {
        string[] strArray = new string[3]
        {
          "Play",
          "FF",
          "FFF"
        };
        bool isCoopMpGame = CGameManager.IsCoopMPGame;
        if (oppSpeed == 0)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText(CGameManager.IsCoopMPGame ? "MP_Coop_Speed_Opp_Pause_Request" : "MP_Speed_Opp_Pause_Request");
          CSoundManager.instance.PlaySFX("pause_request");
        }
        else if (mySpeed == 0)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText(CGameManager.IsCoopMPGame ? "MP_Coop_Speed_Me_Pause_Request" : "MP_Speed_Me_Pause_Request");
          CSoundManager.instance.PlaySFX("pause_request");
        }
        else if (isCoopMpGame && oppSpeed > speed && oppSpeed > mySpeed)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Opp_FastForward_Request").Replace("%s", strArray[oppSpeed - 1]);
          CSoundManager.instance.PlaySFX("fast_forward_request");
        }
        else if (isCoopMpGame && mySpeed > speed && mySpeed > oppSpeed)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Me_FastForward_Request").Replace("%s", strArray[mySpeed - 1]);
          CSoundManager.instance.PlaySFX("fast_forward_request");
        }
        else if (isCoopMpGame && oppSpeed < speed && oppSpeed < mySpeed && ((CoopDisease) this.network.OpponentPlayerInfo.disease).isDefeated)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Opp_SlowDown_Request").Replace("%s", strArray[oppSpeed - 1]);
          CSoundManager.instance.PlaySFX("fast_forward_request");
        }
        else if (isCoopMpGame && mySpeed < speed && mySpeed < oppSpeed && ((CoopDisease) this.network.LocalPlayerInfo.disease).isDefeated)
        {
          this.mpMessage.gameObject.SetActive(true);
          this.mpMessage.title.text = CLocalisationManager.GetText("MP_Speed_Me_SlowDown_Request").Replace("%s", strArray[mySpeed - 1]);
          CSoundManager.instance.PlaySFX("fast_forward_request");
        }
      }
      this.speed0Toggle.Set(state);
      this.speed1Toggle.Set(!state);
      this.speed2Toggle.Set(speed == 2);
      this.speed3Toggle.Set(speed == 3);
    }
    else
    {
      this.speed0Toggle.Set(speed == 0);
      this.speed1Toggle.Set(speed == 1);
      this.speed2Toggle.Set(speed == 2);
      this.speed3Toggle.Set(speed == 3);
      if (!((UnityEngine.Object) this.mpMessage != (UnityEngine.Object) null))
        return;
      this.mpMessage.gameObject.SetActive(false);
    }
  }

  public void CureCompleteAnimation()
  {
    this.timesToPlay = 0;
    if (!((UnityEngine.Object) this.cureAnimator != (UnityEngine.Object) null))
      return;
    this.cureAnimator.SetTrigger("CureComplete");
  }

  public void StopCureAnimations()
  {
    this.timesToPlay = 0;
    if (!((UnityEngine.Object) this.cureAnimator != (UnityEngine.Object) null))
      return;
    this.cureAnimator.SetTrigger("Hidden");
  }

  public void CureThresholdAnimation(float rate)
  {
    if (this.timesToPlay == 0)
      this.PlayCureThresholdAnimation(rate);
    this.timesToPlay = 1;
  }

  private void PlayCureThresholdAnimation(float rate)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine(this.CoFlashCure(rate, 2f));
  }

  private IEnumerator CoFlashCure(float rate, float time)
  {
    if ((UnityEngine.Object) this.cureParticles != (UnityEngine.Object) null)
      this.cureParticles.SetParticleRate(rate);
    if ((UnityEngine.Object) this.cureAnimator != (UnityEngine.Object) null)
      this.cureAnimator.SetTrigger("Show");
    CSoundManager.instance.PlaySFX("cure_bubble");
    CSoundManager.instance.PlaySFX("cure_hex");
    yield return (object) new WaitForSeconds(time);
    this.timesToPlay = 0;
  }

  public void CleanUp()
  {
    this.newsObject.StopNewsItem();
    this.mlNews.Clear();
    this.newsHistory.CleanUp();
    this.abilitySubscreen.Cleanup();
  }

  public void ShowEndGameScreen()
  {
    CInterfaceManager.instance.Cleanup();
    CUIManager.instance.SetActiveScreen("MP_EndScreen");
  }

  public void OnTutorialBegin(Module withModule)
  {
    string name = withModule.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 70024446:
        if (!(name == "15A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 368025088:
        if (!(name == "3B"))
          return;
        break;
      case 418357945:
        if (!(name == "3A"))
          return;
        break;
      case 456498063:
        if (!(name == "11B"))
          return;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        return;
      case 473275682:
        if (!(name == "11A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 553711730:
        if (!(name == "8A"))
          return;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = true;
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        return;
      case 619248088:
        if (!(name == "6A"))
          return;
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        return;
      case 887145539:
        if (!(name == "5A"))
          return;
        break;
      case 1121964165:
        if (!(name == "27A"))
          return;
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 1290499943:
        if (!(name == "1A"))
          return;
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.CloseTutorialPopup), instance.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.PressReturn), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        this.newsButton.gameObject.SetActive(false);
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        return;
      case 1345417680:
        if (!(name == "17A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        this.countryContextButton.gameObject.SetActive(true);
        return;
      case 1525318569:
        if (!(name == "23A"))
          return;
        goto label_61;
      case 1726105544:
        if (!(name == "26A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
        return;
      case 1776438401:
        if (!(name == "26B"))
          return;
        CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
        return;
      case 1797427655:
        if (!(name == "19B"))
          return;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        return;
      case 1814205274:
        if (!(name == "19A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 1994106163:
        if (!(name == "25A"))
          return;
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
        return;
      case 2097105583:
        if (!(name == "9A"))
          return;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = true;
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        return;
      case 2112205916:
        if (!(name == "7B"))
          return;
        this.Speed1Button.isEnabled = true;
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        return;
      case 2129459948:
        if (!(name == "22A"))
          return;
        goto label_61;
      case 2162538773:
        if (!(name == "7A"))
          return;
        CGameManager.SetPaused(true, true);
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        this.diseaseBoxButtonChoose.gameObject.SetActive(false);
        this.diseaseScreenButton.gameObject.SetActive(false);
        this.showDiseaseScreenButton = false;
        this.diseaseScreenControllerButton.SetActive(false);
        this.worldBoxButtonChoose.gameObject.SetActive(false);
        this.worldScreenButton.gameObject.SetActive(false);
        this.showWorldScreenButton = false;
        this.worldScreenControllerButton.SetActive(false);
        this.countryContextButton.gameObject.SetActive(false);
        CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 2625671836:
        if (!(name == "C51"))
          return;
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        return;
      case 4164101599:
        if (!(name == "10A"))
          return;
        this.Speed0Button.isEnabled = false;
        this.Speed1Button.isEnabled = false;
        this.Speed2Button.isEnabled = false;
        this.Speed3Button.isEnabled = false;
        CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        return;
      default:
        return;
    }
    CGameManager.SetPaused(true, true);
    this.Speed0Button.isEnabled = false;
    this.Speed1Button.isEnabled = false;
    this.Speed2Button.isEnabled = false;
    this.Speed3Button.isEnabled = false;
    CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
    return;
label_61:
    CGameManager.SetPaused(true, true);
    this.Speed0Button.isEnabled = false;
    this.Speed1Button.isEnabled = false;
    this.Speed2Button.isEnabled = false;
    this.Speed3Button.isEnabled = false;
    this.diseaseBoxButtonChoose.gameObject.SetActive(false);
    this.diseaseScreenButton.gameObject.SetActive(false);
    this.showDiseaseScreenButton = false;
    this.diseaseScreenControllerButton.SetActive(false);
    this.worldBoxButtonChoose.gameObject.SetActive(false);
    this.worldScreenButton.gameObject.SetActive(false);
    this.showWorldScreenButton = false;
    this.worldScreenControllerButton.SetActive(false);
    this.countryContextButton.gameObject.SetActive(false);
    CActionManager.instance.RemoveListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
  }

  public void OnTutorialComplete(Module completedModule)
  {
    string name = completedModule.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 360546176:
        if (!(name == "C42"))
          return;
        goto label_72;
      case 418357945:
        if (!(name == "3A"))
          return;
        break;
      case 444434271:
        if (!(name == "C47"))
          return;
        this.showWorldScreenButton = true;
        return;
      case 456498063:
        if (!(name == "11B"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 619248088:
        if (!(name == "6A"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CGameManager.SetPaused(false, true);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
        this.newsButton.gameObject.SetActive(true);
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        return;
      case 741276301:
        if (!(name == "16A"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CGameManager.SetPaused(false, true);
        return;
      case 1071631308:
        if (!(name == "27B"))
          return;
        CGameManager.SetPaused(false, true);
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.CloseTutorialPopup), instance.gameObject);
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.PressReturn), this.gameObject);
        return;
      case 1290499943:
        if (!(name == "1A"))
          return;
        WorldMapController.instance.SetSelectedCountry("china");
        return;
      case 1345417680:
        if (!(name == "17A"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 1474509299:
        if (!(name == "4B"))
          return;
        break;
      case 1776438401:
        if (!(name == "26B"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 1994106163:
        if (!(name == "25A"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 2097105583:
        if (!(name == "9A"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        CGameManager.SetPaused(false, true);
        if (TutorialSystem.IsModuleComplete("12A"))
          return;
        this.dnaCountForTutorial11A = this.lastDna + 15;
        return;
      case 2228889920:
        if (!(name == "C2"))
          return;
        goto label_71;
      case 2279222777:
        if (!(name == "C1"))
          return;
        WorldMapController.instance.SetSelectedCountry("canada");
        return;
      case 2347024542:
        if (!(name == "21F"))
          return;
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 2413443729:
        if (!(name == "C9"))
          return;
        goto label_72;
      case 2473540432:
        if (!(name == "C20"))
          return;
        goto label_71;
      case 2558561360:
        if (!(name == "C55"))
          return;
        goto label_72;
      case 2592116598:
        if (!(name == "C57"))
          return;
        (CUIManager.instance.GetScreen("PauseScreen") as CPauseScreen).QuitToMenu();
        return;
      case 2598247542:
        if (!(name == "24A"))
          return;
        CGameManager.SetPaused(false, true);
        this.Speed0Button.isEnabled = true;
        this.Speed1Button.isEnabled = true;
        this.Speed2Button.isEnabled = true;
        this.Speed3Button.isEnabled = true;
        this.diseaseBoxButtonChoose.gameObject.SetActive(true);
        this.diseaseScreenButton.gameObject.SetActive(true);
        this.showDiseaseScreenButton = true;
        this.worldBoxButtonChoose.gameObject.SetActive(true);
        this.worldScreenButton.gameObject.SetActive(true);
        this.showWorldScreenButton = true;
        this.countryContextButton.gameObject.SetActive(true);
        CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
        CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
        return;
      case 2608894217:
        if (!(name == "C56"))
          return;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C57");
        return;
      case 2624539003:
        if (!(name == "C29"))
          return;
        goto label_72;
      case 2624980288:
        if (!(name == "C19"))
          return;
        goto label_72;
      case 2641463717:
        if (!(name == "C34"))
          return;
        goto label_71;
      case 2809534097:
        if (!(name == "C12"))
          return;
        goto label_71;
      default:
        return;
    }
    this.Speed0Button.isEnabled = true;
    this.Speed1Button.isEnabled = true;
    this.Speed2Button.isEnabled = true;
    this.Speed3Button.isEnabled = true;
    CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
    CGameManager.SetPaused(false, true);
    return;
label_71:
    TutorialSystem.CureTutorialFreeTechSelection = false;
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    return;
label_72:
    TutorialSystem.CureTutorialFreeTechSelection = true;
    CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    CGameManager.SetPaused(false, true);
    CActionManager.instance.AddListener("INPUT_NEWS", new Action<CActionManager.ActionType>(this.newsHistory.ToggleNews), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.PressContinue), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_TOGGLESPEED", new Action<CActionManager.ActionType>(this.ToggleSpeed), this.gameObject);
    this.Speed0Button.isEnabled = true;
    this.Speed1Button.isEnabled = true;
    this.Speed2Button.isEnabled = true;
    this.Speed3Button.isEnabled = true;
    this.diseaseBoxButtonChoose.gameObject.SetActive(true);
    this.diseaseScreenButton.gameObject.SetActive(true);
    this.showDiseaseScreenButton = true;
    this.worldBoxButtonChoose.gameObject.SetActive(true);
    this.worldScreenButton.gameObject.SetActive(true);
    this.showWorldScreenButton = true;
    this.countryContextButton.gameObject.SetActive(true);
    this.dnaCountForTutorial11A = int.MaxValue;
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }
}
