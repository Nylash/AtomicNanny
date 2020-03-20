using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWave : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float cooldown;
    public float damage;
    public float explosionRadius;
    public float ammoCons;
    public float ammoGainByHit;
    public GameObject visualEffect;

    //temporary
    bool display;

    //Override GET_METHODS for specifics statistics
    #region GET_METHODS
    public override float GetFireRate()
    {
        return cooldown;
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override float GetAmmunitionConso()
    {
        return ammoCons;
    }

    public override float GetAmmunitionGain()
    {
        return ammoGainByHit;
    }
    #endregion

    //Call DoExplosion and display a Gizmos for 1 second (waiting for FX)
    //The is no Wait before1stShot because we want it to be instantaneous
    public override IEnumerator ModShot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso(), GetAmmoType()))
        {
            AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
            display = true;
            StartCoroutine(ReloadSystem());
            DoExplosion();
            yield return new WaitForSeconds(1);
            display = false;
            while (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso(), GetAmmoType()))
            {
                yield return new WaitForSeconds(GetFireRate());
                AmmunitionManager.instance.UseAmmo(GetAmmunitionConso(), GetAmmoType());
                display = true;
                DoExplosion();
                yield return new WaitForSeconds(1);
                display = false;
            }
        }
        print("not enough ammo");
        //Not enough ammo
    }

    //Simply do a OverlapSphere and apply damage to all enemies in the sphere
    void DoExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(WeaponsManager.instance.aimGuide.position, explosionRadius, WeaponsManager.instance.enemiesMask);
        foreach (Collider item in colliders)
        {
            Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
            scriptRef.TakeDamage(damage, GetAmmunitionGain(), WeaponsStats.instance.GetAmmoType(attachedWeapon));
            //knockback
        }
        //throw fx
    }

    private void OnDrawGizmos()
    {
        if (display)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(WeaponsManager.instance.aimGuide.position, explosionRadius);
        }
    }
}
