using System;
using System.Collections;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] public float breakAwayDistance = 2f;
    SphereCollider sCol;
    public GameObject nearestPath;
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
                StartCoroutine(CheckNewMousePos());
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
    IEnumerator CheckNewMousePos()
    {
        transform.position = TrackMouse();
        yield return new WaitForFixedUpdate();
        if (Vector3.Distance(nearestPath.transform.position, TrackMouse()) > breakAwayDistance)
        {
            followingMouse = true;
            nearPath = false;
        }
    }
}
