using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class MyPlayerController : PlayerController
{
    [SerializeField] private Vector3 _delta = new Vector3(0.0f, 5.0f, -5.0f);

    private Vector3 _moveDir = Vector3.zero;

    public override Vector3 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
            if (value != Vector3.zero)
                State = PlayerState.Move;
            else
                State = PlayerState.Idle;
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = transform.position + _delta;
    }

    protected override void Init()
    {
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
        }

        base.UpdateController();
    }

    private bool _isJump;

    private void UpdateKeyboard()
    {
        if (!CanMove)
            return;

        Vector3 moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            moveVec += Vector3.forward;

        if (Input.GetKey(KeyCode.DownArrow))
            moveVec += Vector3.back;

        if (Input.GetKey(KeyCode.LeftArrow))
            moveVec += Vector3.left;

        if (Input.GetKey(KeyCode.RightArrow))
            moveVec += Vector3.right;

        if (!_isJump && Input.GetKey(KeyCode.Space))
        {
            SendJump();
            _isJump = true;
            DoJump();
        }

        MoveDir = moveVec.normalized;
    }

    protected override void SendJump()
    {
        C_Jump jumpPacket = new C_Jump();

        Managers.Network.Send(jumpPacket);
    }

    private void SendMove(Vector3 destPos, Vector3 moveVec, PlayerState state)
    {
        C_Move movePacket = new C_Move {PosInfo = new PositionInfo(), MoveDir = new PositionInfo()};

        movePacket.PosInfo.PosY = destPos.y;
        movePacket.PosInfo.PosZ = destPos.z;
        movePacket.PosInfo.PosX = destPos.x;

        movePacket.State = state;

        movePacket.MoveDir.PosY = moveVec.y;
        movePacket.MoveDir.PosZ = moveVec.z;
        movePacket.MoveDir.PosX = moveVec.x;

        Managers.Network.Send(movePacket);
    }

    private bool _sendIdle;

    protected override void UpdateMoving()
    {
        if (!CanMove)
            return;

        if (Physics.Raycast(transform.position + Vector3.up, MoveDir, 1.2f, LayerMask.GetMask("Player")))
            return;

        Vector3 destPos = transform.position + MoveDir * Speed * Time.deltaTime;
        if (GameState == GameState.Game && !Managers.Map.CanGo(destPos))
            return;

        DestPos = destPos;
        base.UpdateMoving();
        SendMove(DestPos, MoveDir, PlayerState.Move);
        if (_sendIdle == false)
            _sendIdle = true;
    }

    protected override void UpdateIdle()
    {
        if (_sendIdle == true)
        {
            SendMove(transform.position, Vector3.zero, PlayerState.Idle);
            _sendIdle = false;
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