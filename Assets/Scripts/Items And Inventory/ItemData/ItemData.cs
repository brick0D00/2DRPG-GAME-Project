using System.Text;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
public enum ItemType
{
    Material,
    Equipment
}
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Item")]//在列表中生成选项
public class ItemData : ScriptableObject//用来记录固定数据
{   
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemID;

    [TextArea]
    public string itemDescription;

    protected StringBuilder sb=new StringBuilder();

    private void OnValidate()
    {
        //TODO:自动分配ID
#if UNITY_EDITOR
        string path=AssetDatabase.GetAssetPath(this);
        itemID= AssetDatabase.AssetPathToGUID(path);
#endif
    }
    public virtual string GetDescription()
    {
        return "";
    }
}
