using System;
using System.Collections;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float breakAwayDistance = 2f;
    [SerializeField] float jstbsTime = 0.1f;
    SphereCollider sCol;
    GameObject nearestPath;
    public bool followingMouse = true;
    public bool nearPath = false;
    private void Awake()
    {
        sCol = GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (followingMouse)
        {
            transform.position = TrackMouse();
        }
        if (nearPath)
        {
            if (Vector3.Distance(nearestPath.transform.position, TrackMouse()) > breakAwayDistance)
            {
                followingMouse = true;
                nearPath = false;
            }
            else transform.position = nearestPath.transform.position;
        }
    }
    public Vector3 TrackMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask);
        return raycastHit.point;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Path"))
        {
            nearestPath = other.gameObject;
            nearPath = true;
            followingMouse = false;
        }
    }
}
