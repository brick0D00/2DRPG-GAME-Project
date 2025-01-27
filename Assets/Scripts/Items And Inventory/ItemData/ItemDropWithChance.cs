using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemDropWithChance
{
    public ItemData item;
    [Range(0f, 1f)]
    public float dropChance;
}
