using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ExampleMultiPropertiesObject : ScriptableObject
{
    [Header("Bind to multiple properties")]

    [CreateProperty]
    public Vector3 vector3Value;
    
    [CreateProperty]
    public float sumOfVector3Properties => vector3Value.x + vector3Value.y + vector3Value.z;
}