using System;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class PathLineScript : MonoBehaviour
{
    LineRenderer lineRen;
    [SerializeField] GameObject Line;
    [SerializeField] float pathWidth = 0.5f;
    [SerializeField] GameObject[] points;
    void Awake()
    {
        lineRen = GetComponent<LineRenderer>();
        lineRen.startColor = Color.lightGoldenRodYellow;
        lineRen.endColor = Color.lightGoldenRodYellow;
        lineRen.startWidth = pathWidth;
        lineRen.endWidth = pathWidth;
    }
    public void DrawLine(GameObject[] newPoints)
    {
        points = newPoints;
        if (points.Length == 0)
        {
            Destroy(gameObject);
        }
        lineRen.positionCount = points.Length;
        for (int i = 0; i < lineRen.positionCount; i++)
        {
            lineRen.SetPosition(i, points[i].transform.position);
        }
    }
    public void DeletedObject(int index)
    {
        if (index == 1)
        {
            GameObject[] v = new GameObject[points.Length - 1];
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = points[i + 1];
                v[i].GetComponent<PathMemory>().Line = gameObject;
                v[i].GetComponent<PathMemory>().groupIndex = i + 1;
            }
            DrawLine(v);
        }
        else if (index == points.Length)
        {
            GameObject[] v = new GameObject[points.Length - 1];
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = points[i];
                v[i].GetComponent<PathMemory>().Line = gameObject;
                v[i].GetComponent<PathMemory>().groupIndex = i + 1;
            }
            DrawLine(v);
        }
        else
        {
            GameObject[] first = new GameObject[index - 1];
            GameObject[] second = new GameObject[points.Length - index];
            GameObject newLine = Instantiate(Line);
            for (int i = 0; i < first.Length; i++)
            {
                first[i] = points[i];
                first[i].GetComponent<PathMemory>().Line = gameObject;
                first[i].GetComponent<PathMemory>().groupIndex = i + 1;
            }
            for (int i = 0; i < second.Length; i++)
            {
                second[i] = points[i + index];
                second[i].GetComponent<PathMemory>().Line = newLine;
                second[i].GetComponent<PathMemory>().groupIndex = i + 1;
            }
            DrawLine(first);
            newLine.GetComponent<PathLineScript>().DrawLine(second);
        }
    }
}
