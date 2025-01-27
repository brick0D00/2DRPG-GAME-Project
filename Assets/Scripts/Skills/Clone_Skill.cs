using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{   
    //TODO���ڳ�̺�ԭ�����²�Ӱ ���ƺں
    [SerializeField] private GameObject clonePrefab;
    public float cloneFadingSpeed;
    public float cloneDuration;
    public bool canAttack;

    public void CreateClone()
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.transform.position = PlayerManager.instance.player.transform.position;
    }
    public void CreateClone(Transform _newtransform,Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.transform.position = _newtransform.position+_offset;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        CreateClone();
    }

    public override bool TryToUseSkill()
    {
        return base.TryToUseSkill();
    }
}
