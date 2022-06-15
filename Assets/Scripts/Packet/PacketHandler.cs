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

        if (Managers.Object.Me.Id == movePacket.ObjectId)
            return;

        GameObject go = Managers.Object.FindById(movePacket.ObjectId);
        if (go == null)
            return;

        PlayerController pc = go.GetComponent<PlayerController>();
        pc.State = movePacket.State;
        pc.DestPos = new Vector3(movePacket.DestPos.PosX, movePacket.DestPos.PosY, movePacket.DestPos.PosZ);
        pc.MoveDir = new Vector3(movePacket.MoveDir.PosX, movePacket.MoveDir.PosY, movePacket.MoveDir.PosZ);

        if (movePacket.State == PlayerState.Idle)
            pc.SyncPos();
    }

    public static void S_JumpHandler(PacketSession session, IMessage packet)
    {
        S_Jump jumpPacket = packet as S_Jump;

        if (Managers.Object.Me.Id == jumpPacket.ObjectId)
            return;

        GameObject go = Managers.Object.FindById(jumpPacket.ObjectId);
        if (go == null)
            return;
    }
}