// Decompiled with JetBrains decompiler
// Type: MPAIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MPAIController : MonoBehaviour
{
  private Technology evolveTarget;
  private MPDisease mpDisease;
  private Technology.ETechType targetTechType = Technology.ETechType.all;

  public void AIUpdate()
  {
    this.mpDisease = (MPDisease) World.instance.diseases[1];
    if (this.evolveTarget == null)
    {
      this.ChooseTargetTech();
      if (this.evolveTarget != null)
        Debug.Log((object) ("AI Chose: " + this.evolveTarget.name));
    }
    if (this.evolveTarget != null && this.mpDisease.GetEvolveCost(this.evolveTarget) <= this.mpDisease.evoPoints)
    {
      (CGameManager.game as MultiplayerGame).AIEvolveTech((Disease) this.mpDisease, this.evolveTarget);
      Debug.Log((object) ("AI EVOLVING: " + this.evolveTarget.name + " and now has " + (object) this.mpDisease.evoPoints + " evo points"));
      this.evolveTarget = (Technology) null;
    }
    else
    {
      if (this.evolveTarget == null || !this.GiveUpOnCurrentTech())
        return;
      this.evolveTarget = (Technology) null;
    }
  }

  private void ChooseTargetTech()
  {
    int num = CUtils.IntRand(0, 100);
    this.targetTechType = num > 35 ? (num > 70 ? Technology.ETechType.ability : Technology.ETechType.symptom) : Technology.ETechType.transmission;
    if ((double) this.mpDisease.globalInfectedPercent < 0.019999999552965164)
      this.targetTechType = Technology.ETechType.transmission;
    if (this.mpDisease.turnNumber > 50 && !this.mpDisease.IsTechEvolved("air_1") && !this.mpDisease.IsTechEvolved("water_1"))
    {
      this.evolveTarget = CUtils.IntRand(0, 2) >= 1 ? this.mpDisease.GetTechnology("air_1") : this.mpDisease.GetTechnology("water_1");
      if (this.evolveTarget != null)
        return;
    }
    if ((double) this.mpDisease.cureCompletePercent > 0.9 && (!this.mpDisease.IsTechEvolved("genetic_reshuffle_1") || !this.mpDisease.IsTechEvolved("genetic_reshuffle_2") || !this.mpDisease.IsTechEvolved("genetic_reshuffle_3") || !this.mpDisease.IsTechEvolved("genetic_reshuffle_4") || !this.mpDisease.IsTechEvolved("genetic_reshuffle_5")))
    {
      for (int index = 1; index <= 5; ++index)
      {
        if (!this.mpDisease.IsTechEvolved("genetic_reshuffle_" + (object) index))
        {
          this.evolveTarget = this.mpDisease.GetTechnology("genetic_reshuffle_" + (object) index);
          if (this.evolveTarget != null)
            return;
        }
      }
    }
    if (!this.mpDisease.IsTechEvolved("heat_resistance_1") && (double) this.GetInfectedCountryRatio(Country.Trait.Hot) > 0.5 && CUtils.IntRand(0, 10) < 5)
    {
      this.evolveTarget = this.mpDisease.GetTechnology("heat_resistance_1");
      if (this.evolveTarget != null)
        return;
    }
    if (!this.mpDisease.IsTechEvolved("cold_resistance_1") && (double) this.GetInfectedCountryRatio(Country.Trait.Cold) > 0.5 && CUtils.IntRand(0, 10) < 5)
    {
      this.evolveTarget = this.mpDisease.GetTechnology("cold_resistance_1");
      if (this.evolveTarget != null)
        return;
    }
    if (!this.mpDisease.IsTechEvolved("drug_resistance_1") && (double) this.GetInfectedCountryRatio(Country.Trait.Rich) > 0.5 && CUtils.IntRand(0, 10) < 8)
    {
      this.evolveTarget = this.mpDisease.GetTechnology("drug_resistance_1");
      if (this.evolveTarget != null)
        return;
    }
    List<Technology> technologyList = new List<Technology>();
    for (int index = 0; index < this.mpDisease.technologies.Count; ++index)
    {
      Technology technology = this.mpDisease.technologies[index];
      if ((technology.gridType == this.targetTechType || this.targetTechType == Technology.ETechType.all) && this.mpDisease.CanEvolve(technology) && !this.mpDisease.IsTechEvolved(technology))
        technologyList.Add(technology);
    }
    if (technologyList.Count <= 0)
      return;
    this.evolveTarget = technologyList[UnityEngine.Random.Range(0, technologyList.Count)];
  }

  private bool GiveUpOnCurrentTech()
  {
    return CUtils.IntRand(0, 100) < 1 || this.mpDisease.GetEvolveCost(this.evolveTarget) > 12 && CUtils.IntRand(0, 100) < 10 || (double) this.mpDisease.globalDeadPercent == 0.0 && (this.evolveTarget.id == "corpse_decomposition_1" || this.evolveTarget.id == "corpse_decomposition_2");
  }

  public float GetInfectedCountryRatio(Country.Trait trait)
  {
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < this.mpDisease.localDiseases.Count; ++index)
    {
      LocalDisease localDisease = this.mpDisease.localDiseases[index];
      Country country = localDisease.country;
      if (localDisease.allInfected > 0L)
      {
        ++num1;
        if (country.hasTrait(trait))
          ++num2;
      }
    }
    return num1 > 0 ? (float) num2 * 1f / (float) num1 : 0.0f;
  }

  public float GetUninfectedCountryRatio(Country.Trait trait)
  {
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < World.instance.countries.Count; ++index)
    {
      Country country = World.instance.countries[index];
      LocalDisease localDisease = country.localDiseases.Find((Predicate<LocalDisease>) (a => a.diseaseID == this.mpDisease.id));
      if (localDisease == null || localDisease.allInfected < 1L)
      {
        ++num1;
        if (country.hasTrait(trait))
          ++num2;
      }
    }
    return num1 > 0 ? (float) num2 * 1f / (float) num1 : 0.0f;
  }
}
