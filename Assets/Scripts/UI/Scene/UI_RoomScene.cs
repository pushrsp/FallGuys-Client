using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RoomScene : UI_Scene
{
    public UI_MakeRoom MakeRoomUI { get; private set; }

    private GameObject _grid;

    enum Images
    {
        MakeRoomBtn
    }

    protected override void Init()
    {
        base.Init();

        MakeRoomUI = GetComponentInChildren<UI_MakeRoom>();

        Bind<Image>(typeof(Images));

        _grid = transform.Find("ScrollViewMask").transform.Find("RoomGrid").gameObject;
        foreach (Transform child in _grid.transform)
            Destroy(child.gameObject);

        GetImage((int) Images.MakeRoomBtn).gameObject.BindEvent(OnClickMakeRoomBtn);
        SetUI();

        MakeRoomUI.gameObject.SetActive(false);
    }

    private void OnClickMakeRoomBtn(PointerEventData evt)
    {
        MakeRoomUI.gameObject.SetActive(true);
        if (MakeRoomUI.gameObject.activeSelf)
            MakeRoomUI.gameObject.GetComponent<Canvas>().sortingOrder = 4;
    }

    public void SetUI()
    {
        if (Managers.Room.Rooms.Count == 0)
            return;

        foreach (Transform child in _grid.transform)
            Destroy(child.gameObject);

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