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
            case Define.State.Jump:
                UpdateKeyboard();
                break;
        }

        base.UpdateController();
    }

    private bool _isJump;

    private void UpdateKeyboard()
    {
        if (Input.anyKey == false)
            return;

        if (Input.GetKey(KeyCode.UpArrow))
            MoveVec += Vector3.forward;

        if (Input.GetKey(KeyCode.DownArrow))
            MoveVec += Vector3.back;

        if (Input.GetKey(KeyCode.LeftArrow))
            MoveVec += Vector3.left;

        if (Input.GetKey(KeyCode.RightArrow))
            MoveVec += Vector3.right;

        if (!_isJump && Input.GetKey(KeyCode.Space))
        {
            _isJump = true;
            State = Define.State.Jump;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Terrain":
                _isJump = false;
                break;
        }

        base.OnCollisionEnter(collision);
    }
}