﻿using UnityEngine;
using System.Collections;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    public bool usingGamepad;
    public Transform aimGuide;
    public LayerMask enemiesMask;
#pragma warning disable 0649
    [SerializeField] LayerMask rayMask;
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
    public Vector2 aimDirection;
    public Coroutine shotCoroutine;
    public Coroutine secondaryShotCoroutine;
    public bool startShotNeeded;
    public bool startSecondaryShotNeeded;
    public bool waitEndSpecificBehaviour;
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
        controlsMap.Gameplay.Shot.started += ctx => StartShooting(false);
        controlsMap.Gameplay.Shot.canceled += ctx => StopShooting();
        controlsMap.Gameplay.SecondaryShot.started += ctx => StartSecondaryShot(false);
        controlsMap.Gameplay.SecondaryShot.canceled += ctx => StopSecondaryShot();

        aimDirection = transform.right;
    }

    private void Start()
    {
        WeaponsStats.instance.InstantiateMod(currentWeapon);
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
        if (!WeaponsWheelManager.instance.wheelOpen && !waitEndSpecificBehaviour)
        {
            if (WeaponsStats.instance.IsStanced(currentWeapon))
            {
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                shotCoroutine = StartCoroutine(SecondaryShot(true));
                if (!externalCall)
                    PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
            }
            else
            {
                shotCoroutine = StartCoroutine(Shoot());
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                startSecondaryShotNeeded = false;
                if (!externalCall)
                    PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
            }
        }
    }

    void StopShooting()
    {
        if (!waitEndSpecificBehaviour)
        {
            if (shotCoroutine != null)
                StopCoroutine(shotCoroutine);
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            startShotNeeded = false;
            PlayerMovementManager.instance.recoil = Vector3.zero;
        }
    }

    public void StartSecondaryShot(bool externalCall)
    {
        if (!WeaponsWheelManager.instance.wheelOpen && !waitEndSpecificBehaviour)
        {
            secondaryShotCoroutine = StartCoroutine(SecondaryShot());
            if(shotCoroutine != null)
                StopCoroutine(shotCoroutine);
            startShotNeeded = false;
            if (!externalCall)
                PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
        }
    }

    void StopSecondaryShot()
    {
        if (!waitEndSpecificBehaviour)
        {
            if (secondaryShotCoroutine != null)
                StopCoroutine(secondaryShotCoroutine);
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            startSecondaryShotNeeded = false;
            PlayerMovementManager.instance.recoil = Vector3.zero;
        }
    }

    public IEnumerator Shoot()
    {
        switch (currentWeapon)
        {
            case Weapons.minigun:
                float time = WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon);
                yield return new WaitForSeconds(time / 2);
                CreateBullet(false);
                yield return new WaitForSeconds(time / 3);
                CreateBullet(false);
                yield return new WaitForSeconds(time / 6);
                CreateBullet(false);
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateBullet(false);
                }
            case Weapons.raygun:
                if (WeaponsStats.instance.IsReloading(currentWeapon))
                {
                    startShotNeeded = true;
                    yield break;
                }
                yield return new WaitForSeconds(WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon));
                CreateRay();
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateRay();
                }
            default:
                if (WeaponsStats.instance.IsReloading(currentWeapon))
                {
                    startShotNeeded = true;
                    yield break;
                }
                yield return new WaitForSeconds(WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon));
                CreateBullet(false);
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateBullet(false);
                }
        }
    }

    public IEnumerator SecondaryShot(bool calledFromPrimary = false)
    {
        if (WeaponsStats.instance.GetEquippedMod(currentWeapon))
        {
            if (WeaponsStats.instance.GetEquippedMod(currentWeapon).isStanceMod && !calledFromPrimary)
            {
                WeaponsStats.instance.ChangeStance(currentWeapon);
                if (shotCoroutine != null)
                    StopCoroutine(shotCoroutine);
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                yield break;
            }
            else
            {
                yield return WeaponsStats.instance.GetEquippedMod(currentWeapon).Shot();
            }
        }
    }

    public void CreateBullet(bool isMod)
    {
        if (isMod)
        {
            WeaponMod currentMod = WeaponsStats.instance.GetEquippedMod(currentWeapon);
            currentMod.StartCoroutine(currentMod.ReloadSystem());
            for (int i = 0; i < currentMod.GetProjectileByShoot(); i++)
            {
                GameObject bulletRef = Instantiate(currentMod.GetProjectile(), aimGuide.position, aimGuide.rotation);
                bulletRef.transform.localScale *= currentMod.GetProjectileSize();
                ProjectileBehaviour bulletScriptRef = bulletRef.GetComponent<ProjectileBehaviour>();
                if (usingGamepad)
                {
                    float randomAngle = Random.Range(-currentMod.GetInaccuracyAngle(), currentMod.GetInaccuracyAngle());
                    Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y) * Vector3.Distance(transform.position, new Vector3(aimDirection.x, 0, aimDirection.y));
                    bulletScriptRef.direction = shootDirection.normalized;
                    StartCoroutine(Recoil(shootDirection.normalized));
                }
                else
                {
                    float randomAngle = Random.Range(-currentMod.GetInaccuracyAngle(), currentMod.GetInaccuracyAngle());
                    Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z) * Vector3.Distance(transform.position, new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z));
                    bulletScriptRef.direction = shootDirection.normalized;
                    StartCoroutine(Recoil(shootDirection.normalized));
                }
                bulletScriptRef.speed = currentMod.GetProjectileSpeed();
                bulletScriptRef.range = currentMod.GetRange();
                if (currentWeapon == Weapons.flameThrower)
                    bulletScriptRef.isFlame = true;
                bulletScriptRef.damage = currentMod.GetDamage();
                bulletScriptRef.splashDamage = currentMod.GetSplashDamage();
                bulletScriptRef.splashDamageRadius = currentMod.GetSplashDamageRadius();
                bulletScriptRef.enemyKnockback = currentMod.GetEnemyKnockback();
                bulletRef.SetActive(true);
            }
        }
        else
        {
            WeaponsStats.instance.StartReloadSystem(currentWeapon);
            for (int i = 0; i < WeaponsStats.instance.GetProjectileByShoot(currentWeapon); i++)
            {
                GameObject bulletRef = Instantiate(WeaponsStats.instance.GetProjectile(currentWeapon), aimGuide.position, aimGuide.rotation);
                bulletRef.transform.localScale *= WeaponsStats.instance.GetProjectileSize(currentWeapon);
                ProjectileBehaviour bulletScriptRef = bulletRef.GetComponent<ProjectileBehaviour>();
                if (usingGamepad)
                {
                    float randomAngle = Random.Range(-WeaponsStats.instance.GetInaccuracyAngle(currentWeapon), WeaponsStats.instance.GetInaccuracyAngle(currentWeapon));
                    Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y) * Vector3.Distance(transform.position, new Vector3(aimDirection.x, 0, aimDirection.y));
                    bulletScriptRef.direction = shootDirection.normalized;
                    StartCoroutine(Recoil(shootDirection.normalized));
                }
                else
                {
                    float randomAngle = Random.Range(-WeaponsStats.instance.GetInaccuracyAngle(currentWeapon), WeaponsStats.instance.GetInaccuracyAngle(currentWeapon));
                    Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z) * Vector3.Distance(transform.position, new Vector3(aimDirectionMouse.x, 0, aimDirectionMouse.z));
                    bulletScriptRef.direction = shootDirection.normalized;
                    StartCoroutine(Recoil(shootDirection.normalized));
                }
                bulletScriptRef.speed = WeaponsStats.instance.GetProjectileSpeed(currentWeapon);
                bulletScriptRef.range = WeaponsStats.instance.GetRange(currentWeapon);
                if (currentWeapon == Weapons.flameThrower)
                    bulletScriptRef.isFlame = true;
                bulletScriptRef.damage = WeaponsStats.instance.GetDamage(currentWeapon);
                bulletScriptRef.splashDamage = WeaponsStats.instance.GetSplashDamage(currentWeapon);
                bulletScriptRef.splashDamageRadius = WeaponsStats.instance.GetSplashDamageRadius(currentWeapon);
                bulletScriptRef.enemyKnockback = WeaponsStats.instance.GetEnemyKnockback(currentWeapon);
                bulletRef.SetActive(true);
            }
        }
    }

    void CreateRay()
    {
        WeaponsStats.instance.StartReloadSystem(currentWeapon);
        GameObject rayRef;
        RayBehaviour rayScriptRef;
        if (usingGamepad)
        {
            RaycastHit hit;
            Vector3 fullRangePos = aimGuide.position + (Vector3)aimDirection * WeaponsStats.instance.GetRange(currentWeapon);
            fullRangePos.y = aimGuide.position.y;
            if (Physics.Linecast(aimGuide.position, fullRangePos, out hit, rayMask))
            {
                InstantiateRay(hit.point, out rayRef, out rayScriptRef);
            }
            else
            {
                InstantiateRay(fullRangePos, out rayRef, out rayScriptRef);
            }
        }
        else
        {
            RaycastHit hit;
            Vector3 fullRangePos = aimGuide.position + (Vector3)aimDirectionMouse * WeaponsStats.instance.GetRange(currentWeapon);
            fullRangePos.y = aimGuide.position.y;
            if (Physics.Linecast(aimGuide.position, fullRangePos, out hit, rayMask))
            {
                InstantiateRay(hit.point, out rayRef, out rayScriptRef);
            }
            else
            {
                InstantiateRay(fullRangePos, out rayRef, out rayScriptRef);
            }
        }
        rayScriptRef.width = WeaponsStats.instance.GetProjectileSize(currentWeapon);
        rayRef.SetActive(true);
    }

    void InstantiateRay(Vector3 endPos, out GameObject rayRef, out RayBehaviour rayScriptRef)
    {
        rayRef = Instantiate(WeaponsStats.instance.GetProjectile(currentWeapon), aimGuide.position, aimGuide.rotation);
        rayScriptRef = rayRef.GetComponent<RayBehaviour>();
        rayScriptRef.endPosition = endPos;
        rayScriptRef.damage = WeaponsStats.instance.GetDamage(currentWeapon);
    }

    IEnumerator Recoil(Vector3 shootDirection)
    {
        PlayerMovementManager.instance.recoil = -shootDirection * WeaponsStats.instance.GetRecoilSpeed(currentWeapon);
        yield return new WaitForSeconds(.1f);
        PlayerMovementManager.instance.recoil = -shootDirection * WeaponsStats.instance.GetRecoilSpeed(currentWeapon) / 2;
        yield return new WaitForSeconds(.2f);
        PlayerMovementManager.instance.recoil = Vector3.zero;
    }

    public void ChangeWeapon(Weapons newWeapon)
    {
        if(newWeapon != currentWeapon)
        {
            if(shotCoroutine != null)
                StopCoroutine(shotCoroutine);
            if(secondaryShotCoroutine != null)
                StopCoroutine(secondaryShotCoroutine);
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            startShotNeeded = false;
            currentWeapon = newWeapon;
            if (PlayerMovementManager.instance.currentMovementState == PlayerMovementManager.MovementState.firing)
                PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            WeaponsStats.instance.InstantiateMod(currentWeapon);
        }
    }

    public enum Weapons
    {
        pistol, shotgun, minigun, raygun, plasmaRifle, rocket, flameThrower
    }
}
