using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    public class SimpleBindingExampleUXML : EditorWindow
    {
        [SerializeField]
        VisualTreeAsset visualTree;

        [MenuItem("Window/UIToolkitExamples/Simple Binding Example UXML")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<SimpleBindingExampleUXML>();
            wnd.titleContent = new GUIContent("Simple Binding UXML");
        }

        public void CreateGUI()
        {
            visualTree.CloneTree(rootVisualElement);
            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            GameObject selectedObject = Selection.activeObject as GameObject;
            if (selectedObject != null)
            {
                // Create the SerializedObject from the current selection
                SerializedObject so = new SerializedObject(selectedObject);
                // Bind it to the root of the hierarchy. It will find the right object to bind to...
                rootVisualElement.Bind(so);
            }
            else
            {
                // Unbind the object from the actual visual element
                rootVisualElement.Unbind();

                // Clear the TextField after the binding is removed
                var textField = rootVisualElement.Q<TextField>("GameObjectName");
                if (textField != null)
                {
                    textField.value = string.Empty;
                }
            }
        }
    }
}