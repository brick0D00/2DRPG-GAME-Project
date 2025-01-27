using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/IceAndFireSword")]
public class ItemEffect_IceAndFireSword : ItemEffect
{   //变换冰火双刀同时射出攻击球
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private int iceAndFireDamage;
    [SerializeField] private float movingSpeed = 15;
    #region OVERRIDE
    public override void ExecuteEffectsForAttack(Vector3 _enermyPostion)
    {   
        //TODO:交替使用冰霜和火焰，同时第三下攻击产生火球
        ChangeSwordPower();
        if (PlayerManager.instance.player.primaryAttack.comboCounter==2)
        {   
            CreateIceAndFireBall();
        }
    }

    public override void RemoveEffectsForValue()
    {
        CharacterStats playerStat = PlayerManager.instance.player.stats;
        playerStat.SetCurrentAttackType(AttackType.Ad);
    }
    #endregion
    private void ChangeSwordPower()
    {
        CharacterStats playerStat = PlayerManager.instance.player.stats;
        if (playerStat.currentAttackType != AttackType.fireAttack)
        {
            playerStat.SetCurrentAttackType(AttackType.fireAttack);
        }
        else
        {
            playerStat.SetCurrentAttackType(AttackType.iceAttack);
        }
    }
    private void CreateIceAndFireBall()
    {
        GameObject myIceAndFireBall = Instantiate(iceAndFirePrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);
        IceAndFireBallController myController= myIceAndFireBall.GetComponent<IceAndFireBallController>();
        myController.SetUPController(movingSpeed, iceAndFireDamage);
    }
    
}
