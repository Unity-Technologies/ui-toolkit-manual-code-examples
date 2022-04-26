using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    public class SimpleBindingPropertyTrackingExample : EditorWindow
    {
        TextField m_ObjectNameBinding;

        [MenuItem("Window/UIToolkitExamples/Simple Binding Property Tracking Example")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<SimpleBindingPropertyTrackingExample>();
            wnd.titleContent = new GUIContent("Simple Binding Property Tracking");
        }

        public void CreateGUI()
        {
            m_ObjectNameBinding = new TextField("Object Name Binding");
            rootVisualElement.Add(m_ObjectNameBinding);
            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            GameObject selectedObject = Selection.activeObject as GameObject;
            if (selectedObject != null)
            {
                // Create the SerializedObject from the current selection
                SerializedObject so = new SerializedObject(selectedObject);

                // Note: the "name" property of a GameObject is actually named "m_Name" in serialization.
                SerializedProperty property = so.FindProperty("m_Name");

                // Ensure to use Unbind() before tracking a new property
                m_ObjectNameBinding.Unbind();
                m_ObjectNameBinding.TrackPropertyValue(property, CheckName);

                // Bind the property to the field directly
                m_ObjectNameBinding.BindProperty(property);

                CheckName(property);
            }
            else
            {
                // Unbind any binding from the field
                m_ObjectNameBinding.Unbind();
            }
        }

        void CheckName(SerializedProperty property)
        {
            if (property.stringValue == "GameObject")
            {
                m_ObjectNameBinding.style.backgroundColor = Color.red * 0.5f;
            }
            else
            {
                m_ObjectNameBinding.style.backgroundColor = StyleKeyword.Null;
            }
        }
    }
}