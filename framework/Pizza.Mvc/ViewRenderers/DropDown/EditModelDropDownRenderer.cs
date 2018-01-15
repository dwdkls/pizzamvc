namespace Pizza.Mvc.ViewRenderers.DropDown
{
    internal sealed class EditModelDropDownRenderer : DropDownRendererBase
    {
        protected override string SelectProperItem(string dropDownHtml, object value)
        {
            var selectedValue = value == null ? (object)string.Empty : (int)value;

            var oldValue = $"<option value=\"{selectedValue}\">";
            var newValue = $"<option value=\"{selectedValue}\" selected=\"selected\">";
            dropDownHtml = dropDownHtml.Replace(oldValue, newValue);

            return dropDownHtml;
        }
    }
}