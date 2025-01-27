using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDrinkController : MonoBehaviour
{   

    public static RemoveDrinkController Instance;
    //·ßÅ­ºÏ¼Á
    private float angryDuration;
    private int angryStrength;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    public void RemoveAngry(float _angryDuration,int _angryStrength)
    {   
        angryStrength = _angryStrength;
        Invoke("MyRemoveAngry", _angryDuration);
    }
    private void MyRemoveAngry()
    {
        CharacterStats playerStat = PlayerManager.instance.player.stats;
        playerStat.strength.RemoveModifier(angryStrength);
    }
}
