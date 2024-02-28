// Decompiled with JetBrains decompiler
// Type: CScoresSteam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CScoresSteam : IScores
{
  private IDictionary<string, SteamLeaderboard_t> leaderboards = (IDictionary<string, SteamLeaderboard_t>) new Dictionary<string, SteamLeaderboard_t>();
  private IDictionary<ulong, string> leaderboardNames = (IDictionary<ulong, string>) new Dictionary<ulong, string>();
  private IDictionary<string, List<IScores.Score>> globalScores = (IDictionary<string, List<IScores.Score>>) new Dictionary<string, List<IScores.Score>>();
  private IDictionary<string, List<IScores.Score>> friendScores = (IDictionary<string, List<IScores.Score>>) new Dictionary<string, List<IScores.Score>>();
  private IDictionary<string, List<IScores.Score>> userScores = (IDictionary<string, List<IScores.Score>>) new Dictionary<string, List<IScores.Score>>();
  private ELeaderboardDataRequest processingType;
  private string loadingLeaderboard;
  private string processingLeaderboard;
  protected CallResult<LeaderboardFindResult_t> m_leaderboardFindResult;
  protected CallResult<LeaderboardScoreUploaded_t> m_leaderboardScoreUploaded;
  protected CallResult<LeaderboardScoresDownloaded_t> m_leaderboardScoresDownloaded;
  private Dictionary<string, IScores.Score> localScores;
  private int numCallbacks;
  private List<string> scoreIsInThousands = new List<string>((IEnumerable<string>) new string[1]
  {
    "official_BOARD_GAME"
  });

  public override bool IsAvailable() => true;

  public override void Initialise()
  {
    if (!SteamManager.Initialized)
      return;
    this.m_leaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(new CallResult<LeaderboardFindResult_t>.APIDispatchDelegate(this.LeaderboardFound));
    this.m_leaderboardScoreUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(new CallResult<LeaderboardScoreUploaded_t>.APIDispatchDelegate(this.LeaderboardScoreUploaded));
    this.m_leaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(new CallResult<LeaderboardScoresDownloaded_t>.APIDispatchDelegate(this.LeaderboardScoresDownloaded));
    this.EraseOldFiles();
  }

  private void SaveLeaderboardCallback(
    Action<List<IScores.Score>> callback,
    List<IScores.Score> scores)
  {
    for (int index = 0; index < scores.Count; ++index)
    {
      string leaderboardName = scores[index].GetLeaderboardName();
      this.localScores[leaderboardName] = !this.localScores.ContainsKey(leaderboardName) ? scores[index] : this.localScores[leaderboardName].GetBestScore(scores[index]);
    }
    --this.numCallbacks;
    if (this.numCallbacks <= 0)
    {
      this.numCallbacks = 0;
      this.SaveMyScores();
    }
    callback(scores);
  }

  public override void LoadPersonalData(
    IGame.GameType type,
    Disease.EDiseaseType[] diseases,
    Action<List<IScores.Score>> callback)
  {
    if (this.localScores == null)
      this.LoadPersonalLocal();
    List<IScores.Score> scoreList = new List<IScores.Score>();
    string str1 = type.ToString().ToLower() + "_";
    for (int index = 0; index < diseases.Length; ++index)
    {
      string str2 = str1 + diseases[index].ToString().ToUpper();
      if (this.localScores.ContainsKey(str2))
      {
        scoreList.Add(this.localScores[str2]);
      }
      else
      {
        ++this.numCallbacks;
        this.LoadPersonalLeaderboard(str2, callback);
      }
    }
    callback(scoreList);
  }

  private void LoadPersonalLocal()
  {
    this.localScores = new Dictionary<string, IScores.Score>();
    List<IScores.Score> scoreList = new List<IScores.Score>();
    string str1 = !CSavesSteam.ExistsCloudData("scores.dat") ? (string) null : CSavesSteam.LoadCloudData("scores.dat");
    if (string.IsNullOrEmpty(str1))
      return;
    string str2 = str1;
    char[] chArray = new char[1]{ '\n' };
    foreach (string str3 in str2.Split(chArray))
    {
      try
      {
        string[] strArray = CUtils.Base64Decode(str3).Split(':');
        IScores.Score score = new IScores.Score();
        score.game = (IGame.GameType) Enum.Parse(typeof (IGame.GameType), strArray[0]);
        score.disease = (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), strArray[1]);
        score.score = long.Parse(strArray[2]);
        score.name = "";
        scoreList.Add(score);
        this.localScores.Add(score.GetLeaderboardName(), score);
      }
      catch
      {
      }
    }
  }

  private void LoadPersonalLeaderboard(string lname, Action<List<IScores.Score>> callback)
  {
    this.StartCoroutine(this.LoadLeaderboard(lname, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, 0, 0, callback, true));
  }

  public override void LoadLeaderboardData(
    string lname,
    ERank rankType,
    Action<List<IScores.Score>> callback)
  {
    int start = -10;
    int num = 100;
    ELeaderboardDataRequest type;
    switch (rankType)
    {
      case ERank.Friends:
        type = ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends;
        start = -1000;
        num = 1000;
        break;
      case ERank.User:
        type = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser;
        break;
      default:
        type = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal;
        break;
    }
    this.StartCoroutine(this.LoadLeaderboard(lname, type, start, num, callback));
  }

  private IEnumerator LoadLeaderboard(
    string name,
    ELeaderboardDataRequest type,
    int start,
    int num,
    Action<List<IScores.Score>> callback,
    bool saveLeaderboard = false)
  {
    if (!this.leaderboards.ContainsKey(name))
    {
      while (!string.IsNullOrEmpty(this.loadingLeaderboard))
        yield return (object) null;
      if (!SteamManager.Initialized)
        yield break;
      else if (!this.leaderboards.ContainsKey(name))
      {
        this.loadingLeaderboard = name;
        this.m_leaderboardFindResult.Set(SteamUserStats.FindLeaderboard(name));
        while (this.loadingLeaderboard == name)
          yield return (object) null;
      }
    }
    List<IScores.Score> scores = (List<IScores.Score>) null;
    if (this.leaderboards.ContainsKey(name))
    {
      while (!string.IsNullOrEmpty(this.processingLeaderboard))
        yield return (object) null;
      this.processingLeaderboard = name;
      this.processingType = type;
      this.m_leaderboardScoresDownloaded.Set(SteamUserStats.DownloadLeaderboardEntries(this.leaderboards[name], type, start, num));
      while (this.processingLeaderboard == name && this.processingType == type)
        yield return (object) null;
      if (type == ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends && this.friendScores.ContainsKey(name))
        scores = this.friendScores[name];
      else if (type == ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal && this.globalScores.ContainsKey(name))
        scores = this.globalScores[name];
      else if (type == ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser && this.userScores.ContainsKey(name))
        scores = this.userScores[name];
    }
    if (saveLeaderboard && scores != null)
      this.SaveLeaderboardCallback(callback, scores);
    if (scores == null)
      Debug.Log((object) ("Found no data in: " + name));
    callback(scores);
  }

  private void LeaderboardFound(LeaderboardFindResult_t result, bool ioFailure)
  {
    if (string.IsNullOrEmpty(this.loadingLeaderboard))
      return;
    if (!ioFailure)
    {
      this.leaderboards[this.loadingLeaderboard] = result.m_hSteamLeaderboard;
      this.leaderboardNames[result.m_hSteamLeaderboard.m_SteamLeaderboard] = this.loadingLeaderboard;
    }
    else
      Debug.LogError((object) ("IOFailure: " + this.loadingLeaderboard));
    this.loadingLeaderboard = (string) null;
  }

  private void LeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t value, bool ioFailure)
  {
    if (string.IsNullOrEmpty(this.processingLeaderboard))
      return;
    if (!ioFailure)
    {
      int leaderboardEntryCount = SteamUserStats.GetLeaderboardEntryCount(value.m_hSteamLeaderboard);
      CSteamID steamId = SteamUser.GetSteamID();
      List<IScores.Score> scoreList = new List<IScores.Score>();
      for (int index = 0; index < value.m_cEntryCount; ++index)
      {
        int[] pDetails = new int[3];
        LeaderboardEntry_t pLeaderboardEntry;
        if (SteamUserStats.GetDownloadedLeaderboardEntry(value.m_hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, 3))
        {
          CScoresSteam.SteamScore steamScore = new CScoresSteam.SteamScore();
          steamScore.steamID = pLeaderboardEntry.m_steamIDUser;
          steamScore.score = (long) pLeaderboardEntry.m_nScore;
          steamScore.rank = pLeaderboardEntry.m_nGlobalRank;
          steamScore.percentageRank = 1 + Mathf.FloorToInt(99f * (float) pLeaderboardEntry.m_nGlobalRank / (float) leaderboardEntryCount);
          steamScore.absRank = steamScore.rank;
          steamScore.isUser = pLeaderboardEntry.m_steamIDUser == steamId;
          string str = "";
          if (this.leaderboardNames.ContainsKey(value.m_hSteamLeaderboard.m_SteamLeaderboard))
            str = this.leaderboardNames[value.m_hSteamLeaderboard.m_SteamLeaderboard];
          if (this.scoreIsInThousands.Contains(str))
            steamScore.score *= 1000L;
          string[] strArray = str.Split('_');
          if (strArray.Length == 2)
          {
            try
            {
              steamScore.game = IGame.ParseGameType(strArray[0]);
              steamScore.disease = (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), strArray[1].ToUpper());
            }
            catch (Exception ex)
            {
              Debug.Log((object) ("Score failure: " + this.processingLeaderboard + " ex: " + (object) ex));
            }
          }
          scoreList.Add((IScores.Score) steamScore);
        }
      }
      if (this.processingType == ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends)
        this.friendScores[this.processingLeaderboard] = scoreList;
      else if (this.processingType == ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal)
        this.globalScores[this.processingLeaderboard] = scoreList;
      else if (this.processingType == ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser)
        this.userScores[this.processingLeaderboard] = scoreList;
    }
    else
      Debug.LogError((object) ("IO Failure on scores download: " + this.processingLeaderboard));
    this.processingLeaderboard = (string) null;
  }

  public override void AddScore(IScores.Score score)
  {
    this.StartCoroutine(this.UploadScore(score.GetLeaderboardName(), score.score, score.game == IGame.GameType.SpeedRun));
    this.AddLocalScore(score);
  }

  private IEnumerator UploadScore(string leaderboardName, long score, bool speedrun)
  {
    if (string.IsNullOrEmpty(leaderboardName))
    {
      Debug.LogWarning((object) "Score Error - Invalid Leaderboard Name");
    }
    else
    {
      if (this.scoreIsInThousands.Contains(leaderboardName))
        score /= 1000L;
      else if (score > 50000000L)
      {
        Debug.LogWarning((object) "Score Error - Value out of bounds");
        yield break;
      }
      int iscore = 0;
      iscore = score <= (long) int.MaxValue ? (int) score : int.MaxValue;
      if (!this.leaderboards.ContainsKey(leaderboardName))
      {
        while (!string.IsNullOrEmpty(this.loadingLeaderboard))
          yield return (object) null;
        this.loadingLeaderboard = leaderboardName;
        this.m_leaderboardFindResult.Set(SteamUserStats.FindOrCreateLeaderboard(leaderboardName, speedrun ? ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending : ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric));
        while (this.loadingLeaderboard == leaderboardName)
          yield return (object) null;
      }
      if (this.leaderboards.ContainsKey(leaderboardName))
      {
        DateTime now = DateTime.Now;
        int[] pScoreDetails = new int[3]
        {
          now.Day,
          now.Month,
          now.Year
        };
        Debug.Log((object) string.Format("INFO: Leaderboard entry for {0} of {1}.", (object) this.leaderboards[leaderboardName].m_SteamLeaderboard, (object) leaderboardName));
        this.m_leaderboardScoreUploaded.Set(SteamUserStats.UploadLeaderboardScore(this.leaderboards[leaderboardName], ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, iscore, pScoreDetails, pScoreDetails.Length));
      }
    }
  }

  private void LeaderboardScoreUploaded(LeaderboardScoreUploaded_t result, bool ioFailure)
  {
    Debug.Log((object) string.Format("INFO: Uploaded Score:\nScore: {0}\nNew Rank: {1}\nLeaderboard: {2}\nChanged: {3}\nSuccess? {4}", (object) result.m_nScore, (object) result.m_nGlobalRankNew, (object) result.m_hSteamLeaderboard, (object) result.m_bScoreChanged, (object) result.m_bSuccess));
  }

  public override void AddLocalScore(IScores.Score score)
  {
    string leaderboardName = score.GetLeaderboardName();
    if (string.IsNullOrEmpty(leaderboardName))
    {
      Debug.LogWarning((object) "Score Error - Invalid Leaderboard Name (Local)");
    }
    else
    {
      if (this.localScores == null)
        this.LoadPersonalLocal();
      this.localScores[leaderboardName] = !this.localScores.ContainsKey(leaderboardName) ? score : score.GetBestScore(this.localScores[leaderboardName]);
      this.SaveMyScores();
    }
  }

  private void SaveMyScores()
  {
    string data = "";
    foreach (IScores.Score score in this.localScores.Values)
      data = data + CUtils.Base64Encode(score.game.ToString() + ":" + (object) score.disease + ":" + (object) score.score) + "\n";
    CSavesSteam.SaveCloudData("scores.dat", data);
  }

  private List<IScores.Score> RemoveExtraScores(List<IScores.Score> scores)
  {
    bool flag = false;
    Dictionary<string, long> dictionary = new Dictionary<string, long>();
    for (int index = 0; index < scores.Count; ++index)
    {
      string key = scores[index].game.ToString() + "_" + scores[index].disease.ToString();
      if (dictionary.ContainsKey(key))
      {
        flag = true;
        if (scores[index].game == IGame.GameType.SpeedRun)
        {
          if (scores[index].score < dictionary[key])
            dictionary[key] = scores[index].score;
        }
        else if (scores[index].score > dictionary[key])
          dictionary[key] = scores[index].score;
      }
      else
        dictionary.Add(key, scores[index].score);
    }
    if (!flag)
      return scores;
    List<IScores.Score> scoreList = new List<IScores.Score>();
    foreach (string key in dictionary.Keys)
    {
      IScores.Score score = new IScores.Score();
      string[] strArray = key.Split('_');
      score.game = (IGame.GameType) Enum.Parse(typeof (IGame.GameType), strArray[0]);
      score.disease = (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), strArray[1]);
      score.score = dictionary[key];
      scoreList.Add(score);
    }
    return scoreList;
  }

  public void EraseOldFiles()
  {
    foreach (Disease.EDiseaseType ediseaseType in Enum.GetValues(typeof (Disease.EDiseaseType)) as Disease.EDiseaseType[])
    {
      string filename = "speedrun_" + ediseaseType.ToString() + ".dat";
      if (CSavesSteam.ExistsCloudData(filename))
        CSavesSteam.DeleteCloudData(filename);
    }
  }

  private class SteamScore : IScores.Score
  {
    public CSteamID steamID;

    public override string GetName() => SteamFriends.GetFriendPersonaName(this.steamID);
  }
}
