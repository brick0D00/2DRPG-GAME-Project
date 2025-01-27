using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnermy;
    private BlackHoleController myblackHole;

    public void SetUpHotKey(KeyCode _myHotKey,Transform _myEnermy,BlackHoleController _myblackHole)
    {   
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;  
        myText.text = myHotKey.ToString();
        myEnermy = _myEnermy;
        myblackHole = _myblackHole;

    }
    private void Update()
    {
        if (Input.GetKeyUp(myHotKey))
        {   
            //按下之后就添加
            myblackHole.AddTarget(myEnermy);

            //让文本透明
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
