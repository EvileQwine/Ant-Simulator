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

    MousePosition curScript;

    GameObject currentFollower;
    public List<GameObject> paths = new List<GameObject>();
    public List<Tuple<Vector3[], GameObject>> storedPaths = new List<Tuple<Vector3[], GameObject>>();

    public bool activeFollower = false;
    public bool leftMouseDown = false;
    public bool backspaceDown = false;
    public bool ctrlDown = false;
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
        if (activeFollower)
        {
            if (Input.GetMouseButtonDown(0)) leftMouseDown = true;
            if (Input.GetMouseButtonUp(0)) 
            {
                leftMouseDown = false;
                if (paths.Count > 0)
                {
                    Vector3[] v = new Vector3[paths.Count];
                    for (int i = 0; i < paths.Count; i++)
                    {
                        v[i] = paths[i].transform.position;
                    }
                    GameObject newLine = Instantiate(line);
                    newLine.GetComponent<PathLineScript>().DrawLine(v);
                    storedPaths.Add(Tuple.Create(v, newLine));
                    paths.Clear();
                }
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) backspaceDown = true;
            if (Input.GetKeyUp(KeyCode.Backspace)) backspaceDown = false;
        }
        else
        {
            backspaceDown = false;
            leftMouseDown = false;
        }
        if (leftMouseDown && build == Builds.Path)
        {
            PlacePath();
        }
        if (backspaceDown && build == Builds.Path)
        {
            if (curScript.nearPath)
            {
                Tuple<Vector3[], GameObject> t = storedPaths[curScript.nearestPath.GetComponent<PathMemory>().pathGroup];
                int i = curScript.nearestPath.GetComponent<PathMemory>().groupIndex;
                if (i == 1)
                {
                    Debug.Log("First");
                }
                else if (i == t.Item1.Length)
                {
                    Debug.Log("Last");
                }
                else
                {
                    Vector3[] start = t.Item1.Take(i).ToArray();
                    Vector3[] end = t.Item1.Skip(i).ToArray();
                    
                }
                
                Destroy(curScript.nearestPath);
                curScript.nearPath = false;
                curScript.followingMouse = true;
            }
        }
    }
    void PlacePath()
    {
        if (!curScript.nearPath)
        {
            if (curScript.TrackMouse() == Vector3.zero) return;
            GameObject newPath = Instantiate(path, curScript.TrackMouse(), new Quaternion(0, 0, 0, 0));
            curScript.nearestPath = newPath;
            curScript.nearPath = true;
            paths.Add(newPath);
            newPath.GetComponent<PathMemory>().pathGroup = storedPaths.Count();
            newPath.GetComponent<PathMemory>().groupIndex = paths.Count();
        }
        else
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (curScript.nearestPath == paths[i])
                {
                    return;
                }
            }
            paths.Add(curScript.nearestPath);
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
            build = b;
            if (!activeFollower)
            {
                currentFollower = Instantiate(mouseFollower);
                curScript = currentFollower.GetComponent<MousePosition>();
            }
            activeFollower = true;
        }
    }
}
