using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStates : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        if (movement.prevousState == movement.Idle)
        {
            movement.anim.SetTrigger("IdleJump");
        }
        if(movement.prevousState == movement.Walk||movement.prevousState == movement.Run)
        {
            movement.anim.SetTrigger("WalkJump");
        }

    }

    public override void UpdateState(MovementStateManager movement)
    {

    }

}
