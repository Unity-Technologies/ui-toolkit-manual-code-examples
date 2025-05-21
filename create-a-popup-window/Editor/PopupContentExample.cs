using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupContentExample : PopupWindowContent
{ 
    public override void OnOpen()
    {
        Debug.Log("Popup opened: " + this);
    }

    public override VisualElement CreateGUI()
    {
        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PopupWindowContent.uxml");
        return visualTreeAsset.CloneTree();
    }

    public override void OnClose()
    {
        Debug.Log("Popup closed: " + this);
    }
}