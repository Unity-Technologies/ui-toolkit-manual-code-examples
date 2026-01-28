// When you add a UxmlObject to the inventory list, include an instance of UxmlSerializedData, not an Item. 
// To simplify this process, this example uses `UxmlSerializedDataCreator.CreateUxmlSerializedData`, 
// a utility method that creates a UxmlObject’s UxmlSerializedData with default values.
//
// In this approach, the assignment of an ID value is introduced. To manage this, the last used ID value is stored 
// within the element as a hidden field labeled `nextItemId`. Additionally, buttons are incorporated to add preconfigured 
// sets of items. For instance, a Soldier might receive a Rifle, Machete, and Performance Pack.
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(Inventory.UxmlSerializedData))]
public class InventoryPropertyDrawer : PropertyDrawer
{
    SerializedProperty m_InventoryProperty;
    SerializedProperty m_ItemsProperty;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        m_InventoryProperty = property;

        var root = new VisualElement();

        m_ItemsProperty = property.FindPropertyRelative("items");
        var items = new ListView
        {
            showAddRemoveFooter = true,
            showBorder = true,
            showFoldoutHeader = false,
            reorderable = true,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            reorderMode = ListViewReorderMode.Animated,
            bindingPath = m_ItemsProperty.propertyPath,
            overridingAddButtonBehavior = OnAddItem
        };
        root.Add(items);

        var addSniperGear = new Button(() =>
        {
            AddGun("Rifle", 4.5f, 33, 30, 30);
            AddSword("Knife", 0.5f, 7);
            AddHealthPack();
            m_InventoryProperty.serializedObject.ApplyModifiedProperties();
        });
        addSniperGear.text = "Add Sniper Gear";

        var addWarriorGear = new Button(() =>
        {
            AddGun("Rifle", 4.5f, 33, 30, 30);
            AddHealthPack();
            AddSword("Machete", 1, 11);
            m_InventoryProperty.serializedObject.ApplyModifiedProperties();
        });
        addWarriorGear.text = "Add Warrior Gear";

        var addMedicGear = new Button(() =>
        {
            AddGun("Pistol", 1.5f, 10, 15, 15);
            AddHealthPack();
            AddHealthPack();
            AddHealthPack();
            m_InventoryProperty.serializedObject.ApplyModifiedProperties();
        });
        addMedicGear.text = "Add Medic Gear";

        root.Add(addSniperGear);
        root.Add(addWarriorGear);
        root.Add(addMedicGear);
        root.Bind(property.serializedObject);
        return root;
    }

    void AddGun(string name, float weight, float damage, int ammo, int maxAmmo)
    {
        m_ItemsProperty.arraySize++;
        var newItem = m_ItemsProperty.GetArrayElementAtIndex(m_ItemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(Gun));
        newItem.FindPropertyRelative("id").intValue = NextItemId();
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("damage").floatValue = damage;
        var ammoInstance = newItem.FindPropertyRelative("ammo");
        ammoInstance.FindPropertyRelative("count").intValue = ammo;
        ammoInstance.FindPropertyRelative("maxCount").intValue = maxAmmo;
    }

    void AddSword(string name, float weight, float damage)
    {
        m_ItemsProperty.arraySize++;
        var newItem = m_ItemsProperty.GetArrayElementAtIndex(m_ItemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(Sword));
        newItem.FindPropertyRelative("id").intValue = NextItemId();
        newItem.FindPropertyRelative("name").stringValue = name;
        newItem.FindPropertyRelative("weight").floatValue = weight;
        newItem.FindPropertyRelative("slashDamage").floatValue = damage;
    }

    void AddHealthPack()
    {
        m_ItemsProperty.arraySize++;
        var newItem = m_ItemsProperty.GetArrayElementAtIndex(m_ItemsProperty.arraySize - 1);
        newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(typeof(HealthPack));
        newItem.FindPropertyRelative("id").intValue = NextItemId();
    }

    int NextItemId() => m_InventoryProperty.FindPropertyRelative("nextItemId").intValue++;

    void OnAddItem(BaseListView baseListView, Button button)
    {
        var menu = new GenericMenu();
        var items = TypeCache.GetTypesDerivedFrom<Item>();
        foreach (var item in items)
        {
            if (item.IsAbstract)
                continue;

            menu.AddItem(new GUIContent(item.Name), false, () =>
            {
                m_ItemsProperty.arraySize++;
                var newItem = m_ItemsProperty.GetArrayElementAtIndex(m_ItemsProperty.arraySize - 1);
                newItem.managedReferenceValue = UxmlSerializedDataCreator.CreateUxmlSerializedData(item);
                newItem.FindPropertyRelative("id").intValue = NextItemId();
                m_InventoryProperty.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.DropDown(button.worldBound);
    }
}