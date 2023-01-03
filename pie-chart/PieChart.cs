using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PieChart : VisualElement
{
    float m_Radius = 100.0f;
    float m_Value = 40.0f;

    VisualElement m_Chart;

    public float radius
    {
        get => m_Radius;
        set
        {
            m_Radius = value;
            m_Chart.style.height = diameter;
            m_Chart.style.width = diameter;
            m_Chart.MarkDirtyRepaint();
        }
    }

    public float diameter => m_Radius * 2.0f;

    public float value {
        get { return m_Value; }
        set { m_Value = value; MarkDirtyRepaint(); }
    }

    public PieChart()
    {
        generateVisualContent += DrawCanvas;
    }

    void DrawCanvas(MeshGenerationContext ctx)
    {
        var painter = ctx.painter2D;
        painter.strokeColor = Color.white;
        painter.fillColor = Color.white;

        var percentage = m_Value;

        var percentages = new float[] {
            percentage, 100 - percentage
        };
        var colors = new Color32[] {
            new Color32(182,235,122,255),
            new Color32(251,120,19,255)
        };
        float angle = 0.0f;
        float anglePct = 0.0f;
        int k = 0;
        foreach (var pct in percentages)
        {
            anglePct += 360.0f * (pct / 100);

            painter.fillColor = colors[k++];
            painter.BeginPath();
            painter.MoveTo(new Vector2(m_Radius, m_Radius));
            painter.Arc(new Vector2(m_Radius, m_Radius), m_Radius, angle, anglePct);
            painter.Fill();

            angle = anglePct;
        }
    }
}