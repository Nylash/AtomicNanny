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
    public float ammunitionConso;
    public float recoilSpeed;
    public int projectileByShoot;
    public float range;
    public float projectileSpeed;
    public float projectileSize;
    public float enemyKnockback;
    public AmmunitionManager.AmmoType ammunition;
    public GameObject projectile;
    public GameObject objectMod;
    [Header("RUNNING VARIABLES")]
    public WeaponMod equippedMod;
    public bool reloading;
    public bool isStanced;


    //Basic coroutine that handle reload of the weapon. If at the end of reload time startShotNeeded is true (an input was press and is hold during reloading) we start shooting
    //The system is the same for secondary shot, with the specificity of StanceMod which launch primaryShot instead of secondaryShot when startShotNeeded is true
    public IEnumerator ReloadSystem()
    {
        reloading = true;
        yield return new WaitForSeconds(Mathf.Abs(WeaponsStats.instance.GetFireRate(weapon) - WeaponsStats.instance.GetTimeBeforeFirstShoot(weapon)));
        reloading = false;
        if (WeaponsManager.instance.startPrimaryShotNeeded)
        {
            WeaponsManager.instance.startPrimaryShotNeeded = false;
            WeaponsManager.instance.primaryShotCoroutine = WeaponsManager.instance.StartCoroutine(WeaponsManager.instance.PrimaryShot());
        }
    }
}
