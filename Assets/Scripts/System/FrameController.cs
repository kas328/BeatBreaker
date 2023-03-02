using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    #region Private Field
    public float RotationSpeed { get; set; } = 0f;
    public Vector3 RoatateAxis { get; set; } = Vector3.forward;
    #endregion

    private void Update()
    {
        RotateFrame();
    }

    private void RotateFrame()
    {
        transform.Rotate(RoatateAxis * RotationSpeed * Time.deltaTime);
    }
}
