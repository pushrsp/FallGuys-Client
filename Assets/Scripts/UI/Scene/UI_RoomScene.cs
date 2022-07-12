using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class UI_RoomScene : UI_Scene
{
    private GameObject _grid;
    protected override void Init()
    {
        base.Init();

        _grid = transform.Find("ScrollViewMask").transform.Find("RoomGrid").gameObject;
        foreach (Transform child in _grid.transform)
            Destroy(child.gameObject);

        SetUI();
    }

    public void SetUI()
    {
        if(Managers.Room.Rooms.Count == 0)
            return;

        foreach (RoomInfo room in Managers.Room.Rooms)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_RoomScene_Item", _grid.transform);
            go.name = $"Room_{room.Idx}";
            UI_RoomScene_Item item = go.GetOrAddComponent<UI_RoomScene_Item>();
            {
                item.Idx = room.Idx;
                item.Title = room.Title;
                item.PlayersCount = room.PlayerCount;
                item.State = room.State;
            }
            
            item.SetRoom();
        }
    }
}
