using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrateFlame : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    [Range(0, 45)]
    public float inaccuracyAngle;
    public float range;
    public float projectileSpeed;

    public override float GetInaccuracyAngle()
    {
        return inaccuracyAngle;
    }

    public override float GetRange()
    {
        return range;
    }

    public override float GetProjectileSpeed()
    {
        return projectileSpeed;
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
