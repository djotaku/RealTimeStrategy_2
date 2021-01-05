using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    // Start is called before the first frame update
    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // not using update because we want ot make sure the camera has rotated before we do this
    private void LateUpdate()
    {
        transform.LookAt(
            transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.transform.rotation * Vector3.up);
    }

}
