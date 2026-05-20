using DG.Tweening;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCon : NetworkBehaviour, IPointerClickHandler
{
    public GameObject Entry;
    public Rigidbody2D rb;
    public float speed = 9f;
    public Player player;
    private Sequence moveSeq;

    public bool canMove;
    public bool chestEnter;
    public Chest chest;

    public readonly SyncVar<bool> isMoving = new SyncVar<bool>();

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("点击玩家");
    }

    public override void OnStartClient()
    {
        canMove = true;
        isMoving.OnChange += IsMoving_OnChange;
        base.OnStartClient();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (IsOwner)
        {
            //CameraManager.Instance.cameraFollowTarget.player = this.transform;
            //CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Map;

        }
    }



    public override void OnStopClient()
    {
        isMoving.OnChange -= IsMoving_OnChange;
        base.OnStopClient();
        if (IsOwner)
        {
            //CameraManager.Instance.cameraFollowTarget.player = null;
            //CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.None;
        }
    }
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        //if (Input.GetKeyDown(KeyCode.M) || (Input.GetKeyDown(KeyCode.Tab)))
        //{
        //    MapManager.Instance.mapGameObject.SetActive(!MapManager.Instance.mapGameObject.activeSelf);

        //    Canvas.ForceUpdateCanvases();
        //    if (player.playerInstance.currentRoom.Value == null)
        //    {
        //        return;
        //    }
        //    var target = player.playerInstance.currentRoom.Value.GetComponent<RectTransform>();
        //    if ((target == null))
        //    {
        //        return;
        //    }
        //    var content = MapManager.Instance.content.GetComponent<RectTransform>();

        //    var viewport = MapManager.Instance.viewport.GetComponent<RectTransform>();
        //    // Content 和 Target 的位置
        //    //Vector2 contentPos = content.anchoredPosition;
        //    //Vector2 targetPos = (Vector2)content.InverseTransformPoint(target.position);

        //    //// Viewport 中心
        //    //Vector2 viewportCenter = viewport.rect.size / 2;

        //    //// 计算偏移
        //    //Vector2 offset = targetPos - viewportCenter;

        //    // 移动 Content
        //    content.anchoredPosition = -target.anchoredPosition;

        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  player.playerInstance.Test();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            rb.velocity = Vector2.zero;
            if (BagUI.Instance.root.gameObject.activeSelf)
            {
                BagUI.Instance.Close();
            }
            else
            {
                if (chestEnter)
                {
                    BagUI.Instance.chest = chest;
                    BagUI.Instance.Open(BagType.Chest);
                }
                else
                {
                    BagUI.Instance.Open(BagType.Battle);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Instance.SaveTest();
        }
    }
    void FixedUpdate()
    {

        bool localMove = false;

        if (IsOwner && canMove && !BagUI.Instance.root.gameObject.activeSelf)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector2 dir = new Vector2(h, v).normalized;

            rb.velocity = dir * speed;
            if (h > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            localMove = h != 0 || v != 0;
            if (localMove != isMoving.Value)
            {
                IsMovingRpc(localMove);
            }

            if (localMove || isMoving.Value)//本地移动
            {
                StartMoveTween();
            }
            else
            {
                StopMoveTween();
            }
        }
        if (!IsOwner)
        {
            if (isMoving.Value)//本地移动
            {
                StartMoveTween();
            }
            else
            {
                StopMoveTween();
            }
        }
    }
    public void StartMoveTween()
    {
        Debug.Log("开启动画");
        if (moveSeq != null && moveSeq.IsActive())
        {
            Debug.Log("有动画了");
            return;
        }

        moveSeq = DOTween.Sequence();

        moveSeq.Append(Entry.transform.DORotate(new Vector3(0, 0, 10f), 0.15f));
        moveSeq.Append(Entry.transform.DORotate(new Vector3(0, 0, -10f), 0.15f));

        moveSeq.SetLoops(-1); // 无限循环
        moveSeq.SetLink(Entry);
        moveSeq.SetEase(Ease.InOutSine);
    }

    public void StopMoveTween()
    {
        if (moveSeq != null)
        {
            moveSeq.Kill();
            moveSeq = null;
        }
        moveSeq = DOTween.Sequence();
        // 回正（单独一个 Tween）
        moveSeq.Append(Entry.transform.DORotate(Vector3.zero, 0.15f));
        moveSeq.SetLink(Entry);
    }

    public void MoveToTransform(Transform trans)
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        StartMoveTween();
        transform.DOMove(trans.position, 0.6f)
      .SetEase(Ease.Linear)
      .OnComplete(() =>
      {
          StopMoveTween();
      });
    }

    [ServerRpc(RequireOwnership = false)]
    public void IsMovingRpc(bool b)
    {
        isMoving.Value = b;
        // Debug.Log("别的玩家给信号" + b);
    }
    private void IsMoving_OnChange(bool prev, bool next, bool asServer)
    {
        if (asServer)
        {

        }
        else
        {
            if (!IsOwner)//非本地玩家拖动
            {
                if (next)
                {
                    // Debug.Log("非本玩家开始动画");
                    // Debug.Log("开启动画");
                    StartMoveTween();
                }
                else
                {
                    // Debug.Log("非本玩家停止动画");
                    //  Debug.Log("关闭动画");
                    StopMoveTween();
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            if (!chestEnter || BagUI.Instance.chest == null)
            {
                chestEnter = true;
                var c = collision.gameObject.GetComponent<Chest>();
                chest = c;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            Debug.Log("离开宝箱");
            BagUI.Instance.chest = null;
            chest = null;
            chestEnter = false;
        }
    }
}
