using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    public class ExampleElementCustomStyle : VisualElement
    {
        // Factory class, required to expose this custom control to UXML
        public new class UxmlFactory : UxmlFactory<ExampleElementCustomStyle, UxmlTraits> { }

        // Traits class
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        // Use CustomStyleProperty<T> to fetch custom style properties from USS
        static readonly CustomStyleProperty<Color> S_GradientFrom = new CustomStyleProperty<Color>("--gradient-from");
        static readonly CustomStyleProperty<Color> S_GradientTo = new CustomStyleProperty<Color>("--gradient-to");

        // Image child element and its texture
        Texture2D m_Texture2D;
        Image m_Image;

        public ExampleElementCustomStyle()
        {
            // Create an Image and a texture for it. Attach Image to self.
            m_Texture2D = new Texture2D(100, 100);
            m_Image = new Image();
            m_Image.image = m_Texture2D;
            Add(m_Image);

            RegisterCallback<CustomStyleResolvedEvent>(OnStylesResolved);
        }

        // When custom styles are known for this control, make a gradient from the colors.
        void OnStylesResolved(CustomStyleResolvedEvent evt)
        {
            Color from, to;

            if (evt.customStyle.TryGetValue(S_GradientFrom, out from)
                && evt.customStyle.TryGetValue(S_GradientTo, out to))
            {
                GenerateGradient(from, to);
            }
        }

        public void GenerateGradient(Color from, Color to)
        {
            for (int i = 0; i < m_Texture2D.width; ++i)
            {
                Color color = Color.Lerp(from, to, i / (float)m_Texture2D.width);
                for (int j = 0; j < m_Texture2D.height; ++j)
                {
                    m_Texture2D.SetPixel(i, j, color);
                }
            }

            m_Texture2D.Apply();
            m_Image.MarkDirtyRepaint();
        }
    }
}