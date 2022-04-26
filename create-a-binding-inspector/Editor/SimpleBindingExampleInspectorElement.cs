using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;

namespace UIToolkitExamples
{
    public class SimpleBindingExampleInspectorElement : EditorWindow
    {
        [MenuItem("Window/UIToolkitExamples/Simple Binding Example Inspector Element")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<SimpleBindingExampleInspectorElement>();
            wnd.titleContent = new GUIContent("Simple Binding with Inspector Element");
        }

        TankScript m_Tank;
        public void OnEnable()
        {
            m_Tank = FindObjectOfType<TankScript>();
            if (m_Tank == null)
                return;

            var inspector = new InspectorElement(m_Tank);
            rootVisualElement.Add(inspector);
        }
    }
}