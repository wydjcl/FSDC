using GameKit.Dependencies.Utilities.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 调用的静态数据字典
/// </summary>
public class Dic : SingletonMono<Dic>
{

    public List<ItemData> itemDatas = new List<ItemData>();
    public List<CardDataSO> cards = new List<CardDataSO>();
    public ItemData FindItemDataByItemName(string s)
    {
        foreach (ItemData item in itemDatas)
        {
            if (item.itemName == s)
            {
                return item;
            }
        }
        Debug.LogWarning("没找到对应道具Data!!!道具名:" + s);
        return null;
    }

    public ItemType FindItemTypeByItemName(string s)
    {
        foreach (ItemData item in itemDatas)
        {
            if (item.itemName == s)
            {
                return item.itemType;
            }
        }
        Debug.LogWarning("没找到对应道具Data!!!道具名:" + s);
        return ItemType.Normal;
    }


}
