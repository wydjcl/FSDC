using DG.Tweening;
using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("UI层")]
    public GameObject Entry;
    public SpriteRenderer cardSprite;
    public TextMeshPro cardNameText;
    public TextMeshPro cardCostText;
    public TextMeshPro cardDesText;
    [Header("数据层")]
    public bool isAni = true;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int orSortingOrder;
    public int originaLayerOrder;//原始叠层排序
    public CardDataSO dataSO;
    [Header("数据层")]
    // public NetworkPlayer player;
    public int cardID;
    public string cardName;
    public int cardCost;
    public CardType cardType;
    public List<CardEffectSO> cardEffectSOs;
    public bool isMagic;

    private void Awake()
    {
        transform.SetParent(GameObject.FindWithTag("CardZone").transform, false);
    }
    private void Update()
    {
        // cardDesText.text = RichTextHelper.ReplaceValues(dataSO.cardDes, GameManager.Instance.player.Attack());
    }
    public void InitCard(CardDataSO so)
    {
        cardName = so.cardName;
        cardCost = so.cardCost;
        cardType = so.cardType;
        cardEffectSOs = so.effects;
        dataSO = so;

        //player = InstanceFinder.ClientManager.Connection.FirstObject.GetComponent<NetworkPlayer>();
        cardSprite.sprite = so.cardImage;
        cardNameText.text = so.cardName;
        cardCostText.text = so.cardCost.ToString();


        //  cardDesText.text = RichTextHelper.ReplaceValues(so.cardDes, GameManager.Instance.player.Attack());

    }

    public void UpdatePosRot(Vector3 pos, Quaternion rot)
    {
        originalPosition = pos;
        originalRotation = rot;
        originaLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    public void UseCard(Player caster, Character target)
    {
        //if (caster.cost.Value < cardCost)
        //{
        //    Debug.Log("法力值不够");
        //    return;
        //}
        //else
        //{
        //    caster.ChangeCost(-cardCost);
        //}
        //caster.ServerUseCardAniRpc(cardName);
        //foreach (CardEffectSO so in cardEffectSOs)
        //{
        //    so.ApplyEffect(caster, target, this);
        //}
    }

    public void ChangeRichText(string t)
    {
        cardNameText.text = $"<color=yellow>{cardName}</color>";
        cardCostText.text = $"<color=blue>{cardCost}</color>";
        cardDesText.text = $"<color=white>{cardDesText.text}</color>";
    }
    private void OnDestroy()
    {
        transform.DOKill();
        Entry.transform.DOKill();
    }

}
