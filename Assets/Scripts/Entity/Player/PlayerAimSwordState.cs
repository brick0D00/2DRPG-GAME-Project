using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.throwSword.SetDosActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.idleState);
        }

        AimFlipController();

    }

    private void AimFlipController()
    {
        //TODO:通过鼠标位置判断瞄向来旋转
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePostion.x && player.facingDirection == 1)
        {
            //鼠标左，人向右
            player.Flip();
        }
        else if (player.transform.position.x < mousePostion.x && player.facingDirection == -1)
        {
            //鼠标右，人向左
            player.Flip();
        }
    }
}
