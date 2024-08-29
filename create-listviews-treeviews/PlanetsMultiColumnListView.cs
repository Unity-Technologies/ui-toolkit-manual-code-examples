using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsMultiColumnListView : PlanetsWindow
{
    [MenuItem("Planets/Multicolumn List")]
    static void Summon()
    {
        GetWindow<PlanetsMultiColumnListView>("Multicolumn Planet List");
    }

    void CreateGUI()
    {
        uxmlAsset.CloneTree(rootVisualElement);
        var listView = rootVisualElement.Q<MultiColumnListView>();

        // Set MultiColumnListView.itemsSource to populate the data in the list.
        listView.itemsSource = planets;

        // For each column, set Column.makeCell to initialize each cell in the column.
        // You can index the columns array with names or numerical indices.
        listView.columns["name"].makeCell = () => new Label();
        listView.columns["populated"].makeCell = () => new Toggle();

        // For each column, set Column.bindCell to bind an initialized cell to a data item.
        listView.columns["name"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = planets[index].name;
        listView.columns["populated"].bindCell = (VisualElement element, int index) =>
            (element as Toggle).value = planets[index].populated;
    }
}