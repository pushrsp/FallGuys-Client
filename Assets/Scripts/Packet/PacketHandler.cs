using Core;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;

        Managers.Object.Add(enterPacket.PlayerInfo, true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (PlayerInfo p in spawnPacket.PlayerInfo)
            Managers.Object.Add(p);
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawn = packet as S_Despawn;

        foreach (int objectId in despawn.PlayerId)
            Managers.Object.Remove(objectId);
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        if (Managers.Object.Me.Id == movePacket.PlayerInfo.ObjectId)
            return;

        GameObject go = Managers.Object.FindById(movePacket.PlayerInfo.ObjectId);
        if (go == null)
            return;

        PlayerController pc = go.GetComponent<PlayerController>();
        {
            PlayerInfo info = movePacket.PlayerInfo;
            pc.State = info.State;
            pc.Speed = info.Speed;
            pc.PosInfo = new Vector3(info.PosInfo.PosX, info.PosInfo.PosY, info.PosInfo.PosZ);
            pc.MoveDir = new Vector3(info.MoveDir.PosX, info.MoveDir.PosY, info.MoveDir.PosZ);
        }
    }

    public static void S_RotateObsHandler(PacketSession session, IMessage packet)
    {
        S_RotateObs rotateObsPacket = packet as S_RotateObs;
    }
}