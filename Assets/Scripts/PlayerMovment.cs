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
    public float gravity = -10f;

    Vector3 velocity;
    bool isGrounded;

    private void Update()
    {
        //Input
        Move = InputHandeler.GetComponent<PlayerInputHandeler>().Move;
        Look = InputHandeler.GetComponent<PlayerInputHandeler>().Look;
        transform.position = new Vector3(0, 0.989f, 0);


        #region Move
        //Move
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * Move.x + transform.forward * Move.y;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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
    }

}
