using System.Collections;
using UnityEngine;

public class AtomicBall : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float cooldown;
    public float timeBeforeShoot;
    public float damage;
    public float splashDamage;
    public float splashDamageRadius;
    public float ammoCons;
    public float projectileSpeed;
    public float projectileSize;
    public GameObject projectile;

    //Override GET_METHODS for specifics statistics
    #region GET_METHODS
    public override float GetFireRate()
    {
        return cooldown;
    }

    public override float GetTimeBeforeFirstShoot()
    {
        return timeBeforeShoot;
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override float GetSplashDamage()
    {
        return splashDamage;
    }

    public override float GetSplashDamageRadius()
    {
        return splashDamageRadius;
    }

    public override float GetAmmunitionConso()
    {
        return ammoCons;
    }

    public override float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public override float GetProjectileSize()
    {
        return projectileSize;
    }

    public override GameObject GetProjectile()
    {
        return projectile;
    }
    #endregion

    public override IEnumerator ModShot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso(), GetAmmoType()))
        {
            yield return new WaitForSeconds(GetTimeBeforeFirstShoot());
            AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
            WeaponsManager.instance.CreateBullet(true);
            while (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso(), GetAmmoType()))
            {
                yield return new WaitForSeconds(GetFireRate());
                AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
                WeaponsManager.instance.CreateBullet(true);
            }
        }
        print("not enough ammo");
        //Not enough ammo
    }
}
