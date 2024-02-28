// Decompiled with JetBrains decompiler
// Type: CWorldScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CWorldScreen : IGameScreen, ITutorial
{
  public static CWorldScreen instance;
  public Camera WorldCamera;
  [Header("General")]
  public UILabel mpTitle;
  public UIButton buttonHelp;
  [Header("Border Status")]
  public UILabel BoarderOpenText;
  public UILabel BoarderClosedText;
  public UILabel AirOpenText;
  public UILabel AirClosedText;
  public UILabel SeaOpenText;
  public UILabel SeaClosedText;
  public DefconObject defcon;
  [Header("Map")]
  public UIToggle mapRich;
  public UIToggle mapPoor;
  public UIToggle mapRural;
  public UIToggle mapUrban;
  public UIToggle mapHot;
  public UIToggle mapCold;
  public UIToggle mapHumid;
  public UIToggle mapArid;
  [Header("Chart")]
  public PieChart Chart;
  public UILabel ChartTotal;
  public UIImageButton GraphExpand;
  public UIImageButton CureExpand;
  public UIEventListener GraphWholeExpand;
  public UIEventListener CureWholeExpand;
  [Header("Cure")]
  public UILabel StatusValue;
  public UILabel StatusPercent;
  public UILabel StatusDays;
  public UISlider StatusCureBar;
  public UISprite StatusCureFlask;
  [Header("Research Countries")]
  public UILabel[] KeyCountries;
  [Header("Research Priority")]
  public UILabel PriorityValue;
  public UILabel PriorityDestroyedTitle;
  public UILabel PriorityActiveTitle;
  public UILabel PriorityPotentialTitle;
  public UISlider PriorityActiveBar;
  public UISlider PriorityDestroyedBar;
  [Header("Stats")]
  public UILabel StatsDeadValue;
  public UILabel StatsInfectedValue;
  public UILabel StatsHealthyValue;
  public UILabel statsZombieValue;
  public GameObject statsZombieObject;
  [Header("Status")]
  public UILabel StatusTitle;
  public StatusLabel statusTemplate;
  public GameObject StatusContents;
  public UIScrollBar StatusScroll;
  public UIGrid StatusGrid;
  public UISprite StatusInfectedSprite;
  private List<StatusLabel> StatusBuffer;
  protected Dictionary<UIToggle, MiniMapController.Trait> mFilterMap;
  [Header("Cure ETA")]
  public UILabel CureETADate;
  public UILabel cureETATitle;
  private DateTime cureDate;
  private UIEventListener.VoidDelegate onButtonGraphExpandDoubleClick;
  private UIEventListener.VoidDelegate onButtonCureExpandDoubleClick;
  [Header("Infection sprites")]
  public UISprite healthyStatSprite;
  public UISprite healthyCountrySprite;
  public UISprite deadStatSprite;
  public UISprite deadCountrySprite;
  public UISprite infectedStatSprite;
  public UISprite infectedCountrySprite;
  public Material DeadMaterial;
  public Material HealthyMaterial;
  public Material InfectedMaterial;
  public Material UnawareMaterial;
  public Material ZombieMaterial;
  private bool FakeNewsMaterials;

  public MiniMapController mapController => MiniMapController.instance;

  protected virtual void Awake() => CWorldScreen.instance = this;

  public virtual void OnGameStarted()
  {
  }

  public override void Initialise()
  {
    base.Initialise();
    EventDelegate.Add(this.mapHot.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapCold.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapArid.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapHumid.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapRural.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapUrban.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapRich.onChange, new EventDelegate.Callback(this.FilterAdditional));
    EventDelegate.Add(this.mapPoor.onChange, new EventDelegate.Callback(this.FilterAdditional));
    this.onButtonCureExpandDoubleClick = (UIEventListener.VoidDelegate) (_ => this.GraphExpandClick());
    this.onButtonGraphExpandDoubleClick = (UIEventListener.VoidDelegate) (_ => this.CureExpandClick());
    this.GraphWholeExpand.onDoubleClick += this.onButtonGraphExpandDoubleClick;
    this.CureWholeExpand.onDoubleClick += this.onButtonCureExpandDoubleClick;
    this.mFilterMap = new Dictionary<UIToggle, MiniMapController.Trait>();
    this.mFilterMap.Add(this.mapHot, MiniMapController.Trait.Hot);
    this.mFilterMap.Add(this.mapCold, MiniMapController.Trait.Cold);
    this.mFilterMap.Add(this.mapArid, MiniMapController.Trait.Arid);
    this.mFilterMap.Add(this.mapHumid, MiniMapController.Trait.Humid);
    this.mFilterMap.Add(this.mapRural, MiniMapController.Trait.Rural);
    this.mFilterMap.Add(this.mapUrban, MiniMapController.Trait.Urban);
    this.mFilterMap.Add(this.mapRich, MiniMapController.Trait.Rich);
    this.mFilterMap.Add(this.mapPoor, MiniMapController.Trait.Poor);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    EventDelegate.Set(this.GraphExpand.onClick, new EventDelegate.Callback(this.GraphExpandClick));
    EventDelegate.Set(this.CureExpand.onClick, new EventDelegate.Callback(this.CureExpandClick));
    TutorialSystem.RegisterInterface((ITutorial) this);
  }

  public override float Alpha
  {
    set => base.Alpha = value;
  }

  public void SetHealthySprite(string name)
  {
    this.healthyStatSprite.spriteName = name;
    this.healthyCountrySprite.spriteName = name;
  }

  public void SetDeadSprite(string name)
  {
    this.deadStatSprite.spriteName = name;
    this.deadCountrySprite.spriteName = name;
  }

  public void SetInfectedSprite(string name)
  {
    this.infectedStatSprite.spriteName = name;
    this.infectedCountrySprite.spriteName = name;
  }

  public override void SetActive(bool isActive)
  {
    base.SetActive(isActive);
    if (isActive)
    {
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.Tutorial)
      {
        TutorialSystem.SetLocalisedDictionary("21C", new Dictionary<string, string>()
        {
          {
            "disease",
            World.instance.GetDisease(0).name
          }
        });
        TutorialSystem.MarkModuleComplete("19A");
        TutorialSystem.MarkModuleComplete("19B");
        TutorialSystem.CheckModule((Func<bool>) (() => true), "20A", true);
        CGameManager.SetPaused(true, true);
      }
      if (CGameManager.gameType == IGame.GameType.CureTutorial)
      {
        if (!this.CheckCureTutorialRestrictions())
          return;
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
        instance.StartCoroutine(instance.UpdateTutorial());
      }
      if ((UnityEngine.Object) CHUDScreen.instance != (UnityEngine.Object) null)
        CHUDScreen.instance.AutoCloseChat();
      if (World.instance != null)
      {
        if (World.instance.GetDisease(0).diseaseType == Disease.EDiseaseType.FAKE_NEWS)
        {
          if (!this.FakeNewsMaterials)
          {
            this.FakeNewsMaterials = true;
            this.SetHealthySprite("Hex_Unaware");
            this.SetDeadSprite("Hex_Healthy");
            this.SetInfectedSprite("Hex_Infected");
            this.Chart.SetSprites("Healthy", "questionmark", "Infected", "zz_zombie_icon");
            this.Chart.SetMaterials(this.HealthyMaterial, this.UnawareMaterial, this.InfectedMaterial, this.ZombieMaterial);
          }
        }
        else if (this.FakeNewsMaterials)
        {
          this.SetHealthySprite("Hex_Healthy");
          this.SetDeadSprite("Hex_Dead");
          this.SetInfectedSprite("Hex_Infected");
          this.Chart.SetSprites("Dead", "Healthy", "Infected", "zz_zombie_icon");
          this.Chart.SetMaterials(this.DeadMaterial, this.HealthyMaterial, this.InfectedMaterial, this.ZombieMaterial);
        }
      }
    }
    else if (CGameManager.IsTutorialGame)
    {
      if (CGameManager.gameType == IGame.GameType.CureTutorial && TutorialSystem.IsModuleComplete("C50"))
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C51");
      CGameManager.SetPaused(false, true);
    }
    if (World.instance == null)
      return;
    this.StatusScroll.value = 0.0f;
    if (this.StatusBuffer == null)
    {
      this.StatusBuffer = new List<StatusLabel>();
      Transform transform = this.StatusGrid.transform;
      int count = World.instance.countries.Count;
      UIPanel component = this.StatusContents.GetComponent<UIPanel>();
      for (int index1 = 0; index1 < count; ++index1)
      {
        StatusLabel statusLabel = UnityEngine.Object.Instantiate<StatusLabel>(this.statusTemplate);
        statusLabel.transform.parent = transform;
        statusLabel.transform.localScale = Vector3.one;
        statusLabel.transform.localPosition = Vector3.zero;
        for (int index2 = 0; index2 < statusLabel.cursorAnchors.Length; ++index2)
          statusLabel.cursorAnchors[index2].panel = component;
        this.StatusBuffer.Add(statusLabel);
      }
    }
    this.Refresh();
    MiniMapController.instance.SetMiniMapActive(isActive);
  }

  public override void Refresh()
  {
    base.Refresh();
    Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
    this.defcon.SetDefconState(CGameManager.GetDefconState(disease.globalPriority), disease.globalPriority);
    List<Country> countries = World.instance.countries;
    int count = countries.Count;
    int boarder = 0;
    int air = 0;
    int sea = 0;
    int boarderClosed = 0;
    int airClosed = 0;
    int seaClosed = 0;
    for (int index = 0; index < count; ++index)
    {
      Country country = countries[index];
      if (country.borderStatus)
        ++boarder;
      else
        ++boarderClosed;
      if (country.hasAirport)
      {
        if (country.airportStatus)
          ++air;
        else
          ++airClosed;
      }
      if (country.hasPorts)
      {
        if (country.portStatus)
          ++sea;
        else
          ++seaClosed;
      }
    }
    this.SetBorderStatus(boarder, boarderClosed, air, airClosed, sea, seaClosed);
    MiniMapController.instance.RefreshFilters();
    this.SetChart(disease);
    this.SetCureStatus(disease);
    this.SetKeyResearchers(disease);
    this.SetResearchPriority(disease);
    long totalPopulation = World.instance.totalPopulation;
    long currentTotalInfected = disease.GetCurrentTotalInfected();
    long currentTotalKilled = disease.GetCurrentTotalKilled();
    long currentTotalZombie = disease.GetCurrentTotalZombie();
    this.SetWorldStats(totalPopulation - (currentTotalInfected + currentTotalKilled + currentTotalZombie), currentTotalInfected, currentTotalKilled, currentTotalZombie);
    this.mpTitle.text = CGameManager.GetWorldDefconText();
    this.SetCountryList();
  }

  protected void FilterAdditional()
  {
    if (this.mFilterMap == null || !this.mFilterMap.ContainsKey(UIToggle.current))
      return;
    this.mapController.SetTraitFiltered(this.mFilterMap[UIToggle.current], UIToggle.current.value);
  }

  public virtual void SetCountryList()
  {
    if (CNetworkManager.network.LocalPlayerInfo == null)
      return;
    Disease disease = CNetworkManager.network.LocalPlayerInfo.disease;
    List<Country> healthyCountries = new List<Country>();
    List<Country> infectedCountries = new List<Country>();
    List<Country> deadCountries = new List<Country>();
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = country.GetLocalDisease(disease);
      bool flag = true;
      if (disease.isCure && !localDisease.hasIntel)
        flag = false;
      if (!disease.isCure && localDisease != null && country.totalDead == country.originalPopulation)
        deadCountries.Add(country);
      else if (disease.isCure & flag && localDisease != null && country.healthyRecoveredPopulation > 0L && localDisease.infectedPopulation <= 0L)
        deadCountries.Add(country);
      else if (flag && localDisease != null && (localDisease.infectedPopulation > 0L || localDisease.zombie > 0L))
        infectedCountries.Add(country);
      else
        healthyCountries.Add(country);
    }
    infectedCountries.Sort((Comparison<Country>) ((c1, c2) => c2.myUninfectedPercentage.CompareTo(c1.myUninfectedPercentage)));
    this.SetWorldStatus(healthyCountries, infectedCountries, deadCountries);
  }

  public virtual void GraphExpandClick()
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial)
      return;
    CUIManager.instance.OpenGraphScreen("Population");
  }

  private void CureExpandClick()
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial)
      return;
    CUIManager.instance.OpenGraphScreen("Cure");
  }

  public void SetBorderStatus(
    int boarder,
    int boarderClosed,
    int air,
    int airClosed,
    int sea,
    int seaClosed)
  {
    this.BoarderOpenText.text = boarder.ToString();
    this.BoarderClosedText.text = boarderClosed.ToString();
    this.AirOpenText.text = air.ToString();
    this.AirClosedText.text = airClosed.ToString();
    this.SeaOpenText.text = sea.ToString();
    this.SeaClosedText.text = seaClosed.ToString();
  }

  public virtual void SetChart(Disease disease)
  {
    this.Chart.SetFactors(new float[4]
    {
      (float) disease.totalHealthy,
      (float) disease.totalInfected,
      (float) disease.totalDead,
      (float) disease.totalZombie
    });
    this.ChartTotal.text = World.instance.totalPopulation.ToString();
  }

  public virtual void SetCureStatus(Disease disease)
  {
    float num1 = Mathf.Clamp01(disease.cureCompletePercent);
    if (disease.cureFlag)
    {
      this.StatusValue.text = CLocalisationManager.GetText("IG_Status_Research_Complete");
      this.StatusPercent.text = "";
      this.StatusDays.text = "";
    }
    else if ((double) num1 <= 0.0)
    {
      this.StatusValue.text = CLocalisationManager.GetText("IG_Status_No_Research");
      this.StatusPercent.text = "";
      this.StatusDays.text = "";
      this.cureETATitle.text = "";
      this.CureETADate.text = "";
    }
    else
    {
      this.StatusValue.text = CLocalisationManager.GetText("IG_Status_Researching");
      this.StatusPercent.text = (num1 * 100f).ToString("f2") + "%";
      if (disease.CureDaysRemaining <= 0)
      {
        this.StatusDays.text = "";
        this.CureETADate.text = CLocalisationManager.GetText("Unknown");
      }
      else
      {
        int num2 = disease.CureDaysRemaining / 365;
        this.StatusDays.text = num2 <= 0 ? CLocalisationManager.GetText("IG_Days_Until_Completion_Days_Only").Replace("%days", (disease.CureDaysRemaining % 365).ToString()) : (num2 <= 1 ? CLocalisationManager.GetText("IG_Days_Until_Completion_Singular").Replace("%days", (disease.CureDaysRemaining % 365).ToString()).Replace("%year", num2.ToString()) : CLocalisationManager.GetText("IG_Days_Until_Completion").Replace("%days", (disease.CureDaysRemaining % 365).ToString()).Replace("%years", num2.ToString()));
        this.cureDate = CGameManager.currentGameDate;
        this.cureDate = this.cureDate.AddDays((double) disease.CureDaysRemaining);
        UILabel cureEtaDate = this.CureETADate;
        string[] strArray = new string[5];
        int num3 = this.cureDate.Day;
        strArray[0] = num3.ToString();
        strArray[1] = "-";
        num3 = this.cureDate.Month;
        strArray[2] = num3.ToString();
        strArray[3] = "-";
        num3 = this.cureDate.Year;
        strArray[4] = num3.ToString();
        string str = string.Concat(strArray);
        cureEtaDate.text = str;
      }
      this.cureETATitle.text = CLocalisationManager.GetText("IG_Cure_ETA");
    }
    this.StatusCureBar.value = num1;
    this.StatusCureFlask.fillAmount = num1;
  }

  public void SetKeyResearchers(Disease disease)
  {
    Country[] cureContributors = disease.GetTopCureContributors();
    for (int index = 0; index < this.KeyCountries.Length; ++index)
      this.KeyCountries[index].text = cureContributors == null || index >= cureContributors.Length || cureContributors[index] == null ? "" : CLocalisationManager.GetText(cureContributors[index].name) + " - $" + cureContributors[index].GetLocalDisease(disease).localCureResearch.ToString("N2");
  }

  public void SetResearchPriority(Disease disease)
  {
    if ((double) disease.cureResearchAllocationAverage <= 0.40000000596046448 && (double) disease.cureResearchAllocationAverage <= 0.10000000149011612)
    {
      double allocationAverage = (double) disease.cureResearchAllocationAverage;
    }
    string[] strArray = new string[11];
    int num1 = disease.totalFlaskResearched;
    strArray[0] = num1.ToString();
    strArray[1] = " / ";
    num1 = disease.totalFlaskBroken + disease.totalFlaskResearched + disease.totalFlaskEmpty;
    strArray[2] = num1.ToString();
    strArray[3] = " Activated, $";
    strArray[4] = CWorldScreen.NormalizeMoneyString((double) disease.globalCureResearchThisTurn, 2);
    strArray[5] = " \n";
    strArray[6] = "$";
    strArray[7] = CWorldScreen.NormalizeMoneyString((double) disease.globalCureResearch, 2);
    strArray[8] = " / ";
    strArray[9] = "$";
    strArray[10] = CWorldScreen.NormalizeMoneyString((double) disease.cureRequirements, 2);
    string tagName = string.Concat(strArray);
    Disease disease1 = (Disease) null;
    if (CGameManager.IsMultiplayerGame || CGameManager.IsCoopMPGame)
    {
      for (int index = 0; index < World.instance.diseases.Count; ++index)
      {
        if (World.instance.diseases[index] != disease)
          disease1 = World.instance.diseases[index];
      }
    }
    if (CGameManager.IsMultiplayerGame)
      tagName = "$" + CWorldScreen.NormalizeMoneyString((double) disease.globalCureResearchThisTurn, 2) + " reasearched ag. you\n$" + CWorldScreen.NormalizeMoneyString((double) disease1.globalCureResearchThisTurn, 2) + " reasearched ag. opponent";
    if (CGameManager.IsCoopMPGame)
      tagName = "$" + CWorldScreen.NormalizeMoneyString((double) disease.globalCureResearchThisTurn, 2) + " reasearched ag. you\n$" + CWorldScreen.NormalizeMoneyString((double) disease1.globalCureResearchThisTurn, 2) + " reasearched ag. partner";
    this.PriorityValue.color = new Color((float) (1.0 - (double) disease.globalCureResearchThisTurn / 300000.0), 1f, 1f);
    this.PriorityValue.text = CLocalisationManager.GetText(tagName);
    int num2 = disease.totalFlaskBroken + disease.totalFlaskResearched + disease.totalFlaskEmpty;
    this.PriorityActiveBar.value = (float) disease.totalFlaskResearched / (float) num2;
    this.PriorityDestroyedBar.value = (float) disease.totalFlaskBroken / (float) num2;
  }

  public string FormatPopulation(long pop) => pop.ToString("N0");

  public void SetWorldStats(
    long healthyGlobal,
    long infectedGlobal,
    long deadGlobal,
    long zombieGlobal)
  {
    if (World.instance.diseases.Count > 1)
    {
      long totalInfected = World.instance.GetTotalInfected();
      long totalDead = World.instance.GetTotalDead();
      this.StatsHealthyValue.text = this.FormatPopulation(World.instance.totalPopulation - totalInfected - totalDead);
      this.StatsInfectedValue.text = this.FormatPopulation(totalInfected);
      this.StatsDeadValue.text = this.FormatPopulation(totalDead);
    }
    else
    {
      this.StatsHealthyValue.text = healthyGlobal.ToString();
      this.StatsInfectedValue.text = infectedGlobal.ToString();
      this.StatsDeadValue.text = deadGlobal.ToString();
    }
    if (zombieGlobal > 0L)
    {
      this.statsZombieValue.text = zombieGlobal.ToString();
      if (this.statsZombieObject.activeSelf)
        return;
      this.statsZombieObject.SetActive(true);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.statsZombieObject || !this.statsZombieObject.activeSelf)
        return;
      this.statsZombieObject.SetActive(false);
    }
  }

  public void SetWorldStatus(
    List<Country> healthyCountries,
    List<Country> infectedCountries,
    List<Country> deadCountries,
    int coopToggleState = 1)
  {
    int num = healthyCountries.Count > infectedCountries.Count ? healthyCountries.Count : infectedCountries.Count;
    if (deadCountries.Count > num)
      num = deadCountries.Count;
    int count = this.StatusBuffer == null ? 0 : this.StatusBuffer.Count;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < count; ++index)
    {
      StatusLabel statusLabel = this.StatusBuffer[index];
      if (index >= num)
      {
        statusLabel.gameObject.SetActive(false);
      }
      else
      {
        statusLabel.gameObject.SetActive(true);
        Country healthyCountry = index >= healthyCountries.Count ? (Country) null : healthyCountries[index];
        Country infectedCountry = index >= infectedCountries.Count ? (Country) null : infectedCountries[index];
        Country deadCountry = index >= deadCountries.Count ? (Country) null : deadCountries[index];
        statusLabel.SetCountries(healthyCountry, infectedCountry, deadCountry, index % 2 == 0, coopToggleState: coopToggleState);
        zero.y = (float) (index * -34);
        statusLabel.transform.localPosition = zero;
      }
    }
    this.StatusScroll.ForceUpdate();
    this.StatusGrid.Reposition();
  }

  private void WorldHelp()
  {
    List<IGameSubScreen> overrideSubScreens = new List<IGameSubScreen>();
    CPauseScreen screen = CUIManager.instance.GetScreen("PauseScreen") as CPauseScreen;
    CMainHowToPlaySubScreen subScreen = screen.GetSubScreen("Pause_Sub_Help") as CMainHowToPlaySubScreen;
    overrideSubScreens.Add((IGameSubScreen) subScreen);
    CUIManager.instance.SetActiveScreen((IGameScreen) screen, 0.5f, 0.5f, overrideSubScreens);
    subScreen.GoTo(7, true);
  }

  private void GoBack(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    if (CGameManager.IsTutorialGame)
    {
      switch (CGameManager.gameType)
      {
        case IGame.GameType.Tutorial:
          if (TutorialSystem.IsModuleActive("21D"))
          {
            PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
            instance.StartCoroutine(instance.UpdateTutorial());
          }
          CUIManager.instance.WaitForFrame(new Action(CUIManager.instance.OpenGameScreen));
          break;
        case IGame.GameType.CureTutorial:
          if (!TutorialSystem.IsModuleComplete("C50"))
            break;
          CUIManager.instance.WaitForFrame(new Action(CUIManager.instance.OpenGameScreen));
          break;
      }
    }
    else
      CUIManager.instance.SetActiveScreen("HUDScreen");
  }

  private void GoToDisease(CActionManager.ActionType type)
  {
    if (CGameManager.gameType == IGame.GameType.CureTutorial || type != CActionManager.ActionType.START)
      return;
    CUIManager.instance.SetActiveScreen("DiseaseScreen");
  }

  public void OnTutorialBegin(Module withModule)
  {
    EventDelegate.Remove(this.buttonHelp.onClick, new EventDelegate.Callback(this.WorldHelp));
    switch (withModule.Name)
    {
      case "20A":
        EventDelegate.Remove(this.buttonHelp.onClick, new EventDelegate.Callback(this.WorldHelp));
        EventDelegate.Remove(this.GraphExpand.onClick, new EventDelegate.Callback(this.GraphExpandClick));
        EventDelegate.Remove(this.CureExpand.onClick, new EventDelegate.Callback(this.CureExpandClick));
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        this.GraphWholeExpand.onDoubleClick -= this.onButtonGraphExpandDoubleClick;
        this.CureWholeExpand.onDoubleClick -= this.onButtonCureExpandDoubleClick;
        break;
      case "21D":
        CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        break;
    }
  }

  public void OnTutorialComplete(Module completedModule)
  {
    switch (completedModule.Name)
    {
      case "21D":
        EventDelegate.Set(this.buttonHelp.onClick, new EventDelegate.Callback(this.WorldHelp));
        EventDelegate.Set(this.GraphExpand.onClick, new EventDelegate.Callback(this.GraphExpandClick));
        EventDelegate.Set(this.CureExpand.onClick, new EventDelegate.Callback(this.CureExpandClick));
        CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        this.GraphWholeExpand.onDoubleClick += this.onButtonGraphExpandDoubleClick;
        this.CureWholeExpand.onDoubleClick += this.onButtonCureExpandDoubleClick;
        break;
      case "C48":
        TutorialSystem.CheckModule((Func<bool>) (() => true), "C49", true);
        break;
      case "C50":
        TutorialSystem.MarkModuleComplete("C50");
        break;
    }
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    EventDelegate.Set(this.buttonHelp.onClick, new EventDelegate.Callback(this.WorldHelp));
    EventDelegate.Set(this.GraphExpand.onClick, new EventDelegate.Callback(this.GraphExpandClick));
    EventDelegate.Set(this.CureExpand.onClick, new EventDelegate.Callback(this.CureExpandClick));
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    this.GraphWholeExpand.onDoubleClick = this.onButtonGraphExpandDoubleClick;
    this.CureWholeExpand.onDoubleClick = this.onButtonCureExpandDoubleClick;
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  public bool CheckCureTutorialRestrictions() => TutorialSystem.GetActiveModuleName() == "C48";

  public static string NormalizeMoneyString(double money, int digitCountAfterDot)
  {
    int num1 = 0;
    double num2 = money;
    if (money >= 10000000.0)
    {
      num1 = 2;
      num2 = money / 1000000.0;
    }
    else if (money >= 10000.0)
    {
      num1 = 1;
      num2 = money / 1000.0;
    }
    string str = "";
    switch (num1)
    {
      case 1:
        str = "K";
        break;
      case 2:
        str = "M";
        break;
    }
    return num2.ToString("N" + digitCountAfterDot.ToString()) + str;
  }
}
