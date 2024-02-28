// Decompiled with JetBrains decompiler
// Type: CPauseMainSubScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using System;
using System.Collections.Generic;

#nullable disable
public class CPauseMainSubScreen : CMainSubScreen
{
  public UIButton buttonQuit;
  public UIButton buttonLoad;
  public UIButton buttonSave;
  public UIButton buttonRestart;
  public IGameSubScreen optionsSubscreen;
  public IGameSubScreen saveSubscreen;
  public IGameSubScreen loadSubscreen;
  public IGameSubScreen helpSubscreen;

  public override void Initialise()
  {
    base.Initialise();
    EventDelegate.Add(this.buttonQuit.onClick, new EventDelegate.Callback(this.Quit));
    EventDelegate.Add(this.buttonBack.onClick, new EventDelegate.Callback(((IGameSubScreen) this).Back));
    EventDelegate.Add(this.buttonRestart.onClick, new EventDelegate.Callback(this.Restart));
    this.buttonSave.GetComponent<ButtonChooseSubScreen>().onClickCallback = new Action<bool>(this.OnSaveClicked);
  }

  public override void SetActive(bool active)
  {
    base.SetActive(active);
    if (!active)
      return;
    this.SetLoadEnabled(CGameManager.saves.GetSavedGames().Count > 0);
    if (CGameManager.IsTutorialGame)
      this.buttonSave.GetComponent<ButtonChooseSubScreen>().enabled = false;
    else
      this.buttonSave.GetComponent<ButtonChooseSubScreen>().enabled = true;
  }

  private void Quit()
  {
    CConfirmOverlay cconfirmOverlay = CUIManager.instance.redConfirmOverlay;
    if (CGameManager.IsCureGame)
      cconfirmOverlay = CUIManager.instance.redConfirmOverlayCure;
    if ((this.saveSubscreen as CPauseSaveSubScreen).isGameSavedDuringThisPlaythrough)
      cconfirmOverlay.ShowLocalised("IG_Confirm_Exit_Title", "FE_Are_You_Sure", "No", "Yes", pressB: new CConfirmOverlay.PressDelegate(this.ConfirmQuit));
    else
      cconfirmOverlay.ShowLocalised("IG_No_Save_Game_Warning_Title", "IG_No_Save_Game_Warning_Text", "No", "Yes", pressB: new CConfirmOverlay.PressDelegate(this.ConfirmQuit));
  }

  private void ConfirmQuit()
  {
    string name = CGameManager.IsMultiplayerGame ? "Main_Sub_Multi" : "Main_Sub_NewGame";
    if (CGameManager.IsMultiplayerGame)
      CNetworkManager.network.TerminateAndReinitialise();
    CGameManager.ClearGame();
    CUIManager.instance.SetupScreens();
    DynamicMusic.instance.FadeOut();
    CGameManager.usingDiseaseX = false;
    IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
    CUIManager.instance.ClearHistory();
    List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
    igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
    CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
    igameSubScreenList.Clear();
    igameSubScreenList.Add(screen.GetSubScreen(name));
    CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
    TutorialSystem.Instance.ResetAndRewind();
  }

  private void Restart()
  {
    CConfirmOverlay cconfirmOverlay = CUIManager.instance.redConfirmOverlay;
    if (CGameManager.IsCureGame)
      cconfirmOverlay = CUIManager.instance.redConfirmOverlayCure;
    if ((this.saveSubscreen as CPauseSaveSubScreen).isGameSavedDuringThisPlaythrough)
      cconfirmOverlay.ShowLocalised("IG_Confirm_Exit_Title", "FE_Are_You_Sure", "No", "Yes", pressB: new CConfirmOverlay.PressDelegate(this.ConfirmRestart));
    else
      cconfirmOverlay.ShowLocalised("IG_No_Save_Game_Warning_Title", "IG_No_Save_Game_Warning_Text", "No", "Yes", pressB: new CConfirmOverlay.PressDelegate(this.ConfirmRestart));
  }

