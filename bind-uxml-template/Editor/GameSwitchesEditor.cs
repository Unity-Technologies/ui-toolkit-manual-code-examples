using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class UXMLTemplateBindingExample : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [SerializeField]
    private GameSwitchesAsset gameSwitch;

    [MenuItem("UI Toolkit Examples/UXML Template Binding Example")]
    public static void ShowExample()
    {
        UXMLTemplateBindingExample wnd = GetWindow<UXMLTemplateBindingExample>();
        wnd.titleContent = new GUIContent("UXML Template Binding Example");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        m_VisualTreeAsset.CloneTree(root);

        root.Bind(new SerializedObject(gameSwitch));
       
    }
}