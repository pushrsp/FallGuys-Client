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
        List<WheelController> wheelObs = Helper.FindChildren<WheelController>(obstacles.gameObject, "Wheel");

        foreach (RotateBarController ro in rotateObs)
            Managers.Object.Add(ro);
        foreach (WheelController wheel in wheelObs)
            Managers.Object.Add(wheel);

        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
    }
}