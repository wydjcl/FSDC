using System.Collections.Generic;
using UnityEngine;

public class CardLayout : MonoBehaviour
{
    [Header("布局模式")]
    public bool useArcLayout = false; // true = 弧形布局，false = 直线布局

    [Header("直线参数")]
    public float maxWidth = 12f;//

    public float cardSpacing = 2.2f;//卡牌间隙

    [Header("弧形参数")]
    public float radius = 8f; // 弧形半径

    public float maxAngle = 30f; // 左右最大角度
    public Vector3 centerPoint;

    [SerializeField]
    private List<Vector3> cardPositions = new();//卡牌坐标

    [SerializeField]
    private List<Quaternion> cardRotations = new();//角度

    private void Awake()
    {
        centerPoint = Vector3.up * -9f;//起始高度
    }

    public CardTransForm GetCardTransForm(int index, int totalCards)//第几,总共几
    {
        if (useArcLayout)
        {
            CalculateArcPositions(totalCards);
        }
        else
        {
            CalculateLinePositions(totalCards);
        }

        return new CardTransForm(cardPositions[index], cardRotations[index]);
        // return new CardTransForm(cardPositions[index], cardRotations[index]);
    }

    private void CalculateLinePositions(int numberOfCards)
    {
        cardPositions.Clear();
        cardRotations.Clear();//每次计算要清空
        float currentWidth = cardSpacing * (numberOfCards - 1);//卡牌间隙长,如2张牌就只有一个间隙s
        float totalWidth = Mathf.Min(currentWidth, maxWidth);//如果总宽大于我们设定的
        //totalWidth = 15f;
        float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;//卡牌间隙
        for (int i = 0; i < numberOfCards; i++)
        {
            float xPox = 0 - (totalWidth / 2) + i * currentSpacing;//卡坐标
            var pos = new Vector3(xPox, centerPoint.y, -i * 0.01f);
            var rotation = Quaternion.identity;
            cardPositions.Add(pos);
            cardRotations.Add(rotation);
        }
    }

    private void CalculateArcPositions(int count)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (count == 0) return;
        float angleStep = 0;
        if (count > 1) angleStep = (maxAngle * 2f) / (count - 1);
        for (int i = 0; i < count; i++)
        {
            float angle = -maxAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            float x = Mathf.Sin(rad) * radius;
            float y = Mathf.Cos(rad) * radius - radius;
            Vector3 pos = new Vector3(x, y, -i * 0.01f) + centerPoint;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            cardPositions.Add(pos);
            cardRotations.Add(rot);
        }
    }
}

public struct CardTransForm
{
    public Vector3 pos;
    public Quaternion rotation;

    public CardTransForm(Vector3 position, Quaternion quaternion)
    {
        pos = position;
        rotation = quaternion;
    }
}