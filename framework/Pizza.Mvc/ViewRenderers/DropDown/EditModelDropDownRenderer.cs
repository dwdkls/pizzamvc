namespace Pizza.Mvc.ViewRenderers.DropDown
{
    internal sealed class EditModelDropDownRenderer : DropDownRendererBase
    {
        protected override string SelectProperItem(string dropDownHtml, object value)
        {
            var selectedValue = value == null ? (object)string.Empty : (int)value;

            var oldValue = string.Format("<option value=\"{0}\">", selectedValue);
            var newValue = string.Format("<option value=\"{0}\" selected=\"selected\">", selectedValue);
            dropDownHtml = dropDownHtml.Replace(oldValue, newValue);

            return dropDownHtml;
        }
    }
}