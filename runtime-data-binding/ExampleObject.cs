using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ExampleObject : ScriptableObject
{
    [Header("Simple binding")]
    public string simpleLabel = "Hello World!";
}