using UnityEngine;

public class PathMemory : MonoBehaviour
{
    [SerializeField] int _groupIndex;
    GameObject _line;
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
    public GameObject Line
    {
        get
        {
            return _line;
        }
        set
        {
            _line = value;
        }
    }
    public void DeleteSelf()
    {
        _line.GetComponent<PathLineScript>().DeletedObject(_groupIndex);
    }
}
