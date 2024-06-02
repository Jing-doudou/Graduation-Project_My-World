using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class 房间面板 : MonoBehaviour
{
    private static string id;
    public Text roomIdText;
    public static 房间面板 ins;
    private static List<Player> playerList = new List<Player>();
    public Player playerPrefab;
    public Transform playerParent;

    public GameObject NetUI;
    public GameObject GameUI;

    public static List<Player> PlayerList
    {
        get => playerList; set
        {
            playerList = value;
            刷新玩家List();
        }
    }
    public static string Id
    {
        get => id; set
        {
            id = value;
            PlayerText.des.roomId = 房间面板.Id;
            PlayerText.newUnit.roomId = 房间面板.Id;
        }
    }
    private void Awake()
    {
        ins = this.GetComponent<房间面板>();
    }
    private void OnEnable()
    {
        roomIdText.text = Id;
    }
    public static void 刷新玩家List()
    {
        foreach (Transform child in ins.playerParent.transform)
        {  // 递归删除子对象的子对象
            Destroy(child.gameObject); // 删除子对象本身
        }
        foreach (var p in PlayerList)
        {
            Player thisPlayer = Instantiate(ins.playerPrefab, ins.playerParent);
            thisPlayer.playerDate.Id = p.playerDate.Id;
            thisPlayer.Init(p.playerDate.Id.ToString());
        }
    }
    public void 销毁此房间()
    {

    }
    public void 退出房间()
    {
        ins.gameObject.SetActive(false);
        NetUIManager.ins.聊天.gameObject.SetActive(false);

    }
    public void 发出退出房间协议()
    {
        ExitRoom msg = new ExitRoom();
        msg.id = Id;
        NetManager.Send(msg);
    }
    public void StartGame()
    {
        EnterGame enterGame = new EnterGame();
        enterGame.roomId = Id;
        NetManager.Send(enterGame);
        //ChangeUI();
    }
    public void ChangeUI()
    {
        NetUI.SetActive(!NetUI.active);
        GameUI.SetActive(!GameUI.active);
    }
}
