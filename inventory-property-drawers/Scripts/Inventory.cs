using System.Collections.Generic;
using UnityEngine.UIElements;

[UxmlObject]
public partial class Inventory
{
    List<Item> m_Items = new List<Item>();
    Dictionary<int, Item> m_ItemDictionary = new Dictionary<int, Item>();

    [UxmlAttribute]
    int nextItemId = 1;

    [UxmlObjectReference("Items")]
    public List<Item> items
    {
        get => m_Items;
        set
        {
            m_Items = value;
            m_ItemDictionary.Clear();
            foreach (var item in m_Items)
            {
                m_ItemDictionary[item.id] = item;
            }
        }
    }

    public Item GetItem(int id) => m_ItemDictionary.TryGetValue(id, out var item) ? item : null;
}