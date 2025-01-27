using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/ThunderSword")]
public class ItemEffects_ThunderSword : ItemEffect
{
    [SerializeField] private GameObject thunderExplodePrefab;
    [SerializeField] private int thunderExplodeDamage;
    private float chanceToExplode = .5f;
    public override void ExecuteEffectsForAttack(Vector3 _enermyPostion)
    {
        //每次攻击有50%概率产生落雷
        if (chanceToExplode >= Random.Range(0, .5f))
        {
            CreateThunderExplode(_enermyPostion);
        } 
    }

    public override void ExecuteEffectsForDefend()
    {
        base.ExecuteEffectsForDefend();
    }

    public override void ExecuteEffectsForValue()
    {
        PlayerManager.instance.player.stats.SetCurrentAttackType(AttackType.lightingAttack);
    }

    public override void RemoveEffectsForAttack()
    {
        base.RemoveEffectsForAttack();
    }

    public override void RemoveEffectsForDefend()
    {
        base.RemoveEffectsForDefend();
    }

    public override void RemoveEffectsForValue()
    {
        PlayerManager.instance.player.stats.SetCurrentAttackType(AttackType.Ad);

    }
    private void CreateThunderExplode(Vector3 _enermyPostion)
    {
        GameObject newThunderExplode = Instantiate(thunderExplodePrefab, _enermyPostion, Quaternion.identity);
        ThunderExplodeController myController = newThunderExplode.GetComponent<ThunderExplodeController>();
        myController.SetUpThunderExplodeController(thunderExplodeDamage);
    }
}
