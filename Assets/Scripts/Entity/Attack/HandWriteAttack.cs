using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class HandWriteAttack : MonoBehaviour
{
    [SerializeField] private AttackLevel attackLevel;
    [SerializeField] private AttackType attackType;
    [SerializeField] private int damage;

    protected virtual void OnTriggerEnter2D(Collider2D enermy)
    {
        Debug.Log(114);
        enermy.GetComponent<Entity>()?.Damage(damage, transform.position, attackLevel, attackType);

    }
}
