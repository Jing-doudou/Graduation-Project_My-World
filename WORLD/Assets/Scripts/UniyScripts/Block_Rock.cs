using UnityEngine;

public class Block_Rock : UnitObjBase
{
    public override void InitUnit()
    {
        unitType = UnitType.Block;
        blockTransparentType = UnitTransparentType.IsNotTransparent;
        foreach (Transform item in  transform)
        {
            item.gameObject.SetActive(false);
        }
    }
    public new void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        OnCreateEvent();
    }
    public override void OnCreateEvent()
    {
        NeedNotifyAround(true);
    }

    public virtual void ChackAround()
    {
        int x, y, z;
        foreach (Transform item in transform)
        {
            x = (int)(Posion + MainGame.Direction[item.name].v3).x;
            y = (int)(Posion + MainGame.Direction[item.name].v3).y;
            z = (int)(Posion + MainGame.Direction[item.name].v3).z;
            //如果相邻block为不可遮挡类型，则
            if (MainGame.unit[x, y, z].blockTransparentType != UnitTransparentType.IsTransparent)
            {
                item.gameObject.SetActive(true);
            }
        }
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
