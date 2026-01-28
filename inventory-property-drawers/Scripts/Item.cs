using UnityEngine.UIElements;
using UnityEngine;

[UxmlObject]
public abstract partial class Item
{
    [UxmlAttribute, HideInInspector]
    public int id;

    [UxmlAttribute]
    public string name;

    [UxmlAttribute]
    public float weight;
}
