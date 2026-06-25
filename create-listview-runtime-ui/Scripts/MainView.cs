using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class MainView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_ListEntryTemplate;

    void OnEnable()
    {
        // The UXML is already instantiated by the PanelRenderer component.
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }
    void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
    }
    void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        // Initialize the character list controller.
        var characterListController = new CharacterListController();
        characterListController.InitializeCharacterList(rootElement, m_ListEntryTemplate);
    }
}
