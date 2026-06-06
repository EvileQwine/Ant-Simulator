using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMemory : MonoBehaviour
{
    [SerializeField] int _groupIndex;
    [SerializeField] List<GameObject> _lines = new List<GameObject>();
    public int groupIndex
    {
        get
        {
            return _groupIndex;
        }
        set
        {
            _groupIndex = value;
        }
    }
    public void AddLine(GameObject line)
    {
        _lines.Add(line);
    }
    public void ForgetLines(GameObject line)
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            if (line == _lines[i])
            {
                _lines.RemoveAt(i);
            }
        }
    }
    public void DeleteSelf()
    {
        foreach (GameObject line in _lines)
        {
            line.GetComponent<PathLineScript>().DeletedObject(_groupIndex);
        }   
    }
}
