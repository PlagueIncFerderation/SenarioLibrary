// Decompiled with JetBrains decompiler
// Type: UILabelAutotranslate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UILabelAutotranslate : MonoBehaviour
{
  internal string originalLabelText;
  internal string currentLanguage;
  internal string currentLocalisedScenario;
  private UILabel label;
  private bool useLocalisation = true;
  private UIStretch[] stretchers;

  public virtual void SetInitialText(string newText, bool useLocalisation = true)
  {
    if ((Object) this.label == (Object) null)
      this.label = this.GetComponent<UILabel>();
    if ((Object) this.label != (Object) null)
    {
      this.useLocalisation = useLocalisation;
      this.originalLabelText = newText;
      this.label.text = useLocalisation ? CLocalisationManager.GetText(this.originalLabelText) : this.originalLabelText;
      this.currentLanguage = CLocalisationManager.ActiveLanguage;
    }
    else
    {
      Debug.Log((object) ("GameObject has no UILabel: " + this.gameObject.name));
      Object.Destroy((Object) this);
    }
  }

  public virtual void AutoTranslate()
  {
    if (!Application.isPlaying)
      return;
    if ((Object) this.label == (Object) null)
    {
      this.label = this.GetComponent<UILabel>();
      if ((Object) this.label != (Object) null)
      {
        this.originalLabelText = this.label.text;
        this.label.text = this.useLocalisation ? CLocalisationManager.GetText(this.originalLabelText) : this.originalLabelText;
        this.currentLanguage = CLocalisationManager.ActiveLanguage;
      }
      else
        Object.Destroy((Object) this);
    }
    else if (this.currentLanguage != CLocalisationManager.ActiveLanguage || this.currentLocalisedScenario != CLocalisationManager.CustomLocalisedScenario)
    {
      this.label.text = this.useLocalisation ? CLocalisationManager.GetText(this.originalLabelText) : this.originalLabelText;
      this.currentLanguage = CLocalisationManager.ActiveLanguage;
      this.currentLocalisedScenario = CLocalisationManager.CustomLocalisedScenario;
    }
    if (this.stretchers == null)
      return;
    for (int index = 0; index < this.stretchers.Length; ++index)
    {
      if ((Object) this.stretchers[index] != (Object) null && this.stretchers[index].enabled)
        this.stretchers[index].UpdateStretch();
    }
  }

  protected virtual void Awake() => this.stretchers = this.GetComponentsInChildren<UIStretch>();

  protected virtual void OnEnable()
  {
    this.AutoTranslate();
    this.StartCoroutine(this.RepositionTable(this.transform.parent.GetComponent<UITable>()));
  }

  private IEnumerator RepositionTable(UITable table)
  {
    if (!((Object) table == (Object) null))
    {
      table.Reposition();
      yield return (object) null;
      table.Reposition();
    }
  }

  public void SetLabelText(string text) => this.label.text = text;

  public string GetLabelText() => this.label.text;
}
