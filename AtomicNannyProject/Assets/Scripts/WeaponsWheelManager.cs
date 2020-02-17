using UnityEngine;
using UnityEngine.UI;

public class WeaponsWheelManager : MonoBehaviour
{
    public static WeaponsWheelManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
#pragma warning disable 0649
    [SerializeField] Image pistol;
    [SerializeField] Image shotgun;
    [SerializeField] Image minigun;
    [SerializeField] Image plasmaRifle;
    [SerializeField] Image railgun;
    [SerializeField] Image rocket;
    [SerializeField] Image flameThrower;
#pragma warning restore 0649
    #endregion

    #region COMPONENTS
    [Header("COMPONENTS")]
    ControlsMap controlsMap;
    Canvas weaponsWheel;
    #endregion

    #region VARIABLES
    [Header("VARIABLES")]
    public bool wheelOpen;
    Vector2 weaponsSelectionInput;
    public Image highlightedWeapon;
    Color halfOpacity = new Color(1, 1, 1, .5f);
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

        controlsMap.Gameplay.WeaponsWheel.started += ctx => OpenWheel();
        controlsMap.Gameplay.WeaponsWheel.canceled += ctx => CloseWheel();
        controlsMap.Gameplay.WeaponsSelection.performed += ctx => weaponsSelectionInput = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.WeaponsSelection.canceled += ctx => weaponsSelectionInput = Vector2.zero;

        weaponsWheel = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (wheelOpen)
        {
            if (weaponsSelectionInput != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(new Vector2(1, 0), weaponsSelectionInput);
                if (angle < 0)
                    angle = 360 + angle;

                if (angle >= 20 && angle < 75)
                {
                    print("shotgun");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != shotgun)
                        {
                            highlightedWeapon.color = halfOpacity;
                            shotgun.color = Color.white;
                            highlightedWeapon = shotgun;
                        }
                    }
                    else
                    {
                        shotgun.color = Color.white;
                        highlightedWeapon = shotgun;
                    }

                }
                else if (angle >= 75 && angle < 105)
                {
                    print("pistol");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != pistol)
                        {
                            highlightedWeapon.color = halfOpacity;
                            pistol.color = Color.white;
                            highlightedWeapon = pistol;
                        }
                    }
                    else
                    {
                        pistol.color = Color.white;
                        highlightedWeapon = pistol;
                    }
                }
                else if (angle >= 105 && angle < 160)
                {
                    print("flameThrower");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != flameThrower)
                        {
                            highlightedWeapon.color = halfOpacity;
                            flameThrower.color = Color.white;
                            highlightedWeapon = flameThrower;
                        }
                    }
                    else
                    {
                        flameThrower.color = Color.white;
                        highlightedWeapon = flameThrower;
                    }
                }
                else if (angle >= 160 && angle < 215)
                {
                    print("rocket");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != rocket)
                        {
                            highlightedWeapon.color = halfOpacity;
                            rocket.color = Color.white;
                            highlightedWeapon = rocket;
                        }
                    }
                    else
                    {
                        rocket.color = Color.white;
                        highlightedWeapon = rocket;
                    }
                }
                else if (angle >= 215 && angle < 270)
                {
                    print("railgun");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != railgun)
                        {
                            highlightedWeapon.color = halfOpacity;
                            railgun.color = Color.white;
                            highlightedWeapon = railgun;
                        }
                    }
                    else
                    {
                        railgun.color = Color.white;
                        highlightedWeapon = railgun;
                    }
                }
                else if (angle >= 270 && angle < 325)
                {
                    print("plasmaRifle");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != plasmaRifle)
                        {
                            highlightedWeapon.color = halfOpacity;
                            plasmaRifle.color = Color.white;
                            highlightedWeapon = plasmaRifle;
                        }
                    }
                    else
                    {
                        plasmaRifle.color = Color.white;
                        highlightedWeapon = plasmaRifle;
                    }
                }
                else if (angle >= 325 || angle < 20)
                {
                    print("minigun");
                    if (highlightedWeapon)
                    {
                        if (highlightedWeapon != minigun)
                        {
                            highlightedWeapon.color = halfOpacity;
                            minigun.color = Color.white;
                            highlightedWeapon = minigun;
                        }
                    }
                    else
                    {
                        minigun.color = Color.white;
                        highlightedWeapon = minigun;
                    }
                }
            }
        }
    }

    void OpenWheel()
    {
        wheelOpen = true;
        weaponsWheel.enabled = true;
    }

    void CloseWheel()
    {
        wheelOpen = false;
        weaponsWheel.enabled = false;
        if (highlightedWeapon)
        {
            highlightedWeapon.color = halfOpacity;
            highlightedWeapon = null;
        }    
    }
}
