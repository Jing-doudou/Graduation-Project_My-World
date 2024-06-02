using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]    
public class Player : MonoBehaviour
{
    public PlayerDate playerDate;
    public Text text;
    public void Init(string name)
    {
        text.text = name;
    }
}
[Serializable]
public class PlayerDate
{
    public string Id;
    public string Pw;
}
