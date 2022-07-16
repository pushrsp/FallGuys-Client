using Core;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable All

public class PacketHandler
{
    public static void S_EnterRoomHandler(PacketSession session, IMessage packet)
    {
        S_EnterRoom enterPacket = packet as S_EnterRoom;

        Managers.Object.Add(enterPacket.Player, enterPacket.CanMove, true);

        if (enterPacket.Player.GameState == GameState.Room)
        {
            UI_LobbyScene lobby = Managers.UI.SceneUI as UI_LobbyScene;
            lobby.SetUserList();
        }
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        UI_LobbyScene lobby = Managers.UI.SceneUI as UI_LobbyScene;
        foreach (PlayerInfo p in spawnPacket.Players)
        {
            Managers.Object.Add(p, true, Managers.Object.Me.ObjectId == p.ObjectId);

            if (p.GameState != GameState.Game)
                lobby.SetUserList();
        }
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

        Managers.Room.Clear();
        Managers.Room.EnteredRoom.MergeFrom(makeRoomPacket.Room);

        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();

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

    public static void S_ChangeRoomHandler(PacketSession session, IMessage packet)
    {
        S_ChangeRoom changeRoomPacket = packet as S_ChangeRoom;

        Managers.Room.Rooms[changeRoomPacket.Room.Idx].MergeFrom(changeRoomPacket.Room);
        UI_RoomScene roomScene = Managers.UI.SceneUI as UI_RoomScene;
        roomScene.SetUI();
    }

    public static void S_StartGameHandler(PacketSession session, IMessage packet)
    {
        S_StartGame startGamePacket = packet as S_StartGame;

        Managers.Object.StageId = startGamePacket.StageId;
        Managers.Object.Clear();
        Managers.Scene.LoadScene(GameState.Game);
    }

    public static void S_StartCountDownHandler(PacketSession session, IMessage packet)
    {
        S_StartCountDown startCountDownPacket = packet as S_StartCountDown;
        UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;
        string text = $"{startCountDownPacket.Counter - 1}";

        if (startCountDownPacket.Counter == 1)
        {
            text = "GO";
            Managers.Object.Me.CanMove = true;
        }
        else if (startCountDownPacket.Counter == 0)
        {
            text = "";
        }

        scene.SetText(text);
    }

    public static void S_ArriveHandler(PacketSession session, IMessage packet)
    {
        S_Arrive arrivePacket = packet as S_Arrive;

        GameObject go = Managers.Object.FindById(arrivePacket.ObjectId);
        if (go == null)
            return;

        PlayerController pc = go.GetComponent<PlayerController>();
        pc.CanMove = false;
        pc.State = PlayerState.Idle;

        UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;
        scene.SetArrive(pc.Username);
    }

    public static void S_EndCountDownHandler(PacketSession session, IMessage packet)
    {
        S_EndCountDown endCountDownPacket = packet as S_EndCountDown;
        UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;
        string text = $"{endCountDownPacket.Counter - 1}";

        if (endCountDownPacket.Counter > 1)
            scene.SetText(text);
        else
        {
            Managers.Object.Clear();
            Managers.Scene.LoadScene(GameState.Lobby);
        }
    }
}