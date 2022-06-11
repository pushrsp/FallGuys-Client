using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController Me { get; set; }

    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

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
            Me.SyncPos();
        }
        else
        {
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.Speed = info.Speed;
            pc.Info = info;
            pc.SyncPos();
        }
    }

    public GameObject FindById(int objectId)
    {
        GameObject go;
        if (_objects.TryGetValue(objectId, out go) == false)
            return null;

        return go;
    }
}