// Decompiled with JetBrains decompiler
// Type: MPWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MPWorld : World
{
  public bool allowMultipleInfections = true;
  [NonSerialized]
  public List<GemEffect> updatedGems = new List<GemEffect>();
  [NonSerialized]
  public List<GemEffect> activeGems = new List<GemEffect>();
  [NonSerialized]
  public Dictionary<Disease, MPCountry> updatedNexuses = new Dictionary<Disease, MPCountry>();
  [NonSerialized]
  public Dictionary<Disease, MPCountry> activeNexuses = new Dictionary<Disease, MPCountry>();
  [NonSerialized]
  public List<History> allHistory = new List<History>();
  public string gameOverDescription = "";
  private int lastTurn;
  private long lastTurnTick;
  private int fastForwardCount;
  private int lastDnaZero;
  private int lastDnaOne;

  public override Disease CreateDisease() => (Disease) new MPDisease();

  public override Country CreateCountry() => (Country) new MPCountry();

  public override void Clear()
  {
    base.Clear();
    if (CGameManager.gameType != IGame.GameType.VersusMP)
      return;
    MPDiseaseExternal.Cleanup();
  }

  public void CreateGemEffect(GemAbility ability, Disease d, Country c, Vector3? pos = null)
  {
    GemEffect gemEffect = new GemEffect(d, c, ability);
    gemEffect.position = pos;
    this.updatedGems.Add(gemEffect);
    this.activeGems.Add(gemEffect);
  }

  public void MoveGemEffect(GemEffect gem, Country to) => gem.SetCountry(to, 1);

  public void RemoveGemEffect(GemEffect gem)
  {
    this.updatedGems.Add(gem);
    this.activeGems.Remove(gem);
  }

  public override void DoGameUpdate(int turn)
  {
    this.DiseaseTurn = turn;
    if (this.lastTurn != turn)
    {
      long ticks = DateTime.Now.Ticks;
      long num = ticks - this.lastTurnTick;
      if (num >= 0L)
        Debug.Log((object) ("Turn " + turn.ToString() + " is " + num.ToString() + " ms in length"));
      if (num >= 0L && num <= 9500000L && (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.game.IsReplayActive && !((MultiplayerGame) CGameManager.game).IsAIGame)
        ++this.fastForwardCount;
      else
        this.fastForwardCount = 0;
      if (this.fastForwardCount >= 8 && (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
      {
        this.fastForwardCount = 0;
        CGameManager.cheatDetected = true;
        Debug.Log((object) "Fast forward cheating detected, end game!");
        CGameManager.game.EndGame(IGame.EndGameReason.CHEATING);
      }
      else if (this.fastForwardCount >= 8)
      {
        this.fastForwardCount = 0;
        Debug.Log((object) "No game found local!");
      }
      this.lastTurn = turn;
      this.lastTurnTick = ticks;
    }
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.game.IsReplayActive && !((MultiplayerGame) CGameManager.game).IsAIGame)
    {
      foreach (Disease disease in this.diseases)
      {
        if (disease.evoPoints >= 99999)
        {
          CGameManager.cheatDetected = true;
          Debug.Log((object) "DNA cheating detected, end game!");
          CGameManager.game.EndGame(IGame.EndGameReason.CHEATING);
        }
      }
    }
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.game.IsReplayActive && !((MultiplayerGame) CGameManager.game).IsAIGame)
    {
      if (this.diseases[0].evoPoints > this.lastDnaZero + 60 && !this.diseases[0].cureFlag)
      {
        CGameManager.cheatDetected = true;
        Debug.Log((object) ("DNA cheating detected, abnormal " + (this.diseases[0].evoPoints - this.lastDnaZero).ToString() + " DNA points for Disease[0] observed, end game!"));
        CGameManager.game.EndGame(IGame.EndGameReason.CHEATING);
      }
      else if (this.diseases[1].evoPoints > this.lastDnaOne + 60 && !this.diseases[1].cureFlag)
      {
        CGameManager.cheatDetected = true;
        Debug.Log((object) ("DNA cheating detected, abnormal " + (this.diseases[1].evoPoints - this.lastDnaOne).ToString() + " DNA points for Disease[1] observed, end game!"));
        CGameManager.game.EndGame(IGame.EndGameReason.CHEATING);
      }
      Debug.Log((object) ("DNA change for Disease[0] at turn " + turn.ToString() + " is " + (this.diseases[0].evoPoints - this.lastDnaZero).ToString()));
      Debug.Log((object) ("DNA change for Disease[1] at turn " + turn.ToString() + " is " + (this.diseases[1].evoPoints - this.lastDnaOne).ToString()));
      this.lastDnaZero = this.diseases[0].evoPoints;
      this.lastDnaOne = this.diseases[1].evoPoints;
    }
    if (this.MPCheckWinner())
      return;
    for (int index = 0; index < this.diseases.Count; ++index)
    {
      Disease disease = this.diseases[(index + this.DiseaseTurn) % this.diseases.Count];
      disease.turnNumber = this.DiseaseTurn;
      ++disease.recentEventCounter;
      disease.GameUpdate();
    }
    for (int index = 0; index < this.countries.Count; ++index)
    {
      Country country = this.countries[index];
      country.GameUpdate();
      this.CheckGovernmentActions(country);
    }
    this.MPAfterGameUpdate();
  }

  public void MPAfterGameUpdate()
  {
  }

  public bool MPCheckWinner()
  {
    if (this.DiseaseTurn > 10)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      for (int index = 0; index < this.diseases.Count; ++index)
      {
        Disease disease = this.diseases[(index + this.DiseaseTurn) % this.diseases.Count];
        if (disease.daysToGameWin < 10)
          disease.recentEventCounter = 0;
        --disease.daysToGameWin;
        if (disease.diseaseType == Disease.EDiseaseType.NECROA)
        {
          if (disease.totalInfected == 0L && disease.totalHealthy == 0L)
          {
            flag2 = flag3 = true;
            flag1 = true;
            this.gameOverDescription = "!!GAME OVER STATE[A] - NO INFECTED AND NO HEALTHY LEFT!! Disease[" + disease.name + "] has " + (object) disease.totalInfected + " infected and " + (object) disease.totalHealthy + " healthy";
            Debug.Log((object) this.gameOverDescription);
          }
          else if (disease.totalInfected == 0L && disease.totalZombie == 0L)
          {
            flag4 = true;
            flag1 = true;
            this.gameOverDescription = "!!GAME OVER STATE[B] - NO INFECTED AND NO ZOMBIES LEFT!! Disease[" + disease.name + "] has " + (object) disease.totalInfected + " infected and " + (object) disease.totalHealthy + " healthy";
            Debug.Log((object) this.gameOverDescription);
          }
        }
        else if (disease.totalInfected < 3L && disease.totalHealthy < 3L)
        {
          this.gameEndResult = IGame.EndGameResult.Dead;
          flag2 = true;
          flag1 = true;
          this.gameOverDescription = "!!GAME OVER STATE[C] - NO INFECTED LEFT!! Disease[" + disease.name + "] has " + (object) disease.totalInfected + " infected and " + (object) disease.totalHealthy + " healthy";
          Debug.Log((object) this.gameOverDescription);
        }
        else if (disease.totalInfected + disease.totalDead > World.instance.totalPopulation - 5L)
        {
          this.gameEndResult = IGame.EndGameResult.Infected;
          flag2 = true;
          flag1 = true;
          this.gameOverDescription = "!!GAME OVER STATE[D] - WORLD INFECTED!! Disease[" + disease.name + "] has " + (object) disease.totalInfected + " infected and " + (object) disease.totalDead + " dead";
          Debug.Log((object) this.gameOverDescription);
        }
        else if (World.instance.GetTotalInfected() < 5L && disease.turnNumber > 1)
        {
          if (disease.cureFlag)
            this.gameEndResult = IGame.EndGameResult.Cured;
          else
            this.gameEndResult = IGame.EndGameResult.Dead;
          flag1 = true;
          this.gameOverDescription = "!!GAME OVER STATE[F] - NO INFECTED LEFT!!";
          Debug.Log((object) this.gameOverDescription);
        }
      }
      bool flag6 = false;
      if (!flag2 && this.trackedHordes.Count > 0)
        flag6 = true;
      if (flag1 && !flag6)
      {
        long num1 = -1;
        for (int index = 0; index < this.diseases.Count; ++index)
        {
          Disease disease = this.diseases[index];
          if (disease.zdayOrDone)
          {
            disease.zombieWin = flag3;
            disease.zombieLoss = flag4;
          }
          long num2 = !flag5 ? (disease.diseaseType != Disease.EDiseaseType.NEURAX ? disease.totalInfected + disease.totalDead : disease.totalControlledInfected + disease.totalKilled) : (!disease.IsTechEvolved(disease.victoryTechId) ? disease.totalKilled : disease.totalControlledInfected + disease.totalKilled);
          if (num2 > num1)
          {
            num1 = num2;
            this.winner = disease;
          }
        }
        if (!flag2)
          this.winner = (Disease) null;
        this.gameWon = flag2;
        if (!this.gameEnded)
        {
          if (this.diseases[0].zdayOrDone)
            this.turnsUntilGameEnd = 5;
          else
            this.turnsUntilGameEnd = 0;
        }
        else if (this.turnsUntilGameEnd > 0)
          --this.turnsUntilGameEnd;
        this.gameEnded = true;
        return true;
      }
    }
    return false;
  }

  public override void UseActiveAbility(
    EAbilityType abilityType,
    Disease disease,
    IList<Vector3> positions,
    IList<Country> targetCountries,
    int presetInt = -1)
  {
    Debug.LogError((object) ("Ability " + (object) abilityType + " not supported in MP"));
  }

  public void CreateUnscheduledFlight(
    Disease disease,
    IList<Vector3> positions,
    IList<Country> targetCountries,
    int infected)
  {
    (disease as MPDisease).RecordActiveAbilityUse(EAbilityType.unscheduled_flight);
    Vehicle vehicle = Vehicle.Create();
    vehicle.delay = 0.0f;
    vehicle.type = Vehicle.EVehicleType.Airplane;
    vehicle.subType = Vehicle.EVehicleSubType.Unscheduled;
    vehicle.actingDisease = disease;
    vehicle.SetRoute(targetCountries[0], targetCountries[1], positions[0], positions[1]);
    Debug.Log((object) ("MPWorld.CreateUnscheduled [1]: " + infected.ToString() + "  for: " + disease.name + ", from:" + targetCountries[0].name + ", to:" + targetCountries[1].name));
    vehicle.AddInfected(disease, infected);
    for (int index = 0; index < World.instance.diseases.Count; ++index)
    {
      Disease disease1 = World.instance.diseases[index];
      if (disease1 != disease)
      {
        long uninfected = targetCountries[0].GetLocalDisease(disease1).uninfected;
        if (uninfected < (long) infected)
        {
          long number = (long) infected - uninfected;
          Debug.Log((object) ("MPWorld.CreateUnscheduled [2]: " + infected.ToString() + "  for: " + disease1.name + ", from:" + targetCountries[0].name + ", to:" + targetCountries[1].name));
          vehicle.AddInfected(disease1, (int) number);
        }
      }
    }
    this.AddVehicle(vehicle);
  }

  public void CreateNukeLaunch(
    Disease disease,
    IList<Vector3> positions,
    IList<Country> targetCountries)
  {
    Vehicle vehicle = Vehicle.Create();
    vehicle.delay = 0.0f;
    vehicle.type = Vehicle.EVehicleType.Missile;
    vehicle.subType = Vehicle.EVehicleSubType.Nuke;
    vehicle.actingDisease = disease;
    vehicle.SetRoute(targetCountries[0], targetCountries[1], positions[0], positions[1]);
    vehicle.actingDisease = disease;
    this.AddVehicle(vehicle);
  }

  public override void StartSimulation()
  {
    for (int index = 0; index < this.diseases.Count; ++index)
      this.diseases[index].FindSuperCureCountry();
    Debug.Log((object) "INITIALISE DISEASES");
    MPDiseaseData[] d = new MPDiseaseData[this.diseases.Count];
    MPCountryData[] c = new MPCountryData[this.countries.Count];
    MPLocalDiseaseData[] ld = new MPLocalDiseaseData[this.diseases.Count * this.countries.Count];
    for (int index1 = 0; index1 < this.diseases.Count; ++index1)
    {
      MPDisease disease = (MPDisease) this.diseases[index1];
      Debug.Log((object) ("---d: " + (object) disease.diseaseType + ", d.totalInfected:" + (object) disease.totalInfected));
      d[index1] = disease.mData;
      for (int index2 = 0; index2 < this.countries.Count; ++index2)
      {
        MPCountry country = (MPCountry) this.countries[index2];
        if (index1 == 0)
          c[index2] = country.mData;
        MPLocalDisease localDisease = (MPLocalDisease) disease.GetLocalDisease((Country) country);
        ld[index1 * this.countries.Count + index2] = localDisease.mData;
      }
    }
    Debug.Log((object) ("SETTING DISEASES: " + (object) d.Length));
    MPDiseaseExternal.SetDiseases(d);
    Debug.Log((object) ("SETTING COUNTRIES: " + (object) c.Length));
    MPDiseaseExternal.SetCountries(c);
    Debug.Log((object) ("SETTING LOCAL DISEASES: " + (object) ld.Length));
    MPDiseaseExternal.SetLocalDiseases(ld);
  }

  public override void SetNexus(Disease forDisease, Country forCountry)
  {
    forDisease.SetNexus(forCountry);
    this.nexusCountries.Add(forCountry);
    this.UpdateNexus(forDisease, forCountry);
    if (forDisease.GetLocalDisease(forCountry).allInfected >= 1L)
      return;
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, 0, 0, forDisease, "INITIAL TRANSFER OF POP: " + forDisease.name + " in " + forCountry.name);
    forDisease.nexus.TransferPopulation(100.0, Country.EPopulationType.HEALTHY, forDisease, Country.EPopulationType.INFECTED);
    Debug.Log((object) ("______forDisease:" + (object) forDisease.totalInfected));
  }

  public void UpdateNexus(Disease disease, Country country)
  {
    if (!this.nexusCountries.Contains(country))
      return;
    this.updatedNexuses[disease] = country as MPCountry;
  }

  public void TryCreateNexusDNABubble(Disease disease, Country country)
  {
    if (!this.nexusCountries.Contains(country))
      return;
    this.activeNexuses[disease] = country as MPCountry;
  }
}
