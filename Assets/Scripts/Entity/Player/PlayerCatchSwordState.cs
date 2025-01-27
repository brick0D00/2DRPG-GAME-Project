using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{   
    //用来得知剑的来向
    private Transform swordTr;
    private int catchImpactForce=10;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        swordTr = player.sword.transform;
        CatchFlipController();
        rb.velocity = new Vector2(catchImpactForce * -player.facingDirection, rb.velocity.y);
        player.fx.ScreenShake(player.fx.shakePower_CatchSword);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
    }

    private void CatchFlipController()
    {
        Vector2 swordPostion = swordTr.position;

        if (player.transform.position.x > swordPostion.x && player.facingDirection == 1)
        {
            //剑左，人向右
            player.Flip();
        }
        else if (player.transform.position.x < swordPostion.x && player.facingDirection == -1)
        {
            //剑右，人向左
            player.Flip();
        }
    }
}
