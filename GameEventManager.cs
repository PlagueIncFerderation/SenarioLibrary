// Decompiled with JetBrains decompiler
// Type: GameEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
public class GameEventManager
{
  private static HashSet<string> CustomDisabledCategories = new HashSet<string>()
  {
    "boost_mechanic",
    "narrative_specific",
    "scenario_specific",
    "achievements"
  };
  public List<GameEvent> events = new List<GameEvent>();
  private IDictionary<string, GameEvent> eventList = (IDictionary<string, GameEvent>) new Dictionary<string, GameEvent>();
  private HashSet<GameEvent.EEventDiseaseType> eventTypesLoaded = new HashSet<GameEvent.EEventDiseaseType>();
  private bool allowAchievement;
  private StringBuilder testReport = new StringBuilder();
  private GameEvent currentEvent;
  private float currentTestEventChance = 1f;
  private List<Country> canRunCountries = new List<Country>(200);
  private List<LocalDisease> canRunLocalDiseases = new List<LocalDisease>(200);
  private List<EventOutcome> chances = new List<EventOutcome>();
  private Disease currentDisease;
  private Country currentCountry;
  private LocalDisease currentLocal;
  private IDictionary<string, Disease> diseaseMapReference = (IDictionary<string, Disease>) new Dictionary<string, Disease>();
  private int diseaseTurn;
  private bool _test_randomEncountered;

  public void LoadEvents(
    string eventData,
    GameEvent.EEventDiseaseType eventType,
    bool achievements,
    string categories = null)
  {
    byte[] bytes = (byte[]) null;
    if (!string.IsNullOrEmpty(eventData))
      bytes = Encoding.UTF8.GetBytes(eventData);
    this.LoadEvents(bytes, eventType, achievements, categories);
  }

