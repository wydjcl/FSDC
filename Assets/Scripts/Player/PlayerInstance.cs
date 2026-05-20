using Cinemachine;
using DG.Tweening;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
public class PlayerInstance : Character
{
    public Player player;
    public PlayerCon playerCon;
    public CardLayout cardLayout;

    public readonly SyncVar<string> playerName = new SyncVar<string>();
    public readonly SyncVar<int> characterID = new SyncVar<int>();
    public readonly SyncVar<bool> isGo = new SyncVar<bool>();//是否开始游戏了,开始游戏后初始场景UI就会关闭,或者开始游戏后可以使用道具


    public readonly SyncVar<Vector2Int> playerPos = new SyncVar<Vector2Int>();
    public readonly SyncVar<bool> isExit = new SyncVar<bool>();

    //战斗数据
    public readonly SyncVar<int> cost = new SyncVar<int>();
    public readonly SyncVar<int> maxCost = new SyncVar<int>();

    public readonly SyncList<string> cardNameList = new SyncList<string>();//初始化的卡组
    public List<Card> handDeck = new List<Card>();
    public List<Card> drawDeck = new List<Card>();
    public List<Card> discardDeck = new List<Card>();
    public List<Card> removeDeck = new List<Card>();

    public GameObject healthBarPrefab;
    public TextMeshProUGUI mapPosText;


    private GameObject mainUI;
    public readonly SyncVar<Room> currentRoom = new SyncVar<Room>();
    public Room _currentRoom;

    public override void OnStartClient()
    {
        base.OnStartClient();
        cardLayout = GameManager.Instance.cardLayout;
        foreach (var p in NManager.Instance.players)
        {
            if (p.Owner == this.Owner)
            {
                player = p;
                p.playerInstance = this;
                playerCon.player = p;
            }
        }
        currentRoom.OnChange += CurrentRoom_OnChange;

        if (IsOwner)
        {
            CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Map;
            CameraManager.Instance.cameraFollowTarget.player = this.transform;
        }
    }

    private void CurrentRoom_OnChange(Room prev, Room next, bool asServer)
    {
        _currentRoom = next;
        //if (asServer)
        //{
        //    if (IsOwner)
        //    {
        //        next.roomImage.color = Color.blue;

        //        if (prev != null)
        //        {
        //            prev.roomImage.color = Color.white;

        //        }
        //    }
        //}
        //else
        //{
        //    if (!IsServerStarted)
        //    {
        //        if (IsOwner)
        //        {
        //            next.roomImage.color = Color.blue;

        //            if (prev != null)
        //            {
        //                prev.roomImage.color = Color.white;

        //            }
        //        }
        //    }
        //}
        if (IsOwner)
        {
            //Canvas.ForceUpdateCanvases();
            //if (player.playerInstance.currentRoom.Value == null)
            //{
            //    return;
            //}
            //var target = player.playerInstance.currentRoom.Value.GetComponent<RectTransform>();
            //if ((target == null))
            //{
            //    return;
            //}
            //var content = MapManager.Instance.content.GetComponent<RectTransform>();
            ////var viewport = MapManager.Instance.viewport.GetComponent<RectTransform>();
            //content.anchoredPosition = -target.anchoredPosition;
        }


    }
    #region 卡牌
    [ServerRpc(RequireOwnership = false)]
    public void AddCard(string cardName)
    {
        cardNameList.Add(cardName);
        Debug.Log($"{cardName}已经添加到玩家{this.playerName.Value}");
    }
    public void CreateCard()
    {
        var cardZone = GameObject.FindGameObjectWithTag("CardZone");

        for (int i = cardZone.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardZone.transform.GetChild(i).gameObject);
        }
        handDeck.Clear();
        discardDeck.Clear();
        drawDeck.Clear();
        removeDeck.Clear();


        foreach (var cardName in cardNameList)
        {
            foreach (var so in Dic.Instance.cards)
            {
                if (cardName == so.cardName)
                {
                    //var card = Instantiate(Dic.Instance.cardPrefab).GetComponent<Card>();
                    //card.InitCard(so);
                    //card.gameObject.SetActive(false);
                    //drawDeck.Add(card);
                }
            }
        }

