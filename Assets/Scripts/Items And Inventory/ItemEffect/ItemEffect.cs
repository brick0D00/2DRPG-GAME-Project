using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffectsForValue()
    {   
        //TODO:���ӳ�̬��������������

    }
    public virtual void RemoveEffectsForValue()
    {
  
    }

    public virtual void ExecuteEffectsForAttack(Vector3 _enermyPostion)
    {
        //TODO:�ڹ�����ʱ�򴥷�������Ч��
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
