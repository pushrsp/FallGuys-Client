using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class RotateBarController : ObstacleController
{
    enum Dir
    {
        Left,
        Right
    }

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

    private float runningTime = 0.0f;
    private float yPos = 0.0f;
    void Update()
    {
        runningTime += 70.0f * Time.deltaTime;
        if (runningTime > 360.0f)
            runningTime = 0;
        
        transform.rotation = Quaternion.Euler(new Vector3(0,runningTime * x,0));
        // Debug.Log(Mathf.Abs(Mathf.Cos(runningTime) * 360));
    }
}
