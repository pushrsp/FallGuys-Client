using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class PlayerController : BaseController
{
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
            if (_dirVec.Equals(value))
            {
                State = Define.State.Idle;
                return;
            }

            _dirVec = Vector3Int.RoundToInt(value.normalized);
            State = Define.State.Move;
        }
    }

    private Animator _anim;

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
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
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMoving()
    {
        Vector3Int destPos = Vector3Int.RoundToInt(transform.position + MoveVec.normalized);
        Vector3 moveDir = destPos - transform.position;
        float dist = moveDir.magnitude;

        if (dist < Speed * Time.deltaTime)
        {
            State = Define.State.Idle;
            transform.position = destPos;
        }
        else
        {
            Vector3 look = moveDir.normalized;
            look.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.2f);
            transform.position += moveDir.normalized * Speed * Time.deltaTime;
        }

        MoveVec = Vector3.zero;
    }

    protected virtual void UpdateHit()
    {
    }

    private void UpdateAnimation()
    {
        if (_anim == null)
            return;

        switch (State)
        {
            case Define.State.Idle:
                _anim.SetFloat("Speed", 0.0f);
                break;
            case Define.State.Move:
                _anim.SetFloat("Speed", Speed);
                break;
        }
    }
}