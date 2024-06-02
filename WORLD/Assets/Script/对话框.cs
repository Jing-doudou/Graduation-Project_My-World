using UnityEngine;
using UnityEngine.UI;

public class 对话框 : MonoBehaviour
{
    public InputField inputField;
    public Text TextField;
    public bool isOpen = false;
    public static 对话框 ins;
    private void Awake()
    {
        ins = this;
    }
    public void SendMsg()
    {
        SendTestMsg msg = new SendTestMsg();
        msg.roomId = 房间面板.Id;
        msg.msg = inputField.text.ToString();
        NetManager.Send(msg);
        inputField.text = "";
    }
    public void ReceiveMsg(string s)
    {
        TextField.text = string.Format("{0}\r\n{1}", s, TextField.text);
    }
    public void ClearTalkMsg()
    {
        TextField.text = "";
    }
    public void ChangeState()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
    }
}
