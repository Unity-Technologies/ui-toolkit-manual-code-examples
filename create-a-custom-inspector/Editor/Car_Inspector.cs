using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Car))]
public class Car_Inspector : Editor
{
    public VisualTreeAsset inspectorUXML;
    public override VisualElement CreateInspectorGUI()
    {
         // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();

        // Add a simple label.
        myInspector.Add(new Label("This is a custom Inspector"));

        // Load the UXML file and clone its tree into the inspector.
        if (inspectorUXML != null)
        {
            VisualElement uxmlContent = inspectorUXML.CloneTree();
            myInspector.Add(uxmlContent);
        }

        // Return the finished Inspector UI.
        return myInspector;
    }
}

