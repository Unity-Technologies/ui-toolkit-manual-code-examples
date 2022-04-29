 using UnityEngine;
    using UnityEditor;
    using UnityEngine.UIElements;

    namespace Samples.Editor.General
    {
        // The DragAndDropManipulator class is a private class within DragAndDropWindow.
        public partial class DragAndDropWindow
        {
            // DragAndDropManipulator is a manipulator that stores pointer-related callbacks, so it inherits from
            // PointerManipulator.
            class DragAndDropManipulator : PointerManipulator
            {
                // The Label in the window that shows the stored asset, if any.
                Label dropLabel;
                // The stored asset object, if any.
                Object droppedObject = null;
                // The path of the stored asset, or the empty string if there isn't one.
                string assetPath = string.Empty;

                public DragAndDropManipulator(VisualElement root)
                {
                    // The target of the manipulator, the object to which to register all callbacks, is the drop area.
                    target = root.Q<VisualElement>(className: "drop-area");
                    dropLabel = root.Q<Label>(className: "drop-area__label");
                }

                protected override void RegisterCallbacksOnTarget()
                {
                    // Register a callback when the user presses the pointer down.
                    target.RegisterCallback<PointerDownEvent>(OnPointerDown);
                    // Register callbacks for various stages in the drag process.
                    target.RegisterCallback<DragEnterEvent>(OnDragEnter);
                    target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
                    target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
                    target.RegisterCallback<DragPerformEvent>(OnDragPerform);
                }

                protected override void UnregisterCallbacksFromTarget()
                {
                    // Unregister all callbacks that you registered in RegisterCallbacksOnTarget().
                    target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
                    target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
                    target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
                    target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
                    target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
                }

                // This method runs when a user presses a pointer down on the drop area.
                void OnPointerDown(PointerDownEvent _)
                {
                    // Only do something if the window currently has a reference to an asset object.
                    if (droppedObject != null)
                    {
                        // Clear existing data in DragAndDrop class.
                        DragAndDrop.PrepareStartDrag();

                        // Store reference to object and path to object in DragAndDrop static fields.
                        DragAndDrop.objectReferences = new[] { droppedObject };
                        if (assetPath != string.Empty)
                        {
                            DragAndDrop.paths = new[] { assetPath };
                        }
                        else
                        {
                            DragAndDrop.paths = new string[] { };
                        }

                        // Start a drag.
                        DragAndDrop.StartDrag(string.Empty);
                    }
                }

                // This method runs if a user brings the pointer over the target while a drag is in progress.
                void OnDragEnter(DragEnterEvent _)
                {
                    // Get the name of the object the user is dragging.
                    var draggedName = string.Empty;
                    if (DragAndDrop.paths.Length > 0)
                    {
                        assetPath = DragAndDrop.paths[0];
                        var splitPath = assetPath.Split('/');
                        draggedName = splitPath[splitPath.Length - 1];
                    }
                    else if (DragAndDrop.objectReferences.Length > 0)
                    {
                        draggedName = DragAndDrop.objectReferences[0].name;
                    }

                    // Change the appearance of the drop area if the user drags something over the drop area and holds it
                    // there.
                    dropLabel.text = $"Dropping '{draggedName}'...";
                    target.AddToClassList("drop-area--dropping");
                }

                // This method runs if a user makes the pointer leave the bounds of the target while a drag is in progress.
                void OnDragLeave(DragLeaveEvent _)
                {
                    assetPath = string.Empty;
                    droppedObject = null;
                    dropLabel.text = "Drag an asset here...";
                    target.RemoveFromClassList("drop-area--dropping");
                }

                // This method runs every frame while a drag is in progress.
                void OnDragUpdate(DragUpdatedEvent _)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }

                // This method runs when a user drops a dragged object onto the target.
                void OnDragPerform(DragPerformEvent _)
                {
                    // Set droppedObject and draggedName fields to refer to dragged object.
                    droppedObject = DragAndDrop.objectReferences[0];
                    string draggedName;
                    if (assetPath != string.Empty)
                    {
                        var splitPath = assetPath.Split('/');
                        draggedName = splitPath[splitPath.Length - 1];
                    }
                    else
                    {
                        draggedName = droppedObject.name;
                    }

                    // Visually update target to indicate that it now stores an asset.
                    dropLabel.text = $"Containing '{draggedName}'...\n\n" +
                        $"(You can also drag from here)";
                    target.RemoveFromClassList("drop-area--dropping");
                }
            }
        }
    }