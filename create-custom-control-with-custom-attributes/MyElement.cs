using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
partial class MyElement : VisualElement
{
    string _myString = "default_value";
    int _myInt = 2;

    [UxmlAttribute]
    public string myString
    {
        get => _myString;
        set
        {
            _myString = value;
            Debug.Log($"myString set to: {_myString}");
            // React here, e.g. update UI
        }
    }

    [UxmlAttribute]
    public int myInt
    {
        get => _myInt;
        set
        {
            _myInt = value;
            Debug.Log($"myInt set to: {_myInt}");
            // React here, e.g. update UI
        }
    }
}
