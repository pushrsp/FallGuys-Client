using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene
    {
        get => GameObject.FindObjectOfType<BaseScene>();
    }

    public GameState Scene { get; private set; } = GameState.Login;

    public void LoadScene(GameState type)
    {
        Scene = type;
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(GameState type)
    {
        string name = System.Enum.GetName(typeof(GameState), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}