using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AspectRatioDemo : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Test/AspectRatioDemo")]
    public static void ShowExample()
    {
        AspectRatioDemo wnd = GetWindow<AspectRatioDemo>();
        wnd.titleContent = new GUIContent("AspectRatioDemo");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object.
        VisualElement root = rootVisualElement;

        var aspectRatio = new AspectRatioElement();
        aspectRatio.style.flexGrow = 1;

        var widthField = new IntegerField() { value = aspectRatio.RatioWidth, label = "W"};
        var heightField = new IntegerField() { value = aspectRatio.RatioHeight, label = "H" };

        root.Add(widthField);
        root.Add(heightField);
        root.Add(aspectRatio);

        var contents = new VisualElement();
        aspectRatio.Add(contents);

        aspectRatio.style.backgroundColor = Color.black;

        contents.style.backgroundColor = Color.green;

        widthField.RegisterValueChangedCallback((evt) =>aspectRatio.RatioWidth = evt.newValue);
        heightField.RegisterValueChangedCallback((evt) => aspectRatio.RatioHeight = evt.newValue);
        
        contents.style.width = new Length(100, LengthUnit.Percent);
        contents.style.height = new Length(100, LengthUnit.Percent);
        
        contents.RegisterCallback<GeometryChangedEvent>((evt) =>
        {
            Debug.Log($"Content ratio: {evt.newRect.width} x {evt.newRect.height} : {evt.newRect.width/evt.newRect.height}");
        });

    }
}
