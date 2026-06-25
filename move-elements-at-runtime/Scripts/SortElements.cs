using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SortElements : MonoBehaviour
{
    [SerializeField]
    PanelRenderer m_MovingElements;

    VisualElement m_BaseContainer;

    MovingNameTag[] m_MovingNameTags;

    void Start()
    {
        m_MovingNameTags = FindObjectsByType<MovingNameTag>();
        m_MovingElements.RegisterUIReloadCallback((panelRenderer, root, version) =>
        {
            m_BaseContainer = root.Q<VisualElement>("BaseContainer");
        });
    }

    void Update()
    {
        m_BaseContainer.Sort(CompareOrder);
    }

    static int CompareOrder(VisualElement x, VisualElement y)
    {
        // Compare the scale of the visual elements in the base container, which is
        // determined by the distance of the object it follows in the MovingNameTag component
        return x.style.scale.value.value.x.CompareTo(y.style.scale.value.value.x);
    }
}
