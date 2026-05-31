using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;
    enum zState
    {
        None,
        In,
        Out,
    }
    zState curZoom;
    [SerializeField] float zoomSpeed = 30f;
    [SerializeField] float zoomAllowance = 0.25f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            Zoom(true);
        }
        if (scroll < 0)
        {
            Zoom(false);
        }
    }
    void Zoom(bool direction)
    {
        if (direction)
        {
            switch (curZoom)
            {
                case zState.In:
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
                case zState.Out:
                    curZoom = zState.In;
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
                case zState.None:
                    curZoom = zState.In;
                    StartCoroutine(Zooming(true));
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
            }
        }
        else if (!direction)
        {
            switch (curZoom)
            {
                case zState.Out:
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
                case zState.In:
                    curZoom = zState.Out;
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
                case zState.None:
                    curZoom = zState.Out;
                    StartCoroutine(Zooming(false));
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
            }
        }
    }
    IEnumerator Zooming(bool direction)
    {
        curZoom = direction ? zState.In : zState.Out;
        yield return new WaitForSeconds(zoomAllowance);
        curZoom = zState.None;
        yield return new WaitForSeconds(zoomAllowance);
        ZeroVelocity();
    }
    void ZeroVelocity()
    {
        if (curZoom == zState.None)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
