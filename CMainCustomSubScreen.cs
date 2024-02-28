using AurochDigital;
using AurochDigital.UGC;
using SimpleJSON;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class CMainCustomSubScreen : CMainSubScreen
{
  [Header("Navigation table elements")]
  public UICustomScenarioTableElement customScenarioObjectPrefab;
  public UITable customScenarioTable;
  private UICustomScenarioTableElement selectedScenarioElement;
  public UIButton navigationTablePreviousButton;
  public UIButton navigationTableNextButton;
  public UILabel fromTableCellValueLabel;
  public UILabel toTableCellValueLabel;
  public UILabel totalTableCellsValueLabel;
  public UITable tabTable;
  public UIToggle featuredTabToggle;
  public UIToggle allTabToggle;
  public UIToggle subscribedTabToggle;
  public UIToggle localfilesTabToggle;
  public UIToggle newFilesTabToggle;
  public UIButton useEditorTabButton;
  public UIButton subscribeButton;
  public UILabel subscribePlayButtonLabel;
  public UIButton unsubscribeButton;
  public UIButton moreInformationButton;
  public UIButton reportBugButton;
  public UIButton showMoreFromAuthorButton;
  public UIToggle settingsToggle;
  public GameObject localScenarioHeader;
  public GameObject onlineScenarioHeader;
  public UISlider scenarioDownloadProgressBar;
  public GameObject scenarioDownloadProgressContainer;
  public GameObject[] scenarioDetailViews;
  public GameObject localTabHolder;
  public GameObject scenarioCreatorTabHolder;
  [Header("Recent, Subscribed, Refresh buttons/toggles")]
  public UIDropdownPopupList categoryDropdown;
  public UIDropdownPopupList languageTagDropdown;
  public UIButton refreshTableButton;
  public Transform dropDownSlot1Transform;
  public Transform dropDownSlot2Transform;
  [Header("Refresh Panel")]
  public GameObject refreshPanelRoot;
  public GameObject refreshingObject;
  public GameObject noResultsFoundObject;
  public GameObject featuredNoResults;
  public UIInput searchInputField;
  public UIButton clearSearchTextButton;
  public UIScrollBar scenarioListScrollBar;
  private List<UICustomScenarioTableElement> scenarioButtons;
  private IDictionary<ulong, int> cachedVotes;
  private UIScenarioDetails selectedScenarioDetails;
  public int numberOfScenarioCellsPerPage = 50;
  private int totalScenarioCells;
  private int fromScenarioTableCellValue;
  private int toScenarioTableCellValue;
  private bool showFeatured;
  private bool showSubscribed;
  private bool showAll;
  private bool showNew;
  private bool showLocal;
  private bool isLoading;
  private string currentSearch = string.Empty;
  private string currentCategory = "All Categories";
  private uint currentPage;
  private int pageSize = 50;
  private int ndemicPageSize = 20;
  private int currentIndex;
  public GameObject settingsListPanel;
  public UIGrid settingsGrid;
  private bool openSettings;
  private ulong selectedTableCellPublishID;
  private ulong previewDownloadID;
  private SteamUGCHandler steamUGCHandler;
  private const EUGCQuery defaultAllSort = EUGCQuery.k_EUGCQuery_RankedByVote;
  private CSLocalUGCHandler localUGCHandler;
  private ScenarioInformation selectedScenarioInformation;
  private string selectedLanguage;
  private static IList<ulong> featuredIDs;
  private static IList<ulong> ndemicFeaturedIDs;
  private HashSet<long> ndemicFavouriteIds;
  private HashSet<ulong> recentIds;
  private List<CustomScenarioMetadata> ndemicMetadata;
  private int ndemicMetadataTotal;
  private int ndemicMetadataPage;
  private int currentScenarioLength;
  private int selectedIndex;
  private Coroutine downloadFeaturedCoroutine;
  private Coroutine refreshLocalCoroutine;
  private Coroutine ndemicCoroutine;
  private int connectionRequestsMade;
  private static string[] blockedWords = new string[16]
  {
    "COVID",
    "CoV",
    "武汉",
    "wuhan",
    "冠状",
    "新冠",
    "武漢",
    "中國",
    "中国",
    "Wuhan",
    "Covid",
    "covid",
    "武",
    "汉",
    "冠",
    "武"
  };
  private string federalScenarioMetaInfo;
  private List<string> federalScenarioNames;
  private string federalScenarioPath;
  private List<string> localScenario;
  private string federalLegacyPath;
  private string federalScenarioConstListPath;
  private string federalScenarioUnlockOrderPath;
  private string federalScenarioAuthorPath;
  private string federalScenarioUnlockPotentialPath;
  private string federalScenarioNamePath;

  private CustomScenarioMetadata ndemicSelectedMetadata
  {
    get
    {
      return this.selectedIndex != -1 ? this.ndemicMetadata[this.selectedIndex] : (CustomScenarioMetadata) null;
    }
  }

  public override void Initialise()
  {
    base.Initialise();
    CGameManager.UpdateScenarioInfo();
    this.fromTableCellValueLabel.transform.position -= new Vector3(0.4f, 0.0f, 0.0f);
    CGameManager.FilterScenarioComplete = "all";
    CGameManager.FilterScenarioLevel = "all";
    CGameManager.FilterScenarioType = "all";
    CGameManager.scenarioSort = "level";
    CGameManager.scenarioShow = "level";
    this.cachedVotes = (IDictionary<ulong, int>) new Dictionary<ulong, int>();
    this.scenarioButtons = new List<UICustomScenarioTableElement>();
    CMainCustomSubScreen.featuredIDs = (IList<ulong>) new List<ulong>();
    CMainCustomSubScreen.ndemicFeaturedIDs = (IList<ulong>) new List<ulong>();
    this.ndemicFavouriteIds = new HashSet<long>();
    this.recentIds = new HashSet<ulong>();
    this.ndemicMetadata = new List<CustomScenarioMetadata>();
    this.selectedLanguage = string.Empty;
    this.ndemicMetadataTotal = 0;
    EventDelegate.Set(this.unsubscribeButton.onClick, new EventDelegate.Callback(this.OnUnsubscribeButtonClick));
    EventDelegate.Set(this.moreInformationButton.onClick, new EventDelegate.Callback(this.OnMoreInformationButtonClick));
    EventDelegate.Set(this.reportBugButton.onClick, new EventDelegate.Callback(this.OnReportScenarioButtonClick));
    EventDelegate.Set(this.showMoreFromAuthorButton.onClick, new EventDelegate.Callback(this.OnShowMoreFromAuthorButtonClick));
    EventDelegate.Set(this.subscribeButton.onClick, new EventDelegate.Callback(this.OnSubscribePlayButtonClick));
    EventDelegate.Add(this.settingsToggle.onChange, new EventDelegate.Callback(this.OnSettingsToggleChange));
    EventDelegate.Set(this.searchInputField.onSubmit, new EventDelegate.Callback(this.OnSearchInputFieldValueChange));
    EventDelegate.Set(this.clearSearchTextButton.onClick, new EventDelegate.Callback(this.OnClearSearch));
    EventDelegate.Set(this.useEditorTabButton.onClick, new EventDelegate.Callback(this.OnUseEditorTabButtonClick));
    string localCacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Cache" + Path.DirectorySeparatorChar.ToString();
    this.steamUGCHandler = new SteamUGCHandler();
    this.steamUGCHandler.Initialise(localCacheDirectory);
    this.localUGCHandler = new CSLocalUGCHandler();
    CGameManager.FilterScenarioComplete = "all";
    CGameManager.FilterScenarioLevel = "all";
    CGameManager.FilterScenarioType = "all";
    CGameManager.scenarioSort = "level";
    CGameManager.scenarioShow = "level";
    CGameManager.showConst = false;
    this.connectionRequestsMade = 1;
    this.localScenario = new List<string>();
    this.AddLocalScenario();
    this.federalScenarioPath = CGameManager.federalServerAddress + "ScenarioConfig/AutoScenarios.txt";
    this.federalScenarioConstListPath = CGameManager.federalServerAddress + "ScenarioConfig/ConstList.txt";
    this.federalScenarioUnlockOrderPath = CGameManager.federalServerAddress + "ScenarioConfig/UnlockOrder.txt";
    this.federalLegacyPath = CGameManager.federalServerAddress + "ScenarioConfig/LegacyScenarios.txt";
    this.federalScenarioAuthorPath = CGameManager.federalServerAddress + "ScenarioConfig/ScenarioAuthor.txt";
    this.federalScenarioUnlockPotentialPath = CGameManager.federalServerAddress + "ScenarioConfig/UnlockPotential.txt";
    this.federalScenarioNamePath = CGameManager.federalServerAddress + "ScenarioConfig/ScenarioName.txt";
    this.StartCoroutine(this.GetFederalLegacyList());
    this.InstallFederalScenario();
    this.localUGCHandler.CacheLocalScenarioData();
    this.categoryDropdown.Clear();
    this.categoryDropdown.AddListElements(string.Empty, CSCategoryTags.categoryTags);
    this.categoryDropdown.SetByValue("All Categories");
    EventDelegate.Set(this.categoryDropdown.onSelect, new EventDelegate.Callback(this.OnCategoryDropdownChange));
    this.languageTagDropdown.AddElement("All Languages", value: "All Languages");
    foreach (string language in CLocalisationManager.GetLanguages())
      this.languageTagDropdown.AddElement(language, value: language);
    this.languageTagDropdown.SetByValue("All Languages");
    EventDelegate.Set(this.languageTagDropdown.onSelect, new EventDelegate.Callback(this.OnLanguageDropdownChange));
    this.scenarioDetailViews[1].GetComponent<UIScenarioDetails>().Initialize();
    CActionManager.instance.AddListener("SC_INPUT_TAB_LEFT", new Action<CActionManager.ActionType>(this.TabLeft), this.gameObject, -1);
    CActionManager.instance.AddListener("SC_INPUT_TAB_RIGHT", new Action<CActionManager.ActionType>(this.TabRight), this.gameObject, -1);
    this.ndemicFavouriteIds = CustomScenarioCache.GetCachedIDs();
    CGameManager.currentPotential = CGameManager.GetPotential();
    CGameManager.currentScore = CGameManager.GetScenarioLibraryScore();
  }

  public override void SetActive(bool state)
  {
    base.SetActive(state);
    CGameManager.UpdateScenarioInfo();
    CGameManager.FilterScenarioComplete = "all";
    CGameManager.FilterScenarioLevel = "all";
    CGameManager.FilterScenarioType = "all";
    CGameManager.scenarioSort = "level";
    CGameManager.scenarioShow = "level";
    CGameManager.showConst = false;
    this.connectionRequestsMade = 1;
    this.localScenario = new List<string>();
    this.AddLocalScenario();
    this.federalScenarioPath = CGameManager.federalServerAddress + "ScenarioConfig/AutoScenarios.txt";
    this.federalScenarioConstListPath = CGameManager.federalServerAddress + "ScenarioConfig/ConstList.txt";
    this.federalScenarioUnlockOrderPath = CGameManager.federalServerAddress + "ScenarioConfig/UnlockOrder.txt";
    this.federalLegacyPath = CGameManager.federalServerAddress + "ScenarioConfig/LegacyScenarios.txt";
    this.federalScenarioAuthorPath = CGameManager.federalServerAddress + "ScenarioConfig/ScenarioAuthor.txt";
    this.federalScenarioUnlockPotentialPath = CGameManager.federalServerAddress + "ScenarioConfig/UnlockPotential.txt";
    this.federalScenarioNamePath = CGameManager.federalServerAddress + "ScenarioConfig/ScenarioName.txt";
    this.StartCoroutine(this.GetFederalLegacyList());
    this.InstallFederalScenario();
    if (CGameManager.returnFromScenario)
    {
      CGameManager.returnFromScenario = false;
      this.localUGCHandler.CacheLocalScenarioData();
    }
    if (state)
    {
      if (this.downloadFeaturedCoroutine != null)
        this.StopCoroutine(this.downloadFeaturedCoroutine);
      this.downloadFeaturedCoroutine = this.StartCoroutine(this.DownloadFeaturedItems());
      this.subscribeButton.gameObject.SetActive(false);
      this.settingsListPanel.SetActive(false);
      this.scenarioDetailViews[0].SetActive(true);
      this.scenarioDetailViews[1].SetActive(false);
      this.showFeatured = true;
      this.showAll = false;
      this.showNew = false;
      this.showSubscribed = false;
      this.showLocal = false;
      this.currentPage = 1U;
      this.SetTabToggleState(this.showFeatured, this.showAll, this.showNew, this.showSubscribed, this.showLocal);
      this.isLoading = true;
      this.SetRefreshPanelState(this.isLoading, false);
      this.selectedScenarioElement = (UICustomScenarioTableElement) null;
      this.selectedTableCellPublishID = 0UL;
      this.selectedScenarioInformation = (ScenarioInformation) null;
      this.languageTagDropdown.SetByValue("All Languages");
      this.categoryDropdown.SetByValue("All Categories");
      this.steamUGCHandler.onUGCQueryError = new SteamUGCHandler.OnUGCQueryError(this.OnUGCQueryError);
      this.steamUGCHandler.onUGCSteamDetailsQueryCompleted = new SteamUGCHandler.OnUGCSteamDetailsQueryCompleted(this.OnUGCSteamDetailsQueryCompleted);
      this.searchInputField.label.text = CLocalisationManager.GetText("FE_Search");
      if (this.refreshLocalCoroutine != null)
        this.StopCoroutine(this.refreshLocalCoroutine);
      this.refreshLocalCoroutine = this.StartCoroutine(this.RefreshLocalScenarioData());
    }
    else
    {
      this.steamUGCHandler.onUGCSteamDetailsQueryCompleted = (SteamUGCHandler.OnUGCSteamDetailsQueryCompleted) null;
      if (this.refreshLocalCoroutine != null)
        this.StopCoroutine(this.refreshLocalCoroutine);
      if (this.downloadFeaturedCoroutine == null)
        return;
      this.StopCoroutine(this.downloadFeaturedCoroutine);
    }
  }

  public void OnSettingsToggleChange()
  {
    if (!UIToggle.current.value)
      return;
    this.openSettings = true;
    this.moreInformationButton.gameObject.SetActive(false);
    this.showMoreFromAuthorButton.gameObject.SetActive(this.showAll);
    this.unsubscribeButton.gameObject.SetActive(this.selectedScenarioElement.isSubscribed);
    this.settingsGrid.repositionNow = true;
    this.settingsGrid.Reposition();
  }

  public void OnCategoryDropdownChange()
  {
    this.currentCategory = UIDropdownPopupList.current.value;
    if (!this.showAll && !this.showNew)
      return;
    this.currentPage = 1U;
    this.UpdateSearchResults();
  }

  public void OnLanguageDropdownChange()
  {
    this.selectedLanguage = UIDropdownPopupList.current.value;
    if (!this.showAll && !this.showNew)
      return;
    this.currentPage = 1U;
    this.UpdateSearchResults();
  }

  public void Update()
  {
    if (CGameManager.scenarioListGot && !CGameManager.prepareDone)
      this.InstallFederalScenario();
    if ((UnityEngine.Object) this.settingsToggle != (UnityEngine.Object) null && this.settingsToggle.value && (Input.GetMouseButtonUp(0) || CSteamControllerManager.instance.GetActionUp(CSteamControllerManager.instance.currentSelectAction)) && !this.openSettings)
      this.settingsToggle.value = false;
    if (this.previewDownloadID > 0UL && (bool) (UnityEngine.Object) this.scenarioDownloadProgressBar)
    {
      float? nullable = this.steamUGCHandler.ItemDownloadProgress(this.previewDownloadID);
      this.scenarioDownloadProgressBar.value = nullable.HasValue ? nullable.Value : 0.0f;
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      int count = this.localUGCHandler.cachedScenariosInformation.Count;
      if (!CGameManager.FilterScenarioComplete.Equals("all") || !CGameManager.FilterScenarioLevel.Equals("all") || !CGameManager.FilterScenarioType.Equals("all"))
        count = this.localUGCHandler.subsetScenarios.Count;
      if ((long) this.currentPage == (count % this.pageSize != 0 ? (long) (count / this.pageSize + 1) : (long) (count / this.pageSize)))
        return;
      this.OnNextNavigationButtonClick();
    }
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      if (this.currentPage == 1U)
        return;
      this.OnPreviousNavigationButtonClick();
    }
    if (this.searchInputField.isSelected && Input.GetKeyUp(KeyCode.Backspace))
      this.OnSearchEscape();
    if (this.searchInputField.isSelected)
      return;
    if (Input.GetKeyDown(KeyCode.F))
    {
      switch (CGameManager.FilterScenarioComplete)
      {
        case "all":
          CGameManager.FilterScenarioComplete = "completed";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Complete set to Completed");
          break;
        case "completed":
          CGameManager.FilterScenarioComplete = "not completed";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Complete set to Not Completed");
          break;
        case "not completed":
          CGameManager.FilterScenarioComplete = "all";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Complete set to All");
          break;
      }
      this.OnLittleRefresh();
    }
    if (Input.GetKeyDown(KeyCode.G))
    {
      switch (CGameManager.FilterScenarioLevel)
      {
        case "all":
          CGameManager.FilterScenarioLevel = "1";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 1");
          break;
        case "1":
          CGameManager.FilterScenarioLevel = "2";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 2");
          break;
        case "2":
          CGameManager.FilterScenarioLevel = "3";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 3");
          break;
        case "3":
          CGameManager.FilterScenarioLevel = "4";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 4");
          break;
        case "4":
          CGameManager.FilterScenarioLevel = "5";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 5");
          break;
        case "5":
          CGameManager.FilterScenarioLevel = "6";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 6");
          break;
        case "6":
          CGameManager.FilterScenarioLevel = "7";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 7");
          break;
        case "7":
          CGameManager.FilterScenarioLevel = "8";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 8");
          break;
        case "8":
          CGameManager.FilterScenarioLevel = "9";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 9");
          break;
        case "9":
          CGameManager.FilterScenarioLevel = "9+";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 9+");
          break;
        case "9+":
          CGameManager.FilterScenarioLevel = "10";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 10");
          break;
        case "10":
          CGameManager.FilterScenarioLevel = "10+";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 10+");
          break;
        case "10+":
          CGameManager.FilterScenarioLevel = "11";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 11");
          break;
        case "11":
          CGameManager.FilterScenarioLevel = "11+";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 11+");
          break;
        case "11+":
          CGameManager.FilterScenarioLevel = "12";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 12");
          break;
        case "12":
          CGameManager.FilterScenarioLevel = "12+";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 12+");
          break;
        case "12+":
          CGameManager.FilterScenarioLevel = "13";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to 13");
          break;
        case "13":
          CGameManager.FilterScenarioLevel = "all";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Level set to all");
          break;
      }
      this.OnLittleRefresh();
    }
    if (Input.GetKeyDown(KeyCode.H))
    {
      switch (CGameManager.FilterScenarioType)
      {
        case "all":
          CGameManager.FilterScenarioType = "PST";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to Past");
          break;
        case "PST":
          CGameManager.FilterScenarioType = "PRS";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to Present");
          break;
        case "PRS":
          CGameManager.FilterScenarioType = "FTR";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to Future");
          break;
        case "FTR":
          CGameManager.FilterScenarioType = "BYD";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to Beyond");
          break;
        case "BYD":
          CGameManager.FilterScenarioType = "MXM";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to MXM");
          break;
        case "MXM":
          CGameManager.FilterScenarioType = "all";
          UnityEngine.Debug.Log((object) "Scenario Library Filter.Type set to All");
          break;
      }
      this.OnLittleRefresh();
    }
    if (Input.GetKeyDown(KeyCode.J))
    {
      CGameManager.FilterScenarioComplete = "all";
      CGameManager.FilterScenarioLevel = "all";
      CGameManager.FilterScenarioType = "all";
      CGameManager.scenarioSort = "level";
      CGameManager.scenarioShow = "level";
      CGameManager.showConst = false;
      CGameManager.sortByScore = false;
      UnityEngine.Debug.Log((object) "Scenario Library Filter Setting and Const Setting set to Default");
      this.OnRefreshTableViewButtonClick();
    }
    if (Input.GetKeyDown(KeyCode.I))
    {
      CGameManager.InvertSortType = !CGameManager.InvertSortType;
      UnityEngine.Debug.Log((object) ("Scenario Library Sort Invert set to " + CGameManager.InvertSortType.ToString()));
      this.OnLittleRefresh();
    }
    if (Input.GetKeyDown(KeyCode.K))
    {
      switch (CGameManager.scenarioShow)
      {
        case "level":
          CGameManager.scenarioShow = "const";
          CGameManager.scenarioSort = "const";
          UnityEngine.Debug.Log((object) "Scenario Library Show Mode set to Constant");
          break;
        case "const":
          CGameManager.scenarioShow = "rating";
          CGameManager.scenarioSort = "rating";
          UnityEngine.Debug.Log((object) "Scenario Library Show Mode set to Rating");
          break;
        case "rating":
          CGameManager.scenarioShow = "pack";
          CGameManager.scenarioSort = "pack";
          UnityEngine.Debug.Log((object) "Scenario Library Show Mode set to Pack");
          break;
        case "pack":
          CGameManager.scenarioShow = "date";
          CGameManager.scenarioSort = "date";
          UnityEngine.Debug.Log((object) "Scenario Library Show Mode set to Date");
          break;
        case "date":
          CGameManager.scenarioShow = "level";
          CGameManager.scenarioSort = "level";
          UnityEngine.Debug.Log((object) "Scenario Library Show Mode set to Level");
          break;
      }
      this.OnLittleRefresh();
    }
    if (!Input.GetKeyDown(KeyCode.L))
      return;
    switch (CGameManager.scenarioSort)
    {
      case "level":
        CGameManager.scenarioSort = "const";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Constant");
        break;
      case "const":
        CGameManager.scenarioSort = "rating";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Rating");
        break;
      case "rating":
        CGameManager.scenarioSort = "score";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Score");
        break;
      case "score":
        CGameManager.scenarioSort = "name";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Name");
        break;
      case "name":
        CGameManager.scenarioSort = "pack";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Pack");
        break;
      case "pack":
        CGameManager.scenarioSort = "date";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Date");
        break;
      case "date":
        CGameManager.scenarioSort = "level";
        UnityEngine.Debug.Log((object) "Scenario Library Sort Mode set to Level");
        break;
    }
    this.OnLittleRefresh();
  }

  private void LateUpdate() => this.openSettings = false;

  private void OnUGCQueryError(string error)
  {
    this.isLoading = false;
    if (this.showLocal)
      return;
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
    {
      UnityEngine.Debug.LogError((object) ("UGC QUERY ERROR WHILST GAME IN PROGRESS, SUPPRESSING POPUP: " + error));
    }
    else
    {
      CConfirmOverlay redConfirmOverlay = CUIManager.instance.redConfirmOverlay;
      redConfirmOverlay.ShowLocalised("UGC_Steam_Error_Title", "UGC_Steam_Error_Text", buttonB: "OK");
      redConfirmOverlay.Body = redConfirmOverlay.Body.Replace("%error", error);
    }
  }

  private void OnUGCSteamDetailsQueryCompleted(
    ulong[] publishedFileIDs,
    string[] fileTitles,
    string[] fileDescriptions,
    uint[] fileVoteUps,
    uint[] fileVoteDowns,
    float[] fileScores,
    bool[] fileSubscriptionStatuses,
    uint totalResults)
  {
    if (this.connectionRequestsMade > 1)
      --this.connectionRequestsMade;
    this.isLoading = false;
    int connectionRequestsMade = this.connectionRequestsMade;
    if (this.showLocal)
      return;
    this.StartCoroutine(this.DisplaySteamResults(publishedFileIDs, fileTitles, fileDescriptions, fileVoteUps, fileVoteDowns, fileScores, fileSubscriptionStatuses, totalResults));
  }

  private IEnumerator DisplaySteamResults(
    ulong[] publishedFileIDs,
    string[] fileTitles,
    string[] fileDescriptions,
    uint[] fileVoteUps,
    uint[] fileVoteDowns,
    float[] fileScores,
    bool[] fileSubscriptionStatuses,
    uint totalResults)
  {
    CMainCustomSubScreen cmainCustomSubScreen = this;
    if (cmainCustomSubScreen.ndemicCoroutine != null)
      yield return (object) cmainCustomSubScreen.ndemicCoroutine;
    bool resultsFound = publishedFileIDs.Length >= 1;
    cmainCustomSubScreen.SetRefreshPanelState(cmainCustomSubScreen.isLoading, resultsFound);
    int index1 = 0;
    while (index1 < publishedFileIDs.Length || cmainCustomSubScreen.currentScenarioLength < cmainCustomSubScreen.scenarioButtons.Count)
    {
      bool flag = false;
      for (int index2 = 0; index2 < CMainCustomSubScreen.blockedWords.Length; ++index2)
      {
        if (fileTitles[index1].Normalize().Contains(CMainCustomSubScreen.blockedWords[index2].Normalize()))
          flag = true;
        if (fileDescriptions[index1].Normalize().Contains(CMainCustomSubScreen.blockedWords[index2].Normalize()))
          flag = true;
      }
      if (!flag)
      {
        if (index1 >= publishedFileIDs.Length)
        {
          UICustomScenarioTableElement scenarioButton = cmainCustomSubScreen.scenarioButtons[cmainCustomSubScreen.currentScenarioLength];
          UICustomScenarioTableElement.PoolObject(scenarioButton);
          EventDelegate.Remove(scenarioButton.button.onClick, new EventDelegate.Callback(cmainCustomSubScreen.OnScenarioElementSelect));
          cmainCustomSubScreen.scenarioButtons.RemoveAt(cmainCustomSubScreen.currentScenarioLength);
          --index1;
          goto label_24;
        }
        else if (!cmainCustomSubScreen.recentIds.Contains(publishedFileIDs[index1]))
        {
          cmainCustomSubScreen.recentIds.Add(publishedFileIDs[index1]);
          UICustomScenarioTableElement to;
          if (cmainCustomSubScreen.currentScenarioLength >= cmainCustomSubScreen.scenarioButtons.Count)
          {
            to = UICustomScenarioTableElement.CreateObject(cmainCustomSubScreen.customScenarioObjectPrefab);
            to.name = "Friend_" + (object) index1;
            to.transform.parent = cmainCustomSubScreen.customScenarioTable.transform;
            to.transform.localPosition = Vector3.zero;
            to.transform.localScale = Vector3.one;
            CControllerCursorAnchor componentInChildren = to.GetComponentInChildren<CControllerCursorAnchor>();
            if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
              componentInChildren.panel = NGUITools.FindInParents<UIPanel>(to.gameObject);
            cmainCustomSubScreen.scenarioButtons.Add(to);
          }
          else
            to = cmainCustomSubScreen.scenarioButtons[cmainCustomSubScreen.currentScenarioLength];
          EventDelegate.Set(to.button.onClick, new EventDelegate.Callback(cmainCustomSubScreen.OnScenarioElementSelect));
          int rating = -1;
          if (!cmainCustomSubScreen.showLocal && fileScores != null && fileVoteUps[index1] + fileVoteDowns[index1] > 25U)
            rating = Mathf.CeilToInt(fileScores[index1] * 5f);
          to.PopulateWithData(publishedFileIDs[index1], fileTitles[index1], rating, fileSubscriptionStatuses[index1], false, false, string.Empty);
          cmainCustomSubScreen.steamUGCHandler.DownloadPreviewFile(publishedFileIDs[index1], 0U, (SteamUGCHandler.OnUGCPreviewFileDownloadComplete) ((downloadedData, forPublishFileID) =>
          {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(downloadedData);
            if ((long) to.publishedFileID != (long) forPublishFileID)
              return;
            to.ThumbnailTexture = (Texture) tex;
            if (!to.gameObject.activeSelf)
              return;
            to.StartCoroutine(to.FadeThumbNail());
          }));
          if (fileScores == null && cmainCustomSubScreen.showFeatured)
          {
            cmainCustomSubScreen.steamUGCHandler.GetUGCPublishedItemVoteAndScoreDetails(publishedFileIDs[index1], (SteamUGCHandler.OnUGCItemVoteAndScoreDetailsDownloaded) ((publishID, voteUps, voteDowns, reports, score) =>
            {
              if ((long) to.publishedFileID != (long) publishID || voteUps + voteDowns <= 25)
                return;
              rating = Mathf.CeilToInt(score * 5f);
              to.SetRating(rating);
            }));
            goto label_24;
          }
          else
            goto label_24;
        }
label_23:
        ++index1;
        continue;
label_24:
        ++cmainCustomSubScreen.currentScenarioLength;
        goto label_23;
      }
      else
        ++index1;
    }
    cmainCustomSubScreen.customScenarioTable.repositionNow = true;
    cmainCustomSubScreen.customScenarioTable.Reposition();
    cmainCustomSubScreen.UpdatePaging((int) totalResults + cmainCustomSubScreen.ndemicMetadataTotal, publishedFileIDs.Length + cmainCustomSubScreen.ndemicMetadataPage);
  }

  private void UpdatePaging(int total, int downloaded)
  {
    if (total == 0)
    {
      this.fromTableCellValueLabel.text = "[Potential: " + (CGameManager.currentPotential - 0.0049995).ToString("N2") + " Score: " + CGameManager.currentScore.ToString("N0").Replace(',', '\'') + "] 0";
      this.toTableCellValueLabel.text = "0";
      this.totalTableCellsValueLabel.text = "0 " + this.GetSortString();
      this.totalTableCellsValueLabel.width = 2000;
      this.navigationTableNextButton.gameObject.SetActive(false);
      this.navigationTablePreviousButton.gameObject.SetActive(false);
    }
    else
    {
      this.navigationTableNextButton.gameObject.SetActive((ulong) (this.currentPage - 1U) < (ulong) (total / this.pageSize));
      this.navigationTablePreviousButton.gameObject.SetActive(this.currentPage > 1U);
      int num1 = this.pageSize + this.ndemicPageSize;
      int num2 = (int) ((long) (this.currentPage - 1U) * (long) num1 + 1L);
      int num3 = Mathf.CeilToInt((float) (total - this.ndemicMetadataTotal) * 1f / (float) this.pageSize);
      int num4 = Mathf.CeilToInt((float) this.ndemicMetadataTotal * 1f / (float) this.ndemicPageSize);
      if ((ulong) this.currentPage > (ulong) num4)
      {
        num1 = this.pageSize;
        num2 = 1 + num4 * this.ndemicPageSize + (int) ((long) (this.currentPage - 1U) * (long) this.pageSize);
      }
      else if ((ulong) this.currentPage > (ulong) num3)
      {
        num1 = this.ndemicPageSize;
        num2 = 1 + num3 * this.pageSize + (int) ((long) (this.currentPage - 1U) * (long) this.ndemicPageSize);
      }
      int num5 = Mathf.Min(new int[3]
      {
        num2 + (downloaded - 1),
        num2 + num1 - 1,
        total
      });
      this.fromTableCellValueLabel.text = "[Potential: " + (CGameManager.currentPotential - 0.0049995).ToString("N2") + " Score: " + CGameManager.currentScore.ToString("N0").Replace(',', '\'') + "] " + num2.ToString();
      this.toTableCellValueLabel.text = num5.ToString();
      this.totalTableCellsValueLabel.text = total.ToString() + " " + this.GetSortString();
      this.totalTableCellsValueLabel.width = 2000;
    }
  }

  public void OnPreviousNavigationButtonClick()
  {
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    this.scenarioListScrollBar.value = 0.0f;
    --this.currentPage;
    this.UpdateSearchResults();
    ++this.connectionRequestsMade;
  }

  public void OnNextNavigationButtonClick()
  {
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    this.scenarioListScrollBar.value = 0.0f;
    ++this.currentPage;
    this.UpdateSearchResults();
    ++this.connectionRequestsMade;
  }

  public void OnRefreshTableViewButtonClick()
  {
    CGameManager.UpdateScenarioInfo();
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    this.scenarioListScrollBar.value = 0.0f;
    if (!this.showLocal)
    {
      if (this.ndemicCoroutine != null)
        this.StopCoroutine(this.ndemicCoroutine);
      this.ndemicCoroutine = this.StartCoroutine(this.CoIssueQuery(false));
      if (this.showSubscribed)
        this.steamUGCHandler.QueryFilteredUGCContent(EUserUGCList.k_EUserUGCList_Subscribed, EUserUGCListSortOrder.k_EUserUGCListSortOrder_SubscriptionDateDesc, string.Empty, pageNumber: this.currentPage);
      else if (this.showFeatured)
        this.steamUGCHandler.QuerySelectedUGCContent(CMainCustomSubScreen.featuredIDs);
      else if (this.showNew)
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByTrend;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        if (tagTexts.Count > 0)
          this.currentPage = 1U;
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, this.currentSearch, tagTexts, false, this.currentPage, 7U);
      }
      else
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByVote;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        if (tagTexts.Count > 0)
          this.currentPage = 1U;
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, string.Empty, tagTexts, false, this.currentPage);
      }
    }
    else if (this.localUGCHandler != null && this.localUGCHandler.CacheLocalScenarioData())
    {
      this.currentIndex = 0;
      this.currentPage = 1U;
      this.UpdatePaging(this.localUGCHandler.cachedScenariosInformation.Count, this.localUGCHandler.cachedScenariosInformation.Count);
      this.SetScenarioButtons((IList<ScenarioInformation>) this.localUGCHandler.cachedScenariosInformation.GetRange(this.currentIndex, Mathf.Clamp(this.localUGCHandler.cachedScenariosInformation.Count - this.currentIndex, 0, this.pageSize)));
      this.SetRefreshPanelState(false, true);
    }
    else
    {
      this.currentIndex = 0;
      this.currentPage = 1U;
      this.UpdatePaging(this.localUGCHandler.cachedScenariosInformation.Count, this.localUGCHandler.cachedScenariosInformation.Count);
      this.SetScenarioButtons((IList<ScenarioInformation>) this.localUGCHandler.cachedScenariosInformation.GetRange(this.currentIndex, Mathf.Clamp(this.localUGCHandler.cachedScenariosInformation.Count - this.currentIndex, 0, this.pageSize)));
      this.SetRefreshPanelState(false, true);
    }
  }

  public void OnTabClick()
  {
    for (int index = 0; index < this.scenarioButtons.Count; ++index)
    {
      UICustomScenarioTableElement scenarioButton = this.scenarioButtons[index];
      UICustomScenarioTableElement.PoolObject(scenarioButton);
      EventDelegate.Remove(scenarioButton.button.onClick, new EventDelegate.Callback(this.OnScenarioElementSelect));
      this.scenarioButtons.RemoveAt(index);
    }
    this.SetTabToggleState(this.featuredTabToggle.value, this.allTabToggle.value, this.newFilesTabToggle.value, this.subscribedTabToggle.value, this.localfilesTabToggle.value);
    if (!this.localfilesTabToggle.value)
      ++this.connectionRequestsMade;
    else
      this.connectionRequestsMade = 0;
    this.scenarioDetailViews[0].SetActive(true);
    this.scenarioDetailViews[1].SetActive(false);
    this.selectedScenarioElement = (UICustomScenarioTableElement) null;
    this.selectedTableCellPublishID = 0UL;
    this.selectedScenarioInformation = (ScenarioInformation) null;
    this.currentPage = 1U;
    this.UpdateSearchResults();
  }

  private void UpdateSearchResults()
  {
    this.currentScenarioLength = 0;
    this.recentIds.Clear();
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    if (!this.showLocal)
    {
      if (this.ndemicCoroutine != null)
        this.StopCoroutine(this.ndemicCoroutine);
      this.ndemicCoroutine = this.StartCoroutine(this.CoIssueQuery(false));
      if (this.showSubscribed)
        this.steamUGCHandler.QueryFilteredUGCContent(EUserUGCList.k_EUserUGCList_Subscribed, EUserUGCListSortOrder.k_EUserUGCListSortOrder_SubscriptionDateDesc, string.Empty, pageNumber: this.currentPage);
      else if (this.showFeatured)
        this.steamUGCHandler.QuerySelectedUGCContent(CMainCustomSubScreen.featuredIDs);
      else if (this.showNew)
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByTrend;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, this.currentSearch, tagTexts, false, this.currentPage, 7U);
      }
      else
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByVote;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, this.currentSearch, tagTexts, false, this.currentPage);
      }
    }
    else
    {
      this.localUGCHandler.GetScenarioSubset();
      if (this.localUGCHandler != null && this.localUGCHandler.subsetScenarios != null && this.localUGCHandler.subsetScenarios.Count > 0)
      {
        this.currentIndex = (int) ((long) (this.currentPage - 1U) * (long) this.pageSize);
        List<ScenarioInformation> subsetScenarios = this.localUGCHandler.subsetScenarios;
        this.UpdatePaging(subsetScenarios.Count, subsetScenarios.Count);
        int count = subsetScenarios.Count;
        this.SetScenarioButtons((IList<ScenarioInformation>) subsetScenarios.GetRange(this.currentIndex, Mathf.Clamp(count - this.currentIndex, 0, this.pageSize)));
        this.isLoading = false;
        this.SetRefreshPanelState(this.isLoading, true);
      }
      else
      {
        this.isLoading = false;
        this.SetRefreshPanelState(this.isLoading, false);
      }
    }
  }

  private void SetScenarioButtons(IList<ScenarioInformation> scenarios)
  {
    int index1 = 0;
    while (index1 < scenarios.Count || index1 < this.scenarioButtons.Count)
    {
      if (index1 >= scenarios.Count)
      {
        UICustomScenarioTableElement scenarioButton = this.scenarioButtons[index1];
        UICustomScenarioTableElement.PoolObject(scenarioButton);
        EventDelegate.Remove(scenarioButton.button.onClick, new EventDelegate.Callback(this.OnScenarioElementSelect));
        this.scenarioButtons.RemoveAt(index1);
        --index1;
      }
      else
      {
        bool flag = false;
        for (int index2 = 0; index2 < CMainCustomSubScreen.blockedWords.Length && !this.showLocal; ++index2)
        {
          if (scenarios[index1].scenTitle.Normalize().Contains(CMainCustomSubScreen.blockedWords[index2].Normalize()))
            flag = true;
          if (scenarios[index1].scenDescription.Normalize().Contains(CMainCustomSubScreen.blockedWords[index2].Normalize()))
            flag = true;
        }
        if (flag && this.scenarioButtons.Count >= 114514)
        {
          ++index1;
          continue;
        }
        UICustomScenarioTableElement scenarioButton;
        if (index1 >= this.scenarioButtons.Count)
        {
          scenarioButton = UICustomScenarioTableElement.CreateObject(this.customScenarioObjectPrefab);
          scenarioButton.name = "CustomScenario_" + (object) index1;
          scenarioButton.transform.parent = this.customScenarioTable.transform;
          scenarioButton.transform.localPosition = Vector3.zero;
          scenarioButton.transform.localScale = Vector3.one;
          this.scenarioButtons.Add(scenarioButton);
        }
        else
          scenarioButton = this.scenarioButtons[index1];
        EventDelegate.Set(scenarioButton.button.onClick, new EventDelegate.Callback(this.OnScenarioElementSelect));
        int rating = -1;
        scenarioButton.PopulateWithData(Convert.ToUInt64(scenarios[index1].id), scenarios[index1].scenTitle, rating, true, false, this.showLocal, scenarios[index1].fileName, scenarios[index1]);
        scenarioButton.StopAllCoroutines();
      }
      ++index1;
    }
    this.customScenarioTable.transform.DetachChildren();
    for (int index3 = 0; index3 < this.scenarioButtons.Count; ++index3)
      this.scenarioButtons[index3].transform.parent = this.customScenarioTable.transform;
    this.customScenarioTable.repositionNow = true;
    this.customScenarioTable.Reposition();
  }

  private void OnScenarioElementSelect()
  {
    this.selectedScenarioElement = UIButton.current.GetComponent<UICustomScenarioTableElement>();
    if (this.selectedScenarioElement.metadata != null)
    {
      if ((long) this.selectedTableCellPublishID == this.selectedScenarioElement.metadata.Id)
        return;
      this.selectedScenarioInformation = (ScenarioInformation) null;
      this.selectedTableCellPublishID = (ulong) this.selectedScenarioElement.metadata.Id;
      CustomScenarioMetadata metadata = this.selectedScenarioElement.metadata;
      this.selectedScenarioDetails = this.scenarioDetailViews[1].GetComponent<UIScenarioDetails>();
      this.selectedScenarioDetails.isSubscribed = this.ndemicFavouriteIds.Contains(metadata.Id);
      this.selectedScenarioDetails.publishID = (ulong) metadata.Id;
      Texture2D texture2D = new Texture2D(1, 1);
      texture2D.LoadImage(CustomScenarioCache.GetIcon(metadata.IconUrl));
      this.scenarioDetailViews[0].SetActive(false);
      this.scenarioDetailViews[1].SetActive(true);
      this.selectedScenarioDetails.SetState(false);
      this.selectedScenarioDetails.SetScenarioDetails(metadata.Title, metadata.Description, "", metadata.DateUpdated.ToShortDateString(), (Texture) texture2D, false, true, (IList<string>) metadata.Tags);
      this.selectedScenarioDetails.SetAuthorName(metadata.Author);
      this.SetSubscribePlayButtonState(isSubscribed: this.selectedScenarioDetails.isSubscribed);
    }
    else
    {
      if ((long) this.selectedTableCellPublishID == (long) this.selectedScenarioElement.publishedFileID && this.selectedScenarioInformation == this.selectedScenarioElement.scenarioInformation)
        return;
      this.selectedScenarioInformation = this.selectedScenarioElement.scenarioInformation;
      this.selectedTableCellPublishID = this.selectedScenarioElement.publishedFileID;
      this.scenarioDetailViews[0].SetActive(false);
      this.scenarioDetailViews[1].SetActive(true);
      this.selectedScenarioDetails = this.scenarioDetailViews[1].GetComponent<UIScenarioDetails>();
      this.selectedScenarioDetails.publishID = this.selectedTableCellPublishID;
      if (this.selectedScenarioElement.IsLocal())
      {
        this.selectedScenarioDetails.SetState(false);
        this.settingsToggle.gameObject.SetActive(false);
        this.localUGCHandler.GetLocalImageFromFile(this.selectedScenarioElement.GetFilename(), this.selectedScenarioElement.scenarioInformation.scenIcon, new PNGLoader.TextureLoaded(this.OnScenarioIconLoadedLocal));
        this.SetSubscribePlayButtonState();
      }
      else
      {
        this.selectedScenarioDetails.SetState();
        this.selectedScenarioDetails.SetSubscribed(this.selectedScenarioElement.isSubscribed);
        this.selectedScenarioDetails.SetAuthorName(string.Empty);
        int vote = 0;
        this.settingsToggle.gameObject.SetActive(!this.steamUGCHandler.GetCachedUGCFileIsMine(this.selectedTableCellPublishID));
        if (this.cachedVotes.ContainsKey(this.selectedTableCellPublishID))
          vote = this.cachedVotes[this.selectedTableCellPublishID];
        this.previewDownloadID = this.selectedTableCellPublishID;
        this.scenarioDownloadProgressContainer.SetActive(true);
        this.selectedScenarioDetails.SetVote(vote);
        this.steamUGCHandler.GetUGCUserPublishedItemVoteType(this.selectedTableCellPublishID, new SteamUGCHandler.OnUGCItemVoteTypeDownloaded(this.OnFileVoteDetailsDownloaded));
        this.steamUGCHandler.DownloadPreviewFile(this.selectedTableCellPublishID, 0U, new SteamUGCHandler.OnUGCPreviewFileDownloadComplete(this.OnScenarioPreviewDownloadComplete));
        this.steamUGCHandler.GetCachedUGCFileAuthorName(this.selectedTableCellPublishID, new SteamUGCHandler.OnPersonaDetailsDownloaded(this.OnPersonaDetailsDownloaded));
      }
    }
  }

  private void OnPersonaDetailsDownloaded(string authorName, ulong forPublishFileID)
  {
    if (!((UnityEngine.Object) this.selectedScenarioDetails != (UnityEngine.Object) null) || (long) this.selectedScenarioDetails.publishID != (long) forPublishFileID)
      return;
    this.selectedScenarioDetails.SetAuthorName(authorName);
  }

  private void OnFileVoteDetailsDownloaded(int vote, ulong forPublishFileID)
  {
    if (!((UnityEngine.Object) this.selectedScenarioDetails != (UnityEngine.Object) null) || (long) this.selectedScenarioDetails.publishID != (long) forPublishFileID)
      return;
    this.cachedVotes[forPublishFileID] = vote;
    this.selectedScenarioDetails.SetVote(vote);
  }

  private void OnScenarioIconLoadedLocal(string imageName, Texture image)
  {
    if (!((UnityEngine.Object) this.selectedScenarioDetails != (UnityEngine.Object) null) || this.selectedScenarioInformation == null)
      return;
    if (CGameManager.constedScenarioList != null && (CGameManager.constedScenarioList.Contains(this.selectedScenarioInformation.fileName) || this.selectedScenarioInformation.fileName.Contains("时生虫ReCRAFT")))
      this.selectedScenarioDetails.SetScenarioDetails("[" + this.selectedScenarioInformation.finalLevelString + "]" + this.selectedScenarioInformation.originScenTitle, this.selectedScenarioInformation.scenDescription, this.selectedScenarioInformation.scenVersion, this.selectedScenarioInformation.scenCreatedDate, image, true, false);
    else
      this.selectedScenarioDetails.SetScenarioDetails(this.selectedScenarioInformation.scenTitle, this.selectedScenarioInformation.scenDescription, this.selectedScenarioInformation.scenVersion, this.selectedScenarioInformation.scenCreatedDate, image, true, false);
    this.selectedScenarioDetails.SetAuthorName(this.selectedScenarioInformation.scenAuthorName);
  }

  private void OnScenarioPreviewDownloadComplete(byte[] downloadedData, ulong forPublishFileID)
  {
    if ((UnityEngine.Object) this.selectedScenarioDetails != (UnityEngine.Object) null && (long) this.selectedTableCellPublishID == (long) forPublishFileID)
    {
      Texture2D texture2D = new Texture2D(1, 1);
      texture2D.LoadImage(downloadedData);
      this.previewDownloadID = 0UL;
      this.scenarioDownloadProgressContainer.SetActive(false);
      this.selectedScenarioDetails.SetScenarioDetails(this.steamUGCHandler.GetCachedUGCFileTitle(this.selectedTableCellPublishID), this.steamUGCHandler.GetCachedUGCFileDescription(this.selectedTableCellPublishID), string.Empty, string.Empty, (Texture) texture2D, false, false, this.steamUGCHandler.GetCachedUGCTags(this.selectedTableCellPublishID));
      this.selectedScenarioDetails.SetState(false);
      this.SetSubscribePlayButtonState(isSubscribed: this.selectedScenarioDetails.isSubscribed);
    }
    else
      UnityEngine.Debug.LogError((object) "Invalid scenario details therefore not populating details");
  }

  private IEnumerator ResetScrollbar()
  {
    yield return (object) new WaitForEndOfFrame();
    this.scenarioListScrollBar.value = 0.0f;
  }

  private void SetRefreshPanelState(bool isLoading, bool resultsFound)
  {
    UnityEngine.Debug.Log((object) ("REFRESH PANEL STATE: " + isLoading.ToString() + "/" + resultsFound.ToString()));
    this.featuredTabToggle.enabled = true;
    this.allTabToggle.enabled = true;
    this.subscribedTabToggle.enabled = true;
    this.localfilesTabToggle.enabled = true;
    this.newFilesTabToggle.enabled = true;
    if (resultsFound)
    {
      this.refreshPanelRoot.SetActive(false);
      this.noResultsFoundObject.SetActive(false);
      this.refreshingObject.SetActive(false);
      this.featuredNoResults.SetActive(false);
      this.scenarioListScrollBar.value = 0.0f;
      this.StartCoroutine(this.ResetScrollbar());
    }
    else if (isLoading)
    {
      this.refreshPanelRoot.SetActive(true);
      this.refreshingObject.SetActive(true);
      this.noResultsFoundObject.SetActive(false);
      this.featuredNoResults.SetActive(false);
    }
    else
    {
      if (this.showFeatured)
      {
        this.refreshPanelRoot.SetActive(false);
        this.noResultsFoundObject.SetActive(false);
        this.featuredNoResults.SetActive(true);
      }
      else
      {
        this.refreshPanelRoot.SetActive(true);
        this.featuredNoResults.SetActive(false);
        this.noResultsFoundObject.SetActive(true);
      }
      this.refreshingObject.SetActive(false);
    }
  }

  private void TabLeft(CActionManager.ActionType actionType)
  {
    if (this.isLoading || actionType != CActionManager.ActionType.START || CUIManager.instance.redConfirmOverlay.gameObject.activeInHierarchy)
      return;
    if (this.featuredTabToggle.value)
    {
      this.featuredTabToggle.value = false;
      this.localfilesTabToggle.value = true;
    }
    else if (this.allTabToggle.value)
    {
      this.allTabToggle.value = false;
      this.featuredTabToggle.value = true;
    }
    else if (this.newFilesTabToggle.value)
    {
      this.newFilesTabToggle.value = false;
      this.allTabToggle.value = true;
    }
    else if (this.subscribedTabToggle.value)
    {
      this.subscribedTabToggle.value = false;
      this.newFilesTabToggle.value = true;
    }
    else if (this.localfilesTabToggle.value)
    {
      this.localfilesTabToggle.value = false;
      this.subscribedTabToggle.value = true;
    }
    this.OnTabClick();
  }

  private void TabRight(CActionManager.ActionType actionType)
  {
    if (this.isLoading || actionType != CActionManager.ActionType.START || CUIManager.instance.redConfirmOverlay.gameObject.activeInHierarchy)
      return;
    if (this.featuredTabToggle.value)
    {
      this.featuredTabToggle.value = false;
      this.allTabToggle.value = true;
    }
    else if (this.allTabToggle.value)
    {
      this.allTabToggle.value = false;
      this.newFilesTabToggle.value = true;
    }
    else if (this.newFilesTabToggle.value)
    {
      this.newFilesTabToggle.value = false;
      this.subscribedTabToggle.value = true;
    }
    else if (this.subscribedTabToggle.value)
    {
      this.subscribedTabToggle.value = false;
      this.localfilesTabToggle.value = true;
    }
    else if (this.localfilesTabToggle.value)
    {
      this.localfilesTabToggle.value = false;
      this.OnUseEditorTabButtonClick();
    }
    this.OnTabClick();
  }

  private void SetTabToggleState(
    bool featuredTab,
    bool allTab,
    bool newTab,
    bool subscribedTab,
    bool localFilesTab)
  {
    this.featuredTabToggle.value = featuredTab;
    this.allTabToggle.value = allTab;
    this.subscribedTabToggle.value = subscribedTab;
    this.localfilesTabToggle.value = localFilesTab;
    this.newFilesTabToggle.value = newTab;
    this.showFeatured = this.featuredTabToggle.value;
    this.showAll = this.allTabToggle.value;
    this.showNew = this.newFilesTabToggle.value;
    this.showSubscribed = this.subscribedTabToggle.value;
    this.showLocal = this.localfilesTabToggle.value;
    if (this.showLocal)
    {
      UIInput searchInputField = this.searchInputField;
      this.searchInputField.value = "";
      CGameManager.ScenarioSearchKeyword = "";
      searchInputField.onValidate += new UIInput.OnValidate(this.OnSearchTextChange);
    }
    else
      this.searchInputField.onValidate = (UIInput.OnValidate) null;
    if (localFilesTab)
    {
      bool flag = true;
      this.localScenarioHeader.SetActive(flag);
      this.onlineScenarioHeader.SetActive(!flag);
    }
    else
    {
      bool flag = false;
      this.localScenarioHeader.SetActive(flag);
      this.onlineScenarioHeader.SetActive(!flag);
    }
    if (allTab)
    {
      this.searchInputField.transform.parent.gameObject.SetActive(true);
    }
    else
    {
      this.searchInputField.transform.parent.gameObject.SetActive(true);
      this.searchInputField.value = string.Empty;
      this.currentSearch = string.Empty;
    }
    this.categoryDropdown.gameObject.SetActive(allTab | newTab);
    this.languageTagDropdown.gameObject.SetActive(allTab | newTab);
    if (this.languageTagDropdown.gameObject.activeSelf && !this.categoryDropdown.gameObject.activeSelf)
      this.languageTagDropdown.gameObject.transform.parent = this.dropDownSlot1Transform;
    else if (this.languageTagDropdown.gameObject.activeSelf && this.categoryDropdown.gameObject.activeSelf)
    {
      this.categoryDropdown.gameObject.transform.parent = this.dropDownSlot1Transform;
      this.languageTagDropdown.gameObject.transform.parent = this.dropDownSlot2Transform;
    }
    else
    {
      this.categoryDropdown.gameObject.transform.parent = this.dropDownSlot1Transform;
      this.languageTagDropdown.gameObject.transform.parent = this.dropDownSlot2Transform;
    }
    this.categoryDropdown.transform.localPosition = Vector3.zero;
    this.languageTagDropdown.transform.localPosition = Vector3.zero;
    this.tabTable.Reposition();
    this.tabTable.repositionNow = true;
  }

  public void OnUnsubscribeButtonClick()
  {
    if (!((UnityEngine.Object) this.selectedScenarioElement != (UnityEngine.Object) null))
      return;
    if (this.selectedScenarioElement.metadata != null)
      this.UnsubscribeNdemic(this.selectedScenarioElement);
    else
      this.steamUGCHandler.UnsubscribeUGC(this.selectedScenarioElement.publishedFileID, (SteamUGCHandler.OnUGCUnsubscribeComplete) (withPublishedFileID =>
      {
        UnityEngine.Debug.Log((object) ("Unsubscribed: " + (object) withPublishedFileID));
        this.SetSubscribePlayButtonState(isSubscribed: false);
        this.selectedScenarioElement.SetSubscribed(false);
      }));
  }

  public void OnReportScenarioButtonClick()
  {
    string url = "http://steamcommunity.com/sharedfiles/filedetails/?id=" + (object) this.selectedScenarioElement.publishedFileID + "&searchtext=";
    ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null ? CUIManager.instance.standardConfirmOverlay : CUIManager.instance.redConfirmOverlay).ShowLocalised("FE_Open_Custom_Report_Website_Title", "FE_Open_Custom_Report_Website_Text", "No", "Yes", pressB: !SteamUtils.IsOverlayEnabled() ? (CConfirmOverlay.PressDelegate) (() => Application.OpenURL(url)) : (CConfirmOverlay.PressDelegate) (() => SteamFriends.ActivateGameOverlayToWebPage(url)));
    this.settingsListPanel.SetActive(false);
  }

  public void OnMoreInformationButtonClick() => UnityEngine.Debug.Log((object) "More information button click");

  public void OnShowMoreFromAuthorButtonClick()
  {
    UnityEngine.Debug.Log((object) "Show more from author button click");
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    this.steamUGCHandler.QueryFilteredUGCContent(EUserUGCList.k_EUserUGCList_Published, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, string.Empty, pageNumber: this.currentPage, accountID: this.steamUGCHandler.GetCachedUGCFileOwnerSteamID(this.selectedTableCellPublishID));
  }

  public void OnRateUp()
  {
    if (!((UnityEngine.Object) this.selectedScenarioElement != (UnityEngine.Object) null))
      return;
    if (this.selectedScenarioElement.metadata != null)
    {
      CustomScenarioClient.MakeVoteRequest(this.selectedScenarioElement.metadata.Id, 1);
    }
    else
    {
      this.cachedVotes[this.selectedScenarioElement.publishedFileID] = 1;
      this.steamUGCHandler.SetItemVote(this.selectedScenarioElement.publishedFileID, 1, (SteamUGCHandler.OnItemVoteComplete) null);
    }
  }

  public void OnRateDown()
  {
    if (!((UnityEngine.Object) this.selectedScenarioElement != (UnityEngine.Object) null))
      return;
    if (this.selectedScenarioElement.metadata != null)
    {
      CustomScenarioClient.MakeVoteRequest(this.selectedScenarioElement.metadata.Id, -1);
    }
    else
    {
      this.cachedVotes[this.selectedScenarioElement.publishedFileID] = -1;
      this.steamUGCHandler.SetItemVote(this.selectedScenarioElement.publishedFileID, -1, (SteamUGCHandler.OnItemVoteComplete) null);
    }
  }

  public void OnSubscribePlayButtonClick()
  {
    if (!((UnityEngine.Object) this.selectedScenarioElement != (UnityEngine.Object) null))
      return;
    if (this.selectedScenarioElement.IsLocal())
    {
      if (!this.selectedScenarioElement.scenarioInformation.scoreUnlocked)
      {
        string fileName = Path.GetFileName(this.selectedScenarioElement.scenarioInformation.fileName);
        string body = "You need to meet the following conditions to unlock this scenario:";
        if (CGameManager.scenarioUnlockConditionOverride.ContainsKey(fileName) && CGameManager.scenarioUnlockConditionOverride[fileName] != null && CGameManager.scenarioUnlockConditionOverride[fileName].Count != 0)
        {
          foreach (CGameManager.ScenarioUnlockCondition scenarioUnlockCondition in CGameManager.scenarioUnlockConditionOverride[fileName])
          {
            string unlockScenario = scenarioUnlockCondition.unlockScenario;
            int unlockScore = scenarioUnlockCondition.unlockScore;
            int scenarioHighScore = CSLocalUGCHandler.GetScenarioHighScore(unlockScenario);
            string str1 = unlockScore != 1 ? scenarioHighScore.ToString() + "/" + unlockScore.ToString() : "Complete";
            string str2 = !CGameManager.scenarioNameTitlePair.ContainsKey(unlockScenario) ? "[×]? ? ? ? ? ? ? ? ? ?" : (scenarioHighScore >= unlockScore ? "[√]" + str1 + " " + CGameManager.scenarioNameTitlePair[unlockScenario] : "[×]" + str1 + " " + CGameManager.scenarioNameTitlePair[unlockScenario]);
            body = body + "\n" + str2;
          }
        }
        if (CGameManager.scenarioUnlockPotential.ContainsKey(fileName) && CGameManager.scenarioUnlockPotential[fileName] != 0)
        {
          double potential = CGameManager.GetPotential();
          double num = (double) CGameManager.scenarioUnlockPotential[fileName] / 100.0;
          string str3 = ((double) (int) (potential * 100.0) / 100.0).ToString("N2") + "/" + num.ToString("N2");
          string str4 = potential < num ? "[×]" + str3 + " Achieving personal Scenario Potential" : "[√]" + str3 + " Achieving personal Scenario Potential";
          body = body + "\n" + str4;
        }
        CUIManager.instance.standardConfirmOverlay.ShowLocalised("Scenario Locked", body, buttonB: "OK");
      }
      else
      {
        Scenario scenario = this.localUGCHandler.LocalCreateScenario(this.selectedScenarioElement.GetFilename());
        scenario.scenarioInformation.id = this.selectedScenarioElement.GetFilename();
        CGameManager.gameType = IGame.GameType.Custom;
        CGameManager.CreateGame(scenario);
      }
    }
    else if (this.selectedScenarioElement.isSubscribed)
    {
      if (this.selectedScenarioElement.metadata != null)
        this.StartCoroutine(CustomScenarioCache.Load(this.selectedScenarioElement.metadata.Id, (Action<Scenario>) (scen =>
        {
          scen.scenarioInformation.id = this.selectedScenarioElement.publishedFileID.ToString();
          UnityEngine.Debug.Log((object) ("scen subscribed = " + (object) scen));
          CUIManager.instance.HideOverlay("Scenario_Loading_Overlay");
          CGameManager.gameType = IGame.GameType.Custom;
          CGameManager.CreateGame(scen);
        })));
      else
        CGameManager.LoadScenarioRemote(this.steamUGCHandler, this.selectedScenarioElement.publishedFileID, (Action<Scenario>) (scen =>
        {
          scen.scenarioInformation.id = this.selectedScenarioElement.publishedFileID.ToString();
          UnityEngine.Debug.Log((object) ("scen subscribed = " + (object) scen));
          CUIManager.instance.HideOverlay("Scenario_Loading_Overlay");
          CGameManager.gameType = IGame.GameType.Custom;
          CGameManager.CreateGame(scen);
        }));
    }
    else if (this.selectedScenarioElement.metadata != null)
    {
      this.StartCoroutine(this.CoSubscribeNdemic(this.selectedScenarioElement));
    }
    else
    {
      bool isSubscribing = true;
      bool isSubscribed = true;
      UICustomScenarioTableElement subscribingTableElement = this.selectedScenarioElement;
      this.steamUGCHandler.SubscribeUGC(this.selectedScenarioElement.publishedFileID, (SteamUGCHandler.OnUGCSubscribeComplete) (withPublishedFileID =>
      {
        if (!((UnityEngine.Object) this.selectedScenarioElement != (UnityEngine.Object) null) || (long) this.selectedScenarioElement.publishedFileID != (long) withPublishedFileID)
          return;
        isSubscribing = false;
        isSubscribed = true;
        this.SetSubscribePlayButtonState(isSubscribing, isSubscribed);
        subscribingTableElement.SetSubscribed(isSubscribed);
      }));
    }
  }

  public void SetSubscribePlayButtonState(bool isSubscribing = false, bool isSubscribed = true)
  {
    if (isSubscribing)
      this.subscribePlayButtonLabel.text = CLocalisationManager.GetText("FE_Subscribing");
    else if (isSubscribed)
      this.subscribePlayButtonLabel.text = CLocalisationManager.GetText("FE_Play");
    else
      this.subscribePlayButtonLabel.text = CLocalisationManager.GetText("FE_Subscribe");
  }

  public void OnSearchInputFieldValueChange()
  {
    if (this.showLocal)
      return;
    this.currentSearch = this.searchInputField.value;
    if (this.currentSearch != CLocalisationManager.GetText("FE_Search") && !string.IsNullOrEmpty(this.currentSearch))
    {
      this.currentPage = 1U;
      this.UpdateSearchResults();
    }
    else
    {
      this.currentSearch = string.Empty;
      this.currentPage = 1U;
      this.UpdateSearchResults();
    }
  }

  public void OnClearSearch()
  {
    this.searchInputField.value = string.Empty;
    if (this.showLocal)
      this.OnSearchEscape();
    else
      this.OnSearchInputFieldValueChange();
  }

  public void OnUseEditorTabButtonClick()
  {
    CConfirmOverlay.PressDelegate pressA = (CConfirmOverlay.PressDelegate) (() => Analytics.Event("Scenario Creator Launch Cancel"));
    CConfirmOverlay.PressDelegate pressB = (CConfirmOverlay.PressDelegate) (() =>
    {
      try
      {
        Analytics.Event("Scenario Creator Main Game Launch");
        string empty = string.Empty;
        string path2 = "PlagueIncSC.exe";
        Proc proc = ProcessChecker.CheckIfProcessIsRunning(path2.Substring(0, 11));
        string str = Path.Combine("ScenarioCreator", path2);
        if (proc == null)
        {
          new Process()
          {
            StartInfo = {
              FileName = str,
              ErrorDialog = false,
              Arguments = string.Empty
            }
          }.Start();
        }
        else
        {
          Process processById = Process.GetProcessById(proc.pid);
          processById.StartInfo.FileName = str;
          processById.StartInfo.ErrorDialog = false;
          processById.StartInfo.Arguments = string.Empty;
          processById.CloseMainWindow();
          processById.Start();
        }
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.Log((object) ex.Message);
      }
    });
    string empty1 = string.Empty;
    string body = "UI_Scenario_Creator_Info_Text_Windows";
    CUIManager.instance.redConfirmOverlay.ShowLocalised("UI_Scenario_Creator_Info_Title", body, "MP_Cancel", "UI_Launch", pressA, pressB);
  }

  private IEnumerator DownloadFeaturedItems()
  {
    UnityWebRequest www = UnityWebRequest.Get("http://s-cdn.ndemiccreations.com/plague/steam_featured_scenarios.txt");
    yield return (object) www.SendWebRequest();
    if (www.isDone && string.IsNullOrEmpty(www.error))
    {
      CMainCustomSubScreen.featuredIDs.Clear();
      string text = www.downloadHandler.text;
      char[] chArray = new char[1]{ '\n' };
      foreach (string s in ((IEnumerable<string>) text.Split(chArray)).ToList<string>())
      {
        try
        {
          ulong num = ulong.Parse(s);
          CMainCustomSubScreen.featuredIDs.Add(num);
        }
        catch (FormatException ex)
        {
          string str = s;
          UnityEngine.Debug.Log((object) (ex.ToString() + " for string: " + str));
        }
      }
    }
    UnityWebRequest wwwNdemic = CustomScenarioClient.MakeQueryRequest(CustomScenarioBrowserMode.Featured, 0, 1);
    yield return (object) wwwNdemic.SendWebRequest();
    if (wwwNdemic.isDone && string.IsNullOrEmpty(wwwNdemic.error))
    {
      CMainCustomSubScreen.ndemicFeaturedIDs.Clear();
      JSONArray asArray = JSON.Parse(wwwNdemic.downloadHandler.text)["scenarioMetaDataArray"].AsArray;
      if ((JSONNode) asArray != (object) null)
      {
        for (int aIndex = 0; aIndex < asArray.Count; ++aIndex)
        {
          ulong id = (ulong) CustomScenarioMetadata.FromJson(asArray[aIndex]).Id;
          if (CMainCustomSubScreen.featuredIDs.Contains(id))
          {
            UnityEngine.Debug.Log((object) ("Duplicate Custom Scenario found: " + (object) id));
          }
          else
          {
            CMainCustomSubScreen.ndemicFeaturedIDs.Add(id);
            UnityEngine.Debug.Log((object) ("Downloaded Ndemic Server featured ID : " + (object) id));
          }
        }
      }
    }
    if (!this.showLocal)
      this.OnRefreshTableViewButtonClick();
  }

  private IEnumerator RefreshLocalScenarioData()
  {
    while (true)
    {
      if (this.localUGCHandler != null)
      {
        this.useEditorTabButton.transform.parent.gameObject.SetActive(true);
        this.localfilesTabToggle.gameObject.SetActive(true);
        this.tabTable.repositionNow = true;
      }
      yield return (object) new WaitForSeconds(3f);
    }
  }

  private IEnumerator CoIssueQuery(bool selectLast)
  {
    UnityEngine.Debug.Log((object) nameof (CoIssueQuery));
    UnityWebRequest Request = (UnityWebRequest) null;
    if (this.showSubscribed)
    {
      UnityEngine.Debug.Log((object) ("SHOW SUBSCRIBED: " + (object) this.ndemicFavouriteIds.Count));
      Request = this.ndemicFavouriteIds.Count == 0 ? (UnityWebRequest) null : CustomScenarioClient.MakeFixedListRequest((int) this.currentPage, this.ndemicPageSize, (IEnumerable<long>) this.ndemicFavouriteIds);
    }
    else if (!this.showLocal)
    {
      CustomScenarioBrowserMode mode = CustomScenarioBrowserMode.All;
      if (this.showFeatured)
        mode = CustomScenarioBrowserMode.Featured;
      if (this.showNew)
        mode = CustomScenarioBrowserMode.New;
      string category = (string) null;
      if (!this.currentCategory.Equals("All Categories"))
        category = this.currentCategory;
      Request = CustomScenarioClient.MakeQueryRequest(mode, (int) this.currentPage, this.ndemicPageSize, this.currentSearch, category);
    }
    if (Request != null)
      yield return (object) Request.SendWebRequest();
    this.ndemicMetadata.Clear();
    this.ndemicMetadataTotal = 0;
    this.ndemicMetadataPage = 0;
    if (Request == null)
      this.OnQueryComplete(selectLast, false);
    else if (!Request.isDone || !string.IsNullOrEmpty(Request.error))
    {
      this.OnQueryComplete(selectLast, true);
    }
    else
    {
      JSONNode jsonNode = JSON.Parse(Request.downloadHandler.text);
      JSONArray asArray = jsonNode["scenarioMetaDataArray"].AsArray;
      if ((JSONNode) asArray != (object) null)
      {
        for (int aIndex = 0; aIndex < asArray.Count; ++aIndex)
        {
          string str = (string) asArray[aIndex]["title"];
          this.ndemicMetadata.Add(CustomScenarioMetadata.FromJson(asArray[aIndex]));
        }
      }
      this.ndemicMetadataTotal = jsonNode["totalResults"].AsInt;
      this.OnQueryComplete(selectLast, false);
    }
  }

  private IEnumerator CoSubscribeNdemic(UICustomScenarioTableElement button)
  {
    yield return (object) CustomScenarioCache.Load(button.metadata.Id, (Action<Scenario>) (scen =>
    {
      if (scen != null)
      {
        UnityEngine.Debug.Log((object) ("Ndmeic subscribe " + (object) button.metadata.Id));
        this.ndemicFavouriteIds.Add(button.metadata.Id);
        UnityEngine.Debug.Log((object) ("NDEMIC IDS: " + (object) this.ndemicFavouriteIds.Count));
        this.SetSubscribePlayButtonState();
        button.SetSubscribed(true);
      }
      else
      {
        UnityEngine.Debug.Log((object) "Ndmeic subscribe ERROR");
        this.SetSubscribePlayButtonState(isSubscribed: false);
      }
    }));
  }

  private void UnsubscribeNdemic(UICustomScenarioTableElement button)
  {
    this.ndemicFavouriteIds.Remove(button.metadata.Id);
    CustomScenarioCache.Delete(button.metadata.Id);
    this.SetSubscribePlayButtonState(isSubscribed: false);
    button.SetSubscribed(false);
  }

  private void OnQueryComplete(bool selectLast, bool error)
  {
    int num = 0;
    while (num < this.ndemicMetadata.Count || this.currentScenarioLength < this.scenarioButtons.Count)
    {
      if (num >= this.ndemicMetadata.Count)
      {
        UICustomScenarioTableElement scenarioButton = this.scenarioButtons[this.currentScenarioLength];
        UICustomScenarioTableElement.PoolObject(scenarioButton);
        EventDelegate.Remove(scenarioButton.button.onClick, new EventDelegate.Callback(this.OnScenarioElementSelect));
        this.scenarioButtons.RemoveAt(this.currentScenarioLength);
        --num;
      }
      else if (this.recentIds.Contains((ulong) this.ndemicMetadata[num].Id))
      {
        UnityEngine.Debug.LogFormat("Duplicate Found in ndemic! - {0}  {1}", (object) this.ndemicMetadata[num].Title, (object) this.ndemicMetadata[num].Id);
      }
      else
      {
        bool flag = false;
        for (int index = 0; index < CMainCustomSubScreen.blockedWords.Length; ++index)
        {
          if (this.ndemicMetadata[num].Title != null && this.ndemicMetadata[num].Description != null)
          {
            if (this.ndemicMetadata[num].Title.Normalize().Contains(CMainCustomSubScreen.blockedWords[index].Normalize()))
              flag = true;
            if (this.ndemicMetadata[num].Description.Normalize().Contains(CMainCustomSubScreen.blockedWords[index].Normalize()))
              flag = true;
          }
        }
        if (flag)
        {
          ++num;
          continue;
        }
        this.recentIds.Add((ulong) this.ndemicMetadata[num].Id);
        UICustomScenarioTableElement scenarioButton;
        if (this.currentScenarioLength >= this.scenarioButtons.Count)
        {
          scenarioButton = UICustomScenarioTableElement.CreateObject(this.customScenarioObjectPrefab);
          scenarioButton.name = "Friend_" + (object) num;
          scenarioButton.transform.parent = this.customScenarioTable.transform;
          scenarioButton.transform.localPosition = Vector3.zero;
          scenarioButton.transform.localScale = Vector3.one;
          this.scenarioButtons.Add(scenarioButton);
        }
        else
          scenarioButton = this.scenarioButtons[this.currentScenarioLength];
        EventDelegate.Set(scenarioButton.button.onClick, new EventDelegate.Callback(this.OnScenarioElementSelect));
        CustomScenarioMetadata scenarioMetadata = this.ndemicMetadata[num];
        scenarioButton.PopulateWithData(scenarioMetadata);
        scenarioButton.SetSubscribed(this.ndemicFavouriteIds.Contains(scenarioMetadata.Id));
        this.StartCoroutine(this.DownloadIcon(scenarioMetadata, scenarioButton, num));
        ++this.currentScenarioLength;
        ++this.ndemicMetadataPage;
      }
      ++num;
    }
    UnityEngine.Debug.Log((object) ("AFTER Total ndemic responses: " + (object) this.ndemicMetadata.Count + " scen length: " + (object) this.currentScenarioLength + " buttons: " + (object) this.scenarioButtons.Count));
    if (this.isLoading)
      return;
    this.customScenarioTable.Reposition();
  }

  private IEnumerator DownloadIcon(
    CustomScenarioMetadata Metadata,
    UICustomScenarioTableElement TableElement,
    int delay)
  {
    Texture2D Texture = (Texture2D) null;
    byte[] icon = CustomScenarioCache.GetIcon(Metadata.IconUrl);
    if (icon == null)
    {
      yield return (object) new WaitForSeconds((float) delay * 0.2f);
      using (UnityWebRequest Request = UnityWebRequestTexture.GetTexture(Metadata.IconUrl, true))
      {
        yield return (object) Request.SendWebRequest();
        if (Request.isDone && string.IsNullOrEmpty(Request.error))
        {
          Texture = DownloadHandlerTexture.GetContent(Request);
          CustomScenarioCache.CacheIcon(Request.downloadHandler.data, Metadata.IconUrl);
        }
      }
    }
    else
    {
      Texture = new Texture2D(1, 1);
      Texture.LoadImage(icon);
    }
    if ((UnityEngine.Object) Texture != (UnityEngine.Object) null && (UnityEngine.Object) TableElement != (UnityEngine.Object) null && TableElement.metadata != null && TableElement.metadata.Id == Metadata.Id && TableElement.gameObject.activeSelf)
    {
      TableElement.ShowThumbnail(Texture);
      if ((UnityEngine.Object) TableElement == (UnityEngine.Object) this.selectedScenarioElement)
        this.selectedScenarioDetails.SetScenarioDetails(this.ndemicSelectedMetadata, (Texture) Texture);
    }
  }

  private IEnumerator GetFederalScenarioBasicInformation()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string text = pendingRequest.downloadHandler.text;
      this.federalScenarioMetaInfo = text;
      string[] strArray = text.Replace("\r\n", "\n").Split('\n');
      if (strArray.Length != 0)
      {
        int num = 0;
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrEmpty(strArray[index]))
          {
            this.federalScenarioNames.Add(strArray[index]);
            ++num;
          }
          else
            UnityEngine.Debug.Log((object) ("Tried to load scenario No." + index.ToString() + " to Federal Scenario List, but it seems it's totally empty"));
        }
        UnityEngine.Debug.Log((object) ("All scenarios loaded, " + num.ToString() + " succeeded, " + (strArray.Length - num).ToString() + " failed, let's have a cake"));
      }
      else
        UnityEngine.Debug.Log((object) "But it seems there's nothing in the scenarios");
    }
    else
    {
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario List: \n" + pendingRequest.error));
      string[] strArray1 = new string[7]
      {
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Path.DirectorySeparatorChar.ToString(),
        "Ndemic Creations",
        null,
        null,
        null,
        null
      };
      char directorySeparatorChar = Path.DirectorySeparatorChar;
      strArray1[3] = directorySeparatorChar.ToString();
      strArray1[4] = "Plague Inc. Evolved";
      directorySeparatorChar = Path.DirectorySeparatorChar;
      strArray1[5] = directorySeparatorChar.ToString();
      strArray1[6] = "AutoScenarios.txt";
      string path = string.Concat(strArray1);
      if (!File.Exists(path))
      {
        CUIManager.instance.standardConfirmOverlay.ShowLocalised("Information", "Cannot get critical information needed for Federal Scenario Library, ask admins to get a scenario list for further actions.", "YES", "OK");
      }
      else
      {
        string[] strArray2 = File.ReadAllLines(path);
        this.federalScenarioNames = new List<string>();
        foreach (string str in strArray2)
          this.federalScenarioNames.Add(str);
      }
    }
    this.InstallFederalScenario();
  }

  private void InstallFederalScenario()
  {
    string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Federal Scenario Cache" + Path.DirectorySeparatorChar.ToString();
    string str = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Creator" + Path.DirectorySeparatorChar.ToString();
    if (!Directory.Exists(path1))
      Directory.CreateDirectory(path1);
    if (CGameManager.federalScenarioNames != null && CGameManager.federalScenarioNames.Count > 0)
    {
      using (List<string>.Enumerator enumerator = CGameManager.federalScenarioNames.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          string downloadFileName = path1 + current + ".plagueinc";
          if (!Directory.Exists(downloadFileName + "cache"))
          {
            ++CGameManager.toDownload;
            this.StartCoroutine(this.DownloadAndInstallScenario(new Uri(CGameManager.federalServerAddress + "AutoScenarios/" + current + ".zip"), downloadFileName, current));
          }
          else
          {
            string path2 = str + "PIFSL " + current + "/";
            if (!Directory.Exists(path2))
            {
              ++CGameManager.toExtract;
              ZipFile.ExtractToDirectory(downloadFileName, path2, true);
              ++CGameManager.extracted;
            }
          }
        }
        if (CGameManager.toDownload == 0 && CGameManager.extracted > 0)
          CUIManager.instance.standardConfirmOverlay.ShowLocalised("Information", "All new scenarios downloaded, click any button to continue", "YES", "OK", new CConfirmOverlay.PressDelegate(this.OnRefreshTableViewButtonClick), new CConfirmOverlay.PressDelegate(this.OnRefreshTableViewButtonClick), isEscapeable: false);
        CGameManager.prepareDone = true;
      }
    }
    else
      UnityEngine.Debug.LogError((object) "Failed to install Federal Scenarios: List is empty or not got yet");
  }

  private IEnumerator DownloadAndInstallScenario(
    Uri uri,
    string downloadFileName,
    string scenarioName = "default scenario")
  {
    CMainCustomSubScreen cmainCustomSubScreen = this;
    using (UnityWebRequest downloader = UnityWebRequest.Get(uri))
    {
      downloader.downloadHandler = (DownloadHandler) new DownloadHandlerFile(downloadFileName);
      downloader.SendWebRequest();
      while (!downloader.isDone)
        yield return (object) null;
      if (downloader.error != null)
      {
        UnityEngine.Debug.LogError((object) downloader.error);
      }
      else
      {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "Scenario Creator" + Path.DirectorySeparatorChar.ToString() + "PIFSL " + scenarioName + "/";
        Directory.CreateDirectory(downloadFileName + "cache");
        UnityEngine.Debug.Log((object) ("Trying to install scenario " + scenarioName));
        if (!Directory.Exists(path))
          ZipFile.ExtractToDirectory(downloadFileName, path, true);
        else
          UnityEngine.Debug.Log((object) ("Scenario " + scenarioName + " already installed, skip installing process"));
        ++CGameManager.downloaded;
        if (CGameManager.toDownload == CGameManager.downloaded && CGameManager.toDownload > 0)
          CUIManager.instance.standardConfirmOverlay.ShowLocalised("Information", "All new scenarios downloaded, click any button to continue", "YES", "OK", new CConfirmOverlay.PressDelegate(cmainCustomSubScreen.OnRefreshTableViewButtonClick), new CConfirmOverlay.PressDelegate(cmainCustomSubScreen.OnRefreshTableViewButtonClick), isEscapeable: false);
      }
    }
  }

  private void AddLocalScenario()
  {
    char directorySeparatorChar = Path.DirectorySeparatorChar;
    string str1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + directorySeparatorChar.ToString() + "Ndemic Creations" + directorySeparatorChar.ToString() + "Plague Inc. Evolved" + directorySeparatorChar.ToString();
    string path1 = str1 + "Install Scenario/";
    if (!Directory.Exists(path1))
      Directory.CreateDirectory(path1);
    UnityEngine.Debug.Log((object) ("File Path: " + path1));
    string[] files = Directory.GetFiles(path1);
    if (files.Length != 0)
    {
      for (int index = 0; index < files.Length; ++index)
      {
        this.localScenario.Add(files[index]);
        UnityEngine.Debug.Log((object) (files[index] + " added"));
      }
      foreach (string str2 in this.localScenario)
      {
        if (str2.EndsWith(".plagueinc") || str2.EndsWith(".nohorse") || str2.EndsWith(".zip"))
        {
          string withoutExtension = Path.GetFileNameWithoutExtension(str2);
          string fileName1 = Path.GetFileName(str2);
          string str3 = str1 + "Scenario Creator/[Local] " + withoutExtension;
          if (!Directory.Exists(str3))
          {
            bool flag = true;
            try
            {
              ZipFile.ExtractToDirectory(str2, str3);
            }
            catch (InvalidDataException ex)
            {
              UnityEngine.Debug.Log((object) ("You **** shouldn't pack the file " + fileName1 + " in RAR format!"));
              flag = false;
            }
            UnityEngine.Debug.Log((object) ("Successfully install local scenario " + fileName1));
            if (flag && !File.Exists(Path.Combine(str3, "scenario.txt")))
            {
              UnityEngine.Debug.Log((object) "It seems no scenario.txt exists, trying to install with alternative method");
              string[] directories = Directory.GetDirectories(str3);
              if (directories.Length != 0)
              {
                foreach (string path2 in directories)
                {
                  foreach (string file in Directory.GetFiles(path2))
                  {
                    string fileName2 = Path.GetFileName(file);
                    File.Move(file, Path.Combine(str3, fileName2));
                  }
                }
                UnityEngine.Debug.Log((object) "Successfully moved inside files outside!");
              }
            }
          }
          else
            UnityEngine.Debug.Log((object) (fileName1 + " already exists, skip local scenario"));
        }
      }
    }
    UnityEngine.Debug.Log((object) "No Scenario Found");
  }

  private IEnumerator GetFederalLegacyList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalLegacyPath);
    List<string> LegacyScenarios = new List<string>();
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string text = pendingRequest.downloadHandler.text;
      this.federalScenarioMetaInfo = text;
      string[] strArray = text.Replace("\r\n", "\n").Split('\n');
      if (strArray.Length != 0)
      {
        int num = 0;
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrEmpty(strArray[index]))
          {
            LegacyScenarios.Add(strArray[index]);
            ++num;
          }
          else
            UnityEngine.Debug.Log((object) ("Tried to load scenario No." + index.ToString() + " to Legacy Scenario List, but it seems it's totally empty"));
        }
        UnityEngine.Debug.Log((object) ("All scenarios loaded, " + num.ToString() + " succeeded, " + (strArray.Length - num).ToString() + " failed, let's have a cake"));
      }
      else
        UnityEngine.Debug.Log((object) "But it seems there's nothing in the scenarios");
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Legacy Scenario List: \n" + pendingRequest.error));
    foreach (string str1 in LegacyScenarios)
    {
      string str2 = CSLocalUGCHandler.GetScenarioDataPath() + "PIFSL " + str1 + "/";
      string str3 = str2.Replace("PIFSL", "[Legacy]");
      if (Directory.Exists(str2))
      {
        if (Directory.Exists(str3))
        {
          Directory.Delete(str2, true);
          UnityEngine.Debug.Log((object) (str1 + " deleted due to legacy already exists"));
        }
        else
        {
          Directory.Move(str2, str3);
          UnityEngine.Debug.Log((object) (str1 + " moved to Legacy for update"));
        }
      }
    }
  }

  public IEnumerator GetFederalScenarioConstList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioConstListPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "newConstList.txt";
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, pendingRequest.downloadHandler.text);
      SPDisease.GetConstListExternal();
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario Const List: \n" + pendingRequest.error));
  }

  public IEnumerator GetFederalScenarioUnlockScoreList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioUnlockOrderPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockScore.txt";
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, pendingRequest.downloadHandler.text);
      SPDisease.GetUnlockScoreExternal();
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario Unlock Score List: \n" + pendingRequest.error));
  }

  private void OnLittleRefresh()
  {
    this.isLoading = true;
    this.SetRefreshPanelState(this.isLoading, false);
    this.scenarioListScrollBar.value = 0.0f;
    if (!this.showLocal)
    {
      if (this.ndemicCoroutine != null)
        this.StopCoroutine(this.ndemicCoroutine);
      this.ndemicCoroutine = this.StartCoroutine(this.CoIssueQuery(false));
      if (this.showSubscribed)
        this.steamUGCHandler.QueryFilteredUGCContent(EUserUGCList.k_EUserUGCList_Subscribed, EUserUGCListSortOrder.k_EUserUGCListSortOrder_SubscriptionDateDesc, string.Empty, pageNumber: this.currentPage);
      else if (this.showFeatured)
        this.steamUGCHandler.QuerySelectedUGCContent(CMainCustomSubScreen.featuredIDs);
      else if (this.showNew)
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByTrend;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        if (tagTexts.Count > 0)
          this.currentPage = 1U;
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, this.currentSearch, tagTexts, false, this.currentPage, 7U);
      }
      else
      {
        EUGCQuery sortAndFilterType = EUGCQuery.k_EUGCQuery_RankedByVote;
        IList<string> tagTexts = (IList<string>) new List<string>();
        if (!string.IsNullOrEmpty(this.selectedLanguage) && !this.selectedLanguage.Equals("All Languages"))
          tagTexts.Add(CLocalisationManager.GetSteamLanguageTag(this.selectedLanguage));
        if (!this.currentCategory.Equals("All Categories"))
          tagTexts.Add(this.currentCategory);
        if (tagTexts.Count > 0)
          this.currentPage = 1U;
        this.steamUGCHandler.QueryAllUGCContent(sortAndFilterType, string.Empty, tagTexts, false, this.currentPage);
      }
    }
    else if (this.localUGCHandler != null && this.localUGCHandler.GetScenarioSubset())
    {
      this.currentIndex = 0;
      this.currentPage = 1U;
      this.UpdatePaging(this.localUGCHandler.subsetScenarios.Count, this.localUGCHandler.subsetScenarios.Count);
      this.SetScenarioButtons((IList<ScenarioInformation>) this.localUGCHandler.subsetScenarios.GetRange(this.currentIndex, Mathf.Clamp(this.localUGCHandler.subsetScenarios.Count - this.currentIndex, 0, this.pageSize)));
      this.SetRefreshPanelState(false, true);
    }
    else
    {
      this.currentIndex = 0;
      this.currentPage = 1U;
      this.UpdatePaging(this.localUGCHandler.subsetScenarios.Count, this.localUGCHandler.subsetScenarios.Count);
      this.SetScenarioButtons((IList<ScenarioInformation>) this.localUGCHandler.subsetScenarios.GetRange(this.currentIndex, Mathf.Clamp(this.localUGCHandler.subsetScenarios.Count - this.currentIndex, 0, this.pageSize)));
      this.SetRefreshPanelState(false, true);
    }
  }

  public IEnumerator GetFederalScenarioAuthorList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioAuthorPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioAuthor.txt";
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, pendingRequest.downloadHandler.text);
      SPDisease.GetScenarioAuthorExternal();
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario Author List: \n" + pendingRequest.error));
  }

  public IEnumerator GetFederalScenarioUnlockPotentialList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioUnlockPotentialPath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "UnlockPotential.txt";
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, pendingRequest.downloadHandler.text);
      SPDisease.GetScenarioUnlockPotentialExternal();
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario Unlock Potential List: \n" + pendingRequest.error));
  }

  private string GetSortString()
  {
    string str1;
    switch (CGameManager.FilterScenarioComplete)
    {
      case "all":
        str1 = "[F:All]";
        break;
      case "completed":
        str1 = "[F:Cleared]";
        break;
      default:
        str1 = "[F:Not Cleared]";
        break;
    }
    string str2 = !(CGameManager.FilterScenarioLevel == "all") ? "[G:" + CGameManager.FilterScenarioLevel + "]" : "[G:All]";
    string str3 = !(CGameManager.FilterScenarioType == "all") ? "[H:" + CGameManager.FilterScenarioType + "]" : "[H:All]";
    string str4 = "[K:" + CGameManager.scenarioShow + "]";
    string str5 = "[L:" + CGameManager.scenarioSort + "]";
    return str1 + str2 + str3 + str4 + str5;
  }

  public IEnumerator GetFederalScenarioNameList()
  {
    UnityWebRequest pendingRequest = (UnityWebRequest) null;
    pendingRequest = UnityWebRequest.Get(this.federalScenarioNamePath);
    yield return (object) pendingRequest.SendWebRequest();
    if (pendingRequest.isDone && string.IsNullOrEmpty(pendingRequest.error))
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "ScenarioName.txt";
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, pendingRequest.downloadHandler.text);
      SPDisease.GetScenarioNameExternal();
    }
    else
      UnityEngine.Debug.LogError((object) ("Error while getting Federal Scenario Name List: \n" + pendingRequest.error));
  }

  private string GetScenarioDifficulty(string difficulty)
  {
    string scenarioDifficulty = difficulty;
    if (string.IsNullOrEmpty(scenarioDifficulty))
      return "FTR ?";
    if (scenarioDifficulty.IndexOf("FTR") != -1)
      scenarioDifficulty = ":ff00ff]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("BYD") != -1)
      scenarioDifficulty = ":ff0000]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("PST") != -1)
      scenarioDifficulty = ":00bbff]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("MXM") != -1)
      scenarioDifficulty = ":ffee00]" + scenarioDifficulty + ":ffffff]";
    if (scenarioDifficulty.IndexOf("PRS") != -1)
      scenarioDifficulty = ":00ff11]" + scenarioDifficulty + ":ffffff]";
    return scenarioDifficulty;
  }

  private void GetScenarioList()
  {
    if (CGameManager.scenarioData != (object) null)
    {
      int num = 0;
      this.federalScenarioNames = new List<string>();
      foreach (JSONNode jsonNode in CGameManager.scenarioData["Scenarios"].AsArray)
      {
        if (!string.IsNullOrEmpty((string) jsonNode["fileName"]) && jsonNode["preload"].AsBool)
        {
          this.federalScenarioNames.Add((string) jsonNode["fileName"]);
          ++num;
        }
      }
      UnityEngine.Debug.Log((object) ("All scenarios preloaded, " + this.federalScenarioNames.Count.ToString() + " succeeded, " + "let's have a cake!"));
    }
    else
      CUIManager.instance.standardConfirmOverlay.ShowLocalised("Information", "Cannot get critical data needed for Federal Scenario Library.\nRestart the game to see if it works.\nAsk admins for further information if problem not solved.", "YES", "OK");
    this.InstallFederalScenario();
  }

  private char OnSearchTextChange(string text, int charIndex, char addedChar)
  {
    CGameManager.ScenarioSearchKeyword = text + addedChar.ToString();
    UnityEngine.Debug.Log((object) ("keyword set to: " + text + addedChar.ToString()));
    this.OnLittleRefresh();
    return addedChar;
  }

  private void OnSearchEscape()
  {
    CGameManager.ScenarioSearchKeyword = this.searchInputField.value;
    UnityEngine.Debug.Log((object) ("keyword set to: " + this.searchInputField.value));
    this.OnLittleRefresh();
  }
}
