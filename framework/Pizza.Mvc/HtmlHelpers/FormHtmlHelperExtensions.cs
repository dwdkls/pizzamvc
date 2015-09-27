using System.Web.Mvc;
using Pizza.Contracts.Presentation;
using Pizza.Mvc.ViewRenderers;
using Pizza.Mvc.ViewRenderers.DropDown;

namespace Pizza.Mvc.HtmlHelpers
{
    // TODO: rename methods in this class
    public static class FormHtmlHelperExtensions
    {
        public static MvcHtmlString ModelEditor(this HtmlHelper html, IEditModelBase model)
        {
            return new ModelEditorRenderer(html, model, new EditModelDropDownRenderer()).Render();
        }

        public static MvcHtmlString ModelEditor(this HtmlHelper html, ICreateModelBase model)
        {
            return new ModelEditorRenderer(html, model, new CreateModelDropDownRenderer()).Render();
        }

        public static MvcHtmlString ModelDisplay(this HtmlHelper html, IDetailsModelBase model)
        {
            return new ModelDisplayRenderer(html, model).Render();
        }
    }
}