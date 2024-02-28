// Decompiled with JetBrains decompiler
// Type: CUpgradeSubScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CUpgradeSubScreen : IGameSubScreen, ITutorial
{
  public UIImageButton evolve;
  public UIImageButton devolve;
  public UILabel costTitle;
  public UILabel costLabel;
  public UILabel infoLabel;
  public UILabel nameLabel;
  public UITexture evolveCustom;
  public UISprite evolveSprite;
  public UISprite overlaySprite;
  public UITexture overlayCustom;
  public UISprite numberSprite;
  public UISprite backgroundSprite;
  public List<TooltipDiseaseEvolveButton> EvolveTooltips = new List<TooltipDiseaseEvolveButton>();
  public GameObject hexParent;
  public float newAlpha = 0.5f;
  [Header("Cure")]
  public UILabel EvolveButtonText;
  public UILabel DevolveButtonText;
  private CDiseaseScreen diseaseScreen;
  private Disease disease;
  private Technology tech;
  private TechHex hex;
  private float techClickTime = -1f;
  private bool enableButtons = true;

  private CConfirmOverlay mpConfirm
  {
    get
    {
      return CGameManager.IsCureGame ? CUIManager.instance.redSmallConfirmOverlayCure : CUIManager.instance.redSmallConfirmOverlay;
    }
  }

  public override void Initialise()
  {
    base.Initialise();
    this.diseaseScreen = this.gameScreen as CDiseaseScreen;
    UIEventListener.Get(this.evolve.gameObject).onClick += new UIEventListener.VoidDelegate(this.OnClickEvolve);
    UIEventListener.Get(this.devolve.gameObject).onClick += new UIEventListener.VoidDelegate(this.OnClickDeEvolve);
    CActionManager.instance.AddListener("SC_INPUT_EVOLVE", new Action<CActionManager.ActionType>(this.EvolveAction), this.gameObject);
    CActionManager.instance.AddListener("SC_INPUT_DEVOLVE", new Action<CActionManager.ActionType>(this.DevolveAction), this.gameObject);
    CActionManager.instance.AddListener("INPUT_CONTINUE", new Action<CActionManager.ActionType>(this.OnAccept), this.gameObject);
    TutorialSystem.RegisterInterface((ITutorial) this);
  }

  public void Reset() => this.tech = (Technology) null;

  public void SetUpgrade(TechHex h, Technology t, Disease d)
  {
    if (t == this.tech && (double) Time.time - (double) this.techClickTime < 0.30000001192092896)
    {
      if (!CUpgradeSubScreen.CanFundTech(t))
        return;
      this.techClickTime = -1f;
      this.OnAccept(CActionManager.ActionType.START);
      this.Refresh();
    }
    else
    {
      if (CUpgradeSubScreen.CanSelectTech(t) && t != this.tech)
      {
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
      }
      this.techClickTime = Time.time;
      this.hex = h;
      this.tech = t;
      this.disease = d;
      this.Refresh();
    }
  }

  public override void Refresh()
  {
    base.Refresh();
    if (this.tech == null)
    {
      this.evolve.gameObject.SetActive(false);
      this.devolve.gameObject.SetActive(false);
      this.costLabel.text = "";
      this.infoLabel.text = "";
      this.nameLabel.text = "";
      this.hexParent.SetActive(false);
    }
    else
    {
      if (CGameManager.IsCureGame)
      {
        if (this.tech.id.Contains("_Force_Lockdown") || this.tech.id.Contains("_Restrict_Ground_Connections") || this.tech.id.Contains("_Restrict_Sea_Connections") || this.tech.id.Contains("_Restrict_Air_Connections"))
        {
          this.EvolveButtonText.text = CLocalisationManager.GetText("Enable");
          this.DevolveButtonText.text = CLocalisationManager.GetText("Disable");
        }
        else
        {
          this.EvolveButtonText.text = CLocalisationManager.GetText("IG_Cure_Fund");
          this.DevolveButtonText.text = CLocalisationManager.GetText("FE_Options_Screen_Resolution_Revert");
        }
      }
      foreach (TooltipDiseaseEvolveButton evolveTooltip in this.EvolveTooltips)
        evolveTooltip.Technology = this.tech;
      bool isResearched = this.disease.IsTechEvolved(this.tech);
      this.backgroundSprite.spriteName = CGameManager.GetTechBackground(this.tech.techHexType, isResearched);
      int num;
      if (isResearched)
      {
        this.evolve.gameObject.SetActive(false);
        num = this.disease.GetDeEvolveCost(this.tech);
        if (this.tech.cantDevolve || (double) this.tech.devolveCostMultipler == -1.0)
        {
          this.devolve.gameObject.SetActive(false);
          this.costTitle.gameObject.SetActive(false);
          this.costLabel.gameObject.SetActive(true);
        }
        else
        {
          this.devolve.gameObject.SetActive(true);
          this.costTitle.gameObject.SetActive(true);
          this.costLabel.gameObject.SetActive(true);
          if (this.disease.evoPoints >= this.disease.GetDeEvolveCost(this.tech) && this.enableButtons)
          {
            this.devolve.isEnabled = true;
            this.devolve.transform.Find("Background/Icon_DNA").gameObject.GetComponent<UISprite>().alpha = 1f;
            this.devolve.transform.Find("Background/Text_Evolve").gameObject.GetComponent<UILabel>().alpha = 1f;
          }
          else
          {
            this.devolve.isEnabled = false;
            this.devolve.transform.Find("Background/Icon_DNA").gameObject.GetComponent<UISprite>().alpha = this.newAlpha;
            this.devolve.transform.Find("Background/Text_Evolve").gameObject.GetComponent<UILabel>().alpha = this.newAlpha;
          }
        }
      }
      else
      {
        this.devolve.gameObject.SetActive(false);
        this.evolve.gameObject.SetActive(true);
        this.costTitle.gameObject.SetActive(true);
        this.costLabel.gameObject.SetActive(true);
        num = this.disease.GetEvolveCost(this.tech);
        if (this.tech.padlocked)
        {
          this.evolve.gameObject.SetActive(false);
          this.costTitle.gameObject.SetActive(false);
          this.costLabel.gameObject.SetActive(true);
        }
        else if (this.disease.evoPoints >= num && this.enableButtons)
        {
          this.evolve.isEnabled = true;
          this.evolve.transform.Find("Background/Icon_DNA").gameObject.GetComponent<UISprite>().alpha = 1f;
          this.evolve.transform.Find("Background/Text_Evolve").gameObject.GetComponent<UILabel>().alpha = 1f;
        }
        else
        {
          this.evolve.isEnabled = false;
          this.evolve.transform.Find("Background/Icon_DNA").gameObject.GetComponent<UISprite>().alpha = this.newAlpha;
          this.evolve.transform.Find("Background/Text_Evolve").gameObject.GetComponent<UILabel>().alpha = this.newAlpha;
        }
      }
      this.nameLabel.text = CLocalisationManager.GetText(this.tech.name).ToUpper();
      this.infoLabel.text = "   " + CLocalisationManager.GetText(this.tech.description);
      Technology tech = this.tech;
      string str1 = "";
      if ((double) Mathf.Abs(tech.changeToInfectiousness) > 0.0)
        str1 = CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.Contains("Reconstruction") ? str1 + "Infectivity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToInfectiousness) + "; " : str1 + "Support Level " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToInfectiousness) + "; ";
      if ((double) Mathf.Abs(tech.changeToSeverity) > 0.0)
        str1 = CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.Contains("Reconstruction") ? str1 + "Severity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToSeverity) + "; " : str1 + "Economic Effect " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToSeverity) + "; ";
      if ((double) Mathf.Abs(tech.changeToLethality) > 0.0)
        str1 = CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.Contains("Reconstruction") ? str1 + "Lethality " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToLethality) + "; " : str1 + "Antipathy " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToLethality) + "; ";
      if ((double) Mathf.Abs(tech.changeToWealthy) > 0.0)
        str1 = str1 + "Wealthy " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToWealthy) + "; ";
      if ((double) Mathf.Abs(tech.changeToPoverty) > 0.0)
        str1 = str1 + "Poverty " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToPoverty) + "; ";
      if ((double) Mathf.Abs(tech.changeToHot) > 0.0)
        str1 = str1 + "Hot " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToHot) + "; ";
      if ((double) Mathf.Abs(tech.changeToCold) > 0.0)
        str1 = str1 + "Cold " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToCold) + "; ";
      if ((double) Mathf.Abs(tech.changeToUrban) > 0.0)
        str1 = str1 + "Urban " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToUrban) + "; ";
      if ((double) Mathf.Abs(tech.changeToRural) > 0.0)
        str1 = str1 + "Rural " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToRural) + "; ";
      if ((double) Mathf.Abs(tech.changeToHumid) > 0.0)
        str1 = str1 + "Humid " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToHumid) + "; ";
      if ((double) Mathf.Abs(tech.changeToArid) > 0.0)
        str1 = str1 + "Arid " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToArid) + "; ";
      if ((double) Mathf.Abs(tech.changeToLandTransmission) > 0.0)
        str1 = str1 + "Land " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToLandTransmission) + "; ";
      if ((double) Mathf.Abs(tech.changeToSeaTransmission) > 0.0)
        str1 = str1 + "Sea " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToSeaTransmission) + "; ";
      if ((double) Mathf.Abs(tech.changeToAirTransmission) > 0.0)
        str1 = str1 + "Air " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToAirTransmission) + "; ";
      if ((double) Mathf.Abs(tech.changeToMutation) > 0.0)
        str1 = !CGameManager.IsGiantScenario() ? (!CGameManager.IsFederalScenario("世界狂潮") ? str1 + "Mutation " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToMutation) + "; " : str1 + "Corruption Risk " + CDiseaseControlSubScreen.GetUnifiedNum(-100f * tech.changeToMutation) + "%; ") : str1 + "HP " + CDiseaseControlSubScreen.GetUnifiedNum(-1f * tech.changeToMutation) + "; ";
      if ((double) Mathf.Abs(tech.changeToCureBaseMultiplier) > 0.0)
        str1 = !CGameManager.IsGiantScenario() ? str1 + "Cure Requirement " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToCureBaseMultiplier) + "; " : str1 + "Action Point " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToCureBaseMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToResearchInefficiencyMultiplier) > 0.0)
        str1 = str1 + "Hard to Cure " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToResearchInefficiencyMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToCorpseTransmission) > 0.0)
        str1 = str1 + "Corpse Transimission " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToCorpseTransmission) + "; ";
      if ((double) Mathf.Abs(tech.changeToNonControlInfectAttack) > 0.0)
        str1 = str1 + "Infect Buff " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToNonControlInfectAttack) + "; ";
      if ((double) Mathf.Abs(tech.changeToNonControlLethalAttack) > 0.0)
        str1 = str1 + "Lethal Buff " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToNonControlLethalAttack) + "; ";
      if ((double) Mathf.Abs(tech.changeToControlInfectDefence) > 0.0)
        str1 = str1 + "Infect Debuff " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToControlInfectDefence) + "; ";
      if ((double) Mathf.Abs(tech.changeToControlLethalDefence) > 0.0)
        str1 = str1 + "Lethal Debuff " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToControlLethalDefence) + "; ";
      if ((double) Mathf.Abs(tech.changeToCorpseIncomeMultiplier) > 0.0)
        str1 = str1 + "Corpse DNA " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToCorpseIncomeMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToOtherInfectiousness) > 0.0)
        str1 = str1 + "Partner Infectivity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToOtherInfectiousness) + "; ";
      if ((double) Mathf.Abs(tech.changeToOtherSeverity) > 0.0)
        str1 = str1 + "Partner Severity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToOtherSeverity) + "; ";
      if ((double) Mathf.Abs(tech.changeToOtherLethality) > 0.0)
        str1 = str1 + "Partner Lethality " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToOtherLethality) + "; ";
      if ((double) Mathf.Abs(tech.changeToOtherCureBaseMultiplier) > 0.0)
        str1 = str1 + "Partner Cure Requirement " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToOtherCureBaseMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToOtherResearchInefficiencyMultiplier) > 0.0)
        str1 = str1 + "Partner Hard to Cure " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToOtherResearchInefficiencyMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeXSpeciesInfectiousness) > 0.0)
        str1 = str1 + "Cross Species Infectivity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeXSpeciesInfectiousness) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeInfectiousness) > 0.0)
        str1 = str1 + "Ape Infectivity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeInfectiousness) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeLethality) > 0.0)
        str1 = str1 + "Ape Lethality " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeLethality) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeRescueAbility) > 0.0)
        str1 = str1 + "Ape Rescue " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeRescueAbility) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeStrength) > 0.0)
        str1 = str1 + "Ape Strength " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeStrength) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeIntelligence) > 0.0)
        str1 = str1 + "Ape Intelligence " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeIntelligence) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeSpeed) > 0.0)
        str1 = str1 + "Ape Speed " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeSpeed) + "; ";
      if ((double) Mathf.Abs(tech.changeToApeSurvival) > 0.0)
        str1 = str1 + "Ape Survival " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToApeSurvival) + "; ";
      if ((double) Mathf.Abs(tech.changeToHumanImmunity) > 0.0)
        str1 = str1 + "Human Immunity " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToHumanImmunity) + "; ";
      if ((double) Mathf.Abs(tech.changeToSporeCounter) > 0.0)
        str1 = !CGameManager.IsGiantScenario() ? str1 + "Spore Count " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToSporeCounter) + "; " : str1 + "Attack " + CDiseaseControlSubScreen.GetUnifiedNum(-1f * tech.changeToSporeCounter) + "; ";
      if ((double) Mathf.Abs(tech.changeToWormPlaneChance) > 0.0)
        str1 = str1 + "Trojan Plane Chance " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToWormPlaneChance) + "; ";
      if ((double) Mathf.Abs(tech.changeToZCombatStrength) > 0.0)
        str1 = str1 + "Zombie Strength " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToZCombatStrength) + "; ";
      if ((double) Mathf.Abs(tech.changeToZombieConversionMod) > 0.0)
        str1 = str1 + "Zombie Conversion " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToZombieConversionMod) + "; ";
      if ((double) Mathf.Abs(tech.changeToLocalZCombatStrengthMod) > 0.0)
        str1 = str1 + "Zombie Strenth Modification " + CDiseaseControlSubScreen.GetUnifiedNum(tech.changeToLocalZCombatStrengthMod) + "; ";
      if ((double) Mathf.Abs(tech.reanimateSizeMultiplier) > 0.0)
        str1 = str1 + "Reanimate Size " + CDiseaseControlSubScreen.GetUnifiedNum(tech.reanimateSizeMultiplier) + "; ";
      if ((double) Mathf.Abs(tech.changeToZday) > 0.0)
        str1 += "Enable Zombie Spawn; ";
      if ((double) Mathf.Abs(tech.changeToNucleicAcidFlag) > 0.0)
        str1 += "Trigger Nucleic Acid Neutralisation; ";
      this.infoLabel.text = this.infoLabel.text + "\n" + str1;
      if (isResearched && (this.tech.cantDevolve || (double) this.tech.devolveCostMultipler == -1.0))
        this.costLabel.text = !CGameManager.IsCureGame ? CLocalisationManager.GetText("IG_MouseOver_Help_TechHex_Evolved_Final").Replace("%name", "") : CLocalisationManager.GetText("IG_Cure_Can't_Revert");
      else if (num < 0)
      {
        this.costTitle.text = CLocalisationManager.GetText("IG_Refund");
        this.costLabel.text = !CGameManager.IsCureGame ? CLocalisationManager.GetText("IG_DNA_Points").Replace("%DNA", Mathf.Abs(num).ToString()) : CLocalisationManager.GetText("IG_Cure_Resources_Cost").Replace("%cost", Mathf.Abs(num).ToString());
      }
      else
      {
        this.costTitle.text = CLocalisationManager.GetText("IG_Cost");
        this.costLabel.text = !CGameManager.IsCureGame ? CLocalisationManager.GetText("IG_DNA_Points").Replace("%DNA", Mathf.Abs(num).ToString()) : CLocalisationManager.GetText("IG_Cure_Resources_Cost").Replace("%cost", Mathf.Abs(num).ToString());
      }
      if (this.tech.padlocked)
        this.costLabel.text = CLocalisationManager.GetText("Locked");
      this.hexParent.SetActive(true);
      string str2 = isResearched ? "_0" : "";
      if ((UnityEngine.Object) this.tech.customGraphic != (UnityEngine.Object) null)
      {
        NGUITools.SetActive(this.evolveSprite.gameObject, false);
        NGUITools.SetActive(this.evolveCustom.gameObject, true);
        this.evolveCustom.material.SetFloat("_MaskInvert", isResearched ? 1f : 0.0f);
        this.evolveCustom.mainTexture = (Texture) this.tech.customGraphic;
      }
      else
      {
        NGUITools.SetActive(this.evolveSprite.gameObject, true);
        NGUITools.SetActive(this.evolveCustom.gameObject, false);
        this.evolveSprite.spriteName = this.tech.baseGraphic + str2;
      }
      string number = this.tech.GetNumber();
      NGUITools.SetActive(this.numberSprite.gameObject, number != null);
      if (number != null)
        this.numberSprite.spriteName = number;
      if ((UnityEngine.Object) this.tech.customOverlay != (UnityEngine.Object) null)
      {
        NGUITools.SetActive(this.overlaySprite.gameObject, false);
        NGUITools.SetActive(this.overlayCustom.gameObject, true);
        this.overlayCustom.mainTexture = (Texture) this.tech.customOverlay;
      }
      else
      {
        string overlay = this.tech.GetOverlay();
        NGUITools.SetActive(this.overlaySprite.gameObject, overlay != null);
        NGUITools.SetActive(this.overlayCustom.gameObject, false);
        if (overlay == null)
          return;
        this.overlaySprite.spriteName = overlay + str2;
      }
    }
  }

  private void OnAccept(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START || !this.enableButtons)
      return;
    if (this.evolve.gameObject.activeSelf && this.evolve.isEnabled)
    {
      this.OnClickEvolve((GameObject) null);
    }
    else
    {
      if (!this.devolve.gameObject.activeSelf || !this.devolve.isEnabled)
        return;
      this.OnClickDeEvolve((GameObject) null);
    }
  }

  private void EvolveAction(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START || !this.evolve.isEnabled)
      return;
    this.OnClickEvolve((GameObject) null);
  }

  private void OnClickEvolve(GameObject clicked)
  {
    if (World.instance.gameEnded || this.tech == null || !CUpgradeSubScreen.CanFundTech(this.tech))
      return;
    this.diseaseScreen.EvolveDisease(this.hex, this.tech, false, CGameManager.localPlayerInfo.disease.evoPointsSpent);
    CGameManager.game.EvolveTech(this.tech);
  }

  private void OnClickDeEvolve(GameObject clicked)
  {
    if (CGameManager.IsTutorialGame)
    {
      if (CGameManager.gameType == IGame.GameType.Tutorial && TutorialSystem.IsModuleSectionActive("Evolving a Tech"))
        return;
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        if (!CUpgradeSubScreen.CanDisableTech(this.tech))
          return;
        if (TutorialSystem.IsModuleActive("C40"))
        {
          PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
          instance.StartCoroutine(instance.UpdateTutorial());
        }
      }
    }
    if (this.tech == null)
      return;
    if (CGameManager.IsCureGame)
      this.mpConfirm.ShowLocalised("Disable", this.disease.GetDeEvolveCost(this.tech) <= 0 ? "Do you want to remove this initiative?\nYou will get %d Resources refunded." : "Do you want to remove this initiative?\nIt will cost %d Resources.", "No", "Yes", new CConfirmOverlay.PressDelegate(this.cancelDevolve), new CConfirmOverlay.PressDelegate(this.acceptDevolve), variables: new Dictionary<string, string>()
      {
        {
          "d",
          Mathf.Abs(this.disease.GetDeEvolveCost(this.tech)).ToString()
        }
      }, isEscapeable: !TutorialSystem.CureTutorialBlockCancelTechDisable, blockButtonA: TutorialSystem.CureTutorialBlockCancelTechDisable);
    else
      this.mpConfirm.ShowLocalised("IG_Disease_Devolve_Confirm_Title", "IG_Disease_Devolve_Confirm_Text", "No", "Yes", new CConfirmOverlay.PressDelegate(this.cancelDevolve), new CConfirmOverlay.PressDelegate(this.acceptDevolve));
    this.diseaseScreen.PreviewTechDevolve(this.tech);
  }

  private void cancelDevolve() => this.diseaseScreen.PreviewTechDevolve((Technology) null);

  private void DevolveAction(CActionManager.ActionType actionType)
  {
    if (actionType != CActionManager.ActionType.START || !this.devolve.isEnabled)
      return;
    this.acceptDevolve();
  }

  private void acceptDevolve()
  {
    if (TutorialSystem.IsModuleActive("C41"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if (this.tech != null)
    {
      this.diseaseScreen.EvolveDisease(this.hex, this.tech, true, CGameManager.localPlayerInfo.disease.evoPointsSpent);
      CGameManager.game.DeEvolveTech(this.tech);
    }
    this.diseaseScreen.PreviewTechDevolve((Technology) null);
  }

  public void EnableEvolveButtons(bool b)
  {
    this.enableButtons = b;
    if (!b)
    {
      this.evolve.isEnabled = b;
      this.devolve.isEnabled = b;
    }
    this.Refresh();
  }

  public static bool CanSelectTech(Technology tech)
  {
    bool CanBeSelected;
    CUpgradeSubScreen.CheckCureTutorialTechRestrictions(tech, out CanBeSelected, out bool _, out bool _);
    return CanBeSelected;
  }

  public static bool CanFundTech(Technology tech)
  {
    bool CanBeFunded;
    CUpgradeSubScreen.CheckCureTutorialTechRestrictions(tech, out bool _, out CanBeFunded, out bool _);
    return CanBeFunded;
  }

  public static bool CanDisableTech(Technology tech)
  {
    bool CanbeDisabled;
    CUpgradeSubScreen.CheckCureTutorialTechRestrictions(tech, out bool _, out bool _, out CanbeDisabled);
    return CanbeDisabled;
  }

  private static void CheckCureTutorialTechRestrictions(
    Technology tech,
    out bool CanBeSelected,
    out bool CanBeFunded,
    out bool CanbeDisabled)
  {
    string activeModuleName = TutorialSystem.GetActiveModuleName();
    if (!CGameManager.IsCureTutorialGame)
    {
      CanBeSelected = CanBeFunded = CanbeDisabled = true;
    }
    else
    {
      CanBeSelected = TutorialSystem.CureTutorialFreeTechSelection || TutorialSystem.CureTutorialDisableTechRestrictions;
      CanBeFunded = CanbeDisabled = TutorialSystem.CureTutorialDisableTechRestrictions;
      if (tech == null)
        return;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(activeModuleName))
      {
        case 394101414:
          if (!(activeModuleName == "C40") || !(tech.name == "Force Lockdown"))
            break;
          CanBeSelected = false;
          CanBeFunded = false;
          CanbeDisabled = true;
          break;
        case 2296000396:
          if (!(activeModuleName == "C6") || !(tech.name == "Investigate Outbreaks"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2312778015:
          if (!(activeModuleName == "C7") || !(tech.name == "Deploy Field Operatives"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2346333253:
          if (!(activeModuleName == "C5") || !(tech.name == "Investigate Outbreaks"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2396666110:
          if (!(activeModuleName == "C8") || !(tech.name == "Deploy Field Operatives"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2413443729:
          if (!(activeModuleName == "C9"))
            break;
          CanBeSelected = TutorialSystem.CureTutorialFreeTechSelection = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2473540432:
          if (!(activeModuleName == "C20"))
            break;
          TutorialSystem.CureTutorialFreeTechSelection = false;
          break;
        case 2523873289:
          if (!(activeModuleName == "C23") || !(tech.name == "South America Alert"))
            break;
          TutorialSystem.CureTutorialFreeTechSelection = false;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2540650908:
          if (!(activeModuleName == "C24") || !(tech.name == "South America Alert"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2557428527:
          if (!(activeModuleName == "C25") || !(tech.name == "Force Lockdown"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2558561360:
          if (!(activeModuleName == "C55"))
            break;
          CanBeSelected = TutorialSystem.CureTutorialFreeTechSelection = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2574206146:
          if (!(activeModuleName == "C26") || !(tech.name == "Force Lockdown"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2574353241:
          if (!(activeModuleName == "C30"))
            break;
          TutorialSystem.CureTutorialFreeTechSelection = false;
          break;
        case 2575338979:
          if (!(activeModuleName == "C54") || !(tech.name == "Vaccine Research"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2590983765:
          if (!(activeModuleName == "C27") || !(tech.name == "Close Land Borders"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2591130860:
          if (!(activeModuleName == "C37") || !(tech.name == "Furlough Schemes"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2607761384:
          if (!(activeModuleName == "C28") || !(tech.name == "Close Land Borders"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2607908479:
          if (!(activeModuleName == "C36") || !(tech.name == "Furlough Schemes"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2608894217:
          if (!(activeModuleName == "C56"))
            break;
          TutorialSystem.CureTutorialFreeTechSelection = false;
          break;
        case 2624539003:
          if (!(activeModuleName == "C29"))
            break;
          CanBeSelected = TutorialSystem.CureTutorialFreeTechSelection = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2624980288:
          if (!(activeModuleName == "C19"))
            break;
          CanBeSelected = TutorialSystem.CureTutorialFreeTechSelection = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2641757907:
          if (!(activeModuleName == "C18") || !(tech.name == "Hand Washing"))
            break;
          CanBeSelected = true;
          CanBeFunded = true;
          CanbeDisabled = false;
          break;
        case 2659227074:
          if (!(activeModuleName == "C53") || !(tech.name == "Vaccine Research"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2691796574:
          if (!(activeModuleName == "C39") || !(tech.name == "Force Lockdown"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2775978859:
          if (!(activeModuleName == "C10"))
            break;
          TutorialSystem.CureTutorialFreeTechSelection = false;
          break;
        case 2826311716:
          if (!(activeModuleName == "C15") || !(tech.name == "Contact Tracing 1"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2859866954:
          if (!(activeModuleName == "C17") || !(tech.name == "Hand Washing"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
        case 2876644573:
          if (!(activeModuleName == "C16") || !(tech.name == "Clinical Treatments"))
            break;
          CanBeSelected = true;
          CanBeFunded = false;
          CanbeDisabled = false;
          break;
      }
    }
  }

  public void OnTutorialBegin(Module withModule)
  {
  }

  public void OnTutorialComplete(Module completedModule)
  {
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }
}