  public void LoadEvents(
    byte[] bytes,
    GameEvent.EEventDiseaseType eventType,
    bool achievements,
    string categories = null)
  {
    this.allowAchievement = achievements;
    if (this.eventTypesLoaded.Contains(eventType))
      return;
    this.eventTypesLoaded.Add(eventType);
    AllEvents allEvents = new AllEvents();
    using (MemoryStream input = new MemoryStream(bytes))
    {
      try
      {
        XmlReader xmlReader = XmlReader.Create((Stream) input);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (AllEvents));
        xmlSerializer.UnknownElement += (XmlElementEventHandler) ((sender, e) => Debug.LogError((object) ("Unknown Element: \t'" + e.Element.Name + "' (contains: '" + e.Element.InnerXml + "')\t LineNumber: " + (object) e.LineNumber + "\t LinePosition: " + (object) e.LinePosition)));
        xmlSerializer.UnknownAttribute += (XmlAttributeEventHandler) ((sender, e) => Debug.LogError((object) ("Unknown Attribute: \t'" + e.Attr.Name + "' (value: '" + e.Attr.InnerXml + "')\t LineNumber: " + (object) e.LineNumber + "\t LinePosition: " + (object) e.LinePosition)));
        xmlSerializer.UnknownNode += (XmlNodeEventHandler) ((sender, e) => Debug.LogError((object) ("Unknown Node: \t'" + e.Name + "' (local: '" + e.LocalName + "')\tNamespace: " + e.NamespaceURI + "\tText: " + e.Text + "\t LineNumber: " + (object) e.LineNumber + "\t LinePosition: " + (object) e.LinePosition)));
        allEvents = (AllEvents) xmlSerializer.Deserialize(xmlReader);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error importing xml: " + (object) ex + "\n\n" + Encoding.UTF8.GetString(bytes)));
      }
    }
    List<GameEvent> gameEventList = new List<GameEvent>();
    if (!string.IsNullOrEmpty(categories))
    {
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) categories.Split(','));
      string str1 = CUtils.LoadData("Events/event_catagories");
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string newLine = Environment.NewLine;
      string str2 = str1.Replace(newLine, "\n");
      char[] chArray = new char[1]{ '\n' };
      foreach (string str3 in str2.Split(chArray))
      {
        string str4 = str3.Trim();
        if (!string.IsNullOrEmpty(str4))
        {
          string[] strArray = str4.Split(':');
          dictionary[strArray[0]] = strArray[1];
        }
      }
      foreach (GameEvent gameEvent in allEvents.gameEvents)
      {
        string str5;
        if (dictionary.TryGetValue(gameEvent.name, out str5))
        {
          int num = stringSet.Contains(str5) ? 1 : 0;
          bool flag = GameEventManager.CustomDisabledCategories.Contains(str5);
          if (num != 0 && !flag)
            gameEventList.Add(gameEvent);
        }
      }
    }
    else
      gameEventList = new List<GameEvent>((IEnumerable<GameEvent>) allEvents.gameEvents);
    Disease disease = World.instance.GetDisease(0);
    List<GameEvent> collection = new List<GameEvent>();
    foreach (GameEvent gameEvent in gameEventList)
    {
      if (gameEvent.typeCondition == null || gameEvent.typeCondition.types.Contains(disease.diseaseType))
      {
        bool flag = true;
        if (!string.IsNullOrEmpty(gameEvent.requireScenario) && gameEvent.requireScenario != disease.scenario)
          flag = false;
        if (gameEvent.enableCondition != null)
        {
          this.currentEvent = gameEvent;
          flag = this.CheckParameters(gameEvent.enableCondition.parameterConditions, (object) disease, false) == 1.0;
          this.currentEvent = (GameEvent) null;
        }
        if (flag)
          collection.Add(gameEvent);
      }
    }
    if (this.events == null)
      this.events = new List<GameEvent>((IEnumerable<GameEvent>) collection);
    else
      this.events.AddRange((IEnumerable<GameEvent>) collection);
    foreach (GameEvent gameEvent in gameEventList)
    {
      this.eventList[gameEvent.name] = gameEvent;
      gameEvent.finished = false;
    }
  }

  public void TestEvents(World w, Disease d)
  {
    Country nexus = d.nexus;
    LocalDisease localDisease = d.GetLocalDisease(nexus);
    this.eventList.Clear();
    this.testReport.Length = 0;
    foreach (GameEvent gameEvent in this.events)
      this.eventList[gameEvent.name] = gameEvent;
    foreach (GameEvent ev in this.events)
    {
      Debug.Log((object) ("EVENT: " + ev.name));
      this.TestEvent(ev, w, d, nexus, localDisease);
    }
    Debug.Log((object) ("Disease: " + d.name + " type: " + (object) d.diseaseType + " Report:\n" + this.testReport.ToString()));
  }

  public void TestEvent(
    GameEvent ev,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    this.currentEvent = ev;
    this.currentTestEventChance = 1f;
    if (ev.diseaseCondition != null && ev.diseaseCondition.randomChance > 0 && ev.diseaseCondition.randomRoll > 0)
      this.currentTestEventChance = (float) ev.diseaseCondition.randomRoll * 1f / (float) ev.diseaseCondition.randomChance;
    this.testReport.Append(ev.name).Append(":\n");
    bool flag1 = true;
    this.currentCountry = (Country) null;
    this.currentLocal = (LocalDisease) null;
    this.currentDisease = disease;
    if (ev.enableCondition != null)
      flag1 = this.CheckParameters(ev.enableCondition.parameterConditions, (object) disease, true) != 0.0;
    if (ev.diseaseCondition != null)
    {
      if (ev.diseaseCondition.preemptiveTech != null && ev.diseaseCondition.preemptiveTech.Length != 0)
      {
        Technology technology = disease.SelectRandomTech(ev.diseaseCondition.preemptiveTech);
        if (technology != null)
        {
          disease.preemptiveTech = technology.id;
          disease.preemptiveName = technology.name;
        }
      }
      if (ev.diseaseCondition.eventsFired != null)
      {
        bool flag2 = true;
        if (ev.diseaseCondition.anyEventFired)
          flag2 = false;
        for (int index = 0; index < ev.diseaseCondition.eventsFired.Length; ++index)
        {
          string key = ev.diseaseCondition.eventsFired[index];
          if (this.eventList.ContainsKey(key))
          {
            if (ev.diseaseCondition.anyEventFired)
            {
              if (this.eventList[key].finished)
                flag2 = true;
            }
            else if (!this.eventList[key].finished)
              flag2 = false;
          }
        }
        if (!flag2)
          flag1 = false;
      }
      if (ev.diseaseCondition.eventsNotFired != null)
      {
        bool flag3 = true;
        for (int index = 0; index < ev.diseaseCondition.eventsNotFired.Length; ++index)
        {
          string key = ev.diseaseCondition.eventsNotFired[index];
          if (this.eventList.ContainsKey(key) && this.eventList[key].finished)
            flag3 = false;
        }
        if (!flag3)
          flag1 = false;
      }
      if (disease.recentEventCounter < ev.recentEventCounter)
        flag1 = false;
      flag1 = flag1 && this.CheckParameters(ev.diseaseCondition.parameterConditions, (object) disease, true) != 0.0;
      if (flag1)
      {
        if (ev.diseaseCondition.techNotResearched != null)
        {
          bool flag4 = true;
          for (int index = 0; index < ev.diseaseCondition.techNotResearched.Length; ++index)
          {
            string techID = ev.diseaseCondition.techNotResearched[index];
            if (techID != "disease.preemptiveTech" && disease.GetTechnology(techID) == null)
            {
              Debug.LogError((object) (ev.name + " tried to check for non-researched non-existent tech in " + (object) disease.diseaseType + " '" + techID + "'"));
              this.testReport.Append("ERROR: ").Append(ev.name).Append(" Tried to check for non-researched non-existent tech '").Append(techID).Append("'").Append("\n");
            }
            if (disease.IsTechEvolved(techID))
              flag4 = false;
          }
          if (!flag4)
            flag1 = false;
        }
        if (ev.diseaseCondition.techResearched != null)
        {
          bool flag5 = true;
          if (ev.diseaseCondition.anyTechResearched)
            flag5 = false;
          for (int index = 0; index < ev.diseaseCondition.techResearched.Length; ++index)
          {
            string techID = ev.diseaseCondition.techResearched[index];
            if (techID != "disease.preemptiveTech" && disease.GetTechnology(techID) == null)
            {
              Debug.LogError((object) (ev.name + " tried to check for researched non-existent tech in " + (object) disease.diseaseType + " '" + techID + "'"));
              this.testReport.Append("ERROR: ").Append(ev.name).Append(" Tried to check for researched non-existent tech '").Append(techID).Append("'").Append("\n");
            }
            if (ev.diseaseCondition.anyTechResearched)
            {
              if (disease.IsTechEvolved(techID))
                flag5 = true;
            }
            else if (!disease.IsTechEvolved(techID))
              flag5 = false;
          }
          if (!flag5)
            flag1 = false;
        }
      }
      this.testReport.Append("  DISEASE: ").Append(flag1 ? "YES" : "NO");
      if (flag1)
        this.testReport.Append(". Chance 1 in ").Append(this.currentTestEventChance);
      this.testReport.Append("\n");
    }
    this.currentCountry = country;
    this.currentLocal = localDisease;
    bool flag6 = ev.countryCondition != null && ev.countryCondition.parameterConditions.Length != 0;
    bool flag7 = ev.localCondition != null && ev.localCondition.parameterConditions.Length != 0;
    if (flag6 | flag7)
    {
      bool flag8 = true;
      this.currentTestEventChance = 1f;
      Country ob1 = country;
      LocalDisease ob2 = localDisease;
      if (flag6)
        flag8 &= this.CheckParameters(ev.countryCondition.parameterConditions, (object) ob1, true) != 0.0;
      if (flag7)
      {
        if (ev.localCondition.randomChance > 0 && ev.localCondition.randomRoll > 0)
          this.currentTestEventChance *= (float) ev.localCondition.randomRoll * 1f / (float) ev.localCondition.randomChance;
        flag8 &= this.CheckParameters(ev.localCondition.parameterConditions, (object) ob2, true) != 0.0;
      }
      if (flag1)
      {
        this.testReport.Append("    COUNTRY: ").Append(ob1.id).Append(flag8 ? " YES" : " NO");
        if (flag8)
          this.testReport.Append(". Chance 1 in ").Append(this.currentTestEventChance);
        this.testReport.Append("\n");
      }
    }
    List<EventOutcome> eventOutcomeList = new List<EventOutcome>();
    if (ev.eventOutcomes == null)
      return;
    int max = 0;
    bool flag9 = false;
    bool flag10 = false;
    for (int index1 = 0; index1 < ev.eventOutcomes.Length; ++index1)
    {
      EventOutcome eventOutcome1 = ev.eventOutcomes[index1];
      if (eventOutcome1.logicalOp != EventOutcome.ELogicalOp.NONE && eventOutcomeList.Count > 0)
      {
        int num = ModelUtils.IntRand(0, max);
        for (int index2 = 0; index2 < eventOutcomeList.Count; ++index2)
        {
          EventOutcome eventOutcome2 = eventOutcomeList[index2];
          num -= eventOutcome2.chance;
          if (num <= 0)
          {
            eventOutcomeList.Clear();
            break;
          }
        }
      }
      if (eventOutcome1.logicalOp == EventOutcome.ELogicalOp.ALWAYS)
      {
        if (eventOutcome1.diseaseCondition != null || eventOutcome1.countryCondition != null || eventOutcome1.localCondition != null)
        {
          Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
          continue;
        }
      }
      else if (eventOutcome1.logicalOp == EventOutcome.ELogicalOp.IF)
      {
        flag9 = true;
        flag10 = false;
        if (this.CheckConditions(disease, localDisease, world, country, eventOutcome1, ev, true))
          flag10 = true;
      }
      else if (eventOutcome1.logicalOp == EventOutcome.ELogicalOp.ELSEIF)
      {
        if (!flag9 && !flag10)
        {
          Debug.Log((object) ("Incorrect ELSEIF Statement - " + ev.name));
          continue;
        }
        if (this.CheckConditions(disease, localDisease, world, country, eventOutcome1, ev, true))
          flag10 = true;
      }
      else if (eventOutcome1.logicalOp == EventOutcome.ELogicalOp.ELSE)
      {
        if (!flag9 && !flag10)
          Debug.LogError((object) ("No preceding IF - will always trigger - " + ev.name));
        if (eventOutcome1.diseaseCondition != null || eventOutcome1.countryCondition != null || eventOutcome1.localCondition != null)
          Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
      }
      else
      {
        if (eventOutcome1.chance == 0)
        {
          Debug.Log((object) (ev.name + " Zero chance event found - skipping"));
          continue;
        }
        if (this.CheckConditions(disease, localDisease, world, country, eventOutcome1, ev, true))
        {
          eventOutcomeList.Add(eventOutcome1);
          max += eventOutcome1.chance;
        }
      }
      if (eventOutcome1.diseaseEffect != null)
        this.ApplyParameterEffects(eventOutcome1.diseaseEffect.parameterEffects, (object) disease);
      if (eventOutcome1.localEffect != null)
        this.ApplyParameterEffects(eventOutcome1.localEffect.parameterEffects, (object) localDisease);
      if (eventOutcome1.countryEffect != null)
        this.ApplyParameterEffects(eventOutcome1.countryEffect.parameterEffects, (object) country);
    }
    if (eventOutcomeList.Count <= 0)
      return;
    int num1 = ModelUtils.IntRand(0, max);
    for (int index = 0; index < eventOutcomeList.Count; ++index)
    {
      EventOutcome eventOutcome = eventOutcomeList[index];
      num1 -= eventOutcome.chance;
      if (num1 <= 0)
      {
        eventOutcomeList.Clear();
        break;
      }
    }
  }

  public void Evaluate(World world, Disease disease)
  {
    if (CGameManager.IsFederalScenario("PISMG"))
      return;
    foreach (GameEvent ev in this.events)
    {
      this.currentEvent = ev;
      List<string> stringList = new List<string>();
      if (ev.name.IndexOf("implements ") != -1)
      {
        string[] strArray = ev.name.Substring(ev.name.IndexOf("implements ") + 11).Replace(" ", string.Empty).Split(',');
        if (strArray.Length != 0)
        {
          foreach (string str in strArray)
          {
            if (!string.IsNullOrWhiteSpace(str))
              stringList.Add(str);
          }
        }
      }
      if (disease.recentEventCounter > ev.recentEventCounter && (!world.firedEvents.ContainsKey(ev.name) || !world.firedEvents[ev.name].ContainsKey(disease.id) || ev.limit < 0 || ev.limit > 0 && world.firedEvents[ev.name][disease.id] < ev.limit))
      {
        if (ev.tutorial && (disease.isSpeedRun || !disease.showExtraPopups))
          ev.finished = true;
        else if ((ev.scenarios != GameEvent.EEventScenarioType.RESTRICTED || !world.scenarioEventRestriction) && (ev.scenarios != GameEvent.EEventScenarioType.Empty || !world.isScenario))
        {
          this.currentCountry = (Country) null;
          this.currentLocal = (LocalDisease) null;
          this.currentDisease = disease;
          this.diseaseMapReference.Clear();
          this.diseaseMapReference["ACTING_DISEASE"] = disease;
          if (ev.diseaseCondition == null || this.CheckDiseaseCondition(world, ev.diseaseCondition, disease, false))
          {
            bool flag1 = ev.localCondition != null && ev.localCondition.parameterConditions.Length != 0;
            bool flag2 = ev.countryCondition != null && ev.countryCondition.parameterConditions.Length != 0;
            if (flag2 | flag1)
            {
              this.canRunCountries.Clear();
              this.canRunLocalDiseases.Clear();
              for (int index = 0; index < world.countries.Count; ++index)
              {
                bool flag3 = true;
                this.currentCountry = world.countries[index];
                this.currentLocal = disease.GetLocalDisease(this.currentCountry);
                if (flag2)
                  flag3 = this.CheckParameters(ev.countryCondition.parameterConditions, (object) this.currentCountry, false) != 0.0;
                if (flag3 & flag1)
                {
                  flag3 = this.CheckParameters(ev.localCondition.parameterConditions, (object) this.currentLocal, false) != 0.0;
                  if (flag3 && ev.localCondition.randomRoll > 0 && ev.localCondition.randomChance > 0 && ModelUtils.IntRand(0, ev.localCondition.randomRoll) >= ev.localCondition.randomChance)
                    flag3 = false;
                }
                if (flag3)
                {
                  this.canRunCountries.Add(this.currentCountry);
                  this.canRunLocalDiseases.Add(this.currentLocal);
                }
              }
              if (this.canRunCountries.Count != 0)
              {
                int index = ModelUtils.IntRand(0, this.canRunCountries.Count - 1);
                this.currentCountry = this.canRunCountries[index];
                this.currentLocal = this.canRunLocalDiseases[index];
              }
              else
                continue;
            }
            if (CGameManager.IsMultiplayerGame)
              CGameManager.game.ReplayData.AddEvent(ReplayData.ReplayEventType.LOG_MESSAGE, world.DiseaseTurn, world.eventTurn, disease, World.instance.DiseaseTurn.ToString() + "/" + (object) World.instance.eventTurn + ": event: " + ev.name + " running for " + disease.name + " in country: " + (this.currentCountry != null ? (object) this.currentCountry.id : (object) "none"));
            this.chances.Clear();
            if (stringList.Contains("Enumerator") && ev.lastTurn < world.DiseaseTurn)
            {
              ev.lastTurn = world.DiseaseTurn;
              foreach (Country canRunCountry in this.canRunCountries)
              {
                this.chances.Clear();
                Country country = canRunCountry;
                this.currentCountry = country;
                this.currentLocal = country.GetLocalDisease(disease);
                if (ev.eventOutcomes != null)
                {
                  int max = 0;
                  bool flag4 = false;
                  bool flag5 = false;
                  if (ev.eventOutcomes.Length == 1)
                  {
                    EventOutcome eventOutcome = ev.eventOutcomes[0];
                    if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                      this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                  }
                  else
                  {
                    for (int index1 = 0; index1 < ev.eventOutcomes.Length; ++index1)
                    {
                      EventOutcome eventOutcome = ev.eventOutcomes[index1];
                      if (eventOutcome.logicalOp != EventOutcome.ELogicalOp.NONE && this.chances.Count > 0)
                      {
                        int num = ModelUtils.IntRand(0, max);
                        for (int index2 = 0; index2 < this.chances.Count; ++index2)
                        {
                          EventOutcome chance = this.chances[index2];
                          num -= chance.chance;
                          if (num <= 0)
                          {
                            this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, chance, ev);
                            this.chances.Clear();
                            break;
                          }
                        }
                      }
                      if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ALWAYS)
                      {
                        if (eventOutcome.diseaseCondition != null || eventOutcome.countryCondition != null || eventOutcome.localCondition != null)
                          Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
                        else
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.IF)
                      {
                        if (ev.debug)
                          Debug.Log((object) (ev.name + " IF CONDITION"));
                        flag4 = true;
                        flag5 = false;
                        if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                        {
                          if (ev.debug)
                            Debug.Log((object) (ev.name + " IF CONDITION MATCHED"));
                          flag5 = true;
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                        }
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ELSEIF)
                      {
                        if (!flag4)
                          Debug.LogError((object) (ev.name + " No preceding IF - invalid ELSEIF"));
                        else if (!flag5 && this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                        {
                          flag5 = true;
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                        }
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ELSE)
                      {
                        if (!flag5)
                        {
                          if (!flag4)
                          {
                            Debug.LogError((object) (ev.name + " No preceding IF - invalid ELSE"));
                          }
                          else
                          {
                            if (eventOutcome.diseaseCondition != null || eventOutcome.countryCondition != null || eventOutcome.localCondition != null)
                              Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
                            this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                          }
                        }
                      }
                      else if (!flag5)
                      {
                        if (eventOutcome.chance == 0)
                        {
                          Debug.LogError((object) (ev.name + " Zero chance event found - skipping"));
                        }
                        else
                        {
                          if (ev.debug)
                            Debug.Log((object) (ev.name + " adding chance"));
                          if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                          {
                            this.chances.Add(eventOutcome);
                            max += eventOutcome.chance;
                          }
                        }
                      }
                    }
                    if (ev.debug)
                      Debug.Log((object) (ev.name + " chances: " + (object) this.chances.Count));
                    if (this.chances.Count > 0)
                    {
                      int num = ModelUtils.IntRand(0, max);
                      for (int index = 0; index < this.chances.Count; ++index)
                      {
                        EventOutcome chance = this.chances[index];
                        num -= chance.chance;
                        if (num <= 0)
                        {
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, chance, ev);
                          this.chances.Clear();
                          break;
                        }
                      }
                    }
                  }
                }
              }
            }
            else if (!stringList.Contains("Enumerator") || ev.lastTurn < world.DiseaseTurn)
            {
              if (ev.eventOutcomes != null && !stringList.Contains("Enumerator"))
              {
                if (!stringList.Contains("StrictDaily") || ev.lastTurn < world.DiseaseTurn)
                {
                  int max = 0;
                  bool flag6 = false;
                  bool flag7 = false;
                  if (ev.eventOutcomes.Length == 1)
                  {
                    EventOutcome eventOutcome = ev.eventOutcomes[0];
                    if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                      this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                  }
                  else
                  {
                    for (int index3 = 0; index3 < ev.eventOutcomes.Length; ++index3)
                    {
                      EventOutcome eventOutcome = ev.eventOutcomes[index3];
                      if (eventOutcome.logicalOp != EventOutcome.ELogicalOp.NONE && this.chances.Count > 0)
                      {
                        int num = ModelUtils.IntRand(0, max);
                        for (int index4 = 0; index4 < this.chances.Count; ++index4)
                        {
                          EventOutcome chance = this.chances[index4];
                          num -= chance.chance;
                          if (num <= 0)
                          {
                            this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, chance, ev);
                            this.chances.Clear();
                            break;
                          }
                        }
                      }
                      if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ALWAYS)
                      {
                        if (eventOutcome.diseaseCondition != null || eventOutcome.countryCondition != null || eventOutcome.localCondition != null)
                          Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
                        else
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.IF)
                      {
                        if (ev.debug)
                          Debug.Log((object) (ev.name + " IF CONDITION"));
                        flag6 = true;
                        flag7 = false;
                        if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                        {
                          if (ev.debug)
                            Debug.Log((object) (ev.name + " IF CONDITION MATCHED"));
                          flag7 = true;
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                        }
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ELSEIF)
                      {
                        if (!flag6)
                          Debug.LogError((object) (ev.name + " No preceding IF - invalid ELSEIF"));
                        else if (!flag7 && this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                        {
                          flag7 = true;
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                        }
                      }
                      else if (eventOutcome.logicalOp == EventOutcome.ELogicalOp.ELSE)
                      {
                        if (!flag7)
                        {
                          if (!flag6)
                          {
                            Debug.LogError((object) (ev.name + " No preceding IF - invalid ELSE"));
                          }
                          else
                          {
                            if (eventOutcome.diseaseCondition != null || eventOutcome.countryCondition != null || eventOutcome.localCondition != null)
                              Debug.LogError((object) (ev.name + ": Cannot have DiseaseCondition/CountryContion or LocalCondition in an ELSE Outcome - ignoring."));
                            this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev);
                          }
                        }
                      }
                      else if (!flag7)
                      {
                        if (eventOutcome.chance == 0)
                        {
                          Debug.LogError((object) (ev.name + " Zero chance event found - skipping"));
                        }
                        else
                        {
                          if (ev.debug)
                            Debug.Log((object) (ev.name + " adding chance"));
                          if (this.CheckConditions(disease, this.currentLocal, world, this.currentCountry, eventOutcome, ev))
                          {
                            this.chances.Add(eventOutcome);
                            max += eventOutcome.chance;
                          }
                        }
                      }
                    }
                    if (ev.debug)
                      Debug.Log((object) (ev.name + " chances: " + (object) this.chances.Count));
                    if (this.chances.Count > 0)
                    {
                      int num = ModelUtils.IntRand(0, max);
                      for (int index = 0; index < this.chances.Count; ++index)
                      {
                        EventOutcome chance = this.chances[index];
                        num -= chance.chance;
                        if (num <= 0)
                        {
                          this.ApplyOutcome(disease, this.currentLocal, world, this.currentCountry, chance, ev);
                          this.chances.Clear();
                          break;
                        }
                      }
                    }
                  }
                }
                else
                  continue;
              }
            }
            else
              continue;
            ev.lastTurn = world.DiseaseTurn;
            if (!world.firedEvents.ContainsKey(ev.name))
              world.firedEvents[ev.name] = (IDictionary<int, int>) new Dictionary<int, int>();
            if (!world.firedEvents[ev.name].ContainsKey(disease.id))
              world.firedEvents[ev.name][disease.id] = 1;
            else
              world.firedEvents[ev.name][disease.id]++;
            if (world.firedEvents[ev.name][disease.id] >= ev.limit)
              ev.finished = true;
            if (ev.resetRecentEventCounter)
              disease.recentEventCounter = 0;
          }
        }
      }
    }
  }

  public bool CheckConditions(
    Disease disease,
    LocalDisease localDisease,
    World world,
    Country country,
    EventOutcome oc,
    GameEvent ev,
    bool test = false)
  {
    return (oc.diseaseCondition == null || this.CheckDiseaseCondition(world, oc.diseaseCondition, disease, test) || test) && (oc.countryCondition == null || this.CheckParameters(oc.countryCondition.parameterConditions, (object) country, test) != 0.0 || test) && (oc.localCondition == null || this.CheckParameters(oc.localCondition.parameterConditions, (object) localDisease, test) != 0.0 || test);
  }

  public void ApplyOutcome(
    Disease disease,
    LocalDisease localDisease,
    World world,
    Country country,
    EventOutcome oc,
    GameEvent ev)
  {
    if (oc == null)
    {
      Debug.Log((object) ("Outcome null " + ev.name));
    }
    else
    {
      bool flag = false;
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.Tutorial)
      {
        if (!TutorialSystem.IsModuleSectionComplete("Free Play"))
          this.diseaseTurn = world.DiseaseTurn;
        flag = world.DiseaseTurn - this.diseaseTurn < 10;
      }
      if (oc.gameEndMessage != null)
      {
        disease.endMessageImage = oc.gameEndMessage.icon;
        disease.endMessageText = oc.gameEndMessage.text;
        disease.endMessageTitle = oc.gameEndMessage.title;
        World.instance.ShowGameEndMessage(disease);
      }
      if (oc.diseaseEffect != null && !flag)
      {
        if (oc.diseaseEffect.setFlag != Disease.EGenericDiseaseFlag.None)
          disease.AddFlag(oc.diseaseEffect.setFlag);
        if (oc.diseaseEffect.evolveRandomTech)
          World.instance.EvolveRandomTech(disease, oc.diseaseEffect.randomTech, country);
        if (oc.diseaseEffect.deEvolveRandomTech)
          World.instance.DeEvolveRandomTech(disease);
        if (oc.diseaseEffect.eventLockTech != null)
        {
          for (int index = 0; index < oc.diseaseEffect.eventLockTech.Length; ++index)
          {
            EventLockTech eventLockTech = oc.diseaseEffect.eventLockTech[index];
            if (!string.IsNullOrEmpty(eventLockTech.id))
              disease.EventLockTech(eventLockTech.id, eventLockTech.locked);
            else
              disease.EventLockTech(eventLockTech.techType, eventLockTech.locked);
          }
        }
        if (oc.diseaseEffect.padlockTech != null)
        {
          for (int index = 0; index < oc.diseaseEffect.padlockTech.Length; ++index)
          {
            EventLockTech eventLockTech = oc.diseaseEffect.padlockTech[index];
            if (!string.IsNullOrEmpty(eventLockTech.id))
              disease.PadlockTech(eventLockTech.id, eventLockTech.locked);
            else
              disease.PadlockTech(eventLockTech.techType, eventLockTech.locked);
          }
        }
        if (!string.IsNullOrEmpty(oc.diseaseEffect.evolveTech))
          disease.EvolveTech(disease.GetTechnology(oc.diseaseEffect.evolveTech), true);
        this.ApplyParameterEffects(oc.diseaseEffect.parameterEffects, (object) disease);
        if (!string.IsNullOrEmpty(oc.diseaseEffect.function))
        {
          MethodInfo method = typeof (Disease).GetMethod(oc.diseaseEffect.function);
          if (method != (MethodInfo) null)
          {
            if (method.IsDefined(typeof (GameEventFunction), false))
            {
              if (method.GetParameters().Length == 1)
                method.Invoke((object) disease, new object[1]
                {
                  (object) country
                });
              else if (method.GetParameters().Length < 1)
              {
                method.Invoke((object) disease, (object[]) null);
              }
              else
              {
                int length = method.GetParameters().Length;
                string[] strArray = new string[length];
                for (int index = 0; index < length; ++index)
                {
                  if (index == 0)
                    strArray[index] = disease.gameEventFunctionParameter1;
                  if (index == 1)
                    strArray[index] = disease.gameEventFunctionParameter2;
                  if (index == 2)
                    strArray[index] = disease.gameEventFunctionParameter3;
                  if (index == 3)
                    strArray[index] = disease.gameEventFunctionParameter4;
                  if (index == 4)
                    strArray[index] = disease.gameEventFunctionParameter5;
                }
                for (int index = 0; index < length; ++index)
                {
                  if (strArray[index].ToLower().Equals("thisCountry".ToLower()))
                    strArray[index] = country.id;
                  if (strArray[index].ToLower().Equals("randomCountry".ToLower()))
                    strArray[index] = GameEventManager.GetRandomCountry();
                }
                MethodInfo methodInfo = method;
                object[] objArray = (object[]) strArray;
                Disease disease1 = disease;
                object[] parameters = objArray;
                methodInfo.Invoke((object) disease1, parameters);
              }
            }
            else
              Debug.LogError((object) ("method: " + oc.diseaseEffect.function + " was not marked as [GameEventFunction]"));
          }
          else
            Debug.LogError((object) ("Could not find method: " + oc.diseaseEffect.function));
        }
      }
      if (oc.localEffect != null && oc.localEffect.parameterEffects.Length != 0)
      {
        if (country == null)
        {
          country = this.currentCountry = World.instance.countries[ModelUtils.IntRand(0, World.instance.countries.Count - 1)];
          localDisease = this.currentLocal = this.currentCountry.GetLocalDisease(disease);
        }
        if (localDisease == null)
          localDisease = this.currentLocal = this.currentCountry.GetLocalDisease(disease);
        this.ApplyParameterEffects(oc.localEffect.parameterEffects, (object) localDisease);
      }
      if (oc.countryEffect != null)
      {
        if (country == null)
        {
          country = this.currentCountry = World.instance.countries[ModelUtils.IntRand(0, World.instance.countries.Count - 1)];
          localDisease = this.currentLocal = country.GetLocalDisease(disease);
        }
        if (oc.countryEffect.parameterEffects != null && oc.countryEffect.parameterEffects.Length != 0)
          this.ApplyParameterEffects(oc.countryEffect.parameterEffects, (object) country);
        if (oc.countryEffect.apeLabStatus.HasValue)
        {
          if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
            Debug.Log((object) ("Event changed vampire lab state in: " + country.id + " from: " + (object) country.apeLabStatus + " to: " + (object) oc.countryEffect.apeLabStatus.Value));
          country.ChangeApeLabStateF(oc.countryEffect.apeLabStatus.Value);
        }
      }
      if (oc.newsMessages != null)
      {
        EventOutcome.EEventNewsVisibility? visibility;
        if (!CGameManager.IsMultiplayerGame)
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility = EventOutcome.EEventNewsVisibility.Other;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility & visibility.HasValue))
            goto label_82;
        }
        visibility = oc.visibility;
        EventOutcome.EEventNewsVisibility eeventNewsVisibility1 = EventOutcome.EEventNewsVisibility.Public;
        if (!(visibility.GetValueOrDefault() == eeventNewsVisibility1 & visibility.HasValue))
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility2 = EventOutcome.EEventNewsVisibility.Private;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility2 & visibility.HasValue) || disease != CNetworkManager.network.LocalPlayerInfo.disease)
          {
            visibility = oc.visibility;
            EventOutcome.EEventNewsVisibility eeventNewsVisibility3 = EventOutcome.EEventNewsVisibility.Other;
            if (!(visibility.GetValueOrDefault() == eeventNewsVisibility3 & visibility.HasValue) || CGameManager.IsAIGame || disease != CNetworkManager.network.OpponentPlayerInfo.disease)
              goto label_92;
          }
        }
