using UnityEngine;

public class SkeletonAnimEvents : MonoBehaviour
{
    private Enermy_Skeleton enermy;
    private void Start()
    {
        enermy = GetComponentInParent<Enermy_Skeleton>();
    }
    public void AttackFinishTrigger()
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
    public void RecoverFinishTrigger()
    {
        enermy.stateMachine.ChangeState(enermy.idleState);
    }

}
