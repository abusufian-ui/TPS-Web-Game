using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        // Optional: Ensure speed is resetting when idle
    }

    public override void UpdateState(MovementStateManager movement)
    {
        // FIX: Check for Crouch FIRST, before checking for movement
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            movement.SwitchState(movement.Crouch);
            return; 
        }

        // Now check if we are moving
        if (movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
    }
}