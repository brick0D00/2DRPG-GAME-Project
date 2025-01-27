using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private float colorLoseRate;

    public void SetUpAfterImage(float _colorLoseRate)
    {
        colorLoseRate = _colorLoseRate;
    }

    private void Update()
    {   
        //TODO:如果变得透明就自毁
        float tempA=sr.color.a-colorLoseRate*Time.deltaTime;

        sr.color=new Color(sr.color.r,sr.color.g,sr.color.b,tempA);

        if(sr.color.a<=0)
        {
            Destroy(gameObject);
        }
    }
}
