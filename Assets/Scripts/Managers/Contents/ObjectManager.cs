using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public string Token { get; set; }
    public string Id { get; set; }
    public string Username { get; set; }
    public MyPlayerController Me { get; set; }

    public Dictionary<string, GameObject> _objects = new Dictionary<string, GameObject>();
    private Dictionary<int, GameObject> _obstacles = new Dictionary<int, GameObject>();

    public void Add(int obstacleId, ObstacleType type)
    {
        GameObject go = Managers.Map.GetObstacles(type);
        if (go == null)
            return;

        switch (type)
        {
            case ObstacleType.Rotate:
                RotateBarController rc = go.GetComponent<RotateBarController>();
                rc.ObstacleId = obstacleId;
                _obstacles.Add(rc.ObstacleId, go);
                break;
            case ObstacleType.Pendulum:
                PendulumController pc = go.GetComponent<PendulumController>();
                pc.ObstacleId = obstacleId;
                _obstacles.Add(pc.ObstacleId, go);
                break;
        }
    }

    public void Add(PlayerInfo info, bool me = false)
    {
        GameObject go = Managers.Resource.Instantiate($"Players/{info.PlayerSelect}");
        go.name = info.Name;
        _objects.Add(info.ObjectId, go);

        if (me)
        {
            go.tag = "Me";
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

    public GameObject FindById(string objectId)
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

    public void Remove(string objectId)
    {
        GameObject go = FindById(objectId);
        if (go == null)
            return;

        _objects.Remove(objectId);
        Managers.Resource.Destroy(go);
    }
}