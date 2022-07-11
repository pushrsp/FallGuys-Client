using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Room;
        
        Debug.Log(Managers.Scene.Scene);
    }
}
