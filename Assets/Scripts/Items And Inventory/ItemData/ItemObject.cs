using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{   
    //TODO:��Ϊ��Ϸ�����ɵĶ���ʵ��
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 burstForce;

    public void SetUpToDropItem(ItemData _itemData,Vector2 _velocity)
    {
        //TODO:���õ�����Ʒ
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
        //TODO:�������� ���󲻼�
        if (PlayerManager.instance.player.isdead) return;

        //װ��������
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
