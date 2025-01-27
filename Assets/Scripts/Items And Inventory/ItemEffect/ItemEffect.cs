using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffectsForValue()
    {   
        //TODO:增加常态的特殊属性增益

    }
    public virtual void RemoveEffectsForValue()
    {
  
    }

    public virtual void ExecuteEffectsForAttack(Vector3 _enermyPostion)
    {
        //TODO:在攻击的时候触发的特殊效果
    }
    public virtual void RemoveEffectsForAttack() { }
    public virtual void ExecuteEffectsForDefend()
    {

    }
    public virtual void RemoveEffectsForDefend()
    {

    }

    public virtual void ExecuteEffectsForThrowSword(Vector3 _enermyPostion)
    {

    }
    public virtual void ExecuteEffectsForDrink()
    {

    }
}
