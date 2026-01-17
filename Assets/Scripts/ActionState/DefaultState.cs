using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionStateBase
{
    public override void EnterState(ActionStateManager action)
    {
        action.rHandAim.weight = 1;
        action.lHandIK.weight = 1;

    }

    public override void UpdateState(ActionStateManager action)
    {
        action.rHandAim.weight = Mathf.Lerp(action.rHandAim.weight, 1, Time.deltaTime * 10);
        action.lHandIK.weight = Mathf.Lerp(action.lHandIK.weight, 1, Time.deltaTime * 10);
        if (Input.GetKeyDown(KeyCode.R) && CanReload(action))
        {
            action.SwitchState(action.reloadState);
        }

    }
    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize)
            return false;
        else if (action.ammo.extraAmmo == 0)
            return false;
        else
            return true;
    }
}
