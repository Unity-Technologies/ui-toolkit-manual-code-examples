using System;
using UnityEngine.UIElements;

namespace CollectionTests
{
    [UxmlElement]
    public partial class PlayerDataElement : VisualElement
    {
        public PlayerData data { get; private set; }

        public int id { get; set; }

        public virtual void Bind(PlayerData player)
        {
            data = player;
        }

        public virtual void Reset()
        {
            data = default;
            id = -1;
        }
    }
}
