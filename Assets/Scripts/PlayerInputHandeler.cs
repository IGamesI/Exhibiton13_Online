using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandeler : MonoBehaviour
{
    PlayerControls controls;

    public Vector2 Move;
    public Vector2 Look;

    private void Awake()
    {
        controls = new PlayerControls();
        //Move Input
        controls.Player.Move.started += ctx => Move = ctx.ReadValue<Vector2>();
        controls.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => Move = ctx.ReadValue<Vector2>();
        //Look Input
        controls.Player.Look.started += ctx => Look = ctx.ReadValue<Vector2>();
        controls.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => Look = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

   
}
