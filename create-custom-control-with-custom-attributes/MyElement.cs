using UnityEngine.UIElements;

[UxmlElement]
partial class MyElement : VisualElement
{
    [UxmlAttribute]
    public string myString { get; set; } = "default_value";

    [UxmlAttribute]
    public int myInt { get; set; } = 2;
}
