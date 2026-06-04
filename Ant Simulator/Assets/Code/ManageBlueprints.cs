using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.IO;
using System.Linq;

public class ManageBlueprints : MonoBehaviour
{
    [SerializeField] GameObject mouseFollower;
    [SerializeField] GameObject path;
    [SerializeField] GameObject line;
    [SerializeField] float hardPlaceCooldown = 0.5f;

    public GameObject currentFollower;
    public List<Vector3> paths = new List<Vector3>();
    public List<Vector3[]> storedPaths = new List<Vector3[]>();

    public bool activeFollower = false;
    public bool leftMouseDown = false;
    public bool ctrlDown = false;
    bool canHardPlace = true;
    public enum Builds
    {
        None,
        Path,
        MarkResource,
    }
    Builds build;
    void Awake()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) ctrlDown = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) ctrlDown = false;
        if (activeFollower)
        {
            if (Input.GetMouseButtonDown(0)) leftMouseDown = true;
            if (Input.GetMouseButtonUp(0)) 
            {
                leftMouseDown = false;
                if (paths.Count > 0)
                {
                    storedPaths.Add(paths.ToArray());
                    paths.Clear();
                    GameObject newLine = Instantiate(line);
                    newLine.GetComponent<PathLineScript>().DrawLine(storedPaths.Last());
                }
            }
        }
        if (leftMouseDown && build == Builds.Path)
        {
            if (!currentFollower.GetComponent<MousePosition>().nearPath)
            {
                PlacePath();
            }
            else if (ctrlDown && canHardPlace)
            {
                PlacePath();
                StartCoroutine(HardPlace());
            }
        }
    }
    void PlacePath()
    {
        GameObject newPath = Instantiate(path, mouseFollower.GetComponent<MousePosition>().TrackMouse(), new Quaternion(0, 0, 0, 0));
        currentFollower.GetComponent<MousePosition>().nearestPath = newPath;
        currentFollower.GetComponent<MousePosition>().nearPath = true;
        paths.Add(newPath.transform.position);
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
            build = b;
            if (!activeFollower)
            {
                currentFollower = Instantiate(mouseFollower);
            }
            activeFollower = true;
        }
    }
    IEnumerator HardPlace()
    {
        canHardPlace = false;
        yield return new WaitForSeconds(hardPlaceCooldown);
        canHardPlace = true;
    }
}
