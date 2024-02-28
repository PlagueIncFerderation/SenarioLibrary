// Decompiled with JetBrains decompiler
// Type: ClientSocket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

#nullable disable
public class ClientSocket
{
  private Socket _clientSocket;
  private Thread _recvThread;
  private string _rIP;
  private int _rProt;

  public ClientSocket(string _rIP, int _rProt)
  {
    this._rIP = _rIP;
    this._rProt = _rProt;
    this.Connect();
  }

  private void Connect()
  {
    try
    {
      this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this._clientSocket.Connect((EndPoint) new IPEndPoint(IPAddress.Parse(this._rIP), this._rProt));
      this.OnSend(Encoding.Default.GetBytes("hello，server"));
      this.StartRecv();
    }
    catch
    {
      throw;
    }
  }

  private void StartRecv()
  {
    this._recvThread = new Thread(new ThreadStart(this.OnRecv));
    this._recvThread.IsBackground = true;
    Debug.Log((object) "开始接收消息");
    this._recvThread.Start();
  }

  private void OnRecv()
  {
    while (true)
    {
      byte[] numArray = new byte[256];
      this._clientSocket.Receive(numArray);
      Debug.Log((object) Encoding.Default.GetString(numArray));
    }
  }

  public void OnSend(byte[] data)
  {
    try
    {
      this._clientSocket.Send(data);
    }
    catch (Exception ex)
    {
      this.Close();
      throw;
    }
  }

  public void Close()
  {
    this._recvThread.Abort();
    this._clientSocket.Shutdown(SocketShutdown.Both);
    this._clientSocket.Close();
  }
}
