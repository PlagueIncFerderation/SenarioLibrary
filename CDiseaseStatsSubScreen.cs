// Decompiled with JetBrains decompiler
// Type: CDiseaseStatsSubScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CDiseaseStatsSubScreen : IGameSubScreen
{
  public UILabel startDate;
  public UILabel startLocation;
  public UILabel plagueType;
  public UILabel difficulty;
  public UILabel geneticComplexity;
  public UILabel dnaUsed;
  public UILabel dailyInfections;
  public UILabel dailyDeaths;
  public UILabel averageInfection;
  public UILabel averageDeaths;

  public override void Refresh()
  {
    base.Refresh();
    Disease disease = (this.gameScreen as CDiseaseScreen).disease;
    if (disease == null)
      return;
    if (disease.isCure)
    {
      string text = CLocalisationManager.GetText("No data");
      this.startDate.text = disease.cureDiseaseDiscoveredTurn == 0 ? text : CUtils.FormatDateShort(new DateTime(World.instance.startDate).AddDays((double) disease.cureDiseaseDiscoveredTurn));
      this.startLocation.text = disease.nexus == null || !disease.nexus.GetLocalDisease(disease).hasIntel ? CLocalisationManager.GetText("Unknown") : CLocalisationManager.GetText(disease.nexus.name);
      if (disease.diseaseNoticed)
      {
        this.plagueType.text = CGameManager.GetDiseaseNameLoc(disease.cureScenario);
        if (CGameManager.IsFederalScenario("PIFCURE"))
          this.plagueType.text = CGameManager.game.CurrentLoadedScenario.scenarioInformation.scenTitle;
        long num1 = disease.totalInfectedIntelGUI - disease.lastTotalInfectedIntelGUI;
        this.dailyInfections.text = num1 <= 0L ? "0" : num1.ToString("###################.");
        long num2 = disease.totalDeadIntelGUI - disease.lastTotalDeadIntelGUI;
        this.dailyDeaths.text = num2 <= 0L ? "0" : num2.ToString("###################.");
        this.geneticComplexity.text = CUtils.FormatValueToDisplay(disease.estimatedDeathRate * 100f, true);
        this.averageInfection.text = (double) disease.infectedWeekly <= 0.0 ? "0%" : CUtils.FormatValueToDisplay(disease.infectedWeekly, true);
        this.averageDeaths.text = (double) disease.deadWeekly <= 0.0 ? "0%" : CUtils.FormatValueToDisplay(disease.deadWeekly, true);
      }
      else
      {
        this.plagueType.text = text;
        this.dailyInfections.text = text;
        this.dailyDeaths.text = text;
        this.averageInfection.text = text;
        this.averageDeaths.text = text;
        this.geneticComplexity.text = text;
      }
      this.dnaUsed.text = disease.evoPointsSpent.ToString();
      this.difficulty.text = CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) disease.difficulty]);
    }
    else
    {
      this.startDate.text = disease.mutationCounter.ToString("f2") + " / " + disease.mutationTrigger.ToString("f2");
      this.startLocation.text = disease.nexus == null ? "" : CLocalisationManager.GetText(disease.nexus.name);
      this.plagueType.text = CGameManager.GetDiseaseNameLoc(disease.diseaseType);
      this.difficulty.text = CLocalisationManager.GetText(CGameManager.DifficultyNames[(uint) disease.difficulty]);
      this.geneticComplexity.text = Mathf.FloorToInt(disease.globalSeverity * (float) (disease.evoPoints * 2 + disease.evoPointsSpent)).ToString();
      this.dnaUsed.text = disease.evoPointsSpent.ToString();
      UILabel dailyInfections = this.dailyInfections;
      float num3;
      string str1;
      if (disease.infectedThisTurn > 0L)
      {
        num3 = Mathf.Max(0.0f, (float) disease.infectedThisTurn);
        str1 = num3.ToString("###################.");
      }
      else
        str1 = "0";
      dailyInfections.text = str1;
      UILabel dailyDeaths = this.dailyDeaths;
      string str2;
      if (disease.deadThisTurn > 0L)
      {
        num3 = Mathf.Max(0.0f, (float) disease.deadThisTurn);
        str2 = num3.ToString("###################.");
      }
      else
        str2 = "0";
      dailyDeaths.text = str2;
      long num4;
      if (disease.averageInfected > 0.0)
      {
        if (disease.averageInfected > 1.0)
        {
          UILabel averageInfection = this.averageInfection;
          num4 = (long) disease.averageInfected;
          string str3 = num4.ToString("###################.");
          averageInfection.text = str3;
        }
        else
          this.averageInfection.text = "1";
      }
      else
        this.averageInfection.text = "0";
      if (disease.averageDead > 0.0)
      {
        if (disease.averageDead > 1.0)
        {
          UILabel averageDeaths = this.averageDeaths;
          num4 = (long) disease.averageDead;
          string str4 = num4.ToString("###################.");
          averageDeaths.text = str4;
        }
        else
          this.averageDeaths.text = "1";
      }
      else
        this.averageDeaths.text = "0";
    }
  }
}
