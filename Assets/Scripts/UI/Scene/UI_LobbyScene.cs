using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    public UI_LobbyScene_PlayerSelect PlayerSelectUI { get; private set; }

    private Camera _canvasCamera;

    enum Images
    {
        PlayerSelectBtn
    }

    protected override void Init()
    {
        base.Init();

        PlayerSelectUI = GetComponentInChildren<UI_LobbyScene_PlayerSelect>();
        PlayerSelectUI.gameObject.SetActive(false);
        
        Bind<Image>(typeof(Images));
        
        _canvasCamera =GameObject.Find("Canvas Camera").GetComponent<Camera>();

        GetImage((int)Images.PlayerSelectBtn).gameObject.BindEvent(OnClickPlayerSelect);
    }

    private void OnClickPlayerSelect(PointerEventData evt)
    {
        PlayerSelectUI.gameObject.SetActive(!PlayerSelectUI.gameObject.activeSelf);
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _canvasCamera;
    }
}
