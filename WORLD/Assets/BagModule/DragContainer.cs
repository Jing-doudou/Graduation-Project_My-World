using UnityEngine;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;

    public RectTransform rectTransform;
    public Vector2 offset;

    private ItemBase item;

    public ItemBase Item
    {
        get => item; set
        {
            item = value;
            if (item != null)
            {
                item.CurrentContainer = this;
            }
        }
    }
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        index = transform.GetSiblingIndex(); 
    }
    public void ChangeItem(Container other)
    {
        ItemBase _item = Item;
        Item = other.Item;
        other.Item = _item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item == null)
        {
            return;
        }
        offset = new Vector2(rectTransform.position.x, rectTransform.position.y) - eventData.position;
        Item.transform.SetSiblingIndex(-1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item == null)
        {
            return;
        }
        Item.rectTransform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Item == null)
        {
            return;
        }
        var target = eventData.pointerEnter;
        //背包UI外则丢弃物品
        if (target == null)
        {
            Item.DestroyItem();
            return;
        }
        Container targetContainer = target.GetComponent<Container>();
        if (targetContainer == null)
        {
            ChangeItem(this);
            return;
        }
        //目标容器为空时
        if (targetContainer.Item == null)
        {
            targetContainer.GetComponent<Container>().ChangeItem(this);
        }
        else
        {
            if (targetContainer == this)
            {
                ChangeItem(this);
                return;
            }
            if (targetContainer.Item.Id == Item.Id)
            {
                targetContainer.Item.Num += Item.Num;
                Item.DestroyItem();
                return;
            }
            else
            {
                ChangeItem(targetContainer);
                return;
            }
        }
    }
}