        ShuffleDrawDeck();
    }

    /// <summary>
    /// 为该连接玩家增加单张卡片,i为0的时候弃牌堆,i为1的时候抽牌堆,i为2的时候除外堆
    /// </summary>
    /// <param name="conn"></param>
    [TargetRpc]
    public void CreateOneCard(NetworkConnection conn, string cardName, int i)
    {
        //Debug.Log("只有这个玩家执行了方法！");
        //// 这里写客户端逻辑，比如播放动画、显示 UI
        //if (i == 0)
        //{
        //    Debug.Log("在弃牌堆插入");
        //    var cardP = Instantiate(cardPrefab, cardLayout.transform);
        //    cardP.SetActive(false);
        //    Card card = cardP.GetComponent<Card>();
        //    var so = Dic.Instance.FindCard(cardName);
        //    card.InitCard(so);
        //    discardDeck.Add(card);
        //}
        //if (i == 1)
        //{
        //    Debug.Log("在抽牌堆插入");
        //}
        //if (i == 2)
        //{
        //    Debug.Log("在除外堆插入");
        //}
    }


    public void DestroyCard()
    {
        var cardZone = GameObject.FindGameObjectWithTag("CardZone");

        for (int i = cardZone.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardZone.transform.GetChild(i).gameObject);
        }
        //TODO删除实体
        handDeck.Clear();
        discardDeck.Clear();
        drawDeck.Clear();
        removeDeck.Clear();
    }
    [ContextMenu("抽5牌")]
    public void DrawFive()
    {
        DrawCard(5);
    }
    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (drawDeck.Count == 0)
            {
                // 弃牌堆也空 → 没牌可抽，直接停止
                if (discardDeck.Count == 0)
                {
                    //Debug.Log("抽牌堆和弃牌堆都空了，无法继续抽牌");
                    break;
                }
                //Debug.Log("抽牌堆为空,洗牌");
                // 洗回抽牌堆
                ShuffleDiscardIntoDraw();
            }
            // 洗牌后仍然空 → 安全退出
            if (drawDeck.Count == 0)
            {
                //Debug.Log("洗牌后抽牌堆仍为空");
                break;
            }
            // 现在抽牌堆一定有牌，抽一张
            Card card = drawDeck[0];
            drawDeck.RemoveAt(0);
            handDeck.Add(card);
            card.gameObject.SetActive(true);
            card.transform.position = new Vector3(0, 0, 0);
            card.isAni = true;
            //var delay = i * 0.1f;
        }
        SetCardLayout(0);
    }
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handDeck.Count; i++)
        {
            var currentCard = handDeck[i];
            currentCard.transform.DOKill();
        }//删去所有卡牌的动画
        for (int i = 0; i < handDeck.Count; i++)
        {
            var currentCard = handDeck[i];

            CardTransForm cardTransForm = cardLayout.GetCardTransForm(i, handDeck.Count);
            currentCard.transform.DOScale(Vector3.one, 0.05f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransForm.pos, 0.1f).onComplete = () =>
                {
                    currentCard.isAni = false;
                };
            };
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.orSortingOrder = i;
            currentCard.UpdatePosRot(cardTransForm.pos, cardTransForm.rotation);
        }
    }
    public void ShuffleDiscardIntoDraw()
    {
        drawDeck.AddRange(discardDeck);
        discardDeck.Clear();
        ShuffleDrawDeck();
    }
    /// <summary>
    /// 打乱抽牌堆的顺序（Fisher–Yates 洗牌）
    /// </summary>
    public void ShuffleDrawDeck()
    {
        if (drawDeck.Count <= 1)
            return;
        for (int i = 0; i < drawDeck.Count; i++)
        {
            int rand = Random.Range(i, drawDeck.Count);
            (drawDeck[i], drawDeck[rand]) = (drawDeck[rand], drawDeck[i]);
        }
    }
    public void DiscardCard(Card card)
    {
        discardDeck.Add(card);
        handDeck.Remove(card);
        card.gameObject.SetActive(false);
        SetCardLayout(0f);
    }

    public void RemoveCard(Card card)
    {
        removeDeck.Add(card);
        handDeck.Remove(card);
        card.gameObject.SetActive(false);
        SetCardLayout(0f);
    }

    public void DiscardAllCards()
    {
        // 倒序遍历手牌堆
        for (int i = handDeck.Count - 1; i >= 0; i--)
        {
            Card card = handDeck[i];

            // 移动到弃牌堆
            discardDeck.Add(card);

            // 从手牌堆移除
            handDeck.RemoveAt(i);

            // 隐藏卡牌
            card.gameObject.SetActive(false);
        }

        // 更新手牌布局
        SetCardLayout(0f);
    }
    #endregion

    [ServerRpc(RequireOwnership = false)]
    public void InitDeck()
    {
        if (characterID.Value == 0)
        {
            cardNameList.Clear();
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");
            cardNameList.Add("给你一拳");

            //cardNameList.Add("基础魔法屏障");
            //cardNameList.Add("基础魔法屏障");
            //cardNameList.Add("基础魔法屏障");
            //cardNameList.Add("基础魔法屏障");


            //cardNameList.Add("发现宝箱");

            //cardNameList.Add("发现宝箱");
            //cardNameList.Add("发现宝箱");
            //cardNameList.Add("发现宝箱");

            //cardNameList.Add("战争怒吼");
            //cardNameList.Add("旋风斩");
            //cardNameList.Add("我身为盾");
            //cardNameList.Add("临阵磨剑");
            //cardNameList.Add("二连斩");
            //cardNameList.Add("护盾猛击");
            //cardNameList.Add("荣耀重击");
            //cardNameList.Add("誓约胜利之剑!");
            //cardNameList.Add("骑士斩");

        }
        if (characterID.Value == 1)
        {
            cardNameList.Clear();
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
            cardNameList.Add("发现宝箱");
        }
    }
    [ContextMenu("测试生成卡牌抽牌")]
    public void Test()
    {
        InitDeck();
        CreateCard();
        DrawCard(5);
    }

}
