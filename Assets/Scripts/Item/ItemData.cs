using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "道具", menuName = "SO/道具数据")]
public class ItemData : ScriptableObject
{
    [Header("道具名")]
    public string itemName;
    [Header("道具最大堆叠数量")]
    public int maxStack;
    [Header("道具价格")]
    public int cost;
    [Header("道具种类")]
    public ItemType itemType;
    [Header("道具稀有度")]
    public ItemRate itemRate;
    [Header("道具精灵图")]
    public Sprite itemSprite;
    [Header("文本描述")]
    [TextArea]
    public string describe;
}
