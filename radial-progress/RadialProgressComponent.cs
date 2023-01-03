using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MyUILibrary;

[RequireComponent(typeof(UIDocument))]
public class RadialProgressComponent : MonoBehaviour
{

    RadialProgress m_RadialProgress;

    void start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        // Find the custom control by name.
        m_RadialProgress = root.Q("radial-progress") as RadialProgress;
    }

    void Update()
    {
        // For demo purpose, give the progress property dynamic values.
        m_RadialProgress.progress = ((Mathf.Sin(Time.time) + 1.0f) / 2.0f) * 60.0f + 10.0f;
    }
}