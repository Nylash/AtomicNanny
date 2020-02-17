using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager instance;

    #region VARIABLES
    [Header("VARIABLES")]
    public Weapons currentWeapon;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public enum Weapons
    {
        pistol, shotgun, minigun, railgun, plasmaRifle, rocket, flameThrower
    }
}
