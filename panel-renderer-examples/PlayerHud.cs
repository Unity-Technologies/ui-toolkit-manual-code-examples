using UnityEngine;
using UnityEngine.UIElements;

// Stores health and score in fields. SetHealth and AddScore update the fields first, then
// update the elements if they exist. If called before the UI is ready, the reload callback
// applies the cached values when the UI becomes available.
[RequireComponent(typeof(PanelRenderer))]
public class PlayerHud : MonoBehaviour
{
    PanelRenderer m_PanelRenderer;
    ProgressBar m_HealthBar;
    Label m_ScoreLabel;
    int m_UIVersion;

    // The script owns the HUD state. The UI only displays it.
    float m_Health = 100f;
    int m_Score;

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
        ApplyHealth();
    }

    public void AddScore(int points)
    {
        m_Score += points;
        ApplyScore();
    }

    void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        if (version == m_UIVersion)
            return;
        m_UIVersion = version;

        m_HealthBar = root.Q<ProgressBar>("health");
        m_ScoreLabel = root.Q<Label>("score");

        // The new elements have the default values from the UXML document.
        // Apply the current state so that the HUD doesn't reset when the UI reloads.
        ApplyHealth();
        ApplyScore();
    }

    void ApplyHealth()
    {
        if (m_HealthBar != null)
            m_HealthBar.value = m_Health;
    }

    void ApplyScore()
    {
        if (m_ScoreLabel != null)
            m_ScoreLabel.text = $"Score: {m_Score}";
    }
}
