using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Button button;
    private Toggle toggle;
    private VisualElement root;

    private int clickCount;

    private void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
        if (button != null)
        {
            button.UnregisterCallback<ClickEvent>(PrintClickMessage);
        }
    }

    private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement)
    {
        root = rootElement;

        button = root.Q("button") as Button;
        toggle = root.Q("toggle") as Toggle;

        button.RegisterCallback<ClickEvent>(PrintClickMessage);

        var inputFields = root.Q("input-message");
        inputFields.RegisterCallback<ChangeEvent<string>>(InputMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        ++clickCount;

        Debug.Log($"{"button"} was clicked!" +
                (toggle.value ? " Count: " + clickCount : ""));
    }

    public static void InputMessage(ChangeEvent<string> evt)
    {
        Debug.Log($"{evt.newValue} -> {evt.target}");
    }
}

