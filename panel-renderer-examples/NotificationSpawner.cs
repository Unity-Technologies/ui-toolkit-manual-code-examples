using UnityEngine;
using UnityEngine.UIElements;

// Elements that a script adds aren't part of the Panel Renderer's Source Asset. When the
// Panel Renderer rebuilds its UI, it releases the previous visual tree and every element in it.
// To keep the notifications, this script maintains its own list and re-adds them after each rebuild.
public class NotificationSpawner : MonoBehaviour
{
    // The element that displays the notifications. It can be the root of a Panel Renderer or any element in it.
    [SerializeField] VisualElementReference m_Parent;
    [SerializeField] VisualTreeAsset m_NotificationTemplate;
    [SerializeField] long m_DurationMs = 3000;

    // The script owns this container and the notifications in it.
    VisualElement m_Container;

    void Awake()
    {
        m_Container = new VisualElement();
        m_Parent.RegisterReferenceResolvedCallback(OnParentResolved);
        m_Parent.RegisterReferenceUnloadedCallback(OnParentUnloaded);
    }

    void OnDestroy()
    {
        m_Parent.UnregisterReferenceResolvedCallback(OnParentResolved);
        m_Parent.UnregisterReferenceUnloadedCallback(OnParentUnloaded);
    }

    void OnParentResolved(VisualElement parent)
    {
        parent.Add(m_Container);
    }

    void OnParentUnloaded(VisualElement parent)
    {
        // The Panel Renderer unloads the reference before it releases its visual tree.
        // Detach the container so that it isn't released with the tree, and can be added to the new parent.
        m_Container.RemoveFromHierarchy();
    }

    // Adds the notification to the list. If the UI is ready, it also creates the element immediately.
    // If the UI isn't ready, the callback creates the element when the UI becomes available.
    public void ShowNotification(string message)
    {
        // The container exists even if the UI isn't ready, so notifications are never discarded.
        var notification = m_NotificationTemplate.Instantiate();
        notification.Q<Label>("message").text = message;
        m_Container.Add(notification);

        // Remove the notification after a delay.
        notification.schedule.Execute(notification.RemoveFromHierarchy).ExecuteLater(m_DurationMs);
    }
}
