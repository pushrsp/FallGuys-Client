using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RoomScene_Item : UI_Base
{
    public RoomInfo Info { get; } = new RoomInfo();

    public RoomState State
    {
        get { return Info.State; }
        set { Info.State = value; }
    }

    public string Title
    {
        get { return Info.Title; }
        set { Info.Title = value; }
    }

    public int Idx
    {
        get { return Info.Idx; }
        set { Info.Idx = value; }
    }

    public string OwnerId
    {
        get { return Info.OwnerId; }
        set { Info.OwnerId = value; }
    }

    public int PlayersCount
    {
        get { return Info.PlayerCount; }
        set { Info.PlayerCount = value; }
    }

    enum Texts
    {
        TitleText,
        PlayerCountText
    }

    enum Images
    {
        EnterBtn
    }

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GetImage((int) Images.EnterBtn).gameObject.BindEvent(OnClickEnter);
    }

    private void OnClickEnter(PointerEventData evt)
    {
        Managers.Room.Clear();
        Managers.Room.EnteredRoom.MergeFrom(Info);
        Managers.Scene.LoadScene(GameState.Lobby);
    }

    public void SetRoom()
    {
        GetText((int) Texts.TitleText).text = Title;
        GetText((int) Texts.PlayerCountText).text = $"{PlayersCount}/8";
    }
}