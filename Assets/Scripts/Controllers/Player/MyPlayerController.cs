using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class MyPlayerController : PlayerController
{
    private Vector3 _delta;

    void LateUpdate()
    {
        Camera.main.transform.position = transform.position + _delta;
    }

    protected override void Init()
    {
        _delta = Camera.main.transform.position - transform.position;
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateKeyboard();
                break;
            case Define.State.Move:
                UpdateKeyboard();
                break;
        }

        base.UpdateController();
    }

    private void UpdateKeyboard()
    {
        if (Input.anyKey == false)
            return;

        Vector3 _moveVec = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            _moveVec += Vector3.forward;


        if (Input.GetKey(KeyCode.DownArrow))
            _moveVec += Vector3.back;


        if (Input.GetKey(KeyCode.LeftArrow))
            _moveVec += Vector3.left;


        if (Input.GetKey(KeyCode.RightArrow))
            _moveVec += Vector3.right;


        if (_moveVec != Vector3.zero)
            MoveVec = _moveVec;
    }
}