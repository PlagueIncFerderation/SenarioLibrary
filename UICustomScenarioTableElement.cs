// Decompiled with JetBrains decompiler
// Type: UICustomScenarioTableElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UICustomScenarioTableElement : MonoBehaviour
{
  [Header("UI elements")]
  public UIButton button;
  public UILabel scenarioNameLabel;
  public UISprite subscribedSprite;
  public UISprite featuredContentSprite;
  public GameObject emptyRatingObject;
  public GameObject ratingObject;
  public UIToggle toggle;
  public UITexture thumbnailTexture;
  public ulong publishedFileID;
  private string filename;
  public bool isMine;
  public bool isSubscribed;
  public ScenarioInformation scenarioInformation;
  public CustomScenarioMetadata metadata;
  private static List<UICustomScenarioTableElement> objectPool = new List<UICustomScenarioTableElement>();

  public Texture ThumbnailTexture
  {
    get => this.thumbnailTexture.mainTexture;
    set => this.thumbnailTexture.mainTexture = value;
  }

  public UITexture ThumbnailUITexture
  {
    get => this.thumbnailTexture;
    set => this.thumbnailTexture = value;
  }

  public void ShowThumbnail(Texture2D texture)
  {
    this.thumbnailTexture.material.mainTexture = (Texture) texture;
    this.StartCoroutine(this.FadeThumbNail());
  }

  public IEnumerator FadeThumbNail()
  {
    while ((double) this.ThumbnailUITexture.alpha < 0.40000000596046448)
    {
      this.ThumbnailUITexture.alpha += 0.01f;
      yield return (object) null;
    }
  }

  public void PopulateWithData(
    ulong publishedFileID,
    string title,
    int rating,
    bool isSubscribed,
    bool isFeatured,
    bool isLocal,
    string fname = "",
    ScenarioInformation scenarioInformation = null)
  {
    this.metadata = (CustomScenarioMetadata) null;
    this.scenarioInformation = scenarioInformation;
    this.publishedFileID = publishedFileID;
    this.thumbnailTexture.mainTexture = (Texture) null;
    this.thumbnailTexture.alpha = 0.0f;
    this.StopAllCoroutines();
    this.toggle.Set(false);
    this.scenarioNameLabel.text = title;
    this.scenarioNameLabel.width = 4000;
    this.subscribedSprite.gameObject.SetActive(isSubscribed);
    this.SetSubscribed(isSubscribed);
    this.featuredContentSprite.gameObject.SetActive(isFeatured);
    this.SetRating(Mathf.CeilToInt((float) (rating * 5)));
    this.filename = fname;
    if (isLocal)
    {
      for (int index = 0; index < this.ratingObject.transform.childCount; ++index)
        this.ratingObject.transform.GetChild(index).gameObject.SetActive(false);
      for (int index = 0; index < this.emptyRatingObject.transform.childCount; ++index)
        this.emptyRatingObject.transform.GetChild(index).gameObject.SetActive(false);
      this.featuredContentSprite.gameObject.SetActive(false);
      this.subscribedSprite.gameObject.SetActive(false);
    }
    else
    {
      for (int index = 0; index < this.emptyRatingObject.transform.childCount; ++index)
        this.emptyRatingObject.transform.GetChild(index).gameObject.SetActive(true);
      this.SetRating(rating);
    }
  }

  public void PopulateWithData(CustomScenarioMetadata metadata)
  {
    this.metadata = metadata;
    this.thumbnailTexture.alpha = 0.0f;
    this.StopAllCoroutines();
    this.toggle.Set(false);
    this.scenarioNameLabel.text = metadata.Title;
    this.scenarioNameLabel.width = 4000;
    this.featuredContentSprite.gameObject.SetActive(metadata.IsFeatured);
    int rating = -1;
    if (metadata.VoteUps + metadata.VoteDowns > 25)
      rating = Mathf.CeilToInt(metadata.Score * 5f);
    this.SetRating(rating);
  }

  public void SetRating(int rating)
  {
    for (int index = 0; index < this.ratingObject.transform.childCount; ++index)
      this.ratingObject.transform.GetChild(index).gameObject.SetActive(rating > index);
  }

  public bool IsLocal() => !string.IsNullOrEmpty(this.filename);

  public void SetSubscribed(bool state)
  {
    this.subscribedSprite.gameObject.SetActive(state);
    this.isSubscribed = state;
  }

  public string GetFilename() => this.filename;

  public static void PoolObject(UICustomScenarioTableElement ob)
  {
    ob.thumbnailTexture.mainTexture = (Texture) null;
    ob.thumbnailTexture.alpha = 0.0f;
    ob.StopAllCoroutines();
    ob.gameObject.SetActive(false);
    UICustomScenarioTableElement.objectPool.Add(ob);
  }

  public static UICustomScenarioTableElement CreateObject(UICustomScenarioTableElement prefab)
  {
    UICustomScenarioTableElement component;
    if (UICustomScenarioTableElement.objectPool.Count > 0)
    {
      component = UICustomScenarioTableElement.objectPool[0];
      UICustomScenarioTableElement.objectPool.RemoveAt(0);
      component.gameObject.SetActive(true);
    }
    else
      component = Object.Instantiate<GameObject>(prefab.gameObject).GetComponent<UICustomScenarioTableElement>();
    return component;
  }
}
