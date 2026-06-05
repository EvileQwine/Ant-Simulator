using UnityEngine;

public class PathMemory : MonoBehaviour
{
    [SerializeField] int _pathGroup;
    [SerializeField] int _groupIndex;

    public int pathGroup
    {
        get
        {
            return _pathGroup;
        }
        set
        {
            _pathGroup = value;
        }
    }
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
}
