// Decompiled with JetBrains decompiler
// Type: CountryView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class CountryView : MonoBehaviour
{
  private Country country;
  internal MeshFilter countryMesh;
  internal MeshFilter colliderMesh;
  private NexusObject mpNexus;
  private FortObject mpFort;
  private ApeLabObject mpLab;
  private CoopLabObject coopLab;
  private FortObject mpCureHQ;
  private CureStateObject cureStateObject;
  public GameObject mpNexusLocation;
  public GameObject mpFortLocation;
  public GameObject mpLabLocation;
  public GameObject coopLabLocation;
  public GameObject mpColonyLocation;
  public GameObject mpCastleLocation;
  public GameObject mpVampireContainer;
  public PortObject mpAirport;
  public List<PortObject> mpSeaport;
  private bool mbSeaStatus = true;
  private bool mbAirStatus = true;
  internal CastleObject mpCastleObject;
  internal List<MPVehicleDiseaseTrail> mpVehicleDiseaseTrails = new List<MPVehicleDiseaseTrail>();
  private GameObject overlay;
  private GameObject intelOverlay;
  private Material overlayMaterial;
  private Material overlayMaterialCure;
  private Material mainMaterial;
  private Renderer overlayRenderer;
  private MaterialPropertyBlock intelPropertyBlock;
  private MeshRenderer intelRenderer;
  private MaterialPropertyBlock mainPropertyBlock;
  private Renderer mainRenderer;
  internal float area;
  private Color overlayColor;
  private bool isSelected;
  private MaterialPropertyBlock overlayPropertyBlock;
  private bool apeInfectDisplayed;
  private bool bloodRageDisplayed;
  private Coroutine bloodRageCoroutine;
  private bool borderCloseDisplayed;
  private bool borderClosePulsing;
  private float zComBorder;
  private int borderFadeDirection = 1;
  private bool borderLockdownDisplayed;
  private bool unrestDisplayed;
  private Color lockdownColor = new Color(0.9411765f, 0.9411765f, 0.0f, 0.784313738f);
  private Color borderCloseColor = new Color(1f, 0.0f, 0.0f, 1f);
  private Color unrestColor = new Color(0.0f, 0.0f, 0.0f, 1f);
  private Color normalBorderColor = Color.clear;
  private float borderPulseSpeed = 1f;
  private bool isContinuousPulse;
  private InfectionParticleSystem system;
  private ParticleSystem[] bloodRageParticleEmitters;
  private float minOverlayAlpha = 0.01f;
  private float maxOverlayAlpha = 0.25f;
  private int lastDiseaseTurn = -1;
  private int lastState;
  public CountryParticles reanimateParticles;
  public CountryParticles apeRampageParticles;
  public GameObject bloodRageParticles;
  private bool fortBorder;
  private bool hadApes;
  private bool lockdownBorder;
  private int[] meshTriangles;
  private Vector3[] meshVertices;
  private bool restartingFromReconnect;

  public string countryID => this.name;

  private void Start()
  {
    this.overlayPropertyBlock = new MaterialPropertyBlock();
    this.countryMesh = this.GetComponent<MeshFilter>();
    this.colliderMesh = this.countryMesh;
    this.overlay = new GameObject();
    this.BloodRageEffectEnd();
    this.overlay.AddComponent<MeshRenderer>();
    this.overlay.AddComponent<MeshFilter>().mesh = this.countryMesh.sharedMesh;
    this.mainRenderer = this.GetComponent<Renderer>();
    this.mainMaterial = this.mainRenderer.sharedMaterial;
    this.mainPropertyBlock = new MaterialPropertyBlock();
    this.mainPropertyBlock.SetColor("_BorderColor", Color.clear);
    this.mainPropertyBlock.SetColor("_InfectColor", Color.clear);
    this.mainRenderer.SetPropertyBlock(this.mainPropertyBlock);
    this.overlayRenderer = this.overlay.GetComponent<Renderer>();
    this.overlayMaterial = InfectionPlaneContainer.instance.GetSelectOverlayMaterial(this.mainMaterial.mainTexture.name);
    this.overlayMaterialCure = InfectionPlaneContainer.instance.GetCureSelectOverlayMaterial(this.mainMaterial.mainTexture.name);
    this.overlayRenderer.sharedMaterial = this.overlayMaterial;
    this.overlay.transform.parent = this.transform;
    this.overlay.transform.localPosition = Vector3.zero;
    this.overlay.transform.localRotation = Quaternion.identity;
    this.overlay.name = this.name + " selected";
    this.overlay.layer = 26;
    this.overlay.SetActive(false);
    CInterfaceManager.instance.AddCountryLink(this.name, this);
    this.SetAirportState(true);
    this.SetSeaportState(true);
    this.intelPropertyBlock = new MaterialPropertyBlock();
    this.intelPropertyBlock.SetColor("_BorderColor", Color.clear);
    this.intelOverlay = new GameObject();
    this.intelRenderer = this.intelOverlay.AddComponent<MeshRenderer>();
    this.intelOverlay.AddComponent<MeshFilter>().mesh = this.countryMesh.sharedMesh;
    this.intelRenderer.sharedMaterial = InfectionPlaneContainer.instance.GetIntelOverlayMaterial(this.mainMaterial.mainTexture.name);
    this.intelRenderer.SetPropertyBlock(this.intelPropertyBlock);
    this.intelOverlay.transform.parent = this.transform;
    this.intelOverlay.transform.localPosition = Vector3.zero;
    this.intelOverlay.transform.localRotation = Quaternion.identity;
    this.intelOverlay.name = this.name + " NO INTEL";
    this.intelOverlay.layer = 26;
    this.intelOverlay.SetActive(false);
  }

  internal int GetPeoplePerDot(Disease d)
  {
    return d.isCure ? Mathf.RoundToInt((float) this.country.originalPopulation / (Mathf.Pow(this.area, 1.12f) + 1000f)) : Mathf.RoundToInt((float) this.country.originalPopulation / (this.area / 3f));
  }

  public void Initialise()
  {
    this.StopAllCoroutines();
    for (int index = 0; index < this.mpVehicleDiseaseTrails.Count; ++index)
      this.mpVehicleDiseaseTrails[index].Initialise();
    this.mainPropertyBlock.SetFloat("_Glow", 0.0f);
    this.mainPropertyBlock.SetColor("_BorderColor", Color.clear);
    this.mainPropertyBlock.SetColor("_InfectColor", Color.clear);
    this.mainRenderer.SetPropertyBlock(this.mainPropertyBlock);
    this.intelPropertyBlock.SetColor("_BorderColor", Color.clear);
    this.intelRenderer.SetPropertyBlock(this.intelPropertyBlock);
    this.borderCloseDisplayed = false;
    this.overlay.SetActive(false);
    this.intelOverlay.SetActive(false);
    this.country = World.instance.GetCountry(this.name);
    if (this.country == null)
    {
      Debug.LogError((object) ("NULL COUNTRY FOR: " + this.name));
    }
    else
    {
      Vector3 size = this.colliderMesh.GetComponent<Renderer>().bounds.size;
      this.area = (float) ((double) size.x * (double) size.y * 25.0 * 25.0);
      this.StartCoroutine(this.CountryUpdate());
    }
    if ((bool) (UnityEngine.Object) this.system)
      this.system.Initialise();
    this.SetAirportEnabled(this.country.hasAirport);
    this.SetAirportState(true);
    this.SetSeaportState(true);
    this.SetSelected(CountryView.EOverlayState.OFF);
    this.lastState = 0;
    this.BloodRageEffectEnd(true);
  }

  private void CheckOverlay()
  {
    if (CGameManager.IsCureGame)
    {
      if (!((UnityEngine.Object) this.overlayRenderer.sharedMaterial != (UnityEngine.Object) this.overlayMaterialCure))
        return;
      this.overlayRenderer.sharedMaterial = this.overlayMaterialCure;
    }
    else
    {
      if (!((UnityEngine.Object) this.overlayRenderer.sharedMaterial != (UnityEngine.Object) this.overlayMaterial))
        return;
      this.overlayRenderer.sharedMaterial = this.overlayMaterial;
    }
  }

  public void SetSelected(CountryView.EOverlayState state = CountryView.EOverlayState.SELECTED)
  {
    this.isSelected = state == CountryView.EOverlayState.SELECTED;
    this.overlay.SetActive(state != 0);
    if (state == CountryView.EOverlayState.OFF)
      return;
    this.CheckOverlay();
    Color color = Color.white;
    if (state == CountryView.EOverlayState.SELECTED)
      color = CInterfaceManager.instance.currentAAHighlightSet.selected;
    else if (state == CountryView.EOverlayState.VALID)
      color = CInterfaceManager.instance.currentAAHighlightSet.valid;
    else if (state == CountryView.EOverlayState.P2_INTENT)
      color = CInterfaceManager.instance.currentAAHighlightSet.p2Intent;
    else if (state == CountryView.EOverlayState.P2)
      color = CInterfaceManager.instance.currentAAHighlightSet.p2;
    else if (state == CountryView.EOverlayState.PORTAL_TARGET)
      color = CInterfaceManager.instance.currentAAHighlightSet.portalTarget;
    if (!(this.overlayColor != color))
      return;
    this.overlayPropertyBlock.SetColor("_GlowColor", color);
    this.overlayColor = color;
    this.overlayRenderer.SetPropertyBlock(this.overlayPropertyBlock);
  }

  private void UpdateBorderColor()
  {
    if (this.country == null)
      return;
    if (CGameManager.localPlayerInfo != null && CGameManager.localPlayerInfo.disease != null && CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.CURE)
    {
      LocalDisease localDisease = CGameManager.localPlayerInfo.disease.GetLocalDisease(this.country);
      if ((double) this.country.localDanger >= 1.0 && !this.borderLockdownDisplayed)
      {
        this.borderLockdownDisplayed = true;
        this.StartCoroutine(this.PulseBordersCo(this.lockdownColor));
      }
      else if ((double) this.country.localDanger < 1.0)
      {
        if (this.borderLockdownDisplayed)
        {
          if (this.borderCloseDisplayed)
            this.StartCoroutine(this.PulseBordersCo(this.borderCloseColor));
          else
            this.StartCoroutine(this.PulseBordersCo(Color.clear));
        }
        this.borderLockdownDisplayed = false;
      }
      if (!this.country.borderStatus && !this.borderCloseDisplayed && !this.borderLockdownDisplayed)
      {
        this.borderCloseDisplayed = true;
        this.StartCoroutine(this.PulseBordersCo(this.borderCloseColor));
      }
      if (!this.unrestDisplayed)
      {
        if (!localDisease.unrestActive)
          return;
        this.unrestDisplayed = true;
        this.BeginPulseBorders(this.unrestColor);
      }
      else
      {
        if (!this.unrestDisplayed || localDisease.unrestActive)
          return;
        this.StopPulseBorders();
        this.unrestDisplayed = false;
      }
    }
    else
    {
      if (!this.country.borderStatus && !this.borderCloseDisplayed && !this.borderLockdownDisplayed)
      {
        this.borderCloseDisplayed = true;
        this.StartCoroutine(this.PulseBordersCo(this.borderCloseColor));
      }
      if (!this.apeInfectDisplayed && CGameManager.localPlayerInfo != null && CGameManager.localPlayerInfo.disease != null && CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        LocalDisease localDisease = CGameManager.localPlayerInfo.disease.GetLocalDisease(this.country);
        if (localDisease != null && localDisease.apeInfectedPopulation > 1L)
        {
          this.apeInfectDisplayed = true;
          this.StartCoroutine(this.PulseBordersCo(new Color(0.0f, 0.784313738f, 0.0f, 1f)));
        }
      }
      if (!this.bloodRageDisplayed)
      {
        if (CGameManager.localPlayerInfo == null || CGameManager.localPlayerInfo.disease == null || CGameManager.localPlayerInfo.disease.diseaseType != Disease.EDiseaseType.VAMPIRE)
          return;
        LocalDisease localDisease = CGameManager.localPlayerInfo.disease.GetLocalDisease(this.country);
        if (localDisease == null || localDisease.consumeFlag <= 0)
          return;
        this.BloodRageEffect();
      }
      else
      {
        if (!this.bloodRageDisplayed || CGameManager.localPlayerInfo == null || CGameManager.localPlayerInfo.disease == null || CGameManager.localPlayerInfo.disease.diseaseType != Disease.EDiseaseType.VAMPIRE)
          return;
        LocalDisease localDisease = CGameManager.localPlayerInfo.disease.GetLocalDisease(this.country);
        if (localDisease == null || localDisease.consumeFlag >= 1)
          return;
        this.BloodRageEffectEnd();
      }
    }
  }

  public void SetApeBorderState(bool state)
  {
    IPlayerInfo localPlayerInfo = CGameManager.localPlayerInfo;
    Disease disease = localPlayerInfo.disease;
    if (localPlayerInfo == null || disease == null || disease.diseaseType != Disease.EDiseaseType.SIMIAN_FLU)
      return;
    LocalDisease localDisease = disease.GetLocalDisease(this.country);
    if (localDisease == null)
      return;
    Debug.Log((object) ("Setting Border " + this.country.name + "  to :" + state.ToString()));
    this.normalBorderColor = !(localDisease.apeInfectedPopulation > 1L & state) ? Color.clear : (Color) new Color32((byte) 0, (byte) 200, (byte) 0, byte.MaxValue);
    if (this.borderClosePulsing || this.fortBorder)
      return;
    this.GetComponent<Renderer>().material.SetColor("_BorderColor", this.normalBorderColor);
  }

  private IEnumerator RagePulseBorders(Color baseCol, float speed = 1f)
  {
    CountryView countryView = this;
    countryView.borderClosePulsing = true;
    countryView.borderPulseSpeed = speed;
    float a = 0.0f;
    Material m = countryView.GetComponent<Renderer>().material;
label_5:
    while (countryView.bloodRageDisplayed)
    {
      while ((double) a < 1.0 && countryView.bloodRageDisplayed)
      {
        a += Time.deltaTime * countryView.borderPulseSpeed;
        Color color = baseCol with { a = baseCol.a * a };
        m.SetColor("_BorderColor", color);
        yield return (object) null;
      }
      while (true)
      {
        if ((double) a > 0.0 && countryView.bloodRageDisplayed)
        {
          a -= Time.deltaTime * countryView.borderPulseSpeed;
          Color color = baseCol with { a = baseCol.a * a };
          m.SetColor("_BorderColor", color);
          yield return (object) null;
        }
        else
          goto label_5;
      }
    }
    m.SetColor("_BorderColor", countryView.normalBorderColor);
    countryView.borderClosePulsing = false;
  }

  public void BeginPulseBorders(Color baseCol, float speed = 1f)
  {
    this.isContinuousPulse = true;
    this.StartCoroutine(this.ContinuousPulseBorders(baseCol, speed));
  }

  public void StopPulseBorders() => this.isContinuousPulse = false;

  private void SetBorderColor(Color c)
  {
    this.mainPropertyBlock.SetColor("_BorderColor", c);
    this.mainRenderer.SetPropertyBlock(this.mainPropertyBlock);
    this.intelPropertyBlock.SetColor("_BorderColor", c);
    this.intelRenderer.SetPropertyBlock(this.intelPropertyBlock);
  }

  private IEnumerator ContinuousPulseBorders(Color baseCol, float speed = 1f)
  {
    this.borderClosePulsing = true;
    this.borderPulseSpeed = speed;
    float a = 0.0f;
    Material mainMaterial = this.mainMaterial;
label_9:
    while (this.isContinuousPulse)
    {
      while ((double) a < 1.0 && this.isContinuousPulse)
      {
        a += Time.deltaTime * this.borderPulseSpeed;
        if (!this.isSelected)
          this.SetBorderColor(baseCol with
          {
            a = baseCol.a * a
          });
        yield return (object) null;
      }
      while (true)
      {
        if ((double) a > 0.0 && this.isContinuousPulse)
        {
          a -= Time.deltaTime * this.borderPulseSpeed;
          if (!this.isSelected)
            this.SetBorderColor(baseCol with
            {
              a = baseCol.a * a
            });
          yield return (object) null;
        }
        else
          goto label_9;
      }
    }
    this.SetBorderColor(this.normalBorderColor);
    this.borderClosePulsing = false;
  }

  private IEnumerator PulseBordersCo(Color baseCol, float speed = 1f)
  {
    this.borderClosePulsing = true;
    this.borderPulseSpeed = speed;
    float a = 0.0f;
    while ((UnityEngine.Object) CUIManager.instance.GetCurrentScreen() != (UnityEngine.Object) CHUDScreen.instance)
      yield return (object) null;
    while ((double) a < 1.0)
    {
      a += Time.deltaTime * this.borderPulseSpeed;
      this.SetBorderColor(baseCol with { a = baseCol.a * a });
      yield return (object) null;
    }
    while ((double) a > 0.0)
    {
      a -= Time.deltaTime * this.borderPulseSpeed;
      this.SetBorderColor(baseCol);
      yield return (object) null;
    }
    this.SetBorderColor(this.normalBorderColor);
    this.borderClosePulsing = false;
  }

  public Country GetCountry() => this.country;

  public static int GetCountryState(Country c)
  {
    int countryState = 0;
    if ((double) c.deadPercent > 0.99900001287460327 || c.isDestroyed)
      countryState = 6;
    else if ((double) c.deadPercent > 0.699999988079071)
      countryState = 5;
    else if ((double) c.publicOrder < 0.30000001192092896)
      countryState = 4;
    else if ((double) c.publicOrder < 0.60000002384185791)
      countryState = 3;
    else if ((double) c.publicOrder < 0.89999997615814209)
      countryState = 2;
    else if ((double) c.infectedPercent > 0.10000000149011612)
    {
      if (c is MPCountry)
      {
        if ((double) c.GetLocalDisease(CGameManager.localPlayerInfo.disease).infectedPercent > 0.10000000149011612)
          countryState = 1;
      }
      else
        countryState = 1;
    }
    bool flag = false;
    for (int index = 0; index < World.instance.diseases.Count; ++index)
    {
      if (World.instance.diseases[index].diseaseNoticed)
      {
        flag = true;
        break;
      }
    }
    if (!flag && countryState > 1)
      countryState = 1;
    if (CGameManager.localPlayerInfo.disease != null && CGameManager.localPlayerInfo.disease.isCure && !c.GetLocalDisease(CGameManager.localPlayerInfo.disease).hasIntel)
      countryState = 0;
    return countryState;
  }

  public void AddSpreadWave(Vector3 position, int diseaseID)
  {
    this.GetInfectionSystem().AddWave(position, diseaseID);
  }

  public static CountryView GetCountryAt(Vector3 pos)
  {
    foreach (RaycastHit raycastHit in Physics.RaycastAll(new Ray(pos - Vector3.forward * 10f, Vector3.forward), 20f))
    {
      CountryView component = raycastHit.collider.GetComponent<CountryView>();
      if ((bool) (UnityEngine.Object) component)
        return component;
    }
    return (CountryView) null;
  }

  public void BloodRageEffect()
  {
    this.bloodRageDisplayed = true;
    this.StartCoroutine(this.RagePulseBorders(new Color(0.784313738f, 0.0f, 0.0f, 1f)));
    if (this.bloodRageParticleEmitters == null)
      this.bloodRageParticleEmitters = this.bloodRageParticles.GetComponentsInChildren<ParticleSystem>();
    for (int index = 0; index < this.bloodRageParticleEmitters.Length; ++index)
      this.bloodRageParticleEmitters[index].enableEmission = true;
  }

  public void BloodRageEffectEnd(bool clear = false)
  {
    if (this.bloodRageParticleEmitters == null)
      this.bloodRageParticleEmitters = this.bloodRageParticles.GetComponentsInChildren<ParticleSystem>();
    for (int index = 0; index < this.bloodRageParticleEmitters.Length; ++index)
    {
      this.bloodRageParticleEmitters[index].enableEmission = false;
      if (clear)
        this.bloodRageParticleEmitters[index].Clear();
    }
    this.bloodRageDisplayed = false;
  }

  public void ApeRampageEffect(long amount)
  {
    this.apeRampageParticles.StartEffect(100000L, this.area);
  }

  public void ApeStealthEffect(long amount)
  {
    this.reanimateParticles.StartEffect(1000L, this.area);
  }

  public void ReanimateEffect(long amount)
  {
    this.reanimateParticles.StartEffect(amount, this.area);
  }

  public void ImmuneShockEffect(Vector3 pos, bool isMine = true)
  {
    MultiplayerGame game = (MultiplayerGame) CGameManager.game;
    CInterfaceManager.instance.CreateImmuneShockObject(pos, isMine, this, (float) ((MPDisease) game.GetMyDisease()).immuneShockCounterMax);
  }

  public void NukeStrikeEffect(Vector3 pos, bool isMine = true)
  {
    CInterfaceManager.instance.CreateNukeExplosion(pos, isMine, this, 10f);
  }

  public void BenignMimicEffect(Vector3 pos, bool isMine = true)
  {
    MultiplayerGame game = (MultiplayerGame) CGameManager.game;
    CInterfaceManager.instance.CreateBenignMimicObject(pos, isMine, this, (float) ((MPDisease) game.GetMyDisease()).benignCounterMax);
  }

  public void InfectBoostEffect(Vector3 pos, bool isMine = true)
  {
    CooperativeGame game = (CooperativeGame) CGameManager.game;
    CInterfaceManager.instance.CreateInfectBoostObject(pos, isMine, this, (float) ((CoopDisease) game.GetMyDisease()).infectBoostCounterMax);
  }

  public void LethalBoostEffect(Vector3 pos, bool isMine = true)
  {
    CooperativeGame game = (CooperativeGame) CGameManager.game;
    CInterfaceManager.instance.CreateLethalBoostObject(pos, isMine, this, (float) ((CoopDisease) game.GetMyDisease()).lethalBoostCounterMax);
  }

  public void Cleanup()
  {
    if ((bool) (UnityEngine.Object) this.system)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.system.gameObject);
      this.mainPropertyBlock.SetColor("_BorderColor", Color.clear);
      this.mainPropertyBlock.SetColor("_InfectColor", Color.clear);
      this.mainRenderer.SetPropertyBlock(this.mainPropertyBlock);
      this.borderCloseDisplayed = false;
    }
    if ((UnityEngine.Object) this.mpCureHQ != (UnityEngine.Object) null)
    {
      this.mpCureHQ.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpCureHQ.gameObject);
    }
    if ((UnityEngine.Object) this.mpFort != (UnityEngine.Object) null)
    {
      this.mpFort.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpFort.gameObject);
    }
    if ((UnityEngine.Object) this.mpNexus != (UnityEngine.Object) null)
    {
      this.mpNexus.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mpNexus.gameObject);
    }
    if ((UnityEngine.Object) this.cureStateObject != (UnityEngine.Object) null)
    {
      this.cureStateObject.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.cureStateObject.gameObject);
    }
    this.country = (Country) null;
  }

  public InfectionParticleSystem GetInfectionSystem(bool create = true)
  {
    if (!(bool) (UnityEngine.Object) this.system & create)
    {
      this.system = UnityEngine.Object.Instantiate<InfectionParticleSystem>(EffectsManager.instance.infectionSystemPrefab, this.colliderMesh.transform.position, this.transform.rotation);
      this.system.name = this.system.name.Replace("Clone", this.countryID);
      this.system.countryView = this;
      this.system.Initialise();
      this.system.transform.parent = this.transform;
      this.system.emitMesh = this.colliderMesh;
    }
    return this.system;
  }

  public void UpdateSpreadWaves(bool presim = false)
  {
    if (this.country.totalInfected <= 0L && this.country.totalDead <= 0L && this.country.totalZombie <= 0L)
      return;
    Color color1 = Color.clear;
    float num1 = 0.2f;
    foreach (Disease disease in World.instance.diseases)
    {
      LocalDisease localDisease = disease.GetLocalDisease(this.country);
      if (!(bool) (UnityEngine.Object) this.system || !this.system.HasWaves(disease.id))
      {
        if ((double) localDisease.infectedPercent > 0.25 || disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU && localDisease.infectedPopulation > 10L && localDisease.apeInfectedPopulation > 1L || disease.isCure && (double) localDisease.infectedPercent > 0.0)
        {
          this.AddSpreadWave(this.GetRandomPositionInsideCountry(), localDisease.disease.id);
          this.GetInfectionSystem().UpdateSystem(localDisease);
        }
      }
      else
        this.system.UpdateSystem(localDisease);
    }
    List<LocalDisease> sorted = new List<LocalDisease>((IEnumerable<LocalDisease>) this.country.localDiseases);
    sorted.Sort(new Comparison<LocalDisease>(this.InfectionControlSort));
    if (sorted.Count <= 0)
      return;
    LocalDisease localDisease1 = sorted[0];
    float num2 = (float) ((double) localDisease1.zombiePercent + (double) localDisease1.infectedPercent + (double) localDisease1.deadPercent / (double) World.instance.diseases.Count);
    if ((double) num2 > (double) num1)
    {
      DiseaseColourSet diseaseColourSet = CGameManager.game.GetColourSet(localDisease1.disease);
      if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
        diseaseColourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.country, this.country.GetLocalDisease(World.instance.diseases[0]));
      Color color2 = Color.Lerp(diseaseColourSet.infectedColour, diseaseColourSet.deadColour, localDisease1.zombiePercent + localDisease1.deadPercent) with
      {
        a = Mathf.Min((float) ((double) this.maxOverlayAlpha / (1.0 - (double) num1) * ((double) num2 - (double) num1)), this.maxOverlayAlpha)
      };
      if ((double) color2.a > (double) this.minOverlayAlpha)
        color1 = color2;
    }
    if ((UnityEngine.Object) this.system != (UnityEngine.Object) null)
      this.system.UpdateDiseaseOverlays(sorted, this.country.deadPercent);
    this.mainPropertyBlock.SetColor("_InfectColor", color1);
    this.mainRenderer.SetPropertyBlock(this.mainPropertyBlock);
  }

  private IEnumerator CountryUpdate()
  {
    while (true)
    {
      if (this.country != null && World.instance != null)
      {
        if (this.restartingFromReconnect && this.country.diseaseNexus != null)
          this.AddSpreadWave(this.GetRandomPositionInsideCountry(), this.country.diseaseNexus.id);
        while (World.instance != null && this.lastDiseaseTurn == World.instance.DiseaseTurn)
          yield return (object) null;
        while ((UnityEngine.Object) CGameManager.game == (UnityEngine.Object) null || CGameManager.game.CurrentGameState == IGame.GameState.ChoosingCountry)
          yield return (object) null;
        if (World.instance != null && this.country != null)
        {
          this.lastDiseaseTurn = World.instance.DiseaseTurn;
          if (this.country.portStatus != this.mbSeaStatus)
            this.SetSeaportState(this.country.portStatus);
          if (this.country.airportStatus != this.mbAirStatus)
            this.SetAirportState(this.country.airportStatus);
          this.UpdateBorderColor();
          int countryState = CountryView.GetCountryState(this.country);
          if (countryState != this.lastState)
          {
            this.lastState = countryState;
            CInterfaceManager.instance.CountryStateChanged(this.country);
          }
          this.UpdateSpreadWaves();
          if (CGameManager.IsMultiplayerGame)
          {
            for (int index = 0; index < this.mpVehicleDiseaseTrails.Count; ++index)
              this.mpVehicleDiseaseTrails[index].TrailUpdate();
          }
          this.CheckOverlay();
          foreach (Disease disease in World.instance.diseases)
          {
            if (disease.isCure)
            {
              LocalDisease localDisease = this.country.GetLocalDisease(disease);
              this.SetCureIconState(localDisease.supportActive ? (float) localDisease.supportTimer / (float) disease.supportTimerMAX : 0.0f, (float) localDisease.tempLockdownTimer / (float) disease.lockdownTimerMAX, localDisease.unrestActive, localDisease.fireComplianceIcon, localDisease.complianceIconScale);
              this.SetHQState(localDisease.isHeadquarters);
              bool flag = !localDisease.hasIntel;
              if (flag != this.intelOverlay.activeSelf)
              {
                this.intelPropertyBlock.SetColor("_Tint", disease.easyIntel ? InfectionPlaneContainer.instance.intelEasyTint : InfectionPlaneContainer.instance.intelHardTint);
                this.intelPropertyBlock.SetColor("_Blend", disease.easyIntel ? InfectionPlaneContainer.instance.intelEasyBlend : InfectionPlaneContainer.instance.intelHardBlend);
                this.intelPropertyBlock.SetFloat("_Desaturate", disease.easyIntel ? InfectionPlaneContainer.instance.intelEasyDesaturate : InfectionPlaneContainer.instance.intelHardDesaturate);
                this.intelRenderer.SetPropertyBlock(this.intelPropertyBlock);
                this.intelOverlay.SetActive(flag);
              }
            }
          }
        }
      }
      yield return (object) null;
    }
  }

  private void Update()
  {
    if (this.country == null || CGameManager.localPlayerInfo.disease == null)
      return;
    LocalDisease localDisease = this.country.GetLocalDisease(CGameManager.localPlayerInfo.disease);
    if ((UnityEngine.Object) this.mpLab != (UnityEngine.Object) null)
      this.mpLab.SetAttackedState(localDisease.apeStatusRampage == 1);
    if (CGameManager.localPlayerInfo.disease.isCure)
    {
      if (CGameManager.localPlayerInfo.disease.easyIntel || localDisease.hasIntel)
      {
        if (this.country.dispatchedHiddenInfectedFlights.Count > 0)
        {
          foreach (Country hiddenInfectedFlight in this.country.dispatchedHiddenInfectedFlights)
            this.DrawFlightPath(this.country, hiddenInfectedFlight, CGameManager.localPlayerInfo.disease);
          this.country.dispatchedHiddenInfectedFlights.Clear();
        }
        if (this.country.dispatchedHiddenInfectedBoats.Count > 0)
        {
          foreach (Country hiddenInfectedBoat in this.country.dispatchedHiddenInfectedBoats)
            this.DrawBoatPath(this.country, hiddenInfectedBoat, CGameManager.localPlayerInfo.disease);
          this.country.dispatchedHiddenInfectedBoats.Clear();
        }
      }
      if (!this.lockdownBorder)
      {
        if ((double) this.country.localDanger < 1.0)
          return;
        this.lockdownBorder = true;
        this.normalBorderColor = this.lockdownColor;
        if (this.borderClosePulsing || this.unrestDisplayed)
          return;
        this.SetBorderColor(this.normalBorderColor);
      }
      else
      {
        if (!this.lockdownBorder || (double) this.country.localDanger >= 1.0)
          return;
        this.lockdownBorder = false;
        this.normalBorderColor = Color.clear;
        if (this.borderClosePulsing || this.unrestDisplayed)
          return;
        this.SetBorderColor(this.normalBorderColor);
      }
    }
    else
    {
      if (!this.borderClosePulsing)
      {
        bool flag = this.country.HasApeRampage();
        if (((this.country.HasFort() ? 1 : (this.country.apeLabStatus == EApeLabState.APE_LAB_ACTIVE ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
          this.zComBorder += (float) this.borderFadeDirection * Time.deltaTime * CInterfaceManager.instance.zComBorderPulseSpeed;
          if ((double) this.zComBorder > 1.0)
          {
            this.zComBorder = 1f;
            this.borderFadeDirection = -1;
          }
          if ((double) this.zComBorder < 0.0)
          {
            this.zComBorder = 0.0f;
            this.borderFadeDirection = 1;
          }
          Color c = CInterfaceManager.instance.zComBorderColor;
          if (flag)
            c = CInterfaceManager.instance.apeRampageBorderColor;
          c.a *= (float) (0.75 + (double) this.zComBorder / 4.0);
          this.SetBorderColor(c);
          this.fortBorder = true;
        }
        else if (this.fortBorder)
        {
          this.SetBorderColor(this.normalBorderColor);
          this.fortBorder = false;
        }
      }
      bool flag1 = localDisease.apeInfectedPopulation > 0L;
      if (flag1 != this.hadApes && (UnityEngine.Object) CInterfaceManager.instance.SelectedCountryView != (UnityEngine.Object) this)
        CInterfaceManager.instance.SetCountryHighlight(this);
      this.hadApes = flag1;
    }
  }

  public void DrawFlightPath(Country source, Country destination, Disease d)
  {
    Vector3 position1 = CInterfaceManager.instance.GetCountryView(source.id).mpAirport.transform.position;
    Vector3 position2 = CInterfaceManager.instance.GetCountryView(destination.id).mpAirport.transform.position;
    float magnitude = (position2 - position1).magnitude;
    Vector3 normalized = (position2 - position1).normalized;
    Color infectedColour = CGameManager.game.GetColourSet(d.id).infectedColour;
    float num = 0.02f;
    while ((double) magnitude > 0.0)
    {
      magnitude -= num;
      position1 += normalized * num;
      DiseaseTrailParticles.instance.AddParticle(position1, infectedColour, d.id, Vehicle.EVehicleType.Airplane);
    }
  }

  public void DrawBoatPath(Country source, Country destination, Disease d)
  {
    Color infectedColour = CGameManager.game.GetColourSet(d.id).infectedColour;
    RouteBoat route = CInterfaceManager.instance.GetRoute(source.id, destination.id);
    bool bReverse = false;
    if ((UnityEngine.Object) route == (UnityEngine.Object) null)
    {
      route = CInterfaceManager.instance.GetRoute(destination.id, source.id);
      bReverse = true;
    }
    if ((UnityEngine.Object) route == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Unable to find route between: " + source.id + " and " + destination.id));
    }
    else
    {
      float num1 = 0.3f;
      float num2 = 0.0f;
      Vector3 vector3 = route.GetPositionFromSpline(0.0f, bReverse);
      float num3 = num1 * num1;
      route.CalculateDistances();
      int num4 = (int) ((double) route.Distance / (double) num1);
      for (int index = 0; index < num4; ++index)
      {
        float factor = (float) index * 1f / (float) num4;
        Vector3 positionFromSpline = route.GetPositionFromSpline(factor, bReverse);
        num2 += (positionFromSpline - vector3).sqrMagnitude;
        vector3 = positionFromSpline;
        if ((double) num2 > (double) num3)
        {
          num2 = 0.0f;
          DiseaseTrailParticles.instance.AddParticle(positionFromSpline, infectedColour, d.id, Vehicle.EVehicleType.Boat);
        }
      }
    }
  }

  private int InfectionControlSort(LocalDisease a, LocalDisease b)
  {
    if (a == b)
      return 0;
    if (a == null)
      return 1;
    return b == null ? -1 : a.controlledInfected.CompareTo(b.controlledInfected);
  }

  public Vector3 GetRandomPositionInsideCountry()
  {
    if (this.meshVertices == null)
      this.meshVertices = this.colliderMesh.mesh.vertices;
    if (this.meshTriangles == null)
      this.meshTriangles = this.colliderMesh.mesh.triangles;
    return this.colliderMesh.transform.TransformPoint(CUtils.GetRandomMeshPoint(this.meshTriangles, this.meshVertices));
  }

  public void SetSeaportState(bool isOpen)
  {
    this.mbSeaStatus = isOpen;
    if (this.mpSeaport == null)
      return;
    for (int index = 0; index < this.mpSeaport.Count; ++index)
      this.mpSeaport[index].SetState(isOpen);
  }

  public void PulseDiseaseSpreadWaves(Disease d)
  {
    if (!((UnityEngine.Object) this.system != (UnityEngine.Object) null))
      return;
    this.system.ForceBloom(this.country.GetLocalDisease(d));
  }

  public void SetAirportEnabled(bool isEnabled)
  {
    if ((UnityEngine.Object) this.mpAirport == (UnityEngine.Object) null)
      Debug.Log((object) "Null airport");
    else
      this.mpAirport.gameObject.SetActive(isEnabled);
  }

  public void SetAirportState(bool isOpen)
  {
    this.mbAirStatus = isOpen;
    if ((UnityEngine.Object) this.mpAirport == (UnityEngine.Object) null)
      Debug.Log((object) "Null airport");
    else
      this.mpAirport.SetState(isOpen);
  }

  public string Serialize(bool forNetworking)
  {
    if ((UnityEngine.Object) this.system == (UnityEngine.Object) null)
      return "";
    if (forNetworking)
      return "recalculate";
    StringBuilder stringBuilder = new StringBuilder();
    foreach (InfectionParticleSystem.SpreadWave wave in this.system.waves)
      stringBuilder.Append(CUtils.SerializeVector3(wave.position)).Append(":").Append(wave.dots).Append(":").Append(wave.diseaseID).Append("~");
    return stringBuilder.ToString() + "|" + this.system.SerializeDotCounts();
  }

  public void Unserialize(string str)
  {
    if (string.IsNullOrEmpty(str))
      return;
    if (str == "recalculate")
    {
      Debug.Log((object) "REACALCULATING STUFF");
      this.restartingFromReconnect = true;
    }
    else
    {
      string[] strArray1 = str.Split('|');
      string[] strArray2 = strArray1[0].Split('~');
      InfectionParticleSystem infectionSystem = this.GetInfectionSystem();
      for (int index = 0; index < strArray2.Length; ++index)
      {
        if (!string.IsNullOrEmpty(strArray2[index]))
        {
          string[] strArray3 = strArray2[index].Split(':');
          InfectionParticleSystem.SpreadWave spreadWave = InfectionParticleSystem.SpreadWave.Create();
          spreadWave.diseaseID = int.Parse(strArray3[2]);
          spreadWave.dots = int.Parse(strArray3[1]);
          spreadWave.position = CUtils.UnserializeVector3(strArray3[0]);
          infectionSystem.waves.Add(spreadWave);
        }
      }
      if (strArray1.Length <= 1)
        return;
      infectionSystem.UnserializeDotCounts(strArray1[1]);
    }
  }

  public void BufferRenderTextures()
  {
    if (!(bool) (UnityEngine.Object) this.system)
      return;
    this.system.BufferRenderTextures(true);
  }

  public void RestoreRenderTextures()
  {
    if (!(bool) (UnityEngine.Object) this.system)
      return;
    this.system.ReloadRenderTextures();
  }

  public void AttachNexus(NexusObject nexus)
  {
    if ((UnityEngine.Object) this.mpNexusLocation == (UnityEngine.Object) null)
    {
      this.mpNexusLocation = new GameObject("NexusLocationCreated");
      this.mpNexusLocation.transform.parent = this.transform;
      this.mpNexusLocation.transform.localPosition = Vector3.zero;
      this.mpNexusLocation.transform.localScale = Vector3.one;
    }
    nexus.transform.parent = this.mpNexusLocation.transform;
    nexus.transform.localPosition = new Vector3(0.0f, 0.0f, -0.2f);
    nexus.transform.localScale = Vector3.one;
    this.mpNexus = nexus;
    Vector3 position = this.mpNexusLocation.transform.position;
    if ((double) this.mpNexusLocation.transform.position.y > 6.0 && (double) this.mpNexusLocation.transform.position.x > -8.0 && (double) this.mpNexusLocation.transform.position.x < 8.0)
    {
      position.y = 6f;
      position.x -= 0.25f;
    }
    this.mpNexusLocation.transform.position = position;
  }

  public void AttachFort(FortObject fort)
  {
    if ((UnityEngine.Object) this.mpFortLocation == (UnityEngine.Object) null)
    {
      this.mpFortLocation = new GameObject("FortLocationCreated");
      this.mpFortLocation.transform.parent = this.transform;
      this.mpFortLocation.transform.position = Vector3.zero;
      this.mpFortLocation.transform.localScale = Vector3.one;
    }
    fort.transform.parent = this.mpFortLocation.transform;
    fort.transform.localPosition = -0.1f * Vector3.forward;
    fort.transform.localScale = Vector3.one;
    this.mpFort = fort;
    this.country.fortPosition = new Vector3?(fort.transform.position);
  }

  public void AttachLab(ApeLabObject lab)
  {
    if ((UnityEngine.Object) this.mpLabLocation == (UnityEngine.Object) null)
    {
      this.mpLabLocation = new GameObject("LabLocationCreated");
      this.mpLabLocation.transform.parent = this.transform;
      this.mpLabLocation.transform.localPosition = Vector3.zero;
      this.mpLabLocation.transform.localScale = Vector3.one;
    }
    lab.transform.parent = this.mpLabLocation.transform;
    lab.transform.localPosition = -0.1f * Vector3.forward;
    lab.transform.localScale = Vector3.one;
    this.mpLab = lab;
  }

  public void AttachColony(ApeColonyObject colony)
  {
    if ((UnityEngine.Object) this.mpColonyLocation == (UnityEngine.Object) null)
    {
      this.mpColonyLocation = new GameObject("ColonyLocationCreated");
      this.mpColonyLocation.transform.parent = this.transform;
      this.mpColonyLocation.transform.localPosition = Vector3.zero;
      this.mpColonyLocation.transform.localScale = Vector3.one;
    }
    colony.transform.parent = this.mpColonyLocation.transform;
    colony.transform.localPosition = -0.1f * Vector3.forward;
    colony.transform.localScale = Vector3.one;
    if (this.country.apeColonyPosition.HasValue)
      this.mpColonyLocation.transform.position = this.country.apeColonyPosition.Value;
    CGameManager.game.ResetApeColony(this.country, CGameManager.localPlayerInfo.disease);
  }

  public void AttachCastle(CastleObject castle)
  {
    if ((UnityEngine.Object) this.mpCastleLocation == (UnityEngine.Object) null)
    {
      this.mpCastleLocation = new GameObject("CastleLocationCreated");
      this.mpCastleLocation.transform.parent = this.transform;
      this.mpCastleLocation.transform.localPosition = Vector3.zero;
      this.mpCastleLocation.transform.localScale = Vector3.one;
    }
    this.mpCastleObject = castle;
    castle.transform.parent = this.mpCastleLocation.transform;
    castle.transform.localPosition = -0.1f * Vector3.forward;
    castle.transform.localScale = Vector3.one;
    if (this.country.localDiseases.Count <= 0)
      return;
    LocalDisease localDisease = this.country.localDiseases[0];
    if (!localDisease.castlePosition.HasValue)
      return;
    this.mpCastleLocation.transform.position = localDisease.castlePosition.Value;
  }

  public void AttachVampire(VampireObject vampireOb)
  {
    if ((UnityEngine.Object) this.mpVampireContainer == (UnityEngine.Object) null)
    {
      this.mpVampireContainer = new GameObject("VampireContainerCreated");
      this.mpVampireContainer.transform.parent = this.transform;
      this.mpVampireContainer.transform.localPosition = Vector3.zero;
      this.mpVampireContainer.transform.localScale = Vector3.one;
    }
    vampireOb.transform.parent = this.mpVampireContainer.transform;
    if (!vampireOb.vampire.currentPosition.HasValue)
      vampireOb.vampire.currentPosition = new Vector3?(this.GetRandomPositionInsideCountry());
    vampireOb.transform.position = vampireOb.vampire.currentPosition.Value;
    Vector3 localPosition = vampireOb.transform.localPosition with
    {
      z = 0.1f
    };
    vampireOb.transform.localPosition = localPosition;
    vampireOb.transform.localScale = Vector3.one;
  }

  public void AttachCoopLab(CoopLabObject lab)
  {
    if ((UnityEngine.Object) this.coopLabLocation == (UnityEngine.Object) null)
    {
      this.coopLabLocation = new GameObject("LabLocationCreated");
      this.coopLabLocation.transform.parent = this.transform;
      this.coopLabLocation.transform.localPosition = Vector3.zero;
      this.coopLabLocation.transform.localScale = Vector3.one;
    }
    lab.transform.parent = this.coopLabLocation.transform;
    lab.transform.localPosition = -0.1f * Vector3.forward;
    lab.transform.localScale = Vector3.one;
    this.coopLab = lab;
    if (!((UnityEngine.Object) this.mpNexus != (UnityEngine.Object) null))
      return;
    float num1 = 0.31f;
    float num2 = Vector3.Distance(this.coopLabLocation.transform.position, this.mpNexusLocation.transform.position);
    if ((double) num2 >= (double) num1)
      return;
    Vector3 vector3;
    if (this.transform.position == this.mpNexusLocation.transform.position)
    {
      Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
      vector3 = num1 * new Vector3(normalized.x, normalized.y, 0.0f);
    }
    else
      vector3 = (num1 - num2) * (this.transform.position - this.mpNexusLocation.transform.position).normalized;
    this.coopLabLocation.transform.position = this.mpNexusLocation.transform.position + vector3;
  }

  public void UpdateColonyPosition(ApeColonyObject colony)
  {
    if (!this.country.apeColonyPosition.HasValue)
      return;
    this.mpColonyLocation.transform.position = this.country.apeColonyPosition.Value;
  }

  public void UpdateCastlePosition(CastleObject colony)
  {
    if (this.country.localDiseases.Count <= 0)
      return;
    LocalDisease localDisease = this.country.localDiseases[0];
    if (!localDisease.castlePosition.HasValue)
      return;
    this.mpCastleLocation.transform.position = localDisease.castlePosition.Value;
  }

  public void SetCureIconState(
    float internationalAid,
    float quarantine,
    bool unrestActive,
    bool pulseCompliance,
    float complianceScale)
  {
    CureStateObject.ECureState s = CureStateObject.ECureState.None;
    if (unrestActive)
      s = CureStateObject.ECureState.Unrest;
    else if ((double) quarantine > 0.0)
      s = CureStateObject.ECureState.Lockdown;
    if (s != 0 | pulseCompliance && !(bool) (UnityEngine.Object) this.cureStateObject)
    {
      this.cureStateObject = UnityEngine.Object.Instantiate<CureStateObject>(CInterfaceManager.instance.cureStatePrefab);
      this.cureStateObject.transform.parent = this.mpFortLocation.transform;
      this.cureStateObject.transform.localPosition = -0.1f * Vector3.forward;
      this.cureStateObject.transform.localScale = Vector3.one;
      this.cureStateObject.name = this.countryID + " cure state";
      CInterfaceManager.instance.mpScaleObjects.Add((ScaleObject) this.cureStateObject);
    }
    if (!(bool) (UnityEngine.Object) this.cureStateObject)
      return;
    this.cureStateObject.SetCureState(s, pulseCompliance, complianceScale / 50f);
    this.cureStateObject.RefreshElapsedLockdownTime(quarantine);
  }

  public void SetHQState(bool isHQ)
  {
  }

  public enum EOverlayState
  {
    OFF,
    SELECTED,
    VALID,
    P2_INTENT,
    P2,
    PORTAL_TARGET,
  }
}
