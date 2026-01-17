using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionStateBase
{
    public override void EnterState(ActionStateManager action)
    {
        action.rHandAim.weight = 0;
        action.lHandIK.weight = 0;
        action.anim.SetTrigger("Reload");
    }

    public override void UpdateState(ActionStateManager action)
    {

    }
}
