using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("���")]
    public bool isDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnDash;


    [Header("Ӱ��")]
    public bool isShadowDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnShadowDash;

    protected override void Start()
    {   
        //ΪButton�ĵ������¼�
        unlockButtonOnDash.GetComponent<Button>().onClick.AddListener(UnlockDash);
        unlockButtonOnShadowDash.GetComponent<Button>().onClick.AddListener(UnlockShadowDash);

        base.Start();

    }
    public override bool TryToUseSkill()
    {   
        //TODO:�ڶ�״̬�Լ�δ����״̬���޷�ʹ��
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
        //TODO:�������
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
        //TODO:ֻ���ڼ��ܽ���֮�����ʹ��
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
