using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicBall : WeaponMod
{
    [Header("STATS")]
    public float fireRate;
    public float timeBeforeFirstShoot;
    public int damage;
    public int splashDamage;
    public float projectileSpeed;
    public float projectileSize;
    public GameObject projectile;

    public override float GetFireRate()
    {
        return fireRate;
    }

    public override float GetTimeBeforeFirstShoot()
    {
        return timeBeforeFirstShoot;
    }

    public override int GetDamage()
    {
        return damage;
    }

    public override int GetSplashDamage()
    {
        return splashDamage;
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

    public override IEnumerator Shot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        yield return new WaitForSeconds(GetTimeBeforeFirstShoot());
        WeaponsManager.instance.CreateBullet(true);
        while (true)
        {
            yield return new WaitForSeconds(GetFireRate());
            WeaponsManager.instance.CreateBullet(true);
        }
    }
}
