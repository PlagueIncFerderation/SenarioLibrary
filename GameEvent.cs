// Decompiled with JetBrains decompiler
// Type: GameEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Xml.Serialization;

#nullable disable
[XmlRoot("GameEvent")]
public class GameEvent
{
  [XmlAttribute("Tutorial")]
  public bool tutorial;
  [XmlAttribute("Debug")]
  public bool debug;
  [XmlIgnore]
  [NonSerialized]
  public bool finished;
  [XmlIgnore]
  [NonSerialized]
  public bool advancedEvent;
  [XmlAttribute("RecentEventCounter")]
  public int recentEventCounter;
  [XmlAttribute("ResetRecentEventCounter")]
  public bool resetRecentEventCounter = true;
  public int lastTurn;

  [XmlAttribute("Name")]
  public string name { get; set; }

  [XmlAttribute("Editable")]
  public GameEvent.EEventEditType editable { get; set; }

  [XmlAttribute("Scenarios")]
  public GameEvent.EEventScenarioType scenarios { get; set; }

  [XmlAttribute("RequireScenario")]
  public string requireScenario { get; set; }

  [XmlAttribute("TutorialComplete")]
  public string tutorialComplete { get; set; }

  [XmlAttribute("Limit")]
  public int limit { get; set; }

  [XmlElement("TypeCondition")]
  public TypeCondition typeCondition { get; set; }

  [XmlElement("DiseaseCondition")]
  public DiseaseCondition diseaseCondition { get; set; }

  [XmlElement("EnableCondition")]
  public EnableEventCondition enableCondition { get; set; }

  [XmlElement("CountryCondition")]
  public CountryCondition countryCondition { get; set; }

  [XmlElement("LocalCondition")]
  public LocalCondition localCondition { get; set; }

  [XmlArray("Outcomes")]
  [XmlArrayItem("Outcome")]
  public EventOutcome[] eventOutcomes { get; set; }

  public GameEvent Clone()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlSerializer xmlSerializer = new XmlSerializer(typeof (GameEvent));
    xmlSerializer.Serialize((Stream) memoryStream, (object) this);
    memoryStream.Position = 0L;
    return xmlSerializer.Deserialize((Stream) memoryStream) as GameEvent;
  }

  public enum EEventDiseaseType
  {
    ALL,
    STANDARD,
    NECROA,
    SIMIAN,
    CUSTOM,
  }

  public enum EEventEditType
  {
    HIDDEN,
    PROTECTED,
    EDITABLE,
  }

  public enum EEventScenarioType
  {
    ALL,
    Empty,
    RESTRICTED,
  }
}
