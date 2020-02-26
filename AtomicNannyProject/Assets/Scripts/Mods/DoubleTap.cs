using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap : WeaponMod
{
    [Header("STATS")]
    public float timeBetweenShots;

    public override IEnumerator Shot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        yield return new WaitForSeconds(GetTimeBeforeFirstShoot());
        WeaponsManager.instance.CreateBullet(true);
        yield return new WaitForSeconds(timeBetweenShots);
        WeaponsManager.instance.CreateBullet(true);
        while (true)
        {
            yield return new WaitForSeconds(GetFireRate());
            WeaponsManager.instance.CreateBullet(true);
            yield return new WaitForSeconds(timeBetweenShots);
            WeaponsManager.instance.CreateBullet(true);
        }
    }
}
