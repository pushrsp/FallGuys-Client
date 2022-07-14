using Core;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable All

public class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;

        Managers.Object.Add(enterPacket.Player, GameState.Game, true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterPacket = packet as S_EnterGame;
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (PlayerInfo p in spawnPacket.Players)
            Managers.Object.Add(p, GameState.Game);
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawn = packet as S_Despawn;

        foreach (string objectId in despawn.ObjectId)
            Managers.Object.Remove(objectId);
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        if (Managers.Object.Me.ObjectId == movePacket.ObjectId)
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

        if (Managers.Object.Me.ObjectId == jumpPacket.ObjectId)
            return;

        GameObject go = Managers.Object.FindById(jumpPacket.ObjectId);
        if (go == null)
            return;

        PlayerController pc = go.GetComponent<PlayerController>();
        pc.DoJump();
    }

    public static void S_RotateObstacleHandler(PacketSession session, IMessage packet)
    {
        S_RotateObstacle rotateObstaclePacket = packet as S_RotateObstacle;

        GameObject go = Managers.Object.FindObstacleById(rotateObstaclePacket.ObstacleId);
        if (go == null)
            return;

        RotateBarController rc = go.GetComponent<RotateBarController>();
        rc.YAngle = rotateObstaclePacket.YAngle;
    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        Managers.Object.Remove(diePacket.ObjectId);
    }

    public static void S_PendulumObstacleHandler(PacketSession session, IMessage packet)
    {
        S_PendulumObstacle pendulumObstacle = packet as S_PendulumObstacle;

        GameObject go = Managers.Object.FindObstacleById(pendulumObstacle.ObstacleId);
        if (go == null)
            return;

        PendulumController pc = go.GetComponent<PendulumController>();
        pc.Pos = new Vector3(pendulumObstacle.PosInfo.PosX, pendulumObstacle.PosInfo.PosY,
            pendulumObstacle.PosInfo.PosZ);
    }

    public static void S_SpawnObstacleHandler(PacketSession session, IMessage packet)
    {
        S_SpawnObstacle obstaclesPacket = packet as S_SpawnObstacle;

        foreach (ObstacleInfo obstacle in obstaclesPacket.Obstacles)
            Managers.Object.Add(obstacle.ObstacleId, obstacle.Type);
    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        C_Login loginPacket = new C_Login();
        loginPacket.Token = Managers.Object.Token;

        Managers.Network.Send(loginPacket);
    }

    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginOk = packet as S_Login;

        if (loginOk.Success)
        {
            Managers.Object.Id = loginOk.Id;
            Managers.Object.Username = loginOk.Username;

            Managers.Scene.LoadScene(GameState.Room);
            C_EnterGame enterPacket = new C_EnterGame();
            Managers.Network.Send(enterPacket);
        }
    }

    public static void S_RoomListHandler(PacketSession session, IMessage packet)
    {
        S_RoomList roomList = packet as S_RoomList;

        foreach (RoomInfo info in roomList.Rooms)
            Managers.Room.Add(info);

        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();
    }

    public static void S_MakeRoomOkHandler(PacketSession session, IMessage packet)
    {
        S_MakeRoomOk makeRoomPacket = packet as S_MakeRoomOk;

        if (!makeRoomPacket.Success)
            return;

        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();

        Managers.Room.Clear(makeRoomPacket.RoomIdx);
        Managers.Scene.LoadScene(GameState.Lobby);
    }

    public static void S_AddRoomHandler(PacketSession session, IMessage packet)
    {
        S_AddRoom makeRoomPacket = packet as S_AddRoom;
        Managers.Room.Add(makeRoomPacket.Room);

        //TODO: 에러 State 가 Room일 때만
        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();
    }

    public static void S_SpawnInRoomHandler(PacketSession session, IMessage packet)
    {
        S_SpawnInRoom spawnInRoomPacket = packet as S_SpawnInRoom;

        if (spawnInRoomPacket.Me != null)
            Managers.Object.Add(spawnInRoomPacket.Me, GameState.Lobby, true);

        foreach (PlayerInfo player in spawnInRoomPacket.Players)
            Managers.Object.Add(player, GameState.Lobby);
    }

    public static void S_ChangeRoomHandler(PacketSession session, IMessage packet)
    {
        S_ChangeRoom spawnInRoomPacket = packet as S_ChangeRoom;

        Debug.Log($"S_ChangeRoomHandler: {spawnInRoomPacket.Room.Idx}");
        Managers.Room.Rooms[spawnInRoomPacket.Room.Idx].MergeFrom(spawnInRoomPacket.Room);
        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();
    }

    public static void S_ChangePlayerHandler(PacketSession session, IMessage packet)
    {
        S_ChangePlayer changePlayerPacket = packet as S_ChangePlayer;
    }
}