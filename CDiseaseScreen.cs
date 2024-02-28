// Decompiled with JetBrains decompiler
// Type: CDiseaseScreen
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
public class CDiseaseScreen : IGameScreen, ITutorial
{
  public static CDiseaseScreen instance;
  internal Disease disease;
  private string bodyScreen = string.Empty;
  private Technology previewTech;
  private Technology previewTechDevolve;
  private Dictionary<Technology, TechHex> timerHexes = new Dictionary<Technology, TechHex>();
  private int currentSubscreen;
  internal CDiseaseBodySubScreen bodySubScreen;
  [Header("UI Elements")]
  public UILabel diseaseTitle;
  public UIToggle[] diseaseToggles;
  public UIButton buttonGraphExpand;
  public UIEventListener wholeGraphButton;
  [Header("Genes UI Elements")]
  public UISprite[] geneModification;
  public UISprite[] geneModificationBackground;
  public TooltipDiseaseGenesObject[] geneTooltips;
  [Header("Disease Stats UI Elements")]
  public UISlider infectivity;
  public UISlider severity;
  public UISlider lethality;
  public UISlider resources;
  public UISlider infectivityPreview;
  public UISlider severityPreview;
  public UISlider lethalityPreview;
  private int lastDna;
  [Header("DNA UI Elements")]
  public UILabel dnaPoints;
  public UILabel dnaIncreaseLabel;
  public UILabel dnaIncreaseDirection;
  public UITweener[] dnaIncreaseTweens;
  public UILabelAutotranslate abilitiesToggleText;
  public UILabelAutotranslate abilitiesTraitText;
  public UILabelAutotranslate abilitiesDescriptionText;
  public IGameSubScreen diseaseUpgradeDefault;
  public IGameSubScreen diseaseUpgradeScreen;
  private Color increaseColor = Color.white;
  private Color decreaseColor;
  private Coroutine tutorial12ACoroutine;

  private void Awake() => CDiseaseScreen.instance = this;

  public override void Initialise()
  {
    base.Initialise();
    this.decreaseColor = this.dnaIncreaseLabel.color;
    for (int index = 0; index < this.diseaseToggles.Length; ++index)
      EventDelegate.Set(this.diseaseToggles[index].onChange, new EventDelegate.Callback(this.ChangeTab));
    EventDelegate.Set(this.buttonGraphExpand.onClick, new EventDelegate.Callback(this.GraphExpandClick));
    this.wholeGraphButton.onDoubleClick += (UIEventListener.VoidDelegate) (_ => this.GraphExpandClick());
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.AddListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
    CActionManager.instance.AddListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_TAB_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_TAB_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
    TutorialSystem.RegisterInterface((ITutorial) this);
  }

  public override void Setup()
  {
    base.Setup();
    this.currentSubscreen = 0;
    this.timerHexes.Clear();
    ((CUpgradeSubScreen) this.diseaseUpgradeScreen).Reset();
    ((CUpgradeSubScreen) this.diseaseUpgradeScreen).EnableEvolveButtons(true);
    if (!CGameManager.IsCureGame)
      return;
    this.AddSubScreenSubstitution("Disease_SubScreen", "Control_Subscreen");
    this.AddSubScreenSubstitution("Transmission_SubScreen", "Quarantine_Subscreen");
    this.AddSubScreenSubstitution("Symptoms_SubScreen", "Response_Subscreen");
    this.AddSubScreenSubstitution("Abilities_SubScreen", "Operation_Subscreen");
    this.AddSubScreenSubstitution("DiseaseUpgrade_Default_SubScreen", "CureUpgrade_Default_SubScreen");
    this.AddSubScreenSubstitution("DiseaseUpgrade_SubScreen", "CureUpgrade_SubScreen");
    this.AddSubScreenSubstitution("DiseaseStats_SubScreen", "ControlStats_SubScreen");
    this.AddSubScreenSubstitution("Disease_Window_Small", "CureMode_CountryState");
  }

