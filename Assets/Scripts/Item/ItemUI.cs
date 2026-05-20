using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.MaterialProperty;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("需导入UI")]
    public Image itemImage;
    public Image backImage;
    public TextMeshProUGUI itemAmountText;
    public GameObject Entry;

    [Header("拖动数据")]
    public ItemBox box;
    private Vector2 dragOffset;
    public ItemBox targetBox;
    // [Header("道具数据")]
    //public Items items;

    //public bool isEquip;//是装备
    //public bool isUseful;//是消耗品

    public bool isEnter;
    public bool isDrag;
    private void Start()
    {
        ItemRate r = Dic.Instance.FindItemDataByItemName(box.items.itemName).itemRate;
        switch (r)
        {
            case ItemRate.white:
                backImage.color = Color.white;
                break;
            case ItemRate.green:
                backImage.color = Color.green;
                break;
            case ItemRate.blue:
                backImage.color = new Color(0f, 1f, 1f);
                break;
            case ItemRate.purple:
                backImage.color = new Color(0.5f, 0, 0.5f);
                break;
            case ItemRate.gold:
                backImage.color = new Color(1, 0.84f, 0);
                break;
            case ItemRate.red:
                backImage.color = new Color(0.70f, 0.05f, 0.05f);
                break;
        }

    }
    private void Update()
    {
        if (isEnter)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // E();
            }
        }
    }
    public void ClickLeft()
    {
        if (BagUI.Instance.bagType == BagType.Warehouse)
        {
            if (box.type == ItemBoxType.bag)
            {
                //if (BagUI.Instance.isBattle)
                //{
                //    return;
                //}
                box.items.amount = BagUI.Instance.PutInBox(BagUI.Instance.warehouseBoxes, this);
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
            if (box.type == ItemBoxType.warehouse)
            {
                //if (BagUI.Instance.isBattle)
                //{
                //    return;
                //}
                box.items.amount = BagUI.Instance.PutInBox(BagUI.Instance.bagBoxes, this);
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
        }
        if (BagUI.Instance.bagType == BagType.Chest)
        {
            if (box.type == ItemBoxType.chest)
            {
                int oramount = box.items.amount;
                box.items.amount = BagUI.Instance.PutInBox(BagUI.Instance.bagBoxes, this);
                BagUI.Instance.chest.TakeItem(box.items.itemName, oramount - box.items.amount, box.items.index);
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
        }
        box.RefreshUI();
    }

    public void ClickRight()
    {
        if (BagUI.Instance.bagType == BagType.Warehouse)
        {
            if (box.type == ItemBoxType.bag)
            {
                //if (BagUI.Instance.isBattle)
                //{
                //    return;
                //}
                box.items.amount -= (1 - BagUI.Instance.PutOneInBox(BagUI.Instance.warehouseBoxes, this));
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
            if (box.type == ItemBoxType.warehouse)
            {
                //if (BagUI.Instance.isBattle)
                //{
                //    return;
                //}
                box.items.amount -= (1 - BagUI.Instance.PutOneInBox(BagUI.Instance.bagBoxes, this));
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
        }
        if (BagUI.Instance.bagType == BagType.Chest)
        {
            if (box.type == ItemBoxType.chest)
            {
                int oramount = box.items.amount;
                box.items.amount -= (1 - BagUI.Instance.PutOneInBox(BagUI.Instance.bagBoxes, this));
                BagUI.Instance.chest.TakeItem(box.items.itemName, oramount - box.items.amount, box.items.index);
                if (box.items.amount == 0)//全被放入
                {
                    box.items.itemName = "";
                }
            }
        }
        box.RefreshUI();
    }
    public void ClickMid()
    {
        //if (BagUI.Instance.isWareHouse)
        //{
        if (box.type == ItemBoxType.bag)
        {
            //if (NManager.Instance.player.playerInstance.currentRoom.Value.isBattle.Value)
            //{
            //    return;
            //}
            int am = box.items.amount / 2;
            box.items.amount = box.items.amount - am + BagUI.Instance.DisassembleProp(BagUI.Instance.bagBoxes, box.items.itemName, am);
            if (box.items.amount == 0)//全被放入
            {
                box.items.itemName = "";
            }
        }
        if (box.type == ItemBoxType.warehouse)
        {
            //if (BagUI.Instance.isBattle)
            //{
            //    return;
            //}
            int am = box.items.amount / 2;
            box.items.amount = box.items.amount - am + BagUI.Instance.DisassembleProp(BagUI.Instance.warehouseBoxes, box.items.itemName, am);

            if (box.items.amount == 0)//全被放入
            {
                box.items.itemName = "";
            }
        }
        // }

        box.RefreshUI();
    }
    //public void E()//消耗品或装备E键盘使用
    //{
    //    if (NManager.Instance.player.playerInstance.currentRoom.Value.isBattle.Value)
    //    {
    //        Debug.Log("战斗中无法使用");
    //        return;
    //    }
    //    if (BagUI.Instance.isBattle)
    //    {
    //        if (Dic.Instance.GetItemType(items.itemName) == ItemType.Consumable)
    //        {
    //            Debug.Log("使用消耗品");
    //            foreach (var e in Dic.Instance.GetItemData(items.itemName).effects)
    //            {
    //                //e.ApplyEffect(NManager.Instance.player);
    //            }
    //            box.items.amount -= 1;
    //            if (box.items.amount == 0)
    //            {
    //                box.items.itemName = "";
    //            }
    //        }
    //        else if (Dic.Instance.GetItemType(items.itemName) == ItemType.Equipment)
    //        {
    //            Debug.Log("装备");
    //        }
    //    }
    //    box.RefreshUI();
    //}
    #region 物理
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        //// 开始拖拽时，计算鼠标和图片的偏移
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        dragOffset = (Vector2)transform.localPosition - localPoint;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("拖动中");
        //// 核心：让UI跟随鼠标移动
        RectTransform rect = Entry.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            Input.mousePosition,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        // 应用偏移，让拖拽更自然
        rect.localPosition = localPoint + dragOffset;



        targetBox = null;

        // 1. 创建一个射线事件（从鼠标位置发射，专门找UI）
        PointerEventData clickData = new PointerEventData(EventSystem.current);
        clickData.position = Input.mousePosition;

        // 2. 存储射线碰到的所有UI
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(clickData, results);

        // 3. 遍历找到第一个标签为 ItemBox 的格子
        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("ItemBox"))
            {
                targetBox = result.gameObject.GetComponent<ItemBox>();
                break; // 找到就停
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        if (targetBox != null)
        {
            if (box.type == ItemBoxType.warehouse && (targetBox.type == ItemBoxType.warehouse || targetBox.type == ItemBoxType.bag))
            {
                PutInNewBox(box, targetBox);
            }
            else if (box.type == ItemBoxType.bag && (targetBox.type == ItemBoxType.warehouse || targetBox.type == ItemBoxType.bag))
            {
                PutInNewBox(box, targetBox);
            }
            else if (box.type == ItemBoxType.chest && targetBox.type == ItemBoxType.bag)
            {
                GetChest(box, targetBox);
            }
        }

        targetBox = null;
        box.RefreshUI();

        //transform.position = box.transform.position;
        //items.index = box.index;
        ////transform.SetParent(box.transform, false);
        //target = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag)
        {
            box.RefreshUI();
            Debug.Log("拖动中不能点击");
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Debug.Log("Ctrl + 左键点击");
            }
            else
            {
                ClickLeft();
                Debug.Log("左键点击");
                BagUI.Instance.des.SetActive(false);
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("右键点击");
            BagUI.Instance.des.SetActive(false);
            ClickRight();
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("中键点击！");
            ClickMid();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        BagUI.Instance.des.SetActive(true);
        BagUI.Instance.desImage.sprite = Dic.Instance.FindItemDataByItemName(box.items.itemName).itemSprite;
        BagUI.Instance.desText.text = Dic.Instance.FindItemDataByItemName(box.items.itemName).describe;
        BagUI.Instance.desNameText.text = box.items.itemName;
        BagUI.Instance.desCostText.text = "$" + Dic.Instance.FindItemDataByItemName(box.items.itemName).cost.ToString();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        BagUI.Instance.des.SetActive(false);
    }
    #endregion

    public void ChangeTwoBoxesItem(ItemBox caster, ItemBox target)
    {
        Items tItems = new();
        tItems.itemName = caster.items.itemName;
        tItems.amount = caster.items.amount;

        caster.items.itemName = target.items.itemName;
        caster.items.amount = target.items.amount;

        target.items.itemName = tItems.itemName;
        target.items.amount = tItems.amount;

        caster.RefreshUI();
        target.RefreshUI();
    }

    public void PutInNewBox(ItemBox caster, ItemBox target)
    {
        if (caster.items.itemName == target.items.itemName)
        {
            Debug.Log("相同名字的两个格子");
            int empty = Dic.Instance.FindItemDataByItemName(target.items.itemName).maxStack - target.items.amount;
            if (empty > 0)
            {
                Debug.Log("还有空位");
                if (caster.items.amount > empty)
                {
                    caster.items.amount -= empty;
                    target.items.amount += empty;
                }
                else
                {
                    target.items.amount += caster.items.amount;
                    caster.items.amount = 0;
                    caster.items.itemName = "";
                }
                target.RefreshUI();
                caster.RefreshUI();
            }
            else
            {
                Debug.Log("没有空位");
            }
        }
        else
        {
            Debug.Log("不同名字的两个格子");
            ChangeTwoBoxesItem(caster, target);
        }
    }
    public void GetChest(ItemBox caster, ItemBox target)
    {
        if (caster.items.itemName == target.items.itemName)
        {
            Debug.Log("相同名字的两个格子");
            int empty = Dic.Instance.FindItemDataByItemName(target.items.itemName).maxStack - target.items.amount;
            if (empty > 0)
            {
                Debug.Log("还有空位");
                if (caster.items.amount > empty)
                {
                    BagUI.Instance.chest.TakeItem(caster.items.itemName, empty, caster.items.index);
                    caster.items.amount -= empty;
                    target.items.amount += empty;
                }
                else
                {
                    BagUI.Instance.chest.TakeItem(caster.items.itemName, caster.items.amount, caster.items.index);
                    target.items.amount += caster.items.amount;
                    caster.items.amount = 0;
                    caster.items.itemName = "";
                }
                target.RefreshUI();
                caster.RefreshUI();
            }
            else
            {
                Debug.Log("没有空位");
            }
        }
        else if (string.IsNullOrEmpty(target.items.itemName))
        {
            {
                Debug.Log("放进空格子格子");
                BagUI.Instance.chest.TakeItem(caster.items.itemName, caster.items.amount, caster.items.index);
                ChangeTwoBoxesItem(caster, target);
            }
        }
    }
}
