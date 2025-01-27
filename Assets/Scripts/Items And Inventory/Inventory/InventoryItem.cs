using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventoryItem 
{   
    //��ItemData���з�װ���ڱ����п��Դﵽ������ߵ���ֻռ��һ������
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newItemData)
    {
        data=_newItemData;
        //��ʼ������Ϊ1
        AddStack();
    }
    public void AddStack()
    {
        stackSize++;
    }
    public void RemoveStack()
    {
        stackSize--;
    }
}
