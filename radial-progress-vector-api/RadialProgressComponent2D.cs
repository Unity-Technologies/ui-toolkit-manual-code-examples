using Painter2DExample;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class RadialProgressComponent2D : MonoBehaviour
{
    RadialProgress m_RadialProgress;

    void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement, int version)
    {
        // Find the RadialProgress element defined in UXML
        m_RadialProgress = rootElement.Q<RadialProgress>("radial-progress");
    }

    void Update()
    {
        if (m_RadialProgress != null)
        {
            m_RadialProgress.progress = ((Mathf.Sin(Time.time) + 1.0f) / 2.0f) * 60.0f + 10.0f;
        }
    }
}

