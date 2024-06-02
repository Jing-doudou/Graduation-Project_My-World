using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBase : MonoBehaviour
{
    public int Id;
    public Image image;
    private int num;
    public int Num
    {
        get => num; set
        {
            num = value;
            numText.text = value.ToString();
        }
    }

    public Container CurrentContainer
    {
        get => currentContainer;
        set
        {
            currentContainer = value;
            rectTransform.position = currentContainer.rectTransform.position;
        }
    }
    private Container currentContainer;
    public RectTransform rectTransform;
    public Text numText;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        Num = 1;
    }

    public void DestroyItem()
    {
        CurrentContainer.Item = null;
        Destroy(gameObject);
    }

}