label_82:
        for (int index = 0; index < oc.newsMessages.Count; ++index)
        {
          if ((!oc.newsMessages[index].onlyIfNotNoticed || !disease.diseaseNoticed) && !string.IsNullOrEmpty(oc.newsMessages[index].text))
          {
            int p = 1;
            if (oc.newsPriorities != null)
            {
              if (oc.newsPriorities.Count > index)
                p = oc.newsPriorities[index];
              else if (oc.newsPriorities.Count > 0)
                p = oc.newsPriorities[0];
            }
            world.AddNewsItem(new IGame.NewsItem(oc.newsMessages[index], disease, country, p));
          }
        }
      }
label_92:
      if (oc.debugMessage != null)
        Debug.Log((object) oc.debugMessage);
      if (oc.popupTitle != null && !string.IsNullOrEmpty(oc.popupTitle.text) && !flag)
      {
        EventOutcome.EEventNewsVisibility? visibility;
        if (!CGameManager.IsMultiplayerGame)
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility = EventOutcome.EEventNewsVisibility.Other;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility & visibility.HasValue))
            goto label_100;
        }
        visibility = oc.visibility;
        EventOutcome.EEventNewsVisibility eeventNewsVisibility4 = EventOutcome.EEventNewsVisibility.Public;
        if (!(visibility.GetValueOrDefault() == eeventNewsVisibility4 & visibility.HasValue))
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility5 = EventOutcome.EEventNewsVisibility.Private;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility5 & visibility.HasValue) || disease != CNetworkManager.network.LocalPlayerInfo.disease)
          {
            visibility = oc.visibility;
            EventOutcome.EEventNewsVisibility eeventNewsVisibility6 = EventOutcome.EEventNewsVisibility.Other;
            if (!(visibility.GetValueOrDefault() == eeventNewsVisibility6 & visibility.HasValue) || CGameManager.IsAIGame || disease != CNetworkManager.network.OpponentPlayerInfo.disease)
              goto label_120;
          }
        }
