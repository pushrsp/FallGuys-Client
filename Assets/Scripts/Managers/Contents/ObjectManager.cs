using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController Me { get; set; }

    public Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _obstacles = new Dictionary<int, GameObject>();
    private int _obstacleId = 1;

    public void Add(ObstacleController obs, GameObject go)
    {
        obs.ObstacleId = _obstacleId++;
        _obstacles.Add(obs.ObstacleId, go);
    }

    public void Add(PlayerInfo info, bool me = false)
    {
        GameObject go = Managers.Resource.Instantiate($"Players/{info.PlayerSelect}");
        go.name = info.Name;
        _objects.Add(info.ObjectId, go);

        if (me)
        {
            Me = go.GetOrAddComponent<MyPlayerController>();
            Me.Speed = info.Speed;
            Me.Info = info;
            Me.SyncPos(new Vector3(info.PosInfo.PosX, info.PosInfo.PosY, info.PosInfo.PosZ));
        }
        else
        {
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.Speed = info.Speed;
            pc.Info = info;
            pc.SyncPos(new Vector3(info.PosInfo.PosX, info.PosInfo.PosY, info.PosInfo.PosZ));
        }
    }

    public GameObject FindById(int objectId)
    {
        GameObject go;
        if (_objects.TryGetValue(objectId, out go) == false)
            return null;

        return go;
    }

    public GameObject FindObstacleById(int obstacleId)
    {
        GameObject go;
        if (_obstacles.TryGetValue(obstacleId, out go) == false)
            return null;

        return go;
    }

    public void Remove(int objectId)
    {
        GameObject go = FindById(objectId);
        if (go == null)
            return;

        _objects.Remove(objectId);
        Managers.Resource.Destroy(go);
    }
}