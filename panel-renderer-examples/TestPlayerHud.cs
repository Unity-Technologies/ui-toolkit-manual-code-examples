using UnityEngine;

// Drag the Health slider in the Inspector while in Play mode to update the progress bar.
public class TestPlayerHud : MonoBehaviour
{
    PlayerHud m_PlayerHud;

    [SerializeField, Range(0f, 100f)]
    float m_Health = 100f;

    [SerializeField]
    int m_Score;

    float m_AppliedHealth;
    int m_AppliedScore;

    void Awake()
    {
        m_PlayerHud = GetComponent<PlayerHud>();
        m_AppliedHealth = m_Health;
        m_AppliedScore = m_Score;
    }

    void Update()
    {
        if (m_Health != m_AppliedHealth)
        {
            m_AppliedHealth = m_Health;
            m_PlayerHud.SetHealth(m_Health);
        }

        if (m_Score != m_AppliedScore)
        {
            m_PlayerHud.AddScore(m_Score - m_AppliedScore);
            m_AppliedScore = m_Score;
        }
    }
}
