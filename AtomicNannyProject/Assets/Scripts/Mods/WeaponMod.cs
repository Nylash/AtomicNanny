using System.Collections;
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

    //Define GET_METHODS for mod statistics
    #region GET_METHODS
    public virtual float GetTimeBeforeFirstShoot()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).timeBeforeFirstShoot;
    }

    public virtual float GetFireRate()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).fireRate;
    }

    public virtual float GetDamage()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).damage;
    }

    public virtual float GetSplashDamage()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).splashDamage;
    }

    public virtual float GetSplashDamageRadius()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).splashDamageRadius;
    }

    public virtual float GetInaccuracyAngle()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).inaccuracyAngle;
    }

    public virtual float GetAmmunitionConso()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).ammunitionConso;
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

    public float GetEnemyKnockback()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).enemyKnockback;
    }

    public AmmunitionManager.AmmoType GetAmmoType()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).ammunition;
    }

    public virtual GameObject GetProjectile()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).projectile;
    }

    public virtual bool IsReloading()
    {
        return WeaponsStats.instance.GetWeapon(attachedWeapon.ToString()).reloading;
    }
    #endregion

    //Same reload system than for the primary shot, but if the mod is a StanceMod we call SecondaryShot as an PrimaryCall, because it's when the player will do the input for primary call that we call the SecondaryShot
    //This behaviour simply remplace the call from PrimaryInput by SecondaryShot when the weapon is stanced
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
                    WeaponsManager.instance.startSecondaryShotNeeded = false;
                    WeaponsManager.instance.primaryShotCoroutine = WeaponsManager.instance.StartCoroutine(WeaponsManager.instance.SecondaryShot(true));
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

    //This method must be define by every mod
    public abstract IEnumerator ModShot();
}
