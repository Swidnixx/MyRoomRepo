using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //  Dependencies
    [SerializeField] Camera cam;
    [SerializeField] CinemachineFreeLook _freeLookComponent;
    public WindowInfo windowInfo { get; private set; }
    public PlayerControls playerControls { get; private set; }      // Decide whether to create it Globally (one instance) or to create dynamically in each dependency ( each dep. has each 'pc' instance )?

    #region Settings Fields
    //      Movement
    [SerializeField]
    private Rigidbody rb;
    [SerializeField, Header("Look updown, leftright and Move input sensivity")]
    float lookUpSpeed = 14;
    [SerializeField]
    float lookLeftSpeed = 14;
    [SerializeField]
    float moveSpeed = 14;
    //      Door Interaction
    [SerializeField]
    bool _HasKey = false;
    public bool HasKey { get { return _HasKey; } } 
    public void SetKey(Key key)
    {
        if (key != null)
            _HasKey = true;
        else
            _HasKey = false;
    }
    //   Private Movement Mechanics Fields
    float rotX = 0;
    #endregion

    #region Dependencies Set Dynamically
    //     My Components
    public CharacterController ch { get; private set; }
    public Animator animator { get; private set; }
    //public CapsuleCollider collider { get; private set; }

    //      Input Actions
    public InputAction lookLeftRight { get; private set; }
    public InputAction lookUpDown { get; private set; }
    public InputAction move { get; private set; }
    public InputAction lookAround { get; private set; }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        // Set Dependencies
        windowInfo = FindObjectOfType<WindowInfo>(true);
        playerControls = new PlayerControls();

        //cam = GetComponentInChildren<Camera>();
        //ch = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        lookLeftRight = playerControls.Player.LookLeftRight;
        lookUpDown = playerControls.Player.LookUpDown;
        move = playerControls.Player.Move;
        lookAround = playerControls.Player.LookAround;

        move.started += OnMoveStarted;
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCancelled;

    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnDisable();
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        CameraMovement();

        PlayerBodyMovement();
    }
    #endregion

    #region Private Methods
    private void CameraMovement()
    {
        float rotY = lookLeftRight.ReadValue<float>() * Time.deltaTime * lookLeftSpeed * 70;
        //Quaternion toRot = transform.rotation * Quaternion.AngleAxis(rotY, Vector3.up);
        Quaternion toRot = transform.rotation * Quaternion.Euler(Vector3.up * rotY);
        //cam.transform.RotateAround(transform.position, Vector3.up, rotY);
        //cam.transform.rotation = Quaternion.LookRotation((transform.position - cam.transform.position));
        //cam.transform.position = transform.position + Quaternion.Euler(Vector3.up * rotY) * ( cam.transform.position - transform.position );
        //print(transform.localPosition);
        rb.MoveRotation(toRot);

        rotX += lookUpDown.ReadValue<float>() * Time.deltaTime * lookUpSpeed;
        rotX = Mathf.Clamp(rotX, -15, 30);
        //cam.transform.rotation = Quaternion.Euler(-rotX, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

        Vector2 rotXY = lookAround.ReadValue<Vector2>();

        //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
        Vector2 lookMovement = rotXY.normalized;
        lookMovement.y = true ? -lookMovement.y : lookMovement.y;

        // This is because X axis is only contains between -180 and 180 instead of 0 and 1 like the Y axis
        lookMovement.x = lookMovement.x * 180f;

        //Adjust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
        _freeLookComponent.m_XAxis.Value += lookMovement.x * lookUpSpeed * Time.deltaTime;
        _freeLookComponent.m_YAxis.Value += lookMovement.y * lookLeftSpeed * Time.deltaTime;
    }

    private void PlayerBodyMovement()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector3 forward = moveInput.y * transform.forward;
        Vector3 right = moveInput.x * transform.right;
        Vector3 moveDisplacement = (forward + right).normalized;

        rb.MovePosition(transform.position + moveDisplacement * moveSpeed);
        //ch.Move(moveDisplacement * Time.deltaTime * moveSpeed);
    }
    #endregion

    #region Input Callbacks
    public void OnEnable()
    {
        playerControls.Enable();
    }
    public void OnDisable()
    {
        playerControls.Disable();
    }
    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        GameManager.instance.animationsManager.PlayerGo();
    }
    private void OnMoveCancelled(InputAction.CallbackContext obj)
    {
        GameManager.instance.animationsManager.PlayerStop();
    }
    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
    }
    #endregion

    #region Interaction Callbacks
    #endregion

    #region Miscellaneous
    #endregion
}
