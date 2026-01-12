using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // 1. Added namespace so you don't need to type 'Cinemachine.' every time

public class AimStateManager : MonoBehaviour
{
    public AxisState xAxis, yAxis; // 2. Removed 'Cinemachine.' prefix (cleaner)
    [SerializeField] Transform camFollowPos;

    void Start()
    {
        // CRITICAL FOR WEB: Locks the mouse to the center of the screen
        // so the player can look around without clicking outside the canvas.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        // ERROR FIX: 'vector3' changed to 'Vector3'
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        
        // ERROR FIX: 'xAxis.value' changed to 'xAxis.Value'
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}