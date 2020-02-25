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
    [SerializeField] Image raygun;
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
    Vector3 mouseDirection;
    Vector2 inputDirection;
    Vector2 mousePosition;
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
        controlsMap.Gameplay.WeaponsSelection.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.WeaponsSelection.canceled += ctx => StopPadInput();
        controlsMap.Gameplay.MousePos.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();

        weaponsWheel = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (wheelOpen)
        {
            if (!WeaponsManager.instance.usingGamepad) {
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 target = ray.GetPoint(distance);
                    mouseDirection = target - WeaponsManager.instance.transform.position;
                    mouseDirection.Normalize();
                    inputDirection = new Vector2(mouseDirection.x, mouseDirection.z);
                }
            }
            if (inputDirection != Vector2.zero)
            {
                float angle = Vector2.SignedAngle(new Vector2(1, 0), inputDirection);
                if (angle < 0)
                    angle = 360 + angle;

                if (angle >= 20 && angle < 75)
                {
                    HighlightWeapon(shotgun);
                }
                else if (angle >= 75 && angle < 105)
                {
                    HighlightWeapon(pistol);
                }
                else if (angle >= 105 && angle < 160)
                {
                    HighlightWeapon(flameThrower);
                }
                else if (angle >= 160 && angle < 215)
                {
                    HighlightWeapon(rocket);
                }
                else if (angle >= 215 && angle < 270)
                {
                    HighlightWeapon(raygun);
                }
                else if (angle >= 270 && angle < 325)
                {
                    HighlightWeapon(plasmaRifle);
                }
                else if (angle >= 325 || angle < 20)
                {
                    HighlightWeapon(minigun);
                }
            }
        }
    }

    void HighlightWeapon(Image weapon)
    {
        if (highlightedWeapon)
        {
            if (highlightedWeapon != weapon)
            {
                highlightedWeapon.color = halfOpacity;
                weapon.color = Color.white;
                highlightedWeapon = weapon;
            }
        }
        else
        {
            weapon.color = Color.white;
            highlightedWeapon = weapon;
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
            WeaponsManager.instance.ChangeWeapon(WeaponsStats.instance.GetWeapon(highlightedWeapon.name).weapon);
            highlightedWeapon.color = halfOpacity;
            highlightedWeapon = null;
        }    
    }

    void StopPadInput()
    {
        inputDirection = Vector2.zero;
        if (highlightedWeapon)
        {
            highlightedWeapon.color = halfOpacity;
            highlightedWeapon = null;
        }
    }
}
