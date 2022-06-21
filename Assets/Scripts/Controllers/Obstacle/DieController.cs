using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class DieController : ObstacleController
{
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Me":
                C_Die diePacket = new C_Die();
                Managers.Network.Send(diePacket);
                break;
        }
    }
}
