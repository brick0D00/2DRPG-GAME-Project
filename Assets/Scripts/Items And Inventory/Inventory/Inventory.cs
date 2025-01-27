using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;
    public int startCoins;
    public List<ItemData> startEquipment;

    //List  euipment作为装备栏 inventory作道具栏  stash作为仓库背包
    [SerializeField] private List<InventoryItem> equipment;
    [SerializeField] private List<InventoryItem> inventory;//放没装备的装备
    [SerializeField] private List<InventoryItem> stash;//只放材料

    //类似对照表用来记录背包中是否有对应的Item
    private Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    private Dictionary<ItemData, InventoryItem> InventoryDictionary;
    private Dictionary<ItemData, InventoryItem> StashDictionary;

    [Header("背包UI")]
    [SerializeField] private Transform inventorySlotsParent;//作为父载体
    [SerializeField] private Transform stashSlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private Transform UIStatSlotsParent;

    private UI_ItemSlot[] inventoryItemSlots;//道具栏插槽数组
    private UI_ItemSlot[] stashItemSlots;//仓库插槽数组
    private UI_ItemEquipmentSlot[] equipmentSlots;//装备栏插槽数组
    private UI_StatSlot[] statsSlots;

    [Header("道具冷却")]
    public float flaskCoolDown = 20f;
    public float lastTimeUseFlask;
    public float lastTimeTriggerAromr;

    [Header("道具数据库")]
    [SerializeField]private List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItemData;
    public List<ItemData_Equipment> loadedEquipment;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    #region Start
    private void Start()
    {
        inventory = new List<InventoryItem>();
        InventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        StashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventorySlotsParent.GetComponentsInChildren<UI_ItemSlot>();//获取所有插槽inventorySlotsParent.GetComponentsInChildren还有这种transform操作
        stashItemSlots = stashSlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<UI_ItemEquipmentSlot>();
        statsSlots = UIStatSlotsParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartEquipment();

        lastTimeUseFlask = -999;
        lastTimeTriggerAromr = -999;
    }

    private void AddStartEquipment()
    {
        //加载已有装备

        bool notNeedStartEquipment = false;

        foreach (var equipment in loadedEquipment)
        {
            EquipItem(equipment);
            notNeedStartEquipment = true;
        }

        foreach (InventoryItem item in loadedItemData)
        {
            for (int i = 0; i < item.stackSize; i++)
            {
                AddItem(item.data);
            }
            notNeedStartEquipment = true;
        }

        if (notNeedStartEquipment) return;


        //获取初始装备
        for (int i = 0; i < startEquipment.Count; i++)
        {
            AddItem(startEquipment[i]);
        }

    }
    #endregion

    private void UpdateSlotsUI()
    {   //更新所有插槽UI

        //清除一下
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].isEmpty()) continue;

            equipmentSlots[i].CleanUpSlot();
        }


        //显示UI
        for (int i = 0; i < equipment.Count; i++)
        {
            equipmentSlots[i].UpdateSlotUI(equipment[i]);
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlotUI(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlotUI(stash[i]);
        }
        UpdateUIStatSlots();

    }

    public void UpdateUIStatSlots()
    {
        for (int i = 0; i < statsSlots.Length; i++)
        {
            statsSlots[i].UpdateStatValueUI();
        }
    }
    #region Equip and Unload
    public void EquipItem(ItemData _item)
    {
        //TODO：将装备从道具栏装备到装备栏
        ItemData_Equipment newItemDataEquipment = _item as ItemData_Equipment;//父类转换成子类
        InventoryItem newItem = new InventoryItem(newItemDataEquipment);
        ItemData_Equipment itemToUnload = null;

        //进行字典遍历 如果装备了相同类型的装备，删除原有装备换上新装备
        foreach (var pairItem in equipmentDictionary)
        {
            if (pairItem.Key.equipmentType == newItemDataEquipment.equipmentType)
            {
                itemToUnload = pairItem.Key;
            }
        }
        if (itemToUnload != null)
        {
            UnloadEquipment(itemToUnload);
        }


        equipment.Add(newItem);
        equipmentDictionary.Add(newItemDataEquipment, newItem);

        newItemDataEquipment.AddModifiers();//提升属性
        newItemDataEquipment.ApplyItemEffectsForValue();//生产特殊效果

        //从背包中移除
        RemoveItem(_item);
    }

    public void UnloadEquipment(ItemData_Equipment itemToUnload)
    {   //TODO:卸下装备 并放回道具栏中
        if (itemToUnload == null) { return; }
        if (equipmentDictionary.TryGetValue(itemToUnload, out InventoryItem equipmentToDelet))
        {
            equipment.Remove(equipmentToDelet);
            equipmentDictionary.Remove(itemToUnload);

            //移除增益以及特殊效果
            itemToUnload.RemoveModifiers();
            itemToUnload.RemoveItemEffectsForValue();

            AddItem(itemToUnload);
        }

    }
    #endregion
    #region AddItem
    public void AddItem(ItemData _item)
    {
        //TODO:更新物品同时更新UI

        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            //是装备同时有装备空间就加入到道具栏
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            //是材料就加入到仓库
            AddToStash(_item);
        }

        UpdateSlotsUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (StashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem _newItem = new InventoryItem(_item);
            stash.Add(_newItem);
            StashDictionary.Add(_item, _newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (InventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //如果背包中已经有这个了，增加数量即可
            value.AddStack();
        }
        else
        {
            InventoryItem _newItem = new InventoryItem(_item);
            inventory.Add(_newItem);
            InventoryDictionary.Add(_item, _newItem);
        }
    }
    #endregion
    #region RemoveItem
    public void RemoveItem(ItemData _item)
    {
        //TODO:移除物品同时更新UI
        if (InventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                //背包中数量小于等于1，就直接移除
                inventory.Remove(inventoryValue);
                InventoryDictionary.Remove(_item);
            }
            else
            {
                inventoryValue.RemoveStack();
            }
        }

        if (StashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                StashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotsUI();
    }
    #endregion
    public bool TryToCraft(ItemData_Equipment _itemToCraft)
    {
        //TODO:查看是否可以合成,如果可以就直接合成

        bool canCraft = false;
        List<InventoryItem> UsedMaterial = new List<InventoryItem>();

        //遍历查看所需材料
        for (int i = 0; i < _itemToCraft.craftRquest.Count; i++)
        {
            if (StashDictionary.TryGetValue(_itemToCraft.craftRquest[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _itemToCraft.craftRquest[i].stackSize)
                {
                    //数量不足无法合成
                    return canCraft;
                }
                else
                {
                    //记录消耗的材料
                    UsedMaterial.Add(stashValue);
                }
            }
            else
            {
                //没有材料无法合成
                return canCraft;
            }
        }
        canCraft = true;

        //删除使用的材料
        for (int i = 0; i < UsedMaterial.Count; i++)
        {
            RemoveItem(UsedMaterial[i].data);
        }

        //添加生成的材料
        AddItem(_itemToCraft);
        return canCraft;
    }
    public List<InventoryItem> GetEquipmentsList()
    {
        return equipment;
    }
    public List<InventoryItem> GetStashList()
    {
        return stash;
    }
    public List<InventoryItem> GetInventoryList()
    {
        return inventory;
    }
    public ItemData_Equipment GetWantedEquipment(EquipmentType _wantedType)
    {
        //TODO:更具想要的装备类型返回装备

        ItemData_Equipment wantedEquipment = null;
        foreach (var pairs in equipmentDictionary)
        {
            if (pairs.Key.equipmentType == _wantedType)
            {
                wantedEquipment = pairs.Value.data as ItemData_Equipment;
            }
        }

        return wantedEquipment;
    }

    public void TryToUseFlask()
    {   

        //TODO:使用饰品（饮料）
        ItemData_Equipment wantedFlask = GetWantedEquipment(EquipmentType.Flask);
        Player player= PlayerManager.instance.player;
        if (wantedFlask != null)
        {
            player.fx.CreatePopUpText("未装备合剂");
            bool canUseFlask = Time.time - lastTimeUseFlask > wantedFlask.euipmentCoolDown;
            if (canUseFlask)
            {
                UIController.instance.GetUI_InGame().SetUpImageForFlask();
                wantedFlask.ApplyItemEffectsForDrink();

                AudioManager.instance.PlaySFX(3, null);
                Debug.Log(114);
                lastTimeUseFlask = Time.time;
            }
            else
            {
                player.fx.CreatePopUpText("正在冷却");
            }

        }

    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlots.Length)
        {
            return false;
        }
        return true;
    }

    public void LoadData(GameData _data)
    {
        //TODO:装填loadedItemData和loadedEquipment
        //具体装备loadedItemData和loadedEquipment会在Start的AddStartEquipment中装备


        //加载道具栏和仓库
        foreach (var pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    //存在这个道具ID且不为空，逐个查找
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    //装填道具
                    loadedItemData.Add(itemToLoad);
                }
            }
        }

        //加载装备
        foreach (string equipmentID in _data.equipmentsID)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == equipmentID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }

        }
    }

    public void SaveData(ref GameData _data)
    {
        //TODO:保存库存数据

        //提前清除一下避免越存越多
        _data.inventory.Clear();
        _data.equipmentsID.Clear();

        foreach (var pair in InventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (var pair in StashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (var pair in equipmentDictionary)
        {
            _data.equipmentsID.Add(pair.Key.itemID);
        }
    }


#if UNITY_EDITOR
    [ContextMenu("Fill Up ItemDatabase")]
    private void FillUpItemDatabase()=> itemDataBase=new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        //TODO:获取所有物品ID的数据库这样就不用手动添加了
        List<ItemData> TempitemDataBase = new List<ItemData>();
        //改了路径的话要记得
        string[] assetIDArray = AssetDatabase.FindAssets("", new[] { "Assets/ItemsData/Items" });

        foreach (string SOName in assetIDArray)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var ItemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            TempitemDataBase.Add(ItemData);
        }
        return TempitemDataBase;
    }
#endif
}
