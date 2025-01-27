using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderExplodeController : MonoBehaviour
{
    private Animator anim;
    private int damage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetBool("ThunderExplode", true);
    }
    public void SetUpThunderExplodeController(int _damage)
    {
        damage = _damage;
    }

    public void ExplodeFinishTrigger()
    {
        anim.SetBool("ThunderExplode",false);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D enermy)
    {
        Player player = PlayerManager.instance.player;
        enermy.GetComponent<Entity>().Damage(player.stats.Calculate_Magicdamage(damage), transform.position, AttackLevel.NormalLevelAttack, AttackType.lightingAttack);

    }
}
