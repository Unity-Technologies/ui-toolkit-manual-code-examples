using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class ListViewBindingExample : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [SerializeField]
    private GameSwitchListAsset gameSwitchList;

    [MenuItem("UI Toolkit Examples/ListView SerializedObject Binding Example")]
    public static void ShowExample()
    {
        ListViewBindingExample wnd = GetWindow<ListViewBindingExample>();
        wnd.titleContent = new GUIContent("ListView Binding SerializedObject Example");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        m_VisualTreeAsset.CloneTree(root);

        var listView = root.Q<ListView>();
        if (listView != null && gameSwitchList != null)
        {
            // Set the items source.
            listView.itemsSource = gameSwitchList.switches;
            // Bind the ListView to the GameSwitchListAsset serialized object.
            listView.Bind(new SerializedObject(gameSwitchList));
        }
    }
}
