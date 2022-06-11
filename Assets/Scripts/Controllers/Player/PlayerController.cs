using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class PlayerController : BaseController
{
    private PlayerInfo _playerInfo = new PlayerInfo {PosInfo = new PositionInfo(), MoveDir = new PositionInfo()};

    public PlayerInfo Info
    {
        get { return _playerInfo; }
        set { _playerInfo = value; }
    }

    public int Id
    {
        get { return Info.ObjectId; }
        set { Info.ObjectId = value; }
    }

    public string Name
    {
        get { return Info.Name; }
        set { Info.Name = value; }
    }

    public int PlayerSelect
    {
        get { return Info.PlayerSelect; }
        set { Info.PlayerSelect = value; }
    }

    public Vector3 PosInfo
    {
        get { return new Vector3(Info.PosInfo.PosX, Info.PosInfo.PosY, Info.PosInfo.PosZ); }
        set
        {
            if (Info.PosInfo.PosX == value.x &&
                Info.PosInfo.PosY == value.y &&
                Info.PosInfo.PosZ == value.z)
                return;

            Info.PosInfo.PosX = value.x;
            Info.PosInfo.PosY = value.y;
            Info.PosInfo.PosZ = value.z;
        }
    }

    public PlayerState State
    {
        get { return Info.State; }
        set
        {
            if (Info.State == value)
                return;

            Info.State = value;
            UpdateAnimation();
        }
    }

    public Vector3 MoveDir
    {
        get { return new Vector3(Info.MoveDir.PosX, Info.MoveDir.PosY, Info.MoveDir.PosZ); }
        set
        {
            Info.MoveDir.PosX = value.x;
            Info.MoveDir.PosY = value.y;
            Info.MoveDir.PosZ = value.z;

            if (value != Vector3.zero)
            {
                if (State != PlayerState.Jump)
                    State = PlayerState.Move;
            }
            else
            {
                State = PlayerState.Idle;
            }
        }
    }

    protected Animator _anim;
    protected Rigidbody _rigid;
    private float _fallMultiplyer = 2.5f;
    private float _lowJumpMultiplyer = 2.0f;

    void Start()
    {
        Init();
        SyncPos();
    }

    void FixedUpdate()
    {
        UpdateController();
        UpdateVelocity();
    }

    public void SyncPos()
    {
        PosInfo = new Vector3Int(-7, 0, -260);
        // transform.position = new Vector3(PosInfo.PosX, PosInfo.PosY, PosInfo.PosZ);
    }

    private void UpdateVelocity()
    {
        if (_rigid == null)
            return;

        if (_rigid.velocity.y < 0)
        {
            _rigid.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplyer - 1) * Time.deltaTime;
        }
        else if (_rigid.velocity.y > 0)
        {
            _rigid.velocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplyer - 1) * Time.deltaTime;
        }
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Move:
                UpdateMoving();
                break;
            case PlayerState.Hit:
                UpdateHit();
                break;
            case PlayerState.Jump:
                UpdateJump();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMoving()
    {
        Vector3 moveVec = MoveDir;
        if (moveVec == Vector3.zero)
            return;

        Vector3 destPos = PosInfo;
        Vector3 dir = destPos - transform.position;
        float dist = dir.magnitude;

        transform.position += moveVec * Speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(new Vector3(moveVec.x, 0, moveVec.z)),
            Time.deltaTime * Speed
        );

        // Debug.Log(moveVec);
        // if (dist < Speed * Time.deltaTime)
        // {
        //     transform.position = destPos;
        //     State = PlayerState.Idle;
        // }
        // else
        // {
        //     transform.position += moveVec * Speed * Time.deltaTime;
        //     if (moveVec != Vector3.zero)
        //         transform.rotation = Quaternion.Slerp(
        //             transform.rotation,
        //             Quaternion.LookRotation(new Vector3(moveVec.x, 0, moveVec.z)),
        //             Time.deltaTime * Speed
        //         );
        // }
    }

    protected virtual void UpdateHit()
    {
    }

    private bool _doJump;

    protected virtual void UpdateJump()
    {
        if (!_doJump)
        {
            _doJump = true;
            _rigid.AddForce(Vector3.up * 9, ForceMode.Impulse);
        }

        UpdateMoving();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Terrain":
                _doJump = false;
                if (_anim.GetBool("isJump"))
                    State = PlayerState.Idle;
                _anim.SetBool("isJump", false);
                break;
        }

        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Wheel":
                transform.SetParent(collision.transform);
                break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case "Wheel":
                transform.SetParent(null);
                break;
        }
    }

    private void UpdateAnimation()
    {
        if (_anim == null)
            return;

        switch (State)
        {
            case PlayerState.Idle:
                _anim.SetFloat("speed", 0.0f);
                break;
            case PlayerState.Move:
                _anim.SetFloat("speed", Speed);
                break;
            case PlayerState.Jump:
                _anim.SetTrigger("doJump");
                _anim.SetBool("isJump", true);
                break;
        }
    }
}