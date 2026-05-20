using DG.Tweening;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : NetworkBehaviour, IPointerClickHandler
{
    public GameObject Entry;
    public readonly SyncList<Items> itemsList = new SyncList<Items>();
    public readonly SyncVar<bool> isLock = new SyncVar<bool>();
    public List<Items> _itemsList = new();

    public override void OnStartClient()
    {
        base.OnStartClient();
        itemsList.OnChange += ItemsList_OnChange;
        //itemsList.AddRange(settingList);
        PlayLoopScale_Sequence(Entry.transform);
    }

    private void ItemsList_OnChange(SyncListOperation op, int index, Items oldItem, Items newItem, bool asServer)
    {
        _itemsList.Clear();
        _itemsList.AddRange(itemsList);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientOpenRpc()
    {
        isLock.Value = true;
        //player.isSkip.Value = true;
        // transform.SetParent(room.Value.roomObject.Value.transform);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ClientCloseRpc()
    {
        isLock.Value = false;
        // player.isSkip.Value = false;
    }
    [Server]
    public void InitChest(List<Items> list)
    {
        if (IsServerStarted)
        {
            if (list.Count == 0)
            {
                Debug.Log("空宝藏");
                Despawn();
            }
        }
        //if (boss)
        //{
        //    isBoss.Value = true;//TODOboss箱子不共享
        //}
        if (list.Count > 0)
        {
        }
        Entry.gameObject.SetActive(true);
        itemsList.Clear();
        itemsList.AddRange(list);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        NManager.Instance.player.playerInstance.playerCon.rb.velocity = Vector2.zero;
        if (isLock.Value)
        {
            Debug.Log("锁住了");
        }
        else
        {
            ClientOpenRpc();
            BagUI.Instance.chest = this;
            BagUI.Instance.Open(BagType.Chest);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeItem(string itemName, int amount, int index)
    {
        Debug.Log("拿走了" + itemName + amount + "个");
        foreach (var item in itemsList)
        {
            if (item.index == index)
            {
                item.amount -= amount;
            }
            if (item.amount <= 0)
            {
                item.itemName = "";
            }
        }
        CheckEmpty();
    }
    public void CheckEmpty()
    {
        bool empty = true;
        foreach (var item in itemsList)
        {
            if (!(string.IsNullOrEmpty(item.itemName)) && item.amount > 0)
            {
                empty = false;
            }
        }
        if (empty)
        {
            Debug.Log("这个宝藏点空了,销毁");
            DisableThis();
        }
    }
    public void PlayLoopScale_Sequence(Transform target)
    {
        // target.localScale = Vector3.one * 0.34f;

        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(target.DOScale(1.3f, 0.5f));
        seq.Append(target.DOScale(1f, 0.5f));

        seq.SetEase(Ease.InOutSine);
        seq.SetLoops(-1);
        seq.SetLink(target.gameObject);
    }
    [ObserversRpc]
    public void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
