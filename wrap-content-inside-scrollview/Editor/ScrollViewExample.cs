using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class ScrollViewExample : EditorWindow
{
    [MenuItem("Example/ScrollView Wrapping Example")]
    public static void ShowExample()
    {
        var wnd = GetWindow<ScrollViewExample>();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object.
        VisualElement root = rootVisualElement;

        // Import UXML.
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ScrollViewExample.uxml");
        VisualElement ScrollViewExample = visualTree.Instantiate();
        root.Add(ScrollViewExample);

        // Find the scroll view by name.
        VisualElement scrollview = root.Query<ScrollView>("scroll-view-wrap-example");
            
        // Add 15 buttons inside the scroll view.
        for (int i = 0; i < 15; i++) 
        {
            Button button = new Button();
            button.text = "Button";
            scrollview.Add(button);
        }
    }
}