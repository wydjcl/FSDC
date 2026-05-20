using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : SingletonMono<BagUI>
{
    public GameObject root;
    public BagType bagType;

    public GameObject UIContainer;
    [Header("BOXUI")]
    public GameObject back;
    public GameObject warehouseObj;
    public GameObject chestObj;

    public List<ItemBox> bagBoxes = new List<ItemBox>();
    public List<ItemBox> warehouseBoxes = new List<ItemBox>();
    public List<ItemBox> chestBoxes = new List<ItemBox>();
    public Chest chest;
    [Header("提示框")]
    public GameObject des;
    public Image desImage;
    public TextMeshProUGUI desText;
    public TextMeshProUGUI desCostText;
    public TextMeshProUGUI desNameText;
    public void Start()
    {
        root.SetActive(false);
        des.SetActive(false);
        for (int i = 0; i < bagBoxes.Count; i++)
        {
            bagBoxes[i].items.index = i;
            bagBoxes[i].type = ItemBoxType.bag;
        }
        for (int i = 0; i < warehouseBoxes.Count; i++)
        {
            warehouseBoxes[i].items.index = i;
            warehouseBoxes[i].type = ItemBoxType.warehouse;
        }
        for (int i = 0; i < chestBoxes.Count; i++)
        {
            chestBoxes[i].items.index = i;
            chestBoxes[i].type = ItemBoxType.chest;
        }
        LoadData();
    }
    public void Open(BagType type)
    {
        bagType = type;
        root.SetActive(true);
        if (type == BagType.Warehouse)
        {
            InitBagBoxes();
            InitWarehouseBoxes();
        }
        if (type == BagType.Battle)
        {
            back.SetActive(true);
            InitBagBoxes();
        }
        if (type == BagType.Chest)
        {
            back.SetActive(true);
            InitBagBoxes();
            InitChestBoxes(chest);
        }
    }

    public void Close()
    {
        DiscardContainer();
        root.SetActive(false);
        back.SetActive(false);
        warehouseObj.SetActive(false);
        chestObj.SetActive(false);
        if (chest != null)
        {
            chest.ClientCloseRpc();
        }
        else
        {
            Debug.Log("宝箱不存在");
        }
        chest = null;
    }

    public void InitBagBoxes()
    {
        foreach (ItemBox box in bagBoxes)
        {
            box.RefreshUI();
        }
    }

    public void InitWarehouseBoxes()
    {
        warehouseObj.SetActive(true);
        foreach (ItemBox box in warehouseBoxes)
        {
            box.RefreshUI();
        }
    }
    public void InitChestBoxes(Chest c)
    {
        chestObj.SetActive(true);

        foreach (var chestItems in c.itemsList)
        {
            var box = FindBoxByIndex(chestBoxes, chestItems.index);
            box.items.itemName = chestItems.itemName;
            box.items.amount = chestItems.amount;
            box.RefreshUI();
        }
    }
    public void LoadData()
    {
        foreach (Items items in SaveData.Instance.data.bag)
        {
            var box = FindBoxByIndex(bagBoxes, items.index);
            if (box != null)
            {
                box.items.itemName = items.itemName;
                box.items.amount = items.amount;
                // box.RefreshUI();
            }
        }
        foreach (Items items in SaveData.Instance.data.warehouse)
        {
            var box = FindBoxByIndex(warehouseBoxes, items.index);
            if (box != null)
            {
                box.items.itemName = items.itemName;
                box.items.amount = items.amount;
                // box.RefreshUI();
            }
        }
    }
    public ItemBox FindBoxByIndex(List<ItemBox> list, int index)
    {
        foreach (ItemBox box in list)
        {
            if (box.items.index == index)
            {
                return box;
            }
        }
        Debug.LogWarning("未能通过Index找到对应Box!!!");
        return null;
    }

    public int DisassembleProp(List<ItemBox> boxes, string itemName, int i)//拆解
    {
        int amount = i;
        if (i == 0)
        {
            return amount;
        }
        foreach (var box in boxes)
        {
            if (!box.HaveItem())
            {
                box.items.itemName = itemName;
                box.items.amount = amount;
                amount = 0;
                box.RefreshUI();
                break;
            }
        }
        return amount;
    }
    public int PutInBox(List<ItemBox> boxes, ItemUI itemUI)
    {
        string itemName = itemUI.box.items.itemName;
        int amount = itemUI.box.items.amount;
        foreach (var box in boxes)
        {
            if (string.IsNullOrEmpty(box.items.itemName))
            {
                box.items.itemName = itemName;
                box.items.amount = amount;
                amount = 0;
            }
            else if (box.items.itemName == itemName)
            {
                int empty = Dic.Instance.FindItemDataByItemName(itemName).maxStack - box.items.amount;
                if (empty >= amount)
                {
                    box.items.amount += amount;
                    amount = 0;
                }
                else
                {
                    amount -= empty;
                    box.items.amount = Dic.Instance.FindItemDataByItemName(itemName).maxStack;
                }

            }
            box.RefreshUI();
            if (amount == 0)
            {
                return amount;
            }
        }

        return amount;
    }
    public int PutOneInBox(List<ItemBox> boxes, ItemUI itemUI)
    {
        string itemName = itemUI.box.items.itemName;
        int amount = 1;
        foreach (var box in boxes)
        {
            if (string.IsNullOrEmpty(box.items.itemName))
            {
                box.items.itemName = itemName;
                box.items.amount = amount;
                amount = 0;
            }
            else if (box.items.itemName == itemName)
            {
                int empty = Dic.Instance.FindItemDataByItemName(itemName).maxStack - box.items.amount;
                if (empty >= amount)
                {
                    box.items.amount += amount;
                    amount = 0;
                }
                else
                {
                    amount -= empty;
                    box.items.amount = Dic.Instance.FindItemDataByItemName(itemName).maxStack;
                }

            }
            box.RefreshUI();
            if (amount == 0)
            {
                return amount;
            }
        }

        return amount;
    }
    public void DiscardContainer()
    {

        for (int i = UIContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(UIContainer.transform.GetChild(i).gameObject);
        }
    }
}
