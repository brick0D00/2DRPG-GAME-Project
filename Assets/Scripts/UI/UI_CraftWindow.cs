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
        //TODO:���úϳɴ���

        //�Ƴ��ϳɰ�ť��������м������ں�����Ӻϳɼ���
        craftButton.onClick.RemoveAllListeners();

        //�����ಿ��͸����
        for(int i = 0; i < needMaterialsImage.Length; i++)
        {   
            needMaterialsImage[i].color = Color.clear;
            needMaterialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        //����Ҫ�Ĳ���װ���ʾͼƬ�Լ������������
        for(int i = 0; i < _item.craftRquest.Count; i++)
        {   
            needMaterialsImage[i].sprite=_item.craftRquest[i].data.icon;
            needMaterialsImage[i].color= Color.white;

            TextMeshProUGUI tempText = needMaterialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            tempText.text = _item.craftRquest[i].stackSize.ToString();
            tempText.color= Color.white;
        }

        //����ͼƬ�����Լ�����
        itemIcon.sprite=_item.icon;
        itemName.text = _item.itemName;
        itemDescription.text=_item.GetDescription();

        //Ϊ��ť��Ӻϳɼ���
        craftButton.onClick.AddListener(() => Inventory.instance.TryToCraft(_item));
    }
}
