using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventoryItem 
{   
    //对ItemData进行封装，在背包中可以达到多个道具叠加只占用一个格子
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newItemData)
    {
        data=_newItemData;
        //初始化数量为1
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
