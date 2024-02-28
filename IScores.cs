// Decompiled with JetBrains decompiler
// Type: IScores
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IScores : MonoBehaviour
{
  public virtual void Initialise()
  {
  }

  public virtual void LoadPersonalData(
    IGame.GameType type,
    Disease.EDiseaseType[] diseases,
    Action<List<IScores.Score>> callback)
  {
  }

  public virtual void LoadLeaderboardData(
    string name,
    ERank rankType,
    Action<List<IScores.Score>> callback)
  {
  }

  public virtual void LoadLocalScores(Action<List<IScores.Score>> callback)
  {
  }

  public virtual void AddScore(IScores.Score score)
  {
  }

  public virtual void AddLocalScore(IScores.Score score)
  {
  }

  public virtual bool IsAvailable() => false;

  public class Score
  {
    public string name;
    public string leaderboard;
    public IGame.GameType game;
    public Disease.EDiseaseType disease;
    public string scenario;
    public long score;
    public int rank;
    public int percentageRank;
    public bool isUser;
    public int absRank;

    public Score()
    {
    }

    public virtual string GetName() => this.name;

    public virtual IGame.GameType GetGameMode() => this.game;

    public virtual Disease.EDiseaseType GetDiseaseType() => this.disease;

    public virtual long GetScore() => this.score;

    public virtual int GetRank() => this.rank;

    public virtual int GetPercentageRank() => this.percentageRank;

    public virtual string GetLeaderboardName()
    {
      if (string.IsNullOrEmpty(this.leaderboard))
        this.leaderboard = CGameManager.GetLeaderboardName(this.game, this.disease, this.scenario);
      return this.leaderboard;
    }

    public Score(
      string nom,
      long s,
      int r,
      IGame.GameType g,
      Disease.EDiseaseType d,
      string scen)
    {
      this.name = nom;
      this.score = s;
      this.rank = r;
      this.game = g;
      this.disease = d;
      this.scenario = scen;
      this.leaderboard = this.GetLeaderboardName();
    }

    public virtual IScores.Score GetBestScore(IScores.Score a)
    {
      if (a.game != this.game)
        return this;
      return this.game == IGame.GameType.SpeedRun ? (this.score <= 0L || a.score > 0L && a.score < this.score ? a : this) : (a.score <= this.score ? this : a);
    }

    public int GetAbsRank() => this.absRank;
  }
}
