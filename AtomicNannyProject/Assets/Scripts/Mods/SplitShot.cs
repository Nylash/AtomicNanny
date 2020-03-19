using System.Collections;
using UnityEngine;

public class SplitShot : WeaponMod
{
    //[Header("SPECIFIC CONFIGURATION")]

        //Same behaviour as the primary shot of the minigun
        //I double GetAmmoCons because each time this mod shot 2 projectiles are launched
    public override IEnumerator ModShot()
    {
        float time = GetTimeBeforeFirstShoot();
        if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
        {
            yield return new WaitForSeconds(time / 2);
            AmmunitionManager.instance.UseAmmo(GetAmmunitionConso() * 2, GetAmmoType());
            CreateBullet();
            if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
            {
                yield return new WaitForSeconds(time / 3);
                AmmunitionManager.instance.UseAmmo(GetAmmunitionConso() * 2, GetAmmoType());
                CreateBullet();
                if (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
                {
                    yield return new WaitForSeconds(time / 6);
                    AmmunitionManager.instance.UseAmmo(GetAmmunitionConso() * 2, GetAmmoType());
                    CreateBullet();
                    while (AmmunitionManager.instance.CheckAmmo(GetAmmunitionConso() * 2, GetAmmoType()))
                    {
                        yield return new WaitForSeconds(GetFireRate());
                        AmmunitionManager.instance.UseAmmo(GetAmmunitionConso() * 2, GetAmmoType());
                        CreateBullet();
                    }
                }
            }
        }
        print("not enough ammo");
        //Not enough ammo
    }

    //Same method as one present in WeaponsManager but when the bullet is created I directly create a second one but with the opposite direction
    void CreateBullet()
    {
        StartCoroutine(ReloadSystem());
        for (int i = 0; i < GetProjectileByShoot(); i++)
        {
            GameObject bulletRef = Instantiate(GetProjectile(), WeaponsManager.instance.aimGuide.position, WeaponsManager.instance.aimGuide.rotation);
            bulletRef.transform.localScale *= GetProjectileSize();
            ProjectileBehaviour bulletScriptRef = bulletRef.GetComponent<ProjectileBehaviour>();
            Vector3 shootDirection;
            if (WeaponsManager.instance.usingGamepad)
            {
                float randomAngle = Random.Range(-GetInaccuracyAngle(), GetInaccuracyAngle());
                shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(WeaponsManager.instance.aimDirection.x, 0, WeaponsManager.instance.aimDirection.y) * Vector3.Distance(transform.position, new Vector3(WeaponsManager.instance.aimDirection.x, 0, WeaponsManager.instance.aimDirection.y));
                bulletScriptRef.direction = shootDirection.normalized;
            }
            else
            {
                float randomAngle = Random.Range(-GetInaccuracyAngle(), GetInaccuracyAngle());
                shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(WeaponsManager.instance.aimDirectionMouse.x, 0, WeaponsManager.instance.aimDirectionMouse.z) * Vector3.Distance(transform.position, new Vector3(WeaponsManager.instance.aimDirectionMouse.x, 0, WeaponsManager.instance.aimDirectionMouse.z));
                bulletScriptRef.direction = shootDirection.normalized;
            }
            bulletScriptRef.speed = GetProjectileSpeed();
            bulletScriptRef.range = GetRange();
            bulletScriptRef.damage = GetDamage();
            bulletScriptRef.splashDamage = GetSplashDamage();
            bulletScriptRef.enemyKnockback = GetEnemyKnockback();
            bulletRef.SetActive(true);
            //Opposite shot
            bulletRef = Instantiate(GetProjectile(), WeaponsManager.instance.aimGuide.position, WeaponsManager.instance.aimGuide.rotation);
            bulletRef.transform.localScale *= GetProjectileSize();
            bulletScriptRef = bulletRef.GetComponent<ProjectileBehaviour>();
            bulletScriptRef.direction = -shootDirection.normalized;
            bulletScriptRef.speed = GetProjectileSpeed();
            bulletScriptRef.range = GetRange();
            bulletScriptRef.damage = GetDamage();
            bulletScriptRef.splashDamage = GetSplashDamage();
            bulletScriptRef.enemyKnockback = GetEnemyKnockback();
            bulletScriptRef.ammoType = GetAmmoType();
            bulletRef.SetActive(true);
        }
    }
}
