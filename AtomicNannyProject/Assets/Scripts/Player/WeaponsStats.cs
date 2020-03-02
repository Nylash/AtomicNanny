﻿using UnityEngine;

public class WeaponsStats : MonoBehaviour
{
    public static WeaponsStats instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
#pragma warning disable 0649
    [SerializeField] Weapon pistol;
    [SerializeField] Weapon shotgun;
    [SerializeField] Weapon minigun;
    [SerializeField] Weapon raygun;
    [SerializeField] Weapon plasmaRifle;
    [SerializeField] Weapon rocket;
    [SerializeField] Weapon flameThrower;
#pragma warning restore 0649
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public float GetTimeBeforeFirstShoot(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).timeBeforeFirstShoot;
    }

    public float GetFireRate(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).fireRate;
    }

    public float GetDamage(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).damage;
    }

    public float GetSplashDamage(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).splashDamage;
    }

    public float GetSplashDamageRadius(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).splashDamageRadius;
    }

    public int GetInaccuracyAngle(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).inaccuracyAngle;
    }

    public float GetAmmunitionsConso(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).ammunitionsConso;
    }

    public float GetRecoilSpeed(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).recoilSpeed;
    }

    public int GetProjectileByShoot(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileByShoot;
    }

    public float GetRange(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).range;
    }

    public float GetProjectileSpeed(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileSpeed;
    }

    public float GetProjectileSize(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileSize;
    }

    public GameObject GetProjectile(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectile;
    }

    public bool IsReloading(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).reloading;
    }

    public void StartReloadSystem(WeaponsManager.Weapons weapon)
    {
        StartCoroutine(GetWeapon(weapon.ToString()).ReloadSystem());
    }

    public void StartParallelReload(WeaponsManager.Weapons weapon)
    {
        StartCoroutine(GetWeapon(weapon.ToString()).ParallelReload());
    }

    public WeaponMod GetEquippedMod(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).equippedMod;
    }

    public void ChangeStance(WeaponsManager.Weapons weapon)
    {
        GetWeapon(weapon.ToString()).isStanced = !GetWeapon(weapon.ToString()).isStanced;
    }

    public bool IsStanced(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).isStanced;
    }

    public void InstantiateMod(WeaponsManager.Weapons weapon)
    {
        if (!GetEquippedMod(weapon))
        {
            if (GetWeapon(weapon.ToString()).objectMod){
                GameObject mod = Instantiate(GetWeapon(weapon.ToString()).objectMod);
                GetWeapon(weapon.ToString()).equippedMod = mod.GetComponent<WeaponMod>();
            }
        }
    }

    public Weapon GetWeapon(string name)
    {
        switch (name)
        {
            case "pistol":
                return pistol;
            case "shotgun":
                return shotgun;
            case "minigun":
                return minigun;
            case "raygun":
                return raygun;
            case "plasmaRifle":
                return plasmaRifle;
            case "rocket":
                return rocket;
            case "flameThrower":
                return flameThrower;
            default:
                Debug.LogError("There is no weapon called : " + name);
                return null;
        }
    }
}