// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Animations.Rigging;

// public class ActionStateManager : MonoBehaviour
// {
//     // FIX 1: Added 'public' so WeaponManager can access it
//     [HideInInspector] public ActionBaseState currentState;

//     // FIX 2: Ensure these are 'public'
//     public ReloadState Reload = new ReloadState();
//     public DefaultState Default = new DefaultState();

//     public GameObject currentWeapon;
//     [HideInInspector] public WeaponAmmo ammo;

//     AudioSource audioSource;
//     [HideInInspector] public Animator anim;
//     public MultiAimConstraint rHandAim;
//     public TwoBoneIKConstraint lHandIK;

//     void Start()
//     {
//         SwitchState(Default);
//         ammo = currentWeapon.GetComponent<WeaponAmmo>();
//         audioSource = currentWeapon.GetComponent<AudioSource>();
//         anim = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         currentState.UpdateState(this);
//     }

//     public void SwitchState(ActionBaseState state)
//     {
//         currentState = state;
//         currentState.EnterState(this);
//     }

//     public void WeaponReloaded()
//     {
//         Debug.Log("Reload Event Received! Switching to Default State."); // <--- ADD THIS

//         ammo.Reload();
//         rHandAim.weight = 1;
//         lHandIK.weight = 1;
//         SwitchState(Default);
//     }
//     public void ReloadSoundPlay()
//     {
//        audioSource.PlayOneShot(ammo.reloadSound);
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TMPro;
using System;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState Reload = new ReloadState();
    public DefaultState Default = new DefaultState();

    // NEW: Control how long the reload takes (in seconds)
    public float reloadAnimDuration = 3.0f;

    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;

    AudioSource audioSource;
    [HideInInspector] public Animator anim;
    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;

    #region Ammo Display
    public TextMeshProUGUI ammo_display;
    #endregion

    void Start()
    {
        SwitchState(Default);
        ammo = currentWeapon.GetComponent<WeaponAmmo>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        currentState.UpdateState(this);
        display_ammo();
    }

    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void WeaponReloaded()
    {
        ammo.Reload();
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        SwitchState(Default);
    }

    public void ReloadSoundPlay()
    {
        audioSource.PlayOneShot(ammo.reloadSound);
    }
    void display_ammo()
    {
        ammo_display.text = ammo.currentAmmo.ToString() + " / " + ammo.extraAmmo.ToString();
    }
}