using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : BaseController
{
    protected enum Dir
    {
        Left,
        Right
    }

    public int ObstacleId { get; set; }
}