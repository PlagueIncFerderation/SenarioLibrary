// Decompiled with JetBrains decompiler
// Type: ScoreObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ScoreObject : MonoBehaviour
{
  public UILabel mpRank;
  public UILabel mpName;
  public UILabel mpDate;
  public UILabel mpScore;
  public UISprite highlight;
  private Coroutine pollCoroutine;

  public void SetData(
    string rankText,
    string nameText,
    string dateText,
    string scoreText,
    bool isMine)
  {
    this.mpRank.text = rankText;
    this.mpName.text = nameText;
    this.mpDate.text = dateText;
    this.mpScore.text = scoreText;
    this.highlight.enabled = isMine;
  }

  public void SetData(IScores.Score highScore)
  {
    if (this.pollCoroutine != null)
      this.StopCoroutine(this.pollCoroutine);
    if (highScore.game == IGame.GameType.VersusMP)
    {
      this.SetData(highScore.GetRank().ToString(), highScore.GetName(), "", highScore.GetScore().ToString(), highScore.isUser);
    }
    else
    {
      long num = highScore.GetScore();
      if (num < 0L)
        num = 0L;
      this.SetData(highScore.GetPercentageRank().ToString() + "%(#" + highScore.GetAbsRank().ToString() + ")", highScore.GetName(), "", num.ToString(), highScore.isUser);
    }
    if (!string.IsNullOrEmpty(highScore.GetName()))
      return;
    this.mpName.text = "...";
    this.pollCoroutine = this.StartCoroutine(this.PollScoreName(highScore));
  }

  private IEnumerator PollScoreName(IScores.Score score)
  {
    for (int timeout = 0; timeout < 100; ++timeout)
    {
      yield return (object) new WaitForSeconds(0.1f);
      if (!string.IsNullOrEmpty(score.GetName()))
      {
        this.mpName.text = score.GetName();
        yield break;
      }
    }
    this.mpName.text = "???";
  }
}
