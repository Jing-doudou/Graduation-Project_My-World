using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class Room : MonoBehaviour
{
    public RoomDate roomDate=new RoomDate(); 
    public Text roomMsg; 
    public static List<Player> playerList = new List<Player>();
    public static List<Room> roomList = new List<Room>();
 

    public void InitRoom(Room room)
    {
        roomDate.PlayerConut = room.roomDate.PlayerConut;
        roomDate.roomId = room.roomDate.roomId;
        roomMsg.text = "玩家数量：" + roomDate.PlayerConut + "__ RoomId:" + roomDate.roomId;
    }
    public void OnClickRoom()
    {
        //发送进入协议
        EnterRoom msg = new EnterRoom();
        msg.id = roomDate.roomId;
        NetManager.Send(msg);
    }
}
[Serializable]
public class RoomDate
{
    public int PlayerConut;
    public string roomId;
}
