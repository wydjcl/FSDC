using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 需要保存的数据,包括一些方法
/// </summary>
public class SaveData : SingletonMono<SaveData>
{
    public PlayerData data;
    private void Start()
    {
        SaveManager.LoadOrCreate(0);
    }
    //#region 存储到背包
    //public int TakeFromList(List<items> bag, List<items> warehouse, string propname, int takeAmount)
    //{
    //    if (takeAmount <= 0) return 0;

    //    // 找配置
    //    string configProp = null;
    //    foreach (var item in warehouse)
    //    {
    //        if (item.propName == propname)
    //        {
    //            configProp = item.propName;
    //            break;
    //        }
    //    }

    //    if (configProp == null)
    //    {
    //        Debug.LogWarning("仓库没有这个道具");
    //        return 0;
    //    }

    //    // 👉 背包最多还能放多少
    //    int canTakeMax = BagMaxCapacity(configProp);
    //    if (canTakeMax <= 0)
    //    {
    //        Debug.Log("背包已满");
    //        return 0;
    //    }

    //    // 👉 实际能拿 = 想拿 vs 背包容量
    //    int targetTake = Mathf.Min(takeAmount, canTakeMax);

    //    int remain = targetTake;

    //    // ===== 从仓库扣 =====
    //    for (int i = warehouse.Count - 1; i >= 0; i--)
    //    {
    //        var item = warehouse[i];
    //        if (item == null || item.propName == null) continue;
    //        if (item.propName != propname) continue;

    //        int take = Mathf.Min(item.amount, remain);
    //        item.amount -= take;
    //        remain -= take;

    //        if (item.amount <= 0)
    //            warehouse.RemoveAt(i);

    //        if (remain <= 0)
    //            break;
    //    }

    //    int actuallyTaken = targetTake - remain;

    //    if (actuallyTaken <= 0)
    //    {
    //        Debug.Log("仓库数量不够");
    //        return 0;
    //    }

    //    // 👉 加入背包
    //    AddToBag(bag, configProp, actuallyTaken);

    //    return actuallyTaken;
    //}
    //public int AddToBag(List<items> bag, string propName, int amount)
    //{
    //    int maxStack = Dic.Instance.GetPropData(propName).maxStack;
    //    int remain = amount;

    //    // ===== 1. 填已有堆 =====
    //    foreach (var item in bag)
    //    {
    //        if (item.propName != propName) continue;

    //        int space = maxStack - item.amount;
    //        if (space <= 0) continue;

    //        int add = Mathf.Min(space, remain);
    //        item.amount += add;
    //        remain -= add;

    //        if (remain <= 0)
    //            return amount;
    //    }

    //    // ===== 2. 新开格子（要检查格子上限）=====
    //    while (remain > 0)
    //    {
    //        if (bag.Count >= data.bagCount)
    //        {
    //            // 背包满了，放不下了
    //            break;
    //        }

    //        int add = Mathf.Min(maxStack, remain);

    //        bag.Add(new items
    //        {
    //            propName = propName,
    //            amount = add
    //        });

    //        remain -= add;
    //    }

    //    // 👉 返回实际放入数量
    //    return amount - remain;
    //}

    ///// <summary>
    ///// 背包对应道具最大可放入数量
    ///// </summary>
    ///// <param name="prop"></param>
    ///// <returns></returns>
    //public int BagMaxCapacity(string propName)
    //{
    //    int max = 0;
    //    max += (data.bagCount - data.bag.Count) * Dic.Instance.GetPropData(propName).maxStack;//先加空格子的数量乘上可堆叠数量
    //    foreach (var item in data.bag)
    //    {
    //        if (item.propName == propName)
    //        {
    //            max += Dic.Instance.GetPropData(propName).maxStack - item.amount;//再加上已经有的同名道具格子剩余的数量
    //        }
    //    }
    //    return max;
    //}

    //#endregion
    //#region 存储到仓库
    //public int StoreToWarehouse(List<items> bag, List<items> warehouse, string propname, int amount)
    //{
    //    if (amount <= 0) return 0;

    //    int remain = amount;
    //    string configProp = null;

    //    // ===== 1. 从背包扣 =====
    //    for (int i = bag.Count - 1; i >= 0; i--)
    //    {
    //        var item = bag[i];
    //        //if (item == null || item.prop == null) continue;
    //        if (item.propName != propname) continue;

    //        if (configProp == null)
    //            configProp = item.propName;

    //        int take = Mathf.Min(item.amount, remain);
    //        item.amount -= take;
    //        remain -= take;

    //        if (item.amount <= 0)
    //            bag.RemoveAt(i);

    //        if (remain <= 0)
    //            break;
    //    }

    //    int actuallyTaken = amount - remain;

    //    if (actuallyTaken <= 0)
    //    {
    //        Debug.Log("背包没有这个道具或数量不足");
    //        return 0;
    //    }

    //    // ===== 2. 加入仓库（自动堆叠）=====
    //    AddToWarehouse(warehouse, configProp, actuallyTaken);

