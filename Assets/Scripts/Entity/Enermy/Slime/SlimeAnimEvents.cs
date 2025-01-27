using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimEvents : MonoBehaviour
{
    private Enermy_Slime enermy;
    private void Start()
    {
        enermy = GetComponentInParent<Enermy_Slime>();
    }
    public void AttackFinishTrigger()
    {
        enermy.stateMachine.ChangeState(enermy.battleState);
    }
    public void StunnedOverFinishTrigger()
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

    public void StunOverFinishTrigger()
    {
        StartCoroutine(IE_StunOverFinishTrigger(1));
    }
    private IEnumerator IE_StunOverFinishTrigger(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        enermy.stateMachine.ChangeState(enermy.stunnedOverState);
    }
    public void SpiltSlimeTrigger()
    {
        enermy.SpiltNewSlime();
    }

}
