// Decompiled with JetBrains decompiler
// Type: CGSScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CGSScreen : IGameScreen, ITutorial
{
  public UIImageButton previousButton;
  public UIImageButton nextButton;
  public UILabel nextText;
  public UILabel titleLabel;
  public GameObject titleLabelContainer;
  public UIPanel progressPanel;
  public UILabel progressTitle;
  public UISlider progressBar;
  public UILabel progressValue;
  public CGSGeneSubScreen geneSubScreen;
  public GameObject renderTarget;
  private bool completed;
  private Disease.ECheatType[] cheats = new Disease.ECheatType[0];
  private int screenIndex;
  private IGameSetupSubScreen currentScreen;
  private List<IGameSetupSubScreen> availableSubScreens = new List<IGameSetupSubScreen>();
  private List<IGameSetupSubScreen> lockedSubScreens = new List<IGameSetupSubScreen>();
  public bool resetCheatsOnEnter = true;

  public List<Disease.EDiseaseType> DiseaseTypesUnlocked { get; set; }

  public List<Gene> AllGenes { get; set; }

  public List<Gene> UnlockedGenes { get; set; }

  public Disease.EDiseaseType DiseaseType { get; set; }

  public string DiseaseName { get; set; }

  public uint Difficulty { get; set; }

  public bool AllScreensSeen { get; set; }

  public Dictionary<Gene.EGeneCategory, Gene> SelectedGenes { get; set; }

  public void SetCheats(Disease.ECheatType[] c) => this.cheats = c;

  public Disease.ECheatType[] GetCheats() => this.cheats;

  public CGSScreen()
  {
    this.DiseaseTypesUnlocked = new List<Disease.EDiseaseType>();
    this.AllGenes = new List<Gene>();
    this.UnlockedGenes = new List<Gene>();
    this.SelectedGenes = new Dictionary<Gene.EGeneCategory, Gene>();
  }

  public override void Initialise()
  {
    base.Initialise();
    EventDelegate.Set(this.previousButton.onClick, new EventDelegate.Callback(this.OnClickPrevious));
    EventDelegate.Set(this.nextButton.onClick, new EventDelegate.Callback(this.OnClickNext));
    CActionManager.instance.AddListener("INPUT_ACCEPT", new Action<CActionManager.ActionType>(this.OnInsertAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.OnConfirmAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.OnClickPreviousAction), this.gameObject);
    TutorialSystem.RegisterInterface((ITutorial) this);
  }

  public void SetupCGS()
  {
    this.AllScreensSeen = false;
    if (this.cheats == null || this.resetCheatsOnEnter)
      this.cheats = new Disease.ECheatType[0];
    this.SelectedGenes.Clear();
    foreach (IGameSubScreen subScreen in this.subScreens)
    {
      if ((bool) (UnityEngine.Object) subScreen)
        subScreen.Setup();
    }
    CUIManager.instance.GetOverlay("Cheat_Overlay").Setup();
    this.completed = false;
    ((CGSDiseaseSubScreen) this.GetSubScreen("DiseaseSelection")).SetDiseaseInfoPanels();
    CGSDiseaseXSubScreen subScreen1 = (CGSDiseaseXSubScreen) this.GetSubScreen("DiseaseX_Customisation");
    if ((UnityEngine.Object) subScreen1 != (UnityEngine.Object) null && !subScreen1.setupComplete)
      subScreen1.SetupDiseaseXScreen(this);
    for (int index = 0; index < this.subScreens.Count; ++index)
    {
      this.subScreens[index].SetActive(false);
      ((IGameSetupSubScreen) this.subScreens[index]).setupComplete = false;
      ((IGameSetupSubScreen) this.subScreens[index]).screenIndex = index;
    }
    this.SetCanContinue(true);
    this.resetCheatsOnEnter = true;
  }

  public override void SetActive(bool active)
  {
    base.SetActive(active);
    if (!active)
      return;
    CUIManager.instance.SetHexGrid(CGameManager.IsCureGame);
    CInterfaceManager.instance.SetCursorSelection(EHudMode.Normal);
    this.SetupCGS();
    Camera_Zoom.instance.SetCameraToDefault();
    this.nextText.text = CLocalisationManager.GetText("FE_Continue");
    this.availableSubScreens.Clear();
    this.lockedSubScreens.Clear();
    for (int index = 0; index < this.subScreens.Count; ++index)
    {
      IGameSetupSubScreen subScreen = this.subScreens[index] as IGameSetupSubScreen;
      if (subScreen.CheckScreenLocked())
      {
        this.lockedSubScreens.Add(subScreen);
        subScreen.setupComplete = true;
      }
      else
        this.availableSubScreens.Add(subScreen);
    }
    this.currentScreen = this.availableSubScreens[this.screenIndex = 0];
    this.SetActiveSubScreen((IGameSubScreen) this.currentScreen);
  }

  private void OnClickPreviousAction(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    if (this.geneSubScreen.selectedGenePanel.activeInHierarchy || this.geneSubScreen.geneListPanel.activeInHierarchy)
    {
      this.geneSubScreen.ViewGene((Gene) null);
      this.geneSubScreen.ChooseCategory(Gene.EGeneCategory.NONE);
    }
    else
      this.OnClickPrevious();
  }

  public void OnClickPrevious()
  {
    if (!this.previousButton.isEnabled)
      return;
    if (this.currentScreen is CGSNameSubScreen)
      ((CGSNameSubScreen) this.currentScreen).SaveName();
    if (this.AllScreensSeen)
      this.AllScreensSeen = false;
    if ((UnityEngine.Object) this.currentScreen != (UnityEngine.Object) null && !this.currentScreen.CheckClickPrevious())
      return;
    if (this.screenIndex > 0)
    {
      if ((UnityEngine.Object) this.availableSubScreens[this.screenIndex - 1] == (UnityEngine.Object) this.GetSubScreen("DiseaseX_Customisation"))
      {
        if (this.DiseaseType == Disease.EDiseaseType.DISEASEX || CGameManager.usingDiseaseX)
        {
          this.SetActiveNonDiseaseXElements(false);
        }
        else
        {
          CGameManager.usingDiseaseX = false;
          if (CGameManager.IsCureGame)
            --this.screenIndex;
          this.SetActiveNonDiseaseXElements(true);
        }
        this.SetActiveSubScreen((IGameSubScreen) (this.currentScreen = this.availableSubScreens[--this.screenIndex]));
        CInterfaceManager.instance.SetDiseaseCreationProgress((float) this.screenIndex * 1f / (float) this.availableSubScreens.Count);
      }
      else
      {
        this.SetActiveSubScreen((IGameSubScreen) (this.currentScreen = this.availableSubScreens[--this.screenIndex]));
        CInterfaceManager.instance.SetDiseaseCreationProgress((float) this.screenIndex * 1f / (float) this.availableSubScreens.Count);
        this.SetActiveNonDiseaseXElements(true);
      }
    }
    else
    {
      CGameManager.ClearGame();
      this.SetupCGS();
      CUIManager.instance.SetHexGrid(false);
      CInterfaceManager.instance.SetCursorSelection(EHudMode.Normal);
      CUIManager.instance.BackToScreen("MainMenuScreen");
    }
    this.nextText.text = CLocalisationManager.GetText("FE_Continue");
    DynamicMusic.instance.SetMenuMenuMusic(0.35f);
  }

  private void SetActiveNonDiseaseXElements(bool b)
  {
    if ((UnityEngine.Object) this.renderTarget != (UnityEngine.Object) null)
      this.renderTarget.SetActive(b);
    this.nextButton.gameObject.SetActive(b);
    this.previousButton.gameObject.SetActive(b);
  }

  private void OnInsertAction(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START || !this.geneSubScreen.IsGeneSelected())
      return;
    this.geneSubScreen.ToggleGene();
  }

  private void OnConfirmAction(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.OnClickNext();
  }

  public void OnClickNext()
  {
    if (this.completed || (UnityEngine.Object) this.currentScreen == (UnityEngine.Object) null || !this.currentScreen.ChooseOption())
      return;
    int num1 = this.availableSubScreens.Count - 2;
    int num2 = this.availableSubScreens.Count - 1;
    int num3 = 1;
    if (this.screenIndex == num1)
      this.nextText.text = CLocalisationManager.GetText("FE_Play");
    if (this.screenIndex < num2)
    {
      if (this.screenIndex < num3)
      {
        if (this.DiseaseType == Disease.EDiseaseType.DISEASEX)
        {
          CGameManager.usingDiseaseX = true;
          this.SetActiveNonDiseaseXElements(false);
        }
        else
        {
          if (CGameManager.IsCureGame)
            ++this.screenIndex;
          this.SetActiveNonDiseaseXElements(true);
        }
      }
      else
        this.SetActiveNonDiseaseXElements(true);
      this.SetActiveSubScreen((IGameSubScreen) (this.currentScreen = this.availableSubScreens[++this.screenIndex]));
      CInterfaceManager.instance.SetDiseaseCreationProgress((float) this.screenIndex * 1f / (float) this.availableSubScreens.Count);
      DynamicMusic.instance.SetMenuMenuMusic(-0.35f);
    }
    else
    {
      this.completed = true;
      this.CompleteGameSetup();
    }
  }

  public void JumpToSubScreen(IGameSetupSubScreen screen)
  {
    if (this.lockedSubScreens.Contains(screen) || this.screenIndex != this.availableSubScreens.Count)
      return;
    int index;
    for (index = 0; index < this.availableSubScreens.Count; ++index)
    {
      if ((UnityEngine.Object) this.availableSubScreens[index] == (UnityEngine.Object) screen)
      {
        this.currentScreen = screen;
        this.screenIndex = index;
        this.SetActiveSubScreen((IGameSubScreen) this.currentScreen);
        ++index;
        break;
      }
    }
    for (; index < this.availableSubScreens.Count; ++index)
      this.availableSubScreens[index].SetActive(false);
    this.nextText.text = CLocalisationManager.GetText("FE_Continue");
    CInterfaceManager.instance.SetDiseaseCreationProgress((float) this.screenIndex / 5f);
  }

  public override void SetActiveSubScreen(IGameSubScreen screen)
  {
    base.SetActiveSubScreen(screen);
    foreach (IGameSubScreen availableSubScreen in this.availableSubScreens)
    {
      if ((UnityEngine.Object) availableSubScreen != (UnityEngine.Object) screen)
        availableSubScreen.SetActive(false);
    }
    if ((UnityEngine.Object) screen == (UnityEngine.Object) null)
    {
      this.progressPanel.gameObject.SetActive(false);
      this.titleLabel.text = CLocalisationManager.GetText("FE_Plagues_Summary_Title");
    }
    else
    {
      if (!(screen is IGameSetupSubScreen))
        return;
      IGameSetupSubScreen igameSetupSubScreen = screen as IGameSetupSubScreen;
      if (igameSetupSubScreen.showTitle)
      {
        this.titleLabelContainer.SetActive(true);
        this.titleLabel.text = CLocalisationManager.GetText(igameSetupSubScreen.title);
      }
      else
        this.titleLabelContainer.SetActive(false);
      if (igameSetupSubScreen.progressBar)
      {
        this.progressPanel.gameObject.SetActive(true);
        float progress = igameSetupSubScreen.GetProgress();
        this.progressTitle.text = CLocalisationManager.GetText(igameSetupSubScreen.progressTitle);
        this.progressValue.text = Mathf.FloorToInt(progress * 100f).ToString() + "%";
        this.progressBar.value = progress;
      }
      else
        this.progressPanel.gameObject.SetActive(false);
    }
  }

  public void SetCanContinue(bool enabled) => this.nextButton.isEnabled = enabled;

  public void CompleteGameSetup()
  {
    this.SetCanContinue(false);
    this.StartCoroutine(this.FinishGameSetup());
  }

  private IEnumerator FinishGameSetup()
  {
    yield return (object) null;
    CInterfaceManager.instance.SetPortRendererState(true);
    if (CGameManager.IsCureGame)
    {
      if (CGameManager.usingDiseaseX && this.DiseaseType == Disease.EDiseaseType.DISEASEX)
        this.DiseaseType = CGameManager.game.diseaseSimulator.diseaseType;
      ScenarioInformation cureScenario = CGameManager.GetCureScenario(this.DiseaseType);
      Scenario withScenario = Scenario.LoadScenario(cureScenario.filePath, true, false);
      withScenario.scenarioInformation = cureScenario;
      withScenario.isOfficial = true;
      if (CGameManager.IsFederalScenario("PIFCURE"))
        withScenario = CGameManager.game.CurrentLoadedScenario;
      if (CGameManager.IsFederalScenario("PIFCURE"))
        CGameManager.game.CreateGame(withScenario, CGameManager.game.SetupParameters.defaultDiseaseType);
      else
        CGameManager.game.CreateGame(withScenario);
      CGameManager.game.Difficulty = this.Difficulty;
      CGameManager.game.ChooseCheats(this.cheats);
      CGameManager.game.ChooseDisease(this.DiseaseName, Disease.EDiseaseType.CURE);
    }
    else
    {
      CGameManager.game.Difficulty = this.Difficulty;
      CGameManager.game.ChooseCheats(this.cheats);
      CGameManager.game.ChooseDisease(this.DiseaseName, this.DiseaseType);
    }
    CGameManager.game.ChooseGenes(new List<Gene>((IEnumerable<Gene>) this.SelectedGenes.Values).ToArray());
    CInterfaceManager.instance.SetDiseaseCreationProgress(1f);
    DynamicMusic.instance.FadeOut();
    CInterfaceManager.instance.InitialiseCountryViews();
    if (!CGameManager.IsMultiplayerGame || CNetworkManager.network.IsServer)
    {
      if (CGameManager.IsCureGame)
        CUIManager.instance.SetActiveScreen("StartCountryScreen_Cure");
      else
        CUIManager.instance.SetActiveScreen("StartCountryScreen");
    }
  }

  public void ChooseGene(Gene gene) => this.SelectedGenes[gene.geneCategory] = gene;

  public void ClearGene(Gene.EGeneCategory category) => this.SelectedGenes.Remove(category);

  public Gene GetSelectedGene(Gene.EGeneCategory category)
  {
    return this.SelectedGenes.ContainsKey(category) ? this.SelectedGenes[category] : (Gene) null;
  }

  public void OnTutorialBegin(Module withModule)
  {
    string name = withModule.Name;
    if (!(name == "32A") && !(name == "33A") && !(name == "34A"))
      return;
    EventDelegate.Remove(this.previousButton.onClick, new EventDelegate.Callback(this.OnClickPrevious));
    EventDelegate.Remove(this.nextButton.onClick, new EventDelegate.Callback(this.OnClickNext));
    CActionManager.instance.RemoveListener("INPUT_ACCEPT", new Action<CActionManager.ActionType>(this.OnInsertAction), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.OnConfirmAction), this.gameObject);
    CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.OnClickPreviousAction), this.gameObject);
  }

  public void OnTutorialComplete(Module completedModule)
  {
    string name = completedModule.Name;
    if (!(name == "33B") && !(name == "32B") && !(name == "34D"))
      return;
    EventDelegate.Set(this.previousButton.onClick, new EventDelegate.Callback(this.OnClickPrevious));
    EventDelegate.Set(this.nextButton.onClick, new EventDelegate.Callback(this.OnClickNext));
    CActionManager.instance.AddListener("INPUT_ACCEPT", new Action<CActionManager.ActionType>(this.OnInsertAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.OnConfirmAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.OnClickPreviousAction), this.gameObject);
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    EventDelegate.Set(this.previousButton.onClick, new EventDelegate.Callback(this.OnClickPrevious));
    EventDelegate.Set(this.nextButton.onClick, new EventDelegate.Callback(this.OnClickNext));
    CActionManager.instance.AddListener("INPUT_ACCEPT", new Action<CActionManager.ActionType>(this.OnInsertAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.OnConfirmAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.OnClickPreviousAction), this.gameObject);
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }
}
