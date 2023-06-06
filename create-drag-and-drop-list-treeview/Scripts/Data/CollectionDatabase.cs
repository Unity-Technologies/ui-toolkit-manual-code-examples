using System.Collections.Generic;
using UnityEngine;

namespace CollectionTests
{
    // Create a CollectionDatabase object that you can create as an asset via the Asset menu.
    [CreateAssetMenu]
    public class CollectionDatabase : ScriptableObject
    {
        // Declare a private list of PlayerData that can set in the Unity Editor.
        [SerializeField]
        List<PlayerData> m_InitialLobbyList;

        public IEnumerable<PlayerData> initialLobbyList => m_InitialLobbyList;
    }
}

