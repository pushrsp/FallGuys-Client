using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Room,
        Lobby,
        Game,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
}