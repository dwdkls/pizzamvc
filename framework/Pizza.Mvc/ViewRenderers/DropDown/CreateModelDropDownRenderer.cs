namespace Pizza.Mvc.ViewRenderers.DropDown
{
    internal sealed class CreateModelDropDownRenderer : DropDownRendererBase
    {
        protected override string SelectProperItem(string dropDownHtml, object value)
        {
            // First - clear selected item
            dropDownHtml = dropDownHtml.Replace("selected=\"selected\"", string.Empty);
            // Second - select empty item (always first)
            dropDownHtml = dropDownHtml.Replace("<option value=\"\">", "<option value=\"\" selected=\"selected\">");
            return dropDownHtml;
        }
    }
}