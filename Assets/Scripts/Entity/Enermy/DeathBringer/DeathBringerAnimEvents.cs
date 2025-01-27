using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAnimEvents : MonoBehaviour
{
    private Enermy_DeathBringer enermy;
    private void Start()
    {
        enermy = GetComponentInParent<Enermy_DeathBringer>();
    }
    public void AttackFinishTrigger()
    {
        enermy.ChangeAttackOrTeleport();
    }
    public void StunnedFinishTrigger()
    {
        enermy.stateMachine.ChangeState(enermy.battleState);
    }
    public void OpenParryWindow()
    {
        enermy.attackState.OpenParryWindow();
    }
    public void CloseParryWindow()
    {
        enermy.attackState.CloseParryWindow();
    }
    public void ExitFinsihTrigger()
    {
        enermy.TeleportToRandomPostion();
    }
    public void EnterFinishTrigger()
    {
        enermy.AfterTp_AttackOrCast();
    }
    public void CastStartFinsihTrigger()
    {
        enermy.CastAttack();
    }
}
