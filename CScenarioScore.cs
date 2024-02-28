// Decompiled with JetBrains decompiler
// Type: CScenarioScore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CScenarioScore
{
  public string scenarioID;
  public int score;
  public int cons;
  public double rating;

  public CScenarioScore()
  {
  }

  private CScenarioScore(string name, int s, int c, double rat)
  {
    this.scenarioID = name;
    this.score = s;
    this.cons = c;
    this.rating = rat;
  }

  public static int CompareRating(CScenarioScore s1, CScenarioScore s2)
  {
    if (s1.rating < s2.rating)
      return 1;
    return s1.rating > s2.rating ? -1 : 0;
  }
}
