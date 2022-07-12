using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class RoomScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = GameState.Room;
        Managers.UI.ShowSceneUI<UI_RoomScene>();
    }
}