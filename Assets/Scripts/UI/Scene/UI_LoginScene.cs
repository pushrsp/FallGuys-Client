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

        GetImage((int) Images.SignInBtn).gameObject.BindEvent(OnClickSignIn);
        GetImage((int) Images.SignUpBtn).gameObject.BindEvent(OnClickSignUp);
    }

    private void OnClickSignIn(PointerEventData evt)
    {
        string username = Get<GameObject>((int) GameObjects.Username).GetComponent<InputField>().text;
        string password = Get<GameObject>((int) GameObjects.Password).GetComponent<InputField>().text;

        LoginAccountReq req = new LoginAccountReq {Username = username, Password = password};
        Managers.Web.SendPostRequest<LoginAccountRes>("account/login", req, (res) =>
        {
            if (res.Success)
            {
                Managers.Object.Token = res.Token;
                Managers.Network.Init();
            }
        });
    }

    private void OnClickSignUp(PointerEventData evt)
    {
        string username = Get<GameObject>((int) GameObjects.Username).GetComponent<InputField>().text;
        string password = Get<GameObject>((int) GameObjects.Password).GetComponent<InputField>().text;

        CreateAccountReq req = new CreateAccountReq {Username = username, Password = password};
        Managers.Web.SendPostRequest<CreateAccountRes>("account", req, (res) =>
        {
            if (res.Success)
            {
                Debug.Log("성공");
            }
        });
    }
}