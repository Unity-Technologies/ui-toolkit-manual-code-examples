using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(ExampleFieldComponent))]
    public class ExampleFieldCustomEditor : Editor
    {
        [SerializeField]
        VisualTreeAsset m_Uxml;

        public override VisualElement CreateInspectorGUI()
        {
            var parent = new VisualElement();

            m_Uxml?.CloneTree(parent);

            return parent;
        }
    }
}