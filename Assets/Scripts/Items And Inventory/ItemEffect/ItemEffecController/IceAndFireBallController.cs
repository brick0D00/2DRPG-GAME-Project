using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class IceAndFireBallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float movingSpeed;
    private int damage;
    private Player player;
    private float livingTime=10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = PlayerManager.instance.player;
        if (player.facingDirection < 0)
        {
            transform.Rotate(0, 180, 0);
        }
    }
    private void Disappear()
    {
        Destroy(gameObject);
    }
    private void Start()
    {
        anim.SetBool("KeepMoving", true);
        rb.gravityScale = 0;
        rb.velocity= new Vector2(movingSpeed* player.facingDirection,0);
        Invoke("Disappear", livingTime);
    }
    public void SetUPController(float _movingSpeed,int _damage)
    {
        movingSpeed = _movingSpeed;
        damage = _damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Entity>()?.Damage(player.stats.Calculate_Magicdamage(damage), player.transform.position, AttackLevel.NormalLevelAttack, player.stats.currentAttackType);
    }

}
