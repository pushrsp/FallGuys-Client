using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        CountDownText
    }

    protected override void Init()
    {
        base.Init();
        
        Bind<Text>(typeof(Texts));

        GetText((int) Texts.CountDownText).text = "";
    }

    public void SetText(string text)
    {
        GetText((int) Texts.CountDownText).text = text;
        GetText((int) Texts.CountDownText).fontSize = 40;
    }
}
