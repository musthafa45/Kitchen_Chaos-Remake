using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngleLook : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInvert,
        CameraForward,
        CameraForwardInverted,
    }
   [SerializeField]private Mode _mode;
    private void LateUpdate()
    {
        switch(_mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
            break;
            case Mode.LookAtInvert:
                Vector3 DirFromCam = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + DirFromCam);
            break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
            break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
            break;
        }
    }
}
