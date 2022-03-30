using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TankScript))]
public class TankEditor : Editor
{
    [SerializeField]
    VisualTreeAsset visualTree;

    [SerializeField]
    StyleSheet styleSheet;

    public override VisualElement CreateInspectorGUI()
    {
        var uxmlVE = visualTree.CloneTree();
        uxmlVE.styleSheets.Add(styleSheet);
        return uxmlVE;
    }
}