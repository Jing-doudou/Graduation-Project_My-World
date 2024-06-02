using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public partial class MsgHandler
{
    public static void MsgRegister(ClientState c, MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (DBManager.Register(msg.id, msg.pw))
        {
            msg.result = 0;
        }
        else
        {
            msg.result = 1;
        }
        NetManager.Send(c, msg);
    }
    public static void MsgLogin(ClientState c, MsgBase msgbase)
    {
        MsgLogin msg = (MsgLogin)msgbase;
        if (!DBManager.CheckPassword(msg.id, msg.pw))
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            return;
        }
        DBManager.UpdatePlayerLog(msg.id);
        msg.result = 0;
        Player player = new Player();
        player.playerDate.Id = msg.id;
        player.playerDate.Pw = msg.pw;
        NetManager.PlayerDic[player.playerDate.Id] = player;
        NetManager.PlayerList.Add(c);
        c.Player = player;
        Console.WriteLine("返回登陆协议");
        NetManager.Send(c, msg);
    }
    public static void UpdateRoomList(ClientState c, MsgBase msgbase)
    {
        UpdateRoomList msg = (UpdateRoomList)msgbase;
        List<RoomDate> roomDateList = new List<RoomDate>();
        Console.WriteLine("房间数量：" + NetManager.RoomDic.Count);
        foreach (Room item in NetManager.RoomDic.Values)
        {
            item.roomDate.PlayerConut = item.playerList.Count;
            roomDateList.Add(item.roomDate);
            Console.WriteLine("房间ID:" + item.roomDate.roomId);
            Console.WriteLine("房间玩家数量:" + item.roomDate.PlayerConut);
        }
        msg.roomDateList = roomDateList;
        NetManager.Send(c, msg);
    }
    public static void CreateRoom(ClientState c, MsgBase msgbase)
    {
        Console.WriteLine("客户端创建房间");
        Room room = new Room();
        Room.id = DBManager.GetRoomId();
        room.EnterMsg += "(" + c.Player.playerDate.Id + ")";
        room.roomDate.roomId = Room.id.ToString();
        c.Player.roomId = DBManager.GetRoomId().ToString();
        DBManager.InitRoom(Room.id.ToString());
        room.roomDate.PlayerConut = NetManager.RoomDic.Count;
        NetManager.RoomDic[room.roomDate.roomId.ToString()] = room;
        Console.WriteLine("房间数量：" + NetManager.RoomDic.Count);
        room.playerList.Add(c);
        CreateRoom msg = new CreateRoom();
        msg.roomId = room.roomDate.roomId;
        Console.WriteLine("房间ID：" + room.roomDate.roomId);
        Console.WriteLine("房间玩家数量：" + room.playerList.Count);
        NetManager.Send(c, msg);
        UpdatePlayerList msg2 = new UpdatePlayerList();
        msg2.id = room.roomDate.roomId.ToString();
        Console.WriteLine(NetManager.RoomDic.Count);

        UpdatePlayerList(c, msg2);
        UpdateRoomList msg3 = new UpdateRoomList();
        foreach (var item in NetManager.PlayerList)
        {
            UpdateRoomList(item, msg3);
        }
    }
    public static void UpdatePlayerList(ClientState c, MsgBase msgbase)
    {
        UpdatePlayerList msg = (UpdatePlayerList)msgbase;
        List<PlayerDate> msgList = new List<PlayerDate>();
        foreach (ClientState item in NetManager.RoomDic[msg.id].playerList)
        {
            msgList.Add(item.Player.playerDate);
        }
        msg.PlayerDateList = msgList;
        NetManager.Send(c, msg);
    }
    public static void EnterRoom(ClientState c, MsgBase msgbase)
    {
        EnterRoom msg = (EnterRoom)msgbase;
        if (NetManager.RoomDic[msg.id].playerList.Count == 2)
        {
            //返回错误信息
            SeriveMsg seriveMsg = new SeriveMsg();
            seriveMsg.msg = "房间已满";
            NetManager.Send(c, seriveMsg);
            return;
        }
        c.Player.roomId = msg.id;
        msg.roomChat = NetManager.RoomDic[msg.id].roomChat;
        NetManager.RoomDic[msg.id].EnterMsg += "(" + msg.id + ")";
        NetManager.RoomDic[msg.id].playerList.Add(c);
        NetManager.Send(c, msg);

        UpdatePlayerList msg2 = new UpdatePlayerList();
        msg2.id = msg.id;
        foreach (var item in NetManager.RoomDic[msg.id].playerList)
        {
            UpdatePlayerList(item, msg2);
        }
    }
    public static void ExitRoom(ClientState c, MsgBase msgbase)
    {
        ExitRoom msg = (ExitRoom)msgbase;
        c.Player.roomId = "-1";
        Console.WriteLine("当前房间数量:" + NetManager.RoomDic.Count);
        Console.WriteLine("退出房间的ID:" + msg.id);
        Console.WriteLine("退出房间的玩家ID:" + c.Player.playerDate.Id);
        DBManager.UpdateRoom(msg.id, NetManager.RoomDic[msg.id].roomChat, NetManager.RoomDic[msg.id].EnterMsg, NetManager.RoomDic[msg.id].MapMsg);
        NetManager.RoomDic[msg.id].playerList.Remove(c);
        UpdatePlayerList msg3 = new UpdatePlayerList();
        msg3.id = msg.id;
        NetManager.Send(c, msg);
        if (NetManager.RoomDic[msg.id].playerList.Count != 0)
        {
            foreach (var item in NetManager.RoomDic[msg.id].playerList)
            {
                UpdatePlayerList(item, msg3);
            }
        }
        else
        {
            NetManager.RoomDic.Remove(msg.id);
            UpdateRoomList msg2 = new UpdateRoomList();
            foreach (var item in NetManager.PlayerList)
            {
                UpdateRoomList(item, msg2);
            }
        }
    }
    public static void SendTestMsg(ClientState c, MsgBase msgbase)
    {
        SendTestMsg msg = (SendTestMsg)msgbase;
        msg.msg = "(" + c.Player.playerDate.Id + ")" + "\t" + ":" + msg.msg;
        NetManager.RoomDic[msg.roomId].roomChat = msg.msg + "\r\n" + NetManager.RoomDic[msg.roomId].roomChat;
        foreach (var item in NetManager.RoomDic[msg.roomId].playerList)
        {
            NetManager.Send(item, msg);
        }
    }
    public static void DesUnitObj(ClientState c, MsgBase msgbase)
    {
        DesUnitObj msg = (DesUnitObj)msgbase;
        Room targetRoom = NetManager.RoomDic[msg.roomId];
        targetRoom.ItemIndex = msg.ItemIndex;
        targetRoom.MapMsg[msg.x * 2 * targetRoom.y * targetRoom.z + msg.y * targetRoom.z + msg.z] = 0;
        foreach (var item in NetManager.RoomDic[msg.roomId].playerList)
        {
            if (item.Player.playerDate.isPlaying)
            {
                NetManager.Send(item, msg);
            }
        }
    }
    public static void DesItemMsg(ClientState c, MsgBase msgbase)
    {
        DesItemMsg msg = (DesItemMsg)msgbase;
        Room targetRoom = NetManager.RoomDic[msg.roomId];
        foreach (var item in targetRoom.playerList)
        {
            if (c != item)
            {
                if (item.Player.playerDate.isPlaying)
                {
                    NetManager.Send(item, msg);
                }
            }
        }
    }
    public static void NewUnitObj(ClientState c, MsgBase msgbase)
    {
        NewUnitObj msg = (NewUnitObj)msgbase;
        Room targetRoom = NetManager.RoomDic[msg.roomId];
        targetRoom.MapMsg[msg.x * 2 * targetRoom.y * targetRoom.z + msg.y * targetRoom.z + msg.z] = msg.index;
        foreach (var item in NetManager.RoomDic[msg.roomId].playerList)
        {
            if (item.Player.playerDate.isPlaying)
            {
                NetManager.Send(item, msg);
            }
        }
    }
    public static void CreateWorldInformation(ClientState c, MsgBase msgbase)
    {
        CreateWorldInformation msg = (CreateWorldInformation)msgbase;
        NetManager.RoomDic[msg.roomId].MapMsg = msg.MapMsg;
        NetManager.RoomDic[msg.roomId].isReady = true;
        NetManager.RoomDic[msg.roomId].x = msg.x;
        NetManager.RoomDic[msg.roomId].y = msg.y;
        NetManager.RoomDic[msg.roomId].z = msg.z;
        Console.WriteLine("已创建的大小:" + msg.x + "_" + msg.y + "_" + msg.z);
    }
    public static void ExitGame(ClientState c, MsgBase msgbase)
    {
        Console.WriteLine(c.Player.playerDate.Id + ":退出游戏");
        c.Player.playerDate.isPlaying = false;
        ExitGame msg = (ExitGame)msgbase;
        NetManager.Send(c, msg);
    }
    public static void EnterGame(ClientState c, MsgBase msgbase)
    {
        EnterGame msg = (EnterGame)msgbase;

        if (c == NetManager.RoomDic[msg.roomId].playerList[0])
        {
            if (NetManager.RoomDic[msg.roomId].isReady)
            {
                SyncWorldInformation syncWorldmsg = new SyncWorldInformation();
                syncWorldmsg.x = NetManager.RoomDic[msg.roomId].x;
                syncWorldmsg.y = NetManager.RoomDic[msg.roomId].y;
                syncWorldmsg.z = NetManager.RoomDic[msg.roomId].z;
                syncWorldmsg.MapMsg = NetManager.RoomDic[msg.roomId].MapMsg;
                syncWorldmsg.ItemIndex = NetManager.RoomDic[msg.roomId].ItemIndex;
                Console.WriteLine("玩家:" + c.Player.playerDate.Id + "正在读取世界");
                c.Player.playerDate.isPlaying = true;
                NetManager.Send(c, syncWorldmsg);
                return;
            }
            CreateWorldInformation createWorldmsg = new CreateWorldInformation();
            createWorldmsg.x = 25;
            createWorldmsg.y = 10;
            createWorldmsg.z = 25;
            Console.WriteLine("玩家为房主，正在创建世界");
            c.Player.playerDate.isPlaying = true;
            NetManager.Send(c, createWorldmsg);
        }
        else
        {
            if (!NetManager.RoomDic[msg.roomId].isReady)
            {
                //返回erro信息
                SeriveMsg seriveMsg = new SeriveMsg();
                seriveMsg.msg = "房主未初始化地图";
                NetManager.Send(c, seriveMsg);
                return;
            }
            SyncWorldInformation syncWorldmsg = new SyncWorldInformation();
            syncWorldmsg.x = NetManager.RoomDic[msg.roomId].x;
            syncWorldmsg.y = NetManager.RoomDic[msg.roomId].y;
            syncWorldmsg.z = NetManager.RoomDic[msg.roomId].z;
            syncWorldmsg.MapMsg = NetManager.RoomDic[msg.roomId].MapMsg;
            syncWorldmsg.ItemIndex = NetManager.RoomDic[msg.roomId].ItemIndex;
            Console.WriteLine("玩家为访客，正在读取世界");
            Console.WriteLine("MapSize:" + syncWorldmsg.x + "_" + syncWorldmsg.y + "_" + syncWorldmsg.z);
            c.Player.playerDate.isPlaying = true;
            NetManager.Send(c, syncWorldmsg);
        }
    }

}

