using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneAnimEvents : MonoBehaviour
{
    private Enermy_NightBorne enermy;
    private void Start()
    {
        enermy = GetComponentInParent<Enermy_NightBorne>();
    }
    public void AttackFinishTrigger()
    {
            enermy.stateMachine.ChangeState(enermy.battleState);
    }
    public void RecoverFinishTrigger()
    {
        enermy.stateMachine.ChangeState(enermy.battleState);
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
}
