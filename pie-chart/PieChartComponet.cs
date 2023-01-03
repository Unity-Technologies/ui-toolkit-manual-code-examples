using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PieChartComponent : MonoBehaviour
{
    PieChart m_PieChart;

    void Start()
    {
        m_PieChart = new PieChart();
        GetComponent<UIDocument>().rootVisualElement.Add(m_PieChart);
    }
}