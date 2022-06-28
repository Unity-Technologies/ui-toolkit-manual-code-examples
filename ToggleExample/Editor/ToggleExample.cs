using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace Samples.Editor.Controls
{
    public class ToggleExample : EditorWindow
    {
        private Toggle showToggle;
        private Toggle activateToggle;
        private Label labelToShow;
        private Button buttonToActivate;
        [MenuItem("Window/ToggleExample")]
        public static void OpenWindow()
        {
            var window = GetWindow<ToggleExample>("Controls: Toggle Sample");
            window.minSize = new Vector2(200, 170);
            EditorGUIUtility.PingObject(MonoScript.FromScriptableObject(window));
        }
        public void OnEnable()
        {
            showToggle = new Toggle("Show label")
            {
                value = true
            };
            activateToggle = new Toggle("Active button")
            {
                value = true
            };
            labelToShow = new Label("This label is shown when the above toggle is set to On");
            buttonToActivate = new Button(() => Debug.Log("Button pressed!"))
            {
                text = "Active if above toggle is On"
            };
            rootVisualElement.Add(showToggle);
            rootVisualElement.Add(labelToShow);
            rootVisualElement.Add(activateToggle);
            rootVisualElement.Add(buttonToActivate);
            showToggle.RegisterValueChangedCallback(evt => labelToShow.visible = evt.newValue);
            activateToggle.RegisterValueChangedCallback(evt => buttonToActivate.SetEnabled(evt.newValue));
        }
    }
}