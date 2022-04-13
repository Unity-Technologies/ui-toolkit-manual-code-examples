using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class TransitionExample : EditorWindow
{
    [SerializeField]
    VisualTreeAsset m_VisualTreeAsset;

    [MenuItem("Window/UI Toolkit/TransitionExample")]
    public static void ShowExample()
    {
        TransitionExample wnd = GetWindow<TransitionExample>();
        wnd.titleContent = new GUIContent("TransitionExample");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        cSharpLabel = new Label("Hello World! From C#");
        root.Add(cSharpLabel);

        // Create transition on the new Label.
        cSharpLabel.style.transitionDuration = new List<TimeValue>{ new TimeValue(3) };

        // Record default rotate and scale values.
        defaultRotate = cSharpLabel.resolvedStyle.rotate;
        defaultScale = cSharpLabel.resolvedStyle.scale;

        // Set up event handlers to simulate use of :hover pseudoclass.
        cSharpLabel.RegisterCallback<PointerOverEvent>(OnPointerOver);
        cSharpLabel.RegisterCallback<PointerOutEvent>(OnPointerOut);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

    // The Label created with C#.
    VisualElement cSharpLabel;

    // The default rotate and scale values for the new Label.
    Rotate defaultRotate;
    Scale defaultScale;

    void OnPointerOver(PointerOverEvent evt)
    {
        SetHover(evt.currentTarget as VisualElement, true);
    }

    void OnPointerOut(PointerOutEvent evt)
    {
        SetHover(evt.currentTarget as VisualElement, false);
    }

    // When the user enters or exits the Label, set the rotate and scale.
    void SetHover(VisualElement label, bool hover)
    {
        label.style.rotate = hover ? new(Angle.Degrees(10)) : defaultRotate;
        label.style.scale = hover ? new Vector2(1.1f, 1) : defaultScale;
    }
    
    // Unregister all event callbacks.
    void OnDisable()
    {
        cSharpLabel.UnregisterCallback<PointerOverEvent>(OnPointerOver);
        cSharpLabel.UnregisterCallback<PointerOutEvent>(OnPointerOut);
    }
}
