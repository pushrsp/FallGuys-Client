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
        S_EnterGame enterPacket = packet as S_EnterGame;
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;
    }
}