// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Cinemachine; // 1. Added namespace so you don't need to type 'Cinemachine.' every time

// public class AimStateManager : MonoBehaviour
// {
//     public AxisState xAxis, yAxis; // 2. Removed 'Cinemachine.' prefix (cleaner)
//     [SerializeField] Transform camFollowPos;

//     void Start()
//     {
//         // CRITICAL FOR WEB: Locks the mouse to the center of the screen
//         // so the player can look around without clicking outside the canvas.
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//     }

//     void Update()
//     {
//         xAxis.Update(Time.deltaTime);
//         yAxis.Update(Time.deltaTime);
//     }

//     private void LateUpdate()
//     {
//         // ERROR FIX: 'vector3' changed to 'Vector3'
//         camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        
//         // ERROR FIX: 'xAxis.value' changed to 'xAxis.Value'
//         transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AimStateManager : MonoBehaviour
{
    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    void Start()
    {
        
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis,-80, 80);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,xAxis,transform.eulerAngles.z);
    }
}