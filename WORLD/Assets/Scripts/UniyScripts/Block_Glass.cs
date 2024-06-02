using UnityEngine;

public class Block_Glass : UnitObjBase
{
    public override void InitUnit()
    {
        unitType = UnitType.Block;
        blockTransparentType = UnitTransparentType.IsTransparent;
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
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(true);
        }
    }

    public override void OnDesEvent()
    {
        MainGame.mainGame.ChangeObjUnit((int)Posion.x, (int)Posion.y, (int)Posion.z, 0);
        LootItem();
        Debug.Log("glass Des");

        NeedNotifyAround(false);
    }
    public override void DesUnit()
    {
        NeedNotifyAround(false);
        base.DesUnit();
    }
}
