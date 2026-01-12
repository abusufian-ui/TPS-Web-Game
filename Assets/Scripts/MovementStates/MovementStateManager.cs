using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float WalkSpeed = 3, WalkBackSpeed = 2;
    public float RunSpeed = 7, RunBackSpeed = 5;
    public float CrouchSpeed = 2, CrouchBackSpeed = 1;
    
    [HideInInspector] public float moveSpeed; // Unused now, safe to ignore
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Animator anim;

    [Header("Gravity & Ground Settings")]
    [SerializeField] float groundYOffset = 0.2f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    
    Vector3 velocity;
    Vector3 spherePos;
    CharacterController controller;

    MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update()
    {
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
}