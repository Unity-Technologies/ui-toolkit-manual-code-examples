using UnityEngine;

public class NotificationTester : MonoBehaviour
{
    [SerializeField]
    NotificationSpawner m_NotificationSpawner;

    int m_Count;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ++m_Count;
            m_NotificationSpawner.ShowNotification($"Notification {m_Count}");
        }
    }
}
