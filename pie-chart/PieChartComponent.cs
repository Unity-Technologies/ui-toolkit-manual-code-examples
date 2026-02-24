using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class PieChartComponent : MonoBehaviour
{
    PieChart m_PieChart;

    void OnEnable()
    {
        m_PieChart = new PieChart();
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }
    void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
    }
    void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
    {
        rootElement.Add(m_PieChart);
    }
}