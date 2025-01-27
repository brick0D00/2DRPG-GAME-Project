using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/FreezeEntiyForDefend")]
public class Effect_FreezeEntiyForDefend : ItemEffect
{
    [Range(0, 1f)]
    [SerializeField] private float tiggerHealthPercent;
    [SerializeField] private float freezeTime;
    [SerializeField] private float freezeCoolDown;
    public override void ExecuteEffectsForDefend()
    {   

        if (canTrigger())
        {
            FreezeController.instance.OnlyFreezeEnermyForSeconds(freezeTime);
            Debug.Log(114);
            Inventory.instance.lastTimeTriggerAromr = Time.time;
        }
    }
    public bool canTrigger()
    {   
        //血量低于一定比例且不在冷却中
        CharacterStats playerStat = PlayerManager.instance.player.stats;

        bool trigger_1=Time.time-Inventory.instance.lastTimeTriggerAromr> freezeCoolDown;

        float currentHealthPercent = ((float)playerStat.currentHealth) / ((float)playerStat.Caculate_MaxHealthValue());

        bool trigger_2=currentHealthPercent<tiggerHealthPercent;

        return trigger_1&&trigger_2;
    }
}
