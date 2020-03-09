using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWave : WeaponMod
{
    [Header("SPECIFIC CONFIGURATION")]
    public float cooldown;
    public float damage;
    public float explosionRadius;
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
    #endregion

    //Call DoExplosion and display a Gizmos for 1 second (waiting for FX)
    public override IEnumerator ModShot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        display = true;
        StartCoroutine(ReloadSystem());
        DoExplosion();
        yield return new WaitForSeconds(1);
        display = false;
        while (true)
        {
            yield return new WaitForSeconds(GetFireRate());
            display = true;
            DoExplosion();
            yield return new WaitForSeconds(1);
            display = false;
        }
    }

    //Simply do a OverlapSphere and apply damage to all enemies in the sphere
    void DoExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(WeaponsManager.instance.aimGuide.position, explosionRadius, WeaponsManager.instance.enemiesMask);
        foreach (Collider item in colliders)
        {
            Enemy scriptRef = item.gameObject.GetComponent<Enemy>();
            scriptRef.TakeDamage(damage);
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
