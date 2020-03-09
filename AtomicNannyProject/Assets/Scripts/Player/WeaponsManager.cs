using UnityEngine;
using System.Collections;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    public bool usingGamepad; //usingGamepad will be eventually remplace by a check of PlayerInput.currentScheme when this feature will be corrected in the NewInputSystem
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
    public Coroutine primaryShotCoroutine;
    public Coroutine secondaryShotCoroutine;
    public bool startPrimaryShotNeeded;
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
        controlsMap.Gameplay.Shot.started += ctx => StartPrimaryShot();
        controlsMap.Gameplay.Shot.canceled += ctx => StopPrimaryShot();
        controlsMap.Gameplay.SecondaryShot.started += ctx => StartSecondaryShot();
        controlsMap.Gameplay.SecondaryShot.canceled += ctx => StopSecondaryShot();

        aimDirection = transform.right;
    }

    //Instantiate the current mod to be sure it exists
    private void Start()
    {
        WeaponsStats.instance.InstantiateMod(currentWeapon);
    }

    //Update aimGuide position on each frame depending on the input
    //cf com on usingGamepad variable
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

    //Method call when the play press PrimaryShot input, the result is that a Coroutine will be started, depending on if the weapon is stanced or not
    //If not, we start the PrimaryShot Coroutine otherwise we start SecondaryShot Coroutine with parameter true to explicit this is call from PrimaryShot input
    //This way, instead of changing weapon's stance we call the mod's Shot method
    //Before starting the coroutine, we stop the SecondaryShot Coroutine if it is started
    public void StartPrimaryShot()
    {
        if (!WeaponsWheelManager.instance.wheelOpen && !waitEndSpecificBehaviour)
        {
            if (WeaponsStats.instance.IsStanced(currentWeapon))
            {
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                primaryShotCoroutine = StartCoroutine(SecondaryShot(true));
                PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
            }
            else
            {
                primaryShotCoroutine = StartCoroutine(PrimaryShot());
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                startSecondaryShotNeeded = false;
                PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
            }
        }
    }

    //When the input is release we stop primaryShotCoroutine, and we don't care if the weapon is stanced or not, because this variable always contains the right coroutine
    void StopPrimaryShot()
    {
        if (!waitEndSpecificBehaviour)
        {
            if (primaryShotCoroutine != null)
                StopCoroutine(primaryShotCoroutine);
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.moving;
            startPrimaryShotNeeded = false;
            PlayerMovementManager.instance.recoil = Vector3.zero;
        }
    }

    //Here we simply start SecondaryShot coroutine and stop PrimaryShot coroutine if it was running
    public void StartSecondaryShot()
    {
        if (!WeaponsWheelManager.instance.wheelOpen && !waitEndSpecificBehaviour)
        {
            secondaryShotCoroutine = StartCoroutine(SecondaryShot());
            if(primaryShotCoroutine != null)
                StopCoroutine(primaryShotCoroutine);
            startPrimaryShotNeeded = false;
            PlayerMovementManager.instance.currentMovementState = PlayerMovementManager.MovementState.firing;
        }
    }

    //The SecondaryShot input is release so we stop related Coroutine
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

    //Coroutine handling PrimaryShot behaviour depending on currentWeapon
    public IEnumerator PrimaryShot()
    {
        switch (currentWeapon)
        {
            //Minigun has a specific case to simulate the charging time of minigun weapon
            //Dividing by 2 then 3 then 6 seems to be the same amount as GetTimeBeforeFirstShoot
            //There is no check for reloading because the fire is so small we don't take care of it
            case Weapons.minigun:
                float time = WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon);
                yield return new WaitForSeconds(time / 2);
                CreateBullet(false);
                yield return new WaitForSeconds(time / 3);
                CreateBullet(false);
                yield return new WaitForSeconds(time / 6);
                CreateBullet(false);
                //Then while there is enough amunition to shot again we shot again
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateBullet(false);
                }
            //The raygun works the same way of others weapons but it shots ray instead of bullet so it needs a specific case
            //If the weapon is realoding we cancel the coroutine but we pass a bool at true, so at the end of the reload if it's still true the coroutine will be recall
            //The boolean is pass to false when the player release the input
            case Weapons.raygun:
                if (WeaponsStats.instance.IsReloading(currentWeapon))
                {
                    startPrimaryShotNeeded = true;
                    yield break;
                }
                yield return new WaitForSeconds(WeaponsStats.instance.GetTimeBeforeFirstShoot(currentWeapon));
                CreateRay();
                while (true)
                {
                    yield return new WaitForSeconds(WeaponsStats.instance.GetFireRate(currentWeapon));
                    CreateRay();
                }
            //Same case as the raygun but creating bullet instead of ray
            default:
                if (WeaponsStats.instance.IsReloading(currentWeapon))
                {
                    startPrimaryShotNeeded = true;
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

    //Here we test if the mod is a stanceMod, if it is the case and it isn't called by PrimaryShot input we simply change currentWeapon's stance
    //Otherwise, we return the coroutine define in the mod's script
    public IEnumerator SecondaryShot(bool calledFromPrimary = false)
    {
        if (WeaponsStats.instance.GetEquippedMod(currentWeapon))
        {
            if (WeaponsStats.instance.GetEquippedMod(currentWeapon).isStanceMod && !calledFromPrimary)
            {
                WeaponsStats.instance.ChangeStance(currentWeapon);
                if (primaryShotCoroutine != null)
                    StopCoroutine(primaryShotCoroutine);
                if (secondaryShotCoroutine != null)
                    StopCoroutine(secondaryShotCoroutine);
                yield break;
            }
            else
            {
                yield return WeaponsStats.instance.GetEquippedMod(currentWeapon).ModShot();
            }
        }
    }

    //This method has a big if, but the difference between true or false is only the reference, to the scriptMod or to currentWeapon, besides this, this is the exact same thing
    //We begin by putting the weapon in reaload, then we got a loop which will create as many protectile as define by the weapon/mod
    //We instantiate the projectile and get the script reference for it
    //We calcul a shotDirection modify by the innacuracyAngle (and depending on current input Mouse or Pad) cf com on usingGamepad variable
    //We start Recoil coroutine (see below)
    //We add all the variables the script need, such as speed, direction, damage...
    //And finally we activate the bullet
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

    //It works pretty in the same way as CreateBullet just we don't care about innacuracyAngle and we need to do a raycast to know if the ray go full range or is stop by an obstacle
    //Besides this we pass all required variables to the ray's script then we activate the ray
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

    //Here instantiate the RayObject and pass out some references
    void InstantiateRay(Vector3 endPos, out GameObject rayRef, out RayBehaviour rayScriptRef)
    {
        rayRef = Instantiate(WeaponsStats.instance.GetProjectile(currentWeapon), aimGuide.position, aimGuide.rotation);
        rayScriptRef = rayRef.GetComponent<RayBehaviour>();
        rayScriptRef.endPosition = endPos;
        rayScriptRef.damage = WeaponsStats.instance.GetDamage(currentWeapon);
    }

    //This coroutine handle Recoil, it is simply a movement in the opposite direction of shoot
    //There to step, one shorter but stronger and another longer but softer
    //Then after some time we fix the recoil to 0 to stop the movement
    IEnumerator Recoil(Vector3 shootDirection)
    {
        PlayerMovementManager.instance.recoil = -shootDirection * WeaponsStats.instance.GetRecoilSpeed(currentWeapon);
        yield return new WaitForSeconds(.1f);
        PlayerMovementManager.instance.recoil = -shootDirection * WeaponsStats.instance.GetRecoilSpeed(currentWeapon) / 2;
        yield return new WaitForSeconds(.2f);
        PlayerMovementManager.instance.recoil = Vector3.zero;
    }

    //Method used to change currentWeapon, we stop all coroutine set some bool to false and instance the mod to get a reference to it if needed
    public void ChangeWeapon(Weapons newWeapon)
    {
        if(newWeapon != currentWeapon)
        {
            if(primaryShotCoroutine != null)
                StopCoroutine(primaryShotCoroutine);
            if(secondaryShotCoroutine != null)
                StopCoroutine(secondaryShotCoroutine);
            startPrimaryShotNeeded = false;
            startSecondaryShotNeeded = false;
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
