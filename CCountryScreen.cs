// Decompiled with JetBrains decompiler
// Type: CCountryScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CCountryScreen : IGameScreen, ITutorial
{
  [Header("General")]
  public UILabel TitleText;
  public UILabel TagText;
  public UIButton buttonCountryNext;
  public UIButton buttonCountryPrev;
  public UIButton buttonMap;
  public DefconObject defcon;
  [Header("Graph Buttons")]
  public UIButton buttonGraph;
  public UIButton buttonCureGraph;
  public UIEventListener buttonGraphFull;
  public UIEventListener buttonCureGraphFull;
  [Header("Map")]
  public List<CountryIcon> mpIcons;
  [Header("Border Status")]
  public List<CountryBorder> mpBorders;
  [Header("Chart")]
  public PieChart Chart;
  public UILabel chartPopulation;
  [Header("Cure")]
  public List<UISprite> cureFlasks;
  public UILabel cureFunding;
  public UILabel cureRank;
  [Header("Stats")]
  public UILabel statsDeadValue;
  public UILabel statsInfectedValue;
  public UILabel statsHealthyValue;
  public UILabel statsZombieValue;
  public GameObject statsZombieObject;
  [Header("Government")]
  public GovernmentActionObject actionsTemplate;
  public GameObject actionsParent;
  public GameObject ciaParent;
  public UILabel ciaTitle;
  public UILabel ciaText;
  public UILabel ciaTextExtended;
  public GameObject mapPlus;
  public GameObject mapMinus;
  public List<GovernmentActionObject> mpActions;
  public UIDraggablePanel dragPanel;
  public UIGrid contentGrid;
  protected Country mpCountry;
  protected Disease mpDisease;
  protected LocalDisease mpLocalDisease;
  private UIEventListener.VoidDelegate onButtonGraphFullDoubleClick;
  private UIEventListener.VoidDelegate onButtonCureGraphFullDoubleClick;
  [Header("Infection sprites")]
  public UISprite healthyStatSprite;
  public UISprite deadStatSprite;
  public UISprite infectedStatSprite;
  public Material DeadMaterial;
  public Material HealthyMaterial;
  public Material InfectedMaterial;
  public Material UnawareMaterial;
  public Material ZombieMaterial;
  private bool FakeNewsMaterials;
  public GameObject mapPlusToolTip;
  public GameObject mapMinusToolTip;

  public Country Country => this.mpCountry;

  public LocalDisease LocalDisease => this.mpLocalDisease;

  public void SetHealthySprite(string name) => this.healthyStatSprite.spriteName = name;

  public void SetDeadSprite(string name) => this.deadStatSprite.spriteName = name;

  public void SetInfectedSprite(string name) => this.infectedStatSprite.spriteName = name;

  public override void Initialise()
  {
    base.Initialise();
    EventDelegate.Set(this.buttonCountryNext.onClick, new EventDelegate.Callback(this.CountryNext));
    EventDelegate.Set(this.buttonCountryPrev.onClick, new EventDelegate.Callback(this.CountryPrev));
    EventDelegate.Set(this.buttonMap.onClick, new EventDelegate.Callback(this.ToggleCIA));
    EventDelegate.Set(this.buttonGraph.onClick, new EventDelegate.Callback(this.GraphClick));
    EventDelegate.Set(this.buttonCureGraph.onClick, new EventDelegate.Callback(this.CureGraphClick));
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    CActionManager.instance.AddListener("INPUT_LEFT", (Action<CActionManager.ActionType>) (a =>
    {
      if (a != CActionManager.ActionType.START)
        return;
      this.CountryPrev();
    }), this.gameObject);
    CActionManager.instance.AddListener("INPUT_RIGHT", (Action<CActionManager.ActionType>) (a =>
    {
      if (a != CActionManager.ActionType.START)
        return;
      this.CountryNext();
    }), this.gameObject);
    this.onButtonGraphFullDoubleClick = (UIEventListener.VoidDelegate) (_ => this.GraphClick());
    this.onButtonCureGraphFullDoubleClick = (UIEventListener.VoidDelegate) (_ => this.CureGraphClick());
    this.buttonGraphFull.onDoubleClick += this.onButtonGraphFullDoubleClick;
    this.buttonCureGraphFull.onDoubleClick += this.onButtonCureGraphFullDoubleClick;
    TutorialSystem.RegisterInterface((ITutorial) this);
  }

  public void Update()
  {
  }

  public override float Alpha
  {
    set => base.Alpha = value;
  }

  public override void Setup()
  {
    if (this.mpActions.Count < 8)
    {
      for (int index = 0; index < 8; ++index)
      {
        GovernmentActionObject component = NGUITools.AddChild(this.actionsParent.gameObject, this.actionsTemplate.gameObject).GetComponent<GovernmentActionObject>();
        component.name = index.ToString() + "Action";
        this.mpActions.Add(component);
      }
    }
    else
    {
      for (int index = 0; index < this.mpActions.Count; ++index)
      {
        if (index < 8)
          this.mpActions[index].SetAction("", "", false);
        else
          this.mpActions[index].gameObject.SetActive(false);
      }
    }
  }

  public override void SetActive(bool isActive)
  {
    base.SetActive(isActive);
    CountryStateCamera instance = CountryStateCamera.instance;
    if (isActive)
    {
      if ((UnityEngine.Object) CHUDScreen.instance != (UnityEngine.Object) null)
        CHUDScreen.instance.AutoCloseChat();
      this.contentGrid.Reposition();
      this.ciaParent.SetActive(false);
      this.mapPlus.SetActive(true);
      this.mapMinus.SetActive(false);
      if (CGameManager.IsTutorialGame)
      {
        TutorialSystem.SetLocalisedDictionary(new string[3]
        {
          "18B",
          "18C",
          "18D"
        }, new Dictionary<string, string>()
        {
          {
            "country",
            CLocalisationManager.GetText(this.Country.name)
          },
          {
            "disease",
            World.instance.GetDisease(0).name
          }
        });
        TutorialSystem.MarkModuleComplete("17A");
        TutorialSystem.CheckModule((Func<bool>) (() => true), "18A", true);
        CGameManager.SetPaused(true, true);
      }
      if ((UnityEngine.Object) instance != (UnityEngine.Object) null && (UnityEngine.Object) instance.baseImage != (UnityEngine.Object) null && (UnityEngine.Object) instance.baseImage.transform.parent != (UnityEngine.Object) null)
      {
        instance.baseImage.transform.parent.localPosition = new Vector3(instance.currentCountryScreenOffset.x, instance.currentCountryScreenOffset.y, 1f);
        instance.baseImage.transform.parent.localScale = new Vector3(instance.currentCountryScreenScale.x, instance.currentCountryScreenScale.y, 1f);
      }
    }
    else
    {
      if (CGameManager.IsTutorialGame && TutorialSystem.IsModuleComplete("18F"))
        CGameManager.SetPaused(false, true);
      if ((UnityEngine.Object) instance != (UnityEngine.Object) null && (UnityEngine.Object) instance.baseImage != (UnityEngine.Object) null && (UnityEngine.Object) instance.baseImage.transform.parent != (UnityEngine.Object) null)
      {
        instance.baseImage.transform.parent.localPosition = new Vector3(instance.currentHudScreenOffset.x, instance.currentHudScreenOffset.y, 1f);
        instance.baseImage.transform.parent.localScale = new Vector3(instance.currentHudScreenScale.x, instance.currentHudScreenScale.y, 1f);
      }
    }
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
        this.FakeNewsMaterials = false;
        this.SetHealthySprite("Hex_Healthy");
        this.SetDeadSprite("Hex_Dead");
        this.SetInfectedSprite("Hex_Infected");
        this.Chart.SetSprites("Dead", "Healthy", "Infected", "zz_zombie_icon");
        this.Chart.SetMaterials(this.DeadMaterial, this.HealthyMaterial, this.InfectedMaterial, this.ZombieMaterial);
      }
      this.Refresh();
    }
    instance.countryScreenIsActive = isActive;
    if (!((UnityEngine.Object) CountryStateCamera.instance != (UnityEngine.Object) null))
      return;
    CountryStateCamera.instance.animateImage = isActive;
  }

  public override void Refresh()
  {
    base.Refresh();
    this.RefreshIcons();
    this.SetCureStatus();
    this.SetChart();
    this.SetStats();
    this.SetActions();
    this.TagText.text = this.GetLocalStatus();
    this.TagText.color = this.GetLocalStatusColor();
    this.defcon.SetDefconState(CGameManager.GetDefconState(this.mpLocalDisease.localPriority), this.mpLocalDisease.localPriority);
  }

  public void SetCountry(Country country, Disease disease)
  {
    this.SetCountry(CInterfaceManager.instance.GetCountryView(country.id), disease);
  }

  public virtual void SetCountry(CountryView c, Disease d)
  {
    this.mpCountry = c.GetCountry();
    this.mpDisease = d;
    this.mpLocalDisease = this.mpCountry.GetLocalDisease(d);
    this.SetCountryTitle();
    this.TagText.text = CGameManager.GetDefconText(this.mpDisease.zdayDone, this.mpCountry.zombiePercent, this.mpCountry.publicOrder, this.mpCountry.isDestroyed);
    Vector3 position = c.transform.position;
    Vector3 size = c.GetComponent<MeshFilter>().mesh.bounds.size;
    float num1 = Mathf.Clamp((float) (((double) size.x - 0.800000011920929) / 17.299999237060547 * -12.0 - 3.5), -15.5f, -3.5f);
    float num2 = Mathf.Clamp((float) (((double) size.z - 1.5) / 12.300000190734863 * -12.0 - 3.5), -15.5f, -3.5f);
    CountryCamera.instance.transform.position = new Vector3(position.x, position.y, (double) num1 < (double) num2 ? num1 : num2);
    this.RefreshIcons();
    WorldMapController.instance.SetSelectedCountry(c);
    CInterfaceManager.instance.UpdateCountryState();
    this.SetCIAInfo();
  }

  public virtual void SetCountryTitle()
  {
    this.TitleText.text = CLocalisationManager.GetText(this.mpCountry.name);
  }

  public void RefreshIcons()
  {
    CountryIcon.SetIcons(this.mpIcons, this.mpCountry);
    foreach (CountryBorder mpBorder in this.mpBorders)
      mpBorder.Country = this.mpCountry;
  }

  public virtual void SetChart()
  {
    this.Chart.SetFactors(new float[4]
    {
      (float) this.mpCountry.totalHealthy,
      (float) this.mpLocalDisease.infectedPopulation,
      (float) this.mpCountry.totalDead,
      (float) this.mpLocalDisease.zombie
    });
    this.chartPopulation.text = "\n" + this.FormatPopulation(this.mpCountry.originalPopulation) + "\nRes: " + ((SPLocalDisease) this.mpLocalDisease).GetExternalResistance().ToString("N3");
    this.chartPopulation.height = 300;
    this.chartPopulation.width = 800;
  }

  public void SetCureStatus()
  {
    int num1 = this.mpDisease.GetCureContributors().IndexOf(this.mpCountry) + 1;
    if (num1 >= 0)
    {
      int num2 = num1 % 10;
      if (num1 > 10 && num1 < 20)
        num2 = 4;
      string str;
      switch (num2)
      {
        case 1:
          str = "st";
          break;
        case 2:
          str = "nd";
          break;
        case 3:
          str = "rd";
          break;
        default:
          str = "th";
          break;
      }
      this.cureRank.text = num1.ToString() + str;
    }
    else
      this.cureRank.text = "-";
    UILabel cureFunding1 = this.cureFunding;
    string str1 = this.mpLocalDisease.localCureResearch.ToString("N2");
    float num3 = this.Country.medicalBudget;
    string str2 = num3.ToString("N2");
    string str3 = "$" + str1 + "/ $" + str2;
    cureFunding1.text = str3;
    if (CGameManager.IsMultiplayerGame)
    {
      LocalDisease localDisease = (LocalDisease) null;
      for (int index = 0; index < this.Country.localDiseases.Count; ++index)
      {
        if (this.Country.localDiseases[index] != this.mpLocalDisease)
          localDisease = this.Country.localDiseases[index];
      }
      float num4 = localDisease == null ? 0.0f : localDisease.localCureResearch;
      UILabel cureFunding2 = this.cureFunding;
      num3 = this.mpLocalDisease.localCureResearch;
      string str4 = "$" + num3.ToString("N2") + "/ $" + num4.ToString("N2");
      cureFunding2.text = str4;
      float num5 = (float) ((double) this.Country.medicalBudget / 1000.0 * (double) Mathf.Max(0.7f, (float) (this.mpLocalDisease.controlledInfected - this.mpLocalDisease.uncontrolledInfected) / (float) this.Country.originalPopulation, (float) (localDisease.controlledInfected - localDisease.uncontrolledInfected) / (float) this.Country.originalPopulation) * (double) Mathf.Max(0.3f, this.Country.govPublicOrder) / 0.30000001192092896);
      Color color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
      float num6 = Mathf.Max(0.0f, (float) ((double) byte.MaxValue - 12.5 * (double) num5));
      color.r = (float) byte.MaxValue;
      color.g = num6;
      color.b = num6;
      this.cureRank.color = color;
      this.cureRank.color = new Color(1f, num6 / (float) byte.MaxValue, num6 / (float) byte.MaxValue);
      this.cureRank.text = "Flasks: " + num5.ToString("N2") + "/" + this.cureRank.text;
    }
    float num7 = (float) (this.mpLocalDisease.flaskBroken + this.mpLocalDisease.flaskResearched + this.mpLocalDisease.flaskEmpty);
    int num8 = this.mpLocalDisease.flaskResearched + this.mpLocalDisease.flaskEmpty;
    for (int index = 0; index < this.cureFlasks.Count; ++index)
    {
      UISprite cureFlask = this.cureFlasks[index];
      if ((double) index < (double) num7)
      {
        cureFlask.gameObject.SetActive(true);
        string str5 = index >= this.mpLocalDisease.flaskResearched ? (index >= num8 ? "CureBeaker_Destroyed" : "CureBeaker_Potential") : "CureBeaker_Active";
        if (str5 != cureFlask.spriteName)
        {
          cureFlask.spriteName = str5;
          cureFlask.MarkAsChanged();
        }
      }
      else
        cureFlask.gameObject.SetActive(false);
    }
  }

  public string FormatPopulation(long pop) => pop.ToString("N0");

  public virtual void SetStats()
  {
    if (World.instance.diseases.Count > 1)
    {
      this.statsHealthyValue.text = this.FormatPopulation(this.mpCountry.totalHealthy);
      this.statsInfectedValue.text = this.FormatPopulation(this.mpCountry.totalInfected) + " (" + this.FormatPopulation(this.mpLocalDisease.infectedPopulation) + ")";
      this.statsDeadValue.text = this.FormatPopulation(this.mpCountry.deadPopulation) + " (" + this.FormatPopulation(this.mpLocalDisease.killedPopulation) + ")";
    }
    else
    {
      this.statsHealthyValue.height = 200;
      this.statsInfectedValue.height = 200;
      this.statsDeadValue.height = 200;
      this.statsHealthyValue.width = 800;
      this.statsInfectedValue.width = 800;
      this.statsDeadValue.width = 800;
      string str1 = "传染 " + this.mpLocalDisease.localInfectiousness.ToString("N3") + " (" + this.mpCountry.govLocalInfectiousness.ToString("N3") + ")";
      string str2 = "严重 " + this.mpLocalDisease.localSeverity.ToString("N3") + " (" + this.mpCountry.govLocalSeverity.ToString("N3") + ")";
      string str3 = "致死 " + this.mpLocalDisease.localLethality.ToString("N3") + " (" + this.mpCountry.govLocalLethality.ToString("N3") + ")";
      this.statsHealthyValue.text = "\n" + this.FormatPopulation(this.mpCountry.totalHealthy) + "\n" + str1;
      this.statsInfectedValue.text = "\n" + this.FormatPopulation(this.mpLocalDisease.infectedPopulation) + "\n" + str2;
      this.statsDeadValue.text = "\n" + this.FormatPopulation(this.mpCountry.deadPopulation) + "\n" + str3;
    }
    if (this.mpDisease.zdayOrDone || this.mpDisease.vday || this.mpDisease.vdayDone)
    {
      this.statsZombieValue.text = this.mpLocalDisease.zombie.ToString();
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

  public void SetActions()
  {
    bool flag = false;
    int index1 = 0;
    List<Country.GovernmentActionEvent> govActionEvents = this.mpCountry.GetGovActionEvents();
    for (int index2 = govActionEvents.Count - 1; index2 >= 0; --index2)
    {
      Country.GovernmentActionEvent governmentActionEvent = govActionEvents[index2];
      GovernmentAction action = World.instance.governmentActionManager.FindAction(governmentActionEvent.id, this.mpDisease);
      if (action != null)
      {
        string text = this.mpCountry.GetActionText(action, this.mpDisease, governmentActionEvent.removed);
        if (!string.IsNullOrEmpty(text) && text != "0")
        {
          GovernmentActionObject governmentActionObject;
          if (index1 < this.mpActions.Count)
          {
            governmentActionObject = this.mpActions[index1];
            governmentActionObject.gameObject.SetActive(true);
          }
          else
          {
            governmentActionObject = NGUITools.AddChild(this.actionsParent.gameObject, this.actionsTemplate.gameObject).GetComponent<GovernmentActionObject>();
            governmentActionObject.name = index1.ToString() + "Action";
            this.mpActions.Add(governmentActionObject);
            flag = true;
          }
          string date = governmentActionEvent.turn.ToString() + " Day";
          int importance = 0;
          if ((double) action.changeResearchAllocation > 0.0)
            ++importance;
          if ((double) action.changePublicOrder > 0.0)
          {
            importance += 2;
            text = text + " +" + action.changePublicOrder.ToString("N2");
          }
          if ((double) action.changePublicOrder < 0.0)
          {
            importance -= 2;
            text = text + " " + action.changePublicOrder.ToString("N2");
          }
          governmentActionObject.SetAction(text, date, governmentActionEvent.removed, importance);
          ++index1;
        }
      }
      else
        Debug.LogError((object) ("Unknown local government action in " + this.mpCountry.id + ": '" + governmentActionEvent.id + "'"));
    }
    for (int index3 = index1; index3 < this.mpActions.Count; ++index3)
    {
      if (index3 < 8)
        this.mpActions[index3].SetAction("", "", false);
      else
        this.mpActions[index3].gameObject.SetActive(false);
    }
    if (!flag)
      return;
    this.contentGrid.Reposition();
    this.dragPanel.ResetPosition();
  }

  private void CountryNext()
  {
    int index = World.instance.countries.IndexOf(this.mpCountry) + 1;
    if (index >= World.instance.countries.Count)
      index = 0;
    Country country = World.instance.countries[index];
    Disease disease = CGameManager.localPlayerInfo.disease;
    this.SetCountry(CInterfaceManager.instance.GetCountryView(country.id), disease);
    this.Refresh();
  }

  private void CountryPrev()
  {
    int index = World.instance.countries.IndexOf(this.mpCountry) - 1;
    if (index < 0)
      index = World.instance.countries.Count - 1;
    Country country = World.instance.countries[index];
    Disease disease = CGameManager.localPlayerInfo.disease;
    this.SetCountry(CInterfaceManager.instance.GetCountryView(country.id), disease);
    this.Refresh();
  }

  private void GraphClick() => CUIManager.instance.OpenGraphScreen("Population");

  private void CureGraphClick() => CUIManager.instance.OpenGraphScreen("Cure");

  private void ToggleCIA()
  {
    this.ciaParent.SetActive(!this.ciaParent.activeSelf);
    this.mapPlus.SetActive(!this.ciaParent.activeSelf);
    this.mapMinus.SetActive(this.ciaParent.activeSelf);
    if (COptionsManager.instance.InterfaceType == EInterfaceType.Full)
    {
      this.mapPlusToolTip.SetActive(!this.ciaParent.activeSelf);
      this.mapMinusToolTip.SetActive(this.ciaParent.activeSelf);
    }
    if (!CGameManager.IsTutorialGame || !TutorialSystem.IsModuleActive("18G"))
      return;
    PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
    instance.StartCoroutine(instance.UpdateTutorial());
  }

  private void SetCIAInfo()
  {
    if (this.mpCountry.countryDescription != "0")
      this.ciaText.text = CLocalisationManager.GetText(this.mpCountry.countryDescription);
    else
      this.GenerateCIADescription();
    this.ciaTextExtended.text = CLocalisationManager.GetText(this.mpCountry.countryDescriptionExtended);
  }

  private void GenerateCIADescription()
  {
    Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
    dictionary.Add("Hot", this.mpCountry.hot);
    dictionary.Add("Cold", this.mpCountry.cold);
    dictionary.Add("Humid", this.mpCountry.humid);
    dictionary.Add("Arid", this.mpCountry.arid);
    dictionary.Add("Rich", this.mpCountry.wealthy);
    dictionary.Add("Poor", this.mpCountry.poverty);
    dictionary.Add("Urban", this.mpCountry.urban);
    dictionary.Add("Rural", this.mpCountry.rural);
    this.ciaText.text = CLocalisationManager.GetText("Country Report") + ": ";
    foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
    {
      if (keyValuePair.Value)
      {
        UILabel ciaText = this.ciaText;
        ciaText.text = ciaText.text + CLocalisationManager.GetText(keyValuePair.Key) + ". ";
      }
    }
  }

  private void GoBack(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    CUIManager.instance.SetActiveScreen("HUDScreen");
  }

  private void GoToDisease(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    CUIManager.instance.SetActiveScreen("DiseaseScreen");
  }

  private void GoToWorld(CActionManager.ActionType type)
  {
    if (type != CActionManager.ActionType.START)
      return;
    CUIManager.instance.SetActiveScreen("WorldScreen");
  }

  public void OnTutorialBegin(Module withModule)
  {
    switch (withModule.Name)
    {
      case "18A":
        EventDelegate.Remove(this.buttonCountryNext.onClick, new EventDelegate.Callback(this.CountryNext));
        EventDelegate.Remove(this.buttonCountryPrev.onClick, new EventDelegate.Callback(this.CountryPrev));
        EventDelegate.Remove(this.buttonMap.onClick, new EventDelegate.Callback(this.ToggleCIA));
        EventDelegate.Remove(this.buttonGraph.onClick, new EventDelegate.Callback(this.GraphClick));
        EventDelegate.Remove(this.buttonCureGraph.onClick, new EventDelegate.Callback(this.CureGraphClick));
        CActionManager.instance.RemoveListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
        CActionManager.instance.RemoveListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
        this.buttonGraphFull.onDoubleClick -= this.onButtonGraphFullDoubleClick;
        this.buttonCureGraphFull.onDoubleClick -= this.onButtonCureGraphFullDoubleClick;
        break;
      case "18E":
        EventDelegate.Set(this.buttonMap.onClick, new EventDelegate.Callback(this.ToggleCIA));
        break;
    }
  }

  public void OnTutorialComplete(Module completedModule)
  {
    if (!(completedModule.Name == "18F"))
      return;
    EventDelegate.Set(this.buttonCountryNext.onClick, new EventDelegate.Callback(this.CountryNext));
    EventDelegate.Set(this.buttonCountryPrev.onClick, new EventDelegate.Callback(this.CountryPrev));
    EventDelegate.Set(this.buttonMap.onClick, new EventDelegate.Callback(this.ToggleCIA));
    EventDelegate.Set(this.buttonGraph.onClick, new EventDelegate.Callback(this.GraphClick));
    EventDelegate.Set(this.buttonCureGraph.onClick, new EventDelegate.Callback(this.CureGraphClick));
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    this.buttonGraphFull.onDoubleClick = this.onButtonGraphFullDoubleClick;
    this.buttonCureGraphFull.onDoubleClick = this.onButtonCureGraphFullDoubleClick;
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
    EventDelegate.Set(this.buttonCountryNext.onClick, new EventDelegate.Callback(this.CountryNext));
    EventDelegate.Set(this.buttonCountryPrev.onClick, new EventDelegate.Callback(this.CountryPrev));
    EventDelegate.Set(this.buttonMap.onClick, new EventDelegate.Callback(this.ToggleCIA));
    EventDelegate.Set(this.buttonGraph.onClick, new EventDelegate.Callback(this.GraphClick));
    EventDelegate.Set(this.buttonCureGraph.onClick, new EventDelegate.Callback(this.CureGraphClick));
    CActionManager.instance.AddListener("INPUT_GOBACK", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_EXIT", new Action<CActionManager.ActionType>(this.GoBack), this.gameObject);
    CActionManager.instance.AddListener("INPUT_DISEASE", new Action<CActionManager.ActionType>(this.GoToDisease), this.gameObject);
    CActionManager.instance.AddListener("INPUT_WORLD", new Action<CActionManager.ActionType>(this.GoToWorld), this.gameObject);
    this.buttonGraphFull.onDoubleClick = this.onButtonGraphFullDoubleClick;
    this.buttonCureGraphFull.onDoubleClick = this.onButtonCureGraphFullDoubleClick;
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }

  private string GetLocalStatus()
  {
    float publicOrder = this.mpCountry.publicOrder;
    string str1 = (double) publicOrder < 1.3999999761581421 ? ((double) publicOrder < 1.1499999761581421 ? ((double) publicOrder < 1.0 ? ((double) publicOrder < 0.800000011920929 ? ((double) publicOrder < 0.5 ? ((double) publicOrder < 0.25 ? ((double) publicOrder <= 0.0 ? "Anarchy" : "Total Disorder") : "Great Disorder") : "Widespread Disorder") : "Slightly Disorder") : "Good Order") : "Very Good Order") : "Extremely Good Order";
    string str2 = this.mpCountry.publicOrder.ToString("N2");
    if ((double) this.mpCountry.publicOrder <= 0.0)
      str2 = "0.00";
    string str3 = this.mpCountry.govPublicOrder.ToString("N2");
    if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.Contains("Reconstruction"))
      return "Support: " + (this.GetLocalResistance() * World.instance.diseases[0].globalInfectiousnessMax).ToString("N1") + "  Economy: " + (this.mpCountry.publicOrder * 100f).ToString("N1");
    return str1 + " " + str2 + "(" + str3 + ")";
  }

  private Color GetLocalStatusColor()
  {
    return new Color()
    {
      r = Mathf.Clamp(this.mpCountry.publicOrder, 0.0f, 1f),
      g = Mathf.Clamp(1.5f - this.mpCountry.publicOrder, 0.0f, 1f),
      b = 0.0f,
      a = 1f
    };
  }

  private float GetLocalResistance()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    Country mpCountry = this.mpCountry;
    Disease mpDisease = this.mpDisease;
    int num3 = CGameManager.IsMultiplayerGame ? 1 : 0;
    if (mpDisease.diseaseType == Disease.EDiseaseType.VAMPIRE)
    {
      if (mpCountry.wealthy)
      {
        num1 += 37f;
        num2 += 37f * mpDisease.wealthy;
      }
      if (mpCountry.poverty)
      {
        num1 += 4f;
        num2 += 4f * mpDisease.poverty;
      }
      if (mpCountry.hot)
      {
        num1 += 12f;
        num2 += 12f * mpDisease.hot;
      }
      if (mpCountry.cold)
      {
        num1 += 12f;
        num2 += 12f * mpDisease.cold;
      }
      if (mpCountry.urban)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.urban;
      }
      if (mpCountry.rural)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.rural;
      }
      if (mpCountry.humid)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.humid;
      }
      if (mpCountry.arid)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.arid;
      }
    }
    else if (mpDisease.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      if (mpCountry.wealthy)
      {
        num1 += 6f;
        num2 += 6f * mpDisease.wealthy;
      }
      if (mpCountry.poverty)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.poverty;
      }
      if (mpCountry.hot)
      {
        num1 += 8f;
        num2 += 8f * mpDisease.hot;
      }
      if (mpCountry.cold)
      {
        num1 += 10f;
        num2 += 10f * mpDisease.cold;
      }
      if (mpCountry.urban)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.urban;
      }
      if (mpCountry.rural)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.rural;
      }
      if (mpCountry.humid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.humid;
      }
      if (mpCountry.arid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.arid;
      }
    }
    else if (CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.Contains("Reconstruction"))
    {
      if (mpCountry.wealthy)
      {
        num1 += 10f;
        num2 += 10f * mpDisease.wealthy;
      }
      if (mpCountry.poverty)
      {
        num1 += 10f;
        num2 += 10f * mpDisease.poverty;
      }
      if (mpCountry.hot)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.hot;
      }
      if (mpCountry.cold)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.cold;
      }
      if (mpCountry.urban)
      {
        num1 += 6f;
        num2 += 6f * mpDisease.urban;
      }
      if (mpCountry.rural)
      {
        num1 += 6f;
        num2 += 6f * mpDisease.rural;
      }
      if (mpCountry.humid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.humid;
      }
      if (mpCountry.arid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.arid;
      }
    }
    else
    {
      if (mpCountry.wealthy)
      {
        num1 += 30f;
        num2 += 30f * mpDisease.wealthy;
      }
      if (mpCountry.poverty)
      {
        num1 += 3f;
        num2 += 3f * mpDisease.poverty;
      }
      if (mpCountry.hot)
      {
        num1 += 10f;
        num2 += 10f * mpDisease.hot;
      }
      if (mpCountry.cold)
      {
        num1 += 10f;
        num2 += 10f * mpDisease.cold;
      }
      if (mpCountry.urban)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.urban;
      }
      if (mpCountry.rural)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.rural;
      }
      if (mpCountry.humid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.humid;
      }
      if (mpCountry.arid)
      {
        num1 += 2f;
        num2 += 2f * mpDisease.arid;
      }
    }
    return (double) num1 != 0.0 ? num2 / num1 : 1f;
  }
}
