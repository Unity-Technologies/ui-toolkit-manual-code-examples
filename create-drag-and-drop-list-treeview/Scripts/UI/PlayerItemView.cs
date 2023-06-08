using System;
using UnityEngine.UIElements;

namespace CollectionTests
{
    [UxmlElement]
    public partial class PlayerItemView : PlayerDataElement
    {
        VisualElement m_Icon;
        Label m_Name;

        // Bind the player data to the UI.
        public override void Bind(PlayerData player)
        {
            base.Bind(player);
                
            m_Icon ??= this.Q("Icon");
            m_Name ??= this.Q<Label>();

            m_Icon.style.backgroundImage = player.Icon;
            m_Name.text = player.Name;
        }
    }
}
