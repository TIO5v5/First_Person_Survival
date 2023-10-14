using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private float sensivity = 5f;

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-70f, 80f);

    private Vector2 lookAngles;
    private Vector2 currentMouseLook;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlockCursor();

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LockAndUnlockCursor()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
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
    }

    void LookAround()
    {
        currentMouseLook = new Vector2(
                                        Input.GetAxis("Mouse Y"), 
                                        Input.GetAxis("Mouse X")
                                      );

        lookAngles.x += currentMouseLook.x * sensivity * (invert ? 1 : -1);
        lookAngles.y += currentMouseLook.y * sensivity;

        lookAngles.x = Mathf.Clamp(
                                    lookAngles.x, 
                                    defaultLookLimits.x, 
                                    defaultLookLimits.y
                                  );

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
