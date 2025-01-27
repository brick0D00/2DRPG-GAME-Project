using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{   
    //TODO:作为游戏里生成的对象实例
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 burstForce;

    public void SetUpToDropItem(ItemData _itemData,Vector2 _velocity)
    {
        //TODO:设置掉落物品
        itemData = _itemData;
        MakeVisualAble();
        rb.velocity = _velocity;
    }

    private void MakeVisualAble()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object-" + itemData.name;
    }

    public void PickUpItem(Collider2D collision)
    {
        //TODO:捡起物体 死后不捡
        if (PlayerManager.instance.player.isdead) return;

        //装备栏满了
        if (itemData.itemType == ItemType.Equipment && Inventory.instance.CanAddItem() == false)
        {
            rb.velocity = new Vector2(0, 7);
        }

        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFX(6, null);
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
