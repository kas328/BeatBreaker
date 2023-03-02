using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    #region Serialize Field
    [Range(0.1f, 5f)][SerializeField] private float _normalSpeed = 3f;
    #endregion

    #region Private Field
    private FrameController[] _frames = null;
    #endregion

    private void Start()
    {
        _frames = GetComponentsInChildren<FrameController>();

        foreach (FrameController frame in _frames)
        {
            frame.RotationSpeed = Mathf.Clamp(Random.Range(_normalSpeed - 0.5f, _normalSpeed + 0.5f), 0.1f, 5f);
            frame.RoatateAxis = Random.Range(0, 2) == 0 ? Vector3.forward : Vector3.back;
        }
    }
}