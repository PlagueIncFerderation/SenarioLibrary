// Decompiled with JetBrains decompiler
// Type: BonusIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BonusIcon
{
  private static int idCounter;
  public float showStart;
  public int id;
  public Vector3 position;
  public Vector3 spawnPosition;
  public Disease disease;
  public Country country;
  public BonusIcon.EBonusIconType type;
  public float delay;
  public bool disableAutoHide;
  public int bornTurn;
  public float showTime;
  public int extraEvo;
  public bool forceEvo;
  public bool musicBubble;
  public float musicImportance;

  public static int GetCounter() => BonusIcon.idCounter;

  public static void ResetCounter(int to = 0) => BonusIcon.idCounter = to;

  public BonusIcon() => this.id = ++BonusIcon.idCounter;

  public BonusIcon(Disease disease, Country country, BonusIcon.EBonusIconType bonusIconType)
  {
    this.disease = disease;
    this.country = country;
    this.type = bonusIconType;
    this.id = ++BonusIcon.idCounter;
    this.delay = bonusIconType != BonusIcon.EBonusIconType.INFECT ? 0.0f : ModelUtils.FloatRand(0.0f, 1.5f);
    this.showTime = this.GetBubbleShowTime(disease);
    if (disease.isCure)
      this.disableAutoHide = true;
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, World.instance.DiseaseTurn, World.instance.eventTurn, disease, "----" + (object) World.instance.DiseaseTurn + "/" + (object) World.instance.eventTurn + " - CREATED: " + (object) this);
  }

  private float GetBubbleShowTime(Disease d)
  {
    return !d.isVampire ? 5f : Mathf.Clamp((float) (13.0 + 6.0 * (double) d.totalZombie), 10f, 30f);
  }

  public override string ToString()
  {
    return "BonusIcon(" + (object) this.type + "," + (object) this.id + ", " + (this.disease == null ? (object) "null" : (object) this.disease.name) + "/" + (this.country == null ? (object) "null" : (object) this.country.name) + ")";
  }

  public int GetTurnsSinceLastCreation() => World.instance.DiseaseTurn - this.bornTurn;

  public BonusIcon(
    Disease disease,
    Country country,
    BonusIcon.EBonusIconType bonusIconType,
    int extraDNA,
    bool onlyDNA)
  {
    this.disease = disease;
    this.country = country;
    this.type = bonusIconType;
    this.extraEvo = extraDNA;
    this.forceEvo = onlyDNA;
    this.id = ++BonusIcon.idCounter;
    this.delay = bonusIconType != BonusIcon.EBonusIconType.INFECT ? 0.0f : ModelUtils.FloatRand(0.0f, 1.5f);
    this.showTime = this.GetBubbleShowTime(disease);
    if (disease.isCure)
      this.disableAutoHide = true;
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, World.instance.DiseaseTurn, World.instance.eventTurn, disease, "----" + (object) World.instance.DiseaseTurn + "/" + (object) World.instance.eventTurn + " - CREATED: " + (object) this);
  }

  public BonusIcon(
    Disease disease,
    Country country,
    BonusIcon.EBonusIconType bonusIconType,
    int musicImportance)
  {
    this.disease = disease;
    this.country = country;
    this.type = bonusIconType;
    this.musicBubble = true;
    this.musicImportance = (float) musicImportance;
    this.id = ++BonusIcon.idCounter;
    this.delay = 0.0f;
    this.showTime = this.GetBubbleShowTime(disease);
    if (disease.isCure)
      this.disableAutoHide = true;
    CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, World.instance.DiseaseTurn, World.instance.eventTurn, disease, "----" + (object) World.instance.DiseaseTurn + "/" + (object) World.instance.eventTurn + " - CREATED: " + (object) this);
  }

  public enum EBonusIconType
  {
    DNA,
    CURE,
    INFECT,
    DEATH,
    NEURAX,
    COUNTRY_SELECT,
    COUNTRY_SELECT_P2_INTENTION,
    COUNTRY_SELECT_P2_SELECTED,
    NECROA,
    APE_COLONY,
    NEXUS_FOUND,
    NEXUS_DNA,
    DOUBLE_INFECTED_DNA,
    NUKE,
    CASTLE,
    MEDICAL_SYSTEMS_OVERWHELMED,
    DISEASE_ORIGIN_COUNTRY,
    DEADBUBBLE_FOR_CURE,
  }
}
