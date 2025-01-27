using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/BuffForDrink")]
public class ItemEffect_BuffForDrink : ItemEffect
{
    [SerializeField] BuffType myBuffType;
    [SerializeField] private int buffStrength;
    [SerializeField] private float buffDuration;
    public override void ExecuteEffectsForDrink()
    {
        GetBuff();
        //Ö®ºóÒÆ³ýBuff
    }
    private void GetBuff()
    {
        PlayerManager.instance.player.stats.AddBuffFor(myBuffType, buffStrength, buffDuration);
    }
}
