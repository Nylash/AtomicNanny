using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    #region CONFIGURATION
    [Header("CONFIGURATION")]
#pragma warning disable 0649
    [SerializeField] float normalSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
#pragma warning restore 0649
    #endregion

    #region COMPONENTS
    [Header("COMPONENTS")]
    ControlsMap controlsMap;
    CharacterController controller;
    Animator anim;
    #endregion

    #region VARIABLES
    [Header("VARIABLES")]
    public MovementState currentMovementState;
    Vector2 movementDirection;
    Vector2 dashDirection;
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

        controlsMap.Gameplay.Movement.performed += ctx => ReadMovementInput(ctx);
        controlsMap.Gameplay.Movement.canceled += ctx => movementDirection = Vector2.zero;
        controlsMap.Gameplay.Dash.started += ctx => StartCoroutine(Dash());

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!WeaponsWheelManager.instance.wheelOpen)
        {
            switch (currentMovementState)
            {
                case MovementState.moving:
                    controller.Move(new Vector3(movementDirection.x, 0, movementDirection.y) * normalSpeed * Time.deltaTime);
                    anim.SetFloat("InputX", movementDirection.x);
                    anim.SetFloat("InputY", movementDirection.y);
                    break;
                case MovementState.firing:
                    break;
                case MovementState.dashing:
                    controller.Move(new Vector3(dashDirection.x, 0, dashDirection.y) * dashSpeed * Time.deltaTime);
                    anim.SetFloat("InputX", dashDirection.x);
                    anim.SetFloat("InputY", dashDirection.y);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Dash()
    {
        if (!WeaponsWheelManager.instance.wheelOpen)
        {
            currentMovementState = MovementState.dashing;
            if (movementDirection != Vector2.zero)
                dashDirection = movementDirection;
            yield return new WaitForSeconds(dashTime);
            currentMovementState = MovementState.moving;
        }
    }

    void ReadMovementInput(InputAction.CallbackContext ctx)
    {
        if (!WeaponsWheelManager.instance.wheelOpen)
        {
            movementDirection = ctx.ReadValue<Vector2>();
            dashDirection = ctx.ReadValue<Vector2>();
        }
    }

    public enum MovementState
    {
        moving, firing, dashing
    }
}
