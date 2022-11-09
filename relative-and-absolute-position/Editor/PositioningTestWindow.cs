using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PositioningTestWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Positioning Test Window")]
    public static void ShowExample()
    {
        var wnd = GetWindow<PositioningTestWindow>();
        wnd.titleContent = new GUIContent("Positioning Test Window");
    }

    public void CreateGUI()
    {
        for (int i = 0; i < 4; i++)
        {
            var temp = new VisualElement();
            temp.style.width = 70;
            temp.style.height = 70;
            temp.style.marginBottom = 2;
            temp.style.backgroundColor = Color.gray;
            rootVisualElement.Add(temp);
        }

        // Relative positioning
        var relative = new Label("Relative\nPos\n25, 0");
        relative.style.width = 70;
        relative.style.height = 70;
        relative.style.left = 25;
        relative.style.marginBottom = 2;
        relative.style.backgroundColor = new Color(0.2165094f, 0, 0.254717f);
        rootVisualElement.Add(relative);

        // Absolute positioning
        var absolutePositionElement = new Label("Absolute\nPos\n25, 25");
        absolutePositionElement.style.position = Position.Absolute;
        absolutePositionElement.style.top = 25;
        absolutePositionElement.style.left = 25;
        absolutePositionElement.style.width = 70;
        absolutePositionElement.style.height = 70;
        absolutePositionElement.style.backgroundColor = Color.black;
        rootVisualElement.Add(absolutePositionElement);
    }
}