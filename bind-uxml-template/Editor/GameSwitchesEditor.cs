using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(GameSwitchesAsset))]
    public class GameSwitchesEditor : Editor
    {
        [SerializeField]
        VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            return visualTreeAsset.CloneTree();
        }
    }
}