using System;
using UnityEngine;

public enum UnitType
{
    Null, Block
}
public enum UnitTransparentType
{
    IsTransparent, IsNotTransparent,
}
public abstract class UnitObjBase : MonoBehaviour
{
    public GameObject loot;
    public UnitType unitType;
    public Block_Type blockType;
    public UnitTransparentType blockTransparentType;
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
    private int hp = 1;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                OnDesEvent();
            }
        }
    }
    public void Awake()
    {
        InitUnit();
    }
    public abstract void InitUnit();
    public abstract void OnCreateEvent();
    public abstract void OnDesEvent();
    public void LootItem()
    {
        if (loot != null)
        {
            可拾取道具Base item = Instantiate(loot, MainGame.mainGame.道具).GetComponent<可拾取道具Base>();
            Unit_Null unit_Null = MainGame.unit[(int)Posion.x, (int)Posion.y, (int)Posion.z].GetComponent<Unit_Null>();
            item.Init(Posion, unit_Null);
            unit_Null.toolList.Add(item);
        }
    }
    public virtual void DesUnit()
    {
        Destroy(gameObject);
    }
    public virtual void SetOneActive(string s, bool b)
    {
        transform.Find(s).gameObject.SetActive(b);
    }
    public void NeedNotifyAround(bool isCreate)
    {
        int x, y, z;
        foreach (Transform item in transform)
        {
            x = (int)(Posion + MainGame.Direction[item.name].v3).x;
            y = (int)(Posion + MainGame.Direction[item.name].v3).y;
            z = (int)(Posion + MainGame.Direction[item.name].v3).z;
            if (MainGame.unit[x, y, z] != null && MainGame.unit[x, y, z].unitType == UnitType.Block && MainGame.unit[x, y, z].blockTransparentType == UnitTransparentType.IsNotTransparent)
            {
                UnitObjBase b = MainGame.unit[x, y, z];
                b.SetOneActive(MainGame.Direction[item.name].Other, !isCreate);
            }
            else
            {
                item.gameObject.SetActive(isCreate);
            }
        }
    }
}
