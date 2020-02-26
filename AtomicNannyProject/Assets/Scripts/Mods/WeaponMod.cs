using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class WeaponMod : MonoBehaviour
{
    [Header("CONFIGURATION")]
    public new string name;
    public WeaponsManager.Weapons attachedWeapon;
    public bool isStanceMod;
    [Header("RUNNING VARIABLES")]
    public bool reloading;

    public virtual float GetTimeBeforeFirstShoot()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).timeBeforeFirstShoot;
    }

    public virtual float GetFireRate()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).fireRate;
    }

    public virtual int GetDamage()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).damage;
    }

    public virtual int GetSplashDamage()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).splashDamage;
    }

    public virtual int GetInaccuracyAngle()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).inaccuracyAngle;
    }

    public virtual float GetAmmunitionsConso()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).ammunitionsConso;
    }

    public virtual float GetRecoilSpeed()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).recoilSpeed;
    }

    public virtual int GetProjectileByShoot()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).projectileByShoot;
    }

    public virtual float GetRange()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).range;
    }

    public virtual float GetProjectileSpeed()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).projectileSpeed;
    }

    public virtual float GetProjectileSize()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).projectileSize;
    }

    public virtual GameObject GetProjectile()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).projectile;
    }

    public virtual bool IsReloading()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).reloading;
    }

    public virtual IEnumerator ReloadSystem()
    {
        reloading = true;
        yield return new WaitForSeconds(Mathf.Abs(GetFireRate() - GetTimeBeforeFirstShoot()));
        reloading = false;
        switch (isStanceMod)
        {
            case true :
                if (WeaponsManager.instance.startSecondaryShotNeeded)
                {
                    //WeaponsStats.instance.StartParallelReload(attachedWeapon);
                    WeaponsManager.instance.startSecondaryShotNeeded = false;
                    WeaponsManager.instance.shotCoroutine = WeaponsManager.instance.StartCoroutine(WeaponsManager.instance.SecondaryShot(true));
                }
                break;
            case false :
                if (WeaponsManager.instance.startSecondaryShotNeeded)
                {
                    WeaponsManager.instance.startSecondaryShotNeeded = false;
                    WeaponsManager.instance.secondaryShotCoroutine = WeaponsManager.instance.StartCoroutine(WeaponsManager.instance.SecondaryShot());
                }
                break;
        }
    }

    public abstract IEnumerator Shot();
}
