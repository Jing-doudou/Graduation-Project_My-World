using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum PlayerState
{
    Fly, Walk
}

public class PlayerText : MonoBehaviour
{
    public Camera eye;
    float speedMove = 6;//移动速度
    float speedH = 5f;//横向移动速度
    float speedXAngle = 2;//旋转角速度
    float speedYAngle = 2;//旋转角速度
    public CharacterController cc;//角色控制器
    private PlayerState playerState;
    public PlayerState PlayerState
    {
        get => playerState; set
        {
            playerState = value;
            switch (playerState)
            {
                case PlayerState.Fly:
                    doubleClick = null;
                    doubleClick += () =>
                    {
                        Debug.Log("double");
                        PlayerState = PlayerState.Walk;
                    };
                    onceClick = null;
                    onceClick += () =>
                    {
                        Debug.Log("once");
                        cc.Move(10 * transform.up * Time.deltaTime);
                    };
                    break;
                case PlayerState.Walk:
                    doubleClick = null;
                    doubleClick += () =>
                    {
                        Debug.Log("double");
                        PlayerState = PlayerState.Fly;
                    };
                    onceClick = null;
                    onceClick += () =>
                    {
                        Debug.Log("once");
                        G = 0;
                        cc.Move(transform.up);
                        Invoke(nameof(JunmpInvoke), .1f);
                    };
                    break;
                default:
                    break;
            }
        }
    }
    public void JunmpInvoke()
    {
        G = -5;
    }
    float minAngle = -45;//抬头最高角度 
    float maxAngle = 45;
    float yRote;
    public float G = -10f;
    int RayLen = 5;

    private void Start()
    {
        PlayerState = PlayerState.Walk;
    }

    private void Update()
    {
        if (GameManager.ins.EscState)
        {
            return;
        }
        Attack();
        IsDoubleClick();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (GameManager.ins.EscState)
        {
            return;
        }
        Move();
    }
    private void Move()
    {
        //移动块
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        cc.Move(transform.forward * Time.deltaTime * y * speedMove);
        cc.Move(transform.right * Time.deltaTime * x * speedH);
        if (true)
        {
            float xRote = Input.GetAxis("Mouse X");
            transform.Rotate(transform.up * speedXAngle * xRote);

            yRote -= Input.GetAxis("Mouse Y");
            yRote = Mathf.Clamp(yRote, minAngle, maxAngle);
            eye.transform.localEulerAngles = new Vector3(yRote * speedYAngle, 0, 0);//自身坐标的角度，
        }
        switch (playerState)
        {
            case PlayerState.Fly:
                break;
            case PlayerState.Walk:
                cc.Move(transform.up * G * Time.deltaTime);
                break;
            default:
                break;
        }
    }
    public static DesUnitObj des = new DesUnitObj();
    public static NewUnitObj newUnit = new NewUnitObj();
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit ray;
            if (Physics.Raycast(eye.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out ray, RayLen))
            {
                if (ray.transform.parent.GetComponent<UnitObjBase>() != null)
                {
                    Transform target = ray.transform.parent;
                    des.x = (int)target.position.x;
                    des.y = (int)target.position.y;
                    des.z = (int)target.position.z;
                    des.ItemIndex = 可拾取道具Base.ItemIndex+1;
                    NetManager.Send(des);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit ray;
            int x, y, z;
            if (Physics.Raycast(eye.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out ray, RayLen))
            {
                if (ray.transform.parent.GetComponent<UnitObjBase>() != null)
                {
                    if (MainGame.mainGame.bag.currentItme == null)
                    {
                        return;
                    }
                    UnitObjBase unit = ray.transform.parent.GetComponent<UnitObjBase>();
                    x = (int)(unit.Posion + MainGame.Direction[ray.transform.name].v3).x;
                    y = (int)(unit.Posion + MainGame.Direction[ray.transform.name].v3).y;
                    z = (int)(unit.Posion + MainGame.Direction[ray.transform.name].v3).z;
                    newUnit.x = x; newUnit.y = y; newUnit.z = z;
                    newUnit.index = MainGame.mainGame.bag.currentItme.Id;
                    NetManager.Send(newUnit);
                    MainGame.mainGame.bag.UseCurrentItem();

                }
            }
        }
    }


    bool IEIsRuning = false;
    public Action onceClick;
    public Action doubleClick;


    public void IsDoubleClick()
    {
        if (playerState == PlayerState.Fly)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                onceClick();
                if (IEIsRuning) return;
                StartCoroutine(JudgeClick());
            }
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (IEIsRuning) return;
            StartCoroutine(JudgeClick());
        }
        if (!cc.isGrounded)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onceClick();
            if (IEIsRuning) return;
            StartCoroutine(JudgeClick());
        }
    }

    IEnumerator JudgeClick()
    {
        IEIsRuning = true;
        if (doubleClick != null)
        {
            float time = .2f;
            while (time >= 0)
            {
                time -= Time.deltaTime;
                yield return null;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    doubleClick();
                    IEIsRuning = false;
                    yield break;
                }
            }
        }
        IEIsRuning = false;
    }
}
