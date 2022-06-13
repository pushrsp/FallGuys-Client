using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        GameObject stage = Managers.Map.LoadStage(1);
        Transform obstacles = Helper.FindChild<Transform>(stage, "Obstacles");
        List<RotateBarController> rotateObs = Helper.FindChildren<RotateBarController>(obstacles.gameObject, "obs9");

        foreach (RotateBarController ro in rotateObs)
            Managers.Object._rotateObs.Add(ro);

        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
    }
}