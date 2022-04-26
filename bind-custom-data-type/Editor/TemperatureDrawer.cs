using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    [CustomPropertyDrawer(typeof(Temperature))]
    public class TemperatureDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var asset = Resources.Load<VisualTreeAsset>("temperature_drawer");
            var drawer = asset.Instantiate(property.propertyPath);

            drawer.Q<Label>().text = property.displayName;

            // Do not allow conversion when having multiple objects selected in the Inspector
            if (!property.serializedObject.isEditingMultipleObjects)
            {
                drawer.Q<Button>().RegisterCallback<ClickEvent, SerializedProperty>(Convert, property);
            }

            return drawer;
        }

        static void Convert(ClickEvent evt, SerializedProperty property)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var unitProperty = property.FindPropertyRelative("unit");

            // F -> C
            if (unitProperty.enumValueIndex == (int)TemperatureUnit.Farenheit)
            {
                valueProperty.doubleValue -= 32;
                valueProperty.doubleValue *= 5.0d / 9.0d;
                unitProperty.enumValueIndex = (int)TemperatureUnit.Celsius;
            }
            else // C -> F
            {
                valueProperty.doubleValue *= 9.0d / 5.0d;
                valueProperty.doubleValue += 32;
                unitProperty.enumValueIndex = (int)TemperatureUnit.Farenheit;
            }

            // Important: because we are bypassing the binding system here, we must save the modified SerializedObject
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}