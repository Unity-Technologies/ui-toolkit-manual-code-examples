using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class HealthBarController : MonoBehaviour
{
    PanelRenderer m_PanelRenderer;
    ProgressBar m_HealthBar;
    int m_UIVersion;

    float m_Health = 100f;

    void Awake()
    {
        m_PanelRenderer = GetComponent<PanelRenderer>();
        m_PanelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    void OnDestroy()
    {
        m_PanelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        // Skip the setup if the visual tree hasn't been rebuilt since the last call.
        if (version == m_UIVersion)
            return;
        m_UIVersion = version;

        m_HealthBar = root.Q<ProgressBar>("health");
        root.Q<Button>("heal").clicked += () => m_Health = 100f;
    }

    void Update()
    {
        m_Health = Mathf.Max(0f, m_Health - 5f * Time.deltaTime);

        // The UI might not be ready yet, for example during the first Update.
        if (m_HealthBar == null)
            return;

        m_HealthBar.value = m_Health;
    }
}
