using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("冲刺")]
    public bool isDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnDash;


    [Header("影冲")]
    public bool isShadowDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnShadowDash;

    protected override void Start()
    {   
        //为Button的点击添加事件
        unlockButtonOnDash.GetComponent<Button>().onClick.AddListener(UnlockDash);
        unlockButtonOnShadowDash.GetComponent<Button>().onClick.AddListener(UnlockShadowDash);

        base.Start();

    }
    public override bool TryToUseSkill()
    {   
        //TODO:黑洞状态以及未解锁状态下无法使用
        if (player.stateMachine.currentState == player.blackHoleState||isDashUnlocked==false)
        {
            return false;
        }
        return base.TryToUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        UIController.instance.GetUI_InGame().SetUpImageForDash();
    }

    private void UnlockDash()
    {   
        //TODO:解锁冲刺
        if (unlockButtonOnDash.isUnlocked)
        {
            isDashUnlocked = true;
        }
    }
    private void UnlockShadowDash()
    {
        if (unlockButtonOnShadowDash.isUnlocked)
        {
            isShadowDashUnlocked = true;
        }
    }
    public void CreateCloneAfterDash()
    {
        //TODO:只有在技能解锁之后可以使用
        if (isShadowDashUnlocked)
        {
            Debug.Log(114);
            SkillManager.instance.clone.TryToUseSkill();
        }
    }

    protected override void LoadSkillCheckUnlock()
    {
        UnlockDash();
        UnlockShadowDash();
    }
}
