using System.Collections;
using UnityEngine;

public class SplitShot : WeaponMod
{
    //[Header("SPECIFIC CONFIGURATION")]

    public override IEnumerator Shot()
    {
        float time = GetTimeBeforeFirstShoot();
        yield return new WaitForSeconds(time / 2);
        CreateBullet();
        yield return new WaitForSeconds(time / 3);
        CreateBullet();
        yield return new WaitForSeconds(time / 6);
        CreateBullet();
        while (true)
        {
            yield return new WaitForSeconds(GetFireRate());
            CreateBullet();
        }
    }

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
            bulletRef.SetActive(true);
        }
    }
}
