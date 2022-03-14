using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class PlanetsTreeView : PlanetsWindow
{
    [MenuItem("Planets/Standard Tree")]
    private static void Summon()
    {
        PlanetsTreeView window = GetWindow<PlanetsTreeView>("Standard Planet Tree");
        window.minSize = new Vector2(500, 500);
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(uxml.Instantiate());
        TreeView treeView = rootVisualElement.Q<TreeView>();

        //Call TreeView.SetRootItems() to populate the data in the tree.
        treeView.SetRootItems<PlanetOrGroup>(treeRoots);

        //Set TreeView.makeItem to initialize each node in the tree.
        treeView.makeItem = () => new Label();

        //Set TreeView.bindItem to bind an initialized node to a data item.
        treeView.bindItem = (VisualElement element, int index) =>
            (element as Label).text = treeView.GetItemDataForIndex<PlanetOrGroup>(index).name;
    }
}
