// Decompiled with JetBrains decompiler
// Type: MoviePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.IO;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class MoviePlayer : MonoBehaviour
{
  public string movieTextureName;
  public VideoPlayer video;
  public MeshRenderer meshRenderer;
  public AudioSource audioSource;

  public void Initialise()
  {
    if (!CGameManager.playOnlineVideo)
    {
      this.video.source = VideoSource.VideoClip;
      this.video.clip = Resources.Load<VideoClip>(this.movieTextureName);
    }
    else
    {
      this.video.source = VideoSource.Url;
      this.video.url = CGameManager.onlineVideoLink;
    }
    this.video.audioOutputMode = VideoAudioOutputMode.AudioSource;
    this.video.EnableAudioTrack((ushort) 0, true);
    this.video.SetTargetAudioSource((ushort) 0, this.audioSource);
    this.video.Prepare();
    Debug.Log((object) ("LOADED CLIP: " + (object) this.video.clip));
  }

  public void Play()
  {
    Debug.Log((object) "PLAY VID");
    if (!CGameManager.playOnlineVideo)
    {
      this.video.source = VideoSource.VideoClip;
      this.video.clip = Resources.Load<VideoClip>(this.movieTextureName);
    }
    else
    {
      this.video.source = VideoSource.Url;
      this.video.url = CGameManager.onlineVideoLink;
    }
    this.video.Prepare();
    this.video.Play();
    this.audioSource.Play();
  }

  public void Stop()
  {
    if ((Object) this.video != (Object) null)
      this.video.Stop();
    DynamicMusic.instance.enabled = true;
    if (!CGameManager.playOnlineVideo)
      return;
    string pendingScenarioName = CGameManager.pendingScenarioName;
    CGameManager.canPlaySFX = false;
    CGameManager.ClearGame();
    DynamicMusic.instance.FadeOut();
    Scenario scenario = Scenario.LoadScenario("PIFSL " + pendingScenarioName, false, false, Path.Combine(CSLocalUGCHandler.GetScenarioDataPath(), "PIFSL " + pendingScenarioName));
    scenario.scenarioInformation.id = "PIFSL " + pendingScenarioName;
    CGameManager.gameType = IGame.GameType.Custom;
    CGameManager.CreateGame(scenario);
  }

  public void Pause()
  {
    if (!((Object) this.video != (Object) null))
      return;
    this.video.Pause();
  }

  public bool IsPlaying() => (Object) this.video != (Object) null && this.video.isPlaying;

  public float GetDuration() => (float) this.video.length;
}
