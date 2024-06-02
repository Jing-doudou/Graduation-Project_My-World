using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBtn : MonoBehaviour
{
    public string RoomId;
    public void EnterRoom()
    {
        RoomId = transform.name;
        EnterRoom msg = new EnterRoom();
        msg.id = RoomId; 
        NetManager.Send(msg);
    }
}