label_100:
        List<Disease> diseaseList = new List<Disease>((IEnumerable<Disease>) World.instance.diseases);
        Country[] countries = new Country[1]{ country };
        int id;
        for (int index = 0; index < oc.popupTitle.parameters.Count; ++index)
        {
          StringParameter parameter = oc.popupTitle.parameters[index];
          if (parameter.field != null)
          {
            string[] strArray1 = parameter.field.Split('.');
            if (strArray1[0] == nameof (disease) && strArray1.Length == 2)
            {
              string[] strArray2 = new string[3]
              {
                strArray1[0],
                null,
                strArray1[1]
              };
              if (string.IsNullOrEmpty(parameter.reference))
              {
                string[] strArray3 = strArray2;
                id = disease.id;
                string str = id.ToString();
                strArray3[1] = str;
              }
              else
              {
                string[] strArray4 = strArray2;
                id = this.diseaseMapReference[parameter.reference].id;
                string str = id.ToString();
                strArray4[1] = str;
              }
              parameter.field = string.Join(".", strArray2);
            }
          }
        }
        for (int index = 0; index < oc.popupMessage.parameters.Count; ++index)
        {
          StringParameter parameter = oc.popupMessage.parameters[index];
          if (parameter.field != null)
          {
            string[] strArray5 = parameter.field.Split('.');
            if (strArray5[0] == nameof (disease) && strArray5.Length == 2)
            {
              string[] strArray6 = new string[3]
              {
                strArray5[0],
                null,
                strArray5[1]
              };
              if (string.IsNullOrEmpty(parameter.reference))
              {
                string[] strArray7 = strArray6;
                id = disease.id;
                string str = id.ToString();
                strArray7[1] = str;
              }
              else
              {
                string[] strArray8 = strArray6;
                id = this.diseaseMapReference[parameter.reference].id;
                string str = id.ToString();
                strArray8[1] = str;
              }
              parameter.field = string.Join(".", strArray6);
            }
          }
        }
        if (CGameManager.gameType != IGame.GameType.CureTutorial)
          world.popupMessages.Add(new PopupMessage(oc.popupIcon, oc.popupTitle, CGameManager.GetDisplayDate(), oc.popupMessage, diseaseList.ToArray(), countries, oc.popupType.HasValue ? oc.popupType.Value : PopupMessage.PopupType.Default));
      }
