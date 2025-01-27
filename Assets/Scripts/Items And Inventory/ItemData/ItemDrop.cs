using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{   

    [SerializeField] private GameObject itemObjectPrefab;
    [SerializeField] private List<ItemDropWithChance> itemsToDrop;


    
    public virtual void BurstItems()
    {   
        //TODO：随机力度的掉落随机物品

        for(int i=0;i<itemsToDrop.Count; i++)
        {
            if (canDrop(itemsToDrop[i].dropChance) == false) { continue; }
            GameObject itemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity);

            Vector2 randomVelocity = new Vector2(Random.Range(-6, 6), Random.Range(15, 20));
            itemObject.GetComponent<ItemObject>().SetUpToDropItem(itemsToDrop[i].item, randomVelocity);
        }
    }
    protected virtual bool canDrop(float _dropChance)
    {
        return _dropChance>=Random.Range(0f,1f);
    }
    public virtual void DropItem(ItemData _itemData)
    {
        GameObject itemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-6, 6), Random.Range(15, 20));
        itemObject.GetComponent<ItemObject>().SetUpToDropItem(_itemData, randomVelocity);
    }
}
