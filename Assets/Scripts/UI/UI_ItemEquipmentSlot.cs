using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemEquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;
    public bool isEmpty()
    {
        if (item== null)
        {
            return true;
        }
        return false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {   //Ð¶ÏÂ×°±¸
        Inventory.instance.UnloadEquipment(item.data as ItemData_Equipment);
        ui.itemTips.HideItemInfo();
    }

    private void OnValidate()
    {
        gameObject.name = "EquipmentSlot-"+equipmentType.ToString();
    }
    
}
