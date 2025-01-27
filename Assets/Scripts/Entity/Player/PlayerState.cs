using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected string aniBoolName;

    protected Rigidbody2D rb;

    protected float stateTimer;
    protected float xInput;
    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _aniBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.aniBoolName = _aniBoolName;
    }

    public virtual void Enter()
    {   //TODO:切换进入一个新状态
        player.anim.SetBool(aniBoolName, true);
        rb = player.rb;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
        stateTimer -= Time.deltaTime;
        //Debug.Log(aniBoolName);
       
    }
    public virtual void Exit()
    {
        player.anim.SetBool(aniBoolName, false);
    }
    public float GetxInput()
    {
        return xInput;
    }
}
