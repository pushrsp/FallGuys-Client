using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : ObstacleController
{
    public Vector3 Pos { get; set; }
    
    void Update()
    {
        transform.position = Pos;
    }
}
