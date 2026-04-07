using UnityEngine;
using UnityEngine.UIElements;

public class PseudoStateChecker : MonoBehaviour
{
    private Button myButton;
    private Toggle myToggle;

    void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
    }

    void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
    {
        // Query for the elements. 
        myButton = rootElement.Q<Button>("my-button");
        myToggle = rootElement.Q<Toggle>("my-toggle");

        if (myButton == null || myToggle == null)
        {
            Debug.LogError("Button or Toggle not found in the UI Document!");
            return;
        }

        // Register callbacks to check states on a Button. 
        myButton.RegisterCallback<PointerEnterEvent>(evt =>
            Debug.Log($"Button PointerEnterEvent: hasHoverPseudoState = {myButton.hasHoverPseudoState}"));

        myButton.RegisterCallback<PointerLeaveEvent>(evt =>
            Debug.Log($"Button PointerLeaveEvent: hasHoverPseudoState = {myButton.hasHoverPseudoState}"));

        // Use TrickleDown to ensure the callback receives the event before it is stopped by the Button.
        myButton.RegisterCallback<PointerDownEvent>(
            evt => Debug.Log($"Button PointerDownEvent: hasActivePseudoState = {myButton.hasActivePseudoState}"),
            TrickleDown.TrickleDown);

        myButton.RegisterCallback<PointerUpEvent>(evt =>
            Debug.Log($"Button PointerUpEvent: hasActivePseudoState = {myButton.hasActivePseudoState}"));

        myButton.RegisterCallback<FocusInEvent>(evt =>
            Debug.Log($"Button FocusInEvent: hasFocusPseudoState = {myButton.hasFocusPseudoState}"));

        myButton.RegisterCallback<FocusOutEvent>(evt =>
            Debug.Log($"Button FocusOutEvent: hasFocusPseudoState = {myButton.hasFocusPseudoState}"));

        // Register a callback to check the state on a Toggle. 
        myToggle.RegisterValueChangedCallback(evt =>
            Debug.Log($"Toggle ValueChangedEvent: hasCheckedPseudoState = {myToggle.hasCheckedPseudoState}"));

        // Example of checking the disabled state.
        // You can un-comment this to check the disabled state after 3 seconds.
        // Invoke(nameof(DisableTheButton), 3f);
    }

    private void DisableTheButton()
    {
        if (myButton != null)
        {
            myButton.SetEnabled(false);
            Debug.Log($"Button has been disabled: hasDisabledPseudoState = {myButton.hasDisabledPseudoState}");
        }
    }
}
