using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Room
{
    public bool isReady=false;
    public int ItemIndex; 
    public RoomDate roomDate=new RoomDate();
    public static int id = 0;
    public List<ClientState> playerList = new List<ClientState>();
    public string roomChat;
    public string EnterMsg;
    public List<int> MapMsg;
    public int x;
    public int y;
    public int z;
}
[Serializable]
public class RoomDate
{
    public int PlayerConut;
    public string roomId; 

}
