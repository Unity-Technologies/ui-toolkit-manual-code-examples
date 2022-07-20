using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TransitionExample : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    private Button clickMeButton;
    private VisualElement colorChanger;
    private Label eventLabel;
    private Label timeLabel;

    private DateTime lastEvent;
    private static readonly TimeSpan NearlyInstantaneousThreshold = TimeSpan.FromMilliseconds(10);

    private static readonly string ClickMeButtonClass = "click-me";
    private static readonly string ColorChangerClass = "color-changer";
    private static readonly string ColorChangerTransitionClass = "color-transition";
    private static readonly string EventLabelName = "eventLabel";
    private static readonly string TimeLabelName = "timeLabel";
    private static readonly string TimeBelowThresholdText = "Almost instantaneous.";

    [MenuItem("Window/UI Toolkit/TransitionExample")]
    public static void ShowExample()
    {
        TransitionExample wnd = GetWindow<TransitionExample>();
        wnd.titleContent = new GUIContent("TransitionExample");
        wnd.minSize = new Vector2(500f, 400f);
    }

    public void CreateGUI()
    {
        lastEvent = DateTime.Now;

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement uxmlAsset = m_VisualTreeAsset.Instantiate();
        root.Add(uxmlAsset);

        // Get the relevant elements by querying the root element
        clickMeButton = root.Q<Button>(className: ClickMeButtonClass);

        colorChanger = root.Q<VisualElement>(className: ColorChangerClass);

        eventLabel = root.Q<Label>(name: EventLabelName);

        timeLabel = root.Q<Label>(name: TimeLabelName);

        // Add callbacks for clicking on the button and monitoring the color changing element.
        clickMeButton.RegisterCallback<ClickEvent>(OnClickEvent);

        colorChanger.RegisterCallback<TransitionRunEvent>(OnTransitionRun);
        colorChanger.RegisterCallback<TransitionStartEvent>(OnTransitionStart);
        colorChanger.RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
        colorChanger.RegisterCallback<TransitionCancelEvent>(OnTransitionCancel);
    }

    private void OnDisable()
    {
        clickMeButton.UnregisterCallback<ClickEvent>(OnClickEvent);

        colorChanger.UnregisterCallback<TransitionRunEvent>(OnTransitionRun);
        colorChanger.UnregisterCallback<TransitionStartEvent>(OnTransitionStart);
        colorChanger.UnregisterCallback<TransitionEndEvent>(OnTransitionEnd);
        colorChanger.UnregisterCallback<TransitionCancelEvent>(OnTransitionCancel);
    }

    private void OnClickEvent(ClickEvent evt)
    {
        colorChanger.ToggleInClassList(ColorChangerTransitionClass);
    }

    private void OnTransitionRun(TransitionRunEvent evt)
    {
        DisplayLatestEvent("TransitionRunEvent", DateTime.Now);
    }

    private void OnTransitionStart(TransitionStartEvent evt)
    {
        DisplayLatestEvent("TransitionStartEvent", DateTime.Now);
    }

    private void OnTransitionEnd(TransitionEndEvent evt)
    {
        DisplayLatestEvent("TransitionEndEvent", DateTime.Now);
    }

    private void OnTransitionCancel(TransitionCancelEvent evt)
    {
        DisplayLatestEvent("TransitionCancelEvent", DateTime.Now);
    }

    private void DisplayLatestEvent(string eventType, DateTime timestamp)
    {
        // If two events are sent too close together, add both to the Latest event line.
        // This happens if the delay is set to 0 and the TransitionRun and TransitionStart
        // are sent at the same time, or if the button was pressed before the transition
        // was over, thus sending TransitionCancel and TransitionRun (and potentially
        // TransitionStart) events close together.
        var elapsed = timestamp - lastEvent;
        if (elapsed <= NearlyInstantaneousThreshold)
        {
            timeLabel.text = TimeBelowThresholdText;
            eventLabel.text = $"{eventLabel.text}, {eventType}";
        }
        else
        {
            timeLabel.text = $"{elapsed:s\\.ff} s";
            eventLabel.text = eventType;
        }

        lastEvent = timestamp;
    }
}