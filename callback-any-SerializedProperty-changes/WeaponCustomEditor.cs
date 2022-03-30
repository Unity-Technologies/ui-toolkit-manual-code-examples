using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIToolkitExamples
{
    [CustomEditor(typeof(Weapon))]
    public class WeaponCustomEditor : Editor
    {
        // This is text used for the warning labels.
        const string k_NegativeWarningText =
            "This weapon has a negative final damage on at least 1 difficulty level.";
        static readonly string k_DamageCapWarningText =
            "This weapon has an excessive final damage that is capped to " + Weapon.maxDamage +
            " on at least 1 difficulty level.";

        // These are labels to warn users about negative damage and excessive damage.
        Label m_NegativeWarning, m_DamageCapWarning;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            // Create FloatFields for serialized properties.
            var baseDamageField = new FloatField("Base Damage") { bindingPath = "m_BaseDamage" };
            var modifierField = new FloatField("Hard Mode Modifier") { bindingPath = "m_HardModeModifier" };
            root.Add(baseDamageField);
            root.Add(modifierField);

            // Create warning labels and style them so they stand out.
            var warnings = new VisualElement();
            m_NegativeWarning = new(k_NegativeWarningText);
            m_DamageCapWarning = new(k_DamageCapWarningText);
            warnings.style.color = Color.red;
            warnings.style.unityFontStyleAndWeight = FontStyle.Bold;
            warnings.Add(m_NegativeWarning);
            warnings.Add(m_DamageCapWarning);
            root.Add(warnings);

            // Determine whether to show the warnings at the start.
            CheckForWarnings(serializedObject);

            // Whenever any serialized property on this serialized object changes its value, call CheckForWarnings.
            root.TrackSerializedObjectValue(serializedObject, CheckForWarnings);

            return root;
        }

        // Check the current values of the serialized properties to either display or hide the warnings.
        void CheckForWarnings(SerializedObject serializedObject)
        {
            // For each possible damage values of the weapon, determine whether it's negative and whether it's above the
            // maximum damage value.
            var weapon = serializedObject.targetObject as Weapon;
            var damages = new float[] { weapon.GetDamage(true), weapon.GetDamage(false) };
            var foundNegativeDamage = false;
            var foundCappedDamage = false;
            foreach (var damage in damages)
            {
                foundNegativeDamage = foundNegativeDamage || damage < 0;
                foundCappedDamage = foundCappedDamage || damage > Weapon.maxDamage;
            }

            // Display or hide warnings depending on the values of the damages.
            m_NegativeWarning.style.display = foundNegativeDamage ? DisplayStyle.Flex : DisplayStyle.None;
            m_DamageCapWarning.style.display = foundCappedDamage ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}