using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public GameObject GameUI;
    public GameObject NetUI;
    bool escState;

    public bool EscState
    {
        get => escState; set
        {
            escState = value;
            if (escState)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    private void Awake()
    {
        ins = this;
        EscState = true;

    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void InitGame()
    {
        //清理所有子物体
        GameUI.SetActive(true);
        NetUI.SetActive(false);
        MainGame.isGameTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscState = !EscState;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            对话框.ins.ChangeState();
        }
    }
    public void ChangeGameState()
    {
        gameObject.SetActive(!gameObject.active);
    }
}
