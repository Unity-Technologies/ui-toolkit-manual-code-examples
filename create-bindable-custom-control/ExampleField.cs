using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    // ExampleField inherits from BaseField with the double type. ExampleField's underlying value, then, is a double.
    [UxmlElement]
    public partial class ExampleField : BaseField<double>
    {
        Label m_Input;

        // Default constructor is required for compatibility with UXML factory
        public ExampleField() : this(null)
        {

        }

        // Main constructor accepts label parameter to mimic BaseField constructor.
        // Second argument to base constructor is the input element, the one that displays the value this field is
        // bound to.
        public ExampleField(string label) : base(label, new Label() { })
        {
            // This is the input element instantiated for the base constructor.
            m_Input = this.Q<Label>(className: inputUssClassName);
        }

        // SetValueWithoutNotify needs to be overridden by calling the base version and then making a change to the
        // underlying value be reflected in the input element.
        public override void SetValueWithoutNotify(double newValue)
        {
            base.SetValueWithoutNotify(newValue);

            m_Input.text = value.ToString("N");
        }
    }
}
