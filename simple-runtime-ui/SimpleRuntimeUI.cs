using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;

    private int _clickCount;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        _button = uiDocument.rootVisualElement.Q<Button>();
        _toggle = uiDocument.rootVisualElement.Q<Toggle>();

        _button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        ++_clickCount;

        var button = evt.currentTarget as Button;

        Debug.Log($"{button.name} was clicked!" +
                  (_toggle.value ? " Count: " + _clickCount : ""));
    }
}