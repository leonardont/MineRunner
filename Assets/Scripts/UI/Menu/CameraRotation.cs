using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime);
    }
}
