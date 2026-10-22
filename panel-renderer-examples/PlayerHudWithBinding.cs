using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class PlayerHudWithBinding : MonoBehaviour, IDataSourceViewHashProvider
{
    PanelRenderer m_PanelRenderer;

    // Increases every time the HUD state changes, so that the bindings update only when needed.
    long m_ViewVersion;

    float m_Health = 100f;
    int m_Score;

    // [CreateProperty] exposes these properties to the binding system.
    [CreateProperty]
    public float health => m_Health;

    [CreateProperty]
    public string scoreText => $"Score: {m_Score}";

    void Awake()
    {
        m_PanelRenderer = GetComponent<PanelRenderer>();
        m_PanelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    void OnDestroy()
    {
        m_PanelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    public void SetHealth(float health)
    {
        m_Health = Mathf.Clamp(health, 0f, 100f);
        ++m_ViewVersion;
    }

    public void AddScore(int points)
    {
        m_Score += points;
        ++m_ViewVersion;
    }

    public long GetViewHashCode() => m_ViewVersion;

    void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        // The bindings in the UXML document resolve their data source path against this script.
        // When the UI is rebuilt, the new elements bind to the same source and display the current state.
        // No version check needed: setting the same data source again has no effect.
        root.dataSource = this;
    }
}
