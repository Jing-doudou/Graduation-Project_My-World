using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassItem : ��ʰȡ����Base
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnDesEvent()
    {
        base.OnDesEvent();
        MainGame.mainGame.bag.AddItem(id, 1); 
        Destroy(gameObject);
    }
}