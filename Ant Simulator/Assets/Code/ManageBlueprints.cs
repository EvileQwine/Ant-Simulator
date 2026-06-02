using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.IO;

public class ManageBlueprints : MonoBehaviour
{
    [SerializeField] GameObject mouseFollower;
    [SerializeField] GameObject path;
    public GameObject currentFollower;
    public List<Vector3> paths = new List<Vector3>();

    public bool activeFollower = false;
    public bool leftMouseDown = false;
    public enum Builds
    {
        None,
        Path,
        MarkResource,
    }
    Builds build;
    void Update()
    {
        if (activeFollower)
        {
            if (Input.GetMouseButtonDown(0)) leftMouseDown = true;
            if (Input.GetMouseButtonUp(0)) leftMouseDown = false;
        }
        if (leftMouseDown && build == Builds.Path)
        {
            if (!currentFollower.GetComponent<MousePosition>().nearPath)
            {
                Instantiate(path, mouseFollower.GetComponent<MousePosition>().TrackMouse(), new Quaternion(0, 0, 0, 0));
            }
        }
    }
    void OnZero()
    { CurBuildState(Builds.None); }
    void OnOne()
    { CurBuildState(Builds.Path); }
    void OnTwo()
    { CurBuildState(Builds.MarkResource); }
    void CurBuildState(Builds b)
    {
        if (build == b || b == Builds.None)
        {
            build = Builds.None;
            Destroy(currentFollower);
            activeFollower = false;
            leftMouseDown = false;
        }
        else
        {
            if (activeFollower)
            {
                Destroy(currentFollower);        
            }
            build = b;
            currentFollower = Instantiate(mouseFollower);
            currentFollower.GetComponent<PlaceObjects>().AssignType(build);
            activeFollower = true;
        }
    }
}
