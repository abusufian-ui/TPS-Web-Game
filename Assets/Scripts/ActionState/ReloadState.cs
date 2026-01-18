using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    float timer; 

    public override void EnterState(ActionStateManager action)
    {
        action.rHandAim.weight = 0;
        action.lHandIK.weight = 0;
        action.anim.SetTrigger("Reload");
        timer = 0; // Reset timer start
    }

    public override void UpdateState(ActionStateManager action)
    {
        // 1. Count the time
        timer += Time.deltaTime;

        // 2. If time is up, finish reloading automatically
        if (timer >= action.reloadAnimDuration)
        {
            action.WeaponReloaded();
        }
    }
}