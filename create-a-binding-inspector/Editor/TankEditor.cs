using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TankScript))]
public class TankEditor : Editor
{
    [SerializeField]
    VisualTreeAsset visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        var uxmlVE = visualTree.CloneTree();
        return uxmlVE;
    }
}