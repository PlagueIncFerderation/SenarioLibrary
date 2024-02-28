// Decompiled with JetBrains decompiler
// Type: Client
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System.Net.Sockets;
using UnityEngine;

#nullable disable
public class Client : MonoBehaviour
{
  private Socket tcpClient;
  private string serverIP;
  private int serverPort;
  private ClientSocket c1;

  private void Start() => this.c1 = new ClientSocket(this.serverIP, this.serverPort);

  public Client()
  {
    this.serverIP = "47.242.150.236";
    this.serverPort = 11451;
  }
}
