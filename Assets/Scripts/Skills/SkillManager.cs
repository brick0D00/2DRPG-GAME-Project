using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill dash { get; private set; }
    public RewindTime_Skill rewindTime { get; private set; }
    public Clone_Skill clone { get; private set; }
    public ThrowSword_Skill throwSword { get; private set; }
    public BlackHole_Skill blackHole { get; private set; }
    public Parry_Skill parry { get; private set; }
    public ParryAttack_Skill parryAttack { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        rewindTime = GetComponent<RewindTime_Skill>();
        clone = GetComponent<Clone_Skill>();
        throwSword = GetComponent<ThrowSword_Skill>();
        blackHole = GetComponent<BlackHole_Skill>();
        parry = GetComponent<Parry_Skill>();
        parryAttack = GetComponent<ParryAttack_Skill>();
    }
}
