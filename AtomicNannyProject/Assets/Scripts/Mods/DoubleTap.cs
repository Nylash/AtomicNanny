using System.Collections;
using UnityEngine;

public class DoubleTap : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float timeBetweenShots;


    //Simply add a second shot after the first to do the double tap effect
    public override IEnumerator ModShot()
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
