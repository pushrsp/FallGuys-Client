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

        if (me)
        {
            MyPlayerController mc = go.GetOrAddComponent<MyPlayerController>();
            mc.Info = info;
            Debug.Log($"({info.PosInfo.PosY}, {info.PosInfo.PosZ}, {info.PosInfo.PosX})");
            mc.SyncPos();
        }
        else
        {
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.Info = info;
        }
    }
}