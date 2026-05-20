using UnityEngine;

public class DragArrowLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float heightOffset = 2f;

    Camera cam;

    void Awake()
    {
        cam = CameraManager.Instance.cardCamera;

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        SetArrowPosition();
    }

    public void SetArrowPosition()
    {
        Vector3 startPos = transform.position + Vector3.up * heightOffset;
        Vector3 endPos = GetMouseWorldPos(startPos);

        // 如果是2D/卡牌平面，建议锁Z（很重要）
        startPos.z = 0;
        endPos.z = 0;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    Vector3 GetMouseWorldPos(Vector3 referencePos)
    {
        Vector3 mouse = Input.mousePosition;

        // 用卡牌深度做转换（关键点）
        mouse.z = cam.WorldToScreenPoint(referencePos).z;

        return cam.ScreenToWorldPoint(mouse);
    }
}