using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionStateBase currentState;
    [HideInInspector] public DefaultState defaultState = new DefaultState();
    [HideInInspector] public ReloadState reloadState = new ReloadState();
    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    [HideInInspector] public Animator anim;
    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;
    AudioSource audioSource;
    void Start()
    {
        SwitchState(defaultState);
        ammo = currentWeapon.GetComponent<WeaponAmmo>();
        anim = GetComponent<Animator>();
        currentState.EnterState(this);
        audioSource = currentWeapon.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);


    }
    public void SwitchState(ActionStateBase state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void WeaponReloaded()
    {
        ammo.Reload();
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        SwitchState(defaultState);
    }

    public void ReloadSoundPlay()
    {
       audioSource.PlayOneShot(ammo.reloadSound);
    }
}
