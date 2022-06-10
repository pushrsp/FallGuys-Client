using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class PlayerController : BaseController
{
    protected Coroutine _coPunch;

    private Define.State _state = Define.State.Idle;

    public Define.State State
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            _state = value;
            UpdateAnimation();
        }
    }

    private Vector3 _dirVec = Vector3.zero;

    public Vector3 MoveVec
    {
        get { return _dirVec; }
        set
        {
            _dirVec = value.normalized;
            if (_dirVec != Vector3.zero && State != Define.State.Jump)
                State = Define.State.Move;
        }
    }

    protected Animator _anim;
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
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Move:
                UpdateMoving();
                break;
            case Define.State.Hit:
                UpdateHit();
                break;
            case Define.State.Jump:
                UpdateJump();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMoving()
    {
        Vector3 moveVec = MoveVec;
        Vector3 destPos = transform.position + moveVec;
        Vector3 moveDir = destPos - transform.position;
        float dist = moveDir.magnitude;

        if (Managers.Map.CanGo(Vector3Int.RoundToInt(destPos)))
        {
            if (dist < Time.deltaTime * Speed)
            {
                transform.position = destPos;
                State = Define.State.Idle;
            }
            else
            {
                Vector3 look = moveDir;
                look.y = 0;
                transform.position += moveDir.normalized * Speed * Time.deltaTime;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look),
                    Time.deltaTime * Speed);
            }
        }

        MoveVec = Vector3.zero;
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
                    State = Define.State.Idle;
                _anim.SetBool("isJump", false);
                break;
        }
    }

    private void UpdateAnimation()
    {
        if (_anim == null)
            return;

        switch (State)
        {
            case Define.State.Idle:
                _anim.SetFloat("speed", 0.0f);
                break;
            case Define.State.Move:
                _anim.SetFloat("speed", Speed);
                break;
            case Define.State.Jump:
                _anim.SetTrigger("doJump");
                _anim.SetBool("isJump", true);
                break;
        }
    }
}