using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("弹反")]
    public bool isParryUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnParry;

    protected override void Start()
    {
        unlockButtonOnParry.GetComponent<Button>().onClick.AddListener(UnlockParry);
        base.Start();
    }
    public override bool TryToUseSkill()
    {   //TODO:如果没解锁不使用
        if (isParryUnlocked == false)
        {
            return false;
        }
        return base.TryToUseSkill();
    }

    public override void UseSkill()
    {
        UIController.instance.GetUI_InGame().SetUpImageForParry();
        player.stateMachine.ChangeState(player.preParryState);
    }
    private void UnlockParry()
    {
        if (unlockButtonOnParry.isUnlocked)
        {
            isParryUnlocked=true;
        }
    }

    protected override void LoadSkillCheckUnlock()
    {
        UnlockParry();
    }
}
