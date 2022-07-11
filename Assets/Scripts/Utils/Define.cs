using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Room,
        Game,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
}