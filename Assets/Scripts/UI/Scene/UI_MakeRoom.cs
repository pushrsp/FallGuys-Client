using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MakeRoom : UI_Scene
{
    enum GameObjects
    {
        TitleInput
    }

    enum Images
    {
        CloseBtn,
        MakeBtn
    }

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetImage((int) Images.CloseBtn).gameObject.BindEvent(OnClickCloseBtn);
        GetImage((int) Images.MakeBtn).gameObject.BindEvent(OnClickMakeBtn);
    }

    private void OnClickCloseBtn(PointerEventData evt)
    {
        Get<GameObject>((int) GameObjects.TitleInput).GetComponent<InputField>().text = "";
        gameObject.SetActive(false);
    }

    private void OnClickMakeBtn(PointerEventData evt)
    {
        string title = Get<GameObject>((int) GameObjects.TitleInput).GetComponent<InputField>().text;
        gameObject.SetActive(false);

        if (String.IsNullOrEmpty(title))
        {
            Debug.Log("empty");
            return;
        }

        //TODO: 방 만들기
        C_MakeRoom makeRoomPacket = new C_MakeRoom();
        makeRoomPacket.Id = Managers.Object.Id;
        makeRoomPacket.Title = title;

        Managers.Network.Send(makeRoomPacket);
        Get<GameObject>((int) GameObjects.TitleInput).GetComponent<InputField>().text = "";
    }
}