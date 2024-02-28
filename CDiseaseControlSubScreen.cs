// Decompiled with JetBrains decompiler
// Type: CDiseaseControlSubScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CDiseaseControlSubScreen : IGameSubScreen
{
  public RevealedSymptomHex infoHex;
  public GameObject symptomInfoContainer;
  public UILabelAutotranslate symptomTitle;
  public UILabelAutotranslate symptomDescription;
  public RevealedSymptomHex[] symptomHexes;
  private RevealedSymptomHex lastHex;

  public override void SetActive(bool isActive)
  {
    base.SetActive(isActive);
    if (!isActive)
      return;
    this.lastHex = (RevealedSymptomHex) null;
    this.symptomInfoContainer.SetActive(false);
    int index;
    for (index = 0; index < World.instance.petriDishSymptoms.Count && index < this.symptomHexes.Length; ++index)
    {
      this.symptomHexes[index].controlScreen = this;
      PetriDishSymptom petriDishSymptom = World.instance.petriDishSymptoms[index];
      this.symptomHexes[index].Setup(petriDishSymptom);
      this.symptomHexes[index].SetVisibility(true);
    }
    for (; index < this.symptomHexes.Length; ++index)
      this.symptomHexes[index].SetVisibility(false);
  }

  public void HexSelected(PetriDishSymptom symptom, RevealedSymptomHex hex)
  {
    if ((Object) this.lastHex != (Object) hex)
    {
      this.lastHex = hex;
      CSoundManager.instance.PlaySFX("buttonclick");
    }
    hex.SetSelected(true);
    for (int index = 0; index < this.symptomHexes.Length; ++index)
    {
      if ((Object) this.symptomHexes[index] != (Object) hex)
        this.symptomHexes[index].SetSelected(false);
    }
    this.symptomInfoContainer.SetActive(true);
    this.symptomTitle.SetInitialText(symptom.name);
    this.symptomDescription.SetInitialText(symptom.description);
  }

  public static string GetUnifiedNum(float num)
  {
    return (double) num > 0.0 ? "+" + num.ToString() : num.ToString();
  }
}
