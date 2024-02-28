// Decompiled with JetBrains decompiler
// Type: CGSNameSubScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CGSNameSubScreen : IGameSetupSubScreen
{
  public UILabelAutotranslate nameTitleLabel;
  public UIInput input;
  public UILabel selectedDiseaseName;
  public UIButton randomButton;
  private string initialValue;
  private CGSScreen cgsScreen;

  public override void Initialise()
  {
    base.Initialise();
    this.cgsScreen = this.gameScreen as CGSScreen;
    this.input.onValidate = new UIInput.OnValidate(this.ValidateName);
    EventDelegate.Add(this.input.onSubmit, new EventDelegate.Callback(this.OnSubmit));
    EventDelegate.Add(this.randomButton.onClick, new EventDelegate.Callback(this.OnClickRandomName));
    this.input.characterLimit = 256;
  }

  public override void Setup()
  {
    base.Setup();
    this.input.characterLimit = 256;
  }

  public override void SetActive(bool b)
  {
    base.SetActive(b);
    if (b)
    {
      this.input.characterLimit = 256;
      if (!CSteamControllerManager.instance.controllerActive)
        UICamera.selectedObject = this.input.gameObject;
      if (!string.IsNullOrEmpty(CGameManager.game.SetupParameters.defaultName))
      {
        this.input.value = CLocalisationManager.GetText(CGameManager.game.SetupParameters.defaultName);
        this.input.defaultText = "";
      }
      else
      {
        string key = "CGSName-last" + this.cgsScreen.DiseaseType.ToString();
        if (CGameManager.IsCureGame)
          key = "Cure_" + key;
        this.initialValue = PlayerPrefs.HasKey(key) ? (this.cgsScreen.DiseaseType != Disease.EDiseaseType.TUTORIAL ? PlayerPrefs.GetString(key) : CLocalisationManager.GetText("FE_Plagues_Default_Name")) : (!CGameManager.IsCureGame ? (this.cgsScreen.DiseaseType != Disease.EDiseaseType.SIMIAN_FLU ? (this.cgsScreen.DiseaseType != Disease.EDiseaseType.VAMPIRE ? CLocalisationManager.GetText("FE_Plagues_Default_Name") : "Nox Eternis") : "ALZ-113") : CLocalisationManager.GetText("FE_Plagues_Default_Cure_Name"));
        UIInput.selection = (UIInput) null;
        this.input.value = this.initialValue;
        this.input.defaultText = "";
        UIInput.selection = this.input;
      }
      this.nameTitleLabel.SetInitialText("FE_Plagues_Name_Title");
    }
    else
      this.cgsScreen.SetCanContinue(true);
  }

  public void OnClickRandomName()
  {
    UIInput.selection = (UIInput) null;
    this.input.value = RandomNameGenerator.instance.GenerateName();
    this.input.defaultText = "";
    UIInput.selection = this.input;
  }

  public void Update()
  {
    if (!(bool) (Object) this.gameScreen)
      return;
    (this.gameScreen as CGSScreen).SetCanContinue(!string.IsNullOrEmpty(this.input.value));
  }

  private void OnSubmit() => this.cgsScreen.OnClickNext();

  private char ValidateName(string text, int index, char added)
  {
    return text.Length >= 256 ? char.MinValue : added;
  }

  public override bool ChooseOption()
  {
    if (string.IsNullOrEmpty(this.input.value))
      return false;
    this.SaveName();
    (this.gameScreen as CGSScreen).DiseaseName = this.input.value;
    this.selectedDiseaseName.text = this.input.value;
    this.nameTitleLabel.SetInitialText("IG_Loading_Title");
    return base.ChooseOption();
  }

  public void SaveName()
  {
    Scenario currentLoadedScenario = CGameManager.game.CurrentLoadedScenario;
    if (currentLoadedScenario != null && !string.IsNullOrEmpty(currentLoadedScenario.scenarioInformation.scenName))
      return;
    string key = "CGSName-last" + ((CGSScreen) this.gameScreen).DiseaseType.ToString();
    if (CGameManager.IsCureGame)
      key = "Cure_" + key;
    PlayerPrefs.SetString(key, this.input.value);
  }
}
