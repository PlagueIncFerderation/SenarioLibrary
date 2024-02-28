// Decompiled with JetBrains decompiler
// Type: GovernmentActionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GovernmentActionManager
{
  public List<GovernmentAction> actions = new List<GovernmentAction>();
  private HashSet<GovernmentAction.EActionDiseaseType> actionTypesLoaded = new HashSet<GovernmentAction.EActionDiseaseType>();
  private Dictionary<Disease.EDiseaseType, GovernmentAction.EActionDiseaseType> diseaseActionLookup = new Dictionary<Disease.EDiseaseType, GovernmentAction.EActionDiseaseType>()
  {
    {
      Disease.EDiseaseType.NECROA,
      GovernmentAction.EActionDiseaseType.NECROA
    },
    {
      Disease.EDiseaseType.NEURAX,
      GovernmentAction.EActionDiseaseType.NEURAX
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      GovernmentAction.EActionDiseaseType.SIMIAN_FLU
    },
    {
      Disease.EDiseaseType.CURE,
      GovernmentAction.EActionDiseaseType.CURE
    }
  };

  public void LoadActions(string actionData, GovernmentAction.EActionDiseaseType actionType)
  {
    if (this.actionTypesLoaded.Contains(actionType))
      return;
    this.actionTypesLoaded.Add(actionType);
    if (string.IsNullOrEmpty(actionData))
      return;
    this.actions.AddRange((IEnumerable<GovernmentAction>) DataImporter.ImportGovernmentActions(actionData, actionType));
  }

  public void CheckGovernmentActions(Country c)
  {
    List<GovernmentActionManager.ActionTaken> actionTakenList = new List<GovernmentActionManager.ActionTaken>();
    for (int index1 = 0; index1 < World.instance.diseases.Count; ++index1)
    {
      Disease disease = World.instance.diseases[index1];
      LocalDisease localDisease = disease.GetLocalDisease(c);
      for (int index2 = 0; index2 < this.actions.Count; ++index2)
      {
        if (this.ActionPossible(c, disease, localDisease, this.actions[index2]))
          actionTakenList.Add(new GovernmentActionManager.ActionTaken()
          {
            action = this.actions[index2],
            disease = disease
          });
      }
    }
    if (actionTakenList.Count <= 0)
      return;
    GovernmentActionManager.ActionTaken actionTaken = actionTakenList[Random.Range(0, actionTakenList.Count - 1)];
    this.PerformGovernmentAction(c, actionTaken.disease, actionTaken.action);
  }

  public bool ActionPossible(Country c, Disease d, LocalDisease ld, GovernmentAction a)
  {
    if (!c.govActionsAllowed || CGameManager.IsFederalScenario("时生虫ReCRAFT") && ((double) d.customGlobalVariable2 >= 1.5 || c.totalInfected + c.totalDead < 1L))
      return false;
    if (d.diseaseType == Disease.EDiseaseType.NECROA)
    {
      if (a.type != GovernmentAction.EActionDiseaseType.NECROA && a.type != GovernmentAction.EActionDiseaseType.ALL)
        return false;
    }
    else if (d.diseaseType == Disease.EDiseaseType.NEURAX)
    {
      if (a.type != GovernmentAction.EActionDiseaseType.NEURAX && a.type != GovernmentAction.EActionDiseaseType.ALL)
        return false;
    }
    else if (d.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
    {
      if (a.type != GovernmentAction.EActionDiseaseType.SIMIAN_FLU && a.type != GovernmentAction.EActionDiseaseType.ALL)
        return false;
    }
    else if (d.diseaseType == Disease.EDiseaseType.CURE)
    {
      if (a.type != GovernmentAction.EActionDiseaseType.CURE && a.type != GovernmentAction.EActionDiseaseType.ALL)
        return false;
    }
    else if (a.type != GovernmentAction.EActionDiseaseType.STANDARD && a.type != GovernmentAction.EActionDiseaseType.ALL)
      return false;
    bool flag1 = this.MeetsOverrideConditions(a, ld);
    if (!flag1 && !a.ignorePriorityCounter && d.priorityCounter < 1 || a.conditionHasAirport > 0 && !c.hasAirport || a.conditionHasPort > 0 && !c.hasPorts || a.conditionHasAirport < 0 && c.hasAirport || a.conditionHasPort < 0 && c.hasPorts || !flag1 && !this.MeetsPriorityCondition(a, ld) || (double) a.conditionPopulationDead > 0.0 && (double) c.deadPercent < (double) a.conditionPopulationDead || (double) a.conditionGlobalInfectedPercent > 0.0 && (double) d.globalInfectedPercent < (double) a.conditionGlobalInfectedPercent || (double) a.conditionLocalInfectedPercent > 0.0 && (double) c.infectedPercent < (double) a.conditionLocalInfectedPercent || (double) a.conditionLocalInfectedAndDeadPerc > 0.0 && (double) c.infectedPercent + (double) c.deadPercent < (double) a.conditionLocalInfectedAndDeadPerc || (double) a.conditionDiseaseSeverity > 0.0 && (double) ld.localSeverity < (double) a.conditionDiseaseSeverity || (double) a.conditionPopulationZombie > 0.0 && (double) c.zombiePercent < (double) a.conditionPopulationZombie || (double) a.conditionPopulationDeadOrZombie > 0.0 && (double) c.zombieOrDeadPercent < (double) a.conditionPopulationDeadOrZombie || !string.IsNullOrEmpty(a.conditionTechNotResearched) && a.conditionTechNotResearched != "0" && d.IsTechEvolved(a.conditionTechNotResearched) || a.conditionZday > 0 && !d.zday || a.conditionZday < 0 && d.zday || a.conditionZdayDone > 0 && !d.zdayDone || a.conditionZdayDone < 0 && d.zdayDone || a.conditionIsFort > 0 && !c.HasFort() || a.conditionIsFort < 0 && c.HasFort() || d.diseaseType == Disease.EDiseaseType.SIMIAN_FLU && ((a.triggerApePriorityLevelGlobal <= 0 || (double) d.apePriorityLevelGlobal * (double) c.importance <= (double) a.triggerApePriorityLevelGlobal) && (a.triggerApePriorityLevelLocal <= 0 || (double) ld.apePriorityLevelLocal <= (double) a.triggerApePriorityLevelLocal) && (a.triggerApePriorityLevelLocal > 0 || a.triggerApePriorityLevelGlobal > 0) || a.conditionLabStatus == 1 && !c.hasApeLab || a.conditionLabStatus == -1 && c.hasApeLab || (double) ld.apeInfectedPercent < (double) a.conditionApeInfectedPercent || (double) ld.apeTotalAlivePopulation < (double) a.conditionApeTotalAlivePopulation || (double) d.apeTotalDeadPercent < (double) a.conditionApeDeadPercent) || d.diseaseType == Disease.EDiseaseType.VAMPIRE && ((double) ld.zombie < (double) a.conditionNumZombie || a.conditionIsShadowDayDone > 0 && !d.shadowDayDone || a.conditionIsShadowDayDone < 0 && d.shadowDayDone || (double) c.healthyPercent < (double) a.conditionLocalHealthyPercent || a.conditionIsLab == 1 && !c.hasApeLab || a.conditionIsLab == -1 && c.hasApeLab || (double) a.conditionVcomAlert > (double) d.vcomAlert || a.conditionVcomAlert == -1 && (double) d.vcomAlert > 1.0 || ((double) a.conditionGlobalVampActivity <= 0.0 || (double) d.vampireActivity * (double) c.importance <= (double) a.conditionGlobalVampActivity) && ((double) a.conditionLocalVampActivity <= 0.0 || (double) ld.localVampireActivity <= (double) a.conditionLocalVampActivity) && ((double) a.conditionLocalVampActivity > 0.0 || (double) a.conditionGlobalVampActivity > 0.0)) || d.isCure && ((double) c.publicOrder > (double) a.conditionMaxPublicOrder || (double) c.healthyPercent + (double) c.healthyRecoveredPercent < (double) a.conditionLocalHealthyPercent || (double) a.conditionCompliance < 1.0 && (double) ld.compliance > (double) a.conditionCompliance && !ld.borderOrLockdownOverride || a.conditionIsLab < 1 && !ld.hasIntel || a.conditionContinentType != Country.EContinentType.NONE && c.continentType != a.conditionContinentType) || a.actionLevel == GovernmentAction.EActionLevel.COUNTRY && c.IsActionTaken(a) || a.actionLevel == GovernmentAction.EActionLevel.DISEASE && ld.IsActionTaken(a) || !this.MeetsConditions(a, ld) || a.conditionInterval > 0 && (double) (d.turnNumber - c.GetLastFired(a.id)) < (double) a.conditionInterval)
      return false;
    if (!flag1)
    {
      if (a.conditionTechResearchedArray != null && a.conditionTechResearchedArray.Length != 0)
      {
        bool flag2 = true;
        foreach (string conditionTechResearched in a.conditionTechResearchedArray)
        {
          if (!d.IsTechEvolved(conditionTechResearched))
          {
            flag2 = false;
            break;
          }
        }
        if (!flag2)
          return false;
      }
      if (a.techRequirementOrArray != null && a.techRequirementOrArray.Length != 0)
      {
        bool flag3 = false;
        foreach (string techRequirementOr in a.techRequirementOrArray)
        {
          if (d.IsTechEvolved(techRequirementOr))
          {
            flag3 = true;
            break;
          }
        }
        if (!flag3)
          return false;
      }
      if (a.conditionActionTakenArray != null)
      {
        foreach (string conditionActionTaken in a.conditionActionTakenArray)
        {
          if (!c.IsActionTaken(conditionActionTaken) && !ld.IsActionTaken(conditionActionTaken))
            return false;
        }
      }
      if (a.conditionActionNotTakenArray != null)
      {
        foreach (string conditionActionNotTaken in a.conditionActionNotTakenArray)
        {
          if (c.IsActionTaken(conditionActionNotTaken) || ld.IsActionTaken(conditionActionNotTaken))
            return false;
        }
      }
      if (a.conditionActionTakenOrArray != null)
      {
        bool flag4 = false;
        foreach (string conditionActionTakenOr in a.conditionActionTakenOrArray)
        {
          if (c.IsActionTaken(conditionActionTakenOr) || ld.IsActionTaken(conditionActionTakenOr))
          {
            flag4 = true;
            break;
          }
        }
        if (!flag4)
          return false;
      }
    }
    if (CGameManager.IsFederalScenario("时生虫ReMASTER"))
    {
      a.conditionRandom = 0.02f;
      if ((double) World.instance.diseases[0].customGlobalVariable2 >= 0.5)
        a.conditionRandom = 1f / 1000f;
    }
    float conditionRandom = a.conditionRandom;
    if (a.priorityImpactOdds)
    {
      float a1 = 1f;
      float b = 1f;
      if ((double) ld.localPriority > (double) a.triggerLocalPriority && (double) a.triggerLocalPriority > 0.0)
        a1 = (float) (1.0 + ((double) ld.localPriority - (double) a.triggerLocalPriority) / (double) a.triggerLocalPriority);
      if ((double) d.globalPriority > (double) a.triggerGlobalPriority && (double) a.triggerGlobalPriority > 0.0)
        b = (float) (1.0 + ((double) d.globalPriority - (double) a.triggerGlobalPriority) / (double) a.triggerGlobalPriority);
      conditionRandom *= Mathf.Max(a1, b);
    }
    if (a.priorityImpactOdds)
      conditionRandom *= ld.govActionOddsMulti;
    if (!flag1 && (double) ModelUtils.FloatRand(0.0f, 1f) > (double) conditionRandom + (double) a.GetRandomBonus(d))
    {
      if (a.randomIncreaseFlag)
        a.IncreaseRandomBonus(d, 0.01f);
      return false;
    }
    if (a.conditionHqOrNeighbourActionTakenOrArray != null && a.conditionHqOrNeighbourActionTakenOrArray.Length != 0 && c != d.hqCountry)
    {
      bool flag5 = false;
      foreach (string neighbourActionTakenOr in a.conditionHqOrNeighbourActionTakenOrArray)
      {
        bool flag6 = false;
        foreach (Country neighbour in c.neighbours)
        {
          LocalDisease localDisease = neighbour.GetLocalDisease(d);
          if (localDisease.AreBordersOpen() && localDisease.actionsTaken.Contains(neighbourActionTakenOr))
          {
            flag6 = true;
            break;
          }
        }
        if (!flag6)
        {
          foreach (TravelRoute airRoute in c.airRoutes)
          {
            Country country = airRoute.forward ? airRoute.destination : airRoute.source;
            if (country.actionsTaken.Contains(neighbourActionTakenOr) && country.GetLocalDisease(d).AreAirportsOpen() && (double) airRoute.frequency * 10.0 < (double) ModelUtils.FloatRand(0.0f, 1f))
            {
              flag6 = true;
              break;
            }
          }
        }
        if (!flag6)
        {
          foreach (TravelRoute seaRoute in c.seaRoutes)
          {
            Country country = seaRoute.forward ? seaRoute.destination : seaRoute.source;
            if (country.actionsTaken.Contains(neighbourActionTakenOr) && country.GetLocalDisease(d).ArePortsOpen() && (double) seaRoute.frequency * 10.0 < (double) ModelUtils.FloatRand(0.0f, 1f))
            {
              flag6 = true;
              break;
            }
          }
        }
        flag5 = flag6;
        if (!flag5)
          break;
      }
      if (!flag5)
        return false;
    }
    return true;
  }

  public GovernmentAction.EActionDiseaseType GetActionType(Disease d)
  {
    return this.diseaseActionLookup.ContainsKey(d.diseaseType) ? this.diseaseActionLookup[d.diseaseType] : GovernmentAction.EActionDiseaseType.STANDARD;
  }

  public GovernmentAction FindAction(string name, Disease disease)
  {
    GovernmentAction.EActionDiseaseType actionType = this.GetActionType(disease);
    for (int index = 0; index < this.actions.Count; ++index)
    {
      GovernmentAction action = this.actions[index];
      if (action.id == name && (action.type == actionType || action.type == GovernmentAction.EActionDiseaseType.ALL))
        return action;
    }
    return (GovernmentAction) null;
  }

  public bool MeetsConditions(GovernmentAction ga, LocalDisease ld)
  {
    Country country = ld.country;
    if (ga.conditionVariable == "lockdown_override" && ga.conditionContinentType == country.continentType)
      return ld.lockdownOverride;
    if (ga.conditionVariable == "land_border_override" && ga.conditionContinentType == country.continentType)
      return ld.landBorderOverride;
    if (ga.conditionVariable == "airport_override" && ga.conditionContinentType == country.continentType)
      return ld.airportOverride;
    if (ga.conditionVariable == "port_override" && ga.conditionContinentType == country.continentType)
      return ld.portOverride;
    if (ga.conditionVariable == "border_or_lockdown_override")
      return ld.borderOrLockdownOverride;
    if (!(ga.conditionVariable == "disease_noticed"))
      return true;
    int num = ld.disease.diseaseNoticed ? 1 : 0;
    return true;
  }

  public bool MeetsOverrideConditions(GovernmentAction ga, LocalDisease ld)
  {
    if (ga.conditionVariableOverride == "contact_tracing_override" && ld.contactTracingOverride || ga.conditionVariableOverride == "border_or_lockdown_override" && ld.borderOrLockdownOverride || ga.conditionVariableOverride == "lockdown_aa_active" && ld.lockdownAAActive)
      return true;
    return ga.conditionVariableOverride == "lockdown_aa_upgraded" && ld.lockdownAAUpgraded;
  }

  public bool MeetsPriorityCondition(GovernmentAction ga, LocalDisease ld)
  {
    float num = 1f;
    Disease disease = ld.disease;
    Country country = ld.country;
    if (disease.isCure)
      num = Mathf.Pow(ld.economy, 0.5f);
    return ((double) ga.triggerGlobalPriority > 0.0 && (double) disease.globalPriority * (double) country.importance * (double) num > (double) ga.triggerGlobalPriority || (double) ga.triggerLocalPriority > 0.0 && (double) ld.localPriority > (double) ga.triggerLocalPriority ? 0 : ((double) ga.triggerLocalPriority > 0.0 ? 1 : ((double) ga.triggerGlobalPriority > 0.0 ? 1 : 0))) == 0;
  }

  public void PerformGovernmentAction(Country c, Disease d, GovernmentAction a, bool remove = false)
  {
    int num = 1;
    if (remove)
      num = -1;
    bool flag1 = num > 0;
    bool flag2 = !flag1;
    if (c.IsActionTaken(a) == remove)
    {
      if (!remove && (a.removable || (double) a.economicDamagePerTurn > 0.0))
        c.actionsSpecialInterest.Add(a.id);
      if (remove)
        c.actionsSpecialInterest.Remove(a.id);
      c.govLocalInfectiousness += a.changeLocalInfectiousness * (float) num;
      c.govLocalSeverity += a.changeLocalSeverity * (float) num;
      c.govLocalLethality += a.changeLocalLethality * (float) num;
      if (CGameManager.IsFederalScenario("时生虫ReCRAFT"))
        c.govLocalInfectiousness -= d.globalInfectiousnessMax;
      if ((double) c.govLocalCorpseTransmission >= 0.0)
        c.govLocalCorpseTransmission += a.changeCorpseTransmission * (float) num;
      c.govPublicOrder += a.changePublicOrder * (float) num;
      c.localHumanCombatStrength += a.changeCombatStrength * (float) num;
      c.localHumanCombatStrengthMax += a.changeMaxCombatStrength * (float) num;
      if (d.diseaseType == Disease.EDiseaseType.SIMIAN_FLU)
      {
        c.localDanger += a.changeLocalDanger * (float) num;
        c.govLocalApeInfectiousness += a.changeGovLocalApeInfectiousness * (float) num;
        c.govLocalApeLethality += a.changeGovLocalApeLethality * (float) num;
      }
      if (d.diseaseType == Disease.EDiseaseType.FAKE_NEWS)
        c.localDanger += a.changeLocalDanger * (float) num;
      c.govDeadBodyTransmission += a.changeGovDeadBodyTransmission * (float) num;
      if (a.changeLandBorders != 0)
        c.borderStatus = a.changeLandBorders > 0 ? flag1 : flag2;
      if (a.changeAirBorders != 0)
        c.airportStatus = a.changeAirBorders > 0 ? flag1 : flag2;
      if (a.changeSeaBorders != 0)
        c.portStatus = a.changeSeaBorders > 0 ? flag1 : flag2;
      if (a.changeAllowGovAction != 0)
        c.govActionsAllowed = a.changeAllowGovAction > 0 ? flag1 : flag2;
      if (d.isCure)
      {
        c.medicalCapacity += c.startingMedicalCapacity * a.changeMedicalCapacity * (float) num;
        c.localDanger += a.changeLocalDanger * (float) num;
      }
    }
    if (a.newsFlag)
    {
      string text = a.newsContent;
      if (remove)
        text = a.newsContentDestroyed;
      if (!string.IsNullOrEmpty(text))
      {
        ParameterisedString n = new ParameterisedString(text, new string[1]
        {
          "country.name"
        });
        World.instance.AddNewsItem(new IGame.NewsItem(n, d, c, a.newsPriority));
      }
    }
    if (a.actionLevel == GovernmentAction.EActionLevel.DISEASE || d.isCure)
    {
      LocalDisease localDisease = d.GetLocalDisease(c);
      localDisease.cureResearchAllocation += a.changeResearchAllocation;
      if (a.changeIntel > 0)
        localDisease.hasIntel = flag1;
      else if (a.changeIntel < 0)
        localDisease.hasIntel = flag2;
      if (d.isCure)
      {
        localDisease.authority += a.changeAuthorityGain * (float) num;
        localDisease.economyMAX += a.changeEconomyMax * (float) num;
        localDisease.economyDamage += a.economicDamagePerTurn * (float) num;
        localDisease.economyDefense += a.changeEconomyDefense * (float) num;
        localDisease.compliancePercMod += a.changeCompliancePercMod * (float) num;
        localDisease.localInfectivityReductionPerc += a.changeLocalInfectivityReductionPerc * (float) num;
        localDisease.localContactTracingPop += a.changeLocalContactTracingPop * (float) num;
      }
      if (remove)
        localDisease.RemoveActionTaken(a);
      else
        localDisease.SetActionTaken(a);
    }
    else if ((double) a.changeResearchAllocation > 0.0)
      Debug.LogWarning((object) "Government action set to 'country' scope tried to affect research allocation.");
    if (remove)
      c.RemoveActionTaken(a);
    else
      c.SetActionTaken(a);
  }

  public struct ActionTaken
  {
    public GovernmentAction action;
    public Disease disease;
  }
}
