using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Running", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        // 1. Exit to Walk if Shift is released
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        // 2. Exit to Idle if stopped moving
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        // (Slide Logic Removed)

        // 3. Keep the Speed Fix (Crucial)
        if (movement.vInput < 0) movement.currentMoveSpeed = movement.RunBackSpeed;
        else movement.currentMoveSpeed = movement.RunSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.prevousState = this;// Track previous state
            ExitState(movement, movement.Jump);

        }
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}