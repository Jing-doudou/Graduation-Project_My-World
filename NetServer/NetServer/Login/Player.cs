using System;
using System.Collections.Generic;
using System.Net.Sockets;

[Serializable]
public class Player
{
    public PlayerDate playerDate = new PlayerDate();
    public string roomId;
}
[Serializable]
public class PlayerDate
{
    public string Id;
    public string Pw;
    public bool isPlaying = false;
}