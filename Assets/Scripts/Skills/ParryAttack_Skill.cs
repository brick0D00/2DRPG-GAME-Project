using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryAttack_Skill : Skill
{
    [Header("µ¯·´")]
    public bool isParryAttackUnlocked;
    [SerializeField] private UI_SkillTreeSlot unLockButtonOnParryAttack;
    
    protected override void Start()
    {   
        base.Start();
        unLockButtonOnParryAttack.GetComponent<Button>().onClick.AddListener(UnlockParryAttack);
    }

    public override bool TryToUseSkill()
    {
        if (isParryAttackUnlocked==false)
        {
            return false;
        }
        return base.TryToUseSkill();
    }

    public override void UseSkill()
    {
        player.stateMachine.ChangeState(player.parryAttackState);
        base.UseSkill();
    }
    private void UnlockParryAttack()
    {
        if (unLockButtonOnParryAttack.isUnlocked)
        {
            isParryAttackUnlocked = true;
        }
    }

    protected override void LoadSkillCheckUnlock()
    {
        UnlockParryAttack();
    }
}
