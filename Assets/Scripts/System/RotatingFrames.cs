using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFrames : MonoBehaviour
{
    [SerializeField] private List<GameObject> frames = null;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float angleGap = 6f;

    private void Start()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            frames[i].transform.rotation =
                frames[i].transform.rotation * Quaternion.Euler(Vector3.forward * (angleGap * i + 1));
        }
    }
    private void Update()
    {
        foreach (GameObject frame in frames)
        {
            frame.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
