﻿using System.Collections;
using UnityEngine;

public class AtomicBall : WeaponMod
{
    [Header("STATS")]
    public float fireRate;
    public float timeBeforeFirstShoot;
    public float damage;
    public float splashDamage;
    public float splashDamageRadius;
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