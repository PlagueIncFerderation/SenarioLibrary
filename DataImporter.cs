// Decompiled with JetBrains decompiler
// Type: DataImporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

#nullable disable
public class DataImporter
{
  public static List<GovernmentAction> ImportGovernmentActions(
    string txt,
    GovernmentAction.EActionDiseaseType type)
  {
    List<GovernmentAction> governmentActionList = new List<GovernmentAction>();
    Dictionary<string, GovernmentAction> hash = new Dictionary<string, GovernmentAction>();
    DataImporter.UnserializeDataHash<GovernmentAction>((IDictionary<string, GovernmentAction>) hash, txt, "government_actions");
    foreach (KeyValuePair<string, GovernmentAction> keyValuePair in (IEnumerable<KeyValuePair<string, GovernmentAction>>) hash)
    {
      keyValuePair.Value.id = keyValuePair.Key;
      keyValuePair.Value.type = type;
      keyValuePair.Value.OnImport();
      governmentActionList.Add(keyValuePair.Value);
    }
    return governmentActionList;
  }

  public static List<DynamicNewsItem> ImportDynamicNews(string txt)
  {
    List<DynamicNewsItem> dynamicNewsItemList = new List<DynamicNewsItem>();
    Dictionary<string, DynamicNewsItem> hash = new Dictionary<string, DynamicNewsItem>();
    DataImporter.UnserializeDataHash<DynamicNewsItem>((IDictionary<string, DynamicNewsItem>) hash, txt, "news");
    foreach (KeyValuePair<string, DynamicNewsItem> keyValuePair in (IEnumerable<KeyValuePair<string, DynamicNewsItem>>) hash)
    {
      keyValuePair.Value.id = keyValuePair.Key;
      dynamicNewsItemList.Add(keyValuePair.Value);
    }
    return dynamicNewsItemList;
  }

  public static ScenarioInformation GetScenarioInformation(string textData)
  {
    Dictionary<string, ScenarioInformation> hash = new Dictionary<string, ScenarioInformation>();
    DataImporter.UnserializeDataHash<ScenarioInformation>((IDictionary<string, ScenarioInformation>) hash, textData, "scenarios");
    ScenarioInformation si = (ScenarioInformation) null;
    foreach (KeyValuePair<string, ScenarioInformation> keyValuePair in hash)
    {
      keyValuePair.Value.id = keyValuePair.Key;
      si = keyValuePair.Value;
      DataImporter.DesanitiseStrings(si);
    }
    return si;
  }

  public static ScenarioInformation ImportScenarioInformation(string txt)
  {
    ScenarioInformation scenarioInformation = new ScenarioInformation();
    DataImporter.UnserializeData<ScenarioInformation>(scenarioInformation, txt, "Scenario Information");
    DataImporter.DesanitiseStrings(scenarioInformation);
    return scenarioInformation;
  }

  private static void DesanitiseStrings(ScenarioInformation si)
  {
    si.scenDescription = CUtils.DesanitiseString(si.scenDescription);
    si.scenInitPopup1Title = CUtils.DesanitiseString(si.scenInitPopup1Title);
    si.scenInitPopup1Text = CUtils.DesanitiseString(si.scenInitPopup1Text);
    si.scenInitPopup2Title = CUtils.DesanitiseString(si.scenInitPopup2Title);
    si.scenInitPopup2Text = CUtils.DesanitiseString(si.scenInitPopup2Text);
    si.scenEndMessageTitle = CUtils.DesanitiseString(si.scenEndMessageTitle);
    si.scenEndMessageText = CUtils.DesanitiseString(si.scenEndMessageText);
    si.scenEndGameTagline = CUtils.DesanitiseString(si.scenEndGameTagline);
  }

  public static string GetDiseaseFile(Disease.EDiseaseType type) => type.ToString().ToLower();

  public static Disease ImportDisease(
    IGame.GameType type,
    string txt,
    bool loading,
    Scenario scen)
  {
    if (txt == null)
      Debug.Log((object) "Txt is null");
    if (scen == null)
      Debug.Log((object) "Scenario is null");
    Disease disease;
    switch (type)
    {
      case IGame.GameType.VersusMP:
        disease = (Disease) new MPDisease();
        break;
      case IGame.GameType.CoopMP:
        disease = (Disease) new CoopDisease();
        break;
      default:
        disease = (Disease) new SPDisease();
        break;
    }
    DataImporter.UnserializeData<Disease>(disease, txt, "global");
    DataImporter.ImportDiseaseTech(disease, txt, loading, scen);
    return disease;
  }

