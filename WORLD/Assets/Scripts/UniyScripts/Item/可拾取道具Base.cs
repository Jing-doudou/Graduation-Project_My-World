using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class 可拾取道具Base : MonoBehaviour
{
    float rotateSpeed = 0.5f;
    float offsetSpeed = 0.1f;
    int offsetLength = 100;

    public static int ItemIndex = 0;
    public int index;
    public bool isMoving;
    public int id;
    public int num;
    public float offset;
    public Unit_Null target;
    public Unit_Null target2;
    private Vector3 posion;
    public Vector3 Posion
    {
        get => posion; set
        {
            posion = value;
            gameObject.name = value.ToString("F0");
            gameObject.transform.position = posion;
        }
    } 
    public void Init(Vector3 vector3, Unit_Null target)
    {
        index = ItemIndex++;
        offset = 0;
        Posion = vector3;
        this.target = target;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            DesItemMsg msg = new DesItemMsg();
            msg.roomId = 房间面板.Id;
            msg.index = index;
            NetManager.Send(msg);
            OnDesEvent(); 
        }
    }
    public void FixedUpdate()
    {
        IdleBehavior();
    }
    public void IdleBehavior()
    {
        if (isMoving) { return; }
        offset += offsetSpeed;
        transform.Rotate(0, rotateSpeed, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(offset) / offsetLength), transform.position.z);
    }
    public void MoveToTarget(Unit_Null target, Unit_Null target2)
    {
        this.target = target;
        this.target2 = target2;
        StopAllCoroutines();
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        while (true)
        {
            isMoving = true;
            if (Math.Abs((Posion - target.Posion).magnitude) < 0.1f)
            {
                Posion = target.Posion;
                if (target2 != null)
                {
                    target2.toolList.AddRange(target.toolList);
                    target.toolList.Clear();
                    target = target2;
                    target2 = null;
                    continue;
                }
                isMoving = false;
                yield break;
            }
            Posion = Vector3.Lerp(Posion, target.Posion, 0.2f);
            yield return new WaitForFixedUpdate();
        }
    }
    public virtual void OnDesEvent()
    {
        if (target != null && target.toolList.Contains(this))
        {
            target.toolList.Remove(this);
        }
        if (target2 != null && target2.toolList.Contains(this))
        {
            target2.toolList.Remove(this);
        } 
    }
    public void SyncDesEvent()
    {
        OnDesEvent();
        Destroy(gameObject);
    }
}
