// Decompiled with JetBrains decompiler
// Type: MultiplayerResultOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MultiplayerResultOverlay : CGameOverlay
{
  public UILabel resultTitle;
  public UILabel resultSubTitle;
  public UILabel resultDescription;
  public UIButton endGameButton;
  public UIButton continueButton;
  private MultiplayerResultOverlay.MultiplayerResult result;

  public override void Initialise()
  {
    base.Initialise();
    if ((Object) this.endGameButton != (Object) null)
      EventDelegate.Set(this.endGameButton.onClick, new EventDelegate.Callback(this.GoToEndScreen));
    if (!((Object) this.continueButton != (Object) null))
      return;
    EventDelegate.Set(this.continueButton.onClick, new EventDelegate.Callback(this.Continue));
  }

  public void SetEnding(
    IGame.GameType gameType,
    Disease d,
    MultiplayerResultOverlay.MultiplayerResult result,
    IGame.EndGameResult resultType)
  {
    this.result = result;
    string tagName = string.Empty;
    string str1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    bool flag = true;
    switch (gameType)
    {
      case IGame.GameType.VersusMP:
        switch (resultType)
        {
          case IGame.EndGameResult.Infected:
            str1 = "MP_End_Popup_Full_Infected_Subtitle_";
            str2 = "MP_End_Popup_Full_Infected_Text_";
            break;
          case IGame.EndGameResult.Dead:
            str1 = "MP_End_Popup_Disease_Dead_Subtitle_";
            str2 = "MP_End_Popup_Disease_Dead_Text_";
            break;
          case IGame.EndGameResult.Cured:
            str1 = "MP_End_Popup_Disease_Cure_Subtitle_";
            str2 = "MP_End_Popup_Disease_Cure_Text_";
            break;
          case IGame.EndGameResult.Disconnected:
            str1 = "MP_End_Popup_Disconnect_Subtitle_";
            str2 = "MP_End_Popup_Disconnect_Text_";
            break;
          case IGame.EndGameResult.Resigned:
            str1 = "MP_End_Popup_Resigned_Subtitle_";
            str2 = "MP_End_Popup_Resigned_Text_";
            break;
          case IGame.EndGameResult.Destroyed:
            str1 = "MP_End_Popup_Destroyed_Subtitle_";
            str2 = "MP_End_Popup_Destroyed_Text_";
            break;
        }
        break;
      case IGame.GameType.CoopMP:
        switch (resultType)
        {
          case IGame.EndGameResult.Dead:
            str1 = "Coop_End_Popup_Dead_Subtitle_";
            str2 = "Coop_End_Popup_Dead_Text_";
            break;
          case IGame.EndGameResult.Cured:
            str1 = "Coop_End_Popup_Cured_Subtitle_";
            str2 = "Coop_End_Popup_Cured_Text_";
            break;
          case IGame.EndGameResult.Disconnected:
          case IGame.EndGameResult.Resigned:
            str1 = "FE_Multiplayer_PopUp_Connection_Lost_Title";
            str2 = "FE_Multiplayer_PopUp_Connection_Lost_Text";
            flag = false;
            break;
        }
        break;
    }
    if (flag)
    {
      switch (result)
      {
        case MultiplayerResultOverlay.MultiplayerResult.Win:
          tagName = "FE_EndGame_Title_Victory";
          str3 = "Win";
          break;
        case MultiplayerResultOverlay.MultiplayerResult.Lose:
          tagName = "FE_EndGame_Title_Defeat";
          str3 = "Loss";
          break;
        case MultiplayerResultOverlay.MultiplayerResult.Draw:
          tagName = "FE_EndGame_Title_Draw";
          str3 = "Draw";
          break;
      }
    }
    Disease disease1 = CGameManager.localPlayerInfo.disease;
    Disease disease2 = CGameManager.localPlayerInfo.disease == World.instance.diseases[0] ? World.instance.diseases[1] : World.instance.diseases[0];
    int num1;
    int num2;
    int num3;
    if (result == MultiplayerResultOverlay.MultiplayerResult.Win)
    {
      num1 = Mathf.RoundToInt((float) ((double) World.instance.GetTotalHealthy() / (double) World.instance.totalPopulation * 100.0));
      num2 = Mathf.RoundToInt(MPPlayerData.GetPlayerDeadPercentage(disease1, PlayerDataContext.World));
      num3 = Mathf.RoundToInt(MPPlayerData.GetPlayerDeadPercentage(disease2, PlayerDataContext.World));
    }
    else
    {
      num1 = Mathf.CeilToInt((float) ((double) World.instance.GetTotalHealthy() / (double) World.instance.totalPopulation * 100.0));
      num2 = Mathf.FloorToInt(MPPlayerData.GetPlayerDeadPercentage(disease1, PlayerDataContext.World));
      num3 = Mathf.FloorToInt(MPPlayerData.GetPlayerDeadPercentage(disease2, PlayerDataContext.World));
    }
    this.resultTitle.text = CLocalisationManager.GetText(tagName);
    this.resultSubTitle.text = CLocalisationManager.GetText(str1 + str3).Replace("%plague1", disease1.name).Replace("%plague2", disease2.name).Replace("%deadpercent1", num2.ToString()).Replace("%deadpercent2", num3.ToString()).Replace("%s", World.instance.DiseaseTurn.ToString()).Replace("%healthy", num1.ToString());
    this.resultDescription.text = CLocalisationManager.GetText(str2 + str3).Replace("%plague1", disease1.name).Replace("%plague2", disease2.name).Replace("%deadpercent1", num2.ToString()).Replace("%deadpercent2", num3.ToString()).Replace("%s", World.instance.DiseaseTurn.ToString()).Replace("%healthy", num1.ToString());
    if (!CGameManager.cheatDetected)
      return;
    this.resultSubTitle.text = "对方试图开纪";
    this.resultDescription.text = "你的对手试图开纪，记得保存截图和Log日志文件提交！";
    CGameManager.cheatDetected = false;
  }

  public void Continue()
  {
    CHUDScreen screen = CUIManager.instance.GetScreen("HUDScreen") as CHUDScreen;
    if ((Object) screen != (Object) null)
      screen.SetEnable(true);
    CUIManager.instance.HideOverlay((CGameOverlay) this);
    CHUDScreen.instance.MPGameEnded(this.result);
    CInterfaceManager.instance.resultOverlay = (MultiplayerResultOverlay) null;
  }

  public void GoToEndScreen()
  {
    CUIManager.instance.HideOverlay((CGameOverlay) this);
    CInterfaceManager.instance.Cleanup();
    CUIManager.instance.SetActiveScreen("MP_EndScreen");
  }

  public enum MultiplayerResult
  {
    None,
    Win,
    Lose,
    Draw,
  }
}
