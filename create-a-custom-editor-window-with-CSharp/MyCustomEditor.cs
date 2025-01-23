using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomEditor : EditorWindow
{
    [SerializeField] private int m_SelectedIndex = -1;
    private VisualElement m_RightPane;

    [MenuItem("Window/UI Toolkit/MyCustomEditor")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor.
        EditorWindow wnd = GetWindow<MyCustomEditor>();
        wnd.titleContent = new GUIContent("My Custom Editor");

        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    public void CreateGUI()
    {
        // Get a list of all sprites in the project.
        var query = AssetDatabase.SourceSearchQuery.ByImporter(typeof(TextureImporter));
        var results = AssetDatabase.SearchAsync(query, System.Threading.CancellationToken.None).Result.Items;
        var allObjects = new List<Sprite>();
        foreach (var result in results)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(result.guid));
            if (sprite != null)
                allObjects.Add(sprite);
        }

        // Create a two-pane view with the left pane being fixed.
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the panel to the visual tree by adding it as a child to the root element.
        rootVisualElement.Add(splitView);

        // A TwoPaneSplitView always needs two child elements.
        var leftPane = new ListView();
        splitView.Add(leftPane);
        m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        splitView.Add(m_RightPane);

        // Initialize the list view with all sprites' names.
        leftPane.makeItem = () => new Label();
        leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        leftPane.itemsSource = allObjects;

        // React to the user's selection.
        leftPane.selectionChanged += OnSpriteSelectionChange;

        // Restore the selection index from before the hot reload.
        leftPane.selectedIndex = m_SelectedIndex;

        // Store the selection index when the selection changes.
        leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
    }

    private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane.
        m_RightPane.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var selectedSprite = enumerator.Current as Sprite;
            if (selectedSprite != null)
            {
                // Add a new Image control and display the sprite.
                var spriteImage = new Image();
                spriteImage.scaleMode = ScaleMode.ScaleToFit;
                spriteImage.sprite = selectedSprite;

                // Add the Image control to the right-hand pane.
                m_RightPane.Add(spriteImage);
            }
        }
    }
}