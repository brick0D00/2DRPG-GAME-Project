using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallThunderStrikeController : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetBool("ThunderStrike", true);
    }
}
