using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class LinkTag : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/LinkTag Sample")]
    public static void ShowExample()
    {
        LinkTag wnd = GetWindow<LinkTag>();
        wnd.titleContent = new GUIContent("LinkTag Sample");
    }

    readonly string linkCursorClassName = "link-cursor";
    Dictionary<int, string> m_UrlLookup;
    Label linkLabel;

    public void OnEnable()
    {
        // Use the dictionary to map link IDs to URLs.
        // The link ID is the number in the link tag, and the URL is the destination.
        // This is a simple example, in a real application you can define a more complex structure.
        m_UrlLookup = new Dictionary<int, string>()
        {
            { 1, "https://www.google.com/" },
            { 2, "https://discussions.unity.com/" }
        };
    }

    public void CreateGUI()
    {
        VisualElement uxml = m_VisualTreeAsset.Instantiate();
        rootVisualElement.Add(uxml);

        linkLabel = rootVisualElement.Q<Label>(className: "link");

        linkLabel.RegisterCallback<PointerDownLinkTagEvent>(HyperlinkOnPointerDown);
        linkLabel.RegisterCallback<PointerUpLinkTagEvent>(HyperlinkOnPointerUp);
        linkLabel.RegisterCallback<PointerMoveLinkTagEvent>(HyperlinkPointerMove);
        linkLabel.RegisterCallback<PointerOverLinkTagEvent>(HyperlinkOnPointerOver);
        linkLabel.RegisterCallback<PointerOutLinkTagEvent>(HyperlinkOnPointerOut);
    }

    void HyperlinkOnPointerOver(PointerOverLinkTagEvent _)
    {
        linkLabel.AddToClassList(linkCursorClassName);
    }
    void HyperlinkPointerMove(PointerMoveLinkTagEvent _) { }
    void HyperlinkOnPointerOut(PointerOutLinkTagEvent _)
    {
        linkLabel.RemoveFromClassList(linkCursorClassName);
    }

    void HyperlinkOnPointerDown(PointerDownLinkTagEvent _) { }

    void HyperlinkOnPointerUp(PointerUpLinkTagEvent evt)
    {
        var linkID = int.Parse(evt.linkID);
        if (m_UrlLookup.TryGetValue(linkID, out var url))
            Application.OpenURL(url);
    }

}
