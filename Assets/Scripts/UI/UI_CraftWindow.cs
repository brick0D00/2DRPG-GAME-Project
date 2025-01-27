using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;

    [SerializeField] private Image[] needMaterialsImage;

    [SerializeField] private Button craftButton;

    public void SetUpCraftWindow(ItemData_Equipment _item)
    {   
        //TODO:设置合成窗口

        //移除合成按钮上面的所有监听，在后续添加合成监听
        craftButton.onClick.RemoveAllListeners();

        //将多余部分透明化
        for(int i = 0; i < needMaterialsImage.Length; i++)
        {   
            needMaterialsImage[i].color = Color.clear;
            needMaterialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        //将需要的材料装填，显示图片以及其所需的数量
        for(int i = 0; i < _item.craftRquest.Count; i++)
        {   
            needMaterialsImage[i].sprite=_item.craftRquest[i].data.icon;
            needMaterialsImage[i].color= Color.white;

            TextMeshProUGUI tempText = needMaterialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            tempText.text = _item.craftRquest[i].stackSize.ToString();
            tempText.color= Color.white;
        }

        //设置图片名字以及描述
        itemIcon.sprite=_item.icon;
        itemName.text = _item.itemName;
        itemDescription.text=_item.GetDescription();

        //为按钮添加合成监听
        craftButton.onClick.AddListener(() => Inventory.instance.TryToCraft(_item));
    }
}
