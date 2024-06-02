using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
internal class MsgHandler
{
    public static void MsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 1)
        {
            NetUIManager.ins.msgPanel.gameObject.SetActive(true);
            NetUIManager.ins.msgPanel.msg.text = "登录失败";
        }
        else
        {
            NetUIManager.ins.msgPanel.gameObject.SetActive(true);
            NetUIManager.ins.msgPanel.msg.text = "登录成功";
            NetUIManager.Player = msg.id;
            NetUIManager.ins.msgPanel.closeBtn.onClick.AddListener(delegate ()
            {
                NetUIManager.ins.登录.gameObject.SetActive(false);
                NetUIManager.ins.大厅.gameObject.SetActive(true);
            });
        }
    }
    public static void MsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 1)
        {
            NetUIManager.ins.msgPanel.gameObject.SetActive(true);
            NetUIManager.ins.msgPanel.msg.text = "注册失败";
        }
        else
        {
            NetUIManager.ins.msgPanel.gameObject.SetActive(true);

            NetUIManager.ins.msgPanel.msg.text = "注册成功";
        }
    }
    public static void CreateRoom(MsgBase msgBase)
    {
        CreateRoom msg = (CreateRoom)msgBase;
        房间面板.Id = msg.roomId;
        NetUIManager.ins.聊天.GetComponent<对话框>().ClearTalkMsg();
        大厅面板.进入房间界面();
        UnityEngine.Debug.Log("收到创建协议");
    }
    public static void UpdateRoomList(MsgBase msgBase)
    {
        UnityEngine.Debug.Log("收到刷新房间协议");
        UpdateRoomList msg = (UpdateRoomList)msgBase;
        Room.roomList.Clear();
        for (int i = 0; i < msg.roomDateList.Count; i++)
        {
            Room room = new Room() { roomDate = msg.roomDateList[i] };
            Room.roomList.Add(room);
        }
        大厅面板.刷新房间List();
    }
    public static void EnterRoom(MsgBase msgBase)
    {
        EnterRoom msg = (EnterRoom)msgBase;
        房间面板.Id = msg.id;
        NetUIManager.ins.聊天.GetComponent<对话框>().ClearTalkMsg();
        NetUIManager.ins.聊天.GetComponent<对话框>().ReceiveMsg(msg.roomChat);
        UnityEngine.Debug.Log("收到进入房间协议");
        大厅面板.进入房间界面();
    }
    public static void UpdatePlayerList(MsgBase msgBase)
    {
        UpdatePlayerList msg = (UpdatePlayerList)msgBase;
        Room.playerList.Clear();
        for (int i = 0; i < msg.PlayerDateList.Count; i++)
        {
            Player player = new Player() { playerDate = msg.PlayerDateList[i] };
            Room.playerList.Add(player);
        }
        房间面板.PlayerList = Room.playerList;
        UnityEngine.Debug.Log("收到刷新房客协议");
    }
    public static void ExitRoom(MsgBase msgBase)
    {
        房间面板.ins.退出房间();
        UnityEngine.Debug.Log("收到退出房客协议");
    }
    public static void DesRoom(MsgBase msgBase)
    {
        房间面板.ins.销毁此房间();
        UnityEngine.Debug.Log("收到销毁房间协议");
    }
    public static void SendTestMsg(MsgBase msgBase)
    {
        SendTestMsg msg = (SendTestMsg)msgBase;
        UnityEngine.Debug.Log("收到消息");
        NetUIManager.ins.聊天.GetComponent<对话框>().ReceiveMsg(msg.msg);
    }
    public static void CreateWorldInformation(MsgBase msgbase)
    {
        UnityEngine.Debug.Log("创建地图信息并同步");
        CreateWorldInformation msg = (CreateWorldInformation)msgbase;
        UnityEngine.Debug.Log("世界的大小为：" + msg.x + "_" + msg.y + "_" + msg.z);
        MainGame.mainGame.InitWorld(msg.x, msg.y, msg.z);

    }
    public static void SyncWorldInformation(MsgBase msgbase)
    {
        UnityEngine.Debug.Log("同步地图信息");
        SyncWorldInformation msg = (SyncWorldInformation)msgbase;
        可拾取道具Base.ItemIndex = msg.ItemIndex;
        UnityEngine.Debug.Log("世界的大小为：" + msg.x + "_" + msg.y + "_" + msg.z);
        MainGame.MapMsg = msg.MapMsg;
        MainGame.mainGame.SyncWorld(msg.x, msg.y, msg.z, msg.MapMsg);
    }
    public static void EnterGame(MsgBase msgbase)
    {
        EnterGame msg = (EnterGame)msgbase;
        msg.roomId = 房间面板.Id;
        NetManager.Send(msg);
    }
    public static void DesItemMsg(MsgBase msgbase)
    {
        DesItemMsg msg = (DesItemMsg)msgbase;
        msg.roomId = 房间面板.Id;
        for (int i = 0; i < MainGame.mainGame.道具.childCount; i++)
        {
            UnityEngine.Debug.Log("销毁itme协议");

            可拾取道具Base current = MainGame.mainGame.道具.GetChild(i).GetComponent<可拾取道具Base>();
            if (current.index == msg.index)
            {
                UnityEngine.Debug.Log("找到item:" + msg.index);
                current.SyncDesEvent();
                return;
            }
            else
            {
                UnityEngine.Debug.Log("未找到item:" + msg.index);
            }
        }
    }
    public static void DesUnitObj(MsgBase msgbase)
    {
        DesUnitObj msg = (DesUnitObj)msgbase;
        MainGame.unit[msg.x, msg.y, msg.z].GetComponent<UnitObjBase>().Hp = 0;
    }
    public static void NewUnitObj(MsgBase msgbase)
    {
        NewUnitObj msg = (NewUnitObj)msgbase;
        MainGame.mainGame.ChangeObjUnit(msg.x, msg.y, msg.z, msg.index);
    }

    internal static void SeriveMsg(MsgBase msgBase)
    {
        SeriveMsg serverMsg = (SeriveMsg)msgBase;
        NetUIManager.ins.msgPanel.ShowMsg(serverMsg.msg);
    }
    internal static void ExitGame(MsgBase msgBase)
    {
        MainGame.mainGame.ClearGame();
        UnityEngine.Debug.Log("ExitGame");  
    }
}
