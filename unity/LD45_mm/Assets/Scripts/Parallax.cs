using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;
        lastpos = cam.position;
    }

    public float speedCoefficient;
    Vector3 lastpos;

    void LateUpdate()
    {
        transform.position -= ((lastpos - cam.position) * speedCoefficient);
        lastpos = cam.position;
    }
}