    //    return actuallyTaken;
    //}
    //public void AddToWarehouse(List<items> warehouse, string propName, int amount)
    //{
    //    int maxStack = Dic.Instance.GetPropData(propName).maxStack;
    //    int remain = amount;

    //    // 👉 先填已有堆
    //    foreach (var item in warehouse)
    //    {
    //        if (item.propName != propName) continue;

    //        int space = maxStack - item.amount;
    //        if (space <= 0) continue;

    //        int add = Mathf.Min(space, remain);
    //        item.amount += add;
    //        remain -= add;

    //        if (remain <= 0)
    //            return;
    //    }

    //    // 👉 再新开（仓库无限，不用判断格子数）
    //    while (remain > 0)
    //    {
    //        int add = Mathf.Min(maxStack, remain);

    //        warehouse.Add(new items
    //        {
    //            propName = propName,
    //            amount = add
    //        });

    //        remain -= add;
    //    }
    //}

    //#endregion

    //#region  使用道具
    //public void UseProp(string propName)
    //{
    //    for (int i = 0; i < data.bag.Count; i++)
    //    {
    //        if (data.bag[i].propName == propName)
    //        {
    //            data.bag[i].amount -= 1;
    //            if (data.bag[i].amount <= 0)
    //            {
    //                data.bag.RemoveAt(i);
    //            }
    //            Sort(data.bag);
    //            return;
    //        }
    //    }
    //}


    //#endregion
    ///// <summary>
    ///// 整理
    ///// </summary>
    ///// <param name="bag"></param>
    ///// <returns></returns>
    //public List<items> Sort(List<items> bag)
    //{
    //    if (bag.Count == 0)
    //    {
    //        return bag;
    //    }
    //    // 先按道具类型分组
    //    Dictionary<string, int> totalMap = new Dictionary<string, int>();//字典为 名字和数量

    //    foreach (var item in bag)
    //    {
    //        if (item == null || item.propName == null) continue;

    //        if (!totalMap.ContainsKey(item.propName))
    //            totalMap[item.propName] = 0;

    //        totalMap[item.propName] += item.amount;
    //    }

    //    // 重新生成背包
    //    List<items> newBag = new List<items>();

    //    foreach (var kv in totalMap)
    //    {
    //        string propName = Dic.Instance.GetPropData(kv.Key).propName;
    //        int totalAmount = kv.Value;
    //        int maxStack = Dic.Instance.GetPropData(propName).maxStack;

    //        // 拆分堆叠
    //        while (totalAmount > 0)
    //        {
    //            int take = Mathf.Min(maxStack, totalAmount);

    //            newBag.Add(new items
    //            {
    //                propName = propName,
    //                amount = take
    //            });

    //            totalAmount -= take;
    //        }
    //    }

    //    return newBag;
    //}

    //public void SortAll()
    //{
    //    data.warehouse = Sort(data.warehouse);
    //    data.bag = Sort(data.bag);
    //    //SaveManager.Instance.SaveTest();
    //}

    //public void Swapitems(List<items> aList, int aIndex, List<items> bList, int bIndex)
    //{
    //    // 安全检查
    //    if (aIndex < 0 || aIndex >= aList.Count) return;
    //    if (bIndex < 0 || bIndex >= bList.Count) return;

    //    // 临时变量交换
    //    items temp = aList[aIndex];
    //    aList[aIndex] = bList[bIndex];
    //    bList[bIndex] = temp;
    //}


    //public void ConvertData()
    //{
    //    data.bag.Clear();
    //    data.warehouse.Clear();
    //    foreach (PropBox box in BagUI.Instance.bagBoxes)
    //    {
    //        items newitems = new items();
    //        newitems.propName = box.items.propName;
    //        newitems.amount = box.items.amount;
    //        newitems.index = box.index;
    //        data.bag.Add(newitems);
    //    }
    //    foreach (PropBox box in BagUI.Instance.wareHouseBoxes)
    //    {
    //        items newitems = new items();
    //        newitems.propName = box.items.propName;
    //        newitems.amount = box.items.amount;
    //        newitems.index = box.index;
    //        data.warehouse.Add(newitems);
    //    }
    //}


    //[ContextMenu("测试整理仓库")]
    //public void Test()
    //{
    //    data.bag = Sort(data.bag);
    //    data.warehouse = Sort(data.warehouse);
    //    Test2();
    //}
    //[ContextMenu("输出数据")]
    //public void Test2()
    //{
    //    if (data.bag == null)
    //    {
    //        Debug.Log("找不到背包");
    //    }
    //    if (data.warehouse == null)
    //    {
    //        Debug.Log("找不到仓库");
    //    }
    //    if (data.bag.Count == 0)
    //    {
    //        Debug.Log("背包为空");
    //    }
    //    if (data.warehouse.Count == 0)
    //    {
    //        Debug.Log("仓库为空");
    //    }
    //}
}
