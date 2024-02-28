// Decompiled with JetBrains decompiler
// Type: CSplashScreenVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CSplashScreenVideo : MonoBehaviour
{
  public MoviePlayer moviePlayer;
  public GameObject scaleObject;
  private bool playbackStarted;
  private bool skipping;

  private void Start()
  {
    if (COptionsManager.instance.videoSettings.playVideo == EState.On || CGameManager.playOnlineVideo)
    {
      this.moviePlayer.Initialise();
      this.StartCoroutine(this.BeginPlayback());
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      CSteamControllerManager.instance.LockAndHideCursor(true);
    }
    else
      this.EscapeScreen();
  }

  private IEnumerator BeginPlayback()
  {
    yield return (object) new WaitForSeconds(1f);
    CUIManager.instance.ClearAllCameras();
    CUIManager.instance.hexGrid.GetComponent<UIPanel>().SetAlphaRecursive(0.0f, true);
    while ((Object) this.moviePlayer.video == (Object) null || !this.moviePlayer.video.isPrepared)
      yield return (object) null;
    Debug.Log((object) "BEGIN PLAYBACK");
    CUIManager.instance.IntroSplashReadyToPlay();
    this.moviePlayer.Play();
  }

  public void EscapeScreen()
  {
    if (this.skipping)
      return;
    Debug.Log((object) "SKIP!");
    this.skipping = true;
    this.StartCoroutine(this.CoLoadGame());
  }

  private IEnumerator CoLoadGame()
  {
    CSplashScreenVideo csplashScreenVideo = this;
    if ((bool) (Object) csplashScreenVideo.moviePlayer)
      csplashScreenVideo.moviePlayer.Pause();
    yield return (object) new WaitForSeconds(0.5f);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    CSteamControllerManager.instance.LockAndHideCursor(false);
    CUIManager.instance.hexGrid.GetComponent<UIPanel>().SetAlphaRecursive(1f, true);
    if (!CUIManager.instance.canSkipVideo)
    {
      if ((bool) (Object) csplashScreenVideo.moviePlayer.gameObject)
        csplashScreenVideo.moviePlayer.gameObject.SetActive(false);
      Debug.Log((object) "WAIT TILL WE CAN SKIP");
      CUIManager.instance.SetActiveScreen("SplashLoadingScreen");
      while (!CUIManager.instance.canSkipVideo)
        yield return (object) null;
    }
    DynamicMusic.instance.StartMusic();
    CUIManager.instance.ShowInitialScreen();
    if ((bool) (Object) csplashScreenVideo.moviePlayer)
      csplashScreenVideo.moviePlayer.Stop();
    if (csplashScreenVideo.moviePlayer.gameObject.activeSelf)
      csplashScreenVideo.moviePlayer.gameObject.SetActive(false);
    if (Application.isPlaying)
    {
      Object.DestroyImmediate((Object) csplashScreenVideo.moviePlayer.video.clip);
      Object.Destroy((Object) csplashScreenVideo.moviePlayer.gameObject);
    }
    yield return (object) new WaitForSeconds(0.5f);
    Resources.UnloadUnusedAssets();
    yield return (object) new WaitForSeconds(1f);
    csplashScreenVideo.gameObject.SetActive(false);
  }

  private void Update()
  {
    if (!(bool) (Object) this.moviePlayer || !(bool) (Object) this.moviePlayer.video)
      return;
    if (this.playbackStarted)
    {
      if (Input.anyKeyDown)
        this.EscapeScreen();
      if (!this.moviePlayer.video.isPlaying)
      {
        this.EscapeScreen();
      }
      else
      {
        for (int button = 0; button < 3; ++button)
        {
          if (Input.GetMouseButton(button))
          {
            this.EscapeScreen();
            return;
          }
        }
        CSteamControllerManager instance = CSteamControllerManager.instance;
        if (!instance.GetAction(instance.currentSelectAction))
          return;
        this.EscapeScreen();
      }
    }
    else
    {
      if (!this.moviePlayer.video.isPlaying)
        return;
      this.playbackStarted = true;
    }
  }
}
