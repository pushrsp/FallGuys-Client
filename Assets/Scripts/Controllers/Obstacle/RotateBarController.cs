using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class RotateBarController : ObstacleController
{
    public float YPos { get; set; }

    [SerializeField] private Dir _dir;

    private int x = 1;

    void Start()
    {
        switch (_dir)
        {
            case Dir.Left:
                x = 1;
                break;
            case Dir.Right:
                x = -1;
                break;
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, YPos * x, 0));
    }
}