// Decompiled with JetBrains decompiler
// Type: CSavesSteam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public class CSavesSteam : ISaves
{
  private static IDictionary<int, Texture2D> storedTextures = (IDictionary<int, Texture2D>) new Dictionary<int, Texture2D>();
  private List<ISaves.SaveMetaData> saveCache;

  public override bool IsAvailable() => true;

  public override void DeleteGame(int slotID)
  {
    List<ISaves.SaveMetaData> savedGames = this.GetSavedGames();
    savedGames.RemoveAll((Predicate<ISaves.SaveMetaData>) (a => a.saveSlot == slotID));
    CSavesSteam.DeleteCloudData("save_" + (object) slotID + ".dat");
    CSavesSteam.DeleteCloudData("save_" + (object) slotID + ".png");
    this.DeleteSaveTextures(slotID);
    if (savedGames.Count > 0)
      this.SaveMetaDataFile(savedGames);
    else
      CSavesSteam.DeleteCloudData("meta.dat");
  }

  private void DeleteSaveTextures(int slotID)
  {
    for (int index = 0; index < 5; ++index)
    {
      foreach (string key in (IEnumerable<string>) CInterfaceManager.instance.countryMap.Keys)
        CSavesSteam.DeleteCloudData("save_" + (object) slotID + "_textures/" + key + "_" + (object) index + ".png", true);
      CSavesSteam.DeleteCloudData("save_" + (object) slotID + "_textures/_trails_" + (object) index + ".png", true);
    }
    for (int index = 0; index < 5; ++index)
    {
      foreach (string key in (IEnumerable<string>) CInterfaceManager.instance.countryMap.Keys)
        CSavesSteam.DeleteLocalData("save_" + (object) slotID + "_textures/" + key + "_" + (object) index + ".png", true);
      CSavesSteam.DeleteLocalData("save_" + (object) slotID + "_textures/_trails_" + (object) index + ".png", true);
    }
    foreach (string key in (IEnumerable<string>) CInterfaceManager.instance.countryMap.Keys)
      CSavesSteam.DeleteLocalData("save_" + (object) slotID + "_textures/" + key + ".dat", true);
  }

  public override void SaveGame(
    string str,
    Texture2D tex,
    ISaves.SaveMetaData data,
    string replayData)
  {
    this.saveCache = (List<ISaves.SaveMetaData>) null;
    List<ISaves.SaveMetaData> savedGames = this.GetSavedGames();
    if (data.saveSlot == 0)
    {
      int num = 1;
      foreach (ISaves.SaveMetaData saveMetaData in savedGames)
      {
        if (saveMetaData.saveSlot >= num)
          num = saveMetaData.saveSlot + 1;
      }
      data.saveSlot = num;
    }
    else
    {
      ISaves.SaveMetaData saveMetaData = savedGames.Find((Predicate<ISaves.SaveMetaData>) (a => a.saveSlot == data.saveSlot));
      if (saveMetaData != null)
        savedGames.Remove(saveMetaData);
    }
    data.date = DateTime.Now;
    savedGames.Add(data);
    this.SaveMetaDataFile(savedGames);
    CSavesSteam.SaveCloudData("save_" + (object) data.saveSlot + ".dat", str);
    this.DeleteSaveTextures(data.saveSlot);
    this.SaveInfectionTextures(data.saveSlot);
    CSavesSteam.SaveLocalData("replay_" + (object) data.saveSlot + ".dat", replayData);
    CSavesSteam.SaveCloudData("save_" + (object) data.saveSlot + ".png", tex.EncodeToPNG());
    this.saveCache = (List<ISaves.SaveMetaData>) null;
  }

  private void SaveInfectionTextures(int slot)
  {
    Directory.CreateDirectory(Path.Combine(CSavesSteam.GetLocalDataPath(), "save_" + (object) slot + "_textures"));
    for (int index1 = 0; index1 < World.instance.countries.Count; ++index1)
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView(World.instance.countries[index1].id);
      CSavesSteam.SaveLocalData("save_" + (object) slot + "_textures/" + countryView.countryID + ".dat", countryView.Serialize(false));
      InfectionParticleSystem infectionSystem = countryView.GetInfectionSystem(false);
      if ((UnityEngine.Object) infectionSystem != (UnityEngine.Object) null)
      {
        byte[][] numArray = infectionSystem.SerializeInfectionTexture();
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          CSavesSteam.SaveLocalData("save_" + (object) slot + "_textures/" + countryView.countryID + "_" + (object) index2 + ".png", numArray[index2]);
        byte[] data = infectionSystem.SerializeDeadTexture();
        if (data != null)
          CSavesSteam.SaveLocalData("save_" + (object) slot + "_textures/" + countryView.countryID + "_dead.png", data);
      }
    }
    byte[][] numArray1 = DiseaseTrailParticles.instance.SerializeInfectionTexture();
    for (int index = 0; index < numArray1.Length; ++index)
      CSavesSteam.SaveLocalData("save_" + (object) slot + "_textures/_trails_" + (object) index + ".png", numArray1[index]);
  }

  public override List<ISaves.SaveMetaData> GetSavedGames()
  {
    if (this.saveCache == null)
    {
      this.saveCache = new List<ISaves.SaveMetaData>();
      string str1 = CSavesSteam.LoadCloudData("meta.dat");
      if (!string.IsNullOrEmpty(str1))
      {
        string str2 = str1;
        char[] chArray = new char[1]{ '\n' };
        foreach (string str3 in str2.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str3))
          {
            try
            {
              this.saveCache.Add(new ISaves.SaveMetaData(str3));
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ("Error loading save game meta data: '" + str3 + "'\n" + (object) ex));
            }
          }
        }
      }
      this.saveCache.Sort((Comparison<ISaves.SaveMetaData>) ((x, y) => x.date.CompareTo(y.date)));
    }
    return this.saveCache;
  }

  private void SaveMetaDataFile(List<ISaves.SaveMetaData> metaData)
  {
    string data = "";
    metaData.Sort((Comparison<ISaves.SaveMetaData>) ((x, y) => x.saveSlot.CompareTo(y.saveSlot)));
    foreach (ISaves.SaveMetaData saveMetaData in metaData)
      data = data + saveMetaData.ToString() + "\n";
    CSavesSteam.SaveCloudData("meta.dat", data);
  }

  public override string LoadGame(int slotID)
  {
    return CSavesSteam.LoadCloudData("save_" + (object) slotID + ".dat");
  }

  public override string LoadReplayData(int slotID)
  {
    return CSavesSteam.LoadLocalData("replay_" + (object) slotID + ".dat", true);
  }

  public override void LoadInfectionTextures(int slotID)
  {
    for (int index1 = 0; index1 < World.instance.countries.Count; ++index1)
    {
      CountryView countryView = CInterfaceManager.instance.GetCountryView(World.instance.countries[index1].id);
      string str = CSavesSteam.LoadLocalData("save_" + (object) slotID + "_textures/" + countryView.countryID + ".dat", true);
      if (!string.IsNullOrEmpty(str))
        countryView.Unserialize(str);
      bool flag = false;
      byte[][] textures = new byte[World.instance.diseases.Count][];
      for (int index2 = 0; index2 < World.instance.diseases.Count; ++index2)
      {
        byte[] numArray = CSavesSteam.LoadLocalBinaryData("save_" + (object) slotID + "_textures/" + countryView.countryID + "_" + (object) index2 + ".png", true);
        if (numArray == null)
          numArray = CSavesSteam.LoadCloudBinaryData("save_" + (object) slotID + "_textures/" + countryView.countryID + "_" + (object) index2 + ".png", true);
        if (numArray != null)
        {
          textures[index2] = numArray;
          flag = true;
        }
      }
      if (flag)
      {
        byte[] deadTextureData = CSavesSteam.LoadLocalBinaryData("save_" + (object) slotID + "_textures/" + countryView.countryID + "_dead.png", true);
        countryView.GetInfectionSystem().UnserializeInfectionTexture(textures, deadTextureData);
      }
    }
    byte[][] textures1 = new byte[World.instance.diseases.Count][];
    for (int index = 0; index < World.instance.diseases.Count; ++index)
    {
      byte[] numArray = CSavesSteam.LoadLocalBinaryData("save_" + (object) slotID + "_textures/_trails_" + (object) index + ".png", true);
      if (numArray == null)
        numArray = CSavesSteam.LoadCloudBinaryData("save_" + (object) slotID + "_textures/_trails_" + (object) index + ".png", true);
      if (numArray != null)
        textures1[index] = numArray;
    }
    DiseaseTrailParticles.instance.UnserializeInfectionTexture(textures1);
  }

  public override ISaves.SaveMetaData GetLastSave()
  {
    List<ISaves.SaveMetaData> savedGames = this.GetSavedGames();
    return savedGames.Count > 0 ? savedGames[savedGames.Count - 1] : (ISaves.SaveMetaData) null;
  }

  public override ISaves.SaveMetaData GetMetaData(int slotID)
  {
    return this.GetSavedGames().Find((Predicate<ISaves.SaveMetaData>) (a => a.saveSlot == slotID));
  }

  public override Texture2D GetTexture(int slotID)
  {
    byte[] data = CSavesSteam.LoadCloudBinaryData("save_" + (object) slotID + ".png");
    if (CSavesSteam.storedTextures.ContainsKey(slotID))
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) CSavesSteam.storedTextures[slotID]);
    Texture2D tex = (Texture2D) null;
    if (data != null)
    {
      tex = new Texture2D(1, 1);
      tex.LoadImage(data);
      CSavesSteam.storedTextures[slotID] = tex;
    }
    return tex;
  }

  public static string GetLocalDataPath()
  {
    char directorySeparatorChar = Path.DirectorySeparatorChar;
    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + directorySeparatorChar.ToString() + "Ndemic Creations" + directorySeparatorChar.ToString() + "Plague Inc. Evolved" + directorySeparatorChar.ToString() + "Saves" + directorySeparatorChar.ToString();
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    return path;
  }

  public static string LoadLocalData(string filename, bool quiet = false)
  {
    byte[] bytes = CSavesSteam.LoadLocalBinaryData(filename, quiet);
    return bytes != null ? Encoding.UTF8.GetString(bytes) : (string) null;
  }

  public static byte[] LoadLocalBinaryData(string filename, bool quiet = false)
  {
    string path = Path.Combine(CSavesSteam.GetLocalDataPath(), filename);
    try
    {
      return File.ReadAllBytes(path);
    }
    catch (Exception ex)
    {
      if (!quiet)
        Debug.LogError((object) ("Error reading local file: '" + filename + "'\n" + (object) ex));
    }
    return (byte[]) null;
  }

  public static void SaveLocalData(string filename, string data)
  {
    CSavesSteam.SaveLocalData(filename, Encoding.UTF8.GetBytes(data));
  }

  public static void SaveLocalData(string filename, byte[] data)
  {
    string path = Path.Combine(CSavesSteam.GetLocalDataPath(), filename);
    try
    {
      File.WriteAllBytes(path, data);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error writing local file '" + filename + "'\n" + (object) ex));
    }
  }

  public static string LoadCloudData(string filename)
  {
    byte[] bytes = CSavesSteam.LoadCloudBinaryData(filename);
    if (bytes != null)
      return Encoding.UTF8.GetString(bytes);
    Debug.LogWarning((object) ("CLOUD: Could not load cloud data - " + filename));
    return (string) null;
  }

  public static void DeleteLocalData(string filename, bool quiet = false)
  {
    string path = Path.Combine(CSavesSteam.GetLocalDataPath(), filename);
    try
    {
      File.Delete(path);
    }
    catch (Exception ex)
    {
      if (quiet)
        return;
      Debug.LogError((object) ("Error writing local file '" + filename + "'\n" + (object) ex));
    }
  }

  public static void SaveCloudData(string filename, string data)
  {
    CSavesSteam.SaveCloudData(filename, Encoding.UTF8.GetBytes(data));
  }

  public static void SaveCloudData(string filename, byte[] data)
  {
    try
    {
      if (SteamRemoteStorage.FileWrite(filename, data, data.Length))
        return;
      Debug.LogError((object) ("Unable to write cloud file '" + filename + "'"));
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error writing cloud file '" + filename + "'\n" + (object) ex));
    }
  }

  public static bool ExistsCloudData(string filename)
  {
    return !string.IsNullOrEmpty(filename) && SteamRemoteStorage.FileExists(filename);
  }

  public static byte[] LoadCloudBinaryData(string filename, bool quiet = false)
  {
    if (!SteamManager.Initialized)
      return (byte[]) null;
    int fileSize = SteamRemoteStorage.GetFileSize(filename);
    if (fileSize > 0)
    {
      byte[] pvData = new byte[fileSize];
      try
      {
        if (SteamRemoteStorage.FileRead(filename, pvData, fileSize) > 0)
          return pvData;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error reading cloud file: '" + filename + "'\n" + (object) ex));
      }
    }
    else if (!quiet)
      Debug.LogWarning((object) string.Format("CLOUD: Couldn't read \"{0}\", file does not exist", (object) filename));
    return (byte[]) null;
  }

  public static void DeleteCloudData(string filename, bool quiet = false)
  {
    if (!SteamManager.Initialized || SteamRemoteStorage.FileDelete(filename) || quiet)
      return;
    Debug.LogError((object) ("Error deleting: '" + filename + "'"));
  }

  public void DeleteAllGames()
  {
    List<ISaves.SaveMetaData> savedGames = this.GetSavedGames();
    List<int> source = new List<int>();
    if (savedGames == null || savedGames.Count < 1)
      return;
    foreach (ISaves.SaveMetaData saveMetaData in savedGames)
      source.Add(saveMetaData.saveSlot);
    savedGames.Clear();
    CSavesSteam.DeleteCloudData("meta.dat");
    Parallel.ForEach<int>((IEnumerable<int>) source, (Action<int>) (slotID =>
    {
      string filename1 = "save_" + (object) slotID + ".dat";
      string filename2 = "save_" + (object) slotID + ".png";
      CSavesSteam.DeleteCloudData(filename1);
      CSavesSteam.DeleteCloudData(filename2);
      this.DeleteSaveTextures(slotID);
    }));
  }
}
