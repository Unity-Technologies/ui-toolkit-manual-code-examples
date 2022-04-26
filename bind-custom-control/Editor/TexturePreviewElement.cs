using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UIToolkitExamples
{
    public class TexturePreviewElement : BindableElement, INotifyValueChanged<Object>
    {
        public new class UxmlTraits : BindableElement.UxmlTraits { }
        public new class UxmlFactory : UxmlFactory<TexturePreviewElement, UxmlTraits> { }

        public static readonly string ussClassName = "texture-preview-element";

        Image m_Preview;
        ObjectField m_ObjectField;
        Texture2D m_Value;

        public TexturePreviewElement()
        {
            AddToClassList(ussClassName);

            // Create a preview image.
            m_Preview = new Image();
            Add(m_Preview);

            // Create an ObjectField, set its object type, and register a callback when its value changes.
            m_ObjectField = new ObjectField();
            m_ObjectField.objectType = typeof(Texture2D);
            m_ObjectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(m_ObjectField);

            styleSheets.Add(Resources.Load<StyleSheet>("texture_preview_element"));
        }
        
        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        public void SetValueWithoutNotify(Object newValue)
        {
            if (newValue == null || newValue is Texture2D)
            {
                // Update the preview Image and update the ObjectField.
                m_Value = newValue as Texture2D;
                m_Preview.image = m_Value;
                // Notice that this line calls the ObjectField's SetValueWithoutNotify() method instead of just setting
                // m_ObjectField.value. This is very important; you don't want m_ObjectField to send a ChangeEvent.
                m_ObjectField.SetValueWithoutNotify(m_Value);
            }
            else throw new ArgumentException($"Expected object of type {typeof(Texture2D)}");
        }

        public Object value
        {
            get => m_Value;
            // The setter is called when the user changes the value of the ObjectField, which calls
            // OnObjectFieldValueChanged(), which calls this.
            set
            {
                if (value == this.value)
                    return;

                var previous = this.value;
                SetValueWithoutNotify(value);

                using (var evt = ChangeEvent<Object>.GetPooled(previous, value))
                {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }
    }
}