  public static void ImportDiseaseTech(Disease d, string txt, bool loading, Scenario scen)
  {
    IDictionary<string, Technology> hash = (IDictionary<string, Technology>) new Dictionary<string, Technology>();
    Debug.Log((object) "IMPORTING DISEASE TECH");
    DataImporter.UnserializeDataHash<Technology>(hash, txt, "disease_tech");
    IDictionary<int, List<Technology>> dictionary1 = (IDictionary<int, List<Technology>>) new Dictionary<int, List<Technology>>();
    IDictionary<int, List<Technology>> dictionary2 = (IDictionary<int, List<Technology>>) new Dictionary<int, List<Technology>>();
    IDictionary<int, List<Technology>> dictionary3 = (IDictionary<int, List<Technology>>) new Dictionary<int, List<Technology>>();
    Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
    string str1 = CUtils.LoadData("Game/legacy_sprites");
    char[] chArray = new char[1]{ ';' };
    foreach (string str2 in str1.Split(chArray))
    {
      if (!string.IsNullOrEmpty(str2))
      {
        string[] strArray = str2.Split(':');
        if (strArray.Length == 2 && !dictionary4.ContainsKey(strArray[0]))
          dictionary4[strArray[0].Trim()] = strArray[1].Trim();
      }
    }
    foreach (KeyValuePair<string, Technology> keyValuePair in (IEnumerable<KeyValuePair<string, Technology>>) hash)
    {
      keyValuePair.Value.id = keyValuePair.Key;
      if (dictionary4.ContainsKey(keyValuePair.Value.baseGraphic))
        keyValuePair.Value.baseGraphic = dictionary4[keyValuePair.Value.baseGraphic];
      keyValuePair.Value.requiredTechAND = DataImporter.TechListFromString(keyValuePair.Value.techRequirementAnd);
      keyValuePair.Value.requiredTechOR = DataImporter.TechListFromString(keyValuePair.Value.techRequirementOr);
      keyValuePair.Value.notTechAND = DataImporter.TechListFromString(keyValuePair.Value.notRequirementAnd);
      keyValuePair.Value.notTechOR = DataImporter.TechListFromString(keyValuePair.Value.notRequirementOr);
      keyValuePair.Value.notTechAND.AddRange((IEnumerable<string>) DataImporter.TechListFromString(keyValuePair.Value.techRequirementNotAnd));
      keyValuePair.Value.notTechOR.AddRange((IEnumerable<string>) DataImporter.TechListFromString(keyValuePair.Value.techRequirementNotOr));
      d.technologies.Add(keyValuePair.Value);
      if (keyValuePair.Value.gridType == Technology.ETechType.ability)
      {
        if (!dictionary1.ContainsKey(keyValuePair.Value.gridPosition))
          dictionary1[keyValuePair.Value.gridPosition] = new List<Technology>();
        dictionary1[keyValuePair.Value.gridPosition].Add(keyValuePair.Value);
      }
      else if (keyValuePair.Value.gridType == Technology.ETechType.transmission)
      {
        if (!dictionary2.ContainsKey(keyValuePair.Value.gridPosition))
          dictionary2[keyValuePair.Value.gridPosition] = new List<Technology>();
        dictionary2[keyValuePair.Value.gridPosition].Add(keyValuePair.Value);
      }
      else
      {
        if (!dictionary3.ContainsKey(keyValuePair.Value.gridPosition))
          dictionary3[keyValuePair.Value.gridPosition] = new List<Technology>();
        dictionary3[keyValuePair.Value.gridPosition].Add(keyValuePair.Value);
      }
      if (scen != null && scen.customIcons != null)
      {
        if (scen.customIcons.ContainsKey(keyValuePair.Value.graphicOverlay))
          keyValuePair.Value.customOverlay = scen.customIcons[keyValuePair.Value.graphicOverlay];
        if (scen.customIcons.ContainsKey(keyValuePair.Value.baseGraphic))
          keyValuePair.Value.customGraphic = scen.customIcons[keyValuePair.Value.baseGraphic];
      }
    }
    for (int index = 0; index < d.technologies.Count; ++index)
    {
      Technology technology1 = d.technologies[index];
      IDictionary<int, List<Technology>> dictionary5 = technology1.gridType != Technology.ETechType.transmission ? (technology1.gridType != Technology.ETechType.ability ? dictionary3 : dictionary1) : dictionary2;
      if (technology1.isPreEvolved && !loading)
        d.EvolveTech(technology1, true);
      if ((string.IsNullOrEmpty(technology1.techRequirementOr) || technology1.techRequirementOr == "0" || !string.IsNullOrEmpty(technology1.techRequirementOr) && !(technology1.techRequirementOr == "0") && !DataImporter.JudgeTechnologyOrRequirementValid(d, technology1)) && technology1.startPoint == 0)
      {
        int num1 = technology1.gridPosition - 1;
        int num2 = num1 + num1 / 11;
        int[] numArray;
        if (num2 % 12 > 5)
          numArray = new int[6]{ -1, 1, -6, -5, 6, 7 };
        else
          numArray = new int[6]{ -1, 1, -7, -6, 5, 6 };
        if (num2 % 12 == 0)
        {
          numArray[0] = 0;
          numArray[2] = 0;
          numArray[4] = 0;
        }
        else if (num2 % 6 == 0)
          numArray[0] = 0;
        else if (num2 % 6 == 5)
        {
          numArray[1] = 0;
          numArray[3] = 0;
          numArray[5] = 0;
        }
        else if (num2 % 12 == 10)
          numArray[1] = 0;
        foreach (int num3 in numArray)
        {
          if (num3 != 0)
          {
            int num4 = num2 + num3;
            int key = num4 - num4 / 12 + 1;
            if (dictionary5.ContainsKey(key))
            {
              foreach (Technology technology2 in dictionary5[key])
                technology1.requiredTechOR.Add(technology2.id);
            }
          }
        }
      }
    }
  }

