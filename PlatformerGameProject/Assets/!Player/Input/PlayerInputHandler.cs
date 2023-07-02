using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    public Vector2 MovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; } 
    public bool JumpInput { get; private set; }
    public bool GrabInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)((MovementInput * Vector2.right).normalized.x);
        NormalizedInputY = (int)((MovementInput * Vector2.up).normalized.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //если нажали на кнопку прыжка
        if (context.started)
        {
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void UseJumpInput()
    {
        JumpInput = false;
    }

   private void CheckJumpInputHoldTime()
   {
       if (Time.time >= jumpInputStartTime + inputHoldTime)
       {
           JumpInput = false;
       }
   }
}
