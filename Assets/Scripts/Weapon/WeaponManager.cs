using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAuto;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;

    [SerializeField] int bulletsPerShot;
    AimStateManager aim;

    [Header("Audio")]
    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;
    WeaponAmmo ammo;
    ActionStateManager actionState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        aim = GetComponentInParent<AimStateManager>();
        actionState = GetComponentInParent<ActionStateManager>();
        fireRateTimer = fireRate;

        ammo = GetComponent<WeaponAmmo>();
        if (ammo == null)
        {
            ammo = GetComponentInParent<WeaponAmmo>();
        }
        
        if (ammo == null)
        {
            Debug.LogError("WeaponAmmo component missing! Please attach WeaponAmmo script to: " + gameObject.name);
        }
    }

    void Update()
    {
        if (ShouldFire()) Fire();
        
        // FIX: I commented this out to stop the 999+ console spam
        // if (ammo != null) Debug.Log("Current Ammo: " + ammo.currentAmmo); 
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        
        if (fireRateTimer < fireRate) return false;

        if (ammo == null) return false; 
        if (ammo.currentAmmo <= 0) return false;
        
        // This checks if we are currently reloading to prevent shooting
        if (actionState != null && actionState.currentState == actionState.Reload) return false;

        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);

        if (audioSource != null && gunShot != null)
        {
            audioSource.PlayOneShot(gunShot);
        }
        
        if (ammo != null) ammo.currentAmmo--;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }
}