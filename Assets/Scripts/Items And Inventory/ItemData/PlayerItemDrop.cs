using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("装备掉落几率")]
    [Range(0,1f)]
    [SerializeField] private float playerItemDropChance;

    public override void BurstItems()
    {
        //TODO:死后爆装备噜
        Inventory inventory = Inventory.instance;

        List <InventoryItem> currentEquipments = inventory.GetEquipmentsList();
        List<InventoryItem> currentStash=inventory.GetStashList();
        List<InventoryItem> currentInventory=inventory.GetInventoryList();

        List< InventoryItem >itemsToBurst= new List< InventoryItem >();//爆出去的装备
        List<InventoryItem> equipmentsToUnload = new List< InventoryItem >();


        //TODO:遍历玩家装备清单，掉落并移除
        for(int i = 0; i < currentEquipments.Count; i++)
        {
            if (canDrop(playerItemDropChance))
            {
                DropItem(currentEquipments[i].data);
                itemsToBurst.Add(currentEquipments[i]);
                equipmentsToUnload.Add(currentEquipments[i]);
            }
        }
        for (int i = 0; i < currentInventory.Count; i++)
        {
            if (canDrop(playerItemDropChance))
            {
                DropItem(currentInventory[i].data);
                itemsToBurst.Add(currentInventory[i]);
            }
        }
        for (int i = 0; i < currentStash.Count; i++)
        {
            if (canDrop(playerItemDropChance))
            {
                DropItem(currentStash[i].data);
                itemsToBurst.Add(currentStash[i]);
            }
        }
        //有BUG
       /* for (int i = 0; i < itemsToBurst.Count; i++) 
        {
            inventory.UnloadEquipment(itemsToBurst[i].data as ItemData_Equipment);
            inventory.RemoveItem(itemsToBurst[i].data);
        }*/
       for(int i = 0; i < equipmentsToUnload.Count; i++)
       {
            inventory.UnloadEquipment(equipmentsToUnload[i].data as ItemData_Equipment);
       }

        for (int i = 0; i < itemsToBurst.Count; i++)
        {
            inventory.RemoveItem(itemsToBurst[i].data);
        }
        

    }

    protected override bool canDrop(float _dropChance)
    {
        return _dropChance >= Random.Range(0, 1f);
    }
}
