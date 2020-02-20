using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    public bool usingGamepad;
#pragma warning disable 0649
    [SerializeField] Transform aimGuide;
#pragma warning restore 0649
    #endregion

    #region COMPONENTS
    [Header("COMPONENTS")]
    ControlsMap controlsMap;
    #endregion


    #region VARIABLES
    [Header("VARIABLES")]
    public Weapons currentWeapon;
    public Vector2 aimDirection;
    public Vector2 mousePosition;
    #endregion

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.AimDirection.performed += ctx => aimDirection = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.MousePos.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.Shoot.started += ctx => StartShooting();
        controlsMap.Gameplay.Shoot.canceled += ctx => StopShooting();
    }

    private void Update()
    {
        if (!WeaponsWheelManager.instance.wheelOpen)
        {
            if(usingGamepad)
            {
                aimGuide.position = transform.position + new Vector3(aimDirection.x, -.5f, aimDirection.y);
            }
            else
            {
                mousePosition -= (Vector2)Camera.main.WorldToScreenPoint(new Vector2(transform.position.x,transform.position.z));
                mousePosition.Normalize();
                aimGuide.position = transform.position + new Vector3(mousePosition.x, -.5f, mousePosition.y);
            }
        }
    }

    void StartShooting()
    {
        StartCoroutine("Shoot");
    }

    void StopShooting()
    {
        StopCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        switch (currentWeapon)
        {
            case Weapons.minigun:
                float time = WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon);
                yield return new WaitForSeconds(time / 2);
                CreateBullet();
                yield return new WaitForSeconds(time / 3);
                CreateBullet();
                yield return new WaitForSeconds(time / 6);
                CreateBullet();
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateBullet();
                }
            //case Weapons.raygun:
            //case Weapons.flameThrower:
            default:
                yield return new WaitForSeconds(WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon));
                CreateBullet();
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateBullet();
                }
        }
    }

    void CreateBullet()
    {
        for (int i = 0; i < WeaponsStats.instance.GetProjectileByShoot(currentWeapon); i++)
        {
            GameObject bulletRef = Instantiate(WeaponsStats.instance.GetProjectile(currentWeapon), aimGuide.position, aimGuide.rotation);
            bulletRef.transform.localScale *= WeaponsStats.instance.GetProjectileSize(currentWeapon);
            Projectile bulletRefScript = bulletRef.GetComponent<Projectile>();
            int random = Random.Range(0, 100);
            if (usingGamepad)
            {
                float randomAngle = Random.Range(-WeaponsStats.instance.GetInaccuracyAngle(currentWeapon), WeaponsStats.instance.GetInaccuracyAngle(currentWeapon));
                Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y) * Vector3.Distance(transform.position, new Vector3(aimDirection.x, 0, aimDirection.y));
                bulletRefScript.direction = shootDirection.normalized;
            }
            else
            {
                float randomAngle = Random.Range(-WeaponsStats.instance.GetInaccuracyAngle(currentWeapon), WeaponsStats.instance.GetInaccuracyAngle(currentWeapon));
                Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(mousePosition.x, 0, mousePosition.y) * Vector3.Distance(transform.position, new Vector3(mousePosition.x, 0, mousePosition.y));
                bulletRefScript.direction = shootDirection.normalized;
            }
            bulletRefScript.speed = WeaponsStats.instance.GetProjectileSpeed(currentWeapon);
            bulletRefScript.range = WeaponsStats.instance.GetRange(currentWeapon);
            bulletRef.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(WeaponsStats.instance)
            Gizmos.DrawWireSphere(transform.position, WeaponsStats.instance.GetRange(currentWeapon));
    }

    public enum Weapons
    {
        pistol, shotgun, minigun, raygun, plasmaRifle, rocket, flameThrower
    }
}
