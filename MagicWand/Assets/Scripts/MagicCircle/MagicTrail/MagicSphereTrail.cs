using System.Collections.Generic;
using UnityEngine;

//ì¬Ò:™R
//–‚–@w‚Ì•`‚¢‚½ü‚ğ•`‰æ‚·‚é

public class MagicSphereTrail : MonoBehaviour
{
    [SerializeField]
    LineRenderer _lineRenderer;

    private List<Vector3> points = new List<Vector3>();

    public void Add(Vector3 pointLocalPos)
    {
        points.Add(pointLocalPos);

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    public void Clear()
    {
        points.Clear();

        _lineRenderer.positionCount = 0;
    }
}
