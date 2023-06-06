using System;
using UnityEngine;

namespace CollectionTests
{
    // Make the struct serializable, so its values can be stored in Unity's data format
    [Serializable]
    public struct PlayerData
    {
        // Declare private fields for the player's name, number, and icon, with the SerializeField attribute
        [SerializeField]
        string name;
        [SerializeField]
        int number;
        [SerializeField]
        Texture2D icon;

        // Calculate a unique identifier for the player based on their name and number
        public int id => name.GetHashCode() + 27 * number;

        // Define read-only properties for accessing the private fields
        public string Name => name;
        public int Number => number;
        public Texture2D Icon => icon;

        // Override the ToString() method to return a formatted string representation of the player data
        public override string ToString()
        {
            return $"{Name} #{Number.ToString()}";
        }
    }
}

