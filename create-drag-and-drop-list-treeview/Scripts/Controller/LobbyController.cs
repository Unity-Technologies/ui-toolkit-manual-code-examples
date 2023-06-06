using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CollectionTests
{
    public class LobbyController
    {
        const string k_DraggedItemsKey = "DraggedIndices";
        const string k_SourceKey = "SourceCollection";

        ListView m_LobbyListView;
        MultiColumnListView m_BlueTeamListView;
        TreeView m_RedTeamTreeView;
        Toggle m_IsOwnerToggle;

        List<PlayerData> m_LobbyItemsSource;
        List<PlayerData> m_BlueTeamItemsSource = new();
        List<TreeViewItemData<PlayerData>> m_RedTeamItemsSource = new();

        public LobbyController(VisualElement rootVisualElement, VisualTreeAsset playerItemAsset, CollectionDatabase collectionDatabase)
        {
            // Grab references
            m_IsOwnerToggle = rootVisualElement.Q<Toggle>("Toggle-LobbyOwner");
            m_LobbyListView = rootVisualElement.Q<ListView>("ListView-Lobby");
            m_BlueTeamListView = rootVisualElement.Q<MultiColumnListView>("ListView-BlueTeam");
            m_RedTeamTreeView = rootVisualElement.Q<TreeView>("TreeView-RedTeam");

            // Create a copy of the lobby list.
            m_LobbyItemsSource = collectionDatabase.initialLobbyList.ToList();

            m_LobbyListView.makeItem = MakeItem;
            m_LobbyListView.bindItem = (e, i) => BindItem(e, i, m_LobbyItemsSource[i]);
            m_LobbyListView.destroyItem = DestroyItem;
            m_LobbyListView.fixedItemHeight = 38;
            m_LobbyListView.itemsSource = m_LobbyItemsSource;
            m_LobbyListView.canStartDrag += OnCanStartDrag;
            m_LobbyListView.setupDragAndDrop += args => OnSetupDragAndDrop(args, m_LobbyListView);
            m_LobbyListView.dragAndDropUpdate += args => OnDragAndDropUpdate(args, m_LobbyListView, true);
            m_LobbyListView.handleDrop += args => OnHandleDrop(args, m_LobbyListView, true);

            var scrollView = m_LobbyListView.Q<ScrollView>();
            scrollView.touchScrollBehavior = ScrollView.TouchScrollBehavior.Elastic;
            scrollView.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;

            m_BlueTeamListView.columns["icon"].makeCell = () => new PlayerDataElement { style = { width = 24, height = 24, alignSelf = Align.Center } };
            m_BlueTeamListView.columns["icon"].bindCell = (element, i) =>
            {
                BindItem(element, i, m_BlueTeamItemsSource[i]);
                element.style.backgroundImage = m_BlueTeamItemsSource[i].Icon;
            };
            m_BlueTeamListView.columns["number"].makeCell = () => new Label { style = { alignSelf = Align.Center } };
            m_BlueTeamListView.columns["number"].bindCell = (element, i) => ((Label)element).text = $"#{m_BlueTeamItemsSource[i].Number}";
            m_BlueTeamListView.columns["name"].makeCell = () => new Label { style = { paddingLeft = 10 } };
            m_BlueTeamListView.columns["name"].bindCell = (element, i) => ((Label)element).text = m_BlueTeamItemsSource[i].Name;
            m_BlueTeamListView.fixedItemHeight = 38;
            m_BlueTeamListView.reorderable = false;
            m_BlueTeamListView.itemsSource = m_BlueTeamItemsSource;
            m_BlueTeamListView.canStartDrag += OnCanStartDrag;
            m_BlueTeamListView.setupDragAndDrop += args => OnSetupDragAndDrop(args, m_BlueTeamListView);
            m_BlueTeamListView.dragAndDropUpdate += args => OnDragAndDropUpdate(args, m_BlueTeamListView);
            m_BlueTeamListView.handleDrop += args => OnHandleDrop(args, m_BlueTeamListView);

            m_RedTeamTreeView.makeItem = MakeItem;
            m_RedTeamTreeView.bindItem = (e, i) => BindItem(e, m_RedTeamTreeView.GetIdForIndex(i), (PlayerData)m_RedTeamTreeView.viewController.GetItemForIndex(i));
            m_RedTeamTreeView.destroyItem = DestroyItem;
            m_RedTeamTreeView.fixedItemHeight = 38;
            m_RedTeamTreeView.SetRootItems(m_RedTeamItemsSource);
            m_RedTeamTreeView.canStartDrag += OnCanStartDrag;
            m_RedTeamTreeView.setupDragAndDrop += args => OnSetupDragAndDrop(args, m_RedTeamTreeView);
            m_RedTeamTreeView.dragAndDropUpdate += args => OnDragAndDropUpdate(args, m_RedTeamTreeView);
            m_RedTeamTreeView.handleDrop += args => OnHandleDrop(args, m_RedTeamTreeView);

            VisualElement MakeItem()
            {
                return playerItemAsset.Instantiate();
            }

            static void BindItem(VisualElement element, int index, PlayerData data)
            {
                var playerView = element.Q<PlayerDataElement>();
                playerView.Bind(data);
                playerView.id = index;
            }

            static void DestroyItem(VisualElement element)
            {
                var playerView = element.Q<PlayerDataElement>();
                playerView.Reset();
            }

            bool OnCanStartDrag(CanStartDragArgs _) => m_IsOwnerToggle.value;

            StartDragArgs OnSetupDragAndDrop(SetupDragAndDropArgs args, BaseVerticalCollectionView source)
            {
                var playerView = args.draggedElement.Q<PlayerDataElement>();
                if (playerView == null)
                    return args.startDragArgs;

                var startDragArgs = new StartDragArgs(args.startDragArgs.title, DragVisualMode.Move);
                startDragArgs.SetGenericData(k_SourceKey, source);
                startDragArgs.SetGenericData(k_DraggedItemsKey, args.selectedIds.Any() ? args.selectedIds : new List<int> { playerView.id });
                return startDragArgs;
            }

            DragVisualMode OnDragAndDropUpdate(HandleDragAndDropArgs args, BaseVerticalCollectionView destination, bool isLobby = false)
            {
                var source = args.dragAndDropData.GetGenericData(k_SourceKey);
                if (source == destination)
                    return DragVisualMode.None;

                return !isLobby && destination.itemsSource.Count >= 3 ? DragVisualMode.Rejected : DragVisualMode.Move;
            }

            DragVisualMode OnHandleDrop(HandleDragAndDropArgs args, BaseVerticalCollectionView destination, bool isLobby = false)
            {
                if (args.dragAndDropData.unityObjectReferences != null && args.dragAndDropData.unityObjectReferences.Any())
                {
                    Debug.Log($"That was {string.Join(", ", args.dragAndDropData.unityObjectReferences.Select(o => $"\"{o.name}\""))}");
                    return DragVisualMode.Move;
                }

                if (args.dragAndDropData.GetGenericData(k_DraggedItemsKey) is not List<int> draggedIds)
                    throw new ArgumentNullException($"Indices are null.");
                if (args.dragAndDropData.GetGenericData(k_SourceKey) is not BaseVerticalCollectionView source)
                    throw new ArgumentNullException($"Source is null.");

                // Let default reordering happen.
                if (source == destination)
                    return DragVisualMode.None;

                // Be coherent with the dragAndDropUpdate condition.
                if (!isLobby && destination.itemsSource.Count >= 3)
                    return DragVisualMode.Rejected;

                var treeViewSource = source as BaseTreeView;

                // ********************************************************
                // Add items first, from item indices in the source.
                // ********************************************************

                // Gather ids from dragged indices
                var ids = draggedIds.ToList();

                // Special TreeView case, we need to gather children or selected indices.
                if (treeViewSource != null)
                {
                    GatherChildrenIds(ids, treeViewSource);
                }

                if (destination is BaseTreeView treeView)
                {
                    foreach (var id in ids)
                    {
                        var data = (PlayerData)source.viewController.GetItemForId(id);
                        treeView.AddItem(new TreeViewItemData<PlayerData>(data.id, data), args.parentId, args.childIndex, false);
                    }

                    treeView.viewController.RebuildTree();
                }
                else if (destination.viewController is BaseListViewController destinationListViewController)
                {
                    for (var i = ids.Count - 1; i >= 0; i--)
                    {
                        var id = ids[i];
                        var data = (PlayerData)source.viewController.GetItemForId(id);
                        destinationListViewController.itemsSource.Insert(args.insertAtIndex, data);
                    }
                }
                else
                {
                    throw new ArgumentException("Unhandled destination.");
                }

                // Then remove from the source.
                if (source is BaseTreeView sourceTreeView)
                {
                    foreach (var id in draggedIds)
                    {
                        var data = (PlayerData)source.viewController.GetItemForId(id);
                        sourceTreeView.viewController.TryRemoveItem(data.id, false);
                    }

                    sourceTreeView.viewController.RebuildTree();
                    sourceTreeView.RefreshItems();
                }
                else if (source.viewController is BaseListViewController sourceListViewController)
                {
                    sourceListViewController.RemoveItems(draggedIds);
                }
                else
                {
                    throw new ArgumentException("Unhandled source.");
                }

                destination.SetSelection(ids.Select(id => destination.viewController.GetIndexForId(id)));
                source.ClearSelection();
                destination.RefreshItems();
                LogTeamSizes();
                return DragVisualMode.Move;
            }
        }

        void LogTeamSizes()
        {
            Debug.Log($"Blue: {m_BlueTeamListView.itemsSource.Count} / 3\tRed: {m_RedTeamTreeView.viewController.GetItemsCount()} / 3");
        }

        static void GatherChildrenIds(List<int> ids, BaseTreeView treeView)
        {
            for (var i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                var childrenIds = treeView.viewController.GetChildrenIds(id);
                foreach (var childId in childrenIds)
                {
                    ids.Insert(i + 1, childId);
                    i++;
                }
            }
        }
    }
}