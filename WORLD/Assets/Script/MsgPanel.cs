using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgPanel : MonoBehaviour
{
    public UnityEngine.UI.Text msg;
    public Button closeBtn;
    private void Awake()
    {
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(delegate ()
        {
            this.gameObject.SetActive(false);
        });
    }
    public void ShowMsg(string s)
    {
        this.gameObject.SetActive(true);
        msg.text = s;
    }
}
