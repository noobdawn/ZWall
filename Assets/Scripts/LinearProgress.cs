using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProgress : BaseProgressbar
{
    public Camera mainCamera;
    public LineRenderer lineProgress;
    public float lineWidth;
    public override void SetInfo(CurAudioInfo info)
    {
        base.SetInfo(info);
        OnUpdateInfo();
    }

    private void Awake()
    {
        lineProgress.SetWidth(lineWidth, lineWidth);
        isInited = true;
    }

    protected override void OnUpdateInfo()
    {
        base.OnUpdateInfo();
        Vector3[] far = GetCorners(mainCamera.farClipPlane);
        Vector3[] near = GetCorners(mainCamera.nearClipPlane);
        Vector3 cross1 = Vector3.zero, cross2 = Vector3.zero;
        Vector3 lineOrigin = new Vector3(0, 1, 0);
        Vector3 lineDir = new Vector3(0.1f, 1.1f, -0.15f);
        if (TryGetCrossFromLineAndPlane(lineOrigin, lineDir, far[0], far[1], near[0], ref cross1) &&
            TryGetCrossFromLineAndPlane(lineOrigin, lineDir, far[2], far[3], near[2], ref cross2))
        {
            lineProgress.SetPosition(0, cross1);
            lineProgress.SetPosition(1, cross2);
            lineProgress.transform.Find("Current").transform.position = cross2 + (cross1 - cross2) * (curAudioInfo.CurrentSec / curAudioInfo.TotalSec);
            Debug.DrawLine(cross2, cross1);
        }
    }

    /// <summary>
    /// 获得指定距离摄像机的视口
    /// http://www.xuanyusong.com/archives/3036
    /// </summary>
    /// <param name="distance">距离</param>
    /// <returns></returns>
    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];

        float halfFOV = (mainCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = mainCamera.aspect;
        var tx = mainCamera.transform;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        // UpperLeft
        corners[0] = tx.position - (tx.right * width);
        corners[0] += tx.up * height;
        corners[0] += tx.forward * distance;

        // UpperRight
        corners[1] = tx.position + (tx.right * width);
        corners[1] += tx.up * height;
        corners[1] += tx.forward * distance;

        // LowerLeft
        corners[2] = tx.position - (tx.right * width);
        corners[2] -= tx.up * height;
        corners[2] += tx.forward * distance;

        // LowerRight
        corners[3] = tx.position + (tx.right * width);
        corners[3] -= tx.up * height;
        corners[3] += tx.forward * distance;

        return corners;
    }

    /// <summary>
    /// 获得直线和平面的交点
    /// http://m.blog.csdn.net/abcjennifer/article/details/6688080
    /// </summary>
    /// <param name="lineOrigin"></param>
    /// <param name="lineDir"></param>
    /// <param name="planePoint1"></param>
    /// <param name="planePoint2"></param>
    /// <param name="planePoint3"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryGetCrossFromLineAndPlane(Vector3 lineOrigin, Vector3 lineDir, Vector3 planePoint1, Vector3 planePoint2, Vector3 planePoint3, ref Vector3 result)
    {
        Vector3 normalPlane = Vector3.Cross(planePoint1 - planePoint2, planePoint1 - planePoint3);
        var vpt = Vector3.Dot(normalPlane, lineDir);
        if (vpt == 0)
        {
            return false;
        }
        var t = Vector3.Dot((planePoint1 - lineOrigin), normalPlane) / vpt;//  ((n1 - m1) * vp1 + (n2 - m2) * vp2 + (n3 - m3) * vp3) / vpt;
        result = lineDir * t + lineOrigin;
        return true;
    }
}
