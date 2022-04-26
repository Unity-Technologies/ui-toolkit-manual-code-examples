using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(TextureAsset))]
    public class TextureAssetEditor : Editor
    {
        [SerializeField]
        VisualTreeAsset m_VisualTree;

        public override VisualElement CreateInspectorGUI()
        {
            return m_VisualTree.CloneTree();
        }
    }
}