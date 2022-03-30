using UnityEngine;

namespace UIToolkitExamples
{
    [CreateAssetMenu(menuName = "UIToolkitExamples/TextureAsset")]
    public class TextureAsset : ScriptableObject
    {
        public Texture2D texture;

        public void Reset()
        {
            texture = null;
        }
    }
}