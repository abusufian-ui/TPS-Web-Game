// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using Cinemachine; // 1. Added namespace so you don't need to type 'Cinemachine.' every time

// // public class AimStateManager : MonoBehaviour
// // {
// //     public AxisState xAxis, yAxis; // 2. Removed 'Cinemachine.' prefix (cleaner)
// //     [SerializeField] Transform camFollowPos;

// //     void Start()
// //     {
// //         // CRITICAL FOR WEB: Locks the mouse to the center of the screen
// //         // so the player can look around without clicking outside the canvas.
// //         Cursor.lockState = CursorLockMode.Locked;
// //         Cursor.visible = false;
// //     }

// //     void Update()
// //     {
// //         xAxis.Update(Time.deltaTime);
// //         yAxis.Update(Time.deltaTime);
// //     }

// //     private void LateUpdate()
// //     {
// //         // ERROR FIX: 'vector3' changed to 'Vector3'
// //         camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        
// //         // ERROR FIX: 'xAxis.value' changed to 'xAxis.Value'
// //         transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
// //     }
// // }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  
public class AimStateManager : MonoBehaviour
{
    AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();
    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    [HideInInspector] public Animator anim;

    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float adsFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    [HideInInspector] public Vector3 actualAimPosition;
    [SerializeField] float aimSmoothSpeed = 20;
    
    [SerializeField] LayerMask aimMask;


    void Start()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
        anim = GetComponent<Animator>();
        SwitchState(Hip);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis,-80, 80);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView,currentFov, fovSmoothSpeed * Time.deltaTime);
        
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        
        currentState.UpdateState(this);


    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,xAxis,transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}

