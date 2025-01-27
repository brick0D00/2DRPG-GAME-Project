using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{   
    //TODO:�������ƹ���̨��������ѡ��ť

    [SerializeField] private Transform craftParent;
    [SerializeField] private GameObject craftSlotPrefab;
    

    //�����ɺϳɵ�װ���б�
    [SerializeField] private List<ItemData_Equipment> craftEquipmentList;

    private void Start()
    {   
        //TODO:��ʼ��һ��
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftEquipmentList();
    }

    private void SetUpCraftEquipmentList()
    {   

        //��ԭ���Ĳ�λ���
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
