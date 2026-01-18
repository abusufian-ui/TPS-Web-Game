using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    [Header("States")]
    public AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();

    [Header("Input & Camera")]
    [SerializeField] float mouseSense = 1;
    [SerializeField] Transform camFollowPos;
    float xAxis, yAxis;

    [HideInInspector] public Animator anim;
    [HideInInspector] public CinemachineVirtualCamera vCam;
    
    [Header("FOV Settings")]
    public float adsFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    [Header("Aiming Logic")]
    public Transform aimPos;
    [HideInInspector] public Vector3 actualAimPosition;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;

    [Header("Shoulder Swap & Movement")]
    [SerializeField] float shoulderSwapSpeed = 10;
    
    // UPDATED: Optimal Heights
    [SerializeField] float standingCamHeight = 0.1f; // Idle State
    [SerializeField] float crouchCamHeight = 0f;   // Crouch State
    
    // THESE ARE YOUR OPTIMAL POSITIONS
    [SerializeField] float rightShoulderX = -0.5f; 
    [SerializeField] float leftShoulderX = -1.5f;
    
    // Internal position tracking
    float xFollowPos;
    float yFollowPos;
    float zFollowPos; 
    
    MovementStateManager moving;

    void Start()
    {
        moving = GetComponent<MovementStateManager>();
        
        // Ensure we start at the defined Right Shoulder position
        xFollowPos = rightShoulderX; 
        
        // Ensure we start at the defined Standing Height (0.1)
        yFollowPos = standingCamHeight;
        
        zFollowPos = camFollowPos.localPosition.z; 

        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponent<Animator>();

        SwitchState(Hip);
    }

    void Update()
    {
        // 1. Input
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        // 2. FOV
        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        // 3. Aim Raycast
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);

        // 4. Camera Movement
        MoveCamera();

        // 5. State Update
        currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void MoveCamera()
    {
        // Shoulder Swap Logic (Toggle between -0.5 and -1.5)
        if (Input.GetKeyDown(KeyCode.LeftAlt)) 
        {
            if (xFollowPos == rightShoulderX) xFollowPos = leftShoulderX;
            else xFollowPos = rightShoulderX;
        }

        // Crouch Logic (Using your optimal values)
        if (moving.currentState == moving.Crouch) 
        {
            yFollowPos = -crouchCamHeight; // 0.5
        }
        else 
        {
            yFollowPos = standingCamHeight; // 0.1
        }

        // Apply Position
        Vector3 newFollowPos = new Vector3(xFollowPos, yFollowPos, zFollowPos);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPos, shoulderSwapSpeed * Time.deltaTime);
    }
}