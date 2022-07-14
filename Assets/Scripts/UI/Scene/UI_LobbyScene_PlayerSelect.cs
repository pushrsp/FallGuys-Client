using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene_PlayerSelect : UI_Scene
{
    enum Images
    {
        NextBtn,
        PrevBtn,
        SelectBtn
    }

    private GameObject _playerHolder;
    private int _playerSelect;

    protected override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        GetImage((int) Images.NextBtn).gameObject.BindEvent(OnClickNext);
        GetImage((int) Images.PrevBtn).gameObject.BindEvent(OnClickPrev);
        GetImage((int) Images.SelectBtn).gameObject.BindEvent(OnClickSelect);

        _playerHolder = Helper.FindChild(gameObject, "PlayerHolder");
    }

    private void OnClickNext(PointerEventData evt)
    {
        Transform child = _playerHolder.transform.GetChild(_playerSelect);
        child.gameObject.SetActive(false);

        _playerSelect++;
        if (_playerSelect > 9)
            _playerSelect = 0;

        child = _playerHolder.transform.GetChild(_playerSelect);
        child.gameObject.SetActive(true);
    }

    private void OnClickPrev(PointerEventData evt)
    {
        Transform child = _playerHolder.transform.GetChild(_playerSelect);
        child.gameObject.SetActive(false);

        _playerSelect--;
        if (_playerSelect < 0)
            _playerSelect = 9;

        child = _playerHolder.transform.GetChild(_playerSelect);
        child.gameObject.SetActive(true);
    }

    private void OnClickSelect(PointerEventData evt)
    {
        if (_playerSelect + 1 == Managers.Object.Me.PlayerSelect)
            return;

        C_ChangePlayer changePlayerPacket = new C_ChangePlayer();
        changePlayerPacket.ObjectId = Managers.Object.Me.ObjectId;
        changePlayerPacket.PlayerSelect = _playerSelect + 1;

        Managers.Network.Send(changePlayerPacket);
    }
}