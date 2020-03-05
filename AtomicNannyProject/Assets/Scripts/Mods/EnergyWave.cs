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

    public override float GetFireRate()
    {
        return cooldown;
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override IEnumerator Shot()
    {
        if (reloading)
        {
            WeaponsManager.instance.startSecondaryShotNeeded = true;
            yield break;
        }
        display = true;
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

    void DoExplosion()
    {
        StartCoroutine(ReloadSystem());
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
