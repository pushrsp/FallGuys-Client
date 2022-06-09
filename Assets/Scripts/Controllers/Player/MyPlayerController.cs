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

    private Vector3 _moveVec;
    private void UpdateKeyboard()
    {
        if (Input.anyKey == false)
            return;
        
        if (Input.GetKey(KeyCode.W))
            _moveVec += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            _moveVec += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            _moveVec += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            _moveVec += Vector3.right;

        if (_moveVec != Vector3.zero)
        {
            MoveVec = _moveVec;
            _moveVec = Vector3.zero;
        }
    }
}
