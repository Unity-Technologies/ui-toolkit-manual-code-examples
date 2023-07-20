using UnityEngine.UIElements;

[UxmlObject]
public partial class AddMenuUssClass : CustomBinding
{
    protected override BindingResult Update(in BindingContext context)
    {
        // Assign USS classes based on the sibling index. The binding updates when the sibling index changes.
        var element = context.targetElement;
        var index = element.parent.IndexOf(element);
        element.EnableInClassList("menu-button--first", index == 0);
        element.EnableInClassList("menu-button--last", index == element.parent.childCount - 1);                
        
        return new BindingResult(BindingStatus.Success);
    }
}