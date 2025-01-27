using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public float coolDownSetTime;
    //public bool canuse;
    protected float coolDownTimer;
    protected Player player;
    protected EntityFX fx;
    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        fx = player.GetComponent<EntityFX>();
        LoadSkillCheckUnlock();
    }
    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
        //canuse=IsSkillReady();
    }
    public virtual bool IsSkillReady()
    {
        if (coolDownTimer > 0)
        {
            player.fx.CreatePopUpText("������ȴ");
        }
        return coolDownTimer < 0;
    }
    public virtual void UseSkill()
    {
        coolDownTimer = coolDownSetTime;
    }
    public virtual bool TryToUseSkill()
    {
        if (IsSkillReady())
        {
            UseSkill();
            return true;
        }
        return false;
    }
    protected virtual void LoadSkillCheckUnlock()
    {   //TODO:�ڼ������ݵ�ʱ�򿴿��Ƿ��������

    }
}
