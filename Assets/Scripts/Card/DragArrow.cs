using UnityEngine;

public class DragArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 mousePos;
    public int pointCount = 5000;//曲线点数
    public float arcModifier = 4;//贝塞尔曲线
    public bool useBezier = true; // 是否使用贝塞尔曲线，false 时画直线


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
        Vector3 startPos = transform.position;
        for (int i = 0; i < pointCount; i++) lineRenderer.SetPosition(i, startPos);
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 10f;

            SetArrowPosition();
        }
    }

    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position + Vector3.up * 2f;//卡牌位置
        if (useBezier)
        {
            Vector3 direction = mousePos - cardPosition;//从卡牌指向鼠标的方向
            Vector3 normalizedDirection = direction.normalized;//归一化方向

            //计算垂直于卡牌到鼠标的向量
            Vector3 perpendicular = new(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);
            //设置控制点的偏移量
            Vector3 offset = perpendicular * arcModifier;//调整这个值改曲线
            Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset;//控制点
            lineRenderer.positionCount = pointCount;//设置点的数量

            for (int i = 0; i < pointCount; i++)
            {
                float t = i / (float)(pointCount - 1);
                Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
                lineRenderer.SetPosition(i, point);
            }//所有点添加
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, cardPosition);
            lineRenderer.SetPosition(1, mousePos);
        }
    }

    //计算二次贝塞尔曲线
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;//第一项
        p += 2 * u * t * p1;//第二项
        p += tt * p2;//第三项

        return p;
    }
}