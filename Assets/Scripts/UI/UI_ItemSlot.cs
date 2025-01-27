using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler//点击的时候调用接口
{   //TODO:用于控制道具插槽的UI

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

        itemImage.color = Color.white;//让其可见
        if (item != null)
        {
            //显示图像
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {   //显示数量
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                //只有一个就不显示
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
        //在点击挂载有这个函数的物体的时候会执行
        if (item == null) { return; }
        //删除物品
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
        //TODO:鼠标放上去的时候显示一下信息
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
