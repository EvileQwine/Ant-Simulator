using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] int moveSpeed = 20;
    [SerializeField] int rotateSpeed = 300;
    [SerializeField] int zoomSpeed = 5;
    [SerializeField] int edgeScroll = 20;
    [SerializeField] float dragSpeed = 2f;

    [SerializeField] int[] FOVconstraints = new int[2] { 5, 100 };

    [SerializeField] bool useEdgeScrolling = false;
    [SerializeField] bool useDragging = true;

    [SerializeField] CinemachineFollow followCam;
    Vector2 lastMousePos = Vector2.zero;
    Vector3 target;
    float targetFov;
    bool leftMouseDown = false;
    bool rightMouseDown = false;
    void Awake()
    {

    }
    void Update()
    {
        Movement();
        if (useDragging)
        {
            Dragging();
        }
        if (useEdgeScrolling)
        {
            EdgeScrolling();
        }
        Rotation();
        //CameraZoomFOV();
        CameraZoomMovement();
        RightRotation();
    }
    void EdgeScrolling()
    {
        Vector3 input = Vector3.zero;
        if (useEdgeScrolling)
        {
            if (Input.mousePosition.x < edgeScroll) input.x -= 1f;
            if (Input.mousePosition.y < edgeScroll) input.z -= 1f;
            if (Input.mousePosition.x > Screen.width - edgeScroll) input.x += 1f;
            if (Input.mousePosition.y > Screen.height - edgeScroll) input.z += 1f;
        }
        Vector3 movement = transform.forward * input.z + transform.right * input.x;
        transform.position += moveSpeed * Time.deltaTime * movement;
    }
    void Dragging()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }
        if (leftMouseDown)
        {
            Vector2 mouseMovement = (Vector2)Input.mousePosition - lastMousePos;
            input.x = mouseMovement.x / -dragSpeed;
            input.z = mouseMovement.y / -dragSpeed;
            lastMousePos = Input.mousePosition;
        }
        Vector3 movement = transform.forward * input.z + transform.right * input.x;
        transform.position += moveSpeed * Time.deltaTime * movement;
    }
    void Movement()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) input.z += 1f;
        if (Input.GetKey(KeyCode.S)) input.z -= 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;
        Vector3 movement = transform.forward * input.z + transform.right * input.x;
        transform.position += moveSpeed * Time.deltaTime * movement;
    }
    void Rotation()
    {
        float rotateInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateInput += 1f;
        if (Input.GetKey(KeyCode.E)) rotateInput -= 1f;
        transform.eulerAngles += new Vector3(0, rotateInput * Time.deltaTime * rotateSpeed, 0);
    }
    void CameraZoomFOV()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFov -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFov += 5;
        }
        targetFov = Mathf.Clamp(targetFov, FOVconstraints[0], FOVconstraints[1]);
        followCam.GetComponent<CinemachineCamera>().Lens.FieldOfView =
            Mathf.Lerp(followCam.GetComponent<CinemachineCamera>().Lens.FieldOfView, targetFov, Time.deltaTime * zoomSpeed);
    }
    void CameraZoomMovement()
    {
        followCam.GetComponent<CinemachineFollow>().FollowOffset = new Vector3(0, 60, 0);
    }
    void RightRotation()
    {

    }
}
