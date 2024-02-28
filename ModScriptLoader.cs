// Decompiled with JetBrains decompiler
// Type: ModScriptLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;

#nullable disable
public class ModScriptLoader
{
  public static void LoadAndExecuteScripts(
    string filePath,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    System.Type type = Assembly.LoadFrom(filePath).GetType("ExternalLoad.Functions");
    type.GetMethod("ExternalScript").Invoke(Activator.CreateInstance(type), new object[4]
    {
      (object) world,
      (object) disease,
      (object) country,
      (object) localDisease
    });
  }
}
