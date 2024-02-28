// Decompiled with JetBrains decompiler
// Type: DefconObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DefconObject : MonoBehaviour
{
  public UISprite background;
  public UISprite icon;
  public UILabel label;
  private int currentState;

  public int State
  {
    get => this.currentState;
    set => this.SetDefconState(value);
  }

  public int Max => 6;

  private void Start()
  {
    this.label.text = this.currentState.ToString();
    this.icon.spriteName = "Icon_Defcon_0";
    this.background.spriteName = "Icon_Defcon_Base_0";
  }

  public void SetDefconState(int state)
  {
    int num = state;
    if (num == this.currentState)
      return;
    if (this.currentState < num && num <= 6)
      CSoundManager.instance.PlaySFX("defconchange_up");
    else if (this.currentState > num && num >= 0)
      CSoundManager.instance.PlaySFX("defconchange_down");
    this.currentState = num;
    this.label.text = this.currentState.ToString();
    this.icon.spriteName = "Icon_Defcon_" + this.currentState.ToString();
    this.background.spriteName = "Icon_Defcon_Base_" + this.currentState.ToString();
  }

  public void SetDefconState(int state, float priority)
  {
    if (state != this.currentState)
    {
      if (this.currentState < state && state <= 6)
        CSoundManager.instance.PlaySFX("defconchange_up");
      else if (this.currentState > state && state >= 0)
        CSoundManager.instance.PlaySFX("defconchange_down");
      this.currentState = state;
      this.icon.spriteName = "Icon_Defcon_" + this.currentState.ToString();
      this.background.spriteName = "Icon_Defcon_Base_" + this.currentState.ToString();
    }
    this.label.text = priority.ToString("f2");
  }
}
