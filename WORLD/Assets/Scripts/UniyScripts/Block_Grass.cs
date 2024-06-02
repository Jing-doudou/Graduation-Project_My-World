using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Grass : UnitObjBase
{

    public Texture2D[] Texture2D;
    public new void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        OnCreateEvent();
    }
    public override void InitUnit()
    {
        unitType = UnitType.Block;
        blockTransparentType = UnitTransparentType.IsNotTransparent;
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }
    private IEnumerator MakeGrass()
    {
        yield return new WaitForSeconds(5);
        if ((int)Posion.y == 2 * MainGame.World_y - 1 || MainGame.unit[(int)Posion.x, (int)Posion.y + 1, (int)Posion.z].unitType == UnitType.Null)
        {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = Texture2D[0];
            this.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = Texture2D[1];
            this.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = Texture2D[1];
            this.transform.GetChild(3).GetComponent<MeshRenderer>().material.mainTexture = Texture2D[1];
            this.transform.GetChild(4).GetComponent<MeshRenderer>().material.mainTexture = Texture2D[1];
        }
    }
    private void OnEnable()
    {
        StartCoroutine(MakeGrass());
    }
    public override void SetOneActive(string s, bool b)
    {
        base.SetOneActive(s, b);
        StartCoroutine(MakeGrass());
    }
    public override void OnCreateEvent()
    {
        NeedNotifyAround(true);
    }
    public override void OnDesEvent()
    {
        MainGame.mainGame.ChangeObjUnit((int)Posion.x, (int)Posion.y, (int)Posion.z, 0); 
        LootItem(); 
        NeedNotifyAround(false);
    }
    public override void DesUnit()
    {
        NeedNotifyAround(false);
        base.DesUnit();
    }
}
