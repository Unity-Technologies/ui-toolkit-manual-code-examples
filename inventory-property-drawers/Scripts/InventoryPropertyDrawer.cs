// This drawer showcases four ways to create inspector fields for UxmlSerializedData properties:
//
//   1. UxmlAttributeField in UXML   – binding-path resolves relative to the UxmlSerializedData
//                                      property because UxmlSerializedDataPropertyView sets up
//                                      the binding context.
//   2. UxmlAttributeFieldDecorator in UXML – wraps an explicit field type in UXML while keeping
//                                      the override indicator bar and context menu.
//   3. UxmlAttributeField in C#     – creates a field programmatically from a SerializedProperty.
//   4. UxmlAttributeFieldDecorator in C# – wraps any IBindable element in code.
using Unity.UIToolkit.Editor;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(Inventory.UxmlSerializedData))]
public class InventoryPropertyDrawer : UxmlSerializedDataPropertyDrawer
{
    // Cached to avoid a disk lookup on every drawer instantiation.
    // Note: the path below must match the location of the UI folder in your project.
    // If you move or rename the inventory-property-drawers folder, update this path accordingly.
    static VisualTreeAsset s_Template;

    protected override void CreateChildPropertiesGUI(VisualElement container, SerializedProperty property)
    {
        // Pattern 1 & 2: load a UXML template that uses UxmlAttributeField and
        // UxmlAttributeFieldDecorator with binding-path to render maxSlots and maxWeight.
        // The binding paths resolve relative to this UxmlSerializedData property automatically.
        if (s_Template == null)
            s_Template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/ui-toolkit-manual-code-examples/inventory-property-drawers/UI/InventoryDrawer.uxml");
        if (s_Template != null)
            container.Add(s_Template.Instantiate());

        // Pattern 3: create a UxmlAttributeField in C# for the description property.
        container.Add(new UxmlAttributeField(property.FindPropertyRelative("description")));

        // Pattern 4: create a UxmlAttributeFieldDecorator in C# to wrap the items ListView.
        SerializedProperty itemsProperty = property.FindPropertyRelative("items");
        ListView items = new ListView
        {
            showAddRemoveFooter = true,
            showBorder = true,
            showFoldoutHeader = false,
            reorderable = true,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            reorderMode = ListViewReorderMode.Animated,
            bindingPath = itemsProperty.propertyPath,
            overridingAddButtonBehavior = (baseListView, button) => OnAddItem(property, baseListView, button)
        };

        UxmlAttributeFieldDecorator listViewDecorator = new UxmlAttributeFieldDecorator();
        listViewDecorator.Add(items);
        container.Add(listViewDecorator);

        container.Add(new Button(() =>
        {
            AddGun(property, "Rifle", 4.5f, 33, 2.5f, 30, 30);
            AddSword(property, "Knife", 0.5f, 7);
            AddHealthPack(property);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Sniper Gear" });

        container.Add(new Button(() =>
        {
            AddGun(property, "Rifle", 4.5f, 33, 2.5f, 30, 30);
            AddHealthPack(property);
            AddSword(property, "Machete", 1, 11);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Warrior Gear" });

        container.Add(new Button(() =>
        {
            AddGun(property, "Pistol", 1.5f, 10, 1f, 15, 15);
            AddHealthPack(property);
            AddHealthPack(property);
            AddHealthPack(property);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Medic Gear" });
    }

    // Appends a new item of the given type to the items array and assigns its ID.
    // Returns the SerializedProperty for the new element so callers can set type-specific fields.
    SerializedProperty AppendItem(SerializedProperty property, System.Type itemType)
    {
        var itemsProperty = property.FindPropertyRelative("items");
        itemsProperty.arraySize++;
        var newItem = itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(itemType);
        newItem.FindPropertyRelative("id").intValue = NextItemId(property);
        return newItem;
    }

    void AddGun(SerializedProperty property, string name, float weight, float damage, float fireRate, int ammo, int maxAmmo)
    {
        var newItem = AppendItem(property, typeof(Gun));
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("damage").floatValue = damage;
        newItem.FindPropertyRelative("fireRate").floatValue = fireRate;
        var ammoInstance = newItem.FindPropertyRelative("ammo");
        ammoInstance.FindPropertyRelative("count").intValue = ammo;
        ammoInstance.FindPropertyRelative("maxCount").intValue = maxAmmo;
    }

    void AddSword(SerializedProperty property, string name, float weight, float damage)
    {
        var newItem = AppendItem(property, typeof(Sword));
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("slashDamage").floatValue = damage;
    }

    void AddHealthPack(SerializedProperty property) => AppendItem(property, typeof(HealthPack));

    int NextItemId(SerializedProperty property) => property.FindPropertyRelative("nextItemId").intValue++;

    void OnAddItem(SerializedProperty property, BaseListView baseListView, Button button)
    {
        var menu = new GenericMenu();
        var items = TypeCache.GetTypesDerivedFrom<Item>();
        foreach (var item in items)
        {
            if (item.IsAbstract)
                continue;

            menu.AddItem(new GUIContent(item.Name), false, () =>
            {
                AppendItem(property, item);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.DropDown(button.worldBound);
    }
}