label_120:
      if (oc.achievement.HasValue && this.allowAchievement)
        world.AddAchievement(oc.achievement.Value);
      if (!oc.dynamicNewsItem || world.dynamicNews.Count <= 0)
        return;
      DynamicNewsItem dynamicNew = world.dynamicNews[UnityEngine.Random.Range(0, world.dynamicNews.Count)];
      string text = dynamicNew.title;
      string content = dynamicNew.content;
      if (string.IsNullOrEmpty(text))
        text = content;
      if (!string.IsNullOrEmpty(text))
      {
        ParameterisedString parameterisedString = new ParameterisedString(text, new string[1]
        {
          "disease.name"
        });
        parameterisedString.useLocalisation = false;
        world.AddNewsItem(new IGame.NewsItem(parameterisedString, disease, country, dynamicNew.priority));
        if (dynamicNew.popup && !string.IsNullOrEmpty(content) && !flag)
          world.popupMessages.Add(new PopupMessage(dynamicNew.icon, parameterisedString, CGameManager.GetDisplayDate(), new ParameterisedString(content, new string[1]
          {
            "disease.name"
          })
          {
            useLocalisation = false
          }, disease, country));
        disease.recentEventCounter = 0;
      }
      world.dynamicNews.Remove(dynamicNew);
      world.dynamicNewsSeen.Add(dynamicNew.id);
    }
  }

  private double CheckParameters(
    ParameterCondition[] conditions,
    object ob,
    bool test,
    ParameterCondition.ECombinationOp parentOperation = ParameterCondition.ECombinationOp.NONE)
  {
    return this.EvaluateParameters(conditions, ob, test, this.currentEvent.debug, this.currentEvent.name, parentOperation);
  }

  public double EvaluateParameters(
    ParameterCondition[] conditions,
    object ob,
    bool test = false,
    bool debug = false,
    string debugName = "",
    ParameterCondition.ECombinationOp parentOperation = ParameterCondition.ECombinationOp.NONE)
  {
    if (debug)
      Debug.Log((object) ("[" + (object) World.instance.DiseaseTurn + "/" + (object) World.instance.eventTurn + "] " + debugName + " EVALUATE: " + (object) conditions.Length + " PARENT: " + (object) parentOperation + " D: " + (this.currentDisease != null ? (object) this.currentDisease.name : (object) "NULL") + " C: " + (this.currentCountry != null ? (object) this.currentCountry.id : (object) "none") + " LD: " + (object) (this.currentLocal != null ? this.currentLocal.diseaseID : -1)));
    if (test && parentOperation == ParameterCondition.ECombinationOp.NONE)
      this._test_randomEncountered = false;
    switch (parentOperation)
    {
      case ParameterCondition.ECombinationOp.PLUS:
        double parameters1 = 0.0;
        for (int position = 0; position < conditions.Length; ++position)
          parameters1 += this.GetDoubleOperand(conditions, ob, test, position);
        return parameters1;
      case ParameterCondition.ECombinationOp.MINUS:
        double doubleOperand1 = this.GetDoubleOperand(conditions, ob, test, 0);
        for (int position = 1; position < conditions.Length; ++position)
          doubleOperand1 -= this.GetDoubleOperand(conditions, ob, test, position);
        return doubleOperand1;
      case ParameterCondition.ECombinationOp.MULTIPLY:
        double doubleOperand2 = this.GetDoubleOperand(conditions, ob, test, 0);
        for (int position = 1; position < conditions.Length; ++position)
          doubleOperand2 *= this.GetDoubleOperand(conditions, ob, test, position);
        return doubleOperand2;
      case ParameterCondition.ECombinationOp.DIVIDE:
        double doubleOperand3 = this.GetDoubleOperand(conditions, ob, test, 0);
        for (int position = 1; position < conditions.Length; ++position)
        {
          double doubleOperand4 = this.GetDoubleOperand(conditions, ob, test, position);
          if (doubleOperand4 == 0.0)
            return 0.0;
          doubleOperand3 /= doubleOperand4;
        }
        return doubleOperand3;
      case ParameterCondition.ECombinationOp.LESS_THAN:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) < this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a LESS_THAN with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.GREATER_THAN:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) > this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a GREATER_THAN with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.LESS_THAN_OR_EQUAL:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) <= this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a LESS_THAN_OR_EQUAL with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.GREATER_THAN_OR_EQUAL:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) >= this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a GREATER_THAN_OR_EQUAL with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.NOT_EQUAL:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) != this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a NOT_EQUAL with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.EQUALS:
        if (conditions.Length == 2)
          return this.GetDoubleOperand(conditions, ob, test, 0) == this.GetDoubleOperand(conditions, ob, test, 1) ? 1.0 : 0.0;
        Debug.LogError((object) (debugName + ": CheckParameters had a EQUALS with " + (object) conditions + " conditions, needs exactly 2"));
        return 0.0;
      case ParameterCondition.ECombinationOp.MIN:
        double parameters2 = 0.0;
        for (int position = 0; position < conditions.Length; ++position)
        {
          double doubleOperand5 = this.GetDoubleOperand(conditions, ob, test, position);
          if (doubleOperand5 < parameters2 || position == 0)
            parameters2 = doubleOperand5;
        }
        return parameters2;
      case ParameterCondition.ECombinationOp.MAX:
        double parameters3 = 0.0;
        for (int position = 0; position < conditions.Length; ++position)
        {
          double doubleOperand6 = this.GetDoubleOperand(conditions, ob, test, position);
          if (doubleOperand6 > parameters3 || position == 0)
            parameters3 = doubleOperand6;
        }
        return parameters3;
      default:
        if (conditions == null)
          return 1.0;
        bool flag1 = false;
        for (int index1 = 0; index1 < conditions.Length; ++index1)
        {
          ParameterCondition condition = conditions[index1];
          GameEventManager.TryCacheLookupData(condition, ob);
          bool flag2 = false;
          if (condition.parameterConditions != null && condition.parameterConditions.Length != 0)
          {
            if (condition.mapOperation != EMapOp.SELF)
            {
              if (condition.mapOperation == EMapOp.ALL || condition.mapOperation == EMapOp.ALL_OTHER)
              {
                bool flag3 = true;
                Disease currentDisease = this.currentDisease;
                LocalDisease currentLocal = this.currentLocal;
                for (int index2 = 0; index2 < World.instance.diseases.Count; ++index2)
                {
                  this.currentDisease = World.instance.diseases[index2];
                  if (this.currentDisease != currentDisease || condition.mapOperation != EMapOp.ALL_OTHER)
                  {
                    if (this.currentCountry != null)
                      this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
                    object ob1 = (object) this.currentCountry;
                    if (ob is Disease)
                      ob1 = (object) this.currentDisease;
                    if (ob is LocalDisease)
                      ob1 = (object) this.currentLocal;
                    flag3 = flag3 && this.CheckParameters(condition.parameterConditions, ob1, test, condition.combinationOperation) != 0.0;
                    if (!flag3)
                      break;
                  }
                }
                this.currentDisease = currentDisease;
                this.currentLocal = currentLocal;
                flag2 = flag3;
              }
              else if (condition.mapOperation == EMapOp.ANY_ONE || condition.mapOperation == EMapOp.ONE_OTHER)
              {
                bool flag4 = false;
                Disease currentDisease = this.currentDisease;
                LocalDisease currentLocal = this.currentLocal;
                for (int index3 = 0; index3 < World.instance.diseases.Count; ++index3)
                {
                  int index4 = (index3 + World.instance.DiseaseTurn) % World.instance.diseases.Count;
                  this.currentDisease = World.instance.diseases[index4];
                  if (this.currentDisease != currentDisease || condition.mapOperation != EMapOp.ONE_OTHER)
                  {
                    if (this.currentCountry != null)
                      this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
                    object ob2 = (object) this.currentCountry;
                    if (ob is Disease)
                      ob2 = (object) this.currentDisease;
                    if (ob is LocalDisease)
                      ob2 = (object) this.currentLocal;
                    if (this.CheckParameters(condition.parameterConditions, ob2, test, condition.combinationOperation) != 0.0)
                    {
                      if (!string.IsNullOrEmpty(condition.mapReference))
                        this.diseaseMapReference[condition.mapReference] = this.currentDisease;
                      flag4 = true;
                      break;
                    }
                  }
                }
                this.currentDisease = currentDisease;
                this.currentLocal = currentLocal;
                flag2 = flag4;
              }
            }
            else
              flag2 = this.CheckParameters(condition.parameterConditions, ob, test, condition.combinationOperation) != 0.0;
          }
          else
            flag2 = this.ResolveCondition(condition, ob, test, debug, debugName);
          if (test && this._test_randomEncountered)
          {
            this._test_randomEncountered = false;
            flag2 = true;
          }
          if (parentOperation == ParameterCondition.ECombinationOp.NOT)
          {
            flag1 = !flag2;
            break;
          }
          if (index1 == 0)
          {
            flag1 = flag2;
          }
          else
          {
            switch (parentOperation)
            {
              case ParameterCondition.ECombinationOp.NONE:
              case ParameterCondition.ECombinationOp.AND:
                flag1 &= flag2;
                break;
              case ParameterCondition.ECombinationOp.OR:
                flag1 |= flag2;
                break;
            }
          }
          if (flag1 && parentOperation == ParameterCondition.ECombinationOp.OR || !flag1 && (parentOperation == ParameterCondition.ECombinationOp.AND || parentOperation == ParameterCondition.ECombinationOp.NONE))
            break;
        }
        return flag1 ? 1.0 : 0.0;
    }
  }

  private double GetDoubleOperand(
    ParameterCondition[] conditions,
    object ob,
    bool test,
    int position)
  {
    double doubleOperand = 0.0;
    ob = this.GetConditionTargetObject(conditions[position], ob);
    if (conditions[position].parameterConditions != null)
    {
      ParameterCondition condition = conditions[position];
      if (condition.sumOperation != EMapOp.SELF)
      {
        if (this.currentEvent.debug)
          Debug.Log((object) (this.currentEvent.name + " SUM OP: " + (object) condition.sumOperation));
        if (condition.sumOperation == EMapOp.ALL || condition.mapOperation == EMapOp.ALL_OTHER)
        {
          Disease currentDisease = this.currentDisease;
          LocalDisease currentLocal = this.currentLocal;
          ParameterCondition.ECombinationOp parentOperation = condition.combinationOperation;
          if (parentOperation == ParameterCondition.ECombinationOp.NONE)
            parentOperation = ParameterCondition.ECombinationOp.PLUS;
          for (int index = 0; index < World.instance.diseases.Count; ++index)
          {
            this.currentDisease = World.instance.diseases[index];
            if (this.currentDisease != currentDisease || condition.mapOperation != EMapOp.ALL_OTHER)
            {
              if (this.currentCountry != null)
                this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
              object ob1 = (object) this.currentCountry;
              if (ob is Disease)
                ob1 = (object) this.currentDisease;
              if (ob is LocalDisease)
                ob1 = (object) this.currentLocal;
              doubleOperand += this.CheckParameters(condition.parameterConditions, ob1, test, parentOperation);
            }
          }
          if (this.currentEvent.debug)
            Debug.Log((object) (this.currentEvent.name + " - SUM ALL: " + (object) doubleOperand));
          this.currentDisease = currentDisease;
          this.currentLocal = currentLocal;
        }
        else if (condition.sumOperation == EMapOp.ANY_ONE || condition.sumOperation == EMapOp.ONE_OTHER)
        {
          Disease currentDisease = this.currentDisease;
          LocalDisease currentLocal = this.currentLocal;
          ParameterCondition.ECombinationOp parentOperation = condition.combinationOperation;
          if (parentOperation == ParameterCondition.ECombinationOp.NONE)
            parentOperation = ParameterCondition.ECombinationOp.PLUS;
          for (int index1 = 0; index1 < World.instance.diseases.Count; ++index1)
          {
            int index2 = (index1 + World.instance.DiseaseTurn) % World.instance.diseases.Count;
            this.currentDisease = World.instance.diseases[index2];
            if (this.currentDisease != currentDisease || condition.mapOperation != EMapOp.ONE_OTHER)
            {
              if (this.currentCountry != null)
                this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
              object ob2 = (object) this.currentCountry;
              if (ob is Disease)
                ob2 = (object) this.currentDisease;
              if (ob is LocalDisease)
                ob2 = (object) this.currentLocal;
              doubleOperand = this.CheckParameters(condition.parameterConditions, ob2, test, parentOperation);
              if (!string.IsNullOrEmpty(condition.mapReference))
              {
                this.diseaseMapReference[condition.mapReference] = this.currentDisease;
                Debug.Log((object) ("TARGET FOR: " + condition.mapReference + " is " + this.currentDisease.name));
                break;
              }
              break;
            }
          }
          if (this.currentEvent.debug)
            Debug.Log((object) (this.currentEvent.name + " - SUM ONE: " + (object) doubleOperand));
          this.currentDisease = currentDisease;
          this.currentLocal = currentLocal;
        }
      }
      else
        doubleOperand = this.CheckParameters(conditions[position].parameterConditions, ob, test, conditions[position].combinationOperation);
    }
    else if (conditions[position].arguments != null)
    {
      if (conditions[position].arguments.Length > 1)
        Debug.LogWarning((object) (this.currentEvent.name + " has multiple arguments inside a parameter condition - only first is used: " + (object) conditions[position].arguments[0] + " CONDITION: " + (object) position));
      double num = 0.0;
      BaseArgument baseArgument = conditions[position].arguments[0];
      if (baseArgument.GetType() == typeof (RandomVal))
      {
        RandomVal r = baseArgument as RandomVal;
        this._test_randomEncountered = true;
        num = Convert.ToDouble(this.GetRandom(r));
      }
      else if (baseArgument.GetType() == typeof (FieldVal))
      {
        Disease currentDisease = this.currentDisease;
        LocalDisease currentLocal = this.currentLocal;
        FieldVal fieldVal = (FieldVal) baseArgument;
        if (!string.IsNullOrEmpty(fieldVal.reference))
        {
          this.currentDisease = this.diseaseMapReference[fieldVal.reference];
          if (this.currentCountry != null)
            this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
        }
        if (fieldVal.target == EReflectionTarget.COUNTRY)
          ob = (object) this.currentCountry;
        else if (fieldVal.target == EReflectionTarget.DISEASE)
          ob = (object) this.currentDisease;
        else if (fieldVal.target == EReflectionTarget.LOCALDISEASE)
          ob = (object) this.currentLocal;
        else if (this.currentDisease != currentDisease)
        {
          if (ob is Disease)
            ob = (object) this.currentDisease;
          if (ob is LocalDisease)
            ob = (object) this.currentLocal;
        }
        num = Convert.ToDouble(this.GetOtherField(conditions[position].field, ob, typeof (double)));
        this.currentDisease = currentDisease;
        this.currentLocal = currentLocal;
      }
      else if (baseArgument.GetType() == typeof (LiteralVal))
        num = this.CustomDoubleParse((baseArgument as LiteralVal).val);
      else if (baseArgument.GetType() == typeof (TechVal))
      {
        TechVal techVal = baseArgument as TechVal;
        num = this.currentDisease.IsTechEvolved(techVal.techID) != techVal.triggered ? 0.0 : 1.0;
      }
      doubleOperand = num;
    }
    else
      Debug.Log((object) "GetDoubleOperand - failed to parse argument");
    return doubleOperand;
  }

  private double CustomDoubleParse(string input)
  {
    double num1 = 0.0;
    int num2 = input.Length;
    bool flag1 = false;
    bool flag2 = input[0] == '-';
    for (int index = flag2 ? 1 : 0; index < input.Length; ++index)
    {
      char ch = input[index];
      if (ch == '.' || ch == ',')
      {
        if (flag1)
          return double.NaN;
        num2 = index + 1;
        flag1 = true;
      }
      else
      {
        if (ch < '0' || ch > '9')
          return double.NaN;
        num1 = num1 * 10.0 + (double) ((int) ch - 48);
      }
    }
    return (flag2 ? -1.0 : 1.0) * num1 / this.CustomPow(10, input.Length - num2);
  }

  private double CustomPow(int num, int exp)
  {
    double num1 = 1.0;
    while (exp > 0)
    {
      if (exp % 2 == 1)
        num1 *= (double) num;
      exp >>= 1;
      num *= num;
    }
    return num1;
  }

  private object GetConditionTargetObject(ParameterCondition cond, object ob)
  {
    if (cond.target == EReflectionTarget.COUNTRY)
    {
      if (this.currentCountry == null)
      {
        Debug.LogError((object) ("Error in " + this.currentEvent.name + " - tried to target COUNTRY somewhere we have no country..."));
        return (object) false;
      }
      ob = (object) this.currentCountry;
    }
    else if (cond.target == EReflectionTarget.DISEASE)
    {
      if (this.currentDisease == null)
      {
        Debug.LogError((object) ("Error in " + this.currentEvent.name + " - tried to target DISEASE somewhere we have no disease..."));
        return (object) false;
      }
      ob = (object) this.currentDisease;
    }
    else if (cond.target == EReflectionTarget.LOCALDISEASE)
    {
      if (this.currentLocal == null)
      {
        Debug.LogError((object) ("Error in " + this.currentEvent.name + " - tried to target LOCAL somewhere we have no local disease..."));
        return (object) false;
      }
      ob = (object) this.currentLocal;
    }
    return ob;
  }

  private bool ResolveCondition(
    ParameterCondition cond,
    object ob,
    bool test,
    bool debug,
    string debugName)
  {
    ob = this.GetConditionTargetObject(cond, ob);
    if (cond.techTrigger != null)
      return cond.techTrigger.triggered == this.currentDisease.techEvolved.Contains(cond.techTrigger.techID);
    if (cond.eventTrigger != null)
    {
      IDictionary<int, int> dictionary;
      return (cond.eventTrigger.triggered ? 1 : 0) == (!World.instance.firedEvents.TryGetValue(cond.eventTrigger.eventID, out dictionary) ? 0 : (dictionary.TryGetValue(this.currentDisease.id, out int _) ? 1 : 0));
    }
    System.Type type;
    object empty;
    if (cond.fieldInfo == (FieldInfo) null)
    {
      if (cond.propInfo == (PropertyInfo) null)
      {
        Debug.LogError((object) (debugName + " CONDITION: " + ob + " does not have field named: " + cond.fieldName));
        return false;
      }
      type = cond.propInfo.PropertyType;
      empty = cond.propInfo.GetValue(ob, (object[]) null);
    }
    else
    {
      type = cond.fieldInfo.FieldType;
      empty = cond.fieldInfo.GetValue(ob);
    }
    if (empty == null)
    {
      if (type == typeof (string))
      {
        empty = (object) string.Empty;
      }
      else
      {
        Debug.LogError((object) (debugName + " field: " + (object) cond.fieldInfo + " prop: " + (object) cond.propInfo + " on " + ob + " is null"));
        return false;
      }
    }
    IComparable comparable1 = empty as IComparable;
    bool flag = false;
    IComparable comparable2 = (IComparable) null;
    try
    {
      if (cond.random != null)
        comparable2 = this.GetRandom(cond.random) as IComparable;
      else if (cond.field != null)
        comparable2 = this.GetOtherField(cond.field, ob, type) as IComparable;
      else if (cond.targetOb != null)
      {
        comparable2 = cond.targetOb;
      }
      else
      {
        comparable2 = !type.IsEnum ? Convert.ChangeType((object) cond.val, type) as IComparable : Enum.Parse(type, cond.val) as IComparable;
        cond.targetOb = comparable2;
      }
      switch (cond.comparison)
      {
        case ParameterCondition.EComparison.EQUALS:
          flag = comparable1.CompareTo((object) comparable2) == 0;
          break;
        case ParameterCondition.EComparison.LESS_THAN:
          flag = comparable1.CompareTo((object) comparable2) < 0;
          break;
        case ParameterCondition.EComparison.GREATER_THAN:
          flag = comparable1.CompareTo((object) comparable2) > 0;
          break;
        case ParameterCondition.EComparison.LESS_THAN_OR_EQUAL:
          flag = comparable1.CompareTo((object) comparable2) <= 0;
          break;
        case ParameterCondition.EComparison.GREATER_THAN_OR_EQUAL:
          flag = comparable1.CompareTo((object) comparable2) >= 0;
          break;
        case ParameterCondition.EComparison.NOT_EQUAL:
          flag = comparable1.CompareTo((object) comparable2) != 0;
          break;
        default:
          Debug.LogError((object) (debugName + " effect on " + ob + " field: " + cond.fieldName + ": Unsupported comparison: " + (object) cond.comparison));
          break;
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) (debugName + " - Error comparing " + cond.fieldName + " on " + (object) ob.GetType() + " to '" + cond.val + "' " + (object) comparable1 + " " + (object) cond.comparison + " " + (object) comparable2 + "\n" + (object) ex));
    }
    return flag;
  }

  private static void TryCacheLookupData(ParameterCondition cond, object ob)
  {
    if (string.IsNullOrEmpty(cond.fieldName))
      return;
    System.Type type;
    switch (cond.target)
    {
      case EReflectionTarget.COUNTRY:
        type = typeof (Country);
        break;
      case EReflectionTarget.DISEASE:
        type = typeof (Disease);
        break;
      case EReflectionTarget.LOCALDISEASE:
        type = typeof (LocalDisease);
        break;
      default:
        type = ob.GetType();
        break;
    }
    if (!(cond.fieldInfo == (FieldInfo) null) || !(cond.propInfo == (PropertyInfo) null))
      return;
    cond.fieldInfo = type.GetField(cond.fieldName);
    if (!(cond.fieldInfo == (FieldInfo) null))
      return;
    cond.propInfo = type.GetProperty(cond.fieldName);
  }

  private object GetTarget(EReflectionTarget targetType, object ob)
  {
    switch (targetType)
    {
      case EReflectionTarget.COUNTRY:
        return (object) this.currentCountry;
      case EReflectionTarget.DISEASE:
        return (object) this.currentDisease;
      case EReflectionTarget.LOCALDISEASE:
        return (object) this.currentLocal;
      default:
        return ob;
    }
  }

  private void ApplyParameterEffects(ParameterEffect[] effects, object ob)
  {
    if (effects == null)
      return;
    for (int index1 = 0; index1 < effects.Length; ++index1)
    {
      ParameterEffect effect = effects[index1];
      Disease currentDisease1 = this.currentDisease;
      LocalDisease currentLocal = this.currentLocal;
      if (!string.IsNullOrEmpty(effect.mapReference))
      {
        this.currentDisease = this.diseaseMapReference[effect.mapReference];
        if (this.currentCountry != null)
          this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
        if (ob == currentDisease1)
          ob = (object) this.currentDisease;
        if (ob == currentLocal)
          ob = (object) this.currentLocal;
      }
      if (effect.effects != null && effect.effects.Length != 0)
      {
        if (effect.mapOperation != EMapOp.SELF)
        {
          Disease currentDisease2 = this.currentDisease;
          for (int index2 = 0; index2 < World.instance.diseases.Count; ++index2)
          {
            Disease disease = World.instance.diseases[(index2 + World.instance.DiseaseTurn) % World.instance.diseases.Count];
            if (disease != currentDisease2 || effect.mapOperation != EMapOp.ALL_OTHER && effect.mapOperation != EMapOp.ONE_OTHER)
            {
              this.currentDisease = disease;
              if (this.currentCountry != null)
                this.currentLocal = this.currentCountry.GetLocalDisease(this.currentDisease);
              if (ob is Disease)
                ob = (object) this.currentDisease;
              if (ob is LocalDisease)
                ob = (object) this.currentLocal;
              if (effect.parameters == null || effect.parameters.Length < 1 || this.CheckParameters(effect.parameters, ob, false) != 0.0)
              {
                this.ApplyParameterEffects(effect.effects, ob);
                if (effect.mapOperation == EMapOp.ANY_ONE || effect.mapOperation == EMapOp.ONE_OTHER)
                  break;
              }
            }
          }
        }
        else
          this.ApplyParameterEffects(effect.effects, ob);
      }
      else
      {
        object target = this.GetTarget(effect.target, ob);
        if (target == null)
        {
          Debug.LogError((object) (this.currentEvent.name + ": Unable to retrieve: " + (object) effect.target + " - ob: " + ob + " to apply effect to " + effect.fieldName));
          continue;
        }
        System.Type type1 = target.GetType();
        FieldInfo fieldInfo = effect.fieldInfo;
        PropertyInfo propertyInfo = effect.propInfo;
        System.Type type2 = (System.Type) null;
        if (effect.popFrom.HasValue && effect.popTo.HasValue)
        {
          if (!(target is LocalDisease))
          {
            Debug.LogError((object) (this.currentEvent.name + " EFFECT: " + target + " is not a local disease, cannot affect populations"));
            continue;
          }
          type2 = typeof (double);
        }
        else
        {
          if (fieldInfo == (FieldInfo) null && propertyInfo == (PropertyInfo) null)
          {
            fieldInfo = type1.GetField(effect.fieldName);
            if (fieldInfo == (FieldInfo) null)
            {
              propertyInfo = type1.GetProperty(effect.fieldName);
              if (propertyInfo == (PropertyInfo) null)
              {
                Debug.LogError((object) (this.currentEvent.name + " EFFECT: " + target + " does not have field named: " + effect.fieldName));
                continue;
              }
              effect.propInfo = propertyInfo;
            }
            else
              effect.fieldInfo = fieldInfo;
          }
          if (fieldInfo != (FieldInfo) null)
            type2 = fieldInfo.FieldType;
          if (propertyInfo != (PropertyInfo) null)
            type2 = propertyInfo.PropertyType;
        }
        object changeVal = (object) effect.val;
        try
        {
          if (effect.parameters != null)
            changeVal = (object) this.GetDoubleOperand(effect.parameters, target, false, 0);
          else if (effect.field != null)
            changeVal = this.GetOtherField(effect.field, target, type2);
          else if (effect.random != null)
            changeVal = this.GetRandom(effect.random);
          if (effect.popFrom.HasValue && effect.popTo.HasValue)
          {
            Country country = ((LocalDisease) ob).country;
            Disease disease = ((LocalDisease) ob).disease;
            double number = (double) Convert.ChangeType(changeVal, typeof (double));
            int PT_from = (int) effect.popFrom.Value;
            Disease acting = disease;
            int PT_to = (int) effect.popTo.Value;
            country.TransferPopulation(number, (Country.EPopulationType) PT_from, acting, (Country.EPopulationType) PT_to);
          }
          else if (effect.op == ParameterEffect.EOperator.SET)
          {
            if (type2.IsEnum)
            {
              if (propertyInfo != (PropertyInfo) null)
                propertyInfo.SetValue(target, (object) (Enum.Parse(type2, changeVal.ToString()) as IComparable), (object[]) null);
              else
                fieldInfo.SetValue(target, Enum.Parse(type2, changeVal.ToString()));
            }
            else
            {
              try
              {
                if (propertyInfo != (PropertyInfo) null)
                  propertyInfo.SetValue(target, Convert.ChangeType(changeVal, type2), (object[]) null);
                else
                  fieldInfo.SetValue(target, Convert.ChangeType(changeVal, type2));
              }
              catch
              {
                int num = DataImporter.ParseBool(changeVal.ToString()) ? 1 : 0;
                if (propertyInfo != (PropertyInfo) null)
                  propertyInfo.SetValue(target, Convert.ChangeType((object) num, type2), (object[]) null);
                else
                  fieldInfo.SetValue(target, Convert.ChangeType((object) num, type2));
              }
            }
          }
          else if (propertyInfo != (PropertyInfo) null)
            propertyInfo.SetValue(target, this.ApplyOperation(propertyInfo.GetValue(target, (object[]) null), effect.op, changeVal, type2), (object[]) null);
          else
            fieldInfo.SetValue(target, this.ApplyOperation(fieldInfo.GetValue(target), effect.op, changeVal, type2));
        }
        catch (Exception ex)
        {
          Debug.LogError((object) (this.currentEvent.name + " - Error setting '" + effect.fieldName + "' " + (object) fieldInfo + "/" + (object) propertyInfo + " on " + target + " to '" + changeVal + "'\n" + (object) ex));
        }
      }
      this.currentDisease = currentDisease1;
      this.currentLocal = currentLocal;
    }
  }

  private object GetRandom(RandomVal r)
  {
    this._test_randomEncountered = true;
    return r.isFloat ? (object) ModelUtils.FloatRand(r.min, r.max) : (object) ModelUtils.IntRand((int) r.min, (int) r.max);
  }

  private object GetOtherField(FieldVal field, object ob, System.Type outputType)
  {
    if (field.target == EReflectionTarget.DISEASE)
      ob = (object) this.currentDisease;
    else if (field.target == EReflectionTarget.LOCALDISEASE)
      ob = (object) this.currentLocal;
    else if (field.target == EReflectionTarget.COUNTRY)
      ob = (object) this.currentCountry;
    System.Type type = ob.GetType();
    if (field.fieldInfo == (FieldInfo) null && field.propInfo == (PropertyInfo) null)
      field.fieldInfo = type.GetField(field.fieldName);
    object startVal;
    if (field.fieldInfo == (FieldInfo) null)
    {
      if (field.propInfo == (PropertyInfo) null)
        field.propInfo = type.GetProperty(field.fieldName);
      if (field.propInfo != (PropertyInfo) null)
      {
        startVal = field.propInfo.GetValue(ob, (object[]) null);
      }
      else
      {
        Debug.LogError((object) (this.currentEvent.name + " comparison field " + field.fieldName + " on " + ob + " not found"));
        return (object) null;
      }
    }
    else
      startVal = field.fieldInfo.GetValue(ob);
    if (startVal == null)
      Debug.LogError((object) (this.currentEvent.name + " Comparison field value for " + field.fieldName + " on " + ob + " is null"));
    if (field.val == null)
    {
      if (field.randoms.Count > 0)
      {
        for (int index = 0; index < field.randoms.Count; ++index)
          startVal = this.ApplyOperation(startVal, field.op, this.GetRandom(field.randoms[index]), outputType);
      }
      if (field.fields.Count > 0)
      {
        for (int index = 0; index < field.fields.Count; ++index)
          startVal = this.ApplyOperation(startVal, field.op, this.GetOtherField(field.fields[index], ob, outputType), outputType);
      }
      return startVal;
    }
    if (field.op != ParameterEffect.EOperator.SET)
      return this.ApplyOperation(startVal, field.op, (object) field.val, outputType);
    Debug.LogError((object) (this.currentEvent.name + " Cannot use SET on a comparison field: " + field.fieldName));
    return (object) null;
  }

  private object ApplyOperation(
    object startVal,
    ParameterEffect.EOperator op,
    object changeVal,
    System.Type outputFieldType)
  {
    if (outputFieldType == typeof (int))
    {
      int num1 = (int) Convert.ChangeType(changeVal, typeof (int));
      int num2 = (int) startVal;
      switch (op)
      {
        case ParameterEffect.EOperator.PLUS:
          num2 += num1;
          break;
        case ParameterEffect.EOperator.MINUS:
          num2 -= num1;
          break;
        case ParameterEffect.EOperator.TIMES:
          num2 *= num1;
          break;
        case ParameterEffect.EOperator.DIVIDED_BY:
          if (num1 != 0)
          {
            num2 /= num1;
            break;
          }
          num2 = 0;
          break;
        default:
          Debug.LogError((object) ("Unsupported operator: " + (object) op));
          break;
      }
      return (object) num2;
    }
    if (outputFieldType == typeof (float))
    {
      float num3 = (float) Convert.ChangeType(changeVal, typeof (float));
      float num4 = (float) Convert.ChangeType(startVal, typeof (float));
      switch (op)
      {
        case ParameterEffect.EOperator.PLUS:
          num4 += num3;
          break;
        case ParameterEffect.EOperator.MINUS:
          num4 -= num3;
          break;
        case ParameterEffect.EOperator.TIMES:
          num4 *= num3;
          break;
        case ParameterEffect.EOperator.DIVIDED_BY:
          if ((double) num3 != 0.0)
          {
            num4 /= num3;
            break;
          }
          num4 = 0.0f;
          break;
        default:
          Debug.LogError((object) ("Unsupported operator: " + (object) op));
          break;
      }
      return (object) num4;
    }
    if (outputFieldType == typeof (double))
    {
      double num5 = (double) Convert.ChangeType(changeVal, typeof (double));
      double num6 = (double) Convert.ChangeType(startVal, typeof (double));
      switch (op)
      {
        case ParameterEffect.EOperator.PLUS:
          num6 += num5;
          break;
        case ParameterEffect.EOperator.MINUS:
          num6 -= num5;
          break;
        case ParameterEffect.EOperator.TIMES:
          num6 *= num5;
          break;
        case ParameterEffect.EOperator.DIVIDED_BY:
          if (num5 != 0.0)
          {
            num6 /= num5;
            break;
          }
          num6 = 0.0;
          break;
        default:
          Debug.LogError((object) ("Unsupported operator: " + (object) op));
          break;
      }
      return (object) num6;
    }
    if (outputFieldType == typeof (long))
    {
      double num7 = (double) Convert.ChangeType(changeVal, typeof (double));
      long num8 = (long) startVal;
      switch (op)
      {
        case ParameterEffect.EOperator.PLUS:
          num8 = (long) ((double) num8 + num7);
          break;
        case ParameterEffect.EOperator.MINUS:
          num8 = (long) ((double) num8 - num7);
          break;
        case ParameterEffect.EOperator.TIMES:
          num8 = (long) ((double) num8 * num7);
          break;
        case ParameterEffect.EOperator.DIVIDED_BY:
          num8 = num7 == 0.0 ? 0L : (long) ((double) num8 / num7);
          break;
        default:
          Debug.LogError((object) ("Unsupported operator: " + (object) op));
          break;
      }
      return (object) num8;
    }
    Debug.LogError((object) ("Unable to use: " + (object) op + " on variable of type: " + (object) outputFieldType));
    return (object) null;
  }

  private bool CheckDiseaseCondition(
    World world,
    DiseaseCondition diseaseCondition,
    Disease disease,
    bool test)
  {
    bool flag1 = true;
    if (diseaseCondition.preemptiveTech != null && diseaseCondition.preemptiveTech.Length != 0)
    {
      Technology technology = disease.SelectRandomTech(diseaseCondition.preemptiveTech);
      if (technology != null)
      {
        disease.preemptiveTech = technology.id;
        disease.preemptiveName = technology.name;
      }
    }
    if (diseaseCondition.hasFlags != null)
    {
      foreach (Disease.EGenericDiseaseFlag hasFlag in diseaseCondition.hasFlags)
      {
        if (!disease.HasFlag(hasFlag))
          return false;
      }
    }
    if (diseaseCondition.eventsFired != null)
    {
      if (diseaseCondition.anyEventFired)
      {
        flag1 = false;
        for (int index = 0; index < diseaseCondition.eventsFired.Length; ++index)
        {
          string key = diseaseCondition.eventsFired[index];
          if (this.eventList.ContainsKey(key) && world.firedEvents.ContainsKey(key) && world.firedEvents[key].ContainsKey(disease.id))
          {
            flag1 = true;
            break;
          }
        }
        if (flag1)
          ;
      }
      else
      {
        for (int index = 0; index < diseaseCondition.eventsFired.Length; ++index)
        {
          string key = diseaseCondition.eventsFired[index];
          if (this.eventList.ContainsKey(key) && !this.eventList[key].finished && (!world.firedEvents.ContainsKey(key) || !world.firedEvents[key].ContainsKey(disease.id)))
          {
            flag1 = false;
            break;
          }
        }
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.eventsNotFired != null)
    {
      for (int index = 0; index < diseaseCondition.eventsNotFired.Length; ++index)
      {
        string key = diseaseCondition.eventsNotFired[index];
        if (this.eventList.ContainsKey(key) && world.firedEvents.ContainsKey(key) && world.firedEvents[key].ContainsKey(disease.id))
        {
          flag1 = false;
          break;
        }
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.techNotResearched != null)
    {
      try
      {
        for (int index = 0; index < diseaseCondition.techNotResearched.Length; ++index)
        {
          string preemptiveTech = diseaseCondition.techNotResearched[index];
          if (preemptiveTech == "disease.preemptiveTech")
            preemptiveTech = disease.preemptiveTech;
          if (disease.IsTechEvolved(preemptiveTech))
          {
            flag1 = false;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error evaluating techNotRexearched in " + this.currentEvent.name + "\n" + (object) ex));
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.notHasTech != null)
    {
      if (diseaseCondition.notHasTech.Length != 0)
      {
        try
        {
          for (int index1 = 0; index1 < diseaseCondition.notHasTech.Length; ++index1)
          {
            TechSelect techSelect = diseaseCondition.notHasTech[index1];
            for (int index2 = 0; index2 < disease.technologies.Count; ++index2)
            {
              Technology technology = disease.technologies[index2];
              if (disease.IsTechEvolved(technology) == techSelect.researched && World.instance.eventManager.EvaluateParameters(techSelect.conditions, (object) technology) > 0.0)
              {
                flag1 = false;
                break;
              }
            }
            if (!flag1)
              break;
          }
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Error evaluating notHasTech in " + this.currentEvent.name + "\n" + (object) ex));
        }
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.techResearched != null)
    {
      if (diseaseCondition.techResearched.Length == 0)
      {
        if (disease.techEvolved.Count > 0)
          flag1 = false;
      }
      else if (diseaseCondition.anyTechResearched)
      {
        flag1 = false;
        for (int index = 0; index < diseaseCondition.techResearched.Length; ++index)
        {
          string techID = diseaseCondition.techResearched[index];
          if (disease.IsTechEvolved(techID))
          {
            flag1 = true;
            break;
          }
        }
      }
      else
      {
        for (int index = 0; index < diseaseCondition.techResearched.Length; ++index)
        {
          string preemptiveTech = diseaseCondition.techResearched[index];
          if (preemptiveTech == "disease.preemptiveTech")
            preemptiveTech = disease.preemptiveTech;
          if (!disease.IsTechEvolved(preemptiveTech))
          {
            flag1 = false;
            break;
          }
        }
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.hasTech != null && diseaseCondition.hasTech.Length != 0)
    {
      flag1 = true;
      try
      {
        for (int index3 = 0; index3 < diseaseCondition.hasTech.Length; ++index3)
        {
          TechSelect techSelect = diseaseCondition.hasTech[index3];
          bool flag2 = false;
          for (int index4 = 0; index4 < disease.technologies.Count; ++index4)
          {
            Technology technology = disease.technologies[index4];
            if (disease.IsTechEvolved(technology) == techSelect.researched && World.instance.eventManager.EvaluateParameters(techSelect.conditions, (object) technology) > 0.0)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            flag1 = false;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error evaluating notHasTech in " + this.currentEvent.name + "\n" + (object) ex));
      }
    }
    if (!flag1)
      return flag1;
    if (diseaseCondition.parameterConditions != null && diseaseCondition.parameterConditions.Length != 0)
      flag1 = this.CheckParameters(diseaseCondition.parameterConditions, (object) disease, test) != 0.0;
    if (!flag1 || diseaseCondition.randomRoll <= 0 || diseaseCondition.randomChance <= 0 || ModelUtils.IntRand(0, diseaseCondition.randomRoll) < diseaseCondition.randomChance)
      return flag1;
    flag1 = false;
    return flag1;
  }

  private static string GetRandomCountry()
  {
    int index = ModelUtils.IntRand(0, World.instance.countries.Count - 1);
    return World.instance.countries[index].id;
  }

  public void ApplySubOutcome(
    Disease disease,
    LocalDisease localDisease,
    World world,
    Country country,
    EventOutcome oc,
    GameEvent ev,
    bool applyFurther = false)
  {
    if (oc == null)
    {
      Debug.Log((object) ("Outcome null " + ev.name));
    }
    else
    {
      bool flag = false;
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.Tutorial)
      {
        if (!TutorialSystem.IsModuleSectionComplete("Free Play"))
          this.diseaseTurn = world.DiseaseTurn;
        flag = world.DiseaseTurn - this.diseaseTurn < 10;
      }
      if (oc.gameEndMessage != null)
      {
        disease.endMessageImage = oc.gameEndMessage.icon;
        disease.endMessageText = oc.gameEndMessage.text;
        disease.endMessageTitle = oc.gameEndMessage.title;
        World.instance.ShowGameEndMessage(disease);
      }
      if (oc.diseaseEffect != null && !flag)
      {
        if (oc.diseaseEffect.setFlag != Disease.EGenericDiseaseFlag.None)
          disease.AddFlag(oc.diseaseEffect.setFlag);
        if (oc.diseaseEffect.evolveRandomTech)
          World.instance.EvolveRandomTech(disease, oc.diseaseEffect.randomTech, country);
        if (oc.diseaseEffect.deEvolveRandomTech)
          World.instance.DeEvolveRandomTech(disease);
        if (oc.diseaseEffect.eventLockTech != null)
        {
          for (int index = 0; index < oc.diseaseEffect.eventLockTech.Length; ++index)
          {
            EventLockTech eventLockTech = oc.diseaseEffect.eventLockTech[index];
            if (!string.IsNullOrEmpty(eventLockTech.id))
              disease.EventLockTech(eventLockTech.id, eventLockTech.locked);
            else
              disease.EventLockTech(eventLockTech.techType, eventLockTech.locked);
          }
        }
        if (oc.diseaseEffect.padlockTech != null)
        {
          for (int index = 0; index < oc.diseaseEffect.padlockTech.Length; ++index)
          {
            EventLockTech eventLockTech = oc.diseaseEffect.padlockTech[index];
            if (!string.IsNullOrEmpty(eventLockTech.id))
              disease.PadlockTech(eventLockTech.id, eventLockTech.locked);
            else
              disease.PadlockTech(eventLockTech.techType, eventLockTech.locked);
          }
        }
        if (!string.IsNullOrEmpty(oc.diseaseEffect.evolveTech))
          disease.EvolveTech(disease.GetTechnology(oc.diseaseEffect.evolveTech), true);
        this.ApplyParameterEffects(oc.diseaseEffect.parameterEffects, (object) disease);
        if (!string.IsNullOrEmpty(oc.diseaseEffect.function))
        {
          MethodInfo method = typeof (Disease).GetMethod(oc.diseaseEffect.function);
          if (method != (MethodInfo) null)
          {
            if (method.IsDefined(typeof (GameEventFunction), false))
            {
              if (method.GetParameters().Length == 1)
                method.Invoke((object) disease, new object[1]
                {
                  (object) country
                });
              else if (method.GetParameters().Length < 1)
              {
                method.Invoke((object) disease, (object[]) null);
              }
              else
              {
                int length = method.GetParameters().Length;
                string[] strArray = new string[length];
                for (int index = 0; index < length; ++index)
                {
                  if (index == 0)
                    strArray[index] = disease.gameEventFunctionParameter1;
                  if (index == 1)
                    strArray[index] = disease.gameEventFunctionParameter2;
                  if (index == 2)
                    strArray[index] = disease.gameEventFunctionParameter3;
                  if (index == 3)
                    strArray[index] = disease.gameEventFunctionParameter4;
                  if (index == 4)
                    strArray[index] = disease.gameEventFunctionParameter5;
                }
                for (int index = 0; index < length; ++index)
                {
                  if (strArray[index].ToLower().Equals("thisCountry".ToLower()))
                    strArray[index] = country.id;
                  if (strArray[index].ToLower().Equals("randomCountry".ToLower()))
                    strArray[index] = GameEventManager.GetRandomCountry();
                }
                MethodInfo methodInfo = method;
                object[] objArray = (object[]) strArray;
                Disease disease1 = disease;
                object[] parameters = objArray;
                methodInfo.Invoke((object) disease1, parameters);
              }
            }
            else
              Debug.LogError((object) ("method: " + oc.diseaseEffect.function + " was not marked as [GameEventFunction]"));
          }
          else
            Debug.LogError((object) ("Could not find method: " + oc.diseaseEffect.function));
        }
      }
      if (oc.localEffect != null && oc.localEffect.parameterEffects.Length != 0)
      {
        if (country == null)
        {
          country = this.currentCountry = World.instance.countries[ModelUtils.IntRand(0, World.instance.countries.Count - 1)];
          localDisease = this.currentLocal = this.currentCountry.GetLocalDisease(disease);
        }
        if (localDisease == null)
          localDisease = this.currentLocal = this.currentCountry.GetLocalDisease(disease);
        this.ApplyParameterEffects(oc.localEffect.parameterEffects, (object) localDisease);
      }
      if (oc.countryEffect != null)
      {
        if (country == null)
        {
          country = this.currentCountry = World.instance.countries[ModelUtils.IntRand(0, World.instance.countries.Count - 1)];
          localDisease = this.currentLocal = country.GetLocalDisease(disease);
        }
        if (oc.countryEffect.parameterEffects != null && oc.countryEffect.parameterEffects.Length != 0)
          this.ApplyParameterEffects(oc.countryEffect.parameterEffects, (object) country);
        if (oc.countryEffect.apeLabStatus.HasValue)
        {
          if (disease.diseaseType == Disease.EDiseaseType.VAMPIRE)
            Debug.Log((object) ("Event changed vampire lab state in: " + country.id + " from: " + (object) country.apeLabStatus + " to: " + (object) oc.countryEffect.apeLabStatus.Value));
          country.ChangeApeLabStateF(oc.countryEffect.apeLabStatus.Value);
        }
      }
      if (oc.newsMessages != null)
      {
        EventOutcome.EEventNewsVisibility? visibility;
        if (!CGameManager.IsMultiplayerGame)
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility = EventOutcome.EEventNewsVisibility.Other;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility & visibility.HasValue))
            goto label_82;
        }
        visibility = oc.visibility;
        EventOutcome.EEventNewsVisibility eeventNewsVisibility1 = EventOutcome.EEventNewsVisibility.Public;
        if (!(visibility.GetValueOrDefault() == eeventNewsVisibility1 & visibility.HasValue))
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility2 = EventOutcome.EEventNewsVisibility.Private;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility2 & visibility.HasValue) || disease != CNetworkManager.network.LocalPlayerInfo.disease)
          {
            visibility = oc.visibility;
            EventOutcome.EEventNewsVisibility eeventNewsVisibility3 = EventOutcome.EEventNewsVisibility.Other;
            if (!(visibility.GetValueOrDefault() == eeventNewsVisibility3 & visibility.HasValue) || CGameManager.IsAIGame || disease != CNetworkManager.network.OpponentPlayerInfo.disease)
              goto label_92;
          }
        }
label_82:
        for (int index = 0; index < oc.newsMessages.Count; ++index)
        {
          if ((!oc.newsMessages[index].onlyIfNotNoticed || !disease.diseaseNoticed) && !string.IsNullOrEmpty(oc.newsMessages[index].text))
          {
            int p = 1;
            if (oc.newsPriorities != null)
            {
              if (oc.newsPriorities.Count > index)
                p = oc.newsPriorities[index];
              else if (oc.newsPriorities.Count > 0)
                p = oc.newsPriorities[0];
            }
            world.AddNewsItem(new IGame.NewsItem(oc.newsMessages[index], disease, country, p));
          }
        }
      }
label_92:
      if (oc.debugMessage != null)
        Debug.Log((object) oc.debugMessage);
      if (oc.popupTitle != null && !string.IsNullOrEmpty(oc.popupTitle.text) && !flag)
      {
        EventOutcome.EEventNewsVisibility? visibility;
        if (!CGameManager.IsMultiplayerGame)
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility = EventOutcome.EEventNewsVisibility.Other;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility & visibility.HasValue))
            goto label_100;
        }
        visibility = oc.visibility;
        EventOutcome.EEventNewsVisibility eeventNewsVisibility4 = EventOutcome.EEventNewsVisibility.Public;
        if (!(visibility.GetValueOrDefault() == eeventNewsVisibility4 & visibility.HasValue))
        {
          visibility = oc.visibility;
          EventOutcome.EEventNewsVisibility eeventNewsVisibility5 = EventOutcome.EEventNewsVisibility.Private;
          if (!(visibility.GetValueOrDefault() == eeventNewsVisibility5 & visibility.HasValue) || disease != CNetworkManager.network.LocalPlayerInfo.disease)
          {
            visibility = oc.visibility;
            EventOutcome.EEventNewsVisibility eeventNewsVisibility6 = EventOutcome.EEventNewsVisibility.Other;
            if (!(visibility.GetValueOrDefault() == eeventNewsVisibility6 & visibility.HasValue) || CGameManager.IsAIGame || disease != CNetworkManager.network.OpponentPlayerInfo.disease)
              goto label_120;
          }
        }
label_100:
        List<Disease> diseaseList = new List<Disease>((IEnumerable<Disease>) World.instance.diseases);
        Country[] countries = new Country[1]{ country };
        int id;
        for (int index = 0; index < oc.popupTitle.parameters.Count; ++index)
        {
          StringParameter parameter = oc.popupTitle.parameters[index];
          if (parameter.field != null)
          {
            string[] strArray1 = parameter.field.Split('.');
            if (strArray1[0] == nameof (disease) && strArray1.Length == 2)
            {
              string[] strArray2 = new string[3]
              {
                strArray1[0],
                null,
                strArray1[1]
              };
              if (string.IsNullOrEmpty(parameter.reference))
              {
                string[] strArray3 = strArray2;
                id = disease.id;
                string str = id.ToString();
                strArray3[1] = str;
              }
              else
              {
                string[] strArray4 = strArray2;
                id = this.diseaseMapReference[parameter.reference].id;
                string str = id.ToString();
                strArray4[1] = str;
              }
              parameter.field = string.Join(".", strArray2);
            }
          }
        }
        for (int index = 0; index < oc.popupMessage.parameters.Count; ++index)
        {
          StringParameter parameter = oc.popupMessage.parameters[index];
          if (parameter.field != null)
          {
            string[] strArray5 = parameter.field.Split('.');
            if (strArray5[0] == nameof (disease) && strArray5.Length == 2)
            {
              string[] strArray6 = new string[3]
              {
                strArray5[0],
                null,
                strArray5[1]
              };
              if (string.IsNullOrEmpty(parameter.reference))
              {
                string[] strArray7 = strArray6;
                id = disease.id;
                string str = id.ToString();
                strArray7[1] = str;
              }
              else
              {
                string[] strArray8 = strArray6;
                id = this.diseaseMapReference[parameter.reference].id;
                string str = id.ToString();
                strArray8[1] = str;
              }
              parameter.field = string.Join(".", strArray6);
            }
          }
        }
        if (CGameManager.gameType != IGame.GameType.CureTutorial)
          world.popupMessages.Add(new PopupMessage(oc.popupIcon, oc.popupTitle, CGameManager.GetDisplayDate(), oc.popupMessage, diseaseList.ToArray(), countries, oc.popupType.HasValue ? oc.popupType.Value : PopupMessage.PopupType.Default));
      }
label_120:
      if (oc.achievement.HasValue && this.allowAchievement)
        world.AddAchievement(oc.achievement.Value);
      if (!oc.dynamicNewsItem || world.dynamicNews.Count <= 0)
        return;
      DynamicNewsItem dynamicNew = world.dynamicNews[UnityEngine.Random.Range(0, world.dynamicNews.Count)];
      string text = dynamicNew.title;
      string content = dynamicNew.content;
      if (string.IsNullOrEmpty(text))
        text = content;
      if (!string.IsNullOrEmpty(text))
      {
        ParameterisedString parameterisedString = new ParameterisedString(text, new string[1]
        {
          "disease.name"
        });
        parameterisedString.useLocalisation = false;
        world.AddNewsItem(new IGame.NewsItem(parameterisedString, disease, country, dynamicNew.priority));
        if (dynamicNew.popup && !string.IsNullOrEmpty(content) && !flag)
          world.popupMessages.Add(new PopupMessage(dynamicNew.icon, parameterisedString, CGameManager.GetDisplayDate(), new ParameterisedString(content, new string[1]
          {
            "disease.name"
          })
          {
            useLocalisation = false
          }, disease, country));
        disease.recentEventCounter = 0;
      }
      world.dynamicNews.Remove(dynamicNew);
      world.dynamicNewsSeen.Add(dynamicNew.id);
    }
  }
}
