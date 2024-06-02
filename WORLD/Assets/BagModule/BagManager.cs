using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{

    public ItemBase[] itemsPrefab;
    public Transform itemListParent;
    public Transform BagListParent;

    public ItemBase currentItme;
    public RectTransform selectIcon;
    public List<Container> UIBagList = new List<Container>();
    private int selectIndex = 0;
    public int SelectIndex
    {
        get => selectIndex;
        set
        {
            selectIndex = value;
            if (selectIndex > 8)
            {
                selectIndex = 0;
            }
            if (selectIndex < 0)
            {
                selectIndex = 8;
            }
            currentItme = UIBagList[selectIndex].Item;
            selectIcon.position = UIBagList[selectIndex].rectTransform.position;
        }
    }
    public Animator bagAnimator;
    private bool isOpen;
    public bool IsOpen
    {
        get => isOpen; set
        {
            isOpen = value;
            selectIcon.gameObject.SetActive(!IsOpen);
            bagAnimator.SetBool("Open", IsOpen);
        }
    }

    private void Start()
    {
        {
            IsOpen = false;
            //init bag
            for (int i = 0; i < 9; i++)
            {
                Container c = BagListParent.GetChild(i).GetComponent<Container>();
                UIBagList.Add(c);
            }
        }
    }
    public void func1()
    {
        AddItem(1, 10);
    }
    public void func2()
    {
        AddItem(2, 10);
    }
    public void func3()
    {
        AddItem(3, 10);
    }
    public void BagSwitch()
    {
        IsOpen = !IsOpen;
    }
    private void Update()
    {
        SwitchCurrentItem();
    }
    public void SwitchCurrentItem()
    {
        if (IsOpen) return;
        float scrollWheelInput1 = Input.mouseScrollDelta.y;
        if (scrollWheelInput1 < 0f)
        {
            SelectIndex++;
        }
        if (scrollWheelInput1 > 0f)
        {
            SelectIndex--;
        }

    }
    private bool haveThisItem;
    public void AddItem(int id, int num)
    {
        haveThisItem = false;
        //检查是否有此类道具，无后生成新的，有则更改数量
        for (int i = 0; i < 9; i++)
        {
            Container c = BagListParent.GetChild(i).GetComponent<Container>();

            if (c.Item != null && c.Item.Id == id)
            {
                c.Item.Num += num;
                haveThisItem = true;
                return;
            }
        }
        if (haveThisItem)
        {
            return;
        }
        Container container = FindNullSpace();
        if (!container)
        {
            return;
        }
        container.Item = Instantiate(itemsPrefab[id], itemListParent);
        container.Item.Num = num;
    }
    public void UseCurrentItem()
    {
        if (currentItme == null)
        {
            return;
        }
        if (currentItme.Num > 0)
        {
            currentItme.Num--;
            if (currentItme.Num == 0)
            {
                Destroy(currentItme.gameObject);
                currentItme = null;
            }
        }
    }
    public Container FindNullSpace()
    {
        for (int i = 0; i < 9; i++)
        {
            Container c = BagListParent.GetChild(i).GetComponent<Container>();
            if (c.Item == null)
            {
                Debug.Log("have null");
                return c;
            }
        }
        Debug.Log("don't have null");
        return null;
    }
}
