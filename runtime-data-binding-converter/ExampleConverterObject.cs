using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ExampleConverterObject : ScriptableObject
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void RegisterConverters()
    {
    
        // Create local Converters.
        var group = new ConverterGroup("Value To Progress");
    
        // Converter groups can have multiple converters. This example converts a float to both a color and a string.
        group.AddConverter((ref float v) => new StyleColor(Color.Lerp(Color.red, Color.green, v)));
        group.AddConverter((ref float value) =>
        {
            return value switch
            {
                >= 0 and < 1.0f/3.0f => "Danger",
                >= 1.0f/3.0f and < 2.0f/3.0f => "Neutral",
                _ => "Good"
            };
        });
    
        // Register the converter group in InitializeOnLoadMethod to make it accessible from the UI Builder.
        ConverterGroups.RegisterConverterGroup(group);
    }
    
    [Header("Binding using a converter group")]
    [Range(0, 1)] public float dangerLevel;
}