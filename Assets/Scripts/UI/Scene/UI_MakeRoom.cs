using System.Collections;
using System.Collections.Generic;
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
        
        GetImage((int)Images.CloseBtn).gameObject.BindEvent(OnClickCloseBtn);
        GetImage((int)Images.MakeBtn).gameObject.BindEvent(OnClickMakeBtn);
        
        Debug.Log(GetImage((int)Images.CloseBtn).gameObject);
    }

    private void OnClickCloseBtn(PointerEventData evt)
    {
        gameObject.SetActive(false);
    }

    private void OnClickMakeBtn(PointerEventData evt)
    {
        string title = Get<GameObject>((int) GameObjects.TitleInput).GetComponent<InputField>().text;
        gameObject.SetActive(false);
        
        //TODO: 방 만들기
        
        Debug.Log(title);
    }
}
