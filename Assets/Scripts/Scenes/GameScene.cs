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
        List<ObstacleController> rotateObs =
            Helper.FindChildrenByTag<ObstacleController>(obstacles.gameObject, "Obstacle");

        foreach (ObstacleController obs in rotateObs)
            Managers.Object.Add(obs, obs.gameObject);

        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
    }
}