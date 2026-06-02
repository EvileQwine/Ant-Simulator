using Unity.VisualScripting;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    public ManageBlueprints.Builds build;
    public bool leftMouseDown = false;
    void Awake()
    {
        build = ManageBlueprints.Builds.None;
    }
    public void AssignType(ManageBlueprints.Builds b)
    {
        build = b;
    }
    void Update()
    {


        if (leftMouseDown)
        {

        }
    }
}
