using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : SkeletonState
{
    public SkeletonStunnedState(Enermy_Skeleton _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //���к����˸��ʱΪ0��Ƶ��0.1f
        enermy.fx.InvokeRepeating("RedWhiteBlink",0,0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        if (enermy.stats.isInElementStates())
        {
            enermy.fx.CancelRedWhiteBlink();
        }
        else
        {
            enermy.fx.Invoke("CancelColorChange", 0.5f);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
