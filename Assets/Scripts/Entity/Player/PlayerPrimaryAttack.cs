using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public int comboCounter;
    public float lastAttackTime;
    public float comboResetTime;
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
        this.comboResetTime = player.comboResetTime;
    }

    public override void Enter()
    {
        base.Enter();
        
        if (comboCounter == 1)
        {
            AudioManager.instance.PlaySFX(1, null);
        }
        else
        {
            AudioManager.instance.PlaySFX(0, null);
        }
        
        ComboCounterController();
        player.anim.SetInteger("ComboCounter", comboCounter);
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput*player.moveSpeed*0.05f, rb.velocity.y*0.3f);
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
        comboCounter++;
    }
    public void ComboCounterController()
    {
        if (comboCounter > 2 || Time.time >= lastAttackTime + comboResetTime)
        {
            comboCounter = 0;
        }
    }
}
