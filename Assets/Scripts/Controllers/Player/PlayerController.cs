using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class PlayerController : BaseController
{
    private PlayerInfo _playerInfo = new PlayerInfo {PosInfo = new PositionInfo(), MoveDir = new PositionInfo()};

    public bool CanMove { get; set; } = false;

    public GameState GameState
    {
        get { return Info.GameState; }
        set { Info.GameState = value; }
    }

    public override float Speed
    {
        get { return Info.Speed; }
        set { Info.Speed = value; }
    }

    public PlayerInfo Info
    {
        get { return _playerInfo; }
        set { _playerInfo = value; }
    }

    public string ObjectId
    {
        get { return Info.ObjectId; }
        set { Info.ObjectId = value; }
    }

    public string Username
    {
        get { return Info.Username; }
        set { Info.Username = value; }
    }

    public int PlayerSelect
    {
        get { return Info.PlayerSelect; }
        set { Info.PlayerSelect = value; }
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

    public Vector3 DestPos
    {
        get { return new Vector3(Info.PosInfo.PosX, Info.PosInfo.PosY, Info.PosInfo.PosZ); }
        set
        {
            Info.PosInfo.PosX = value.x;
            Info.PosInfo.PosY = value.y;
            Info.PosInfo.PosZ = value.z;
        }
    }

    public virtual Vector3 MoveDir
    {
        get { return new Vector3(Info.MoveDir.PosX, Info.MoveDir.PosY, Info.MoveDir.PosZ); }
        set
        {
            Info.MoveDir.PosX = value.x;
            Info.MoveDir.PosY = value.y;
            Info.MoveDir.PosZ = value.z;
        }
    }

    public Animator Anim { get; set; }
    protected Rigidbody _rigid;
    private float _fallMultiplyer = 2.5f;
    private float _lowJumpMultiplyer = 2.0f;

    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        UpdateController();
        UpdateVelocity();
    }

    public void SyncPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SyncPos()
    {
        transform.position = DestPos;
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
        Anim = GetComponent<Animator>();
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
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMoving()
    {
        Vector3 moveDist = DestPos - transform.position;

        transform.position += moveDist.normalized * Speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(new Vector3(MoveDir.x, 0, MoveDir.z)),
            Time.deltaTime * Speed
        );
    }

    protected virtual void UpdateHit()
    {
    }

    protected virtual void SendJump()
    {
    }

    public void DoJump()
    {
        _rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
        Anim.SetTrigger("doJump");
        Anim.SetBool("isJump", true);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Terrain":
                if (Anim.GetBool("isJump"))
                    State = PlayerState.Idle;
                Anim.SetBool("isJump", false);
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
        if (Anim == null)
            return;

        switch (State)
        {
            case PlayerState.Idle:
                Anim.SetFloat("speed", 0.0f);
                break;
            case PlayerState.Move:
                Anim.SetFloat("speed", Speed);
                break;
        }
    }
}