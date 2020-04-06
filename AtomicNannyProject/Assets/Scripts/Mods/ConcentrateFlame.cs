using System.Collections;
using UnityEngine;

public class ConcentrateFlame : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    [Range(0, 45)]
    public float inaccuracyAngle;
    public float range;
    public float projectileSpeed;

    //Override GET_METHODS for specifics statistics
    #region GET_METHODS
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
