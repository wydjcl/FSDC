using DG.Tweening;
using TMPro;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
    public TextMeshPro tmp;
    public bool upEffect = true;
    public bool blockEffect = false;
    void Start()
    {
        if (upEffect)
        {
            UPText();
        }
        else if (blockEffect)
        {

        }
    }

    public void UPText()
    {
        Vector3 start = transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(new Vector3(start.x + 0.1f, start.y + 2.5f, 0), 0.5f).SetEase(Ease.OutQuad)
        );
        seq.SetLink(transform.gameObject);
        // 结束后销毁
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        })

            ;
    }


    public void ChangeToHurtText(int i)
    {
        tmp.text = i.ToString();
        if (i < 0)
        {
            tmp.color = Color.red;
        }
        if (i > 0)
        {
            tmp.color = Color.green;
        }
        if (i == 0)
        {
            tmp.color = Color.blue;
        }
    }

    public void ChangeToBlockText()
    {
        tmp.color = Color.white;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
