using UnityEngine;

namespace UIToolkitExamples
{
    public class Weapon : MonoBehaviour
    {
        public const float maxDamage = 9999f;

        [SerializeField]
        float m_BaseDamage;

        [SerializeField]
        float m_HardModeModifier;

        public float GetDamage(bool hardMode)
        {
            return hardMode ? m_BaseDamage * m_HardModeModifier : m_BaseDamage;
        }
    }
}