// GunPropertyDrawer showcases loading a UXML template inside a UxmlSerializedDataPropertyDrawer.
// The template uses UxmlAttributeField (pattern 1) and UxmlAttributeFieldDecorator (pattern 2)
// with binding-path. Because this is a UxmlSerializedDataPropertyDrawer, the binding context
// set by UxmlSerializedDataPropertyView makes those relative binding-path values resolve correctly.
//
// The ammo field is rendered by calling CreateChildPropertyGUI, which creates a UxmlAttributeField.
// UxmlAttributeField internally uses a PropertyField, which invokes AmmoPropertyDrawer and
// preserves the override indicator bar alongside the ammo count/max row and ProgressBar.
using Unity.UIToolkit.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Gun.UxmlSerializedData))]
public class GunPropertyDrawer : UxmlSerializedDataPropertyDrawer
{
    protected override void CreateChildPropertiesGUI(VisualElement container, SerializedProperty property)
    {
        container.Add(ItemTypeLabel("Gun"));

        // Pattern 1 & 2: load a UXML template that uses UxmlAttributeField and
        // UxmlAttributeFieldDecorator with binding-path for name, weight, damage, and fireRate.
        var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/ui-toolkit-manual-code-examples/inventory-property-drawers/UI/GunDrawer.uxml");
        if (template != null)
            container.Add(template.Instantiate());

        // Render the ammo property via CreateChildPropertyGUI. The base implementation creates a
        // UxmlAttributeField, which wraps a PropertyField that delegates to AmmoPropertyDrawer.
        var ammoProperty = property.FindPropertyRelative("ammo");
        if (ammoProperty != null)
            CreateChildPropertyGUI(container, property, ammoProperty);
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
