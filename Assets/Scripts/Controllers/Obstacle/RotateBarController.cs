using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class RotateBarController : ObstacleController
{
    public float YAngle { get; set; }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, YAngle, 0));
    }
}