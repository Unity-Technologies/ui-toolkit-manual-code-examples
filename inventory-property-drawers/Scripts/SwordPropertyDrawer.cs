// SwordPropertyDrawer showcases the C# approach to UxmlAttributeField and UxmlAttributeFieldDecorator.
// It overrides CreateChildPropertiesGUI to prepend a type label, then delegates to CreateChildPropertyGUI
// for each property. CreateChildPropertyGUI customizes slashDamage while letting the base class handle
// all other properties (name, weight) with the default UxmlAttributeField.
using Unity.UIToolkit.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Sword.UxmlSerializedData))]
public class SwordPropertyDrawer : UxmlSerializedDataPropertyDrawer
{
    protected override void CreateChildPropertiesGUI(VisualElement container, SerializedProperty property)
    {
        container.Add(ItemTypeLabel("Sword"));
        base.CreateChildPropertiesGUI(container, property);
    }

    protected override void CreateChildPropertyGUI(VisualElement container, SerializedProperty property,
        SerializedProperty childProperty)
    {
        if (childProperty.name == "slashDamage")
        {
            // Pattern 3 & 4 in C#: UxmlAttributeFieldDecorator wrapping an explicit Slider.
            // This uses the same control type as the Slider in InventoryDrawer.uxml, but created
            // in code rather than UXML.
            UxmlAttributeFieldDecorator decorator = new UxmlAttributeFieldDecorator();
            Slider slider = new Slider(childProperty.displayName, 1, 100)
            {
                showInputField = true,
                bindingPath = childProperty.propertyPath
            };
            slider.AddToClassList(Slider.alignedFieldUssClassName);
            decorator.Add(slider);
            container.Add(decorator);
        }
        else
        {
            // Pattern 3: let the base class create a UxmlAttributeField for name and weight.
            base.CreateChildPropertyGUI(container, property, childProperty);
        }
    }

    static Label ItemTypeLabel(string typeName) => new Label(typeName)
    {
        style =
        {
            unityFontStyleAndWeight = FontStyle.Bold,
            paddingLeft = 2,
            paddingBottom = 2,
            marginBottom = 2,
            borderBottomWidth = 1,
            borderBottomColor = new Color(0.5f, 0.5f, 0.5f, 0.3f),
        }
    };
}
