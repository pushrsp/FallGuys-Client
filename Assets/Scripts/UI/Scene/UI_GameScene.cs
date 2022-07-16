using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    private GameObject _grid;

    enum Texts
    {
        CountDownText
    }

    protected override void Init()
    {
        base.Init();

        _grid = transform.Find("ArriveListGrid").gameObject;
        Bind<Text>(typeof(Texts));
        GetText((int) Texts.CountDownText).text = "";
    }

    public void SetText(string text)
    {
        GetText((int) Texts.CountDownText).text = text;
        GetText((int) Texts.CountDownText).fontSize = 40;
        GetText((int) Texts.CountDownText).fontStyle = FontStyle.Bold;
        GetText((int) Texts.CountDownText).color = Color.red;
    }

    public void SetArrive(string text)
    {
        GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_GameScene_UsernameText", _grid.transform);
        go.GetComponent<Text>().text = text;
    }
}