// Note that this example creates a PropertyDrawer for the Ammo type because it's not a UxmlObject.
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Ammo))]
public class AmmoPropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement { style = { flexDirection = FlexDirection.Row } };

        var count = property.FindPropertyRelative("count");
        var maxCount = property.FindPropertyRelative("maxCount");

        var ammoField = new IntegerField("Ammo") { isDelayed = true, bindingPath = count.propertyPath };
        ammoField.TrackPropertyValue(count, p =>
        {
            count.intValue = Mathf.Min(p.intValue, maxCount.intValue);
            property.serializedObject.ApplyModifiedProperties();
        });
        root.Add(ammoField);
        root.Add(new Label("/"));

        var countField = new IntegerField { isDelayed = true, bindingPath = maxCount.propertyPath };
        countField.TrackPropertyValue(maxCount, p =>
        {
            count.intValue = Mathf.Min(p.intValue, count.intValue);
            property.serializedObject.ApplyModifiedProperties();
        });
        root.Add(countField);

        root.Bind(property.serializedObject);

        return root;
    }
}