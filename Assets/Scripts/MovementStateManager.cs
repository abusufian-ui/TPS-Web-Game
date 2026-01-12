using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    float hzInput, vInput;

    [Header("Gravity & Ground Settings")]
    [SerializeField] float groundYOffset = 0.2f; // Distance from center to feet
    [SerializeField] LayerMask groundMask;       // Layers considered "Ground"
    [SerializeField] float gravity = -9.81f;     // Standard gravity
    
    Vector3 velocity;      // To store vertical speed
    Vector3 spherePos;     // Position for the ground check
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity(); // ERROR FIX: You must call this here!
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // Calculate the position at the bottom of the player
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        
        // ERROR FIX: Fixed spelling of 'controller'
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity over time
        }
        else if (velocity.y < 0)
        {
            // ERROR FIX: Reset velocity when on ground. 
            // We use -2f instead of 0 to ensure the player snaps to the floor.
            velocity.y = -2f; 
        }

        // ERROR FIX: Actually apply the gravity to the controller
        controller.Move(velocity * Time.deltaTime);
    }
    
    // Visualization to help you debug ground checks in the Editor
    void OnDrawGizmos()
    {
        if (controller == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z), controller.radius - 0.05f);
    }
}