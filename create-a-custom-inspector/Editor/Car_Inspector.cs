using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Car))]
public class Car_Inspector : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
        // Load the reference UXML.
        m_InspectorXML= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/create-a-custom-inspector/Car_Inspector_UXML.uxml");

        // Instantiate the UXML.
        VisualElement myInspector = m_InspectorXML.Instantiate();

        // Get a reference to the default Inspector Foldout control.
        VisualElement InspectorFoldout = myInspector.Q("Default_Inspector");

        // Attach a default Inspector to the Foldout.
        InspectorElement.FillDefaultInspector(InspectorFoldout, serializedObject, this);

        // Return the finished Inspector UI.
        return myInspector;
    }
}