  public override void SetActive(bool isActive)
  {
    if (isActive)
    {
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial && !this.CheckCureTutorialRestrictions())
        return;
      if ((UnityEngine.Object) CHUDScreen.instance != (UnityEngine.Object) null)
        CHUDScreen.instance.AutoCloseChat();
      if (CGameManager.IsTutorialGame)
      {
        if (CGameManager.gameType == IGame.GameType.Tutorial)
        {
          TutorialSystem.MarkModuleComplete("6A");
          TutorialSystem.MarkModuleComplete("11A");
          TutorialSystem.MarkModuleComplete("11B");
          if (this.tutorial12ACoroutine != null)
            CUIManager.instance.StopCoroutine(this.tutorial12ACoroutine);
          this.tutorial12ACoroutine = CUIManager.instance.StartCoroutine(this.Execute12ATutorial());
          CGameManager.SetPaused(true, true);
        }
        if (CGameManager.gameType == IGame.GameType.CureTutorial)
        {
          TutorialSystem.MarkModuleComplete("C1");
          TutorialSystem.MarkModuleComplete("C2");
          TutorialSystem.MarkModuleComplete("C3");
          PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
          instance.StartCoroutine(instance.UpdateTutorial());
          CGameManager.SetPaused(true, true);
        }
      }
      this.dnaIncreaseLabel.gameObject.SetActive(false);
      this.disease = CInterfaceManager.instance.localPlayerDisease;
      switch (this.disease.diseaseType)
      {
        case Disease.EDiseaseType.NEURAX:
          this.bodyScreen = "Disease_Sub_Neurax";
          break;
        case Disease.EDiseaseType.NECROA:
          this.bodyScreen = "Disease_Sub_Necroa";
          break;
        case Disease.EDiseaseType.SIMIAN_FLU:
          this.bodyScreen = "Disease_Sub_Simian";
          break;
        case Disease.EDiseaseType.VAMPIRE:
          this.bodyScreen = "Disease_Sub_Vampire";
          break;
        case Disease.EDiseaseType.CURE:
          this.bodyScreen = "Disease_Sub_Cure";
          break;
        default:
          this.bodyScreen = "Disease_Sub_Body";
          break;
      }
      if (this.disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
      {
        if (this.abilitiesToggleText.originalLabelText != "Vampires")
        {
          this.abilitiesToggleText.SetInitialText("Vampires");
          this.abilitiesTraitText.SetInitialText("IG_Vampires_Trait");
          this.abilitiesDescriptionText.SetInitialText("Spend DNA points to develop your vampire's powers!");
        }
      }
      else if (this.disease.diseaseType != Disease.EDiseaseType.CURE && this.abilitiesToggleText.originalLabelText != "Abilities")
      {
        this.abilitiesToggleText.SetInitialText("Abilities");
        this.abilitiesTraitText.SetInitialText("IG_Abilities_Trait");
        this.abilitiesDescriptionText.SetInitialText("IG_Abilities_Default");
      }
      if (this.disease != null)
      {
        this.lastDna = this.disease.evoPoints;
        this.diseaseTitle.text = CLocalisationManager.GetText("IG_Overview").ToUpper().Replace("%S", this.disease.name);
        this.dnaPoints.text = this.disease.evoPoints.ToString();
        float[] infSevLethTotalMax = this.disease.GetInfSevLethTotalMax();
        this.infectivity.value = infSevLethTotalMax[0];
        this.severity.value = infSevLethTotalMax[1];
        this.lethality.value = infSevLethTotalMax[2];
        if ((bool) (UnityEngine.Object) this.resources)
          this.resources.value = Mathf.Clamp01((float) this.disease.evoPoints / 100f);
        int index;
        for (index = 0; index < this.disease.genes.Count; ++index)
        {
          Gene gene = this.disease.genes[index];
          if (index < this.geneModification.Length && (UnityEngine.Object) this.geneModification[index] != (UnityEngine.Object) null)
          {
            UISprite uiSprite = this.geneModification[index];
            uiSprite.spriteName = gene.geneGraphic.Replace("'", "");
            uiSprite.transform.parent.gameObject.SetActive(true);
            this.geneTooltips[index].gene = gene;
            switch (gene.geneCategory)
            {
              case Gene.EGeneCategory.zombie:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen_Necroa";
                continue;
              case Gene.EGeneCategory.simian1:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen_Human";
                continue;
              case Gene.EGeneCategory.simian2:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen_Ape";
                continue;
              case Gene.EGeneCategory.blood:
              case Gene.EGeneCategory.flight:
              case Gene.EGeneCategory.shadow:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen_Vampire";
                continue;
              case Gene.EGeneCategory.cure_transmission:
              case Gene.EGeneCategory.cure_quarantine:
              case Gene.EGeneCategory.cure_country:
              case Gene.EGeneCategory.cure_abilities:
              case Gene.EGeneCategory.cure_operation:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen_Cure";
                continue;
              default:
                this.geneModificationBackground[index].spriteName = "GeneModification_DiseaseScreen";
                continue;
            }
          }
        }
        for (; index < this.geneModification.Length; ++index)
          this.geneModification[index].transform.parent.gameObject.SetActive(false);
      }
      ((CUpgradeSubScreen) this.diseaseUpgradeScreen).EnableEvolveButtons(true);
      foreach (KeyValuePair<Technology, TechHex> timerHex in this.timerHexes)
        timerHex.Value.StartEvolveAnim();
    }
    else
    {
      CUIManager.instance.HideOverlay("Red_Confirm_Overlay_Devolve");
      CUIManager.instance.HideOverlay("Red_Confirm_Overlay_Devolve_Cure");
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        TutorialSystem.CureTutorialFreeTechSelection = false;
      }
      if (CGameManager.IsTutorialGame)
        CGameManager.SetPaused(false, true);
    }
    base.SetActive(isActive);
    if (!isActive)
      return;
    this.diseaseToggles[this.currentSubscreen].value = true;
    this.SetTab();
  }

  public void OnGameStarted()
  {
    (this.GetSubScreen("EvolutionHistory") as CEvolutionHistorySubScreen).OnGameStarted();
  }

  public override void Refresh()
  {
    base.Refresh();
    this.dnaIncreaseLabel.depth = 10000;
    if (this.disease != null)
    {
      int num = this.disease.evoPoints - this.lastDna;
      if (num != 0)
      {
        this.dnaIncreaseLabel.gameObject.SetActive(true);
        if (this.disease.diseaseType == Disease.EDiseaseType.CURE)
        {
          this.dnaIncreaseLabel.text = CUtils.FormatValueToDisplay((float) num, showPlus: true);
          this.dnaIncreaseLabel.color = num > 0 ? this.increaseColor : this.decreaseColor;
        }
        else
        {
          this.dnaIncreaseLabel.text = Mathf.Abs(num).ToString();
          this.dnaIncreaseDirection.text = num > 0 ? "+" : "-";
          this.dnaIncreaseLabel.color = num > 0 ? this.increaseColor : this.decreaseColor;
          this.dnaIncreaseDirection.color = num > 0 ? this.increaseColor : this.decreaseColor;
        }
        for (int index = 0; index < this.dnaIncreaseTweens.Length; ++index)
        {
          this.dnaIncreaseTweens[index].Reset();
          this.dnaIncreaseTweens[index].PlayForward();
        }
      }
    }
    this.RefreshPreview();
  }

  public void RefreshPreview()
  {
    if (this.disease == null)
      return;
    if ((bool) (UnityEngine.Object) this.resources)
      this.resources.value = Mathf.Clamp01((float) this.disease.evoPoints / 100f);
    this.lastDna = this.disease.evoPoints;
    this.dnaPoints.text = this.disease.evoPoints.ToString();
    this.dnaPoints.width = 2000;
    this.dnaPoints.depth = 200;
    this.dnaPoints.color = new Color(0.85f, 0.6f, 1f);
    if (!this.disease.isCure)
    {
      UILabel dnaPoints = this.dnaPoints;
      string str1 = string.Format("{0,-45}", (object) this.disease.evoPoints);
      string str2 = this.disease.globalInfectiousness.ToString("N1");
      float num = this.disease.globalInfectiousnessMax;
      string str3 = num.ToString("N1");
      string str4 = string.Format("{0,-68}", (object) (str2 + "/" + str3));
      num = this.disease.globalSeverity;
      string str5 = num.ToString("N1");
      num = this.disease.globalSeverityMax;
      string str6 = num.ToString("N1");
      string str7 = string.Format("{0,-68}", (object) (str5 + "/" + str6));
      num = this.disease.globalLethality;
      string str8 = num.ToString("N1");
      num = this.disease.globalLethalityMax;
      string str9 = num.ToString("N1");
      string str10 = string.Format("{0,-20}", (object) (str8 + "/" + str9));
      string str11 = str1 + str4 + str7 + str10;
      dnaPoints.text = str11;
    }
    float[] numArray = new float[3];
    float[] infSevLethTotalMax = this.disease.GetInfSevLethTotalMax();
    if (this.previewTech != null && this.disease.IsTechEvolved(this.previewTech))
      this.previewTech = (Technology) null;
    if (this.previewTechDevolve != null && !this.disease.IsTechEvolved(this.previewTechDevolve))
      this.previewTechDevolve = (Technology) null;
    float[] infSevLethChange;
    if (this.previewTech != null)
    {
      infSevLethChange = this.disease.GetInfSevLethChange(this.previewTech, false);
    }
    else
    {
      if (this.previewTechDevolve == null)
      {
        this.infectivity.value = infSevLethTotalMax[0] - (float) (int) infSevLethTotalMax[0];
        this.infectivityPreview.value = 0.0f;
        this.severityPreview.value = 0.0f;
        this.lethalityPreview.value = 0.0f;
        this.severity.value = infSevLethTotalMax[1] - (float) (int) infSevLethTotalMax[1];
        this.lethality.value = infSevLethTotalMax[2] - (float) (int) infSevLethTotalMax[2];
        if ((double) infSevLethTotalMax[0] > 1.0)
        {
          this.infectivity.value = infSevLethTotalMax[0] - (float) (int) infSevLethTotalMax[0];
          this.infectivityPreview.value = 1f;
        }
        if ((double) infSevLethTotalMax[1] > 1.0)
        {
          this.severity.value = infSevLethTotalMax[1] - (float) (int) infSevLethTotalMax[1];
          this.severityPreview.value = 1f;
        }
        if ((double) infSevLethTotalMax[2] <= 1.0)
          return;
        this.lethality.value = infSevLethTotalMax[2] - (float) (int) infSevLethTotalMax[2];
        this.lethalityPreview.value = 1f;
        return;
      }
      infSevLethChange = this.disease.GetInfSevLethChange(this.previewTechDevolve, true);
    }
    float num1 = infSevLethTotalMax[0];
    float num2 = infSevLethTotalMax[1];
    float num3 = infSevLethTotalMax[2];
    float num4 = infSevLethTotalMax[0] + infSevLethChange[0];
    float num5 = infSevLethTotalMax[1] + infSevLethChange[1];
    float num6 = infSevLethTotalMax[2] + infSevLethChange[2];
    if ((double) infSevLethChange[0] > 0.0)
    {
      if ((int) num4 != (int) num1)
      {
        this.infectivity.value = 0.0f;
        this.infectivityPreview.value = num4 - (float) (int) num4;
      }
      else
      {
        this.infectivity.value = num1 - (float) (int) num1;
        this.infectivityPreview.value = num4 - (float) (int) num4;
      }
    }
    else if ((int) num4 == (int) num1)
    {
      this.infectivity.value = num4 - (float) (int) num4;
      this.infectivityPreview.value = num1 - (float) (int) num1;
    }
    else
    {
      this.infectivity.value = num4 - (float) (int) num4;
      this.infectivityPreview.value = 1f;
    }
    if ((double) infSevLethChange[1] > 0.0)
    {
      if ((int) num5 != (int) num2)
      {
        this.severity.value = 0.0f;
        this.severityPreview.value = num5 - (float) (int) num5;
      }
      else
      {
        this.severity.value = num2 - (float) (int) num2;
        this.severityPreview.value = num5 - (float) (int) num5;
      }
    }
    else if ((int) num5 == (int) num2)
    {
      this.severity.value = num5 - (float) (int) num5;
      this.severityPreview.value = num2 - (float) (int) num2;
    }
    else
    {
      this.severity.value = num5 - (float) (int) num5;
      this.severityPreview.value = 1f;
    }
    if ((double) infSevLethChange[2] > 0.0)
    {
      if ((int) num6 == (int) num3)
      {
        this.lethality.value = num3 - (float) (int) num3;
        this.lethalityPreview.value = num6 - (float) (int) num6;
      }
      else
      {
        this.lethality.value = 0.0f;
        this.lethalityPreview.value = num6 - (float) (int) num6;
      }
    }
    else if ((int) num6 == (int) num3)
    {
      this.lethality.value = num6 - (float) (int) num6;
      this.lethalityPreview.value = num3 - (float) (int) num3;
    }
    else
    {
      this.lethality.value = num6 - (float) (int) num6;
      this.lethalityPreview.value = 1f;
    }
  }

  public void PreviewTech(Technology tech)
  {
    if (tech != null && this.disease != null && this.disease.IsTechEvolved(tech))
    {
      this.previewTech = (Technology) null;
      this.previewTechDevolve = (Technology) null;
      this.RefreshPreview();
    }
    else
    {
      this.previewTech = tech == null || this.disease == null ? (Technology) null : tech;
      this.previewTechDevolve = (Technology) null;
      this.RefreshPreview();
    }
  }

  public void PreviewTechDevolve(Technology tech)
  {
    this.previewTechDevolve = tech == null || this.disease == null || !this.disease.IsTechEvolved(tech) ? (Technology) null : tech;
    this.previewTech = (Technology) null;
    this.RefreshPreview();
  }

  private void GoBack(CActionManager.ActionType type)
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial && type == CActionManager.ActionType.START)
    {
      if (!this.CheckCureTutorialRestrictions())
        return;
      TutorialSystem.CureTutorialFreeTechSelection = false;
    }
    if (type != CActionManager.ActionType.START || CUIManager.instance.IsActiveOverlay("Red_Confirm_Overlay_Devolve") || CUIManager.instance.IsActiveOverlay("Red_Confirm_Overlay_Devolve_Cure"))
      return;
    CUIManager.instance.SetActiveScreen("HUDScreen");
  }

  private void GoToHUD(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    if (CGameManager.IsTutorialGame)
    {
      if (TutorialSystem.IsModuleActive("14B"))
      {
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
      }
      CUIManager.instance.WaitForFrame(new Action(CUIManager.instance.OpenGameScreen));
      TutorialSystem.CureTutorialFreeTechSelection = false;
    }
    else
      CUIManager.instance.OpenGameScreen();
  }

  private void GoToWorld(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    CUIManager.instance.OpenWorldScreen();
  }

  private void GraphExpandClick()
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial || CGameManager.IsTutorialGame && TutorialSystem.IsModuleActive("12A") || TutorialSystem.IsModuleActive("12B") || TutorialSystem.IsModuleActive("13A") || TutorialSystem.IsModuleActive("13B") || TutorialSystem.IsModuleActive("13C") || TutorialSystem.IsModuleActive("14A") || TutorialSystem.IsModuleActive("14B"))
      return;
    CUIManager.instance.OpenGraphScreen("Disease");
  }

  private void ChangeTab1(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.diseaseToggles[0].value = true;
  }

  private void ChangeTab2(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.diseaseToggles[1].value = true;
  }

  private void ChangeTab3(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.diseaseToggles[2].value = true;
  }

  private void ChangeTab4(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    this.diseaseToggles[3].value = true;
  }

  private void TabLeft(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START)
      return;
    int index = this.currentSubscreen - 1;
    if (index < 0)
      index = this.diseaseToggles.Length - 1;
    if (!this.diseaseToggles[index].enabled)
      return;
    this.diseaseToggles[index].value = true;
    this.currentSubscreen = index;
  }

  private void TabRight(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START)
      return;
    int index = this.currentSubscreen + 1;
    if (index >= this.diseaseToggles.Length)
      index = 0;
    if (!this.diseaseToggles[index].enabled)
      return;
    this.diseaseToggles[index].value = true;
    this.currentSubscreen = index;
  }

  private void ChangeTab()
  {
    this.currentSubscreen = Array.IndexOf<UIToggle>(this.diseaseToggles, UIToggle.current);
    this.SetTab();
  }

  private void SetTab()
  {
    switch (this.currentSubscreen)
    {
      case 0:
        this.SetActiveSubScreen(this.GetSubScreen("Disease_SubScreen"));
        this.SetActiveSubScreen(this.GetSubScreen("DiseaseStats_SubScreen"));
        this.SetActiveSubScreen(this.GetSubScreen("Disease_Sub_Graph"));
        this.SetActiveSubScreen(this.GetSubScreen("EvolutionHistory"));
        break;
      case 1:
        this.SetActiveSubScreen(this.GetSubScreen("DiseaseUpgrade_Default_SubScreen"));
        this.SetActiveSubScreen(this.GetSubScreen("Disease_Window_Small"));
        this.SetActiveSubScreen(this.GetSubScreen("Transmission_SubScreen"));
        this.bodySubScreen = this.GetSubScreen(this.bodyScreen) as CDiseaseBodySubScreen;
        this.SetActiveSubScreen((IGameSubScreen) this.bodySubScreen);
        this.bodySubScreen.Refresh();
        break;
      case 2:
        this.SetActiveSubScreen(this.GetSubScreen("DiseaseUpgrade_Default_SubScreen"));
        this.SetActiveSubScreen(this.GetSubScreen("Disease_Window_Small"));
        this.SetActiveSubScreen(this.GetSubScreen("Symptoms_SubScreen"));
        this.bodySubScreen = this.GetSubScreen(this.bodyScreen) as CDiseaseBodySubScreen;
        this.SetActiveSubScreen((IGameSubScreen) this.bodySubScreen);
        this.bodySubScreen.Refresh();
        break;
      case 3:
        this.SetActiveSubScreen(this.GetSubScreen("DiseaseUpgrade_Default_SubScreen"));
        this.SetActiveSubScreen(this.GetSubScreen("Disease_Window_Small"));
        this.SetActiveSubScreen(this.GetSubScreen("Abilities_SubScreen"));
        this.bodySubScreen = this.GetSubScreen(this.bodyScreen) as CDiseaseBodySubScreen;
        this.SetActiveSubScreen((IGameSubScreen) this.bodySubScreen);
        this.bodySubScreen.Refresh();
        break;
    }
  }

  public void EvolveDisease(TechHex hex, Technology tech, bool devolve, int cost)
  {
    this.bodySubScreen.EvolveTech(this.disease, tech, devolve, cost);
    if ((UnityEngine.Object) hex != (UnityEngine.Object) null && CGameManager.IsMultiplayerGame)
    {
      this.timerHexes[tech] = hex;
      hex.StartEvolveAnim();
      ((CUpgradeSubScreen) this.diseaseUpgradeScreen).EnableEvolveButtons(false);
    }
    if (!CGameManager.IsTutorialGame)
      return;
    if (TutorialSystem.IsModuleActive("13C") && tech.name == "Livestock 1")
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (CGameManager.gameType != IGame.GameType.CureTutorial)
      return;
    if (TutorialSystem.IsModuleActive("C6") || TutorialSystem.IsModuleActive("C8"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (TutorialSystem.IsModuleActive("C18") || TutorialSystem.IsModuleActive("C24") || TutorialSystem.IsModuleActive("C25") || TutorialSystem.IsModuleActive("C26") || TutorialSystem.IsModuleActive("C28"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (TutorialSystem.IsModuleActive("C37"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (!TutorialSystem.IsModuleActive("C54"))
      return;
    PIETutorialSystem instance1 = (PIETutorialSystem) TutorialSystem.Instance;
    instance1.StartCoroutine(instance1.UpdateTutorial());
  }

  public void EndEvolve(Technology tech)
  {
    if (this.timerHexes.ContainsKey(tech))
    {
      Debug.Log((object) "Try End Called");
      this.timerHexes[tech].StopEvolveAnim(new Action<Technology>(this.OnEvolveAnimationEnded));
    }
    else
      Debug.Log((object) "Try End Failed");
    if ((UnityEngine.Object) this.GetSubScreen("Transmission_SubScreen") != (UnityEngine.Object) null)
      this.GetSubScreen("Transmission_SubScreen").Refresh();
    if ((UnityEngine.Object) this.GetSubScreen("Symptoms_SubScreen") != (UnityEngine.Object) null)
      this.GetSubScreen("Symptoms_SubScreen").Refresh();
    if (!((UnityEngine.Object) this.GetSubScreen("Abilities_SubScreen") != (UnityEngine.Object) null))
      return;
    this.GetSubScreen("Abilities_SubScreen").Refresh();
  }

  private void OnEvolveAnimationEnded(Technology tech)
  {
    this.timerHexes.Remove(tech);
    ((CUpgradeSubScreen) this.diseaseUpgradeScreen).EnableEvolveButtons(true);
  }

  private IEnumerator Execute12ATutorial()
  {
    yield return (object) new WaitForEndOfFrame();
    TutorialSystem.CheckModule((Func<bool>) (() => true), "12A", true);
  }

  public void OnTutorialBegin(Module withModule)
  {
    string name = withModule.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 1094194680:
        if (!(name == "12B"))
          break;
        this.diseaseToggles[1].enabled = true;
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        break;
      case 1144527537:
        if (!(name == "12A"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[1].enabled = false;
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = false;
        TutorialSystem.CureTutorialFreeTechSelection = false;
        break;
      case 1630092750:
        if (!(name == "14B"))
          break;
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        break;
      case 1748668916:
        if (!(name == "13A"))
          break;
        this.diseaseToggles[0].enabled = false;
        break;
      case 2329555634:
        if (!(name == "C4"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.ChangeTab1(CActionManager.ActionType.START);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = false;
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = true;
        break;
      case 2507095670:
        if (!(name == "C22"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.ChangeTab1(CActionManager.ActionType.START);
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = false;
        break;
      case 2624686098:
        if (!(name == "C35"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[3].enabled = true;
        this.ChangeTab1(CActionManager.ActionType.START);
        this.diseaseToggles[1].enabled = false;
        this.diseaseToggles[2].enabled = false;
        break;
      case 2676004693:
        if (!(name == "C52"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = true;
        this.ChangeTab1(CActionManager.ActionType.START);
        this.diseaseToggles[1].enabled = false;
        break;
      case 2843089335:
        if (!(name == "C14"))
          break;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        this.ChangeTab1(CActionManager.ActionType.START);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = false;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = false;
        break;
    }
  }

  public void OnTutorialComplete(Module completedModule)
  {
    string name = completedModule.Name;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
    {
      case 360546176:
        if (!(name == "C42"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 394101414:
        if (!(name == "C40"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C41");
        break;
      case 410879033:
        if (!(name == "C41"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C42");
        break;
      case 1630092750:
        if (!(name == "14B"))
          break;
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
        CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        CActionManager.instance.AddListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
        CActionManager.instance.AddListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        break;
      case 2245667539:
        if (!(name == "C3"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C4");
        break;
      case 2296000396:
        if (!(name == "C6"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C7");
        break;
      case 2312778015:
        if (!(name == "C7"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C8");
        break;
      case 2329555634:
        if (!(name == "C4"))
          break;
        this.diseaseToggles[0].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C5");
        break;
      case 2346333253:
        if (!(name == "C5"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C6");
        break;
      case 2396666110:
        if (!(name == "C8"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C9");
        break;
      case 2413443729:
        if (!(name == "C9"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 2490318051:
        if (!(name == "C21"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C22");
        break;
      case 2507095670:
        if (!(name == "C22"))
          break;
        this.diseaseToggles[0].enabled = false;
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C23");
        break;
      case 2523873289:
        if (!(name == "C23"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C24");
        break;
      case 2540650908:
        if (!(name == "C24"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C25");
        break;
      case 2557428527:
        if (!(name == "C25"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C26");
        break;
      case 2558561360:
        if (!(name == "C55"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 2574206146:
        if (!(name == "C26"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C27");
        break;
      case 2575338979:
        if (!(name == "C54"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C55");
        break;
      case 2590983765:
        if (!(name == "C27"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C28");
        break;
      case 2591130860:
        if (!(name == "C37"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        this.diseaseToggles[1].enabled = true;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C38");
        break;
      case 2607761384:
        if (!(name == "C28"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C29");
        break;
      case 2607908479:
        if (!(name == "C36"))
          break;
        this.diseaseToggles[0].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C37");
        break;
      case 2624539003:
        if (!(name == "C29"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 2624686098:
        if (!(name == "C35"))
          break;
        this.diseaseToggles[0].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C36");
        break;
      case 2624980288:
        if (!(name == "C19"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = true;
        this.diseaseToggles[1].enabled = true;
        this.diseaseToggles[2].enabled = true;
        this.diseaseToggles[3].enabled = true;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 2625671836:
        if (!(name == "C51"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C52");
        break;
      case 2641463717:
        if (!(name == "C34"))
          break;
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C35");
        break;
      case 2641757907:
        if (!(name == "C18"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C19");
        break;
      case 2642449455:
        if (!(name == "C50"))
          break;
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
      case 2659227074:
        if (!(name == "C53"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C54");
        break;
      case 2676004693:
        if (!(name == "C52"))
          break;
        this.diseaseToggles[0].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C53");
        break;
      case 2691796574:
        if (!(name == "C39"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C40");
        break;
      case 2708574193:
        if (!(name == "C38"))
          break;
        CActionManager.instance.RemoveListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        this.diseaseToggles[0].enabled = false;
        this.diseaseToggles[2].enabled = false;
        this.diseaseToggles[3].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C39");
        break;
      case 2792756478:
        if (!(name == "C13"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C14");
        break;
      case 2826311716:
        if (!(name == "C15"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C16");
        break;
      case 2843089335:
        if (!(name == "C14"))
          break;
        this.diseaseToggles[0].enabled = false;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C15");
        break;
      case 2859866954:
        if (!(name == "C17"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C18");
        break;
      case 2876644573:
        if (!(name == "C16"))
          break;
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C17");
        break;
    }
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    this.diseaseToggles[0].enabled = true;
    this.diseaseToggles[1].enabled = true;
    this.diseaseToggles[2].enabled = true;
    this.diseaseToggles[3].enabled = true;
    CActionManager.instance.AddListener("INPUT_TAB_1", new Action<CActionManager.ActionType>(this.ChangeTab1), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
    CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToHUD), this.gameObject);
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.AddListener("INPUT_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject);
    CActionManager.instance.AddListener("INPUT_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject);
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  public bool CheckCureTutorialRestrictions()
  {
    string activeModuleName = TutorialSystem.GetActiveModuleName();
    string lastCompletedModule = TutorialSystem.GetLastCompletedModule();
    switch (activeModuleName)
    {
      case "C3":
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        return true;
      case "C13":
        CActionManager.instance.AddListener("INPUT_TAB_3", new Action<CActionManager.ActionType>(this.ChangeTab3), this.gameObject);
        return true;
      case "C21":
        CActionManager.instance.AddListener("INPUT_TAB_2", new Action<CActionManager.ActionType>(this.ChangeTab2), this.gameObject);
        return true;
      case "C34":
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        return true;
      case "C51":
        CActionManager.instance.AddListener("INPUT_TAB_4", new Action<CActionManager.ActionType>(this.ChangeTab4), this.gameObject);
        return true;
      default:
        switch (lastCompletedModule)
        {
          case "C9":
            return true;
          case "C19":
            return true;
          case "C29":
            return true;
          case "C42":
            TutorialSystem.CheckModule((Func<bool>) (() => true), "C43");
            return true;
          case "C50":
            return true;
          case "C55":
            TutorialSystem.CheckModule((Func<bool>) (() => true), "C56");
            return true;
          default:
            return false;
        }
    }
  }
}
