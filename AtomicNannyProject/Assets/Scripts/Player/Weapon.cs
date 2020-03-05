using System.Collections;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    [Header("CONFIGURATION")]
    public WeaponsManager.Weapons weapon;
    public float timeBeforeFirstShoot;
    public float fireRate;
    public float damage;
    public float splashDamage;
    public float splashDamageRadius;
    [Range(0,45)]
    public float inaccuracyAngle;
    public float ammunitionsConso;
    public float recoilSpeed;
    public int projectileByShoot;
    public float range;
    public float projectileSpeed;
    public float projectileSize;
    public float enemyKnockback;
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
}
