using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;
    PlayerInput input;
    Vector2 lastMousePosition;
    enum ZState
    {
        None,
        In,
        Out,
    }
    ZState curZoom;

    [SerializeField] bool leftMouseDown = false;
    [SerializeField] float dragSpeed = 2f;
    [SerializeField] float zoomSpeed = 30f;
    [SerializeField] float zoomAllowance = 0.25f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        input = GetComponent<PlayerInput>();
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

        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }
        if (leftMouseDown)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            rb.AddForce((-mouseMovementDelta.x * dragSpeed), 0, (-mouseMovementDelta.y * dragSpeed));
        }
        else
        {
            rb.linearVelocity = new Vector3 (0, 0, rb.linearVelocity.z);
        }
    }
    void Zoom(bool direction)
    {
        if (direction)
        {
            switch (curZoom)
            {
                case ZState.In:
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
                case ZState.Out:
                    curZoom = ZState.In;
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
                case ZState.None:
                    curZoom = ZState.None;
                    StartCoroutine(Zooming(true));
                    rb.AddForce(zoomSpeed * rb.transform.forward);
                    break;
            }
        }
        else if (!direction)
        {
            switch (curZoom)
            {
                case ZState.Out:
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
                case ZState.In:
                    curZoom = ZState.Out;
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
                case ZState.None:
                    curZoom = ZState.Out;
                    StartCoroutine(Zooming(false));
                    rb.AddForce(-zoomSpeed * rb.transform.forward);
                    break;
            }
        }
    }
    IEnumerator Zooming(bool direction)
    {
        curZoom = direction ? ZState.In : ZState.Out;
        yield return new WaitForSeconds(zoomAllowance);
        curZoom = ZState.None;
        yield return new WaitForSeconds(zoomAllowance);
        if (curZoom == ZState.None)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
