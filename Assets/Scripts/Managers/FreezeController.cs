using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeController : MonoBehaviour
{

    public static FreezeController instance;
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
    public  void FreezeAll()
    {
        Entity.allEntityCallToFreeze = true;
    }
    public void OnlyFreezeEnermy()
    {
        Enermy.allEnermyCallToFreeze = true;
    }
    public void UnfreezeAll()
    {
        Entity.allEntityCallToFreeze = false;
    }
    public void OnlyUnfreezeEnermy()
    {
        Enermy.allEnermyCallToFreeze = false;
    }

    public void FreezeAllForSeconds(float _freezeDuration)
    {
        StartCoroutine(IE_FreezeAllForSeconds(_freezeDuration));
    }

    public void OnlyFreezeEnermyForSeconds(float _freezeDuration)
    {
        StartCoroutine(IE_OnlyFreezeEnermyForSeconds(_freezeDuration));
    }

    public IEnumerator IE_FreezeAllForSeconds(float _freezeDuration)
    {
        FreezeAll();
        yield return new WaitForSeconds(_freezeDuration);
        UnfreezeAll();
    }
    public IEnumerator IE_OnlyFreezeEnermyForSeconds(float _freezeDuration)
    {
        OnlyFreezeEnermy();
        yield return new WaitForSeconds(_freezeDuration);
        OnlyUnfreezeEnermy();
    }

}