  private static List<string> TechListFromString(string s)
  {
    if (string.IsNullOrEmpty(s) || s == "0")
      return new List<string>();
    return new List<string>((IEnumerable<string>) s.Split(','));
  }

  public static void ApplyLuckyDip(Disease d)
  {
    List<Technology> technologyList1 = new List<Technology>((IEnumerable<Technology>) d.technologies);
    List<Technology> technologyList2 = new List<Technology>();
    while (technologyList2.Count < 5 && technologyList1.Count > 0)
    {
      int index = ModelUtils.IntRand(0, technologyList1.Count - 1);
      Technology technology = d.technologies[index];
      if (!d.IsTechEvolved(technology) && !technologyList2.Contains(technology))
        technologyList2.Add(technology);
      technologyList1.Remove(technology);
    }
    for (int index = 0; index < technologyList2.Count; ++index)
    {
      d.EvolveTech(technologyList2[index], true);
      technologyList2[index].cantDevolve = true;
    }
  }

  public static void ShuffleTech(Disease d)
  {
    IDictionary<Technology.ETechType, List<int>> dictionary1 = (IDictionary<Technology.ETechType, List<int>>) new Dictionary<Technology.ETechType, List<int>>();
    IDictionary<Technology.ETechType, List<int>> dictionary2 = (IDictionary<Technology.ETechType, List<int>>) new Dictionary<Technology.ETechType, List<int>>();
    IDictionary<Technology.ETechType, IDictionary<int, bool>> dictionary3 = (IDictionary<Technology.ETechType, IDictionary<int, bool>>) new Dictionary<Technology.ETechType, IDictionary<int, bool>>();
    for (int index = 0; index < d.technologies.Count; ++index)
    {
      Technology technology = d.technologies[index];
      if (!dictionary2.ContainsKey(technology.gridType))
        dictionary2[technology.gridType] = new List<int>();
      if (!dictionary1.ContainsKey(technology.gridType))
        dictionary1[technology.gridType] = new List<int>();
      if (!dictionary3.ContainsKey(technology.gridType))
        dictionary3[technology.gridType] = (IDictionary<int, bool>) new Dictionary<int, bool>();
      if (!dictionary3[technology.gridType].ContainsKey(technology.gridPosition))
      {
        if (dictionary1[technology.gridType].Contains(technology.gridPosition))
        {
          dictionary3[technology.gridType][technology.gridPosition] = true;
          dictionary1[technology.gridType].Remove(technology.gridPosition);
          dictionary2[technology.gridType].Remove(technology.gridPosition);
        }
        else
        {
          dictionary1[technology.gridType].Add(technology.gridPosition);
          if (technology.startPoint != 0)
            dictionary2[technology.gridType].Add(technology.gridPosition);
        }
      }
    }
    IDictionary<string, int> dictionary4 = (IDictionary<string, int>) new Dictionary<string, int>();
    IDictionary<string, int> dictionary5 = (IDictionary<string, int>) new Dictionary<string, int>();
    for (int index1 = 0; index1 < d.technologies.Count; ++index1)
    {
      Technology technology = d.technologies[index1];
      if (!dictionary3[technology.gridType].ContainsKey(technology.gridPosition))
      {
        technology.requiredTechAND.Clear();
        technology.requiredTechOR.Clear();
        technology.startPoint = 0;
        int index2 = ModelUtils.IntRand(0, dictionary1[technology.gridType].Count - 1);
        int num = dictionary1[technology.gridType][index2];
        dictionary1[technology.gridType].RemoveAt(index2);
        if (dictionary2[technology.gridType].Contains(num))
          technology.startPoint = 1;
        technology.gridPosition = num;
        dictionary4[technology.id] = num;
        dictionary5[technology.id] = technology.startPoint;
      }
    }
    d.shuffleMap = dictionary4;
    d.shuffleStartMap = dictionary5;
    DataImporter.RegenerateGridRequirements(d);
  }

