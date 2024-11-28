using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dictItemData", menuName = "Scriptable Object/dictItemData", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public string ItemName;
    [Multiline(4)]
    public string ItemDesciption;
    public float size;
}
