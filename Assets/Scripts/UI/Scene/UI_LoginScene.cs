using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    enum GameObjects
    {
        Username,
        Password
    }

    enum Images
    {
        SignInBtn,
        SignUpBtn
    }

    protected override void Init()
    {
        base.Init();
        
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        
        GetImage((int)Images.SignInBtn).gameObject.BindEvent(OnClickSignIn);
        GetImage((int)Images.SignUpBtn).gameObject.BindEvent(OnClickSignUp);
    }

    private void OnClickSignIn(PointerEventData evt)
    {
        string username = Get<GameObject>((int) GameObjects.Username).GetComponent<InputField>().text;
        string password = Get<GameObject>((int) GameObjects.Password).GetComponent<InputField>().text;
        
        Managers.Scene.LoadScene(Define.Scene.Game);
        Managers.Network.Init();
        // Debug.Log($"{username}, {password}");
    }

    private void OnClickSignUp(PointerEventData evt)
    {
        string username = Get<GameObject>((int) GameObjects.Username).GetComponent<InputField>().text;
        string password = Get<GameObject>((int) GameObjects.Password).GetComponent<InputField>().text;
        Debug.Log("OnClickSignUp");
    }
}
