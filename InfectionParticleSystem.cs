// Decompiled with JetBrains decompiler
// Type: InfectionParticleSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InfectionParticleSystem : MonoBehaviour
{
  private const int DEAD_PEOPLE_PER_DOT_MULTIPLIER = 4;
  public MeshFilter emitMesh;
  public DiseaseColourSet colourSet;
  private int colourRatioTotal;
  public float minSize;
  public float maxSize;
  private IDictionary<int, int> particleCounter = (IDictionary<int, int>) new Dictionary<int, int>();
  private IDictionary<int, int> deadParticleCounter = (IDictionary<int, int>) new Dictionary<int, int>();
  private long numDotsNeeded;
  private long numDeadDotsNeeded;
  private const int numHiddenBuckets = 4;
  private float cureDecayFade = 1f;
  private float cureDecayAlpha = 1f;
  private TemporalBuckets dotsOverTime = new TemporalBuckets(10);
  private TemporalBuckets deadDotsOverTime = new TemporalBuckets(10);
  public List<InfectionParticleSystem.SpreadWave> waves = new List<InfectionParticleSystem.SpreadWave>();
  public List<InfectionParticleSystem.SpreadWave> deadWaves = new List<InfectionParticleSystem.SpreadWave>();
  public GameObject overlayPrefab;
  private GameObject[] overlays;
  private GameObject bloomOverlay;
  private GameObject deadOverlay;
  private GameObject mpStateOverlay;
  private Color32[] pixels;
  private int textureSize;
  private float textureScale = 1f;
  public Texture2D clearTexture;
  public Texture2D particleTexture;
  public Texture2D deadParticleTexture;
  public Color colorMultipleInfectedMPOverlay;
  public Color colorMineInfectedMPOverlay;
  public Color colorOtherInfectedMPOverlay;
  public Color colorDeadMPOverlay;
  private int particleWidth;
  private float particleScale;
  private int deadParticleWidth;
  private float deadParticleScale;
  public int resolution = 6;
  public int maxResolution = 11;
  private bool useDeadOverlay;
  private float boundSize;
  private Vector3 boundsOffset;
  private float spreadMult;
  public Material bloomMaterial;
  public Material deadMaterial;
  public Material mpStateMaterial;
  private float bloomSpeed = 1f;
  private float bloomTime;
  private const float CYCLE_DURATION = 10f;
  private const float CYCLE_GAP = 0.0f;
  public float mpOverlayFadeOutSpeed = 1f;
  public float mpOverlayFadeInSpeed = 1f;
  private float overlayFade;
  private InfectionParticleSystem.MPOverlayState targetMPOverlayState;
  private InfectionParticleSystem.MPOverlayState lastMPOverlayState;
  private Color lastMPOverlayColor;
  private Color targetMPOverlayColor;
  private Vector3 targetMPOverlayPosition;
  public CountryView countryView;
  private long[] infectedChange;
  private long[] infectedAtLastUpdate;
  private long[] deadChange;
  private long[] deadAtLastUpdate;
  private const float SPREAD_DISTANCE = 0.8f;
  private HashSet<int> diseases = new HashSet<int>();
  public bool debugMe;
  private int colourDiseaseID = -1;
  private LocalDisease localDisease;
  private List<InfectionParticleSystem.ParticleInfo> particleQueue = new List<InfectionParticleSystem.ParticleInfo>();
  private List<InfectionParticleSystem.ParticleInfo> deadParticleQueue = new List<InfectionParticleSystem.ParticleInfo>();
  private float queueCount;
  private const int MAX_PARTICLES_PER_COUNTRY_PER_FRAME = 100;
  private const long MAX_TICKS_PER_FRAME = 25000;
  private static float countryParticleBias = 1f;
  private const int GLOBAL_PARTICLES_PER_FRAME = 500;
  private static int globalParticlesThisFrame = 0;
  private static int frameNumber = -1;
  private static long ticksThisFrame = 0;
  private RenderTexture deadRenderTexture;
  private RenderTexture[] renderTextures;
  private float lastBufferTime;
  private Texture2D infectionTex;
  private Texture2D[] baseInfectionTextures;
  private Texture2D baseDeadTexture;
  private Texture2D deadBuffer;
  private bool deadBufferActive;
  private Texture2D[] buffers;
  private bool[] bufferedRTs;
  private Texture2D tempBuffer;
  private bool reloading;

  public void Clear()
  {
    if (this.renderTextures != null)
    {
      for (int index = 0; index < this.renderTextures.Length; ++index)
      {
        if ((UnityEngine.Object) RenderTexture.active == (UnityEngine.Object) this.renderTextures[index])
          RenderTexture.active = (RenderTexture) null;
        this.renderTextures[index].Release();
      }
    }
    if ((bool) (UnityEngine.Object) this.deadRenderTexture)
    {
      if ((UnityEngine.Object) RenderTexture.active == (UnityEngine.Object) this.deadRenderTexture)
        RenderTexture.active = (RenderTexture) null;
      this.deadRenderTexture.Release();
    }
    if (this.buffers == null)
      return;
    for (int index = 0; index < this.buffers.Length; ++index)
    {
      if ((UnityEngine.Object) this.buffers[index] != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.buffers[index]);
    }
    this.buffers = (Texture2D[]) null;
  }

  public void Initialise()
  {
    this.Clear();
    Bounds bounds = this.countryView.countryMesh.GetComponent<Renderer>().bounds;
    this.boundSize = Mathf.Max(bounds.size.x, bounds.size.y);
    this.boundsOffset = new Vector3((float) (((double) bounds.size.x - (double) this.boundSize) / 2.0 + (double) this.boundSize / 2.0), (float) (((double) bounds.size.y - (double) this.boundSize) / 2.0 + (double) this.boundSize / 2.0), 0.0f);
    this.textureSize = (int) Mathf.Pow(2f, Mathf.Min((float) this.maxResolution, Mathf.Round(Mathf.Log(this.boundSize, 2f)) + (float) this.resolution));
    this.textureScale = (float) this.textureSize / this.boundSize;
    if (this.overlays == null || this.overlays.Length != World.instance.diseases.Count)
      this.overlays = new GameObject[World.instance.diseases.Count];
    if (this.infectedChange == null || this.infectedChange.Length != World.instance.diseases.Count)
      this.infectedChange = new long[World.instance.diseases.Count];
    if (this.infectedAtLastUpdate == null || this.infectedAtLastUpdate.Length != World.instance.diseases.Count)
      this.infectedAtLastUpdate = new long[World.instance.diseases.Count];
    if (this.deadChange == null || this.deadChange.Length != World.instance.diseases.Count)
      this.deadChange = new long[World.instance.diseases.Count];
    if (this.deadAtLastUpdate == null || this.deadAtLastUpdate.Length != World.instance.diseases.Count)
      this.deadAtLastUpdate = new long[World.instance.diseases.Count];
    if (this.renderTextures == null || this.renderTextures.Length != this.overlays.Length)
      this.renderTextures = new RenderTexture[this.overlays.Length];
    this.waves.Clear();
    this.deadWaves.Clear();
    this.particleWidth = this.particleTexture.width;
    this.particleScale = (float) this.particleWidth / this.textureScale;
    this.deadParticleWidth = this.deadParticleTexture.width;
    this.deadParticleScale = (float) this.deadParticleWidth / this.textureScale;
    this.spreadMult = Mathf.Clamp(this.countryView.area / 5000f, 1f, 2f);
    GameObject gameObject1 = InfectionPlaneContainer.instance.GetPlane(this.countryView.name).gameObject;
    this.transform.position = gameObject1.transform.position;
    this.transform.rotation = gameObject1.transform.rotation;
    for (int index = 0; index < this.overlays.Length; ++index)
    {
      GameObject gameObject2 = this.overlays[index];
      if (!(bool) (UnityEngine.Object) gameObject2)
      {
        gameObject2 = this.overlays[index] = UnityEngine.Object.Instantiate<GameObject>(gameObject1);
        gameObject2.transform.localPosition = new Vector3(0.0f, 0.01f, 0.0f);
        gameObject2.name = "infection overlay " + this.countryView.name + " Disease " + (object) index;
      }
      gameObject2.SetActive(true);
      RenderTexture dest = this.renderTextures[index];
      if (!(bool) (UnityEngine.Object) dest)
      {
        dest = this.renderTextures[index] = new RenderTexture(this.textureSize, this.textureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        dest.useMipMap = false;
      }
      if (this.baseInfectionTextures != null && this.baseInfectionTextures.Length > index)
        Graphics.Blit((Texture) this.baseInfectionTextures[index], dest);
      else
        Graphics.Blit((Texture) this.clearTexture, dest);
      gameObject2.GetComponent<Renderer>().material.mainTexture = (Texture) dest;
      gameObject2.transform.parent = this.transform;
      gameObject2.transform.localPosition = Vector3.zero;
      gameObject2.transform.localRotation = Quaternion.identity;
    }
    this.useDeadOverlay = CGameManager.IsMultiplayerGame || CGameManager.IsCureGame;
    if (this.useDeadOverlay)
    {
      if (!(bool) (UnityEngine.Object) this.deadOverlay)
      {
        this.deadOverlay = UnityEngine.Object.Instantiate<GameObject>(gameObject1);
        this.deadOverlay.name = "dead overlay " + this.countryView.name;
      }
      RenderTexture dest = this.deadRenderTexture;
      if (!(bool) (UnityEngine.Object) dest)
      {
        dest = this.deadRenderTexture = new RenderTexture(this.textureSize, this.textureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        dest.useMipMap = false;
      }
      if ((UnityEngine.Object) this.baseDeadTexture != (UnityEngine.Object) null)
        Graphics.Blit((Texture) this.baseDeadTexture, dest);
      else
        Graphics.Blit((Texture) this.clearTexture, dest);
      this.deadOverlay.GetComponent<Renderer>().material = this.deadMaterial;
      this.deadOverlay.GetComponent<Renderer>().material.mainTexture = (Texture) dest;
      this.deadOverlay.transform.parent = this.transform;
      this.deadOverlay.transform.localPosition = new Vector3(0.0f, 0.05f, 0.0f);
      this.deadOverlay.transform.localRotation = Quaternion.identity;
      if (!(bool) (UnityEngine.Object) this.mpStateOverlay && this.overlays.Length != 0 && (bool) (UnityEngine.Object) this.overlays[0])
      {
        this.mpStateOverlay = UnityEngine.Object.Instantiate<GameObject>(gameObject1);
        this.mpStateOverlay.name = "mp state overlay " + this.countryView.name;
        this.mpStateOverlay.GetComponent<Renderer>().material = this.mpStateMaterial;
        this.mpStateOverlay.transform.parent = this.transform;
        this.mpStateOverlay.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        this.mpStateOverlay.transform.localRotation = Quaternion.identity;
      }
      this.mpStateOverlay.SetActive(false);
    }
    if (!(bool) (UnityEngine.Object) this.bloomOverlay && this.overlays.Length != 0 && (bool) (UnityEngine.Object) this.overlays[0])
    {
      this.bloomOverlay = UnityEngine.Object.Instantiate<GameObject>(gameObject1);
      this.bloomOverlay.name = "infection overlay " + this.countryView.name + " bloom";
      this.bloomOverlay.GetComponent<Renderer>().material = this.bloomMaterial;
      this.bloomOverlay.GetComponent<Renderer>().material.mainTexture = this.overlays[0].GetComponent<Renderer>().material.mainTexture;
      this.bloomOverlay.transform.parent = this.transform;
      this.bloomOverlay.transform.localPosition = new Vector3(0.0f, 0.03f, 0.0f);
      this.bloomOverlay.transform.localRotation = Quaternion.identity;
    }
    this.bloomOverlay.SetActive(false);
    this.StartCoroutine(this.SystemBloomUpdate());
  }

  public void Bloom(LocalDisease ld, float duration, float speed, bool continuous)
  {
    IGame game = CGameManager.game;
    if ((UnityEngine.Object) game == (UnityEngine.Object) null)
      return;
    float num = 1f;
    if (CGameManager.IsMultiplayerGame)
    {
      this.bloomOverlay.GetComponent<Renderer>().material.SetFloat("_Scale", 4f);
      num = this.GetAlpha(ld);
    }
    else
    {
      if (ld.disease.isCure && (double) this.cureDecayFade < 1.0)
        num = 0.0f;
      this.bloomOverlay.GetComponent<Renderer>().material.SetFloat("_Scale", 2f);
    }
    this.bloomOverlay.GetComponent<Renderer>().material.mainTexture = this.overlays[ld.disease.id].GetComponent<Renderer>().material.mainTexture;
    this.bloomOverlay.GetComponent<Renderer>().material.SetFloat("_Duration", duration);
    if (!continuous)
    {
      this.bloomTime = 0.0f;
      this.bloomOverlay.GetComponent<Renderer>().material.SetFloat("_BloomTime", 0.0f);
    }
    this.bloomSpeed = speed;
    this.bloomOverlay.GetComponent<Renderer>().material.SetInt("_Continuous", continuous ? 1 : 0);
    DiseaseColourSet diseaseColourSet = game.GetColourSet(ld.disease);
    if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
      diseaseColourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
    Color color = Color.Lerp(Color.white, diseaseColourSet.deadColour, ld.deadPercent * 0.5f);
    if ((double) ld.deadPercent >= 1.0 && CGameManager.IsMultiplayerGame)
      color = diseaseColourSet.deadColour;
    if (ld.disease.isFakeNews)
      color = Color.white;
    color.a *= num;
    this.bloomOverlay.GetComponent<Renderer>().material.color = color;
    this.bloomOverlay.SetActive(true);
  }

  public void MultiPlayerToSinglePlayer()
  {
    this.bloomOverlay.SetActive(false);
    int id = CGameManager.localPlayerInfo.disease.id;
    bool flag = false;
    for (int index = 0; index < this.waves.Count; index = index - 1 + 1)
    {
      InfectionParticleSystem.SpreadWave wave = this.waves[index];
      if (wave.diseaseID == id)
      {
        flag = true;
        break;
      }
      this.waves.Remove(wave);
      wave.Destroy();
    }
    if (flag)
      return;
    Country country = this.countryView.GetCountry();
    long num = 0;
    for (int index = 0; index < country.localDiseases.Count; ++index)
    {
      if (country.localDiseases[index].diseaseID != id)
        num = country.localDiseases[index].infectedPopulation;
    }
    if (num <= 0L)
      return;
    this.AddWave(this.countryView.GetRandomPositionInsideCountry(), id);
  }

  private IEnumerator SystemBloomUpdate()
  {
    while (!((UnityEngine.Object) CGameManager.game == (UnityEngine.Object) null))
    {
      while ((UnityEngine.Object) this.countryView == (UnityEngine.Object) null)
        yield return (object) new WaitForSeconds(1f);
      Country c;
      for (c = this.countryView.GetCountry(); c == null; c = this.countryView.GetCountry())
        yield return (object) new WaitForSeconds(1f);
      if (CGameManager.gameType == IGame.GameType.VersusMP && ((MultiplayerGame) CGameManager.game).IsSPContinue || CGameManager.gameType == IGame.GameType.CoopMP && ((CooperativeGame) CGameManager.game).IsSPContinue)
      {
        this.bloomOverlay.SetActive(false);
        yield return (object) new WaitForSeconds(1f);
      }
      else if (c.totalInfected + c.totalZombie > 0L)
      {
        bool blooming = false;
        if (this.CanSeeDots(c))
        {
          for (int i = 0; i < c.localDiseases.Count; ++i)
          {
            LocalDisease localDisease = c.localDiseases[i];
            if ((!CGameManager.IsMultiplayerGame || CGameManager.localPlayerInfo.disease.id != localDisease.diseaseID) && (localDisease.infectedPopulation + localDisease.zombie > 0L || CGameManager.IsMultiplayerGame))
            {
              blooming = true;
              float b = 1f - c.deadPercent;
              if (localDisease.disease.cureFlag)
                b -= c.healthyPercent;
              long num = c.totalInfected + c.totalZombie;
              float duration = 10f;
              if (num > 0L)
                duration = 10f * Mathf.Max(0.1f, 1f * (float) (localDisease.controlledInfected + localDisease.zombie) / (float) num);
              float speed = Mathf.Max(0.1f, b);
              this.Bloom(c.localDiseases[i], duration, speed, true);
              float endTime = CInterfaceManager.instance.gameTime + 10f;
              while ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.ActualSpeed == 0)
                yield return (object) null;
              while ((double) CInterfaceManager.instance.gameTime < (double) endTime)
                yield return (object) null;
            }
          }
        }
        if (!blooming)
        {
          if (CGameManager.IsMultiplayerGame)
          {
            for (int index = 0; index < c.localDiseases.Count; ++index)
            {
              if (CGameManager.localPlayerInfo.disease.id != c.localDiseases[index].diseaseID && this.CanSeeDots(c))
              {
                this.Bloom(c.localDiseases[index], 10f, 0.0001f, true);
                blooming = true;
                break;
              }
            }
            if (!blooming)
              this.bloomOverlay.SetActive(false);
          }
          else if (this.CanSeeDots(c))
            this.Bloom(c.localDiseases[0], 10f, 0.0001f, true);
        }
        yield return (object) null;
      }
      else
      {
        if (CGameManager.IsMultiplayerGame)
        {
          bool flag = false;
          for (int index = 0; index < c.localDiseases.Count; ++index)
          {
            if (CGameManager.localPlayerInfo.disease.id != c.localDiseases[index].diseaseID && this.CanSeeDots(c))
            {
              this.Bloom(c.localDiseases[index], 10f, 0.0001f, true);
              flag = true;
              break;
            }
          }
          if (!flag)
            this.bloomOverlay.SetActive(false);
        }
        else if (this.CanSeeDots(c))
          this.Bloom(c.localDiseases[0], 10f, 0.0001f, true);
        yield return (object) new WaitForSeconds(1f);
      }
      c = (Country) null;
      c = (Country) null;
    }
  }

  public void UpdateDiseaseOverlays(List<LocalDisease> sorted, float deadPercent)
  {
    for (int index = 0; index < sorted.Count; ++index)
    {
      bool flag = false;
      if (sorted[index].disease == CGameManager.localPlayerInfo.disease)
      {
        Country country = this.countryView.GetCountry();
        flag = CGameManager.game.CanSeeDots(country);
      }
      if (flag)
      {
        Vector3 localPosition = this.overlays[sorted[index].disease.id].transform.localPosition;
        this.overlays[sorted[index].disease.id].transform.localPosition = localPosition;
        DiseaseColourSet diseaseColourSet = CGameManager.game.GetColourSet(sorted[index].disease);
        if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
          diseaseColourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
        Color color = Color.Lerp(Color.white, diseaseColourSet.deadColour, deadPercent * 0.75f) with
        {
          a = 0.7f
        };
        if (CGameManager.IsMultiplayerGame)
          color.a = this.GetAlpha(sorted[index]);
        else if (sorted[index].disease.isCure)
        {
          color.a = this.cureDecayAlpha;
          this.cureDecayAlpha = Mathf.Lerp(this.cureDecayAlpha, this.cureDecayFade, Time.deltaTime * 4f);
        }
        this.overlays[sorted[index].disease.id].GetComponent<Renderer>().material.color = color;
        this.overlays[sorted[index].disease.id].GetComponent<Renderer>().enabled = true;
        if (this.useDeadOverlay)
          this.deadOverlay.GetComponent<Renderer>().enabled = true;
      }
      else
      {
        this.overlays[sorted[index].disease.id].GetComponent<Renderer>().enabled = false;
        if (this.useDeadOverlay)
          this.deadOverlay.GetComponent<Renderer>().enabled = false;
      }
    }
    if (!CGameManager.IsMultiplayerGame)
      return;
    for (int index = 0; index < sorted.Count; ++index)
    {
      LocalDisease ld = sorted[index];
      if (ld.disease != CGameManager.localPlayerInfo.disease)
      {
        DiseaseColourSet colourSet = CGameManager.game.GetColourSet(ld.disease);
        Color color = Color.Lerp(Color.white, colourSet.deadColour, ld.deadPercent * 0.5f);
        if ((double) ld.deadPercent >= 1.0)
          color = colourSet.deadColour;
        color.a *= this.GetAlpha(ld);
        this.bloomOverlay.GetComponent<Renderer>().material.color = color;
      }
    }
    Country country1 = this.countryView.GetCountry();
    if (CGameManager.game.CanSeeDots(country1))
    {
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < sorted.Count; ++index)
      {
        if ((double) deadPercent >= 0.99900001287460327)
        {
          flag1 = true;
          break;
        }
        if ((double) sorted[index].infectedPercent > 0.0 && (double) sorted[index].infectedPercent >= 1.0 - (double) deadPercent - 1.0 / 1000.0)
        {
          if (sorted[index].diseaseID == CGameManager.localPlayerInfo.disease.id)
            flag2 = true;
          ++num;
        }
      }
      this.targetMPOverlayColor = Color.clear;
      this.targetMPOverlayPosition = Vector3.zero;
      if (flag1)
      {
        this.targetMPOverlayState = InfectionParticleSystem.MPOverlayState.Dead;
        this.targetMPOverlayColor = this.colorDeadMPOverlay;
        this.targetMPOverlayPosition = new Vector3(0.0f, 0.06f, 0.0f);
      }
      else if (num > 1)
      {
        this.targetMPOverlayState = InfectionParticleSystem.MPOverlayState.Both;
        this.targetMPOverlayColor = this.colorMultipleInfectedMPOverlay;
        this.targetMPOverlayPosition = new Vector3(0.0f, 0.04f, 0.0f);
      }
      else if (num != 1)
      {
        this.targetMPOverlayState = InfectionParticleSystem.MPOverlayState.None;
      }
      else
      {
        this.targetMPOverlayState = InfectionParticleSystem.MPOverlayState.Mine;
        this.targetMPOverlayColor = this.colorMineInfectedMPOverlay;
        this.targetMPOverlayPosition = new Vector3(0.0f, 0.02f, 0.0f);
        if (flag2)
          return;
        this.targetMPOverlayColor = this.colorOtherInfectedMPOverlay;
        this.targetMPOverlayPosition = Vector3.zero;
        this.targetMPOverlayState = InfectionParticleSystem.MPOverlayState.Theirs;
      }
    }
    else
      this.mpStateOverlay.SetActive(false);
  }

  public bool CanSeeDots(Country c)
  {
    return (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.CanSeeDots(c);
  }

  private IEnumerator DoForceBloom(LocalDisease ld)
  {
    InfectionParticleSystem infectionParticleSystem = this;
    if (ld.zombie + ld.infectedPopulation > 0L)
    {
      float b = 1f - ld.country.deadPercent;
      if (ld.disease.cureFlag)
        b -= ld.country.healthyPercent;
      float speed = Mathf.Max(0.1f, b);
      infectionParticleSystem.Bloom(ld, 10f, speed, false);
      yield return (object) new WaitForSeconds(10f);
      infectionParticleSystem.StartCoroutine(infectionParticleSystem.SystemBloomUpdate());
    }
  }

  public void ForceBloom(LocalDisease ld)
  {
    if (ld.infectedPopulation <= 0L)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine(this.DoForceBloom(ld));
  }

  public float GetAlpha(LocalDisease ld)
  {
    int num1 = 0;
    if (this.particleCounter.ContainsKey(ld.disease.id))
      num1 = this.particleCounter[ld.disease.id];
    int num2 = (int) (ld.infectedPopulation / (long) this.countryView.GetPeoplePerDot(ld.disease));
    if (ld.infectedPopulation > 0L)
      num2 += ld.disease.cureFlag ? 1 : 10;
    return num1 > 0 && num1 > num2 ? Mathf.Clamp(Mathf.Pow(Mathf.Clamp01((float) num2 * 1f / (float) num1), 0.5f), 0.0f, 0.7f) : 0.7f;
  }

  public bool HasWaves(int id) => this.diseases.Contains(id);

  private bool ComputeNewWavePosition(
    InfectionParticleSystem.SpreadWave wave,
    bool forSpreadWave,
    bool forDeadWave = false)
  {
    bool flag1 = false;
    bool flag2 = false;
    int num1 = 0;
    Vector3 position = wave.position;
    Vector3 vector3;
    do
    {
      ++num1;
      if (num1 <= (forSpreadWave ? 4 : 2))
      {
        Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
        float num2 = 0.8f;
        if (CGameManager.IsCureGame)
        {
          num2 *= 0.75f;
          if (!forDeadWave)
            num2 *= 0.4f;
        }
        vector3 = wave.position + new Vector3(insideUnitCircle.x, insideUnitCircle.y, 0.0f) * num2 * this.spreadMult;
        CountryView countryView;
        if (forDeadWave || CUtils.IntRand(0, 20) < 9)
        {
          countryView = CountryView.GetCountryAt(vector3);
          if ((UnityEngine.Object) countryView != (UnityEngine.Object) this.countryView)
            countryView = (CountryView) null;
        }
        else
        {
          flag2 = true;
          countryView = CountryView.GetCountryAt(vector3);
        }
        if (!((UnityEngine.Object) countryView == (UnityEngine.Object) null))
        {
          flag1 = (UnityEngine.Object) countryView == (UnityEngine.Object) this.countryView;
          if (flag2)
          {
            countryView.AddSpreadWave(vector3, wave.diseaseID);
            flag2 = false;
          }
        }
      }
      else
        goto label_13;
    }
    while (!flag1);
    goto label_14;
label_13:
    return false;
label_14:
    wave.position = vector3;
    return true;
  }

  public void AddWave(Vector3 position, int diseaseID)
  {
    this.diseases.Add(diseaseID);
    InfectionParticleSystem.SpreadWave sw = InfectionParticleSystem.SpreadWave.Create();
    sw.position = position;
    sw.diseaseID = diseaseID;
    this.AddWave(sw);
  }

  public void AddWave(InfectionParticleSystem.SpreadWave sw)
  {
    this.waves.Add(sw);
    if (!CGameManager.IsCureGame)
      return;
    this.deadWaves.Add(InfectionParticleSystem.SpreadWave.CreateClone(sw));
  }

  public void UpdateSystem(LocalDisease ld) => this.UpdateSystem(ld, 5);

  public void UpdateSystem(LocalDisease ld, int maxIterations)
  {
    bool flag = false;
    int peoplePerDot = this.countryView.GetPeoplePerDot(ld.disease);
    if (ld.disease.isCure)
      flag = true;
    if (flag)
    {
      float num1 = 1.25f;
      long num2 = (long) (40.0 / (double) num1);
      float y1 = 0.55f;
      float num3 = 0.25f / num1;
      long num4 = (long) (40.0 / (double) num1);
      float y2 = 0.55f;
      float num5 = 0.15f / num1;
      long num6 = Math.Min(ld.infectedPopulation, ld.infectedPopulation < num2 ? num2 : (long) ((double) num2 + Math.Pow((double) ld.infectedPopulation, (double) y1) * (double) num3));
      this.numDotsNeeded = num6 - this.dotsOverTime.Sum();
      this.numDeadDotsNeeded = Math.Min(ld.killedPopulation, ld.killedPopulation < num4 ? num4 : (long) ((double) num4 + Math.Pow((double) ld.killedPopulation, (double) y2) * (double) num5));
      this.numDeadDotsNeeded -= this.deadDotsOverTime.Sum();
      if (ld.infectedPopulation == 0L)
      {
        if (this.dotsOverTime.Sum() > 0L)
        {
          this.cureDecayFade = 1f / (float) (this.dotsOverTime.Size() + 4);
          this.dotsOverTime.AdvanceBucket();
        }
        else
          this.cureDecayFade = 0.0f;
      }
      else
      {
        this.cureDecayFade = 1f;
        if (this.numDotsNeeded < 0L)
        {
          if (this.dotsOverTime.Sum() > 0L)
          {
            this.cureDecayFade = (float) (num6 / this.dotsOverTime.Sum());
            this.cureDecayFade = (float) (0.20000000298023224 + (double) this.cureDecayFade * 0.800000011920929);
          }
          else
            this.cureDecayFade = 0.0f;
        }
      }
    }
    else
    {
      this.cureDecayFade = 1f;
      long a = ld.infectedPopulation + ld.zombie;
      this.infectedChange[ld.diseaseID] += a - this.infectedAtLastUpdate[ld.diseaseID];
      this.infectedAtLastUpdate[ld.diseaseID] = a;
      if (a > 0L && (!this.particleCounter.ContainsKey(ld.disease.id) || this.particleCounter[ld.disease.id] < 10) && this.infectedChange[ld.diseaseID] < (long) peoplePerDot)
        this.infectedChange[ld.diseaseID] += (long) ((double) Mathf.Min((float) a, 10f) * (double) peoplePerDot / 10.0);
    }
    int num7 = 0;
    if (CGameManager.game.IsReplayActive)
      maxIterations *= 4;
    if (flag)
      maxIterations = 1;
    while ((flag || this.infectedChange[ld.diseaseID] > (long) peoplePerDot) && num7 < maxIterations)
    {
      ++num7;
      for (int index = 0; index < this.waves.Count; ++index)
      {
        InfectionParticleSystem.SpreadWave wave = this.waves[index];
        if (flag)
        {
          if (!this.UpdateSpreadWaveCure(wave))
          {
            this.waves.Remove(wave);
            wave.Destroy();
            --index;
          }
        }
        else if (!this.UpdateSpreadWave(wave))
        {
          this.waves.Remove(wave);
          wave.Destroy();
          --index;
        }
      }
    }
    if (!this.useDeadOverlay)
      return;
    long killedPopulation = ld.killedPopulation;
    this.deadChange[ld.diseaseID] += killedPopulation - this.deadAtLastUpdate[ld.diseaseID];
    if (killedPopulation > 100L && (!this.deadParticleCounter.ContainsKey(ld.disease.id) || this.deadParticleCounter[ld.disease.id] < 2))
      this.deadChange[ld.diseaseID] += (long) ((double) Mathf.Min(10f, (float) killedPopulation) * (double) peoplePerDot * 4.0 / 10.0);
    if (this.deadAtLastUpdate[ld.diseaseID] == 0L && killedPopulation > 0L)
    {
      InfectionParticleSystem.SpreadWave spreadWave = InfectionParticleSystem.SpreadWave.Create();
      spreadWave.position = this.countryView.GetRandomPositionInsideCountry();
      spreadWave.diseaseID = ld.diseaseID;
      this.deadWaves.Add(spreadWave);
    }
    this.deadAtLastUpdate[ld.diseaseID] = killedPopulation;
    int num8 = 0;
    if (CGameManager.game.IsReplayActive)
      maxIterations *= 4;
    while ((flag || this.deadChange[ld.diseaseID] > (long) (peoplePerDot * 4)) && num8 < maxIterations)
    {
      ++num8;
      for (int index = 0; index < this.deadWaves.Count; ++index)
      {
        InfectionParticleSystem.SpreadWave deadWave = this.deadWaves[index];
        if (flag)
        {
          if (!this.UpdateDeadSpreadWaveCure(deadWave))
          {
            this.deadWaves.Remove(deadWave);
            deadWave.Destroy();
            --index;
          }
        }
        else if (!this.UpdateDeadSpreadWave(deadWave))
        {
          this.deadWaves.Remove(deadWave);
          deadWave.Destroy();
          --index;
        }
      }
    }
    this.deadOverlay.SetActive(true);
  }

  private bool UpdateSpreadWave(InfectionParticleSystem.SpreadWave wave)
  {
    if (this.colourDiseaseID != wave.diseaseID)
    {
      this.colourDiseaseID = wave.diseaseID;
      Disease disease = World.instance.GetDisease(wave.diseaseID);
      this.localDisease = disease.GetLocalDisease(this.countryView.GetCountry());
      this.colourSet = CGameManager.game.GetColourSet(disease);
      if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
        this.colourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
      this.colourRatioTotal = 0;
      for (int index = 0; index < this.colourSet.ratios.Length; ++index)
        this.colourRatioTotal += this.colourSet.ratios[index];
    }
    bool flag = false;
    if (!this.particleCounter.ContainsKey(wave.diseaseID))
      this.particleCounter[wave.diseaseID] = 0;
    if (this.particleCounter[wave.diseaseID] < 2 && this.countryView.GetCountry().diseaseNexus != null && this.countryView.GetCountry().diseaseNexus.id == wave.diseaseID)
      flag = true;
    if ((double) this.particleCounter[wave.diseaseID] > (double) this.countryView.area / 2.7000000476837158)
    {
      this.infectedChange[wave.diseaseID] = 0L;
      return false;
    }
    if (this.infectedChange[wave.diseaseID] <= (long) this.countryView.GetPeoplePerDot(this.localDisease.disease))
      return true;
    if (this.localDisease.applyDotLimiter)
    {
      long num = this.localDisease.infectedPopulation + this.localDisease.zombie;
      if (num > 500L)
        this.localDisease.applyDotLimiter = false;
      if ((long) this.particleCounter[wave.diseaseID] > num)
        return true;
    }
    if (!this.ComputeNewWavePosition(wave, false))
      return true;
    this.particleCounter[wave.diseaseID]++;
    this.infectedChange[wave.diseaseID] = this.infectedChange[wave.diseaseID] - (long) this.countryView.GetPeoplePerDot(this.localDisease.disease);
    ++wave.dots;
    int num1 = CUtils.IntRand(0, this.colourRatioTotal - 1);
    Color c = Color.magenta;
    for (int index = 0; index < this.colourSet.ratios.Length; ++index)
    {
      num1 -= this.colourSet.ratios[index];
      if (num1 <= 0)
      {
        c = this.colourSet.colours[index];
        break;
      }
    }
    this.particleQueue.Add(InfectionParticleSystem.ParticleInfo.Create(wave.position, c, wave.diseaseID));
    if (wave.dots % 10 == 0)
    {
      InfectionParticleSystem.SpreadWave wave1 = InfectionParticleSystem.SpreadWave.Create();
      wave1.Clone(wave);
      if (this.ComputeNewWavePosition(wave1, true))
      {
        this.countryView.AddSpreadWave(wave1.position, wave1.diseaseID);
      }
      else
      {
        if (!flag)
          return false;
        wave.dots = 0;
        return true;
      }
    }
    if (!flag)
      return wave.dots < 15;
    wave.dots = 0;
    return true;
  }

  private bool UpdateSpreadWaveCure(InfectionParticleSystem.SpreadWave wave)
  {
    bool flag = this.waves.Count <= 1;
    if (this.waves.Count > 50 || this.numDotsNeeded <= 0L)
      return flag;
    if (this.colourDiseaseID != wave.diseaseID)
    {
      this.colourDiseaseID = wave.diseaseID;
      Disease disease = World.instance.GetDisease(wave.diseaseID);
      this.localDisease = disease.GetLocalDisease(this.countryView.GetCountry());
      this.colourSet = CGameManager.game.GetColourSet(disease);
      if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
        this.colourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
      this.colourRatioTotal = 0;
      for (int index = 0; index < this.colourSet.ratios.Length; ++index)
        this.colourRatioTotal += this.colourSet.ratios[index];
    }
    if (!this.ComputeNewWavePosition(wave, false))
      return true;
    if (!this.particleCounter.ContainsKey(wave.diseaseID))
      this.particleCounter[wave.diseaseID] = 0;
    --this.numDotsNeeded;
    this.particleCounter[wave.diseaseID]++;
    float num1 = UnityEngine.Random.Range(0.0f, 1f);
    int index1 = (int) ((double) this.dotsOverTime.Size() * (double) num1);
    this.dotsOverTime.Add(index1, 1L);
    ++wave.dots;
    int num2 = CUtils.IntRand(0, this.colourRatioTotal - 1);
    Color c = Color.magenta;
    for (int index2 = 0; index2 < this.colourSet.ratios.Length; ++index2)
    {
      num2 -= this.colourSet.ratios[index2];
      if (num2 <= 0)
      {
        c = this.colourSet.colours[index2];
        break;
      }
    }
    c.a *= (float) (1.0 - 1.0 / (double) (this.dotsOverTime.Size() + 4) * (double) index1);
    this.particleQueue.Add(InfectionParticleSystem.ParticleInfo.Create(wave.position, c, wave.diseaseID));
    if (wave.dots % 10 == 0)
    {
      InfectionParticleSystem.SpreadWave clone = InfectionParticleSystem.SpreadWave.CreateClone(wave);
      if (!this.ComputeNewWavePosition(clone, true))
        return flag;
      this.waves.Add(clone);
      if (this.deadWaves.Count < 50)
        this.deadWaves.Add(InfectionParticleSystem.SpreadWave.CreateClone(clone));
    }
    return wave.dots % 15 != 0 | flag;
  }

  private bool UpdateDeadSpreadWaveCure(InfectionParticleSystem.SpreadWave wave)
  {
    bool flag = this.deadWaves.Count <= 1;
    if (this.deadWaves.Count > 50 || this.numDeadDotsNeeded <= 0L)
      return flag;
    if (this.colourDiseaseID != wave.diseaseID)
    {
      this.colourDiseaseID = wave.diseaseID;
      Disease disease = World.instance.GetDisease(wave.diseaseID);
      this.localDisease = disease.GetLocalDisease(this.countryView.GetCountry());
      this.colourSet = CGameManager.game.GetColourSet(disease);
      if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
        this.colourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
      this.colourRatioTotal = 0;
      for (int index = 0; index < this.colourSet.ratios.Length; ++index)
        this.colourRatioTotal += this.colourSet.ratios[index];
    }
    if (!this.ComputeNewWavePosition(wave, false, true))
      return true;
    if (!this.deadParticleCounter.ContainsKey(wave.diseaseID))
      this.deadParticleCounter[wave.diseaseID] = 0;
    --this.numDeadDotsNeeded;
    this.deadParticleCounter[wave.diseaseID]++;
    float num = UnityEngine.Random.Range(0.0f, 1f);
    this.deadDotsOverTime.Add((int) ((double) this.deadDotsOverTime.Size() * (double) num), 1L);
    ++wave.dots;
    Color deadParticleColour = this.colourSet.deadParticleColour;
    this.deadParticleQueue.Add(InfectionParticleSystem.ParticleInfo.Create(wave.position, deadParticleColour, wave.diseaseID));
    if (wave.dots % 10 == 0)
    {
      InfectionParticleSystem.SpreadWave wave1 = InfectionParticleSystem.SpreadWave.Create();
      wave1.Clone(wave);
      if (!this.ComputeNewWavePosition(wave1, true, true))
        return flag;
      this.deadWaves.Add(wave1);
    }
    return wave.dots % 15 != 0 | flag;
  }

  private bool UpdateDeadSpreadWave(InfectionParticleSystem.SpreadWave wave)
  {
    if (this.colourDiseaseID != wave.diseaseID)
    {
      this.colourDiseaseID = wave.diseaseID;
      Disease disease = World.instance.GetDisease(wave.diseaseID);
      this.localDisease = disease.GetLocalDisease(this.countryView.GetCountry());
      this.colourSet = CGameManager.game.GetColourSet(disease);
      if (CGameManager.CheckExternalMethodExist("GetCurrentDiseaseColor"))
        this.colourSet = (DiseaseColourSet) CGameManager.CallExternalMethodWithReturnValue("GetCurrentDiseaseColor", World.instance, World.instance.diseases[0], this.countryView.GetCountry(), this.localDisease);
      this.colourRatioTotal = 0;
      for (int index = 0; index < this.colourSet.ratios.Length; ++index)
        this.colourRatioTotal += this.colourSet.ratios[index];
    }
    bool flag = true;
    if (!this.deadParticleCounter.ContainsKey(wave.diseaseID))
      this.deadParticleCounter[wave.diseaseID] = 0;
    int num = Mathf.RoundToInt((float) (this.countryView.GetPeoplePerDot(this.localDisease.disease) * 4));
    if ((double) this.deadParticleCounter[wave.diseaseID] > (double) this.countryView.area / 2.7000000476837158)
    {
      this.deadChange[wave.diseaseID] = 0L;
      return false;
    }
    if (this.deadChange[wave.diseaseID] <= (long) num)
      return true;
    long killedPopulation = this.localDisease.killedPopulation;
    if (killedPopulation <= 500L && (long) this.deadParticleCounter[wave.diseaseID] > killedPopulation || !this.ComputeNewWavePosition(wave, false))
      return true;
    this.deadParticleCounter[wave.diseaseID]++;
    this.deadChange[wave.diseaseID] -= (long) num;
    ++wave.dots;
    Color deadParticleColour = this.colourSet.deadParticleColour;
    this.deadParticleQueue.Add(InfectionParticleSystem.ParticleInfo.Create(wave.position, deadParticleColour, wave.diseaseID));
    if (wave.dots % 10 == 0)
    {
      InfectionParticleSystem.SpreadWave wave1 = InfectionParticleSystem.SpreadWave.Create();
      wave1.Clone(wave);
      if (this.ComputeNewWavePosition(wave1, true, true))
      {
        this.deadWaves.Add(wave1);
      }
      else
      {
        if (!flag)
          return false;
        wave.dots = 0;
        return true;
      }
    }
    if (!flag)
      return wave.dots < 15;
    wave.dots = 0;
    return true;
  }

  public void Update()
  {
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null))
      return;
    if (CGameManager.game.ActualSpeed > 0)
    {
      if (this.particleQueue.Count > 0 || this.deadParticleQueue.Count > 0)
      {
        int num1 = 500;
        if (CGameManager.game.IsReplayActive)
          num1 *= 4;
        if (this.renderTextures != null && !this.renderTextures[0].IsCreated())
        {
          if (this.reloading)
            return;
          this.ReloadRenderTextures();
          return;
        }
        if (Time.frameCount != InfectionParticleSystem.frameNumber)
        {
          if (InfectionParticleSystem.globalParticlesThisFrame == num1 || InfectionParticleSystem.ticksThisFrame >= 25000L)
            InfectionParticleSystem.countryParticleBias *= 0.5f;
          else if (InfectionParticleSystem.globalParticlesThisFrame < num1)
            InfectionParticleSystem.countryParticleBias = Mathf.Min(InfectionParticleSystem.countryParticleBias * 1.5f, 1f);
          InfectionParticleSystem.globalParticlesThisFrame = 0;
          InfectionParticleSystem.frameNumber = Time.frameCount;
          InfectionParticleSystem.ticksThisFrame = 0L;
        }
        this.queueCount += Time.deltaTime * (float) this.particleQueue.Count * (float) CGameManager.game.ActualSpeed;
        this.queueCount += Time.deltaTime * (float) this.deadParticleQueue.Count * (float) CGameManager.game.ActualSpeed;
        int num2 = Mathf.Min(new int[3]
        {
          Mathf.FloorToInt(this.queueCount),
          Mathf.Max(1, Mathf.FloorToInt(100f * InfectionParticleSystem.countryParticleBias)),
          num1 - InfectionParticleSystem.globalParticlesThisFrame
        });
        this.queueCount -= (float) num2;
        long num3 = 0;
        DateTime now = DateTime.Now;
        long ticks = now.Ticks;
        int num4 = 0;
        int num5 = num2;
        if (this.useDeadOverlay && this.deadParticleQueue.Count > 0)
          num5 = num2 - this.deadParticleQueue.Count;
        for (int index = 0; index < num5 && this.particleQueue.Count > 0; ++index)
        {
          ++num4;
          InfectionParticleSystem.ParticleInfo particle = this.particleQueue[0];
          this.particleQueue.RemoveAt(0);
          this.CreateParticle(particle.pos, particle.col, particle.diseaseID);
          particle.Recycle();
          ++InfectionParticleSystem.globalParticlesThisFrame;
          now = DateTime.Now;
          num3 = now.Ticks - ticks;
          if (InfectionParticleSystem.ticksThisFrame + num3 >= 25000L)
            break;
        }
        if (this.useDeadOverlay && this.deadParticleQueue.Count > 0)
        {
          for (int index = num4; index < num2 && this.deadParticleQueue.Count > 0; ++index)
          {
            InfectionParticleSystem.ParticleInfo deadParticle = this.deadParticleQueue[0];
            this.deadParticleQueue.RemoveAt(0);
            this.CreateParticle(deadParticle.pos, deadParticle.col, deadParticle.diseaseID, true);
            deadParticle.Recycle();
            ++InfectionParticleSystem.globalParticlesThisFrame;
            now = DateTime.Now;
            num3 = now.Ticks - ticks;
            if (InfectionParticleSystem.ticksThisFrame + num3 >= 25000L)
              break;
          }
        }
        InfectionParticleSystem.ticksThisFrame += num3;
      }
      else
        this.queueCount = 0.0f;
    }
    if ((UnityEngine.Object) this.bloomOverlay != (UnityEngine.Object) null || (UnityEngine.Object) this.mpStateOverlay != (UnityEngine.Object) null)
    {
      float num = Time.deltaTime * (float) CGameManager.game.ActualSpeed * this.bloomSpeed;
      this.bloomTime += num;
      if ((double) num > 0.0 && (UnityEngine.Object) this.bloomOverlay != (UnityEngine.Object) null)
        this.bloomOverlay.GetComponent<Renderer>().material.SetFloat("_BloomTime", this.bloomTime);
    }
    if (!CGameManager.IsMultiplayerGame)
      return;
    if (this.targetMPOverlayState != this.lastMPOverlayState)
    {
      this.overlayFade -= Time.deltaTime * this.mpOverlayFadeOutSpeed;
      if ((double) this.overlayFade <= 0.0)
      {
        this.overlayFade = 0.0f;
        this.mpStateOverlay.SetActive(false);
        this.lastMPOverlayState = this.targetMPOverlayState;
      }
      else
      {
        Color lastMpOverlayColor = this.lastMPOverlayColor;
        lastMpOverlayColor.a *= this.overlayFade;
        this.mpStateOverlay.GetComponent<Renderer>().material.color = lastMpOverlayColor;
      }
    }
    else if (this.targetMPOverlayState == InfectionParticleSystem.MPOverlayState.None)
    {
      this.mpStateOverlay.SetActive(false);
    }
    else
    {
      if ((double) this.overlayFade >= 1.0)
        return;
      this.overlayFade += Time.deltaTime * this.mpOverlayFadeInSpeed;
      if ((double) this.overlayFade > 1.0)
        this.overlayFade = 1f;
      Color targetMpOverlayColor = this.targetMPOverlayColor;
      this.lastMPOverlayColor = this.targetMPOverlayColor;
      targetMpOverlayColor.a *= this.overlayFade;
      this.mpStateOverlay.SetActive(true);
      this.mpStateOverlay.GetComponent<Renderer>().material.color = targetMpOverlayColor;
      this.mpStateOverlay.transform.localPosition = this.targetMPOverlayPosition;
      if (this.targetMPOverlayState == InfectionParticleSystem.MPOverlayState.Theirs)
        this.overlays[CGameManager.localPlayerInfo.disease.id].transform.localPosition = new Vector3(0.0f, 0.025f, 0.0f);
      else
        this.overlays[CGameManager.localPlayerInfo.disease.id].transform.localPosition = new Vector3(0.0f, 0.01f, 0.0f);
    }
  }

  private void CreateParticle(Vector3 pos, Color col, int diseaseID, bool deadParticle = false)
  {
    GameObject gameObject = this.overlays[diseaseID];
    if (deadParticle)
      gameObject = this.deadOverlay;
    Texture2D texture2D = this.particleTexture;
    int num1 = this.particleWidth;
    float num2 = this.particleScale;
    if (deadParticle)
    {
      texture2D = this.deadParticleTexture;
      num1 = this.deadParticleWidth;
      num2 = this.deadParticleScale;
    }
    if (CGameManager.IsCureGame)
      num2 *= 2f;
    Vector3 vector3 = (pos - gameObject.transform.position + this.boundsOffset) * this.textureScale;
    float num3 = Mathf.Lerp(this.minSize, this.maxSize, UnityEngine.Random.value);
    int num4 = Mathf.Max(1, Mathf.FloorToInt(num2 / num3));
    int num5 = num1 / num4;
    if (num5 > this.textureSize / 2)
      num5 = this.textureSize / 2;
    int x = Mathf.FloorToInt(vector3.x) - num5 / 2;
    int y = this.textureSize - (Mathf.FloorToInt(vector3.y) - num5 / 2) - num5;
    if (x < 0)
      x = 0;
    if (x >= this.textureSize - num5 - 1)
      x = this.textureSize - num5 - 1;
    if (y < 0)
      y = 0;
    if (y >= this.textureSize - num5 - 1)
      y = this.textureSize - num5 - 1;
    RenderTexture renderTexture = this.renderTextures[diseaseID];
    if (deadParticle)
      renderTexture = this.deadRenderTexture;
    RenderTexture.active = renderTexture;
    GL.PushMatrix();
    GL.LoadPixelMatrix(0.0f, (float) this.textureSize, (float) this.textureSize, 0.0f);
    Graphics.DrawTexture(new Rect((float) x, (float) y, (float) num5, (float) num5), (Texture) texture2D, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, col);
    GL.PopMatrix();
    RenderTexture.active = (RenderTexture) null;
  }

  public byte[][] SerializeInfectionTexture()
  {
    this.infectionTex = new Texture2D(this.textureSize, this.textureSize, TextureFormat.ARGB32, false, false);
    byte[][] numArray = new byte[this.renderTextures.Length][];
    for (int index = 0; index < this.renderTextures.Length; ++index)
    {
      RenderTexture renderTexture = this.renderTextures[index];
      RenderTexture.active = renderTexture;
      this.infectionTex.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTexture.width, (float) renderTexture.height), 0, 0);
      this.infectionTex.Apply();
      RenderTexture.active = (RenderTexture) null;
      numArray[index] = this.infectionTex.EncodeToPNG();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.infectionTex);
    this.infectionTex = (Texture2D) null;
    return numArray;
  }

  public byte[] SerializeDeadTexture()
  {
    if (!this.useDeadOverlay || !((UnityEngine.Object) this.deadRenderTexture != (UnityEngine.Object) null))
      return (byte[]) null;
    this.infectionTex = new Texture2D(this.textureSize, this.textureSize, TextureFormat.ARGB32, false, false);
    RenderTexture deadRenderTexture = this.deadRenderTexture;
    RenderTexture.active = deadRenderTexture;
    this.infectionTex.ReadPixels(new Rect(0.0f, 0.0f, (float) deadRenderTexture.width, (float) deadRenderTexture.height), 0, 0);
    this.infectionTex.Apply();
    RenderTexture.active = (RenderTexture) null;
    byte[] png = this.infectionTex.EncodeToPNG();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.infectionTex);
    this.infectionTex = (Texture2D) null;
    return png;
  }

  public void UnserializeInfectionTexture(byte[][] textures, byte[] deadTextureData)
  {
    if (this.baseInfectionTextures == null || this.baseInfectionTextures.Length != textures.Length)
      this.baseInfectionTextures = new Texture2D[textures.Length];
    for (int index = 0; index < textures.Length; ++index)
    {
      if (textures[index] != null)
      {
        this.infectionTex = new Texture2D(1, 1);
        this.infectionTex.LoadImage(textures[index]);
        this.baseInfectionTextures[index] = this.infectionTex;
        if (this.renderTextures != null && this.renderTextures.Length > index)
          Graphics.Blit((Texture) this.baseInfectionTextures[index], this.renderTextures[index]);
      }
    }
    if (deadTextureData != null)
    {
      this.infectionTex = new Texture2D(1, 1);
      this.infectionTex.LoadImage(deadTextureData);
      this.baseDeadTexture = this.infectionTex;
      if ((UnityEngine.Object) this.deadRenderTexture != (UnityEngine.Object) null)
        Graphics.Blit((Texture) this.baseDeadTexture, this.deadRenderTexture);
    }
    this.infectionTex = (Texture2D) null;
  }

  public void UnserializeDotCounts(string str)
  {
    if (str == null)
      return;
    string[] strArray = str.Split('/');
    if (strArray.Length >= 2)
    {
      this.dotsOverTime = TemporalBuckets.Deserialize(strArray[0]);
      this.deadDotsOverTime = TemporalBuckets.Deserialize(strArray[1]);
    }
    if (strArray.Length < 4)
      return;
    this.cureDecayFade = float.Parse(strArray[2]);
    this.cureDecayAlpha = float.Parse(strArray[3]);
  }

  public string SerializeDotCounts()
  {
    return this.dotsOverTime.Serialize() + "/" + this.deadDotsOverTime.Serialize() + "/" + (object) this.cureDecayFade + "/" + (object) this.cureDecayAlpha;
  }

  public void OnApplicationFocus(bool showing)
  {
    if (!Application.isPlaying)
      return;
    if (!showing)
      this.BufferRenderTextures();
    else
      this.ReloadRenderTextures();
  }

  public void BufferRenderTextures(bool force = false)
  {
    if (this.renderTextures == null || this.renderTextures.Length == 0)
      return;
    this.reloading = false;
    this.StopCoroutine("DoReloadRenderTextures");
    if (this.buffers == null || this.buffers.Length != this.renderTextures.Length)
      this.buffers = new Texture2D[this.renderTextures.Length];
    if (this.bufferedRTs == null || this.bufferedRTs.Length != this.renderTextures.Length)
      this.bufferedRTs = new bool[this.renderTextures.Length];
    RenderTexture active = RenderTexture.active;
    for (int index = 0; index < this.renderTextures.Length; ++index)
    {
      RenderTexture renderTexture = this.renderTextures[index];
      if (renderTexture.IsCreated() && (force || !this.bufferedRTs[index]))
      {
        RenderTexture.active = renderTexture;
        this.tempBuffer = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);
        this.tempBuffer.anisoLevel = 0;
        this.tempBuffer.filterMode = FilterMode.Point;
        this.tempBuffer.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTexture.width, (float) renderTexture.height), 0, 0, false);
        this.tempBuffer.Apply();
        if (!force)
        {
          Color pixel = this.tempBuffer.GetPixel(0, 0);
          if ((double) pixel.r == (double) pixel.g && (double) pixel.r == (double) pixel.b && (double) pixel.r == (double) pixel.a && (double) pixel.a > 0.0)
          {
            RenderTexture.active = (RenderTexture) null;
            continue;
          }
        }
        if ((UnityEngine.Object) this.buffers[index] != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.buffers[index]);
        this.buffers[index] = this.tempBuffer;
        RenderTexture.active = (RenderTexture) null;
        if (!force)
        {
          this.overlays[index].GetComponent<Renderer>().material.mainTexture = (Texture) this.buffers[index];
          this.bufferedRTs[index] = true;
        }
      }
    }
    if (this.useDeadOverlay && (UnityEngine.Object) this.deadRenderTexture != (UnityEngine.Object) null && this.deadRenderTexture.IsCreated() && (force || !this.deadBufferActive))
    {
      bool flag = true;
      RenderTexture.active = this.deadRenderTexture;
      this.tempBuffer = new Texture2D(this.deadRenderTexture.width, this.deadRenderTexture.height, TextureFormat.ARGB32, false, false);
      this.tempBuffer.anisoLevel = 0;
      this.tempBuffer.filterMode = FilterMode.Point;
      this.tempBuffer.ReadPixels(new Rect(0.0f, 0.0f, (float) this.deadRenderTexture.width, (float) this.deadRenderTexture.height), 0, 0, false);
      this.tempBuffer.Apply();
      if (!force)
      {
        Color pixel = this.tempBuffer.GetPixel(0, 0);
        if ((double) pixel.r == (double) pixel.g && (double) pixel.r == (double) pixel.b && (double) pixel.r == (double) pixel.a && (double) pixel.a > 0.0)
        {
          RenderTexture.active = (RenderTexture) null;
          flag = false;
        }
      }
      if (flag)
      {
        if ((UnityEngine.Object) this.deadBuffer != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.deadBuffer);
        this.deadBuffer = this.tempBuffer;
        RenderTexture.active = (RenderTexture) null;
        if (!force)
        {
          this.deadOverlay.GetComponent<Renderer>().material.mainTexture = (Texture) this.deadBuffer;
          this.deadBufferActive = true;
        }
      }
    }
    if (!((UnityEngine.Object) active != (UnityEngine.Object) null) || !active.IsCreated())
      return;
    RenderTexture.active = active;
  }

  public void ReloadRenderTextures()
  {
    this.reloading = true;
    this.StartCoroutine("DoReloadRenderTextures");
  }

  private IEnumerator DoReloadRenderTextures()
  {
    yield return (object) new WaitForEndOfFrame();
    if (this.reloading)
    {
      if (this.renderTextures != null && this.renderTextures.Length != 0)
      {
        for (int index = 0; index < this.renderTextures.Length; ++index)
        {
          if (!this.renderTextures[index].IsCreated())
          {
            this.renderTextures[index].Create();
            if (this.buffers != null && this.buffers.Length > index && (UnityEngine.Object) this.buffers[index] != (UnityEngine.Object) null)
              Graphics.Blit((Texture) this.buffers[index], this.renderTextures[index]);
            else
              Graphics.Blit((Texture) this.clearTexture, this.renderTextures[index]);
          }
          this.overlays[index].GetComponent<Renderer>().material.mainTexture = (Texture) this.renderTextures[index];
        }
      }
      if ((UnityEngine.Object) this.deadRenderTexture != (UnityEngine.Object) null)
      {
        if (!this.deadRenderTexture.IsCreated())
        {
          this.deadRenderTexture.Create();
          if ((UnityEngine.Object) this.deadBuffer != (UnityEngine.Object) null)
            Graphics.Blit((Texture) this.deadBuffer, this.deadRenderTexture);
          else
            Graphics.Blit((Texture) this.clearTexture, this.deadRenderTexture);
        }
        this.deadOverlay.GetComponent<Renderer>().material.mainTexture = (Texture) this.deadRenderTexture;
      }
      yield return (object) new WaitForSeconds(1f);
      if (this.reloading)
      {
        if (this.bufferedRTs != null)
        {
          for (int index = 0; index < this.bufferedRTs.Length; ++index)
            this.bufferedRTs[index] = false;
        }
        this.deadBufferActive = false;
      }
    }
  }

  private void OnDestroy()
  {
    if (!Application.isPlaying || this.renderTextures == null)
      return;
    for (int index = 0; index < this.renderTextures.Length; ++index)
    {
      if ((UnityEngine.Object) this.renderTextures[index] != (UnityEngine.Object) null)
        this.renderTextures[index].Release();
    }
  }

  public class SpreadWave
  {
    public Vector3 position;
    public int diseaseID = -1;
    public int dots;
    public static List<InfectionParticleSystem.SpreadWave> recycle = new List<InfectionParticleSystem.SpreadWave>();

    private SpreadWave()
    {
    }

    public static InfectionParticleSystem.SpreadWave CreateClone(
      InfectionParticleSystem.SpreadWave wave)
    {
      InfectionParticleSystem.SpreadWave clone = InfectionParticleSystem.SpreadWave.Create();
      clone.Clone(wave);
      return clone;
    }

    public void Clone(InfectionParticleSystem.SpreadWave wave)
    {
      this.position = wave.position;
      this.diseaseID = wave.diseaseID;
    }

    public static InfectionParticleSystem.SpreadWave Create()
    {
      if (InfectionParticleSystem.SpreadWave.recycle.Count <= 0)
        return new InfectionParticleSystem.SpreadWave();
      InfectionParticleSystem.SpreadWave spreadWave = InfectionParticleSystem.SpreadWave.recycle[0];
      InfectionParticleSystem.SpreadWave.recycle.RemoveAt(0);
      spreadWave.diseaseID = -1;
      spreadWave.position = Vector3.zero;
      spreadWave.dots = 0;
      return spreadWave;
    }

    public void Destroy() => InfectionParticleSystem.SpreadWave.recycle.Add(this);
  }

  private enum MPOverlayState
  {
    None,
    Mine,
    Theirs,
    Both,
    Dead,
  }

  private class ParticleInfo
  {
    public Vector3 pos;
    public Color col;
    public int diseaseID;
    private static List<InfectionParticleSystem.ParticleInfo> cache = new List<InfectionParticleSystem.ParticleInfo>();

    public static InfectionParticleSystem.ParticleInfo Create(Vector3 p, Color c, int id)
    {
      InfectionParticleSystem.ParticleInfo particleInfo;
      if (InfectionParticleSystem.ParticleInfo.cache.Count > 0)
      {
        particleInfo = InfectionParticleSystem.ParticleInfo.cache[0];
        InfectionParticleSystem.ParticleInfo.cache.RemoveAt(0);
      }
      else
        particleInfo = new InfectionParticleSystem.ParticleInfo();
      particleInfo.pos = p;
      particleInfo.col = c;
      particleInfo.diseaseID = id;
      return particleInfo;
    }

    public void Recycle() => InfectionParticleSystem.ParticleInfo.cache.Add(this);
  }
}
