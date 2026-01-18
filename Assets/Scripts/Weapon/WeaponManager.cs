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

    WeaponBloom bloom;
    ActionStateManager actionState;

    WeaponRecoil recoil;

    Light muzzleFlashLight;
    
    // FIX: Changed "ParticalSystem" to "ParticleSystem"
    ParticleSystem muzzleFlashParticles;
    
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 2;  

    void Start()
    {
        recoil = GetComponent<WeaponRecoil>();
        
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
        bloom = GetComponent<WeaponBloom>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        
        // Add safety check for light to prevent errors if you haven't added one yet
        if (muzzleFlashLight != null)
        {
            lightIntensity = muzzleFlashLight.intensity;
            muzzleFlashLight.intensity = 0;
        }

        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        
        if (ammo == null)
        {
            Debug.LogError("WeaponAmmo component missing! Please attach WeaponAmmo script to: " + gameObject.name);
        }
    }

    void Update()
    {
        if (ShouldFire()) Fire();
        
        // Only modify light if it exists
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
        }
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        
        if (fireRateTimer < fireRate) return false;

        if (ammo == null) return false; 
        if (ammo.currentAmmo <= 0) return false;
        
        if (actionState != null && actionState.currentState == actionState.Reload) return false;

        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.BloomAngle(barrelPos);

        if (audioSource != null && gunShot != null)
        {
            audioSource.PlayOneShot(gunShot);
        }
        
        // Add check for recoil script presence
        if (recoil != null) recoil.TriggerRecoil();
        TriggerMuzzleFlash();      
        
        if (ammo != null) ammo.currentAmmo--;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        if (muzzleFlashParticles != null) muzzleFlashParticles.Play();
        if (muzzleFlashLight != null) muzzleFlashLight.intensity = lightIntensity; 
    }
}