// Decompiled with JetBrains decompiler
// Type: CEndGameScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using AurochDigital.UGC;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CEndGameScreen : SinglePlayerGraphScreen, ITutorial
{
  private Disease disease;
  private CConfirmOverlay confirm;
  private bool replayStarted;
  private float countryChange;
  private int lastReplayTurn = -1;
  private float restartTime;
  public float countryChangeSpeed = 1f;
  public string nonSimianShareLink = "http://www.ndemiccreations.com/en/25-plague-inc-evolved";
  public string simianShareLink = "http://l.ndemiccreations.com/pota_social_share";
  [Header("UI Elements")]
  public UILabel replayTurnLabel;
  public UILabel time;
  public UILabel difficulty;
  public UILabel cureProgress;
  public UILabel geneticComplexity;
  public UILabel plagueType;
  public UILabel gameType;
  public UILabel totalScore;
  public UILabel scoreLabel;
  public UILabel mainTitle;
  public UILabel resultTitle;
  public UILabel resultDescription;
  public UIButton retry;
  public UIButton exit;
  public UISprite background;
  public UILabel shareMessageLabel;
  public GameObject difficultyParent;
  public GameObject zombieParent;
  public GameObject simianBottomPanel;
  public GameObject nonSimianBottomPanel;
  public UILabel healthyText;
  public UILabel infectedText;
  public UILabel deadText;
  public UILabel zombieText;
  public UILabel apeHealthyText;
  public UILabel apeInfectedText;
  public UILabel apeDeadText;
  public UIImageButton shareFacebook;
  public UIImageButton shareEmail;
  public UIImageButton shareTwitter;
  public UIImageButton restartReplay;
  public UIToggle pauseReplayTgl;
  public UIToggle playReplayTgl;
  public UIToggle fastReplayTgl;
  public UIToggle superFastReplayTgl;
  public UISprite[] scoreBiohazardMarks;
  private int[] scoreThresholds = new int[5]
  {
    40,
    2500,
    5000,
    30000,
    100000
  };
  public GameObject[] scenarioScoreBiohazardMarks;
  public GameObject scenarioBiohazardContainer;
  public GameObject biohazardContainer;
  public GameObject replayButtonContainer;
  public GameObject noReplayAvailableContainer;
  public GameObject graphParent;
  public GameObject graphButtons;
  public GameObject mapParent;
  private CConfirmOverlay rateModal;
  private bool gameWasWon;
  private string socialTxt = string.Empty;
  private Action<CActionManager.ActionType> inputSpeed1Action;
  private Action<CActionManager.ActionType> inputSpeed2Action;
  private Action<CActionManager.ActionType> inputSpeed3Action;

  public CUIManager.Unlock Unlocked { get; set; }

  public override void Initialise()
  {
    base.Initialise();
    EventDelegate.Set(this.retry.onClick, new EventDelegate.Callback(this.OnClickRetry));
    EventDelegate.Set(this.exit.onClick, new EventDelegate.Callback(this.OnClickExit));
    EventDelegate.Set(this.shareFacebook.onClick, new EventDelegate.Callback(this.OnClickShareFacebook));
    EventDelegate.Set(this.shareTwitter.onClick, new EventDelegate.Callback(this.OnClickShareTwitter));
    EventDelegate.Set(this.shareEmail.onClick, new EventDelegate.Callback(this.OnClickShareEmail));
    EventDelegate.Set(this.restartReplay.onClick, new EventDelegate.Callback(this.OnClickRestartReplay));
    this.inputSpeed1Action = (Action<CActionManager.ActionType>) (type => this.SetReplaySpeed(CEndGameScreen.ReplaySpeed.PLAY));
    this.inputSpeed2Action = (Action<CActionManager.ActionType>) (type => this.SetReplaySpeed(CEndGameScreen.ReplaySpeed.FAST));
    this.inputSpeed3Action = (Action<CActionManager.ActionType>) (type => this.SetReplaySpeed(CEndGameScreen.ReplaySpeed.SUPER_FAST));
    CActionManager.instance.AddListener("INPUT_SPEED1", this.inputSpeed1Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED2", this.inputSpeed2Action, this.gameObject);
    CActionManager.instance.AddListener("INPUT_SPEED3", this.inputSpeed3Action, this.gameObject);
    this.confirm = CUIManager.instance.redConfirmOverlay;
    this.rateModal = CUIManager.instance.rateModal;
  }

  public void RateScenario(bool positive, ScenarioInformation scenario)
  {
    new SteamUGCHandler().SetItemVote(ulong.Parse(scenario.id), positive ? 1 : -1, (SteamUGCHandler.OnItemVoteComplete) (forPublishID => Debug.Log((object) "Custom Scenario Vote successful")));
  }

  public override void SetActive(bool active)
  {
    if (active)
    {
      this.totalScore.effectStyle = UILabel.Effect.None;
      this.gameWasWon = World.instance.gameWon;
      Camera_Zoom.instance.SetCameraToDefault();
      this.disease = CNetworkManager.network.LocalPlayerInfo.disease;
      if (World.instance is MPWorld && this.disease != World.instance.winner)
        this.gameWasWon = false;
      this.replayTurnLabel.text = string.Empty;
      if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        this.nonSimianBottomPanel.SetActive(false);
        this.simianBottomPanel.SetActive(true);
        Transform transform = this.simianBottomPanel.transform.Find("Humans");
        this.healthyText = transform.Find("Healthy").Find("Text_Healthy_Num").GetComponent<UILabel>();
        this.infectedText = transform.Find("Infected").Find("Text_Infected_Num").GetComponent<UILabel>();
        this.deadText = transform.Find("Dead").Find("Text_Dead_Num").GetComponent<UILabel>();
        this.apeHealthyText.text = "0";
        this.apeInfectedText.text = "0";
        this.apeDeadText.text = "0";
      }
      else
      {
        this.nonSimianBottomPanel.SetActive(true);
        this.simianBottomPanel.SetActive(false);
        Transform transform = this.nonSimianBottomPanel.transform.Find("Humans");
        this.healthyText = transform.Find("Healthy").Find("Text_Healthy_Num").GetComponent<UILabel>();
        this.infectedText = transform.Find("Infected").Find("Text_Infected_Num").GetComponent<UILabel>();
        this.deadText = transform.Find("Dead").Find("Text_Dead_Num").GetComponent<UILabel>();
        UISprite component1 = transform.Find("Healthy").Find("Sprite").GetComponent<UISprite>();
        UISprite component2 = transform.Find("Dead").Find("Sprite").GetComponent<UISprite>();
        if (this.disease.diseaseType == Disease.EDiseaseType.FAKE_NEWS)
        {
          component1.spriteName = "questionmark";
          component2.spriteName = "Healthy";
        }
        else
        {
          component1.spriteName = "Healthy";
          component2.spriteName = "Dead";
        }
      }
      if ((UnityEngine.Object) this.healthyText == (UnityEngine.Object) null || (UnityEngine.Object) this.infectedText == (UnityEngine.Object) null || (UnityEngine.Object) this.deadText == (UnityEngine.Object) null)
        Debug.LogError((object) "Unable to update UI object references for healthy, infected, dead, prefab links might be broken or game object names are modified");
      this.zombieParent.SetActive(this.disease.diseaseType == Disease.EDiseaseType.NECROA || this.disease.diseaseType == Disease.EDiseaseType.CURE);
      this.healthyText.text = "0";
      this.infectedText.text = "0";
      this.deadText.text = "0";
      this.zombieText.text = "0";
      this.replayTurnLabel.text = CLocalisationManager.GetText("FE_EndGame_Days").Replace("%days", "0");
      if (this.gameWasWon)
      {
        this.background.spriteName = "Red_Bg_01";
        this.mainTitle.text = CLocalisationManager.GetText("FE_EndGame_MainTitle_Victory");
        if (this.disease.zdayOrDone)
        {
          this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Win_Necroa");
          this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Victory_Necroa_2").Replace("%name", this.disease.name);
        }
        else if (this.disease.daysToGameWin <= 0)
        {
          if (this.disease.diseaseType == Disease.EDiseaseType.NEURAX)
          {
            this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Win_Enslaved");
            this.mainTitle.text = CLocalisationManager.GetText("FE_EndGame_MainTitle_Victory_Enslaved");
            this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Victory_Neurax_Enslaved").Replace("%name", this.disease.name);
          }
          else if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
          {
            this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Win_Simian_Flu");
          }
          else
          {
            this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Win");
            this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Victory_Default_2").Replace("%name", this.disease.name);
          }
          Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
          if (currentLoadedScenario != null && !string.IsNullOrEmpty(currentLoadedScenario.scenarioInformation.scenEndGameTagline) && currentLoadedScenario.scenarioInformation.scenEndGameTagline != "0")
            this.resultDescription.text = CLocalisationManager.GetText(currentLoadedScenario.scenarioInformation.scenEndGameTagline).Replace("%@", this.disease.name);
          if (currentLoadedScenario != null && !string.IsNullOrEmpty(currentLoadedScenario.scenarioInformation.scenEndGameScreenTitlePc) && currentLoadedScenario.scenarioInformation.scenEndGameScreenTitlePc != "0")
            this.mainTitle.text = CLocalisationManager.GetText(currentLoadedScenario.scenarioInformation.scenEndGameScreenTitlePc).Replace("%@", this.disease.name);
        }
        else
        {
          this.socialTxt = this.disease.diseaseType != Disease.EDiseaseType.SIMIAN_FLU ? CLocalisationManager.GetText("Social_End_Game_Win") : CLocalisationManager.GetText("Social_End_Game_Win_Simian_Flu");
          this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Victory_Default_2").Replace("%name", this.disease.name);
        }
        this.resultTitle.text = CLocalisationManager.GetText("FE_EndGame_Title_Victory");
        if (CGameManager.IsFederalScenario("PISMG"))
          this.resultTitle.text = ((SPDisease) this.disease).maxMusicCombo != ((SPDisease) this.disease).totalMusicBubbleCount ? "TRACK COMPLETE" : "FULL COMBO";
        if (CGameManager.IsFederalScenario("世界狂潮"))
          this.resultTitle.text = (double) this.disease.globalCureResearch < (double) this.disease.cureRequirements ? "病原体摧毁世界" : "您成功拯救世界";
      }
      else
      {
        this.background.spriteName = "Blue_Bg_01";
        this.mainTitle.text = CLocalisationManager.GetText("FE_EndGame_MainTitle_Defeat");
        this.resultTitle.text = CLocalisationManager.GetText("FE_EndGame_Title_Defeat");
        if (this.disease.zdayOrDone)
        {
          this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Lose_Necroa");
          this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Defeat_Necroa_5").Replace("%name", this.disease.name);
        }
        else if (!this.disease.cureFlag)
        {
          this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Lose");
          this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Defeat_Neurax").Replace("%name", this.disease.name);
        }
        else
        {
          this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Lose_Cure");
          this.resultDescription.text = CLocalisationManager.GetText("FE_EndGame_Defeat").Replace("%name", this.disease.name);
        }
        if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
          this.socialTxt = CLocalisationManager.GetText("Social_End_Game_Loose_Simian_Flu");
        if (CGameManager.IsFederalScenario("PISMG"))
          this.resultTitle.text = "TRACK LOST";
      }
      bool flag = false;
      int num1;
      if (CGameManager.gameType == IGame.GameType.Official || CGameManager.gameType == IGame.GameType.Custom)
      {
        Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
        if (currentLoadedScenario == null)
        {
          Debug.LogError((object) "ERROR: MISSING SCENARIO!");
          this.shareMessageLabel.text = this.socialTxt.Replace("%name", this.disease.name).Replace("%plague", CGameManager.GetDiseaseName(this.disease.diseaseType)).Replace("%days", this.disease.turnNumber.ToString());
        }
        else
        {
          if (currentLoadedScenario.filename.Contains("PIFSL"))
            flag = true;
          this.socialTxt = !World.instance.gameWon ? CLocalisationManager.GetText("My %@, called %@ just lost the %@ scenario in Plague Inc.") : CLocalisationManager.GetText("My %@, called %@ just won the %@ scenario in Plague Inc.");
          this.socialTxt = CUtils.ReplaceTokens(this.socialTxt, "%@", new string[3]
          {
            CGameManager.GetDiseaseNameLoc(this.disease.diseaseType),
            this.disease.name,
            CLocalisationManager.GetText(currentLoadedScenario.scenarioInformation.scenTitle)
          });
          this.shareMessageLabel.text = this.socialTxt;
        }
      }
      else
      {
        UILabel shareMessageLabel = this.shareMessageLabel;
        string str1 = this.socialTxt.Replace("%name", this.disease.name).Replace("%plague", CGameManager.GetDiseaseName(this.disease.diseaseType));
        num1 = this.disease.turnNumber;
        string newValue = num1.ToString();
        string str2 = str1.Replace("%days", newValue);
        shareMessageLabel.text = str2;
      }
      if (this.disease != null)
      {
        UILabel time = this.time;
        string text = CLocalisationManager.GetText("FE_EndGame_Days");
        num1 = this.disease.turnNumber;
        string newValue = num1.ToString();
        string str3 = text.Replace("%days", newValue);
        time.text = str3;
        if (CGameManager.IsFederalScenario("绯色审判"))
          this.time.text = this.disease.customGlobalVariable4.ToString("f3");
        this.difficulty.text = !flag || this.disease.difficulty != 3 ? CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) this.disease.difficulty]) : (CGameManager.oldPotential - 0.004999).ToString("N2") + " → " + (CGameManager.newPotential - 0.004999).ToString("N2");
        this.cureProgress.text = CLocalisationManager.GetText("FE_EndGame_CureProgress").Replace("%amount", (100f * this.disease.cureCompletePercent).ToString("f2"));
        if (CGameManager.IsFederalScenario("时生虫ReMASTER") || CGameManager.IsFederalScenario("时生虫ReCRAFT") || CGameManager.IsFederalScenario("终末千面"))
          this.cureProgress.text = CLocalisationManager.GetText("FE_EndGame_CureProgress").Replace("%amount", (100f * this.disease.globalDeadPercent).ToString("f2"));
        this.geneticComplexity.text = Mathf.FloorToInt(this.disease.globalSeverity * (float) (this.disease.evoPoints * 2 + this.disease.evoPointsSpent)).ToString();
        if (CGameManager.IsFederalScenario("镜生虫ReMASTER") || CGameManager.IsFederalScenario("绯色审判"))
          this.geneticComplexity.text = this.disease.globalDeadPerc.ToString("N4");
        if (CGameManager.IsFederalScenario("PISMG"))
          this.geneticComplexity.text = ((SPDisease) this.disease).maxMusicCombo.ToString();
        if (CGameManager.IsFederalScenario("世界狂潮"))
        {
          this.geneticComplexity.width = 1080;
          this.geneticComplexity.text = this.disease.mutationCounter.ToString("N2") + " / " + (100.0 * (double) this.disease.globalDeadPerc).ToString("N2") + "%";
        }
        this.plagueType.text = CGameManager.GetDiseaseNameLoc(this.disease.diseaseType);
        long score;
        if (CGameManager.gameType == IGame.GameType.SpeedRun)
        {
          this.biohazardContainer.SetActive(true);
          this.scenarioBiohazardContainer.SetActive(false);
          score = (long) this.disease.turnNumber;
          this.scoreLabel.text = CLocalisationManager.GetText("Time") + CLocalisationManager.GetText("(Days)");
          this.difficultyParent.SetActive(false);
          int speedrunScore = CGameManager.GetSpeedrunScore(this.disease.diseaseType, score);
          for (int index = 0; index < this.scoreBiohazardMarks.Length; ++index)
          {
            this.scoreBiohazardMarks[index].gameObject.SetActive(true);
            this.scoreBiohazardMarks[index].alpha = index >= speedrunScore || !World.instance.gameWon ? 0.25f : 1f;
          }
        }
        else if (CGameManager.game.CurrentLoadedScenario != null)
        {
          this.biohazardContainer.SetActive(false);
          this.scenarioBiohazardContainer.SetActive(true);
          int[] numArray = CGameManager.ScenarioRatingBands;
          if (this.disease.scenario == "board_game")
            numArray = CGameManager.BoardGameRatingBands;
          if (this.disease.scenario == "fake_news")
            numArray = CGameManager.FakeNewsRatingBands;
          score = this.disease.GetScore(World.instance.gameWon, true);
          this.scoreLabel.text = CLocalisationManager.GetText("IG_Total_Score");
          this.difficultyParent.SetActive(true);
          for (int index = 0; index < this.scenarioScoreBiohazardMarks.Length && index < numArray.Length; ++index)
          {
            if (World.instance.gameWon)
            {
              if (index == 0 || score >= (long) numArray[index])
                NGUITools.SetActive(this.scenarioScoreBiohazardMarks[index], true);
              else
                NGUITools.SetActive(this.scenarioScoreBiohazardMarks[index], false);
            }
            else
              NGUITools.SetActive(this.scenarioScoreBiohazardMarks[index], false);
          }
        }
        else
        {
          this.biohazardContainer.SetActive(true);
          this.scenarioBiohazardContainer.SetActive(false);
          score = this.disease.GetScore(World.instance.gameWon, false);
          this.scoreLabel.text = CLocalisationManager.GetText("IG_Total_Score");
          this.difficultyParent.SetActive(true);
          for (int index = 0; index < this.scoreBiohazardMarks.Length && index < this.scoreThresholds.Length; ++index)
          {
            this.scoreBiohazardMarks[index].gameObject.SetActive(true);
            if (World.instance.gameWon)
            {
              if (index == 0 || score >= (long) this.scoreThresholds[index])
                this.scoreBiohazardMarks[index].alpha = 1f;
              else
                this.scoreBiohazardMarks[index].alpha = 0.25f;
            }
            else if (index == 0 && score >= (long) this.scoreThresholds[index])
              this.scoreBiohazardMarks[index].alpha = 1f;
            else
              this.scoreBiohazardMarks[index].alpha = 0.25f;
          }
        }
        this.totalScore.width = 1080;
        this.gameType.text = CLocalisationManager.GetText(CGameManager.GameTypeNames[CGameManager.gameType]);
        if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.filename.Contains("PIFSL"))
        {
          this.gameType.text = "联邦官方场景";
          if (this.disease.difficulty == 3)
          {
            int num2 = (int) score;
            int oldScore = CGameManager.oldScore;
            int num3 = num2 - oldScore;
            if (num3 >= 0)
              this.totalScore.text = num2.ToString() + " (+[00ffff]" + num3.ToString() + "[ffffff])";
            else if (num3 < 0)
            {
              UILabel totalScore = this.totalScore;
              string str4 = num2.ToString();
              num1 = -1 * num3;
              string str5 = num1.ToString();
              string str6 = str4 + " (-[ff5555]" + str5 + "[ffffff])";
              totalScore.text = str6;
            }
          }
          else
            this.totalScore.text = score.ToString();
        }
        else
          this.totalScore.text = score.ToString();
      }
      if (this.disease.isCure)
      {
        this.plagueType.text = CGameManager.GetDiseaseNameLoc(this.disease.cureScenario);
        if (CGameManager.IsFederalScenario("PIFCURE"))
          this.plagueType.text = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle;
        if (CGameManager.usingDiseaseX)
          this.plagueType.text += " X";
        UILabel cureProgress = this.cureProgress;
        string text = CLocalisationManager.GetText("FE_EndGame_CureProgress");
        num1 = Mathf.FloorToInt(Mathf.Clamp(this.disease.authority * 100f, 0.0f, 450f));
        string newValue = num1.ToString();
        string str7 = text.Replace("%amount%", newValue);
        cureProgress.text = str7;
        this.geneticComplexity.text = CUtils.FormatValueToDisplay(this.disease.estimatedDeathRate * 100f, true);
        if (this.gameWasWon)
        {
          this.background.spriteName = "Blue_Bg_01_Cure";
          this.resultTitle.text = CLocalisationManager.GetText("FE_EndGame_Title_Victory");
          this.mainTitle.text = CLocalisationManager.GetText("FE_EndGame_MainTitle_Victory_Cure");
          this.socialTxt = CLocalisationManager.GetText("The '%s' %s only killed %s people in %s days. I've saved the world!");
          string str8 = "";
          if (CGameManager.usingDiseaseX)
            str8 += " X";
          string socialTxt = this.socialTxt;
          string[] replacements = new string[4]
          {
            this.disease.name,
            CGameManager.GetDiseaseNameLoc(this.disease.cureScenario) + str8,
            CUtils.FormatNumberWithCommas(this.disease.totalKilled, true),
            null
          };
          num1 = this.disease.turnNumber;
          replacements[3] = num1.ToString();
          this.socialTxt = CUtils.ReplaceTokens(socialTxt, "%s", replacements);
          this.resultDescription.text = !this.disease.cureFlag ? CLocalisationManager.GetText("%s has been eradicated without the use of a vaccine. The world is saved!").Replace("%s", this.disease.name) : CLocalisationManager.GetText("%s has been eradicated by your vaccine. The world is saved!").Replace("%s", this.disease.name);
        }
        else
        {
          this.background.spriteName = "Red_Bg_01";
          this.resultTitle.text = CLocalisationManager.GetText("FE_EndGame_Title_Defeat");
          this.mainTitle.text = CLocalisationManager.GetText("FE_EndGame_MainTitle_Defeat_Cure");
          this.socialTxt = CLocalisationManager.GetText("The '%s' %s killed %s people in %s days. I've failed humanity...");
          string str9 = "";
          if (CGameManager.usingDiseaseX)
            str9 += " X";
          string socialTxt = this.socialTxt;
          string[] replacements = new string[4]
          {
            this.disease.name,
            CGameManager.GetDiseaseNameLoc(this.disease.cureScenario) + str9,
            CUtils.FormatNumberWithCommas(this.disease.totalKilled, true),
            null
          };
          num1 = this.disease.turnNumber;
          replacements[3] = num1.ToString();
          this.socialTxt = CUtils.ReplaceTokens(socialTxt, "%s", replacements);
          if (this.disease.diseaseType == Disease.EDiseaseType.CURE)
          {
            switch (this.disease.GetAuthorityLossReasons(0)[0].type)
            {
              case Disease.EAuthLoss.AUTH_LOSS_PANIC:
                this.resultDescription.text = CLocalisationManager.GetText("Your Authority collapsed, primarily from infected people panicking about dying from %s").Replace("%s", this.disease.name);
                break;
              case Disease.EAuthLoss.AUTH_LOSS_DEATHS:
                this.resultDescription.text = CLocalisationManager.GetText("Your Authority collapsed, primarily from too many people dying of %s").Replace("%s", this.disease.name);
                break;
              case Disease.EAuthLoss.AUTH_LOSS_SPREAD:
                this.resultDescription.text = CLocalisationManager.GetText("Your Authority collapsed, primarily from too many countries being infected by %s").Replace("%s", this.disease.name);
                break;
              case Disease.EAuthLoss.AUTH_LOSS_COMPLIANCE:
                this.resultDescription.text = CLocalisationManager.GetText("Your Authority collapsed, primarily from too much Non-Compliance").Replace("%s", this.disease.name);
                break;
            }
          }
        }
        this.shareMessageLabel.text = this.socialTxt;
      }
      this.SetGraphExclusive("Population");
      this.graphParent.SetActive(false);
      this.graphButtons.SetActive(false);
      this.mapParent.SetActive(true);
      this.populationToggle.SetOpen(true);
      this.diseaseToggle.SetOpen(true);
      this.cureToggle.SetOpen(true);
      if (CGameManager.game.HasReplayData)
      {
        this.replayButtonContainer.SetActive(true);
        this.noReplayAvailableContainer.SetActive(false);
        this.Invoke("StartReplay", 0.1f);
      }
      else if (this.mapParent.activeSelf)
      {
        this.replayButtonContainer.SetActive(false);
        this.noReplayAvailableContainer.SetActive(true);
        this.RefreshData();
        this.ToggleGraph();
      }
      if (CGameManager.IsMultiplayerGame)
        this.retry.gameObject.SetActive(false);
      else
        this.retry.gameObject.SetActive(true);
      if (CGameManager.IsTutorialGame)
      {
        TutorialSystem.RegisterInterface((ITutorial) this);
        if (this.gameWasWon)
        {
          TutorialSystem.MarkModuleComplete("31A");
          TutorialSystem.MarkModuleComplete("31B");
          TutorialSystem.MarkModuleComplete("31C");
          TutorialSystem.MarkModuleComplete("33A");
          TutorialSystem.MarkModuleComplete("33B");
          TutorialSystem.CheckModule((Func<bool>) (() => true), "29A", true);
        }
        else
        {
          TutorialSystem.MarkModuleComplete("29A");
          TutorialSystem.MarkModuleComplete("29B");
          TutorialSystem.MarkModuleComplete("29C");
          TutorialSystem.MarkModuleComplete("32A");
          TutorialSystem.MarkModuleComplete("32B");
          TutorialSystem.CheckModule((Func<bool>) (() => true), "31A", true);
        }
      }
      if (CGameManager.gameType == IGame.GameType.Classic && CGameManager.game.Difficulty == 1U && !COptionsManager.instance.mbPityMode && !this.gameWasWon)
      {
        COptionsManager.instance.GetPityModeStatus(this.disease.diseaseType);
        int pityModeStatus = COptionsManager.instance.GetPityModeStatus(this.disease.diseaseType);
        if (pityModeStatus >= 0)
        {
          int completed;
          if (pityModeStatus >= 2)
          {
            CUIManager.instance.redConfirmOverlay.ShowLocalised("FE_Options_Pity_Mode", "FE_Options_Gameplay_Tooltip_Pity_Mode", "Yes", "No", new CConfirmOverlay.PressDelegate(this.TurnOnPityMode));
            completed = -1;
          }
          else
            completed = pityModeStatus + 1;
          COptionsManager.instance.SetPityModeStatus(this.disease.diseaseType, completed);
        }
      }
    }
    base.SetActive(active);
  }

  private void TurnOnPityMode() => COptionsManager.instance.PityMode = EState.On;

  private void Update()
  {
    this.countryChange -= Time.deltaTime;
    if ((double) this.countryChange < 0.0)
    {
      this.countryChange += this.countryChangeSpeed;
      if ((bool) (UnityEngine.Object) CInterfaceManager.instance)
        CInterfaceManager.instance.UpdateCountryState();
      if (this.disease != null)
      {
        this.healthyText.text = this.disease.totalUninfected.ToString();
        this.infectedText.text = this.disease.totalInfected.ToString();
        UILabel deadText = this.deadText;
        long num = this.disease.totalDead;
        string str1 = num.ToString();
        deadText.text = str1;
        if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
        {
          this.apeHealthyText.text = this.disease.apeTotalHealthy.ToString();
          this.apeInfectedText.text = this.disease.apeTotalInfected.ToString();
          this.apeDeadText.text = this.disease.apeTotalDead.ToString();
        }
        else if (this.disease.isCure)
        {
          UILabel zombieText = this.zombieText;
          num = this.disease.totalHealthyRecovered;
          string str2 = num.ToString();
          zombieText.text = str2;
        }
        else
        {
          UILabel zombieText = this.zombieText;
          num = this.disease.totalZombie;
          string str3 = num.ToString();
          zombieText.text = str3;
        }
      }
    }
    if (World.instance == null || !this.replayStarted || this.lastReplayTurn == World.instance.DiseaseTurn)
      return;
    this.lastReplayTurn = World.instance.DiseaseTurn;
    this.replayTurnLabel.text = CLocalisationManager.GetText("FE_EndGame_Days").Replace("%days", this.lastReplayTurn.ToString());
  }

  public void ToggleGraph()
  {
    if (this.mapParent.activeSelf)
    {
      this.RefreshData();
      this.graphParent.SetActive(true);
      this.graphButtons.SetActive(true);
      this.mapParent.SetActive(false);
    }
    else if (!CGameManager.game.HasReplayData)
    {
      this.confirm.ShowLocalised("IG_Cloud_Save_Replay_Title", "IG_Cloud_Save_Replay_Text", "OK", pressA: (CConfirmOverlay.PressDelegate) (() => this.confirm.Hide()));
    }
    else
    {
      this.graphParent.SetActive(false);
      this.graphButtons.SetActive(false);
      this.mapParent.SetActive(true);
    }
  }

  private void StartReplay()
  {
    CGameManager.game.ReplayGame();
    this.SetReplaySpeed(CEndGameScreen.ReplaySpeed.PLAY);
    this.disease = CNetworkManager.network.LocalPlayerInfo.disease;
    this.replayStarted = true;
    this.restartTime = Time.realtimeSinceStartup;
  }

  private void SetReplaySpeed(CEndGameScreen.ReplaySpeed speed)
  {
    this.pauseReplayTgl.value = speed == CEndGameScreen.ReplaySpeed.PAUSED;
    this.playReplayTgl.value = speed == CEndGameScreen.ReplaySpeed.PLAY;
    this.fastReplayTgl.value = speed == CEndGameScreen.ReplaySpeed.FAST;
    this.superFastReplayTgl.value = speed == CEndGameScreen.ReplaySpeed.SUPER_FAST;
    CGameManager.game.SetSpeed((int) speed);
  }

  public void OnClickPauseReplay()
  {
    Analytics.Event("ReplaySpeed", "Pause");
    CGameManager.game.SetSpeed(0);
  }

  public void OnClickPlayReplay()
  {
    Analytics.Event("ReplaySpeed", "Normal");
    CGameManager.game.SetSpeed(4);
  }

  public void OnClickFastReplay()
  {
    Analytics.Event("ReplaySpeed", "Fast");
    CGameManager.game.SetSpeed(8);
  }

  public void OnClickSuperFastReplay()
  {
    Analytics.Event("ReplaySpeed", "SuperFast");
    CGameManager.game.SetSpeed(15);
  }

  public void OnClickRestartReplay()
  {
    if (!CGameManager.game.HasReplayData || (double) Time.realtimeSinceStartup - (double) this.restartTime <= 1.0 || !this.replayStarted || this.lastReplayTurn <= 0)
      return;
    this.replayStarted = false;
    this.restartTime = Time.realtimeSinceStartup;
    CInterfaceManager.instance.Cleanup();
    this.replayTurnLabel.text = CLocalisationManager.GetText("FE_EndGame_Days").Replace("%days", "0");
    this.Invoke("StartReplay", 0.1f);
  }

  public void OnClickShareFacebook()
  {
    if (TutorialSystem.IsModuleActive())
      return;
    Analytics.Event("EndScreen Share Facebook" + (this.disease != null ? " " + this.disease.diseaseType.ToString() : ""));
    if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      CSocialShare.instance.ShareMessageFacebook(this.shareMessageLabel.text, this.simianShareLink);
    else
      CSocialShare.instance.ShareMessageFacebook(this.shareMessageLabel.text, this.nonSimianShareLink);
  }

  public void OnClickShareEmail()
  {
    if (TutorialSystem.IsModuleActive())
      return;
    Analytics.Event("EndScreen Share Email" + (this.disease != null ? " " + this.disease.diseaseType.ToString() : ""));
    if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      CSocialShare.instance.ShareMessageEmail(this.shareMessageLabel.text, this.simianShareLink);
    else
      CSocialShare.instance.ShareMessageEmail(this.shareMessageLabel.text, this.nonSimianShareLink);
  }

  public void OnClickShareTwitter()
  {
    if (TutorialSystem.IsModuleActive())
      return;
    Analytics.Event("EndScreen Share Twitter" + (this.disease != null ? " " + this.disease.diseaseType.ToString() : ""));
    if (this.disease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      CSocialShare.instance.ShareMessageTwitter(this.shareMessageLabel.text, this.simianShareLink);
    else
      CSocialShare.instance.ShareMessageTwitter(this.shareMessageLabel.text, this.nonSimianShareLink);
  }

  public void OnClickRetry()
  {
    int gameType = (int) CGameManager.gameType;
    Disease.EDiseaseType diseaseType = CGameManager.localPlayerInfo.disease.diseaseType;
    Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
    DynamicMusic.instance.FadeOut();
    CGameManager.ClearGame();
    TutorialSystem.Instance.ResetAndRewind();
    CGameManager.gameType = (IGame.GameType) gameType;
    if (CGameManager.gameType == IGame.GameType.SpeedRun)
      CGameManager.CreateGame(currentLoadedScenario, diseaseType);
    else
      CGameManager.CreateGame(currentLoadedScenario);
    if (gameType == 8)
    {
      CGSScreen screen = CUIManager.instance.GetScreen("GameSetupScreen_Cure") as CGSScreen;
      screen.OnClickNext();
      screen.OnClickNext();
    }
    else
    {
      CGSScreen screen = CUIManager.instance.GetScreen("GameSetupScreen") as CGSScreen;
      screen.OnClickNext();
      screen.OnClickNext();
      screen.OnClickNext();
    }
  }

  public void OnClickExit()
  {
    if (CGameManager.IsTutorialGame)
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.JustPlayedTutorial = true;
      instance.DidWinTutorial = this.gameWasWon;
      instance.ResetInGameTutorials();
    }
    Scenario scenario_ = CGameManager.game.CurrentLoadedScenario;
    if (scenario_ != null && !scenario_.isOfficial && !scenario_.scenarioInformation.isMine && this.gameWasWon)
      this.rateModal.ShowLocalised("FE_Scenario_Rate_Title", "FE_Scenario_Rate_Content", "No", "Yes", (CConfirmOverlay.PressDelegate) (() => this.RateScenario(false, scenario_.scenarioInformation)), (CConfirmOverlay.PressDelegate) (() => this.RateScenario(true, scenario_.scenarioInformation)), variables: new Dictionary<string, string>()
      {
        {
          "scenarioname",
          scenario_.scenarioInformation.scenTitle
        }
      });
    IGameScreen screen = CUIManager.instance.GetScreen("MainMenuScreen");
    string name = (string) null;
    if (CGameManager.game.CurrentLoadedScenario != null)
    {
      switch (CGameManager.gameType)
      {
        case IGame.GameType.Official:
          name = "Menu_Sub_Official";
          break;
        case IGame.GameType.Custom:
          name = "Menu_Sub_Custom";
          CGameManager.returnFromScenario = true;
          break;
      }
    }
    bool flag = false;
    if (CGameManager.gameType == IGame.GameType.SpeedRun)
      flag = true;
    bool isMultiplayerGame = CGameManager.IsMultiplayerGame;
    CGameManager.ClearGame();
    DynamicMusic.instance.FadeOut();
    CUIManager.instance.SetupScreens();
    if (!string.IsNullOrEmpty(name))
    {
      CUIManager.instance.ClearHistory();
      List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Clear();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_NewGame"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Add(screen.GetSubScreen(name));
      CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
    }
    else if (flag)
    {
      CUIManager.instance.ClearHistory();
      List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Clear();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_NewGame"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Add(screen.GetSubScreen("Menu_Sub_Speedrun"));
      CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
    }
    else if (isMultiplayerGame)
    {
      CUIManager.instance.ClearHistory();
      List<IGameSubScreen> igameSubScreenList = new List<IGameSubScreen>();
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Main"));
      CUIManager.instance.SaveBreadcrumb(screen, igameSubScreenList);
      igameSubScreenList.Clear();
      screen.HideSubScreen("Start");
      igameSubScreenList.Add(screen.GetSubScreen("Main_Sub_Multi"));
      CUIManager.instance.SetActiveScreen(screen, overrideSubScreens: igameSubScreenList);
      CNetworkManager.network.Terminate();
      CNetworkManager.network.Initialize();
    }
    else
      CUIManager.instance.SetActiveScreen(screen);
    if (!this.Unlocked.disease.HasValue && this.Unlocked.gene == null)
      return;
    CUIManager.instance.ShowUnlockPopup(this.Unlocked);
  }

  public void OnTutorialBegin(Module withModule)
  {
    string name = withModule.Name;
    if (!(name == "31A") && !(name == "29A"))
      return;
    this.retry.gameObject.SetActive(false);
    this.exit.gameObject.SetActive(false);
  }

  public void OnTutorialComplete(Module completedModule)
  {
    string name = completedModule.Name;
    if (!(name == "31C") && !(name == "29C"))
      return;
    this.exit.gameObject.SetActive(true);
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    this.retry.gameObject.SetActive(true);
    this.exit.gameObject.SetActive(true);
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  private enum ReplaySpeed
  {
    PAUSED = 0,
    PLAY = 4,
    FAST = 8,
    SUPER_FAST = 15, // 0x0000000F
  }
}
