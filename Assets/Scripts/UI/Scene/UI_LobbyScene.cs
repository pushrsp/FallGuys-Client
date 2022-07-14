using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    public UI_LobbyScene_PlayerSelect PlayerSelectUI { get; private set; }
    public GameObject UserList { get; private set; }

    private Camera _canvasCamera;

    enum Images
    {
        PlayerSelectBtn,
        StartBtn
    }

    protected override void Init()
    {
        base.Init();

        PlayerSelectUI = GetComponentInChildren<UI_LobbyScene_PlayerSelect>();
        PlayerSelectUI.gameObject.SetActive(false);

        Bind<Image>(typeof(Images));

        _canvasCamera = GameObject.Find("Canvas Camera").GetComponent<Camera>();
        UserList = Helper.FindChild(gameObject, "UserList");

        foreach (Transform child in UserList.transform)
            Destroy(child.gameObject);

        GetImage((int) Images.PlayerSelectBtn).gameObject.BindEvent(OnClickPlayerSelect);
        GetImage((int) Images.StartBtn).gameObject.BindEvent(OnClickStart);
    }

    private void OnClickPlayerSelect(PointerEventData evt)
    {
        PlayerSelectUI.gameObject.SetActive(!PlayerSelectUI.gameObject.activeSelf);
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _canvasCamera;
    }

    private void OnClickStart(PointerEventData evt)
    {
        Debug.Log("OnClickStart");
    }

    public void SetUserList()
    {
        List<GameObject> users = Managers.Object.GetAll();
        if (users.Count == 0)
            return;

        foreach (Transform child in UserList.transform)
            Destroy(child.gameObject);

        foreach (GameObject user in users)
        {
            PlayerController pc = user.GetComponent<PlayerController>();

            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_LobbyScene_Text", UserList.transform);
            Text text = go.GetComponent<Text>();
            text.text = pc.Username;
            text.fontSize = 15;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.black;

            if (Managers.Room.EnteredRoom.OwnerId == pc.ObjectId)
                text.color = Color.red;
        }
    }
}