using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Mouse
    [Header("Mouse")]
    private float xRotation;
    [SerializeField] float mouseSen;
    [SerializeField] Camera cam;
    Vector2 mousePos;
    float xRot = 0;

    //Movement
    [Header("Movement")]
    private Controls controls;

    [SerializeField]
    float maxSpeed = 100f, acceleration = 10f, airAccDamp = 1f, maxAccForce = 150f;
    [SerializeField]
    float stopMovementDamper = 1f;
    float stopFromVelTimer = 0f;
    bool isStopped = false;


    Vector3 velGoal;
    Vector3 desiredVel;
    private Vector2 moveInput;
    private Vector3 slopeMoveDir;
    private RaycastHit slopeHit;
    private Vector3 moveDirection;
    private Rigidbody rb;

    //Jump
    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 15;
    [SerializeField]
    private Transform groudCheckPos;
    [SerializeField]
    private float groundCheckRad;
    [SerializeField]
    private LayerMask ground;
    private bool isGrounded;

    [Header("Kick")]
    [SerializeField]
    float kickForce;
    [SerializeField]
    Transform kickTransform;
    [SerializeField]
    float kickRadius = 1f;
    [SerializeField]
    LayerMask kickableLayer;

    //Crouch
    [Header("Crouch")]
    [SerializeField]
    private CapsuleCollider capCollider;
    [SerializeField]
    private float crouchAmount = 2;
    private bool isCrouching;

    private void Awake()
    {
        controls = new Controls();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        controls.Player.Kick.performed += _ => Kick();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Look.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => mousePos = ctx.ReadValue<Vector2>();
        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => moveInput = _.ReadValue<Vector2>();
    }

    private void Update()
    {
        //Jumping
        isGrounded = Physics.CheckSphere(groudCheckPos.position, groundCheckRad, ground);
    }

    private void FixedUpdate()
    {
        PLayerMove();
        PlayerLook();
    }

    void PLayerMove()
    {
        //Movement
        Vector3 localMoveInput = Vector3.ClampMagnitude(moveInput, 1f);
        moveDirection = transform.right * localMoveInput.x + transform.forward * localMoveInput.y;

        Vector3 goalVel = moveDirection * maxSpeed;

        float maxSpedChange = !isGrounded ? acceleration * airAccDamp * Time.fixedDeltaTime : acceleration *  Time.fixedDeltaTime;

        velGoal = Vector3.MoveTowards(velGoal, goalVel, maxSpedChange);

        Vector3 forceNeeded = (velGoal - rb.velocity) / Time.fixedDeltaTime;

        float maxAccel = maxAccForce;

   
        forceNeeded = Vector3.ClampMagnitude(new Vector3(forceNeeded.x, 0, forceNeeded.z), maxAccel);

        if (localMoveInput.sqrMagnitude > 0)
        {
            isStopped = false;
        }
        else
        {
            isStopped = true;
        }

        rb.AddForce(forceNeeded);
    }

    void Jump()
    {
        if (!isGrounded)
            return;
        rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);
    }

    void Kick()
    {
        Collider[] objects = Physics.OverlapSphere(kickTransform.position, kickRadius, kickableLayer);
        foreach (Collider col in objects)
        {
           
            Kickable kick = col.GetComponent<Kickable>();
            if (kick == null)
            {
                Debug.Log(col);
                return;
            }
            kick.Kick(transform.forward, kickForce);
        }
    }

    float desiredX;

    private void PlayerLook()
    {
        if (cam == null)
            return;
        float mouseX = mousePos.x * Time.fixedDeltaTime * mouseSen;
        float mouseY = mousePos.y * Time.fixedDeltaTime * mouseSen;

        //Find current look rotation
        Vector3 rot = cam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        cam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groudCheckPos.position, groundCheckRad);
        Gizmos.DrawRay(groudCheckPos.position, Vector3.down);
       
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}
