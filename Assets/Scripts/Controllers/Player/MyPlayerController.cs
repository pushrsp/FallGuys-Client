using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class MyPlayerController : PlayerController
{
    [SerializeField] private Vector3 _delta = new Vector3(0.0f, 5.0f, -5.0f);

    void LateUpdate()
    {
        Camera.main.transform.position = transform.position + _delta;
    }

    protected override void Init()
    {
        // _delta = Camera.main.transform.position - transform.position;
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case PlayerState.Idle:
                UpdateKeyboard();
                break;
            case PlayerState.Move:
                UpdateKeyboard();
                break;
            case PlayerState.Jump:
                UpdateKeyboard();
                break;
        }

        base.UpdateController();
    }

    private bool _isJump;

    private void UpdateKeyboard()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            moveDir += Vector3.forward;

        if (Input.GetKey(KeyCode.DownArrow))
            moveDir += Vector3.back;

        if (Input.GetKey(KeyCode.LeftArrow))
            moveDir += Vector3.left;

        if (Input.GetKey(KeyCode.RightArrow))
            moveDir += Vector3.right;

        if (!_isJump && Input.GetKey(KeyCode.Space))
        {
            moveDir += Vector3.up;
            _isJump = true;
            State = PlayerState.Jump;
        }

        MoveDir = moveDir.normalized;
    }

    private bool _sendIdle;

    protected override void SendMove(Vector3 destPos, Vector3 moveVec, PlayerState state)
    {
        C_Move movePacket = new C_Move {PosInfo = new PositionInfo(), MoveDir = new PositionInfo()};

        movePacket.PosInfo.PosY = destPos.y;
        movePacket.PosInfo.PosZ = destPos.z;
        movePacket.PosInfo.PosX = destPos.x;

        movePacket.MoveDir.PosY = moveVec.y;
        movePacket.MoveDir.PosZ = moveVec.z;
        movePacket.MoveDir.PosX = moveVec.x;

        movePacket.State = state;

        Managers.Network.Send(movePacket);
    }

    protected override void UpdateIdle()
    {
        if (_sendIdle)
        {
            SendMove(transform.position, Vector3.zero, PlayerState.Idle);
            _sendIdle = false;
        }
    }

    protected override void Move(Vector3 position, Vector3 moveDir)
    {
        moveDir.y = moveDir.y > 0 ? 1 : moveDir.y;
        SendMove(position, moveDir, moveDir.y > 0 ? PlayerState.Jump : PlayerState.Move);
        _sendIdle = true;
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