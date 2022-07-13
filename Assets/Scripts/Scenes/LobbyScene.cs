using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = GameState.Lobby;
    }

    void Start()
    {
        C_EnterRoom enterRoomPacket = new C_EnterRoom();
        enterRoomPacket.RoomIdx = Managers.Room.EnterRoomIdx;

        Managers.Network.Send(enterRoomPacket);
    }
}