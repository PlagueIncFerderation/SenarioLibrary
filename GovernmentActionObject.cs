// Decompiled with JetBrains decompiler
// Type: GovernmentActionObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GovernmentActionObject : MonoBehaviour
{
  public UILabel actionText;
  public UILabel actionDate;
  public Color removedCol;
  public Color standardCol;
  public UISprite background;

  public void SetAction(string text, string date, bool removed)
  {
    this.actionText.text = text;
    this.actionDate.text = date;
    this.background.color = removed ? this.removedCol : this.standardCol;
  }

  public void SetAction(string text, string date, bool removed, int importance)
  {
    this.actionText.text = text;
    if (importance == 3)
      this.actionText.color = Color.magenta;
    if (importance == 2)
      this.actionText.color = Color.red;
    if (importance == 1)
      this.actionText.color = Color.blue;
    if (importance == 0)
      this.actionText.color = Color.white;
    if (importance == -2)
      this.actionText.color = Color.green;
    this.actionDate.text = date;
    this.background.color = removed ? this.removedCol : this.standardCol;
  }
}
