using System;
using System.Collections.Generic;
using UnityEngine;
///<summary>
/// uv贴图闪电链
///</summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class UVChainLightning :MonoBehaviour
{
    //美术资源中进行调整
    public float detail = 1;//增加后，线条数量会减少，每个线条会更长。
    public float displacement = 15;//位移量，也就是线条数值方向偏移的最大值

    public Transform end;//链接目标
    public Transform start;
    public float yOffset = 0;
    private LineRenderer _lineRender;
    private List<Vector3> _linePosList;

    public void SetTarget(Transform start,Transform end)
    {
        this.start = start;
        this.end = end;
    }

    public void SetColor(Color color)
    {
        _lineRender.startColor = color;
        _lineRender.endColor = color;
        _lineRender.material.SetColor("MainColor",color);
        _lineRender.material.SetColor("Color2",color);
    }
    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _lineRender.positionCount = 0;
        _linePosList = new List<Vector3>();
    }

    private void Update()
    {
        if(end==null||start==null||(end==start))
        {
            _lineRender.positionCount = 0;
        }
        if (Time.timeScale != 0&&(end!=null&&start!=null) )
        {
            _linePosList.Clear();
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            if (end != null)
            {
                endPos = end.position + Vector3.up * yOffset;
            }
            if (start != null)
            {
                startPos = start.position + Vector3.up * yOffset;
            }

            CollectLinPos(startPos, endPos, displacement);
            _linePosList.Add(endPos);

            _lineRender.positionCount=_linePosList.Count;
            for (int i = 0, n = _linePosList.Count; i < n; i++)
            {
                _lineRender.SetPosition(i, _linePosList[i]);
            }
        }
    }

    //收集顶点，中点分形法插值抖动
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList.Add(startPos);
        }
        else
        {

            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }


}   