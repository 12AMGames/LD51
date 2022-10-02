using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //UI
    [Header("UI")]
    [SerializeField] GameObject handsUI;
    public Animator handAnim;

    //Mouse
    [Header("Mouse")]
    [SerializeField] float mouseSen;
    [SerializeField] float mouseSenMultiplyer = 1;
    private float xRotation;
    [SerializeField] Camera cam;
    Vector2 mousePos;
    float xRot = 0;

    //Movement
    [Header("Movement")]
    private Controls controls;

    [SerializeField]
    float maxSpeed = 100f, acceleration = 10f, airAccDamp = 1f, maxAccForce = 150f;
    [SerializeField]
    float wallBounceIntensity = 1f;
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
     GameObject sodaPrefab;
    [SerializeField]
    int kickDamage = 1;

    [SerializeField]
    float kickForce;
    [SerializeField]
    float kickRadius = 1f;
    [SerializeField]
    private float attackCoolDownTime = 1f;
    private float attackCoolDown = 0f;

    public bool canThrow = false;

    [SerializeField]
    Transform kickTransform;
    [SerializeField]
    LayerMask kickableLayer;

    private void Awake()
    {
        GameObject ui = GameObject.FindGameObjectWithTag("UI");

        GameObject obj = Instantiate(handsUI, ui.transform);
        obj.transform.SetSiblingIndex(0);
        handAnim = obj.GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        controls = new Controls();
        controls.Player.Kick.performed += _ => Kick();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Look.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => mousePos = ctx.ReadValue<Vector2>();
        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => moveInput = _.ReadValue<Vector2>();
        controls.Player.Escape.performed += _ => GameManager.Instance.PauseGame();

        mouseSenMultiplyer = PlayerPrefs.GetFloat("mSen", 1);
    }

    private void Update()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
        //Jumping
        isGrounded = Physics.CheckSphere(groudCheckPos.position, groundCheckRad, ground);
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerLook();
    }

    void PlayerMove()
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
            if (isGrounded)
            {
                rb.drag = 0;
                isStopped = false;
            }      
        }
        else
        {
            if (isGrounded)
            {
                rb.drag = 10;
                isStopped = true;
            }
        }

        rb.AddForce(forceNeeded);
    }

    void Jump()
    {
        if (!isGrounded)
            return;
        AudioManager.PlaySound(SoundNames.PlayerGrunt);
        rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);
    }

    void Kick()
    {
        if (!canThrow)
        {
            if (attackCoolDown <= 0)
            {
                handAnim.SetTrigger("PlayerAttack");
                Collider[] objects = Physics.OverlapSphere(kickTransform.position, kickRadius, kickableLayer);
                foreach (Collider col in objects)
                {

                    Enemy kick = col.GetComponent<Enemy>();
                    if (kick == null)
                    {
                        Debug.Log(col);
                        return;
                    }
                    kick.DamageEnemy(kickDamage);
                }
                attackCoolDown = attackCoolDownTime;
            }
        }
        else
        {
            handAnim.SetBool("HasSoda", false);
            handAnim.SetTrigger("PlayerAttack");
            Instantiate(sodaPrefab, kickTransform.position + (transform.forward * 2), transform.rotation);
            canThrow = false;
        }
       
    }

    float desiredX;

    private void PlayerLook()
    {
        if (cam == null)
            return;
        float mouseX = mousePos.x * Time.fixedDeltaTime * mouseSen * mouseSenMultiplyer;
        float mouseY = mousePos.y * Time.fixedDeltaTime * mouseSen * mouseSenMultiplyer;

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

    public void Knockback(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;
        dir += Vector3.up * 5;
        rb.AddForce(dir, ForceMode.Impulse);
        //velGoal = dir;
    }

    public void Setsen(float value)
    {
        mouseSenMultiplyer = value;
        PlayerPrefs.SetFloat("mSen", value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Mathf.Abs(Vector3.Dot(collision.GetContact(0).normal, Vector3.down)) < 0.5f)
        {
            rb.velocity = new Vector3 (0, rb.velocity.y, 0);
            velGoal = collision.GetContact(0).normal * wallBounceIntensity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groudCheckPos.position, groundCheckRad);
        Gizmos.DrawWireSphere(kickTransform.position, kickRadius);
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
