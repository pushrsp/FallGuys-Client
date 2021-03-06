using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = GameState.Login;
        Managers.UI.ShowSceneUI<UI_LoginScene>();
    }
}