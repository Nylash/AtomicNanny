using System.Collections;
using UnityEngine;

public class DoubleTap : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float timeBetweenShots;


    //Simply add a second shot after the first to do the double tap effect
    //I multiply by 2 GetAmmoConso because the DoubleTap shoot twice
    public override IEnumerator ModShot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
        {
            yield return new WaitForSeconds(GetTimeBeforeFirstShoot());
            AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
            WeaponsManager.instance.CreateBullet(true);
            yield return new WaitForSeconds(timeBetweenShots);
            AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
            WeaponsManager.instance.CreateBullet(true);
            while (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
            {
                yield return new WaitForSeconds(GetFireRate());
                AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
                WeaponsManager.instance.CreateBullet(true);
                yield return new WaitForSeconds(timeBetweenShots);
                AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
                WeaponsManager.instance.CreateBullet(true);
            }
        }
        print("not enough ammo");
        //Not enough ammo
    }
}
