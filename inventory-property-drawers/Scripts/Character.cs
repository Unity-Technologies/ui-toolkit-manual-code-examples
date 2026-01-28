using UnityEngine.UIElements;

[UxmlElement]
public partial class Character : VisualElement
{
    [UxmlObjectReference("inventory")]
    public Inventory Inventory { get; set; } = new Inventory();
}
