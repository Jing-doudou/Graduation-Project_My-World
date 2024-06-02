using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class ClientState
{
    public Player Player = new Player();
    public Socket socket;
    public ByteArray readBuff = new ByteArray();
}
public class MsgLogin : MsgBase
{
    public MsgLogin()
    {
        protoName = "MsgLogin";
    }
    public string id;
    public string pw;
    public int result;
}
public class MsgRegister : MsgBase
{
    public MsgRegister()
    {
        protoName = "MsgRegister";
    }
    public string id;
    public string pw;
    public int result;
}
public class MsgSendToHall : MsgBase
{
    public MsgSendToHall()
    {
        protoName = "MsgSendToHall";
    }
    public string msg;
}
[Serializable]
public class UpdateRoomList : MsgBase
{
    public UpdateRoomList()
    {
        protoName = "UpdateRoomList";
    }
    public List<RoomDate> roomDateList;
}
public class CreateRoom : MsgBase
{
    public CreateRoom()
    {
        protoName = "CreateRoom";
    }
    public string roomId;
}
public class ExitRoom : MsgBase
{
    public ExitRoom()
    {
        protoName = "ExitRoom";
    }
    public string id;
}
public class DesRoom : MsgBase
{
    public DesRoom()
    {
        protoName = "DesRoom";
    }
    public string id;
}
[Serializable]
public class EnterRoom : MsgBase
{
    public EnterRoom()
    {
        protoName = "EnterRoom";
    }
    public string id;
    public string roomChat;
}
[Serializable]
public class UpdatePlayerList : MsgBase
{
    public UpdatePlayerList()
    {
        protoName = "UpdatePlayerList";
    }
    public string id;
    public List<PlayerDate> PlayerDateList;
}
public class SendTestMsg : MsgBase
{
    public SendTestMsg()
    {
        protoName = "SendTestMsg";
    }
    public string roomId;
    public string msg;
}
[Serializable]
public class CreateWorldInformation : MsgBase
{
    public CreateWorldInformation()
    {
        protoName = "CreateWorldInformation";
    }
    public string roomId;
    public int x;
    public int y;
    public int z;
    public List<int> MapMsg;

}
[Serializable]
public class SyncWorldInformation : MsgBase
{
    public SyncWorldInformation()
    {
        protoName = "SyncWorldInformation";
    }
    public int x;
    public int y;
    public int z;
    public string roomId;
    public List<int> MapMsg;
    public int ItemIndex; 
}
public class NewUnitObj : MsgBase
{
    public NewUnitObj()
    {
        protoName = "NewUnitObj";
    }
    public string roomId;
    public int x;
    public int y;
    public int z;
    public int index;
}

[Serializable]
public class EnterGame : MsgBase
{
    public EnterGame()
    {
        protoName = "EnterGame";
    }
    public string roomId;
}

public class DesUnitObj : MsgBase
{
    public DesUnitObj()
    {
        protoName = "DesUnitObj";
    }
    public string roomId;
    public int ItemIndex; 
    public int x;
    public int y;
    public int z;
}
public class DesItemMsg : MsgBase
{
    public DesItemMsg()
    {
        protoName = "DesItemMsg";
    }
    public string roomId;
    public int index;
}
public class SeriveMsg : MsgBase
{
    public SeriveMsg()
    {
        protoName = "SeriveMsg";
    }
    public string msg;
}
public class ExitGame : MsgBase
{
    public ExitGame()
    {
        protoName = "ExitGame";
    } 
}
