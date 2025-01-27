using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UI_ItemTips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowItemInfo(ItemData_Equipment _item,Vector2 _mousePostion)
    {
        itemNameText.text=_item.itemName;
        itemTypeText.text=_item.equipmentType.ToString();
        itemDescription.text=_item.GetDescription();

        float xOffset = -150f, yOffset = 150f;
        transform.position=new Vector2(_mousePostion.x+ xOffset, _mousePostion.y+yOffset);
        gameObject.SetActive(true);
    }
    

    public void HideItemInfo()
    {
        gameObject.SetActive(false);
    }
}
