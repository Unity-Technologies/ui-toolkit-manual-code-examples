using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingNameTag : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_NameTagTemplate;

    [SerializeField]
    UIDocument m_BaseContainerDocument;

    [SerializeField]
    Transform m_UITransform;

    [SerializeField]
    float m_ScaleMultiplier;

    [SerializeField]
    float m_DistanceCullingRange;

    VisualElement m_Root;
    VisualElement m_BaseContainer;
    VisualElement m_NpcNameTag;
    
    Camera m_MainCamera;
    
    void Awake()
    {
        m_MainCamera = Camera.main;
        
        m_BaseContainer = m_BaseContainerDocument.rootVisualElement.Q<VisualElement>("BaseContainer");
        
        m_NpcNameTag = m_NameTagTemplate.Instantiate();
        m_BaseContainer.Add(m_NpcNameTag);
        m_NpcNameTag.style.position = new StyleEnum<Position>(Position.Absolute);
    }

    void Update()
    {
        SetNameTagPositionAndScale();
    }

    void SetNameTagPositionAndScale()
    {
        var cameraSpaceLocation = GetCameraSpaceLocation(m_UITransform);
        
        // Use style.translate to set the position of the name tag.
        m_NpcNameTag.style.translate = new Translate(cameraSpaceLocation.x, cameraSpaceLocation.y);

        // Get distance of NPC from camera.
        var distance = Vector3.Distance(m_UITransform.position, m_MainCamera.transform.position);
        
        // Calculate 1/distance so the name tag get smaller as the distance gets bigger.
        var scale = 1 / distance * m_ScaleMultiplier;

        m_NpcNameTag.style.scale = new Scale(new Vector2(scale, scale));
        
        / /Display name tag based on whether it's in front of the camera and within culling range.
        if (cameraSpaceLocation.z < 0 || distance > m_DistanceCullingRange)
        {
            m_NpcNameTag.style.display = DisplayStyle.None;
        }
        else
        {
            m_NpcNameTag.style.display = DisplayStyle.Flex;
        }
    }

    Vector3 GetCameraSpaceLocation(Transform objectTransform)
    {
        // Get the size of the parent visual element of the name tag.
        var containerSize = m_BaseContainer.layout.size;
        var screenPoint = m_MainCamera.WorldToViewportPoint(objectTransform.position);
        var output = new Vector3(screenPoint.x * containerSize.x, (1 - screenPoint.y) * containerSize.y, screenPoint.z);
        
        return output;
    }
}