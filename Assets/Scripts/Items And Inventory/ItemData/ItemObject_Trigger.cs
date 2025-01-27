using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject;
    private void Awake()
    {
        myItemObject = GetComponentInParent<ItemObject>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        myItemObject.PickUpItem(collision);
    }
}
