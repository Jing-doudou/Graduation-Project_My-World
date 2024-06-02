using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 大厅面板 : MonoBehaviour
{
    public Transform roomParent;
    public Room roomPrefab;
    public 对话框 talkPanel;
    public static 大厅面板 ins;


    private void Awake()
    {
        ins = this.GetComponent<大厅面板>();
    }
    private void Start()
    {
        发送刷新房间请求();
    }
    public void 发送刷新房间请求()
    {
        Debug.Log("发送刷新房间List协议");
        UpdateRoomList udateRoomList = new UpdateRoomList();
        NetManager.Send(udateRoomList);
        //发送协议
    }
    public void 发送创建房间请求()
    {
        Debug.Log("发送CreateRoom协议");
        CreateRoom createRoom = new CreateRoom();
        NetManager.Send(createRoom);
    }
    public static void 刷新房间List()
    {
        Debug.Log("刷新房间list：" + Room.roomList.Count);
        foreach (Transform child in ins.roomParent.transform)
        {  // 递归删除子对象的子对象
            Destroy(child.gameObject); // 删除子对象本身
        }
        foreach (var room in Room.roomList)
        {
            Room thisRoom = Instantiate(ins.roomPrefab, ins.roomParent);
            thisRoom.InitRoom(room);
            thisRoom.transform.name = room.roomDate.roomId;
        }
    }
    public static void 进入房间界面()
    {
        //进入房间界面
        UnityEngine.Debug.Log("创建并进入房间");

        NetUIManager.ins.房间.gameObject.SetActive(true);
        NetUIManager.ins.聊天.gameObject.SetActive(true);
    }
}
