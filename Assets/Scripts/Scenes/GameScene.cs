using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = GameState.Game;

        Managers.Map.LoadStage(Managers.Object.StageId);

        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
    }

    void Start()
    {
        //TODO: 준비 완료 패킷 보내기
        C_EnterGameRoom enterGameRoomPacket = new C_EnterGameRoom();
        Managers.Network.Send(enterGameRoomPacket);
    }
}