  public static void ApplyShuffle(Disease d)
  {
    if (d.shuffleMap != null)
    {
      foreach (Technology technology in d.technologies)
      {
        if (d.shuffleMap.ContainsKey(technology.id))
        {
          technology.requiredTechAND.Clear();
          technology.requiredTechOR.Clear();
          technology.gridPosition = d.shuffleMap[technology.id];
          technology.startPoint = d.shuffleStartMap[technology.id];
        }
        else
          Debug.LogError((object) ("Applying disease tech shuffle - could not find position for " + technology.id));
      }
      DataImporter.RegenerateGridRequirements(d);
    }
    else
      Debug.LogError((object) "Tried to reapply a shuffle but disease had no map set");
  }

  public static void RegenerateGridRequirements(Disease d)
  {
    Debug.Log((object) "Regenerating Requirements");
    IDictionary<int, Technology> dictionary1 = (IDictionary<int, Technology>) new Dictionary<int, Technology>();
    IDictionary<int, Technology> dictionary2 = (IDictionary<int, Technology>) new Dictionary<int, Technology>();
    IDictionary<int, Technology> dictionary3 = (IDictionary<int, Technology>) new Dictionary<int, Technology>();
    for (int index = 0; index < d.technologies.Count; ++index)
    {
      Technology technology = d.technologies[index];
      if (technology.gridType == Technology.ETechType.ability)
        dictionary1[technology.gridPosition] = technology;
      else if (technology.gridType == Technology.ETechType.symptom)
        dictionary3[technology.gridPosition] = technology;
      else if (technology.gridType == Technology.ETechType.transmission)
        dictionary2[technology.gridPosition] = technology;
    }
    for (int index = 0; index < d.technologies.Count; ++index)
    {
      Technology technology = d.technologies[index];
      IDictionary<int, Technology> dictionary4 = technology.gridType != Technology.ETechType.transmission ? (technology.gridType != Technology.ETechType.ability ? dictionary3 : dictionary1) : dictionary2;
      if (technology.startPoint == 0)
      {
        int num1 = technology.gridPosition - 1;
        int num2 = num1 + num1 / 11;
        int[] numArray;
        if (num2 % 12 > 5)
          numArray = new int[6]{ -1, 1, -6, -5, 6, 7 };
        else
          numArray = new int[6]{ -1, 1, -7, -6, 5, 6 };
        if (num2 % 12 == 0)
        {
          numArray[0] = 0;
          numArray[2] = 0;
          numArray[4] = 0;
        }
        else if (num2 % 6 == 0)
          numArray[0] = 0;
        else if (num2 % 6 == 5)
        {
          numArray[1] = 0;
          numArray[3] = 0;
          numArray[5] = 0;
        }
        else if (num2 % 12 == 10)
          numArray[1] = 0;
        foreach (int num3 in numArray)
        {
          if (num3 != 0)
          {
            int num4 = num2 + num3;
            int key = num4 - num4 / 12 + 1;
            if (dictionary4.ContainsKey(key))
              technology.requiredTechOR.Add(dictionary4[key].id);
          }
        }
      }
    }
  }

  public static string FixLineEndings(string txt)
  {
    return !string.IsNullOrEmpty(txt) ? txt.Replace("\r\n", "\n").Replace("\r", "\n") : txt;
  }

  public static Country.EContinentType ParseContinent(string str)
  {
    switch (str)
    {
      case "Asia":
        return Country.EContinentType.ASIA;
      case "Europe":
        return Country.EContinentType.EUROPE;
      case "Africa":
        return Country.EContinentType.AFRICA;
      case "North_America":
        return Country.EContinentType.NORTH_AMERICA;
      case "South_America":
        return Country.EContinentType.SOUTH_AMERICA;
      default:
        return Country.EContinentType.NONE;
    }
  }

