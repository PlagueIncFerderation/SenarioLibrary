// Decompiled with JetBrains decompiler
// Type: DynamicMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DynamicMusic : MonoBehaviour
{
  private Dictionary<string, DynamicSFX> allSFX = new Dictionary<string, DynamicSFX>();
  private float pauseTime;
  private float NaturalLayer;
  private float HumanCalmLayer;
  private float HumanPanicLayer;
  private float BaseLayer;
  private float InfectionLayer;
  private float DeathLayer;
  private float WorldLayer;
  private float CureLayer;
  private AudioSource newSource;
  public Disease playerDisease;
  private float min = -21f;
  private float max = 5f;
  private float range;
  public List<AudioClip> randomSFX = new List<AudioClip>();
  private List<AudioSource> musicSources = new List<AudioSource>();
  public AudioSource menuMusicSource;
  public AudioClip menuClip;
  public List<AudioClip> cureTracks;
  public List<AudioClip> simianTracks;
  public List<AudioClip> vampireTracks;
  public List<AudioClip> necroaTracks;
  public List<AudioClip> standardTracks;
  public List<AudioClip> neuraxTracks;
  private List<float> faders = new List<float>();
  private float menuFade;
  private float menuMusicMultiplier = 1f;
  private float menuTargetMultiplier = 1f;
  private float gameFade;
  public DynamicMusic.MusicState state;
  public static DynamicMusic instance;
  private bool resetFade;
  private float fadeTime = 0.5f;
  private float soundFactor;
  private float menuChange;
  private float target;
  private float newTarget;
  private float startSound;
  private bool fadedOut;
  private Disease.EDiseaseType initialisedType = Disease.EDiseaseType.TUTORIAL;
  private const float FADE_SPEED = 0.35f;

  private void Awake() => DynamicMusic.instance = this;

  public void UpdateVolume() => this.FaderUpdate();

  private void CreateMenuSource()
  {
    GameObject gameObject = new GameObject();
    gameObject.name = "Menu Music: " + this.menuClip.name;
    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.loop = true;
    audioSource.volume = 0.0f;
    audioSource.clip = this.menuClip;
    this.menuMusicSource = audioSource;
    audioSource.Play();
  }

  private void Update()
  {
    if (this.state == DynamicMusic.MusicState.Menu)
    {
      if ((UnityEngine.Object) this.menuMusicSource == (UnityEngine.Object) null)
        this.CreateMenuSource();
      if ((double) this.gameFade > 0.0)
      {
        this.gameFade -= Time.deltaTime * 0.8f;
        if ((double) this.gameFade < 0.0)
          this.gameFade = 0.0f;
      }
      else if ((double) this.menuFade < 1.0)
      {
        this.menuFade += Time.deltaTime * 0.8f;
        if ((double) this.menuFade > 1.0)
          this.menuFade = 1f;
      }
      float f = this.menuTargetMultiplier - this.menuMusicMultiplier;
      this.menuMusicMultiplier += Mathf.Sign(f) * Mathf.Min(Mathf.Abs(f), Time.unscaledDeltaTime * 0.35f);
    }
    if (this.state == DynamicMusic.MusicState.Stop)
    {
      this.gameFade = 0.0f;
      this.menuFade = 0.0f;
    }
    if (this.state == DynamicMusic.MusicState.Faded)
    {
      if ((double) this.gameFade > 0.0)
      {
        this.gameFade -= Time.deltaTime * 0.8f;
        if ((double) this.gameFade < 0.0)
          this.gameFade = 0.0f;
      }
      if ((double) this.menuFade > 0.0)
      {
        this.menuFade -= Time.deltaTime * 0.8f;
        if ((double) this.menuFade < 0.0)
          this.menuFade = 0.0f;
      }
      if ((double) this.gameFade <= 0.0 && (double) this.menuFade <= 0.0)
      {
        this.state = !((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.game.CurrentGameState != IGame.GameState.InProgress && CGameManager.game.CurrentGameState != IGame.GameState.EndGame && CGameManager.game.CurrentGameState != IGame.GameState.ChoosingCountry ? DynamicMusic.MusicState.Menu : DynamicMusic.MusicState.Game;
        this.menuTargetMultiplier = 1f;
        this.menuMusicMultiplier = 1f;
      }
    }
    if (this.state == DynamicMusic.MusicState.Game)
    {
      if ((double) this.menuFade > 0.0)
      {
        this.menuFade -= Time.deltaTime * 0.8f;
        if ((double) this.menuFade < 0.0)
          this.menuFade = 0.0f;
      }
      else if ((double) this.gameFade < 1.0)
      {
        if (this.CheckSoundSources())
        {
          this.GameMusicUpdate();
          this.gameFade += Time.deltaTime * 0.8f;
          if ((double) this.gameFade > 1.0)
            this.gameFade = 1f;
        }
      }
      else
        this.GameMusicUpdate();
      this.pauseTime += CGameManager.game.DeltaGameTime;
      if ((UnityEngine.Object) this.newSource != (UnityEngine.Object) null)
      {
        if (!this.newSource.isPlaying)
          this.RecycleSFX();
      }
      else
        this.PlayRandomSound();
    }
    this.FaderUpdate();
  }

  private bool CheckSoundSources()
  {
    if (CGameManager.game.CurrentGameState == IGame.GameState.InProgress || CGameManager.game.CurrentGameState == IGame.GameState.EndGame || CGameManager.game.CurrentGameState == IGame.GameState.ChoosingCountry)
    {
      if (this.playerDisease != CGameManager.localPlayerInfo.disease)
      {
        this.playerDisease = CGameManager.localPlayerInfo.disease;
        this.state = DynamicMusic.MusicState.Faded;
        if (this.allSFX != null)
        {
          Debug.Log((object) "Reset dynamic SFX");
          foreach (KeyValuePair<string, DynamicSFX> keyValuePair in this.allSFX)
          {
            keyValuePair.Value.timesPlayed = 0;
            keyValuePair.Value.nextPlayTime = 0.0f;
          }
        }
      }
      if (this.state == DynamicMusic.MusicState.Faded && (double) this.gameFade <= 0.0 && (double) this.menuFade <= 0.0)
      {
        this.SetupSoundSources(this.playerDisease);
        this.state = DynamicMusic.MusicState.Game;
      }
    }
    else
      this.state = DynamicMusic.MusicState.Menu;
    return this.state == DynamicMusic.MusicState.Game;
  }

  private void SetupSoundSources(Disease d)
  {
    this.faders.Clear();
    foreach (UnityEngine.Component musicSource in this.musicSources)
      UnityEngine.Object.Destroy((UnityEngine.Object) musicSource.gameObject);
    switch (d.diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        this.SetNeuraxLayers();
        this.CreateSources(this.neuraxTracks);
        break;
      case Disease.EDiseaseType.NECROA:
        this.SetNecroaLayers();
        this.CreateSources(this.necroaTracks);
        break;
      case Disease.EDiseaseType.SIMIAN_FLU:
        this.SetSimianLayers();
        this.CreateSources(this.simianTracks);
        break;
      case Disease.EDiseaseType.VAMPIRE:
        this.SetVampireLayers();
        this.CreateSources(this.vampireTracks);
        break;
      case Disease.EDiseaseType.CURE:
        this.SetCureLayers();
        this.CreateSources(this.cureTracks);
        break;
      default:
        this.SetStandardLayers();
        this.CreateSources(this.standardTracks);
        break;
    }
  }

  private void CreateSources(List<AudioClip> clips)
  {
    this.faders.Clear();
    foreach (UnityEngine.Component musicSource in this.musicSources)
      UnityEngine.Object.Destroy((UnityEngine.Object) musicSource.gameObject);
    this.musicSources.Clear();
    foreach (AudioClip clip in clips)
    {
      GameObject gameObject = new GameObject();
      gameObject.name = "Track " + (object) this.musicSources.Count + ": " + clip.name;
      AudioSource audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.loop = true;
      audioSource.volume = 0.0f;
      this.musicSources.Add(audioSource);
      audioSource.clip = clip;
      audioSource.Play();
      this.faders.Add(0.0f);
    }
  }

  private void FaderUpdate()
  {
    float musicVolume = CSoundManager.instance.MusicVolume;
    for (int index = 0; index < this.faders.Count; ++index)
      this.musicSources[index].volume = this.faders[index] * this.gameFade * musicVolume;
    if (!(bool) (UnityEngine.Object) this.menuMusicSource)
      return;
    this.menuMusicSource.volume = this.menuFade * musicVolume * this.menuMusicMultiplier;
  }

  private void GameMusicUpdate()
  {
    if (this.playerDisease == null)
      return;
    switch (this.playerDisease.diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        this.UpdateNeuraxSound();
        break;
      case Disease.EDiseaseType.NECROA:
        this.UpdateNecroaSound();
        break;
      case Disease.EDiseaseType.SIMIAN_FLU:
        this.UpdateSimianSound();
        break;
      case Disease.EDiseaseType.VAMPIRE:
        this.UpdateVampireSound();
        break;
      case Disease.EDiseaseType.CURE:
        this.UpdateCureSound();
        break;
      default:
        this.UpdateStandardSound();
        break;
    }
  }

  private void UpdateFader(int index, float val)
  {
    float num = (val - this.min) / this.range;
    if (index >= this.faders.Count)
    {
      Debug.LogError((object) ("FADER OUT OF RANGE: " + (object) index + " MAX: " + (object) this.faders.Count));
    }
    else
    {
      if ((double) this.faders[index] >= (double) num)
        return;
      this.faders[index] += Time.unscaledDeltaTime * 0.35f * num;
    }
  }

  private void SetCureLayers()
  {
    this.InfectionLayer = 0.0f;
    this.range = this.max - this.min;
  }

  private void UpdateCureSound() => this.UpdateFader(0, this.InfectionLayer);

  private void SetStandardLayers()
  {
    this.NaturalLayer = -5f;
    this.HumanCalmLayer = -21f;
    this.HumanPanicLayer = -21f;
    this.BaseLayer = -4f;
    this.InfectionLayer = 0.0f;
    this.WorldLayer = 0.0f;
    this.CureLayer = -7f;
    this.DeathLayer = -6f;
    this.range = this.max - this.min;
  }

  private void UpdateStandardSound()
  {
    this.UpdateFader(0, this.NaturalLayer);
    this.UpdateFader(1, this.HumanCalmLayer);
    this.UpdateFader(2, this.HumanPanicLayer);
    this.UpdateFader(3, this.BaseLayer);
    this.UpdateFader(4, this.InfectionLayer);
    this.UpdateFader(5, this.DeathLayer);
    this.UpdateFader(6, this.WorldLayer);
    this.UpdateFader(7, this.CureLayer);
  }

  private void SetNeuraxLayers()
  {
    this.NaturalLayer = 0.0f;
    this.HumanCalmLayer = -3f;
    this.HumanPanicLayer = -21f;
    this.InfectionLayer = 0.0f;
    this.WorldLayer = -1f;
    this.CureLayer = -4f;
    this.DeathLayer = -4f;
    this.range = this.max - this.min;
  }

  private void UpdateNeuraxSound()
  {
    this.UpdateFader(0, this.NaturalLayer);
    this.UpdateFader(1, this.HumanCalmLayer);
    this.UpdateFader(2, this.HumanPanicLayer);
    this.UpdateFader(3, this.InfectionLayer);
    this.UpdateFader(4, this.DeathLayer);
    this.UpdateFader(5, this.WorldLayer);
    this.UpdateFader(6, this.CureLayer);
  }

  private void SetNecroaLayers()
  {
    this.NaturalLayer = -2f;
    this.HumanCalmLayer = -3f;
    this.HumanPanicLayer = -21f;
    this.InfectionLayer = -1f;
    this.WorldLayer = -2f;
    this.CureLayer = -5f;
    this.DeathLayer = -7f;
    this.range = this.max - this.min;
  }

  private void UpdateNecroaSound()
  {
    this.UpdateFader(0, this.NaturalLayer);
    this.UpdateFader(1, this.HumanCalmLayer);
    this.UpdateFader(2, this.HumanPanicLayer);
    this.UpdateFader(3, this.InfectionLayer);
    this.UpdateFader(4, this.DeathLayer);
    this.UpdateFader(5, this.WorldLayer);
    this.UpdateFader(6, this.CureLayer);
  }

  private void SetSimianLayers()
  {
    this.NaturalLayer = 0.0f;
    this.HumanCalmLayer = -3f;
    this.HumanPanicLayer = -21f;
    this.InfectionLayer = 0.0f;
    this.WorldLayer = -1f;
    this.CureLayer = -4f;
    this.DeathLayer = -4f;
    this.range = this.max - this.min;
  }

  private void UpdateSimianSound()
  {
    this.UpdateFader(0, this.NaturalLayer);
    this.UpdateFader(1, this.HumanCalmLayer);
    this.UpdateFader(2, this.HumanPanicLayer);
    this.UpdateFader(3, this.InfectionLayer);
    this.UpdateFader(4, this.DeathLayer);
    this.UpdateFader(5, this.WorldLayer);
    this.UpdateFader(6, this.CureLayer);
  }

  private void SetVampireLayers()
  {
    this.NaturalLayer = -2f;
    this.HumanCalmLayer = -3f;
    this.HumanPanicLayer = -21f;
    this.InfectionLayer = 0.0f;
    this.WorldLayer = -2f;
    this.CureLayer = -5f;
    this.DeathLayer = -7f;
    this.range = this.max - this.min;
  }

  private void UpdateVampireSound()
  {
    this.UpdateFader(0, this.NaturalLayer);
    this.UpdateFader(1, this.HumanCalmLayer);
    this.UpdateFader(2, this.HumanPanicLayer);
    this.UpdateFader(3, this.InfectionLayer);
  }

  public void SetMenuMenuMusic(float soundChange)
  {
    this.menuTargetMultiplier = Mathf.Clamp01(this.menuTargetMultiplier + soundChange);
  }

  public void FadeOut() => this.state = DynamicMusic.MusicState.Faded;

  public void StartMusic() => this.state = DynamicMusic.MusicState.Menu;

  private void PlayRandomSound()
  {
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.localPlayerInfo == null || CGameManager.localPlayerInfo.disease == null)
      return;
    if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      if (this.initialisedType != Disease.EDiseaseType.VAMPIRE)
      {
        this.allSFX.Clear();
        TextAsset textAsset = EncodedResources.Load("Data/Sound/vampire_sounds");
        if ((UnityEngine.Object) textAsset == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "vampire_sounds.txt Not Found");
          return;
        }
        DataImporter.UnserializeDataHash<DynamicSFX>((IDictionary<string, DynamicSFX>) this.allSFX, textAsset.text, "sounds");
        foreach (KeyValuePair<string, DynamicSFX> keyValuePair in this.allSFX)
          keyValuePair.Value.id = keyValuePair.Key;
        this.initialisedType = Disease.EDiseaseType.VAMPIRE;
      }
    }
    else if (CGameManager.localPlayerInfo.disease.diseaseType == Disease.EDiseaseType.CURE)
    {
      if (this.initialisedType != Disease.EDiseaseType.CURE)
      {
        this.allSFX.Clear();
        TextAsset textAsset = EncodedResources.Load("Data/Sound/cure_sounds");
        if ((UnityEngine.Object) textAsset == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "cure_sounds.txt Not Found");
          return;
        }
        DataImporter.UnserializeDataHash<DynamicSFX>((IDictionary<string, DynamicSFX>) this.allSFX, textAsset.text, "sounds");
        foreach (KeyValuePair<string, DynamicSFX> keyValuePair in this.allSFX)
          keyValuePair.Value.id = keyValuePair.Key;
        this.initialisedType = Disease.EDiseaseType.CURE;
      }
    }
    else if (this.initialisedType != Disease.EDiseaseType.BACTERIA)
    {
      this.allSFX.Clear();
      TextAsset textAsset = EncodedResources.Load("Data/Sound/sounds");
      if ((UnityEngine.Object) textAsset == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "sounds.txt Not Found");
        return;
      }
      DataImporter.UnserializeDataHash<DynamicSFX>((IDictionary<string, DynamicSFX>) this.allSFX, textAsset.text, "sounds");
      foreach (KeyValuePair<string, DynamicSFX> keyValuePair in this.allSFX)
        keyValuePair.Value.id = keyValuePair.Key;
      this.initialisedType = Disease.EDiseaseType.BACTERIA;
    }
    if ((UnityEngine.Object) this.newSource != (UnityEngine.Object) null && this.newSource.isPlaying)
      return;
    foreach (KeyValuePair<string, DynamicSFX> keyValuePair in this.allSFX)
    {
      DynamicSFX currentSFX = keyValuePair.Value;
      if ((currentSFX.playCount <= 0 || currentSFX.timesPlayed < currentSFX.playCount) && this.isSFXTime(currentSFX))
      {
        Debug.Log((object) ("PLAY SFX: " + currentSFX.id));
        this.PlaySounds(currentSFX);
        break;
      }
    }
  }

  private void PlaySounds(DynamicSFX currentSFX)
  {
    int index = this.randomSFX.FindIndex((Predicate<AudioClip>) (e => e.name == currentSFX.id));
    this.newSource = this.gameObject.AddComponent<AudioSource>();
    this.newSource.clip = this.randomSFX[index];
    this.newSource.volume = currentSFX.playVolume * CSoundManager.instance.SFXVolume;
    this.newSource.Play();
    if (currentSFX.playCount > 0)
      ++currentSFX.timesPlayed;
    currentSFX.nextPlayTime = this.pauseTime + (float) UnityEngine.Random.Range(currentSFX.pauseRandomMin, currentSFX.pauseRandomMax);
  }

  private bool isSFXTime(DynamicSFX currentSFX)
  {
    Disease disease = CGameManager.localPlayerInfo.disease;
    if ((double) currentSFX.conditionIsShadowDayDone > 0.0 && !disease.shadowDayDone || (double) currentSFX.conditionIsShadowDayDone < 0.0 && disease.shadowDayDone || (double) currentSFX.conditionInfectedMin != 0.0 && (double) disease.globalInfectedPercent + (double) disease.globalDeadPercent < (double) currentSFX.conditionInfectedMin || (double) currentSFX.conditionInfectedMax != 0.0 && (double) disease.globalInfectedPercent + (double) disease.globalDeadPercent > (double) currentSFX.conditionInfectedMax || (double) currentSFX.conditionDeadMin != 0.0 && (double) disease.globalDeadPercent + (double) disease.globalZombiePercent < (double) currentSFX.conditionDeadMin || (double) currentSFX.conditionDeadMax != 0.0 && (double) disease.globalDeadPercent + (double) disease.globalZombiePercent > (double) currentSFX.conditionDeadMax || (double) currentSFX.conditionHealthyMin != 0.0 && (double) disease.globalHealthyPercent < (double) currentSFX.conditionHealthyMin || (double) currentSFX.conditionZombieMin != 0.0 && (double) disease.globalZombiePercent < (double) currentSFX.conditionZombieMin || (double) currentSFX.conditionNumberOfZombieMin != 0.0 && (double) disease.totalZombie < (double) currentSFX.conditionNumberOfZombieMin || (double) currentSFX.conditionApeInfectedMin != 0.0 && (double) disease.apeTotalInfectedPercent < (double) currentSFX.conditionApeInfectedMin || (double) currentSFX.conditionApeDeadMax != 0.0 && (double) disease.apeTotalDeadPercent > (double) currentSFX.conditionApeDeadMax)
      return false;
    if ((double) currentSFX.nextPlayTime != 0.0)
      return (double) currentSFX.nextPlayTime <= (double) this.pauseTime;
    currentSFX.nextPlayTime = this.pauseTime + UnityEngine.Random.Range(0.0f, (float) currentSFX.randomDelay * 1f);
    return false;
  }

  private void RecycleSFX()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.newSource);
    this.newSource = (AudioSource) null;
  }

  public enum MusicState
  {
    None,
    Menu,
    Game,
    Faded,
    Stop,
  }
}
