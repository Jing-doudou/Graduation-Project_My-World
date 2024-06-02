using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 登录面板 : MonoBehaviour
{
    public InputField account;
    public InputField password;
    public static 登录面板 ins;
    private void Awake()
    {
        ins = this.GetComponent<登录面板>();
    }
    private void Start()
    {
    }
    public void Login()
    {
        MsgLogin login = new MsgLogin();
        login.id = account.text;
        login.pw = password.text;
        NetManager.Send(login);
    }
    public void Register()
    {
        MsgRegister msgRegister = new MsgRegister();
        msgRegister.id = account.text;
        msgRegister.pw = password.text;
        NetManager.Send(msgRegister);
    } 
}
