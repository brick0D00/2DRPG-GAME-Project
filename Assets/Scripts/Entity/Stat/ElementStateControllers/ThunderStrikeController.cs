using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D bc;

    private void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        bc = GetComponentInChildren<BoxCollider2D>();
    }
    private void Start()
    {
        anim.SetBool("ThunderStrike", true);
    }
    private void OnTriggerEnter2D(Collider2D enermy)
    {
        enermy.GetComponent<Enermy>()?.stats.BeSmallStrike();
    }

}
