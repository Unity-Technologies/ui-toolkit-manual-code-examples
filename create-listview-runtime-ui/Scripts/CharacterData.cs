    using UnityEngine;

    public enum ECharacterClass
    {
        Knight, Ranger, Wizard
    }

    [CreateAssetMenu]
    public class CharacterData : ScriptableObject
    {
        public string m_CharacterName;
        public ECharacterClass m_Class;
        public Sprite m_PortraitImage;
    }