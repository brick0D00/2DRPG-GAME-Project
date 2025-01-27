using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
//TODO:封装出一个可以序列化操作的字典
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]private List<TKey> keys = new List<TKey>();
    [SerializeField]private List<TValue> values = new List<TValue>();   
    public void OnBeforeSerialize()
    {   
        //将先前的内容清除
        keys.Clear(); 
        values.Clear();

        foreach(KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }

    }
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("键值对数量不对等");
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}
