using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObstacleController : BaseController
{
    protected enum Dir
    {
        Left,
        Right
    }

    public int ObstacleId { get; set; }
    public ObstacleType Type { get; set; }
}