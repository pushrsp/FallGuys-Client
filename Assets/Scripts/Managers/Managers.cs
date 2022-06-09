using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private static Managers Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    #region Contents

    private MapManager _map = new MapManager();

    public static MapManager Map
    {
        get => Instance._map;
    }

    #endregion

    #region Core

    private ResourceManager _resource = new ResourceManager();

    public static ResourceManager Resource
    {
        get => Instance._resource;
    }

    #endregion

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject {name = "@Managers"};
                _instance = go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
        }
    }
}