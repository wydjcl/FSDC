using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardData", menuName = "SO/卡牌Data")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cardCost;
    public int cardCoin;
    public CardType cardType;
    public CardRate cardRate;
    public int characterID;
    [TextArea]
    public string cardDes;

    public List<CardEffectSO> effects;
}
