using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandeler : MonoBehaviour
{
    PlayerControls controls;

    public Vector2 Move;
    public Vector2 Look;
    public bool Interact;
    public bool Release;

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
        // Interact Input
        controls.Player.Interact.started += ctx => Interact = true;
        controls.Player.Interact.canceled += ctx => Interact = false;
        // Release Input
        controls.Player.Release.started += ctx => Release = true;
        controls.Player.Release.canceled += ctx => Release = false;
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
