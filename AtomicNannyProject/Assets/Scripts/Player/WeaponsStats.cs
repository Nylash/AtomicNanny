using UnityEngine;

public class WeaponsStats : MonoBehaviour
{
    public static WeaponsStats instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    //All the values are directly configure in the inspector
#pragma warning disable 0649
    [SerializeField] Weapon pistol;
    [SerializeField] Weapon shotgun;
    [SerializeField] Weapon minigun;
    [SerializeField] Weapon raygun;
    [SerializeField] Weapon plasmaRifle;
    [SerializeField] Weapon rocket;
    [SerializeField] Weapon flameThrower;
#pragma warning restore 0649
    [Header("DOT CONFIGURATION")]
    public float numberOfTicks;
    public float intervalBtwTicks;
    public float firstTickReducingRatio;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    //All method used to get statistics of the specified weapon
    #region GET_METHOD
    public float GetTimeBeforeFirstShoot(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).timeBeforeFirstShoot;
    }

    public float GetFireRate(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).fireRate;
    }

    public float GetDamage(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).damage;
    }

    public float GetSplashDamage(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).splashDamage;
    }

    public float GetSplashDamageRadius(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).splashDamageRadius;
    }

    public float GetInaccuracyAngle(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).inaccuracyAngle;
    }

    public float GetAmmunitionConso(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).ammunitionConsoByShot;
    }

    public float GetAmmunitionGain(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).ammunitionGainByHit;
    }

    public float GetRecoilSpeed(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).recoilSpeed;
    }

    public int GetProjectileByShoot(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileByShoot;
    }

    public float GetRange(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).range;
    }

    public float GetProjectileSpeed(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileSpeed;
    }

    public float GetProjectileSize(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectileSize;
    }

    public float GetEnemyKnockback(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).enemyKnockback;
    }

    public AmmunitionManager.AmmoType GetAmmoType(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).ammunition;
    }

    public GameObject GetProjectile(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).projectile;
    }

    public bool IsReloading(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).reloading;
    }

    public void StartReloadSystem(WeaponsManager.Weapons weapon)
    {
        StartCoroutine(GetWeapon(weapon.ToString()).ReloadSystem());
    }

    public WeaponMod GetEquippedMod(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).equippedMod;
    }

    public void ChangeStance(WeaponsManager.Weapons weapon)
    {
        GetWeapon(weapon.ToString()).isStanced = !GetWeapon(weapon.ToString()).isStanced;
    }

    public bool IsStanced(WeaponsManager.Weapons weapon)
    {
        return GetWeapon(weapon.ToString()).isStanced;
    }
    #endregion

    //Method used to instantiate the currendWeapon's mod, if there is one, to get a reference to it
    public void InstantiateMod(WeaponsManager.Weapons weapon)
    {
        if (!GetEquippedMod(weapon))
        {
            if (GetWeapon(weapon.ToString()).objectMod){
                GameObject mod = Instantiate(GetWeapon(weapon.ToString()).objectMod);
                GetWeapon(weapon.ToString()).equippedMod = mod.GetComponent<WeaponMod>();
            }
        }
    }

    //Method used to get a reference to the Weapon class from a string
    public Weapon GetWeapon(string name)
    {
        switch (name)
        {
            case "pistol":
                return pistol;
            case "shotgun":
                return shotgun;
            case "minigun":
                return minigun;
            case "raygun":
                return raygun;
            case "plasmaRifle":
                return plasmaRifle;
            case "rocket":
                return rocket;
            case "flameThrower":
                return flameThrower;
            default:
                Debug.LogError("There is no weapon called : " + name);
                return null;
        }
    }
}
