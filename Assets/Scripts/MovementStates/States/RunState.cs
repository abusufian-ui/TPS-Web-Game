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
        if(Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        else if(movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        // FIXED: Apply Running Speed
        if(movement.vInput < 0) movement.currentMoveSpeed = movement.RunBackSpeed;
        else movement.currentMoveSpeed = movement.RunSpeed;
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}