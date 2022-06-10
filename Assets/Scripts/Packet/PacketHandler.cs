using Core;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void S_ChatHandler(PacketSession session, IMessage packet)
    {
        S_Chat chatPacket = packet as S_Chat;

        Debug.Log(chatPacket.Context);
    }
}