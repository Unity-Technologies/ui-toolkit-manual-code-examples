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
    // Cached to avoid a disk lookup on every drawer instantiation.
    // Note: the path below must match the location of the UI folder in your project.
    // If you move or rename the inventory-property-drawers folder, update this path accordingly.
    static VisualTreeAsset s_Template;

    protected override void CreateChildPropertiesGUI(VisualElement container, SerializedProperty property)
    {
        container.Add(ItemTypeLabel("Gun"));

        // Pattern 1 & 2: load a UXML template that uses UxmlAttributeField and
        // UxmlAttributeFieldDecorator with binding-path for name, weight, damage, and fireRate.
        if (s_Template == null)
            s_Template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/ui-toolkit-manual-code-examples/inventory-property-drawers/UI/GunDrawer.uxml");
        if (s_Template != null)
            container.Add(s_Template.Instantiate());

        // Render the ammo property via CreateChildPropertyGUI. The base implementation creates a
        // UxmlAttributeField, which wraps a PropertyField that delegates to AmmoPropertyDrawer.
        SerializedProperty ammoProperty = property.FindPropertyRelative("ammo");
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
