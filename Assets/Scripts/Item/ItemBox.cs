using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public Items items;
    public ItemUI ItemUI;
    public ItemBoxType type;
    [ContextMenu("刷新该格子UI")]
    public void RefreshUI()
    {
        if (ItemUI != null)
        {
            Destroy(ItemUI.gameObject);
        }
        ItemUI = null;

        if (HaveItem())
        {
            var data = Dic.Instance.FindItemDataByItemName(items.itemName);
            if ((data != null))
            {
                var ui = Instantiate(itemUIPrefab, BagUI.Instance.UIContainer.transform).GetComponent<ItemUI>();
                ItemUI = ui;
                ui.box = this;
                ui.transform.position = this.transform.position;
                ui.itemImage.sprite = data.itemSprite;
                ui.itemAmountText.text = items.amount.ToString();
            }
        }
    }

    public bool HaveItem()
    {
        if (string.IsNullOrEmpty(items.itemName) || items.amount <= 0)
        {
            return false;
        }
        return true;
    }
}
