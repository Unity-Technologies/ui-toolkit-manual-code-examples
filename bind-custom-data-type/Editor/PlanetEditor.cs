using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(PlanetScript))]
    public class PlanetEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return new PropertyField(serializedObject.FindProperty("coreTemperature"));
        }
    }
}