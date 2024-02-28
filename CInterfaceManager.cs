// Decompiled with JetBrains decompiler
// Type: CInterfaceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CInterfaceManager : MonoBehaviour, ITutorial
{
  public static CInterfaceManager instance;
  [Header("Bonus Bubbles")]
  public BonusObject templateCountrySelect;
  public BonusObject templateInfect;
  public BonusObject templateDNA;
  public BonusObject templateCure;
  public BonusObject templateNeurax;
  public BonusObject templateDeath;
  public BonusObject templateNecroa;
  public BonusObject templateApeColony;
  public BonusObject templateVampireLair;
  public BonusObject templateInfectOther;
  public BonusObject templateDNAOther;
  public BonusObject templateCureOther;
  public BonusObject templateNeuraxOther;
  public BonusObject templateDeathOther;
  public BonusObject templateNexusFound;
  public BonusObject templateNexusDNA;
  public BonusObject templateDoubleInfectedDNA;
  public BonusObject templateNuke;
  public BonusObject templateCountrySelectP2Intention;
  public BonusObject templateCountrySelectP2Selected;
  public TimedObject templateMyImmuneShockObject;
  public TimedObject templateTheirImmuneShockObject;
  public TimedObject templateMyBenignMimicObject;
  public TimedObject templateTheirBenignMimicObject;
  public TimedObject templateMyInfectBoostObject;
  public TimedObject templateTheirInfectBoostObject;
  public TimedObject templateMyLethalBoostObject;
  public TimedObject templateTheirLethalBoostObject;
  public GameObject templateNukeExplosion;
  public BonusObject templateCureDeath;
  public BonusObject templateCureInfected;
  public BonusObject templateCureProgress;
  public BonusObject templateCureSelect;
  public BonusObject templateCureNexusInfected;
  public float minBonusScale;
  public float maxBonusScale;
  [Header("Necroa")]
  public NecroaReanimateDisplay templateNecroaDisplay;
  public GameObject parentNecroaDisplay;
  [Header("Vehicles")]
  public BoatObject[] boatPrefab;
  public PlaneObject[] planePrefab;
  public PlaneObject zcomPlanePrefab;
  public HordeObject hordePrefab;
  public HordeObject apeHordePrefab;
  public VampireTravelObject vampireHordePrefab;
  public DroneObject dronePrefab;
  public TargetingDroneObject targetingDronePrefab;
  public MissileObject missilePrefab;
  public VehicleObject helicopterPrefab;
  [Header("Other Prefabs")]
  public NexusObject nexusPrefab;
  public NexusObject otherNexusPrefab;
  public FortObject zcomPrefab;
  public ApeLabObject labPrefab;
  public ApeLabObject vampLabPrefab;
  public ApeColonyObject colonyPrefab;
  public CoopLabObject coopLabPrefab;
  public CastleObject castlePrefab;
  public VampireObject vampirePrefab;
  public FortObject templarFortPrefab;
  [Header("Cure")]
  public FortObject cureIncHQPrefab;
  public CureStateObject cureStatePrefab;
  public VampireObject fieldTeamPrefab;
  public PlaneObject responseTeamPlanePrefab;
  [Header("Cameras")]
  public Camera geometryCamera;
  public Camera_Zoom mpWorldCamera;
  [Header("Layers")]
  public LayerMask countryLayer;
  public LayerMask bubbleLayer;
  [Header("Colours")]
  public Color colorNormal = Color.white;
  public Color colorCure = new Color(0.215f, 0.576f, 0.917f);
  public Color colorNeurax = new Color(0.77f, 0.3f, 0.0f);
  public Color colorFort = new Color(0.115f, 0.376f, 0.917f);
  public Color colorInfected = new Color(0.115f, 0.376f, 0.917f);
  public Color colorNormalRef = Color.white;
  public Color colorCureRef = new Color(0.215f, 0.576f, 0.917f);
  public Color colorNeuraxRef = new Color(0.77f, 0.3f, 0.0f);
  public Color colorFortRef = new Color(0.115f, 0.376f, 0.917f);
  public Color colorInfectedRef = new Color(0.115f, 0.376f, 0.917f);
  public Color colorLineDisabled = new Color(0.5f, 0.5f, 0.5f);
  public CInterfaceManager.HighlightColourSet defaultAAHighlightSet;
  public List<CInterfaceManager.HighlightColourSet> AAHighlightSets = new List<CInterfaceManager.HighlightColourSet>();
  public CInterfaceManager.HighlightColourSet currentAAHighlightSet;
  [Header("Cursors")]
  public Texture2D[] cursorNormal;
  public Texture2D[] cursorNeurax;
  public Texture2D[] cursorHorde;
  public Texture2D[] cursorReanimate;
  public Texture2D[] cursorApeHorde;
  public Texture2D[] cursorApeColonyHorde;
  public Texture2D[] cursorApeCreateColony;
  public Texture2D[] cursorApeRampage;
  public Texture2D[] cursorUnscheduledFlight;
  public Texture2D[] cursorNuclearStrike;
  public Texture2D[] cursorImmuneShock;
  public Texture2D[] cursorBenignMimic;
  public Texture2D[] cursorInfectBoost;
  public Texture2D[] cursorLethalBoost;
  public Texture2D[] cursorBloodRage;
  public Texture2D[] cursorVampTravel;
  public Texture2D[] cursorVampLair;
  public Texture2D[] cursorNormalCure;
  public Texture2D[] cursorQuarantine;
  public Texture2D[] cursorEconomicAid;
  public Texture2D[] cursorFieldOperatives;
  public ECursorMode cursorMode;
  [Header("Disease")]
  public MonoBehaviour[] diseaseCameraEffects;
  public DiseaseCameraController _diseaseCameraController_normal;
  public DiseaseCameraController _diseaseCameraController_cure;
  [Header("Misc")]
  public AuthorityIconController authorityIconControllerRef;
  public SplineLineApproximater splineRenderer;
  public int countryStateRandomChangeTurns = 5;
  public PortObject[] mpPortObjects;
  public Color zComBorderColor = (Color) new Color32((byte) 4, (byte) 73, (byte) 198, (byte) 200);
  public float zComBorderPulseSpeed = 1f;
  public Color apeRampageBorderColor = (Color) new Color32((byte) 200, (byte) 0, (byte) 0, (byte) 200);
  private Dictionary<string, Dictionary<string, RouteBoat>> mpRoutes = new Dictionary<string, Dictionary<string, RouteBoat>>();
  private List<ForcedBubble> forcedBubbles = new List<ForcedBubble>();
  [HideInInspector]
  public CountryView mpTargetBubbleStart;
  [HideInInspector]
  public BonusObject splineBubble;
  [HideInInspector]
  public GemObject movingGem;
  [HideInInspector]
  public Vector3 splineEndPosition;
  [HideInInspector]
  public Vector3 splineStartPosition;
  [HideInInspector]
  public NexusObject nexusObject;
  [HideInInspector]
  public NexusObject otherNexusObject;
  [HideInInspector]
  public BonusObject nukeBubble;
  internal Disease localPlayerDisease;
  private IGame mpGame;
  [NonSerialized]
  public List<BonusObject> mpBonuses = new List<BonusObject>();
  internal List<ScaleObject> mpScaleObjects = new List<ScaleObject>();
  [NonSerialized]
  public Dictionary<string, NexusObject> nexusMap = new Dictionary<string, NexusObject>();
  private Dictionary<string, FortObject> fortsMap = new Dictionary<string, FortObject>();
  private Dictionary<string, ApeLabObject> labMap = new Dictionary<string, ApeLabObject>();
  private Dictionary<string, ApeColonyObject> colonyMap = new Dictionary<string, ApeColonyObject>();
  private Dictionary<string, CoopLabObject> coopLabMap = new Dictionary<string, CoopLabObject>();
  private Dictionary<string, CastleObject> castleMap = new Dictionary<string, CastleObject>();
  private Dictionary<int, VampireObject> vampireMap = new Dictionary<int, VampireObject>();
  private BonusObject mpLastObjectClicked;
  private Texture2D[] cursorCurrent;
  private BonusObject mpUserBubble;
  private BonusObject mpP2UserBubble;
  private Country countryStateShowing;
  private Country lastWorldCountry;
  private int countryStateChangeCounter;
  private List<Country> countryStateChanged = new List<Country>();
  private int lastTechTotal;
  [HideInInspector]
  public DiseaseGeometry3D selectedDiseaseGeometry;
  public Vector3 mapHoverPoint;
  internal CountryView currentCountry;
  private List<ParticleSystem> activeParticleSystems = new List<ParticleSystem>();
  public float borderDistForCountrySelect = 0.285f;
  public float closedDestroyedFadedDelay = 5f;
  [Header("Gems")]
  public GemObject gemTemplate;
  private List<GemObject> gemObjects = new List<GemObject>();
  private Dictionary<GemEffect, GemObject> activeGemObjects = new Dictionary<GemEffect, GemObject>();
  [NonSerialized]
  public List<TimedObject> aaObjects = new List<TimedObject>();
  public bool mbBubbleClick;
  internal float gameTime;
  private List<UIDraggablePanel> dragPanels = new List<UIDraggablePanel>();
  public float dragSpeed = 1f;
  internal IDictionary<string, CountryView> countryMap = (IDictionary<string, CountryView>) new Dictionary<string, CountryView>();
  internal IDictionary<CountryView, List<CInterfaceManager.EdgeHelpers.Edge>> countryBoundaries = (IDictionary<CountryView, List<CInterfaceManager.EdgeHelpers.Edge>>) new Dictionary<CountryView, List<CInterfaceManager.EdgeHelpers.Edge>>();
  private CountryView selectedCountryView;
  private CountryView p2SelectedCountryView;
  private CountryView p2IntentSelectedCountryView;
  private int lastChangeTurn;
  private CountryView mouseDownCountryView;
  private BaseMapScreen mapScreen;
  public MultiplayerResultOverlay resultOverlay;
  private Texture2D currentCursorTexture;

  [HideInInspector]
  public DiseaseCameraController diseaseCameraController
  {
    get
    {
      return CGameManager.IsCureGame ? this._diseaseCameraController_cure : this._diseaseCameraController_normal;
    }
  }

  [HideInInspector]
  public Animation[] diseaseGeometryAnimations
  {
    get => this.diseaseCameraController.diseaseGeometryAnimations;
  }

  public CountryView SelectedCountryView
  {
    get => this.selectedCountryView;
    set
    {
      this.selectedCountryView = value;
      if (!CHUDScreen.instance.gameObject.activeSelf)
        return;
      CHUDScreen.instance.DisplayCurrentContext();
    }
  }

  public CountryView P2SelectedCountryView
  {
    get => this.p2SelectedCountryView;
    set => this.p2SelectedCountryView = value;
  }

  public CountryView P2IntentSelectedCountryView
  {
    get => this.p2IntentSelectedCountryView;
    set => this.p2IntentSelectedCountryView = value;
  }

  public CountryView HoverCountry { get; set; }

  public static bool ZombiesActive
  {
    get
    {
      Disease localPlayerDisease = CInterfaceManager.instance.localPlayerDisease;
      if (localPlayerDisease.diseaseType != Disease.EDiseaseType.NECROA)
        return false;
      return localPlayerDisease.zday || localPlayerDisease.zdayDone;
    }
  }

  public EHudTabMode TabMode { get; set; }

  private void Awake()
  {
    CInterfaceManager.instance = this;
    this.enabled = false;
  }

  public void Initialise()
  {
    this.cursorMode = ECursorMode.NORMAL;
    this.enabled = true;
    this.mpWorldCamera.enabled = true;
    this.countryStateChanged.Clear();
    CountryStateCamera.instance.UpdateCountryState((Country) null);
    this.SetScaleFactor(1f);
    this.TabMode = EHudTabMode.NORMAL;
    TutorialSystem.RegisterInterface((ITutorial) this);
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
      this.countryBoundaries[country.Value] = CInterfaceManager.EdgeHelpers.FindBoundary(CInterfaceManager.EdgeHelpers.GetEdges(country.Value.colliderMesh.mesh.triangles));
    this._diseaseCameraController_normal.Initialise();
    this._diseaseCameraController_cure.Initialise();
  }

  public void Cleanup()
  {
    CHUDScreen.instance.StopLoopingCureSound();
    this.StopAllCoroutines();
    VehicleObject[] objectsOfType1 = UnityEngine.Object.FindObjectsOfType(typeof (VehicleObject)) as VehicleObject[];
    for (int index = 0; index < objectsOfType1.Length; ++index)
    {
      objectsOfType1[index].Recycle();
      objectsOfType1[index].gameObject.SetActive(false);
    }
    ApeColonyObject[] objectsOfType2 = UnityEngine.Object.FindObjectsOfType(typeof (ApeColonyObject)) as ApeColonyObject[];
    for (int index = 0; index < objectsOfType2.Length; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) objectsOfType2[index]);
      objectsOfType2[index].Remove();
    }
    ApeLabObject[] objectsOfType3 = UnityEngine.Object.FindObjectsOfType(typeof (ApeLabObject)) as ApeLabObject[];
    for (int index = 0; index < objectsOfType3.Length; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) objectsOfType3[index]);
      objectsOfType3[index].Remove();
    }
    CoopLabObject[] objectsOfType4 = UnityEngine.Object.FindObjectsOfType(typeof (CoopLabObject)) as CoopLabObject[];
    for (int index = 0; index < objectsOfType4.Length; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) objectsOfType4[index]);
      objectsOfType4[index].Remove();
    }
    CastleObject[] objectsOfType5 = UnityEngine.Object.FindObjectsOfType(typeof (CastleObject)) as CastleObject[];
    for (int index = 0; index < objectsOfType5.Length; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) objectsOfType5[index]);
      objectsOfType5[index].Remove();
    }
    VampireObject[] objectsOfType6 = UnityEngine.Object.FindObjectsOfType(typeof (VampireObject)) as VampireObject[];
    for (int index = 0; index < objectsOfType6.Length; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) objectsOfType6[index]);
      objectsOfType6[index].Remove();
    }
    foreach (CountryView countryView in (IEnumerable<CountryView>) this.countryMap.Values)
    {
      countryView.SetSelected(CountryView.EOverlayState.OFF);
      countryView.Cleanup();
    }
    this.HoverCountry = (CountryView) null;
    this.SelectedCountryView = (CountryView) null;
    this.fortsMap.Clear();
    for (int index = 0; index < this.mpBonuses.Count; ++index)
    {
      this.mpScaleObjects.Remove((ScaleObject) this.mpBonuses[index]);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpBonuses[index].gameObject);
    }
    this.mpBonuses.Clear();
    this.selectedDiseaseGeometry = (DiseaseGeometry3D) null;
    for (int index = 0; index < this.gemObjects.Count; ++index)
    {
      GemObject gemObject = this.gemObjects[index];
      this.mpScaleObjects.Remove((ScaleObject) gemObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) gemObject.gameObject);
    }
    this.gemObjects.Clear();
    this.activeGemObjects.Clear();
    for (int index = 0; index < this.aaObjects.Count; ++index)
    {
      TimedObject aaObject = this.aaObjects[index];
      if ((UnityEngine.Object) aaObject != (UnityEngine.Object) null)
      {
        this.mpScaleObjects.Remove((ScaleObject) aaObject);
        if ((UnityEngine.Object) aaObject.gameObject != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) aaObject.gameObject);
      }
    }
    this.aaObjects.Clear();
    CHUDScreen.instance.CleanUp();
    this.countryStateChanged = new List<Country>();
    CountryStateCamera.instance.UpdateCountryState((Country) null);
    DiseaseTrailParticles.instance.Cleanup();
    this.SetCursorSelection(EHudMode.Normal);
    this.SetCursorMode(ECursorMode.NORMAL);
    this.colonyMap.Clear();
    this.labMap.Clear();
    this.coopLabMap.Clear();
    this.nexusMap.Clear();
    this.castleMap.Clear();
    this.vampireMap.Clear();
    this.localPlayerDisease = (Disease) null;
    this.SetHighlightMode(new Disease.EDiseaseType?());
    this._diseaseCameraController_normal.CleanUp();
    this._diseaseCameraController_cure.CleanUp();
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null))
      return;
    CGameManager.game.OnWorldUpdate -= new Action(this.UpdateInterface);
  }

  public void SetupOnlineGame()
  {
    this.gameTime = 0.0f;
    this.mpGame = CGameManager.game;
    if (!this.enabled)
      this.Initialise();
    CGSScreen screen = CUIManager.instance.GetScreen("GameSetupScreen") as CGSScreen;
    screen.DiseaseTypesUnlocked = CGameManager.localPlayerInfo.GetUnlockedDiseases();
    screen.AllGenes = this.mpGame.AvailableGenes;
    List<Gene> geneList = new List<Gene>();
    foreach (Gene availableGene in this.mpGame.AvailableGenes)
      geneList.Add(availableGene);
    screen.UnlockedGenes = geneList;
  }

  public void SetupOfflineGame()
  {
    this.gameTime = 0.0f;
    this.mpGame = CGameManager.game;
    if (!this.enabled)
      this.Initialise();
    CGSScreen screen1 = CUIManager.instance.GetScreen("GameSetupScreen") as CGSScreen;
    if (CGameManager.IsCureGame)
    {
      screen1 = CUIManager.instance.GetScreen("GameSetupScreen_Cure") as CGSScreen;
      screen1.DiseaseTypesUnlocked = CGameManager.localPlayerInfo.GetUnlockedCureDiseases();
    }
    else
      screen1.DiseaseTypesUnlocked = CGameManager.localPlayerInfo.GetUnlockedDiseases();
    screen1.AllGenes = this.mpGame.AvailableGenes;
    List<Gene> geneList = new List<Gene>();
    foreach (Gene allGene in screen1.AllGenes)
    {
      if (CGameManager.localPlayerInfo.GetGeneUnlocked(allGene))
        geneList.Add(allGene);
    }
    screen1.UnlockedGenes = geneList;
    CUIManager.instance.ClearHistory();
    IGameScreen screen2 = CUIManager.instance.GetScreen("MainMenuScreen");
    List<IGameSubScreen> subScreens = new List<IGameSubScreen>();
    subScreens.Add(screen2.GetSubScreen("Main_Sub_Main"));
    CUIManager.instance.SaveBreadcrumb(screen2, subScreens);
    subScreens.Clear();
    subScreens.Add(screen2.GetSubScreen("Main_Sub_NewGame"));
    CUIManager.instance.SaveBreadcrumb(screen2, subScreens);
    switch (CGameManager.gameType)
    {
      case IGame.GameType.Official:
        subScreens.Clear();
        subScreens.Add(screen2.GetSubScreen("Menu_Sub_Official"));
        CUIManager.instance.SaveBreadcrumb(screen2, subScreens);
        break;
      case IGame.GameType.Custom:
        subScreens.Clear();
        subScreens.Add(screen2.GetSubScreen("Menu_Sub_Custom"));
        CUIManager.instance.SaveBreadcrumb(screen2, subScreens);
        break;
    }
    CUIManager.instance.SetupScreens();
    CUIManager.instance.SetActiveScreen((IGameScreen) screen1);
  }

  public void InitialiseCountryViews()
  {
    foreach (KeyValuePair<string, CountryView> country1 in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
    {
      CountryView cv = country1.Value;
      cv.Initialise();
      Country country2 = cv.GetCountry();
      cv.mpAirport.gameObject.SetActive(country2.hasAirport);
      for (int index = 0; index < cv.mpSeaport.Count; ++index)
        cv.mpSeaport[index].gameObject.SetActive(country2.hasPorts);
      if (country2.fortState != EFortState.FORT_NONE)
      {
        this.SpawnFort(cv);
        if (country2.fortState == EFortState.FORT_DESTROYED)
          this.DestroyFort(country2);
      }
      if (country2.apeLabStatus != EApeLabState.APE_LAB_NONE)
        this.UpdateApeLab(cv, country2.apeLabStatus);
      if (country2.apeColonyStatus != EApeColonyState.APE_COLONY_NONE)
        this.UpdateApeColony(cv, country2.apeColonyStatus);
      for (int index = 0; index < World.instance.diseases.Count; ++index)
      {
        LocalDisease localDisease = country2.GetLocalDisease(World.instance.diseases[index]);
        if (localDisease.castleState != ECastleState.CASTLE_NONE)
          this.UpdateCastle(cv, localDisease.castleState);
      }
    }
    DiseaseTrailParticles.instance.Initialise();
    this.TabMode = EHudTabMode.NORMAL;
    this.SetHighlightMode(new Disease.EDiseaseType?());
    this.SetCountryHighlights();
    this.SelectedCountryView = (CountryView) null;
    this.HoverCountry = (CountryView) null;
  }

  public void MultiPlayerToSinglePlayer()
  {
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
    {
      InfectionParticleSystem infectionSystem = country.Value.GetInfectionSystem(false);
      if ((bool) (UnityEngine.Object) infectionSystem)
        infectionSystem.MultiPlayerToSinglePlayer();
    }
  }

  public void PulseDiseaseSpreadWaves(Disease d)
  {
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
      country.Value.PulseDiseaseSpreadWaves(d);
  }

  public void SetCountryHighlights()
  {
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
      this.SetCountryHighlight(country.Value, CountryView.EOverlayState.SELECTED);
  }

  public void SetCountryHighlight(CountryView cv)
  {
    this.SetCountryHighlight(cv, CountryView.EOverlayState.SELECTED);
  }

  public void SetCountryHighlight(CountryView cv, CountryView.EOverlayState overlayState)
  {
    if ((UnityEngine.Object) cv == (UnityEngine.Object) this.SelectedCountryView || (UnityEngine.Object) cv == (UnityEngine.Object) this.P2SelectedCountryView || (UnityEngine.Object) cv == (UnityEngine.Object) this.P2IntentSelectedCountryView)
    {
      cv.SetSelected(overlayState);
    }
    else
    {
      CountryView.EOverlayState state = CountryView.EOverlayState.OFF;
      if (CHUDScreen.instance.HudInterfaceMode == EHudMode.Normal)
      {
        if (this.TabMode == EHudTabMode.APES && this.IsValidApeState(cv))
          state = CountryView.EOverlayState.VALID;
      }
      else if (this.IsValidHoverState(CHUDScreen.instance.HudInterfaceMode, cv))
        state = (UnityEngine.Object) cv == (UnityEngine.Object) this.HoverCountry ? overlayState : CountryView.EOverlayState.VALID;
      cv.SetSelected(state);
    }
  }

  public bool IsValidApeState(CountryView cv)
  {
    Country country = cv.GetCountry();
    Disease disease = CGameManager.localPlayerInfo.disease;
    return country != null && disease != null && country.GetLocalDisease(disease).apeInfectedPopulation > 0L;
  }

  public bool IsValidHoverState(EHudMode hud, CountryView cvFrom, CountryView cvTo = null)
  {
    if ((UnityEngine.Object) cvFrom != (UnityEngine.Object) null)
    {
      Country country = cvFrom.GetCountry();
      Disease disease = CGameManager.localPlayerInfo.disease;
      if (country != null && disease != null)
      {
        LocalDisease localDisease1 = country.GetLocalDisease(disease);
        LocalDisease localDisease2 = (LocalDisease) null;
        if (World.instance.diseases.Count == 2)
          localDisease2 = country.GetLocalDisease(World.instance.diseases[World.instance.diseases[0] == disease ? 1 : 0]);
        switch (hud)
        {
          case EHudMode.Neurax:
          case EHudMode.SendHorde:
          case EHudMode.SendApeHorde:
          case EHudMode.SendApeColony:
            return (UnityEngine.Object) cvTo != (UnityEngine.Object) null;
          case EHudMode.Reanimate:
            return localDisease1.GetReanimateValue() > 0L;
          case EHudMode.SelectHorde:
            return localDisease1.zombie > 0L;
          case EHudMode.ApeRampage:
            return localDisease1.apeInfectedPopulation > 0L;
          case EHudMode.SelectApeHorde:
            return localDisease1.apeInfectedPopulation > 0L;
          case EHudMode.ApeCreateColony:
            return localDisease1.apeInfectedPopulation > 0L && !country.hasApeColony;
          case EHudMode.PlaceGem:
          case EHudMode.MoveGem:
            return (UnityEngine.Object) cvTo != (UnityEngine.Object) null;
          case EHudMode.SelectUnscheduledFlight:
            return CGameManager.IsCoopMPGame ? country.localDiseases.Where<LocalDisease>((Func<LocalDisease, bool>) (ld => ld.infectedPopulation > 1000L)).Count<LocalDisease>() > 0 : localDisease1.infectedPopulation > 1000L;
          case EHudMode.SendUnscheduledFlight:
            return (UnityEngine.Object) cvTo != (UnityEngine.Object) null;
          case EHudMode.ImmuneShock:
            return CGameManager.IsVersusMPGame && localDisease1.uncontrolledInfected + localDisease1.controlledInfected > 0L && ((MPLocalDisease) localDisease1).immuneShockCounter <= 0;
          case EHudMode.BenignMimic:
            return CGameManager.IsVersusMPGame && localDisease1.allInfected > 0L && localDisease1.allInfected + country.deadPopulation >= country.originalPopulation && ((MPLocalDisease) localDisease1).benignMimicCounter <= 0 && localDisease2 != null && ((MPLocalDisease) localDisease2).benignMimicCounter <= 0;
          case EHudMode.InfectBoost:
            return CGameManager.IsCoopMPGame && country.totalInfected > 0L && ((CoopLocalDisease) localDisease1).infectBoostCounter <= 0;
          case EHudMode.LethalBoost:
            return CGameManager.IsCoopMPGame && country.totalInfected > 0L && ((CoopLocalDisease) localDisease1).lethalBoostCounter <= 0;
          case EHudMode.NukeStrike:
            return (UnityEngine.Object) cvTo != (UnityEngine.Object) null && (UnityEngine.Object) cvTo != (UnityEngine.Object) cvFrom;
          case EHudMode.BloodRage:
            return localDisease1.zombie > 0L;
          case EHudMode.SelectVampire:
            return localDisease1.zombie > 0L;
          case EHudMode.SendVampire:
            return (UnityEngine.Object) cvTo != (UnityEngine.Object) null;
          case EHudMode.CreateCastle:
            return localDisease1.zombie > 0L && localDisease1.castleState == ECastleState.CASTLE_NONE;
          case EHudMode.SendInvestigationTeam:
            return !localDisease1.hasTeam && disease.vampires.Count > 0 && disease.vampires[0].currentCountry != null;
          case EHudMode.RaisePriority:
            return localDisease1.hasIntel && !localDisease1.lockdownAAActive && (double) localDisease1.infectedPercent > 0.0;
          case EHudMode.EconomicSupport:
            return localDisease1.hasIntel && (double) localDisease1.compliance < 1.0;
        }
      }
    }
    return false;
  }

  public void DroneAttackEffect(bool success, Vehicle drone)
  {
  }

  public void SetupGameHUD(bool isReplay = false)
  {
    this.lastWorldCountry = (Country) null;
    this.lastChangeTurn = -1;
    CUIManager.instance.ClearScreenSubstitutions();
    if (CGameManager.IsMultiplayerGame)
    {
      CUIManager.instance.AddScreenSubstitution("HUDScreen", "HUDScreenMP");
      CUIManager.instance.AddScreenSubstitution("CountryScreen", "CountryScreenMP");
      CUIManager.instance.AddScreenSubstitution("WorldScreen", "WorldScreenMP");
    }
    this.localPlayerDisease = CNetworkManager.network.LocalPlayerInfo.disease;
    if (this.localPlayerDisease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      CUIManager.instance.AddScreenSubstitution("WorldScreen", "WorldScreenApe");
      CUIManager.instance.AddScreenSubstitution("CountryScreen", "CountryScreenApe");
    }
    if (this.localPlayerDisease.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      CUIManager.instance.AddScreenSubstitution("WorldScreen", "WorldScreenVampire");
      CUIManager.instance.AddScreenSubstitution("CountryScreen", "CountryScreenVampire");
    }
    if (this.localPlayerDisease.diseaseType == Disease.EDiseaseType.CURE)
    {
      CUIManager.instance.AddScreenSubstitution("HUDScreen", "HUDScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("DiseaseScreen", "OutbreakScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("WorldScreen", "WorldScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("CountryScreen", "CountryScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("GraphScreen", "GraphScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("EndScreen", "EndScreen_Cure");
      CUIManager.instance.AddScreenSubstitution("PauseScreen", "PauseScreen_Cure");
    }
    CHUDScreen.SetInstance();
    CHUDScreen.instance.Setup();
    CHUDScreen.instance.SetContextType();
    this.SetHighlightMode(new Disease.EDiseaseType?());
    if (!isReplay)
      CUIManager.instance.SetActiveScreen("HUDScreen");
    Debug.Log((object) ("SETTING INITIAL SPREADWAVES: " + (object) World.instance.diseases.Count));
    for (int index = 0; index < World.instance.diseases.Count; ++index)
    {
      Disease disease = World.instance.diseases[index];
      CountryView country = this.countryMap[disease.nexus.id];
      country.AddSpreadWave(country.GetRandomPositionInsideCountry(), disease.id);
    }
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      if (this.countryMap.ContainsKey(country.id))
      {
        CountryView component = this.countryMap[country.id].gameObject.GetComponent<CountryView>();
        component.SetAirportState(true);
        component.SetSeaportState(true);
      }
    }
    this.diseaseCameraController.UpdateDiseaseGeometry();
    if ((UnityEngine.Object) this.selectedDiseaseGeometry == (UnityEngine.Object) null)
      Debug.LogError((object) "NULL DISEASE GEOMETRY!");
    else
      this.selectedDiseaseGeometry.SetDiseaseCreationProgress(1f);
    CHUDScreen.instance.SetStartDate(new DateTime(World.instance.startDate));
    if (World.instance.DiseaseTurn > 0)
      CHUDScreen.instance.SetDay(World.instance.DiseaseTurn);
    CGameManager.game.OnWorldUpdate += new Action(this.UpdateInterface);
  }

  public void SetHighlightMode(Disease.EDiseaseType? type)
  {
    if (type.HasValue)
    {
      for (int index = 0; index < this.AAHighlightSets.Count; ++index)
      {
        Disease.EDiseaseType? nullable = type;
        Disease.EDiseaseType diseaseType = this.AAHighlightSets[index].diseaseType;
        if (nullable.GetValueOrDefault() == diseaseType & nullable.HasValue)
        {
          this.currentAAHighlightSet = this.AAHighlightSets[index];
          break;
        }
      }
    }
    else
      this.currentAAHighlightSet = (CInterfaceManager.HighlightColourSet) null;
    if (this.currentAAHighlightSet != null)
      return;
    this.currentAAHighlightSet = this.defaultAAHighlightSet;
  }

  private void UpdateDiseaseGeometry() => this.diseaseCameraController.UpdateDiseaseGeometry();

  public void SetDiseaseGeometry(Disease.EDiseaseType type)
  {
    if (CGameManager.IsCureGame)
    {
      this._diseaseCameraController_cure.SetCameraActive(true);
      this._diseaseCameraController_normal.SetCameraActive(false);
    }
    else
    {
      this._diseaseCameraController_normal.SetCameraActive(true);
      this._diseaseCameraController_cure.SetCameraActive(false);
    }
    this.diseaseCameraController.SetDiseaseGeometry(type);
  }

  public void SetDiseaseCreationProgress(float f)
  {
    if (!((UnityEngine.Object) this.selectedDiseaseGeometry != (UnityEngine.Object) null))
      return;
    this.selectedDiseaseGeometry.SetDiseaseCreationProgress(f);
  }

  public void CountryStateChanged(Country c)
  {
    if (this.countryStateChanged.Contains(c))
      return;
    this.countryStateChanged.Add(c);
  }

  public void UpdateCountryState()
  {
    World instance = World.instance;
    if (instance == null)
      return;
    List<Country> countryList = instance.countries;
    if (CGameManager.IsCureGame && !CGameManager.game.IsReplayActive)
    {
      countryList = new List<Country>();
      foreach (Country country in instance.countries)
      {
        if (country.GetLocalDisease(this.localPlayerDisease).hasIntel)
          countryList.Add(country);
      }
      if (countryList == null)
      {
        Debug.LogError((object) "Error in CInterfaceManager.UpdateCountryState: 'countryList' was null");
        return;
      }
      if (countryList.Count == 0)
      {
        Debug.LogError((object) "Error in CInterfaceManager.UpdateCountryState: 'countryList' should at least have one element in it");
        return;
      }
    }
    if ((UnityEngine.Object) this.SelectedCountryView != (UnityEngine.Object) null)
      this.countryStateShowing = this.SelectedCountryView.GetCountry();
    else if (instance.DiseaseTurn > this.lastChangeTurn + this.countryStateRandomChangeTurns || this.lastWorldCountry == null)
    {
      this.lastChangeTurn = instance.DiseaseTurn;
      this.countryStateChangeCounter = this.countryStateRandomChangeTurns;
      if (this.countryStateChanged.Count > 0)
      {
        this.countryStateShowing = this.countryStateChanged[0];
        this.countryStateChanged.Remove(this.countryStateShowing);
      }
      else if (countryList.Count > 0)
        this.countryStateShowing = countryList[CUtils.IntRand(0, countryList.Count - 1)];
      this.lastWorldCountry = this.countryStateShowing;
    }
    else
      this.countryStateShowing = this.lastWorldCountry;
    if (this.countryStateShowing == null && countryList.Count > 0)
      this.countryStateShowing = countryList[CUtils.IntRand(0, countryList.Count - 1)];
    CountryStateCamera.instance.UpdateCountryState(this.countryStateShowing);
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.mpGame == (UnityEngine.Object) null)
      return;
    this.gameTime += Time.deltaTime * (float) this.mpGame.ActualSpeed;
    Shader.SetGlobalFloat("_GameTime", this.gameTime);
    CSteamControllerManager instance = CSteamControllerManager.instance;
    if ((UnityEngine.Object) CHUDScreen.instance != (UnityEngine.Object) null && CHUDScreen.instance.HudInterfaceMode == EHudMode.Normal)
    {
      if (Input.GetMouseButton(0) || instance.GetAction(instance.currentSelectAction))
        CInterfaceManager.instance.SetCursorMode(ECursorMode.CLICK);
      else if (CUIManager.instance.IsHovering())
        CInterfaceManager.instance.SetCursorMode(ECursorMode.HOVER);
      else
        CInterfaceManager.instance.SetCursorMode(ECursorMode.NORMAL);
    }
    bool actionDown = instance.GetActionDown(instance.currentSelectAction);
    if (actionDown)
    {
      if ((UnityEngine.Object) this.mpUserBubble != (UnityEngine.Object) null && this.mpUserBubble.hovered)
      {
        this.mpUserBubble.OnMouseDown();
      }
      else
      {
        for (int index = 0; index < this.mpBonuses.Count; ++index)
        {
          if (this.mpBonuses[index].hovered)
          {
            this.mpBonuses[index].OnMouseDown();
            break;
          }
        }
      }
    }
    if (CUIManager.instance.IsHovering())
    {
      this.StopSpline();
    }
    else
    {
      this.GetHoverCountry();
      if ((UnityEngine.Object) this.currentCountry == (UnityEngine.Object) null)
        this.mouseDownCountryView = (CountryView) null;
      if ((UnityEngine.Object) CUIManager.instance.GetCurrentScreen() != (UnityEngine.Object) null && !DevCheats.cheatsEnabled)
      {
        if (CGameManager.IsTutorialGame)
        {
          switch (CGameManager.gameType)
          {
            case IGame.GameType.Tutorial:
              if (!TutorialSystem.IsModuleComplete("3B") && ((UnityEngine.Object) this.currentCountry == (UnityEngine.Object) null || this.currentCountry.name != "china"))
                return;
              break;
            case IGame.GameType.CureTutorial:
              if (!TutorialSystem.IsModuleComplete("C2") && ((UnityEngine.Object) this.currentCountry == (UnityEngine.Object) null || this.currentCountry.name != "canada"))
                return;
              break;
          }
        }
        this.mapScreen = CUIManager.instance.GetCurrentScreen() as BaseMapScreen;
        if ((UnityEngine.Object) this.mapScreen != (UnityEngine.Object) null)
        {
          this.mapScreen.OnCountryHover(this.currentCountry, this.mapHoverPoint);
          if (Input.GetMouseButtonDown(0) | actionDown)
          {
            if ((UnityEngine.Object) this.currentCountry == (UnityEngine.Object) null)
            {
              Vector3 zero = Vector3.zero;
              this.currentCountry = this.GetNearestCountryViewToPoint(this.mapHoverPoint, this.borderDistForCountrySelect, ref zero);
            }
            this.mouseDownCountryView = this.currentCountry;
          }
          if (!this.mbBubbleClick && (Input.GetMouseButtonUp(0) || instance.GetActionUp(instance.currentSelectAction)))
          {
            Vector3 mapHoverPoint = this.mapHoverPoint;
            if ((UnityEngine.Object) this.currentCountry == (UnityEngine.Object) null)
              this.currentCountry = this.GetNearestCountryViewToPoint(this.mapHoverPoint, this.borderDistForCountrySelect, ref mapHoverPoint);
            this.mapScreen.OnCountryClick(this.currentCountry, mapHoverPoint, (UnityEngine.Object) this.mouseDownCountryView == (UnityEngine.Object) this.currentCountry);
            this.mouseDownCountryView = (CountryView) null;
          }
        }
      }
      if (!Input.GetMouseButtonUp(0) && !instance.GetActionUp(instance.currentSelectAction))
        return;
      this.mbBubbleClick = false;
    }
  }

  public CountryView GetNearestCountryViewToPoint(
    Vector3 point,
    float searchRadius,
    ref Vector3 closestHitPoint)
  {
    point.z = 0.0f;
    CountryView countryViewToPoint = (CountryView) null;
    float num1 = float.MaxValue;
    float num2 = searchRadius * searchRadius;
    foreach (KeyValuePair<CountryView, List<CInterfaceManager.EdgeHelpers.Edge>> countryBoundary in (IEnumerable<KeyValuePair<CountryView, List<CInterfaceManager.EdgeHelpers.Edge>>>) this.countryBoundaries)
    {
      CountryView key = countryBoundary.Key;
      Mesh mesh = key.colliderMesh.mesh;
      if ((double) mesh.bounds.SqrDistance(key.transform.InverseTransformPoint(point)) <= (double) num2)
      {
        foreach (CInterfaceManager.EdgeHelpers.Edge edge in countryBoundary.Value)
        {
          Vector3 vector3 = key.transform.TransformPoint(mesh.vertices[edge.v1]);
          float sqrMagnitude = (vector3 - point).sqrMagnitude;
          if ((double) sqrMagnitude <= (double) num2 && (double) sqrMagnitude < (double) num1)
          {
            num1 = sqrMagnitude;
            countryViewToPoint = key;
            closestHitPoint = vector3;
          }
        }
      }
    }
    return countryViewToPoint;
  }

  public CountryView GetCountryView(string id)
  {
    return this.countryMap.ContainsKey(id) ? this.countryMap[id] : (CountryView) null;
  }

  public void AddDragablePanel(UIDraggablePanel drag)
  {
    drag.scrollWheelFactor = this.dragSpeed;
    this.dragPanels.Add(drag);
  }

  public void AddCountryLink(string name, CountryView view)
  {
    if ((UnityEngine.Object) view == (UnityEngine.Object) null)
      Debug.Log((object) (name + " is null"));
    this.countryMap[name] = view;
    for (int index = 0; index < view.mpSeaport.Count; ++index)
      this.mpScaleObjects.Add(view.mpSeaport[index].GetComponent<ScaleObject>());
    this.mpScaleObjects.Add(view.mpAirport.GetComponent<ScaleObject>());
  }

  public void AddRoute(string source, string dest, RouteBoat pRoute)
  {
    if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
      return;
    if (!this.mpRoutes.ContainsKey(source))
      this.mpRoutes[source] = new Dictionary<string, RouteBoat>();
    this.mpRoutes[source][dest] = pRoute;
  }

  public RouteBoat GetRoute(string source, string dest)
  {
    return this.mpRoutes.ContainsKey(source) && this.mpRoutes[source].ContainsKey(dest) ? this.mpRoutes[source][dest] : (RouteBoat) null;
  }

  public void UpdateInterface()
  {
    if (!(bool) (UnityEngine.Object) CUIManager.instance)
      return;
    this.diseaseCameraController.UpdateDiseaseGeometry();
    this.UpdateCountryState();
    IGameScreen currentScreen = CUIManager.instance.GetCurrentScreen();
    if (!(bool) (UnityEngine.Object) currentScreen)
      return;
    currentScreen.Refresh();
  }

  public void PostEventUpdate()
  {
    if (this.localPlayerDisease == null || !this.localPlayerDisease.mutatedThisTurn)
      return;
    this.localPlayerDisease.mutatedThisTurn = false;
    if (CGameManager.game.IsReplayActive)
      return;
    CSoundManager.instance.PlaySFX("autoevolve");
    CDiseaseScreen screen = CUIManager.instance.GetScreen("DiseaseScreen") as CDiseaseScreen;
    if (!((UnityEngine.Object) screen.bodySubScreen != (UnityEngine.Object) null))
      return;
    screen.bodySubScreen.ForceRefresh();
  }

  public Texture2D GetWorldScreenshot()
  {
    Camera component = this.mpWorldCamera.GetComponent<Camera>();
    int width = 338;
    int height = 170;
    Texture2D worldScreenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
    RenderTexture temporary = RenderTexture.GetTemporary(width, height, 24);
    float fieldOfView = component.fieldOfView;
    float aspect = component.aspect;
    component.targetTexture = temporary;
    component.aspect = 1.98823524f;
    component.Render();
    component.targetTexture = (RenderTexture) null;
    component.aspect = aspect;
    component.fieldOfView = fieldOfView;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = temporary;
    worldScreenshot.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
    worldScreenshot.Apply();
    RenderTexture.active = active;
    return worldScreenshot;
  }

  public void AddNews(List<IGame.NewsItem> news)
  {
    if (news.Count <= 0)
      return;
    CHUDScreen.instance.AddNews(news);
  }

  public void QueuePopups(List<PopupMessage> messages)
  {
    bool isMultiplayerGame = CGameManager.IsMultiplayerGame;
    foreach (PopupMessage message in messages)
    {
      if (!isMultiplayerGame && !CGameManager.spaceTime)
      {
        CGameManager.pausePopup = true;
        CGameManager.SetPaused(true);
      }
      CHUDScreen.instance.ShowPopup(message);
    }
  }

  public void SpawnVehicle(Vehicle vehicle)
  {
    if (!(bool) (UnityEngine.Object) this.mpGame)
    {
      Debug.LogWarning((object) "Trying to spawn when game is invalid");
    }
    else
    {
      CountryView source = (CountryView) null;
      CountryView destination = (CountryView) null;
      if (vehicle.source == null)
        Debug.LogError((object) ("Weird vehicle - no source! " + (object) vehicle.type + " / " + (object) vehicle.subType + " source: " + (object) vehicle.source + " dest: " + (object) vehicle.destination));
      if (this.countryMap.ContainsKey(vehicle.source.id))
        source = this.countryMap[vehicle.source.id];
      if (this.countryMap.ContainsKey(vehicle.destination.id))
        destination = this.countryMap[vehicle.destination.id];
      if (vehicle.type == Vehicle.EVehicleType.Airplane && vehicle.subType == Vehicle.EVehicleSubType.Neurax)
        CSoundManager.instance.PlaySFX("neuraxrelease");
      if (vehicle.type == Vehicle.EVehicleType.Airplane)
      {
        int subType = (int) vehicle.subType;
      }
      if (vehicle.subType == Vehicle.EVehicleSubType.Neurax || vehicle.type == Vehicle.EVehicleType.ZombieHorde)
      {
        VehicleObject vehicle1 = VehicleObject.CreateVehicle(vehicle);
        vehicle1.transform.parent = this.transform;
        if (vehicle.subType == Vehicle.EVehicleSubType.VampireHordeFast || vehicle.subType == Vehicle.EVehicleSubType.VampireHordeSlow)
          vehicle1.InitialiseTargeted(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle), new Vector3?(vehicle.sourcePosition.Value), new Vector3?(vehicle.destinationPosition.Value));
        else if (vehicle.subType == Vehicle.EVehicleSubType.ApeHorde || vehicle.subType == Vehicle.EVehicleSubType.ApeHordeNoColony)
          vehicle1.InitialiseTargeted(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle), new Vector3?(vehicle.sourcePosition.Value), new Vector3?(vehicle.destinationPosition.Value));
        else if (this.mpGame.IsReplayActive)
          vehicle1.Initialise(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle));
        else
          vehicle1.InitialiseTargeted(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle), new Vector3?(vehicle.sourcePosition.Value), new Vector3?(vehicle.destinationPosition.Value));
        vehicle1.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle1);
      }
      else if (vehicle.subType == Vehicle.EVehicleSubType.Fort || vehicle.subType == Vehicle.EVehicleSubType.FortEscapees || vehicle.subType == Vehicle.EVehicleSubType.MiniFort)
      {
        VehicleObject vehicle2 = VehicleObject.CreateVehicle(vehicle);
        vehicle2.transform.parent = this.transform;
        Vector3 vector3_1 = (UnityEngine.Object) source.mpFortLocation == (UnityEngine.Object) null ? source.transform.position : source.mpFortLocation.transform.position;
        Vector3 vector3_2 = (UnityEngine.Object) destination.mpFortLocation == (UnityEngine.Object) null ? destination.transform.position : destination.mpFortLocation.transform.position;
        vehicle2.InitialiseTargeted(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle), new Vector3?(vector3_1), new Vector3?(vector3_2));
        vehicle2.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle2);
      }
      else if (vehicle.type == Vehicle.EVehicleType.TargetingDrone)
      {
        VehicleObject vehicle3 = VehicleObject.CreateVehicle(vehicle);
        vehicle3.transform.parent = this.transform;
        vehicle3.Initialise(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle));
        vehicle3.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle3);
        this.mpGame.DroneSetSpawnPosition(vehicle);
      }
      else if (vehicle.type == Vehicle.EVehicleType.Drone)
      {
        VehicleObject vehicle4 = VehicleObject.CreateVehicle(vehicle);
        vehicle4.transform.parent = this.transform;
        vehicle4.Initialise(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle));
        vehicle4.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle4);
        this.mpGame.DroneSetSpawnPosition(vehicle);
      }
      else if (vehicle.type == Vehicle.EVehicleType.Missile && vehicle.subType == Vehicle.EVehicleSubType.Nuke)
      {
        VehicleObject vehicle5 = VehicleObject.CreateVehicle(vehicle);
        vehicle5.transform.parent = this.transform;
        vehicle5.InitialiseTargeted(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle), vehicle.sourcePosition, vehicle.destinationPosition);
        vehicle5.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle5);
      }
      else if ((UnityEngine.Object) source != (UnityEngine.Object) null && (UnityEngine.Object) destination != (UnityEngine.Object) null)
      {
        VehicleObject vehicle6 = VehicleObject.CreateVehicle(vehicle);
        vehicle6.transform.parent = this.transform;
        vehicle6.Initialise(vehicle, source, destination, this.GetVehicleColor(vehicle), this.GetVehicleColorRef(vehicle));
        vehicle6.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        this.mpScaleObjects.Add((ScaleObject) vehicle6);
      }
      else
        Debug.Log((object) ("Vehicle route failed - Source: " + (object) source + " (" + vehicle.source.id + ") Dest: " + (object) destination + " (" + vehicle.destination.id + ")"));
    }
  }

  public VehicleObject GetVehiclePrefab(Vehicle v)
  {
    switch (v.type)
    {
      case Vehicle.EVehicleType.Airplane:
        switch (v.subType)
        {
          case Vehicle.EVehicleSubType.Fort:
          case Vehicle.EVehicleSubType.MiniFort:
          case Vehicle.EVehicleSubType.FortEscapees:
            return (VehicleObject) this.zcomPlanePrefab;
          default:
            return (VehicleObject) this.planePrefab[UnityEngine.Random.Range(0, this.planePrefab.Length)];
        }
      case Vehicle.EVehicleType.Boat:
        return (VehicleObject) this.boatPrefab[UnityEngine.Random.Range(0, this.boatPrefab.Length)];
      case Vehicle.EVehicleType.ZombieHorde:
        switch (v.subType)
        {
          case Vehicle.EVehicleSubType.ApeHorde:
          case Vehicle.EVehicleSubType.ApeHordeNoColony:
            return (VehicleObject) this.apeHordePrefab;
          case Vehicle.EVehicleSubType.VampireHordeFast:
          case Vehicle.EVehicleSubType.VampireHordeSlow:
            return (VehicleObject) this.vampireHordePrefab;
          case Vehicle.EVehicleSubType.CureInvestigate:
            return (VehicleObject) this.responseTeamPlanePrefab;
          default:
            return (VehicleObject) this.hordePrefab;
        }
      case Vehicle.EVehicleType.Drone:
        return (VehicleObject) this.dronePrefab;
      case Vehicle.EVehicleType.Missile:
        return (VehicleObject) this.missilePrefab;
      case Vehicle.EVehicleType.TargetingDrone:
        return (VehicleObject) this.targetingDronePrefab;
      case Vehicle.EVehicleType.Helicopter:
        return this.helicopterPrefab;
      default:
        return (VehicleObject) null;
    }
  }

  public void RemoveVehicle(Vehicle vehicle)
  {
    for (int index = 0; index < this.mpScaleObjects.Count; ++index)
    {
      if (this.mpScaleObjects[index] is VehicleObject)
      {
        VehicleObject mpScaleObject = this.mpScaleObjects[index] as VehicleObject;
        if (mpScaleObject.mpVehicle == vehicle)
        {
          mpScaleObject.Recycle();
          break;
        }
      }
    }
  }

  public void ForceSpawnVehicles(List<Vehicle> vehicles)
  {
    for (int index = 0; index < vehicles.Count; ++index)
      this.SpawnVehicle(vehicles[index]);
  }

  public void SpawnVehicles(List<Vehicle> vehicles)
  {
    this.StartCoroutine(this.DoSpawnVehicles(vehicles));
  }

  private IEnumerator DoSpawnVehicles(List<Vehicle> vehicles)
  {
    while (vehicles.Count > 0)
    {
      if ((UnityEngine.Object) this.mpGame != (UnityEngine.Object) null && this.mpGame.ActualSpeed > 0)
      {
        for (int index = 0; index < vehicles.Count; ++index)
        {
          if (vehicles[index].subType == Vehicle.EVehicleSubType.Cure || vehicles[index].subType == Vehicle.EVehicleSubType.Fort || vehicles[index].subType == Vehicle.EVehicleSubType.MiniFort || vehicles[index].type == Vehicle.EVehicleType.ZombieHorde)
            vehicles[index].delay = 0.0f;
          else
            vehicles[index].delay -= this.mpGame.DeltaGameTime;
          if ((double) vehicles[index].delay <= 0.0)
          {
            vehicles[index].delay = 0.0f;
            this.SpawnVehicle(vehicles[index]);
          }
        }
        vehicles.RemoveAll((Predicate<Vehicle>) (a => (double) a.delay <= 0.0));
      }
      else if ((UnityEngine.Object) this.mpGame == (UnityEngine.Object) null)
        break;
      yield return (object) null;
    }
  }

  public void RemoveScaleObject(ScaleObject so) => this.mpScaleObjects.Remove(so);

  public void UpdateApeLabs(List<Country> countries)
  {
    for (int index = 0; index < countries.Count; ++index)
      this.UpdateApeLab(this.GetCountryView(countries[index].id), countries[index].apeLabStatus);
  }

  public void UpdateCoopLabs(List<Country> countries)
  {
    for (int index = 0; index < countries.Count; ++index)
      this.UpdateCoopLab(this.GetCountryView(countries[index].id), countries[index].coopLabStatus, countries[index].coopLabDiseaseId);
  }

  private void UpdateApeLab(CountryView cv, EApeLabState state)
  {
    bool flag = CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.VAMPIRE;
    if (this.labMap.ContainsKey(cv.countryID) && (UnityEngine.Object) this.labMap[cv.countryID] != (UnityEngine.Object) null)
    {
      this.labMap[cv.countryID].SetState(state);
      if (state != EApeLabState.APE_LAB_DESTROYED)
        return;
      CSoundManager.instance.PlaySFX(flag ? "vampires_lab_destroyed" : "lab_destroyed");
    }
    else
    {
      if (state == EApeLabState.APE_LAB_NONE)
        return;
      ApeLabObject apeLabObject = UnityEngine.Object.Instantiate<ApeLabObject>(flag ? this.vampLabPrefab : this.labPrefab);
      apeLabObject.InitialiseLab(state, cv);
      this.labMap[cv.countryID] = apeLabObject;
      this.mpScaleObjects.Add((ScaleObject) apeLabObject);
      apeLabObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
      if (state == EApeLabState.APE_LAB_DESTROYED)
        CSoundManager.instance.PlaySFX(flag ? "vampires_lab_destroyed" : "lab_destroyed");
      else
        CSoundManager.instance.PlaySFX("lab_created");
    }
  }

  private void UpdateCoopLab(CountryView cv, ECoopLabState state, int diseaseId)
  {
    if (this.coopLabMap.ContainsKey(cv.countryID) && (UnityEngine.Object) this.coopLabMap[cv.countryID] != (UnityEngine.Object) null)
    {
      this.coopLabMap[cv.countryID].SetState(state);
      if (state != ECoopLabState.COOP_LAB_DESTROYED)
        return;
      CSoundManager.instance.PlaySFX("vampires_lab_destroyed");
    }
    else
    {
      if (state == ECoopLabState.COOP_LAB_NONE)
        return;
      CoopLabObject coopLabObject = UnityEngine.Object.Instantiate<CoopLabObject>(this.coopLabPrefab);
      coopLabObject.InitialiseLab(diseaseId, state, cv);
      this.coopLabMap[cv.countryID] = coopLabObject;
      this.mpScaleObjects.Add((ScaleObject) coopLabObject);
      coopLabObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
      if (state == ECoopLabState.COOP_LAB_DESTROYED)
        CSoundManager.instance.PlaySFX("vampires_lab_destroyed");
      else
        CSoundManager.instance.PlaySFX("lab_created");
    }
  }

  public void UpdateVampires()
  {
    for (int index1 = 0; index1 < World.instance.diseases.Count; ++index1)
    {
      Disease disease = World.instance.diseases[index1];
      for (int index2 = 0; index2 < disease.vampires.Count; ++index2)
        this.UpdateVampire(disease.vampires[index2]);
    }
    for (int index = 0; index < World.instance.destroyedVampires.Count; ++index)
      this.UpdateVampire(World.instance.destroyedVampires[index]);
    World.instance.destroyedVampires.Clear();
  }

  private void UpdateVampire(Vampire v)
  {
    if (this.vampireMap.ContainsKey(v.id) && (UnityEngine.Object) this.vampireMap[v.id] != (UnityEngine.Object) null)
      this.vampireMap[v.id].UpdateVampire();
    else if (v.vampireType == Vampire.VampireType.VT_TEAM)
    {
      VampireObject vampireObject = UnityEngine.Object.Instantiate<VampireObject>(this.fieldTeamPrefab);
      vampireObject.Initialise(v);
      this.vampireMap[v.id] = vampireObject;
      this.mpScaleObjects.Add((ScaleObject) vampireObject);
      vampireObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
    else
    {
      VampireObject vampireObject = UnityEngine.Object.Instantiate<VampireObject>(this.vampirePrefab);
      vampireObject.Initialise(v);
      this.vampireMap[v.id] = vampireObject;
      this.mpScaleObjects.Add((ScaleObject) vampireObject);
      vampireObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
  }

  public void UpdateApeColonies(List<Country> countries)
  {
    for (int index = 0; index < countries.Count; ++index)
      this.UpdateApeColony(this.GetCountryView(countries[index].id), countries[index].apeColonyStatus);
  }

  private void UpdateApeColony(CountryView cv, EApeColonyState state)
  {
    if (state == EApeColonyState.APE_COLONY_ALIVE)
      CSoundManager.instance.PlaySFX("ape_colony_created");
    if (this.colonyMap.ContainsKey(cv.countryID) && (UnityEngine.Object) this.colonyMap[cv.countryID] != (UnityEngine.Object) null)
    {
      this.colonyMap[cv.countryID].SetState(state);
      cv.UpdateColonyPosition(this.colonyMap[cv.countryID]);
    }
    else
    {
      if (state == EApeColonyState.APE_COLONY_NONE)
        return;
      ApeColonyObject apeColonyObject = UnityEngine.Object.Instantiate<ApeColonyObject>(this.colonyPrefab);
      apeColonyObject.Initialise(state, cv);
      this.colonyMap[cv.countryID] = apeColonyObject;
      this.mpScaleObjects.Add((ScaleObject) apeColonyObject);
      apeColonyObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
  }

  public void UpdateCastles(List<LocalDisease> localDiseases)
  {
    for (int index = 0; index < localDiseases.Count; ++index)
      this.UpdateCastle(this.GetCountryView(localDiseases[index].country.id), localDiseases[index].castleState);
  }

  private void UpdateCastle(CountryView cv, ECastleState state)
  {
    if (this.castleMap.ContainsKey(cv.countryID) && (UnityEngine.Object) this.castleMap[cv.countryID] != (UnityEngine.Object) null)
    {
      this.castleMap[cv.countryID].SetState(state);
      cv.UpdateCastlePosition(this.castleMap[cv.countryID]);
    }
    else
    {
      if (state == ECastleState.CASTLE_NONE)
        return;
      CastleObject castleObject = UnityEngine.Object.Instantiate<CastleObject>(this.castlePrefab);
      castleObject.Initialise(state, cv);
      this.castleMap[cv.countryID] = castleObject;
      this.mpScaleObjects.Add((ScaleObject) castleObject);
      castleObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
  }

  public void SpawnAndUpdateNexus(Disease disease, Country country)
  {
    CountryView countryView = this.GetCountryView(country.id);
    if (this.nexusMap != null && !this.nexusMap.ContainsKey(countryView.countryID))
    {
      NexusObject nexusObject;
      if (disease == CGameManager.localPlayerInfo.disease)
      {
        this.nexusObject = UnityEngine.Object.Instantiate<NexusObject>(this.nexusPrefab);
        nexusObject = this.nexusObject;
      }
      else
      {
        this.otherNexusObject = UnityEngine.Object.Instantiate<NexusObject>(this.otherNexusPrefab);
        nexusObject = this.otherNexusObject;
      }
      nexusObject.Initialise(disease, countryView, disease == CGameManager.localPlayerInfo.disease || CGameManager.game.IsReplayActive || CGameManager.gameType == IGame.GameType.CoopMP);
      this.nexusMap[countryView.countryID] = nexusObject;
      this.mpScaleObjects.Add((ScaleObject) nexusObject);
      nexusObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
    else
    {
      if (this.nexusMap[countryView.countryID].disease == disease || CGameManager.gameType != IGame.GameType.VersusMP)
        return;
      this.SpawnNexusFoundBubble(disease, country);
    }
  }

  public void SetNexusFound(Country country) => this.nexusMap[country.id].SetFound();

  public void SpawnNexusFoundBubble(Disease disease, Country country)
  {
    if (!this.nexusMap.ContainsKey(country.id))
      return;
    CountryView countryView = this.GetCountryView(country.id);
    World.instance.AddBonusIcon(new BonusIcon(disease, country, BonusIcon.EBonusIconType.NEXUS_FOUND)
    {
      disableAutoHide = true,
      position = countryView.mpNexusLocation.transform.position
    });
  }

  public void SpawnNexusDNABubble(Disease disease, Country country)
  {
    if (!this.nexusMap.ContainsKey(country.id))
      return;
    NexusObject nexus = this.nexusMap[country.id];
    CountryView countryView = this.GetCountryView(country.id);
    if (nexus.nexusStatus != NexusStatus.Hidden)
      return;
    BonusIcon bonusIcon = new BonusIcon(disease, country, BonusIcon.EBonusIconType.NEXUS_DNA);
    this.AddForcedPositionBubble(country, countryView.transform.position, BonusIcon.EBonusIconType.NEXUS_DNA);
    World.instance.AddBonusIcon(bonusIcon);
    if (disease != CGameManager.localPlayerInfo.disease)
      return;
    this.nexusObject.animator.SetTrigger("spawn");
  }

  public void DestroyNexus(Country country)
  {
    if (!this.nexusMap.ContainsKey(country.id))
      return;
    this.nexusMap[country.id].DestroyNexus();
  }

  public void SpawnForts(Country[] countries)
  {
    for (int index = 0; index < countries.Length; ++index)
      this.SpawnFort(this.GetCountryView(countries[index].id));
    if (countries.Length == 0 || World.instance.diseases[0].diseaseType != Disease.EDiseaseType.VAMPIRE || !(bool) (UnityEngine.Object) CSoundManager.instance)
      return;
    CSoundManager.instance.PlaySFX("vampire_fort_create");
  }

  private void SpawnFort(CountryView cv)
  {
    Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "] - Spawning Fort: " + cv.name));
    if (World.instance.diseases[0].diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      FortObject fortObject = UnityEngine.Object.Instantiate<FortObject>(this.templarFortPrefab);
      fortObject.Initialise(true, cv);
      this.fortsMap.Add(cv.countryID, fortObject);
      this.mpScaleObjects.Add((ScaleObject) fortObject);
      fortObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
    else
    {
      FortObject fortObject = UnityEngine.Object.Instantiate<FortObject>(this.zcomPrefab);
      fortObject.Initialise(true, cv);
      this.fortsMap.Add(cv.countryID, fortObject);
      this.mpScaleObjects.Add((ScaleObject) fortObject);
      fortObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
  }

  public void DestroyForts(Country[] countries)
  {
    for (int index = 0; index < countries.Length; ++index)
      this.DestroyFort(countries[index]);
    if (countries.Length == 0 || World.instance.diseases[0].diseaseType != Disease.EDiseaseType.VAMPIRE || !(bool) (UnityEngine.Object) CSoundManager.instance)
      return;
    CSoundManager.instance.PlaySFX("vampire_fort_destroyed");
  }

  private void DestroyFort(Country inCountry)
  {
    if (!this.fortsMap.ContainsKey(inCountry.id))
      return;
    Debug.Log((object) ("Destroying Fort: " + inCountry.id));
    this.fortsMap[inCountry.id].SetState(false);
  }

  public void SpawnBonuses(List<BonusIcon> bonuses)
  {
    for (int index = 0; index < bonuses.Count; ++index)
    {
      BonusIcon bonuse = bonuses[index];
      if (!CGameManager.IsMultiplayerGame || !CGameManager.IsAIGame || bonuse.disease == CGameManager.game.GetMyDisease())
      {
        if (!this.countryMap.ContainsKey(bonuse.country.id))
        {
          Debug.Log((object) (bonuse.country.id + "not found"));
          break;
        }
        this.StartCoroutine(this.SpawnBonus(bonuses[index]));
      }
    }
  }

  public void AddForcedPositionBubble(
    Country country,
    Vector3 position,
    BonusIcon.EBonusIconType bonusIconType)
  {
    if (bonusIconType == BonusIcon.EBonusIconType.INFECT && country.totalInfected > 0L)
      return;
    this.forcedBubbles.Add(new ForcedBubble()
    {
      type = bonusIconType,
      country = country,
      position = position
    });
  }

  public void SetUserBubble(BonusObject bonusObject)
  {
    Debug.Log((object) ("SetUserBubble - bonusObject:" + (object) bonusObject));
    if ((UnityEngine.Object) this.mpUserBubble != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpUserBubble.gameObject);
    this.mpUserBubble = bonusObject;
  }

  public void SetP2UserBubble(BonusObject bonusObject)
  {
    Debug.Log((object) ("SetP2UserBubble - bonusObject:" + (object) bonusObject));
    if ((UnityEngine.Object) this.mpP2UserBubble != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpP2UserBubble.gameObject);
    this.mpP2UserBubble = bonusObject;
  }

  public void CreateImmuneShockObject(Vector3 pos, bool myShock, CountryView cv, float life)
  {
    TimedObject timedObject = UnityEngine.Object.Instantiate<TimedObject>(myShock ? this.templateMyImmuneShockObject : this.templateTheirImmuneShockObject);
    timedObject.transform.parent = this.transform;
    timedObject.transform.position = pos;
    timedObject.SetTime(life, new Action<TimedObject>(this.RecycleTimedObject));
    timedObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    timedObject.countryView = cv;
    timedObject.abiltyType = EAbilityType.immune_shock;
    this.mpScaleObjects.Add((ScaleObject) timedObject);
    this.aaObjects.Add(timedObject);
    CSoundManager.instance.PlaySFX("immune_shock");
  }

  public void CreateNukeExplosion(Vector3 pos, bool myShock, CountryView cv, float life)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.templateNukeExplosion);
    gameObject.transform.parent = this.transform;
    gameObject.transform.position = pos;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, life);
    CSoundManager.instance.PlaySFX("nuke_explosion");
  }

  public void CreateBenignMimicObject(Vector3 pos, bool myShock, CountryView cv, float life)
  {
    TimedObject timedObject = UnityEngine.Object.Instantiate<TimedObject>(myShock ? this.templateMyBenignMimicObject : this.templateTheirBenignMimicObject);
    timedObject.transform.parent = this.transform;
    timedObject.transform.position = pos;
    timedObject.SetTime(life, new Action<TimedObject>(this.RecycleTimedObject));
    timedObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    timedObject.countryView = cv;
    timedObject.abiltyType = EAbilityType.benign_mimic;
    this.mpScaleObjects.Add((ScaleObject) timedObject);
    this.aaObjects.Add(timedObject);
    CSoundManager.instance.PlaySFX("benign_mimic");
  }

  public void CreateInfectBoostObject(Vector3 pos, bool myShock, CountryView cv, float life)
  {
    TimedObject timedObject = UnityEngine.Object.Instantiate<TimedObject>(myShock ? this.templateMyInfectBoostObject : this.templateTheirInfectBoostObject);
    timedObject.transform.parent = this.transform;
    timedObject.transform.position = pos;
    timedObject.SetTime(life, new Action<TimedObject>(this.RecycleTimedObject));
    timedObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    this.mpScaleObjects.Add((ScaleObject) timedObject);
    this.aaObjects.Add(timedObject);
    CSoundManager.instance.PlaySFX("infect_boost");
  }

  public void CreateLethalBoostObject(Vector3 pos, bool myShock, CountryView cv, float life)
  {
    TimedObject timedObject = UnityEngine.Object.Instantiate<TimedObject>(myShock ? this.templateMyLethalBoostObject : this.templateTheirLethalBoostObject);
    timedObject.transform.parent = this.transform;
    timedObject.transform.position = pos;
    timedObject.SetTime(life, new Action<TimedObject>(this.RecycleTimedObject));
    timedObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    this.mpScaleObjects.Add((ScaleObject) timedObject);
    this.aaObjects.Add(timedObject);
    CSoundManager.instance.PlaySFX("lethal_boost");
  }

  private void RecycleTimedObject(TimedObject to)
  {
    to.CleanUp();
    this.RecycleObject(to.gameObject);
  }

  private IEnumerator SpawnBonus(BonusIcon bonusIcon)
  {
    if ((bool) (UnityEngine.Object) this.mpGame)
    {
      BonusObject bonusObject = BonusObject.CreateBonus(bonusIcon.type, bonusIcon.disease);
      bonusObject.bonusID = bonusIcon.id;
      this.mpBonuses.Add(bonusObject);
      this.mpScaleObjects.Add((ScaleObject) bonusObject);
      bonusObject.gameObject.SetActive(false);
      while ((UnityEngine.Object) this.mpGame != (UnityEngine.Object) null && (double) bonusIcon.delay > 0.0)
      {
        bonusIcon.delay -= this.mpGame.DeltaGameTime;
        yield return (object) null;
      }
      Vector3? startPos = new Vector3?();
      int index = this.forcedBubbles.FindIndex((Predicate<ForcedBubble>) (b => b.country.id == bonusIcon.country.id && b.type == bonusIcon.type));
      if (index >= 0)
      {
        startPos = new Vector3?(this.forcedBubbles[index].position);
        this.forcedBubbles.RemoveAt(index);
      }
      if (bonusIcon.type == BonusIcon.EBonusIconType.APE_COLONY && (bool) (UnityEngine.Object) this.countryMap[bonusIcon.country.id].mpColonyLocation)
        startPos = new Vector3?(this.countryMap[bonusIcon.country.id].mpColonyLocation.transform.position);
      if (bonusIcon.type == BonusIcon.EBonusIconType.CASTLE && (bool) (UnityEngine.Object) this.countryMap[bonusIcon.country.id].mpCastleLocation)
        startPos = new Vector3?(this.countryMap[bonusIcon.country.id].mpCastleLocation.transform.position);
      if (bonusIcon.type == BonusIcon.EBonusIconType.NEXUS_FOUND)
        startPos = new Vector3?(bonusIcon.position);
      if (bonusIcon.type == BonusIcon.EBonusIconType.NUKE)
        startPos = new Vector3?(CInterfaceManager.instance.GetCountryView(bonusIcon.country.id).GetRandomPositionInsideCountry());
      if (startPos.HasValue)
        bonusIcon.spawnPosition = startPos.Value;
      bonusObject.Initialise(bonusIcon, this.countryMap[bonusIcon.country.id], startPos);
      if (this.localPlayerDisease != null)
      {
        if (this.localPlayerDisease.isCure)
        {
          if (this.localPlayerDisease.HasFlag(Disease.EGenericDiseaseFlag.GeneProcurementDirector) && (bonusIcon.type == BonusIcon.EBonusIconType.CURE || bonusIcon.type == BonusIcon.EBonusIconType.DISEASE_ORIGIN_COUNTRY || bonusIcon.type == BonusIcon.EBonusIconType.INFECT || bonusIcon.type == BonusIcon.EBonusIconType.DEADBUBBLE_FOR_CURE))
            bonusObject.autoPop = true;
        }
        else if (this.localPlayerDisease.bubbleAutopress && bonusIcon.disease == this.localPlayerDisease && (bonusIcon.type == BonusIcon.EBonusIconType.DNA || bonusIcon.type == BonusIcon.EBonusIconType.INFECT || bonusIcon.type == BonusIcon.EBonusIconType.DEATH || bonusIcon.type == BonusIcon.EBonusIconType.CASTLE || bonusIcon.type == BonusIcon.EBonusIconType.APE_COLONY))
          bonusObject.autoPop = true;
      }
      bonusObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
      bonusObject.SetVisible(this.localPlayerDisease == null || this.localPlayerDisease == bonusIcon.disease || CGameManager.IsCoopMPGame && ((CoopDisease) this.localPlayerDisease).isDefeated && bonusIcon.type != BonusIcon.EBonusIconType.NUKE);
      if (CGameManager.IsTutorialGame && bonusIcon.type == BonusIcon.EBonusIconType.INFECT && TutorialSystem.IsModuleComplete("9A") && !TutorialSystem.IsModuleSectionActive() && !TutorialSystem.IsModuleComplete("15A"))
      {
        TutorialSystem.SetLocalisedDictionary(new string[2]
        {
          "15A",
          "17A"
        }, new Dictionary<string, string>()
        {
          {
            "country",
            CLocalisationManager.GetText(bonusIcon.country.name)
          },
          {
            "s",
            bonusIcon.disease.name
          }
        });
        TutorialSystem.CheckModule((Func<bool>) (() => true), "15A", true);
        bonusObject.lifeMax = 0.0f;
        CountryView countryView = CInterfaceManager.instance.GetCountryView(bonusIcon.country.id);
        WorldMapController.instance.SetSelectedCountry(countryView, CountryView.EOverlayState.SELECTED);
        CInterfaceManager.instance.UpdateCountryState();
      }
      if (bonusObject.hide)
        bonusObject.DoHideImmediate();
      else if (bonusObject.startHide)
        bonusObject.DoHide();
      bonusObject = (BonusObject) null;
    }
  }

  public void ClickBonus(BonusIcon bonusIcon)
  {
    BonusObject lastObjectClicked;
    if ((UnityEngine.Object) this.mpLastObjectClicked == (UnityEngine.Object) null || this.mpLastObjectClicked.mpBonus != bonusIcon)
    {
      lastObjectClicked = this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bonusIcon.id));
    }
    else
    {
      lastObjectClicked = this.mpLastObjectClicked;
      this.mpLastObjectClicked = (BonusObject) null;
    }
    this.DoClickBonus(lastObjectClicked, CNetworkManager.network.LocalPlayerInfo.disease);
  }

  public void SetBonusClickable(BonusIcon bi, bool clickable)
  {
    this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bi.id)).SetUnclickable(clickable);
  }

  public void ForceClickBonus(BonusIcon bonusIcon, Disease clicker)
  {
    this.DoClickBonus(this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bonusIcon.id)), clicker);
  }

  public void DoClickBonus(BonusObject bonusObject, Disease clicker)
  {
    if (!((UnityEngine.Object) bonusObject != (UnityEngine.Object) null) || bonusObject.mpBonus == null)
      return;
    switch (bonusObject.mpBonus.type)
    {
      case BonusIcon.EBonusIconType.NEURAX:
        this.mpTargetBubbleStart = bonusObject.mpCountry;
        this.splineBubble = bonusObject;
        this.splineStartPosition = this.splineBubble.transform.position;
        CHUDScreen.instance.HudInterfaceMode = EHudMode.Neurax;
        break;
      case BonusIcon.EBonusIconType.NECROA:
        this.mpTargetBubbleStart = bonusObject.mpCountry;
        this.splineBubble = bonusObject;
        this.splineStartPosition = this.splineBubble.transform.position;
        CHUDScreen.instance.HudInterfaceMode = EHudMode.SendHorde;
        break;
      default:
        if (bonusObject.mpBonus.type == BonusIcon.EBonusIconType.NEXUS_FOUND)
          this.DestroyNexus(bonusObject.mpCountry.GetCountry());
        if (bonusObject.mpBonus.type != BonusIcon.EBonusIconType.NUKE)
        {
          bonusObject.DoBubblePop(clicker, false);
          this.mpScaleObjects.Remove((ScaleObject) bonusObject);
          this.mpBonuses.Remove(bonusObject);
        }
        CHUDScreen.instance.SetDNAPoints(CGameManager.localPlayerInfo.disease.evoPoints);
        if ((!CGameManager.IsTutorialGame || !TutorialSystem.IsModuleActive("5A")) && (!TutorialSystem.IsModuleActive("3B") && !TutorialSystem.IsModuleActive("16A") || bonusObject.mpBonus.type != BonusIcon.EBonusIconType.INFECT))
          break;
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
        break;
    }
  }

  public bool CanClickBubble()
  {
    switch (CHUDScreen.instance.HudInterfaceMode)
    {
      case EHudMode.Disabled:
      case EHudMode.Normal:
        return true;
      default:
        return false;
    }
  }

  public void TargetBubbleCancel()
  {
    if ((UnityEngine.Object) this.splineBubble != (UnityEngine.Object) null)
    {
      this.splineBubble.ForgetBubbleClick();
      this.StopSpline();
    }
    if (CHUDScreen.instance.HudInterfaceMode == EHudMode.SelectHorde || CHUDScreen.instance.HudInterfaceMode == EHudMode.Reanimate || CHUDScreen.instance.HudInterfaceMode == EHudMode.ApeRampage || CHUDScreen.instance.HudInterfaceMode == EHudMode.ApeCreateColony || CHUDScreen.instance.HudInterfaceMode == EHudMode.SelectApeHorde)
      CSoundManager.instance.PlaySFX("aafail");
    this.StopSpline();
    CHUDScreen.instance.Default();
  }

  public void TargetBubbleSuccess(CountryView hitCountry, Vector3 hitpos)
  {
    this.AddForcedPositionBubble(hitCountry.GetCountry(), hitpos, BonusIcon.EBonusIconType.INFECT);
    this.splineEndPosition = hitpos;
    if ((UnityEngine.Object) this.splineBubble != (UnityEngine.Object) null)
    {
      this.splineBubble.DoBubblePop(isPlaySound: this.splineBubble.type != BonusIcon.EBonusIconType.NEURAX);
      Debug.Log((object) ("SPLINE BUBBLE SUCCESS " + (object) this.splineBubble.bonusID));
      this.mpScaleObjects.Remove((ScaleObject) this.splineBubble);
      this.mpBonuses.Remove(this.splineBubble);
      this.splineBubble = (BonusObject) null;
    }
    this.StopSpline();
    this.mpTargetBubbleStart = (CountryView) null;
    CHUDScreen.instance.Default();
    WorldMapController.instance.SetSelectedCountry(hitCountry, CountryView.EOverlayState.SELECTED);
  }

  public void DoGemClick(GemObject gemObject)
  {
    CHUDScreen.instance.HudInterfaceMode = EHudMode.MoveGem;
    this.splineStartPosition = gemObject.transform.position;
    this.movingGem = gemObject;
  }

  public void DoGemPlace(CountryView countryView, Vector3 position)
  {
    if ((UnityEngine.Object) countryView == (UnityEngine.Object) this.movingGem.mpCountry)
    {
      this.CancelGem(new Vector3?(position));
    }
    else
    {
      this.movingGem.Place(countryView, position);
      this.movingGem = (GemObject) null;
    }
  }

  public void CancelGem(Vector3? position = null)
  {
    this.movingGem.CancelMove(position);
    this.movingGem = (GemObject) null;
  }

  public void RegisterBubbleClick(BonusObject bonusObject)
  {
    this.mpLastObjectClicked = bonusObject;
  }

  public void StartHideBonus(BonusIcon bonusIcon)
  {
    BonusObject bonusObject = this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bonusIcon.id));
    if ((bool) (UnityEngine.Object) bonusObject)
      bonusObject.DoHide();
    else
      Debug.LogError((object) ("UNABLE TO FIND BONUS TO START HIDE: " + (object) bonusIcon.id + " " + (object) bonusIcon.type + " object: " + (object) bonusObject));
  }

  public void HideBonus(BonusIcon bonusIcon)
  {
    BonusObject bonusObject = this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bonusIcon.id));
    if (!(bool) (UnityEngine.Object) bonusObject)
      return;
    if ((UnityEngine.Object) this.mpGame != (UnityEngine.Object) null && this.mpGame.IsReplayActive)
      Debug.Log((object) ("HIDING A BONUS: " + (object) bonusIcon.id + " " + (object) bonusIcon.type + " object: " + (object) bonusObject));
    bonusObject.DoHideImmediate();
  }

  public void PopBonus(BonusIcon bonusIcon, Disease clicker)
  {
    BonusObject bonusObject = this.mpBonuses.Find((Predicate<BonusObject>) (a => a.bonusID == bonusIcon.id));
    if ((bool) (UnityEngine.Object) bonusObject)
    {
      if ((UnityEngine.Object) this.mpGame != (UnityEngine.Object) null && this.mpGame.IsReplayActive)
        Debug.Log((object) ("POPPING A BONUS: " + (object) bonusIcon.id + " " + (object) bonusIcon.type + " object: " + (object) bonusObject));
      bonusObject.DoBubblePop(clicker);
    }
    else
    {
      if (!((UnityEngine.Object) this.mpGame != (UnityEngine.Object) null) || this.mpGame.IsReplayActive)
        return;
      Debug.LogError((object) ("UNABLE TO FIND BONUS TO POP: " + (object) bonusIcon.id + " " + (object) bonusIcon.type + " object: " + (object) bonusObject));
    }
  }

  public void RecycleObject(GameObject go)
  {
    ScaleObject component = go.GetComponent<ScaleObject>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.mpScaleObjects.Remove(component);
    if (component is BonusObject)
      this.mpBonuses.Remove(component as BonusObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) go);
  }

  public void SetScaleFactor(float scaleFactor)
  {
    for (int index = 0; index < this.mpScaleObjects.Count; ++index)
      this.mpScaleObjects[index].SetScaleFactor(scaleFactor);
  }

  public void SetGameSpeed(int speed)
  {
    if (!(bool) (UnityEngine.Object) this.mpGame)
    {
      Debug.LogWarning((object) "Trying to set game speed when game is invalid");
    }
    else
    {
      if (speed < 0 || speed > 3)
        return;
      this.mpGame.SetSpeed(speed);
    }
  }

  public void SetSpeedState(int speed, int mySpeed = 0, int oppSpeed = 0)
  {
    for (int index = 0; index < this.mpBonuses.Count; ++index)
    {
      float num = Mathf.Clamp01((float) speed);
      if ((double) num != 0.0 || this.mpBonuses[index].IsEnding())
        this.mpBonuses[index].bonusAnimator.speed = num;
    }
    if (CGameManager.game.IsReplayActive)
      return;
    CHUDScreen.instance.SetButtonState(speed, mySpeed, oppSpeed);
  }

  public CountryView GetHoverCountry(bool setVar = true)
  {
    CountryView countryView1 = (CountryView) null;
    if ((bool) (UnityEngine.Object) Camera.main)
    {
      Vector3 mousePosition = Input.mousePosition;
      Vector3 hitpoint1;
      if (this.CheckColliderUnderPoint(mousePosition, out hitpoint1, out countryView1))
        this.mapHoverPoint = hitpoint1;
      if (CHUDScreen.instance.HudInterfaceMode == EHudMode.SendVampire)
      {
        Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 normalized = ray.direction.normalized;
        Vector3 vector3_1 = ray.origin;
        if ((double) normalized.z != 0.0)
          vector3_1 = ray.origin + normalized.normalized * (-ray.origin.z / normalized.z);
        if ((UnityEngine.Object) countryView1 != (UnityEngine.Object) null)
        {
          Vector3 vector3_2 = (vector3_1 - this.splineStartPosition) with
          {
            z = 0.0f
          };
          float vampireFlightDistance = disease.GetMaxVampireFlightDistance();
          Color c = CGameManager.game.GetColourSet(CGameManager.localPlayerInfo.disease.id).lineColour;
          float magnitude = vector3_2.magnitude;
          float num = 1f;
          if ((double) magnitude > (double) vampireFlightDistance)
          {
            if ((double) disease.castleReturnSpeed > 0.0 && countryView1.GetCountry().GetLocalDisease(disease).castleState == ECastleState.CASTLE_ALIVE)
            {
              CHUDScreen.instance.vampireFlightRadius.SetState(false);
            }
            else
            {
              Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.splineStartPosition + vector3_2.normalized * vampireFlightDistance) with
              {
                z = mousePosition.z
              };
              Vector3 hitpoint2;
              CountryView countryView2;
              if ((double) magnitude < (double) vampireFlightDistance + (double) num && this.CheckColliderUnderPoint(screenPoint, out hitpoint2, out countryView2) && (UnityEngine.Object) countryView2 == (UnityEngine.Object) countryView1)
              {
                this.mapHoverPoint = hitpoint2;
              }
              else
              {
                CHUDScreen.instance.vampireFlightRadius.SetState(false);
                countryView1 = (CountryView) null;
                c = this.colorLineDisabled;
              }
            }
          }
          if (c != this.splineRenderer.currentColor)
          {
            CHUDScreen.instance.vampireFlightRadius.SetState(true);
            this.splineRenderer.UpdateColour(c);
          }
        }
      }
    }
    this.currentCountry = setVar ? countryView1 : this.currentCountry;
    return countryView1;
  }

  public bool CheckColliderUnderPoint(
    Vector3 point,
    out Vector3 hitpoint,
    out CountryView countryView)
  {
    hitpoint = Vector3.zero;
    countryView = (CountryView) null;
    RaycastHit hitInfo;
    if (!Physics.Raycast(Camera.main.ScreenPointToRay(point), out hitInfo, float.PositiveInfinity, ~((int) this.bubbleLayer | (int) UITooltip.instance.tooltipLayer)))
      return false;
    countryView = hitInfo.collider.GetComponent<CountryView>();
    hitpoint = hitInfo.point;
    return true;
  }

  public bool CountryUnderPoint(Vector3 point, out Country country)
  {
    country = (Country) null;
    CountryView countryView;
    return this.CheckColliderUnderPoint(point, out Vector3 _, out countryView) && (UnityEngine.Object) countryView != (UnityEngine.Object) null;
  }

  public void ShowAchievement(EAchievement achievement)
  {
  }

  public void SetLineType(EHudMode hudMode) => this.splineRenderer.SetLineType(hudMode);

  public void SetSplineRenderer(Vector3 positionTo, Color color, bool curvedLine)
  {
    Vector3 vector3 = new Vector3(0.0f, 0.0f, -0.1f);
    if (curvedLine)
      this.splineRenderer.StartCurve(this.splineStartPosition + vector3, positionTo + vector3, color);
    else
      this.splineRenderer.StartLine(this.splineStartPosition + vector3, positionTo + vector3, color);
  }

  public void StopSpline() => this.splineRenderer.gameObject.SetActive(false);

  public void EndGame(
    IGame.GameType gameType,
    bool didWin,
    Disease diseaseWon,
    IGame.EndGameResult resultType,
    CUIManager.Unlock unlocked)
  {
    Debug.Log((object) ("**EndGame** gameType:" + (object) gameType + " didWin:" + didWin.ToString() + " diseaseWon:" + (diseaseWon != null ? (object) diseaseWon.name : (object) "null") + " resultType:" + (object) resultType + " unlocked:" + (object) unlocked));
    if (!CGameManager.IsMultiplayerGame)
    {
      (CUIManager.instance.GetScreen("EndScreen") as CEndGameScreen).Unlocked = unlocked;
      CResultScreen newScreen = !didWin ? (!CGameManager.IsCureGame ? CUIManager.instance.GetScreen("LoseScreen").GetComponent<CResultScreen>() : CUIManager.instance.GetScreen("LoseScreen_Cure").GetComponent<CResultScreen>()) : (!CGameManager.IsCureGame ? CUIManager.instance.GetScreen("WinScreen").GetComponent<CResultScreen>() : CUIManager.instance.GetScreen("WinScreen_Cure").GetComponent<CResultScreen>());
      int endID = 0;
      if (CNetworkManager.network.LocalPlayerInfo.disease.cureFlag)
        endID = 1;
      newScreen.SetEnding(CNetworkManager.network.LocalPlayerInfo.disease, endID);
      CUIManager.instance.SetActiveScreen((IGameScreen) newScreen);
      this.resultOverlay = (MultiplayerResultOverlay) null;
      this.Cleanup();
    }
    else
    {
      CUIManager.instance.HideAllOverlays();
      if (World.instance != null && World.instance.DiseaseTurn > 0)
      {
        CUIManager.instance.SetActiveScreen("HUDScreenMP");
        (CUIManager.instance.GetScreen("MP_EndScreen") as MultiplayerEndScreen).Unlocked = unlocked;
        this.resultOverlay = (MultiplayerResultOverlay) null;
        Debug.Log((object) ("DISEASE WON: " + (object) diseaseWon));
        MultiplayerResultOverlay.MultiplayerResult result;
        if (didWin)
        {
          result = MultiplayerResultOverlay.MultiplayerResult.Win;
          this.resultOverlay = CUIManager.instance.GetOverlay("MP_WinOverlay") as MultiplayerResultOverlay;
        }
        else if (diseaseWon == null && gameType != IGame.GameType.CoopMP)
        {
          result = MultiplayerResultOverlay.MultiplayerResult.Draw;
          this.resultOverlay = CUIManager.instance.GetOverlay("MP_DrawOverlay") as MultiplayerResultOverlay;
        }
        else
        {
          result = MultiplayerResultOverlay.MultiplayerResult.Lose;
          this.resultOverlay = CUIManager.instance.GetOverlay("MP_LoseOverlay") as MultiplayerResultOverlay;
        }
        this.resultOverlay.SetEnding(gameType, CNetworkManager.network.LocalPlayerInfo.disease, result, resultType);
        CHUDScreen screen = CUIManager.instance.GetScreen("HUDScreen") as CHUDScreen;
        if ((UnityEngine.Object) screen != (UnityEngine.Object) null)
          screen.SetEnable(false);
        CUIManager.instance.ShowOverlay((CGameOverlay) this.resultOverlay);
      }
      else
      {
        CUIManager.instance.ClearHistory();
        CGameManager.ClearGame();
        DynamicMusic.instance.FadeOut();
        CUIManager.instance.SetupScreens();
        IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
        List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
        igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
        CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
        igameSubScreenList.Clear();
        igameSubScreenList.Add(screen.GetSubScreen("MultiplayerLobby"));
        CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
        CUIManager.instance.redSingleButtonConfirmOverlay.ShowLocalised("MP_Only_Player_Left_Title", "MP_Only_Player_Left_Text", "OK", pressA: (CConfirmOverlay.PressDelegate) (() => this.ReenterLobby()));
        CNetworkManager.network.TerminateAndReinitialise();
      }
    }
  }

  private void ReenterLobby()
  {
    if (MultiplayerLobbyScreen.instance.lobbyParams.friendMode != INetwork.FriendMode.Public)
      return;
    MultiplayerLobbyScreen.instance.PressQuickMatch();
  }

  public BonusObject GetBonusTemplate(BonusIcon.EBonusIconType bonusIconType, bool isMine)
  {
    if (CGameManager.IsCureGame)
    {
      switch (bonusIconType)
      {
        case BonusIcon.EBonusIconType.CURE:
          return this.templateCureProgress;
        case BonusIcon.EBonusIconType.INFECT:
          return this.templateCureInfected;
        case BonusIcon.EBonusIconType.COUNTRY_SELECT:
          return this.templateCureSelect;
        case BonusIcon.EBonusIconType.DISEASE_ORIGIN_COUNTRY:
          return this.templateCureNexusInfected;
        case BonusIcon.EBonusIconType.DEADBUBBLE_FOR_CURE:
          return this.templateCureDeath;
        default:
          Debug.LogError((object) ("Unexpected bubble type for cure: " + (object) bonusIconType));
          return this.templateInfect;
      }
    }
    else
    {
      switch (bonusIconType)
      {
        case BonusIcon.EBonusIconType.DNA:
          return !isMine ? this.templateDNAOther : this.templateDNA;
        case BonusIcon.EBonusIconType.CURE:
          return !isMine ? this.templateCureOther : this.templateCure;
        case BonusIcon.EBonusIconType.INFECT:
          return !isMine ? this.templateInfectOther : this.templateInfect;
        case BonusIcon.EBonusIconType.DEATH:
          return !isMine ? this.templateDeathOther : this.templateDeath;
        case BonusIcon.EBonusIconType.NEURAX:
          return !isMine ? this.templateNeuraxOther : this.templateNeurax;
        case BonusIcon.EBonusIconType.COUNTRY_SELECT:
          return this.templateCountrySelect;
        case BonusIcon.EBonusIconType.COUNTRY_SELECT_P2_INTENTION:
          return this.templateCountrySelectP2Intention;
        case BonusIcon.EBonusIconType.COUNTRY_SELECT_P2_SELECTED:
          return this.templateCountrySelectP2Selected;
        case BonusIcon.EBonusIconType.NECROA:
          return this.templateNecroa;
        case BonusIcon.EBonusIconType.APE_COLONY:
          return this.templateApeColony;
        case BonusIcon.EBonusIconType.NEXUS_FOUND:
          return this.templateNexusFound;
        case BonusIcon.EBonusIconType.NEXUS_DNA:
          return this.templateNexusDNA;
        case BonusIcon.EBonusIconType.DOUBLE_INFECTED_DNA:
          return this.templateDoubleInfectedDNA;
        case BonusIcon.EBonusIconType.NUKE:
          return this.templateNuke;
        case BonusIcon.EBonusIconType.CASTLE:
          return this.templateVampireLair;
        default:
          return !isMine ? this.templateInfectOther : this.templateInfect;
      }
    }
  }

  public void SetPortRendererState(bool isActive)
  {
    for (int index = 0; index < this.mpPortObjects.Length; ++index)
      this.mpPortObjects[index].gameObject.SetActive(isActive);
  }

  private Color GetVehicleColor(Vehicle vehicle)
  {
    switch (vehicle.subType)
    {
      case Vehicle.EVehicleSubType.Cure:
      case Vehicle.EVehicleSubType.Intel:
      case Vehicle.EVehicleSubType.Blue:
        return vehicle.actingDisease != null && CGameManager.localPlayerInfo.disease != null && vehicle.actingDisease != CGameManager.localPlayerInfo.disease ? this.colorNormal : this.colorCure;
      case Vehicle.EVehicleSubType.Neurax:
        return this.colorNeurax;
      case Vehicle.EVehicleSubType.Fort:
      case Vehicle.EVehicleSubType.MiniFort:
      case Vehicle.EVehicleSubType.FortEscapees:
        return this.colorFort;
      case Vehicle.EVehicleSubType.ApeLabPlane:
        return this.colorCure;
      case Vehicle.EVehicleSubType.Unscheduled:
        return CGameManager.game.CanSeeDots(vehicle.source) ? CGameManager.game.GetColourSet(vehicle.actingDisease.id).infectedColour : this.colorNormal;
      default:
        if (CInterfaceManager.instance.localPlayerDisease != null)
        {
          int id = CInterfaceManager.instance.localPlayerDisease.id;
          if (!vehicle.hiddenInfected && vehicle.infectedTotal > 0 && vehicle.infected[id] > 0)
            return this.colorInfected;
        }
        return this.colorNormal;
    }
  }

  private Color GetVehicleColorRef(Vehicle forVehicle)
  {
    switch (forVehicle.subType)
    {
      case Vehicle.EVehicleSubType.Cure:
        return this.colorCureRef;
      case Vehicle.EVehicleSubType.Neurax:
        return this.colorNeuraxRef;
      case Vehicle.EVehicleSubType.Fort:
      case Vehicle.EVehicleSubType.MiniFort:
      case Vehicle.EVehicleSubType.FortEscapees:
        return this.colorFortRef;
      default:
        return !forVehicle.hiddenInfected && forVehicle.infectedTotal > 0 ? this.colorInfectedRef : this.colorNormalRef;
    }
  }

  private void OnDefault(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    Camera_Zoom.instance.DeltaCameraToDefault();
  }

  public void SetCursorSelection(EHudMode state)
  {
    switch (state)
    {
      case EHudMode.Neurax:
        this.cursorCurrent = this.cursorNeurax;
        break;
      case EHudMode.Reanimate:
        this.cursorCurrent = this.cursorReanimate;
        break;
      case EHudMode.SelectHorde:
      case EHudMode.SendHorde:
        this.cursorCurrent = this.cursorHorde;
        break;
      case EHudMode.ApeRampage:
        this.cursorCurrent = this.cursorApeRampage;
        break;
      case EHudMode.SelectApeHorde:
      case EHudMode.SendApeHorde:
      case EHudMode.SendApeColony:
        this.cursorCurrent = this.cursorApeHorde;
        break;
      case EHudMode.ApeCreateColony:
        this.cursorCurrent = this.cursorApeCreateColony;
        break;
      case EHudMode.PlaceGem:
      case EHudMode.MoveGem:
        this.cursorCurrent = this.cursorNormal;
        break;
      case EHudMode.SelectUnscheduledFlight:
      case EHudMode.SendUnscheduledFlight:
        this.cursorCurrent = this.cursorUnscheduledFlight;
        break;
      case EHudMode.ImmuneShock:
        this.cursorCurrent = this.cursorImmuneShock;
        break;
      case EHudMode.BenignMimic:
        this.cursorCurrent = this.cursorBenignMimic;
        break;
      case EHudMode.InfectBoost:
        this.cursorCurrent = this.cursorInfectBoost;
        break;
      case EHudMode.LethalBoost:
        this.cursorCurrent = this.cursorLethalBoost;
        break;
      case EHudMode.NukeStrike:
        this.cursorCurrent = this.cursorNuclearStrike;
        break;
      case EHudMode.BloodRage:
        this.cursorCurrent = this.cursorBloodRage;
        break;
      case EHudMode.SelectVampire:
      case EHudMode.SendVampire:
        this.cursorCurrent = this.cursorVampTravel;
        break;
      case EHudMode.CreateCastle:
        this.cursorCurrent = this.cursorVampLair;
        break;
      case EHudMode.SendInvestigationTeam:
        this.cursorCurrent = this.cursorFieldOperatives;
        break;
      case EHudMode.RaisePriority:
        this.cursorCurrent = this.cursorQuarantine;
        break;
      case EHudMode.EconomicSupport:
        this.cursorCurrent = this.cursorEconomicAid;
        break;
      default:
        this.cursorCurrent = !CGameManager.IsCureGame ? this.cursorNormal : this.cursorNormalCure;
        break;
    }
    this.SetCursorMode(this.cursorMode);
  }

  public void SetCursorMode(ECursorMode cursorMode)
  {
    this.cursorMode = cursorMode;
    Texture2D texture = this.cursorCurrent[0];
    switch (cursorMode)
    {
      case ECursorMode.HOVER:
        this.currentCursorTexture = this.cursorCurrent[1];
        break;
      case ECursorMode.CLICK:
        this.currentCursorTexture = this.cursorCurrent[2];
        break;
    }
    UITexture fakeCursor = CSteamControllerManager.instance.fakeCursor;
    if ((UnityEngine.Object) this.currentCursorTexture == (UnityEngine.Object) null || (UnityEngine.Object) this.currentCursorTexture != (UnityEngine.Object) texture)
      fakeCursor.mainTexture = (Texture) texture;
    if (!fakeCursor.gameObject.activeInHierarchy)
      Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    this.currentCursorTexture = texture;
  }

  public void BufferRenderTextures()
  {
    DiseaseTrailParticles.instance.BufferRenderTextures(true);
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
      country.Value.BufferRenderTextures();
  }

  public void RestoreRenderTextures()
  {
    DiseaseTrailParticles.instance.ReloadRenderTextures();
    foreach (KeyValuePair<string, CountryView> country in (IEnumerable<KeyValuePair<string, CountryView>>) this.countryMap)
      country.Value.RestoreRenderTextures();
  }

  public void SetNecroaDisplay(long number, Vector3 position)
  {
    NecroaReanimateDisplay reanimateDisplay = NecroaReanimateDisplay.CreateDisplay();
    if ((UnityEngine.Object) reanimateDisplay == (UnityEngine.Object) null)
    {
      reanimateDisplay = NGUITools.AddChild(this.parentNecroaDisplay, this.templateNecroaDisplay.gameObject).GetComponent<NecroaReanimateDisplay>();
      this.mpScaleObjects.Add((ScaleObject) reanimateDisplay);
      reanimateDisplay.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
    }
    reanimateDisplay.StartDisplay(number, position);
  }

  public void NecroaParticles(ParticleSystem particleSystem, bool canAdd)
  {
    if (canAdd)
      this.activeParticleSystems.Add(particleSystem);
    else
      this.activeParticleSystems.Remove(particleSystem);
  }

  public void DestroyDrone(Vehicle vehicle)
  {
    for (int index = 0; index < this.mpScaleObjects.Count; ++index)
    {
      if (this.mpScaleObjects[index] is TargetingDroneObject)
      {
        TargetingDroneObject mpScaleObject = this.mpScaleObjects[index] as TargetingDroneObject;
        if (mpScaleObject.mpVehicle == vehicle)
          mpScaleObject.Explode();
      }
    }
  }

  internal void SpawnAndUpdateGems(List<GemEffect> updatedGems)
  {
    foreach (GemEffect updatedGem in updatedGems)
    {
      if (updatedGem.lifeTime <= 0)
      {
        if (this.activeGemObjects.ContainsKey(updatedGem))
        {
          this.activeGemObjects[updatedGem].Expire();
          this.gemObjects.Remove(this.activeGemObjects[updatedGem]);
        }
        this.activeGemObjects.Remove(updatedGem);
      }
      else
      {
        if (!this.countryMap.ContainsKey(updatedGem.country.id))
        {
          Debug.Log((object) ("GEM: " + updatedGem.country.id + " not found"));
          break;
        }
        GemObject gemObject = UnityEngine.Object.Instantiate<GemObject>(this.gemTemplate);
        gemObject.name = "Gem+" + updatedGem.abilityName + "+" + (object) updatedGem.id;
        gemObject.transform.parent = this.transform;
        gemObject.isMine = this.localPlayerDisease == updatedGem.disease;
        this.gemObjects.Add(gemObject);
        this.activeGemObjects.Add(updatedGem, gemObject);
        this.mpScaleObjects.Add((ScaleObject) gemObject);
        gemObject.Initialise(updatedGem, this.countryMap[updatedGem.country.id], this.localPlayerDisease == updatedGem.disease, updatedGem.position);
        gemObject.SetScaleFactor(Camera_Zoom.instance.currentZoomFactor);
        gemObject.SetGemType(updatedGem.abilityName);
        gemObject.SetVisible(this.localPlayerDisease == updatedGem.disease);
      }
    }
  }

  public void OnTutorialBegin(Module withModule)
  {
    if (!(withModule.Name == "15A"))
      return;
    CGameManager.SetPaused(true, true);
  }

  public void OnTutorialComplete(Module completedModule)
  {
    if (!(completedModule.Name == "5A"))
      return;
    TutorialSystem.CheckModule((Func<bool>) (() => true), "6A");
  }

  public void OnTutorialModeExit(Module currentModule)
  {
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  [Serializable]
  public class HighlightColourSet
  {
    public Disease.EDiseaseType diseaseType;
    public Color selected = new Color(1f, 1f, 1f, 0.5f);
    public Color valid = new Color(1f, 1f, 1f, 1f);
    public Color p2Intent = new Color(1f, 1f, 1f, 1f);
    public Color p2 = new Color(1f, 1f, 1f, 1f);
    public Color portalTarget = new Color(1f, 1f, 1f, 1f);
  }

  public static class EdgeHelpers
  {
    public static List<CInterfaceManager.EdgeHelpers.Edge> GetEdges(int[] aIndices)
    {
      List<CInterfaceManager.EdgeHelpers.Edge> edges = new List<CInterfaceManager.EdgeHelpers.Edge>();
      for (int aIndex1 = 0; aIndex1 < aIndices.Length; aIndex1 += 3)
      {
        int aIndex2 = aIndices[aIndex1];
        int aIndex3 = aIndices[aIndex1 + 1];
        int aIndex4 = aIndices[aIndex1 + 2];
        edges.Add(new CInterfaceManager.EdgeHelpers.Edge(aIndex2, aIndex3, aIndex1));
        edges.Add(new CInterfaceManager.EdgeHelpers.Edge(aIndex3, aIndex4, aIndex1));
        edges.Add(new CInterfaceManager.EdgeHelpers.Edge(aIndex4, aIndex2, aIndex1));
      }
      return edges;
    }

    public static List<CInterfaceManager.EdgeHelpers.Edge> FindBoundary(
      List<CInterfaceManager.EdgeHelpers.Edge> aEdges)
    {
      List<CInterfaceManager.EdgeHelpers.Edge> boundary = new List<CInterfaceManager.EdgeHelpers.Edge>((IEnumerable<CInterfaceManager.EdgeHelpers.Edge>) aEdges);
      for (int index1 = boundary.Count - 1; index1 > 0; --index1)
      {
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          if (boundary[index1].v1 == boundary[index2].v2 && boundary[index1].v2 == boundary[index2].v1)
          {
            boundary.RemoveAt(index1);
            boundary.RemoveAt(index2);
            --index1;
            break;
          }
        }
      }
      return boundary;
    }

    public struct Edge
    {
      public int v1;
      public int v2;
      public int triangleIndex;

      public Edge(int aV1, int aV2, int aIndex)
      {
        this.v1 = aV1;
        this.v2 = aV2;
        this.triangleIndex = aIndex;
      }
    }
  }
}
