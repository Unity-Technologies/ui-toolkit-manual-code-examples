using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(GameSwitchListAsset))]
    public class GameSwitchListEditor : Editor
    {
        [SerializeField]
        VisualTreeAsset m_ItemAsset;

        [SerializeField]
        VisualTreeAsset m_EditorAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = m_EditorAsset.CloneTree();
            var listView = root.Q<ListView>();
            listView.makeItem = m_ItemAsset.CloneTree;
            return root;
        }
    }
}