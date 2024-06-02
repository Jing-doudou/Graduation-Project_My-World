using System.Collections.Generic;
using UnityEngine;

public class Unit_Null : UnitObjBase
{
    public List<可拾取道具Base> toolList;
    private new void Awake()
    {
        base.Awake();
        toolList = new List<可拾取道具Base>();
    }
    public void Start()
    {
        OnCreateEvent();
    }
    public override void InitUnit()
    {
        unitType = UnitType.Null;
    }

    public override void OnCreateEvent()
    {
        if (!MainGame.isGameTime)
        {
            return;
        }
        if (Posion.y == 2 * MainGame.World_y)
        {
            return;
        }
        UnitObjBase unitObjBase = MainGame.unit[(int)Posion.x, (int)Posion.y + 1, (int)Posion.z];
        Unit_Null UpNull = MainGame.unit[(int)Posion.x, (int)Posion.y + 1, (int)Posion.z].GetComponent<Unit_Null>();
 
        if (unitObjBase!=null&&unitObjBase.unitType == UnitType.Null && UpNull.toolList.Count != 0)
        {
            toolList.AddRange(UpNull.toolList);
            UpNull.toolList.Clear();
            foreach (var item in toolList)
            {
                item.MoveToTarget(this, null);
            }
        }
        Unit_Null targetNull = GetDownNullUnit();
        if (targetNull != null)
        {
            TransferList(targetNull, null);
        }
    }
    /// <summary>
    /// 获得最下面的空单位
    /// </summary>
    /// <returns>返回最下面的空单位，如果没有则返回空</returns>
    public Unit_Null GetDownNullUnit()
    {
        int i = 0;
        while ((int)Posion.y - i >= 0)
        {
            UnitObjBase unitObjBase = MainGame.unit[(int)Posion.x, (int)Posion.y - i, (int)Posion.z];
            if (unitObjBase.unitType == UnitType.Null)
            {
                i++;
                continue;
            }
            else
            {
                if (i == 1)
                {
                    return null;
                }
                Unit_Null UpNull = MainGame.unit[(int)Posion.x, (int)Posion.y - i + 1, (int)Posion.z].GetComponent<Unit_Null>();
                return UpNull;
            }
        }
        return null;
    }
    public void TransferList(Unit_Null UpNull, Unit_Null downNull)
    {
        UpNull.toolList.AddRange(toolList);
        foreach (var item in toolList)
        {
            item.MoveToTarget(UpNull, downNull);
        }
        toolList.Clear();
    }
    public override void DesUnit()
    {
        //if (!MainGame.isGameTime)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Unit_Null targetUnit;
        if (Posion.y + 1 < 2 * MainGame.World_y)
        {
            int x, y, z;
            foreach (Transform item in transform)
            {
                x = (int)(Posion + MainGame.Direction[item.name].v3).x;
                y = (int)(Posion + MainGame.Direction[item.name].v3).y;
                z = (int)(Posion + MainGame.Direction[item.name].v3).z;
                if (MainGame.unit[x, y, z].unitType == UnitType.Null)
                {
                    targetUnit = MainGame.unit[x, y, z].GetComponent<Unit_Null>();
                    Unit_Null downUnit = targetUnit.GetDownNullUnit();
                    TransferList(targetUnit, downUnit);
                    Destroy(gameObject);
                    return;
                }
            }
        } 
        //寻找四周

        Destroy(gameObject);
    }

    public override void OnDesEvent()
    {
    }
}
