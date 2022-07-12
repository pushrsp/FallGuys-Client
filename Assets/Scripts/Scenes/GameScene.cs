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

        Managers.Map.LoadStage(1);

        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
    }
}