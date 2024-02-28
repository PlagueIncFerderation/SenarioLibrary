// Decompiled with JetBrains decompiler
// Type: EvolutionHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EvolutionHistory : MonoBehaviour
{
  public UIDragPanelContents panel;
  public UILabel techName;
  public UILabel techDate;
  public TechHex techHex;

  public void SetTech(Disease d, TechHistory techEvent, bool canAfford = false)
  {
    this.techHex.gameObject.SetActive(true);
    Technology technology = d.GetTechnology(techEvent.id);
    this.techName.text = CLocalisationManager.GetText(technology.name);
    this.techDate.text = string.Empty;
    if (!technology.isPreEvolved)
      this.techDate.text = "Day " + techEvent.turn.ToString();
    this.techHex.SetTech(d, technology, true, canAfford);
  }

  public void SetEmpty(Disease d)
  {
    if (d != null)
      this.techHex.SetTechEmpty(d);
    this.techName.text = "";
    this.techDate.text = "";
  }
}
