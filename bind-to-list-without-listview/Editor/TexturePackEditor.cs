using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(TexturePackAsset))]
    public class TexturePackEditor : Editor
    {
        [SerializeField]
        VisualTreeAsset m_VisualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var editor = m_VisualTreeAsset.CloneTree();

            var container = editor.Q(className: "preview-container");

            SetupList(container);

            // Watch the array size to handle the list being changed        
            var propertyForSize = serializedObject.FindProperty(nameof(TexturePackAsset.textures) + ".Array");
            propertyForSize.Next(true); // Expand to obtain array size
            editor.TrackPropertyValue(propertyForSize, prop => SetupList(container));

            editor.Q<Button>("add-button").RegisterCallback<ClickEvent>(OnClick);

            return editor;
        }

        void SetupList(VisualElement container)
        {
            var property = serializedObject.FindProperty(nameof(TexturePackAsset.textures) + ".Array");

            var endProperty = property.GetEndProperty();

            property.NextVisible(true); // Expand the first child.

            var childIndex = 0;

            // Iterate each property under the array and populate the container with preview elements
            do
            {
                // Stop if we've reached the end of the array
                if (SerializedProperty.EqualContents(property, endProperty))
                    break;

                // Skip the array size property
                if (property.propertyType == SerializedPropertyType.ArraySize)
                    continue;

                TexturePreviewElement element;

                // Find an existing element or create one
                if (childIndex < container.childCount)
                {
                    element = (TexturePreviewElement)container[childIndex];
                }
                else
                {
                    element = new TexturePreviewElement();
                    container.Add(element);
                }

                element.BindProperty(property);

                ++childIndex;
            }
            while (property.NextVisible(false));   // Never expand children.

            // Remove excess elements if the array is now smaller
            while (childIndex < container.childCount)
            {
                container.RemoveAt(container.childCount - 1);
            }
        }

        void OnClick(ClickEvent evt)
        {
            var property = serializedObject.FindProperty(nameof(TexturePackAsset.textures));
            property.arraySize += 1;
            serializedObject.ApplyModifiedProperties();
        }
    }
}