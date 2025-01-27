using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeExplodeController : MonoBehaviour
{
    private CircleCollider2D cd;
    private Animator anim;
    private int explodeDamage;

    private void Awake()
    {
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetBool("Explode", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = PlayerManager.instance.player;
        collision.GetComponent<Enermy>()?.Damage(player.stats.Calculate_Magicdamage(explodeDamage), player.transform.position, AttackLevel.SuperLevelAttack, AttackType.lightingAttack);
    }
    public void SetUpRTEController(int _explodeDamage)
    {
        explodeDamage = _explodeDamage;
    }


    private void ExplodeFinishTrigger()//FOR Animation Trigger
    {
        anim.SetBool("Explode", false);
        FreezeController.instance.UnfreezeAll();
        PlayerManager.instance.player.MakeItTransparent(false);
        Destroy(gameObject);
    }

}
