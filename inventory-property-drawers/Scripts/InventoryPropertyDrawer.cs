// When you add a UxmlObject to the inventory list, include an instance of UxmlSerializedData, not an Item.
// To simplify this process, this example uses `UxmlSerializedDataCreator.CreateUxmlSerializedData`,
// a utility method that creates a UxmlObject's UxmlSerializedData with default values.
//
// In this approach, the assignment of an ID value is introduced. To manage this, the last used ID value is stored
// within the element as a hidden field labeled `nextItemId`. Additionally, buttons are incorporated to add preconfigured
// sets of items. For instance, a Soldier might receive a Rifle, Machete, and Performance Pack.
using Unity.UIToolkit.Editor;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(Inventory.UxmlSerializedData))]
public class InventoryPropertyDrawer : UxmlSerializedDataPropertyDrawer
{
    protected override void CreateChildPropertiesGUI(VisualElement container, SerializedProperty property)
    {
        var itemsProperty = property.FindPropertyRelative("items");
        var items = new ListView
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
        container.Add(items);

        container.Add(new Button(() =>
        {
            AddGun(property, "Rifle", 4.5f, 33, 30, 30);
            AddSword(property, "Knife", 0.5f, 7);
            AddHealthPack(property);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Sniper Gear" });

        container.Add(new Button(() =>
        {
            AddGun(property, "Rifle", 4.5f, 33, 30, 30);
            AddHealthPack(property);
            AddSword(property, "Machete", 1, 11);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Warrior Gear" });

        container.Add(new Button(() =>
        {
            AddGun(property, "Pistol", 1.5f, 10, 15, 15);
            AddHealthPack(property);
            AddHealthPack(property);
            AddHealthPack(property);
            property.serializedObject.ApplyModifiedProperties();
        }) { text = "Add Medic Gear" });
    }

    void AddGun(SerializedProperty property, string name, float weight, float damage, int ammo, int maxAmmo)
    {
        var itemsProperty = property.FindPropertyRelative("items");
        itemsProperty.arraySize++;
        var newItem = itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(Gun));
        newItem.FindPropertyRelative("id").intValue = NextItemId(property);
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("damage").floatValue = damage;
        var ammoInstance = newItem.FindPropertyRelative("ammo");
        ammoInstance.FindPropertyRelative("count").intValue = ammo;
        ammoInstance.FindPropertyRelative("maxCount").intValue = maxAmmo;
    }

    void AddSword(SerializedProperty property, string name, float weight, float damage)
    {
        var itemsProperty = property.FindPropertyRelative("items");
        itemsProperty.arraySize++;
        var newItem = itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(Sword));
        newItem.FindPropertyRelative("id").intValue = NextItemId(property);
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("slashDamage").floatValue = damage;
    }

    void AddHealthPack(SerializedProperty property)
    {
        var itemsProperty = property.FindPropertyRelative("items");
        itemsProperty.arraySize++;
        var newItem = itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(HealthPack));
        newItem.FindPropertyRelative("id").intValue = NextItemId(property);
    }

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
                var itemsProperty = property.FindPropertyRelative("items");
                itemsProperty.arraySize++;
                var newItem = itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize - 1);
                newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(item);
                newItem.FindPropertyRelative("id").intValue = NextItemId(property);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.DropDown(button.worldBound);
    }
}