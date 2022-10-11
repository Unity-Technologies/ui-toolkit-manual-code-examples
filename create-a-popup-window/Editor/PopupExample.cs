using UnityEditor;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

public class PopupExample : EditorWindow
{
    // Add menu item
    [MenuItem("Example/Popup Example")]
    static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<PopupExample>();
        window.Show();
    }

    private void CreateGUI()
    {
        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PopupExample.uxml");
        visualTreeAsset.CloneTree(rootVisualElement);

        var button = rootVisualElement.Q<Button>();
        button.clicked += () => PopupWindow.Show(button.worldBound, new PopupContentExample());
    }
}