  public static void ImportWorld(
    World w,
    string countryTxt,
    string routeTxt,
    string countryOrderTxt)
  {
    countryTxt = DataImporter.FixLineEndings(countryTxt);
    routeTxt = DataImporter.FixLineEndings(routeTxt);
    w.Clear();
    List<string> sections = DataImporter.GetSections(countryTxt);
    List<Country> countryList1 = new List<Country>();
    IDictionary<string, Country> dictionary1 = (IDictionary<string, Country>) new Dictionary<string, Country>();
    for (int index = 0; index < sections.Count; ++index)
    {
      string sectionName = sections[index].Trim();
      if (!string.IsNullOrEmpty(sectionName) && sectionName[0] != '#')
      {
        Country country = w.CreateCountry();
        country.id = sectionName;
        DataImporter.UnserializeData<Country>(country, countryTxt, sectionName, true);
        if (country.airport)
          countryList1.Add(country);
        country.continentType = DataImporter.ParseContinent(country.continent);
        w.AddCountry(country);
        dictionary1[country.id] = country;
      }
    }
    List<string> lines1 = DataImporter.GetLines(routeTxt, "air_routes");
    IDictionary<Country, IDictionary<Country, bool>> dictionary2 = (IDictionary<Country, IDictionary<Country, bool>>) new Dictionary<Country, IDictionary<Country, bool>>();
    List<TravelRoute> travelRouteList = new List<TravelRoute>();
    for (int index = 0; index < lines1.Count; ++index)
    {
      string str = lines1[index];
      string[] strArray1 = str.Split(':');
      if (strArray1.Length > 1)
      {
        string[] strArray2 = strArray1[0].Split(',');
        float freq;
        try
        {
          freq = float.Parse(strArray1[1]);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Error parsing air route spawn rate: '" + str + "' - " + (object) ex));
          continue;
        }
        TravelRoute travelRoute = new TravelRoute(freq);
        travelRoute.routeType = ERouteType.Air;
        if (!string.IsNullOrEmpty(strArray2[0]))
        {
          if (!string.IsNullOrEmpty(strArray2[1]))
          {
            try
            {
              travelRoute.source = !(strArray2[0] == "-") ? dictionary1[strArray2[0]] : (Country) null;
              travelRoute.destination = !(strArray2[1] == "-") ? dictionary1[strArray2[1]] : (Country) null;
              if (travelRoute.source == null)
              {
                travelRoute.source = travelRoute.destination;
                travelRoute.destination = (Country) null;
              }
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ("Error finding source/destination countries: '" + str + "' - " + (object) ex));
              continue;
            }
            if (travelRoute.source == null && travelRoute.destination == null)
              travelRouteList.Add(travelRoute);
            else if (travelRoute.source.airport && (travelRoute.destination == null || travelRoute.destination.airport))
            {
              if (travelRoute.source != null && travelRoute.destination != null)
              {
                travelRoute.source.airRoutes.Add(travelRoute);
                if (!dictionary2.ContainsKey(travelRoute.source))
                  dictionary2[travelRoute.source] = (IDictionary<Country, bool>) new Dictionary<Country, bool>();
                if (!dictionary2.ContainsKey(travelRoute.destination))
                  dictionary2[travelRoute.destination] = (IDictionary<Country, bool>) new Dictionary<Country, bool>();
                dictionary2[travelRoute.source][travelRoute.destination] = true;
                dictionary2[travelRoute.destination][travelRoute.source] = true;
              }
              if (travelRoute.destination == null)
                travelRouteList.Add(travelRoute);
            }
          }
        }
      }
    }
    for (int index = 0; index < travelRouteList.Count; ++index)
    {
      TravelRoute travelRoute1 = travelRouteList[index];
      if (travelRoute1.source != null)
      {
        using (List<Country>.Enumerator enumerator = countryList1.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Country current = enumerator.Current;
            if (current != travelRoute1.source && (!dictionary2.ContainsKey(travelRoute1.source) || !dictionary2[travelRoute1.source].ContainsKey(current)) && (!dictionary2.ContainsKey(current) || !dictionary2[current].ContainsKey(travelRoute1.source)))
            {
              TravelRoute travelRoute2 = new TravelRoute(travelRoute1.frequency)
              {
                routeType = ERouteType.Air,
                source = travelRoute1.source,
                destination = current
              };
              travelRoute2.source.airRoutes.Add(travelRoute2);
            }
          }
          continue;
        }
      }
      foreach (Country country1 in countryList1)
      {
        foreach (Country country2 in countryList1)
        {
          TravelRoute travelRoute3 = new TravelRoute(travelRoute1.frequency)
          {
            routeType = ERouteType.Air,
            source = country1,
            destination = country2
          };
          travelRoute3.source.airRoutes.Add(travelRoute3);
        }
      }
    }
    List<string> lines2 = DataImporter.GetLines(routeTxt, "ocean_routes");
    for (int index = 0; index < lines2.Count; ++index)
    {
      string str = lines2[index];
      string[] strArray3 = str.Split(':');
      if (strArray3.Length > 1)
      {
        string[] strArray4 = strArray3[0].Split(',');
        float freq;
        try
        {
          freq = float.Parse(strArray3[1]);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Error parsing sea route spawn rate: '" + str + "' - " + (object) ex));
          continue;
        }
        TravelRoute travelRoute = new TravelRoute(freq);
        travelRoute.routeType = ERouteType.Sea;
        if (strArray4[0] != "-")
        {
          if (strArray4[1] != "-")
          {
            try
            {
              travelRoute.source = dictionary1[strArray4[0]];
              travelRoute.destination = dictionary1[strArray4[1]];
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ("Error finding source/destination countries: '" + str + "' - " + (object) ex));
              continue;
            }
            travelRoute.source.hasPorts = true;
            travelRoute.destination.hasPorts = true;
            travelRoute.source.seaRoutes.Add(travelRoute);
          }
        }
      }
    }
    List<string> lines3 = DataImporter.GetLines(routeTxt, "ape_migration_ocean_routes");
    for (int index = 0; index < lines3.Count; ++index)
    {
      string str = lines2[index];
      string[] strArray5 = str.Split(':');
      if (strArray5.Length > 1)
      {
        string[] strArray6 = strArray5[0].Split(',');
        float freq;
        try
        {
          freq = float.Parse(strArray5[1]);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Error parsing sea route spawn rate: '" + str + "' - " + (object) ex));
          continue;
        }
        TravelRoute travelRoute = new TravelRoute(freq);
        travelRoute.routeType = ERouteType.Ape;
        if (strArray6[0] != "-")
        {
          if (strArray6[1] != "-")
          {
            try
            {
              travelRoute.source = dictionary1[strArray6[0]];
              travelRoute.destination = dictionary1[strArray6[1]];
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ("Error finding source/destination countries: '" + str + "' - " + (object) ex));
              continue;
            }
            travelRoute.source.apeMigrationRoutes.Add(travelRoute);
            travelRoute.destination.apeMigrationRoutes.Add(travelRoute);
          }
        }
      }
    }
    string singleValue = DataImporter.GetSingleValue(routeTxt, "land_routes_mult");
    if (!string.IsNullOrEmpty(singleValue))
      w.landRoutesMult = int.Parse(singleValue);
    if (string.IsNullOrEmpty(countryOrderTxt))
    {
      Debug.LogError((object) "countryorder.txt not found, therefore not sorting countries");
    }
    else
    {
      List<Country> countryList2 = new List<Country>();
      bool flag = false;
      using (StringReader stringReader = new StringReader(countryOrderTxt))
      {
        while (stringReader.Peek() >= 0)
        {
          string countryID = stringReader.ReadLine();
          int index = w.countries.FindIndex((Predicate<Country>) (o => o.id.Equals(countryID)));
          if (index == -1)
          {
            flag = true;
            break;
          }
          countryList2.Add(w.countries[index]);
          w.countries[index].countryNumber = countryList2.Count - 1;
        }
      }
      if (!flag)
        w.countries = countryList2;
      else
        Debug.LogWarning((object) "Failed ordering countries using default");
    }
  }

  public static List<Gene> ImportGenes(string geneText)
  {
    Dictionary<string, Gene> hash = new Dictionary<string, Gene>();
    DataImporter.UnserializeDataHash<Gene>((IDictionary<string, Gene>) hash, geneText, "genes");
    List<Gene> geneList = new List<Gene>();
    foreach (KeyValuePair<string, Gene> keyValuePair in (IEnumerable<KeyValuePair<string, Gene>>) hash)
    {
      keyValuePair.Value.id = keyValuePair.Key;
      geneList.Add(keyValuePair.Value);
    }
    return geneList;
  }

  public static Dictionary<EAbilityType, ActiveAbility> ImportAbilities(
    string abilityText,
    Scenario scen)
  {
    Dictionary<string, ActiveAbility> hash = new Dictionary<string, ActiveAbility>();
    DataImporter.UnserializeDataHash<ActiveAbility>((IDictionary<string, ActiveAbility>) hash, abilityText, "AA");
    Dictionary<EAbilityType, ActiveAbility> dictionary = new Dictionary<EAbilityType, ActiveAbility>();
    foreach (KeyValuePair<string, ActiveAbility> keyValuePair in hash)
    {
      try
      {
        EAbilityType key = (EAbilityType) Enum.Parse(typeof (EAbilityType), keyValuePair.Key);
        if (!dictionary.ContainsKey(key))
          dictionary[key] = keyValuePair.Value;
      }
      catch
      {
      }
    }
    return dictionary;
  }

  public static List<PetriDishSymptom> ImportPetriDishSymptoms(string abilityText, Scenario scen)
  {
    if (scen == null || string.IsNullOrEmpty(scen.scenarioInformation.scenSymptoms))
      return new List<PetriDishSymptom>();
    Dictionary<string, PetriDishSymptom> dictionary = new Dictionary<string, PetriDishSymptom>();
    DataImporter.UnserializeDataHash<PetriDishSymptom>((IDictionary<string, PetriDishSymptom>) dictionary, abilityText, "cure_symptoms");
    foreach (KeyValuePair<string, PetriDishSymptom> keyValuePair in dictionary)
      keyValuePair.Value.id = keyValuePair.Key;
    return scen.GetPetriDishSymptoms((IDictionary<string, PetriDishSymptom>) dictionary);
  }

  public static Dictionary<string, GemAbility> ImportGemAbilities(string gemText)
  {
    Dictionary<string, GemAbility> hash = new Dictionary<string, GemAbility>();
    DataImporter.UnserializeDataHash<GemAbility>((IDictionary<string, GemAbility>) hash, gemText, "gems");
    return hash;
  }

  public static void UnserializeDataHash<T>(
    IDictionary<string, T> hash,
    string txt,
    string sectionName,
    bool quiet = false)
    where T : new()
  {
    txt = txt.Replace("\r\n", "\n");
    txt = txt.Replace("\r", "\n");
    string[] strArray1 = txt.Split('\n');
    bool flag = false;
    int num = 0;
    System.Type type1 = (System.Type) null;
    T object_ = default (T);
    for (int index1 = 0; index1 < strArray1.Length; ++index1)
    {
      string str1 = strArray1[index1];
      if (flag)
      {
        if (!string.IsNullOrEmpty(str1) && str1[0] != '#')
        {
          for (int index2 = 0; index2 < num && index2 < str1.Length; ++index2)
          {
            if (str1[index2] != ' ')
            {
              num = index2;
              break;
            }
          }
          switch (num)
          {
            case 0:
              return;
            case 1:
              object_ = new T();
              type1 = object_.GetType();
              string str2 = str1.Trim();
              if (str2.Length > 0)
              {
                string key = str2.Split(':')[0];
                hash[key] = object_;
                ++num;
                continue;
              }
              continue;
            default:
              string[] strArray2 = str1.Trim().Split(':');
              if (strArray2.Length < 2)
              {
                Debug.LogError((object) ("Invalid data line '" + str1 + "' - expected param:val line:" + (object) (index1 + 1) + " depth:" + (object) num));
                continue;
              }
              if (strArray2.Length > 2)
              {
                for (int index3 = 2; index3 < strArray2.Length; ++index3)
                {
                  ref string local = ref strArray2[1];
                  local = local + ":" + strArray2[index3];
                }
              }
              string name = DataImporter.ConvertParameterName(strArray2[0]);
              FieldInfo field = type1.GetField(name);
              PropertyInfo property = (PropertyInfo) null;
              System.Type type2;
              if (field == (FieldInfo) null)
              {
                property = type1.GetProperty(name);
                if (property == (PropertyInfo) null)
                {
                  if (!quiet)
                  {
                    Debug.LogError((object) ("Could not find field (" + (object) field + ") or property (" + (object) property + ") for: '" + name + "' - original - '" + strArray2[0] + "' on type: " + (object) type1));
                    continue;
                  }
                  continue;
                }
                type2 = property.PropertyType;
              }
              else
                type2 = field.FieldType;
              if (type2 == typeof (bool))
              {
                DataImporter.SetValue((object) object_, field, property, (object) DataImporter.ParseBool(strArray2[1].ToLower()));
                continue;
              }
              if (type2.IsEnum)
              {
                try
                {
                  DataImporter.SetValue((object) object_, field, property, Enum.Parse(type2, strArray2[1]));
                  continue;
                }
                catch (FormatException ex)
                {
                  Debug.LogError((object) ("Unable to convert '" + strArray2[1] + "' to " + (object) type2 + " for " + name + "\n" + (object) ex));
                  continue;
                }
              }
              else
              {
                object obj;
                try
                {
                  obj = !(type2 != typeof (string)) || !(strArray2[1] == "") ? Convert.ChangeType((object) strArray2[1], type2) : Convert.ChangeType((object) 0, type2);
                }
                catch (FormatException ex)
                {
                  obj = (object) (DataImporter.ParseBool(strArray2[1].ToLower()) ? 1 : 0);
                }
                DataImporter.SetValue((object) object_, field, property, obj);
                continue;
              }
          }
        }
      }
      else if (str1.StartsWith(sectionName + ":"))
      {
        flag = true;
        ++num;
      }
    }
  }

  public static void SetValue(
    object object_,
    FieldInfo field,
    PropertyInfo property,
    object value)
  {
    if (property != (PropertyInfo) null)
      property.SetValue(object_, value, (object[]) null);
    else
      field.SetValue(object_, value);
  }

  public static bool ParseBool(string data)
  {
    if (data == "1" || data.ToLower() == "true")
      return true;
    if (data == "0" || data.ToLower() == "false" || string.IsNullOrEmpty(data))
      return false;
    Debug.LogWarning((object) string.Format("Unrecognised boolean value '{0}'.", (object) data));
    return false;
  }

  public static void UnserializeData<T>(T ob, string txt, string sectionName, bool quiet = false)
  {
    txt = txt.Replace("\r\n", "\n");
    txt = txt.Replace("\r", "\n");
    string[] strArray1 = txt.Split('\n');
    bool flag = false;
    int num = 0;
    System.Type type1 = ob.GetType();
    foreach (string str1 in strArray1)
    {
      string str2 = str1.Replace("\r", "");
      if (flag)
      {
        if (!string.IsNullOrEmpty(str2))
        {
          string str3 = str2.Trim();
          if (!string.IsNullOrEmpty(str3) && str3[0] != '#')
          {
            for (int index = 0; index < num; ++index)
            {
              if (str2[index] != ' ')
              {
                num = index;
                break;
              }
            }
            if (num == 0)
              break;
            string[] strArray2 = str3.Split(":".ToCharArray(), 2);
            if (strArray2.Length < 2)
            {
              Debug.LogError((object) ("Invalid data line '" + str2 + "' - expected param:val"));
            }
            else
            {
              string name = DataImporter.ConvertParameterName(strArray2[0]);
              FieldInfo field = type1.GetField(name);
              PropertyInfo propertyInfo = (PropertyInfo) null;
              System.Type type2;
              if (field == (FieldInfo) null)
              {
                propertyInfo = type1.GetProperty(name);
                if (propertyInfo == (PropertyInfo) null)
                {
                  if (quiet)
                    continue;
                  continue;
                }
                type2 = propertyInfo.PropertyType;
              }
              else
                type2 = field.FieldType;
              string str4 = strArray2[1].Split('#')[0].Trim();
              if (type2 == typeof (bool))
                str4 = str4 == "1" || str4 == "True" || str4 == "true" ? "true" : "false";
              if ((type2 == typeof (long) || type2 == typeof (int)) && str4.Contains("."))
              {
                Debug.LogWarning((object) (sectionName + "." + name + " has float value but is long/int type - line\n" + str2));
                str4 = str4.Split('.')[0];
              }
              if (type2.IsEnum)
              {
                try
                {
                  if (propertyInfo != (PropertyInfo) null)
                    propertyInfo.SetValue((object) ob, Enum.Parse(type2, str4), (object[]) null);
                  else
                    field.SetValue((object) ob, Enum.Parse(type2, str4));
                }
                catch (Exception ex)
                {
                  Debug.LogError((object) ("Error setting '" + name + "' - line: '" + str2 + "' error:\n" + (object) ex));
                }
              }
              else if (!(type2 == typeof (Vector2)))
              {
                try
                {
                  if (propertyInfo != (PropertyInfo) null)
                  {
                    propertyInfo.SetValue((object) ob, Convert.ChangeType((object) str4, type2), (object[]) null);
                  }
                  else
                  {
                    if (type2 == typeof (string))
                    {
                      str4 = str4.Replace("*ret*", "\n");
                      str4 = str4.Replace("\\n", "\n");
                    }
                    else if ((type2 == typeof (long) || type2 == typeof (int) || type2 == typeof (float) || type2 == typeof (double)) && str4 == "")
                      str4 = "0";
                    field.SetValue((object) ob, Convert.ChangeType((object) str4, type2));
                  }
                }
                catch (Exception ex)
                {
                  Debug.LogError((object) ("Error setting '" + name + "' - line: '" + str2 + "' val: '" + str4 + "' error:\n" + (object) ex));
                }
              }
              else
              {
                Vector2 vector2 = new Vector2();
                string[] strArray3 = str4.Split(',');
                vector2.x = float.Parse(strArray3[0]);
                vector2.y = float.Parse(strArray3[1]);
                field.SetValue((object) ob, (object) vector2);
              }
            }
          }
        }
      }
      else if (str2 == sectionName + ":")
      {
        flag = true;
        ++num;
      }
    }
  }

  public static string ConvertParameterName(string param)
  {
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = false;
    for (int index = 0; index < param.Length; ++index)
    {
      if (param[index] != ' ')
      {
        if (param[index] == '_' && index > 0)
          flag = true;
        else if (flag)
        {
          stringBuilder.Append(char.ToUpper(param[index]));
          flag = false;
        }
        else
          stringBuilder.Append(param[index]);
      }
    }
    return stringBuilder.ToString();
  }

  private static List<string> GetSections(string txt)
  {
    string[] strArray = txt.Split('\n');
    List<string> sections = new List<string>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      string str = strArray[index];
      if (!string.IsNullOrEmpty(str) && str[0] != '#' && str[0] != ' ')
        sections.Add(str.Split(':')[0].Trim());
    }
    return sections;
  }

  private static string GetSingleValue(string txt, string id)
  {
    string str1 = txt;
    char[] chArray = new char[1]{ '\n' };
    foreach (string str2 in str1.Split(chArray))
    {
      if (str2.StartsWith(id + ":"))
        return str2.Substring(id.Length + 1);
    }
    return (string) null;
  }

  private static List<string> GetLines(string txt, string section)
  {
    string[] strArray = txt.Split('\n');
    bool flag = false;
    int num = 0;
    List<string> lines = new List<string>();
    for (int index1 = 0; index1 < strArray.Length; ++index1)
    {
      string str1 = strArray[index1];
      if (flag)
      {
        if (!string.IsNullOrEmpty(str1))
        {
          string str2 = str1.Trim();
          if (!string.IsNullOrEmpty(str2) && str2[0] != '#')
          {
            for (int index2 = 0; index2 < num; ++index2)
            {
              if (str1[index2] != ' ')
              {
                num = index2;
                break;
              }
            }
            if (num != 0)
              lines.Add(str2);
            else
              break;
          }
        }
      }
      else if (str1 == section + ":")
      {
        flag = true;
        ++num;
      }
    }
    return lines;
  }

  private static bool JudgeTechnologyOrRequirementValid(Disease disease, Technology technology)
  {
    for (int index = 0; index < technology.requiredTechOR.Count; ++index)
    {
      if (disease.GetTechnology(technology.requiredTechOR[index]) != null)
        return true;
    }
    return false;
  }
}
