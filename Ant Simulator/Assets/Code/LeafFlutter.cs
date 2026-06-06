using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class LeafFlutter : MonoBehaviour
{
    [SerializeField] float gravityScale = 0.1f;
    [SerializeField] float swaySpeed = 0.5f;
    [SerializeField] float swayAmount = 0.2f;
    [SerializeField] int maxSwayForce = 30;
    [SerializeField] int swayConstraint = 30;
    Rigidbody rb;
    Collider col;

    bool touchingGround = false;
    bool addSway = false;
    float rotateAmount = 0f;

    float realRotation; 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = gravityScale;
        col = GetComponentInChildren<Collider>();
    }
    void Update()
    {
        if (!touchingGround && rb.useGravity)
        {
            realRotation = transform.localRotation.eulerAngles.z;
            if (realRotation >= 180)
            {
                realRotation -= 360;
            }
            if (addSway && rotateAmount < maxSwayForce)
            {
                rotateAmount += swayAmount;
            }
            else if (!addSway && rotateAmount > -maxSwayForce)
            {
                rotateAmount -= swayAmount;
            }

            transform.eulerAngles += new Vector3(0, 0, rotateAmount * Time.deltaTime * swaySpeed);

            if (realRotation < -swayConstraint)
            {
                addSway = true;
            }
            if (realRotation > swayConstraint)
            {
                addSway = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        touchingGround = true;
    }
    void OnCollisionExit(Collision collision)
    {
        touchingGround = false;
    }
}
