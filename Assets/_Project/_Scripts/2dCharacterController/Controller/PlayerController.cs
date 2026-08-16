using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Reads player input and forwards it to CharacterController2D.
[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    public InputMaster controls;

    private CharacterController2D character;
    private InteractSystem interact;
    private Vector2 axis;

    private bool isAwake = true;

    public bool IsAwake => isAwake;
    public bool IsAsleep => !isAwake;

    public static Action<bool> OnSwitch;
    
    void Awake()
    {
        character = GetComponent<CharacterController2D>();
        interact = GetComponent<InteractSystem>();

        controls = new InputMaster();
        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled  += ctx => Move(Vector2.zero);
        controls.Player.Jump.started       += Jump;
        controls.Player.Jump.canceled      += EndJump;
        controls.Player.Dash.started       += Dash;
        controls.Player.Interact.started   += Interact;
        controls.Player.Switch.started     += Switch;
    }

    void FixedUpdate()
    {
        character.Walk(axis.x);
    }

    private void Move(Vector2 _axis)
    {
        axis = _axis;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (axis.y < 0)
        {
            character.JumpDown();
        }
        else
        {
            character.Jump();
        }
    }

    private void EndJump(InputAction.CallbackContext context)
    {
        character.EndJump();
    }

    private void Dash(InputAction.CallbackContext context)
    {
        character.Dash(axis);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (interact)
        {
            interact.Interact();
        }
    }

    private void Switch(InputAction.CallbackContext context)
    {
        isAwake = !isAwake;
        OnSwitch?.Invoke(isAwake);
    }

    void OnEnable()  { controls.Player.Enable(); }
    void OnDisable() { controls.Player.Disable(); }
}