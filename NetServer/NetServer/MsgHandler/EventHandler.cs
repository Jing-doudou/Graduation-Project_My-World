using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EventHandler
{
    public static void OnDisconnect(ClientState cs)
    {
        Console.WriteLine(cs.Player.playerDate.Id + "玩家退出");
        if (cs.Player != null)
        {
            string roomId = cs.Player.roomId;
            if (roomId == null)
            {
                NetManager.PlayerDic.Remove(cs.Player.playerDate.Id);
                NetManager.PlayerList.Remove(cs);
                return;
            }
            if (int.Parse(roomId) >= 0)
            {
                Room room = NetManager.RoomDic[roomId];
                for (int i = room.playerList.Count - 1; i >= 0; i--)
                {
                    if (room.playerList[i] == cs)
                    {
                        room.playerList.Remove(room.playerList[i]);
                    }
                }
                NetManager.PlayerDic.Remove(cs.Player.playerDate.Id);
                NetManager.PlayerList.Remove(cs);
            }
        }
    }
}