  public void ConfirmRestart()
  {
    IGame.GameType gameType = CGameManager.gameType;
    Disease.EDiseaseType diseaseType = CGameManager.localPlayerInfo.disease.diseaseType;
    Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
    CGSScreen cgsScreen = gameType == IGame.GameType.Cure || gameType == IGame.GameType.CureTutorial ? CUIManager.instance.GetScreen("GameSetupScreen_Cure") as CGSScreen : CUIManager.instance.GetScreen("GameSetupScreen") as CGSScreen;
    Disease.ECheatType[] cheats = cgsScreen.GetCheats();
    CGameManager.ClearGame();
    TutorialSystem.Instance.ResetAndRewind();
    DynamicMusic.instance.FadeOut();
    CUIManager.instance.ClearHistory();
    CUIManager.instance.SetupScreens();
    CGameManager.gameType = gameType;
    cgsScreen.SetCheats(cheats);
    cgsScreen.resetCheatsOnEnter = false;
    if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      TutorialSystem.Instance.ResetAndRewind();
    if (CGameManager.gameType == IGame.GameType.SpeedRun)
    {
      CGameManager.CreateGame(currentLoadedScenario, diseaseType);
    }
    else
    {
      if (CGameManager.usingDiseaseX)
        cgsScreen.DiseaseType = Disease.EDiseaseType.DISEASEX;
      CGameManager.CreateGame(currentLoadedScenario);
    }
    if (gameType != IGame.GameType.CureTutorial)
      cgsScreen.OnClickNext();
    if (gameType == IGame.GameType.Cure || gameType == IGame.GameType.CureTutorial)
      return;
    cgsScreen.OnClickNext();
  }

  public void SetSaveEnabled(bool b) => this.buttonSave.isEnabled = b;

  private void OnSaveClicked(bool active)
  {
    if (active)
      return;
    CUIManager.instance.standardConfirmOverlay.ShowLocalised("Tutorial_Save_Game_Warning_Title", "Tutorial_Save_Game_Warning_Text", buttonB: "OK");
  }

  public void SetLoadEnabled(bool b) => this.buttonLoad.isEnabled = b;

  public void QuitDirectly() => this.ConfirmQuit();

  public static void ExternalRestartGame()
  {
    IGame.GameType gameType = CGameManager.gameType;
    Disease.EDiseaseType diseaseType = CGameManager.localPlayerInfo.disease.diseaseType;
    Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
    CGSScreen cgsScreen = gameType == IGame.GameType.Cure || gameType == IGame.GameType.CureTutorial ? CUIManager.instance.GetScreen("GameSetupScreen_Cure") as CGSScreen : CUIManager.instance.GetScreen("GameSetupScreen") as CGSScreen;
    Disease.ECheatType[] cheats = cgsScreen.GetCheats();
    CGameManager.ClearGame();
    TutorialSystem.Instance.ResetAndRewind();
    DynamicMusic.instance.FadeOut();
    CUIManager.instance.ClearHistory();
    CUIManager.instance.SetupScreens();
    CGameManager.gameType = gameType;
    cgsScreen.SetCheats(cheats);
    cgsScreen.resetCheatsOnEnter = false;
    if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial)
      TutorialSystem.Instance.ResetAndRewind();
    if (CGameManager.gameType == IGame.GameType.SpeedRun)
    {
      CGameManager.CreateGame(currentLoadedScenario, diseaseType);
    }
    else
    {
      if (CGameManager.usingDiseaseX)
        cgsScreen.DiseaseType = Disease.EDiseaseType.DISEASEX;
      CGameManager.CreateGame(currentLoadedScenario);
    }
    if (gameType != IGame.GameType.CureTutorial)
      cgsScreen.OnClickNext();
    if (gameType == IGame.GameType.Cure || gameType == IGame.GameType.CureTutorial)
      return;
    cgsScreen.OnClickNext();
  }
}
