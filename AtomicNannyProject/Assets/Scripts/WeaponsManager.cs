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
    public Vector3 aimDirectionMouse;
    public bool startShootNeeded;
    Vector2 aimDirection;
    Vector2 mousePosition;
    
    
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
        controlsMap.Gameplay.Shoot.started += ctx => StartShooting(false);
        controlsMap.Gameplay.Shoot.canceled += ctx => StopShooting();

        aimDirection = transform.right;
    }

    private void Update()
    {
        if (!WeaponsWheelManager.instance.wheelOpen)
        {
            if (usingGamepad)
            {
                aimGuide.position = transform.position + new Vector3(aimDirection.x, -.5f, aimDirection.y);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 target = ray.GetPoint(distance);
                    aimDirectionMouse = target - transform.position;
                    aimDirectionMouse.Normalize();
                    aimGuide.position = transform.position + new Vector3(aimDirectionMouse.x, -.5f, aimDirectionMouse.z);
                }
            }   
        }
    }

    public void StartShooting(bool externalCall)
    {
        StartCoroutine("Shoot");
        if(!externalCall)
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
    }

    void StopShooting()
    {
        StopCoroutine("Shoot");
        PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
        startShootNeeded = false;
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
                if (WeaponsStats.instance.IsReloading(currentWeapon))
                {
                    startShootNeeded = true;
                    yield break;
                }
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
        WeaponsStats.instance.StartReloadSystem(currentWeapon);
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
                Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z) * Vector3.Distance(transform.position, new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z));
                bulletRefScript.direction = shootDirection.normalized;
            }
            bulletRefScript.speed = WeaponsStats.instance.GetProjectileSpeed(currentWeapon);
            bulletRefScript.range = WeaponsStats.instance.GetRange(currentWeapon);
            bulletRef.SetActive(true);
        }
    }

    public void ChangeWeapon(Weapons newWeapon)
    {
        if(newWeapon != currentWeapon)
        {
            StopCoroutine("Shoot");
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            startShootNeeded = false;
            currentWeapon = newWeapon;
            if (PlayerMovementManager.instance.currentMovementState == PlayerMovementManager.MovementState.firing)
                PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
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
