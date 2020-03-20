using UnityEngine;
using UnityEngine.UI;

public class AmmunitionManager : MonoBehaviour
{
    public static AmmunitionManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
#pragma warning disable 0649
    [SerializeField] float maxClassicAmmo;
    [SerializeField] float maxAtomicAmmo;
    [SerializeField] float maxExplosiveAmmo;
#pragma warning restore 0649
    #endregion

    [Header("VARIABLES")]
    public float currentClassicAmmo;
    public float currentAtomicAmmo;
    public float currentExplosiveAmmo;

    Image classicAmmoBar;
    Image atomicAmmoBar;
    Image explosiveAmmoBar;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        currentClassicAmmo = maxClassicAmmo;
        currentAtomicAmmo = maxAtomicAmmo;
        currentExplosiveAmmo = maxExplosiveAmmo;

        classicAmmoBar = GameObject.Find("ClassicFill").GetComponent<Image>();
        atomicAmmoBar = GameObject.Find("AtomicFill").GetComponent<Image>();
        explosiveAmmoBar = GameObject.Find("ExplosiveFill").GetComponent<Image>();

        UpdateBarsValues();
    }

    //This method is used to verify if there is enough ammo for a shot
    public bool CheckAmmo(float ammoCons, AmmoType ammoType)
    {
        switch (ammoType)
        {
            case AmmoType.none:
                return true;
            case AmmoType.classic:
                return (currentClassicAmmo - ammoCons < 0) ?  false :  true;
            case AmmoType.atomic:
                return (currentAtomicAmmo - ammoCons < 0) ? false : true;
            case AmmoType.explosive:
                return (currentExplosiveAmmo - ammoCons < 0) ? false : true;
            default:
                Debug.LogError("You can't arrive there.");
                return false;
        }
    }

    //This method is used to consume ammunition
    public void UseAmmo(float ammoCons, AmmoType ammoType)
    {
        switch (ammoType)
        {
            case AmmoType.none:
                break;
            case AmmoType.classic:
                currentClassicAmmo -= ammoCons;
                break;
            case AmmoType.atomic:
                currentAtomicAmmo -= ammoCons;
                break;
            case AmmoType.explosive:
                currentExplosiveAmmo -= ammoCons;
                break;
        }
        UpdateBarsValues();
    }

    //Refill the ammo depending on the ammoType parameter
    public void RefillAmmo(float ammoGain, AmmoType ammoType)
    {
        switch (ammoType)
        {
            case AmmoType.none:
                currentClassicAmmo += ammoGain;
                currentAtomicAmmo += ammoGain;
                currentExplosiveAmmo += ammoGain;
                break;
            case AmmoType.classic:
                currentAtomicAmmo += ammoGain;
                currentExplosiveAmmo += ammoGain;
                break;
            case AmmoType.atomic:
                currentClassicAmmo += ammoGain;
                currentExplosiveAmmo += ammoGain;
                break;
            case AmmoType.explosive:
                currentClassicAmmo += ammoGain;
                currentAtomicAmmo += ammoGain;
                break;
        }
        if (currentClassicAmmo > maxClassicAmmo)
            currentClassicAmmo = maxClassicAmmo;
        if (currentAtomicAmmo > maxAtomicAmmo)
            currentAtomicAmmo = maxAtomicAmmo;
        if (currentExplosiveAmmo > maxExplosiveAmmo)
            currentExplosiveAmmo = maxExplosiveAmmo;
        UpdateBarsValues();

    }

    void UpdateBarsValues()
    {
        classicAmmoBar.fillAmount = currentClassicAmmo / maxClassicAmmo;
        atomicAmmoBar.fillAmount = currentAtomicAmmo / maxAtomicAmmo;
        explosiveAmmoBar.fillAmount = currentExplosiveAmmo / maxExplosiveAmmo;
    }

    public enum AmmoType
    {
        none, classic, atomic, explosive
    }
}
