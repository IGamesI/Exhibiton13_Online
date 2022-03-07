using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    private Vector2 Move;
    private Vector2 Look;
    public GameObject InputHandeler;

    public GameObject Camera;

    public float mouseSpeed = 100f;
    
    float xRotation = 0f;


    //Move Vars
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -9.81f;

    Vector3 velocity;
    private float fallingSpeed;
    public LayerMask groundLayer;


    private void FixedUpdate()
    {
        //Input
        Move = InputHandeler.GetComponent<PlayerInputHandeler>().Move;
        Look = InputHandeler.GetComponent<PlayerInputHandeler>().Look;
        transform.position = new Vector3(0, 0.989f, 0);

        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }
        fallingSpeed += gravity * Time.fixedDeltaTime;
        controller.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        #region Move

        Vector3 move = transform.right * Move.x + transform.forward * Move.y;

        controller.Move(move * speed * Time.deltaTime);
        #endregion

        #region Look
        //Look
        Look = Look * mouseSpeed * Time.deltaTime;

        xRotation -= Look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * Look.x);
        #endregion

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller.enableOverlapRecovery = false;
    }

    bool CheckIfGrounded()
    {
        // tells us if on ground
        Vector3 rayStart = transform.TransformPoint(controller.center);
        float rayLength = controller.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, controller.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

}
