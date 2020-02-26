using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    [Header("CONFIGURATION")]
    public WeaponsManager.Weapons weapon;
    public float timeBeforeFirstShoot;
    public float fireRate;
    public int damage;
    public int splashDamage;
    [Range(0,45)]
    public int inaccuracyAngle;
    public float ammunitionsConso;
    public float recoilSpeed;
    public int projectileByShoot;
    public float range;
    public float projectileSpeed;
    public float projectileSize;
    public GameObject projectile;
    public GameObject objectMod;
    [Header("RUNNING VARIABLES")]
    public WeaponMod equippedMod;
    public bool reloading;
    public bool isStanced;

    public IEnumerator ReloadSystem()
    {
        reloading = true;
        yield return new WaitForSeconds(Mathf.Abs(WeaponsStats.instance.GetFireRate(weapon) - WeaponsStats.instance.GetTimeBeforeFirstShoot(weapon)));
        reloading = false;
        if (WeaponsManager.instance.startShotNeeded)
        {
            WeaponsManager.instance.startShotNeeded = false;
            WeaponsManager.instance.shotCoroutine = WeaponsManager.instance.StartCoroutine(WeaponsManager.instance.Shoot());
        }
    }

    public IEnumerator ParallelReload()
    {
        reloading = true;
        yield return new WaitForSeconds(Mathf.Abs(WeaponsStats.instance.GetFireRate(weapon) - WeaponsStats.instance.GetTimeBeforeFirstShoot(weapon)));
        reloading = false;
    }
}
