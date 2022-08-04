using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class LoopingExample : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    private Label _yoyoLabel;
    private Label _a2bLabel;
    [MenuItem("Window/UI Toolkit/Transition Looping Example")]
    public static void ShowExample()
    {
        var wnd = GetWindow<LoopingExample>();
        wnd.titleContent = new GUIContent("TransitionStyle");
    }
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualElement asset = m_VisualTreeAsset.Instantiate();
        root.Add(asset);
        SetupYoyo(root);
        SetupA2B(root);
    }
    // This method powers the yo-yo loop.
    private void SetupYoyo(VisualElement root)
    {
        _yoyoLabel = root.Q<Label>(name: "yoyo-label");
        // When the animation ends, the callback toggles a class to set the scale to 1.3 
        // or back to 1.0 when it's removed.
        _yoyoLabel.RegisterCallback<TransitionEndEvent>(evt => _yoyoLabel.ToggleInClassList("enlarge-scale-yoyo"));
        // Schedule the first transition 100 milliseconds after the root.schedule.Execute method is called.
        root.schedule.Execute(() => _yoyoLabel.ToggleInClassList("enlarge-scale-yoyo")).StartingIn(100);
    }
    // This method powers the A-to-B cycle.
    private void SetupA2B(VisualElement root)
    {
        _a2bLabel = root.Q<Label>(name:"a2b-label");
        _a2bLabel.RegisterCallback<TransitionEndEvent>(evt =>
        {
            _a2bLabel.RemoveFromClassList("enlarge-scale-a2b");
            _a2bLabel.schedule.Execute(() => _a2bLabel.AddToClassList("enlarge-scale-a2b")).StartingIn(10);
        });
        _a2bLabel.schedule.Execute(() => _a2bLabel.AddToClassList("enlarge-scale-a2b")).StartingIn(100);
    }
}
