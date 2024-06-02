using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Load面板 : MonoBehaviour
{
    public Slider loadSlider;
    public Action endAction;
    [SerializeField]
    private float currentFinishExtent;
    public float finishMax;
    public float CurrentFinishExtent
    {
        get => currentFinishExtent; set
        {
            currentFinishExtent = value;
        }
    }
    private void FixedUpdate()
    {
        loadSlider.value = Vector2.Lerp(new Vector2(loadSlider.value, 0), new Vector2(CurrentFinishExtent / finishMax, 0), 0.3f).x;
        if (Math.Abs(loadSlider.value - 1) < 0.01f)
        {
            Invoke(nameof(EnadLoad), 1f);
        }
    }
    private void EnadLoad()
    {
        endAction.Invoke();
    }
    public void Init(float value, Action action)
    { 
        endAction = null;
        finishMax = value;
        CurrentFinishExtent = 0;
        endAction += delegate ()
        {
            CurrentFinishExtent = 0;
            gameObject.SetActive(false);
        };
        endAction += action;
        gameObject.SetActive(true); 
    }
}
