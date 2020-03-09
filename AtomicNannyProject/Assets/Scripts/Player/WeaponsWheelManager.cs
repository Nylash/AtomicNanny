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
    [SerializeField] Image arrow;
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
            //Since the script WeaponsManager is blocked during the wheel we need to get the input directly here (cf usingGamepad com on WeaponsManager)
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
                if (!arrow.enabled)
                    arrow.enabled = true;

                //Once we get the inputDirection we calculate an angle to know where it is, then depending on this angle, we know which weapon is selected (the space is divided in seven like a pizza)
                float angle = Vector2.SignedAngle(new Vector2(1, 0), inputDirection);
                arrow.transform.eulerAngles= new Vector3(0, 0, angle - 90 );

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
            else
            {
                if (arrow.enabled)
                    arrow.enabled = false;
            }
        }
    }

    //Here we highlight the selected weapon and stock a reference to it
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
        if (!WeaponsManager.instance.waitEndSpecificBehaviour)
        {
            wheelOpen = true;
            weaponsWheel.enabled = true;
        }
    }

    //When we close the wheel if there is a selected weapon we say to WeaponsManager to equip it
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

    //This method is used to reset to 0 the input if the player release the pad, this way there is no strange behaviour
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
