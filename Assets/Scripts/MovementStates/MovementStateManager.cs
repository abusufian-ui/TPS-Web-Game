using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementStateManager : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float WalkSpeed = 3, WalkBackSpeed = 2;
    public float RunSpeed = 7, RunBackSpeed = 5;
    public float CrouchSpeed = 2, CrouchBackSpeed = 1;
    float health = 100f;
    float damageAmount = 10f;

    [HideInInspector] public float moveSpeed; // Unused now, safe to ignore
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Animator anim;

    [Header("Gravity & Ground Settings")]
    [SerializeField] float groundYOffset = 0.2f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10f;

    Vector3 velocity;
    Vector3 spherePos;

    [HideInInspector] public CharacterController controller;

    public MovementBaseState currentState;
    public MovementBaseState prevousState; // to track from which state we came

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public JumpStates Jump = new JumpStates();
    public RunState Run = new RunState();

    bool jumped = false;

    public TMPro.TextMeshProUGUI health_display;

    // public MovementBaseState currentState;

    void Start()
    {
        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Makes the cursor invisible
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update()
    {
        display_health();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if it's currently locked
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        GetDirectionAndMove();
        Gravity();

        // Send inputs to Animator
        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        // FIXED: Now using 'currentMoveSpeed' instead of 'moveSpeed'
        controller.Move(dir * currentMoveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2f;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (controller == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z), controller.radius - 0.05f);
    }
    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped() => jumped = true;

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //         Debug.Log("Collided with Enemy");
    //         health -= damageAmount;
    //         if (health <= 0)
    //         {
    //             Die();
    //         }
    //     }
    // }

    void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    void display_health()
    {
        health_display.text = "Health: " + health.ToString();
    }

    // Replace OnCollisionEnter2D with this:
    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.gameObject.CompareTag("Enemy"))
    //     {
    //         // To prevent losing health every single frame the enemy touches you, 
    //         // we check if the enemy is currently in an attacking state.
    //         EnemyAi enemy = hit.gameObject.GetComponent<EnemyAi>();

    //         if (enemy != null)
    //         {
    //             Debug.Log("Player hit by Enemy!");
    //             TakeDamage(10f); // Create a helper function for damage
    //         }
    //     }
    // }

    // public void TakeDamage(float amount)
    // {
    //     health -= amount;
    //     if (health <= 0)
    //     {
    //         Die();
    //     }
    // }

    private void OnTriggerEnter(Collider other)
{
    // Check if the thing hitting us is the enemy's hand (which has the Enemy tag)
    if (other.CompareTag("Enemy"))
    {
        // We check 'alreadyAttacked' logic to ensure we don't take damage 
        // 60 times per second while the hand is overlapping the player.
        TakeDamage(damageAmount);
        Debug.Log("Player Punched! Health: " + health);
    }
}

void TakeDamage(float amount)
{
    health -= amount;
    if (health <= 0)
    {
        Die();
    }
}
}