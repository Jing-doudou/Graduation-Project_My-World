using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NetUIManager : MonoBehaviour
{
    public static string Player;
    public MsgPanel msgPanel;
    public static NetUIManager ins;
    public GameObject 登录;
    public GameObject 大厅;
    public GameObject 房间;
    public GameObject 聊天;
    private void Awake()
    {
        ins = GameObject.Find("Canvas").GetComponent<NetUIManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddMsgListener("MsgRegister", MsgHandler.MsgRegister);
        NetManager.AddMsgListener("MsgLogin", MsgHandler.MsgLogin);
        NetManager.AddMsgListener("CreateRoom", MsgHandler.CreateRoom);
        NetManager.AddMsgListener("EnterRoom", MsgHandler.EnterRoom);
        NetManager.AddMsgListener("ExitRoom", MsgHandler.ExitRoom);
        NetManager.AddMsgListener("DesRoom", MsgHandler.DesRoom);
        NetManager.AddMsgListener("UpdateRoomList", MsgHandler.UpdateRoomList);
        NetManager.AddMsgListener("UpdatePlayerList", MsgHandler.UpdatePlayerList);
        NetManager.AddMsgListener("SendTestMsg", MsgHandler.SendTestMsg);

        NetManager.AddMsgListener("EnterGame", MsgHandler.EnterGame);
        NetManager.AddMsgListener("SyncWorldInformation", MsgHandler.SyncWorldInformation);
        NetManager.AddMsgListener("CreateWorldInformation", MsgHandler.CreateWorldInformation);
        NetManager.AddMsgListener("DesUnitObj", MsgHandler.DesUnitObj);
        NetManager.AddMsgListener("NewUnitObj", MsgHandler.NewUnitObj);
        NetManager.AddMsgListener("DesItemMsg", MsgHandler.DesItemMsg);
        NetManager.AddMsgListener("SeriveMsg", MsgHandler.SeriveMsg); 
        NetManager.AddMsgListener("ExitGame", MsgHandler.ExitGame); 
        NetManager.Connect("127.0.0.1", 8888);

    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }

}
