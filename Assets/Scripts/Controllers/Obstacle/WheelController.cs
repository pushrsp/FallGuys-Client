using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : ObstacleController
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

    void Update()
    {
        transform.Rotate(new Vector3(0,(100 * x) * Time.deltaTime,0));
    }
}
