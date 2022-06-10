using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Core;
using Google.Protobuf;
using UnityEngine;

public class NetworkManager
{
    ServerSession _session = new ServerSession();

    public void Send(IMessage pkt)
    {
        _session.Send(pkt);
    }

    public void Init()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }
}
