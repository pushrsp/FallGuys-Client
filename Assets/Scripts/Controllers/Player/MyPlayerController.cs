using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
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
        if (Input.anyKey == false)
            return;

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
            _isJump = true;
            State = PlayerState.Jump;
        }

        MoveDir = moveDir.normalized;
    }

    private bool _sendIdle;

    private void SendMove(Vector3Int destPos, Vector3 moveVec)
    {
        C_Move movePacket = new C_Move {PosInfo = new PositionInfo(), MoveDir = new PositionInfo()};

        movePacket.PosInfo.PosY = destPos.y;
        movePacket.PosInfo.PosZ = destPos.z;
        movePacket.PosInfo.PosX = destPos.x;

        movePacket.MoveDir.PosY = moveVec.y;
        movePacket.MoveDir.PosZ = moveVec.z;
        movePacket.MoveDir.PosX = moveVec.x;

        movePacket.State = State;

        Managers.Network.Send(movePacket);
    }

    protected override void UpdateIdle()
    {
        if (_sendIdle)
        {
            SendMove(Vector3Int.RoundToInt(transform.position), Vector3.zero);
            _sendIdle = false;
        }
    }

    protected override void UpdateMoving()
    {
        Vector3Int destPos = Vector3Int.RoundToInt(transform.position + MoveDir);
        if (!Managers.Map.CanGo(destPos))
            return;

        SendMove(destPos, MoveDir);
        _sendIdle = true;

        PosInfo = destPos;
        base.UpdateMoving();
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