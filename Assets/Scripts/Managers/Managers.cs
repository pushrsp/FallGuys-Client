using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    public static Managers Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    #region Contents

    private MapManager _map = new MapManager();
    private NetworkManager _network = new NetworkManager();
    private ObjectManager _object = new ObjectManager();
    private WebManager _web = new WebManager();

    public static MapManager Map
    {
        get => Instance._map;
    }

    public static NetworkManager Network
    {
        get => Instance._network;
    }

    public static ObjectManager Object
    {
        get => Instance._object;
    }

    public static WebManager Web
    {
        get => Instance._web;
    }

    #endregion

    #region Core

    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private UIManager _ui = new UIManager();


    public static ResourceManager Resource
    {
        get => Instance._resource;
    }

    public static SceneManagerEx Scene
    {
        get => Instance._scene;
    }

    public static UIManager UI
    {
        get => Instance._ui;
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

    void Update()
    {
        _network.Update();
    }
}