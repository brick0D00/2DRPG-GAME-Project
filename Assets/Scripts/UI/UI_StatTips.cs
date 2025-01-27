using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;

    public void ShowStatTip(string _statDescription, Vector2 _mousePostion)
    {
        statDescription.text = _statDescription;
        float xOffset = 250f, yOffset = 150f;
        
        transform.position = new Vector2(_mousePostion.x + xOffset, _mousePostion.y + yOffset);
        gameObject.SetActive(true);
        
    }

    public void HideStatTip()
    {
        statDescription.text = "";
        gameObject.SetActive(false);
    }
   
}
