using UnityEngine;

public class Attack : MonoBehaviour
{
    protected Entity entity;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D enermy)
    {
        Debug.Log(114);
        enermy.GetComponent<Entity>()?.Damage(entity.stats.Calculate_ADdamage(), entity.transform.position, AttackLevel.NormalLevelAttack, entity.stats.currentAttackType);

    }
}
