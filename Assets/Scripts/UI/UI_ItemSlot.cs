using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler//�����ʱ����ýӿ�
{   //TODO:���ڿ��Ƶ��߲�۵�UI

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UIController ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui=GetComponentInParent<UIController>();
    }
    public void UpdateSlotUI(InventoryItem _newItem)
    {
        item= _newItem;

        itemImage.color = Color.white;//����ɼ�
        if (item != null)
        {
            //��ʾͼ��
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {   //��ʾ����
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                //ֻ��һ���Ͳ���ʾ
                itemText.text = "";
            }
        }
    }
    public void CleanUpSlot()
    {
        item= null;
        itemImage.sprite= null;
        itemText.text= "";
        itemImage.color=Color.clear;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //�ڵ����������������������ʱ���ִ��
        if (item == null) { return; }
        //ɾ����Ʒ
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment) 
        {
            AudioManager.instance.PlaySFX(4, null);
            Inventory.instance.EquipItem(item.data);   
        }
        ui.itemTips.HideItemInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        //TODO:������ȥ��ʱ����ʾһ����Ϣ
        if(item == null) { return; }
        if(item.data.itemType == ItemType.Material) { return; }
        Vector2 mousePostion=Input.mousePosition;
        ui.itemTips.ShowItemInfo(item.data as ItemData_Equipment,mousePostion);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) { return; }
        if (item.data.itemType == ItemType.Material) { return; }
        ui.itemTips.HideItemInfo();
    }
}
