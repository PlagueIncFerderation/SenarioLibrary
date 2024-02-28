// Decompiled with JetBrains decompiler
// Type: WorldMapController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldMapController : MonoBehaviour
{
  public static WorldMapController instance;
  private List<CountryView> countries;

  private void Awake()
  {
    WorldMapController.instance = this;
    this.countries = new List<CountryView>();
    for (int index = 0; index < this.transform.childCount; ++index)
      this.countries.Add(this.transform.GetChild(index).GetComponent<CountryView>());
  }

  public void SetSelectedCountry(string countryID)
  {
    this.SetSelectedCountry(this.countries.Find((Predicate<CountryView>) (c => c.name == countryID)));
  }

  public void SetSelectedCountry(CountryView cv = null)
  {
    this.SetSelectedCountry(cv, CountryView.EOverlayState.SELECTED);
  }

  public void SetSelectedCountry(CountryView cv, CountryView.EOverlayState overlayState)
  {
    if (CGameManager.spaceTime)
    {
      CInterfaceManager.instance.SelectedCountryView = (CountryView) null;
    }
    else
    {
      switch (overlayState)
      {
        case CountryView.EOverlayState.P2_INTENT:
          CInterfaceManager.instance.P2IntentSelectedCountryView = cv;
          break;
        case CountryView.EOverlayState.P2:
          CInterfaceManager.instance.P2SelectedCountryView = cv;
          break;
        default:
          CInterfaceManager.instance.SelectedCountryView = cv;
          break;
      }
      if (CGameManager.IsTutorialGame && CGameManager.gameType == IGame.GameType.CureTutorial && TutorialSystem.IsModuleActive("C11"))
      {
        for (int index = 0; index < this.countries.Count; ++index)
        {
          CountryView country = this.countries[index];
          if (country.name == "brazil")
            CInterfaceManager.instance.SetCountryHighlight(country, CountryView.EOverlayState.SELECTED);
        }
      }
      else
      {
        for (int index = 0; index < this.countries.Count; ++index)
        {
          CountryView country = this.countries[index];
          if ((UnityEngine.Object) country == (UnityEngine.Object) CInterfaceManager.instance.P2SelectedCountryView)
            CInterfaceManager.instance.SetCountryHighlight(country, CountryView.EOverlayState.P2);
          else if ((UnityEngine.Object) country == (UnityEngine.Object) CInterfaceManager.instance.P2IntentSelectedCountryView)
            CInterfaceManager.instance.SetCountryHighlight(country, CountryView.EOverlayState.P2_INTENT);
          else if ((UnityEngine.Object) country == (UnityEngine.Object) CInterfaceManager.instance.SelectedCountryView)
            CInterfaceManager.instance.SetCountryHighlight(country, CountryView.EOverlayState.SELECTED);
          else
            CInterfaceManager.instance.SetCountryHighlight(country, CountryView.EOverlayState.OFF);
        }
      }
    }
  }
}
