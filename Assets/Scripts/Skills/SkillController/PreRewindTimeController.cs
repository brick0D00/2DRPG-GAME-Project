using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRewindTimeController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim.SetBool("Disappear", true);
    }
    public void PreRewindTimeFinishTrigger()
    {
        anim.SetBool("Disappear", false);
        Destroy(gameObject);
    }
}
