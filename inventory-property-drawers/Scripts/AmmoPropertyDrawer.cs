// AmmoPropertyDrawer inherits from PropertyDrawer because Ammo is a plain [Serializable] struct,
// not a UxmlObject. There is no UxmlSerializedDataPropertyView to establish a relative binding
// context, so the UI is built in C# and binding paths use absolute SerializedProperty.propertyPath
// values. A ProgressBar provides visual feedback for the current ammo fill level.
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Ammo))]
public class AmmoPropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        var count = property.FindPropertyRelative("count");
        var maxCount = property.FindPropertyRelative("maxCount");

        var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };

        var countField = new IntegerField("Ammo") { isDelayed = true, bindingPath = count.propertyPath };
        countField.AddToClassList(IntegerField.alignedFieldUssClassName);
        row.Add(countField);
        row.Add(new Label("/") { style = { marginLeft = 2, marginRight = 2 } });

        var maxCountField = new IntegerField { isDelayed = true, bindingPath = maxCount.propertyPath, style = { width = 50 } };
        row.Add(maxCountField);
        root.Add(row);

        var ammoBar = new ProgressBar();
        root.Add(ammoBar);

        void UpdateBar()
        {
            ammoBar.highValue = Mathf.Max(maxCount.intValue, 1);
            ammoBar.value = count.intValue;
            ammoBar.title = $"{count.intValue}/{maxCount.intValue}";
        }

        countField.TrackPropertyValue(count, p =>
        {
            count.intValue = Mathf.Min(p.intValue, maxCount.intValue);
            property.serializedObject.ApplyModifiedProperties();
            UpdateBar();
        });

        maxCountField.TrackPropertyValue(maxCount, p =>
        {
            count.intValue = Mathf.Min(count.intValue, p.intValue);
            property.serializedObject.ApplyModifiedProperties();
            UpdateBar();
        });

        root.Bind(property.serializedObject);
        UpdateBar();

        return root;
    }
}
