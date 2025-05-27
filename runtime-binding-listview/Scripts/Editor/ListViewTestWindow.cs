using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using Unity.Properties;

internal class ListViewTestWindow : EditorWindow
{
    // This example sets the itemTemplate and bindingSourceSelectionMode in the UXML file.
    // If you want to set them in the C# script, declare an itemLayout field of type VisualTreeAsset
    // and then set the "Item Layout" to "ListViewItem.uxml" in the Inspector window.
    //[SerializeField] private VisualTreeAsset itemLayout;
    [SerializeField] private VisualTreeAsset editorLayout;

    ExampleItemObject m_ExampleItemObject;
    
    [MenuItem("Window/ListViewTestWindow")]
    static void Init()
    {
        ListViewTestWindow window = EditorWindow.GetWindow<ListViewTestWindow>();
        window.Show();
    }

    void CreateGUI()
    {
        m_ExampleItemObject = new();

        editorLayout.CloneTree(rootVisualElement);
        var listView = rootVisualElement.Q<ListView>();
        
        // This example sets the itemTemplate and bindingSourceSelectionMode in the UXML file.
        // You can also set them in the C# script like the following:
        // listView.itemTemplate = itemLayout;
        // listView.bindingSourceSelectionMode = BindingSourceSelectionMode.AutoAssign;

        // Set the binding source to the ExampleItemObject instance.
        listView.dataSource = m_ExampleItemObject;

        // Set the itemsSource binding to the items property of the List object.
        // You can also set the itemsSource binding manually in the UXML file and comment out this line. 
        // Refer to the next section for how to set binding in UXML.
        listView.SetBinding("itemsSource", new DataBinding() {dataSourcePath = new PropertyPath("items")});
        
        m_ExampleItemObject.Reset();
    }
}

