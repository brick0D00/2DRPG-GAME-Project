using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{   
    //TODO:用来控制工作台左侧的类型选择按钮

    [SerializeField] private Transform craftParent;
    [SerializeField] private GameObject craftSlotPrefab;
    

    //用来可合成的装备列表
    [SerializeField] private List<ItemData_Equipment> craftEquipmentList;

    private void Start()
    {   
        //TODO:初始化一下
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftEquipmentList();
    }

    private void SetUpCraftEquipmentList()
    {   

        //将原本的槽位清空
        for(int i=0; i < craftParent.childCount; i++)
        {
            Destroy(craftParent.GetChild(i).gameObject);
        }

        for(int i=0; i < craftEquipmentList.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipmentList[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftEquipmentList();
    }
}
