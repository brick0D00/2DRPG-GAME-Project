using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public void IdleOrMove()
    {
        if (player.stateMachine.currentState.GetxInput() == 0)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
        else
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }
    public void AttackFinishTrigger()
    {
        IdleOrMove();
    }
    public void ParryFinishTrigger()
    {
        if (player.wantToParryAttack&& SkillManager.instance.parryAttack.TryToUseSkill())
        {

        }
        else
        {
            player.stateMachine.ChangeState(player.idleState);
        }
        player.wantToParryAttack = false;
    }
    public void ParryAttackFinishTrigger()
    {
        IdleOrMove();
    }
    public void ThrowSwordTrigger()
    {
        SkillManager.instance.throwSword.TryToUseSkill();
    }
    public void CatchSwordFinishTrigger()
    {
        player.stateMachine.ChangeState(player.idleState);
    }
    public void StopMove()
    {
        player.SetVelocity(0, 0);
    }
    public void ParryThinkTime()
    {
        FreezeController.instance.StartCoroutine("FreezeAllForSeconds", 0.3f);
    }
    public void SwitchToEndScreen()
    {
        UIController.instance.SwitchToEndScreen();
    }
}
