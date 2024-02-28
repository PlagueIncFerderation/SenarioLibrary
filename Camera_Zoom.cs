// Decompiled with JetBrains decompiler
// Type: Camera_Zoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital;
using AurochDigital.Tutorial;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Camera_Zoom : MonoBehaviour, ITutorial
{
  public static Camera_Zoom instance;
  public float CameraZoomIn = -5f;
  public float CameraZoomDefault = -15f;
  public float CameraZoomOut = -20f;
  public float CameraZoomSpeed = 0.5f;
  public float CameraZoomSpring = 7f;
  public float CameraSpringTime = 0.5f;
  public float CameraPanSpeed = 0.2f;
  public float CameraPanDelay = 0.2f;
  public float CameraTiltIn = -25f;
  public float CameraTiltOut;
  public float CameraKeySpeed = 20f;
  public float CameraKeyZoom = 30f;
  public Vector2 lowMin;
  public Vector2 lowMax;
  public Vector2 highMin;
  public Vector2 highMax;
  private Vector3 mOldMouse;
  public Vector3 mTargetPos;
  public Vector3 mCameraPos;
  private float mOffsetX;
  private float mOffsetY;
  private float mTargetFactor;
  private float mfScrollTimer;
  private bool mbIsScrolling;
  private bool mbCanMove;
  private float mfScrollSpringTime;
  public float currentZoomFactor;
  private List<UIScrollBar> CameraScrollBars = new List<UIScrollBar>();

  public bool IsScrolling => this.mbIsScrolling;

  public float ZoomFactor => this.mTargetFactor;

  private void Awake()
  {
    this.mbIsScrolling = false;
    this.mbCanMove = false;
    this.mfScrollTimer = 0.0f;
    this.SetCameraToDefault();
    if (!Application.isPlaying)
      return;
    Camera_Zoom.instance = this;
  }

  private void Start() => TutorialSystem.RegisterInterface((ITutorial) this);

  private void LateUpdate() => this.UpdateCameraPosition();

  private void UpdateCameraPosition()
  {
    float num1 = Mathf.Clamp(Time.deltaTime * this.CameraZoomSpring, 0.0f, 1f);
    bool flag1 = false;
    Quaternion rotation = this.transform.rotation;
    rotation.Set(rotation.x, rotation.y + 0.4f * Time.deltaTime * ModelUtils.FloatRand(-1f, 1f), rotation.z + 0.4f * Time.deltaTime * ModelUtils.FloatRand(-1f, 1f), rotation.w + 0.4f * Time.deltaTime * ModelUtils.FloatRand(-1f, 1f));
    rotation.Set(rotation.x, 0.4f, 0.0f, 0.0f);
    rotation.eulerAngles = new Vector3(0.0f, 0.0f, 180f);
    CGameOverlay overlay;
    if (!CUIManager.instance.GetOpenOverlay(out overlay) || ((object) overlay).GetType() != typeof (MultiplayerResultOverlay))
    {
      if (CActionManager.instance.ActionContinue("INPUT_IN"))
      {
        this.mTargetPos.z += this.CameraKeyZoom * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag1 = true;
      }
      if (CActionManager.instance.ActionContinue("INPUT_OUT"))
      {
        this.mTargetPos.z -= this.CameraKeyZoom * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag1 = true;
      }
    }
    float axis = Input.GetAxis("Mouse ScrollWheel");
    if (!CUIManager.instance.IsHovering() && (flag1 || (double) axis != 0.0))
    {
      if (!Input.GetKey(KeyCode.LeftControl))
      {
        this.mfScrollSpringTime = this.CameraSpringTime;
        this.SetZoom(this.mTargetPos.z + axis * this.CameraZoomSpeed * COptionsManager.instance.MouseSensivityConverted);
      }
    }
    else
    {
      this.mfScrollSpringTime -= Time.deltaTime;
      if ((double) this.mfScrollSpringTime < 0.0 && (double) this.mTargetPos.z < (double) this.CameraZoomDefault)
        this.mTargetPos.z = this.CameraZoomDefault;
    }
    this.mCameraPos.z += (this.mTargetPos.z - this.mCameraPos.z) * num1;
    this.currentZoomFactor = Mathf.Clamp((float) (((double) this.mCameraPos.z - (double) this.CameraZoomIn) / ((double) this.CameraZoomDefault - (double) this.CameraZoomIn)), 0.0f, 1f);
    bool flag2 = false;
    if (!CHUDScreen.instance.newsHistory.gameObject.activeInHierarchy || !CSteamControllerManager.instance.controllerActive)
    {
      if (CActionManager.instance.ActionContinue("INPUT_LEFT"))
      {
        this.mTargetPos.x -= this.CameraKeySpeed * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag2 = true;
      }
      if (CActionManager.instance.ActionContinue("INPUT_RIGHT"))
      {
        this.mTargetPos.x += this.CameraKeySpeed * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag2 = true;
      }
      if (CActionManager.instance.ActionContinue("INPUT_UP"))
      {
        this.mTargetPos.y += this.CameraKeySpeed * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag2 = true;
      }
      if (CActionManager.instance.ActionContinue("INPUT_DOWN"))
      {
        this.mTargetPos.y -= this.CameraKeySpeed * COptionsManager.instance.KeySensivityConverted * Time.deltaTime;
        flag2 = true;
      }
    }
    if (CGameManager.IsTutorialGame & flag2 && TutorialSystem.IsModuleActive("23A"))
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    if ((Input.GetMouseButtonDown(0) || CSteamControllerManager.instance.GetActionDown(ESteamControllerDigitalAction.main_select)) && !CUIManager.instance.IsHovering())
    {
      this.mOldMouse = Input.mousePosition;
      this.mOldMouse.z = this.transform.position.z;
      this.mbCanMove = true;
    }
    else if ((Input.GetMouseButton(0) || CSteamControllerManager.instance.GetAction(ESteamControllerDigitalAction.main_select)) && this.mbCanMove)
    {
      this.mfScrollSpringTime = this.CameraSpringTime;
      Vector3 worldPoint1 = this.GetComponent<Camera>().ScreenToWorldPoint(this.mOldMouse);
      this.mOldMouse = Input.mousePosition;
      this.mOldMouse.z = this.transform.position.z;
      Vector3 worldPoint2 = this.GetComponent<Camera>().ScreenToWorldPoint(this.mOldMouse);
      if ((double) (worldPoint2 - worldPoint1).sqrMagnitude > 0.0099999997764825821)
        this.mbIsScrolling = true;
      this.mTargetPos.x += worldPoint2.x - worldPoint1.x;
      this.mTargetPos.y += worldPoint2.y - worldPoint1.y;
    }
    else
    {
      float num2 = 0.0f;
      float num3 = 0.0f;
      bool flag3 = false;
      Vector3 mousePosition = Input.mousePosition;
      if ((double) mousePosition.x > (double) Screen.width * 0.99000000953674316)
      {
        flag3 = true;
        num2 = 1f;
      }
      else if ((double) mousePosition.x < (double) Screen.width * 0.0099999997764825821)
      {
        flag3 = true;
        num2 = -1f;
      }
      if ((double) mousePosition.y < (double) Screen.height * 0.0099999997764825821)
      {
        flag3 = true;
        num3 = -1f;
      }
      else if ((double) mousePosition.y > (double) Screen.height * 0.99000000953674316)
      {
        flag3 = true;
        num3 = 1f;
      }
      if (flag3)
      {
        this.mfScrollTimer += Time.deltaTime;
        if ((double) this.mfScrollTimer > (double) this.CameraPanDelay)
        {
          this.mTargetPos += new Vector3(this.CameraPanSpeed * num2 * COptionsManager.instance.MouseSensivityConverted, this.CameraPanSpeed * num3 * COptionsManager.instance.MouseSensivityConverted);
          this.mbIsScrolling = true;
          this.mbCanMove = true;
        }
      }
      else
      {
        this.mfScrollTimer = 0.0f;
        this.mbIsScrolling = false;
        this.mbCanMove = false;
      }
    }
    this.ClampTarget(this.currentZoomFactor);
    this.mCameraPos.x += (this.mTargetPos.x - this.mCameraPos.x) * num1;
    this.mCameraPos.y += (this.mTargetPos.y - this.mCameraPos.y) * num1;
    float x = this.CameraTiltIn + this.currentZoomFactor * (this.CameraTiltOut - this.CameraTiltIn);
    float num4 = -this.mCameraPos.z * Mathf.Sin(x * ((float) Math.PI / 180f));
    if (Application.isPlaying && !this.mTargetPos.z.Equals(this.mCameraPos.z))
      CInterfaceManager.instance.SetScaleFactor(this.currentZoomFactor);
    this.GetComponent<Camera>().transform.rotation = Quaternion.Euler(new Vector3(x, 0.0f, 0.0f));
    this.GetComponent<Camera>().transform.position = new Vector3(this.mCameraPos.x, this.mCameraPos.y + num4, this.mCameraPos.z);
  }

  public void SetZoom(float zoom)
  {
    this.mTargetPos.z = Mathf.Clamp(zoom, this.CameraZoomOut, this.CameraZoomIn);
    this.mTargetFactor = Mathf.Clamp((float) (((double) this.mTargetPos.z - (double) this.CameraZoomIn) / ((double) this.CameraZoomDefault - (double) this.CameraZoomIn)), 0.0f, 1f);
    for (int index = 0; index < this.CameraScrollBars.Count; ++index)
      this.CameraScrollBars[index].value = this.mTargetFactor;
  }

  public void SetZoomFactor(float factor)
  {
    if (!this.enabled)
      return;
    this.mTargetFactor = Mathf.Clamp(factor, 0.0f, 1f);
    this.mTargetPos.z = this.CameraZoomIn + factor * (this.CameraZoomDefault - this.CameraZoomIn);
    for (int index = 0; index < this.CameraScrollBars.Count; ++index)
      this.CameraScrollBars[index].value = this.mTargetFactor;
    if (!CGameManager.IsTutorialGame)
      return;
    if (TutorialSystem.IsModuleActive("22A") && (double) this.ZoomFactor < 0.5)
    {
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StopAllCoroutines();
      instance.StartCoroutine(instance.UpdateTutorial());
    }
    else
    {
      if (!TutorialSystem.IsModuleActive("24A") || (double) this.ZoomFactor <= 0.5)
        return;
      PIETutorialSystem instance = (PIETutorialSystem) TutorialSystem.Instance;
      instance.StopAllCoroutines();
      instance.StartCoroutine(instance.UpdateTutorial());
    }
  }

  private void ClampTarget(float zoomFactor)
  {
    Vector2 vector2_1 = this.lowMin + zoomFactor * (this.highMin - this.lowMin);
    Vector2 vector2_2 = this.lowMax + zoomFactor * (this.highMax - this.lowMax);
    if ((double) this.mTargetPos.x < (double) vector2_1.x)
      this.mTargetPos.x = vector2_1.x;
    else if ((double) this.mTargetPos.x > (double) vector2_2.x)
      this.mTargetPos.x = vector2_2.x;
    if ((double) this.mTargetPos.y < (double) vector2_1.y)
    {
      this.mTargetPos.y = vector2_1.y;
    }
    else
    {
      if ((double) this.mTargetPos.y <= (double) vector2_2.y)
        return;
      this.mTargetPos.y = vector2_2.y;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(this.mCameraPos, 0.1f);
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(this.mTargetPos, 0.1f);
  }

  public void SetCameraToDefault()
  {
    this.mCameraPos.z = this.mTargetPos.z = this.CameraZoomDefault;
    this.mCameraPos.x = this.mTargetPos.x = 0.0f;
    this.mCameraPos.y = this.mTargetPos.y = 0.0f;
    this.currentZoomFactor = this.mTargetFactor = 1f;
    this.transform.position = this.mCameraPos;
    this.GetComponent<Camera>().transform.rotation = Quaternion.Euler(Vector3.zero);
    for (int index = 0; index < this.CameraScrollBars.Count; ++index)
      this.CameraScrollBars[index].value = 1f;
  }

  public void DeltaCameraToDefault() => this.mTargetPos.z = this.CameraZoomDefault;

  public void RegisterScrollBar(UIScrollBar scroll)
  {
    this.CameraScrollBars.Add(scroll);
    scroll.value = 1f;
  }

  public void OnTutorialBegin(Module withModule)
  {
  }

  public void OnTutorialComplete(Module completedModule)
  {
  }

  public void OnTutorialSkip(Module skippedModule)
  {
  }

  public void OnTutorialModeExit(Module currentModule)
  {
  }

  public void OnTutorialSuspend(Module currentModule)
  {
  }

  public void OnTutorialResume(Module currentModule)
  {
  }
}
