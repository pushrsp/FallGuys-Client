using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class RoomManager
{
    public List<RoomInfo> Rooms { get; } = new List<RoomInfo>();
    public int EnterRoomIdx = -1;

    public void Add(RoomInfo info)
    {
        Rooms.Add(info);
        Rooms.Sort((a, b) => a.Idx - b.Idx);
    }

    public void Clear(int idx)
    {
        Rooms.Clear();
        EnterRoomIdx = idx;
    }
}