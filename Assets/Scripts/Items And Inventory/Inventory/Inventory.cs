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

    //List  euipment��Ϊװ���� inventory��������  stash��Ϊ�ֿⱳ��
    [SerializeField] private List<InventoryItem> equipment;
    [SerializeField] private List<InventoryItem> inventory;//��ûװ����װ��
    [SerializeField] private List<InventoryItem> stash;//ֻ�Ų���

    //���ƶ��ձ�������¼�������Ƿ��ж�Ӧ��Item
    private Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    private Dictionary<ItemData, InventoryItem> InventoryDictionary;
    private Dictionary<ItemData, InventoryItem> StashDictionary;

    [Header("����UI")]
    [SerializeField] private Transform inventorySlotsParent;//��Ϊ������
    [SerializeField] private Transform stashSlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private Transform UIStatSlotsParent;

    private UI_ItemSlot[] inventoryItemSlots;//�������������
    private UI_ItemSlot[] stashItemSlots;//�ֿ�������
    private UI_ItemEquipmentSlot[] equipmentSlots;//װ�����������
    private UI_StatSlot[] statsSlots;

    [Header("������ȴ")]
    public float flaskCoolDown = 20f;
    public float lastTimeUseFlask;
    public float lastTimeTriggerAromr;

    [Header("�������ݿ�")]
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

        inventoryItemSlots = inventorySlotsParent.GetComponentsInChildren<UI_ItemSlot>();//��ȡ���в��inventorySlotsParent.GetComponentsInChildren��������transform����
        stashItemSlots = stashSlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<UI_ItemEquipmentSlot>();
        statsSlots = UIStatSlotsParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartEquipment();

        lastTimeUseFlask = -999;
        lastTimeTriggerAromr = -999;
    }

    private void AddStartEquipment()
    {
        //��������װ��

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


        //��ȡ��ʼװ��
        for (int i = 0; i < startEquipment.Count; i++)
        {
            AddItem(startEquipment[i]);
        }

    }
    #endregion

    private void UpdateSlotsUI()
    {   //�������в��UI

        //���һ��
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


        //��ʾUI
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
        //TODO����װ���ӵ�����װ����װ����
        ItemData_Equipment newItemDataEquipment = _item as ItemData_Equipment;//����ת��������
        InventoryItem newItem = new InventoryItem(newItemDataEquipment);
        ItemData_Equipment itemToUnload = null;

        //�����ֵ���� ���װ������ͬ���͵�װ����ɾ��ԭ��װ��������װ��
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

        newItemDataEquipment.AddModifiers();//��������
        newItemDataEquipment.ApplyItemEffectsForValue();//��������Ч��

        //�ӱ������Ƴ�
        RemoveItem(_item);
    }

    public void UnloadEquipment(ItemData_Equipment itemToUnload)
    {   //TODO:ж��װ�� ���Żص�������
        if (itemToUnload == null) { return; }
        if (equipmentDictionary.TryGetValue(itemToUnload, out InventoryItem equipmentToDelet))
        {
            equipment.Remove(equipmentToDelet);
            equipmentDictionary.Remove(itemToUnload);

            //�Ƴ������Լ�����Ч��
            itemToUnload.RemoveModifiers();
            itemToUnload.RemoveItemEffectsForValue();

            AddItem(itemToUnload);
        }

    }
    #endregion
    #region AddItem
    public void AddItem(ItemData _item)
    {
        //TODO:������Ʒͬʱ����UI

        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            //��װ��ͬʱ��װ���ռ�ͼ��뵽������
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            //�ǲ��Ͼͼ��뵽�ֿ�
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
            //����������Ѿ�������ˣ�������������
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
        //TODO:�Ƴ���Ʒͬʱ����UI
        if (InventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                //����������С�ڵ���1����ֱ���Ƴ�
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
        //TODO:�鿴�Ƿ���Ժϳ�,������Ծ�ֱ�Ӻϳ�

        bool canCraft = false;
        List<InventoryItem> UsedMaterial = new List<InventoryItem>();

        //�����鿴�������
        for (int i = 0; i < _itemToCraft.craftRquest.Count; i++)
        {
            if (StashDictionary.TryGetValue(_itemToCraft.craftRquest[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _itemToCraft.craftRquest[i].stackSize)
                {
                    //���������޷��ϳ�
                    return canCraft;
                }
                else
                {
                    //��¼���ĵĲ���
                    UsedMaterial.Add(stashValue);
                }
            }
            else
            {
                //û�в����޷��ϳ�
                return canCraft;
            }
        }
        canCraft = true;

        //ɾ��ʹ�õĲ���
        for (int i = 0; i < UsedMaterial.Count; i++)
        {
            RemoveItem(UsedMaterial[i].data);
        }

        //������ɵĲ���
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
        //TODO:������Ҫ��װ�����ͷ���װ��

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

        //TODO:ʹ����Ʒ�����ϣ�
        ItemData_Equipment wantedFlask = GetWantedEquipment(EquipmentType.Flask);
        Player player= PlayerManager.instance.player;
        if (wantedFlask != null)
        {
            player.fx.CreatePopUpText("δװ���ϼ�");
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
                player.fx.CreatePopUpText("������ȴ");
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
        //TODO:װ��loadedItemData��loadedEquipment
        //����װ��loadedItemData��loadedEquipment����Start��AddStartEquipment��װ��


        //���ص������Ͳֿ�
        foreach (var pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    //�����������ID�Ҳ�Ϊ�գ��������
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    //װ�����
                    loadedItemData.Add(itemToLoad);
                }
            }
        }

        //����װ��
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
        //TODO:����������

        //��ǰ���һ�±���Խ��Խ��
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
        //TODO:��ȡ������ƷID�����ݿ������Ͳ����ֶ������
        List<ItemData> TempitemDataBase = new List<ItemData>();
        //����·���Ļ�Ҫ�ǵ�
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
