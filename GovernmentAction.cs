// Decompiled with JetBrains decompiler
// Type: GovernmentAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class GovernmentAction
{
  public string id;
  public string actionName;
  public GovernmentAction.EActionDiseaseType type;
  public float triggerLocalPriority;
  public float triggerGlobalPriority;
  public float conditionPopulationDead;
  public float conditionLocalInfectedPercent;
  public float conditionGlobalInfectedPercent;
  public float conditionLocalInfectedAndDeadPerc;
  public float conditionDiseaseSeverity;
  public float conditionPopulationZombie;
  public float conditionPopulationDeadOrZombie;
  public string conditionActionTaken;
  public string conditionActionNotTaken;
  public string conditionTechResearched;
  public string techRequirementOr;
  public string conditionTechNotResearched;
  public int conditionIsFort;
  public int conditionHasAirport;
  public int conditionHasPort;
  public float conditionRandom;
  public int conditionZday;
  public int conditionZdayDone;
  public bool randomIncreaseFlag;
  public float changeLocalInfectiousness;
  public float changeLocalSeverity;
  public float changeLocalLethality;
  public int changeAirBorders;
  public int changeLandBorders;
  public int changeSeaBorders;
  public float changeCorpseTransmission;
  public float changePublicOrder;
  public float changeResearchAllocation;
  public int changeAllowGovAction;
  public float changeCombatStrength;
  public float changeMaxCombatStrength;
  public bool newsFlag;
  public int newsPriority;
  public string newsContent;
  public string newsCategory;
  public bool ignorePriorityCounter;
  public int triggerApePriorityLevelLocal;
  public int triggerApePriorityLevelGlobal;
  public int conditionLabStatus;
  public float conditionApeInfectedPercent;
  public float conditionApeTotalAlivePopulation;
  public float conditionApeDeadPercent;
  public float changeLocalDanger;
  public float changeGovLocalApeInfectiousness;
  public float changeGovLocalApeLethality;
  public float conditionNumZombie;
  public float conditionLocalHealthyPercent;
  public int conditionIsShadowDayDone;
  public int conditionIsLab;
  public float conditionLocalVampActivity;
  public float conditionGlobalVampActivity;
  public int conditionVcomAlert;
  public float changeGovDeadBodyTransmission;
  public string conditionHqOrNeighbourActionTakenOr;
  public int changeIntel;
  public int lastFired;
  public int conditionInterval;
  public float changeDetectedChance;
  public float changeAuthorityGain;
  public float changeMedicalCapacity;
  public float economicDamagePerTurn;
  public float changeEconomyMax;
  public float changeCompliancePercMod;
  public string newsContentDestroyed;
  public float conditionMaxPublicOrder = 1f;
  public float conditionCompliance = 1f;
  public bool removable;
  public bool priorityImpactOdds;
  public float changeLocalInfectivityReductionPerc;
  public float changeLocalContactTracingPop;
  public float changeEconomyDefense;
  public string conditionVariable;
  public string conditionVariableOverride;
  public string conditionActionTakenOr;
  public string conditionActionNotTakenOr;
  public string conditionContinent;
  public Country.EContinentType conditionContinentType;
  public IDictionary<string, float> bonuses = (IDictionary<string, float>) new Dictionary<string, float>();
  public string[] conditionActionTakenArray;
  public string[] conditionActionNotTakenArray;
  public string[] conditionActionTakenOrArray;
  public string[] techRequirementOrArray;
  public string[] conditionTechResearchedArray;
  public string[] conditionHqOrNeighbourActionTakenOrArray;
  private int triggerDay;

  public static GovernmentAction.EActionDiseaseType GetType(Disease.EDiseaseType diseaseType)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        return GovernmentAction.EActionDiseaseType.NEURAX;
      case Disease.EDiseaseType.NECROA:
        return GovernmentAction.EActionDiseaseType.NECROA;
      case Disease.EDiseaseType.SIMIAN_FLU:
        return GovernmentAction.EActionDiseaseType.SIMIAN_FLU;
      case Disease.EDiseaseType.CURE:
        return GovernmentAction.EActionDiseaseType.CURE;
      default:
        return GovernmentAction.EActionDiseaseType.STANDARD;
    }
  }

  public static string GetData(Disease.EDiseaseType diseaseType)
  {
    return CGameManager.LoadGameText(GovernmentAction.GetPath(diseaseType));
  }

  public static string GetPath(Disease.EDiseaseType diseaseType)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        return "World/GovernmentActions/govactions_neurax";
      case Disease.EDiseaseType.NECROA:
        return "World/GovernmentActions/govactions_zombie";
      case Disease.EDiseaseType.SIMIAN_FLU:
        return "World/GovernmentActions/govactions_simian_flu";
      case Disease.EDiseaseType.TUTORIAL:
        return "World/GovernmentActions/govactions_tutorial";
      case Disease.EDiseaseType.VAMPIRE:
        return "World/GovernmentActions/govactions_vampire";
      case Disease.EDiseaseType.FAKE_NEWS:
        return "World/GovernmentActions/govactions_fake_news";
      case Disease.EDiseaseType.CURE:
        return "World/GovernmentActions/govactions_cure";
      case Disease.EDiseaseType.CURETUTORIAL:
        return "World/GovernmentActions/govactions_cure";
      default:
        return "World/GovernmentActions/govactions_standard";
    }
  }

  public GovernmentAction.EActionLevel actionLevel
  {
    get
    {
      return (double) this.changeResearchAllocation == 0.0 ? GovernmentAction.EActionLevel.COUNTRY : GovernmentAction.EActionLevel.DISEASE;
    }
  }

  public float GetRandomBonus(Disease d)
  {
    return this.bonuses.ContainsKey(d.name) ? this.bonuses[d.name] : 0.0f;
  }

  public void IncreaseRandomBonus(Disease d, float f)
  {
    if (!this.bonuses.ContainsKey(d.name))
      this.bonuses[d.name] = f;
    else
      this.bonuses[d.name] += f;
  }

  public void OnImport()
  {
    if (!string.IsNullOrEmpty(this.techRequirementOr) && this.techRequirementOr != "0")
      this.techRequirementOrArray = this.techRequirementOr.Split(',');
    else
      this.techRequirementOrArray = (string[]) null;
    if (!string.IsNullOrEmpty(this.conditionTechResearched) && this.conditionTechResearched != "0")
      this.conditionTechResearchedArray = this.conditionTechResearched.Split(',');
    else
      this.conditionTechResearchedArray = (string[]) null;
    if (!string.IsNullOrEmpty(this.conditionActionTaken) && this.conditionActionTaken != "0")
      this.conditionActionTakenArray = this.conditionActionTaken.Split(',');
    else
      this.conditionActionTakenArray = (string[]) null;
    if (!string.IsNullOrEmpty(this.conditionActionNotTaken) && this.conditionActionNotTaken != "0")
      this.conditionActionNotTakenArray = this.conditionActionNotTaken.Split(',');
    else
      this.conditionActionNotTakenArray = (string[]) null;
    if (!string.IsNullOrEmpty(this.conditionActionTakenOr) && this.conditionActionTakenOr != "0")
      this.conditionActionTakenOrArray = this.conditionActionTakenOr.Split(',');
    else
      this.conditionActionTakenOrArray = (string[]) null;
    if (!string.IsNullOrEmpty(this.conditionHqOrNeighbourActionTakenOr) && this.conditionHqOrNeighbourActionTakenOr != "0")
      this.conditionHqOrNeighbourActionTakenOrArray = this.conditionHqOrNeighbourActionTakenOr.Split(',');
    else
      this.conditionHqOrNeighbourActionTakenOrArray = (string[]) null;
    this.conditionContinentType = Country.EContinentType.NONE;
    if (string.IsNullOrEmpty(this.conditionContinent))
      return;
    this.conditionContinentType = DataImporter.ParseContinent(this.conditionContinent);
  }

  public int GetDate() => this.triggerDay;

  public void SetTriggerDate(int day) => this.triggerDay = day;

  public enum EActionDiseaseType
  {
    ALL,
    STANDARD,
    NEURAX,
    NECROA,
    SIMIAN_FLU,
    CURE,
  }

  public enum EActionLevel
  {
    COUNTRY,
    DISEASE,
  }
}
