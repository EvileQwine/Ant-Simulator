using Unity.VisualScripting;
using UnityEngine;

public class PathLineScript : MonoBehaviour
{
    LineRenderer lineRen;
    [SerializeField] float pathWidth = 0.5f;
    void Awake()
    {
        lineRen = GetComponent<LineRenderer>();
        lineRen.startColor = Color.lightGoldenRodYellow;
        lineRen.endColor = Color.lightGoldenRodYellow;
        lineRen.startWidth = pathWidth;
        lineRen.endWidth = pathWidth;
    }
    public void DrawLine(Vector3[] points)
    {
        lineRen.positionCount = points.Length;
        for (int i = 0; i < lineRen.positionCount; i++)
        {
            lineRen.SetPosition(i, points[i]);
        }
    }
}
