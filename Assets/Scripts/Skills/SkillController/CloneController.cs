using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField]private LayerMask whatIsEnermy;
    private float cloneFadingSpeed;
    private float cloneDuration;
    private float cloneTimer;
    private bool canAttack;
    private int enermyDetectDistance=15;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cloneFadingSpeed = SkillManager.instance.clone.cloneFadingSpeed;
        cloneDuration = SkillManager.instance.clone.cloneDuration;
        canAttack = SkillManager.instance.clone.canAttack;
    }
    private void Start()
    {
        FaceToClosestTarget();
        cloneTimer = cloneDuration;
        if (canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1,3));
        }
      
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            //做出一个逐步消失的效果
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * cloneFadingSpeed));
        }
        if (sr.color.a < 0)
        {
            Destroy(gameObject);
        }

    }
    public void AttackFinishTrigger()
    {
        cloneTimer = .1f;
    }
    private void FaceToClosestTarget()
    {
        RaycastHit2D right =Physics2D.Raycast(transform.position, Vector2.right, enermyDetectDistance, whatIsEnermy );
        RaycastHit2D left = Physics2D.Raycast(transform.position, -Vector2.right, enermyDetectDistance, whatIsEnermy);

        if (right.distance < left.distance)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
