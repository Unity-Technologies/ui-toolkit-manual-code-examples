using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Tire))]
public class Tire_PropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create drawer UI using C#.
        var popup = new UnityEngine.UIElements.PopupWindow();
        popup.text = "Tire Details";
        popup.Add(new PropertyField(property.FindPropertyRelative("m_AirPressure"), "Air Pressure (psi)"));
        popup.Add(new PropertyField(property.FindPropertyRelative("m_ProfileDepth"), "Profile Depth (mm)"));
        container.Add(popup);

        // Return the finished UI.
        return container;
    }
}