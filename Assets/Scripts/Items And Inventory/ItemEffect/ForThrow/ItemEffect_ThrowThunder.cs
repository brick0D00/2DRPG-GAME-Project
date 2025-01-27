using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item Effect/ThrowThunder")]
public class ItemEffect_ThrowThunder : ItemEffect
{   //TODO:丢出的剑会附带雷电
    [SerializeField] private GameObject thunderExplodePrefab;
    [SerializeField] private int explodeDamage;

    public override void ExecuteEffectsForThrowSword(Vector3 _enermyPostion)
    {
        CreateThunderExplode(_enermyPostion);
    }
    private void CreateThunderExplode(Vector3 _enermyPostion)
    {
        GameObject newThunderExplode=Instantiate(thunderExplodePrefab,_enermyPostion,Quaternion.identity);
        ThunderExplodeController myController=newThunderExplode.GetComponent<ThunderExplodeController>();
        myController.SetUpThunderExplodeController(explodeDamage);
    }
}
