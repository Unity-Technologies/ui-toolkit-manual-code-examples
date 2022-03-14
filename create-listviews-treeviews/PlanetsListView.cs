using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsListView : PlanetsWindow
{
    [MenuItem("Planets/Standard List")]
    private static void Summon()
    {
        PlanetsListView window = GetWindow<PlanetsListView>("Standard Planet List");
        window.minSize = new Vector2(500, 500);
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(uxml.Instantiate());
        ListView listView = rootVisualElement.Q<ListView>();

        //Set ListView.itemsSource to populate the data in the list.
        listView.itemsSource = planets;

        //Set ListView.makeItem to initialize each entry in the list.
        listView.makeItem = () => new Label();

        //Set ListView.bindItem to bind an initialized entry to a data item.
        listView.bindItem = (VisualElement element, int index) =>
            (element as Label).text = (listView.itemsSource[index] as Planet).name;
    }
}
