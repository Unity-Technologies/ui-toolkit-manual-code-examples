using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsMultiColumnListView : PlanetsWindow
{
    [MenuItem("Planets/Multicolumn List")]
    private static void Summon()
    {
        PlanetsMultiColumnListView window = GetWindow<PlanetsMultiColumnListView>("Multicolumn Planet List");
        window.minSize = new Vector2(500, 500);
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(uxml.Instantiate());
        MultiColumnListView listView = rootVisualElement.Q<MultiColumnListView>();

        //Set MultiColumnListView.itemsSource to populate the data in the list.
        listView.itemsSource = planets;

        //For each column, set Column.makeCell to initialize each cell in the column.
        listView.columns["name"].makeCell = () => new Label();
        listView.columns["populated"].makeCell = () => new Toggle();

        //For each column, set Column.bindCell to bind an initialized cell to a data item.
        listView.columns["name"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = (listView.itemsSource[index] as Planet).name;
        listView.columns["populated"].bindCell = (VisualElement element, int index) =>
            (element as Toggle).value = (listView.itemsSource[index] as Planet).populated;
    }
}
