using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAnimEvents : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void ThunderFinishTrigger()
    {
        anim.SetBool("ThunderStrike", false);
        Destroy(gameObject);
    }
}
