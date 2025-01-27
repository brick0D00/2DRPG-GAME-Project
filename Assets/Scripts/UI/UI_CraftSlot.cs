using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {   
        //¸üÐÂÍ¼ÎÄ
        ui.craftWindow.SetUpCraftWindow(item.data as ItemData_Equipment);
    }

    public void SetUpCraftSlot(ItemData_Equipment _item)
    {
        item.data = _item;
        itemText.text = _item.name;
        itemImage.sprite = _item.icon;
    }

}
