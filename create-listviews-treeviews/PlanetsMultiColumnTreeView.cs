using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsMultiColumnTreeView : PlanetsWindow
{
    [MenuItem("Planets/Multicolumn Tree")]
    static void Summon()
    {
        GetWindow<PlanetsMultiColumnTreeView>("Multicolumn Planet Tree");
    }

    void CreateGUI()
    {
        uxmlAsset.CloneTree(rootVisualElement);
        var treeView = rootVisualElement.Q<MultiColumnTreeView>();

        // Call MultiColumnTreeView.SetRootItems() to populate the data in the tree.
        treeView.SetRootItems(treeRoots);

        // For each column, set Column.makeCell to initialize each node in the tree.
        // You can index the columns array with names or numerical indices.
        treeView.columns["name"].makeCell = () => new Label();
        treeView.columns["populated"].makeCell = () => new Toggle();

        // For each column, set Column.bindCell to bind an initialized node to a data item.
        treeView.columns["name"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = treeView.GetItemDataForIndex<IPlanetOrGroup>(index).name;
        treeView.columns["populated"].bindCell = (VisualElement element, int index) =>
            (element as Toggle).value = treeView.GetItemDataForIndex<IPlanetOrGroup>(index).populated;
    }
}