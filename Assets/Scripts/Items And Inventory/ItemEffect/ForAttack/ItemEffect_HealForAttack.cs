using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/HealForAttack")]
public class ItemEffect_HealForAttack : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffectsForAttack(Vector3 _enermyPostion)
    {
        HealMyselfWith_percent();
    }
    private void HealMyselfWith_percent()
    {
        //TODO:ÿ�ι����ظ�healPercent�������ֵ
        CharacterStats myStat =PlayerManager.instance.player.stats;
        int healValue = Mathf.RoundToInt(myStat.Caculate_MaxHealthValue() * healPercent);
        myStat.HealMyselfWith(healValue);
    }

}
