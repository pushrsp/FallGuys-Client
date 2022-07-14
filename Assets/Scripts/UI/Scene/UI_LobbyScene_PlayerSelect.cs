using System;
using System.Collections;
using System.Collections.Generic;
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
    protected override void Init()
    {
        base.Init();
        
        Bind<Image>(typeof(Images));
        
        GetImage((int)Images.NextBtn).gameObject.BindEvent(OnClickNext);
        GetImage((int)Images.PrevBtn).gameObject.BindEvent(OnClickPrev);
        GetImage((int)Images.SelectBtn).gameObject.BindEvent(OnClickSelect);
    }

    private void OnClickNext(PointerEventData evt)
    {
        Debug.Log("OnClickNext");
    }

    private void OnClickPrev(PointerEventData evt)
    {
        Debug.Log("OnClickPrev");
    }

    private void OnClickSelect(PointerEventData evt)
    {
        Debug.Log("OnClickSelect");
    }
}
