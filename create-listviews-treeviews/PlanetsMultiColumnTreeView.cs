using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsMultiColumnTreeView : PlanetsWindow
{
    [MenuItem("Planets/Multicolumn Tree")]
    private static void Summon()
    {
        PlanetsMultiColumnTreeView window = GetWindow<PlanetsMultiColumnTreeView>("Multicolumn Planet Tree");
        window.minSize = new Vector2(500, 500);
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(uxml.Instantiate());
        MultiColumnTreeView treeView = rootVisualElement.Q<MultiColumnTreeView>();

        //Call MultiColumnTreeView.SetRootItems() to populate the data in the tree.
        treeView.SetRootItems<PlanetOrGroup>(treeRoots);

        //For each column, set Column.makeCell to initialize each node in the tree.
        treeView.columns["name"].makeCell = () => new Label();
        treeView.columns["populated"].makeCell = () => new Toggle();

        //For each column, set Column.bindCell to bind an initialized node to a data item.
        treeView.columns["name"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = treeView.GetItemDataForIndex<PlanetOrGroup>(index).name;
        treeView.columns["populated"].bindCell = (VisualElement element, int index) =>
            (element as Toggle).value = treeView.GetItemDataForIndex<PlanetOrGroup>(index).populated;
    }
}
