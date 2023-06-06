using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CollectionTests
{
    public class ListDragAndDropTestWindow : EditorWindow
    {
        [MenuItem("Collection Tests/List DragAndDrop Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<ListDragAndDropTestWindow>();
            wnd.titleContent = new GUIContent("List DragAndDrop Test");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;

            // Import UXML
            var visualTreeAsset = EditorGUIUtility.Load("Assets/create-drag-and-drop-list-treeview/UI/ListDragAndDropTestWindow.uxml") as VisualTreeAsset;
            visualTreeAsset.CloneTree(root);

            // Load the PlayerItemView.uxml file
            var playerItemAsset = EditorGUIUtility.Load("Assets/create-drag-and-drop-list-treeview/UI/PlayerItemView.uxml") as VisualTreeAsset;

            //Load the CollectionDatabase from the Resources folder
            var collectionDatabase = Resources.Load<CollectionDatabase>("CollectionDatabaseAsset");

            // Create the LobbyController
            var lobbyController = new LobbyController(root, playerItemAsset, collectionDatabase);
        }
    }
}